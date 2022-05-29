using System;

public sealed class ResPath
{
	public ResPath()
	{
	}

	public static string Combine(string a, string b)
	{
		if (a == null || b == null)
		{
			return string.Empty;
		}
		return string.Concat(a, "/", b);
	}
}