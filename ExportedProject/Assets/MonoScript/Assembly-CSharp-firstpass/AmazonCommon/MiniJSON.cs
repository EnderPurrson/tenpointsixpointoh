using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AmazonCommon
{
	public class MiniJSON
	{
		private const int TOKEN_NONE = 0;

		private const int TOKEN_CURLY_OPEN = 1;

		private const int TOKEN_CURLY_CLOSE = 2;

		private const int TOKEN_SQUARED_OPEN = 3;

		private const int TOKEN_SQUARED_CLOSE = 4;

		private const int TOKEN_COLON = 5;

		private const int TOKEN_COMMA = 6;

		private const int TOKEN_STRING = 7;

		private const int TOKEN_NUMBER = 8;

		private const int TOKEN_TRUE = 9;

		private const int TOKEN_FALSE = 10;

		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		protected static int lastErrorIndex;

		protected static string lastDecode;

		static MiniJSON()
		{
			MiniJSON.lastErrorIndex = -1;
			MiniJSON.lastDecode = string.Empty;
		}

		public MiniJSON()
		{
		}

		protected static void eatWhitespace(char[] json, ref int index)
		{
			while (index < (int)json.Length)
			{
				if (" \t\n\r".IndexOf(json[index]) != -1)
				{
					index++;
				}
				else
				{
					break;
				}
			}
		}

		public static int getLastErrorIndex()
		{
			return MiniJSON.lastErrorIndex;
		}

		public static string getLastErrorSnippet()
		{
			if (MiniJSON.lastErrorIndex == -1)
			{
				return string.Empty;
			}
			int num = MiniJSON.lastErrorIndex - 5;
			int length = MiniJSON.lastErrorIndex + 15;
			if (num < 0)
			{
				num = 0;
			}
			if (length >= MiniJSON.lastDecode.Length)
			{
				length = MiniJSON.lastDecode.Length - 1;
			}
			return MiniJSON.lastDecode.Substring(num, length - num + 1);
		}

		protected static int getLastIndexOfNumber(char[] json, int index)
		{
			int num = index;
			while (num < (int)json.Length)
			{
				if ("0123456789+-.eE".IndexOf(json[num]) != -1)
				{
					num++;
				}
				else
				{
					break;
				}
			}
			return num - 1;
		}

		public static object jsonDecode(string json)
		{
			MiniJSON.lastDecode = json;
			if (json == null)
			{
				return null;
			}
			char[] charArray = json.ToCharArray();
			int num = 0;
			bool flag = true;
			object obj = MiniJSON.parseValue(charArray, ref num, ref flag);
			if (!flag)
			{
				MiniJSON.lastErrorIndex = num;
			}
			else
			{
				MiniJSON.lastErrorIndex = -1;
			}
			return obj;
		}

		public static string jsonEncode(object json)
		{
			string str;
			StringBuilder stringBuilder = new StringBuilder(2000);
			if (!MiniJSON.serializeValue(json, stringBuilder))
			{
				str = null;
			}
			else
			{
				str = stringBuilder.ToString();
			}
			return str;
		}

		public static bool lastDecodeSuccessful()
		{
			return MiniJSON.lastErrorIndex == -1;
		}

		protected static int lookAhead(char[] json, int index)
		{
			int num = index;
			return MiniJSON.nextToken(json, ref num);
		}

		protected static int nextToken(char[] json, ref int index)
		{
			int length;
			MiniJSON.eatWhitespace(json, ref index);
			if (index == (int)json.Length)
			{
				return 0;
			}
			char chr = json[index];
			index++;
			char chr1 = chr;
			switch (chr1)
			{
				case '\"':
				{
					return 7;
				}
				case ',':
				{
					return 6;
				}
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				{
					return 8;
				}
				case ':':
				{
					return 5;
				}
				default:
				{
					switch (chr1)
					{
						case '[':
						{
							return 3;
						}
						case ']':
						{
							return 4;
						}
						default:
						{
							switch (chr1)
							{
								case '{':
								{
									return 1;
								}
								case '|':
								{
									index--;
									length = (int)json.Length - index;
									if (length >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
									{
										index += 5;
										return 10;
									}
									if (length >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
									{
										index += 4;
										return 9;
									}
									if (length < 4 || json[index] != 'n' || json[index + 1] != 'u' || json[index + 2] != 'l' || json[index + 3] != 'l')
									{
										return 0;
									}
									index += 4;
									return 11;
								}
								case '}':
								{
									return 2;
								}
								default:
								{
									index--;
									length = (int)json.Length - index;
									if (length >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
									{
										index += 5;
										return 10;
									}
									if (length >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
									{
										index += 4;
										return 9;
									}
									if (length < 4 || json[index] != 'n' || json[index + 1] != 'u' || json[index + 2] != 'l' || json[index + 3] != 'l')
									{
										return 0;
									}
									index += 4;
									return 11;
								}
							}
							break;
						}
					}
					break;
				}
			}
		}

		protected static ArrayList parseArray(char[] json, ref int index)
		{
			ArrayList arrayLists = new ArrayList();
			MiniJSON.nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = MiniJSON.lookAhead(json, index);
				if (num == 0)
				{
					return null;
				}
				if (num == 6)
				{
					MiniJSON.nextToken(json, ref index);
				}
				else if (num != 4)
				{
					bool flag1 = true;
					object obj = MiniJSON.parseValue(json, ref index, ref flag1);
					if (!flag1)
					{
						return null;
					}
					arrayLists.Add(obj);
				}
				else
				{
					MiniJSON.nextToken(json, ref index);
					break;
				}
			}
			return arrayLists;
		}

		protected static double parseNumber(char[] json, ref int index)
		{
			MiniJSON.eatWhitespace(json, ref index);
			int lastIndexOfNumber = MiniJSON.getLastIndexOfNumber(json, index);
			int num = lastIndexOfNumber - index + 1;
			char[] chrArray = new char[num];
			Array.Copy(json, index, chrArray, 0, num);
			index = lastIndexOfNumber + 1;
			return double.Parse(new string(chrArray));
		}

		protected static Hashtable parseObject(char[] json, ref int index)
		{
			Hashtable hashtables = new Hashtable();
			MiniJSON.nextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				int num = MiniJSON.lookAhead(json, index);
				if (num == 0)
				{
					return null;
				}
				if (num != 6)
				{
					if (num == 2)
					{
						MiniJSON.nextToken(json, ref index);
						return hashtables;
					}
					string str = MiniJSON.parseString(json, ref index);
					if (str == null)
					{
						return null;
					}
					num = MiniJSON.nextToken(json, ref index);
					if (num != 5)
					{
						return null;
					}
					bool flag1 = true;
					object obj = MiniJSON.parseValue(json, ref index, ref flag1);
					if (!flag1)
					{
						return null;
					}
					hashtables[str] = obj;
				}
				else
				{
					MiniJSON.nextToken(json, ref index);
				}
			}
			return hashtables;
		}

		protected static string parseString(char[] json, ref int index)
		{
			string empty = string.Empty;
			MiniJSON.eatWhitespace(json, ref index);
			int num = index;
			int num1 = num;
			index = num + 1;
			char chr = json[num1];
			bool flag = false;
			while (!flag)
			{
				if (index != (int)json.Length)
				{
					int num2 = index;
					num1 = num2;
					index = num2 + 1;
					chr = json[num1];
					if (chr == '\"')
					{
						flag = true;
						break;
					}
					else if (chr != '\\')
					{
						empty = string.Concat(empty, chr);
					}
					else if (index != (int)json.Length)
					{
						int num3 = index;
						num1 = num3;
						index = num3 + 1;
						chr = json[num1];
						if (chr == '\"')
						{
							empty = string.Concat(empty, '\"');
						}
						else if (chr == '\\')
						{
							empty = string.Concat(empty, '\\');
						}
						else if (chr == '/')
						{
							empty = string.Concat(empty, '/');
						}
						else if (chr == 'b')
						{
							empty = string.Concat(empty, '\b');
						}
						else if (chr == 'f')
						{
							empty = string.Concat(empty, '\f');
						}
						else if (chr == 'n')
						{
							empty = string.Concat(empty, '\n');
						}
						else if (chr == 'r')
						{
							empty = string.Concat(empty, '\r');
						}
						else if (chr == 't')
						{
							empty = string.Concat(empty, '\t');
						}
						else if (chr == 'u')
						{
							if ((int)json.Length - index < 4)
							{
								break;
							}
							else
							{
								char[] chrArray = new char[4];
								Array.Copy(json, index, chrArray, 0, 4);
								uint num4 = uint.Parse(new string(chrArray), NumberStyles.HexNumber);
								empty = string.Concat(empty, char.ConvertFromUtf32((int)num4));
								index += 4;
							}
						}
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			if (!flag)
			{
				return null;
			}
			return empty;
		}

		protected static object parseValue(char[] json, ref int index, ref bool success)
		{
			switch (MiniJSON.lookAhead(json, index))
			{
				case 0:
				{
					success = false;
					return null;
				}
				case 1:
				{
					return MiniJSON.parseObject(json, ref index);
				}
				case 2:
				case 4:
				case 5:
				case 6:
				{
					success = false;
					return null;
				}
				case 3:
				{
					return MiniJSON.parseArray(json, ref index);
				}
				case 7:
				{
					return MiniJSON.parseString(json, ref index);
				}
				case 8:
				{
					return MiniJSON.parseNumber(json, ref index);
				}
				case 9:
				{
					MiniJSON.nextToken(json, ref index);
					return bool.Parse("TRUE");
				}
				case 10:
				{
					MiniJSON.nextToken(json, ref index);
					return bool.Parse("FALSE");
				}
				case 11:
				{
					MiniJSON.nextToken(json, ref index);
					return null;
				}
				default:
				{
					success = false;
					return null;
				}
			}
		}

		protected static bool serializeArray(ArrayList anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			for (int i = 0; i < anArray.Count; i++)
			{
				object item = anArray[i];
				if (!flag)
				{
					builder.Append(", ");
				}
				if (!MiniJSON.serializeValue(item, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("]");
			return true;
		}

		protected static bool serializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
		{
			builder.Append("{");
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				if (!flag)
				{
					builder.Append(", ");
				}
				MiniJSON.serializeString(keyValuePair.Key, builder);
				builder.Append(":");
				MiniJSON.serializeString(keyValuePair.Value, builder);
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		protected static bool serializeDictionary(Dictionary<string, double> dict, StringBuilder builder)
		{
			builder.Append("{");
			bool flag = true;
			foreach (KeyValuePair<string, double> keyValuePair in dict)
			{
				if (!flag)
				{
					builder.Append(", ");
				}
				MiniJSON.serializeString(keyValuePair.Key, builder);
				builder.Append(":");
				MiniJSON.serializeString(keyValuePair.Value.ToString(), builder);
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		protected static void serializeNumber(double number, StringBuilder builder)
		{
			builder.Append(Convert.ToString(number));
		}

		protected static bool serializeObject(Hashtable anObject, StringBuilder builder)
		{
			builder.Append("{");
			IDictionaryEnumerator enumerator = anObject.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext())
			{
				string str = enumerator.Key.ToString();
				object value = enumerator.Value;
				if (!flag)
				{
					builder.Append(", ");
				}
				MiniJSON.serializeString(str, builder);
				builder.Append(":");
				if (!MiniJSON.serializeValue(value, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		protected static bool serializeObjectOrArray(object objectOrArray, StringBuilder builder)
		{
			if (objectOrArray is Hashtable)
			{
				return MiniJSON.serializeObject((Hashtable)objectOrArray, builder);
			}
			if (!(objectOrArray is ArrayList))
			{
				return false;
			}
			return MiniJSON.serializeArray((ArrayList)objectOrArray, builder);
		}

		protected static void serializeString(string aString, StringBuilder builder)
		{
			builder.Append("\"");
			char[] charArray = aString.ToCharArray();
			for (int i = 0; i < (int)charArray.Length; i++)
			{
				char chr = charArray[i];
				if (chr == '\"')
				{
					builder.Append("\\\"");
				}
				else if (chr == '\\')
				{
					builder.Append("\\\\");
				}
				else if (chr == '\b')
				{
					builder.Append("\\b");
				}
				else if (chr == '\f')
				{
					builder.Append("\\f");
				}
				else if (chr == '\n')
				{
					builder.Append("\\n");
				}
				else if (chr == '\r')
				{
					builder.Append("\\r");
				}
				else if (chr != '\t')
				{
					int num = Convert.ToInt32(chr);
					if (num < 32 || num > 126)
					{
						builder.Append(string.Concat("\\u", Convert.ToString(num, 16).PadLeft(4, '0')));
					}
					else
					{
						builder.Append(chr);
					}
				}
				else
				{
					builder.Append("\\t");
				}
			}
			builder.Append("\"");
		}

		protected static bool serializeValue(object value, StringBuilder builder)
		{
			if (value == null)
			{
				builder.Append("null");
			}
			else if (value.GetType().IsArray)
			{
				MiniJSON.serializeArray(new ArrayList((ICollection)value), builder);
			}
			else if (value is string)
			{
				MiniJSON.serializeString((string)value, builder);
			}
			else if (value is char)
			{
				MiniJSON.serializeString(Convert.ToString((char)value), builder);
			}
			else if (value is decimal)
			{
				MiniJSON.serializeString(Convert.ToString((decimal)value), builder);
			}
			else if (value is Hashtable)
			{
				MiniJSON.serializeObject((Hashtable)value, builder);
			}
			else if (value is Dictionary<string, string>)
			{
				MiniJSON.serializeDictionary((Dictionary<string, string>)value, builder);
			}
			else if (value is Dictionary<string, double>)
			{
				MiniJSON.serializeDictionary((Dictionary<string, double>)value, builder);
			}
			else if (value is ArrayList)
			{
				MiniJSON.serializeArray((ArrayList)value, builder);
			}
			else if (value is bool && (bool)value)
			{
				builder.Append("true");
			}
			else if (!(value is bool) || (bool)value)
			{
				if (!value.GetType().IsPrimitive)
				{
					return false;
				}
				MiniJSON.serializeNumber(Convert.ToDouble(value), builder);
			}
			else
			{
				builder.Append("false");
			}
			return true;
		}
	}
}