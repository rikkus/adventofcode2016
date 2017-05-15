defmodule GridNode do

  defstruct x: nil, y: nil, size: nil, used: nil

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
          used: String.to_integer(used)
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
    node1.used <= GridNode.avail(node2)
  end

end