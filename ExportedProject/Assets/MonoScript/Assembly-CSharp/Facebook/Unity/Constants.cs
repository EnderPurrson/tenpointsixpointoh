using System;
using System.Globalization;
using UnityEngine;

namespace Facebook.Unity
{
	internal static class Constants
	{
		public const string CallbackIdKey = "callback_id";

		public const string AccessTokenKey = "access_token";

		public const string UrlKey = "url";

		public const string RefKey = "ref";

		public const string ExtrasKey = "extras";

		public const string TargetUrlKey = "target_url";

		public const string CancelledKey = "cancelled";

		public const string ErrorKey = "error";

		public const string OnPayCompleteMethodName = "OnPayComplete";

		public const string OnShareCompleteMethodName = "OnShareLinkComplete";

		public const string OnAppRequestsCompleteMethodName = "OnAppRequestsComplete";

		public const string OnGroupCreateCompleteMethodName = "OnGroupCreateComplete";

		public const string OnGroupJoinCompleteMethodName = "OnJoinGroupComplete";

		public const string GraphApiVersion = "v2.5";

		public const string GraphUrlFormat = "https://graph.{0}/{1}/";

		public const string UserLikesPermission = "user_likes";

		public const string EmailPermission = "email";

		public const string PublishActionsPermission = "publish_actions";

		public const string PublishPagesPermission = "publish_pages";

		private static FacebookUnityPlatform? currentPlatform;

		public static FacebookUnityPlatform CurrentPlatform
		{
			get
			{
				if (!Constants.currentPlatform.HasValue)
				{
					Constants.currentPlatform = new FacebookUnityPlatform?(Constants.GetCurrentPlatform());
				}
				return Constants.currentPlatform.Value;
			}
			set
			{
				Constants.currentPlatform = new FacebookUnityPlatform?(value);
			}
		}

		public static bool DebugMode
		{
			get
			{
				return Debug.isDebugBuild;
			}
		}

		public static string GraphApiUserAgent
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[] { FB.FacebookImpl.SDKUserAgent, Constants.UnitySDKUserAgent });
			}
		}

		public static Uri GraphUrl
		{
			get
			{
				return new Uri(string.Format(CultureInfo.InvariantCulture, "https://graph.{0}/{1}/", new object[] { FB.FacebookDomain, FB.GraphApiVersion }));
			}
		}

		public static bool IsEditor
		{
			get
			{
				return false;
			}
		}

		public static bool IsMobile
		{
			get
			{
				return (Constants.CurrentPlatform == FacebookUnityPlatform.Android ? true : Constants.CurrentPlatform == FacebookUnityPlatform.IOS);
			}
		}

		public static bool IsWeb
		{
			get
			{
				return (Constants.CurrentPlatform == FacebookUnityPlatform.WebGL ? true : Constants.CurrentPlatform == FacebookUnityPlatform.WebPlayer);
			}
		}

		public static string UnitySDKUserAgent
		{
			get
			{
				return Utilities.GetUserAgent("FBUnitySDK", FacebookSdkVersion.Build);
			}
		}

		public static string UnitySDKUserAgentSuffixLegacy
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "Unity.{0}", new object[] { FacebookSdkVersion.Build });
			}
		}

		private static FacebookUnityPlatform GetCurrentPlatform()
		{
			RuntimePlatform runtimePlatform = Application.platform;
			switch (runtimePlatform)
			{
				case RuntimePlatform.OSXWebPlayer:
				case RuntimePlatform.WindowsWebPlayer:
				{
					return FacebookUnityPlatform.WebPlayer;
				}
				case RuntimePlatform.IPhonePlayer:
				{
					return FacebookUnityPlatform.IOS;
				}
				default:
				{
					if (runtimePlatform == RuntimePlatform.Android)
					{
						break;
					}
					else
					{
						if (runtimePlatform == RuntimePlatform.WebGLPlayer)
						{
							return FacebookUnityPlatform.WebGL;
						}
						return FacebookUnityPlatform.Unknown;
					}
				}
			}
			return FacebookUnityPlatform.Android;
		}
	}
}