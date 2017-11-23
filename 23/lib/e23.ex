defmodule E23 do

  # To avoid dynamic creation of atoms - define these now and use String.to_existing_atom later.
  [:nop, :tgl, :jnz, :cpy, :inc, :dec, :a, :b, :c, :d]

  @functions %{
    :cpy => &E23.cpy/2,
    :inc => &E23.inc/2,
    :dec => &E23.dec/2,
    :jnz => &E23.jnz/2,
    :tgl => &E23.tgl/2
  }

  @zero_registers %{:a => 0, :b => 0, :c => 0, :d => 0}

  def set_register(computer, r, value) do
    %{
      :registers => Map.put(computer.registers, r, value),
      :instructions => computer.instructions,
      :ip => computer.ip + 1
    }
  end

  def register(computer, [r]) do
    Map.get(computer.registers, r)
  end

  def nop(computer, _) do
    %{
      :registers => computer.registers,
      :instructions => computer.instructions,
      :ip => computer.ip + 1
    }
  end

  def tgl([instruction, function | args]) do
    (
      case {instruction, Enum.count(args)} do
        {:inc, _} -> [:dec, &E23.dec/2]
        {:dec, _} -> [:inc, &E23.inc/2]
        {_, 1} -> [:inc, &E23.inc/2]
        {:jnz, _} -> [:cpy, &E23.cpy/2]
        {_, 2} -> [:jnz, &E23.jnz/2]
      end
    ) ++ args
  end

  def tgl(computer, [r]) do
    %{
      :registers => computer.registers,
      :instructions => tgl(computer.instructions, computer.ip + register(computer, [r])),
      :ip => computer.ip + 1
    }
  end

  def tgl(instructions, n) do
    cond do
      n >= Enum.count(instructions) -> instructions
      true -> List.replace_at(instructions, n, tgl(Enum.at(instructions,n)))
    end
  end

  def cpy(computer, [a, b]) when is_integer(a) do
    set_register(computer, b, a)
  end

  def cpy(computer, [a, b]) do
    set_register(computer, b, register(computer, [a]))
  end

  def inc(computer, [r]) do
    set_register(computer, r, register(computer, [r]) + 1)
  end

  def dec(computer, [r]) do
    set_register(computer, r, register(computer, [r]) - 1)
  end

  def jnz(computer, [r, offset]) when is_atom(r) do
    jnz(computer, [register(computer, [r]), offset])
  end

  def jnz(computer, [n, offset]) when is_integer(n) and is_atom(offset) do
    jnz(computer, [n, register(computer, [offset])])
  end

  def jnz(computer, [n, offset]) when is_integer(n) do
    %{
      :registers => computer.registers,
      :instructions => computer.instructions,
      :ip => computer.ip + case n do
        0 -> 1
        _ -> offset
      end
    }
  end

  def execute_instruction(computer, instruction_definition, ip) do

    cond do
      ip >= Enum.count(computer.instructions) -> computer.registers.a
      true -> 
        [instruction, function | args] = instruction_definition

        #IO.inspect({instruction, args})
        computer = function.(computer, args)
        #IO.inspect(computer)
        #IO.puts ""
        execute_instruction(computer, Enum.at(computer.instructions, computer.ip), computer.ip)
    end

  end

  def parse(input) do
    String.split(input, ~r/[\r]?[\n]/)
    |> Enum.map(fn(s) ->
        [instruction | args] = String.split(s)
        instruction = String.to_existing_atom(instruction)
        args = Enum.map(args, &to_atom_or_integer/1)
        function = Map.get(@functions, instruction)
        Enum.concat([[instruction, function], args])
      end
    )
  end

  def to_atom_or_integer(arg) do
    case Integer.parse(arg) do
      {n, _} -> n
      :error -> String.to_existing_atom(arg)
    end
  end

  def execute(input, registers \\ @zero_registers) do
    computer = %{
      :registers => registers,
      :instructions => parse(input),
      :ip => 0
    }

    IO.inspect({computer.instructions, computer.ip, computer.registers})
    execute_instruction(computer, hd(computer.instructions), computer.ip)
  end
end

