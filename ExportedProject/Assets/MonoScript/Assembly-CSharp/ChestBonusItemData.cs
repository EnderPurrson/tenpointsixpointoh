using System;

public class ChestBonusItemData
{
	public string tag;

	public int count;

	public int timeLife;

	public ChestBonusItemData()
	{
	}

	public string GetTimeLabel(bool isShort = false)
	{
		int num = this.timeLife / 24;
		if (num <= 0)
		{
			return string.Format("{0}h.", this.timeLife);
		}
		if (isShort)
		{
			return string.Format("{0}d.", num);
		}
		return string.Format("{0} {1}", LocalizationStore.Get("Key_1231"), num);
	}
}