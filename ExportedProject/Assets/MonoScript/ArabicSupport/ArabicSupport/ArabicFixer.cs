using System;

namespace ArabicSupport
{
	public class ArabicFixer
	{
		public static string Fix(string str)
		{
			return Fix(str, false, true);
		}

		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			ArabicFixerTool.showTashkeel = showTashkeel;
			ArabicFixerTool.useHinduNumbers = useHinduNumbers;
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", Environment.get_NewLine());
			}
			if (str.Contains(Environment.get_NewLine()))
			{
				string[] array = new string[1] { Environment.get_NewLine() };
				string[] array2 = str.Split(array, (StringSplitOptions)0);
				if (array2.Length == 0)
				{
					return ArabicFixerTool.FixLine(str);
				}
				if (array2.Length == 1)
				{
					return ArabicFixerTool.FixLine(str);
				}
				string text = ArabicFixerTool.FixLine(array2[0]);
				int i = 1;
				if (array2.Length > 1)
				{
					for (; i < array2.Length; i++)
					{
						text = text + Environment.get_NewLine() + ArabicFixerTool.FixLine(array2[i]);
					}
				}
				return text;
			}
			return ArabicFixerTool.FixLine(str);
		}
	}
}
