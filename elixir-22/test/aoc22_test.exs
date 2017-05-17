defmodule Aoc22Test do
  use ExUnit.Case
  doctest Aoc22

  def assert_set_equal(enum1, enum2) do
    assert MapSet.new(enum1) == MapSet.new(enum2)
  end

  test "the truth" do
    assert 1 + 1 == 2
  end

  test "2_x_2_from_0_0_r_and_d" do
    assert_set_equal [{0, 1}, {1, 0}], Grid.possible_directions(%GridNode{x: 0, y: 0}, 1, 1)
  end

  test "2_x_2_from_1_1_u_and_l" do
    assert_set_equal [{0, 1}, {1, 0}], Grid.possible_directions(%GridNode{x: 1, y: 1}, 1, 1)
  end

  test "2_x_2_from_0_1_r_and_u" do
    assert_set_equal [{0, 0}, {1, 1}], Grid.possible_directions(%GridNode{x: 0, y: 1}, 1, 1)
  end

  test "2_x_2_from_0_1_l_and_d" do
    assert_set_equal [{0, 0}, {1, 1}], Grid.possible_directions(%GridNode{x: 1, y: 0}, 1, 1)
  end

  test "3_x_3_from_0_0_u_d_r" do
    assert_set_equal [{1, 1}, {0, 0}, {0, 2}], Grid.possible_directions(%GridNode{x: 0, y: 1}, 2, 2)
  end

  test "3_x_3_from_0_1_u_d_r" do
    assert_set_equal [{1, 1}, {0, 0}, {0, 2}], Grid.possible_directions(%GridNode{x: 0, y: 1}, 2, 2)
  end

  test "3_x_3_from_0_2_u_r" do
    assert_set_equal [{0, 1}, {1, 2}], Grid.possible_directions(%GridNode{x: 0, y: 2}, 2, 2)
  end

  test "3_x_3_from_1_0_l_d_r" do
    assert_set_equal [{0, 0}, {1, 1}, {2, 0}], Grid.possible_directions(%GridNode{x: 1, y: 0}, 2, 2)
  end

  test "3_x_3_from_1_1_u_d_l_r" do
    assert_set_equal [{0, 1}, {2, 1}, {1, 0}, {1, 2}], Grid.possible_directions(%GridNode{x: 1, y: 1}, 2, 2)
  end

  test "3_x_3_from_1_2_l_u_r" do
    assert_set_equal [{1, 1}, {0, 2}, {2, 2}], Grid.possible_directions(%GridNode{x: 1, y: 2}, 2, 2)
  end

  test "3_x_3_from_2_0_l_d" do
    assert_set_equal [{1, 0}, {2, 1}], Grid.possible_directions(%GridNode{x: 2, y: 0}, 2, 2)
  end

  test "3_x_3_from_2_1_u_l_d" do
    assert_set_equal [{2, 0}, {1, 1}, {2, 2}], Grid.possible_directions(%GridNode{x: 2, y: 1}, 2, 2)
  end

  test "3_x_3_from_2_2_l_u" do
    assert_set_equal [{1, 2}, {2, 1}], Grid.possible_directions(%GridNode{x: 2, y: 2}, 2, 2)
  end
end
