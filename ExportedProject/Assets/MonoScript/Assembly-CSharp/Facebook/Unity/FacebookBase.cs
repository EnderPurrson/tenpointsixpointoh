using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook.Unity
{
	internal abstract class FacebookBase : IFacebook, IFacebookCallbackHandler, IFacebookImplementation
	{
		private InitDelegate onInitCompleteDelegate;

		private HideUnityDelegate onHideUnityDelegate;

		protected Facebook.Unity.CallbackManager CallbackManager
		{
			get;
			private set;
		}

		public bool Initialized
		{
			get
			{
				return JustDecompileGenerated_get_Initialized();
			}
			set
			{
				JustDecompileGenerated_set_Initialized(value);
			}
		}

		private bool JustDecompileGenerated_Initialized_k__BackingField;

		public bool JustDecompileGenerated_get_Initialized()
		{
			return this.JustDecompileGenerated_Initialized_k__BackingField;
		}

		private void JustDecompileGenerated_set_Initialized(bool value)
		{
			this.JustDecompileGenerated_Initialized_k__BackingField = value;
		}

		public abstract bool LimitEventUsage
		{
			get;
			set;
		}

		public bool LoggedIn
		{
			get
			{
				return AccessToken.CurrentAccessToken != null;
			}
		}

		public abstract string SDKName
		{
			get;
		}

		public virtual string SDKUserAgent
		{
			get
			{
				return Utilities.GetUserAgent(this.SDKName, this.SDKVersion);
			}
		}

		public abstract string SDKVersion
		{
			get;
		}

		protected FacebookBase(Facebook.Unity.CallbackManager callbackManager)
		{
			this.CallbackManager = callbackManager;
		}

		public abstract void ActivateApp(string appId = null);

		public void API(string query, HttpMethod method, IDictionary<string, string> formData, FacebookDelegate<IGraphResult> callback)
		{
			IDictionary<string, string> strs;
			if (formData == null)
			{
				strs = new Dictionary<string, string>();
			}
			else
			{
				strs = this.CopyByValue(formData);
			}
			IDictionary<string, string> strs1 = strs;
			if (!strs1.ContainsKey("access_token") && !query.Contains("access_token="))
			{
				strs1["access_token"] = (!FB.IsLoggedIn ? string.Empty : AccessToken.CurrentAccessToken.TokenString);
			}
			AsyncRequestString.Request(this.GetGraphUrl(query), method, strs1, callback);
		}

		public void API(string query, HttpMethod method, WWWForm formData, FacebookDelegate<IGraphResult> callback)
		{
			if (formData == null)
			{
				formData = new WWWForm();
			}
			formData.AddField("access_token", (AccessToken.CurrentAccessToken == null ? string.Empty : AccessToken.CurrentAccessToken.TokenString));
			AsyncRequestString.Request(this.GetGraphUrl(query), method, formData, callback);
		}

		public abstract void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters);

		public abstract void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters);

		public void AppRequest(string message, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			OGActionType? nullable = null;
			this.AppRequest(message, nullable, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public abstract void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback);

		private IDictionary<string, string> CopyByValue(IDictionary<string, string> data)
		{
			string str;
			Dictionary<string, string> strs = new Dictionary<string, string>(data.Count);
			IEnumerator<KeyValuePair<string, string>> enumerator = data.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.Current;
					Dictionary<string, string> strs1 = strs;
					string key = current.Key;
					if (current.Value == null)
					{
						str = null;
					}
					else
					{
						str = new string(current.Value.ToCharArray());
					}
					strs1[key] = str;
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return strs;
		}

		public abstract void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback);

		public abstract void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback);

		public abstract void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback);

		public abstract void GetAppLink(FacebookDelegate<IAppLinkResult> callback);

		private Uri GetGraphUrl(string query)
		{
			if (!string.IsNullOrEmpty(query) && query.StartsWith("/"))
			{
				query = query.Substring(1);
			}
			return new Uri(Constants.GraphUrl, query);
		}

		public virtual void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			this.onHideUnityDelegate = hideUnityDelegate;
			this.onInitCompleteDelegate = onInitComplete;
		}

		public abstract void LogInWithPublishPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback);

		public abstract void LogInWithReadPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback);

		public virtual void LogOut()
		{
			AccessToken.CurrentAccessToken = null;
		}

		public abstract void OnAppRequestsComplete(string message);

		protected void OnAuthResponse(LoginResult result)
		{
			if (result.AccessToken != null)
			{
				AccessToken.CurrentAccessToken = result.AccessToken;
			}
			this.CallbackManager.OnFacebookResponse(result);
		}

		public abstract void OnGetAppLinkComplete(string message);

		public abstract void OnGroupCreateComplete(string message);

		public abstract void OnGroupJoinComplete(string message);

		public virtual void OnHideUnity(bool isGameShown)
		{
			if (this.onHideUnityDelegate != null)
			{
				this.onHideUnityDelegate(isGameShown);
			}
		}

		public virtual void OnInitComplete(string message)
		{
			this.Initialized = true;
			this.OnLoginComplete(message);
			if (this.onInitCompleteDelegate != null)
			{
				this.onInitCompleteDelegate();
			}
		}

		public abstract void OnLoginComplete(string message);

		public void OnLogoutComplete(string message)
		{
			AccessToken.CurrentAccessToken = null;
		}

		public abstract void OnShareLinkComplete(string message);

		public abstract void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback);

		protected void ValidateAppRequestArgs(string message, OGActionType? actionType, string objectId, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			if (string.IsNullOrEmpty(message))
			{
				throw new ArgumentNullException("message", "message cannot be null or empty!");
			}
			if (!string.IsNullOrEmpty(objectId))
			{
				if ((actionType.GetValueOrDefault() != OGActionType.ASKFOR ? true : !actionType.HasValue))
				{
					if ((actionType.GetValueOrDefault() != OGActionType.SEND ? true : !actionType.HasValue))
					{
						throw new ArgumentNullException("objectId", "Object ID must be set if and only if action type is SEND or ASKFOR");
					}
				}
			}
			if (!actionType.HasValue && !string.IsNullOrEmpty(objectId))
			{
				throw new ArgumentNullException("actionType", "You cannot provide an objectId without an actionType");
			}
		}
	}
}