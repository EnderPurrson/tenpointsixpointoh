using Facebook.MiniJSON;
using Facebook.Unity;
using System;
using System.Collections.Generic;

namespace Facebook.Unity.Mobile
{
	internal abstract class MobileFacebook : FacebookBase, IFacebook, IFacebookCallbackHandler, IMobileFacebook, IMobileFacebookCallbackHandler, IMobileFacebookImplementation
	{
		private const string CallbackIdKey = "callback_id";

		private Facebook.Unity.ShareDialogMode shareDialogMode;

		public Facebook.Unity.ShareDialogMode ShareDialogMode
		{
			get
			{
				return this.shareDialogMode;
			}
			set
			{
				this.shareDialogMode = value;
				this.SetShareDialogMode(this.shareDialogMode);
			}
		}

		protected MobileFacebook(Facebook.Unity.CallbackManager callbackManager) : base(callbackManager)
		{
		}

		public abstract void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback);

		private static IDictionary<string, object> DeserializeMessage(string message)
		{
			return (Dictionary<string, object>)Json.Deserialize(message);
		}

		public abstract void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback);

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

		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			AccessTokenRefreshResult accessTokenRefreshResult = new AccessTokenRefreshResult(message);
			if (accessTokenRefreshResult.AccessToken != null)
			{
				AccessToken.CurrentAccessToken = accessTokenRefreshResult.AccessToken;
			}
			base.CallbackManager.OnFacebookResponse(accessTokenRefreshResult);
		}

		public override void OnShareLinkComplete(string message)
		{
			ShareResult shareResult = new ShareResult(message);
			base.CallbackManager.OnFacebookResponse(shareResult);
		}

		public abstract void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback);

		private static string SerializeDictionary(IDictionary<string, object> dict)
		{
			return Json.Serialize(dict);
		}

		protected abstract void SetShareDialogMode(Facebook.Unity.ShareDialogMode mode);

		private static bool TryGetCallbackId(IDictionary<string, object> result, out string callbackId)
		{
			object obj;
			callbackId = null;
			if (!result.TryGetValue("callback_id", out obj))
			{
				return false;
			}
			callbackId = obj as string;
			return true;
		}

		private static bool TryGetError(IDictionary<string, object> result, out string errorMessage)
		{
			object obj;
			errorMessage = null;
			if (!result.TryGetValue("error", out obj))
			{
				return false;
			}
			errorMessage = obj as string;
			return true;
		}
	}
}