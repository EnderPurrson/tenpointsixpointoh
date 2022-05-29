using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StickersController : MonoBehaviour
{
	public static string ClassicSmileKey;

	public static string ChristmasSmileKey;

	public static string EasterSmileKey;

	static StickersController()
	{
		StickersController.ClassicSmileKey = "SmileKey";
		StickersController.ChristmasSmileKey = "ChristmasSmileKey";
		StickersController.EasterSmileKey = "EasterSmileKey";
	}

	public StickersController()
	{
	}

	public static void BuyStickersPack(TypePackSticker buyPack)
	{
		Storager.setInt(StickersController.KeyForBuyPack(buyPack), 1, true);
	}

	public static void EventPackBuy()
	{
		if (StickersController.onBuyPack != null)
		{
			StickersController.onBuyPack();
		}
	}

	public static List<TypePackSticker> GetAvaliablePack()
	{
		List<TypePackSticker> typePackStickers = new List<TypePackSticker>();
		IEnumerator enumerator = Enum.GetValues(typeof(TypePackSticker)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if ((int)current != 0)
				{
					if (!StickersController.IsBuyPack((TypePackSticker)((int)current)))
					{
						continue;
					}
					typePackStickers.Add((TypePackSticker)((int)current));
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return typePackStickers;
	}

	public static ItemPrice GetPricePack(TypePackSticker needPack)
	{
		return VirtualCurrencyHelper.Price(StickersController.KeyForBuyPack(needPack));
	}

	public static bool IsBuyAllPack()
	{
		bool flag;
		IEnumerator enumerator = Enum.GetValues(typeof(TypePackSticker)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if ((int)current != 0 && (int)current != 3)
				{
					if (StickersController.IsBuyPack((TypePackSticker)((int)current)))
					{
						continue;
					}
					flag = false;
					return flag;
				}
			}
			return true;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return flag;
	}

	public static bool IsBuyAnyPack()
	{
		bool flag;
		IEnumerator enumerator = Enum.GetValues(typeof(TypePackSticker)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if ((int)current != 0)
				{
					if (!StickersController.IsBuyPack((TypePackSticker)((int)current)))
					{
						continue;
					}
					flag = true;
					return flag;
				}
			}
			return false;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return flag;
	}

	public static bool IsBuyPack(TypePackSticker needPack)
	{
		return Storager.getInt(StickersController.KeyForBuyPack(needPack), true) == 1;
	}

	public static string KeyForBuyPack(TypePackSticker needPack)
	{
		switch (needPack)
		{
			case TypePackSticker.classic:
			{
				return StickersController.ClassicSmileKey;
			}
			case TypePackSticker.christmas:
			{
				return StickersController.ChristmasSmileKey;
			}
			case TypePackSticker.easter:
			{
				return StickersController.EasterSmileKey;
			}
		}
		return null;
	}

	public static event Action onBuyPack;
}