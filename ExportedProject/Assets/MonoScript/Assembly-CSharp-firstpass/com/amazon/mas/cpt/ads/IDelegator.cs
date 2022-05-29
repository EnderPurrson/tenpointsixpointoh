using System;
using System.Collections.Generic;

namespace com.amazon.mas.cpt.ads
{
	public interface IDelegator
	{
		void ExecuteError(AmazonException e);

		void ExecuteSuccess();

		void ExecuteSuccess(Dictionary<string, object> objDict);
	}
}