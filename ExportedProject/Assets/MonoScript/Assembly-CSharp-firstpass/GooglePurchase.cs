using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GooglePurchase
{
	public string developerPayload
	{
		get;
		private set;
	}

	public string orderId
	{
		get;
		private set;
	}

	public string originalJson
	{
		get;
		private set;
	}

	public string packageName
	{
		get;
		private set;
	}

	public string productId
	{
		get;
		private set;
	}

	public GooglePurchase.GooglePurchaseState purchaseState
	{
		get;
		private set;
	}

	public long purchaseTime
	{
		get;
		private set;
	}

	public string purchaseToken
	{
		get;
		private set;
	}

	public string signature
	{
		get;
		private set;
	}

	public string type
	{
		get;
		private set;
	}

	public GooglePurchase(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("packageName"))
		{
			this.packageName = dict["packageName"].ToString();
		}
		if (dict.ContainsKey("orderId"))
		{
			this.orderId = dict["orderId"].ToString();
		}
		if (dict.ContainsKey("productId"))
		{
			this.productId = dict["productId"].ToString();
		}
		if (dict.ContainsKey("developerPayload"))
		{
			this.developerPayload = dict["developerPayload"].ToString();
		}
		if (dict.ContainsKey("type"))
		{
			this.type = dict["type"] as string;
		}
		if (dict.ContainsKey("purchaseTime"))
		{
			this.purchaseTime = long.Parse(dict["purchaseTime"].ToString());
		}
		if (dict.ContainsKey("purchaseState"))
		{
			this.purchaseState = (GooglePurchase.GooglePurchaseState)int.Parse(dict["purchaseState"].ToString());
		}
		if (dict.ContainsKey("purchaseToken"))
		{
			this.purchaseToken = dict["purchaseToken"].ToString();
		}
		if (dict.ContainsKey("signature"))
		{
			this.signature = dict["signature"].ToString();
		}
		if (dict.ContainsKey("originalJson"))
		{
			this.originalJson = dict["originalJson"].ToString();
		}
	}

	public static List<GooglePurchase> fromList(List<object> items)
	{
		List<GooglePurchase> googlePurchases = new List<GooglePurchase>();
		foreach (Dictionary<string, object> item in items)
		{
			googlePurchases.Add(new GooglePurchase(item));
		}
		return googlePurchases;
	}

	public override string ToString()
	{
		return string.Format("<GooglePurchase> packageName: {0}, orderId: {1}, productId: {2}, developerPayload: {3}, purchaseToken: {4}, purchaseState: {5}, signature: {6}, type: {7}, json: {8}", new object[] { this.packageName, this.orderId, this.productId, this.developerPayload, this.purchaseToken, this.purchaseState, this.signature, this.type, this.originalJson });
	}

	public enum GooglePurchaseState
	{
		Purchased,
		Canceled,
		Refunded
	}
}