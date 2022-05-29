using System;
using System.Collections;
using UnityEngine;

public sealed class BlinkHealthButton : MonoBehaviour
{
	public BlinkHealthButton.RegimButton typeButton;

	public static bool isBlink;

	private bool isBlinkOld;

	public float timerBlink;

	public float maxTimerBlink = 0.5f;

	public Color blinkColor = new Color(1f, 0f, 0f);

	public Color unBlinkColor = new Color(1f, 1f, 1f);

	public bool isBlinkState;

	public UISprite[] blinkObjs;

	public bool isBlinkTemp;

	public UISprite shine;

	private Player_move_c player_move_c;

	private Color _blinkColorNoAlpha;

	static BlinkHealthButton()
	{
	}

	public BlinkHealthButton()
	{
	}

	private void Start()
	{
		BlinkHealthButton.isBlink = false;
		this.isBlinkState = false;
		this._blinkColorNoAlpha = new Color(this.blinkColor.r, this.blinkColor.g, this.blinkColor.b, 0f);
	}

	private void Update()
	{
		if (this.player_move_c == null)
		{
			if (!Defs.isMulti)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.player_move_c = gameObject.GetComponent<SkinName>().playerMoveC;
				}
			}
			else
			{
				this.player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		if (this.player_move_c == null)
		{
			return;
		}
		if (this.typeButton == BlinkHealthButton.RegimButton.Health)
		{
			if (this.player_move_c.CurHealth + this.player_move_c.curArmor >= 3f || this.player_move_c.isMechActive)
			{
				BlinkHealthButton.isBlink = false;
			}
			else
			{
				BlinkHealthButton.isBlink = true;
			}
		}
		if (this.typeButton == BlinkHealthButton.RegimButton.Ammo)
		{
			Weapon item = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			if (item.currentAmmoInClip != 0 || item.currentAmmoInBackpack != 0 || item.weaponPrefab.GetComponent<WeaponSounds>().isMelee && !item.weaponPrefab.GetComponent<WeaponSounds>().isShotMelee || this.player_move_c.isMechActive)
			{
				BlinkHealthButton.isBlink = false;
			}
			else
			{
				BlinkHealthButton.isBlink = true;
			}
		}
		this.isBlinkTemp = BlinkHealthButton.isBlink;
		if (this.isBlinkOld != BlinkHealthButton.isBlink)
		{
			this.timerBlink = this.maxTimerBlink;
		}
		if (BlinkHealthButton.isBlink)
		{
			this.timerBlink -= Time.deltaTime;
			if (this.timerBlink < 0f)
			{
				this.timerBlink = this.maxTimerBlink;
				this.isBlinkState = !this.isBlinkState;
				if (this.shine != null)
				{
					this.shine.color = (!this.isBlinkState ? this._blinkColorNoAlpha : this.blinkColor);
				}
			}
		}
		if (!BlinkHealthButton.isBlink && this.isBlinkState)
		{
			this.isBlinkState = !this.isBlinkState;
			if (this.shine != null)
			{
				this.shine.color = (!this.isBlinkState ? this._blinkColorNoAlpha : this.blinkColor);
			}
		}
		this.isBlinkOld = BlinkHealthButton.isBlink;
	}

	public enum RegimButton
	{
		Health,
		Ammo
	}
}