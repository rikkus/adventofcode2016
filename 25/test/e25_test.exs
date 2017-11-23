defmodule E25Test do
  use ExUnit.Case
  use Stopwatch
  doctest E25

  @test_input "
Hello, world!
" |> String.trim 

  test "doit" do
    assert E25.doit == 42
  end

end
