defmodule E23Test do
  use ExUnit.Case
  use Stopwatch
  doctest E23

  @input "
  cpy a b
  dec b
  cpy a d
  cpy 0 a
  cpy b c
  inc a
  dec c
  jnz c -2
  dec d
  jnz d -5
  dec b
  cpy b c
  cpy c d
  dec d
  inc c
  jnz d -2
  tgl c
  cpy -16 c
  jnz 1 c
  cpy 95 c
  jnz 73 d
  inc a
  inc d
  jnz d -2
  inc c
  jnz c -5
" |> String.trim 

@test_input "
cpy 2 a
tgl a
tgl a
tgl a
cpy 1 a
dec a
dec a
" |> String.trim 

  test "example" do
    assert E23.execute(@test_input) == 3
  end

  test "part one" do
    assert E23.execute(@input, %{:a => 7, :b => 0, :c => 0, :d => 0}) == 11975
  end

  @tag timeout: 6000000
  test "part two" do
    assert E23.execute(@input, %{:a => 12, :b => 0, :c => 0, :d => 0}) == 0
  end
end
