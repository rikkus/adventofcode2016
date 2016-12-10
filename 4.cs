static Regex matcher = new Regex(@"^(?<letters>[a-z-]+)-(?<sectorid>\d+)\[(?<checksum>[a-z]{5})\]$");

public struct Row
{
	public string Letters;
	public uint SectorID;
	public string CheckSum;
	public bool IsValid;

	public static Row Parse(string s)
	{
		var match = matcher.Match(s);

		if (match.Groups.Count != 4)
		{
			Console.WriteLine("Can't match " + s);
			return new Row();
		}

		var letters = match.Groups["letters"].Value.Replace("-", "");
		var sectorID = match.Groups["sectorid"].Value;
		var checksum = match.Groups["checksum"].Value;

		return new Row
		{
			Letters = letters,
			SectorID = uint.Parse(sectorID),
			CheckSum = checksum,
			IsValid = CheckSumValid(letters, checksum)
		};
	}

	private static bool CheckSumValid(string letters, string checkSum)
	{
		var letterFrequency = new Dictionary<char, int>(26);

		foreach (var letter in "abcdefghijklmnopqrstuvwxyz")
		{
			letterFrequency[letter] = 0;
		}

		foreach (var letter in letters.Where(v => v != '-'))
		{
			letterFrequency[letter]++;
		}

		var calculatedChecksum =
		string.Join
		(
			"",
			letterFrequency
				.OrderByDescending(x => x.Value)
				.ThenBy(x => x.Key)
				.Take(5)
				.Select(x => x.Key)
		);

		return calculatedChecksum == checkSum;
	}
}

void Main()
{
	var input = @"hqcfqwydw-fbqijys-whqii-huiuqhsx-660[qhiwf]
oxjmxdfkd-pzxsbkdbo-erkq-ixyloxqlov-913[xodkb]
bpvctixr-eaphixr-vgphh-gthtpgrw-947[smrkl]
[... etc]"
	.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
	
	var testInput =
	@"aaaaa-bbb-z-y-x-123[abxyz]
a-b-c-d-e-f-g-h-987[abcde]
not-a-real-room-404[oarel]
totally-real-room-200[decoy]
"
		.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		
	input
		.Select(row => Row.Parse(row))
		.Where(row => row.IsValid)
		.Sum(row => row.SectorID)
		.Dump();
		
	// 245102?
}
