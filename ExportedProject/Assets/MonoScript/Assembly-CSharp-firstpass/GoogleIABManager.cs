using Prime31;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GoogleIABManager : AbstractManager
{
	static GoogleIABManager()
	{
		AbstractManager.initialize(typeof(GoogleIABManager));
	}

	public GoogleIABManager()
	{
	}

	public void billingNotSupported(string error)
	{
		GoogleIABManager.billingNotSupportedEvent.fire<string>(error);
	}

	public void billingSupported(string empty)
	{
		GoogleIABManager.billingSupportedEvent.fire();
	}

	public void consumePurchaseFailed(string error)
	{
		GoogleIABManager.consumePurchaseFailedEvent.fire<string>(error);
	}

	public void consumePurchaseSucceeded(string json)
	{
		if (GoogleIABManager.consumePurchaseSucceededEvent != null)
		{
			GoogleIABManager.consumePurchaseSucceededEvent.fire<GooglePurchase>(new GooglePurchase(json.dictionaryFromJson()));
		}
	}

	public void purchaseCompleteAwaitingVerification(string json)
	{
		if (GoogleIABManager.purchaseCompleteAwaitingVerificationEvent != null)
		{
			Dictionary<string, object> strs = json.dictionaryFromJson();
			string str = strs["purchaseData"].ToString();
			string str1 = strs["signature"].ToString();
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent(str, str1);
		}
	}

	public void purchaseFailed(string json)
	{
		if (GoogleIABManager.purchaseFailedEvent != null)
		{
			Dictionary<string, object> strs = Json.decode<Dictionary<string, object>>(json, null);
			GoogleIABManager.purchaseFailedEvent(strs["result"].ToString(), int.Parse(strs["response"].ToString()));
		}
	}

	public void purchaseSucceeded(string json)
	{
		GoogleIABManager.purchaseSucceededEvent.fire<GooglePurchase>(new GooglePurchase(json.dictionaryFromJson()));
	}

	public void queryInventoryFailed(string error)
	{
		GoogleIABManager.queryInventoryFailedEvent.fire<string>(error);
	}

	public void queryInventorySucceeded(string json)
	{
		if (GoogleIABManager.queryInventorySucceededEvent != null)
		{
			Dictionary<string, object> strs = json.dictionaryFromJson();
			GoogleIABManager.queryInventorySucceededEvent(GooglePurchase.fromList(strs["purchases"] as List<object>), GoogleSkuInfo.fromList(strs["skus"] as List<object>));
		}
	}

	public static event Action<string> billingNotSupportedEvent;

	public static event Action billingSupportedEvent;

	public static event Action<string> consumePurchaseFailedEvent;

	public static event Action<GooglePurchase> consumePurchaseSucceededEvent;

	public static event Action<string, string> purchaseCompleteAwaitingVerificationEvent;

	public static event Action<string, int> purchaseFailedEvent;

	public static event Action<GooglePurchase> purchaseSucceededEvent;

	public static event Action<string> queryInventoryFailedEvent;

	public static event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent;
}