void Main()
{
	var testInput = @"abba[mnop]qrst
abcd[bddb]xyyx
aaaa[qwer]tyui
ioxxoj[asdfgh]zxcvbn"
	.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

	var realInput = @"xdsqxnovprgovwzkus[fmadbfsbqwzzrzrgdg]aeqornszgvbizdm
itgslvpxoqqakli[arktzcssgkxktejbno]wsgkbwwtbmfnddt[zblrboqsvezcgfmfvcz]iwyhyatqetsreeyhh
pyxuijrepsmyiacl[rskpebsqdfctoqg]hbwageeiufvcmuk[wfvdhxyzmfgmcphpfnc]aotmbcnntmdltjxuusn
[... etc...]
"
	.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

	realInput
	.Count
	(
		line =>
		{
			if (Regex.IsMatch(line, @"\[.*(.)(.)\2\1\.*]"))
				return false;
			
			var match = Regex.Match(line, @"(?<a>.)(?<b>.)\2\1");
				
			return match.Success && (match.Groups["a"].Value != match.Groups["b"].Value);		
		}
	).Dump();
	
	// 183?
	// No. Too high.
  // Need to fix and try again...
}
