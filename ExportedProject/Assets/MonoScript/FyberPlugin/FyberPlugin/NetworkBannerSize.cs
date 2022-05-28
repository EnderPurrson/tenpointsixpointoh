using System.Runtime.CompilerServices;

namespace FyberPlugin
{
	public class NetworkBannerSize
	{
		public string network
		{
			[CompilerGenerated]
			get
			{
				return _003Cnetwork_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003Cnetwork_003Ek__BackingField = value;
			}
		}

		public BannerSize size
		{
			[CompilerGenerated]
			get
			{
				return _003Csize_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003Csize_003Ek__BackingField = value;
			}
		}

		public NetworkBannerSize(string network, BannerSize size)
		{
			this.size = size;
			this.network = network;
		}

		public string GetNetwork()
		{
			return network;
		}

		public BannerSize GetSize()
		{
			return size;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", (object)network, (object)size);
		}
	}
}
