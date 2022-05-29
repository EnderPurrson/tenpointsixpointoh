using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class WeaponSounds : MonoBehaviour
{
	public const string RememberedTierWhereGetGunKey = "RememberedTierWhenObtainGun_";

	public WeaponManager.WeaponTypeForLow typeForLow;

	public static Dictionary<WeaponSounds.Effects, KeyValuePair<string, string>> keysAndSpritesForEffects;

	public List<WeaponSounds.Effects> InShopEffects = new List<WeaponSounds.Effects>();

	public int zoomShop;

	public bool isSlowdown;

	[Range(0.01f, 10f)]
	public float slowdownCoeff;

	public float slowdownTime;

	public GameObject[] noFillObjects;

	private GameObject BearWeapon;

	private bool bearActive;

	public WeaponSounds.TypeTracer typeTracer;

	private InnerWeaponPars _innerWeaponPars;

	private BearInnerWeaponPars _bearPars;

	public WeaponSounds.TypeDead typeDead;

	public Transform gunFlash;

	public Transform[] gunFlashDouble;

	public float lengthForShot;

	private float[] damageByTierRememberedTierWhereGet;

	public float[] damageByTier = new float[(int)ExpController.LevelsForTiers.Length];

	public float[] dpses = new float[6];

	private float[] _dpsesCorrectedByRememberedGun;

	public int tier;

	public int categoryNabor = 1;

	public bool isGrenadeWeapon;

	public int fireRateShop;

	public int CapacityShop;

	public int mobilityShop;

	public int[] filterMap;

	public string alternativeName = WeaponManager.PistolWN;

	public bool isBurstShooting;

	public int countShootInBurst = 3;

	public float delayInBurstShooting = 1f;

	public bool isDaterWeapon;

	public string daterMessage = string.Empty;

	public int ammoInClip = 12;

	public int InitialAmmo = 24;

	public int maxAmmo = 84;

	public int ammoForBonusShotMelee = 10;

	public bool isMelee;

	public bool isRoundMelee;

	public float radiusRoundMelee = 5f;

	public bool isLoopShoot;

	public bool isCharging;

	public int chargeMax = 25;

	public float chargeTime = 2f;

	public bool isShotGun;

	public bool isDoubleShot;

	public int countShots = 15;

	public bool isShotMelee;

	public bool isZooming;

	public bool isMagic;

	public bool flamethrower;

	public bool bulletExplode;

	public bool bazooka;

	public int countInSeriaBazooka = 1;

	public float stepTimeInSeriaBazooka = 0.2f;

	public bool railgun;

	public string railName = "Weapon77";

	public bool freezer;

	public int countReflectionRay = 1;

	public bool grenadeLauncher;

	public string bazookaExplosionName = "Weapon75";

	public float bazookaExplosionRadius = 5f;

	public float bazookaExplosionRadiusSelf = 2.5f;

	public float bazookaImpulseRadius = 6f;

	public float impulseForce = 90f;

	public float impulseForceSelf = 133.4f;

	public float fieldOfViewZomm = 75f;

	public float range = 3f;

	public int damage = 50;

	public float speedModifier = 1f;

	public int Probability = 1;

	public Vector2 damageRange = new Vector2(-15f, 15f);

	public Vector3 gunPosition = new Vector3(0.35f, -0.25f, 0.6f);

	public int inAppExtensionModifier = 10;

	public float meleeAngle = 50f;

	public float multiplayerDamage = 1f;

	public float meleeAttackTimeModifier = 0.57f;

	public Vector2 startZone;

	public float tekKoof = 1f;

	public float upKoofFire = 0.5f;

	public float maxKoof = 4f;

	public float downKoofFirst = 0.2f;

	public float downKoof = 0.2f;

	public bool campaignOnly;

	public int rocketNum;

	public int scopeNum;

	public float scaleShop = 150f;

	public Vector3 positionShop;

	public Vector3 rotationShop;

	public WeaponSounds.SpecialEffects specialEffect = WeaponSounds.SpecialEffects.None;

	public float protectionEffectValue = 1f;

	public string localizeWeaponKey;

	private float animLength;

	private float timeFromFire = 1000f;

	private Player_move_c myPlayerC;

	public bool DPSRememberWhenGet;

	[HideInInspector]
	public InnerWeaponPars _innerPars
	{
		get
		{
			if (this._innerWeaponPars == null)
			{
				this.Initialize();
			}
			return this._innerWeaponPars;
		}
	}

	public Texture2D aimTextureH
	{
		get
		{
			Texture2D texture2D;
			if (this._innerPars == null)
			{
				texture2D = null;
			}
			else
			{
				texture2D = this._innerPars.aimTextureH;
			}
			return texture2D;
		}
	}

	public Texture2D aimTextureV
	{
		get
		{
			Texture2D texture2D;
			if (this._innerPars == null)
			{
				texture2D = null;
			}
			else
			{
				texture2D = this._innerPars.aimTextureV;
			}
			return texture2D;
		}
	}

	public GameObject animationObject
	{
		get
		{
			GameObject bearWeapon;
			if (this.bearActive)
			{
				bearWeapon = this.BearWeapon;
			}
			else if (this._innerPars == null)
			{
				bearWeapon = null;
			}
			else
			{
				bearWeapon = this._innerPars.animationObject;
			}
			return bearWeapon;
		}
	}

	public GameObject BearWeaponObject
	{
		get
		{
			return this.BearWeapon;
		}
	}

	public GameObject bonusPrefab
	{
		get
		{
			GameObject gameObject;
			if (this._innerPars == null)
			{
				gameObject = null;
			}
			else
			{
				gameObject = this._innerPars.bonusPrefab;
			}
			return gameObject;
		}
	}

	public AudioClip charge
	{
		get
		{
			AudioClip audioClip;
			if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = this._innerPars.charge;
			}
			return audioClip;
		}
	}

	public float[] DamageByTier
	{
		get
		{
			if (!this.DPSRememberWhenGet)
			{
				string str = base.gameObject.name.Replace("(Clone)", string.Empty);
				if (!FriendsController.damageWeaponsFromABTestBalans.ContainsKey(str))
				{
					return this.damageByTier;
				}
				return FriendsController.damageWeaponsFromABTestBalans[str];
			}
			if (this.damageByTierRememberedTierWhereGet == null)
			{
				int num = Storager.getInt(string.Concat("RememberedTierWhenObtainGun_", base.gameObject.name.Replace("(Clone)", string.Empty)), false);
				this.damageByTierRememberedTierWhereGet = new float[(int)this.damageByTier.Length];
				for (int i = 0; i <= num; i++)
				{
					this.damageByTierRememberedTierWhereGet[i] = this.damageByTier[i];
				}
				for (int j = num + 1; j < (int)this.damageByTierRememberedTierWhereGet.Length; j++)
				{
					this.damageByTierRememberedTierWhereGet[j] = this.damageByTier[num];
				}
			}
			return this.damageByTierRememberedTierWhereGet;
		}
	}

	public int damageShop
	{
		get
		{
			if (!this.DPSRememberWhenGet)
			{
				return Mathf.RoundToInt(this.dpses[(int)this.dpses.Length - 1] * (!this.isShotGun ? (float)((!this.bazooka ? 1 : this.countInSeriaBazooka)) : (float)this.countShots * WeaponManager.ShotgunShotsCountModif()));
			}
			int num = Storager.getInt(string.Concat("RememberedTierWhenObtainGun_", base.gameObject.name.Replace("(Clone)", string.Empty)), false);
			return Mathf.RoundToInt(this.dpses[num] * (!this.isShotGun ? (float)((!this.bazooka ? 1 : this.countInSeriaBazooka)) : (float)this.countShots * WeaponManager.ShotgunShotsCountModif()));
		}
	}

	public float DPS
	{
		get
		{
			if (ExpController.Instance == null)
			{
				return 0f;
			}
			int ourTier = ExpController.Instance.OurTier;
			int num = Math.Max(ourTier, this.tier);
			if ((int)this.dpsesCorrectedByRememberedGun.Length <= num)
			{
				return 0f;
			}
			return this.dpsesCorrectedByRememberedGun[num] * (!this.isShotGun ? (float)this.countInSeriaBazooka : (float)this.countShots * WeaponManager.ShotgunShotsCountModif());
		}
	}

	public float[] dpsesCorrectedByRememberedGun
	{
		get
		{
			if (!this.DPSRememberWhenGet)
			{
				string str = base.gameObject.name.Replace("(Clone)", string.Empty);
				if (!FriendsController.dpsWeaponsFromABTestBalans.ContainsKey(str))
				{
					return this.dpses;
				}
				return FriendsController.dpsWeaponsFromABTestBalans[str];
			}
			if (this._dpsesCorrectedByRememberedGun == null)
			{
				int num = Storager.getInt(string.Concat("RememberedTierWhenObtainGun_", base.gameObject.name.Replace("(Clone)", string.Empty)), false);
				this._dpsesCorrectedByRememberedGun = new float[(int)this.dpses.Length];
				for (int i = 0; i <= num; i++)
				{
					this._dpsesCorrectedByRememberedGun[i] = this.dpses[i];
				}
				for (int j = num + 1; j < (int)this._dpsesCorrectedByRememberedGun.Length; j++)
				{
					this._dpsesCorrectedByRememberedGun[j] = this.dpses[num];
				}
			}
			return this._dpsesCorrectedByRememberedGun;
		}
	}

	public AudioClip empty
	{
		get
		{
			AudioClip audioClip;
			if (this.bearActive && this._bearPars != null && this._bearPars.empty != null)
			{
				audioClip = this._bearPars.empty;
			}
			else if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = this._innerPars.empty;
			}
			return audioClip;
		}
	}

	public Transform grenatePoint
	{
		get
		{
			Transform transforms;
			if (this.bearActive && this._bearPars != null)
			{
				transforms = this._bearPars.grenatePoint;
			}
			else if (this._innerPars == null)
			{
				transforms = null;
			}
			else
			{
				transforms = this._innerPars.grenatePoint;
			}
			return transforms;
		}
	}

	public AudioClip idle
	{
		get
		{
			AudioClip audioClip;
			if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = this._innerPars.idle;
			}
			return audioClip;
		}
	}

	public int InitialAmmoWithEffectsApplied
	{
		get
		{
			return (int)((float)this.InitialAmmo * EffectsController.AmmoModForCategory(this.categoryNabor - 1));
		}
	}

	public Transform LeftArmorHand
	{
		get
		{
			Transform leftArmorHand;
			if (this._innerPars == null)
			{
				leftArmorHand = null;
			}
			else
			{
				leftArmorHand = this._innerPars.LeftArmorHand;
			}
			return leftArmorHand;
		}
	}

	public int MaxAmmoWithEffectApplied
	{
		get
		{
			return (int)((float)this.maxAmmo * EffectsController.AmmoModForCategory(this.categoryNabor - 1));
		}
	}

	public Texture preview
	{
		get
		{
			Texture texture;
			if (this._innerPars == null)
			{
				texture = null;
			}
			else
			{
				texture = this._innerPars.preview;
			}
			return texture;
		}
	}

	public AudioClip reload
	{
		get
		{
			AudioClip audioClip;
			if (this.bearActive && this._bearPars != null && this._bearPars.reload != null)
			{
				audioClip = this._bearPars.reload;
			}
			else if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = this._innerPars.reload;
			}
			return audioClip;
		}
	}

	public Transform RightArmorHand
	{
		get
		{
			Transform rightArmorHand;
			if (this._innerPars == null)
			{
				rightArmorHand = null;
			}
			else
			{
				rightArmorHand = this._innerPars.RightArmorHand;
			}
			return rightArmorHand;
		}
	}

	public AudioClip shoot
	{
		get
		{
			AudioClip audioClip;
			if (this.bearActive && this._bearPars != null && this._bearPars.shoot != null)
			{
				audioClip = this._bearPars.shoot;
			}
			else if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = this._innerPars.shoot;
			}
			return audioClip;
		}
	}

	public string shopName
	{
		get
		{
			return LocalizationStore.Get(this.localizeWeaponKey);
		}
	}

	public string shopNameNonLocalized
	{
		get
		{
			return LocalizationStore.GetByDefault(this.localizeWeaponKey);
		}
	}

	public AudioClip zoomIn
	{
		get
		{
			AudioClip audioClip;
			if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = this._innerPars.zoomIn;
			}
			return audioClip;
		}
	}

	public AudioClip zoomOut
	{
		get
		{
			AudioClip audioClip;
			if (this._innerPars == null)
			{
				audioClip = null;
			}
			else
			{
				audioClip = (this._innerPars.zoomOut == null ? this._innerPars.zoomIn : this._innerPars.zoomOut);
			}
			return audioClip;
		}
	}

	static WeaponSounds()
	{
		Dictionary<WeaponSounds.Effects, KeyValuePair<string, string>> effects = new Dictionary<WeaponSounds.Effects, KeyValuePair<string, string>>()
		{
			{ WeaponSounds.Effects.Automatic, new KeyValuePair<string, string>("shop_stats_auto", "Key_1391") },
			{ WeaponSounds.Effects.SingleShot, new KeyValuePair<string, string>("shop_stats_sngl", "Key_1392") },
			{ WeaponSounds.Effects.Rockets, new KeyValuePair<string, string>("shop_stats_rkt", "Key_1394") },
			{ WeaponSounds.Effects.Mortar, new KeyValuePair<string, string>("shop_stats_grav", "Key_1396") },
			{ WeaponSounds.Effects.Laser, new KeyValuePair<string, string>("shop_stats_lsr", "Key_1393") },
			{ WeaponSounds.Effects.Shotgun, new KeyValuePair<string, string>("shop_stats_shtgn", "Key_1390") },
			{ WeaponSounds.Effects.Chainsaw, new KeyValuePair<string, string>("shop_stats_chain", "Key_1383") },
			{ WeaponSounds.Effects.Flamethrower, new KeyValuePair<string, string>("shop_stats_fire", "Key_1387") },
			{ WeaponSounds.Effects.ElectroThrower, new KeyValuePair<string, string>("shop_stats_elctrc", "Key_1395") },
			{ WeaponSounds.Effects.WallBreak, new KeyValuePair<string, string>("shop_stats_no_wall", "Key_0402") },
			{ WeaponSounds.Effects.AreaDamage, new KeyValuePair<string, string>("shop_stats_area_dmg", "Key_0403") },
			{ WeaponSounds.Effects.Zoom, new KeyValuePair<string, string>("shop_stats_zoom", "Key_0404") },
			{ WeaponSounds.Effects.ThroughEnemies, new KeyValuePair<string, string>("shop_stats_mtpl_enms", "Key_1388") },
			{ WeaponSounds.Effects.Detonation, new KeyValuePair<string, string>("shop_stats_det", "Key_1385") },
			{ WeaponSounds.Effects.GuidedAmmunition, new KeyValuePair<string, string>("shop_stats_cntrl", "Key_1384") },
			{ WeaponSounds.Effects.Ricochet, new KeyValuePair<string, string>("shop_stats_refl", "Key_1389") },
			{ WeaponSounds.Effects.SeveralMissiles, new KeyValuePair<string, string>("shop_stats_few", "Key_1386") },
			{ WeaponSounds.Effects.Silent, new KeyValuePair<string, string>("shop_stats_g_slnt", "Key_1397") },
			{ WeaponSounds.Effects.ForSandbox, new KeyValuePair<string, string>("shop_stats_sandbox", "Key_1603") },
			{ WeaponSounds.Effects.SlowTheTarget, new KeyValuePair<string, string>("shop_stats_slow_target", "Key_1759") },
			{ WeaponSounds.Effects.SemiAuto, new KeyValuePair<string, string>("shop_stats_semi_auto", "Key_2138") },
			{ WeaponSounds.Effects.ChargingShot, new KeyValuePair<string, string>("shop_stats_charging_shot", "Key_2226") },
			{ WeaponSounds.Effects.StickyMines, new KeyValuePair<string, string>("shop_stats_sticky_mines", "Key_2227") },
			{ WeaponSounds.Effects.DamageAbsorbtion, new KeyValuePair<string, string>("shop_stats_damage_absorbtion", "Key_2228") },
			{ WeaponSounds.Effects.ChainShot, new KeyValuePair<string, string>("shop_stats_cahin_shot", "Key_2229") },
			{ WeaponSounds.Effects.AutoHoming, new KeyValuePair<string, string>("shop_stats_auto_homing", "Key_2230") }
		};
		WeaponSounds.keysAndSpritesForEffects = effects;
	}

	public WeaponSounds()
	{
	}

	private void CheckPlayDefaultAnimInMulti()
	{
		if (!Defs.isInet)
		{
			return;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		Player_move_c component = base.transform.parent.GetComponent<Player_move_c>();
		if (component != null && !component.isMine && !this._innerPars.GetComponent<Animation>().isPlaying)
		{
			this._innerPars.GetComponent<Animation>().Play("Idle");
		}
	}

	public void fire()
	{
		this.timeFromFire = 0f;
		WeaponSounds weaponSound = this;
		weaponSound.tekKoof = weaponSound.tekKoof + (this.upKoofFire + this.downKoofFirst);
		if (this.tekKoof > this.maxKoof + this.downKoofFirst)
		{
			this.tekKoof = this.maxKoof + this.downKoofFirst;
		}
	}

	public List<GameObject> GetListWeaponAnimEffects()
	{
		if (this._innerPars == null)
		{
			return null;
		}
		WeaponAnimParticleEffects component = this._innerPars.GetComponent<WeaponAnimParticleEffects>();
		if (component == null)
		{
			return null;
		}
		return component.GetListAnimEffects();
	}

	public void Initialize()
	{
		string str = ResPath.Combine(Defs.InnerWeaponsFolder, string.Concat(base.gameObject.name.Replace("(Clone)", string.Empty), Defs.InnerWeapons_Suffix));
		LoadAsyncTool.ObjectRequest objectRequest = LoadAsyncTool.Get(str, true);
		this.Initialize(objectRequest.asset as GameObject);
		if (this._innerWeaponPars != null)
		{
			Player_move_c.SetLayerRecursively(this._innerWeaponPars.gameObject, base.gameObject.layer);
		}
	}

	public void Initialize(GameObject pref)
	{
		Transform child;
		if (!base.gameObject.name.Contains("Weapon"))
		{
			return;
		}
		if (this._innerWeaponPars != null)
		{
			return;
		}
		if (pref != null)
		{
			this._innerWeaponPars = (UnityEngine.Object.Instantiate(pref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<InnerWeaponPars>();
			if (Defs.isDaterRegim)
			{
				string str = string.Concat("MechBearWeapons/", base.gameObject.name.Replace("(Clone)", string.Empty), "_MechBear");
				UnityEngine.Object obj = Resources.Load(str);
				if (obj != null)
				{
					this.BearWeapon = (GameObject)UnityEngine.Object.Instantiate(obj, new Vector3(0f, 0f, 0f), Quaternion.identity);
					this._bearPars = this.BearWeapon.GetComponent<BearInnerWeaponPars>();
					this.BearWeapon.transform.SetParent(base.gameObject.transform, false);
					this.BearWeapon.SetActive(false);
				}
			}
			this._innerWeaponPars.gameObject.transform.SetParent(base.gameObject.transform, false);
		}
		if (!this.isMelee)
		{
			if (base.transform.childCount <= 0 || base.transform.GetChild(0).childCount <= 0)
			{
				child = null;
			}
			else
			{
				child = base.transform.GetChild(0).GetChild(0);
			}
			this.gunFlash = child;
		}
	}

	public bool IsAvalibleFromFilter(int filter)
	{
		if (filter == 0)
		{
			return true;
		}
		if (this.filterMap != null && this.filterMap.Contains(filter))
		{
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		if (this._innerPars != null)
		{
			UnityEngine.Object.Destroy(this._innerPars.gameObject);
		}
	}

	public void SetDaterBearHandsAnim(bool set)
	{
		this.bearActive = (!set ? false : this.BearWeapon != null);
		this._innerPars.animationObject.SetActive(!this.bearActive);
		if (this.BearWeapon != null)
		{
			this.BearWeapon.SetActive(this.bearActive);
		}
	}

	private void Start()
	{
		if (string.IsNullOrEmpty(this.bazookaExplosionName))
		{
			this.bazookaExplosionName = base.gameObject.name.Replace("(Clone)", string.Empty);
		}
		if (this.isDoubleShot)
		{
			if (this.animationObject != null && this.animationObject.GetComponent<Animation>()["Shoot1"] != null)
			{
				this.animLength = this.animationObject.GetComponent<Animation>()["Shoot1"].length;
			}
		}
		else if (this.animationObject != null && this.animationObject.GetComponent<Animation>()["Shoot"] != null)
		{
			this.animLength = this.animationObject.GetComponent<Animation>()["Shoot"].length;
		}
	}

	private void Update()
	{
		if (base.transform.parent != null)
		{
			if (this.myPlayerC == null)
			{
				this.myPlayerC = base.transform.parent.GetComponent<Player_move_c>();
			}
			if (base.transform.parent != null && this.myPlayerC != null && !this.myPlayerC.isMine && this.myPlayerC.isMulti && this.animationObject.activeSelf == this.myPlayerC.isInvisible)
			{
				this.animationObject.SetActive(!this.myPlayerC.isInvisible);
			}
		}
		if (this.timeFromFire >= this.animLength)
		{
			if (this.tekKoof > 1f)
			{
				WeaponSounds weaponSound = this;
				weaponSound.tekKoof = weaponSound.tekKoof - this.downKoof * Time.deltaTime / this.animLength;
			}
			if (this.tekKoof < 1f)
			{
				this.tekKoof = 1f;
			}
		}
		else
		{
			this.timeFromFire += Time.deltaTime;
			if (this.tekKoof > 1f)
			{
				WeaponSounds weaponSound1 = this;
				weaponSound1.tekKoof = weaponSound1.tekKoof - this.downKoofFirst * Time.deltaTime / this.animLength;
			}
			if (this.tekKoof < 1f)
			{
				this.tekKoof = 1f;
			}
		}
		this.CheckPlayDefaultAnimInMulti();
	}

	public enum Effects
	{
		Automatic,
		SingleShot,
		Rockets,
		Mortar,
		Laser,
		Shotgun,
		Chainsaw,
		Flamethrower,
		ElectroThrower,
		WallBreak,
		AreaDamage,
		Zoom,
		ThroughEnemies,
		Detonation,
		GuidedAmmunition,
		Ricochet,
		SeveralMissiles,
		Silent,
		ForSandbox,
		SlowTheTarget,
		SemiAuto,
		ChargingShot,
		StickyMines,
		DamageAbsorbtion,
		ChainShot,
		AutoHoming
	}

	public enum SpecialEffects
	{
		None = -1,
		PlayerShield = 0
	}

	public enum TypeDead
	{
		angel,
		explosion,
		energyBlue,
		energyRed,
		energyPink,
		energyCyan,
		energyLight,
		energyGreen,
		energyOrange,
		energyWhite,
		like
	}

	public enum TypeTracer
	{
		none = -1,
		standart = 0,
		red = 1,
		for252 = 2,
		turquoise = 3,
		green = 4,
		violet = 5
	}
}