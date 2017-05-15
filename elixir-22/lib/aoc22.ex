defmodule Aoc22 do
  use Application

  def start(_type, _args) do
    input = File.stream!("C:\\Users\\rik\\Documents\\22-input.txt")

    input
    |> Enum.map(&(GridNode.parse(&1)))
    |> Enum.filter(&(&1 != nil))
    |> Permute.perm_rep(2)
    |> Enum.filter(fn([a, _]) -> !GridNode.is_empty(a) end)
    |> Enum.filter(fn([a, b]) -> !GridNode.equals(a, b) end)
    |> Enum.filter(fn([a, b]) -> GridNode.would_fit_on(a, b) end)
    |> Enum.count
    |> IO.puts

    Supervisor.start_link [], strategy: :one_for_one
  end
end
