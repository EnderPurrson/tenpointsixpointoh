using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ActiveCurrentCup : MonoBehaviour
{
	public bool isTir;

	[Header("Добавить в порядке активации")]
	public List<CupHUD> arrCup = new List<CupHUD>();

	public float timeForFull = 1.5f;

	public float timeDelayLevelSec = 2f;

	public float timeDelayTirSec = 2f;

	public float timeDelayLev2Sec = 4.5f;

	private int curOrder;

	public ActiveCurrentCup()
	{
	}

	private void ActivateCurCupAnimation(float timeDelay = 0)
	{
		int curOrderCup = ProfileController.CurOrderCup;
		if (this.arrCup.Count > curOrderCup)
		{
			this.arrCup[curOrderCup].AnimateCup(ExperienceController.sharedController.currentLevel, timeDelay, this.timeForFull);
		}
	}

	[ContextMenu("add all cup")]
	private void AddAllCup()
	{
		this.arrCup.Clear();
		this.arrCup.AddRange(base.GetComponentsInChildren<CupHUD>(true));
		this.arrCup.Sort((CupHUD x, CupHUD y) => {
			if (x.gameObject.name == null && y.gameObject.name == null)
			{
				return 0;
			}
			if (x.gameObject.name == null)
			{
				return -1;
			}
			if (y.gameObject.name == null)
			{
				return 1;
			}
			return x.gameObject.name.CompareTo(y.gameObject.name);
		});
	}

	[ContextMenu("Reanimate")]
	private void Hrenanimate()
	{
		this.ActivateCurCupAnimation(0f);
	}

	private void OnEnable()
	{
		float single;
		if (ExperienceController.sharedController.currentLevel != 2)
		{
			single = (!this.isTir ? this.timeDelayLevelSec : this.timeDelayTirSec);
		}
		else
		{
			single = this.timeDelayLev2Sec;
		}
		foreach (CupHUD cupHUD in this.arrCup)
		{
			cupHUD.gameObject.SetActive(false);
		}
		this.ActivateCurCupAnimation(single);
	}
}