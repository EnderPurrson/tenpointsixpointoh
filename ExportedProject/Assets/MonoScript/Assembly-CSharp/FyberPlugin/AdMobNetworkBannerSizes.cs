using System;

namespace FyberPlugin
{
	public class AdMobNetworkBannerSizes
	{
		private const string NETWORK_NAME = "AdMob";

		public readonly static NetworkBannerSize BANNER;

		public readonly static NetworkBannerSize LARGE_BANNER;

		public readonly static NetworkBannerSize MEDIUM_RECTANGLE;

		public readonly static NetworkBannerSize FULL_BANNER;

		public readonly static NetworkBannerSize LEADERBOARD;

		public readonly static NetworkBannerSize SMART_BANNER;

		static AdMobNetworkBannerSizes()
		{
			AdMobNetworkBannerSizes.BANNER = new NetworkBannerSize("AdMob", new BannerSize(320, 50));
			AdMobNetworkBannerSizes.LARGE_BANNER = new NetworkBannerSize("AdMob", new BannerSize(320, 100));
			AdMobNetworkBannerSizes.MEDIUM_RECTANGLE = new NetworkBannerSize("AdMob", new BannerSize(300, 250));
			AdMobNetworkBannerSizes.FULL_BANNER = new NetworkBannerSize("AdMob", new BannerSize(468, 60));
			AdMobNetworkBannerSizes.LEADERBOARD = new NetworkBannerSize("AdMob", new BannerSize(728, 90));
			AdMobNetworkBannerSizes.SMART_BANNER = new NetworkBannerSize("AdMob", new BannerSize(-1, -2));
		}

		public AdMobNetworkBannerSizes()
		{
		}
	}
}