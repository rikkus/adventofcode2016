defmodule E24 do

  def maybe_move(location, path) do
    case MapSet.member?(path, location) do
      true -> [location]
      _ -> []
    end
  end

  def adjacent_pairs(vertex, vertices) do
    [
      %{x: vertex.pos.x - 1, y: vertex.pos.y},
      %{x: vertex.pos.x + 1, y: vertex.pos.y},
      %{x: vertex.pos.x, y: vertex.pos.y - 1},
      %{x: vertex.pos.x, y: vertex.pos.y + 1}
    ]
    |> Enum.filter(fn(v) -> Map.has_key?(vertices, v) end)
    |> Enum.map(fn(v) -> Map.get(vertices, v) end)
  end
 
  defp edges(vertices) do
    pos_to_n = vertices |> Map.new(fn(v) -> {v.pos, v} end)
    vertices |> Enum.flat_map(
      fn(vertex) ->
        Enum.zip(
          Stream.cycle([vertex]),
          adjacent_pairs(vertex, pos_to_n)
        )
      end
    )
  end

  defp to_graph_where_up_down_left_right_are_adjacent(vertices) do
    g = Graph.new()
    {_, g} = Enum.map_reduce(vertices, g, fn(v, g) -> {v, Graph.add_vertex(g, v, "#{v.n} (#{v.pos.x},#{v.pos.y})")} end)
    {_, g} = Enum.map_reduce(edges(vertices), g, fn(e, g) -> {e, Graph.add_edge(g, elem(e, 0), elem(e, 1))} end)
    {_, g} = Enum.map_reduce(edges(vertices), g, fn(e, g) -> {e, Graph.add_edge(g, elem(e, 1), elem(e, 0))} end)
    g
  end

  defp number_vertices(vertices) do
    vertices |> Enum.filter(fn(v) ->
        case Integer.parse(v.n) do
          {_, ""} -> true
          _ -> false
        end
      end
    )
  end

  defp path(input, width) do
    Regex.scan(~r/[0-9\.]/, input, return: :index)
      |> Enum.map(
        fn([{i, _}]) -> %{
            n: String.at(input, i),
            pos: %{
              x: rem(i, width + 1),
              y: div(i, width + 1)
            }
          }
        end
      )
      |> MapSet.new
  end

  defp width(input) do
    input
      |> String.split("\n")
      |> Enum.at(0)
      |> String.length
  end

  defp pair_up(list) do
    # pair up
    # [:a, :b, :c] => [[:a, :b], [:b, :c]]
    Enum.chunk_every(list, 2, 1, :discard)
  end

  defp full_path(vertices, opts) do
    case opts do
      :return_to_start -> Enum.concat([vertices, hd(vertices)])
      _ -> vertices
    end
  end

  defp shortest_path_visiting_all(graph, vertices, opts) do

    full_path = full_path(vertices, opts)

    pair_up(vertices)
    |> Enum.map_reduce(
      0,
      fn(pair, acc) ->
        length = Enum.count(Graph.dijkstra(graph, Enum.at(pair, 0), Enum.at(pair, 1))) - 1
        {length, acc + length}
      end
    )
    |> elem(1)
  end

  def shortest(input, opts \\ nil) do

    width = width(input)
    path = path(input, width)

    graph = to_graph_where_up_down_left_right_are_adjacent(path)

    number_vertices = number_vertices(path)

    starting_location = Enum.find(number_vertices, fn(v) -> v.n == "0" end)
    non_starting_locations = Enum.filter(number_vertices, fn(v) -> v.n != "0" end)

    permutations = non_starting_locations
    |> Combination.permutate
    |> Enum.map(fn(permutation) -> Enum.concat([[starting_location], permutation]) end)

    stream = Task.async_stream(permutations, fn(permutation) -> shortest_path_visiting_all(graph, permutation, opts) end)

    {:ok, min} = Enum.min(stream)

    min
  end

end

