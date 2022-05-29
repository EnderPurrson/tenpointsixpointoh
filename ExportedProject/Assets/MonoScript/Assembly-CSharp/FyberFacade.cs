using FyberPlugin;
using Rilisoft;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class FyberFacade
{
	private readonly LinkedList<Task<Ad>> _requests = new LinkedList<Task<Ad>>();

	private readonly static Lazy<FyberFacade> _instance;

	public static FyberFacade Instance
	{
		get
		{
			return FyberFacade._instance.Value;
		}
	}

	public LinkedList<Task<Ad>> Requests
	{
		get
		{
			return this._requests;
		}
	}

	static FyberFacade()
	{
		FyberFacade._instance = new Lazy<FyberFacade>(() => new FyberFacade());
	}

	private FyberFacade()
	{
	}

	public Task<Ad> RequestImageInterstitial(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		return this.RequestImageInterstitialCore(new TaskCompletionSource<Ad>(), callerName);
	}

	private Task<Ad> RequestImageInterstitialCore(TaskCompletionSource<Ad> promise, string callerName)
	{
		Action<Ad> isDeveloperBuild = null;
		Action<AdFormat> action = null;
		Action<RequestError> isDeveloperBuild1 = null;
		isDeveloperBuild = (Ad ad) => {
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdAvailable: {{ format: {0}, placementId: '{1}' }}", new object[] { ad.AdFormat, ad.PlacementId });
			}
			this.promise.SetResult(ad);
			FyberCallback.AdAvailable -= this.onAdAvailable;
			FyberCallback.AdNotAvailable -= this.onAdNotAvailable;
			FyberCallback.RequestFail -= this.onRequestFail;
		};
		action = (AdFormat adFormat) => {
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdNotAvailable: {{ format: {0} }}", new object[] { adFormat });
			}
			AdNotAwailableException adNotAwailableException = new AdNotAwailableException(string.Concat("Ad not available: ", adFormat));
			this.promise.SetException(adNotAwailableException);
			FyberCallback.AdAvailable -= this.onAdAvailable;
			FyberCallback.AdNotAvailable -= this.onAdNotAvailable;
			FyberCallback.RequestFail -= this.onRequestFail;
		};
		isDeveloperBuild1 = (RequestError requestError) => {
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > RequestFail: {{ requestError: {0} }}", new object[] { requestError.Description });
			}
			AdRequestException adRequestException = new AdRequestException(requestError.Description);
			this.promise.SetException(adRequestException);
			FyberCallback.AdAvailable -= this.onAdAvailable;
			FyberCallback.AdNotAvailable -= this.onAdNotAvailable;
			FyberCallback.RequestFail -= this.onRequestFail;
		};
		FyberCallback.AdAvailable += isDeveloperBuild;
		FyberCallback.AdNotAvailable += action;
		FyberCallback.RequestFail += isDeveloperBuild1;
		FyberFacade.RequestInterstitialAds(callerName);
		if (Application.isEditor)
		{
			promise.SetException(new NotSupportedException("Ads are not supported in Editor."));
		}
		return promise.Task;
	}

	private static void RequestInterstitialAds(string callerName)
	{
		InterstitialRequester.Create().Request();
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("[Rilisoft] RequestInterstitialAds('{0}')", callerName));
		}
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Fyber - Interstitial", "Request" }
		};
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", strs, true);
	}

	public void SetUserPaying(string payingBin)
	{
		if (string.IsNullOrEmpty(payingBin))
		{
			payingBin = "0";
		}
		this.SetUserPayingCore(payingBin);
	}

	private void SetUserPayingCore(string payingBin)
	{
		User.PutCustomValue("pg3d_paying", payingBin);
	}

	public Task<AdResult> ShowInterstitial(Dictionary<string, string> parameters, string callerName = null)
	{
		Dictionary<string, string> strs = parameters ?? new Dictionary<string, string>();
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		Debug.LogFormat("[Rilisoft] ShowInterstitial('{0}')", new object[] { callerName });
		if (this.Requests.Count == 0)
		{
			Debug.LogWarning("[Rilisoft]No active requests.");
			TaskCompletionSource<AdResult> taskCompletionSource = new TaskCompletionSource<AdResult>();
			taskCompletionSource.SetException(new InvalidOperationException("No active requests."));
			return taskCompletionSource.Task;
		}
		Debug.LogWarning(string.Concat("[Rilisoft] Active requests count: ", this.Requests.Count));
		LinkedListNode<Task<Ad>> linkedListNode = null;
		for (LinkedListNode<Task<Ad>> i = this.Requests.Last; i != null; i = i.Previous)
		{
			if (!i.Value.IsFaulted)
			{
				if (i.Value.IsCompleted)
				{
					linkedListNode = i;
					break;
				}
				else if (linkedListNode == null)
				{
					linkedListNode = i;
				}
			}
		}
		if (linkedListNode == null)
		{
			string str1 = string.Concat("All requests are faulted: ", this.Requests.Count);
			Debug.LogWarning(string.Concat("[Rilisoft]", str1));
			TaskCompletionSource<AdResult> taskCompletionSource1 = new TaskCompletionSource<AdResult>();
			taskCompletionSource1.SetException(new InvalidOperationException(str1));
			return taskCompletionSource1.Task;
		}
		TaskCompletionSource<AdResult> taskCompletionSource2 = new TaskCompletionSource<AdResult>();
		Action<Task<Ad>> isFaulted = (Task<Ad> requestFuture) => {
			if (requestFuture.IsFaulted)
			{
				string str = string.Concat("Ad request failed: ", requestFuture.Exception.InnerException.Message);
				Debug.LogWarningFormat("[Rilisoft] {0}", new object[] { str });
				taskCompletionSource2.SetException(new AdRequestException(str, requestFuture.Exception.InnerException));
				return;
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] Ad request succeeded: {{ adFormat: {0}, placementId: '{1}' }}", new object[] { requestFuture.Result.AdFormat, requestFuture.Result.PlacementId });
			}
			Action<AdResult> action = null;
			action = (AdResult adResult) => {
				Lazy<string> lazy = new Lazy<string>(() => string.Format("[Rilisoft] Ad show finished: {{ format: {0}, status: {1}, message: '{2}' }}", adResult.AdFormat, adResult.Status, adResult.Message));
				if (adResult.Status == AdStatus.Error)
				{
					Debug.LogWarning(lazy.Value);
				}
				else if (Defs.IsDeveloperBuild)
				{
					Debug.Log(lazy.Value);
				}
				FyberCallback.AdFinished -= action;
				taskCompletionSource2.SetResult(adResult);
				if (adResult.Status == AdStatus.OK)
				{
					strs["Fyber - Interstitial"] = string.Concat("Impression: ", adResult.Message);
					FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", strs, true);
				}
			};
			FyberCallback.AdFinished += action;
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Start showing ad: {{ format: {0}, placementId: '{1}' }}", new object[] { requestFuture.Result.AdFormat, requestFuture.Result.PlacementId });
			}
			requestFuture.Result.Start();
			this.Requests.Remove(linkedListNode);
		};
		if (!linkedListNode.Value.IsCompleted)
		{
			linkedListNode.Value.ContinueWith(isFaulted);
		}
		else
		{
			isFaulted(linkedListNode.Value);
		}
		return taskCompletionSource2.Task;
	}

	public void UpdateUserPaying()
	{
		int num = Storager.getInt("PayingUser", true);
		this.SetUserPayingCore(num.ToString(CultureInfo.InvariantCulture));
	}
}