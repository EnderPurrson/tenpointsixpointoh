using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdPair : Jsonable
	{
		private static AmazonLogger logger;

		public Ad AdOne
		{
			get;
			set;
		}

		public Ad AdTwo
		{
			get;
			set;
		}

		static AdPair()
		{
			AdPair.logger = new AmazonLogger("Pi");
		}

		public AdPair()
		{
		}

		public static AdPair CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			AdPair adPair;
			try
			{
				if (jsonMap != null)
				{
					AdPair adPair1 = new AdPair();
					if (jsonMap.ContainsKey("adOne"))
					{
						adPair1.AdOne = Ad.CreateFromDictionary(jsonMap["adOne"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("adTwo"))
					{
						adPair1.AdTwo = Ad.CreateFromDictionary(jsonMap["adTwo"] as Dictionary<string, object>);
					}
					adPair = adPair1;
				}
				else
				{
					adPair = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return adPair;
		}

		public static AdPair CreateFromJson(string jsonMessage)
		{
			AdPair adPair;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				adPair = AdPair.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return adPair;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			object objectDictionary;
			object obj;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>();
				Dictionary<string, object> strs2 = strs1;
				if (this.AdOne == null)
				{
					objectDictionary = null;
				}
				else
				{
					objectDictionary = this.AdOne.GetObjectDictionary();
				}
				strs2.Add("adOne", objectDictionary);
				Dictionary<string, object> strs3 = strs1;
				if (this.AdTwo == null)
				{
					obj = null;
				}
				else
				{
					obj = this.AdTwo.GetObjectDictionary();
				}
				strs3.Add("adTwo", obj);
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<AdPair> ListFromJson(List<object> array)
		{
			List<AdPair> adPairs = new List<AdPair>();
			foreach (object obj in array)
			{
				adPairs.Add(AdPair.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return adPairs;
		}

		public static Dictionary<string, AdPair> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AdPair> strs = new Dictionary<string, AdPair>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				AdPair adPair = AdPair.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, adPair);
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