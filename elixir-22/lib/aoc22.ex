defmodule Aoc22 do
  use Application

  def is_solved(grid) do
    grid.nodes[{0, 0}].tag == {grid.max_x, 0}
  end

  def solve(grid, depth) do

    if is_solved(grid) do
      depth
    else
      Grid.all_possible_moves(grid)
        |> Enum.map(fn(move) -> solve(Grid.move(move.node, move.new_position, grid), depth + 1) end)
    end

  end

  def start(_type, _args) do
    input = File.stream!("C:\\Users\\rik\\Documents\\22-input.txt")

    IO.puts "Part one:"

    input
    |> Enum.map(&(GridNode.parse(&1)))
    |> Enum.filter(&(&1 != nil))
    |> Permute.perm_rep(2)
    |> Enum.filter(fn([a, _]) -> !GridNode.is_empty(a) end)
    |> Enum.filter(fn([a, b]) -> !GridNode.equals(a, b) end)
    |> Enum.filter(fn([a, b]) -> GridNode.would_fit_on(a, b) end)
    |> Enum.count
    |> IO.puts

    IO.puts "Part two:"

    part_2_test_input = "Filesystem            Size  Used  Avail  Use%
/dev/grid/node-x0-y0   10T    8T     2T   80%
/dev/grid/node-x0-y1   11T    6T     5T   54%
/dev/grid/node-x0-y2   32T   28T     4T   87%
/dev/grid/node-x1-y0    9T    7T     2T   77%
/dev/grid/node-x1-y1    8T    0T     8T    0%
/dev/grid/node-x1-y2   11T    7T     4T   63%
/dev/grid/node-x2-y0   10T    6T     4T   60%
/dev/grid/node-x2-y1    9T    8T     1T   88%
/dev/grid/node-x2-y2    9T    6T     3T   66%"

    # Assumption: We never split data from a node onto more than one node.

    all_nodes = part_2_test_input
    |> String.split("\n")
    |> Enum.map(&(GridNode.parse(&1)))
    |> Enum.filter(&(&1 != nil))

    max_x = all_nodes |> Enum.map(&(&1.x)) |> Enum.max
    max_y = all_nodes |> Enum.map(&(&1.y)) |> Enum.max

    node_map = all_nodes |> Enum.map(fn(node) -> {{node.x, node.y}, node} end) |> Enum.into(%{})

    grid = %Grid{max_x: max_x, max_y: max_y, nodes: node_map}

    IO.puts solve(grid, 0)

    Supervisor.start_link [], strategy: :one_for_one
  end
end
