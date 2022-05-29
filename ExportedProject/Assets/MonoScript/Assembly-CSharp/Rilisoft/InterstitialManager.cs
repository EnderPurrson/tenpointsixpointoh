using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class InterstitialManager
	{
		private readonly static Lazy<InterstitialManager> _instance;

		private int _interstitialProviderIndex;

		public static InterstitialManager Instance
		{
			get
			{
				return InterstitialManager._instance.Value;
			}
		}

		public AdProvider Provider
		{
			get
			{
				return this.GetProviderByIndex(this._interstitialProviderIndex);
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
				return this._interstitialProviderIndex % PromoActionsManager.MobileAdvert.InterstitialProviders.Count;
			}
		}

		static InterstitialManager()
		{
			InterstitialManager._instance = new Lazy<InterstitialManager>(() => new InterstitialManager());
		}

		private InterstitialManager()
		{
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

		internal void ResetAdProvider()
		{
			int num = this._interstitialProviderIndex;
			AdProvider provider = this.Provider;
			this._interstitialProviderIndex = 0;
			if (provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyImageInterstitial();
			}
			if (this.Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.ResetImageAdUnitId();
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Format("Resetting image interstitial provider from {0} ({1}) to {2} ({3})", new object[] { num, provider, this._interstitialProviderIndex, this.Provider }));
			}
		}

		internal int SwitchAdProvider()
		{
			int num = this._interstitialProviderIndex;
			AdProvider provider = this.Provider;
			this._interstitialProviderIndex++;
			if (provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyImageInterstitial();
			}
			if (this.Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.SwitchImageIdGroup();
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Format("Switching interstitial provider from {0} ({1}) to {2} ({3})", new object[] { num, provider, this._interstitialProviderIndex, this.Provider }));
			}
			return this._interstitialProviderIndex;
		}
	}
}