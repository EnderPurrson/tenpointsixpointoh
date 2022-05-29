using Rilisoft;
using System;
using System.Runtime.CompilerServices;

internal sealed class AnalyticsHelper
{
	public const string AdStatisticsTotalEventName = "ADS Statistics Total";

	private AdvertisementInfo _advertisementContext = AdvertisementInfo.Default;

	private readonly static Lazy<AnalyticsHelper> _instance;

	public AdvertisementInfo AdvertisementContext
	{
		get
		{
			return this._advertisementContext;
		}
		set
		{
			this._advertisementContext = value ?? AdvertisementInfo.Default;
		}
	}

	public static AnalyticsHelper Instance
	{
		get
		{
			return AnalyticsHelper._instance.Value;
		}
	}

	static AnalyticsHelper()
	{
		AnalyticsHelper._instance = new Lazy<AnalyticsHelper>(() => new AnalyticsHelper());
	}

	public AnalyticsHelper()
	{
	}

	public static string GetAdProviderName(AdProvider provider)
	{
		return (provider != AdProvider.GoogleMobileAds ? provider.ToString() : "AdMob");
	}
}