using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class Ad : Jsonable
	{
		private static AmazonLogger logger;

		public com.amazon.mas.cpt.ads.AdType AdType
		{
			get;
			set;
		}

		public long Identifier
		{
			get;
			set;
		}

		static Ad()
		{
			Ad.logger = new AmazonLogger("Pi");
		}

		public Ad()
		{
		}

		public static Ad CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			Ad ad;
			try
			{
				if (jsonMap != null)
				{
					Ad item = new Ad();
					if (jsonMap.ContainsKey("adType"))
					{
						item.AdType = (com.amazon.mas.cpt.ads.AdType)((int)Enum.Parse(typeof(com.amazon.mas.cpt.ads.AdType), (string)jsonMap["adType"]));
					}
					if (jsonMap.ContainsKey("identifier"))
					{
						item.Identifier = (long)jsonMap["identifier"];
					}
					ad = item;
				}
				else
				{
					ad = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return ad;
		}

		public static Ad CreateFromJson(string jsonMessage)
		{
			Ad ad;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				ad = Ad.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return ad;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "adType", this.AdType },
					{ "identifier", this.Identifier }
				};
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<Ad> ListFromJson(List<object> array)
		{
			List<Ad> ads = new List<Ad>();
			foreach (object obj in array)
			{
				ads.Add(Ad.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return ads;
		}

		public static Dictionary<string, Ad> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, Ad> strs = new Dictionary<string, Ad>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				Ad ad = Ad.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, ad);
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