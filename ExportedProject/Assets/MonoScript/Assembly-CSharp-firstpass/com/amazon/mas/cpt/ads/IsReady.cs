using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class IsReady : Jsonable
	{
		private static AmazonLogger logger;

		public bool BooleanValue
		{
			get;
			set;
		}

		static IsReady()
		{
			IsReady.logger = new AmazonLogger("Pi");
		}

		public IsReady()
		{
		}

		public static IsReady CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			IsReady isReady;
			try
			{
				if (jsonMap != null)
				{
					IsReady item = new IsReady();
					if (jsonMap.ContainsKey("booleanValue"))
					{
						item.BooleanValue = (bool)jsonMap["booleanValue"];
					}
					isReady = item;
				}
				else
				{
					isReady = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return isReady;
		}

		public static IsReady CreateFromJson(string jsonMessage)
		{
			IsReady isReady;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				isReady = IsReady.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return isReady;
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

		public static List<IsReady> ListFromJson(List<object> array)
		{
			List<IsReady> isReadies = new List<IsReady>();
			foreach (object obj in array)
			{
				isReadies.Add(IsReady.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return isReadies;
		}

		public static Dictionary<string, IsReady> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, IsReady> strs = new Dictionary<string, IsReady>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				IsReady isReady = IsReady.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, isReady);
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