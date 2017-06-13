defmodule GridNode do

  defstruct x: nil, y: nil, size: nil, used: nil, tag: {-1, -1}

  def to_s(node, grid) do

    percent_full = (node.used / node.size) * 100

    {l, r} = case {node.x, node.y} do
      {0, 0}  -> {"(", ")"}
      _       -> {" ", " "}
    end

    m = cond do
      node.tag == {grid.max_x, 0}         -> "G"
      percent_full > 85 && node.used > 20 -> "#"
      node.used == 0                      -> "_"
      true                                -> "."
    end

    l <> m <> r

  end

  def avail(s) do
    s.size - s.used
  end

  def parse(line) do

    case Regex.named_captures(~r/-x(?<x>\d+)-y(?<y>\d+)\s+(?<size>\d+)T\s+(?<used>\d+)T/, line) do
      %{"x" => x, "y" => y, "size" => size, "used" => used} ->
        %GridNode{
          x: String.to_integer(x),
          y: String.to_integer(y),
          size: String.to_integer(size),
          used: String.to_integer(used),
          tag: {String.to_integer(x), String.to_integer(y)}
        }
      _ -> nil
    end

  end

  def is_empty(node) do
    node.used == 0
  end

  def equals(a, b) do
    (a.x == b.x) && (a.y == b.y)
  end

  def would_fit_on(node1, node2) do
    node1.used > 0 and node1.used <= GridNode.avail(node2)
  end

end