defmodule Aoc21 do

  def scramble(password, input) do
    input |> Enum.reduce(password, fn(line, acc) -> modify(acc, line |> String.split(" ")) end)
  end

  def modify(password, ["swap", "position", x, "with", "position", y]) do
    [a, b] = Enum.sort([String.to_integer(x), String.to_integer(y)])
    String.slice(password, 0, a) <> String.at(password, b) <> String.slice(password, a + 1, b - a - 1) <> String.at(password, a) <> String.slice(password, (b + 1)..-1)
  end

  def modify(password, ["swap", "letter", letter, "with", "letter", other_letter]) do
    password |> String.codepoints |> Enum.map(fn(c) ->
        case c do
          ^letter -> other_letter
          ^other_letter -> letter
          x -> x
        end
      end
    ) |> Enum.join
  end

  def modify(password, ["reverse", "positions", x, "through", y]) do
    [a, b] = Enum.sort([String.to_integer(x), String.to_integer(y)])
    String.slice(password, 0, a) <> String.reverse(String.slice(password, a, b - a + 1)) <> String.slice(password, b + 1, b..-1)
  end

  def modify(password, ["rotate", "left", _, "step"]) do
    rotate(password, 1)
  end 

  def modify(password, ["rotate", "left", n, "steps"]) do
    rotate(password, String.to_integer(n))
  end 

  def modify(password, ["rotate", "right", _, "step"]) do
    rotate(password, 1)
  end 

  def modify(password, ["rotate", "right", n, "steps"]) do
    rotate(password, -String.to_integer(n))
  end 

  def modify(password, ["rotate", "based", "on", "position", "of", "letter", c]) do
    {pos, _} = :binary.match(password, c)
    n = 1 + pos + (if pos >= 4, do: 1, else: 0)
    rotate(password, -n)
  end 
  
  def modify(password, ["move", "position", x, "to", "position", y]) do
    [x, y] = [String.to_integer(x), String.to_integer(y)]
    c = String.at(password, x)
    removed = String.slice(password, 0, x) <> String.slice(password, x + 1, x..-1)
    String.slice(removed, 0, y) <> c <> String.slice(removed, y, y..-1)
  end

  def rotate(password, n) do
    l = String.length(password)
    x = rem(n, l)
    x = if x < 0, do: l + x, else: x
    String.slice(password, x, x..-1) <> String.slice(password, 0, x)
  end 

end

defmodule Permute do
  def permute([]), do: [[]]
  def permute(list) do
    for x <- list, y <- permute(list -- [x]), do: [x|y]
  end
end

input = File.read!("input.txt") |>
  String.split("\n") |>
  Enum.map(&(String.trim(&1))) |>
  Enum.filter(&(String.length(&1) > 0))

Permute.permute(String.codepoints("abcdefgh")) |>
  Enum.map(fn(permutation) -> { Enum.join(permutation), Aoc21.scramble(Enum.join(permutation), input) } end) |>
  Enum.filter(fn(x) -> elem(x, 1) == "fbgdceah" end) |>
  IO.inspect
