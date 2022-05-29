using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdShown : Jsonable
	{
		private static AmazonLogger logger;

		public bool BooleanValue
		{
			get;
			set;
		}

		static AdShown()
		{
			AdShown.logger = new AmazonLogger("Pi");
		}

		public AdShown()
		{
		}

		public static AdShown CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			AdShown adShown;
			try
			{
				if (jsonMap != null)
				{
					AdShown item = new AdShown();
					if (jsonMap.ContainsKey("booleanValue"))
					{
						item.BooleanValue = (bool)jsonMap["booleanValue"];
					}
					adShown = item;
				}
				else
				{
					adShown = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return adShown;
		}

		public static AdShown CreateFromJson(string jsonMessage)
		{
			AdShown adShown;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				adShown = AdShown.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return adShown;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "booleanValue", this.BooleanValue }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<AdShown> ListFromJson(List<object> array)
		{
			List<AdShown> adShowns = new List<AdShown>();
			foreach (object obj in array)
			{
				adShowns.Add(AdShown.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return adShowns;
		}

		public static Dictionary<string, AdShown> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AdShown> strs = new Dictionary<string, AdShown>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				AdShown adShown = AdShown.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, adShown);
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