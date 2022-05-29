using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class ShouldEnable : Jsonable
	{
		private static AmazonLogger logger;

		public bool BooleanValue
		{
			get;
			set;
		}

		static ShouldEnable()
		{
			ShouldEnable.logger = new AmazonLogger("Pi");
		}

		public ShouldEnable()
		{
		}

		public static ShouldEnable CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			ShouldEnable shouldEnable;
			try
			{
				if (jsonMap != null)
				{
					ShouldEnable item = new ShouldEnable();
					if (jsonMap.ContainsKey("booleanValue"))
					{
						item.BooleanValue = (bool)jsonMap["booleanValue"];
					}
					shouldEnable = item;
				}
				else
				{
					shouldEnable = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return shouldEnable;
		}

		public static ShouldEnable CreateFromJson(string jsonMessage)
		{
			ShouldEnable shouldEnable;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				shouldEnable = ShouldEnable.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return shouldEnable;
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

		public static List<ShouldEnable> ListFromJson(List<object> array)
		{
			List<ShouldEnable> shouldEnables = new List<ShouldEnable>();
			foreach (object obj in array)
			{
				shouldEnables.Add(ShouldEnable.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return shouldEnables;
		}

		public static Dictionary<string, ShouldEnable> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ShouldEnable> strs = new Dictionary<string, ShouldEnable>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				ShouldEnable shouldEnable = ShouldEnable.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, shouldEnable);
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