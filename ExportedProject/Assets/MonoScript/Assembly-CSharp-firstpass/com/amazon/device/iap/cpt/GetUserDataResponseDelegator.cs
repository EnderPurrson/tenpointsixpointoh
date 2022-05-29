using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetUserDataResponseDelegator : IDelegator
	{
		public readonly GetUserDataResponseDelegate responseDelegate;

		public GetUserDataResponseDelegator(GetUserDataResponseDelegate responseDelegate)
		{
			this.responseDelegate = responseDelegate;
		}

		public void ExecuteError(AmazonException e)
		{
		}

		public void ExecuteSuccess()
		{
		}

		public void ExecuteSuccess(Dictionary<string, object> objectDictionary)
		{
			this.responseDelegate(GetUserDataResponse.CreateFromDictionary(objectDictionary));
		}
	}
}