using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseResponseDelegator : IDelegator
	{
		public readonly PurchaseResponseDelegate responseDelegate;

		public PurchaseResponseDelegator(PurchaseResponseDelegate responseDelegate)
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
			this.responseDelegate(PurchaseResponse.CreateFromDictionary(objectDictionary));
		}
	}
}