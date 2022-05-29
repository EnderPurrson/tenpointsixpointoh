using Rilisoft;
using System;
using System.Runtime.CompilerServices;

public sealed class ItemPrice
{
	private const float CoefGemsToCoins = 1.7f;

	private readonly SaltedInt _price;

	private readonly static Random _prng;

	public string Currency
	{
		get;
		private set;
	}

	public int Price
	{
		get
		{
			return this._price.Value;
		}
	}

	static ItemPrice()
	{
		ItemPrice._prng = new Random(268898311);
	}

	public ItemPrice(int price, string currency)
	{
		this._price = new SaltedInt(ItemPrice._prng.Next(), price);
		this.Currency = currency;
	}

	public int CompareTo(ItemPrice p)
	{
		int num;
		if (p == null)
		{
			return 1;
		}
		if (this.Currency.Equals(p.Currency))
		{
			return this.Price - p.Price;
		}
		float single = 0f;
		single = (!this.Currency.Equals("Coins") ? (float)this.Price * 1.7f - (float)p.Price : (float)this.Price - (float)p.Price * 1.7f);
		if (single <= 0f)
		{
			num = (single >= 0f ? 0 : -1);
		}
		else
		{
			num = 1;
		}
		return num;
	}
}