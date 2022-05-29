using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class ApplicationKey : Jsonable
	{
		private static AmazonLogger logger;

		public string StringValue
		{
			get;
			set;
		}

		static ApplicationKey()
		{
			ApplicationKey.logger = new AmazonLogger("Pi");
		}

		public ApplicationKey()
		{
		}

		public static ApplicationKey CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			ApplicationKey applicationKey;
			try
			{
				if (jsonMap != null)
				{
					ApplicationKey item = new ApplicationKey();
					if (jsonMap.ContainsKey("stringValue"))
					{
						item.StringValue = (string)jsonMap["stringValue"];
					}
					applicationKey = item;
				}
				else
				{
					applicationKey = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return applicationKey;
		}

		public static ApplicationKey CreateFromJson(string jsonMessage)
		{
			ApplicationKey applicationKey;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				applicationKey = ApplicationKey.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return applicationKey;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				strs = new Dictionary<string, object>()
				{
					{ "stringValue", this.StringValue }
				};
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<ApplicationKey> ListFromJson(List<object> array)
		{
			List<ApplicationKey> applicationKeys = new List<ApplicationKey>();
			foreach (object obj in array)
			{
				applicationKeys.Add(ApplicationKey.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return applicationKeys;
		}

		public static Dictionary<string, ApplicationKey> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ApplicationKey> strs = new Dictionary<string, ApplicationKey>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				ApplicationKey applicationKey = ApplicationKey.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, applicationKey);
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