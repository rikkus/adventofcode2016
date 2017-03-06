defmodule Compression do
  @expr ~r/\((?<len>\d+)+x(?<count>\d+)\)/
  def decompressed_length(input) do
    matches = Regex.run(@expr, input, return: :index)
    case matches do
      [{match_start, match_length}, {length_start, length_length}, {count_start, count_length}] ->
        length = String.to_integer(String.slice(input, length_start, length_length))
        count = String.to_integer(String.slice(input, count_start, count_length))
        split_position = match_start + match_length

        # xyz(lxc)qrs
        # | ||   || |
        # | ||   || |
        # \_\_______|___ before
        #    |   || |
        #    \___\______ match (length x count)
        #         | |
        #         \_\___ rest

        to_repeat = String.slice(input, split_position, length)
        rest = String.slice(input, (split_position + length)..-1)

        # Assumption: If we don't match in to_repeat, we won't match in repeated. Not necessarily true,
        # but it happens that the data we've been provided is simple.
        match_start + (decompressed_length(to_repeat) * count) + decompressed_length(rest)
      nil -> String.length(input)
    end
  end

  def decompress(input) do
    matches = Regex.run(@expr, input, return: :index)
    case matches do
      [{match_start, match_length}, {length_start, length_length}, {count_start, count_length}] ->
        length = String.to_integer(String.slice(input, length_start, length_length))
        count = String.to_integer(String.slice(input, count_start, count_length))
        before_match = (match_start == 0) && "" || String.slice(input, 0..(match_start - 1))
        after_match = String.slice(input, (match_start + match_length)..-1)
        to_repeat = String.slice(after_match, 0..(length - 1))
        repeated = String.duplicate(to_repeat, count)
        rest = String.slice(after_match, length..-1)
        before_match <> decompress(repeated <> rest)
      nil -> input
    end
  end
end

ExUnit.start()
defmodule CompressionTest do
  use ExUnit.Case

  test "simple" do
    assert Compression.decompress("(3x3)XYZ") == "XYZXYZXYZ"
  end

  test "multipart" do
    assert Compression.decompress("X(8x2)(3x3)ABCY") == "XABCABCABCABCABCABCY"
  end

  test "complex" do
    assert Compression.decompressed_length("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN") == 445
  end

  @tag timeout: 10000
  test "long" do
    assert Compression.decompressed_length("(27x12)(20x12)(13x14)(7x10)(1x12)A") == 241920
  end

  @tag timeout: 10000
  test "huge" do
    {:ok, input} = File.read("9.txt")
    IO.puts(Compression.decompressed_length(String.strip(input)))
    # 11451628995 was the right answer
  end
end

