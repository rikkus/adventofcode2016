defmodule Queue do
    def new do
        []
    end
    def new(list) do
        list
    end
    def dequeue(list) do
        case list do
            [hd|tl] -> {hd, tl}
            [hd] -> {hd, []}
            [] -> {nil, []}
        end
    end
    def any?(list) do 
        Enum.any?(list)
    end
end