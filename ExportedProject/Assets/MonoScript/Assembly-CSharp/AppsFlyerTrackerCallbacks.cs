using System;
using UnityEngine;

public class AppsFlyerTrackerCallbacks : MonoBehaviour
{
	public AppsFlyerTrackerCallbacks()
	{
	}

	public void didFinishValidateReceipt(string validateResult)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got didFinishValidateReceipt  = ", validateResult));
	}

	public void didFinishValidateReceiptWithError(string error)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got idFinishValidateReceiptWithError error = ", error));
	}

	public void didReceiveConversionData(string conversionData)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got conversion data = ", conversionData));
	}

	public void didReceiveConversionDataWithError(string error)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got conversion data error = ", error));
	}

	public void onAppOpenAttribution(string validateResult)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got onAppOpenAttribution  = ", validateResult));
	}

	public void onAppOpenAttributionFailure(string error)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got onAppOpenAttributionFailure error = ", error));
	}

	public void onInAppBillingFailure(string error)
	{
		MonoBehaviour.print(string.Concat("AppsFlyerTrackerCallbacks:: got onInAppBillingFailure error = ", error));
	}

	public void onInAppBillingSuccess()
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got onInAppBillingSuccess succcess");
	}

	private void Start()
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks on Start");
	}

	private void Update()
	{
	}
}