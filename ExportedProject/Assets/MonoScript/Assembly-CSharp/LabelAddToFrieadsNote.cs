using System;
using UnityEngine;

public class LabelAddToFrieadsNote : MonoBehaviour
{
	private bool isBigPorog;

	private bool isBigPorogOld;

	public LabelAddToFrieadsNote()
	{
	}

	private void Update()
	{
		this.isBigPorog = !Defs2.IsAvalibleAddFrends();
		if (this.isBigPorog != this.isBigPorogOld)
		{
			if (this.isBigPorog)
			{
				base.GetComponent<UILabel>().text = Defs.bigPorogString;
			}
			else
			{
				base.GetComponent<UILabel>().text = Defs.smallPorogString;
			}
		}
		this.isBigPorogOld = this.isBigPorog;
	}
}