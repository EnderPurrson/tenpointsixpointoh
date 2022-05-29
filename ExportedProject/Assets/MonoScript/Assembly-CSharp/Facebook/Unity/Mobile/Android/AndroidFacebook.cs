using Facebook.Unity;
using Facebook.Unity.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Facebook.Unity.Mobile.Android
{
	internal sealed class AndroidFacebook : MobileFacebook
	{
		public const string LoginPermissionsKey = "scope";

		private bool limitEventUsage;

		private IAndroidJavaClass facebookJava;

		public string KeyHash
		{
			get;
			private set;
		}

		public override bool LimitEventUsage
		{
			get
			{
				return this.limitEventUsage;
			}
			set
			{
				this.limitEventUsage = value;
				this.CallFB("SetLimitEventUsage", value.ToString());
			}
		}

		public override string SDKName
		{
			get
			{
				return "FBAndroidSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return this.facebookJava.CallStatic<string>("GetSdkVersion");
			}
		}

		public AndroidFacebook() : this(new FBJavaClass(), new Facebook.Unity.CallbackManager())
		{
		}

		public AndroidFacebook(IAndroidJavaClass facebookJavaClass, Facebook.Unity.CallbackManager callbackManager) : base(callbackManager)
		{
			this.KeyHash = string.Empty;
			this.facebookJava = facebookJavaClass;
		}

		public override void ActivateApp(string appId)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("app_id", appId);
			(new AndroidFacebook.JavaMethodCall<IResult>(this, "ActivateApp")).Call(methodArgument);
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("logEvent", logEvent);
			methodArgument.AddNullablePrimitive<float>("valueToSum", valueToSum);
			methodArgument.AddDictionary("parameters", parameters);
			(new AndroidFacebook.JavaMethodCall<IResult>(this, "LogAppEvent")).Call(methodArgument);
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddPrimative<float>("logPurchase", logPurchase);
			methodArgument.AddString("currency", currency);
			methodArgument.AddDictionary("parameters", parameters);
			(new AndroidFacebook.JavaMethodCall<IResult>(this, "LogAppEvent")).Call(methodArgument);
		}

		public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddUri("appLinkUrl", appLinkUrl);
			methodArgument.AddUri("previewImageUrl", previewImageUrl);
			AndroidFacebook.JavaMethodCall<IAppInviteResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IAppInviteResult>(this, "AppInvite")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("message", message);
			methodArgument.AddNullablePrimitive<OGActionType>("action_type", actionType);
			methodArgument.AddString("object_id", objectId);
			methodArgument.AddCommaSeparatedList("to", to);
			if (filters != null && filters.Any<object>())
			{
				string str = filters.First<object>() as string;
				if (str != null)
				{
					methodArgument.AddString("filters", str);
				}
			}
			methodArgument.AddNullablePrimitive<int>("max_recipients", maxRecipients);
			methodArgument.AddString("data", data);
			methodArgument.AddString("title", title);
			AndroidFacebook.JavaMethodCall<IAppRequestResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IAppRequestResult>(this, "AppRequest")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		private void CallFB(string method, string args)
		{
			this.facebookJava.CallStatic(method, new object[] { args });
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("toId", toId);
			methodArgument.AddUri("link", link);
			methodArgument.AddString("linkName", linkName);
			methodArgument.AddString("linkCaption", linkCaption);
			methodArgument.AddString("linkDescription", linkDescription);
			methodArgument.AddUri("picture", picture);
			methodArgument.AddString("mediaSource", mediaSource);
			AndroidFacebook.JavaMethodCall<IShareResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IShareResult>(this, "FeedShare")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			AndroidFacebook.JavaMethodCall<IAppLinkResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IAppLinkResult>(this, "FetchDeferredAppLinkData")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("name", name);
			methodArgument.AddString("description", description);
			methodArgument.AddString("privacy", privacy);
			AndroidFacebook.JavaMethodCall<IGroupCreateResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IGroupCreateResult>(this, "GameGroupCreate")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("id", id);
			AndroidFacebook.JavaMethodCall<IGroupJoinResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IGroupJoinResult>(this, "GameGroupJoin")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			AndroidFacebook.JavaMethodCall<IAppLinkResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IAppLinkResult>(this, "GetAppLink")
			{
				Callback = callback
			};
			javaMethodCall.Call(null);
		}

		public void Init(string appId, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			this.CallFB("SetUserAgentSuffix", string.Format("Unity.{0}", Constants.UnitySDKUserAgentSuffixLegacy));
			base.Init(hideUnityDelegate, onInitComplete);
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("appId", appId);
			(new AndroidFacebook.JavaMethodCall<IResult>(this, "Init")).Call(methodArgument);
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddCommaSeparatedList("scope", permissions);
			AndroidFacebook.JavaMethodCall<ILoginResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<ILoginResult>(this, "LoginWithPublishPermissions")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddCommaSeparatedList("scope", permissions);
			AndroidFacebook.JavaMethodCall<ILoginResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<ILoginResult>(this, "LoginWithReadPermissions")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		public override void LogOut()
		{
			(new AndroidFacebook.JavaMethodCall<IResult>(this, "Logout")).Call(null);
		}

		public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			AndroidFacebook.JavaMethodCall<IAccessTokenRefreshResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IAccessTokenRefreshResult>(this, "RefreshCurrentAccessToken")
			{
				Callback = callback
			};
			javaMethodCall.Call(null);
		}

		protected override void SetShareDialogMode(Facebook.Unity.ShareDialogMode mode)
		{
			this.CallFB("SetShareDialogMode", mode.ToString());
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddUri("content_url", contentURL);
			methodArgument.AddString("content_title", contentTitle);
			methodArgument.AddString("content_description", contentDescription);
			methodArgument.AddUri("photo_url", photoURL);
			AndroidFacebook.JavaMethodCall<IShareResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IShareResult>(this, "ShareLink")
			{
				Callback = callback
			};
			javaMethodCall.Call(methodArgument);
		}

		private class JavaMethodCall<T> : MethodCall<T>
		where T : IResult
		{
			private AndroidFacebook androidImpl;

			public JavaMethodCall(AndroidFacebook androidImpl, string methodName) : base(androidImpl, methodName)
			{
				this.androidImpl = androidImpl;
			}

			public override void Call(MethodArguments args = null)
			{
				MethodArguments methodArgument;
				methodArgument = (args != null ? new MethodArguments(args) : new MethodArguments());
				if (base.Callback != null)
				{
					methodArgument.AddString("callback_id", this.androidImpl.CallbackManager.AddFacebookDelegate<T>(base.Callback));
				}
				this.androidImpl.CallFB(base.MethodName, methodArgument.ToJsonString());
			}
		}
	}
}