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

	[CompilerGenerated]
	private static Comparison<CupHUD> _003C_003Ef__am_0024cache7;

	private void OnEnable()
	{
		float timeDelay = ((ExperienceController.sharedController.currentLevel != 2) ? ((!isTir) ? timeDelayLevelSec : timeDelayTirSec) : timeDelayLev2Sec);
		foreach (CupHUD item in arrCup)
		{
			item.gameObject.SetActive(false);
		}
		ActivateCurCupAnimation(timeDelay);
	}

	private void ActivateCurCupAnimation(float timeDelay = 0f)
	{
		int curOrderCup = ProfileController.CurOrderCup;
		if (arrCup.Count > curOrderCup)
		{
			arrCup[curOrderCup].AnimateCup(ExperienceController.sharedController.currentLevel, timeDelay, timeForFull);
		}
	}

	[ContextMenu("add all cup")]
	private void AddAllCup()
	{
		arrCup.Clear();
		arrCup.AddRange(GetComponentsInChildren<CupHUD>(true));
		List<CupHUD> list = arrCup;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CAddAllCup_003Em__394;
		}
		list.Sort(_003C_003Ef__am_0024cache7);
	}

	[ContextMenu("Reanimate")]
	private void Hrenanimate()
	{
		ActivateCurCupAnimation(0f);
	}

	[CompilerGenerated]
	private static int _003CAddAllCup_003Em__394(CupHUD x, CupHUD y)
	{
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
	}
}
