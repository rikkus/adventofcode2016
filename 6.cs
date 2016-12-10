string testInput = @"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar";

string liveInput = @"cqvkxhip
tgswqbjh
[... etc ...]
";

void Main()
{
	var input = liveInput.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
	var columnWidth = input.First().Length;

	var dicts = new Dictionary<char, int>[columnWidth];

	for (int x = 0; x < columnWidth; x++)
	{
		dicts[x] = new Dictionary<char, int>(26);

		for (int i = 0; i < 26; i++)
			dicts[x][(char)(i + (int)'a')] = 0;
	}

	foreach (var line in input)
	{
		for (int i = 0; i < line.Length; i++)
		{
			dicts[i][line[i]]++;
		}
	}

	Console.WriteLine
	(
		string.Join
		(
			"",
			dicts.Select
			(
				d =>
					d.OrderByDescending(kv => kv.Value)
					.Take(5)
					.Select(kv => kv.Key)
					.First()
			)
		)
	);
	
	// gyvwpxaz?
}
