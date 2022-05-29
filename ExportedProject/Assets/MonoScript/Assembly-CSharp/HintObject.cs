using System;
using UnityEngine;

internal class HintObject : MonoBehaviour
{
	public UILabel label;

	public Transform arrow;

	private HintController.HintItem myHint;

	private bool indicOn;

	private float lastIndic;

	public HintObject()
	{
	}

	public void Hide()
	{
		if (this.myHint.indicateTarget)
		{
			if (this.myHint.targetSprites == null || (int)this.myHint.targetSprites.Length == 0)
			{
				this.myHint.targetSprite.spriteName = this.myHint.defaultSpriteName;
			}
			else
			{
				for (int i = 0; i < (int)this.myHint.targetSprites.Length; i++)
				{
					this.myHint.targetSprites[i].color = Color.white;
				}
			}
		}
		this.myHint = null;
		base.gameObject.SetActive(false);
	}

	public void Show(HintController.HintItem hint)
	{
		base.gameObject.SetActive(!hint.showLabelByCode);
		base.transform.parent = hint.target.transform;
		base.transform.localPosition = hint.relativeHintPosition;
		this.label.text = LocalizationStore.Get(hint.hintText);
		if (!hint.manualRotateArrow)
		{
			this.arrow.localRotation = Quaternion.identity;
		}
		else
		{
			this.arrow.localRotation = Quaternion.Euler(hint.manualArrowRotation);
		}
		this.label.transform.localPosition = hint.relativeLabelPosition;
		if (this.label.transform.localPosition.x <= 0f)
		{
			this.arrow.localScale = Vector3.one;
		}
		else
		{
			this.arrow.localScale = new Vector3(-1f, 1f, 1f);
		}
		this.myHint = hint;
		this.lastIndic = Time.time;
		if (!hint.scaleTween)
		{
			base.transform.localScale = Vector3.one;
		}
		else
		{
			base.transform.localScale = Vector3.one * 0.3f;
		}
	}

	private void Update()
	{
		if (this.myHint.indicateTarget && this.lastIndic < Time.time)
		{
			this.lastIndic = Time.time + 0.5f;
			this.indicOn = !this.indicOn;
			if (this.myHint.targetSprites == null || (int)this.myHint.targetSprites.Length == 0)
			{
				this.myHint.targetSprite.spriteName = (!this.indicOn ? this.myHint.defaultSpriteName : this.myHint.indicatedSpriteName);
			}
			else
			{
				for (int i = 0; i < (int)this.myHint.targetSprites.Length; i++)
				{
					this.myHint.targetSprites[i].color = (!this.indicOn ? Color.white : Color.green);
				}
			}
		}
		if (this.myHint.scaleTween)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), 3f * Time.unscaledDeltaTime);
		}
	}
}