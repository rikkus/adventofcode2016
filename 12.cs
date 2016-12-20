void Main()
{
	var testInput = @"cpy 41 a
inc a
inc a
dec a
jnz a 2
dec a"
	.Split(new[] { "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

	var realInput = @"cpy 1 a
cpy 1 b
cpy 26 d
jnz c 2
jnz 1 5
cpy 7 c
inc d
dec c
jnz c -2
cpy a c
inc a
dec b
jnz b -2
cpy c b
dec d
jnz d -6
cpy 13 c
cpy 14 d
inc a
dec d
jnz d -2
dec c
jnz c -5"
.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

	Computer.Create(realInput).Execute();
	
}

class RegisterBank : Dictionary<char, int>
{
	public RegisterBank()
	{
		this['a'] = 0;
		this['b'] = 0;
		this['c'] = 0;
		this['d'] = 0;
	}
}

abstract class Instruction
{
	public abstract void Execute(RegisterBank r, ref int ip);
}

class Copy : Instruction
{
	private static Regex Ex = new Regex(@"^cpy (?<source>([a-d]|-?\d+)) (?<target>[a-d])$", RegexOptions.ExplicitCapture);
	
	char sourceRegister;
	int sourceValue;
	char targetRegister;

	public string Source
	{
		get
		{
			return sourceRegister == '\0' ? sourceValue.ToString() : sourceRegister.ToString();
		}
	}
	
	public override string ToString()
	{
		return $"cpy {Source} {targetRegister}";
	}
	
	public static Copy Parse(string text)
	{
		var match = Ex.Match(text);

		if (!match.Success)
			return null;

		int sourceValue;

		if (int.TryParse(match.Groups["source"].Value, out sourceValue))
		{
			return new Copy { sourceRegister = '\0', sourceValue = sourceValue, targetRegister = match.Groups["target"].Value[0] };
		}
		
		return new Copy { sourceRegister = match.Groups["source"].Value[0], sourceValue = 0, targetRegister = match.Groups["target"].Value[0] };
	}

	public override void Execute(RegisterBank registers, ref int ip)
	{
		if (sourceRegister == '\0')
		{
			registers[targetRegister] = sourceValue;
		}
		else
		{
			registers[targetRegister] = registers[sourceRegister];
		}
		
		++ip;
	}
}

class Inc : Instruction
{
	private static Regex Ex = new Regex(@"^inc (?<register>[a-d])$", RegexOptions.ExplicitCapture);
	char register;
	
	public override string ToString()
	{
		return $"inc {register}";
	}
	
	public static Inc Parse(string text)
	{
		var match = Ex.Match(text);

		if (!match.Success)
			return null;

		return new Inc { register = match.Groups["register"].Value[0] };
	}
	
	public override void Execute(RegisterBank registers, ref int ip)
	{
		registers[register]++;
		++ip;
	}
}

class Dec : Instruction
{
	private static Regex Ex = new Regex(@"^dec (?<register>[a-d])$", RegexOptions.ExplicitCapture);
	char register;

	public override string ToString()
	{
		return $"dec {register}";
	}

	public static Dec Parse(string text)
	{
		var match = Ex.Match(text);

		if (!match.Success)
			return null;

		return new Dec { register = match.Groups["register"].Value[0] };
	}

	public override void Execute(RegisterBank registers, ref int ip)
	{
		registers[register]--;
		++ip;
	}
}

class Jnz : Instruction
{
	private static Regex Ex = new Regex(@"^jnz (?<source>([a-d]|-?\d+)) (?<offset>-?\d+)$", RegexOptions.ExplicitCapture);
	char sourceRegister;
	int sourceValue;
	int offset;

	public string Source
	{
		get
		{
			return sourceRegister == '\0' ? sourceValue.ToString() : sourceRegister.ToString();
		}
	}
	
	public override string ToString()
	{
		return $"jnz {Source} {offset}";
	}

	public static Jnz Parse(string text)
	{
		var match = Ex.Match(text);
		
		if (!match.Success)
			return null;
			
		int sourceValue;

		if (int.TryParse(match.Groups["source"].Value, out sourceValue))
		{
			return new Jnz { sourceRegister = '\0', sourceValue = sourceValue, offset = int.Parse(match.Groups["offset"].Value) };
		}

		return new Jnz { sourceRegister = match.Groups["source"].Value[0], sourceValue = 0, offset = int.Parse(match.Groups["offset"].Value) };
	}

	public override void Execute(RegisterBank registers, ref int ip)
	{
		if ((sourceRegister != '\0' && registers[sourceRegister] != 0) || (sourceValue != 0))
			ip += offset;
		else
		++ip;
	}
}

class Computer
{
	RegisterBank registers = new RegisterBank();
	
	List<Instruction> instructions;
	
	int ip;

	public static Computer Create(IEnumerable<string> instructions)
	{
		return new Computer() { instructions = instructions.Select(ParseInstruction).ToList(), ip = 0};
	}
	
	public void Execute()
	{
		for (int i = 0; i < instructions.Count(); i++)
		{
			Console.WriteLine($"{i}: " + instructions[i]);
		}
		
		registers['c'] = 1;
		
		while (ip >= 0 && ip < instructions.Count)
		{
			var i = instructions[ip];//.Execute(registers, ref ip);

			//Console.WriteLine(i.ToString());
			i.Execute(registers, ref ip);
			var a = registers['a'];
			var b = registers['b'];
			var c = registers['c'];
			var d = registers['d'];
			//Console.WriteLine($"a={a} b={b} c={c} d={d}");
			//Console.WriteLine($"ip: {ip}");
			//Console.WriteLine("--------");
		}
		
		Console.WriteLine(registers['a']);
	}
	
	private static Instruction ParseInstruction(string instruction)
	{
		Instruction i = null;
		
		i = Copy.Parse(instruction); if (i != null) return i;

		i = Inc.Parse(instruction); if (i != null) return i;

		i = Dec.Parse(instruction); if (i != null) return i;

		i = Jnz.Parse(instruction); if (i != null) return i;
		
		throw new Exception("Unparsed instruction: '" + instruction + "'");
	}
}

// Define other methods and classes here
