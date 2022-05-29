using Facebook.MiniJSON;
using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Facebook.Unity.Canvas
{
	internal sealed class CanvasFacebook : FacebookBase, ICanvasFacebook, ICanvasFacebookCallbackHandler, ICanvasFacebookImplementation, IFacebook, IFacebookCallbackHandler
	{
		internal const string MethodAppRequests = "apprequests";

		internal const string MethodFeed = "feed";

		internal const string MethodPay = "pay";

		internal const string MethodGameGroupCreate = "game_group_create";

		internal const string MethodGameGroupJoin = "game_group_join";

		internal const string CancelledResponse = "{\"cancelled\":true}";

		internal const string FacebookConnectURL = "https://connect.facebook.net";

		private const string AuthResponseKey = "authResponse";

		private const string ResponseKey = "response";

		private string appId;

		private string appLinkUrl;

		private ICanvasJSWrapper canvasJSWrapper;

		public override bool LimitEventUsage
		{
			get;
			set;
		}

		public override string SDKName
		{
			get
			{
				return "FBJSSDK";
			}
		}

		public override string SDKUserAgent
		{
			get
			{
				string str;
				FacebookUnityPlatform currentPlatform = Constants.CurrentPlatform;
				if (currentPlatform == FacebookUnityPlatform.WebGL || currentPlatform == FacebookUnityPlatform.WebPlayer)
				{
					str = string.Format(CultureInfo.InvariantCulture, "FBUnity{0}", new object[] { Constants.CurrentPlatform.ToString() });
				}
				else
				{
					FacebookLogger.Warn("Currently running on uknown web platform");
					str = "FBUnityWebUnknown";
				}
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[] { base.SDKUserAgent, Utilities.GetUserAgent(str, FacebookSdkVersion.Build) });
			}
		}

		public override string SDKVersion
		{
			get
			{
				return this.canvasJSWrapper.GetSDKVersion();
			}
		}

		public CanvasFacebook() : this(new CanvasJSWrapper(), new Facebook.Unity.CallbackManager())
		{
		}

		public CanvasFacebook(ICanvasJSWrapper canvasJSWrapper, Facebook.Unity.CallbackManager callbackManager) : base(callbackManager)
		{
			this.canvasJSWrapper = canvasJSWrapper;
		}

		public override void ActivateApp(string appId)
		{
			this.canvasJSWrapper.ExternalCall("FBUnity.activateApp", new object[0]);
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			this.canvasJSWrapper.ExternalCall("FBUnity.logAppEvent", new object[] { logEvent, valueToSum, Json.Serialize(parameters) });
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			this.canvasJSWrapper.ExternalCall("FBUnity.logPurchase", new object[] { logPurchase, currency, Json.Serialize(parameters) });
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			string str;
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("message", message);
			methodArgument.AddCommaSeparatedList("to", to);
			MethodArguments methodArgument1 = methodArgument;
			if (!actionType.HasValue)
			{
				str = null;
			}
			else
			{
				str = actionType.ToString();
			}
			methodArgument1.AddString("action_type", str);
			methodArgument.AddString("object_id", objectId);
			methodArgument.AddList<object>("filters", filters);
			methodArgument.AddList<string>("exclude_ids", excludeIds);
			methodArgument.AddNullablePrimitive<int>("max_recipients", maxRecipients);
			methodArgument.AddString("data", data);
			methodArgument.AddString("title", title);
			CanvasFacebook.CanvasUIMethodCall<IAppRequestResult> canvasUIMethodCall = new CanvasFacebook.CanvasUIMethodCall<IAppRequestResult>(this, "apprequests", "OnAppRequestsComplete")
			{
				Callback = callback
			};
			canvasUIMethodCall.Call(methodArgument);
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("to", toId);
			methodArgument.AddUri("link", link);
			methodArgument.AddString("name", linkName);
			methodArgument.AddString("caption", linkCaption);
			methodArgument.AddString("description", linkDescription);
			methodArgument.AddUri("picture", picture);
			methodArgument.AddString("source", mediaSource);
			CanvasFacebook.CanvasUIMethodCall<IShareResult> canvasUIMethodCall = new CanvasFacebook.CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete")
			{
				Callback = callback
			};
			canvasUIMethodCall.Call(methodArgument);
		}

		private static string FormatAuthResponse(string result)
		{
			IDictionary<string, object> strs;
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}
			IDictionary<string, object> formattedResponseDictionary = CanvasFacebook.GetFormattedResponseDictionary(result);
			if (formattedResponseDictionary.TryGetValue<IDictionary<string, object>>("authResponse", out strs))
			{
				formattedResponseDictionary.Remove("authResponse");
				IEnumerator<KeyValuePair<string, object>> enumerator = strs.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, object> current = enumerator.Current;
						formattedResponseDictionary[current.Key] = current.Value;
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
			}
			return Json.Serialize(formattedResponseDictionary);
		}

		private static string FormatResult(string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}
			return Json.Serialize(CanvasFacebook.GetFormattedResponseDictionary(result));
		}

		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("name", name);
			methodArgument.AddString("description", description);
			methodArgument.AddString("privacy", privacy);
			methodArgument.AddString("display", "async");
			CanvasFacebook.CanvasUIMethodCall<IGroupCreateResult> canvasUIMethodCall = new CanvasFacebook.CanvasUIMethodCall<IGroupCreateResult>(this, "game_group_create", "OnGroupCreateComplete")
			{
				Callback = callback
			};
			canvasUIMethodCall.Call(methodArgument);
		}

		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("id", id);
			methodArgument.AddString("display", "async");
			CanvasFacebook.CanvasUIMethodCall<IGroupJoinResult> canvasUIMethodCall = new CanvasFacebook.CanvasUIMethodCall<IGroupJoinResult>(this, "game_group_join", "OnJoinGroupComplete")
			{
				Callback = callback
			};
			canvasUIMethodCall.Call(methodArgument);
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "url", this.appLinkUrl }
			};
			callback(new AppLinkResult(Json.Serialize(strs)));
			this.appLinkUrl = string.Empty;
		}

		private static IDictionary<string, object> GetFormattedResponseDictionary(string result)
		{
			IDictionary<string, object> strs;
			object obj;
			IDictionary<string, object> strs1 = (IDictionary<string, object>)Json.Deserialize(result);
			if (!strs1.TryGetValue<IDictionary<string, object>>("response", out strs))
			{
				return strs1;
			}
			if (strs1.TryGetValue("callback_id", out obj))
			{
				strs["callback_id"] = obj;
			}
			return strs;
		}

		public void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, string jsSDKLocale, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			if (this.canvasJSWrapper.IntegrationMethodJs == null)
			{
				throw new Exception("Cannot initialize facebook javascript");
			}
			base.Init(hideUnityDelegate, onInitComplete);
			this.canvasJSWrapper.ExternalEval(this.canvasJSWrapper.IntegrationMethodJs);
			this.appId = appId;
			bool flag = true;
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("appId", appId);
			methodArgument.AddPrimative<bool>("cookie", cookie);
			methodArgument.AddPrimative<bool>("logging", logging);
			methodArgument.AddPrimative<bool>("status", status);
			methodArgument.AddPrimative<bool>("xfbml", xfbml);
			methodArgument.AddString("channelUrl", channelUrl);
			methodArgument.AddString("authResponse", authResponse);
			methodArgument.AddPrimative<bool>("frictionlessRequests", frictionlessRequests);
			methodArgument.AddString("version", FB.GraphApiVersion);
			this.canvasJSWrapper.ExternalCall("FBUnity.init", new object[] { (!flag ? 0 : 1), "https://connect.facebook.net", jsSDKLocale, (!Constants.DebugMode ? 0 : 1), methodArgument.ToJsonString(), (!status ? 0 : 1) });
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.canvasJSWrapper.DisableFullScreen();
			this.canvasJSWrapper.ExternalCall("FBUnity.login", new object[] { permissions, base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback) });
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.canvasJSWrapper.DisableFullScreen();
			this.canvasJSWrapper.ExternalCall("FBUnity.login", new object[] { permissions, base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback) });
		}

		public override void LogOut()
		{
			base.LogOut();
			this.canvasJSWrapper.ExternalCall("FBUnity.logout", new object[0]);
		}

		public override void OnAppRequestsComplete(string responseJsonData)
		{
			AppRequestResult appRequestResult = new AppRequestResult(CanvasFacebook.FormatResult(responseJsonData));
			base.CallbackManager.OnFacebookResponse(appRequestResult);
		}

		public void OnFacebookAuthResponseChange(string responseJsonData)
		{
			string str = CanvasFacebook.FormatAuthResponse(responseJsonData);
			AccessToken.CurrentAccessToken = (new LoginResult(str)).AccessToken;
		}

		public override void OnGetAppLinkComplete(string message)
		{
			throw new NotImplementedException();
		}

		public override void OnGroupCreateComplete(string responseJsonData)
		{
			GroupCreateResult groupCreateResult = new GroupCreateResult(CanvasFacebook.FormatResult(responseJsonData));
			base.CallbackManager.OnFacebookResponse(groupCreateResult);
		}

		public override void OnGroupJoinComplete(string responseJsonData)
		{
			GroupJoinResult groupJoinResult = new GroupJoinResult(CanvasFacebook.FormatResult(responseJsonData));
			base.CallbackManager.OnFacebookResponse(groupJoinResult);
		}

		public override void OnLoginComplete(string responseJsonData)
		{
			base.OnAuthResponse(new LoginResult(CanvasFacebook.FormatAuthResponse(responseJsonData)));
		}

		public void OnPayComplete(string responseJsonData)
		{
			PayResult payResult = new PayResult(CanvasFacebook.FormatResult(responseJsonData));
			base.CallbackManager.OnFacebookResponse(payResult);
		}

		public override void OnShareLinkComplete(string responseJsonData)
		{
			ShareResult shareResult = new ShareResult(CanvasFacebook.FormatResult(responseJsonData));
			base.CallbackManager.OnFacebookResponse(shareResult);
		}

		public void OnUrlResponse(string url)
		{
			this.appLinkUrl = url;
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddString("product", product);
			methodArgument.AddString("action", action);
			methodArgument.AddPrimative<int>("quantity", quantity);
			methodArgument.AddNullablePrimitive<int>("quantity_min", quantityMin);
			methodArgument.AddNullablePrimitive<int>("quantity_max", quantityMax);
			methodArgument.AddString("request_id", requestId);
			methodArgument.AddString("pricepoint_id", pricepointId);
			methodArgument.AddString("test_currency", testCurrency);
			CanvasFacebook.CanvasUIMethodCall<IPayResult> canvasUIMethodCall = new CanvasFacebook.CanvasUIMethodCall<IPayResult>(this, "pay", "OnPayComplete")
			{
				Callback = callback
			};
			canvasUIMethodCall.Call(methodArgument);
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArgument = new MethodArguments();
			methodArgument.AddUri("link", contentURL);
			methodArgument.AddString("name", contentTitle);
			methodArgument.AddString("description", contentDescription);
			methodArgument.AddUri("picture", photoURL);
			CanvasFacebook.CanvasUIMethodCall<IShareResult> canvasUIMethodCall = new CanvasFacebook.CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete")
			{
				Callback = callback
			};
			canvasUIMethodCall.Call(methodArgument);
		}

		private class CanvasUIMethodCall<T> : MethodCall<T>
		where T : IResult
		{
			private CanvasFacebook canvasImpl;

			private string callbackMethod;

			public CanvasUIMethodCall(CanvasFacebook canvasImpl, string methodName, string callbackMethod) : base(canvasImpl, methodName)
			{
				this.canvasImpl = canvasImpl;
				this.callbackMethod = callbackMethod;
			}

			public override void Call(MethodArguments args)
			{
				this.UI(base.MethodName, args, base.Callback);
			}

			private void UI(string method, MethodArguments args, FacebookDelegate<T> callback = null)
			{
				this.canvasImpl.canvasJSWrapper.DisableFullScreen();
				MethodArguments methodArgument = new MethodArguments(args);
				methodArgument.AddString("app_id", this.canvasImpl.appId);
				methodArgument.AddString("method", method);
				string str = this.canvasImpl.CallbackManager.AddFacebookDelegate<T>(callback);
				this.canvasImpl.canvasJSWrapper.ExternalCall("FBUnity.ui", new object[] { methodArgument.ToJsonString(), str, this.callbackMethod });
			}
		}
	}
}