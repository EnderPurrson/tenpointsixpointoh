using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FyberPlugin;
using Rilisoft;
using UnityEngine;

internal sealed class FyberFacade
{
	private readonly LinkedList<System.Threading.Tasks.Task<Ad>> _requests = new LinkedList<System.Threading.Tasks.Task<Ad>>();

	private static readonly Lazy<FyberFacade> _instance = new Lazy<FyberFacade>(() => new FyberFacade());

	public static FyberFacade Instance
	{
		get
		{
			return _instance.Value;
		}
	}

	public LinkedList<System.Threading.Tasks.Task<Ad>> Requests
	{
		get
		{
			return _requests;
		}
	}

	private FyberFacade()
	{
	}

	public System.Threading.Tasks.Task<Ad> RequestImageInterstitial(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		TaskCompletionSource<Ad> promise = new TaskCompletionSource<Ad>();
		return RequestImageInterstitialCore(promise, callerName);
	}

	private System.Threading.Tasks.Task<Ad> RequestImageInterstitialCore(TaskCompletionSource<Ad> promise, string callerName)
	{
		Action<Ad> onAdAvailable = null;
		Action<AdFormat> onAdNotAvailable = null;
		Action<RequestError> onRequestFail = null;
		onAdAvailable = delegate(Ad ad)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdAvailable: {{ format: {0}, placementId: '{1}' }}", ad.get_AdFormat(), ad.get_PlacementId());
			}
			promise.SetResult(ad);
			FyberCallback.remove_AdAvailable(onAdAvailable);
			FyberCallback.remove_AdNotAvailable(onAdNotAvailable);
			FyberCallback.remove_RequestFail(onRequestFail);
		};
		onAdNotAvailable = delegate(AdFormat adFormat)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdNotAvailable: {{ format: {0} }}", adFormat);
			}
			AdNotAwailableException exception2 = new AdNotAwailableException("Ad not available: " + adFormat);
			promise.SetException((Exception)exception2);
			FyberCallback.remove_AdAvailable(onAdAvailable);
			FyberCallback.remove_AdNotAvailable(onAdNotAvailable);
			FyberCallback.remove_RequestFail(onRequestFail);
		};
		onRequestFail = delegate(RequestError requestError)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > RequestFail: {{ requestError: {0} }}", requestError.get_Description());
			}
			AdRequestException exception = new AdRequestException(requestError.get_Description());
			promise.SetException((Exception)exception);
			FyberCallback.remove_AdAvailable(onAdAvailable);
			FyberCallback.remove_AdNotAvailable(onAdNotAvailable);
			FyberCallback.remove_RequestFail(onRequestFail);
		};
		FyberCallback.add_AdAvailable(onAdAvailable);
		FyberCallback.add_AdNotAvailable(onAdNotAvailable);
		FyberCallback.add_RequestFail(onRequestFail);
		RequestInterstitialAds(callerName);
		if (Application.isEditor)
		{
			promise.SetException((Exception)new NotSupportedException("Ads are not supported in Editor."));
		}
		return promise.get_Task();
	}

	public System.Threading.Tasks.Task<AdResult> ShowInterstitial(Dictionary<string, string> parameters, string callerName = null)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, string>();
		}
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		Debug.LogFormat("[Rilisoft] ShowInterstitial('{0}')", callerName);
		if (Requests.get_Count() == 0)
		{
			Debug.LogWarning("[Rilisoft]No active requests.");
			TaskCompletionSource<AdResult> val = new TaskCompletionSource<AdResult>();
			val.SetException((Exception)new InvalidOperationException("No active requests."));
			return val.get_Task();
		}
		Debug.LogWarning("[Rilisoft] Active requests count: " + Requests.get_Count());
		LinkedListNode<System.Threading.Tasks.Task<Ad>> requestNode = null;
		for (LinkedListNode<System.Threading.Tasks.Task<Ad>> val2 = Requests.get_Last(); val2 != null; val2 = val2.get_Previous())
		{
			if (!((System.Threading.Tasks.Task)val2.get_Value()).get_IsFaulted())
			{
				if (((System.Threading.Tasks.Task)val2.get_Value()).get_IsCompleted())
				{
					requestNode = val2;
					break;
				}
				if (requestNode == null)
				{
					requestNode = val2;
				}
			}
		}
		if (requestNode == null)
		{
			string text = "All requests are faulted: " + Requests.get_Count();
			Debug.LogWarning("[Rilisoft]" + text);
			TaskCompletionSource<AdResult> val3 = new TaskCompletionSource<AdResult>();
			val3.SetException((Exception)new InvalidOperationException(text));
			return val3.get_Task();
		}
		TaskCompletionSource<AdResult> showPromise = new TaskCompletionSource<AdResult>();
		Action<System.Threading.Tasks.Task<Ad>> action = delegate(System.Threading.Tasks.Task<Ad> requestFuture)
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			if (((System.Threading.Tasks.Task)requestFuture).get_IsFaulted())
			{
				string text2 = "Ad request failed: " + ((Exception)(object)((System.Threading.Tasks.Task)requestFuture).get_Exception()).InnerException.Message;
				Debug.LogWarningFormat("[Rilisoft] {0}", text2);
				showPromise.SetException((Exception)new AdRequestException(text2, ((Exception)(object)((System.Threading.Tasks.Task)requestFuture).get_Exception()).InnerException));
			}
			else
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("[Rilisoft] Ad request succeeded: {{ adFormat: {0}, placementId: '{1}' }}", requestFuture.get_Result().get_AdFormat(), requestFuture.get_Result().get_PlacementId());
				}
				Action<AdResult> adFinished = null;
				adFinished = delegate(AdResult adResult)
				{
					//IL_0038: Unknown result type (might be due to invalid IL or missing references)
					//IL_003e: Invalid comparison between Unknown and I4
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					Lazy<string> lazy = new Lazy<string>(() => string.Format("[Rilisoft] Ad show finished: {{ format: {0}, status: {1}, message: '{2}' }}", adResult.get_AdFormat(), adResult.get_Status(), adResult.get_Message()));
					if ((int)adResult.get_Status() == 1)
					{
						Debug.LogWarning(lazy.Value);
					}
					else if (Defs.IsDeveloperBuild)
					{
						Debug.Log(lazy.Value);
					}
					FyberCallback.remove_AdFinished(adFinished);
					showPromise.SetResult(adResult);
					if ((int)adResult.get_Status() == 0)
					{
						parameters["Fyber - Interstitial"] = "Impression: " + adResult.get_Message();
						FlurryPluginWrapper.LogEventAndDublicateToConsole("Ads Show Stats - Total", parameters);
					}
				};
				FyberCallback.add_AdFinished(adFinished);
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Start showing ad: {{ format: {0}, placementId: '{1}' }}", requestFuture.get_Result().get_AdFormat(), requestFuture.get_Result().get_PlacementId());
				}
				requestFuture.get_Result().Start();
				Requests.Remove(requestNode);
			}
		};
		if (((System.Threading.Tasks.Task)requestNode.get_Value()).get_IsCompleted())
		{
			action(requestNode.get_Value());
		}
		else
		{
			requestNode.get_Value().ContinueWith(action);
		}
		return showPromise.get_Task();
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
		((Requester<InterstitialRequester, RequestCallback>)(object)InterstitialRequester.Create()).Request();
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
}
