using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace FyberPlugin.LitJson
{
	public class JsonReader
	{
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		private Stack<int> automaton_stack;

		private int current_input;

		private int current_symbol;

		private bool end_of_json;

		private bool end_of_input;

		private Lexer lexer;

		private bool parser_in_string;

		private bool parser_return;

		private bool read_started;

		private TextReader reader;

		private bool reader_is_owned;

		private bool skip_non_members;

		private object token_value;

		private JsonToken token;

		public bool AllowComments
		{
			get
			{
				return lexer.AllowComments;
			}
			set
			{
				lexer.AllowComments = value;
			}
		}

		public bool AllowSingleQuotedStrings
		{
			get
			{
				return lexer.AllowSingleQuotedStrings;
			}
			set
			{
				lexer.AllowSingleQuotedStrings = value;
			}
		}

		public bool SkipNonMembers
		{
			get
			{
				return skip_non_members;
			}
			set
			{
				skip_non_members = value;
			}
		}

		public bool EndOfInput
		{
			get
			{
				return end_of_input;
			}
		}

		public bool EndOfJson
		{
			get
			{
				return end_of_json;
			}
		}

		public JsonToken Token
		{
			get
			{
				return token;
			}
		}

		public object Value
		{
			get
			{
				return token_value;
			}
		}

		public JsonReader(string json_text)
			: this((TextReader)new StringReader(json_text), true)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown


		public JsonReader(TextReader reader)
			: this(reader, false)
		{
		}

		private JsonReader(TextReader reader, bool owned)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			parser_in_string = false;
			parser_return = false;
			read_started = false;
			automaton_stack = new Stack<int>();
			automaton_stack.Push(65553);
			automaton_stack.Push(65543);
			lexer = new Lexer(reader);
			end_of_input = false;
			end_of_json = false;
			skip_non_members = true;
			this.reader = reader;
			reader_is_owned = owned;
		}

		static JsonReader()
		{
			PopulateParseTable();
		}

		private static void PopulateParseTable()
		{
			parse_table = (IDictionary<int, IDictionary<int, int[]>>)(object)new Dictionary<int, IDictionary<int, int[]>>();
			TableAddRow(ParserToken.Array);
			TableAddCol(ParserToken.Array, 91, 91, 65549);
			TableAddRow(ParserToken.ArrayPrime);
			int[] array = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 34, array);
			int[] array2 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array2, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 91, array2);
			TableAddCol(ParserToken.ArrayPrime, 93, 93);
			int[] array3 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array3, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 123, array3);
			int[] array4 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array4, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 65537, array4);
			int[] array5 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array5, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 65538, array5);
			int[] array6 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array6, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 65539, array6);
			int[] array7 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array7, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ArrayPrime, 65540, array7);
			TableAddRow(ParserToken.Object);
			TableAddCol(ParserToken.Object, 123, 123, 65545);
			TableAddRow(ParserToken.ObjectPrime);
			int[] array8 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array8, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ObjectPrime, 34, array8);
			TableAddCol(ParserToken.ObjectPrime, 125, 125);
			TableAddRow(ParserToken.Pair);
			int[] array9 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array9, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.Pair, 34, array9);
			TableAddRow(ParserToken.PairRest);
			int[] array10 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array10, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.PairRest, 44, array10);
			TableAddCol(ParserToken.PairRest, 125, 65554);
			TableAddRow(ParserToken.String);
			int[] array11 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array11, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.String, 34, array11);
			TableAddRow(ParserToken.Text);
			TableAddCol(ParserToken.Text, 91, 65548);
			TableAddCol(ParserToken.Text, 123, 65544);
			TableAddRow(ParserToken.Value);
			TableAddCol(ParserToken.Value, 34, 65552);
			TableAddCol(ParserToken.Value, 91, 65548);
			TableAddCol(ParserToken.Value, 123, 65544);
			TableAddCol(ParserToken.Value, 65537, 65537);
			TableAddCol(ParserToken.Value, 65538, 65538);
			TableAddCol(ParserToken.Value, 65539, 65539);
			TableAddCol(ParserToken.Value, 65540, 65540);
			TableAddRow(ParserToken.ValueRest);
			int[] array12 = new int[3];
			RuntimeHelpers.InitializeArray((global::System.Array)array12, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			TableAddCol(ParserToken.ValueRest, 44, array12);
			TableAddCol(ParserToken.ValueRest, 93, 65554);
		}

		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			parse_table.get_Item((int)row).Add(col, symbols);
		}

		private static void TableAddRow(ParserToken rule)
		{
			parse_table.Add((int)rule, (IDictionary<int, int[]>)(object)new Dictionary<int, int[]>());
		}

		private void ProcessNumber(string number)
		{
			double num = default(double);
			int num2 = default(int);
			long num3 = default(long);
			ulong num4 = default(ulong);
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, ref num))
			{
				token = JsonToken.Double;
				token_value = num;
			}
			else if (int.TryParse(number, ref num2))
			{
				token = JsonToken.Int;
				token_value = num2;
			}
			else if (long.TryParse(number, ref num3))
			{
				token = JsonToken.Long;
				token_value = num3;
			}
			else if (ulong.TryParse(number, ref num4))
			{
				token = JsonToken.Long;
				token_value = num4;
			}
			else
			{
				token = JsonToken.Int;
				token_value = 0;
			}
		}

		private void ProcessSymbol()
		{
			if (current_symbol == 91)
			{
				token = JsonToken.ArrayStart;
				parser_return = true;
			}
			else if (current_symbol == 93)
			{
				token = JsonToken.ArrayEnd;
				parser_return = true;
			}
			else if (current_symbol == 123)
			{
				token = JsonToken.ObjectStart;
				parser_return = true;
			}
			else if (current_symbol == 125)
			{
				token = JsonToken.ObjectEnd;
				parser_return = true;
			}
			else if (current_symbol == 34)
			{
				if (parser_in_string)
				{
					parser_in_string = false;
					parser_return = true;
					return;
				}
				if (token == JsonToken.None)
				{
					token = JsonToken.String;
				}
				parser_in_string = true;
			}
			else if (current_symbol == 65541)
			{
				token_value = lexer.StringValue;
			}
			else if (current_symbol == 65539)
			{
				token = JsonToken.Boolean;
				token_value = false;
				parser_return = true;
			}
			else if (current_symbol == 65540)
			{
				token = JsonToken.Null;
				parser_return = true;
			}
			else if (current_symbol == 65537)
			{
				ProcessNumber(lexer.StringValue);
				parser_return = true;
			}
			else if (current_symbol == 65546)
			{
				token = JsonToken.PropertyName;
			}
			else if (current_symbol == 65538)
			{
				token = JsonToken.Boolean;
				token_value = true;
				parser_return = true;
			}
		}

		private bool ReadToken()
		{
			if (end_of_input)
			{
				return false;
			}
			lexer.NextToken();
			if (lexer.EndOfInput)
			{
				Close();
				return false;
			}
			current_input = lexer.Token;
			return true;
		}

		public void Close()
		{
			if (!end_of_input)
			{
				end_of_input = true;
				end_of_json = true;
				if (reader_is_owned)
				{
					reader.Close();
				}
				reader = null;
			}
		}

		public bool Read()
		{
			//Discarded unreachable code: IL_0144
			//IL_0137: Expected O, but got Unknown
			if (end_of_input)
			{
				return false;
			}
			if (end_of_json)
			{
				end_of_json = false;
				automaton_stack.Clear();
				automaton_stack.Push(65553);
				automaton_stack.Push(65543);
			}
			parser_in_string = false;
			parser_return = false;
			token = JsonToken.None;
			token_value = null;
			if (!read_started)
			{
				read_started = true;
				if (!ReadToken())
				{
					return false;
				}
			}
			while (true)
			{
				if (parser_return)
				{
					if (automaton_stack.Peek() == 65553)
					{
						end_of_json = true;
					}
					return true;
				}
				current_symbol = automaton_stack.Pop();
				ProcessSymbol();
				if (current_symbol == current_input)
				{
					if (!ReadToken())
					{
						break;
					}
					continue;
				}
				int[] array;
				try
				{
					array = parse_table.get_Item(current_symbol).get_Item(current_input);
				}
				catch (KeyNotFoundException val)
				{
					KeyNotFoundException inner_exception = val;
					throw new JsonException((ParserToken)current_input, (global::System.Exception)(object)inner_exception);
				}
				if (array[0] != 65554)
				{
					for (int num = array.Length - 1; num >= 0; num--)
					{
						automaton_stack.Push(array[num]);
					}
				}
			}
			if (automaton_stack.Peek() != 65553)
			{
				throw new JsonException("Input doesn't evaluate to proper JSON text");
			}
			if (parser_return)
			{
				return true;
			}
			return false;
		}
	}
}
