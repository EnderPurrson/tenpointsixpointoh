using System;
using UnityEngine;

public class ShopPositionParams : MonoBehaviour
{
	public int tier = 10;

	public int league = 1;

	public float scaleShop = 150f;

	public Vector3 positionShop;

	public Vector3 rotationShop;

	public string localizeKey;

	public int League
	{
		get
		{
			int num;
			if (!FriendsController.isUseRatingSystem)
			{
				num = (!base.name.Contains("league") ? 0 : 100000);
			}
			else
			{
				num = this.league - 1;
			}
			return num;
		}
	}

	public string shopName
	{
		get
		{
			return LocalizationStore.Get(this.localizeKey);
		}
	}

	public string shopNameNonLocalized
	{
		get
		{
			return LocalizationStore.GetByDefault(this.localizeKey);
		}
	}

	public ShopPositionParams()
	{
	}
}