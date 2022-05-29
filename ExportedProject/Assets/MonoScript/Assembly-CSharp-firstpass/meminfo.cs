using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class meminfo
{
	public static meminfo.meminf minf;

	private static Regex re;

	static meminfo()
	{
		meminfo.minf = new meminfo.meminf();
		meminfo.re = new Regex("\\d+");
	}

	public meminfo()
	{
	}

	public static void gc_Collect()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.lang.System");
		androidJavaClass.CallStatic("gc", new object[0]);
		androidJavaClass.Dispose();
	}

	public static bool getMemInfo()
	{
		if (!File.Exists("/proc/meminfo"))
		{
			return false;
		}
		FileStream fileStream = new FileStream("/proc/meminfo", FileMode.Open, FileAccess.Read, FileShare.Read);
		StreamReader streamReader = new StreamReader(fileStream);
		while (true)
		{
			string str = streamReader.ReadLine();
			string str1 = str;
			if (str == null)
			{
				break;
			}
			str1 = str1.ToLower().Replace(" ", string.Empty);
			if (str1.Contains("memtotal"))
			{
				meminfo.minf.memtotal = meminfo.mVal(str1);
			}
			if (str1.Contains("memfree"))
			{
				meminfo.minf.memfree = meminfo.mVal(str1);
			}
			if (str1.Contains("active"))
			{
				meminfo.minf.active = meminfo.mVal(str1);
			}
			if (str1.Contains("inactive"))
			{
				meminfo.minf.inactive = meminfo.mVal(str1);
			}
			if (str1.Contains("cached") && !str1.Contains("swapcached"))
			{
				meminfo.minf.cached = meminfo.mVal(str1);
			}
			if (str1.Contains("swapcached"))
			{
				meminfo.minf.swapcached = meminfo.mVal(str1);
			}
			if (str1.Contains("swaptotal"))
			{
				meminfo.minf.swaptotal = meminfo.mVal(str1);
			}
			if (str1.Contains("swapfree"))
			{
				meminfo.minf.swapfree = meminfo.mVal(str1);
			}
		}
		streamReader.Close();
		fileStream.Close();
		fileStream.Dispose();
		return true;
	}

	private static int mVal(string s)
	{
		Match match = meminfo.re.Match(s);
		return int.Parse(match.Value);
	}

	public struct meminf
	{
		public int memtotal;

		public int memfree;

		public int active;

		public int inactive;

		public int cached;

		public int swapcached;

		public int swaptotal;

		public int swapfree;
	}
}