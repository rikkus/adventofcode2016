void Main()
{
	var doorID = "uqwqemis";
	var password = "";

	var i = 0;
	
	while (true)
	{
		var hash = 
			System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(doorID + i));
	
		if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xf0) == 0)
		{
			password += string.Format("{0:x}", (hash[2] & 0x0f));
			Console.WriteLine("Password now " + password);
		}
		
		if (i % 1000000 == 0)
			Console.WriteLine(i);
		
		if (password.Length == 8)
			break;
		
		++i;
	}
	
	Console.WriteLine(password);
	
	// 1a3099aa?
}
