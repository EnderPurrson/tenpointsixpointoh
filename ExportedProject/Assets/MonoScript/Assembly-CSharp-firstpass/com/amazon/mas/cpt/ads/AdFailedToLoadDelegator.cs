using System;
using System.Collections.Generic;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdFailedToLoadDelegator : IDelegator
	{
		public readonly AdFailedToLoadDelegate responseDelegate;

		public AdFailedToLoadDelegator(AdFailedToLoadDelegate responseDelegate)
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
			this.responseDelegate(Ad.CreateFromDictionary(objectDictionary));
		}
	}
}