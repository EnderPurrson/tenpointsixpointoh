using System;

namespace com.amazon.mas.cpt.ads
{
	public interface IAmazonMobileAds
	{
		void AddAdCollapsedListener(AdCollapsedDelegate responseDelegate);

		void AddAdDismissedListener(AdDismissedDelegate responseDelegate);

		void AddAdExpandedListener(AdExpandedDelegate responseDelegate);

		void AddAdFailedToLoadListener(AdFailedToLoadDelegate responseDelegate);

		void AddAdLoadedListener(AdLoadedDelegate responseDelegate);

		void AddAdResizedListener(AdResizedDelegate responseDelegate);

		IsEqual AreAdsEqual(AdPair adPair);

		void CloseFloatingBannerAd(Ad ad);

		Ad CreateFloatingBannerAd(Placement placement);

		Ad CreateInterstitialAd();

		void EnableGeoLocation(ShouldEnable shouldEnable);

		void EnableLogging(ShouldEnable shouldEnable);

		void EnableTesting(ShouldEnable shouldEnable);

		IsReady IsInterstitialAdReady();

		LoadingStarted LoadAndShowFloatingBannerAd(Ad ad);

		LoadingStarted LoadInterstitialAd();

		void RegisterApplication();

		void RemoveAdCollapsedListener(AdCollapsedDelegate responseDelegate);

		void RemoveAdDismissedListener(AdDismissedDelegate responseDelegate);

		void RemoveAdExpandedListener(AdExpandedDelegate responseDelegate);

		void RemoveAdFailedToLoadListener(AdFailedToLoadDelegate responseDelegate);

		void RemoveAdLoadedListener(AdLoadedDelegate responseDelegate);

		void RemoveAdResizedListener(AdResizedDelegate responseDelegate);

		void SetApplicationKey(ApplicationKey applicationKey);

		AdShown ShowInterstitialAd();

		void UnityFireEvent(string jsonMessage);
	}
}