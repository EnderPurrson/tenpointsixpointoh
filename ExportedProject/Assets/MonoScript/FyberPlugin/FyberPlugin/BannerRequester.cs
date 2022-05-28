using System.Collections.Generic;

namespace FyberPlugin
{
	public sealed class BannerRequester : Requester<BannerRequester, RequestCallback>
	{
		private const string NETWORK_BANNER_SIZE = "networkBannerSizes";

		private BannerRequester()
		{
		}

		public static BannerRequester Create()
		{
			return new BannerRequester();
		}

		public BannerRequester WithNetworkSize(NetworkBannerSize networkBannerSize)
		{
			List<NetworkBannerSize> sizesParameter = GetSizesParameter();
			sizesParameter.Add(networkBannerSize);
			return this;
		}

		internal List<NetworkBannerSize> GetSizesParameter()
		{
			if (!requesterAttributes.ContainsKey("networkBannerSizes"))
			{
				requesterAttributes.set_Item("networkBannerSizes", (object)new List<NetworkBannerSize>());
			}
			return requesterAttributes.get_Item("networkBannerSizes") as List<NetworkBannerSize>;
		}

		protected override RequesterType GetRequester()
		{
			return RequesterType.Banners;
		}
	}
}
