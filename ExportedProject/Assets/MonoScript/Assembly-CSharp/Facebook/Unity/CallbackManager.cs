using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	internal class CallbackManager
	{
		private IDictionary<string, object> facebookDelegates = new Dictionary<string, object>();

		private int nextAsyncId;

		public CallbackManager()
		{
		}

		public string AddFacebookDelegate<T>(FacebookDelegate<T> callback)
		where T : IResult
		{
			if (callback == null)
			{
				return null;
			}
			this.nextAsyncId++;
			this.facebookDelegates.Add(this.nextAsyncId.ToString(), callback);
			return this.nextAsyncId.ToString();
		}

		private static void CallCallback(object callback, IResult result)
		{
			if (callback == null || result == null)
			{
				return;
			}
			if (!CallbackManager.TryCallCallback<IAppRequestResult>(callback, result) && !CallbackManager.TryCallCallback<IShareResult>(callback, result) && !CallbackManager.TryCallCallback<IGroupCreateResult>(callback, result) && !CallbackManager.TryCallCallback<IGroupJoinResult>(callback, result) && !CallbackManager.TryCallCallback<IPayResult>(callback, result) && !CallbackManager.TryCallCallback<IAppInviteResult>(callback, result) && !CallbackManager.TryCallCallback<IAppLinkResult>(callback, result) && !CallbackManager.TryCallCallback<ILoginResult>(callback, result) && !CallbackManager.TryCallCallback<IAccessTokenRefreshResult>(callback, result))
			{
				throw new NotSupportedException(string.Concat("Unexpected result type: ", callback.GetType().FullName));
			}
		}

		public void OnFacebookResponse(IInternalResult result)
		{
			object obj;
			if (result == null || result.CallbackId == null)
			{
				return;
			}
			if (this.facebookDelegates.TryGetValue(result.CallbackId, out obj))
			{
				CallbackManager.CallCallback(obj, result);
				this.facebookDelegates.Remove(result.CallbackId);
			}
		}

		private static bool TryCallCallback<T>(object callback, IResult result)
		where T : IResult
		{
			FacebookDelegate<T> facebookDelegate = callback as FacebookDelegate<T>;
			if (facebookDelegate == null)
			{
				return false;
			}
			facebookDelegate((T)result);
			return true;
		}
	}
}