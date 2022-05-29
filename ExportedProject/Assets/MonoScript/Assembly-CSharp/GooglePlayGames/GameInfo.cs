using System;

namespace GooglePlayGames
{
	public static class GameInfo
	{
		private const string UnescapedApplicationId = "APP_ID";

		private const string UnescapedIosClientId = "IOS_CLIENTID";

		private const string UnescapedWebClientId = "WEB_CLIENTID";

		private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";

		private const string UnescapedRequireGooglePlus = "REQUIRE_GOOGLE_PLUS";

		public const string ApplicationId = "339873998127";

		public const string IosClientId = "";

		public const string WebClientId = "";

		public const string NearbyConnectionServiceId = "com.pixel.gun3d";

		public static bool ApplicationIdInitialized()
		{
			return (string.IsNullOrEmpty("339873998127") ? false : !"339873998127".Equals(GameInfo.ToEscapedToken("APP_ID")));
		}

		public static bool IosClientIdInitialized()
		{
			return (string.IsNullOrEmpty(string.Empty) ? false : !string.Empty.Equals(GameInfo.ToEscapedToken("IOS_CLIENTID")));
		}

		public static bool NearbyConnectionsInitialized()
		{
			return (string.IsNullOrEmpty("com.pixel.gun3d") ? false : !"com.pixel.gun3d".Equals(GameInfo.ToEscapedToken("NEARBY_SERVICE_ID")));
		}

		public static bool RequireGooglePlus()
		{
			return false;
		}

		private static string ToEscapedToken(string token)
		{
			return string.Format("__{0}__", token);
		}

		public static bool WebClientIdInitialized()
		{
			return (string.IsNullOrEmpty(string.Empty) ? false : !string.Empty.Equals(GameInfo.ToEscapedToken("WEB_CLIENTID")));
		}
	}
}