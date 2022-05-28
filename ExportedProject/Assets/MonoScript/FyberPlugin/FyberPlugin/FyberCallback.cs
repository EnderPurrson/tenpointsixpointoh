using System;
using System.Threading;

namespace FyberPlugin
{
	public class FyberCallback : AdCallback, Callback, RequestCallback, VirtualCurrencyCallback, BannerAdCallback
	{
		private static FyberCallback instance;

		public static FyberCallback Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new FyberCallback();
				}
				return instance;
			}
		}

		public static event Action<Ad> AdAvailable
		{
			add
			{
				Action<Ad> val = FyberCallback.AdAvailable;
				Action<Ad> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<Ad>>(ref FyberCallback.AdAvailable, (Action<Ad>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<Ad> val = FyberCallback.AdAvailable;
				Action<Ad> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<Ad>>(ref FyberCallback.AdAvailable, (Action<Ad>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<AdFormat> AdNotAvailable
		{
			add
			{
				Action<AdFormat> val = FyberCallback.AdNotAvailable;
				Action<AdFormat> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<AdFormat>>(ref FyberCallback.AdNotAvailable, (Action<AdFormat>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<AdFormat> val = FyberCallback.AdNotAvailable;
				Action<AdFormat> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<AdFormat>>(ref FyberCallback.AdNotAvailable, (Action<AdFormat>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<VirtualCurrencyResponse> VirtualCurrencySuccess
		{
			add
			{
				Action<VirtualCurrencyResponse> val = FyberCallback.VirtualCurrencySuccess;
				Action<VirtualCurrencyResponse> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<VirtualCurrencyResponse>>(ref FyberCallback.VirtualCurrencySuccess, (Action<VirtualCurrencyResponse>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<VirtualCurrencyResponse> val = FyberCallback.VirtualCurrencySuccess;
				Action<VirtualCurrencyResponse> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<VirtualCurrencyResponse>>(ref FyberCallback.VirtualCurrencySuccess, (Action<VirtualCurrencyResponse>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<VirtualCurrencyErrorResponse> VirtualCurrencyError
		{
			add
			{
				Action<VirtualCurrencyErrorResponse> val = FyberCallback.VirtualCurrencyError;
				Action<VirtualCurrencyErrorResponse> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<VirtualCurrencyErrorResponse>>(ref FyberCallback.VirtualCurrencyError, (Action<VirtualCurrencyErrorResponse>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<VirtualCurrencyErrorResponse> val = FyberCallback.VirtualCurrencyError;
				Action<VirtualCurrencyErrorResponse> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<VirtualCurrencyErrorResponse>>(ref FyberCallback.VirtualCurrencyError, (Action<VirtualCurrencyErrorResponse>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<RequestError> RequestFail
		{
			add
			{
				Action<RequestError> val = FyberCallback.RequestFail;
				Action<RequestError> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<RequestError>>(ref FyberCallback.RequestFail, (Action<RequestError>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<RequestError> val = FyberCallback.RequestFail;
				Action<RequestError> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<RequestError>>(ref FyberCallback.RequestFail, (Action<RequestError>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<Ad> AdStarted
		{
			add
			{
				Action<Ad> val = FyberCallback.AdStarted;
				Action<Ad> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<Ad>>(ref FyberCallback.AdStarted, (Action<Ad>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<Ad> val = FyberCallback.AdStarted;
				Action<Ad> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<Ad>>(ref FyberCallback.AdStarted, (Action<Ad>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<AdResult> AdFinished
		{
			add
			{
				Action<AdResult> val = FyberCallback.AdFinished;
				Action<AdResult> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<AdResult>>(ref FyberCallback.AdFinished, (Action<AdResult>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<AdResult> val = FyberCallback.AdFinished;
				Action<AdResult> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<AdResult>>(ref FyberCallback.AdFinished, (Action<AdResult>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<string> NativeError
		{
			add
			{
				Action<string> val = FyberCallback.NativeError;
				Action<string> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<string>>(ref FyberCallback.NativeError, (Action<string>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<string> val = FyberCallback.NativeError;
				Action<string> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<string>>(ref FyberCallback.NativeError, (Action<string>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<BannerAd, string> BannerAdError
		{
			add
			{
				Action<BannerAd, string> val = FyberCallback.BannerAdError;
				Action<BannerAd, string> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd, string>>(ref FyberCallback.BannerAdError, (Action<BannerAd, string>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<BannerAd, string> val = FyberCallback.BannerAdError;
				Action<BannerAd, string> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd, string>>(ref FyberCallback.BannerAdError, (Action<BannerAd, string>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<BannerAd> BannerAdLoaded
		{
			add
			{
				Action<BannerAd> val = FyberCallback.BannerAdLoaded;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdLoaded, (Action<BannerAd>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<BannerAd> val = FyberCallback.BannerAdLoaded;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdLoaded, (Action<BannerAd>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<BannerAd> BannerAdClicked
		{
			add
			{
				Action<BannerAd> val = FyberCallback.BannerAdClicked;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdClicked, (Action<BannerAd>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<BannerAd> val = FyberCallback.BannerAdClicked;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdClicked, (Action<BannerAd>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<BannerAd> BannerAdLeftApplication
		{
			add
			{
				Action<BannerAd> val = FyberCallback.BannerAdLeftApplication;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdLeftApplication, (Action<BannerAd>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<BannerAd> val = FyberCallback.BannerAdLeftApplication;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdLeftApplication, (Action<BannerAd>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<BannerAd> BannerAdWillPresentModalView
		{
			add
			{
				Action<BannerAd> val = FyberCallback.BannerAdWillPresentModalView;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdWillPresentModalView, (Action<BannerAd>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<BannerAd> val = FyberCallback.BannerAdWillPresentModalView;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdWillPresentModalView, (Action<BannerAd>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		public static event Action<BannerAd> BannerAdDidDismissModalView
		{
			add
			{
				Action<BannerAd> val = FyberCallback.BannerAdDidDismissModalView;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdDidDismissModalView, (Action<BannerAd>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
			remove
			{
				Action<BannerAd> val = FyberCallback.BannerAdDidDismissModalView;
				Action<BannerAd> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<BannerAd>>(ref FyberCallback.BannerAdDidDismissModalView, (Action<BannerAd>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while ((global::System.Delegate)(object)val != (global::System.Delegate)(object)val2);
			}
		}

		internal FyberCallback()
		{
		}

		public void OnAdAvailable(Ad ad)
		{
			if (FyberCallback.AdAvailable != null)
			{
				FyberCallback.AdAvailable.Invoke(ad);
			}
		}

		public void OnAdNotAvailable(AdFormat adFormat)
		{
			if (FyberCallback.AdNotAvailable != null)
			{
				FyberCallback.AdNotAvailable.Invoke(adFormat);
			}
		}

		public void OnError(VirtualCurrencyErrorResponse response)
		{
			if (FyberCallback.VirtualCurrencyError != null)
			{
				FyberCallback.VirtualCurrencyError.Invoke(response);
			}
		}

		public void OnSuccess(VirtualCurrencyResponse response)
		{
			if (FyberCallback.VirtualCurrencySuccess != null)
			{
				FyberCallback.VirtualCurrencySuccess.Invoke(response);
			}
		}

		public void OnRequestError(RequestError error)
		{
			if (FyberCallback.RequestFail != null)
			{
				FyberCallback.RequestFail.Invoke(error);
			}
		}

		public void OnAdStarted(Ad ad)
		{
			if (FyberCallback.AdStarted != null)
			{
				FyberCallback.AdStarted.Invoke(ad);
			}
		}

		public void OnAdFinished(AdResult result)
		{
			if (FyberCallback.AdFinished != null)
			{
				FyberCallback.AdFinished.Invoke(result);
			}
		}

		public void OnNativeError(string message)
		{
			if (FyberCallback.NativeError != null)
			{
				FyberCallback.NativeError.Invoke(message);
			}
		}

		public void OnAdError(BannerAd ad, string error)
		{
			if (FyberCallback.BannerAdError != null)
			{
				FyberCallback.BannerAdError.Invoke(ad, error);
			}
		}

		public void OnAdLoaded(BannerAd ad)
		{
			if (FyberCallback.BannerAdLoaded != null)
			{
				FyberCallback.BannerAdLoaded.Invoke(ad);
			}
		}

		public void OnAdClicked(BannerAd ad)
		{
			if (FyberCallback.BannerAdClicked != null)
			{
				FyberCallback.BannerAdClicked.Invoke(ad);
			}
		}

		public void OnAdLeftApplication(BannerAd ad)
		{
			if (FyberCallback.BannerAdLeftApplication != null)
			{
				FyberCallback.BannerAdLeftApplication.Invoke(ad);
			}
		}

		public void OnAdWillPresentModalView(BannerAd ad)
		{
			if (FyberCallback.BannerAdWillPresentModalView != null)
			{
				FyberCallback.BannerAdWillPresentModalView.Invoke(ad);
			}
		}

		public void OnAdDidDismissModalView(BannerAd ad)
		{
			if (FyberCallback.BannerAdDidDismissModalView != null)
			{
				FyberCallback.BannerAdDidDismissModalView.Invoke(ad);
			}
		}
	}
}
