using System;
using System.Collections;
using UnityEngine;

internal sealed class AmmoUpdater : MonoBehaviour
{
	private UILabel _label;

	public GameObject ammoSprite;

	public AmmoUpdater()
	{
	}

	private void Start()
	{
		this._label = base.GetComponent<UILabel>();
	}

	private void Update()
	{
		if (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.currentWeaponSounds != null) || ((Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex]).weaponPrefab.GetComponent<WeaponSounds>().isMelee && !((Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex]).weaponPrefab.GetComponent<WeaponSounds>().isShotMelee || !(this._label != null))
		{
			this._label.text = string.Empty;
			if (this.ammoSprite != null && this.ammoSprite.activeSelf)
			{
				this.ammoSprite.SetActive(false);
			}
		}
		else
		{
			Weapon item = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			this._label.text = (!((Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex]).weaponPrefab.GetComponent<WeaponSounds>().isShotMelee ? string.Format("{0}/{1}", item.currentAmmoInClip, item.currentAmmoInBackpack) : (item.currentAmmoInClip + item.currentAmmoInBackpack).ToString());
			if (this.ammoSprite != null && !this.ammoSprite.activeSelf)
			{
				this.ammoSprite.SetActive(true);
			}
		}
	}
}