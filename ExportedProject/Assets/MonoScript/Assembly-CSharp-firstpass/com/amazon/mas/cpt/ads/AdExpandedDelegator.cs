using System;
using System.Collections.Generic;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdExpandedDelegator : IDelegator
	{
		public readonly AdExpandedDelegate responseDelegate;

		public AdExpandedDelegator(AdExpandedDelegate responseDelegate)
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