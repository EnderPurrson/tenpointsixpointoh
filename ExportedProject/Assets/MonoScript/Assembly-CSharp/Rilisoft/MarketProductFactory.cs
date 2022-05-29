using com.amazon.device.iap.cpt;
using System;

namespace Rilisoft
{
	internal static class MarketProductFactory
	{
		internal static AmazonMarketProduct CreateAmazonMarketProduct(ProductData amazonItem)
		{
			return new AmazonMarketProduct(amazonItem);
		}

		internal static GoogleMarketProduct CreateGoogleMarketProduct(GoogleSkuInfo googleSkuInfo)
		{
			return new GoogleMarketProduct(googleSkuInfo);
		}
	}
}