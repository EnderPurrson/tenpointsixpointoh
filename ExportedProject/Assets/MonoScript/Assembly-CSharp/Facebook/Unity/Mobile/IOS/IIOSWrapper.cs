using System;

namespace Facebook.Unity.Mobile.IOS
{
	internal interface IIOSWrapper
	{
		void AppInvite(int requestId, string appLinkUrl, string previewImageUrl);

		void AppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "");

		void CreateGameGroup(int requestId, string name, string description, string privacy);

		void FBAppEventsSetLimitEventUsage(bool limitEventUsage);

		string FBSdkVersion();

		void FBSettingsActivateApp(string appId);

		void FeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource);

		void FetchDeferredAppLink(int requestId);

		void GetAppLink(int requestId);

		void Init(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix);

		void JoinGameGroup(int requestId, string groupId);

		void LogAppEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals);

		void LogInWithPublishPermissions(int requestId, string scope);

		void LogInWithReadPermissions(int requestId, string scope);

		void LogOut();

		void LogPurchaseAppEvent(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals);

		void RefreshCurrentAccessToken(int requestId);

		void SetShareDialogMode(int mode);

		void ShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL);
	}
}