using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal class AccessTokenRefreshResult : ResultBase, IAccessTokenRefreshResult, IResult
	{
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

		public AccessTokenRefreshResult(string result) : base(result)
		{
			if (this.ResultDictionary != null && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
			{
				this.AccessToken = Utilities.ParseAccessTokenFromResult(this.ResultDictionary);
			}
		}
	}
}