using System;
using System.Collections.Generic;

public class ChestBonusesData
{
	public int timeStart;

	public int duration;

	public List<ChestBonusData> bonuses;

	public ChestBonusesData()
	{
	}

	public void Clear()
	{
		if (this.bonuses == null)
		{
			return;
		}
		for (int i = 0; i < this.bonuses.Count; i++)
		{
			this.bonuses[i].items.Clear();
		}
		this.bonuses.Clear();
	}
}