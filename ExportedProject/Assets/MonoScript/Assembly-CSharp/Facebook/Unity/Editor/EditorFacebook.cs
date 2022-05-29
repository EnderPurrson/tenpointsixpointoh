using Facebook.MiniJSON;
using Facebook.Unity;
using Facebook.Unity.Canvas;
using Facebook.Unity.Editor.Dialogs;
using Facebook.Unity.Mobile;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity.Editor
{
	internal class EditorFacebook : FacebookBase, ICanvasFacebook, ICanvasFacebookCallbackHandler, ICanvasFacebookImplementation, IFacebook, IFacebookCallbackHandler, IMobileFacebook, IMobileFacebookCallbackHandler, IMobileFacebookImplementation
	{
		private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.";

		private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";

		private IFacebookCallbackHandler EditorGameObject
		{
			get
			{
				return ComponentFactory.GetComponent<EditorFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
			}
		}

		public override bool LimitEventUsage
		{
			get;
			set;
		}

		public override string SDKName
		{
			get
			{
				return "FBUnityEditorSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return FacebookSdkVersion.Build;
			}
		}

		public Facebook.Unity.ShareDialogMode ShareDialogMode
		{
			get;
			set;
		}

		public EditorFacebook() : base(new Facebook.Unity.CallbackManager())
		{
		}

		public override void ActivateApp(string appId)
		{
			FacebookLogger.Info("This only needs to be called for iOS or Android.");
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			EditorFacebook editorFacebook = this;
			this.ShowEmptyMockDialog<IAppInviteResult>(new EditorFacebookMockDialog.OnComplete(editorFacebook.OnAppInviteComplete), callback, "Mock App Invite");
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			EditorFacebook editorFacebook = this;
			this.ShowEmptyMockDialog<IAppRequestResult>(new EditorFacebookMockDialog.OnComplete(editorFacebook.OnAppRequestsComplete), callback, "Mock App Request");
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			this.ShowMockShareDialog("FeedShare", callback);
		}

		public void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["url"] = "mockurl://testing.url";
			strs["ref"] = "mock ref";
			strs["extras"] = new Dictionary<string, object>()
			{
				{ "mock extra key", "mock extra value" }
			};
			strs["target_url"] = "mocktargeturl://mocktarget.url";
			strs["callback_id"] = base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback);
			this.OnFetchDeferredAppLinkComplete(Json.Serialize(strs));
		}

		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			EditorFacebook editorFacebook = this;
			this.ShowEmptyMockDialog<IGroupCreateResult>(new EditorFacebookMockDialog.OnComplete(editorFacebook.OnGroupCreateComplete), callback, "Mock Group Create");
		}

		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			EditorFacebook editorFacebook = this;
			this.ShowEmptyMockDialog<IGroupJoinResult>(new EditorFacebookMockDialog.OnComplete(editorFacebook.OnGroupJoinComplete), callback, "Mock Group Join");
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["url"] = "mockurl://testing.url";
			strs["callback_id"] = base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback);
			this.OnGetAppLinkComplete(Json.Serialize(strs));
		}

		public override void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			FacebookLogger.Warn("You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.");
			base.Init(hideUnityDelegate, onInitComplete);
			this.EditorGameObject.OnInitComplete(string.Empty);
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			MockLoginDialog component = ComponentFactory.GetComponent<MockLoginDialog>(ComponentFactory.IfNotExist.AddNew);
			IFacebookCallbackHandler editorGameObject = this.EditorGameObject;
			component.Callback = new EditorFacebookMockDialog.OnComplete(editorGameObject.OnLoginComplete);
			component.CallbackID = base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback);
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.LogInWithPublishPermissions(permissions, callback);
		}

		public void OnAppInviteComplete(string message)
		{
			AppInviteResult appInviteResult = new AppInviteResult(message);
			base.CallbackManager.OnFacebookResponse(appInviteResult);
		}

		public override void OnAppRequestsComplete(string message)
		{
			AppRequestResult appRequestResult = new AppRequestResult(message);
			base.CallbackManager.OnFacebookResponse(appRequestResult);
		}

		public void OnFacebookAuthResponseChange(string message)
		{
			throw new NotSupportedException();
		}

		public void OnFetchDeferredAppLinkComplete(string message)
		{
			AppLinkResult appLinkResult = new AppLinkResult(message);
			base.CallbackManager.OnFacebookResponse(appLinkResult);
		}

		public override void OnGetAppLinkComplete(string message)
		{
			AppLinkResult appLinkResult = new AppLinkResult(message);
			base.CallbackManager.OnFacebookResponse(appLinkResult);
		}

		public override void OnGroupCreateComplete(string message)
		{
			GroupCreateResult groupCreateResult = new GroupCreateResult(message);
			base.CallbackManager.OnFacebookResponse(groupCreateResult);
		}

		public override void OnGroupJoinComplete(string message)
		{
			GroupJoinResult groupJoinResult = new GroupJoinResult(message);
			base.CallbackManager.OnFacebookResponse(groupJoinResult);
		}

		public override void OnLoginComplete(string message)
		{
			base.OnAuthResponse(new LoginResult(message));
		}

		public void OnPayComplete(string message)
		{
			PayResult payResult = new PayResult(message);
			base.CallbackManager.OnFacebookResponse(payResult);
		}

		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			AccessTokenRefreshResult accessTokenRefreshResult = new AccessTokenRefreshResult(message);
			base.CallbackManager.OnFacebookResponse(accessTokenRefreshResult);
		}

		public override void OnShareLinkComplete(string message)
		{
			ShareResult shareResult = new ShareResult(message);
			base.CallbackManager.OnFacebookResponse(shareResult);
		}

		public void OnUrlResponse(string message)
		{
			throw new NotSupportedException();
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			EditorFacebook editorFacebook = this;
			this.ShowEmptyMockDialog<IPayResult>(new EditorFacebookMockDialog.OnComplete(editorFacebook.OnPayComplete), callback, "Mock Pay Dialog");
		}

		public void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			if (callback == null)
			{
				return;
			}
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "callback_id", base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback) }
			};
			Dictionary<string, object> strs1 = strs;
			if (AccessToken.CurrentAccessToken != null)
			{
				IDictionary<string, object> strs2 = (IDictionary<string, object>)Json.Deserialize(AccessToken.CurrentAccessToken.ToJson());
				strs1.AddAllKVPFrom<string, object>(strs2);
			}
			else
			{
				strs1["error"] = "No current access token";
			}
			this.OnRefreshCurrentAccessTokenComplete(strs1.ToJson());
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.ShowMockShareDialog("ShareLink", callback);
		}

		private void ShowEmptyMockDialog<T>(EditorFacebookMockDialog.OnComplete callback, FacebookDelegate<T> userCallback, string title)
		where T : IResult
		{
			EmptyMockDialog component = ComponentFactory.GetComponent<EmptyMockDialog>(ComponentFactory.IfNotExist.AddNew);
			component.Callback = callback;
			component.CallbackID = base.CallbackManager.AddFacebookDelegate<T>(userCallback);
			component.EmptyDialogTitle = title;
		}

		private void ShowMockShareDialog(string subTitle, FacebookDelegate<IShareResult> userCallback)
		{
			MockShareDialog component = ComponentFactory.GetComponent<MockShareDialog>(ComponentFactory.IfNotExist.AddNew);
			component.SubTitle = subTitle;
			IFacebookCallbackHandler editorGameObject = this.EditorGameObject;
			component.Callback = new EditorFacebookMockDialog.OnComplete(editorGameObject.OnShareLinkComplete);
			component.CallbackID = base.CallbackManager.AddFacebookDelegate<IShareResult>(userCallback);
		}
	}
}