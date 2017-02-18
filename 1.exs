defmodule Int do
  def to_unit(n) do
    case n do
      0 -> 0
      x when x < 0 -> -1
      _ -> 1
    end
  end
end

defmodule Vec do
  
  defstruct x: 0, y: 0

  def origin, do: %Vec{}
  def subtract(from, other), do: %Vec{x: other.x - from.x, y: other.y - from.y}
  def magnitude(v), do: v.x + v.y
  def manhattan_distance(vec1, vec2), do: Vec.subtract(vec1, vec2) |> Vec.magnitude
  def to_unit(vec), do: %Vec{x: Int.to_unit(vec.x), y: Int.to_unit(vec.y)}

  def move(position, direction), do: %Vec{x: position.x + direction.x, y: position.y + direction.y}
  def move(position, unit_direction, distance) do
    position |> move(%Vec{x: unit_direction.x * distance, y: unit_direction.y * distance})
  end

  @doc """
  Walk from start to finish, which must be on a horizontal or vertical line from start.
  Returns the steps taken
  """
  def walk(start, finish) do
    unit = Vec.to_unit(start |> Vec.subtract(finish))
    distance = Vec.manhattan_distance(start, finish) |> abs
    1..distance |> Enum.map(&(Vec.move(start, unit, &1)))
  end

end
 
defimpl String.Chars, for: Vec do
  def to_string(v), do: "{#{v.x}, #{v.y}}" 
end

defimpl Inspect, for: Vec do
  def inspect(v, _), do: to_string(v)
end

defmodule App do

  @n %Vec{y: 1}
  @s %Vec{y: -1}
  @e %Vec{x: 1}
  @w %Vec{x: -1}

  def turn(@n,  "R"), do: @e
  def turn(@e,  "R"), do: @s
  def turn(@s,  "R"), do: @w
  def turn(@w,  "R"), do: @n
  def turn(@n,  "L"), do: @w
  def turn(@e,  "L"), do: @n
  def turn(@s,  "L"), do: @e
  def turn(@w,  "L"), do: @s

  def parse_move_list(input) do
    input
    |> String.split(", ") # After this we have something like [RL]\d{1,3}
    |> Enum.map(
      fn(s) ->
      %{
        turn_direction: String.first(s),
        distance: elem(Integer.parse(String.slice(s, 1..-1)), 0)
      }
      end
    )
  end

 def first_visited_path_element(path, visited_nodes) do
    Enum.find(path, nil, fn(x) -> MapSet.member?(visited_nodes, x) end)
  end

  @doc """
    Find the first position at which the path given crosses itself
  """
  def find_first_crossing(input) do
    parse_move_list(input)
    |> Enum.reduce_while(
      %{
        visited: MapSet.new([Vec.origin]),
        position: Vec.origin,
        direction: @n
      },
      fn (step, acc) ->

        # Which direction will we be facing after modifying our current direction with the move's direction?
        new_direction = turn(acc.direction, step.turn_direction)

        # After moving in new_direction by the move's number of steps, where are we?
        new_position = acc.position |> Vec.move(new_direction, step.distance)

        # What is the path from where we were to where we are now?
        path = acc.position |> Vec.walk(new_position)

        # Debug...
        # IO.puts "#{inspect step} -> #{inspect new_direction} Walking #{inspect w}"

        crossing = first_visited_path_element(path, acc.visited)

        case crossing do
          nil -> {
            # No crossing found, so let's say we should continue
            :cont,
            %{
              # Add the steps we've walked to the set of visited nodes
              visited: MapSet.union(acc.visited, MapSet.new(path)),
              position: new_position,
              direction: new_direction
            }
          }
          # Ah, we found a crossing - stop!
          _ -> { :halt, crossing }
        end

      end
    )
  end # find_first_crossing

end

real_input = "R3, L2, L2, R4, L1, R2, R3, R4, L2, R4, L2, L5, L1, R5, R2, R2, L1, R4, R1, L5, L3, R4, R3, R1, L1, L5, L4, L2, R5, L3, L4, R3, R1, L3, R1, L3, R3, L4, R2, R5, L190, R2, L3, R47, R4, L3, R78, L1, R3, R190, R4, L3, R4, R2, R5, R3, R4, R3, L1, L4, R3, L4, R1, L4, L5, R3, L3, L4, R1, R2, L4, L3, R3, R3, L2, L5, R1, L4, L1, R5, L5, R1, R5, L4, R2, L2, R1, L5, L4, R4, R4, R3, R2, R3, L1, R4, R5, L2, L5, L4, L1, R4, L4, R4, L4, R1, R5, L1, R1, L5, R5, R1, R1, L3, L1, R4, L1, L4, L4, L3, R1, R4, R1, R1, R2, L5, L2, R4, L1, R3, L5, L2, R5, L4, R5, L5, R3, R4, L3, L3, L2, R2, L5, L5, R3, R4, R3, R4, R3, R1"

first_crossing = App.find_first_crossing(real_input)

IO.puts(first_crossing.x + first_crossing.y)
