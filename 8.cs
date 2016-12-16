Func<int, int, char[][]> makeScreen = (w, h) =>
{
	var a = new char[h][];
	for (int y = 0; y < h; y++)
	{
		a[y] = new String('.', w).ToCharArray();
	}
	return a;
};

var realScreen = makeScreen(50, 6);
var testScreen = makeScreen(7, 3);

var realInput = @"rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 3
rect 1x1
rotate row y=0 by 3
rect 2x1
rotate row y=0 by 5
rect 1x1
rotate row y=0 by 5
rect 4x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 2
rect 1x1
rotate row y=0 by 5
rect 4x1
rotate row y=0 by 3
rect 2x1
rotate row y=0 by 5
rect 4x1
rotate row y=0 by 2
rect 1x2
rotate row y=1 by 6
rotate row y=0 by 2
rect 1x2
rotate column x=32 by 1
rotate column x=23 by 1
rotate column x=13 by 1
rotate row y=0 by 6
rotate column x=0 by 1
rect 5x1
rotate row y=0 by 2
rotate column x=30 by 1
rotate row y=1 by 20
rotate row y=0 by 18
rotate column x=13 by 1
rotate column x=10 by 1
rotate column x=7 by 1
rotate column x=2 by 1
rotate column x=0 by 1
rect 17x1
rotate column x=16 by 3
rotate row y=3 by 7
rotate row y=0 by 5
rotate column x=2 by 1
rotate column x=0 by 1
rect 4x1
rotate column x=28 by 1
rotate row y=1 by 24
rotate row y=0 by 21
rotate column x=19 by 1
rotate column x=17 by 1
rotate column x=16 by 1
rotate column x=14 by 1
rotate column x=12 by 2
rotate column x=11 by 1
rotate column x=9 by 1
rotate column x=8 by 1
rotate column x=7 by 1
rotate column x=6 by 1
rotate column x=4 by 1
rotate column x=2 by 1
rotate column x=0 by 1
rect 20x1
rotate column x=47 by 1
rotate column x=40 by 2
rotate column x=35 by 2
rotate column x=30 by 2
rotate column x=10 by 3
rotate column x=5 by 3
rotate row y=4 by 20
rotate row y=3 by 10
rotate row y=2 by 20
rotate row y=1 by 16
rotate row y=0 by 9
rotate column x=7 by 2
rotate column x=5 by 2
rotate column x=3 by 2
rotate column x=0 by 2
rect 9x2
rotate column x=22 by 2
rotate row y=3 by 40
rotate row y=1 by 20
rotate row y=0 by 20
rotate column x=18 by 1
rotate column x=17 by 2
rotate column x=16 by 1
rotate column x=15 by 2
rotate column x=13 by 1
rotate column x=12 by 1
rotate column x=11 by 1
rotate column x=10 by 1
rotate column x=8 by 3
rotate column x=7 by 1
rotate column x=6 by 1
rotate column x=5 by 1
rotate column x=3 by 1
rotate column x=2 by 1
rotate column x=1 by 1
rotate column x=0 by 1
rect 19x1
rotate column x=44 by 2
rotate column x=40 by 3
rotate column x=29 by 1
rotate column x=27 by 2
rotate column x=25 by 5
rotate column x=24 by 2
rotate column x=22 by 2
rotate column x=20 by 5
rotate column x=14 by 3
rotate column x=12 by 2
rotate column x=10 by 4
rotate column x=9 by 3
rotate column x=7 by 3
rotate column x=3 by 5
rotate column x=2 by 2
rotate row y=5 by 10
rotate row y=4 by 8
rotate row y=3 by 8
rotate row y=2 by 48
rotate row y=1 by 47
rotate row y=0 by 40
rotate column x=47 by 5
rotate column x=46 by 5
rotate column x=45 by 4
rotate column x=43 by 2
rotate column x=42 by 3
rotate column x=41 by 2
rotate column x=38 by 5
rotate column x=37 by 5
rotate column x=36 by 5
rotate column x=33 by 1
rotate column x=28 by 1
rotate column x=27 by 5
rotate column x=26 by 5
rotate column x=25 by 1
rotate column x=23 by 5
rotate column x=22 by 1
rotate column x=21 by 2
rotate column x=18 by 1
rotate column x=17 by 3
rotate column x=12 by 2
rotate column x=11 by 2
rotate column x=7 by 5
rotate column x=6 by 5
rotate column x=5 by 4
rotate column x=3 by 5
rotate column x=2 by 5
rotate column x=1 by 3
rotate column x=0 by 4"
.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

var testInput = @"rect 3x2
rotate column x=1 by 1
rotate row y=0 by 4
rotate column x=1 by 1"
.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

var rectExpr = new Regex(@"^rect (?<x>\d)+x(?<y>\d+)$");
var rotateColumnExpr = new Regex(@"^rotate column x=(?<x>\d+) by (?<n>\d+)$");
var rotateRowExpr = new Regex(@"^rotate row y=(?<y>\d+) by (?<n>\d+)$");

Action<char[][], int, int> Rect = (char[][] a, int w, int h) =>
{
	for (int y = 0; y < h; y++)
	{
		for (int x = 0; x < w; x++)
		{
			a[y][x] = '#';
		}
	}
};

Func<string, int, string> ShiftRight = (string s, int count) =>
{
	if (count == 0 || count == s.Length)
		return s;

	count = count % s.Length;

	var overflow = s.Substring(s.Length - count, count);

	var left = s.Substring(0, s.Length - count);

	return overflow + left;
};

Action<Char[][], int, int> ShiftDown = (Char[][] a, int index, int count) =>
{
	string s = new String(a.Select(b => b[index]).ToArray());
	s = ShiftRight(s, count);

	for (int i = 0; i < a.Length; i++)
	{
		a[i][index] = s[i];
	}
};

Action<char[][]> Print = (char[][] a) =>
{
	for (int y = 0; y < a.Length; y++)
	{
		Console.WriteLine(new String(a[y]));
	}
};

var input = realInput;
var screen = realScreen;

foreach (var line in input)
{
	// Print(screen);
	
	var match = rectExpr.Match(line);

	if (match.Success)
	{
		var w = int.Parse(match.Groups["x"].Value);
		var h = int.Parse(match.Groups["y"].Value);

		Console.WriteLine($"rect({w}, {h})");

		Rect(screen, w, h);
		continue;
	}
	
	match = rotateColumnExpr.Match(line);

	if (match.Success)
	{
		var x = int.Parse(match.Groups["x"].Value);
		var n = int.Parse(match.Groups["n"].Value);

		ShiftDown(screen, x, n);
		
		Console.WriteLine($"rotate(x={x}, {n})");
		continue;
	}
	
	match = rotateRowExpr.Match(line);

	if (match.Success)
	{
		var y = int.Parse(match.Groups["y"].Value);
		var n = int.Parse(match.Groups["n"].Value);

		screen[y] = ShiftRight(new String(screen[y]), n).ToCharArray();
		
		Console.WriteLine($"rotate(y={y}, {n})");
		continue;
	}
	
	throw new Exception("Bad input line: '" + line + "'");
}

Print(screen);

screen.Sum(line => line.Sum(c => c == '#' ? 1 : 0)).Dump();

// 70? No, too low.
