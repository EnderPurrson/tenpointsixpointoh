using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FyberPlugin;
using Rilisoft;
using UnityEngine;

internal sealed class FyberFacade
{
	[CompilerGenerated]
	private sealed class _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB
	{
		internal TaskCompletionSource<Ad> promise;

		internal Action<Ad> onAdAvailable;

		internal Action<AdFormat> onAdNotAvailable;

		internal Action<RequestError> onRequestFail;

		internal void _003C_003Em__2B5(Ad ad)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdAvailable: {{ format: {0}, placementId: '{1}' }}", ad.AdFormat, ad.PlacementId);
			}
			promise.SetResult(ad);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		}

		internal void _003C_003Em__2B6(AdFormat adFormat)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdNotAvailable: {{ format: {0} }}", adFormat);
			}
			AdNotAwailableException exception = new AdNotAwailableException("Ad not available: " + adFormat);
			promise.SetException(exception);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		}

		internal void _003C_003Em__2B7(RequestError requestError)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > RequestFail: {{ requestError: {0} }}", requestError.Description);
			}
			AdRequestException exception = new AdRequestException(requestError.Description);
			promise.SetException(exception);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		}
	}

	[CompilerGenerated]
	private sealed class _003CShowInterstitial_003Ec__AnonStorey2AC
	{
		private sealed class _003CShowInterstitial_003Ec__AnonStorey2AE
		{
			private sealed class _003CShowInterstitial_003Ec__AnonStorey2AD
			{
				internal AdResult adResult;

				internal _003CShowInterstitial_003Ec__AnonStorey2AC _003C_003Ef__ref_0024684;

				internal _003CShowInterstitial_003Ec__AnonStorey2AE _003C_003Ef__ref_0024686;

				internal string _003C_003Em__2BA()
				{
					return string.Format("[Rilisoft] Ad show finished: {{ format: {0}, status: {1}, message: '{2}' }}", adResult.AdFormat, adResult.Status, adResult.Message);
				}
			}

			internal Action<AdResult> adFinished;

			internal _003CShowInterstitial_003Ec__AnonStorey2AC _003C_003Ef__ref_0024684;

			internal void _003C_003Em__2B9(AdResult adResult)
			{
				_003CShowInterstitial_003Ec__AnonStorey2AD _003CShowInterstitial_003Ec__AnonStorey2AD = new _003CShowInterstitial_003Ec__AnonStorey2AD();
				_003CShowInterstitial_003Ec__AnonStorey2AD._003C_003Ef__ref_0024684 = _003C_003Ef__ref_0024684;
				_003CShowInterstitial_003Ec__AnonStorey2AD._003C_003Ef__ref_0024686 = this;
				_003CShowInterstitial_003Ec__AnonStorey2AD.adResult = adResult;
				Lazy<string> lazy = new Lazy<string>(_003CShowInterstitial_003Ec__AnonStorey2AD._003C_003Em__2BA);
				if (_003CShowInterstitial_003Ec__AnonStorey2AD.adResult.Status == AdStatus.Error)
				{
					Debug.LogWarning(lazy.Value);
				}
				else if (Defs.IsDeveloperBuild)
				{
					Debug.Log(lazy.Value);
				}
				FyberCallback.AdFinished -= adFinished;
				_003C_003Ef__ref_0024684.showPromise.SetResult(_003CShowInterstitial_003Ec__AnonStorey2AD.adResult);
				if (_003CShowInterstitial_003Ec__AnonStorey2AD.adResult.Status == AdStatus.OK)
				{
					_003C_003Ef__ref_0024684.parameters["Fyber - Interstitial"] = "Impression: " + _003CShowInterstitial_003Ec__AnonStorey2AD.adResult.Message;
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", _003C_003Ef__ref_0024684.parameters);
				}
			}
		}

		internal TaskCompletionSource<AdResult> showPromise;

		internal Dictionary<string, string> parameters;

		internal LinkedListNode<Task<Ad>> requestNode;

		internal FyberFacade _003C_003Ef__this;

		internal void _003C_003Em__2B8(Task<Ad> requestFuture)
		{
			_003CShowInterstitial_003Ec__AnonStorey2AE _003CShowInterstitial_003Ec__AnonStorey2AE = new _003CShowInterstitial_003Ec__AnonStorey2AE();
			_003CShowInterstitial_003Ec__AnonStorey2AE._003C_003Ef__ref_0024684 = this;
			if (requestFuture.IsFaulted)
			{
				string text = "Ad request failed: " + requestFuture.Exception.InnerException.Message;
				Debug.LogWarningFormat("[Rilisoft] {0}", text);
				showPromise.SetException(new AdRequestException(text, requestFuture.Exception.InnerException));
				return;
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] Ad request succeeded: {{ adFormat: {0}, placementId: '{1}' }}", requestFuture.Result.AdFormat, requestFuture.Result.PlacementId);
			}
			_003CShowInterstitial_003Ec__AnonStorey2AE.adFinished = null;
			_003CShowInterstitial_003Ec__AnonStorey2AE.adFinished = _003CShowInterstitial_003Ec__AnonStorey2AE._003C_003Em__2B9;
			FyberCallback.AdFinished += _003CShowInterstitial_003Ec__AnonStorey2AE.adFinished;
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Start showing ad: {{ format: {0}, placementId: '{1}' }}", requestFuture.Result.AdFormat, requestFuture.Result.PlacementId);
			}
			requestFuture.Result.Start();
			_003C_003Ef__this.Requests.Remove(requestNode);
		}
	}

	private readonly LinkedList<Task<Ad>> _requests = new LinkedList<Task<Ad>>();

	private static readonly Lazy<FyberFacade> _instance;

	[CompilerGenerated]
	private static Func<FyberFacade> _003C_003Ef__am_0024cache2;

	public static FyberFacade Instance
	{
		get
		{
			return _instance.Value;
		}
	}

	public LinkedList<Task<Ad>> Requests
	{
		get
		{
			return _requests;
		}
	}

	private FyberFacade()
	{
	}

	static FyberFacade()
	{
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = _003C_instance_003Em__2B4;
		}
		_instance = new Lazy<FyberFacade>(_003C_003Ef__am_0024cache2);
	}

	public Task<Ad> RequestImageInterstitial(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		TaskCompletionSource<Ad> promise = new TaskCompletionSource<Ad>();
		return RequestImageInterstitialCore(promise, callerName);
	}

	private Task<Ad> RequestImageInterstitialCore(TaskCompletionSource<Ad> promise, string callerName)
	{
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB = new _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB();
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.promise = promise;
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onAdAvailable = null;
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onAdNotAvailable = null;
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onRequestFail = null;
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onAdAvailable = _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB._003C_003Em__2B5;
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onAdNotAvailable = _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB._003C_003Em__2B6;
		_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onRequestFail = _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB._003C_003Em__2B7;
		FyberCallback.AdAvailable += _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onAdAvailable;
		FyberCallback.AdNotAvailable += _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onAdNotAvailable;
		FyberCallback.RequestFail += _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.onRequestFail;
		RequestInterstitialAds(callerName);
		if (Application.isEditor)
		{
			_003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.promise.SetException(new NotSupportedException("Ads are not supported in Editor."));
		}
		return _003CRequestImageInterstitialCore_003Ec__AnonStorey2AB.promise.Task;
	}

	public Task<AdResult> ShowInterstitial(Dictionary<string, string> parameters, string callerName = null)
	{
		_003CShowInterstitial_003Ec__AnonStorey2AC _003CShowInterstitial_003Ec__AnonStorey2AC = new _003CShowInterstitial_003Ec__AnonStorey2AC();
		_003CShowInterstitial_003Ec__AnonStorey2AC.parameters = parameters;
		_003CShowInterstitial_003Ec__AnonStorey2AC._003C_003Ef__this = this;
		if (_003CShowInterstitial_003Ec__AnonStorey2AC.parameters == null)
		{
			_003CShowInterstitial_003Ec__AnonStorey2AC.parameters = new Dictionary<string, string>();
		}
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		Debug.LogFormat("[Rilisoft] ShowInterstitial('{0}')", callerName);
		if (Requests.Count == 0)
		{
			Debug.LogWarning("[Rilisoft]No active requests.");
			TaskCompletionSource<AdResult> taskCompletionSource = new TaskCompletionSource<AdResult>();
			taskCompletionSource.SetException(new InvalidOperationException("No active requests."));
			return taskCompletionSource.Task;
		}
		Debug.LogWarning("[Rilisoft] Active requests count: " + Requests.Count);
		_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode = null;
		for (LinkedListNode<Task<Ad>> linkedListNode = Requests.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
		{
			if (!linkedListNode.Value.IsFaulted)
			{
				if (linkedListNode.Value.IsCompleted)
				{
					_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode = linkedListNode;
					break;
				}
				if (_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode == null)
				{
					_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode = linkedListNode;
				}
			}
		}
		if (_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode == null)
		{
			string text = "All requests are faulted: " + Requests.Count;
			Debug.LogWarning("[Rilisoft]" + text);
			TaskCompletionSource<AdResult> taskCompletionSource2 = new TaskCompletionSource<AdResult>();
			taskCompletionSource2.SetException(new InvalidOperationException(text));
			return taskCompletionSource2.Task;
		}
		_003CShowInterstitial_003Ec__AnonStorey2AC.showPromise = new TaskCompletionSource<AdResult>();
		Action<Task<Ad>> action = _003CShowInterstitial_003Ec__AnonStorey2AC._003C_003Em__2B8;
		if (_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode.Value.IsCompleted)
		{
			action(_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode.Value);
		}
		else
		{
			_003CShowInterstitial_003Ec__AnonStorey2AC.requestNode.Value.ContinueWith(action);
		}
		return _003CShowInterstitial_003Ec__AnonStorey2AC.showPromise.Task;
	}

	public void SetUserPaying(string payingBin)
	{
		if (string.IsNullOrEmpty(payingBin))
		{
			payingBin = "0";
		}
		SetUserPayingCore(payingBin);
	}

	public void UpdateUserPaying()
	{
		string userPayingCore = Storager.getInt("PayingUser", true).ToString(CultureInfo.InvariantCulture);
		SetUserPayingCore(userPayingCore);
	}

	private void SetUserPayingCore(string payingBin)
	{
		User.PutCustomValue("pg3d_paying", payingBin);
	}

	private static void RequestInterstitialAds(string callerName)
	{
		InterstitialRequester.Create().Request();
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("[Rilisoft] RequestInterstitialAds('{0}')", callerName);
			Debug.Log(message);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Fyber - Interstitial", "Request");
		Dictionary<string, string> parameters = dictionary;
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", parameters);
	}

	[CompilerGenerated]
	private static FyberFacade _003C_instance_003Em__2B4()
	{
		return new FyberFacade();
	}
}
