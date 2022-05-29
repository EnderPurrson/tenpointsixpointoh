using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetProductDataResponseDelegator : IDelegator
	{
		public readonly GetProductDataResponseDelegate responseDelegate;

		public GetProductDataResponseDelegator(GetProductDataResponseDelegate responseDelegate)
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
			this.responseDelegate(GetProductDataResponse.CreateFromDictionary(objectDictionary));
		}
	}
}