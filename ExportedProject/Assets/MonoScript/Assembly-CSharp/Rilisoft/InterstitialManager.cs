using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class InterstitialManager
	{
		private static readonly Lazy<InterstitialManager> _instance;

		private int _interstitialProviderIndex;

		[CompilerGenerated]
		private static Func<InterstitialManager> _003C_003Ef__am_0024cache2;

		public static InterstitialManager Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public AdProvider Provider
		{
			get
			{
				return GetProviderByIndex(_interstitialProviderIndex);
			}
		}

		public int ProviderClampedIndex
		{
			get
			{
				if (PromoActionsManager.MobileAdvert == null)
				{
					return -1;
				}
				if (PromoActionsManager.MobileAdvert.InterstitialProviders.Count == 0)
				{
					return -1;
				}
				return _interstitialProviderIndex % PromoActionsManager.MobileAdvert.InterstitialProviders.Count;
			}
		}

		private InterstitialManager()
		{
		}

		static InterstitialManager()
		{
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003C_instance_003Em__2DD;
			}
			_instance = new Lazy<InterstitialManager>(_003C_003Ef__am_0024cache2);
		}

		public AdProvider GetProviderByIndex(int index)
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return AdProvider.None;
			}
			if (PromoActionsManager.MobileAdvert.InterstitialProviders.Count == 0)
			{
				return AdProvider.None;
			}
			return (AdProvider)PromoActionsManager.MobileAdvert.InterstitialProviders[index % PromoActionsManager.MobileAdvert.InterstitialProviders.Count];
		}

		internal int SwitchAdProvider()
		{
			int interstitialProviderIndex = _interstitialProviderIndex;
			AdProvider provider = Provider;
			_interstitialProviderIndex++;
			if (provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyImageInterstitial();
			}
			if (Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.SwitchImageIdGroup();
			}
			if (Defs.IsDeveloperBuild)
			{
				string message = string.Format("Switching interstitial provider from {0} ({1}) to {2} ({3})", interstitialProviderIndex, provider, _interstitialProviderIndex, Provider);
				Debug.Log(message);
			}
			return _interstitialProviderIndex;
		}

		internal void ResetAdProvider()
		{
			int interstitialProviderIndex = _interstitialProviderIndex;
			AdProvider provider = Provider;
			_interstitialProviderIndex = 0;
			if (provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyImageInterstitial();
			}
			if (Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.ResetImageAdUnitId();
			}
			if (Defs.IsDeveloperBuild)
			{
				string message = string.Format("Resetting image interstitial provider from {0} ({1}) to {2} ({3})", interstitialProviderIndex, provider, _interstitialProviderIndex, Provider);
				Debug.Log(message);
			}
		}

		[CompilerGenerated]
		private static InterstitialManager _003C_instance_003Em__2DD()
		{
			return new InterstitialManager();
		}
	}
}
