using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizationReader
	{
		public LocalizationReader()
		{
		}

		private static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string str = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			str = str.Replace("\"\"", "\"");
			if (str.Length > 1 && str[0] == '\"' && str[str.Length - 1] == '\"')
			{
				str = str.Substring(1, str.Length - 2);
			}
			list.Add(str);
		}

		public static string DecodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("<\\n>", "\r\n");
		}

		public static string EncodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("\r\n", "<\\n>").Replace("\r", "<\\n>").Replace("\n", "<\\n>");
		}

		private static string[] ParseCSVline(string Line, ref int iStart)
		{
			List<string> strs = new List<string>();
			int length = Line.Length;
			int num = iStart;
			bool flag = false;
			while (iStart < length)
			{
				char line = Line[iStart];
				if (flag)
				{
					if (line == '\"')
					{
						if (iStart + 1 >= length || Line[iStart + 1] != '\"')
						{
							flag = false;
						}
						else if (iStart + 2 >= length || Line[iStart + 2] != '\"')
						{
							iStart++;
						}
						else
						{
							flag = false;
							iStart += 2;
						}
					}
				}
				else if (line == '\n' || line == ',')
				{
					LocalizationReader.AddCSVtoken(ref strs, ref Line, iStart, ref num);
					if (line == '\n')
					{
						iStart++;
						break;
					}
				}
				else if (line == '\"')
				{
					flag = true;
				}
				iStart++;
			}
			if (iStart > num)
			{
				LocalizationReader.AddCSVtoken(ref strs, ref Line, iStart, ref num);
			}
			return strs.ToArray();
		}

		public static List<string[]> ReadCSV(string Text)
		{
			int num = 0;
			List<string[]> strArrays = new List<string[]>();
			while (num < Text.Length)
			{
				string[] strArrays1 = LocalizationReader.ParseCSVline(Text, ref num);
				if (strArrays1 != null)
				{
					strArrays.Add(strArrays1);
				}
				else
				{
					break;
				}
			}
			return strArrays;
		}

		public static string ReadCSVfile(string Path)
		{
			string empty = string.Empty;
			using (StreamReader streamReader = File.OpenText(Path))
			{
				empty = streamReader.ReadToEnd();
			}
			empty = empty.Replace("\r\n", "\n");
			return empty;
		}

		public static Dictionary<string, string> ReadTextAsset(TextAsset asset)
		{
			string str;
			string str1;
			string str2;
			string str3;
			string str4;
			string str5 = Encoding.UTF8.GetString(asset.bytes, 0, (int)asset.bytes.Length);
			StringReader stringReader = new StringReader(str5.Replace("\r\n", "\n"));
			Dictionary<string, string> strs = new Dictionary<string, string>();
			while (true)
			{
				string str6 = stringReader.ReadLine();
				string str7 = str6;
				if (str6 == null)
				{
					break;
				}
				if (LocalizationReader.TextAsset_ReadLine(str7, out str, out str1, out str2, out str4, out str3))
				{
					if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str1))
					{
						strs[str] = str1;
					}
				}
			}
			return strs;
		}

		public static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int num = line.LastIndexOf("//");
			if (num >= 0)
			{
				comment = line.Substring(num + 2).Trim();
				comment = LocalizationReader.DecodeString(comment);
				line = line.Substring(0, num);
			}
			int num1 = line.IndexOf("=");
			if (num1 < 0)
			{
				return false;
			}
			key = line.Substring(0, num1).Trim();
			value = line.Substring(num1 + 1).Trim();
			value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
			value = LocalizationReader.DecodeString(value);
			if (key.Length > 2 && key[0] == '[')
			{
				int num2 = key.IndexOf(']');
				if (num2 >= 0)
				{
					termType = key.Substring(1, num2 - 1);
					key = key.Substring(num2 + 1);
				}
			}
			LocalizationReader.ValidateFullTerm(ref key);
			return true;
		}

		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				return;
			}
			while (true)
			{
				int num1 = Term.LastIndexOf('/');
				int num2 = num1;
				if (num1 == num)
				{
					break;
				}
				Term = Term.Remove(num2, 1);
			}
		}
	}
}