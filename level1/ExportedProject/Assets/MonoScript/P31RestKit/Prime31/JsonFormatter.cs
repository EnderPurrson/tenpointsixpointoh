using System;
using System.Collections.Generic;
using System.Text;

namespace Prime31
{
	public class JsonFormatter
	{
		private enum JsonContextType
		{
			Object,
			Array
		}

		private const int defaultIndent = 0;

		private const string indent = "\t";

		private const string space = " ";

		private bool inDoubleString = false;

		private bool inSingleString = false;

		private bool inVariableAssignment = false;

		private char prevChar = '\0';

		private Stack<JsonContextType> context = new Stack<JsonContextType>();

		public static string prettyPrint(string input)
		{
			try
			{
				return new JsonFormatter().print(input);
			}
			catch (global::System.Exception)
			{
				return null;
			}
		}

		private static void buildIndents(int indents, StringBuilder output)
		{
			for (indents = indents; indents > 0; indents--)
			{
				output.Append("\t");
			}
		}

		private bool inString()
		{
			return inDoubleString || inSingleString;
		}

		public string print(string input)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			StringBuilder val = new StringBuilder(input.get_Length() * 2);
			for (int i = 0; i < input.get_Length(); i++)
			{
				char c = input.get_Chars(i);
				switch (c)
				{
				case '{':
					if (!inString())
					{
						if (inVariableAssignment || (context.get_Count() > 0 && context.Peek() != JsonContextType.Array))
						{
							val.Append(Environment.get_NewLine());
							buildIndents(context.get_Count(), val);
						}
						val.Append(c);
						context.Push(JsonContextType.Object);
						val.Append(Environment.get_NewLine());
						buildIndents(context.get_Count(), val);
					}
					else
					{
						val.Append(c);
					}
					break;
				case '}':
					if (!inString())
					{
						val.Append(Environment.get_NewLine());
						context.Pop();
						buildIndents(context.get_Count(), val);
						val.Append(c);
					}
					else
					{
						val.Append(c);
					}
					break;
				case '[':
					val.Append(c);
					if (!inString())
					{
						context.Push(JsonContextType.Array);
					}
					break;
				case ']':
					if (!inString())
					{
						val.Append(c);
						context.Pop();
					}
					else
					{
						val.Append(c);
					}
					break;
				case '=':
					val.Append(c);
					break;
				case ',':
					val.Append(c);
					if (!inString())
					{
						val.Append(" ");
					}
					if (!inString() && context.Peek() != JsonContextType.Array)
					{
						buildIndents(context.get_Count(), val);
						val.Append(Environment.get_NewLine());
						buildIndents(context.get_Count(), val);
						inVariableAssignment = false;
					}
					break;
				case '\'':
					if (!inDoubleString && prevChar != '\\')
					{
						inSingleString = !inSingleString;
					}
					val.Append(c);
					break;
				case ':':
					if (!inString())
					{
						inVariableAssignment = true;
						val.Append(c);
						val.Append(" ");
					}
					else
					{
						val.Append(c);
					}
					break;
				case '"':
					if (!inSingleString && prevChar != '\\')
					{
						inDoubleString = !inDoubleString;
					}
					val.Append(c);
					break;
				case ' ':
					if (inString())
					{
						val.Append(c);
					}
					break;
				default:
					val.Append(c);
					break;
				}
				prevChar = c;
			}
			return ((object)val).ToString();
		}
	}
}
