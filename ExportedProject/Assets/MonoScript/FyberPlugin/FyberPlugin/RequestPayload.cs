using System.Reflection;
using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	[Obfuscation(Exclude = true)]
	internal class RequestPayload
	{
		public int? RequestError
		{
			[CompilerGenerated]
			get
			{
				return _003CRequestError_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CRequestError_003Ek__BackingField = value;
			}
		}

		public string PlacementId
		{
			[CompilerGenerated]
			get
			{
				return _003CPlacementId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CPlacementId_003Ek__BackingField = value;
			}
		}

		public bool? AdAvailable
		{
			[CompilerGenerated]
			get
			{
				return _003CAdAvailable_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CAdAvailable_003Ek__BackingField = value;
			}
		}

		public VirtualCurrencyResponse CurrencyResponse
		{
			[CompilerGenerated]
			get
			{
				return _003CCurrencyResponse_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCurrencyResponse_003Ek__BackingField = value;
			}
		}

		public VirtualCurrencyErrorResponse CurrencyErrorResponse
		{
			[CompilerGenerated]
			get
			{
				return _003CCurrencyErrorResponse_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CCurrencyErrorResponse_003Ek__BackingField = value;
			}
		}
	}
}
