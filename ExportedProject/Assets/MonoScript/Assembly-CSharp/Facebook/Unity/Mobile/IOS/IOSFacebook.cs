using Facebook.Unity;
using Facebook.Unity.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Facebook.Unity.Mobile.IOS
{
	internal class IOSFacebook : MobileFacebook
	{
		private const string CancelledResponse = "{\"cancelled\":true}";

		private bool limitEventUsage;

		private IIOSWrapper iosWrapper;

		public override bool LimitEventUsage
		{
			get
			{
				return this.limitEventUsage;
			}
			set
			{
				this.limitEventUsage = value;
				this.iosWrapper.FBAppEventsSetLimitEventUsage(value);
			}
		}

		public override string SDKName
		{
			get
			{
				return "FBiOSSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return this.iosWrapper.FBSdkVersion();
			}
		}

		public IOSFacebook() : this(new IOSWrapper(), new Facebook.Unity.CallbackManager())
		{
		}

		public IOSFacebook(IIOSWrapper iosWrapper, Facebook.Unity.CallbackManager callbackManager) : base(callbackManager)
		{
			this.iosWrapper = iosWrapper;
		}

		public override void ActivateApp(string appId)
		{
			this.iosWrapper.FBSettingsActivateApp(appId);
		}

		private int AddCallback<T>(FacebookDelegate<T> callback)
		where T : IResult
		{
			return Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<T>(callback));
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			IOSFacebook.NativeDict nativeDict = IOSFacebook.MarshallDict(parameters);
			if (!valueToSum.HasValue)
			{
				this.iosWrapper.LogAppEvent(logEvent, 0, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
			}
			else
			{
				this.iosWrapper.LogAppEvent(logEvent, (double)valueToSum.Value, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
			}
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			IOSFacebook.NativeDict nativeDict = IOSFacebook.MarshallDict(parameters);
			this.iosWrapper.LogPurchaseAppEvent((double)logPurchase, currency, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
		}

		public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			string empty = string.Empty;
			string absoluteUri = string.Empty;
			if (appLinkUrl != null && !string.IsNullOrEmpty(appLinkUrl.AbsoluteUri))
			{
				empty = appLinkUrl.AbsoluteUri;
			}
			if (previewImageUrl != null && !string.IsNullOrEmpty(previewImageUrl.AbsoluteUri))
			{
				absoluteUri = previewImageUrl.AbsoluteUri;
			}
			this.iosWrapper.AppInvite(this.AddCallback<IAppInviteResult>(callback), empty, absoluteUri);
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			string str;
			string str1;
			string[] array;
			int num;
			string str2;
			string[] strArrays;
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			string str3 = null;
			if (filters != null && filters.Any<object>())
			{
				str3 = filters.First<object>() as string;
			}
			IIOSWrapper iOSWrapper = this.iosWrapper;
			int num1 = this.AddCallback<IAppRequestResult>(callback);
			string str4 = message;
			str = (!actionType.HasValue ? string.Empty : actionType.ToString());
			str1 = (objectId == null ? string.Empty : objectId);
			if (to == null)
			{
				array = null;
			}
			else
			{
				array = to.ToArray<string>();
			}
			num = (to == null ? 0 : to.Count<string>());
			str2 = (str3 == null ? string.Empty : str3);
			if (excludeIds == null)
			{
				strArrays = null;
			}
			else
			{
				strArrays = excludeIds.ToArray<string>();
			}
			iOSWrapper.AppRequest(num1, str4, str, str1, array, num, str2, strArrays, (excludeIds == null ? 0 : excludeIds.Count<string>()), maxRecipients.HasValue, (!maxRecipients.HasValue ? 0 : maxRecipients.Value), data, title);
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			string str = (link == null ? string.Empty : link.ToString());
			string str1 = (picture == null ? string.Empty : picture.ToString());
			this.iosWrapper.FeedShare(this.AddCallback<IShareResult>(callback), toId, str, linkName, linkCaption, linkDescription, str1, mediaSource);
		}

		public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			this.iosWrapper.FetchDeferredAppLink(this.AddCallback<IAppLinkResult>(callback));
		}

		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			this.iosWrapper.CreateGameGroup(this.AddCallback<IGroupCreateResult>(callback), name, description, privacy);
		}

		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			this.iosWrapper.JoinGameGroup(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IGroupJoinResult>(callback)), id);
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			this.iosWrapper.GetAppLink(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback)));
		}

		public void Init(string appId, bool frictionlessRequests, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			base.Init(hideUnityDelegate, onInitComplete);
			this.iosWrapper.Init(appId, frictionlessRequests, FacebookSettings.IosURLSuffix, Constants.UnitySDKUserAgentSuffixLegacy);
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.iosWrapper.LogInWithPublishPermissions(this.AddCallback<ILoginResult>(callback), permissions.ToCommaSeparateList());
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.iosWrapper.LogInWithReadPermissions(this.AddCallback<ILoginResult>(callback), permissions.ToCommaSeparateList());
		}

		public override void LogOut()
		{
			base.LogOut();
			this.iosWrapper.LogOut();
		}

		private static IOSFacebook.NativeDict MarshallDict(Dictionary<string, object> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (KeyValuePair<string, object> keyValuePair in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = keyValuePair.Key;
					nativeDict.Values[nativeDict.NumEntries] = keyValuePair.Value.ToString();
					IOSFacebook.NativeDict numEntries = nativeDict;
					numEntries.NumEntries = numEntries.NumEntries + 1;
				}
			}
			return nativeDict;
		}

		private static IOSFacebook.NativeDict MarshallDict(Dictionary<string, string> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (KeyValuePair<string, string> keyValuePair in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = keyValuePair.Key;
					nativeDict.Values[nativeDict.NumEntries] = keyValuePair.Value;
					IOSFacebook.NativeDict numEntries = nativeDict;
					numEntries.NumEntries = numEntries.NumEntries + 1;
				}
			}
			return nativeDict;
		}

		public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			this.iosWrapper.RefreshCurrentAccessToken(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback)));
		}

		protected override void SetShareDialogMode(Facebook.Unity.ShareDialogMode mode)
		{
			this.iosWrapper.SetShareDialogMode((int)mode);
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.iosWrapper.ShareLink(this.AddCallback<IShareResult>(callback), contentURL.AbsoluteUrlOrEmptyString(), contentTitle, contentDescription, photoURL.AbsoluteUrlOrEmptyString());
		}

		public enum FBInsightsFlushBehavior
		{
			FBInsightsFlushBehaviorAuto,
			FBInsightsFlushBehaviorExplicitOnly
		}

		private class NativeDict
		{
			public string[] Keys
			{
				get;
				set;
			}

			public int NumEntries
			{
				get;
				set;
			}

			public string[] Values
			{
				get;
				set;
			}

			public NativeDict()
			{
				this.NumEntries = 0;
				this.Keys = null;
				this.Values = null;
			}
		}
	}
}