using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal class LoginResult : ResultBase, ILoginResult, IResult
	{
		public const string LastRefreshKey = "last_refresh";

		public readonly static string UserIdKey;

		public readonly static string ExpirationTimestampKey;

		public readonly static string PermissionsKey;

		public readonly static string AccessTokenKey;

		public Facebook.Unity.AccessToken AccessToken
		{
			get
			{
				return JustDecompileGenerated_get_AccessToken();
			}
			set
			{
				JustDecompileGenerated_set_AccessToken(value);
			}
		}

		private Facebook.Unity.AccessToken JustDecompileGenerated_AccessToken_k__BackingField;

		public Facebook.Unity.AccessToken JustDecompileGenerated_get_AccessToken()
		{
			return this.JustDecompileGenerated_AccessToken_k__BackingField;
		}

		private void JustDecompileGenerated_set_AccessToken(Facebook.Unity.AccessToken value)
		{
			this.JustDecompileGenerated_AccessToken_k__BackingField = value;
		}

		static LoginResult()
		{
			LoginResult.UserIdKey = (!Constants.IsWeb ? "user_id" : "userID");
			LoginResult.ExpirationTimestampKey = (!Constants.IsWeb ? "expiration_timestamp" : "expiresIn");
			LoginResult.PermissionsKey = (!Constants.IsWeb ? "permissions" : "grantedScopes");
			LoginResult.AccessTokenKey = (!Constants.IsWeb ? "access_token" : "accessToken");
		}

		internal LoginResult(string response) : base(response)
		{
			if (this.ResultDictionary != null && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
			{
				this.AccessToken = Utilities.ParseAccessTokenFromResult(this.ResultDictionary);
			}
		}
	}
}