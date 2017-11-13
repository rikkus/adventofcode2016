defmodule E24Test do
  use ExUnit.Case
  use Stopwatch
  doctest E24

  @test_input "
###########
#0.1.....2#
#.#######.#
#4.......3#
###########
" |> String.trim 

  test "wins" do
    watch = Watch.new
    shortest = E24.shortest(@test_input)
    IO.inspect {:elapsed, Watch.get_total_time(Watch.stop(watch))}
    assert shortest == 14
  end

  @tag timeout: 3600000
  @tag :skip
  test "part_one" do
    # 204 seconds (parallel, 4 cores)
    watch = Watch.new
    shortest = E24.shortest(File.read!("input.txt"))
    IO.inspect {:elapsed, Watch.get_total_time(Watch.stop(watch))}
    assert shortest == 518
  end

  @tag timeout: 3600000
  test "part_two" do
    # 347 seconds (parallel, 4 cores)
    watch = Watch.new
    shortest = E24.shortest(File.read!("input.txt"), :return_to_start)
    IO.inspect {:elapsed, Watch.get_total_time(Watch.stop(watch))}
    assert shortest == 716
  end

end
