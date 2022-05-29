using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class AmazonUserData : Jsonable
	{
		public string Marketplace
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public AmazonUserData()
		{
		}

		public static AmazonUserData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			AmazonUserData amazonUserDatum;
			try
			{
				if (jsonMap != null)
				{
					AmazonUserData item = new AmazonUserData();
					if (jsonMap.ContainsKey("userId"))
					{
						item.UserId = (string)jsonMap["userId"];
					}
					if (jsonMap.ContainsKey("marketplace"))
					{
						item.Marketplace = (string)jsonMap["marketplace"];
					}
					amazonUserDatum = item;
				}
				else
				{
					amazonUserDatum = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return amazonUserDatum;
		}

		public static AmazonUserData CreateFromJson(string jsonMessage)
		{
			AmazonUserData amazonUserDatum;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				amazonUserDatum = AmazonUserData.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return amazonUserDatum;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "userId", this.UserId },
					{ "marketplace", this.Marketplace }
				};
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<AmazonUserData> ListFromJson(List<object> array)
		{
			List<AmazonUserData> amazonUserDatas = new List<AmazonUserData>();
			foreach (object obj in array)
			{
				amazonUserDatas.Add(AmazonUserData.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return amazonUserDatas;
		}

		public static Dictionary<string, AmazonUserData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AmazonUserData> strs = new Dictionary<string, AmazonUserData>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				AmazonUserData amazonUserDatum = AmazonUserData.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, amazonUserDatum);
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