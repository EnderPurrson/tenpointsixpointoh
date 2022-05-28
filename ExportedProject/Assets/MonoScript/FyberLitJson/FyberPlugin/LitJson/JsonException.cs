using System;

namespace FyberPlugin.LitJson
{
	public class JsonException : ApplicationException
	{
		public JsonException()
		{
		}

		internal JsonException(ParserToken token)
			: base(string.Format("Invalid token '{0}' in input string", (object)token))
		{
		}

		internal JsonException(ParserToken token, global::System.Exception inner_exception)
			: base(string.Format("Invalid token '{0}' in input string", (object)token), inner_exception)
		{
		}

		internal JsonException(int c)
			: base(string.Format("Invalid character '{0}' in input string", (object)(char)c))
		{
		}

		internal JsonException(int c, global::System.Exception inner_exception)
			: base(string.Format("Invalid character '{0}' in input string", (object)(char)c), inner_exception)
		{
		}

		public JsonException(string message)
			: base(message)
		{
		}

		public JsonException(string message, global::System.Exception inner_exception)
			: base(message, inner_exception)
		{
		}
	}
}
