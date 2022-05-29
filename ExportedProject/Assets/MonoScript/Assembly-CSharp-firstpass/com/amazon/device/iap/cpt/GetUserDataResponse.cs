using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetUserDataResponse : Jsonable
	{
		public com.amazon.device.iap.cpt.AmazonUserData AmazonUserData
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

		public GetUserDataResponse()
		{
		}

		public static GetUserDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			GetUserDataResponse getUserDataResponse;
			try
			{
				if (jsonMap != null)
				{
					GetUserDataResponse item = new GetUserDataResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						item.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("amazonUserData"))
					{
						item.AmazonUserData = com.amazon.device.iap.cpt.AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("status"))
					{
						item.Status = (string)jsonMap["status"];
					}
					getUserDataResponse = item;
				}
				else
				{
					getUserDataResponse = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return getUserDataResponse;
		}

		public static GetUserDataResponse CreateFromJson(string jsonMessage)
		{
			GetUserDataResponse getUserDataResponse;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				getUserDataResponse = GetUserDataResponse.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return getUserDataResponse;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			object objectDictionary;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "requestId", this.RequestId }
				};
				Dictionary<string, object> strs2 = strs1;
				if (this.AmazonUserData == null)
				{
					objectDictionary = null;
				}
				else
				{
					objectDictionary = this.AmazonUserData.GetObjectDictionary();
				}
				strs2.Add("amazonUserData", objectDictionary);
				strs1.Add("status", this.Status);
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<GetUserDataResponse> ListFromJson(List<object> array)
		{
			List<GetUserDataResponse> getUserDataResponses = new List<GetUserDataResponse>();
			foreach (object obj in array)
			{
				getUserDataResponses.Add(GetUserDataResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return getUserDataResponses;
		}

		public static Dictionary<string, GetUserDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetUserDataResponse> strs = new Dictionary<string, GetUserDataResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				GetUserDataResponse getUserDataResponse = GetUserDataResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, getUserDataResponse);
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