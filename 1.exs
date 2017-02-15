defmodule Vec, do: defstruct x: 0, y: 0

defimpl String.Chars, for: Vec do
    def to_string(v), do: "{#{v.x}, #{v.y}}" 
end

defimpl Inspect, for: Vec do
    def inspect(v, _), do: to_string(v)
end

defmodule App do

    @origin %Vec{}

    @n %Vec{y: 1}
    @s %Vec{y: -1}
    @e %Vec{x: 1}
    @w %Vec{x: -1}

    def turnVector(@n,  "R"), do: @e
    def turnVector(@e,  "R"), do: @s
    def turnVector(@s,  "R"), do: @w
    def turnVector(@w,  "R"), do: @n
    def turnVector(@n,  "L"), do: @w
    def turnVector(@e,  "L"), do: @n
    def turnVector(@s,  "L"), do: @e
    def turnVector(@w,  "L"), do: @s

    def parseMoveList(input) do
        String.split(input, ", ")
        # Now we have something like [RL]\d{1,3}
        |> Enum.map(
            fn(s) ->
            %{
                turnDirection: String.first(s),
                distance: elem(Integer.parse(String.slice(s, 1..-1)), 0)
            }
            end
        )
    end
 
    @doc """
    Walk from start to finish, which must be on a horizontal or vertical line from start.
    Returns the steps taken
    """
    def walk(start, finish) do
        sign =  if start.x == finish.x do
                if start.y < finish.y, do: 1, else: -1
            else
                if start.x < finish.x, do: 1, else: -1
            end

        if (start.x == finish.x) do
            Stream.iterate(1, &(&1+1))
                    |> Enum.take(abs(finish.y - start.y))
                    |> Enum.map(fn(y) -> %Vec{x: start.x, y: start.y + y * sign} end)
            else
                Stream.iterate(1, &(&1+1))
                    |> Enum.take(abs(finish.x - start.x))
                    |> Enum.map(fn(x) -> %Vec{x: start.x + x * sign, y: start.y} end)
        end
    end

    @doc """
        Find the first position at which the path given crosses itself
    """
    def findFirstCrossing(input) do
        parseMoveList(input)
        |> Enum.reduce_while(
            %{
                visited: MapSet.new([@origin]),
                position: @origin,
                direction: @n
            },
            fn (item, acc) ->

                # Which direction will we be facing after modifying our current direction with the move's direction?
                newDirection = turnVector(acc[:direction], item[:turnDirection])

                # After moving in newDirection by the move's number of steps, where are we?
                newPosition = %Vec{
                    x: acc[:position].x + newDirection.x * item[:distance],
                    y: acc[:position].y + newDirection.y * item[:distance]
                }

                # What is the path from where we were to where we are now?
                w = walk(acc[:position], newPosition)

                # Debug...
                # IO.puts "#{inspect item} -> #{inspect newDirection} Walking #{inspect w}"

                # Find where we cross a visited node (if at all).
                crossing = Enum.find(w, nil, fn(x) -> MapSet.member?(acc[:visited], x) end)


                case crossing do
                    nil -> {
                        # No crossing found, so let's say we should continue
                        :cont,
                        %{
                            # Add the steps we've walked to the set of visited nodes
                            visited: MapSet.union(acc[:visited], MapSet.new(w)),
                            position: newPosition,
                            direction: newDirection
                        }
                    }
                    # Ah, we found a crossing - stop!
                    _ -> { :halt, crossing }
                end

            end
        )
    end
end

realInput = "R3, L2, L2, R4, L1, R2, R3, R4, L2, R4, L2, L5, L1, R5, R2, R2, L1, R4, R1, L5, L3, R4, R3, R1, L1, L5, L4, L2, R5, L3, L4, R3, R1, L3, R1, L3, R3, L4, R2, R5, L190, R2, L3, R47, R4, L3, R78, L1, R3, R190, R4, L3, R4, R2, R5, R3, R4, R3, L1, L4, R3, L4, R1, L4, L5, R3, L3, L4, R1, R2, L4, L3, R3, R3, L2, L5, R1, L4, L1, R5, L5, R1, R5, L4, R2, L2, R1, L5, L4, R4, R4, R3, R2, R3, L1, R4, R5, L2, L5, L4, L1, R4, L4, R4, L4, R1, R5, L1, R1, L5, R5, R1, R1, L3, L1, R4, L1, L4, L4, L3, R1, R4, R1, R1, R2, L5, L2, R4, L1, R3, L5, L2, R5, L4, R5, L5, R3, R4, L3, L3, L2, R2, L5, L5, R3, R4, R3, R4, R3, R1"

firstCrossing = App.findFirstCrossing(realInput)

IO.puts(firstCrossing.x + firstCrossing.y)
