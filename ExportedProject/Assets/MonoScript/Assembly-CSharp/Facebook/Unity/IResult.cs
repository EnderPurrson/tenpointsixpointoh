using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	public interface IResult
	{
		bool Cancelled
		{
			get;
		}

		string Error
		{
			get;
		}

		string RawResult
		{
			get;
		}

		IDictionary<string, object> ResultDictionary
		{
			get;
		}
	}
}