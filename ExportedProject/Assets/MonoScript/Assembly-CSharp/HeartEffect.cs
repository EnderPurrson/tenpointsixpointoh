using System;
using UnityEngine;

public class HeartEffect : MonoBehaviour
{
	private UISprite mySprite;

	private bool activeEffect;

	private bool activeHeart;

	private Vector3 targetScale;

	private string spriteName;

	public int heartIndex;

	private readonly string[] mechShieldsSpriteName = new string[] { "mech_armor1", "mech_armor2", "mech_armor3", "mech_armor4", "mech_armor5", "mech_armor6" };

	private readonly string[] armSpriteName = new string[] { "wood_armor", "armor", "gold_armor", "crystal_armor", "red_armor", "adamant_armor", "adamant_armor" };

	public HeartEffect()
	{
	}

	public void Animate(int index, HeartEffect.IndicatorType type)
	{
		this.heartIndex = index;
		if (this.heartIndex >= 1)
		{
			if (this.heartIndex == 1)
			{
				this.ShowHide(true);
			}
			switch (type)
			{
				case HeartEffect.IndicatorType.Hearts:
				{
					this.spriteName = string.Concat("heart", this.heartIndex.ToString());
					break;
				}
				case HeartEffect.IndicatorType.Armor:
				{
					this.spriteName = this.armSpriteName[this.heartIndex - 1];
					break;
				}
				case HeartEffect.IndicatorType.Mech:
				{
					this.spriteName = this.mechShieldsSpriteName[this.heartIndex - 1];
					break;
				}
			}
			this.ChangeSpriteEffect(this.spriteName);
		}
		else
		{
			this.heartIndex = 0;
			this.ShowHide(false);
		}
	}

	private void Awake()
	{
		this.mySprite = base.GetComponent<UISprite>();
	}

	private void ChangeSpriteEffect(string newSprite)
	{
		this.spriteName = newSprite;
		this.activeEffect = true;
		this.activeHeart = true;
		this.targetScale = Vector3.one * 1.7f;
		base.gameObject.SetActive(true);
	}

	public void SetIndex(int index, HeartEffect.IndicatorType type)
	{
		this.heartIndex = index;
		this.activeEffect = false;
		this.targetScale = Vector3.one;
		base.transform.localScale = Vector3.one;
		if (this.heartIndex >= 1)
		{
			this.activeHeart = true;
			base.gameObject.SetActive(true);
			switch (type)
			{
				case HeartEffect.IndicatorType.Hearts:
				{
					this.mySprite.spriteName = string.Concat("heart", this.heartIndex.ToString());
					break;
				}
				case HeartEffect.IndicatorType.Armor:
				{
					this.mySprite.spriteName = this.armSpriteName[this.heartIndex - 1];
					break;
				}
				case HeartEffect.IndicatorType.Mech:
				{
					this.mySprite.spriteName = this.mechShieldsSpriteName[this.heartIndex - 1];
					break;
				}
			}
		}
		else
		{
			this.heartIndex = 0;
			base.gameObject.SetActive(false);
			this.activeHeart = false;
			switch (type)
			{
				case HeartEffect.IndicatorType.Hearts:
				{
					this.mySprite.spriteName = "heart1";
					break;
				}
				case HeartEffect.IndicatorType.Armor:
				{
					this.mySprite.spriteName = this.armSpriteName[0];
					break;
				}
				case HeartEffect.IndicatorType.Mech:
				{
					this.mySprite.spriteName = this.mechShieldsSpriteName[0];
					break;
				}
			}
		}
	}

	private void ShowHide(bool show)
	{
		this.activeHeart = show;
		this.activeEffect = true;
		if (!show)
		{
			this.targetScale = Vector3.one * 0.001f;
		}
		else
		{
			base.gameObject.SetActive(true);
			this.targetScale = Vector3.one;
		}
	}

	private void Update()
	{
		if (this.activeEffect)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.targetScale, 7f * Time.deltaTime);
			if (base.transform.localScale == this.targetScale)
			{
				if (!this.activeHeart || base.transform.localScale == Vector3.one)
				{
					this.activeEffect = false;
					base.gameObject.SetActive(this.activeHeart);
				}
				else
				{
					this.mySprite.spriteName = this.spriteName;
					this.targetScale = Vector3.one;
				}
			}
		}
	}

	public enum IndicatorType
	{
		Hearts,
		Armor,
		Mech
	}
}