using System;
using UnityEngine;

public class ExpHUD : MonoBehaviour
{
	public UILabel lbCurLev;

	public UILabel lbExp;

	public UITexture txExp;

	public ExpHUD()
	{
	}

	private void OnDisable()
	{
		if (ExpController.Instance == null)
		{
			Debug.LogWarning("ExpController.Instance == null");
			return;
		}
		if (ExpController.Instance.experienceView == null)
		{
			Debug.LogWarning("experienceView == null");
			return;
		}
		ExpController.Instance.experienceView.VisibleHUD = true;
	}

	private void OnEnable()
	{
		ExpController.Instance.experienceView.VisibleHUD = false;
		this.UpdateHUD();
	}

	public void UpdateHUD()
	{
		this.lbCurLev.text = ExperienceController.sharedController.currentLevel.ToString();
		this.lbExp.text = ExpController.ExpToString();
		if (ExperienceController.sharedController.currentLevel != ExperienceController.maxLevel)
		{
			this.txExp.fillAmount = ExpController.progressExpInPer();
		}
		else
		{
			this.txExp.fillAmount = 1f;
		}
	}
}