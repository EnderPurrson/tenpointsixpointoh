using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetProductDataResponse : Jsonable
	{
		public Dictionary<string, ProductData> ProductDataMap
		{
			get;
			set;
		}

		public string RequestId
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public List<string> UnavailableSkus
		{
			get;
			set;
		}

		public GetProductDataResponse()
		{
		}

		public static GetProductDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			GetProductDataResponse getProductDataResponse;
			try
			{
				if (jsonMap != null)
				{
					GetProductDataResponse item = new GetProductDataResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						item.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("productDataMap"))
					{
						item.ProductDataMap = ProductData.MapFromJson(jsonMap["productDataMap"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("unavailableSkus"))
					{
						item.UnavailableSkus = (
							from element in (List<object>)jsonMap["unavailableSkus"]
							select (string)element).ToList<string>();
					}
					if (jsonMap.ContainsKey("status"))
					{
						item.Status = (string)jsonMap["status"];
					}
					getProductDataResponse = item;
				}
				else
				{
					getProductDataResponse = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return getProductDataResponse;
		}

		public static GetProductDataResponse CreateFromJson(string jsonMessage)
		{
			GetProductDataResponse getProductDataResponse;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				getProductDataResponse = GetProductDataResponse.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return getProductDataResponse;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			object obj;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "requestId", this.RequestId }
				};
				Dictionary<string, object> strs2 = strs1;
				if (this.ProductDataMap == null)
				{
					obj = null;
				}
				else
				{
					obj = Jsonable.unrollObjectIntoMap<ProductData>(this.ProductDataMap);
				}
				strs2.Add("productDataMap", obj);
				strs1.Add("unavailableSkus", this.UnavailableSkus);
				strs1.Add("status", this.Status);
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<GetProductDataResponse> ListFromJson(List<object> array)
		{
			List<GetProductDataResponse> getProductDataResponses = new List<GetProductDataResponse>();
			foreach (object obj in array)
			{
				getProductDataResponses.Add(GetProductDataResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return getProductDataResponses;
		}

		public static Dictionary<string, GetProductDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetProductDataResponse> strs = new Dictionary<string, GetProductDataResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				GetProductDataResponse getProductDataResponse = GetProductDataResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, getProductDataResponse);
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