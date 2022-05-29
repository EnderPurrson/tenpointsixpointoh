using System;
using System.Collections.Generic;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdResizedDelegator : IDelegator
	{
		public readonly AdResizedDelegate responseDelegate;

		public AdResizedDelegator(AdResizedDelegate responseDelegate)
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
			this.responseDelegate(AdPosition.CreateFromDictionary(objectDictionary));
		}
	}
}