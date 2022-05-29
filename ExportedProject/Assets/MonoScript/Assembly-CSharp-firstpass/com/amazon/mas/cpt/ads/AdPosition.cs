using com.amazon.mas.cpt.ads.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.mas.cpt.ads
{
	public sealed class AdPosition : Jsonable
	{
		private static AmazonLogger logger;

		public com.amazon.mas.cpt.ads.Ad Ad
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}

		public int XCoordinate
		{
			get;
			set;
		}

		public int YCoordinate
		{
			get;
			set;
		}

		static AdPosition()
		{
			AdPosition.logger = new AmazonLogger("Pi");
		}

		public AdPosition()
		{
		}

		public static AdPosition CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			AdPosition adPosition;
			try
			{
				if (jsonMap != null)
				{
					AdPosition num = new AdPosition();
					if (jsonMap.ContainsKey("ad"))
					{
						num.Ad = com.amazon.mas.cpt.ads.Ad.CreateFromDictionary(jsonMap["ad"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("xCoordinate"))
					{
						num.XCoordinate = Convert.ToInt32(jsonMap["xCoordinate"]);
					}
					if (jsonMap.ContainsKey("yCoordinate"))
					{
						num.YCoordinate = Convert.ToInt32(jsonMap["yCoordinate"]);
					}
					if (jsonMap.ContainsKey("width"))
					{
						num.Width = Convert.ToInt32(jsonMap["width"]);
					}
					if (jsonMap.ContainsKey("height"))
					{
						num.Height = Convert.ToInt32(jsonMap["height"]);
					}
					adPosition = num;
				}
				else
				{
					adPosition = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return adPosition;
		}

		public static AdPosition CreateFromJson(string jsonMessage)
		{
			AdPosition adPosition;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				adPosition = AdPosition.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return adPosition;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			object objectDictionary;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>();
				Dictionary<string, object> strs2 = strs1;
				if (this.Ad == null)
				{
					objectDictionary = null;
				}
				else
				{
					objectDictionary = this.Ad.GetObjectDictionary();
				}
				strs2.Add("ad", objectDictionary);
				strs1.Add("xCoordinate", this.XCoordinate);
				strs1.Add("yCoordinate", this.YCoordinate);
				strs1.Add("width", this.Width);
				strs1.Add("height", this.Height);
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<AdPosition> ListFromJson(List<object> array)
		{
			List<AdPosition> adPositions = new List<AdPosition>();
			foreach (object obj in array)
			{
				adPositions.Add(AdPosition.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return adPositions;
		}

		public static Dictionary<string, AdPosition> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AdPosition> strs = new Dictionary<string, AdPosition>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				AdPosition adPosition = AdPosition.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, adPosition);
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