using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2016_11
{
    internal class Solver
    {
        /*
         * At initial position, we have a current state and some possible moves.
         * For each move, work out the new state, then go to that state and see if we've terminated.
         * If not, add the state to the stack and go to the next move.
         * If we've tried every move, pop off the stack.
         */

        public Tracker Solve(State initialState)
        {
            var seen = new HashSet<int>();

            var actorCount = initialState.Floors.Sum(f => f.Count);

            var q = new Queue<Tracker>();
            q.Enqueue(new Tracker {State = initialState, Previous = null, Depth = 0});

            var maxDepth = 0;

            while (q.Any())
            {
                var current = q.Dequeue();

                if (current.Depth > maxDepth)
                {
                    Console.WriteLine(maxDepth = current.Depth);
                }

                /*
                Console.WriteLine($"Depth: {current.Depth}");
                Console.WriteLine(current.State);
                Console.WriteLine("-----------------------");
                */

                if (IsWinningState(current.State, actorCount))
                    return current;

                var validMoves = ValidMoves(current.State);

                foreach (var move in validMoves)
                {
                    var stateAfterApplyingMove = ApplyMove(move, current.State);
                    var stateAfterApplyingMoveKey = stateAfterApplyingMove.GetHashCode();

                    if (seen.Contains(stateAfterApplyingMoveKey))
                        continue;

                    q.Enqueue(new Tracker
                    {
                        State = stateAfterApplyingMove,
                        Previous = current,
                        Depth = current.Depth + 1
                    });

                    seen.Add(stateAfterApplyingMoveKey);
                }
            }

            return null;
        }

        private static State ApplyMove(Move move, State state)
        {
            var originalFloor = state.Elevator;
            var newFloor = NewFloor(state, move.Direction);

            return new State
            (
                newFloor,
                Enumerable.Range(0, state.Floors.Count)
                    .Zip
                    (
                        state.Floors,
                        (floorIndex, actors) => floorIndex == originalFloor
                                ? actors.Except(move.Actors).ToList()
                                : (floorIndex == newFloor ? actors.Concat(move.Actors) : actors)
                                .ToList()
                    ).ToList()
            );

        }

        private bool IsWinningState(State s, int actorCount)
        {
            return (s.Elevator == s.Floors.Count - 1)
                && (s.Floors[s.Elevator].Count == actorCount);
        }

        private static bool HasOtherGenerator(IEnumerable<Actor> floor, string element)
        {
            var generators = floor.Where(actor => actor.IsGenerator).ToArray();

            return generators.Any(generator => generator.Element != element) &&
                   generators.All(g => g.Element != element);
        }

        private static bool IsValidFloor(IList<Actor> floor)
        {
            return floor.Where(actor => actor.IsChip).All(chip => !HasOtherGenerator(floor, chip.Element));
        }

        private static readonly Direction[] UpOnly = {Direction.Up};
        private static readonly Direction[] DownOnly = {Direction.Down};
        private static readonly Direction[] UpAndDown = {Direction.Up, Direction.Down};

        private static Direction[] PossibleDirections(State state)
        {
            return state.Elevator == 0
                ? UpOnly
                : (
                    state.Elevator == state.Floors.Count - 1
                        ? DownOnly
                        : UpAndDown
                );
        }

        private static int NewFloor(State state, Direction moveDirection)
        {
            switch (moveDirection)
            {
                case Direction.Up:
                    return Math.Min(state.Elevator + 1, state.Floors.Count - 1);
                case Direction.Down:
                    return Math.Max(state.Elevator - 1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
            }
        }

        private static IEnumerable<Move> ValidMoves(State state)
        {
            return PossibleMoves(state).Where(move => IsValid(move, state));
        }

        private static bool IsValid(Move move, State state)
        {
            return IsValidFloor(state.Floors[state.Elevator].Except(move.Actors).ToList())
                   && IsValidFloor(state.Floors[NewFloor(state, move.Direction)].Concat(move.Actors).ToList());
        }

        private static IEnumerable<Move> PossibleMoves(State state)
        {
            foreach (var possibleDirection in PossibleDirections(state))
            {
                var currentFloor = state.Floors[state.Elevator];
                var actorCount = currentFloor.Count;

                for (var i = 0; i < actorCount; i++)
                {
                    for (var j = 0; j < actorCount; j++)
                    {
                        if (i == j)
                        {
                            yield return new Move {Actors = new[] { currentFloor[i] }, Direction = possibleDirection};
                        }
                        else
                        {
                            yield return new Move {Actors = new[] { currentFloor[i], currentFloor[j] }, Direction = possibleDirection};
                        }
                    }
                }
            }
        }
    }
}