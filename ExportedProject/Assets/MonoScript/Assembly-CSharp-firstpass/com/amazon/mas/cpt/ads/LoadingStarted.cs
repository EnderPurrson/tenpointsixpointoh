using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class LoadingStarted : Jsonable
	{
		private static AmazonLogger logger;

		public bool BooleanValue
		{
			get;
			set;
		}

		static LoadingStarted()
		{
			LoadingStarted.logger = new AmazonLogger("Pi");
		}

		public LoadingStarted()
		{
		}

		public static LoadingStarted CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			LoadingStarted loadingStarted;
			try
			{
				if (jsonMap != null)
				{
					LoadingStarted item = new LoadingStarted();
					if (jsonMap.ContainsKey("booleanValue"))
					{
						item.BooleanValue = (bool)jsonMap["booleanValue"];
					}
					loadingStarted = item;
				}
				else
				{
					loadingStarted = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return loadingStarted;
		}

		public static LoadingStarted CreateFromJson(string jsonMessage)
		{
			LoadingStarted loadingStarted;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				loadingStarted = LoadingStarted.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return loadingStarted;
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

		public static List<LoadingStarted> ListFromJson(List<object> array)
		{
			List<LoadingStarted> loadingStarteds = new List<LoadingStarted>();
			foreach (object obj in array)
			{
				loadingStarteds.Add(LoadingStarted.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return loadingStarteds;
		}

		public static Dictionary<string, LoadingStarted> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, LoadingStarted> strs = new Dictionary<string, LoadingStarted>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				LoadingStarted loadingStarted = LoadingStarted.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, loadingStarted);
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