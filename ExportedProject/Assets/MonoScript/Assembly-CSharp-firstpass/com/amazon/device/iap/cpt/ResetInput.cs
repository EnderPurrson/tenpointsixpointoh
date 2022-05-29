using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class ResetInput : Jsonable
	{
		public bool Reset
		{
			get;
			set;
		}

		public ResetInput()
		{
		}

		public static ResetInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			ResetInput resetInput;
			try
			{
				if (jsonMap != null)
				{
					ResetInput item = new ResetInput();
					if (jsonMap.ContainsKey("reset"))
					{
						item.Reset = (bool)jsonMap["reset"];
					}
					resetInput = item;
				}
				else
				{
					resetInput = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return resetInput;
		}

		public static ResetInput CreateFromJson(string jsonMessage)
		{
			ResetInput resetInput;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				resetInput = ResetInput.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return resetInput;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "reset", this.Reset }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<ResetInput> ListFromJson(List<object> array)
		{
			List<ResetInput> resetInputs = new List<ResetInput>();
			foreach (object obj in array)
			{
				resetInputs.Add(ResetInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return resetInputs;
		}

		public static Dictionary<string, ResetInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ResetInput> strs = new Dictionary<string, ResetInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				ResetInput resetInput = ResetInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, resetInput);
			}
			return strs;
		}

		public string ToJson()
		{
			string str;
			try
			{
				str = Json.Serialize(this.GetObjectDictionary());
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while Jsoning", applicationException);
			}
			return str;
		}
	}
}