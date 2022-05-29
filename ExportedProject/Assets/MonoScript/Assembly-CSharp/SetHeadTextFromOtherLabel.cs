using System;
using UnityEngine;

public class SetHeadTextFromOtherLabel : MonoBehaviour
{
	public UILabel otherLabel;

	public UILabel[] headLabels;

	public SetHeadTextFromOtherLabel()
	{
	}

	private void Update()
	{
		for (int i = 0; i < (int)this.headLabels.Length; i++)
		{
			this.headLabels[i].text = this.otherLabel.text;
		}
	}
}