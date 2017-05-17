defmodule Grid do
  defstruct max_x: 0, max_y: 0, nodes: %{}

  def all_possible_moves(grid) do
    Map.values(grid.nodes)
    |> Enum.map(fn(node) -> %{node: node, moves: possible_moves(node, grid)} end)
    |> Enum.filter(fn(node_and_moves) -> !Enum.empty?(node_and_moves.moves) end)
  end

  def move(node, new_position, grid) do
    %Grid{
      max_x: grid.max_x,
      max_y: grid.max_y,
      nodes: move_node(node, new_position, grid.nodes)
    }
  end

  defp move_node(from_node, new_position, nodes) do
    [new_x, new_y] = [elem(new_position, 0), elem(new_position, 1)]
    Map.values(nodes)
    |> Enum.map(fn(n) ->
        cond do
          n.x == from_node.x && n.y == from_node.y -> %GridNode{size: n.size, used: 0, size: n.size, x: n.x, y: n.y}
          n.x == new_x && n.y == new_y -> %GridNode{size: n.size, used: from_node.used, size: n.size, x: n.x, y: n.y}
          n -> n
        end
      end
    )
  end

  defp possible_moves(node, grid) do
    node
    |> possible_directions(grid.max_x, grid.max_y)
    |> Enum.filter(fn(pos) -> GridNode.would_fit_on(node, grid.nodes[pos]) end)
  end

  defp possible_directions(node, max_x, max_y) do
    case {node.x, node.y} do
      {0, 0} -> [{0, 1}, {1, 0}]
      {0, y} when y === max_y -> [{0, max_y - 1}, {1, max_y}]
      {x, 0} when x === max_x -> [{max_x - 1, 0}, {max_x, 1}]
      {x, y} when x === max_x and y === max_y -> [{x - 1, y}, {x, y - 1}]
      {x, y} when x === max_x -> [{x - 1, y}, {x, y - 1}, {x, y + 1}]
      {x, y} when y === max_y -> [{x, y - 1}, {x - 1, y}, {x + 1, y}]
      {0, y} -> [{1, y}, {0, y - 1}, {0, y + 1}]
      {x, 0} -> [{x, 1}, {x - 1, 0}, {x + 1, 0}]
      {x, y} -> [{x - 1, y}, {x + 1, y}, {x, y - 1}, {x, y + 1}]
    end
  end
end