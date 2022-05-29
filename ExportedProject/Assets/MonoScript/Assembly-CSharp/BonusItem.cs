using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class BonusItem : MonoBehaviour
{
	private Player_move_c playerMoveC;

	private GameObject player;

	public bool isActive = true;

	public bool isPickedUp;

	public PhotonPlayer playerPicked;

	public AudioClip[] itemSounds = new AudioClip[Enum.GetValues(typeof(BonusController.TypeBonus)).Length];

	public GameObject[] itemMdls = new GameObject[Enum.GetValues(typeof(BonusController.TypeBonus)).Length];

	private bool isMulti;

	private bool isInet;

	private bool isCOOP;

	private WeaponManager _weaponManager;

	public BonusController.TypeBonus type;

	public double expireTime = -1;

	public bool isTimeBonus;

	public int mySpawnNumber = -1;

	public int myIndex;

	public BonusItem()
	{
	}

	public void ActivateBonus(BonusController.TypeBonus type, Vector3 position, double expireTime, int zoneNumber, int index)
	{
		if (this.isActive)
		{
			return;
		}
		this.type = type;
		base.transform.position = position;
		this.SetModel(true);
		this.mySpawnNumber = zoneNumber;
		this.myIndex = index;
		if (expireTime == -1)
		{
			this.isTimeBonus = false;
			this.expireTime = -1;
		}
		else
		{
			this.isTimeBonus = true;
			this.expireTime = expireTime;
		}
		this.isActive = true;
	}

	private void Awake()
	{
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.isCOOP = Defs.isCOOP;
	}

	public void DeactivateBonus()
	{
		this.isPickedUp = false;
		this.playerPicked = null;
		this.isActive = false;
		base.transform.position = Vector3.down * 100f;
		this.SetModel(false);
	}

	public void PickupBonus(PhotonPlayer player)
	{
		this.isPickedUp = true;
		this.playerPicked = player;
		base.transform.position = Vector3.down * 100f;
		this.SetModel(false);
	}

	private void SetModel(bool show = true)
	{
		this.itemMdls[(int)this.type].SetActive(show);
	}

	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		if (Defs.isMulti)
		{
			this.player = this._weaponManager.myPlayer;
			this.playerMoveC = this._weaponManager.myPlayerMoveC;
		}
		else
		{
			this.player = GameObject.FindGameObjectWithTag("Player");
			if (this.player == null)
			{
				Debug.LogWarning("BonusItem.Start(): player == null");
			}
			else
			{
				this.playerMoveC = this.player.GetComponent<SkinName>().playerMoveC;
			}
		}
	}

	private void Update()
	{
		if (!this.isActive)
		{
			return;
		}
		if (this.isMulti)
		{
			if (this.player == null)
			{
				this.player = this._weaponManager.myPlayer;
				this.playerMoveC = this._weaponManager.myPlayerMoveC;
			}
		}
		else if (this.player == null || this.playerMoveC == null || this.playerMoveC.isKilled || this.playerMoveC.inGameGUI != null && (this.playerMoveC.inGameGUI.pausePanel.activeSelf || this.playerMoveC.inGameGUI.blockedCollider.activeSelf) || ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.isTimeBonus && (PhotonNetwork.isMasterClient && Defs.isInet && PhotonNetwork.time > this.expireTime || !Defs.isInet && Network.time > this.expireTime))
		{
			BonusController.sharedController.RemoveBonus(this.myIndex);
			return;
		}
		if (this.player == null || this.playerMoveC == null || this.playerMoveC.isKilled)
		{
			return;
		}
		if (Vector3.SqrMagnitude(base.transform.position - this.player.transform.position) < 4f)
		{
			bool flag = false;
			switch (this.type)
			{
				case BonusController.TypeBonus.Ammo:
				{
					if (!this._weaponManager.AddAmmoForAllGuns())
					{
						GlobalGameController.Score = GlobalGameController.Score + Defs.ScoreForSurplusAmmo;
					}
					flag = true;
					if (Defs.isMulti)
					{
						this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Ammo);
					}
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.Health:
				{
					if (this.playerMoveC.CurHealth != this.playerMoveC.MaxHealth)
					{
						Player_move_c curHealth = this.playerMoveC;
						curHealth.CurHealth = curHealth.CurHealth + this.playerMoveC.MaxHealth / 2f;
						if (this.playerMoveC.CurHealth > this.playerMoveC.MaxHealth)
						{
							this.playerMoveC.CurHealth = this.playerMoveC.MaxHealth;
						}
						flag = true;
						if (Defs.isMulti)
						{
							this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Health);
						}
						goto case BonusController.TypeBonus.Chest;
					}
					else
					{
						if (!this.isMulti || this.isCOOP)
						{
							GlobalGameController.Score = GlobalGameController.Score + 100;
						}
						flag = true;
						goto case BonusController.TypeBonus.Chest;
					}
				}
				case BonusController.TypeBonus.Armor:
				{
					if (this.playerMoveC.curArmor + 1f <= this.playerMoveC.MaxArmor)
					{
						Player_move_c playerMoveC = this.playerMoveC;
						playerMoveC.curArmor = playerMoveC.curArmor + 1f;
					}
					else if (!this.isMulti || this.isCOOP)
					{
						GlobalGameController.Score = GlobalGameController.Score + 100;
					}
					flag = true;
					if (Defs.isMulti)
					{
						this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Armor);
					}
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.Chest:
				{
					if (!flag)
					{
						break;
					}
					if (Defs.isSoundFX)
					{
						this.playerMoveC.gameObject.GetComponent<AudioSource>().PlayOneShot(this.itemSounds[(int)this.type]);
					}
					if (this.type != BonusController.TypeBonus.Gem)
					{
						BonusController.sharedController.RemoveBonus(this.myIndex);
						break;
					}
					else
					{
						BonusController.sharedController.GetAndRemoveBonus(this.myIndex);
						break;
					}
				}
				case BonusController.TypeBonus.Grenade:
				{
					if (WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount < 10)
					{
						Player_move_c grenadeCount = WeaponManager.sharedManager.myPlayerMoveC;
						grenadeCount.GrenadeCount = grenadeCount.GrenadeCount + 1;
					}
					if (Defs.isMulti)
					{
						this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Grenade);
					}
					flag = true;
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.Mech:
				{
					if (this.playerMoveC.myCurrentWeaponSounds != null && !this.playerMoveC.myCurrentWeaponSounds.isGrenadeWeapon && !this.playerMoveC.isGrenadePress && !ShopNGUIController.GuiActive && (Pauser.sharedPauser == null || !Pauser.sharedPauser.paused) && !this.playerMoveC.isImmortality)
					{
						PotionsController.sharedController.ActivatePotion("Mech", this.playerMoveC, new Dictionary<string, object>(), true);
						flag = true;
					}
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.JetPack:
				{
					PotionsController.sharedController.ActivatePotion("Jetpack", this.playerMoveC, new Dictionary<string, object>(), true);
					flag = true;
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.Invisible:
				{
					PotionsController.sharedController.ActivatePotion("InvisibilityPotion", this.playerMoveC, new Dictionary<string, object>(), true);
					flag = true;
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.Turret:
				{
					if (this.playerMoveC.myCurrentWeaponSounds != null && !this.playerMoveC.myCurrentWeaponSounds.isGrenadeWeapon && InGameGUI.sharedInGameGUI != null && !InGameGUI.sharedInGameGUI.turretPanel.activeSelf && !this.playerMoveC.isGrenadePress && !ShopNGUIController.GuiActive && (Pauser.sharedPauser == null || !Pauser.sharedPauser.paused) && !this.playerMoveC.isImmortality)
					{
						if (!PotionsController.sharedController.PotionIsActive("Turret"))
						{
							InGameGUI.sharedInGameGUI.ShowTurretInterface();
						}
						else
						{
							PotionsController.sharedController.ActivatePotion("Turret", this.playerMoveC, new Dictionary<string, object>(), true);
						}
						flag = true;
					}
					goto case BonusController.TypeBonus.Chest;
				}
				case BonusController.TypeBonus.Gem:
				{
					flag = true;
					goto case BonusController.TypeBonus.Chest;
				}
				default:
				{
					goto case BonusController.TypeBonus.Chest;
				}
			}
		}
	}
}