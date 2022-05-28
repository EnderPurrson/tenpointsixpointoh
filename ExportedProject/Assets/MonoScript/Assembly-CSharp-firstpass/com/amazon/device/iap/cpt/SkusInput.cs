using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkusInput : Jsonable
	{
		[CompilerGenerated]
		private static Func<object, string> _003C_003Ef__am_0024cache1;

		public List<string> Skus { get; set; }

		public string ToJson()
		{
			//Discarded unreachable code: IL_0013, IL_0025
			try
			{
				Dictionary<string, object> objectDictionary = GetObjectDictionary();
				return Json.Serialize(objectDictionary);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while Jsoning", inner);
			}
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			//Discarded unreachable code: IL_001e, IL_0030
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("skus", Skus);
				return dictionary;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
		}

		public static SkusInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			//Discarded unreachable code: IL_0067, IL_0079
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				SkusInput skusInput = new SkusInput();
				if (jsonMap.ContainsKey("skus"))
				{
					List<object> source = (List<object>)jsonMap["skus"];
					if (_003C_003Ef__am_0024cache1 == null)
					{
						_003C_003Ef__am_0024cache1 = _003CCreateFromDictionary_003Em__1;
					}
					skusInput.Skus = source.Select(_003C_003Ef__am_0024cache1).ToList();
				}
				return skusInput;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
		}

		public static SkusInput CreateFromJson(string jsonMessage)
		{
			//Discarded unreachable code: IL_001e, IL_0030
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
		}

		public static Dictionary<string, SkusInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkusInput> dictionary = new Dictionary<string, SkusInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				SkusInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<SkusInput> ListFromJson(List<object> array)
		{
			List<SkusInput> list = new List<SkusInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}

		[CompilerGenerated]
		private static string _003CCreateFromDictionary_003Em__1(object element)
		{
			return (string)element;
		}
	}
}
