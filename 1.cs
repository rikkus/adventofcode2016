struct Vector {
	public int X;
	public int Y;
	
	public override bool Equals(object other) {
		if (other == null)
			return false;
	
		if (other.GetType() != typeof(Vector))
			return false;
		
		return ((Vector)other).X == X && ((Vector)other).Y == Y;
	}
};

Vector North = new Vector { X = 0, Y = 1 };
Vector East = new Vector { X = 1, Y = 0 };
Vector South = new Vector { X = 0, Y = -1 };
Vector West = new Vector { X = -1, Y = 0 };

Vector TurnVector(Vector currentDirection, char turn)
{
	switch (turn)
	{
		case 'R':
			if (currentDirection.Equals(North))
				return East;
			if (currentDirection.Equals(East))
				return South;
			if (currentDirection.Equals(South))
				return West;
			return North;			
			
		case 'L':
			if (currentDirection.Equals(North))
				return West;
			if (currentDirection.Equals(West))
				return South;
			if (currentDirection.Equals(South))
				return East;
			return North;
			
		default:
			throw new ArgumentOutOfRangeException(nameof(turn), turn, "Must be R or L");
	}
}

void Main()
{
	var newState = "R3, L2, L2, R4, L1, R2, R3, R4, L2, R4, L2, L5, L1, R5, R2, R2, L1, R4, R1, L5, L3, R4, R3, R1, L1, L5, L4, L2, R5, L3, L4, R3, R1, L3, R1, L3, R3, L4, R2, R5, L190, R2, L3, R47, R4, L3, R78, L1, R3, R190, R4, L3, R4, R2, R5, R3, R4, R3, L1, L4, R3, L4, R1, L4, L5, R3, L3, L4, R1, R2, L4, L3, R3, R3, L2, L5, R1, L4, L1, R5, L5, R1, R5, L4, R2, L2, R1, L5, L4, R4, R4, R3, R2, R3, L1, R4, R5, L2, L5, L4, L1, R4, L4, R4, L4, R1, R5, L1, R1, L5, R5, R1, R1, L3, L1, R4, L1, L4, L4, L3, R1, R4, R1, R1, R2, L5, L2, R4, L1, R3, L5, L2, R5, L4, R5, L5, R3, R4, L3, L3, L2, R2, L5, L5, R3, R4, R3, R4, R3, R1"	
		.Split(new[] { ", " }, StringSplitOptions.None)
		.Select(pair => new { TurnDirection = pair[0], Distance = int.Parse(pair.Substring(1)) })
		.Aggregate(
			new { Position = new Vector(), Direction = North },
			(state, instruction) =>
			{
				var newDirection = TurnVector(state.Direction, instruction.TurnDirection);

				return new
				{
					Position = new Vector
					{
						X = state.Position.X + newDirection.X * instruction.Distance,
						Y = state.Position.Y + newDirection.Y * instruction.Distance
					},
					Direction = newDirection
				};
            }
		);
	
	newState.Dump();
	
	var blocksAway = (newState.Position.X + newState.Position.Y);
	
	blocksAway.Dump();
		
	// 262?
}
