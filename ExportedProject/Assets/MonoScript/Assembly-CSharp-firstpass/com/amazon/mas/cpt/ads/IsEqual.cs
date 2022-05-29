using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class IsEqual : Jsonable
	{
		private static AmazonLogger logger;

		public bool BooleanValue
		{
			get;
			set;
		}

		static IsEqual()
		{
			IsEqual.logger = new AmazonLogger("Pi");
		}

		public IsEqual()
		{
		}

		public static IsEqual CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			IsEqual isEqual;
			try
			{
				if (jsonMap != null)
				{
					IsEqual item = new IsEqual();
					if (jsonMap.ContainsKey("booleanValue"))
					{
						item.BooleanValue = (bool)jsonMap["booleanValue"];
					}
					isEqual = item;
				}
				else
				{
					isEqual = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return isEqual;
		}

		public static IsEqual CreateFromJson(string jsonMessage)
		{
			IsEqual isEqual;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				isEqual = IsEqual.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return isEqual;
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

		public static List<IsEqual> ListFromJson(List<object> array)
		{
			List<IsEqual> isEquals = new List<IsEqual>();
			foreach (object obj in array)
			{
				isEquals.Add(IsEqual.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return isEquals;
		}

		public static Dictionary<string, IsEqual> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, IsEqual> strs = new Dictionary<string, IsEqual>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				IsEqual isEqual = IsEqual.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, isEqual);
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