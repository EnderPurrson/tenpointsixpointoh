using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkuInput : Jsonable
	{
		public string Sku
		{
			get;
			set;
		}

		public SkuInput()
		{
		}

		public static SkuInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			SkuInput skuInput;
			try
			{
				if (jsonMap != null)
				{
					SkuInput item = new SkuInput();
					if (jsonMap.ContainsKey("sku"))
					{
						item.Sku = (string)jsonMap["sku"];
					}
					skuInput = item;
				}
				else
				{
					skuInput = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return skuInput;
		}

		public static SkuInput CreateFromJson(string jsonMessage)
		{
			SkuInput skuInput;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				skuInput = SkuInput.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return skuInput;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "sku", this.Sku }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<SkuInput> ListFromJson(List<object> array)
		{
			List<SkuInput> skuInputs = new List<SkuInput>();
			foreach (object obj in array)
			{
				skuInputs.Add(SkuInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return skuInputs;
		}

		public static Dictionary<string, SkuInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkuInput> strs = new Dictionary<string, SkuInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				SkuInput skuInput = SkuInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, skuInput);
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