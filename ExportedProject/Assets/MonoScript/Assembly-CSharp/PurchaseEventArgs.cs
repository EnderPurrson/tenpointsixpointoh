using System;
using System.Runtime.CompilerServices;

public sealed class PurchaseEventArgs : EventArgs
{
	public int Count
	{
		get;
		set;
	}

	public string Currency
	{
		get;
		private set;
	}

	public decimal CurrencyAmount
	{
		get;
		set;
	}

	public int Discount
	{
		get;
		private set;
	}

	public int Index
	{
		get;
		private set;
	}

	public PurchaseEventArgs(int index, int count, decimal currencyAmount, string currency = "Coins", int discount = 0)
	{
		this.Index = index;
		this.Count = count;
		this.CurrencyAmount = currencyAmount;
		this.Currency = currency;
		this.Discount = discount;
	}

	public PurchaseEventArgs(PurchaseEventArgs other)
	{
		if (other == null)
		{
			return;
		}
		this.Index = other.Index;
		this.Count = other.Count;
		this.CurrencyAmount = other.CurrencyAmount;
		this.Currency = other.Currency;
		this.Discount = other.Discount;
	}

	public override string ToString()
	{
		return string.Format("{{ Index: {0}, Count: {1}, CurrencyAmount: {2}, Currency: {3}, Discount: {4} }}", new object[] { this.Index, this.Count, this.CurrencyAmount, this.Currency, this.Discount });
	}
}