using System;
using UnityEngine;

public class KillerInfo
{
	public bool isSuicide;

	public bool isGrenade;

	public bool isMech;

	public bool isTurret;

	public string nickname;

	public Texture2D rankTex;

	public int rank;

	public Texture clanLogoTex;

	public string clanName;

	public string weapon;

	public Texture skinTex;

	public string hat;

	public string mask;

	public string armor;

	public string cape;

	public Texture capeTex;

	public string boots;

	public int mechUpgrade;

	public int turretUpgrade;

	public Transform killerTransform;

	public int healthValue;

	public int armorValue;

	public KillerInfo()
	{
	}

	public void CopyTo(KillerInfo killerInfo)
	{
		killerInfo.isSuicide = this.isSuicide;
		killerInfo.isGrenade = this.isGrenade;
		killerInfo.isMech = this.isMech;
		killerInfo.isTurret = this.isTurret;
		killerInfo.nickname = this.nickname;
		killerInfo.rankTex = this.rankTex;
		killerInfo.rank = this.rank;
		killerInfo.clanLogoTex = this.clanLogoTex;
		killerInfo.clanName = this.clanName;
		killerInfo.weapon = this.weapon;
		killerInfo.skinTex = this.skinTex;
		killerInfo.hat = this.hat;
		killerInfo.mask = this.mask;
		killerInfo.armor = this.armor;
		killerInfo.cape = this.cape;
		killerInfo.capeTex = this.capeTex;
		killerInfo.boots = this.boots;
		killerInfo.mechUpgrade = this.mechUpgrade;
		killerInfo.turretUpgrade = this.turretUpgrade;
		killerInfo.killerTransform = this.killerTransform;
		killerInfo.healthValue = this.healthValue;
		killerInfo.armorValue = this.armorValue;
	}

	public void Reset()
	{
		this.isSuicide = false;
		this.isGrenade = false;
		this.isMech = false;
		this.isTurret = false;
		this.nickname = string.Empty;
		this.rankTex = null;
		this.rank = 0;
		this.clanLogoTex = null;
		this.clanName = string.Empty;
		this.weapon = string.Empty;
		this.skinTex = null;
		this.hat = string.Empty;
		this.mask = string.Empty;
		this.armor = string.Empty;
		this.cape = string.Empty;
		this.capeTex = null;
		this.boots = string.Empty;
		this.mechUpgrade = 0;
		this.turretUpgrade = 0;
		this.killerTransform = null;
		this.healthValue = 0;
		this.armorValue = 0;
	}
}