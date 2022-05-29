using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoinsAddIndic : MonoBehaviour
{
	private const float blinkR = 255f;

	private const float blinkG = 255f;

	private const float blinkB = 0f;

	private const float blinkA = 115f;

	private const float blinkGemsR = 0f;

	private const float blinkGemsG = 0f;

	private const float blinkGemsB = 255f;

	private const float blinkGemsA = 115f;

	public bool stopBlinkingOnEnable;

	public bool isGems;

	public bool isX3;

	private UISprite ind;

	private bool blinking;

	public bool remembeState;

	private int backgroundAdd;

	private bool isSurvival;

	private Color BlinkColor
	{
		get
		{
			return (!this.isGems ? new Color(1f, 1f, 0f, 0.4509804f) : new Color(0f, 0f, 1f, 0.4509804f));
		}
	}

	public CoinsAddIndic()
	{
	}

	private void BackgroundEventAdd(bool gems, int count)
	{
		if (this.isGems != gems)
		{
			return;
		}
		if ((BankController.Instance == null || !BankController.Instance.InterfaceEnabled) && GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
		{
			if (gems && this.isGems)
			{
				this.backgroundAdd = 1;
			}
			if (!gems && !this.isGems)
			{
				this.backgroundAdd = 2;
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator blink()
	{
		CoinsAddIndic.u003cblinku003ec__Iterator17 variable = null;
		return variable;
	}

	private void IndicateCoinsAdd(bool gems, int count)
	{
		if (this.isGems == gems && !this.blinking)
		{
			base.StartCoroutine(this.blink());
		}
	}

	private Color NormalColor()
	{
		return (!this.isX3 ? new Color(0f, 0f, 0f, 0.4509804f) : new Color(1f, 0f, 0f, 0.5882353f));
	}

	private void OnDestroy()
	{
		if (this.remembeState)
		{
			CoinsMessage.CoinsLabelDisappeared -= new CoinsMessage.CoinsLabelDisappearedDelegate(this.BackgroundEventAdd);
		}
	}

	private void OnDisable()
	{
		CoinsMessage.CoinsLabelDisappeared -= new CoinsMessage.CoinsLabelDisappearedDelegate(this.IndicateCoinsAdd);
	}

	private void OnEnable()
	{
		CoinsMessage.CoinsLabelDisappeared += new CoinsMessage.CoinsLabelDisappearedDelegate(this.IndicateCoinsAdd);
		if (this.ind != null)
		{
			this.ind.color = this.NormalColor();
		}
		if (this.backgroundAdd > 0)
		{
			this.blinking = false;
			this.IndicateCoinsAdd((this.backgroundAdd != 1 ? false : true), 2);
			this.backgroundAdd = 0;
		}
		if (this.blinking && !this.stopBlinkingOnEnable)
		{
			base.StartCoroutine(this.blink());
		}
		else if (this.stopBlinkingOnEnable)
		{
			this.blinking = false;
		}
	}

	private void Start()
	{
		this.ind = base.GetComponent<UISprite>();
		this.isSurvival = Defs.IsSurvival;
		if (this.remembeState)
		{
			CoinsMessage.CoinsLabelDisappeared -= new CoinsMessage.CoinsLabelDisappearedDelegate(this.BackgroundEventAdd);
			CoinsMessage.CoinsLabelDisappeared += new CoinsMessage.CoinsLabelDisappearedDelegate(this.BackgroundEventAdd);
		}
	}
}