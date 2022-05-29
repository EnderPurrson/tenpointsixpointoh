using System;

namespace Rilisoft
{
	internal sealed class GoogleMarketProduct : IMarketProduct
	{
		private readonly GoogleSkuInfo _marketProduct;

		public string Description
		{
			get
			{
				return this._marketProduct.description;
			}
		}

		public string Id
		{
			get
			{
				return this._marketProduct.productId;
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
				return this._marketProduct.price;
			}
		}

		public string Title
		{
			get
			{
				return this._marketProduct.title;
			}
		}

		public GoogleMarketProduct(GoogleSkuInfo googleSkuInfo)
		{
			this._marketProduct = googleSkuInfo;
		}

		public override bool Equals(object obj)
		{
			GoogleMarketProduct googleMarketProduct = obj as GoogleMarketProduct;
			if (googleMarketProduct == null)
			{
				return false;
			}
			GoogleSkuInfo googleSkuInfo = googleMarketProduct._marketProduct;
			return this._marketProduct.Equals(googleSkuInfo);
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