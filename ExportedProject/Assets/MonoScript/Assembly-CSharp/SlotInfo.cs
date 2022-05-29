using Rilisoft;
using System;
using UnityEngine;

[Serializable]
public class SlotInfo
{
	public GiftInfo gift;

	public int positionInScroll;

	public float percentGetSlot;

	public GiftCategory category;

	public bool NoDropped;

	[HideInInspector]
	public bool isActiveEvent;

	private SaltedInt _countGift = new SaltedInt(15645675, 0);

	[HideInInspector]
	public int numInScroll;

	public int CountGift
	{
		get
		{
			if (this.isActiveEvent)
			{
				return this._countGift.Value;
			}
			return this.gift.Count.Value;
		}
		set
		{
			this._countGift.Value = value;
		}
	}

	public SlotInfo()
	{
	}

	public bool CheckAvaliableGift()
	{
		if ((!(GiftController.Instance != null) || this.gift != null) && this.category != null && this.category.AvailableGift(this.gift.Id, this.category.Type))
		{
			return false;
		}
		GiftController.Instance.UpdateSlot(this);
		return true;
	}
}