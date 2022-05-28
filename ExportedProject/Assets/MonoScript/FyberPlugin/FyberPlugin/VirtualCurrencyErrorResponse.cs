using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	public class VirtualCurrencyErrorResponse
	{
		public enum ErrorType
		{
			ERROR_INVALID_RESPONSE = 0,
			ERROR_INVALID_RESPONSE_SIGNATURE = 1,
			SERVER_RETURNED_ERROR = 2,
			ERROR_OTHER = 3
		}

		public ErrorType Type
		{
			[CompilerGenerated]
			get
			{
				return _003CType_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CType_003Ek__BackingField = value;
			}
		}

		public string Code
		{
			[CompilerGenerated]
			get
			{
				return _003CCode_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCode_003Ek__BackingField = value;
			}
		}

		public string Message
		{
			[CompilerGenerated]
			get
			{
				return _003CMessage_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CMessage_003Ek__BackingField = value;
			}
		}
	}
}
