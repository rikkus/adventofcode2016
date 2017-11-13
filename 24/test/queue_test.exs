defmodule QueueTest do
    use ExUnit.Case
    doctest Queue
  
    test "any when empty is none" do
      assert (Queue.new |> Queue.any?) == :false
    end

    test "dequeue returns {x, [y]} when has two items" do
      assert {:x, [:y]} == Queue.dequeue([:x, :y])
    end

    test "dequeue returns {x, []} when has one item" do
      assert {:x, []} == Queue.dequeue([:x])
    end

    test "dequeue returns {nil, []} when empty" do
      assert {nil, []} == Queue.dequeue([])
    end

    
  end
  