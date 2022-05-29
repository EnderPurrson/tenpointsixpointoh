using System;

internal sealed class AdvertisementInfo
{
	private readonly static AdvertisementInfo _default;

	private readonly int _round;

	private readonly int _slot;

	private readonly int _unit;

	private readonly string _details;

	public static AdvertisementInfo Default
	{
		get
		{
			return AdvertisementInfo._default;
		}
	}

	public string Details
	{
		get
		{
			return this._details;
		}
	}

	public int Round
	{
		get
		{
			return this._round;
		}
	}

	public int Slot
	{
		get
		{
			return this._slot;
		}
	}

	public int Unit
	{
		get
		{
			return this._unit;
		}
	}

	static AdvertisementInfo()
	{
		AdvertisementInfo._default = new AdvertisementInfo(-1, -1, -1, null);
	}

	public AdvertisementInfo(int round, int slot, int unit = 0, string details = null)
	{
		this._round = round;
		this._slot = slot;
		this._unit = unit;
		this._details = details ?? string.Empty;
	}
}