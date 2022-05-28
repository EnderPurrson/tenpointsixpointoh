using System;
using System.Runtime.CompilerServices;
using Com.Google.Android.Gms.Common.Api;
using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AndroidTokenClient : TokenClient
	{
		[CompilerGenerated]
		private sealed class _003CFetch_003Ec__AnonStorey200
		{
			internal string scope;

			internal bool fetchEmail;

			internal bool fetchAccessToken;

			internal bool fetchIdToken;

			internal Action<CommonStatusCodes> doneCallback;

			internal AndroidTokenClient _003C_003Ef__this;

			internal void _003C_003Em__84()
			{
				FetchToken(scope, _003C_003Ef__this.playerId, _003C_003Ef__this.rationale, fetchEmail, fetchAccessToken, fetchIdToken, _003C_003Em__88);
			}

			internal void _003C_003Em__88(int rc, string access, string id, string email)
			{
				if (rc != 0)
				{
					_003C_003Ef__this.apiAccessDenied = rc == 3001 || rc == 16;
					GooglePlayGames.OurUtils.Logger.w("Non-success returned from fetch: " + rc);
					doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
					return;
				}
				if (fetchAccessToken)
				{
					GooglePlayGames.OurUtils.Logger.d("a = " + access);
				}
				if (fetchEmail)
				{
					GooglePlayGames.OurUtils.Logger.d("email = " + email);
				}
				if (fetchIdToken)
				{
					GooglePlayGames.OurUtils.Logger.d("idt = " + id);
				}
				if (fetchAccessToken && !string.IsNullOrEmpty(access))
				{
					_003C_003Ef__this.accessToken = access;
				}
				if (fetchIdToken && !string.IsNullOrEmpty(id))
				{
					_003C_003Ef__this.idToken = id;
					_003C_003Ef__this.idTokenCb(_003C_003Ef__this.idToken);
				}
				if (fetchEmail && !string.IsNullOrEmpty(email))
				{
					_003C_003Ef__this.accountName = email;
				}
				doneCallback(CommonStatusCodes.Success);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetAccountName_003Ec__AnonStorey201
		{
			internal Action<CommonStatusCodes, string> callback;

			internal AndroidTokenClient _003C_003Ef__this;

			internal void _003C_003Em__85(CommonStatusCodes status)
			{
				_003C_003Ef__this.fetchingEmail = false;
				if (callback != null)
				{
					callback(status, _003C_003Ef__this.accountName);
				}
			}
		}

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

		public static AndroidJavaObject GetActivity()
		{
			//Discarded unreachable code: IL_001c
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
		}

		public void SetRationale(string rationale)
		{
			this.rationale = rationale;
		}

		internal void Fetch(string scope, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<CommonStatusCodes> doneCallback)
		{
			_003CFetch_003Ec__AnonStorey200 _003CFetch_003Ec__AnonStorey = new _003CFetch_003Ec__AnonStorey200();
			_003CFetch_003Ec__AnonStorey.scope = scope;
			_003CFetch_003Ec__AnonStorey.fetchEmail = fetchEmail;
			_003CFetch_003Ec__AnonStorey.fetchAccessToken = fetchAccessToken;
			_003CFetch_003Ec__AnonStorey.fetchIdToken = fetchIdToken;
			_003CFetch_003Ec__AnonStorey.doneCallback = doneCallback;
			_003CFetch_003Ec__AnonStorey._003C_003Ef__this = this;
			if (apiAccessDenied)
			{
				if (apiWarningCount++ % apiWarningFreq == 0)
				{
					GooglePlayGames.OurUtils.Logger.w("Access to API denied");
					apiWarningCount = apiWarningCount / apiWarningFreq + 1;
				}
				_003CFetch_003Ec__AnonStorey.doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
			}
			else
			{
				PlayGamesHelperObject.RunOnGameThread(_003CFetch_003Ec__AnonStorey._003C_003Em__84);
			}
		}

		internal static void FetchToken(string scope, string playerId, string rationale, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<int, string, string, string> callback)
		{
			object[] args = new object[7];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
				{
					using (AndroidJavaObject androidJavaObject = GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "fetchToken", "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
						array[0].l = androidJavaObject.GetRawObject();
						array[1].l = AndroidJNI.NewStringUTF(playerId);
						array[2].l = AndroidJNI.NewStringUTF(rationale);
						array[3].z = fetchEmail;
						array[4].z = fetchAccessToken;
						array[5].z = fetchIdToken;
						array[6].l = AndroidJNI.NewStringUTF(scope);
						IntPtr ptr = AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
						PendingResult<TokenResult> pendingResult = new PendingResult<TokenResult>(ptr);
						pendingResult.setResultCallback(new TokenResultCallback(callback));
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching token request: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		private string GetAccountName(Action<CommonStatusCodes, string> callback)
		{
			_003CGetAccountName_003Ec__AnonStorey201 _003CGetAccountName_003Ec__AnonStorey = new _003CGetAccountName_003Ec__AnonStorey201();
			_003CGetAccountName_003Ec__AnonStorey.callback = callback;
			_003CGetAccountName_003Ec__AnonStorey._003C_003Ef__this = this;
			if (string.IsNullOrEmpty(accountName))
			{
				if (!fetchingEmail)
				{
					fetchingEmail = true;
					Fetch(idTokenScope, true, false, false, _003CGetAccountName_003Ec__AnonStorey._003C_003Em__85);
				}
			}
			else if (_003CGetAccountName_003Ec__AnonStorey.callback != null)
			{
				_003CGetAccountName_003Ec__AnonStorey.callback(CommonStatusCodes.Success, accountName);
			}
			return accountName;
		}

		public string GetEmail()
		{
			return GetAccountName(null);
		}

		public void GetEmail(Action<CommonStatusCodes, string> callback)
		{
			GetAccountName(callback);
		}

		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string GetAccessToken()
		{
			if (string.IsNullOrEmpty(accessToken) && !fetchingAccessToken)
			{
				fetchingAccessToken = true;
				Fetch(idTokenScope, false, true, false, _003CGetAccessToken_003Em__86);
			}
			return accessToken;
		}

		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(string serverClientId, Action<string> idTokenCallback)
		{
			if (string.IsNullOrEmpty(serverClientId))
			{
				if (webClientWarningCount++ % webClientWarningFreq == 0)
				{
					GooglePlayGames.OurUtils.Logger.w("serverClientId is empty, cannot get Id Token");
					webClientWarningCount = webClientWarningCount / webClientWarningFreq + 1;
				}
				idTokenCallback(null);
				return;
			}
			string text = "audience:server:client_id:" + serverClientId;
			if (string.IsNullOrEmpty(idToken) || text != idTokenScope)
			{
				if (!fetchingIdToken)
				{
					fetchingIdToken = true;
					idTokenScope = text;
					idTokenCb = idTokenCallback;
					Fetch(idTokenScope, false, false, true, _003CGetIdToken_003Em__87);
				}
			}
			else
			{
				idTokenCallback(idToken);
			}
		}

		[CompilerGenerated]
		private void _003CGetAccessToken_003Em__86(CommonStatusCodes rc)
		{
			fetchingAccessToken = false;
		}

		[CompilerGenerated]
		private void _003CGetIdToken_003Em__87(CommonStatusCodes status)
		{
			fetchingIdToken = false;
			if (status == CommonStatusCodes.Success)
			{
				idTokenCb(null);
			}
			else
			{
				idTokenCb(idToken);
			}
		}
	}
}
