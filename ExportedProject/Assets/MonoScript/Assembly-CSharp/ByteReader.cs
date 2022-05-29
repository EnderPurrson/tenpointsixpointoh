using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ByteReader
{
	private byte[] mBuffer;

	private int mOffset;

	private static BetterList<string> mTemp;

	public bool canRead
	{
		get
		{
			return (this.mBuffer == null ? false : this.mOffset < (int)this.mBuffer.Length);
		}
	}

	static ByteReader()
	{
		ByteReader.mTemp = new BetterList<string>();
	}

	public ByteReader(byte[] bytes)
	{
		this.mBuffer = bytes;
	}

	public ByteReader(TextAsset asset)
	{
		this.mBuffer = asset.bytes;
	}

	public static ByteReader Open(string path)
	{
		FileStream fileStream = File.OpenRead(path);
		if (fileStream == null)
		{
			return null;
		}
		fileStream.Seek((long)0, SeekOrigin.End);
		byte[] numArray = new byte[checked((IntPtr)fileStream.Position)];
		fileStream.Seek((long)0, SeekOrigin.Begin);
		fileStream.Read(numArray, 0, (int)numArray.Length);
		fileStream.Close();
		return new ByteReader(numArray);
	}

	public BetterList<string> ReadCSV()
	{
		ByteReader.mTemp.Clear();
		string empty = string.Empty;
		bool flag = false;
		int num = 0;
		while (true)
		{
			if (!this.canRead)
			{
				return null;
			}
			if (!flag)
			{
				empty = this.ReadLine(true);
				if (empty == null)
				{
					return null;
				}
				empty = empty.Replace("\\n", "\n");
				num = 0;
			}
			else
			{
				string str = this.ReadLine(false);
				if (str == null)
				{
					return null;
				}
				str = str.Replace("\\n", "\n");
				empty = string.Concat(empty, "\n", str);
			}
			int num1 = num;
			int length = empty.Length;
			while (num1 < length)
			{
				char chr = empty[num1];
				if (chr == ',')
				{
					if (!flag)
					{
						ByteReader.mTemp.Add(empty.Substring(num, num1 - num));
						num = num1 + 1;
					}
				}
				else if (chr == '\"')
				{
					if (!flag)
					{
						num = num1 + 1;
						flag = true;
					}
					else
					{
						if (num1 + 1 >= length)
						{
							ByteReader.mTemp.Add(empty.Substring(num, num1 - num).Replace("\"\"", "\""));
							return ByteReader.mTemp;
						}
						if (empty[num1 + 1] == '\"')
						{
							num1++;
						}
						else
						{
							ByteReader.mTemp.Add(empty.Substring(num, num1 - num).Replace("\"\"", "\""));
							flag = false;
							if (empty[num1 + 1] == ',')
							{
								num1++;
								num = num1 + 1;
							}
						}
					}
				}
				num1++;
			}
			if (num >= empty.Length)
			{
				break;
			}
			if (!flag)
			{
				ByteReader.mTemp.Add(empty.Substring(num, empty.Length - num));
				break;
			}
		}
		return ByteReader.mTemp;
	}

	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> strs = new Dictionary<string, string>();
		char[] chrArray = new char[] { '=' };
		while (this.canRead)
		{
			string str = this.ReadLine();
			if (str == null)
			{
				break;
			}
			else if (!str.StartsWith("//"))
			{
				string[] strArrays = str.Split(chrArray, 2, StringSplitOptions.RemoveEmptyEntries);
				if ((int)strArrays.Length != 2)
				{
					continue;
				}
				string str1 = strArrays[0].Trim();
				string str2 = strArrays[1].Trim().Replace("\\n", "\n");
				strs[str1] = str2;
			}
		}
		return strs;
	}

	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}

	public string ReadLine()
	{
		return this.ReadLine(true);
	}

	public string ReadLine(bool skipEmptyLines)
	{
		int length = (int)this.mBuffer.Length;
		if (skipEmptyLines)
		{
			while (this.mOffset < length && this.mBuffer[this.mOffset] < 32)
			{
				this.mOffset++;
			}
		}
		int num = this.mOffset;
		if (num >= length)
		{
			this.mOffset = length;
			return null;
		}
		while (true)
		{
			if (num >= length)
			{
				num++;
				break;
			}
			else
			{
				int num1 = num;
				num = num1 + 1;
				int num2 = this.mBuffer[num1];
				if (num2 == 10 || num2 == 13)
				{
					break;
				}
			}
		}
		string str = ByteReader.ReadLine(this.mBuffer, this.mOffset, num - this.mOffset - 1);
		this.mOffset = num;
		return str;
	}
}