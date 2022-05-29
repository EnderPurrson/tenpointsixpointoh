using System;
using UnityEngine;

[Obsolete]
public sealed class SkipTrainNOPresser : SkipTrainingButton
{
	public GameObject skipButton;

	public SkipTrainNOPresser()
	{
	}

	protected override void OnClick()
	{
		base.gameObject.transform.parent.gameObject.SetActive(false);
		this.skipButton.SetActive(true);
		base.OnClick();
		GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
		if (gameObject && gameObject != null)
		{
			Transform child = gameObject.transform.GetChild(0);
			if (child && child != null)
			{
				child.gameObject.SetActive(true);
			}
		}
		TrainingController.CancelSkipTraining();
	}
}