using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SimpleJSON
{
	public class JSONNode
	{
		public virtual JSONArray AsArray
		{
			get
			{
				return this as JSONArray;
			}
		}

		public virtual bool AsBool
		{
			get
			{
				bool flag = false;
				if (bool.TryParse(this.Value, out flag))
				{
					return flag;
				}
				return !string.IsNullOrEmpty(this.Value);
			}
			set
			{
				this.Value = (!value ? "false" : "true");
			}
		}

		public virtual double AsDouble
		{
			get
			{
				double num = 0;
				if (double.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		public virtual float AsFloat
		{
			get
			{
				float single = 0f;
				if (float.TryParse(this.Value, out single))
				{
					return single;
				}
				return 0f;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		public virtual int AsInt
		{
			get
			{
				int num = 0;
				if (int.TryParse(this.Value, out num))
				{
					return num;
				}
				return 0;
			}
			set
			{
				this.Value = value.ToString();
			}
		}

		public virtual JSONClass AsObject
		{
			get
			{
				return this as JSONClass;
			}
		}

		public virtual IEnumerable<JSONNode> Childs
		{
			get
			{
				JSONNode.u003cu003ec__Iterator82 variable = null;
				return variable;
			}
		}

		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		public IEnumerable<JSONNode> DeepChilds
		{
			get
			{
				JSONNode.u003cu003ec__Iterator83 variable = null;
				return variable;
			}
		}

		public virtual JSONNode this[int aIndex]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual JSONNode this[string aKey]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual string Value
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public JSONNode()
		{
		}

		public virtual void Add(string aKey, JSONNode aItem)
		{
		}

		public virtual void Add(JSONNode aItem)
		{
			this.Add(string.Empty, aItem);
		}

		public static JSONNode Deserialize(BinaryReader aReader)
		{
			JSONBinaryTag jSONBinaryTag = (JSONBinaryTag)aReader.ReadByte();
			switch (jSONBinaryTag)
			{
				case JSONBinaryTag.Array:
				{
					int num = aReader.ReadInt32();
					JSONArray jSONArrays = new JSONArray();
					for (int i = 0; i < num; i++)
					{
						jSONArrays.Add(JSONNode.Deserialize(aReader));
					}
					return jSONArrays;
				}
				case JSONBinaryTag.Class:
				{
					int num1 = aReader.ReadInt32();
					JSONClass jSONClasses = new JSONClass();
					for (int j = 0; j < num1; j++)
					{
						jSONClasses.Add(aReader.ReadString(), JSONNode.Deserialize(aReader));
					}
					return jSONClasses;
				}
				case JSONBinaryTag.Value:
				{
					return new JSONData(aReader.ReadString());
				}
				case JSONBinaryTag.IntValue:
				{
					return new JSONData(aReader.ReadInt32());
				}
				case JSONBinaryTag.DoubleValue:
				{
					return new JSONData(aReader.ReadDouble());
				}
				case JSONBinaryTag.BoolValue:
				{
					return new JSONData(aReader.ReadBoolean());
				}
				case JSONBinaryTag.FloatValue:
				{
					return new JSONData(aReader.ReadSingle());
				}
			}
			throw new Exception(string.Concat("Error deserializing JSON. Unknown tag: ", jSONBinaryTag));
		}

		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj);
		}

		internal static string Escape(string aText)
		{
			string empty = string.Empty;
			string str = aText;
			for (int i = 0; i < str.Length; i++)
			{
				char chr = str[i];
				char chr1 = chr;
				switch (chr1)
				{
					case '\b':
					{
						empty = string.Concat(empty, "\\b");
						break;
					}
					case '\t':
					{
						empty = string.Concat(empty, "\\t");
						break;
					}
					case '\n':
					{
						empty = string.Concat(empty, "\\n");
						break;
					}
					case '\f':
					{
						empty = string.Concat(empty, "\\f");
						break;
					}
					case '\r':
					{
						empty = string.Concat(empty, "\\r");
						break;
					}
					default:
					{
						if (chr1 == '\"')
						{
							empty = string.Concat(empty, "\\\"");
							break;
						}
						else if (chr1 == '\\')
						{
							empty = string.Concat(empty, "\\\\");
							break;
						}
						else
						{
							empty = string.Concat(empty, chr);
							break;
						}
					}
				}
			}
			return empty;
		}

		public override int GetHashCode()
		{
			return this.GetHashCode();
		}

		public static JSONNode LoadFromBase64(string aBase64)
		{
			MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(aBase64))
			{
				Position = (long)0
			};
			return JSONNode.LoadFromStream(memoryStream);
		}

		public static JSONNode LoadFromCompressedBase64(string aBase64)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode LoadFromCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode LoadFromCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public static JSONNode LoadFromFile(string aFileName)
		{
			JSONNode jSONNode;
			using (FileStream fileStream = File.OpenRead(aFileName))
			{
				jSONNode = JSONNode.LoadFromStream(fileStream);
			}
			return jSONNode;
		}

		public static JSONNode LoadFromStream(Stream aData)
		{
			JSONNode jSONNode;
			using (BinaryReader binaryReader = new BinaryReader(aData))
			{
				jSONNode = JSONNode.Deserialize(binaryReader);
			}
			return jSONNode;
		}

		public static bool operator ==(JSONNode a, object b)
		{
			if (b == null && a is JSONLazyCreator)
			{
				return true;
			}
			return object.ReferenceEquals(a, b);
		}

		public static implicit operator JSONNode(string s)
		{
			return new JSONData(s);
		}

		public static implicit operator String(JSONNode d)
		{
			string value;
			if (d != null)
			{
				value = d.Value;
			}
			else
			{
				value = null;
			}
			return value;
		}

		public static bool operator !=(JSONNode a, object b)
		{
			return !(a == b);
		}

		public static JSONNode Parse(string aJSON)
		{
			Stack<JSONNode> jSONNodes = new Stack<JSONNode>();
			JSONNode jSONNode = null;
			int num = 0;
			string empty = string.Empty;
			string str = string.Empty;
			bool flag = false;
			while (num < aJSON.Length)
			{
				char chr = aJSON[num];
				switch (chr)
				{
					case '\t':
					{
					Label0:
						if (flag)
						{
							empty = string.Concat(empty, aJSON[num]);
						}
						break;
					}
					case '\n':
					case '\r':
					{
						break;
					}
					default:
					{
						switch (chr)
						{
							case ' ':
							{
								goto Label0;
							}
							case '\"':
							{
								flag ^= 1;
								break;
							}
							default:
							{
								switch (chr)
								{
									case '[':
									{
										if (!flag)
										{
											jSONNodes.Push(new JSONArray());
											if (jSONNode != null)
											{
												str = str.Trim();
												if (jSONNode is JSONArray)
												{
													jSONNode.Add(jSONNodes.Peek());
												}
												else if (str != string.Empty)
												{
													jSONNode.Add(str, jSONNodes.Peek());
												}
											}
											str = string.Empty;
											empty = string.Empty;
											jSONNode = jSONNodes.Peek();
										}
										else
										{
											empty = string.Concat(empty, aJSON[num]);
										}
										break;
									}
									case '\\':
									{
										num++;
										if (flag)
										{
											char chr1 = aJSON[num];
											char chr2 = chr1;
											switch (chr2)
											{
												case 'n':
												{
													empty = string.Concat(empty, '\n');
													break;
												}
												case 'r':
												{
													empty = string.Concat(empty, '\r');
													break;
												}
												case 't':
												{
													empty = string.Concat(empty, '\t');
													break;
												}
												case 'u':
												{
													string str1 = aJSON.Substring(num + 1, 4);
													empty = string.Concat(empty, (char)int.Parse(str1, NumberStyles.AllowHexSpecifier));
													num += 4;
													break;
												}
												default:
												{
													if (chr2 == 'b')
													{
														empty = string.Concat(empty, '\b');
														break;
													}
													else if (chr2 == 'f')
													{
														empty = string.Concat(empty, '\f');
														break;
													}
													else
													{
														empty = string.Concat(empty, chr1);
														break;
													}
												}
											}
										}
										break;
									}
									case ']':
									{
									Label1:
										if (!flag)
										{
											if (jSONNodes.Count == 0)
											{
												throw new Exception("JSON Parse: Too many closing brackets");
											}
											jSONNodes.Pop();
											if (empty != string.Empty)
											{
												str = str.Trim();
												if (jSONNode is JSONArray)
												{
													jSONNode.Add(empty);
												}
												else if (str != string.Empty)
												{
													jSONNode.Add(str, empty);
												}
											}
											str = string.Empty;
											empty = string.Empty;
											if (jSONNodes.Count > 0)
											{
												jSONNode = jSONNodes.Peek();
											}
										}
										else
										{
											empty = string.Concat(empty, aJSON[num]);
										}
										break;
									}
									default:
									{
										switch (chr)
										{
											case '{':
											{
												if (!flag)
												{
													jSONNodes.Push(new JSONClass());
													if (jSONNode != null)
													{
														str = str.Trim();
														if (jSONNode is JSONArray)
														{
															jSONNode.Add(jSONNodes.Peek());
														}
														else if (str != string.Empty)
														{
															jSONNode.Add(str, jSONNodes.Peek());
														}
													}
													str = string.Empty;
													empty = string.Empty;
													jSONNode = jSONNodes.Peek();
												}
												else
												{
													empty = string.Concat(empty, aJSON[num]);
												}
												break;
											}
											case '}':
											{
												goto Label1;
											}
											default:
											{
												if (chr == ',')
												{
													if (!flag)
													{
														if (empty != string.Empty)
														{
															if (jSONNode is JSONArray)
															{
																jSONNode.Add(empty);
															}
															else if (str != string.Empty)
															{
																jSONNode.Add(str, empty);
															}
														}
														str = string.Empty;
														empty = string.Empty;
													}
													else
													{
														empty = string.Concat(empty, aJSON[num]);
													}
												}
												else if (chr != ':')
												{
													empty = string.Concat(empty, aJSON[num]);
												}
												else if (!flag)
												{
													str = empty;
													empty = string.Empty;
												}
												else
												{
													empty = string.Concat(empty, aJSON[num]);
												}
												break;
											}
										}
										break;
									}
								}
								break;
							}
						}
						break;
					}
				}
				num++;
			}
			if (flag)
			{
				throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
			}
			return jSONNode;
		}

		public virtual JSONNode Remove(string aKey)
		{
			return null;
		}

		public virtual JSONNode Remove(int aIndex)
		{
			return null;
		}

		public virtual JSONNode Remove(JSONNode aNode)
		{
			return aNode;
		}

		public string SaveToBase64()
		{
			string base64String;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.SaveToStream(memoryStream);
				memoryStream.Position = (long)0;
				base64String = Convert.ToBase64String(memoryStream.ToArray());
			}
			return base64String;
		}

		public string SaveToCompressedBase64()
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void SaveToCompressedFile(string aFileName)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void SaveToCompressedStream(Stream aData)
		{
			throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
		}

		public void SaveToFile(string aFileName)
		{
			Directory.CreateDirectory((new FileInfo(aFileName)).Directory.FullName);
			using (FileStream fileStream = File.OpenWrite(aFileName))
			{
				this.SaveToStream(fileStream);
			}
		}

		public void SaveToStream(Stream aData)
		{
			this.Serialize(new BinaryWriter(aData));
		}

		public virtual void Serialize(BinaryWriter aWriter)
		{
		}

		public override string ToString()
		{
			return "JSONNode";
		}

		public virtual string ToString(string aPrefix)
		{
			return "JSONNode";
		}
	}
}