using System;
using UnityEngine;

public class SetHeadLabelText : MonoBehaviour
{
	public UILabel[] labels;

	public SetHeadLabelText()
	{
	}

	public void SetText(string text)
	{
		for (int i = 0; i < (int)this.labels.Length; i++)
		{
			this.labels[i].text = text;
		}
	}
}