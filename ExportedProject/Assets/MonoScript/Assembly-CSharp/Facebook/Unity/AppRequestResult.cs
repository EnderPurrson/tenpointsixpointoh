using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal class AppRequestResult : ResultBase, IAppRequestResult, IResult
	{
		public const string RequestIDKey = "request";

		public const string ToKey = "to";

		public string RequestID
		{
			get
			{
				return JustDecompileGenerated_get_RequestID();
			}
			set
			{
				JustDecompileGenerated_set_RequestID(value);
			}
		}

		private string JustDecompileGenerated_RequestID_k__BackingField;

		public string JustDecompileGenerated_get_RequestID()
		{
			return this.JustDecompileGenerated_RequestID_k__BackingField;
		}

		private void JustDecompileGenerated_set_RequestID(string value)
		{
			this.JustDecompileGenerated_RequestID_k__BackingField = value;
		}

		public IEnumerable<string> To
		{
			get
			{
				return JustDecompileGenerated_get_To();
			}
			set
			{
				JustDecompileGenerated_set_To(value);
			}
		}

		private IEnumerable<string> JustDecompileGenerated_To_k__BackingField;

		public IEnumerable<string> JustDecompileGenerated_get_To()
		{
			return this.JustDecompileGenerated_To_k__BackingField;
		}

		private void JustDecompileGenerated_set_To(IEnumerable<string> value)
		{
			this.JustDecompileGenerated_To_k__BackingField = value;
		}

		public AppRequestResult(string result) : base(result)
		{
			string str;
			string str1;
			IEnumerable<object> objs;
			if (this.ResultDictionary != null)
			{
				if (this.ResultDictionary.TryGetValue<string>("request", out str))
				{
					this.RequestID = str;
				}
				if (this.ResultDictionary.TryGetValue<string>("to", out str1))
				{
					this.To = str1.Split(new char[] { ',' });
				}
				else if (this.ResultDictionary.TryGetValue<IEnumerable<object>>("to", out objs))
				{
					List<string> strs = new List<string>();
					IEnumerator<object> enumerator = objs.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							string current = enumerator.Current as string;
							if (current == null)
							{
								continue;
							}
							strs.Add(current);
						}
					}
					finally
					{
						if (enumerator == null)
						{
						}
						enumerator.Dispose();
					}
					this.To = strs;
				}
			}
		}
	}
}