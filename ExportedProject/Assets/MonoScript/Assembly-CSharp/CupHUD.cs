using Holoville.HOTween;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class CupHUD : MonoBehaviour
{
	public float curProgress;

	public UITexture txCup;

	public UITexture txFillLine;

	public GameObject objSelectCup;

	private float sizePerFillLine = 0.02f;

	public CupHUD()
	{
	}

	public void AnimateCup(int animToLev, float delay = 0, float timeFull = 1.5f)
	{
		base.gameObject.SetActive(true);
		int curOrderCup = ProfileController.CurOrderCup;
		float perFillProgress = ProfileController.GetPerFillProgress(curOrderCup, animToLev - 1);
		float single = ProfileController.GetPerFillProgress(curOrderCup, animToLev);
		this.curProgress = perFillProgress;
		this.SetHUDProgress(perFillProgress);
		HOTween.To(this, timeFull, (new TweenParms()).Prop("curProgress", single).Delay(delay).Ease(EaseType.Linear).OnUpdate(() => this.UpdateCurProgress()));
	}

	private void Awake()
	{
		HOTween.Init();
	}

	[ContextMenu("Find need Tx")]
	private void FindNeedTX()
	{
		this.txCup = base.transform.FindChild("Cup_usual_Filled").GetComponent<UITexture>();
		this.txFillLine = base.transform.FindChild("Cup_line").GetComponent<UITexture>();
	}

	[ContextMenu("Find Select obj")]
	private void FindSelectObj()
	{
		this.objSelectCup = base.transform.FindChild("Select").gameObject;
	}

	private void SetHUDProgress(float val)
	{
		this.txCup.fillAmount = val;
		if (val >= this.sizePerFillLine)
		{
			this.txFillLine.fillAmount = val + this.sizePerFillLine;
		}
		else
		{
			this.txFillLine.fillAmount = 0f;
		}
	}

	[ContextMenu("TestAnimate")]
	private void TestAnimate()
	{
		this.AnimateCup(ExperienceController.sharedController.currentLevel, 0f, 1.5f);
	}

	public void UpdateByOrder(int order)
	{
		this.curProgress = ProfileController.GetPerFillProgress(order, ExperienceController.sharedController.currentLevel);
		this.SetHUDProgress(this.curProgress);
		if (this.objSelectCup)
		{
			if (ProfileController.CurOrderCup != order)
			{
				this.objSelectCup.SetActive(false);
			}
			else
			{
				this.objSelectCup.SetActive(true);
			}
		}
	}

	public void UpdateCurProgress()
	{
		this.SetHUDProgress(this.curProgress);
	}
}