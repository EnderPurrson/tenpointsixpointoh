using System;
using UnityEngine;

internal sealed class BlinkReloadButton : MonoBehaviour
{
	public static bool isBlink;

	private bool isBlinkOld;

	private float timerBlink;

	public float maxTimerBlink = 0.5f;

	public Color blinkColor = new Color(1f, 0f, 0f);

	public Color unBlinkColor = new Color(1f, 1f, 1f);

	public static bool isBlinkState;

	public UISprite[] blinkObjs;

	public bool isBlinkTemp;

	static BlinkReloadButton()
	{
	}

	public BlinkReloadButton()
	{
	}

	private void Start()
	{
		BlinkReloadButton.isBlink = false;
		BlinkReloadButton.isBlinkState = false;
	}

	private void Update()
	{
		this.isBlinkTemp = BlinkReloadButton.isBlink;
		if (this.isBlinkOld != BlinkReloadButton.isBlink)
		{
			this.timerBlink = this.maxTimerBlink;
		}
		if (BlinkReloadButton.isBlink)
		{
			this.timerBlink -= Time.deltaTime;
			if (this.timerBlink < 0f)
			{
				this.timerBlink = this.maxTimerBlink;
				BlinkReloadButton.isBlinkState = !BlinkReloadButton.isBlinkState;
				for (int i = 0; i < (int)this.blinkObjs.Length; i++)
				{
					this.blinkObjs[i].color = (!BlinkReloadButton.isBlinkState ? this.unBlinkColor : this.blinkColor);
				}
			}
		}
		if (!BlinkReloadButton.isBlink && BlinkReloadButton.isBlinkState)
		{
			BlinkReloadButton.isBlinkState = !BlinkReloadButton.isBlinkState;
			for (int j = 0; j < (int)this.blinkObjs.Length; j++)
			{
				this.blinkObjs[j].color = (!BlinkReloadButton.isBlinkState ? this.unBlinkColor : this.blinkColor);
			}
		}
		this.isBlinkOld = BlinkReloadButton.isBlink;
	}
}