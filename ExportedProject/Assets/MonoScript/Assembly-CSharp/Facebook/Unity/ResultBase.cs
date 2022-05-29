using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal abstract class ResultBase : IInternalResult, IResult
	{
		public virtual string CallbackId
		{
			get;
			protected set;
		}

		public virtual bool Cancelled
		{
			get;
			protected set;
		}

		public virtual string Error
		{
			get;
			protected set;
		}

		public virtual string RawResult
		{
			get;
			protected set;
		}

		public virtual IDictionary<string, object> ResultDictionary
		{
			get;
			protected set;
		}

		internal ResultBase(string result)
		{
			string errorValue = null;
			bool cancelledValue = false;
			string callbackId = null;
			if (!string.IsNullOrEmpty(result))
			{
				Dictionary<string, object> strs = Json.Deserialize(result) as Dictionary<string, object>;
				if (strs != null)
				{
					this.ResultDictionary = strs;
					errorValue = ResultBase.GetErrorValue(strs);
					cancelledValue = ResultBase.GetCancelledValue(strs);
					callbackId = ResultBase.GetCallbackId(strs);
				}
			}
			this.Init(result, errorValue, cancelledValue, callbackId);
		}

		internal ResultBase(string result, string error, bool cancelled)
		{
			this.Init(result, error, cancelled, null);
		}

		private static string GetCallbackId(IDictionary<string, object> result)
		{
			string str;
			if (result == null)
			{
				return null;
			}
			if (result.TryGetValue<string>("callback_id", out str))
			{
				return str;
			}
			return null;
		}

		private static bool GetCancelledValue(IDictionary<string, object> result)
		{
			object obj;
			if (result == null)
			{
				return false;
			}
			if (result.TryGetValue("cancelled", out obj))
			{
				bool? nullable = (bool?)(obj as bool?);
				if (nullable.HasValue)
				{
					return (!nullable.HasValue ? false : nullable.Value);
				}
				string str = obj as string;
				if (str != null)
				{
					return Convert.ToBoolean(str);
				}
				int? nullable1 = (int?)(obj as int?);
				if (nullable1.HasValue)
				{
					return (!nullable1.HasValue ? false : nullable1.Value != 0);
				}
			}
			return false;
		}

		private static string GetErrorValue(IDictionary<string, object> result)
		{
			string str;
			if (result == null)
			{
				return null;
			}
			if (result.TryGetValue<string>("error", out str))
			{
				return str;
			}
			return null;
		}

		protected void Init(string result, string error, bool cancelled, string callbackId)
		{
			this.RawResult = result;
			this.Cancelled = cancelled;
			this.Error = error;
			this.CallbackId = callbackId;
		}

		public override string ToString()
		{
			return string.Format("[BaseResult: Error={0}, Result={1}, RawResult={2}, Cancelled={3}]", new object[] { this.Error, this.ResultDictionary, this.RawResult, this.Cancelled });
		}
	}
}