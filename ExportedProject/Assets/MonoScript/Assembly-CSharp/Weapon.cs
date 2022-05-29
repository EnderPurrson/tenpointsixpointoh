using Rilisoft;
using System;
using UnityEngine;

public class Weapon
{
	public GameObject weaponPrefab;

	private SaltedInt _currentAmmoInBackpack = new SaltedInt(901269156);

	private SaltedInt _currentAmmoInClip = new SaltedInt(384354114);

	public int currentAmmoInBackpack
	{
		get
		{
			return this._currentAmmoInBackpack.Value;
		}
		set
		{
			this._currentAmmoInBackpack.Value = value;
		}
	}

	public int currentAmmoInClip
	{
		get
		{
			return this._currentAmmoInClip.Value;
		}
		set
		{
			this._currentAmmoInClip.Value = value;
		}
	}

	public Weapon()
	{
	}
}