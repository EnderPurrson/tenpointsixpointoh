using System;
using System.Runtime.CompilerServices;
using Rilisoft;

internal sealed class AnalyticsHelper
{
	public const string AdStatisticsTotalEventName = "ADS Statistics Total";

	private AdvertisementInfo _advertisementContext = AdvertisementInfo.Default;

	private static readonly Lazy<AnalyticsHelper> _instance;

	[CompilerGenerated]
	private static Func<AnalyticsHelper> _003C_003Ef__am_0024cache2;

	public static AnalyticsHelper Instance
	{
		get
		{
			return _instance.Value;
		}
	}

	public AdvertisementInfo AdvertisementContext
	{
		get
		{
			return _advertisementContext;
		}
		set
		{
			_advertisementContext = value ?? AdvertisementInfo.Default;
		}
	}

	static AnalyticsHelper()
	{
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = _003C_instance_003Em__240;
		}
		_instance = new Lazy<AnalyticsHelper>(_003C_003Ef__am_0024cache2);
	}

	public static string GetAdProviderName(AdProvider provider)
	{
		return (provider != AdProvider.GoogleMobileAds) ? provider.ToString() : "AdMob";
	}

	[CompilerGenerated]
	private static AnalyticsHelper _003C_instance_003Em__240()
	{
		return new AnalyticsHelper();
	}
}
