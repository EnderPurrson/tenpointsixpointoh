using System;

public class RandomGenerator
{
	private const uint B = 1842502087;

	private const uint C = 1357980759;

	private const uint D = 273326509;

	private static uint counter;

	private uint a;

	private uint b;

	private uint c;

	private uint d;

	static RandomGenerator()
	{
	}

	public RandomGenerator(uint val)
	{
		this.SetSeed(val);
	}

	public RandomGenerator()
	{
		uint num = RandomGenerator.counter;
		RandomGenerator.counter = num + 1;
		this.SetSeed(num);
	}

	public double GenerateDouble()
	{
		return 2.3283064370807974E-10 * (double)((float)this.GenerateUint());
	}

	public float GenerateFloat()
	{
		return 2.3283064E-10f * (float)((float)this.GenerateUint());
	}

	public double GenerateRangeDouble()
	{
		return 4.656612874161595E-10 * (double)this.GenerateUint();
	}

	public float GenerateRangeFloat()
	{
		return 4.656613E-10f * (float)this.GenerateUint();
	}

	public uint GenerateUint()
	{
		uint num = this.a ^ this.a << 11;
		this.a = this.b;
		this.b = this.c;
		this.c = this.d;
		uint num1 = this.d ^ this.d >> 19 ^ num ^ num >> 8;
		uint num2 = num1;
		this.d = num1;
		return num2;
	}

	public int Range(int max)
	{
		return (int)((ulong)this.GenerateUint() % (long)max);
	}

	public int Range(int min, int max)
	{
		return min + (int)((ulong)this.GenerateUint() % (long)(max - min));
	}

	public void SetSeed(uint val)
	{
		this.a = val;
		this.b = val ^ 1842502087;
		this.c = val >> 5 ^ 1357980759;
		this.d = val >> 7 ^ 273326509;
		for (uint i = 0; i < 4; i++)
		{
			this.a = this.GenerateUint();
		}
	}
}