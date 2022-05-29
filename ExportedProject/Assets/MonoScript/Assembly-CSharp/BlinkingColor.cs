using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class BlinkingColor : MonoBehaviour
{
	private Material mainMaterial;

	public bool IsActive = true;

	public string nameColor = "_MainColor";

	public float speed = 1f;

	public Color normal;

	public Color blink;

	[HideInInspector]
	public Color curColor;

	private Color cashColor;

	private bool startBlink;

	public BlinkingColor()
	{
	}

	private void OnDestroy()
	{
		this.ResetColor();
	}

	private void ResetColor()
	{
		if (this.mainMaterial)
		{
			this.mainMaterial.SetColor(this.nameColor, this.cashColor);
		}
		this.startBlink = false;
		HOTween.Kill(this);
	}

	private void SetColorOne()
	{
		this.startBlink = true;
		HOTween.To(this, this.speed, (new TweenParms()).Prop("curColor", this.normal).Ease(EaseType.Linear).OnComplete(new TweenDelegate.TweenCallback(this.SetColorTwo)));
	}

	private void SetColorTwo()
	{
		this.startBlink = true;
		HOTween.To(this, this.speed, (new TweenParms()).Prop("curColor", this.blink).Ease(EaseType.Linear).OnComplete(new TweenDelegate.TweenCallback(this.SetColorOne)));
	}

	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component)
		{
			this.mainMaterial = component.sharedMaterial;
			if (this.mainMaterial)
			{
				this.cashColor = this.mainMaterial.GetColor(this.nameColor);
			}
		}
	}

	private void Update()
	{
		if (this.IsActive)
		{
			if (this.mainMaterial)
			{
				this.mainMaterial.SetColor(this.nameColor, this.curColor);
			}
			if (!this.startBlink)
			{
				this.SetColorTwo();
			}
		}
		else if (this.startBlink)
		{
			this.ResetColor();
		}
	}
}