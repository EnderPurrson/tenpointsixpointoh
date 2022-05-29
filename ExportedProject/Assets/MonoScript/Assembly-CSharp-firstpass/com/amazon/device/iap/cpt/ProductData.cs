using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.amazon.device.iap.cpt
{
	public sealed class ProductData : Jsonable
	{
		public string Description
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}

		public string ProductType
		{
			get;
			set;
		}

		public string Sku
		{
			get;
			set;
		}

		public string SmallIconUrl
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public ProductData()
		{
		}

		public static ProductData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			ProductData productDatum;
			try
			{
				if (jsonMap != null)
				{
					ProductData item = new ProductData();
					if (jsonMap.ContainsKey("sku"))
					{
						item.Sku = (string)jsonMap["sku"];
					}
					if (jsonMap.ContainsKey("productType"))
					{
						item.ProductType = (string)jsonMap["productType"];
					}
					if (jsonMap.ContainsKey("price"))
					{
						item.Price = (string)jsonMap["price"];
					}
					if (jsonMap.ContainsKey("title"))
					{
						item.Title = (string)jsonMap["title"];
					}
					if (jsonMap.ContainsKey("description"))
					{
						item.Description = (string)jsonMap["description"];
					}
					if (jsonMap.ContainsKey("smallIconUrl"))
					{
						item.SmallIconUrl = (string)jsonMap["smallIconUrl"];
					}
					productDatum = item;
				}
				else
				{
					productDatum = null;
				}
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", applicationException);
			}
			return productDatum;
		}

		public static ProductData CreateFromJson(string jsonMessage)
		{
			ProductData productDatum;
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(strs);
				productDatum = ProductData.CreateFromDictionary(strs);
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while UnJsoning", applicationException);
			}
			return productDatum;
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> strs;
			try
			{
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "sku", this.Sku },
					{ "productType", this.ProductType },
					{ "price", this.Price },
					{ "title", this.Title },
					{ "description", this.Description },
					{ "smallIconUrl", this.SmallIconUrl }
				};
				strs = strs1;
			}
			catch (ApplicationException applicationException)
			{
				throw new AmazonException("Error encountered while getting object dictionary", applicationException);
			}
			return strs;
		}

		public static List<ProductData> ListFromJson(List<object> array)
		{
			List<ProductData> productDatas = new List<ProductData>();
			foreach (object obj in array)
			{
				productDatas.Add(ProductData.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return productDatas;
		}

		public static Dictionary<string, ProductData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ProductData> strs = new Dictionary<string, ProductData>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				ProductData productDatum = ProductData.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				strs.Add(keyValuePair.Key, productDatum);
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