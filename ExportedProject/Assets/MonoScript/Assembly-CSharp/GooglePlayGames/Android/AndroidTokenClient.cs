using Com.Google.Android.Gms.Common.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTokenClient : TokenClient
	{
		private const string TokenFragmentClass = "com.google.games.bridge.TokenFragment";

		private const string FetchTokenSignature = "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";

		private const string FetchTokenMethod = "fetchToken";

		private string playerId;

		private bool fetchingEmail;

		private bool fetchingAccessToken;

		private bool fetchingIdToken;

		private string accountName;

		private string accessToken;

		private string idToken;

		private string idTokenScope;

		private Action<string> idTokenCb;

		private string rationale;

		private bool apiAccessDenied;

		private int apiWarningFreq = 100000;

		private int apiWarningCount;

		private int webClientWarningFreq = 100000;

		private int webClientWarningCount;

		public AndroidTokenClient(string playerId)
		{
			this.playerId = playerId;
		}

		internal void Fetch(string scope, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<CommonStatusCodes> doneCallback)
		{
			if (!this.apiAccessDenied)
			{
				PlayGamesHelperObject.RunOnGameThread(() => AndroidTokenClient.FetchToken(scope, this.playerId, this.rationale, fetchEmail, fetchAccessToken, fetchIdToken, (int rc, string access, string id, string email) => {
					if (rc != 0)
					{
						this.apiAccessDenied = (rc == 3001 ? true : rc == 16);
						GooglePlayGames.OurUtils.Logger.w(string.Concat("Non-success returned from fetch: ", rc));
						doneCallback(3001);
						return;
					}
					if (fetchAccessToken)
					{
						GooglePlayGames.OurUtils.Logger.d(string.Concat("a = ", access));
					}
					if (fetchEmail)
					{
						GooglePlayGames.OurUtils.Logger.d(string.Concat("email = ", email));
					}
					if (fetchIdToken)
					{
						GooglePlayGames.OurUtils.Logger.d(string.Concat("idt = ", id));
					}
					if (fetchAccessToken && !string.IsNullOrEmpty(access))
					{
						this.accessToken = access;
					}
					if (fetchIdToken && !string.IsNullOrEmpty(id))
					{
						this.idToken = id;
						this.idTokenCb(this.idToken);
					}
					if (fetchEmail && !string.IsNullOrEmpty(email))
					{
						this.accountName = email;
					}
					doneCallback(0);
				}));
				return;
			}
			AndroidTokenClient androidTokenClient = this;
			int num = androidTokenClient.apiWarningCount;
			int num1 = num;
			androidTokenClient.apiWarningCount = num + 1;
			if (num1 % this.apiWarningFreq == 0)
			{
				GooglePlayGames.OurUtils.Logger.w("Access to API denied");
				this.apiWarningCount = this.apiWarningCount / this.apiWarningFreq + 1;
			}
			doneCallback(3001);
		}

		internal static void FetchToken(string scope, string playerId, string rationale, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<int, string, string, string> callback)
		{
			object[] objArray = new object[7];
			jvalue[] rawObject = AndroidJNIHelper.CreateJNIArgArray(objArray);
			try
			{
				try
				{
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
					{
						using (AndroidJavaObject activity = AndroidTokenClient.GetActivity())
						{
							IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "fetchToken", "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
							rawObject[0].l = activity.GetRawObject();
							rawObject[1].l = AndroidJNI.NewStringUTF(playerId);
							rawObject[2].l = AndroidJNI.NewStringUTF(rationale);
							rawObject[3].z = fetchEmail;
							rawObject[4].z = fetchAccessToken;
							rawObject[5].z = fetchIdToken;
							rawObject[6].l = AndroidJNI.NewStringUTF(scope);
							IntPtr intPtr = AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, rawObject);
							(new PendingResult<TokenResult>(intPtr)).setResultCallback(new TokenResultCallback(callback));
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					GooglePlayGames.OurUtils.Logger.e(string.Concat("Exception launching token request: ", exception.Message));
					GooglePlayGames.OurUtils.Logger.e(exception.ToString());
				}
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(objArray, rawObject);
			}
		}

		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string GetAccessToken()
		{
			if (string.IsNullOrEmpty(this.accessToken) && !this.fetchingAccessToken)
			{
				this.fetchingAccessToken = true;
				this.Fetch(this.idTokenScope, false, true, false, (CommonStatusCodes rc) => this.fetchingAccessToken = false);
			}
			return this.accessToken;
		}

		private string GetAccountName(Action<CommonStatusCodes, string> callback)
		{
			if (string.IsNullOrEmpty(this.accountName))
			{
				if (!this.fetchingEmail)
				{
					this.fetchingEmail = true;
					this.Fetch(this.idTokenScope, true, false, false, (CommonStatusCodes status) => {
						this.fetchingEmail = false;
						if (callback != null)
						{
							callback(status, this.accountName);
						}
					});
				}
			}
			else if (callback != null)
			{
				callback(0, this.accountName);
			}
			return this.accountName;
		}

		public static AndroidJavaObject GetActivity()
		{
			AndroidJavaObject @static;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return @static;
		}

		public string GetEmail()
		{
			return this.GetAccountName(null);
		}

		public void GetEmail(Action<CommonStatusCodes, string> callback)
		{
			this.GetAccountName(callback);
		}

		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(string serverClientId, Action<string> idTokenCallback)
		{
			if (string.IsNullOrEmpty(serverClientId))
			{
				AndroidTokenClient androidTokenClient = this;
				int num = androidTokenClient.webClientWarningCount;
				int num1 = num;
				androidTokenClient.webClientWarningCount = num + 1;
				if (num1 % this.webClientWarningFreq == 0)
				{
					GooglePlayGames.OurUtils.Logger.w("serverClientId is empty, cannot get Id Token");
					this.webClientWarningCount = this.webClientWarningCount / this.webClientWarningFreq + 1;
				}
				idTokenCallback(null);
				return;
			}
			string str = string.Concat("audience:server:client_id:", serverClientId);
			if (!string.IsNullOrEmpty(this.idToken) && str == this.idTokenScope)
			{
				idTokenCallback(this.idToken);
			}
			else if (!this.fetchingIdToken)
			{
				this.fetchingIdToken = true;
				this.idTokenScope = str;
				this.idTokenCb = idTokenCallback;
				this.Fetch(this.idTokenScope, false, false, true, (CommonStatusCodes status) => {
					this.fetchingIdToken = false;
					if (status != CommonStatusCodes.Success)
					{
						this.idTokenCb(this.idToken);
					}
					else
					{
						this.idTokenCb(null);
					}
				});
			}
		}

		public void SetRationale(string rationale)
		{
			this.rationale = rationale;
		}
	}
}