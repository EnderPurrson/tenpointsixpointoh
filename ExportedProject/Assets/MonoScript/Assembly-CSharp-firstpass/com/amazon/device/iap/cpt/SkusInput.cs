using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkusInput : Jsonable
	{
		public List<string> Skus
		{
			get;
			set;
		}

		public SkusInput()
		{
		}

		public static SkusInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			SkusInput skusInput;
			try
			{
				if (jsonMap != null)
				{
					SkusInput list = new SkusInput();
					if (jsonMap.ContainsKey("skus"))
					{
						list.Skus = (
							from element in (List<object>)jsonMap["skus"]
							select (string)element).ToList<string>();
					}
					skusInput = list;
				}
				else
				{
					skusInput = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return skusInput;
		}

		public static SkusInput CreateFromJson(string jsonMessage)
		{
			SkusInput skusInput;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				skusInput = SkusInput.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return skusInput;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "skus", this.Skus }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<SkusInput> ListFromJson(List<object> array)
		{
			List<SkusInput> skusInputs = new List<SkusInput>();
			foreach (object obj in array)
			{
				skusInputs.Add(SkusInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return skusInputs;
		}

		public static Dictionary<string, SkusInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkusInput> strs = new Dictionary<string, SkusInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				SkusInput skusInput = SkusInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, skusInput);
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