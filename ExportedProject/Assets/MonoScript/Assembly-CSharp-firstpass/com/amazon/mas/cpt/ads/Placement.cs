using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class Placement : Jsonable
	{
		private static AmazonLogger logger;

		public com.amazon.mas.cpt.ads.AdFit AdFit
		{
			get;
			set;
		}

		public com.amazon.mas.cpt.ads.Dock Dock
		{
			get;
			set;
		}

		public com.amazon.mas.cpt.ads.HorizontalAlign HorizontalAlign
		{
			get;
			set;
		}

		static Placement()
		{
			Placement.logger = new AmazonLogger("Pi");
		}

		public Placement()
		{
		}

		public static Placement CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			Placement placement;
			try
			{
				if (jsonMap != null)
				{
					Placement placement1 = new Placement();
					if (jsonMap.ContainsKey("dock"))
					{
						placement1.Dock = (com.amazon.mas.cpt.ads.Dock)((int)Enum.Parse(typeof(com.amazon.mas.cpt.ads.Dock), (string)jsonMap["dock"]));
					}
					if (jsonMap.ContainsKey("horizontalAlign"))
					{
						placement1.HorizontalAlign = (com.amazon.mas.cpt.ads.HorizontalAlign)((int)Enum.Parse(typeof(com.amazon.mas.cpt.ads.HorizontalAlign), (string)jsonMap["horizontalAlign"]));
					}
					if (jsonMap.ContainsKey("adFit"))
					{
						placement1.AdFit = (com.amazon.mas.cpt.ads.AdFit)((int)Enum.Parse(typeof(com.amazon.mas.cpt.ads.AdFit), (string)jsonMap["adFit"]));
					}
					placement = placement1;
				}
				else
				{
					placement = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return placement;
		}

		public static Placement CreateFromJson(string jsonMessage)
		{
			Placement placement;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				placement = Placement.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return placement;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "dock", this.Dock },
					{ "horizontalAlign", this.HorizontalAlign },
					{ "adFit", this.AdFit }
				};
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<Placement> ListFromJson(List<object> array)
		{
			List<Placement> placements = new List<Placement>();
			foreach (object obj in array)
			{
				placements.Add(Placement.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return placements;
		}

		public static Dictionary<string, Placement> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, Placement> strs = new Dictionary<string, Placement>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				Placement placement = Placement.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, placement);
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