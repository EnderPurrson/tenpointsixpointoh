using System.Reflection;
using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	[Obfuscation(Exclude = true)]
	internal class NativeMessage
	{
		public int Type
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

		public string Id
		{
			[CompilerGenerated]
			get
			{
				return _003CId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CId_003Ek__BackingField = value;
			}
		}

		public int Origin
		{
			[CompilerGenerated]
			get
			{
				return _003COrigin_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003COrigin_003Ek__BackingField = value;
			}
		}

		public RequestPayload RequestPayload
		{
			[CompilerGenerated]
			get
			{
				return _003CRequestPayload_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CRequestPayload_003Ek__BackingField = value;
			}
		}

		public AdPayload AdPayload
		{
			[CompilerGenerated]
			get
			{
				return _003CAdPayload_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CAdPayload_003Ek__BackingField = value;
			}
		}

		public BannerAdPayload BannerAdPayload
		{
			[CompilerGenerated]
			get
			{
				return _003CBannerAdPayload_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CBannerAdPayload_003Ek__BackingField = value;
			}
		}
	}
}
