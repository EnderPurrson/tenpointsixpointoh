using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	public class AccessToken
	{
		public static AccessToken CurrentAccessToken
		{
			get;
			internal set;
		}

		public DateTime ExpirationTime
		{
			get;
			private set;
		}

		public DateTime? LastRefresh
		{
			get;
			private set;
		}

		public IEnumerable<string> Permissions
		{
			get;
			private set;
		}

		public string TokenString
		{
			get;
			private set;
		}

		public string UserId
		{
			get;
			private set;
		}

		internal AccessToken(string tokenString, string userId, DateTime expirationTime, IEnumerable<string> permissions, DateTime? lastRefresh)
		{
			if (string.IsNullOrEmpty(tokenString))
			{
				throw new ArgumentNullException("tokenString");
			}
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("userId");
			}
			if (expirationTime == DateTime.MinValue)
			{
				throw new ArgumentException("Expiration time is unassigned");
			}
			if (permissions == null)
			{
				throw new ArgumentNullException("permissions");
			}
			this.TokenString = tokenString;
			this.ExpirationTime = expirationTime;
			this.Permissions = permissions;
			this.UserId = userId;
			this.LastRefresh = lastRefresh;
		}

		internal string ToJson()
		{
			Dictionary<string, string> strs = new Dictionary<string, string>();
			strs[LoginResult.PermissionsKey] = string.Join(",", this.Permissions.ToArray<string>());
			string expirationTimestampKey = LoginResult.ExpirationTimestampKey;
			long num = this.ExpirationTime.TotalSeconds();
			strs[expirationTimestampKey] = num.ToString();
			strs[LoginResult.AccessTokenKey] = this.TokenString;
			strs[LoginResult.UserIdKey] = this.UserId;
			if (this.LastRefresh.HasValue)
			{
				long num1 = this.LastRefresh.Value.TotalSeconds();
				strs["last_refresh"] = num1.ToString();
			}
			return Json.Serialize(strs);
		}
	}
}