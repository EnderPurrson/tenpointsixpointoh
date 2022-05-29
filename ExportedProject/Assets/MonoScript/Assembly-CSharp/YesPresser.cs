using System;
using UnityEngine;

public sealed class YesPresser : SkipTrainingButton
{
	public GameObject noButton;

	private bool _clicked;

	public YesPresser()
	{
	}

	protected override void OnClick()
	{
		if (this._clicked)
		{
			return;
		}
		this.noButton.GetComponent<UIButton>().enabled = false;
		base.enabled = false;
		GotToNextLevel.GoToNextLevel();
		this._clicked = true;
	}
}