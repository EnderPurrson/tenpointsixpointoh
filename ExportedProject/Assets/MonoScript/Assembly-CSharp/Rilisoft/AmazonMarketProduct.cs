using com.amazon.device.iap.cpt;
using System;

namespace Rilisoft
{
	internal sealed class AmazonMarketProduct : IMarketProduct
	{
		private readonly ProductData _marketProduct;

		public string Description
		{
			get
			{
				return this._marketProduct.Description;
			}
		}

		public string Id
		{
			get
			{
				return this._marketProduct.Sku;
			}
		}

		public object PlatformProduct
		{
			get
			{
				return this._marketProduct;
			}
		}

		public string Price
		{
			get
			{
				return this._marketProduct.Price;
			}
		}

		public string Title
		{
			get
			{
				return this._marketProduct.Title;
			}
		}

		public AmazonMarketProduct(ProductData amazonItem)
		{
			this._marketProduct = amazonItem;
		}

		public override bool Equals(object obj)
		{
			AmazonMarketProduct amazonMarketProduct = obj as AmazonMarketProduct;
			if (amazonMarketProduct == null)
			{
				return false;
			}
			ProductData productDatum = amazonMarketProduct._marketProduct;
			return this._marketProduct.Equals(productDatum);
		}

		public override int GetHashCode()
		{
			return this._marketProduct.GetHashCode();
		}

		public override string ToString()
		{
			return this._marketProduct.ToString();
		}
	}
}