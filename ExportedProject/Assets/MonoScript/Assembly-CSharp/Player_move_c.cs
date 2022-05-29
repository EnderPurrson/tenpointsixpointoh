using com.amazon.device.iap.cpt;
using ExitGames.Client.Photon;
using Holoville.HOTween;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using RilisoftBot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public sealed class Player_move_c : MonoBehaviour
{
	private const string keyKilledPlayerCharactersCount = "KilledPlayerCharactersCount";

	private const string startShootAnimName = "Shoot_start";

	private const string endShootAnimName = "Shoot_end";

	private const float slowdownCoefConst = 0.75f;

	private int tierForKilledRate;

	private readonly Dictionary<string, int> weKillForKillRate = new Dictionary<string, int>();

	private readonly Dictionary<string, int> weWereKilledForKillRate = new Dictionary<string, int>();

	public TextMesh nickLabel;

	public AudioClip mechActivSound;

	public AudioClip mechBearActivSound;

	public AudioClip invisibleActivSound;

	public AudioClip jetpackActivSound;

	public AudioClip portalSound;

	public PlayerScoreController myScoreController;

	public bool isRocketJump;

	public float armorSynch;

	public bool isReloading;

	public bool _isPlacemarker;

	public GameObject placemarkerMark;

	public GameObject redMark;

	public GameObject blueMark;

	public Player_move_c placemarkerMoveC;

	public ParticleBonuse[] bonusesParticles;

	public GameObject particleBonusesPoint;

	public Transform myTransform;

	public Transform myPlayerTransform;

	public int myPlayerID;

	public NetworkViewID myPlayerIDLocal;

	public SkinName mySkinName;

	public GameObject mechPoint;

	public GameObject mechBody;

	public GameObject mechBearPoint;

	public GameObject mechBearBody;

	public GameObject mechBearHands;

	public GameObject shieldObject;

	public Animation mechGunAnimation;

	public Animation mechBodyAnimation;

	public Animation mechBearGunAnimation;

	public Animation mechBearBodyAnimation;

	public WeaponSounds mechWeaponSounds;

	public WeaponSounds mechBearWeaponSounds;

	public ParticleSystem[] flashMech;

	public GameObject fpsPlayerBody;

	public GameObject myCurrentWeapon;

	public WeaponSounds myCurrentWeaponSounds;

	public GameObject mechExplossion;

	public GameObject bearExplosion;

	public AudioSource mechExplossionSound;

	public AudioSource mechBearExplosionSound;

	public AudioClip shootMechClip;

	public AudioClip shootMechBearClip;

	public SkinnedMeshRenderer playerBodyRenderer;

	public SkinnedMeshRenderer mechBodyRenderer;

	public SkinnedMeshRenderer mechHandRenderer;

	public SkinnedMeshRenderer mechGunRenderer;

	public SkinnedMeshRenderer mechBearBodyRenderer;

	public SkinnedMeshRenderer mechBearHandRenderer;

	public SynhRotationWithGameObject mechBearSyncRot;

	public Transform PlayerHeadTransform;

	public Transform MechHeadTransform;

	public CapsuleCollider bodyCollayder;

	public CapsuleCollider headCollayder;

	public Material[] mechGunMaterials;

	public Material[] mechBodyMaterials;

	private int numShootInDoubleShot = 1;

	public bool isMechActive;

	public bool isBearActive;

	public AudioClip flagGetClip;

	public AudioClip flagLostClip;

	public AudioClip flagScoreEnemyClip;

	public AudioClip flagScoreMyCommandClip;

	public float deltaAngle;

	public GameObject playerDeadPrefab;

	public ThirdPersonNetwork1 myPersonNetwork;

	public GameObject grenadePrefab;

	public GameObject likePrefab;

	public GameObject turretPrefab;

	public GameObject turretPoint;

	public GameObject currentTurret;

	public float liveMech;

	public float[] liveMechByTier;

	private GameObject currentGrenade;

	public int currentWeaponBeforeTurret = -1;

	private int currentWeaponBeforeGrenade;

	private float stdFov = 60f;

	private int countMultyFlag;

	private string[] iconShotName = new string[] { string.Empty, "Chat_Death", "Chat_HeadShot", "Chat_Explode", "Chat_Sniper", "Chat_Flag", "Chat_grenade", "Chat_grenade_hell", "Chat_Turret", "Chat_Turret_Explode", string.Empty, "Smile_1_15" };

	public bool isImVisible;

	public bool isWeaponSet;

	public NetworkStartTableNGUIController networkStartTableNGUIController;

	public GameObject invisibleParticle;

	public bool isInvisible;

	public bool isBigHead;

	public float maxTimeSetTimerShow = 0.5f;

	private float _koofDamageWeaponFromPotoins;

	public bool isRegenerationLiveZel;

	private float maxTimerRegenerationLiveZel = 5f;

	public bool isRegenerationLiveCape;

	private float maxTimerRegenerationLiveCape = 15f;

	private float timerRegenerationLiveZel;

	private float timerRegenerationLiveCape;

	private float timerRegenerationArmor;

	private Shader[] oldShadersInInvisible;

	private Color[] oldColorInInvisible;

	public bool isCaptureFlag;

	public GameObject myBaza;

	public Camera myCamera;

	public Camera gunCamera;

	public GameObject hatsPoint;

	public GameObject capesPoint;

	public GameObject flagPoint;

	public GameObject bootsPoint;

	public GameObject armorPoint;

	public GameObject arrowToPortalPoint;

	public bool isZooming;

	public AudioClip headShotSound;

	public AudioClip flagCaptureClip;

	public AudioClip flagPointClip;

	public GameObject headShotParticle;

	public GameObject healthParticle;

	public GameObject chatViewer;

	public GUISkin MySkin;

	public GameObject myTable;

	public NetworkStartTable myNetworkStartTable;

	private float[] _byCatDamageModifs = new float[6];

	public int AimTextureWidth = 50;

	public int AimTextureHeight = 50;

	public Transform GunFlash;

	private bool isZachetWin;

	public bool showGUIUnlockFullVersion;

	public float timeHingerGame;

	public int BulletForce = 5000;

	public bool killed;

	public ZombiManager zombiManager;

	public NickLabelController myNickLabelController;

	public visibleObjPhoton visibleObj;

	public string textChat;

	public bool showGUI = true;

	public bool showRanks;

	public Player_move_c.SystemMessage[] killedSpisok = new Player_move_c.SystemMessage[3];

	public GUIStyle combatRifleStyle;

	public GUIStyle goldenEagleInappStyle;

	public GUIStyle magicBowInappStyle;

	public GUIStyle spasStyle;

	public GUIStyle axeStyle;

	public GUIStyle famasStyle;

	public GUIStyle glockStyle;

	public GUIStyle chainsawStyle;

	public GUIStyle scytheStyle;

	public GUIStyle shovelStyle;

	private Vector3 camPosition;

	private Quaternion camRotaion;

	public bool showChat;

	public bool showChatOld;

	public bool showRanksOld;

	private bool isDeadFrame;

	private int _myCommand;

	public bool respawnedForGUI = true;

	private int _nickColorInd;

	public float liveTime;

	public float timerShowUp;

	public float timerShowLeft;

	public float timerShowDown;

	public float timerShowRight;

	public string myIp = string.Empty;

	public TrainingController trainigController;

	public bool isKilled;

	public bool theEnd;

	public string nickPobeditel;

	public Texture hitTexture;

	public Texture _skin;

	public float showNoInetTimer = 5f;

	private SaltedInt _killCount = new SaltedInt(428452539);

	private float _timeWhenPurchShown;

	private bool inAppOpenedFromPause;

	public bool isMulti;

	public bool isInet;

	public bool isMine;

	public bool isCompany;

	public bool isCOOP;

	private ExperienceController expController;

	private float inGameTime;

	public int multiKill;

	private HungerGameController hungerGameController;

	private bool isHunger;

	public static float maxTimerShowMultyKill;

	public FlagController flag1;

	public FlagController flag2;

	public FlagController myFlag;

	public FlagController enemyFlag;

	private GameObject rocketToLaunch;

	public bool isStartAngel;

	private float maxTimerImmortality = 3f;

	public bool isImmortality = true;

	private float timerImmortality = 3f;

	private float timerImmortalityForAlpha = 3f;

	private readonly KillerInfo _killerInfo = new KillerInfo();

	private List<int> myKillAssists = new List<int>();

	private readonly List<NetworkViewID> myKillAssistsLocal = new List<NetworkViewID>();

	private bool showGrenadeHint = true;

	private bool showZoomHint = true;

	private bool showChangeWeaponHint = true;

	private int respawnCountForTraining;

	[NonSerialized]
	public string currentWeapon;

	[NonSerialized]
	public int mechUpgrade;

	[NonSerialized]
	public int turretUpgrade;

	private bool _weaponPopularityCacheIsDirty;

	public int[] counterSerials = new int[6];

	private float _curBaseArmor;

	public int AmmoBoxWidth = 100;

	public int AmmoBoxHeight = 100;

	public int AmmoBoxOffset = 10;

	public int ScoreBoxWidth = 100;

	public int ScoreBoxHeight = 100;

	public int ScoreBoxOffset = 10;

	public float[] timerShow = new float[] { -1f, -1f, -1f };

	public AudioClip deadPlayerSound;

	public AudioClip damagePlayerSound;

	public AudioClip damageArmorPlayerSound;

	private float GunFlashLifetime;

	public GameObject[] zoneCreatePlayer;

	public GUIStyle ScoreBox;

	public GUIStyle AmmoBox;

	private float mySens;

	public GUIStyle sliderSensStyle;

	public GUIStyle thumbSensStyle;

	private GameObject damage;

	private bool damageShown;

	private Pauser _pauser;

	private bool _backWasPressed;

	public GameObject _player;

	public GameObject bulletPrefab;

	public GameObject bulletPrefabRed;

	public GameObject bulletPrefabFor252;

	public GameObject _bulletSpawnPoint;

	private GameObject _inAppGameObject;

	public StoreKitEventListener _listener;

	public GUIStyle puliInApp;

	public InGameGUI inGameGUI;

	private Dictionary<string, Action<string>> _actionsForPurchasedItems = new Dictionary<string, Action<string>>();

	public GUIStyle crystalSwordInapp;

	public GUIStyle elixirInapp;

	public GUIStyle pulemetInApp;

	public bool _isInappWinOpen;

	private WeaponManager ___weaponManager;

	public GUIStyle armorStyle;

	private SaltedInt _countKillsCommandBlue = new SaltedInt(180068360);

	private SaltedInt _countKillsCommandRed = new SaltedInt(180068361);

	private bool canReceiveSwipes = true;

	public float slideMagnitudeX;

	public float slideMagnitudeY;

	public AudioClip ChangeWeaponClip;

	public AudioClip ChangeGrenadeClip;

	public AudioClip WeaponBonusClip;

	public PhotonView photonView;

	public AudioClip clickShop;

	public List<Player_move_c.MessageChat> messages = new List<Player_move_c.MessageChat>();

	public bool isSurvival;

	public string myTableId;

	private int oldKilledPlayerCharactersCount;

	public Material _bodyMaterial;

	public Material _mechMaterial;

	public Material _bearMaterial;

	public Material curMainSelect;

	public GameObject jetPackPoint;

	public GameObject jetPackPointMech;

	public GameObject wingsPoint;

	public GameObject wingsPointBear;

	public Animation wingsAnimation;

	public Animation wingsBearAnimation;

	private bool isPlayerFlying;

	public ParticleSystem[] jetPackParticle;

	public GameObject jetPackSound;

	public AudioSource wingsSound;

	private int indexWeapon;

	private bool shouldSetMaxAmmoWeapon;

	private bool _changingWeapon;

	private IDisposable _backSubscription;

	private bool BonusEffectForArmorWorksInThisMatch;

	private bool ArmorBonusGiven;

	private bool isDaterRegim;

	private bool pausedRating;

	public float _currentReloadAnimationSpeed = 1f;

	private int countHouseKeeperEvent;

	private bool isJumpPresedOld;

	private int countFixedUpdateForResetLabel;

	private float _chanceToIgnoreHeadshot;

	private float _protectionShieldValue = 1f;

	private bool isShieldActivated;

	private bool roomTierInitialized;

	private int roomTier;

	private bool _escapePressed;

	private float oldAlphaImmortality = -1f;

	private float _timeOfSlowdown;

	private bool isActiveTurretPanelInPause;

	private SaltedInt numberOfGrenadesOnStart = new SaltedInt(45853678);

	private SaltedInt numberOfGrenades = new SaltedInt(29076718);

	public float timeBuyHealth = -10000f;

	private SaltedFloat _curHealthSalt = new SaltedFloat();

	public float synhHealth = -10000000f;

	public double synchTimeHealth;

	public bool isSuicided;

	public bool killedInMatch;

	private float damageBuff = 1f;

	private float protectionBuff = 1f;

	private bool getLocalHurt;

	private bool timeSettedAfterRegenerationSwitchedOn;

	private bool isRaiderMyPoint;

	private int countMySpotEvent;

	public Vector3 pointAutoAim;

	private float timerUpdatePointAutoAi = -1f;

	private Ray rayAutoAim;

	private bool isShooting;

	public bool isShootingLoop;

	public float chargeValue;

	private CancellationTokenSource ctsShootLoop = new CancellationTokenSource();

	private int _countShootInBurst;

	private float _timerDelayInShootingBurst = -1f;

	private int ammoInClipBeforeCharge;

	private int lastChargeWeaponIndex;

	private float nextChargeConsumeTime = -1f;

	private float timeGrenadePress;

	public bool isGrenadePress;

	private Player_move_c.OnMessagesUpdate messageDelegate;

	private EventHandler<EventArgs> WeaponChanged;

	private Action<float> FreezerFired;

	private float _curHealth
	{
		get
		{
			return this._curHealthSalt.@value;
		}
		set
		{
			this._curHealthSalt.@value = value;
		}
	}

	public NetworkView _networkView
	{
		get;
		set;
	}

	public static int _ShootRaycastLayerMask
	{
		get
		{
			return -2053 & ~(1 << (LayerMask.NameToLayer("DamageCollider") & 31)) & ~(1 << (LayerMask.NameToLayer("TransparentFX") & 31));
		}
	}

	public WeaponManager _weaponManager
	{
		get
		{
			return this.___weaponManager;
		}
		set
		{
			this.___weaponManager = value;
		}
	}

	internal static bool AnotherNeedApply
	{
		get;
		set;
	}

	public float[] byCatDamageModifs
	{
		get
		{
			return this._byCatDamageModifs;
		}
	}

	public int countKills
	{
		get
		{
			return this._killCount.Value;
		}
		set
		{
			this._killCount.Value = value;
		}
	}

	public int countKillsCommandBlue
	{
		get
		{
			return this._countKillsCommandBlue.Value;
		}
		set
		{
			this._countKillsCommandBlue.Value = value;
		}
	}

	public int countKillsCommandRed
	{
		get
		{
			return this._countKillsCommandRed.Value;
		}
		set
		{
			this._countKillsCommandRed.Value = value;
		}
	}

	public float curArmor
	{
		get
		{
			return this.CurrentBaseArmor + this.CurrentBodyArmor + this.CurrentHatArmor;
		}
		set
		{
			float currentHatArmor = this.curArmor - value;
			if (currentHatArmor >= 0f)
			{
				if (this.CurrentHatArmor < currentHatArmor)
				{
					currentHatArmor -= this.CurrentHatArmor;
					this.CurrentHatArmor = 0f;
					if (this.CurrentBodyArmor < currentHatArmor)
					{
						currentHatArmor -= this.CurrentBodyArmor;
						this.CurrentBodyArmor = 0f;
						Player_move_c currentBaseArmor = this;
						currentBaseArmor.CurrentBaseArmor = currentBaseArmor.CurrentBaseArmor - currentHatArmor;
					}
					else
					{
						Player_move_c currentBodyArmor = this;
						currentBodyArmor.CurrentBodyArmor = currentBodyArmor.CurrentBodyArmor - currentHatArmor;
					}
				}
				else
				{
					Player_move_c playerMoveC = this;
					playerMoveC.CurrentHatArmor = playerMoveC.CurrentHatArmor - currentHatArmor;
				}
			}
			else if (currentHatArmor < 0f)
			{
				currentHatArmor *= -1f;
				if (this.WearedMaxArmor <= 0f)
				{
					currentHatArmor = 1f;
				}
				else
				{
					currentHatArmor = (this.WearedMaxArmor <= 5f ? this.WearedMaxArmor - this.WearedCurrentArmor : Mathf.Min(this.WearedMaxArmor - this.WearedCurrentArmor, this.WearedMaxArmor * 0.5f));
				}
				this.AddArmor(currentHatArmor);
			}
		}
	}

	public float CurHealth
	{
		get
		{
			return this._curHealth;
		}
		set
		{
			float single = this._curHealth - value;
			Player_move_c playerMoveC = this;
			playerMoveC._curHealth = playerMoveC._curHealth - single;
		}
	}

	private float CurrentBaseArmor
	{
		get
		{
			return this._curBaseArmor;
		}
		set
		{
			this._curBaseArmor = value;
		}
	}

	private float CurrentBodyArmor
	{
		get
		{
			float single = 0f;
			Wear.curArmor.TryGetValue(Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty, out single);
			return single;
		}
		set
		{
			if (Wear.curArmor.ContainsKey(Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty))
			{
				Wear.curArmor[Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty] = value;
			}
		}
	}

	private float CurrentHatArmor
	{
		get
		{
			float single = 0f;
			Wear.curArmor.TryGetValue(Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty, out single);
			return single;
		}
		set
		{
			if (Wear.curArmor.ContainsKey(Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty))
			{
				Wear.curArmor[Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty] = value;
			}
		}
	}

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	public int GrenadeCount
	{
		get
		{
			return this.numberOfGrenades.Value;
		}
		set
		{
			this.numberOfGrenades.Value = value;
		}
	}

	public bool isInappWinOpen
	{
		get
		{
			return this._isInappWinOpen;
		}
		set
		{
			this._isInappWinOpen = value;
			ShopNGUIController.GuiActive = value;
			if (this.myCurrentWeaponSounds != null)
			{
				if (!(PauseGUIController.Instance != null) || !PauseGUIController.Instance.IsPaused)
				{
					this.myCurrentWeaponSounds.animationObject.SetActive(!value);
				}
				else
				{
					this.myCurrentWeaponSounds.animationObject.SetActive(false);
				}
				if (this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
				{
					if (!value)
					{
						this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
					}
					else
					{
						this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Stop();
					}
				}
			}
		}
	}

	private bool isNeedShowRespawnWindow
	{
		get
		{
			return (this.isHunger || Defs.isRegimVidosDebug || this._killerInfo.isSuicide || !Defs.isMulti ? false : !Defs.isCOOP);
		}
	}

	public bool isNeedTakePremiumAccountRewards
	{
		get;
		private set;
	}

	public bool isPlacemarker
	{
		get
		{
			return this._isPlacemarker;
		}
		set
		{
			this._isPlacemarker = value;
			this.placemarkerMark.SetActive(value);
		}
	}

	public KillerInfo killerInfo
	{
		get
		{
			return this._killerInfo;
		}
	}

	public float koofDamageWeaponFromPotoins
	{
		get
		{
			return this._koofDamageWeaponFromPotoins;
		}
		set
		{
			this._koofDamageWeaponFromPotoins = value;
		}
	}

	private Material mainDamageMaterial
	{
		get
		{
			if (this.isMechActive)
			{
				this.curMainSelect = this._mechMaterial;
				return this._mechMaterial;
			}
			if (this.isBearActive)
			{
				this.curMainSelect = this._bearMaterial;
				return this._bearMaterial;
			}
			this.curMainSelect = this._bodyMaterial;
			return this._bodyMaterial;
		}
	}

	public float MaxArmor
	{
		get
		{
			return this.maxBaseArmor + this.WearedMaxArmor;
		}
	}

	private float maxBaseArmor
	{
		get
		{
			return 9f + EffectsController.ArmorBonus;
		}
	}

	public float MaxHealth
	{
		get
		{
			return (!Defs.isMulti || !Defs.isHunger ? ExperienceController.HealthByLevel[(!Defs.isMulti || !(this.myNetworkStartTable != null) ? ExperienceController.sharedController.currentLevel : this.myNetworkStartTable.myRanks)] : ExperienceController.HealthByLevel[1]);
		}
	}

	public static int MaxPlayerGUIHealth
	{
		get
		{
			return 9;
		}
	}

	private float maxTimerRegenerationArmor
	{
		get
		{
			return EffectsController.RegeneratingArmorTime;
		}
	}

	public int myCommand
	{
		get
		{
			return this._myCommand;
		}
		set
		{
			this._myCommand = value;
			this.UpdateNickLabelColor();
		}
	}

	internal static bool NeedApply
	{
		get;
		set;
	}

	private float WearedCurrentArmor
	{
		get
		{
			return this.CurrentBodyArmor + this.CurrentHatArmor;
		}
	}

	public float WearedMaxArmor
	{
		get
		{
			float single = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN, false), this.TierOrRoomTier((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier)));
			float single1 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN, false), this.TierOrRoomTier((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier)));
			return single + single1;
		}
	}

	static Player_move_c()
	{
		Player_move_c.maxTimerShowMultyKill = 3f;
	}

	public Player_move_c()
	{
	}

	private void _DoHit(RaycastHit _hit, bool slowdown = false)
	{
		bool flag = false;
		if (_hit.transform.parent)
		{
			if (_hit.transform.parent.CompareTag("Enemy"))
			{
				flag = (_hit.collider.name == "HeadCollider" ? true : _hit.collider is SphereCollider);
				this._HitEnemy(_hit.transform.parent.gameObject, flag, 0f);
				return;
			}
			if (_hit.transform.parent.CompareTag("Player"))
			{
				flag = _hit.collider.name == "HeadCollider";
				this._HitEnemy(_hit.transform.parent.gameObject, flag, 0f);
				return;
			}
		}
		flag = _hit.collider.name == "HeadCollider";
		this._HitEnemy(_hit.transform.gameObject, flag, 0f);
	}

	private void _FireFlash(bool isFlash = true, int numFlash = 0)
	{
		if (this.myCurrentWeaponSounds.isLoopShoot)
		{
			return;
		}
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				this._networkView.RPC("fireFlash", RPCMode.Others, new object[] { isFlash, numFlash });
			}
			else
			{
				this.photonView.RPC("fireFlash", PhotonTargets.Others, new object[] { isFlash, numFlash });
			}
		}
	}

	private void _FireFlashWithHole(bool _isBloodParticle, Vector3 _pos, Quaternion _rot, bool isFlash = true, int numFlash = 0)
	{
		if (this.isMulti && this.isInet)
		{
			this.photonView.RPC("fireFlashWithHole", PhotonTargets.Others, new object[] { _isBloodParticle, _pos, _rot, isFlash, numFlash });
		}
	}

	private void _FireFlashWithManyHoles(bool[] _isBloodParticle, Vector3[] _pos, Quaternion[] _rot, bool isFlash = true, int numFlash = 0)
	{
		if (this.isMulti && this.isInet)
		{
			this.photonView.RPC("fireFlashWithManyHoles", PhotonTargets.Others, new object[] { _isBloodParticle, _pos, _rot, isFlash, numFlash });
		}
	}

	private void _HitChest(GameObject go)
	{
		WeaponSounds weaponSound = (!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		go.GetComponent<ChestController>().MinusLive(((float)weaponSound.damage + UnityEngine.Random.Range(weaponSound.damageRange.x, weaponSound.damageRange.y)) * (1f + this.koofDamageWeaponFromPotoins + EffectsController.DamageModifsByCats(weaponSound.categoryNabor - 1)));
	}

	private void _HitEnemy(GameObject hitEnemy, bool headshot = false, float sqrDistance = 0)
	{
		int num;
		string str = hitEnemy.tag;
		if (str != null)
		{
			if (Player_move_c.u003cu003ef__switchu0024map8 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(5)
				{
					{ "Enemy", 0 },
					{ "Player", 1 },
					{ "Chest", 2 },
					{ "Turret", 3 },
					{ "DamagedExplosion", 4 }
				};
				Player_move_c.u003cu003ef__switchu0024map8 = strs;
			}
			if (Player_move_c.u003cu003ef__switchu0024map8.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						this._HitZombie(hitEnemy.transform.GetChild(0).gameObject, headshot, sqrDistance);
						break;
					}
					case 1:
					{
						this._HitPlayer(hitEnemy, headshot, sqrDistance);
						break;
					}
					case 2:
					{
						this._HitChest(hitEnemy);
						break;
					}
					case 3:
					{
						this._HitTurret(hitEnemy, sqrDistance);
						break;
					}
					case 4:
					{
						float damageForBotsAndExplosionObjects = this.GetDamageForBotsAndExplosionObjects(false);
						if (((!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds)).isCharging)
						{
							damageForBotsAndExplosionObjects *= this.chargeValue;
						}
						DamagedExplosionObject.TryApplyDamageToObject(hitEnemy, damageForBotsAndExplosionObjects);
						break;
					}
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator _HitEnemyWithDelay(GameObject hitEnemy, float time, bool headshot = false)
	{
		Player_move_c.u003c_HitEnemyWithDelayu003ec__IteratorE8 variable = null;
		return variable;
	}

	private void _HitPlayer(GameObject plr, bool isHeadShot, float sqrDistance)
	{
		Player_move_c.TypeKills typeKill;
		PlayerEventScoreController.ScoreEvent scoreEvent;
		string str;
		float damageByTier;
		WeaponSounds weaponSound = (!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		Player_move_c component = plr.GetComponent<SkinName>().playerMoveC;
		float single = 1f;
		if (isHeadShot)
		{
			isHeadShot = UnityEngine.Random.Range(0f, 1f) >= component._chanceToIgnoreHeadshot;
		}
		if (isHeadShot)
		{
			single = 2f;
			if (!this.isMechActive)
			{
				single += EffectsController.AddingForHeadshot(weaponSound.categoryNabor - 1);
			}
		}
		if (this.isMulti && !this.isCOOP && !this.isCompany && !Defs.isFlag && !Defs.isCapturePoints || (this.isCompany || Defs.isFlag || Defs.isCapturePoints) && this.myCommand != component.myCommand)
		{
			if (!Defs.isDaterRegim || !weaponSound.isDaterWeapon)
			{
				if (this.isMechActive)
				{
					typeKill = Player_move_c.TypeKills.mech;
				}
				else if (!isHeadShot)
				{
					typeKill = (!this.isZooming ? Player_move_c.TypeKills.none : Player_move_c.TypeKills.zoomingshot);
				}
				else
				{
					typeKill = Player_move_c.TypeKills.headshot;
				}
				Player_move_c.TypeKills typeKill1 = typeKill;
				float multyDamage = 0f;
				if (!weaponSound.isRoundMelee)
				{
					multyDamage = this.GetMultyDamage() * single * (1f + this.koofDamageWeaponFromPotoins + (!this.isMechActive ? EffectsController.DamageModifsByCats(weaponSound.categoryNabor - 1) : 0f));
				}
				else
				{
					if (!(ExpController.Instance != null) || ExpController.Instance.OurTier >= (int)weaponSound.DamageByTier.Length)
					{
						damageByTier = ((int)weaponSound.DamageByTier.Length <= 0 ? 0f : weaponSound.DamageByTier[0]);
					}
					else
					{
						damageByTier = weaponSound.DamageByTier[this.TierOrRoomTier(ExpController.Instance.OurTier)];
					}
					float single1 = damageByTier;
					float single2 = single1 * 0.7f;
					float single3 = single1;
					multyDamage = (single2 + (single3 - single2) * (1f - sqrDistance / (weaponSound.radiusRoundMelee * weaponSound.radiusRoundMelee))) * (1f + this.koofDamageWeaponFromPotoins + (!this.isMechActive ? EffectsController.DamageModifsByCats(weaponSound.categoryNabor - 1) : 0f));
				}
				if (weaponSound.isCharging)
				{
					multyDamage *= this.chargeValue;
				}
				PlayerScoreController playerScoreController = this.myScoreController;
				if (!component.isMechActive)
				{
					scoreEvent = (!isHeadShot ? PlayerEventScoreController.ScoreEvent.damageBody : PlayerEventScoreController.ScoreEvent.damageHead);
				}
				else
				{
					scoreEvent = (!isHeadShot ? PlayerEventScoreController.ScoreEvent.damageMechBody : PlayerEventScoreController.ScoreEvent.damageMechHead);
				}
				playerScoreController.AddScoreOnEvent(scoreEvent, multyDamage);
				if (this.isInet)
				{
					component.MinusLive(this.myPlayerID, multyDamage, typeKill1, (int)weaponSound.typeDead, (!this.isMechActive ? weaponSound.gameObject.name.Replace("(Clone)", string.Empty) : "Chat_Mech"), 0);
				}
				else
				{
					Player_move_c playerMoveC = component;
					NetworkViewID networkViewID = this.myPlayerIDLocal;
					float single4 = multyDamage;
					Player_move_c.TypeKills typeKill2 = typeKill1;
					WeaponSounds.TypeDead typeDead = weaponSound.typeDead;
					str = (!this.isMechActive ? weaponSound.gameObject.name.Replace("(Clone)", string.Empty) : "Chat_Mech");
					NetworkViewID networkViewID1 = new NetworkViewID();
					playerMoveC.MinusLive(networkViewID, single4, typeKill2, (int)typeDead, str, networkViewID1);
				}
			}
			else
			{
				component.SendDaterChat(this.mySkinName.NickName, weaponSound.daterMessage, component.mySkinName.NickName);
			}
		}
	}

	private void _HitTurret(GameObject _turret, float sqrDistance)
	{
		float damageByTier;
		if (Defs.isCOOP)
		{
			return;
		}
		WeaponSounds weaponSound = (!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		TurretController component = _turret.GetComponent<TurretController>();
		if (!component.isEnemyTurret)
		{
			return;
		}
		float multyDamage = 0f;
		if (!weaponSound.isRoundMelee)
		{
			multyDamage = this.GetMultyDamage() * (1f + this.koofDamageWeaponFromPotoins);
		}
		else
		{
			if (!(ExpController.Instance != null) || ExpController.Instance.OurTier >= (int)weaponSound.DamageByTier.Length)
			{
				damageByTier = ((int)weaponSound.DamageByTier.Length <= 0 ? 0f : weaponSound.DamageByTier[0]);
			}
			else
			{
				damageByTier = weaponSound.DamageByTier[this.TierOrRoomTier(ExpController.Instance.OurTier)];
			}
			float single = damageByTier;
			float single1 = single * 0.7f;
			float single2 = single;
			multyDamage = (single1 + (single2 - single1) * (1f - sqrDistance / (weaponSound.radiusRoundMelee * weaponSound.radiusRoundMelee))) * (1f + this.koofDamageWeaponFromPotoins + (!this.isMechActive ? EffectsController.DamageModifsByCats(weaponSound.categoryNabor - 1) : 0f));
		}
		if (weaponSound.isCharging)
		{
			multyDamage *= this.chargeValue;
		}
		this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.damageTurret, multyDamage);
		if (!Defs.isInet)
		{
			component.MinusLive(multyDamage, 0, this.myPlayerTransform.GetComponent<NetworkView>().viewID);
		}
		else
		{
			component.MinusLive(multyDamage, this.myPlayerTransform.GetComponent<PhotonView>().viewID, new NetworkViewID());
		}
	}

	private void _HitZombie(GameObject zmb, bool isHeadShot, float sqrDistance)
	{
		WeaponSounds weaponSound = (!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		float damageValueForTargetsInRadius = 0f;
		if (!weaponSound.isRoundMelee)
		{
			damageValueForTargetsInRadius = ((float)weaponSound.damage + UnityEngine.Random.Range(weaponSound.damageRange.x, weaponSound.damageRange.y)) * (1f + this.koofDamageWeaponFromPotoins + (!this.isMechActive ? EffectsController.DamageModifsByCats(weaponSound.categoryNabor - 1) : 0f));
		}
		else
		{
			damageValueForTargetsInRadius = this.GetDamageValueForTargetsInRadius(sqrDistance, weaponSound.radiusRoundMelee * weaponSound.radiusRoundMelee);
			UnityEngine.Debug.Log(damageValueForTargetsInRadius);
		}
		if (weaponSound.isCharging)
		{
			damageValueForTargetsInRadius *= this.chargeValue;
		}
		BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(zmb.transform.parent);
		if (!this.isMulti)
		{
			if (botScriptForObject == null)
			{
				TrainingEnemy componentInParent = zmb.GetComponentInParent<TrainingEnemy>();
				if (componentInParent != null)
				{
					componentInParent.ApplyDamage(damageValueForTargetsInRadius, isHeadShot);
				}
			}
			else
			{
				botScriptForObject.GetDamage(-damageValueForTargetsInRadius, this.myPlayerTransform, this.myCurrentWeaponSounds.name, true, isHeadShot);
			}
		}
		else if (this.isCOOP && !botScriptForObject.IsDeath)
		{
			botScriptForObject.GetDamageForMultiplayer(-damageValueForTargetsInRadius, null, this.myCurrentWeaponSounds.name, isHeadShot);
			this._weaponManager.myNetworkStartTable.score = GlobalGameController.Score;
			this._weaponManager.myNetworkStartTable.SynhScore();
		}
	}

	private void _SetGunFlashActive(bool state)
	{
		WeaponSounds weaponSound = (!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		if (weaponSound.isDoubleShot && !this._weaponManager.currentWeaponSounds.isMelee)
		{
			weaponSound.gunFlashDouble[this.numShootInDoubleShot - 1].GetChild(0).gameObject.SetActive(state);
			if (state)
			{
				return;
			}
		}
		if (this.GunFlash != null && !this._weaponManager.currentWeaponSounds.isMelee && (!this.isZooming || this.isZooming && !state))
		{
			WeaponManager.SetGunFlashActive(this.GunFlash.gameObject, state);
		}
	}

	private void _Shot()
	{
		if (!TrainingController.TrainingCompleted)
		{
			TrainingController.timeShowFire = 1000f;
			HintController.instance.HideHintByName("press_fire");
		}
		if (this.isGrenadePress || this.showChat)
		{
			return;
		}
		if (Defs.isMulti)
		{
			ProfileController.OnGameShoot();
		}
		float item = 0f;
		if (!this.isMechActive)
		{
			if (this._weaponManager.currentWeaponSounds.isDoubleShot)
			{
				int numShootInDouble = this.GetNumShootInDouble();
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play(string.Concat("Shoot", numShootInDouble));
				item = this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()[string.Concat("Shoot", numShootInDouble)].length;
			}
			else
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
				item = this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].length;
			}
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.shoot);
			}
		}
		else
		{
			int num = this.GetNumShootInDouble();
			this.mechGunAnimation.Play(string.Concat("Shoot", num));
			item = this.mechGunAnimation[string.Concat("Shoot", num)].length;
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.shootMechClip);
			}
		}
		if (this.inGameGUI != null)
		{
			this.inGameGUI.StartFireCircularIndicators(item);
		}
		this.shootS();
	}

	public bool _singleOrMultiMine()
	{
		return (!this.isMulti ? true : this.isMine);
	}

	public void ActivateBear()
	{
		if (this.isBearActive)
		{
			return;
		}
		float item = -1f;
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			item = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time;
		}
		this.mechPoint = this.mechBearPoint;
		this.mechBody = this.mechBearBody;
		this.mechBodyAnimation = this.mechBearBodyAnimation;
		this.mechGunAnimation = this.mechBearGunAnimation;
		this.mechBodyRenderer = this.mechBearBodyRenderer;
		this.mechHandRenderer = this.mechBearHandRenderer;
		this.shootMechClip = this.shootMechBearClip;
		this.mechExplossionSound = this.mechBearExplosionSound;
		this.mySkinName.walkMech = this.mySkinName.walkMechBear;
		this.mechExplossion = this.bearExplosion;
		if ((!Defs.isMulti || this.isMine) && this.isZooming)
		{
			this.ZoomPress();
		}
		this.deltaAngle = 0f;
		this.mechUpgrade = 0;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.mechBearActivSound);
		}
		this.isBearActive = true;
		this.fpsPlayerBody.SetActive(false);
		if (this.myCurrentWeapon != null)
		{
			this.SetWeaponVisible(false);
		}
		if (this.isMine || !this.isMine && !this.isInvisible || !this.isMulti)
		{
			this.mechPoint.SetActive(true);
		}
		this.mechPoint.GetComponent<DisableObjectFromTimer>().timer = -1f;
		if (!this.isMulti || this.isMine)
		{
			base.transform.localPosition = this.myCamera.transform.localPosition;
			this.mechBody.SetActive(false);
			this.mechBearSyncRot.enabled = true;
			this.mechPoint.transform.localPosition = Vector3.zero;
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
			if (this.myCurrentWeaponSounds.animationObject != null)
			{
				if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				}
				if (this.myCurrentWeaponSounds.isDoubleShot)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
				}
				else if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
				}
			}
		}
		else
		{
			this.bodyCollayder.height = 2.07f;
			this.bodyCollayder.center = new Vector3(0f, 0.19f, 0f);
			this.headCollayder.center = new Vector3(0f, 0.54f, 0f);
			if (!this.isBigHead)
			{
				this.nickLabel.transform.localPosition = Vector3.up * 1.54f;
			}
			else
			{
				this.nickLabel.transform.localPosition = 2.549f * Vector3.up;
			}
		}
		this.liveMech = this.liveMechByTier[0];
		this._mechMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		if (this.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("ActivateMechRPC", RPCMode.Others, new object[] { 0 });
			}
			else
			{
				this.photonView.RPC("ActivateMechRPC", PhotonTargets.Others, new object[] { 0 });
			}
		}
		if (item != -1f)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time = item;
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
	}

	public void ActivateMech(int num = 0)
	{
		if (this.isMechActive)
		{
			return;
		}
		if (Defs.isDaterRegim)
		{
			this.ActivateBear();
			return;
		}
		if ((!Defs.isMulti || this.isMine) && this.isZooming)
		{
			this.ZoomPress();
		}
		this.deltaAngle = 0f;
		this.mechUpgrade = num;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.mechActivSound);
		}
		this.ShotUnPressed(true);
		this.isMechActive = true;
		this.fpsPlayerBody.SetActive(false);
		if (this.myCurrentWeapon != null)
		{
			this.myCurrentWeapon.SetActive(false);
		}
		if (this.isMine || !this.isMine && !this.isInvisible || !this.isMulti)
		{
			this.mechPoint.SetActive(true);
		}
		this.mechPoint.GetComponent<DisableObjectFromTimer>().timer = -1f;
		this.myCamera.transform.localPosition = new Vector3(0.12f, 0.7f, -0.3f);
		if (!this.isMulti || this.isMine)
		{
			num = GearManager.CurrentNumberOfUphradesForGear(GearManager.Mech);
			this.mechBody.SetActive(false);
			this.mechBearSyncRot.enabled = true;
			this.mechPoint.transform.localPosition = new Vector3(0f, -0.3f, 0f);
			this.gunCamera.fieldOfView = 45f;
			this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			if (this.inGameGUI != null)
			{
				this.inGameGUI.fireButtonSprite.spriteName = "controls_fire";
				this.inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
			}
		}
		else
		{
			this.bodyCollayder.height = 2.07f;
			this.bodyCollayder.center = new Vector3(0f, 0.19f, 0f);
			this.headCollayder.center = new Vector3(0f, 0.54f, 0f);
			this.nickLabel.transform.localPosition = Vector3.up * 1.72f;
		}
		this.liveMech = this.liveMechByTier[num];
		if (!Defs.isDaterRegim)
		{
			this._mechMaterial = new Material(this.mechBodyMaterials[num]);
			this.mechBodyRenderer.sharedMaterial = this._mechMaterial;
			this.mechHandRenderer.sharedMaterial = this._mechMaterial;
			this.mechGunRenderer.material = this.mechGunMaterials[num];
		}
		if (Defs.isDaterRegim || !this.isInvisible || this.isMulti && !this.isMine)
		{
			this._mechMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		}
		else
		{
			this._mechMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			this.mechGunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
		}
		if (this.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("ActivateMechRPC", RPCMode.Others, new object[] { num });
			}
			else
			{
				this.photonView.RPC("ActivateMechRPC", PhotonTargets.Others, new object[] { num });
			}
		}
		if (!Defs.isDaterRegim)
		{
			for (int i = 0; i < (int)this.mechWeaponSounds.gunFlashDouble.Length; i++)
			{
				this.mechWeaponSounds.gunFlashDouble[i].GetChild(0).gameObject.SetActive(false);
			}
		}
		if ((!this.isMulti || this.isMine) && this.inGameGUI != null)
		{
			this.inGameGUI.SetCrosshair(this.mechWeaponSounds);
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
	}

	[PunRPC]
	[RPC]
	public void ActivateMechRPC(int num)
	{
		this.ActivateMech(num);
	}

	[PunRPC]
	[RPC]
	public void ActivateMechRPC()
	{
		this.ActivateMech(0);
	}

	private void ActualizeNumberOfGrenades()
	{
		if (!Defs.isHunger && !SceneLoader.ActiveSceneName.Equals(Defs.TrainingSceneName))
		{
			if (this.numberOfGrenades.Value != this.numberOfGrenadesOnStart.Value)
			{
				Storager.setInt((!Defs.isDaterRegim ? "GrenadeID" : "LikeID"), this.numberOfGrenades.Value, false);
				this.numberOfGrenadesOnStart.Value = this.numberOfGrenades.Value;
			}
		}
	}

	private void AddArmor(float dt)
	{
		if (this.WearedMaxArmor <= 0f)
		{
			float currentBaseArmor = this.maxBaseArmor - this.CurrentBaseArmor;
			if (currentBaseArmor < 0f)
			{
				currentBaseArmor = 0f;
			}
			if (dt > currentBaseArmor)
			{
				Player_move_c playerMoveC = this;
				playerMoveC.CurrentBaseArmor = playerMoveC.CurrentBaseArmor + currentBaseArmor;
			}
			else
			{
				Player_move_c currentBaseArmor1 = this;
				currentBaseArmor1.CurrentBaseArmor = currentBaseArmor1.CurrentBaseArmor + dt;
			}
		}
		else
		{
			float single = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN, false), this.TierOrRoomTier((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier)));
			float currentBodyArmor = single - this.CurrentBodyArmor;
			if (currentBodyArmor < 0f)
			{
				currentBodyArmor = 0f;
			}
			if (dt > currentBodyArmor)
			{
				Player_move_c currentBodyArmor1 = this;
				currentBodyArmor1.CurrentBodyArmor = currentBodyArmor1.CurrentBodyArmor + currentBodyArmor;
				dt -= currentBodyArmor;
				float single1 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN, false), this.TierOrRoomTier((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier)));
				float currentHatArmor = single1 - this.CurrentHatArmor;
				if (currentHatArmor < 0f)
				{
					currentHatArmor = 0f;
				}
				Player_move_c currentHatArmor1 = this;
				currentHatArmor1.CurrentHatArmor = currentHatArmor1.CurrentHatArmor + Mathf.Min(currentHatArmor, dt);
			}
			else
			{
				Player_move_c playerMoveC1 = this;
				playerMoveC1.CurrentBodyArmor = playerMoveC1.CurrentBodyArmor + dt;
			}
		}
	}

	public void AddButtonHandlers()
	{
		PauseTapReceiver.PauseClicked += new Action(this.SwitchPause);
		ShopTapReceiver.ShopClicked += new Action(this.ShopPressed);
		RanksTapReceiver.RanksClicked += new Action(this.RanksPressed);
		TopPanelsTapReceiver.OnClicked += new Action(this.RanksPressed);
		ChatTapReceiver.ChatClicked += new Action(this.ShowChat);
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(true);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(true);
		}
	}

	private void AddCountSerials(int categoryNabor, Player_move_c killerPlayerMoveC)
	{
		killerPlayerMoveC.counterSerials[categoryNabor]++;
		switch (killerPlayerMoveC.counterSerials[categoryNabor])
		{
			case 1:
			{
				if (categoryNabor == 2)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee, 1f);
				}
				return;
			}
			case 2:
			{
				if (categoryNabor == 2)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee2, 1f);
				}
				return;
			}
			case 3:
			{
				if (categoryNabor == 0)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary1, 1f);
				}
				if (categoryNabor == 1)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup1, 1f);
				}
				if (categoryNabor == 2)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee3, 1f);
				}
				if (categoryNabor == 3)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special1, 1f);
				}
				if (categoryNabor == 4)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper1, 1f);
				}
				if (categoryNabor == 5)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium1, 1f);
				}
				return;
			}
			case 4:
			case 6:
			{
				return;
			}
			case 5:
			{
				if (categoryNabor == 0)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary2, 1f);
				}
				if (categoryNabor == 1)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup2, 1f);
				}
				if (categoryNabor == 2)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee5, 1f);
				}
				if (categoryNabor == 3)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special2, 1f);
				}
				if (categoryNabor == 4)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper2, 1f);
				}
				if (categoryNabor == 5)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium2, 1f);
				}
				return;
			}
			case 7:
			{
				if (categoryNabor == 0)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary3, 1f);
				}
				if (categoryNabor == 1)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup3, 1f);
				}
				if (categoryNabor == 2)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee7, 1f);
				}
				if (categoryNabor == 3)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special3, 1f);
				}
				if (categoryNabor == 4)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper3, 1f);
				}
				if (categoryNabor == 5)
				{
					killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium3, 1f);
				}
				return;
			}
			default:
			{
				return;
			}
		}
	}

	[PunRPC]
	[RPC]
	public void AddFreezerRayWithLength(float len)
	{
		Transform gunFlash = this.GunFlash;
		if (gunFlash == null && this.myTransform.childCount > 0)
		{
			FlashFire component = this.myTransform.GetChild(0).GetComponent<FlashFire>();
			if (component != null && component.gunFlashObj != null)
			{
				gunFlash = component.gunFlashObj.transform;
			}
		}
		if (gunFlash != null)
		{
			if (this.FreezerFired == null)
			{
				GameObject gameObject = WeaponManager.AddRay(gunFlash.gameObject.transform.parent.position, gunFlash.gameObject.transform.parent.parent.forward, gunFlash.gameObject.transform.parent.parent.GetComponent<WeaponSounds>().railName, len);
				if (gameObject != null)
				{
					FreezerRay freezerRay = gameObject.GetComponent<FreezerRay>();
					if (freezerRay != null)
					{
						freezerRay.SetParentMoveC(this);
					}
				}
			}
			else
			{
				this.FreezerFired(len);
			}
		}
	}

	public void AddMessage(string text, float time, int ID, NetworkViewID IDLocal, int _command, string clanLogo, string iconName)
	{
		Player_move_c.MessageChat messageChat = new Player_move_c.MessageChat()
		{
			text = text,
			iconName = iconName,
			time = time,
			ID = ID,
			IDLocal = IDLocal,
			command = _command
		};
		if (string.IsNullOrEmpty(clanLogo))
		{
			messageChat.clanLogo = null;
		}
		else
		{
			byte[] numArray = Convert.FromBase64String(clanLogo);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(numArray);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			messageChat.clanLogo = texture2D;
		}
		this.messages.Add(messageChat);
		if (this.messages.Count > 30)
		{
			this.messages.RemoveAt(0);
		}
		Player_move_c.OnMessagesUpdate onMessagesUpdate = this.messageDelegate;
		if (onMessagesUpdate != null)
		{
			onMessagesUpdate();
		}
	}

	public void addMultyKill()
	{
		this.multiKill++;
		if (this.multiKill > 1)
		{
			if (this.multiKill > 1 && !NetworkStartTable.LocalOrPasswordRoom())
			{
				QuestMediator.NotifyMakeSeries();
			}
			int num = this.multiKill;
			switch (num)
			{
				case 2:
				{
					this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill2, 1f);
					break;
				}
				case 3:
				{
					this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill3, 1f);
					break;
				}
				case 4:
				{
					this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill4, 1f);
					break;
				}
				case 5:
				{
					this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill5, 1f);
					break;
				}
				case 6:
				{
					this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill6, 1f);
					break;
				}
				case 10:
				{
					this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill10, 1f);
					break;
				}
				default:
				{
					if (num == 20)
					{
						this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill20, 1f);
						break;
					}
					else if (num == 50)
					{
						this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.multyKill50, 1f);
						break;
					}
					else
					{
						break;
					}
				}
			}
			if (Defs.isMulti)
			{
				if (!Defs.isInet)
				{
					base.GetComponent<NetworkView>().RPC("ShowMultyKillRPC", RPCMode.Others, new object[] { this.multiKill });
				}
				else
				{
					this.photonView.RPC("ShowMultyKillRPC", PhotonTargets.Others, new object[] { this.multiKill });
				}
			}
		}
	}

	public void AddScoreDuckHunt()
	{
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("AddScoreDuckHuntRPC", RPCMode.All, new object[0]);
		}
		else
		{
			this.photonView.RPC("AddScoreDuckHuntRPC", PhotonTargets.All, new object[0]);
		}
	}

	[PunRPC]
	[RPC]
	public void AddScoreDuckHuntRPC()
	{
		if (this.isMine)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.duckHunt, 1f);
		}
	}

	public void AddSystemMessage(string _nick1, string _message2, string _nick2, string _message = null)
	{
		this.AddSystemMessage(_nick1, _message2, _nick2, Color.white, _message);
	}

	public void AddSystemMessage(string _nick1, string _message2, string _nick2, Color color, string _message = null)
	{
		this.killedSpisok[2] = this.killedSpisok[1];
		this.killedSpisok[1] = this.killedSpisok[0];
		this.killedSpisok[0] = new Player_move_c.SystemMessage(_nick1, _message2, _nick2, _message, color);
		this.timerShow[2] = this.timerShow[1];
		this.timerShow[1] = this.timerShow[0];
		this.timerShow[0] = 3f;
	}

	public void AddSystemMessage(string nick1, int _typeKills, Color color)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], string.Empty, color, null);
	}

	public void AddSystemMessage(string nick1, int _typeKills)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], string.Empty, null);
	}

	public void AddSystemMessage(string nick1, int _typeKills, string nick2, Color color, string iconWeapon = null)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], nick2, color, iconWeapon);
	}

	public void AddSystemMessage(string nick1, int _typeKills, string nick2, string iconWeapon = null)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], nick2, iconWeapon);
	}

	public void AddSystemMessage(string _message)
	{
		this.AddSystemMessage(_message, string.Empty, string.Empty, null);
	}

	public void AddSystemMessage(string _message, Color color)
	{
		this.AddSystemMessage(_message, string.Empty, string.Empty, color, null);
	}

	public void AddWeapon(GameObject weaponPrefab)
	{
		int num;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			int num1 = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToList<Weapon>().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == weaponPrefab.GetComponent<WeaponSounds>().categoryNabor);
			if (num1 >= 0)
			{
				this.ChangeWeapon(num1, false);
			}
			return;
		}
		if (this._weaponManager.AddWeapon(weaponPrefab, out num))
		{
			this.ChangeWeapon(this._weaponManager.CurrentWeaponIndex, false);
		}
		else if (!ItemDb.IsWeaponCanDrop(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag))
		{
			IEnumerator enumerator = this._weaponManager.playerWeapons.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Weapon current = (Weapon)enumerator.Current;
					if (current.weaponPrefab != weaponPrefab)
					{
						continue;
					}
					this.ChangeWeapon(this._weaponManager.playerWeapons.IndexOf(current), false);
					break;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}
		else
		{
			GlobalGameController.Score = GlobalGameController.Score + num;
			if (Defs.isSoundFX)
			{
				if (this.WeaponBonusClip == null)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.ChangeWeaponClip);
				}
				else
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.WeaponBonusClip);
				}
			}
		}
	}

	private void AddWeaponToInv(string shopId)
	{
		string tagByShopId = ItemDb.GetTagByShopId(shopId);
		ItemRecord byTag = ItemDb.GetByTag(tagByShopId);
		if ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && byTag != null && !byTag.TemporaryGun)
		{
			Player_move_c.SaveWeaponInPrefs(tagByShopId, 0);
		}
		this.AddWeapon(this._weaponManager.GetPrefabByTag(tagByShopId));
	}

	private void AddWeKillStatisctics(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			UnityEngine.Debug.LogError("AddWeKillStatisctics string.IsNullOrEmpty (weaponName)");
			return;
		}
		if (!this.weKillForKillRate.ContainsKey(weaponName))
		{
			this.weKillForKillRate.Add(weaponName, 1);
		}
		else
		{
			Dictionary<string, int> item = this.weKillForKillRate;
			Dictionary<string, int> strs = item;
			string str = weaponName;
			item[str] = strs[str] + 1;
		}
	}

	private void AddWeWereKilledStatisctics(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			UnityEngine.Debug.LogError("AddWeWereKilledStatisctics string.IsNullOrEmpty (weaponName)");
			return;
		}
		if (!this.weWereKilledForKillRate.ContainsKey(weaponName))
		{
			this.weWereKilledForKillRate.Add(weaponName, 1);
		}
		else
		{
			Dictionary<string, int> item = this.weWereKilledForKillRate;
			Dictionary<string, int> strs = item;
			string str = weaponName;
			item[str] = strs[str] + 1;
		}
	}

	private void Awake()
	{
		this.isSurvival = Defs.IsSurvival;
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.isCompany = Defs.isCompany;
		this.isCOOP = Defs.isCOOP;
		this.isHunger = Defs.isHunger;
		if (this.isHunger)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("HungerGameController");
			if (gameObject != null)
			{
				this.hungerGameController = gameObject.GetComponent<HungerGameController>();
			}
			else
			{
				UnityEngine.Debug.LogError("hungerGameControllerObject == null");
			}
		}
		this.myCamera.fieldOfView = this.stdFov;
	}

	public void BackRanksPressed()
	{
		this.AddButtonHandlers();
		this.showRanks = false;
		if (this.inGameGUI != null && this.inGameGUI.interfacePanel != null)
		{
			this.inGameGUI.gameObject.SetActive(true);
		}
	}

	[DebuggerHidden]
	private IEnumerator BazookaShoot()
	{
		Player_move_c.u003cBazookaShootu003ec__IteratorE6 variable = null;
		return variable;
	}

	public void BlockPlayerInEnd()
	{
		this.mySkinName.BlockFirstPersonController();
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().enabled = false;
		if (this.GunFlash != null)
		{
			this.GunFlash.gameObject.SetActive(false);
		}
		this.mySkinName.character.enabled = false;
		base.enabled = false;
	}

	private void BulletShot(WeaponSounds weapon)
	{
		RaycastHit raycastHit;
		int num = (!weapon.isShotGun ? 1 : weapon.countShots);
		float single = (!weapon.isShotGun ? 100f : 30f);
		Vector3[] vector3Array = null;
		Quaternion[] quaternionArray = null;
		bool[] flagArray = null;
		int num1 = Mathf.Min(7, num);
		bool flag = false;
		bool flag1 = false;
		Vector3 vector3 = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		if (weapon.bulletExplode)
		{
			single = 250f;
		}
		for (int i = 0; i < num; i++)
		{
			float single1 = weapon.tekKoof * Defs.Coef;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(((float)Screen.width - weapon.startZone.x * single1) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.x * single1)), ((float)Screen.height - weapon.startZone.y * single1) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.y * single1)), 0f));
			if ((!weapon.isDoubleShot ? this.GunFlash : weapon.gunFlashDouble[this.numShootInDoubleShot - 1]) != null && !Defs.isDaterRegim)
			{
				GameObject currentBullet = BulletStackController.sharedController.GetCurrentBullet((int)weapon.typeTracer);
				if (currentBullet != null)
				{
					currentBullet.transform.rotation = this.myTransform.rotation;
					Bullet component = currentBullet.GetComponent<Bullet>();
					component.endPos = ray.GetPoint(200f);
					component.startPos = (!weapon.isDoubleShot ? this.GunFlash.position : weapon.gunFlashDouble[this.numShootInDoubleShot - 1].position);
					component.StartBullet();
				}
				weapon.fire();
			}
			if (Physics.Raycast(ray, out raycastHit, single, Player_move_c._ShootRaycastLayerMask))
			{
				if (weapon.bulletExplode)
				{
					Rocket rocket = Player_move_c.CreateRocket(raycastHit.point, Quaternion.identity, this.koofDamageWeaponFromPotoins, this.isMulti, this.isInet, this.TierOrRoomTier((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier)));
					rocket.dontExecStart = true;
					rocket.SendSetRocketActiveRPC();
					rocket.KillRocket(raycastHit.collider);
				}
				else
				{
					if (!raycastHit.collider.gameObject.transform.CompareTag("DamagedExplosion"))
					{
						vector3 = raycastHit.point + (raycastHit.normal * 0.001f);
						rotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
						flag1 = true;
						flag = (!(raycastHit.collider.gameObject.transform.parent != null) || raycastHit.collider.gameObject.transform.parent.CompareTag("Enemy") || raycastHit.collider.gameObject.transform.parent.CompareTag("Player") ? true : false);
						this.HoleRPC(flag, vector3, rotation);
						if (this.isMulti)
						{
							if (!this.isInet)
							{
								this._networkView.RPC("HoleRPC", RPCMode.Others, new object[] { flag, vector3, rotation });
							}
							else if (num1 > 1 && i < num1)
							{
								if (vector3Array == null)
								{
									vector3Array = new Vector3[num1];
									quaternionArray = new Quaternion[num1];
									flagArray = new bool[num1];
								}
								vector3Array[i] = vector3;
								quaternionArray[i] = rotation;
								flagArray[i] = flag;
							}
						}
					}
					this._DoHit(raycastHit, false);
				}
			}
		}
		if (!flag1 || !this.isInet)
		{
			this._FireFlash(true, (!weapon.isDoubleShot ? 0 : this.numShootInDoubleShot));
		}
		else if (num1 <= 1)
		{
			this._FireFlashWithHole(flag, vector3, rotation, true, (!weapon.isDoubleShot ? 0 : this.numShootInDoubleShot));
		}
		else
		{
			this._FireFlashWithManyHoles(flagArray, vector3Array, quaternionArray, true, (!weapon.isDoubleShot ? 0 : this.numShootInDoubleShot));
		}
	}

	public void CancelTurret()
	{
		this.ChangeWeapon(this.currentWeaponBeforeTurret, false);
		this.currentWeaponBeforeTurret = -1;
		if (!Defs.isMulti)
		{
			UnityEngine.Object.Destroy(this.currentTurret);
		}
		else if (!Defs.isInet)
		{
			Network.RemoveRPCs(this.currentTurret.GetComponent<NetworkView>().viewID);
			Network.Destroy(this.currentTurret);
		}
		else
		{
			PhotonNetwork.Destroy(this.currentTurret);
		}
	}

	[Obfuscation(Exclude=true)]
	private void ChangePositionAfterRespawn()
	{
		Transform transforms = this.myPlayerTransform;
		transforms.position = transforms.position + (Vector3.forward * 0.01f);
	}

	public void ChangeWeapon(int index, bool shouldSetMaxAmmo = true)
	{
		if (index == 1001)
		{
			this.currentWeaponBeforeTurret = WeaponManager.sharedManager.CurrentWeaponIndex;
		}
		this.indexWeapon = index;
		this.shouldSetMaxAmmoWeapon = shouldSetMaxAmmo;
		base.StopCoroutine("ChangeWeaponCorutine");
		base.StopCoroutine(this.BazookaShoot());
		base.StartCoroutine("ChangeWeaponCorutine");
		if (base.GetComponent<AudioSource>() != null && !this.isMechActive)
		{
			base.GetComponent<AudioSource>().Stop();
		}
	}

	[DebuggerHidden]
	private IEnumerator ChangeWeaponCorutine()
	{
		Player_move_c.u003cChangeWeaponCorutineu003ec__IteratorD6 variable = null;
		return variable;
	}

	public void ChangeWeaponReal(int index, bool shouldSetMaxAmmo = true)
	{
		GameObject gameObject;
		GameObject gameObject1 = null;
		if (index != 1000)
		{
			gameObject = (index != 1001 ? ((Weapon)this._weaponManager.playerWeapons[index]).weaponPrefab : this.turretPrefab);
		}
		else
		{
			gameObject = (!Defs.isDaterRegim ? this.grenadePrefab : this.likePrefab);
		}
		GameObject gameObject2 = gameObject;
		string str = ResPath.Combine(Defs.InnerWeaponsFolder, string.Concat(gameObject2.name.Replace("(Clone)", string.Empty), Defs.InnerWeapons_Suffix));
		GameObject gameObject3 = LoadAsyncTool.Get(str, true).asset as GameObject;
		gameObject1 = (GameObject)UnityEngine.Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity);
		gameObject1.GetComponent<WeaponSounds>().Initialize(gameObject3);
		this.ChangeWeaponReal(gameObject2, gameObject1, index, shouldSetMaxAmmo);
	}

	public void ChangeWeaponReal(GameObject _weaponPrefab, GameObject nw, int index, bool shouldSetMaxAmmo = true)
	{
		GameObject gameObject;
		float damageByTier;
		if (this.inGameGUI != null)
		{
			this.inGameGUI.StopAllCircularIndicators();
		}
		EventHandler<EventArgs> weaponChanged = this.WeaponChanged;
		if (weaponChanged != null)
		{
			weaponChanged(this, EventArgs.Empty);
		}
		if (this.isZooming)
		{
			this.ZoomPress();
		}
		this.photonView = PhotonView.Get(this);
		this._networkView = base.GetComponent<NetworkView>();
		Quaternion quaternion = Quaternion.identity;
		if (this._player)
		{
			quaternion = this._player.transform.rotation;
		}
		this.ShotUnPressed(true);
		if (this._weaponManager.currentWeaponSounds)
		{
			quaternion = this._weaponManager.currentWeaponSounds.gameObject.transform.rotation;
			this._SetGunFlashActive(false);
			this._weaponManager.currentWeaponSounds.gameObject.transform.parent = null;
			UnityEngine.Object.Destroy(this._weaponManager.currentWeaponSounds.gameObject);
			this._weaponManager.currentWeaponSounds = null;
		}
		this.ResetShootingBurst();
		this.myCurrentWeapon = nw;
		this.myCurrentWeaponSounds = this.myCurrentWeapon.GetComponent<WeaponSounds>();
		if (!ShopNGUIController.GuiActive && this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		if (!this.myCurrentWeaponSounds.isDoubleShot || this.isMechActive)
		{
			this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
		}
		else
		{
			this.gunCamera.transform.localPosition = Vector3.zero;
		}
		nw.transform.parent = base.gameObject.transform;
		nw.transform.rotation = quaternion;
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
		if (this.isMechActive)
		{
			this.myCurrentWeapon.SetActive(false);
		}
		if (Defs.isDaterRegim)
		{
			this.SetWeaponVisible(!this.isBearActive);
		}
		if (this.myCurrentWeaponSounds != null && PhotonNetwork.room != null)
		{
			Statistics.Instance.IncrementWeaponPopularity(LocalizationStore.GetByDefault(this.myCurrentWeaponSounds.localizeWeaponKey), false);
			this._weaponPopularityCacheIsDirty = true;
		}
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				base.GetComponent<NetworkView>().RPC("SetWeaponRPC", RPCMode.OthersBuffered, new object[] { _weaponPrefab.name, _weaponPrefab.GetComponent<WeaponSounds>().alternativeName });
			}
			else
			{
				this.photonView.RPC("SetWeaponRPC", PhotonTargets.Others, new object[] { _weaponPrefab.name, _weaponPrefab.GetComponent<WeaponSounds>().alternativeName });
			}
		}
		if (index == 1000)
		{
			WeaponSounds component = _weaponPrefab.GetComponent<WeaponSounds>();
			GameObject rocket = RocketStack.sharedController.GetRocket();
			if (rocket != null)
			{
				Rocket grenadeExplosionDamageIncreaseCoef = rocket.GetComponent<Rocket>();
				grenadeExplosionDamageIncreaseCoef.rocketNum = (!Defs.isDaterRegim ? 10 : 40);
				grenadeExplosionDamageIncreaseCoef.weaponName = (!Defs.isDaterRegim ? "WeaponGrenade" : "WeaponLike");
				grenadeExplosionDamageIncreaseCoef.weaponPrefabName = grenadeExplosionDamageIncreaseCoef.weaponName;
				grenadeExplosionDamageIncreaseCoef.damage = (float)component.damage * (1f + this.koofDamageWeaponFromPotoins + EffectsController.GrenadeExplosionDamageIncreaseCoef);
				grenadeExplosionDamageIncreaseCoef.radiusDamage = component.bazookaExplosionRadius * EffectsController.GrenadeExplosionRadiusIncreaseCoef;
				grenadeExplosionDamageIncreaseCoef.radiusDamageSelf = component.bazookaExplosionRadiusSelf;
				grenadeExplosionDamageIncreaseCoef.radiusImpulse = component.bazookaImpulseRadius * (1f + EffectsController.ExplosionImpulseRadiusIncreaseCoef);
				grenadeExplosionDamageIncreaseCoef.impulseForce = component.impulseForce;
				grenadeExplosionDamageIncreaseCoef.impulseForceSelf = component.impulseForceSelf;
				grenadeExplosionDamageIncreaseCoef.damageRange = component.damageRange * (1f + this.koofDamageWeaponFromPotoins);
				if (!(ExpController.Instance != null) || ExpController.Instance.OurTier >= (int)component.DamageByTier.Length)
				{
					damageByTier = ((int)component.DamageByTier.Length <= 0 ? 0f : component.DamageByTier[0]);
				}
				else
				{
					damageByTier = component.DamageByTier[this.TierOrRoomTier(ExpController.Instance.OurTier)];
				}
				float single = damageByTier;
				grenadeExplosionDamageIncreaseCoef.multiplayerDamage = single * (1f + this.koofDamageWeaponFromPotoins + EffectsController.GrenadeExplosionDamageIncreaseCoef);
				rocket.GetComponent<Rigidbody>().useGravity = false;
				rocket.GetComponent<Rigidbody>().isKinematic = true;
				grenadeExplosionDamageIncreaseCoef.SendSetRocketActiveRPC();
				if (Defs.isMulti && !Defs.isInet)
				{
					grenadeExplosionDamageIncreaseCoef.SendNetworkViewMyPlayer(this.myPlayerTransform.GetComponent<NetworkView>().viewID);
				}
			}
			this.currentGrenade = rocket;
		}
		if (index == 1001)
		{
			Defs.isTurretWeapon = true;
			this.turretUpgrade = GearManager.CurrentNumberOfUphradesForGear(GearManager.Turret);
			if (this.isMulti)
			{
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("SyncTurretUpgrade", RPCMode.Others, new object[] { this.turretUpgrade });
				}
				else
				{
					this.photonView.RPC("SyncTurretUpgrade", PhotonTargets.Others, new object[] { this.turretUpgrade });
				}
			}
			if (!this.isMulti)
			{
				GameObject gameObject1 = Resources.Load((!Defs.isDaterRegim ? "Turret" : "MusicBox")) as GameObject;
				gameObject = UnityEngine.Object.Instantiate(gameObject1, new Vector3(-10000f, -10000f, -10000f), base.transform.rotation) as GameObject;
			}
			else if (this.isInet)
			{
				gameObject = PhotonNetwork.Instantiate((!Defs.isDaterRegim ? "Turret" : "MusicBox"), new Vector3(-10000f, -10000f, -10000f), base.transform.rotation, 0);
			}
			else
			{
				UnityEngine.Object obj = Resources.Load((!Defs.isDaterRegim ? "Turret" : "MusicBox"));
				gameObject = (GameObject)Network.Instantiate(obj, new Vector3(-10000f, -10000f, -10000f), base.transform.rotation, 0);
			}
			if (gameObject != null)
			{
				TurretController turretController = gameObject.GetComponent<TurretController>();
				gameObject.GetComponent<Rigidbody>().useGravity = false;
				gameObject.GetComponent<Rigidbody>().isKinematic = true;
				if (Defs.isMulti && !Defs.isInet)
				{
					turretController.SendNetworkViewMyPlayer(this.myPlayerTransform.GetComponent<NetworkView>().viewID);
				}
			}
			this.currentTurret = gameObject;
		}
		if (!this.myCurrentWeaponSounds.isMelee)
		{
			IEnumerator enumerator = nw.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform current = (Transform)enumerator.Current;
					if (!current.gameObject.name.Equals("BulletSpawnPoint") || current.childCount <= 0)
					{
						continue;
					}
					WeaponManager.SetGunFlashActive(current.GetChild(0).gameObject, false);
					break;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}
		this.SetTextureForBodyPlayer(this._skin);
		Player_move_c.SetLayerRecursively(nw, 9);
		this._weaponManager.currentWeaponSounds = this.myCurrentWeaponSounds;
		if (index < 1000)
		{
			this._weaponManager.CurrentWeaponIndex = index;
			this._weaponManager.SaveWeaponAsLastUsed(this._weaponManager.CurrentWeaponIndex);
			if (this.inGameGUI != null)
			{
				if (!this._weaponManager.currentWeaponSounds.isMelee || this._weaponManager.currentWeaponSounds.isShotMelee || this.isMechActive)
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_fire";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
				}
				else
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_strike";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_strike";
				}
			}
		}
		if (nw.transform.parent == null)
		{
			UnityEngine.Debug.LogWarning("nw.transform.parent == null");
		}
		else if (this._weaponManager.currentWeaponSounds != null)
		{
			nw.transform.position = nw.transform.parent.TransformPoint(this._weaponManager.currentWeaponSounds.gunPosition);
		}
		else
		{
			UnityEngine.Debug.LogWarning("_weaponManager.currentWeaponSounds == null");
		}
		TouchPadController touchPadController = JoystickController.rightJoystick;
		if (index < 1000 && touchPadController != null)
		{
			if (((Weapon)this._weaponManager.playerWeapons[index]).currentAmmoInClip > 0 || this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee)
			{
				touchPadController.HasAmmo();
				if (this.inGameGUI != null)
				{
					this.inGameGUI.BlinkNoAmmo(0);
				}
			}
			else
			{
				touchPadController.NoAmmo();
				if (this.inGameGUI != null)
				{
					this.inGameGUI.BlinkNoAmmo(1);
				}
			}
		}
		if (this._weaponManager.currentWeaponSounds.animationObject != null)
		{
			if (this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
			}
			if (this._weaponManager.currentWeaponSounds.isDoubleShot)
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
			}
			else if (this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
			}
		}
		if (!this._weaponManager.currentWeaponSounds.isMelee)
		{
			IEnumerator enumerator1 = this._weaponManager.currentWeaponSounds.gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator1.MoveNext())
				{
					Transform transforms = (Transform)enumerator1.Current;
					if (!transforms.name.Equals("BulletSpawnPoint"))
					{
						continue;
					}
					this._bulletSpawnPoint = transforms.gameObject;
					break;
				}
			}
			finally
			{
				IDisposable disposable1 = enumerator1 as IDisposable;
				if (disposable1 == null)
				{
				}
				disposable1.Dispose();
			}
			this.GunFlash = this._bulletSpawnPoint.transform.GetChild(0);
		}
		if (Defs.isSoundFX && !Defs.isDaterRegim && !this.isMechActive)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot((index != 1000 ? this.ChangeWeaponClip : this.ChangeGrenadeClip));
		}
		if (!Defs.isDaterRegim && this.isInvisible)
		{
			this.SetInVisibleShaders(this.isInvisible);
		}
		if (this.inGameGUI != null)
		{
			if (!this.isMechActive)
			{
				this.inGameGUI.SetCrosshair(this._weaponManager.currentWeaponSounds);
			}
			else
			{
				this.inGameGUI.SetCrosshair(this.mechWeaponSounds);
			}
		}
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
		if (this.myCurrentWeaponSounds.isZooming && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showZoomHint)
		{
			base.Invoke("TrainingShowZoomHint", 3f);
		}
	}

	private void CheckRookieKillerAchievement()
	{
		int num = this.oldKilledPlayerCharactersCount + 1;
		if (num <= 15)
		{
			Storager.setInt("KilledPlayerCharactersCount", num, false);
		}
		this.oldKilledPlayerCharactersCount = num;
		if (Social.localUser.authenticated && !Storager.hasKey("RookieKillerAchievmentCompleted") && num >= 15)
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				GpgFacade instance = GpgFacade.Instance;
				instance.IncrementAchievement("CgkIr8rGkPIJEAIQBw", 1, (bool success) => UnityEngine.Debug.Log(string.Concat("Achievement Rookie Killer incremented: ", success)));
			}
			Storager.setInt("RookieKillerAchievmentCompleted", 1, false);
		}
	}

	private void CheckTimeCondition()
	{
		CampaignLevel campaignLevel = null;
		foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
		{
			if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
			{
				continue;
			}
			foreach (CampaignLevel level in campaignBox.levels)
			{
				if (!level.sceneName.Equals(CurrentCampaignGame.levelSceneName))
				{
					continue;
				}
				campaignLevel = level;
				break;
			}
			break;
		}
		float single = campaignLevel.timeToComplete;
		if (this.inGameTime >= single)
		{
			CurrentCampaignGame.completeInTime = false;
		}
	}

	[PunRPC]
	[RPC]
	private void CountKillsCommandSynch(int _blue, int _red)
	{
		GlobalGameController.countKillsBlue = _blue;
		GlobalGameController.countKillsRed = _red;
	}

	public static Rocket CreateRocket(Vector3 pos, Quaternion rot, float customDamageAdd, bool isMulti, bool isInet, int tierOrRoomTier)
	{
		float damageByTier;
		GameObject rocket = null;
		rocket = RocketStack.sharedController.GetRocket();
		rocket.transform.position = pos;
		rocket.transform.rotation = rot;
		Rocket component = rocket.GetComponent<Rocket>();
		component.rocketNum = WeaponManager.sharedManager.currentWeaponSounds.rocketNum;
		component.weaponPrefabName = WeaponManager.sharedManager.currentWeaponSounds.gameObject.name.Replace("(Clone)", string.Empty);
		component.weaponName = WeaponManager.sharedManager.currentWeaponSounds.bazookaExplosionName;
		component.damage = (float)WeaponManager.sharedManager.currentWeaponSounds.damage * (1f + customDamageAdd + EffectsController.DamageModifsByCats(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1));
		component.radiusDamage = WeaponManager.sharedManager.currentWeaponSounds.bazookaExplosionRadius;
		component.radiusDamageSelf = WeaponManager.sharedManager.currentWeaponSounds.bazookaExplosionRadiusSelf;
		component.radiusImpulse = WeaponManager.sharedManager.currentWeaponSounds.bazookaImpulseRadius * (1f + EffectsController.ExplosionImpulseRadiusIncreaseCoef);
		component.damageRange = WeaponManager.sharedManager.currentWeaponSounds.damageRange * (1f + customDamageAdd + EffectsController.DamageModifsByCats(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1));
		component.isSlowdown = WeaponManager.sharedManager.currentWeaponSounds.isSlowdown;
		component.slowdownCoeff = WeaponManager.sharedManager.currentWeaponSounds.slowdownCoeff;
		component.slowdownTime = WeaponManager.sharedManager.currentWeaponSounds.slowdownTime;
		component.impulseForce = WeaponManager.sharedManager.currentWeaponSounds.impulseForce;
		component.impulseForceSelf = WeaponManager.sharedManager.currentWeaponSounds.impulseForceSelf;
		if (!(ExpController.Instance != null) || ExpController.Instance.OurTier >= (int)WeaponManager.sharedManager.currentWeaponSounds.DamageByTier.Length)
		{
			damageByTier = ((int)WeaponManager.sharedManager.currentWeaponSounds.DamageByTier.Length <= 0 ? 0f : WeaponManager.sharedManager.currentWeaponSounds.DamageByTier[0]);
		}
		else
		{
			damageByTier = WeaponManager.sharedManager.currentWeaponSounds.DamageByTier[tierOrRoomTier];
		}
		component.multiplayerDamage = damageByTier;
		rocket.GetComponent<Rigidbody>().useGravity = WeaponManager.sharedManager.currentWeaponSounds.grenadeLauncher;
		return component;
	}

	private bool DamagePlayerAndCheckDeath(float damage)
	{
		if (!this.isMechActive)
		{
			if (this.curArmor < damage)
			{
				Player_move_c curHealth = this;
				curHealth.CurHealth = curHealth.CurHealth - (damage - this.curArmor);
				this.curArmor = 0f;
				CurrentCampaignGame.withoutHits = false;
			}
			else
			{
				Player_move_c playerMoveC = this;
				playerMoveC.curArmor = playerMoveC.curArmor - damage;
			}
			if (this.CurHealth <= 0f)
			{
				return true;
			}
		}
		else
		{
			this.MinusMechHealth(damage);
		}
		return false;
	}

	public void DeactivateBear()
	{
		if (!this.isBearActive)
		{
			return;
		}
		this.isBearActive = false;
		float item = -1f;
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			item = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time;
		}
		if (this.myCurrentWeapon != null)
		{
			this.SetWeaponVisible(true);
		}
		this.myCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
		if (Defs.isSoundFX)
		{
			this.mechExplossionSound.Play();
		}
		if (!this.isMulti || this.isMine)
		{
			this.mechPoint.SetActive(false);
			this.gunCamera.fieldOfView = 75f;
			base.transform.localPosition = this.myCamera.transform.localPosition;
			this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
		}
		else
		{
			if (!this.isInvisible)
			{
				this.fpsPlayerBody.SetActive(true);
			}
			this.bodyCollayder.height = 1.51f;
			this.bodyCollayder.center = Vector3.zero;
			this.headCollayder.center = Vector3.zero;
			this.mechExplossion.SetActive(true);
			this.mechExplossion.GetComponent<DisableObjectFromTimer>().timer = 1f;
			this.mechBodyAnimation.Play("Dead");
			this.mechGunAnimation.Play("Dead");
			this.mechPoint.GetComponent<DisableObjectFromTimer>().timer = 0.46f;
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
			if (this.myCurrentWeaponSounds.animationObject != null)
			{
				if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				}
				if (this.myCurrentWeaponSounds.isDoubleShot)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
				}
				else if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
				}
			}
			if (!this.isBigHead)
			{
				this.nickLabel.transform.localPosition = Vector3.up * 1.08f;
			}
			else
			{
				this.nickLabel.transform.localPosition = Vector3.up * 1.54f;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Mech, this, true);
		}
		if (this.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("DeactivateMechRPC", RPCMode.Others, new object[0]);
			}
			else
			{
				this.photonView.RPC("DeactivateMechRPC", PhotonTargets.Others, new object[0]);
			}
		}
		if (item != -1f)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time = Mathf.Min(item, this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].length);
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
	}

	public void DeactivateMech()
	{
		if (Defs.isDaterRegim)
		{
			this.DeactivateBear();
			return;
		}
		if (!this.isMechActive)
		{
			return;
		}
		this.isMechActive = false;
		if (this.myCurrentWeapon != null)
		{
			this.myCurrentWeapon.SetActive(true);
		}
		this.myCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
		if (Defs.isSoundFX)
		{
			this.mechExplossionSound.Play();
		}
		if (!this.isMulti || this.isMine)
		{
			this.mechPoint.SetActive(false);
			this.gunCamera.fieldOfView = 75f;
			if (!this.myCurrentWeaponSounds.isDoubleShot)
			{
				this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			}
			else
			{
				this.gunCamera.transform.localPosition = Vector3.zero;
			}
			if (this.inGameGUI != null)
			{
				if (!this._weaponManager.currentWeaponSounds.isMelee || this._weaponManager.currentWeaponSounds.isShotMelee)
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_fire";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
				}
				else
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_strike";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_strike";
				}
			}
		}
		else
		{
			if (!this.isInvisible)
			{
				this.fpsPlayerBody.SetActive(true);
			}
			this.bodyCollayder.height = 1.51f;
			this.bodyCollayder.center = Vector3.zero;
			this.headCollayder.center = Vector3.zero;
			this.mechExplossion.SetActive(true);
			this.mechExplossion.GetComponent<DisableObjectFromTimer>().timer = 1f;
			this.mechBodyAnimation.Play("Dead");
			this.mechGunAnimation.Play("Dead");
			this.mechPoint.GetComponent<DisableObjectFromTimer>().timer = 0.46f;
			this.nickLabel.transform.localPosition = Vector3.up * 1.08f;
		}
		if (!this.isMulti || this.isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Mech, this, true);
		}
		if (this.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("DeactivateMechRPC", RPCMode.Others, new object[0]);
			}
			else
			{
				this.photonView.RPC("DeactivateMechRPC", PhotonTargets.Others, new object[0]);
			}
		}
		if ((!this.isMulti || this.isMine) && this.inGameGUI != null)
		{
			this.inGameGUI.SetCrosshair(this._weaponManager.currentWeaponSounds);
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
	}

	[PunRPC]
	[RPC]
	public void DeactivateMechRPC()
	{
		this.DeactivateMech();
	}

	[DebuggerHidden]
	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		Player_move_c.u003cFadeu003ec__IteratorDD variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	private void fireFlash(bool isFlash, int numFlash)
	{
		string str;
		WeaponSounds weaponSound = (!this.isMechActive ? this.myCurrentWeaponSounds : this.mechWeaponSounds);
		if (weaponSound == null)
		{
			return;
		}
		if (isFlash)
		{
			if (numFlash == 0)
			{
				FlashFire component = weaponSound.GetComponent<FlashFire>();
				if (component != null)
				{
					component.fire(this);
				}
			}
			else if ((int)weaponSound.gunFlashDouble.Length > numFlash - 1)
			{
				weaponSound.gunFlashDouble[numFlash - 1].GetComponent<FlashFire>().fire(this);
			}
		}
		if (weaponSound.isRoundMelee)
		{
			float single = Player_move_c.TimeOfMeleeAttack(weaponSound);
			base.StartCoroutine(this.RunOnGroundEffectCoroutine(weaponSound.gameObject.name.Replace("(Clone)", string.Empty), single));
		}
		str = (weaponSound.isDoubleShot ? string.Concat("Shoot", numFlash.ToString()) : "Shoot");
		if (!this.isMechActive)
		{
			weaponSound.animationObject.GetComponent<Animation>().Play(str);
		}
		else
		{
			this.mechGunAnimation.Play(str);
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.shootMechClip);
			}
		}
		if (Defs.isSoundFX && !this.isMechActive)
		{
			base.GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().PlayOneShot(weaponSound.shoot);
		}
	}

	[PunRPC]
	[RPC]
	private void fireFlashWithHole(bool _isBloodParticle, Vector3 _pos, Quaternion _rot, bool isFlash, int numFlash)
	{
		this.fireFlash(isFlash, numFlash);
		this.HoleRPC(_isBloodParticle, _pos, _rot);
	}

	[PunRPC]
	[RPC]
	private void fireFlashWithManyHoles(bool[] _isBloodParticle, Vector3[] _pos, Quaternion[] _rot, bool isFlash, int numFlash)
	{
		this.fireFlash(isFlash, numFlash);
		if (_isBloodParticle != null)
		{
			for (int i = 0; i < (int)_isBloodParticle.Length; i++)
			{
				this.HoleRPC(_isBloodParticle[i], _pos[i], _rot[i]);
			}
		}
	}

	private void FixedUpdate()
	{
		if (this.rocketToLaunch != null)
		{
			Rocket component = this.rocketToLaunch.GetComponent<Rocket>();
			this.rocketToLaunch.GetComponent<Rigidbody>().AddForce(component.currentRocketSettings.startForce * this.rocketToLaunch.transform.forward);
			this.rocketToLaunch = null;
		}
		if (!this.isMulti || this.isMine)
		{
			ShopNGUIController.sharedShop.SetInGame(true);
			if ((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed ? Defs.isJetpackEnabled : false) != this.isJumpPresedOld && (Defs.isJetpackEnabled || this.isJumpPresedOld))
			{
				this.SetJetpackParticleEnabled((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed ? Defs.isJetpackEnabled : false));
				this.isJumpPresedOld = (JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed ? Defs.isJetpackEnabled : false);
			}
		}
		if (!this.isMulti || !this.isMine)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
	}

	private void FlamethrowerShot(WeaponSounds weapon)
	{
		RaycastHit raycastHit;
		this._FireFlash(true, 0);
		GameObject gameObject = null;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, weapon.range, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null)
		{
			gameObject = (!raycastHit.transform.parent || !raycastHit.transform.parent.CompareTag("Enemy") && !raycastHit.transform.parent.CompareTag("Player") ? raycastHit.transform.gameObject : raycastHit.transform.parent.gameObject);
			this._HitEnemy(gameObject, false, 0f);
		}
		List<GameObject> allTargets = this.GetAllTargets();
		float single = weapon.range * weapon.range;
		for (int i = 0; i < allTargets.Count; i++)
		{
			if (!(allTargets[i] == this._player) && !(gameObject == allTargets[i]))
			{
				Vector3 item = allTargets[i].transform.position - this._player.transform.position;
				if (item.sqrMagnitude < single && Vector3.Angle(base.gameObject.transform.forward, item) < weapon.meleeAngle)
				{
					this._HitEnemy(allTargets[i], false, 0f);
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator Flash(GameObject _obj)
	{
		Player_move_c.u003cFlashu003ec__IteratorDB variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator FlashWhenDead()
	{
		Player_move_c.u003cFlashWhenDeadu003ec__IteratorE1 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator FlashWhenHit()
	{
		Player_move_c.u003cFlashWhenHitu003ec__IteratorE0 variable = null;
		return variable;
	}

	private List<GameObject> GetAllTargets()
	{
		List<GameObject> gameObjects = new List<GameObject>();
		if (!this.isMulti || this.isCOOP)
		{
			gameObjects.AddRange(Initializer.enemiesObj);
		}
		else
		{
			gameObjects.AddRange(Initializer.playersObj);
			gameObjects.AddRange(Initializer.turretsObj);
		}
		if (Defs.isHunger)
		{
			gameObjects.AddRange(Initializer.chestsObj);
		}
		gameObjects.AddRange(Initializer.damagedObj);
		return gameObjects;
	}

	private float GetDamageForBotsAndExplosionObjects(bool isTakeDamageMech = false)
	{
		WeaponSounds weaponSound = (!isTakeDamageMech ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		float single = ((float)(-weaponSound.damage) + UnityEngine.Random.Range(weaponSound.damageRange.x, weaponSound.damageRange.y)) * (1f + this.koofDamageWeaponFromPotoins + EffectsController.DamageModifsByCats(weaponSound.categoryNabor - 1));
		return single;
	}

	public void GetDamageFromEnv(float damage, Vector3 pos)
	{
		if (this.isKilled || this.isImmortality)
		{
			return;
		}
		if (pos != Vector3.zero)
		{
			this.ShowDamageDirection(pos);
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound((this.curArmor > 0f || this.isMechActive ? this.damageArmorPlayerSound : this.damagePlayerSound));
		}
		if (this.DamagePlayerAndCheckDeath(damage) && Defs.isMulti)
		{
			this.ImSuicide();
			if (!Defs.isCOOP)
			{
				this.SendImKilled();
			}
		}
		if (!Defs.isMulti)
		{
			this.StartFlashRPC();
		}
		else
		{
			this.SendStartFlashMine();
		}
	}

	[PunRPC]
	[RPC]
	public void GetDamageFromEnvRPC(float damage, Vector3 pos)
	{
		base.StartCoroutine(this.Flash(this.myPlayerTransform.gameObject));
		if (!this.isMine || this.isKilled || this.isImmortality)
		{
			return;
		}
		if (pos != Vector3.zero)
		{
			this.ShowDamageDirection(pos);
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound((this.curArmor > 0f || this.isMechActive ? this.damageArmorPlayerSound : this.damagePlayerSound));
		}
		if (this.DamagePlayerAndCheckDeath(damage) && Defs.isMulti)
		{
			this.ImSuicide();
			if (!Defs.isCOOP)
			{
				this.SendImKilled();
			}
		}
	}

	private float GetDamageValueForTargetsInRadius(float distanceToTagetSqr, float radiusDamageSqr)
	{
		float single = (float)this._weaponManager.currentWeaponSounds.damage + WeaponManager.sharedManager.currentWeaponSounds.damageRange.x;
		float single1 = (float)this._weaponManager.currentWeaponSounds.damage + WeaponManager.sharedManager.currentWeaponSounds.damageRange.y;
		float single2 = single;
		single1 = single2;
		float single3 = (single + single2 * (1f - distanceToTagetSqr / radiusDamageSqr)) * (1f + this.koofDamageWeaponFromPotoins + EffectsController.DamageModifsByCats(this._weaponManager.currentWeaponSounds.categoryNabor - 1));
		return single3;
	}

	[DebuggerHidden]
	private IEnumerator GetHardwareKeysInput()
	{
		Player_move_c.u003cGetHardwareKeysInputu003ec__IteratorD8 variable = null;
		return variable;
	}

	public Player_move_c.RayHitsInfo GetHitsFromRay(Ray ray, bool getAll = true)
	{
		Player_move_c.RayHitsInfo array = new Player_move_c.RayHitsInfo()
		{
			obstacleFound = false,
			lenRay = 150f
		};
		RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, 150f, Player_move_c._ShootRaycastLayerMask);
		if (raycastHitArray == null || (int)raycastHitArray.Length == 0)
		{
			raycastHitArray = new RaycastHit[0];
		}
		if (getAll)
		{
			array.hits = raycastHitArray;
		}
		else
		{
			Array.Sort<RaycastHit>(raycastHitArray, (RaycastHit hit1, RaycastHit hit2) => {
				float single = (hit1.point - this.GunFlash.position).sqrMagnitude - (hit2.point - this.GunFlash.position).sqrMagnitude;
				return (single <= 0f ? (single != 0f ? -1 : 0) : 1);
			});
			bool flag = false;
			RaycastHit raycastHit = new RaycastHit();
			List<RaycastHit> raycastHits = new List<RaycastHit>();
			RaycastHit[] raycastHitArray1 = raycastHitArray;
			for (int i = 0; i < (int)raycastHitArray1.Length; i++)
			{
				RaycastHit raycastHit1 = raycastHitArray1[i];
				if (this.isHunger && raycastHit1.collider.gameObject != null && raycastHit1.collider.gameObject.CompareTag("Chest"))
				{
					raycastHits.Add(raycastHit1);
				}
				else if (raycastHit1.collider.gameObject.transform.parent != null && raycastHit1.collider.gameObject.transform.parent.CompareTag("Enemy"))
				{
					raycastHits.Add(raycastHit1);
				}
				else if (raycastHit1.collider.gameObject.transform.parent != null && raycastHit1.collider.gameObject.transform.parent.CompareTag("Player"))
				{
					raycastHits.Add(raycastHit1);
				}
				else if (raycastHit1.collider.gameObject != null && raycastHit1.collider.gameObject.CompareTag("Turret"))
				{
					raycastHits.Add(raycastHit1);
				}
				else if (!(raycastHit1.collider.gameObject != null) || !raycastHit1.collider.gameObject.CompareTag("DamagedExplosion"))
				{
					flag = true;
					raycastHit = raycastHit1;
					array.obstacleFound = true;
					Vector3 vector3 = raycastHit1.point;
					Vector3 vector31 = vector3 - ray.origin;
					array.lenRay = Vector3.Magnitude(vector31);
					array.rayReflect = new Ray(vector3, Vector3.Reflect(ray.direction, raycastHit1.normal));
					break;
				}
				else
				{
					raycastHits.Add(raycastHit1);
				}
			}
			array.hits = raycastHits.ToArray();
		}
		return array;
	}

	private float GetMultyDamage()
	{
		float damageByTier;
		float single;
		WeaponSounds weaponSound = (!this.isMechActive ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds);
		if (!this.isMechActive)
		{
			if (!(ExpController.Instance != null) || ExpController.Instance.OurTier >= (int)this._weaponManager.currentWeaponSounds.DamageByTier.Length)
			{
				single = ((int)this._weaponManager.currentWeaponSounds.DamageByTier.Length <= 0 ? 0f : this._weaponManager.currentWeaponSounds.DamageByTier[0]);
			}
			else
			{
				single = this._weaponManager.currentWeaponSounds.DamageByTier[this.TierOrRoomTier(ExpController.Instance.OurTier)];
			}
			damageByTier = single;
		}
		else
		{
			damageByTier = weaponSound.DamageByTier[this.TierOrRoomTier(GearManager.CurrentNumberOfUphradesForGear(GearManager.Mech))];
		}
		return damageByTier;
	}

	private int GetNumShootInDouble()
	{
		this.numShootInDoubleShot++;
		if (this.numShootInDoubleShot == 3)
		{
			this.numShootInDoubleShot = 1;
		}
		return this.numShootInDoubleShot;
	}

	public Vector3 GetPointAutoAim(Vector3 _posTo)
	{
		RaycastHit raycastHit;
		if (this.timerUpdatePointAutoAi < 0f)
		{
			this.rayAutoAim = this.myCamera.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			if (!Physics.Raycast(this.rayAutoAim, out raycastHit, 300f, Tools.AllWithoutDamageCollidersMaskAndWithoutRocket))
			{
				this.pointAutoAim = Vector3.down * 10000f;
			}
			else
			{
				if (raycastHit.collider.gameObject.name.Equals("Rocket(Clone)"))
				{
					UnityEngine.Debug.Log("Rocket(Clone)");
				}
				this.pointAutoAim = raycastHit.point;
			}
			this.timerUpdatePointAutoAi = 0.2f;
		}
		if (this.pointAutoAim.y >= -1000f)
		{
			return this.pointAutoAim;
		}
		return this.rayAutoAim.GetPoint(Vector3.Magnitude(this.myCamera.transform.position - _posTo));
	}

	public static GameObject[] GetStopObjFromPlayer(GameObject _obj)
	{
		List<GameObject> gameObjects = new List<GameObject>();
		Transform transforms = _obj.transform;
		int num = 0;
		while (num < transforms.childCount)
		{
			Transform child = transforms.GetChild(num);
			if (!child.gameObject.name.Equals("GameObject") || child.transform.childCount <= 0)
			{
				num++;
			}
			else
			{
				for (int i = 0; i < child.transform.childCount; i++)
				{
					GameObject gameObject = null;
					GameObject gameObject1 = null;
					WeaponSounds component = child.transform.GetChild(i).gameObject.GetComponent<WeaponSounds>();
					gameObject = component.bonusPrefab;
					if (!component.isMelee)
					{
						gameObject1 = child.transform.GetChild(i).Find("BulletSpawnPoint").gameObject;
					}
					if (component.noFillObjects != null && (int)component.noFillObjects.Length > 0)
					{
						for (int j = 0; j < (int)component.noFillObjects.Length; j++)
						{
							gameObjects.Add(component.noFillObjects[j]);
						}
					}
					if (gameObject != null)
					{
						gameObjects.Add(gameObject);
					}
					if (gameObject1 != null)
					{
						gameObjects.Add(gameObject1);
					}
					if (component.LeftArmorHand != null)
					{
						gameObjects.Add(component.LeftArmorHand.gameObject);
					}
					if (component.RightArmorHand != null)
					{
						gameObjects.Add(component.RightArmorHand.gameObject);
					}
					if (component.grenatePoint != null)
					{
						gameObjects.Add(component.grenatePoint.gameObject);
					}
					if (component.animationObject != null && component.animationObject.GetComponent<InnerWeaponPars>() != null && component.animationObject.GetComponent<InnerWeaponPars>().particlePoint != null)
					{
						gameObjects.Add(component.animationObject.GetComponent<InnerWeaponPars>().particlePoint);
					}
					List<GameObject> listWeaponAnimEffects = component.GetListWeaponAnimEffects();
					if (listWeaponAnimEffects != null)
					{
						gameObjects.AddRange(listWeaponAnimEffects);
					}
				}
				break;
			}
		}
		if (!(_obj != null) || !(_obj.GetComponent<SkinName>() != null))
		{
			UnityEngine.Debug.Log("Condition failed: “_obj != null && _obj.GetComponent<SkinName>() != null”");
		}
		else
		{
			SkinName skinName = _obj.GetComponent<SkinName>();
			gameObjects.Add(skinName.capesPoint);
			gameObjects.Add(skinName.hatsPoint);
			gameObjects.Add(skinName.maskPoint);
			gameObjects.Add(skinName.bootsPoint);
			gameObjects.Add(skinName.armorPoint);
			gameObjects.Add(skinName.onGroundEffectsPoint.gameObject);
			if (skinName.playerMoveC != null)
			{
				gameObjects.Add(skinName.playerMoveC.flagPoint);
				gameObjects.Add(skinName.playerMoveC.invisibleParticle);
				gameObjects.Add(skinName.playerMoveC.jetPackPoint);
				gameObjects.Add(skinName.playerMoveC.jetPackPointMech);
				gameObjects.Add(skinName.playerMoveC.wingsPoint);
				gameObjects.Add(skinName.playerMoveC.wingsPointBear);
				gameObjects.Add(skinName.playerMoveC.turretPoint);
				gameObjects.Add(skinName.playerMoveC.mechPoint);
				gameObjects.Add(skinName.playerMoveC.mechBearPoint);
				gameObjects.Add(skinName.playerMoveC.mechExplossion);
				gameObjects.Add(skinName.playerMoveC.bearExplosion);
				if (Defs.isDaterRegim && skinName.playerMoveC.myCurrentWeaponSounds != null)
				{
					gameObjects.Add(skinName.playerMoveC.myCurrentWeaponSounds.BearWeaponObject);
				}
				gameObjects.Add(skinName.playerMoveC.particleBonusesPoint);
				List<GameObject> gameObjects1 = gameObjects;
				skinName.playerMoveC.arrowToPortalPoint.Do<GameObject>(new Action<GameObject>(gameObjects1.Add));
			}
		}
		return gameObjects.ToArray();
	}

	public void GoToShopFromPause()
	{
		this.SetInApp();
		this.inAppOpenedFromPause = true;
	}

	public void GrenadeFire()
	{
		if (!this.isGrenadePress)
		{
			return;
		}
		float single = Time.realtimeSinceStartup - this.timeGrenadePress;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			TrainingController.isNextStep = TrainingState.TapToThrowGrenade;
		}
		Defs.isGrenateFireEnable = false;
		if (single - 0.4f <= 0f)
		{
			base.Invoke("GrenadeStartFire", 0.4f - single);
		}
		else
		{
			this.GrenadeStartFire();
		}
	}

	public void GrenadePress()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			this.showGrenadeHint = false;
			HintController.instance.HideHintByName("use_grenade");
		}
		if (this.indexWeapon == 1001)
		{
			return;
		}
		this.GrenadePressInvoke();
	}

	[Obfuscation(Exclude=true)]
	public void GrenadePressInvoke()
	{
		this.isGrenadePress = true;
		this.currentWeaponBeforeGrenade = WeaponManager.sharedManager.CurrentWeaponIndex;
		this.ChangeWeapon(1000, false);
		this.timeGrenadePress = Time.realtimeSinceStartup;
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider != null)
		{
			this.inGameGUI.blockedCollider.SetActive(true);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider2 != null)
		{
			this.inGameGUI.blockedCollider2.SetActive(true);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedColliderDater != null)
		{
			this.inGameGUI.blockedColliderDater.SetActive(true);
		}
		if (this.inGameGUI != null)
		{
			for (int i = 0; i < (int)this.inGameGUI.upButtonsInShopPanel.Length; i++)
			{
				this.inGameGUI.upButtonsInShopPanel[i].GetComponent<ButtonHandler>().isEnable = false;
			}
			for (int j = 0; j < (int)this.inGameGUI.upButtonsInShopPanelSwipeRegim.Length; j++)
			{
				this.inGameGUI.upButtonsInShopPanelSwipeRegim[j].GetComponent<ButtonHandler>().isEnable = false;
			}
		}
	}

	[Obfuscation(Exclude=true)]
	public void GrenadeStartFire()
	{
		if (!this.isMulti)
		{
			this.fireFlash(false, 0);
		}
		else if (this.isInet)
		{
			this.photonView.RPC("fireFlash", PhotonTargets.All, new object[] { false, 0 });
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("fireFlash", RPCMode.All, new object[] { false, 0 });
		}
		Player_move_c grenadeCount = this;
		grenadeCount.GrenadeCount = grenadeCount.GrenadeCount - 1;
		base.Invoke("RunGrenade", 0.2667f);
		base.Invoke("SetGrenateFireEnabled", 1f);
	}

	private void HandleEscape()
	{
		if (this.trainigController != null)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Ignoring [Escape] in training scene.");
			}
			return;
		}
		if (this.isMulti && !this.isMine)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("Ignoring [Escape]; isMulti: {0}, isMine: {1}", new object[] { this.isMulti, this.isMine });
			}
			return;
		}
		if (!Cursor.visible)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Handling [Escape]. Cursor locked.");
			}
			this._escapePressed = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			return;
		}
		if (this.showRanks)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("Ignoring [Escape]; showRanks: {0}", new object[] { this.showRanks });
			}
			return;
		}
		if (RespawnWindow.Instance != null && RespawnWindow.Instance.isShown)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Handling [Escape] in Respawn Window.");
			}
			RespawnWindow.Instance.OnBtnGoBattleClick();
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject != null)
		{
			if (!gameObject.GetComponent<ChatViewrController>().buySmileBannerPrefab.activeSelf)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Handling [Escape]. Closing chat");
				}
				gameObject.GetComponent<ChatViewrController>().CloseChat(false);
			}
			return;
		}
		if (!this.isInappWinOpen && Cursor.lockState != CursorLockMode.Locked)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("Handling [Escape]; isInappWinOpen: {0}, lockState: '{1}'", new object[] { this.isInappWinOpen, Cursor.lockState });
			}
			this._escapePressed = true;
		}
	}

	private void HandlePurchaseSuccessful(PurchaseResponse response)
	{
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			UnityEngine.Debug.LogWarning(string.Concat("Amazon PurchaseResponse (Player_move_c): ", response.Status));
			return;
		}
		UnityEngine.Debug.Log(string.Concat("Amazon PurchaseResponse (Player_move_c): ", response.PurchaseReceipt.ToJson()));
		this.PurchaseSuccessful(response.PurchaseReceipt.Sku);
	}

	private void HandleShowArmorChanged()
	{
		this.mySkinName.SetArmor(null);
		this.mySkinName.SetHat(null);
	}

	public bool HasFreezerFireSubscr()
	{
		return this.FreezerFired != null;
	}

	public void HideChangeWeaponTrainingHint()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showChangeWeaponHint)
		{
			this.showChangeWeaponHint = false;
			HintController.instance.HideHintByName("change_weapon");
		}
	}

	public void hideGUI()
	{
		this.showGUI = false;
	}

	public void hit(float dam, Vector3 posEnemy, bool damageColliderHit = false)
	{
		if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
		{
			dam *= this._protectionShieldValue;
			if (this.isMechActive)
			{
				this.MinusMechHealth(dam);
			}
			else if (this.curArmor < dam)
			{
				Player_move_c curHealth = this;
				curHealth.CurHealth = curHealth.CurHealth - (dam - this.curArmor);
				this.curArmor = 0f;
				CurrentCampaignGame.withoutHits = false;
			}
			else
			{
				Player_move_c playerMoveC = this;
				playerMoveC.curArmor = playerMoveC.curArmor - dam;
			}
		}
		if (!damageColliderHit)
		{
			this.ShowDamageDirection(posEnemy);
		}
		if (!this.damageShown)
		{
			base.StartCoroutine(this.FlashWhenHit());
		}
	}

	[DebuggerHidden]
	private IEnumerator HitRoundMelee(WeaponSounds weapon)
	{
		Player_move_c.u003cHitRoundMeleeu003ec__IteratorE7 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	public void HoleRPC(bool _isBloodParticle, Vector3 _pos, Quaternion _rot)
	{
		if (Device.isPixelGunLow)
		{
			return;
		}
		if (!_isBloodParticle)
		{
			HoleScript currentHole = HoleBulletStackController.sharedController.GetCurrentHole(false);
			if (currentHole != null)
			{
				currentHole.StartShowHole(_pos, _rot, false);
			}
			WallBloodParticle currentParticle = WallParticleStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				currentParticle.StartShowParticle(_pos, _rot, false);
			}
		}
		else
		{
			WallBloodParticle wallBloodParticle = BloodParticleStackController.sharedController.GetCurrentParticle(false);
			if (wallBloodParticle != null)
			{
				wallBloodParticle.StartShowParticle(_pos, _rot, false);
			}
		}
	}

	public void IdleAnimation()
	{
		if (!this._singleOrMultiMine() && (!Defs.isDaterRegim || !this.isBearActive))
		{
			return;
		}
		if ((this.isBearActive || this.isMechActive) && !this.mechGunAnimation.IsPlaying("Shoot"))
		{
			this.mechGunAnimation.CrossFade("Idle");
		}
		if (this.___weaponManager && this.___weaponManager.currentWeaponSounds && this.___weaponManager.currentWeaponSounds.animationObject != null)
		{
			this.___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Idle");
		}
	}

	[PunRPC]
	[RPC]
	public void imDeath(string _name)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		this._weaponManager.myPlayerMoveC.AddSystemMessage(_name, 1, Color.white);
	}

	public void ImKill(NetworkViewID idKiller, int _typeKill)
	{
		Player_move_c playerMoveC = this;
		playerMoveC.countKills = playerMoveC.countKills + 1;
		GlobalGameController.CountKills = this.countKills;
		this.CheckRookieKillerAchievement();
		this.addMultyKill();
		if (this.isCompany)
		{
			if (this.myCommand == 1)
			{
				Player_move_c playerMoveC1 = this;
				playerMoveC1.countKillsCommandBlue = playerMoveC1.countKillsCommandBlue + 1;
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[] { 1 });
				}
				else
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[] { 1 });
				}
			}
			if (this.myCommand == 2)
			{
				Player_move_c playerMoveC2 = this;
				playerMoveC2.countKillsCommandRed = playerMoveC2.countKillsCommandRed + 1;
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[] { 2 });
				}
				else
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[] { 2 });
				}
			}
		}
		this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
		this._weaponManager.myNetworkStartTable.SynhCountKills(null);
	}

	public void ImKill(int idKiller, int _typeKill)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponSounds weaponSound = WeaponManager.sharedManager.currentWeaponSounds;
			if (weaponSound != null)
			{
				Initializer initializer = UnityEngine.Object.FindObjectOfType<Initializer>();
				if (initializer != null)
				{
					initializer.IncrementKillCountForWeapon((_typeKill != 6 ? weaponSound.shopName : "GRENADE"));
				}
				else
				{
					UnityEngine.Debug.LogWarning("initializer == null");
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("ws == null");
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("WeaponManager.sharedManager == null");
		}
		Player_move_c playerMoveC = this;
		playerMoveC.countKills = playerMoveC.countKills + 1;
		GlobalGameController.CountKills = this.countKills;
		this.CheckRookieKillerAchievement();
		this.addMultyKill();
		if (this.isCompany)
		{
			if (this.myCommand == 1)
			{
				Player_move_c playerMoveC1 = this;
				playerMoveC1.countKillsCommandBlue = playerMoveC1.countKillsCommandBlue + 1;
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[] { 1 });
				}
				else
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[] { 1 });
				}
			}
			if (this.myCommand == 2)
			{
				Player_move_c playerMoveC2 = this;
				playerMoveC2.countKillsCommandRed = playerMoveC2.countKillsCommandRed + 1;
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[] { 2 });
				}
				else
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[] { 2 });
				}
			}
		}
		this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
		this._weaponManager.myNetworkStartTable.SynhCountKills(null);
		if (this.isHunger && Initializer.players.Count == 1)
		{
			if (Defs.isHunger)
			{
				int num = Storager.getInt(Defs.RatingHunger, false) + 1;
				Storager.setInt(Defs.RatingHunger, num, false);
			}
			this.photonView.RPC("pobedaPhoton", PhotonTargets.All, new object[] { idKiller, this.myCommand });
			int num1 = Storager.getInt("Rating", false) + 1;
			Storager.setInt("Rating", num1, false);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.TryIncrementWinCountTimestamp();
			}
			this._weaponManager.myNetworkStartTable.isIwin = true;
		}
	}

	[PunRPC]
	[RPC]
	private void ImKilled(Vector3 pos, Quaternion rot)
	{
		this.ImKilled(pos, rot, 0);
	}

	[PunRPC]
	[RPC]
	private void ImKilled(Vector3 pos, Quaternion rot, int _typeDead = 0)
	{
		if (Device.isPixelGunLow)
		{
			_typeDead = 0;
		}
		if (!this.isStartAngel || Defs.isCOOP)
		{
			this.isStartAngel = true;
			if (Defs.inComingMessagesCounter < 15)
			{
				PlayerDeadController currentParticle = PlayerDeadStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShow(pos, rot, _typeDead, false, this._skin);
				}
				if (Defs.isSoundFX)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.deadPlayerSound);
				}
			}
		}
		if (!this.isMine && this.getLocalHurt)
		{
			WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killAssist, 1f);
			this.getLocalHurt = false;
		}
	}

	public void ImSuicide()
	{
		this.isSuicided = true;
		this.respawnedForGUI = true;
		if (Defs.isFlag && this.isCaptureFlag)
		{
			this.enemyFlag.GoBaza();
			this.isCaptureFlag = false;
			this.SendSystemMessegeFromFlagReturned(this.enemyFlag.isBlue);
		}
		if (this.countKills > 0)
		{
			GlobalGameController.CountKills = this.countKills;
		}
		this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
		this._weaponManager.myNetworkStartTable.SynhCountKills(null);
		this.sendImDeath(this.mySkinName.NickName);
	}

	public void IndicateDamage()
	{
		this.isDeadFrame = true;
		base.Invoke("setisDeadFrameFalse", 1f);
	}

	private void InitiailizeIcnreaseArmorEffectFlags()
	{
		this.BonusEffectForArmorWorksInThisMatch = EffectsController.IcnreaseEquippedArmorPercentage > 1f;
		this.ArmorBonusGiven = EffectsController.ArmorBonus > 0f;
	}

	private void InitPurchaseActions()
	{
		this._actionsForPurchasedItems.Add("bigammopack", new Action<string>(this.ProvideAmmo));
		this._actionsForPurchasedItems.Add("Fullhealth", new Action<string>(this.ProvideHealth));
		this._actionsForPurchasedItems.Add(StoreKitEventListener.elixirID, new Action<string>((string inShopId) => Defs.NumberOfElixirs++));
		this._actionsForPurchasedItems.Add(StoreKitEventListener.armor, new Action<string>((string inShopId) => {
		}));
		this._actionsForPurchasedItems.Add(StoreKitEventListener.armor2, new Action<string>((string inShopId) => {
		}));
		this._actionsForPurchasedItems.Add(StoreKitEventListener.armor3, new Action<string>((string inShopId) => {
		}));
		string[] strArrays = PotionsController.potions;
		for (int i = 0; i < (int)strArrays.Length; i++)
		{
			string str = strArrays[i];
			this._actionsForPurchasedItems.Add(str, new Action<string>(this.providePotion));
		}
		string[] canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(true);
		for (int j = 0; j < (int)canBuyWeaponTags.Length; j++)
		{
			string shopIdByTag = ItemDb.GetShopIdByTag(canBuyWeaponTags[j]);
			this._actionsForPurchasedItems.Add(shopIdByTag, new Action<string>(this.AddWeaponToInv));
		}
	}

	[DebuggerHidden]
	private IEnumerator KillCam()
	{
		Player_move_c.u003cKillCamu003ec__IteratorE2 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	public void Killed(NetworkViewID idKiller, int _typeKill, int _typeWeapon, string weaponName)
	{
		PlayerEventScoreController.ScoreEvent scoreEvent;
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		string empty = string.Empty;
		string nickName = string.Empty;
		nickName = this.mySkinName.NickName;
		foreach (Player_move_c player in Initializer.players)
		{
			if (!player.mySkinName.GetComponent<NetworkView>().viewID.Equals(idKiller))
			{
				continue;
			}
			empty = player.mySkinName.NickName;
			if (this.isMine && Defs.isJetpackEnabled && !this.mySkinName.character.isGrounded)
			{
				player.AddScoreDuckHunt();
			}
			if (this._weaponManager && player == this._weaponManager.myPlayerMoveC)
			{
				ProfileController.OnGameTotalKills();
				PlayerScoreController playerScoreController = player.myScoreController;
				if (_typeKill == 6)
				{
					scoreEvent = PlayerEventScoreController.ScoreEvent.deadGrenade;
				}
				else if (_typeKill == 9)
				{
					scoreEvent = PlayerEventScoreController.ScoreEvent.deadTurret;
				}
				else if (_typeKill != 2)
				{
					scoreEvent = (_typeKill != 3 ? PlayerEventScoreController.ScoreEvent.dead : PlayerEventScoreController.ScoreEvent.deadExplosion);
				}
				else
				{
					scoreEvent = PlayerEventScoreController.ScoreEvent.deadHeadShot;
				}
				playerScoreController.AddScoreOnEvent(scoreEvent, 1f);
				if (Defs.isJetpackEnabled && !this._weaponManager.myPlayerMoveC.mySkinName.character.isGrounded && _typeKill != 6 && _typeKill != 8)
				{
					player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deathFromAbove, 1f);
				}
				if (player.isRocketJump && _typeKill != 6 && _typeKill != 8)
				{
					player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.rocketJumpKill, 1f);
				}
				if (this.multiKill > 1)
				{
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyBreakSeries();
					}
					if (this.multiKill == 2)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill2, 1f);
					}
					else if (this.multiKill == 3)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill3, 1f);
					}
					else if (this.multiKill == 4)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill4, 1f);
					}
					else if (this.multiKill == 5)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill5, 1f);
					}
					else if (this.multiKill < 10)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill6, 1f);
					}
					else if (this.multiKill < 20)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill10, 1f);
					}
					else if (this.multiKill >= 50)
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill50, 1f);
					}
					else
					{
						player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill20, 1f);
					}
				}
				if (this.isInvisible)
				{
					player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.invisibleKill, 1f);
				}
				if (this.isPlacemarker)
				{
					player.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.revenge, 1f);
				}
				if (_typeKill != 6 && _typeKill != 8 && _typeKill != 10)
				{
					GameObject gameObject = Resources.Load(string.Concat("Weapons/", weaponName)) as GameObject;
					if (gameObject != null && gameObject.GetComponent<WeaponSounds>() != null)
					{
						this.AddCountSerials(gameObject.GetComponent<WeaponSounds>().categoryNabor - 1, player);
					}
				}
				player.ImKill(idKiller, _typeKill);
				if (this.Equals(this._weaponManager.myPlayerMoveC.placemarkerMoveC))
				{
					this._weaponManager.myPlayerMoveC.placemarkerMoveC = null;
					this.isPlacemarker = false;
				}
				if (this.getLocalHurt)
				{
					this.getLocalHurt = false;
				}
			}
			if (this.isMine)
			{
				player.isPlacemarker = true;
				this.placemarkerMoveC = player;
			}
			this.UpdateKillerInfo(player, _typeKill);
			break;
		}
		this.ImKilled(this.myPlayerTransform.position, this.myPlayerTransform.rotation, _typeWeapon);
		if (this._weaponManager && this._weaponManager.myPlayer != null)
		{
			this._weaponManager.myPlayerMoveC.AddSystemMessage(empty, _typeKill, nickName, Color.white, weaponName);
		}
	}

	[PunRPC]
	[RPC]
	public void KilledPhoton(int idKiller, int _typekill)
	{
		this.KilledPhoton(idKiller, _typekill, string.Empty);
	}

	[PunRPC]
	[RPC]
	public void KilledPhoton(int idKiller, int _typekill, string weaponName)
	{
		this.KilledPhoton(idKiller, _typekill, weaponName, 0);
	}

	[PunRPC]
	[RPC]
	public void KilledPhoton(int idKiller, int _typekill, string weaponName, int _typeWeapon)
	{
		PlayerEventScoreController.ScoreEvent scoreEvent;
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		string empty = string.Empty;
		string nickName = this.mySkinName.NickName;
		int num = 0;
		while (num < Initializer.players.Count)
		{
			if (!(Initializer.players[num].mySkinName.photonView != null) || Initializer.players[num].mySkinName.photonView.viewID != idKiller)
			{
				num++;
			}
			else
			{
				SkinName item = Initializer.players[num].mySkinName;
				Player_move_c playerMoveC = Initializer.players[num];
				empty = item.NickName;
				if (this.isMine && Defs.isJetpackEnabled && !this.mySkinName.character.isGrounded)
				{
					playerMoveC.AddScoreDuckHunt();
				}
				if (this._weaponManager != null && Initializer.players[num] == this._weaponManager.myPlayerMoveC)
				{
					ProfileController.OnGameTotalKills();
					if (!FriendsController.useBuffSystem)
					{
						KillRateCheck.instance.IncrementKills();
					}
					else
					{
						BuffSystem.instance.KillInteraction();
					}
					WeaponManager.sharedManager.myNetworkStartTable.IncrementKills();
					if (this.isRaiderMyPoint)
					{
						WeaponManager.sharedManager.myPlayerMoveC.SendHouseKeeperEvent();
						this.isRaiderMyPoint = false;
					}
					if (Defs.isJetpackEnabled && !this._weaponManager.myPlayerMoveC.mySkinName.character.isGrounded && _typekill != 6 && _typekill != 8)
					{
						playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deathFromAbove, 1f);
					}
					if (playerMoveC.isRocketJump && _typekill != 6 && _typekill != 8)
					{
						playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.rocketJumpKill, 1f);
					}
					if (_typekill != 6 && _typekill != 8 && _typekill != 10)
					{
						GameObject gameObject = Resources.Load(string.Concat("Weapons/", weaponName)) as GameObject;
						if (gameObject != null && gameObject.GetComponent<WeaponSounds>() != null)
						{
							this.AddCountSerials(gameObject.GetComponent<WeaponSounds>().categoryNabor - 1, playerMoveC);
						}
					}
					if (this.multiKill > 1)
					{
						if (!NetworkStartTable.LocalOrPasswordRoom())
						{
							QuestMediator.NotifyBreakSeries();
						}
						if (this.multiKill == 2)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill2, 1f);
						}
						else if (this.multiKill == 3)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill3, 1f);
						}
						else if (this.multiKill == 4)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill4, 1f);
						}
						else if (this.multiKill == 5)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill5, 1f);
						}
						else if (this.multiKill < 10)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill6, 1f);
						}
						else if (this.multiKill < 20)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill10, 1f);
						}
						else if (this.multiKill >= 50)
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill50, 1f);
						}
						else
						{
							playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill20, 1f);
						}
					}
					if (!Defs.isFlag)
					{
						playerMoveC.ImKill(idKiller, _typekill);
					}
					ShopNGUIController.CategoryNames categoryName = ShopNGUIController.CategoryNames.BackupCategory | ShopNGUIController.CategoryNames.MeleeCategory | ShopNGUIController.CategoryNames.SpecilCategory | ShopNGUIController.CategoryNames.SniperCategory | ShopNGUIController.CategoryNames.PremiumCategory | ShopNGUIController.CategoryNames.HatsCategory | ShopNGUIController.CategoryNames.ArmorCategory | ShopNGUIController.CategoryNames.SkinsCategory | ShopNGUIController.CategoryNames.CapesCategory | ShopNGUIController.CategoryNames.BootsCategory | ShopNGUIController.CategoryNames.GearCategory | ShopNGUIController.CategoryNames.MaskCategory;
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
					if (byPrefabName != null)
					{
						categoryName = (ShopNGUIController.CategoryNames)PromoActionsGUIController.CatForTg(byPrefabName.Tag);
					}
					Player_move_c.TypeKills typeKill = (Player_move_c.TypeKills)_typekill;
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyKillOtherPlayer(ConnectSceneNGUIController.regim, categoryName, typeKill == Player_move_c.TypeKills.headshot, typeKill == Player_move_c.TypeKills.grenade, this.isPlacemarker);
					}
					PlayerScoreController playerScoreController = playerMoveC.myScoreController;
					if (_typekill == 6)
					{
						scoreEvent = PlayerEventScoreController.ScoreEvent.deadGrenade;
					}
					else if (_typekill == 9)
					{
						scoreEvent = PlayerEventScoreController.ScoreEvent.deadTurret;
					}
					else if (_typekill != 2)
					{
						scoreEvent = (_typekill != 3 ? PlayerEventScoreController.ScoreEvent.dead : PlayerEventScoreController.ScoreEvent.deadExplosion);
					}
					else
					{
						scoreEvent = PlayerEventScoreController.ScoreEvent.deadHeadShot;
					}
					playerScoreController.AddScoreOnEvent(scoreEvent, 1f);
					if (this.isInvisible)
					{
						playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.invisibleKill, 1f);
					}
					if (this.isPlacemarker)
					{
						playerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.revenge, 1f);
					}
					if (this.Equals(this._weaponManager.myPlayerMoveC.placemarkerMoveC))
					{
						this._weaponManager.myPlayerMoveC.placemarkerMoveC = null;
						this.isPlacemarker = false;
					}
					if (this.getLocalHurt)
					{
						this.getLocalHurt = false;
					}
				}
				if (this.isMine)
				{
					playerMoveC.isPlacemarker = true;
					this.placemarkerMoveC = playerMoveC;
				}
				this.UpdateKillerInfo(Initializer.players[num], _typekill);
				break;
			}
		}
		this.ImKilled(this.myPlayerTransform.position, this.myPlayerTransform.rotation, _typeWeapon);
		if (this._weaponManager && this._weaponManager.myPlayerMoveC != null)
		{
			this._weaponManager.myPlayerMoveC.AddSystemMessage(empty, _typekill, nickName, Color.white, weaponName);
		}
	}

	public void KillSelf()
	{
		if (this.isMulti && !this.isMine || this.isKilled || this.CurHealth <= 0f)
		{
			return;
		}
		this.curArmor = 0f;
		this.CurHealth = 0f;
		if (!Defs.isMulti)
		{
			this.StartFlash(this.mySkinName.gameObject);
		}
		else
		{
			this.ImSuicide();
			if (!Defs.isCOOP)
			{
				this.SendImKilled();
			}
		}
	}

	private void Like(Player_move_c whoMoveC, Player_move_c whomMoveC)
	{
		if (whomMoveC.Equals(WeaponManager.sharedManager.myPlayerMoveC))
		{
			Player_move_c playerMoveC = this;
			playerMoveC.countKills = playerMoveC.countKills + 1;
			GlobalGameController.CountKills = this.countKills;
			WeaponManager.sharedManager.myNetworkStartTable.CountKills = this.countKills;
			WeaponManager.sharedManager.myNetworkStartTable.SynhCountKills(null);
			ProfileController.OnGetLike();
		}
	}

	[PunRPC]
	[RPC]
	private void LikeRPC(int idWho, int idWhom)
	{
		Player_move_c playerMoveC = null;
		Player_move_c playerMoveC1 = null;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			Player_move_c item = Initializer.players[i];
			if (idWho == item.photonView.ownerId)
			{
				playerMoveC = item;
			}
			if (idWhom == item.photonView.ownerId)
			{
				playerMoveC1 = item;
			}
		}
		if (playerMoveC != null && playerMoveC1 != null)
		{
			this.Like(playerMoveC, playerMoveC1);
		}
	}

	[PunRPC]
	[RPC]
	private void LikeRPCLocal(NetworkViewID idWho, NetworkViewID idWhom)
	{
		Player_move_c playerMoveC = null;
		Player_move_c playerMoveC1 = null;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			Player_move_c item = Initializer.players[i];
			if (idWho.Equals(item.GetComponent<NetworkView>().viewID))
			{
				playerMoveC = item;
			}
			if (idWhom.Equals(item.GetComponent<NetworkView>().viewID))
			{
				playerMoveC1 = item;
			}
		}
		if (playerMoveC != null && playerMoveC1 != null)
		{
			this.Like(playerMoveC, playerMoveC1);
		}
	}

	[DebuggerHidden]
	private IEnumerator MeleeShot(WeaponSounds weapon)
	{
		Player_move_c.u003cMeleeShotu003ec__IteratorE5 variable = null;
		return variable;
	}

	public void MinusLive(int idKiller, float minus, Player_move_c.TypeKills _typeKills, int _typeWeapon = 0, string weaponName = "", int idTurret = 0)
	{
		if (Defs.isDaterRegim || this.isImmortality)
		{
			return;
		}
		ProfileController.OnGameHit();
		if (_typeKills != Player_move_c.TypeKills.turret && InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.ShowImpact();
		}
		minus *= this._protectionShieldValue;
		if (!FriendsController.useBuffSystem || !BuffSystem.instance.haveBuffForWeapon(weaponName))
		{
			minus *= WeaponManager.sharedManager.myPlayerMoveC.damageBuff;
		}
		else
		{
			minus *= BuffSystem.instance.weaponBuffValue;
			UnityEngine.Debug.Log("Buffed shot!");
		}
		minus /= this.protectionBuff;
		if (this.isMechActive)
		{
			if (this.MinusMechHealth(minus))
			{
				WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deadMech, 1f);
				minus = 1000f;
				if (_typeKills != Player_move_c.TypeKills.grenade && _typeKills != Player_move_c.TypeKills.mech && _typeKills != Player_move_c.TypeKills.turret)
				{
					try
					{
						WeaponManager.sharedManager.myPlayerMoveC.AddWeKillStatisctics((weaponName ?? string.Empty).Replace("(Clone)", string.Empty));
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception we were killed AddWeKillStatisctics: ", exception));
					}
				}
			}
		}
		else if (this.synhHealth > 0f)
		{
			this.getLocalHurt = true;
			this.synhHealth -= minus;
			if (this.synhHealth < 0f)
			{
				this.synhHealth = 0f;
			}
			if (this.armorSynch <= minus)
			{
				this.armorSynch = 0f;
			}
			else
			{
				this.armorSynch -= minus;
			}
			if (this.synhHealth <= 0f)
			{
				if (_typeKills != Player_move_c.TypeKills.grenade && _typeKills != Player_move_c.TypeKills.mech && _typeKills != Player_move_c.TypeKills.turret)
				{
					try
					{
						WeaponManager.sharedManager.myPlayerMoveC.AddWeKillStatisctics((weaponName ?? string.Empty).Replace("(Clone)", string.Empty));
					}
					catch (Exception exception1)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception we were killed AddWeKillStatisctics: ", exception1));
					}
				}
				minus = 10000f;
				if (this.isCaptureFlag)
				{
					WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deadWithFlag, 1f);
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyKillOtherPlayerWithFlag();
					}
				}
				if (Defs.isCapturePoints && WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					int num = 0;
					while (num < (int)CapturePointController.sharedController.basePointControllers.Length)
					{
						if ((int)CapturePointController.sharedController.basePointControllers[num].captureConmmand != WeaponManager.sharedManager.myPlayerMoveC.myCommand || !CapturePointController.sharedController.basePointControllers[num].capturePlayers.Contains(this))
						{
							num++;
						}
						else
						{
							this.isRaiderMyPoint = true;
							break;
						}
					}
				}
				if (this.getLocalHurt)
				{
					this.getLocalHurt = false;
				}
				this.ImKilled(this.myPlayerTransform.position, this.myPlayerTransform.rotation, _typeWeapon);
				this.myPersonNetwork.StartAngel();
				if (Defs.isFlag && this.isCaptureFlag)
				{
					FlagController flagController = null;
					if (this.flag1.targetTrasform == this.flagPoint.transform)
					{
						flagController = this.flag1;
					}
					if (this.flag2.targetTrasform == this.flagPoint.transform)
					{
						flagController = this.flag2;
					}
					if (flagController != null)
					{
						flagController.SetNOCaptureRPC(this.myPlayerTransform.position, this.myPlayerTransform.rotation);
					}
				}
			}
		}
		this.photonView.RPC("MinusLiveRPCPhoton", PhotonTargets.Others, new object[] { idKiller, minus, (int)_typeKills, _typeWeapon, idTurret, weaponName });
		this.MinusLiveRPCEffects((int)_typeKills);
	}

	public void MinusLive(NetworkViewID idKiller, float minus, Player_move_c.TypeKills _typeKills, int _typeWeapon, string nameWeapon = "", NetworkViewID idTurret = default(NetworkViewID))
	{
		if (Defs.isDaterRegim)
		{
			return;
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.ShowImpact();
		}
		ProfileController.OnGameHit();
		this.getLocalHurt = true;
		base.GetComponent<NetworkView>().RPC("MinusLiveRPC", RPCMode.All, new object[] { idKiller, minus, (int)_typeKills, _typeWeapon, idTurret, nameWeapon });
	}

	public void minusLiveFromZombi(float _minusLive, Vector3 posZombi)
	{
		this.photonView.RPC("minusLiveFromZombiRPC", PhotonTargets.All, new object[] { _minusLive, posZombi });
	}

	[PunRPC]
	[RPC]
	public void minusLiveFromZombiRPC(float live, Vector3 posZombi)
	{
		if (this.photonView.isMine && !this.isKilled && !this.isImmortality)
		{
			live *= this._protectionShieldValue;
			if (!this.isMechActive)
			{
				float single = live - this.curArmor;
				if (single >= 0f)
				{
					this.curArmor = 0f;
				}
				else
				{
					Player_move_c playerMoveC = this;
					playerMoveC.curArmor = playerMoveC.curArmor - live;
					single = 0f;
				}
				Player_move_c curHealth = this;
				curHealth.CurHealth = curHealth.CurHealth - single;
			}
			else
			{
				this.MinusMechHealth(live);
			}
			this.ShowDamageDirection(posZombi);
		}
		base.StartCoroutine(this.Flash(this.myPlayerTransform.gameObject));
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPC(NetworkViewID idKiller, float minus, int _typeKills, int _typeWeapon, NetworkViewID idTurret, string weaponName)
	{
		this.MinusLiveRPCEffects(_typeKills);
		if (this.isMine && !this.isKilled && !this.isImmortality)
		{
			float single = 0f;
			if (!this.isMechActive)
			{
				single = minus - this.curArmor;
				if (single >= 0f)
				{
					this.curArmor = 0f;
				}
				else
				{
					Player_move_c playerMoveC = this;
					playerMoveC.curArmor = playerMoveC.curArmor - minus;
					single = 0f;
				}
			}
			else
			{
				this.MinusMechHealth(minus);
			}
			if (this.CurHealth > 0f)
			{
				Player_move_c curHealth = this;
				curHealth.CurHealth = curHealth.CurHealth - single;
				if (this.CurHealth <= 0f)
				{
					if (this.myKillAssistsLocal.Contains(idKiller))
					{
						this.myKillAssistsLocal.Remove(idKiller);
					}
					if (this.placemarkerMoveC != null)
					{
						this.placemarkerMoveC.isPlacemarker = false;
					}
					base.GetComponent<NetworkView>().RPC("Killed", RPCMode.All, new object[] { idKiller, _typeKills, _typeWeapon, weaponName });
				}
				else if (!this.myKillAssistsLocal.Contains(idKiller))
				{
					this.myKillAssistsLocal.Add(idKiller);
				}
				this.SendSynhHealth(false, null);
				Vector3 vector3 = Vector3.zero;
				if (_typeKills == 8)
				{
					GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Turret");
					int num = 0;
					while (num < (int)gameObjectArray.Length)
					{
						GameObject gameObject = gameObjectArray[num];
						if (!(gameObject.GetComponent<NetworkView>() != null) || !gameObject.GetComponent<NetworkView>().viewID.Equals(idTurret))
						{
							num++;
						}
						else
						{
							this.ShowDamageDirection(gameObject.transform.position);
							break;
						}
					}
				}
				else
				{
					foreach (Player_move_c player in Initializer.players)
					{
						if (!(player.GetComponent<NetworkView>() != null) || !player.GetComponent<NetworkView>().viewID.Equals(idKiller))
						{
							continue;
						}
						this.ShowDamageDirection(player.transform.position);
						break;
					}
				}
			}
		}
	}

	private void MinusLiveRPCEffects(int _typeKills)
	{
		AudioClip audioClip;
		if (!Device.isPixelGunLow && !this.isDaterRegim && !this.isMine)
		{
			if (_typeKills != 2)
			{
				HitParticle currentParticle = HitStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
			else
			{
				HitParticle hitParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
				if (hitParticle != null)
				{
					hitParticle.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
		}
		if (Defs.isSoundFX)
		{
			AudioSource component = base.gameObject.GetComponent<AudioSource>();
			if (this.curArmor > 0f || this.isMechActive)
			{
				audioClip = this.damageArmorPlayerSound;
			}
			else
			{
				audioClip = (_typeKills != 2 ? this.damagePlayerSound : this.headShotSound);
			}
			component.PlayOneShot(audioClip);
		}
		base.StartCoroutine(this.Flash(this.myPlayerTransform.gameObject));
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPCPhoton(int idKiller, float minus, int _typeKills, int _typeWeapon, int idTurret, string weaponName)
	{
		this.MinusLiveRPCEffects(_typeKills);
		if (!this.isMine)
		{
			this.synhHealth -= minus;
			if (this.synhHealth < 0f)
			{
				this.synhHealth = 0f;
			}
			if (this.armorSynch <= minus)
			{
				this.armorSynch = 0f;
			}
			else
			{
				this.armorSynch -= minus;
			}
		}
		if (this.isMine && !this.isKilled && !this.isImmortality)
		{
			float single = 0f;
			if (!this.isMechActive)
			{
				single = minus - this.curArmor;
				if (single >= 0f)
				{
					this.curArmor = 0f;
				}
				else
				{
					Player_move_c playerMoveC = this;
					playerMoveC.curArmor = playerMoveC.curArmor - minus;
					single = 0f;
				}
			}
			else
			{
				this.MinusMechHealth(minus);
			}
			if (this.CurHealth > 0f)
			{
				Player_move_c curHealth = this;
				curHealth.CurHealth = curHealth.CurHealth - single;
				if (this.CurHealth <= 0f)
				{
					try
					{
						if (!WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
						{
							WeaponManager.sharedManager.myPlayerMoveC.AddWeWereKilledStatisctics((WeaponManager.sharedManager.currentWeaponSounds.name ?? string.Empty).Replace("(Clone)", string.Empty));
						}
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogError(string.Concat("Exception we were killed AddWeWereKilledStatisctics: ", exception));
					}
					if (this.myKillAssists.Contains(idKiller))
					{
						this.myKillAssists.Remove(idKiller);
					}
					if (this.placemarkerMoveC != null)
					{
						this.placemarkerMoveC.isPlacemarker = false;
					}
					this.photonView.RPC("KilledPhoton", PhotonTargets.All, new object[] { idKiller, _typeKills, weaponName, _typeWeapon });
				}
				else if (!this.myKillAssists.Contains(idKiller))
				{
					this.myKillAssists.Add(idKiller);
				}
				this.SynhHealthRPC(this.CurHealth + this.curArmor, this.curArmor, false);
			}
			if (_typeKills == 8)
			{
				GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Turret");
				Vector3 vector3 = Vector3.zero;
				int num = 0;
				while (num < (int)gameObjectArray.Length)
				{
					PhotonView component = gameObjectArray[num].GetComponent<PhotonView>();
					if (!(component != null) || component.viewID != idTurret)
					{
						num++;
					}
					else
					{
						this.ShowDamageDirection(gameObjectArray[num].transform.position);
						break;
					}
				}
			}
			else
			{
				Vector3 item = Vector3.zero;
				int num1 = 0;
				while (num1 < Initializer.players.Count)
				{
					PhotonView photonView = Initializer.players[num1].mySkinName.photonView;
					if (!(photonView != null) || photonView.viewID != idKiller)
					{
						num1++;
					}
					else
					{
						item = Initializer.players[num1].myPlayerTransform.position;
						this.ShowDamageDirection(item);
						break;
					}
				}
			}
		}
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPCWithTurretPhoton(int idKiller, float minus, int _typeKills, int idTurret)
	{
		this.MinusLiveRPCPhoton(idKiller, minus, _typeKills, 0, idTurret, null);
	}

	[PunRPC]
	[RPC]
	public void MinusLiveRPCWithTurretPhoton(int idKiller, float minus, int _typeKills, int idTurret, string weaponName)
	{
		this.MinusLiveRPCPhoton(idKiller, minus, _typeKills, 0, idTurret, null);
	}

	public bool MinusMechHealth(float _minus)
	{
		this.liveMech -= _minus;
		if (this.liveMech > 0f)
		{
			return false;
		}
		this.DeactivateMech();
		return true;
	}

	public bool NeedAmmo()
	{
		if (this._weaponManager == null)
		{
			return false;
		}
		int currentWeaponIndex = this._weaponManager.CurrentWeaponIndex;
		Weapon item = (Weapon)this._weaponManager.playerWeapons[currentWeaponIndex];
		return item.currentAmmoInBackpack < this._weaponManager.currentWeaponSounds.MaxAmmoWithEffectApplied;
	}

	public void OnApplicationPause(bool pause)
	{
		if (!this._singleOrMultiMine())
		{
			return;
		}
		if (pause)
		{
			this.ActualizeNumberOfGrenades();
			if (Application.platform == RuntimePlatform.IPhonePlayer && this.liveTime > 90f)
			{
				this.pausedRating = true;
				this.myNetworkStartTable.CalculateMatchRating(true);
			}
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer && this.pausedRating)
		{
			this.pausedRating = false;
			RatingSystem.instance.BackupLastRatingTake();
		}
	}

	private void OnDestroy()
	{
		if (this.isMine && Defs.isMulti && Defs.isInet && FriendsController.useBuffSystem)
		{
			BuffSystem.instance.PlayerLeaved();
		}
		this._bodyMaterial = null;
		this._mechMaterial = null;
		this._bearMaterial = null;
		Initializer.players.Remove(this);
		Initializer.playersObj.Remove(this.myPlayerTransform.gameObject);
		if (Defs.isMulti && Defs.isInet)
		{
			BonusController.sharedController.lowLevelPlayers.Remove(this.photonView.ownerId);
		}
		if (Initializer.bluePlayers.Contains(this))
		{
			Initializer.bluePlayers.Remove(this);
		}
		if (Initializer.redPlayers.Contains(this))
		{
			Initializer.redPlayers.Remove(this);
		}
		if (Defs.isCapturePoints && CapturePointController.sharedController != null)
		{
			for (int i = 0; i < (int)CapturePointController.sharedController.basePointControllers.Length; i++)
			{
				if (CapturePointController.sharedController.basePointControllers[i].capturePlayers.Contains(this))
				{
					CapturePointController.sharedController.basePointControllers[i].capturePlayers.Remove(this);
				}
			}
		}
		if (this._weaponPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveWeaponPopularity();
			this._weaponPopularityCacheIsDirty = false;
		}
		if (!this.isMulti)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
		}
		if (this._singleOrMultiMine())
		{
			this.ActualizeNumberOfGrenades();
			this.SaveKillRate();
			if (this.networkStartTableNGUIController != null)
			{
				this.networkStartTableNGUIController.ranksInterface.SetActive(false);
			}
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.resumeAction = null;
			}
			if (this.inGameGUI && this.inGameGUI.gameObject)
			{
				if (this.isHunger || Defs.isRegimVidosDebug)
				{
					this.inGameGUI.topAnchor.SetActive(false);
					this.inGameGUI.leftAnchor.SetActive(false);
					this.inGameGUI.rightAnchor.SetActive(false);
					this.inGameGUI.joystickContainer.SetActive(false);
					this.inGameGUI.bottomAnchor.SetActive(false);
					this.inGameGUI.fastShopPanel.SetActive(false);
					this.inGameGUI.swipeWeaponPanel.gameObject.SetActive(false);
					this.inGameGUI.turretPanel.SetActive(false);
					for (int j = 0; j < 3; j++)
					{
						if (this.inGameGUI.messageAddScore[j].gameObject.activeSelf)
						{
							this.inGameGUI.messageAddScore[j].gameObject.SetActive(false);
						}
					}
				}
				else
				{
					UnityEngine.Object.Destroy(this.inGameGUI.gameObject);
				}
			}
			if (ChatViewrController.sharedController != null)
			{
				ChatViewrController.sharedController.CloseChat(true);
			}
			if (coinsShop.thisScript != null && coinsShop.thisScript.enabled)
			{
				coinsShop.ExitFromShop(false);
			}
			coinsPlashka.hidePlashka();
		}
		if (this.isMulti && this.isMine && CameraSceneController.sharedController != null)
		{
			CameraSceneController.sharedController.SetTargetKillCam(null);
		}
		if (!this.isMulti || this.isMine)
		{
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				GoogleIABManager.purchaseSucceededEvent -= new Action<GooglePurchase>(this.purchaseSuccessful);
			}
			else
			{
				AmazonIapV2Impl.Instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessful));
			}
			if (Defs.isTurretWeapon && this.currentTurret != null)
			{
				if (!Defs.isMulti)
				{
					UnityEngine.Object.Destroy(this.currentTurret);
				}
				else if (!Defs.isInet)
				{
					Network.RemoveRPCs(this.currentTurret.GetComponent<NetworkView>().viewID);
					Network.Destroy(this.currentTurret);
				}
				else
				{
					PhotonNetwork.Destroy(this.currentTurret);
				}
			}
		}
		if (this._singleOrMultiMine() || this._weaponManager != null && this._weaponManager.myPlayer == this.myPlayerTransform.gameObject)
		{
			if (this._pauser != null && this._pauser && this._pauser.paused)
			{
				this._pauser.paused = !this._pauser.paused;
				Time.timeScale = 1f;
				this.AddButtonHandlers();
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("DamageFrame");
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			this.RemoveButtonHandelrs();
			ShopNGUIController.sharedShop.buyAction = null;
			ShopNGUIController.sharedShop.equipAction = null;
			ShopNGUIController.sharedShop.activatePotionAction = null;
			ShopNGUIController.sharedShop.resumeAction = null;
			ShopNGUIController.sharedShop.wearEquipAction = null;
			ShopNGUIController.sharedShop.wearUnequipAction = null;
			ZombieCreator.BossKilled -= new Action(this.CheckTimeCondition);
			ShopNGUIController.ShowArmorChanged -= new Action(this.HandleShowArmorChanged);
		}
		if (this.isMulti && this.isMine)
		{
			ProfileController.ResaveStatisticToKeychain();
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		if (Defs.isMulti && Defs.isCOOP)
		{
			int num = Storager.getInt(Defs.COOPScore, false);
			int num1 = (this.myNetworkStartTable.score != -1 ? this.myNetworkStartTable.score : this.myNetworkStartTable.scoreOld);
			if (num1 > num)
			{
				Storager.setInt(Defs.COOPScore, num1, false);
			}
		}
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView && this.photonView.isMine)
		{
			this.photonView.RPC("CountKillsCommandSynch", player, new object[] { this.countKillsCommandBlue, this.countKillsCommandRed });
			this.photonView.RPC("SetInvisibleRPC", player, new object[] { (!Defs.isDaterRegim ? this.isInvisible : this.isBigHead) });
			this.photonView.RPC("SetWeaponRPC", player, new object[] { ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).weaponPrefab.name, ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).weaponPrefab.GetComponent<WeaponSounds>().alternativeName });
			this.SendSynhHealth(true, player);
			if (Defs.isJetpackEnabled)
			{
				this.photonView.RPC("SetJetpackEnabledRPC", player, new object[] { Defs.isJetpackEnabled });
			}
			if (this.isMechActive || this.isBearActive)
			{
				this.photonView.RPC("ActivateMechRPC", player, new object[] { this.mechUpgrade });
			}
			this.photonView.RPC("SynhIsZoming", player, new object[] { this.isZooming });
			if (FriendsController.useBuffSystem || KillRateCheck.instance.buffEnabled)
			{
				this.photonView.RPC("SendBuffParameters", player, new object[] { this.damageBuff, this.protectionBuff });
			}
		}
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (this.isMine)
		{
			this._networkView.RPC("SetInvisibleRPC", player, new object[] { (!Defs.isDaterRegim ? this.isInvisible : this.isBigHead) });
			this._networkView.RPC("CountKillsCommandSynch", player, new object[] { this.countKillsCommandBlue, this.countKillsCommandRed });
			this._networkView.RPC("SetWeaponRPC", player, new object[] { ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).weaponPrefab.name, ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).weaponPrefab.GetComponent<WeaponSounds>().alternativeName });
			this.SendSynhHealth(true, null);
			if (Defs.isJetpackEnabled)
			{
				this._networkView.RPC("SetJetpackEnabledRPC", player, new object[] { Defs.isJetpackEnabled });
			}
			if (this.isMechActive || this.isBearActive)
			{
				this._networkView.RPC("ActivateMechRPC", player, new object[] { this.mechUpgrade });
			}
			this._networkView.RPC("SynhIsZoming", player, new object[] { this.isZooming });
		}
	}

	public static void PerformActionRecurs(GameObject obj, Action<Transform> act)
	{
		if (act == null || null == obj)
		{
			return;
		}
		act(obj.transform);
		int num = obj.transform.childCount;
		Transform transforms = obj.transform;
		for (int i = 0; i < num; i++)
		{
			Transform child = transforms.GetChild(i);
			if (null != child)
			{
				Player_move_c.PerformActionRecurs(child.gameObject, act);
			}
		}
	}

	public void PlayPortalSound()
	{
		if (!Defs.isMulti)
		{
			this.PlayPortalSoundRPC();
		}
		else if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("PlayPortalSoundRPC", RPCMode.All, new object[0]);
		}
		else
		{
			this.photonView.RPC("PlayPortalSoundRPC", PhotonTargets.All, new object[0]);
		}
	}

	[PunRPC]
	[RPC]
	public void PlayPortalSoundRPC()
	{
		if (Defs.isSoundFX && this.portalSound != null)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.portalSound);
		}
	}

	[PunRPC]
	[RPC]
	private void plusCountKillsCommand(int _command)
	{
		UnityEngine.Debug.Log(string.Concat("plusCountKillsCommand: ", _command));
		if (_command == 1)
		{
			if (!this._weaponManager || !this._weaponManager.myPlayer)
			{
				GlobalGameController.countKillsBlue++;
			}
			else
			{
				Player_move_c playerMoveC = this._weaponManager.myPlayerMoveC;
				playerMoveC.countKillsCommandBlue = playerMoveC.countKillsCommandBlue + 1;
			}
		}
		if (_command == 2)
		{
			if (!this._weaponManager || !this._weaponManager.myPlayer)
			{
				GlobalGameController.countKillsRed++;
			}
			else
			{
				Player_move_c playerMoveC1 = this._weaponManager.myPlayerMoveC;
				playerMoveC1.countKillsCommandRed = playerMoveC1.countKillsCommandRed + 1;
			}
		}
	}

	[PunRPC]
	[RPC]
	public void pobeda(NetworkViewID idKiller)
	{
		foreach (Player_move_c player in Initializer.players)
		{
			if (!idKiller.Equals(player.mySkinName.GetComponent<NetworkView>().viewID))
			{
				continue;
			}
			this.nickPobeditel = player.mySkinName.NickName;
		}
		if (this._weaponManager && this._weaponManager.myTable)
		{
			this._weaponManager.myNetworkStartTable.win(this.nickPobeditel, 0, 0, 0);
		}
	}

	[PunRPC]
	[RPC]
	public void pobedaPhoton(int idKiller, int _command)
	{
		foreach (Player_move_c player in Initializer.players)
		{
			if (idKiller != player.mySkinName.photonView.viewID)
			{
				continue;
			}
			this.nickPobeditel = player.mySkinName.NickName;
		}
		if (!(this._weaponManager != null) || !(this._weaponManager.myTable != null))
		{
			UnityEngine.Debug.Log("_weaponManager.myTable==null");
		}
		else
		{
			this._weaponManager.myNetworkStartTable.win(this.nickPobeditel, _command, 0, 0);
		}
	}

	private void ProvideAmmo(string inShopId)
	{
		this._listener.ProvideContent();
		this._weaponManager.SetMaxAmmoFrAllWeapons();
		if (JoystickController.rightJoystick == null)
		{
			UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
		}
		else
		{
			if (this.inGameGUI != null)
			{
				this.inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
	}

	private void ProvideHealth(string inShopId)
	{
		this.CurHealth = this.MaxHealth;
		CurrentCampaignGame.withoutHits = true;
	}

	private void providePotion(string inShopId)
	{
	}

	private void purchaseSuccessful(GooglePurchase purchase)
	{
		try
		{
			if (purchase == null)
			{
				throw new ArgumentNullException("purchase");
			}
			this.PurchaseSuccessful(purchase.productId);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(exception);
		}
	}

	public void PurchaseSuccessful(string id)
	{
		if (this._actionsForPurchasedItems.ContainsKey(id))
		{
			this._actionsForPurchasedItems[id](id);
		}
		this._timeWhenPurchShown = Time.realtimeSinceStartup;
	}

	public void QuitGame()
	{
		Time.timeScale = 1f;
		Time.timeScale = 1f;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = Defs.MainMenuScene;
			Application.LoadLevel("LevelToCompleteProm");
		}
		else if (this.isMulti)
		{
			if (EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Paused || EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Recording)
			{
				EveryplayWrapper.Instance.Stop();
			}
			if (this.isInet)
			{
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
				PhotonNetwork.LeaveRoom();
			}
			else
			{
				if (PlayerPrefs.GetString("TypeGame").Equals("server"))
				{
					Network.Disconnect(200);
					GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
				}
				else if ((int)Network.connections.Length == 1)
				{
					Network.CloseConnection(Network.connections[0], true);
				}
				ActivityIndicator.IsActiveIndicator = false;
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				ConnectSceneNGUIController.Local();
			}
		}
		else if (!Defs.IsSurvival)
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "ChooseLevel";
			bool flag = !this.isMulti;
			if (!flag)
			{
				FlurryPluginWrapper.LogEvent("Back to Main Menu");
			}
			Application.LoadLevel((!flag ? Defs.MainMenuScene : "LevelToCompleteProm"));
		}
		else
		{
			if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
			{
				GlobalGameController.HasSurvivalRecord = true;
				PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
				FriendsController.sharedController.survivalScore = GlobalGameController.Score;
				FriendsController.sharedController.SendOurData(false);
			}
			if (Storager.getInt("SendFirstResaltArena", false) != 1)
			{
				Storager.setInt("SendFirstResaltArena", 1, false);
				AnalyticsStuff.LogArenaFirst(true, false);
			}
			UnityEngine.Debug.Log(string.Concat("Player_move_c.QuitGame(): Trying to report survival score: ", GlobalGameController.Score));
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				AGSLeaderboardsClient.SubmitScore("best_survival_scores", (long)GlobalGameController.Score, 0);
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite && Social.localUser.authenticated)
			{
				Social.ReportScore((long)GlobalGameController.Score, "CgkIr8rGkPIJEAIQCg", (bool success) => UnityEngine.Debug.Log(string.Concat("Player_move_c.QuitGame(): ", (!success ? "Failed to report score." : "Reported score successfully."))));
			}
			PlayerPrefs.SetInt("IsGameOver", 1);
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
			Application.LoadLevel("LevelToCompleteProm");
		}
	}

	private void RailgunShot(WeaponSounds weapon)
	{
		bool flag;
		Vector3 gunFlash;
		float single;
		weapon.fire();
		this._FireFlash(true, 0);
		float single1 = weapon.tekKoof * Defs.Coef;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(((float)Screen.width - weapon.startZone.x * single1) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.x * single1)), ((float)Screen.height - weapon.startZone.y * single1) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.y * single1)), 0f));
		if (!weapon.freezer)
		{
			bool flag1 = false;
			int num = 0;
			do
			{
				Player_move_c.RayHitsInfo hitsFromRay = this.GetHitsFromRay(ray, (weapon.countReflectionRay != 1 ? false : true));
				RaycastHit[] raycastHitArray = hitsFromRay.hits;
				for (int i = 0; i < (int)raycastHitArray.Length; i++)
				{
					this._DoHit(raycastHitArray[i], false);
				}
				if (num != 0)
				{
					flag = false;
				}
				else
				{
					flag = (weapon.countReflectionRay == 1 ? true : !hitsFromRay.obstacleFound);
				}
				bool flag2 = flag;
				Vector3 vector3 = (num != 0 ? ray.origin : this.GunFlash.gameObject.transform.parent.position);
				if (!flag2)
				{
					gunFlash = (num != 0 ? ray.direction : hitsFromRay.rayReflect.origin - this.GunFlash.gameObject.transform.parent.position);
				}
				else
				{
					gunFlash = this.GunFlash.gameObject.transform.parent.parent.forward;
				}
				Vector3 vector31 = gunFlash;
				if (flag2)
				{
					single = 150f;
				}
				else if (num != 0)
				{
					single = hitsFromRay.lenRay;
				}
				else
				{
					Vector3 gunFlash1 = hitsFromRay.rayReflect.origin - this.GunFlash.gameObject.transform.parent.position;
					single = gunFlash1.magnitude;
				}
				float single2 = single;
				base.StartCoroutine(this.ShowRayWithDelay(vector3, vector31, weapon.railName, single2, (float)num * 0.05f));
				if (hitsFromRay.obstacleFound)
				{
					ray = hitsFromRay.rayReflect;
					flag1 = true;
				}
				num++;
			}
			while (flag1 && num < weapon.countReflectionRay);
		}
		else
		{
			Player_move_c.RayHitsInfo rayHitsInfo = this.GetHitsFromRay(ray, false);
			RaycastHit[] raycastHitArray1 = rayHitsInfo.hits;
			for (int j = 0; j < (int)raycastHitArray1.Length; j++)
			{
				this._DoHit(raycastHitArray1[j], true);
			}
			this.AddFreezerRayWithLength(rayHitsInfo.lenRay);
			if (this.isMulti)
			{
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("AddFreezerRayWithLength", RPCMode.Others, new object[] { rayHitsInfo.lenRay });
				}
				else
				{
					this.photonView.RPC("AddFreezerRayWithLength", PhotonTargets.Others, new object[] { rayHitsInfo.lenRay });
				}
			}
		}
	}

	public void RanksPressed()
	{
		if (this.mySkinName.playerMoveC.isKilled)
		{
			return;
		}
		this.ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		this.RemoveButtonHandelrs();
		this.showRanks = true;
		this.networkStartTableNGUIController.winnerPanelCom1.SetActive(false);
		this.networkStartTableNGUIController.winnerPanelCom2.SetActive(false);
		this.networkStartTableNGUIController.ShowRanksTable();
		this.inGameGUI.gameObject.SetActive(false);
	}

	private void Reload()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && this.inGameGUI != null)
		{
			if (WeaponManager.sharedManager.currentWeaponSounds.ammoInClip > 1 || !WeaponManager.sharedManager.currentWeaponSounds.isShotMelee)
			{
				this.inGameGUI.ShowCircularIndicatorOnReload(WeaponManager.sharedManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].length / this._currentReloadAnimationSpeed);
			}
			else
			{
				WeaponManager.sharedManager.ReloadAmmo();
			}
		}
		WeaponManager.sharedManager.Reload();
	}

	[PunRPC]
	[RPC]
	private void ReloadGun()
	{
		if (this.myCurrentWeaponSounds == null)
		{
			return;
		}
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].speed = this._currentReloadAnimationSpeed;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.myCurrentWeaponSounds.reload);
		}
	}

	[Obfuscation(Exclude=true)]
	public void ReloadPressed()
	{
		if (this.myCurrentWeaponSounds.isCharging && this.chargeValue > 0f)
		{
			return;
		}
		if (this.isGrenadePress || this.isReloading)
		{
			return;
		}
		if (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee)
		{
			return;
		}
		if (this.isZooming)
		{
			this.ZoomPress();
		}
		if (this._weaponManager.CurrentWeaponIndex < 0 || this._weaponManager.CurrentWeaponIndex >= this._weaponManager.playerWeapons.Count)
		{
			return;
		}
		if (((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack > 0 && ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).currentAmmoInClip != this._weaponManager.currentWeaponSounds.ammoInClip)
		{
			this.Reload();
			if (this._weaponManager.currentWeaponSounds.isShotMelee)
			{
				return;
			}
			if (this.isMulti)
			{
				if (this.isInet)
				{
					this.photonView.RPC("ReloadGun", PhotonTargets.Others, new object[0]);
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("ReloadGun", RPCMode.Others, new object[0]);
				}
			}
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.reload);
			}
			if (JoystickController.rightJoystick == null)
			{
				UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
			}
			else
			{
				JoystickController.rightJoystick.HasAmmo();
				if (this.inGameGUI != null)
				{
					this.inGameGUI.BlinkNoAmmo(0);
				}
			}
		}
	}

	public void RemoveButtonHandelrs()
	{
		PauseTapReceiver.PauseClicked -= new Action(this.SwitchPause);
		ShopTapReceiver.ShopClicked -= new Action(this.ShopPressed);
		RanksTapReceiver.RanksClicked -= new Action(this.RanksPressed);
		TopPanelsTapReceiver.OnClicked -= new Action(this.RanksPressed);
		ChatTapReceiver.ChatClicked -= new Action(this.ShowChat);
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(false);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(false);
		}
	}

	private void ResetHouseKeeperEvent()
	{
		this.countHouseKeeperEvent = 0;
	}

	public void resetMultyKill()
	{
		this.multiKill = 0;
		for (int i = 0; i < (int)this.counterSerials.Length; i++)
		{
			this.counterSerials[i] = 0;
		}
	}

	private void ResetMySpotEvent()
	{
		this.countMySpotEvent = 0;
	}

	public void ResetShootingBurst()
	{
		this._countShootInBurst = 0;
		this._timerDelayInShootingBurst = -1f;
	}

	public void RespawnPlayer()
	{
		string str;
		Defs.inRespawnWindow = false;
		this.respawnedForGUI = true;
		this.SetMapCameraActive(false);
		this._killerInfo.Reset();
		Func<bool> func = () => (this._pauser == null ? false : this._pauser.paused);
		if (base.transform.parent == null)
		{
			UnityEngine.Debug.Log("transform.parent == null");
			return;
		}
		this.myPlayerTransform.localScale = new Vector3(1f, 1f, 1f);
		this.myTransform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		if (this.isHunger || Defs.isRegimVidosDebug)
		{
			this.myTable.GetComponent<NetworkStartTable>().ImDeadInHungerGames();
			PhotonNetwork.Destroy(this.myPlayerTransform.gameObject);
			return;
		}
		this.InitiailizeIcnreaseArmorEffectFlags();
		this.isDeadFrame = false;
		this.isImmortality = true;
		this.timerImmortality = this.maxTimerImmortality;
		this.SetNoKilled();
		if (this._weaponManager.myPlayer == null)
		{
			UnityEngine.Debug.Log("_weaponManager.myPlayer == null");
			return;
		}
		this._weaponManager.myPlayerMoveC.mySkinName.camPlayer.transform.parent = this._weaponManager.myPlayer.transform;
		if (!func())
		{
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.gameObject.SetActive(true);
				JoystickController.rightJoystick._isFirstFrame = false;
			}
		}
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(true);
		}
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.MakeActive();
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(true);
		}
		if (JoystickController.rightJoystick == null)
		{
			UnityEngine.Debug.Log("JoystickController.rightJoystick = null");
		}
		else
		{
			if (this.inGameGUI != null)
			{
				this.inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		this.CurHealth = this.MaxHealth;
		Wear.RenewCurArmor(this.TierOrRoomTier((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier)));
		this.CurrentBaseArmor = EffectsController.ArmorBonus;
		if (this.isCOOP)
		{
			str = "MultyPlayerCreateZoneCOOP";
		}
		else if (this.isCompany)
		{
			str = string.Concat("MultyPlayerCreateZoneCommand", this.myCommand);
		}
		else if (!Defs.isFlag)
		{
			str = (!Defs.isCapturePoints ? "MultyPlayerCreateZone" : string.Concat("MultyPlayerCreateZonePointZone", this.myCommand));
		}
		else
		{
			str = string.Concat("MultyPlayerCreateZoneFlagCommand", this.myCommand);
		}
		this.zoneCreatePlayer = GameObject.FindGameObjectsWithTag(str);
		GameObject gameObject = this.zoneCreatePlayer[UnityEngine.Random.Range(0, (int)this.zoneCreatePlayer.Length - 1)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		float single = component.size.x * gameObject.transform.localScale.x;
		float single1 = component.size.z;
		Vector3 vector3 = gameObject.transform.localScale;
		Vector2 vector2 = new Vector2(single, single1 * vector3.z);
		Vector3 vector31 = gameObject.transform.position;
		float single2 = vector31.x - vector2.x / 2f;
		Vector3 vector32 = gameObject.transform.position;
		Rect rect = new Rect(single2, vector32.z - vector2.y / 2f, vector2.x, vector2.y);
		float single3 = rect.x + UnityEngine.Random.Range(0f, rect.width);
		Vector3 vector33 = gameObject.transform.position;
		Vector3 vector34 = new Vector3(single3, vector33.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		Quaternion quaternion = gameObject.transform.rotation;
		this.myPlayerTransform.position = vector34;
		this.myPlayerTransform.rotation = quaternion;
		if (Storager.getInt("GrenadeID", false) <= 0)
		{
			Storager.setInt("GrenadeID", 1, false);
		}
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		Vector3 vector35 = this.myCamera.transform.rotation.eulerAngles;
		this.myCamera.transform.rotation = Quaternion.Euler(0f, vector35.y, vector35.z);
		base.Invoke("ChangePositionAfterRespawn", 0.01f);
		IEnumerator enumerator = this._weaponManager.allAvailablePlayerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				current.currentAmmoInClip = current.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
				current.currentAmmoInBackpack = current.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		if (WeaponManager.sharedManager != null)
		{
			for (int i = 0; i < WeaponManager.sharedManager.playerWeapons.Count; i++)
			{
				WeaponSounds weaponSound = (WeaponManager.sharedManager.playerWeapons[i] as Weapon).weaponPrefab.GetComponent<WeaponSounds>();
				if (weaponSound != null && (!weaponSound.isMelee || weaponSound.isShotMelee))
				{
					WeaponManager.sharedManager.ReloadWeaponFromSet(i);
				}
			}
		}
		EffectsController.SlowdownCoeff = 1f;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showGrenadeHint)
		{
			Player_move_c playerMoveC = this;
			int num = playerMoveC.respawnCountForTraining + 1;
			int num1 = num;
			playerMoveC.respawnCountForTraining = num;
			if (num1 == 2)
			{
				HintController.instance.ShowHintByName("use_grenade", 5f);
				this.respawnCountForTraining = 0;
			}
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showChangeWeaponHint)
		{
			HintController.instance.ShowHintByName("change_weapon", 5f);
		}
	}

	[Obfuscation(Exclude=true)]
	private void ReturnWeaponAfterGrenade()
	{
		this.ChangeWeapon(this.currentWeaponBeforeGrenade, false);
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider != null)
		{
			this.inGameGUI.blockedCollider.SetActive(false);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider2 != null)
		{
			this.inGameGUI.blockedCollider2.SetActive(false);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedColliderDater != null)
		{
			this.inGameGUI.blockedColliderDater.SetActive(false);
		}
		if (this.inGameGUI != null)
		{
			for (int i = 0; i < (int)this.inGameGUI.upButtonsInShopPanel.Length; i++)
			{
				this.inGameGUI.upButtonsInShopPanel[i].GetComponent<ButtonHandler>().isEnable = true;
			}
			for (int j = 0; j < (int)this.inGameGUI.upButtonsInShopPanelSwipeRegim.Length; j++)
			{
				this.inGameGUI.upButtonsInShopPanelSwipeRegim[j].GetComponent<ButtonHandler>().isEnable = true;
			}
		}
	}

	[Obfuscation(Exclude=true)]
	private void RunGrenade()
	{
		if (this.currentGrenade)
		{
			this.currentGrenade.GetComponent<Rigidbody>().isKinematic = false;
			this.currentGrenade.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(0f, -5f, 0f) * 150f * this.myTransform.forward);
			this.currentGrenade.GetComponent<Rigidbody>().useGravity = true;
			this.currentGrenade.GetComponent<Rocket>().StartRocket();
		}
		base.Invoke("ReturnWeaponAfterGrenade", 0.5f);
		this.isGrenadePress = false;
	}

	private void RunOnGroundEffect(string name)
	{
		if (name == null || this.mySkinName == null)
		{
			return;
		}
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName(string.Concat("OnGroundWeaponEffects/", name, "_OnGroundEffect"));
		if (objectFromName == null)
		{
			return;
		}
		Player_move_c.PerformActionRecurs(objectFromName, (Transform t) => t.gameObject.SetActive(false));
		objectFromName.transform.parent = this.mySkinName.onGroundEffectsPoint;
		objectFromName.transform.localPosition = Vector3.zero;
		objectFromName.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		Player_move_c.PerformActionRecurs(objectFromName, (Transform t) => t.gameObject.SetActive(true));
		ParticleSystem component = objectFromName.GetComponent<ParticleSystem>();
		if (component != null)
		{
			component.Play();
		}
	}

	[DebuggerHidden]
	private IEnumerator RunOnGroundEffectCoroutine(string name, float tm)
	{
		Player_move_c.u003cRunOnGroundEffectCoroutineu003ec__IteratorDC variable = null;
		return variable;
	}

	public void RunTurret()
	{
		if (Defs.isTurretWeapon)
		{
			string str = (!Defs.isDaterRegim ? GearManager.Turret : GearManager.MusicBox);
			Storager.setInt(str, Storager.getInt(str, false) - 1, false);
			PotionsController.sharedController.ActivatePotion(GearManager.Turret, this, new Dictionary<string, object>(), false);
			this.currentTurret.transform.parent = null;
			this.currentTurret.GetComponent<TurretController>().StartTurret();
			this.ChangeWeapon(this.currentWeaponBeforeTurret, false);
			this.currentWeaponBeforeTurret = -1;
		}
	}

	private void SaveKillRate()
	{
		try
		{
			if (this.isMulti && !Defs.isHunger && !Defs.isCOOP && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && !Defs.IsSurvival)
			{
				Action<Dictionary<string, int>, Dictionary<string, Dictionary<int, int>>> action = (Dictionary<string, int> battleDict, Dictionary<string, Dictionary<int, int>> dictToDisk) => {
					foreach (KeyValuePair<string, int> keyValuePair in battleDict)
					{
						if (!dictToDisk.ContainsKey(keyValuePair.Key))
						{
							dictToDisk.Add(keyValuePair.Key, new Dictionary<int, int>()
							{
								{ this.tierForKilledRate, keyValuePair.Value }
							});
						}
						else
						{
							Dictionary<int, int> item = dictToDisk[keyValuePair.Key];
							if (!item.ContainsKey(this.tierForKilledRate))
							{
								item.Add(this.tierForKilledRate, keyValuePair.Value);
							}
							else
							{
								Dictionary<int, int> nums = item;
								Dictionary<int, int> nums1 = nums;
								int num = this.tierForKilledRate;
								nums[num] = nums1[num] + keyValuePair.Value;
							}
						}
					}
				};
				action(this.weKillForKillRate, KillRateStatisticsManager.WeKillOld);
				action(this.weWereKilledForKillRate, KillRateStatisticsManager.WeWereKilledOld);
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "version", GlobalGameController.AppVersion },
					{ "wekill", KillRateStatisticsManager.WeKillOld },
					{ "wewerekilled", KillRateStatisticsManager.WeWereKilledOld }
				};
				Storager.setString("KillRateKeyStatistics", Json.Serialize(strs), false);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in save kill rate statistics: ", exception));
		}
	}

	public static void SaveWeaponInPrefs(string weaponTag, int timeForRentIndex = 0)
	{
		string storageIdByTag = ItemDb.GetStorageIdByTag(weaponTag);
		if (storageIdByTag == null)
		{
			int num = TempItemsController.RentTimeForIndex(timeForRentIndex);
			TempItemsController.sharedController.AddTemporaryItem(weaponTag, num);
			return;
		}
		Storager.setInt(storageIdByTag, 1, true);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	[PunRPC]
	private void SendBuffParameters(float damage, float protection)
	{
		if (!this.isMine)
		{
			this.SetBuffParameters(damage, protection);
		}
	}

	public void SendChat(string text, bool clanMode, string iconName)
	{
		if (text.Equals("-=ATTACK!=-"))
		{
			text = LocalizationStore.Get("Key_1086");
		}
		else if (text.Equals("-=HELP!=-"))
		{
			text = LocalizationStore.Get("Key_1087");
		}
		else if (text.Equals("-=OK!=-"))
		{
			text = LocalizationStore.Get("Key_1088");
		}
		else if (!text.Equals("-=NO!=-"))
		{
			text = FilterBadWorld.FilterString(text);
		}
		else
		{
			text = LocalizationStore.Get("Key_1089");
		}
		if (!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(iconName))
		{
			if (this.isInet)
			{
				this.photonView.RPC("SendChatMessageWithIcon", PhotonTargets.All, new object[] { string.Concat("< ", this._weaponManager.myNetworkStartTable.NamePlayer, " > ", text), clanMode, FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName, iconName });
			}
			else
			{
				this._networkView.RPC("SendChatMessageWithIcon", RPCMode.All, new object[] { string.Concat("< ", this._weaponManager.myNetworkStartTable.NamePlayer, " > ", text), clanMode, FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName, iconName });
			}
		}
	}

	[PunRPC]
	[RPC]
	private void SendChatMessage(string text, bool _clanMode, string _clanLogo, string _ClanID, string _clanName)
	{
		this.SendChatMessageWithIcon(text, _clanMode, _clanLogo, _ClanID, _clanName, string.Empty);
	}

	[PunRPC]
	[RPC]
	private void SendChatMessageWithIcon(string text, bool _clanMode, string _clanLogo, string _ClanID, string _clanName, string _iconName)
	{
		if (_clanMode && !_ClanID.Equals(FriendsController.sharedController.ClanID))
		{
			return;
		}
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayerMoveC == null)
		{
			return;
		}
		if (this.isInet)
		{
			this._weaponManager.myPlayerMoveC.AddMessage(text, Time.time, this.mySkinName.photonView.viewID, this.myPlayerTransform.GetComponent<NetworkView>().viewID, this.myCommand, _clanLogo, _iconName);
		}
		else
		{
			this._weaponManager.myPlayerMoveC.AddMessage(text, Time.time, -1, this.myPlayerTransform.GetComponent<NetworkView>().viewID, 0, _clanLogo, _iconName);
		}
	}

	public void SendDamageFromEnv(float damage, Vector3 pos)
	{
		if (this.isInet)
		{
			this.photonView.RPC("GetDamageFromEnvRPC", PhotonTargets.All, new object[] { damage, pos });
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("GetDamageFromEnvRPC", RPCMode.All, new object[] { damage, pos });
		}
	}

	public void SendDaterChat(string nick1, string text, string nick2)
	{
		if (text != string.Empty)
		{
			if (this.isInet)
			{
				this.photonView.RPC("SendDaterChatRPC", PhotonTargets.All, new object[] { nick1, text, nick2, false, FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName });
			}
			else
			{
				this._networkView.RPC("SendDaterChatRPC", RPCMode.All, new object[] { nick1, text, nick2, false, FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName });
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SendDaterChatRPC(string nick1, string text, string nick2, bool _clanMode, string _clanLogo, string _ClanID, string _clanName)
	{
		text = string.Concat(new string[] { "< ", nick1, "[-] > ", LocalizationStore.Get(text), " < ", nick2, "[-] >" });
		this.SendChatMessage(text, _clanMode, _clanLogo, _ClanID, _clanName);
	}

	public void SendHouseKeeperEvent()
	{
		this.countHouseKeeperEvent++;
		if (this.countHouseKeeperEvent == 1)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.houseKeeperPoint, 1f);
		}
		if (this.countHouseKeeperEvent == 3)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.defenderPoint, 1f);
		}
		if (this.countHouseKeeperEvent == 5)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.guardianPoint, 1f);
		}
		if (this.countHouseKeeperEvent == 10)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.oneManArmyPoint, 1f);
		}
	}

	public void sendImDeath(string _name)
	{
		if (this.isInet)
		{
			this.photonView.RPC("imDeath", PhotonTargets.All, new object[] { _name });
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("imDeath", RPCMode.All, new object[] { _name });
		}
		this._killerInfo.isSuicide = true;
	}

	public void SendImKilled()
	{
		if (Defs.isInet)
		{
			this.photonView.RPC("ImKilled", PhotonTargets.All, new object[] { this.myPlayerTransform.position, this.myPlayerTransform.rotation, 0 });
			this.SendSynhHealth(false, null);
		}
	}

	public void SendLike(Player_move_c whomMoveC)
	{
		if (whomMoveC != null)
		{
			whomMoveC.SendDaterChat(this.mySkinName.NickName, "Key_1803", whomMoveC.mySkinName.NickName);
		}
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("LikeRPCLocal", RPCMode.All, new object[] { base.GetComponent<NetworkView>().viewID, whomMoveC.GetComponent<NetworkView>().viewID });
		}
		else
		{
			this.photonView.RPC("LikeRPC", PhotonTargets.All, new object[] { this.photonView.ownerId, whomMoveC.photonView.ownerId });
		}
	}

	public void SendMySpotEvent()
	{
		this.countMySpotEvent++;
		if (this.countMySpotEvent == 1)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.mySpotPoint, 1f);
		}
		if (this.countMySpotEvent == 2)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.unstoppablePoint, 1f);
		}
		if (this.countMySpotEvent >= 3)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.monopolyPoint, 1f);
		}
	}

	public void SendStartFlashMine()
	{
		if (this.isInet)
		{
			this.photonView.RPC("StartFlashRPC", PhotonTargets.All, new object[0]);
		}
		else
		{
			this._networkView.RPC("StartFlashRPC", RPCMode.All, new object[0]);
		}
	}

	public void SendSynhHealth(bool isUp, PhotonPlayer player = null)
	{
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SynhHealthRPC", RPCMode.All, new object[] { this.CurHealth + this.curArmor, this.curArmor, isUp });
		}
		else if (player != null)
		{
			this.photonView.RPC("SynhHealthRPC", player, new object[] { this.CurHealth + this.curArmor, this.curArmor, isUp });
		}
		else
		{
			this.photonView.RPC("SynhHealthRPC", PhotonTargets.All, new object[] { this.CurHealth + this.curArmor, this.curArmor, isUp });
		}
	}

	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagAddScoreRPC(bool isCommandBlue, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot((isCommandBlue != (this._weaponManager.myPlayerMoveC.myCommand == 1) ? this.flagScoreEnemyClip : this.flagScoreMyCommandClip));
			}
			this.isCaptureFlag = false;
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(nick, 5);
		}
	}

	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagCaptureRPC(bool isBlueFlag, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1 != isBlueFlag)
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1002"));
				if (Defs.isSoundFX)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.flagGetClip);
				}
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_1001")));
				if (Defs.isSoundFX)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.flagLostClip);
				}
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagDroppedRPC(bool isBlueFlag, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if ((!isBlueFlag || WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1) && (isBlueFlag || WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2))
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_1799")));
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_1798")));
			}
		}
	}

	public void SendSystemMessegeFromFlagReturned(bool isBlueFlag)
	{
		this.photonView.RPC("SendSystemMessegeFromFlagReturnedRPC", PhotonTargets.All, new object[] { isBlueFlag });
	}

	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagReturnedRPC(bool isBlueFlag)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if ((!isBlueFlag || WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1) && (isBlueFlag || WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2))
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1801"));
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1800"));
			}
		}
	}

	private void SetBuffParameters(float damage, float protection)
	{
		this.damageBuff = Mathf.Clamp(damage, 0.01f, 10f);
		this.protectionBuff = Mathf.Clamp(protection, 0.01f, 10f);
		UnityEngine.Debug.Log(string.Format("<color=green>{0}Damage: {1}, Protection: {2}</color>", (!this.isMine ? string.Concat("(", this.mySkinName.NickName, ") ") : "(you) "), this.damageBuff, this.protectionBuff));
	}

	[DebuggerHidden]
	private IEnumerator SetCanReceiveSwipes()
	{
		Player_move_c.u003cSetCanReceiveSwipesu003ec__IteratorDE variable = null;
		return variable;
	}

	[Obfuscation(Exclude=true)]
	private void SetGrenateFireEnabled()
	{
		Defs.isGrenateFireEnable = true;
	}

	public void SetIDMyTable(string _id)
	{
		this.myTableId = _id;
		base.Invoke("SetIDMyTableInvoke", 0.1f);
	}

	[Obfuscation(Exclude=true)]
	private void SetIDMyTableInvoke()
	{
		base.GetComponent<NetworkView>().RPC("SetIDMyTableRPC", RPCMode.AllBuffered, new object[] { this.myTableId });
	}

	[PunRPC]
	[RPC]
	private void SetIDMyTableRPC(string _id)
	{
		this.myTableId = _id;
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("NetworkTable");
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			GameObject gameObject = gameObjectArray[i];
			if (gameObject.GetComponent<NetworkView>().viewID.ToString().Equals(_id))
			{
				this.myTable = gameObject;
				this.setMyTamble(this.myTable);
			}
		}
	}

	private void SetInApp()
	{
		this.isInappWinOpen = !this.isInappWinOpen;
		if (!this.isInappWinOpen)
		{
			if (InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.activeSelf)
			{
				InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(false);
				InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive((TrainingController.TrainingCompleted ? true : TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None));
			}
			if (InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.activeSelf)
			{
				InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(false);
				InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(true);
			}
			ActivityIndicator.IsActiveIndicator = false;
			if (this._pauser == null)
			{
				UnityEngine.Debug.LogWarning("SetInApp(): _pauser is null.");
			}
			else if (!this._pauser.paused)
			{
				Time.timeScale = 1f;
			}
		}
		else
		{
			if (StoreKitEventListener.restoreInProcess)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (!this.isMulti)
			{
				Time.timeScale = 0f;
			}
		}
	}

	public void setInString(string nick)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		this._weaponManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_0995")));
	}

	public void SetInvisible(bool _isInvisible)
	{
		if (!this.isMulti)
		{
			this.SetInvisibleRPC(_isInvisible);
		}
		else if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SetInvisibleRPC", RPCMode.All, new object[] { _isInvisible });
		}
		else if (this.photonView != null)
		{
			this.photonView.RPC("SetInvisibleRPC", PhotonTargets.All, new object[] { _isInvisible });
		}
	}

	[PunRPC]
	[RPC]
	private void SetInvisibleRPC(bool _isInvisible)
	{
		if (Defs.isDaterRegim)
		{
			if (this.isBigHead == _isInvisible)
			{
				return;
			}
			this.isBigHead = _isInvisible;
			if (Defs.isSoundFX && _isInvisible)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.invisibleActivSound);
			}
			if (this.isMulti && !this.isMine)
			{
				if (!_isInvisible)
				{
					this.MechHeadTransform.localScale = Vector3.one;
					this.PlayerHeadTransform.localScale = Vector3.one;
					if (!this.isBearActive)
					{
						this.nickLabel.transform.localPosition = Vector3.up * 1.08f;
					}
					else
					{
						this.nickLabel.transform.localPosition = Vector3.up * 1.54f;
					}
				}
				else
				{
					this.MechHeadTransform.localScale = Vector3.one * 2f;
					this.PlayerHeadTransform.localScale = Vector3.one * 2f;
					if (!this.isBearActive)
					{
						this.nickLabel.transform.localPosition = 1.678f * Vector3.up;
					}
					else
					{
						this.nickLabel.transform.localPosition = 2.549f * Vector3.up;
					}
				}
			}
		}
		else
		{
			if (this.isInvisible == _isInvisible)
			{
				return;
			}
			this.isInvisible = _isInvisible;
			if (Defs.isSoundFX && _isInvisible)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.invisibleActivSound);
			}
			if (!this.isMulti || this.isMine)
			{
				this.SetInVisibleShaders(this.isInvisible);
			}
			else
			{
				this.SetNicklabelVisible();
				if (this.isInvisible)
				{
					this.invisibleParticle.SetActive(true);
					this.mySkinName.FPSplayerObject.SetActive(false);
					this.mechPoint.SetActive(false);
				}
				else
				{
					this.invisibleParticle.SetActive(false);
					if (!this.isMechActive)
					{
						this.mySkinName.FPSplayerObject.SetActive(true);
					}
					else
					{
						this.mechPoint.SetActive(true);
					}
				}
			}
		}
	}

	private void SetInVisibleShaders(bool _isInvisible)
	{
		if (this.isGrenadePress)
		{
			return;
		}
		if (!_isInvisible)
		{
			if (WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab != null)
			{
				WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
				if (WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() != null)
				{
					for (int i = 0; i < (int)WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials.Length; i++)
					{
						WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[i].shader = this.oldShadersInInvisible[i + 1];
						WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[i].color = this.oldColorInInvisible[i + 1];
					}
				}
			}
			this._mechMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
			this.mechGunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		}
		else
		{
			if (WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab != null)
			{
				this.oldShadersInInvisible = new Shader[(int)WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().materials.Length + (WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() == null ? 0 : (int)WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials.Length)];
				this.oldColorInInvisible = new Color[(int)this.oldShadersInInvisible.Length];
				this.oldShadersInInvisible[0] = WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.shader;
				WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.shader = Shader.Find("Mobile/Diffuse-Color");
				WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
				this.oldColorInInvisible[0] = WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.color;
				if (WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() != null)
				{
					for (int j = 0; j < (int)WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials.Length; j++)
					{
						this.oldShadersInInvisible[j + 1] = WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[j].shader;
						this.oldColorInInvisible[j + 1] = WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[j].color;
						WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[j].shader = Shader.Find("Mobile/Diffuse-Color");
						WeaponManager.sharedManager.currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[j].SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
					}
				}
			}
			this._mechMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			this.mechGunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
		}
	}

	[PunRPC]
	[RPC]
	private void setIp(string _ip)
	{
		this.myIp = _ip;
	}

	[Obfuscation(Exclude=true)]
	private void setisDeadFrameFalse()
	{
		this.isDeadFrame = false;
	}

	public void SetJetpackEnabled(bool _isEnabled)
	{
		Defs.isJetpackEnabled = _isEnabled;
		if (Defs.isSoundFX && _isEnabled)
		{
			AudioSource component = base.GetComponent<AudioSource>();
			if (component != null)
			{
				component.PlayOneShot(this.jetpackActivSound);
			}
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				if (this.photonView != null)
				{
					this.photonView.RPC("SetJetpackEnabledRPC", PhotonTargets.Others, new object[] { _isEnabled });
				}
			}
			else if (this._networkView != null)
			{
				this._networkView.RPC("SetJetpackEnabledRPC", RPCMode.Others, new object[] { _isEnabled });
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SetJetpackEnabledRPC(bool _isEnabled)
	{
		if (Defs.isSoundFX && _isEnabled)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.jetpackActivSound);
		}
		if (!Defs.isDaterRegim)
		{
			this.jetPackPoint.SetActive(_isEnabled);
			this.jetPackPointMech.SetActive(_isEnabled);
		}
		else
		{
			this.wingsPoint.SetActive(_isEnabled);
			this.wingsPointBear.SetActive(_isEnabled);
		}
		if (!_isEnabled)
		{
			for (int i = 0; i < (int)this.jetPackParticle.Length; i++)
			{
				this.jetPackParticle[i].enableEmission = _isEnabled;
			}
		}
	}

	public void SetJetpackParticleEnabled(bool _isEnabled)
	{
		if (_isEnabled)
		{
			if (Defs.isDaterRegim)
			{
				this.isPlayerFlying = true;
			}
			if (ButtonClickSound.Instance != null && Defs.isSoundFX && !Defs.isDaterRegim)
			{
				this.jetPackSound.SetActive(true);
			}
		}
		else if (!Defs.isDaterRegim)
		{
			this.jetPackSound.SetActive(false);
		}
		else
		{
			this.isPlayerFlying = false;
		}
		if (Defs.isMulti)
		{
			if (!Defs.isInet)
			{
				this._networkView.RPC("SetJetpackParticleEnabledRPC", RPCMode.Others, new object[] { _isEnabled });
			}
			else
			{
				this.photonView.RPC("SetJetpackParticleEnabledRPC", PhotonTargets.Others, new object[] { _isEnabled });
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SetJetpackParticleEnabledRPC(bool _isEnabled)
	{
		if (_isEnabled)
		{
			if (Defs.isDaterRegim)
			{
				this.isPlayerFlying = true;
			}
			if (ButtonClickSound.Instance != null && Defs.isSoundFX && !Defs.isDaterRegim)
			{
				this.jetPackSound.SetActive(true);
			}
		}
		else if (!Defs.isDaterRegim)
		{
			this.jetPackSound.SetActive(false);
		}
		else
		{
			this.isPlayerFlying = false;
		}
		for (int i = 0; i < (int)this.jetPackParticle.Length; i++)
		{
			this.jetPackParticle[i].enableEmission = _isEnabled;
		}
	}

	public static void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
		obj.layer = newLayer;
		int num = obj.transform.childCount;
		Transform transforms = obj.transform;
		for (int i = 0; i < num; i++)
		{
			Transform child = transforms.GetChild(i);
			if (null != child)
			{
				Player_move_c.SetLayerRecursively(child.gameObject, newLayer);
			}
		}
	}

	private void SetMapCameraActive(bool active)
	{
		InGameGUI.sharedInGameGUI.SetInterfaceVisible(!active);
		Camera component = Initializer.Instance.tc.GetComponent<Camera>();
		Camera camera = this.myCamera;
		component.gameObject.SetActive(active);
		camera.gameObject.SetActive(!active);
		NickLabelController.currentCamera = (!active ? camera : component);
	}

	private void SetMaterialForArms()
	{
		if (this.myCurrentWeaponSounds != null)
		{
			if (!this.isBearActive)
			{
				this.myCurrentWeaponSounds._innerPars.SetMaterialForArms(this._bodyMaterial);
			}
		}
	}

	public void setMyTamble(GameObject _myTable)
	{
		if (this.myTable == null || _myTable == null)
		{
			return;
		}
		NetworkStartTable component = this.myTable.GetComponent<NetworkStartTable>();
		if (component == null)
		{
			return;
		}
		component.myPlayerMoveC = this;
		this.myTable = _myTable;
		this.myNetworkStartTable = this.myTable.GetComponent<NetworkStartTable>();
		if (this.myNetworkStartTable == null)
		{
			return;
		}
		this.CurHealth = this.MaxHealth;
		this.myCommand = this.myNetworkStartTable.myCommand;
		if (Initializer.redPlayers.Contains(this) && this.myCommand == 1)
		{
			Initializer.redPlayers.Remove(this);
		}
		if (Initializer.bluePlayers.Contains(this) && this.myCommand == 2)
		{
			Initializer.bluePlayers.Remove(this);
		}
		if (this.myCommand == 1 && !Initializer.bluePlayers.Contains(this))
		{
			Initializer.bluePlayers.Add(this);
		}
		if (this.myCommand == 2 && !Initializer.redPlayers.Contains(this))
		{
			Initializer.redPlayers.Add(this);
		}
		this._skin = this.myNetworkStartTable.mySkin;
		this.SetTextureForBodyPlayer(this._skin);
		if (this.isMine)
		{
			if (FriendsController.useBuffSystem)
			{
				BuffSystem.instance.CheckForPlayerBuff();
			}
			else if (!KillRateCheck.instance.buffEnabled)
			{
				this.SetupBuffParameters(1f, 1f);
			}
			else
			{
				this.SetupBuffParameters(KillRateCheck.instance.damageBuff, KillRateCheck.instance.healthBuff);
			}
		}
		if (Defs.isMulti && Defs.isInet && this.myNetworkStartTable.myRanks < 4)
		{
			BonusController.sharedController.lowLevelPlayers.Add(this.photonView.ownerId);
		}
	}

	public void SetNicklabelVisible()
	{
		bool flag;
		if (this.isMine)
		{
			return;
		}
		GameObject gameObject = this.nickLabel.gameObject;
		if (!this.isInvisible)
		{
			flag = true;
		}
		else
		{
			flag = ((ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture) && WeaponManager.sharedManager.myPlayerMoveC != null ? this.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand : false);
		}
		gameObject.SetActive(flag);
	}

	[PunRPC]
	[RPC]
	public void SetNickName(string _nickName)
	{
		this.photonView = PhotonView.Get(this);
		this.mySkinName.NickName = _nickName;
		if (!this.isMine)
		{
			this.nickLabel.gameObject.SetActive(true);
			this.nickLabel.text = _nickName;
		}
	}

	[Obfuscation(Exclude=true)]
	private void SetNoKilled()
	{
		this.isKilled = false;
		this.resetMultyKill();
	}

	public void setOutString(string nick)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		this._weaponManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_0996")));
	}

	public void SetPause(bool showGUI = true)
	{
		this.ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		if (this._pauser == null)
		{
			UnityEngine.Debug.LogWarning("SetPause(): _pauser is null.");
			return;
		}
		this._pauser.paused = !this._pauser.paused;
		if (this.myCurrentWeaponSounds != null)
		{
			this.myCurrentWeaponSounds.animationObject.SetActive(!this._pauser.paused);
		}
		if (!this._pauser.paused)
		{
			InGameGUI.sharedInGameGUI.turretPanel.SetActive(this.isActiveTurretPanelInPause);
		}
		else
		{
			this.isActiveTurretPanelInPause = InGameGUI.sharedInGameGUI.turretPanel.activeSelf;
			InGameGUI.sharedInGameGUI.turretPanel.SetActive(false);
		}
		if (showGUI && this.inGameGUI != null && this.inGameGUI.pausePanel != null)
		{
			this.inGameGUI.pausePanel.SetActive(this._pauser.paused);
			this.inGameGUI.fastShopPanel.SetActive(!this._pauser.paused);
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = this._pauser.paused;
				ExpController.Instance.InterfaceEnabled = this._pauser.paused;
			}
		}
		if (!this._pauser.paused)
		{
			Time.timeScale = 1f;
			TrainingController.isPause = false;
		}
		else if (!this.isMulti)
		{
			Time.timeScale = 0f;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				TrainingController.isPause = true;
			}
		}
		if (!this._pauser.paused)
		{
			this.AddButtonHandlers();
		}
		else
		{
			this.RemoveButtonHandelrs();
		}
	}

	[Obfuscation(Exclude=true)]
	public void SetStealthModifier()
	{
		!(this._player != null);
	}

	public void SetTextureForActiveMesh(Texture needTx)
	{
		this.SetMaterialForArms();
		if (this.mainDamageMaterial != null)
		{
			this.mainDamageMaterial.mainTexture = needTx;
		}
	}

	public void SetTextureForBodyPlayer(Texture needTx)
	{
		this.SetMaterialForArms();
		if (this._bodyMaterial != null)
		{
			this._bodyMaterial.mainTexture = needTx;
		}
	}

	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt, GameObject[] stopObjs)
	{
		Transform transforms = obj.transform;
		int num = obj.transform.childCount;
		for (int i = 0; i < num; i++)
		{
			Transform child = transforms.GetChild(i);
			bool flag = false;
			int num1 = 0;
			while (num1 < (int)stopObjs.Length)
			{
				GameObject gameObject = stopObjs[num1];
				if (!child.gameObject.Equals(gameObject))
				{
					num1++;
				}
				else
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (child.gameObject.GetComponent<Renderer>() && child.gameObject.GetComponent<Renderer>().material)
				{
					child.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
				flag = false;
				int num2 = 0;
				while (num2 < (int)stopObjs.Length)
				{
					GameObject gameObject1 = stopObjs[num2];
					if (!child.gameObject.Equals(gameObject1))
					{
						num2++;
					}
					else
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Player_move_c.SetTextureRecursivelyFrom(child.gameObject, txt, stopObjs);
				}
			}
		}
	}

	public void SetupBuffParameters(float damage, float protection)
	{
		bool flag = (this.damageBuff != damage ? true : this.protectionBuff != protection);
		this.SetBuffParameters(damage, protection);
		if (flag && Defs.isMulti && Defs.isInet)
		{
			this.photonView.RPC("SendBuffParameters", PhotonTargets.Others, new object[] { this.damageBuff, this.protectionBuff });
		}
	}

	[DebuggerHidden]
	[PunRPC]
	[RPC]
	private IEnumerator SetWeaponRPC(string _nameWeapon, string _alternativeNameWeapon)
	{
		Player_move_c.u003cSetWeaponRPCu003ec__IteratorD7 variable = null;
		return variable;
	}

	private void SetWeaponVisible(bool visible)
	{
		this.myCurrentWeaponSounds.SetDaterBearHandsAnim(!visible);
		if (this.currentGrenade != null)
		{
			this.currentGrenade.transform.parent = this.myCurrentWeaponSounds.grenatePoint;
		}
	}

	[DebuggerHidden]
	private IEnumerator ShootLoop(CancellationToken token, float _lengthAnimShootDown)
	{
		Player_move_c.u003cShootLoopu003ec__IteratorE3 variable = null;
		return variable;
	}

	public void shootS()
	{
		if (this.isGrenadePress)
		{
			return;
		}
		if (this.isMechActive)
		{
			this.BulletShot(this.mechWeaponSounds);
			return;
		}
		WeaponSounds weaponSound = this._weaponManager.currentWeaponSounds;
		if (weaponSound.bazooka)
		{
			base.StartCoroutine(this.BazookaShoot());
			return;
		}
		if (weaponSound.railgun || weaponSound.freezer)
		{
			this.RailgunShot(weaponSound);
			return;
		}
		if (weaponSound.flamethrower)
		{
			this.FlamethrowerShot(weaponSound);
			return;
		}
		if (weaponSound.isRoundMelee)
		{
			base.StartCoroutine(this.HitRoundMelee(weaponSound));
			return;
		}
		if (!weaponSound.isMelee)
		{
			this.BulletShot(weaponSound);
			return;
		}
		base.StartCoroutine(this.MeleeShot(weaponSound));
	}

	private void ShootUpdate()
	{
		bool flag = this.isShooting;
		this.isShooting = (JoystickController.rightJoystick.isShooting || JoystickController.rightJoystick.isShootingPressure ? true : JoystickController.leftTouchPad.isShooting);
		bool flag1 = (this.isShooting ? false : flag);
		bool flag2 = (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None ? true : TrainingController.FireButtonEnabled);
		if (!this.isShooting)
		{
			if (flag1)
			{
				this.ShotUnPressed(false);
			}
			this.ResetShootingBurst();
		}
		else if (flag2 && (!this.isHunger || this.hungerGameController.isGo) && !this.myCurrentWeaponSounds.isGrenadeWeapon)
		{
			this.ShotPressed();
		}
	}

	private void ShopPressed()
	{
		this.ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			if (!TrainingController.stepTrainingList.ContainsKey("InterTheShop"))
			{
				TrainingController.isNextStep = TrainingState.TapToShoot;
			}
			else
			{
				TrainingController.isNextStep = TrainingState.EnterTheShop;
				if (Player_move_c.StopBlinkShop != null)
				{
					Player_move_c.StopBlinkShop();
				}
			}
		}
		if (this.CurHealth > 0f)
		{
			this.SetInApp();
			this.SetPause(false);
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(this.clickShop);
			}
		}
	}

	public void ShotPressed()
	{
		if (this.deltaAngle > 10f)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			TrainingController.isNextStep = TrainingState.TapToShoot;
		}
		if (this.isMulti && this.isInet && this.photonView && !this.photonView.isMine || this._weaponManager == null || this._weaponManager.currentWeaponSounds == null || this._weaponManager.currentWeaponSounds.animationObject == null)
		{
			return;
		}
		if (this._weaponManager.currentWeaponSounds.name.Contains("WeaponGrenade"))
		{
			return;
		}
		if (Defs.isTurretWeapon)
		{
			return;
		}
		if (!this.isMechActive && this._weaponManager.currentWeaponSounds.isLoopShoot)
		{
			if (!this.isShootingLoop)
			{
				this.StartLoopShot();
			}
			return;
		}
		Animation animations = (!this.isMechActive ? this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>() : this.mechGunAnimation);
		if (animations.IsPlaying("Shoot1"))
		{
			return;
		}
		if (animations.IsPlaying("Shoot2"))
		{
			return;
		}
		if (animations.IsPlaying("Shoot"))
		{
			return;
		}
		if (animations.IsPlaying("Shoot1"))
		{
			return;
		}
		if (animations.IsPlaying("Shoot2"))
		{
			return;
		}
		if (animations.IsPlaying("Reload"))
		{
			return;
		}
		if (animations.IsPlaying("Empty"))
		{
			return;
		}
		if (this._timerDelayInShootingBurst > 0f)
		{
			return;
		}
		Weapon item = (Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex];
		if (this.isMechActive || !this._weaponManager.currentWeaponSounds.isCharging)
		{
			animations.Stop();
			if (this._weaponManager.currentWeaponSounds.isBurstShooting)
			{
				this._countShootInBurst++;
				if (this._countShootInBurst >= this._weaponManager.currentWeaponSounds.countShootInBurst)
				{
					this._timerDelayInShootingBurst = this._weaponManager.currentWeaponSounds.delayInBurstShooting;
					this._countShootInBurst = 0;
				}
			}
			if (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee && !this.isMechActive)
			{
				this._Shot();
				return;
			}
			if (item.currentAmmoInClip > 0 || this.isMechActive)
			{
				if (!this.isMechActive)
				{
					Weapon weapon = item;
					weapon.currentAmmoInClip = weapon.currentAmmoInClip - 1;
					if (item.currentAmmoInClip == 0)
					{
						if (item.currentAmmoInBackpack <= 0)
						{
							TouchPadController touchPadController = JoystickController.rightJoystick;
							if (touchPadController)
							{
								touchPadController.NoAmmo();
							}
							if (this.inGameGUI != null)
							{
								this.inGameGUI.BlinkNoAmmo(3);
								this.inGameGUI.PlayLowResourceBeep(3);
							}
						}
						else if (this._weaponManager.currentWeaponSounds.isShotMelee)
						{
							this.Reload();
						}
					}
				}
				this._Shot();
				if (!this._weaponManager.currentWeaponSounds.isShotMelee || this.isMechActive)
				{
					this._SetGunFlashActive(true);
					if (!this.isMechActive)
					{
						this.GunFlashLifetime = this._weaponManager.currentWeaponSounds.gameObject.GetComponent<FlashFire>().timeFireAction;
					}
					else
					{
						this.GunFlashLifetime = 0.15f;
					}
				}
			}
			else
			{
				this.ShowNoAmmo();
			}
			return;
		}
		if (item.currentAmmoInClip > 0 && this.chargeValue < 1f)
		{
			if (this.chargeValue == 0f)
			{
				this.ammoInClipBeforeCharge = item.currentAmmoInClip;
				this.lastChargeWeaponIndex = this._weaponManager.CurrentWeaponIndex;
			}
			if (this.nextChargeConsumeTime < Time.time)
			{
				this.nextChargeConsumeTime = Time.time + this._weaponManager.currentWeaponSounds.chargeTime / (float)this._weaponManager.currentWeaponSounds.chargeMax;
				this.chargeValue = Math.Min(1f, this.chargeValue + 1f / (float)this._weaponManager.currentWeaponSounds.chargeMax);
				animations["Charge"].speed = this.chargeValue;
				Weapon weapon1 = item;
				weapon1.currentAmmoInClip = weapon1.currentAmmoInClip - 1;
				if (this.inGameGUI != null)
				{
					this.inGameGUI.ChargeValue.gameObject.SetActive(true);
					this.inGameGUI.ChargeValue.fillAmount = this.chargeValue;
					this.inGameGUI.ChargeValue.color = new Color(1f, 1f - this.chargeValue, 0f);
				}
			}
		}
		else if (this.chargeValue == 0f)
		{
			this.ShowNoAmmo();
		}
		if (this.chargeValue > 0f)
		{
			if (!animations.IsPlaying("Charge") && animations.GetClip("Charge") != null)
			{
				animations.Stop();
				animations.Play("Charge");
			}
			if (Defs.isSoundFX && this._weaponManager.currentWeaponSounds.charge != null && (!base.GetComponent<AudioSource>().isPlaying || base.GetComponent<AudioSource>().clip != this._weaponManager.currentWeaponSounds.charge))
			{
				base.GetComponent<AudioSource>().clip = this._weaponManager.currentWeaponSounds.charge;
				base.GetComponent<AudioSource>().Play();
			}
		}
	}

	public void ShotUnPressed(bool weaponChanged = false)
	{
		if (this._weaponManager.currentWeaponSounds.isLoopShoot && this.isShootingLoop)
		{
			this.StopLoopShot();
		}
		if (this._weaponManager.currentWeaponSounds.isCharging)
		{
			this.UnchargeGun(weaponChanged);
		}
	}

	public void ShowBonuseParticle(Player_move_c.TypeBonuses _type)
	{
		if (!Defs.isMulti)
		{
			return;
		}
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("ShowBonuseParticleRPC", RPCMode.Others, new object[] { (int)_type });
		}
		else
		{
			this.photonView.RPC("ShowBonuseParticleRPC", PhotonTargets.Others, new object[] { (int)_type });
		}
	}

	[PunRPC]
	[RPC]
	public void ShowBonuseParticleRPC(int _type)
	{
		if ((int)this.bonusesParticles.Length >= _type)
		{
			this.bonusesParticles[_type].ShowParticle();
		}
	}

	public void ShowChat()
	{
		if (this.isKilled)
		{
			return;
		}
		this.ShotUnPressed(true);
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.jumpPressed = false;
			JoystickController.leftTouchPad.isJumpPressed = false;
			JoystickController.rightJoystick.Reset();
		}
		this.RemoveButtonHandelrs();
		this.showChat = true;
		if (this.inGameGUI.gameObject != null)
		{
			this.inGameGUI.gameObject.SetActive(false);
		}
		this._weaponManager.currentWeaponSounds.gameObject.SetActive(false);
		this.mechPoint.SetActive(false);
		UnityEngine.Object.Instantiate<GameObject>(this.chatViewer);
	}

	private void ShowDamageDirection(Vector3 posDamage)
	{
		if (this.isDaterRegim)
		{
			return;
		}
		bool flag = false;
		bool flag1 = false;
		bool flag2 = false;
		bool flag3 = false;
		Vector3 vector3 = posDamage - this.myPlayerTransform.position;
		float single = Mathf.Atan(vector3.z / vector3.x);
		single = single * 180f / 3.1415927f;
		if (vector3.x > 0f)
		{
			single = 90f - single;
		}
		if (vector3.x < 0f)
		{
			single = 270f - single;
		}
		float single1 = single - this.myPlayerTransform.rotation.eulerAngles.y;
		if (single1 > 180f)
		{
			single1 -= 360f;
		}
		if (single1 < -180f)
		{
			single1 += 360f;
		}
		if (this.inGameGUI != null)
		{
			this.inGameGUI.AddDamageTaken(single);
		}
		if (single1 > -45f && single1 <= 45f)
		{
			flag2 = true;
		}
		if (single1 < -45f && single1 >= -135f)
		{
			flag = true;
		}
		if (single1 > 45f && single1 <= 135f)
		{
			flag1 = true;
		}
		if (single1 < -135f || single1 >= 135f)
		{
			flag3 = true;
		}
		if (flag2)
		{
			this.timerShowUp = this.maxTimeSetTimerShow;
		}
		if (flag3)
		{
			this.timerShowDown = this.maxTimeSetTimerShow;
		}
		if (flag)
		{
			this.timerShowLeft = this.maxTimeSetTimerShow;
		}
		if (flag1)
		{
			this.timerShowRight = this.maxTimeSetTimerShow;
		}
	}

	[PunRPC]
	[RPC]
	public void ShowMultyKillRPC(int countMulty)
	{
		this.multiKill = countMulty;
	}

	private void ShowNoAmmo()
	{
		Weapon item = (Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex];
		if (this.inGameGUI != null)
		{
			this.inGameGUI.BlinkNoAmmo(1);
			if (item.currentAmmoInBackpack == 0)
			{
				this.inGameGUI.PlayLowResourceBeepIfNotPlaying(1);
			}
		}
		if (this._weaponManager.currentWeaponSounds.isMelee)
		{
			return;
		}
		if (!this.isMechActive && item.currentAmmoInBackpack <= 0 && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showChangeWeaponHint)
		{
			HintController.instance.ShowHintByName("change_weapon", 2f);
		}
		this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Empty");
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.empty);
		}
	}

	[DebuggerHidden]
	private IEnumerator ShowRayWithDelay(Vector3 _origin, Vector3 _direction, string _railName, float _len, float _delay)
	{
		Player_move_c.u003cShowRayWithDelayu003ec__IteratorE4 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	public void SlowdownRPC(float coef, float time)
	{
		if (this.isMine || !this.isMulti)
		{
			EffectsController.SlowdownCoeff = coef;
			this._timeOfSlowdown = time;
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		Player_move_c.u003cStartu003ec__IteratorD9 variable = null;
		return variable;
	}

	public void StartFlash(GameObject _obj)
	{
		base.StartCoroutine(this.Flash(_obj));
	}

	[PunRPC]
	[RPC]
	public void StartFlashRPC()
	{
		base.StartCoroutine(this.Flash(this.myPlayerTransform.gameObject));
	}

	public void StartLoopShot()
	{
		if (this.isMulti && this.isMine)
		{
			if (!this.isInet)
			{
				this._networkView.RPC("StartShootLoopRPC", RPCMode.Others, new object[] { true });
			}
			else
			{
				this.photonView.RPC("StartShootLoopRPC", PhotonTargets.Others, new object[] { true });
			}
		}
		this.isShootingLoop = true;
		Animation component = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		float item = 0f;
		if (component.GetClip("Shoot_start") == null)
		{
			component.Stop();
		}
		else if (!component.IsPlaying("Shoot_end"))
		{
			component.Stop();
			item = component["Shoot_start"].length;
			component.Play("Shoot_start");
		}
		else
		{
			item = component["Shoot_end"].length - component["Shoot_end"].time;
		}
		this.ctsShootLoop.Cancel();
		this.ctsShootLoop = new CancellationTokenSource();
		base.StartCoroutine(this.ShootLoop(this.ctsShootLoop.Token, item));
	}

	[PunRPC]
	[RPC]
	private void StartShootLoopRPC(bool isStart)
	{
		if (isStart && !this.isShootingLoop)
		{
			this.StartLoopShot();
		}
		if (!isStart && this.isShootingLoop)
		{
			this.StopLoopShot();
		}
	}

	[DebuggerHidden]
	private IEnumerator StartSteps()
	{
		Player_move_c.u003cStartStepsu003ec__IteratorDA variable = null;
		return variable;
	}

	private void StopLoopShot()
	{
		if (this.isMulti && this.isMine)
		{
			if (!this.isInet)
			{
				this._networkView.RPC("StartShootLoopRPC", RPCMode.Others, new object[] { false });
			}
			else
			{
				this.photonView.RPC("StartShootLoopRPC", PhotonTargets.Others, new object[] { false });
			}
		}
		this.isShootingLoop = false;
		this.ctsShootLoop.Cancel();
		Animation component = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		if (component.IsPlaying("Shoot"))
		{
			component.Stop();
		}
		else if (component.IsPlaying("Shoot_start"))
		{
			float item = component["Shoot_start"].length - component["Shoot_start"].time;
			component.Stop();
			component["Shoot_end"].time = component["Shoot_start"].length - component["Shoot_start"].time;
		}
		if (component["Shoot_end"] != null)
		{
			component.Play("Shoot_end");
		}
		if (Defs.isSoundFX)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().clip = this.myCurrentWeaponSounds.idle;
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
	}

	private void SwitchPause()
	{
		if (this.CurHealth > 0f)
		{
			this.SetPause(true);
		}
	}

	[PunRPC]
	[RPC]
	private void SyncTurretUpgrade(int turretUpgrade)
	{
		this.turretUpgrade = turretUpgrade;
	}

	[PunRPC]
	[RPC]
	private void SynhHealth(float _synhHealth, bool isUp)
	{
		this.SynhHealthRPC(_synhHealth, (_synhHealth <= 9f ? 0f : _synhHealth - 9f), isUp);
	}

	[PunRPC]
	[RPC]
	private void SynhHealthRPC(float _synhHealth, float _synchArmor, bool isUp)
	{
		if (this.isMine)
		{
			this.synhHealth = _synhHealth;
		}
		else if (isUp)
		{
			this.synhHealth = _synhHealth;
			this.armorSynch = _synchArmor;
			this.isRaiderMyPoint = false;
		}
		else
		{
			if (_synhHealth < this.synhHealth)
			{
				this.synhHealth = _synhHealth;
			}
			if (_synchArmor < this.armorSynch)
			{
				this.armorSynch = _synchArmor;
			}
		}
		if (this.synhHealth > 0f)
		{
			this.isStartAngel = false;
			this.myPersonNetwork.isStartAngel = false;
		}
	}

	[PunRPC]
	[RPC]
	private void SynhIsZoming(bool _isZoomming)
	{
		this.isZooming = _isZoomming;
	}

	[ContextMenu("Active mech")]
	public void TestActiveMech()
	{
		this.ActivateMech(0);
	}

	public static int TierOfCurrentRoom()
	{
		if (PhotonNetwork.room == null || !PhotonNetwork.room.customProperties.ContainsKey("tier"))
		{
			return ExpController.Instance.OurTier;
		}
		return (int)PhotonNetwork.room.customProperties["tier"];
	}

	private int TierOrRoomTier(int tier)
	{
		if (!this.roomTierInitialized)
		{
			this.roomTierInitialized = true;
			this.roomTier = Player_move_c.TierOfCurrentRoom();
		}
		return Math.Min(tier, this.roomTier);
	}

	private static float TimeOfMeleeAttack(WeaponSounds ws)
	{
		return ws.animationObject.GetComponent<Animation>()[(!ws.isDoubleShot ? "Shoot" : "Shoot1")].length * ws.meleeAttackTimeModifier;
	}

	[DebuggerHidden]
	private IEnumerator ToggleShield()
	{
		Player_move_c.u003cToggleShieldu003ec__IteratorDF variable = null;
		return variable;
	}

	private void TrainingShowZoomHint()
	{
		if (this.myCurrentWeaponSounds.isZooming && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showZoomHint)
		{
			HintController.instance.ShowHintByName("use_zoom", 0f);
		}
	}

	private void UnchargeGun(bool weaponChanged)
	{
		if (this.isMechActive)
		{
			return;
		}
		base.GetComponent<AudioSource>().Stop();
		this.inGameGUI.ChargeValue.gameObject.SetActive(false);
		if (this.chargeValue > 0f)
		{
			Weapon item = (Weapon)this._weaponManager.playerWeapons[this.lastChargeWeaponIndex];
			if (weaponChanged)
			{
				item.currentAmmoInClip = this.ammoInClipBeforeCharge;
			}
			else
			{
				this._Shot();
				if (!this._weaponManager.currentWeaponSounds.isShotMelee)
				{
					this._SetGunFlashActive(true);
					this.GunFlashLifetime = this._weaponManager.currentWeaponSounds.gameObject.GetComponent<FlashFire>().timeFireAction;
				}
			}
			UnityEngine.Debug.Log(string.Concat("Charge release: ", this.chargeValue));
			this.chargeValue = 0f;
		}
	}

	private void Update()
	{
		PlayerEventScoreController.ScoreEvent scoreEvent;
		this.liveTime += Time.deltaTime;
		if (this._timerDelayInShootingBurst > 0f)
		{
			this._timerDelayInShootingBurst -= Time.deltaTime;
		}
		this.UpdateHealth();
		this.UpdateNickLabelColor();
		if (this.timerUpdatePointAutoAi > 0f)
		{
			this.timerUpdatePointAutoAi -= Time.deltaTime;
		}
		if ((!this.isMulti || this.isMine) && this._timeOfSlowdown > 0f)
		{
			this._timeOfSlowdown -= Time.deltaTime;
			if (this._timeOfSlowdown <= 0f)
			{
				EffectsController.SlowdownCoeff = 1f;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			Defs.isZooming = this.isZooming;
		}
		if (!this.isKilled && this.timerImmortality > 0f)
		{
			this.timerImmortality -= Time.deltaTime;
			if (this.timerImmortality <= 0f)
			{
				this.isImmortality = false;
			}
		}
		if (!this.isInvisible)
		{
			if (!this.isImmortality)
			{
				this.UpdateImmortalityAlpColor(1f);
			}
			else
			{
				float single = 1f;
				this.timerImmortalityForAlpha += Time.deltaTime;
				float single1 = 2f * (this.timerImmortalityForAlpha - Mathf.Floor(this.timerImmortalityForAlpha / single) * single) / single;
				if (single1 > 1f)
				{
					single1 = 2f - single1;
				}
				this.UpdateImmortalityAlpColor(0.5f + single1 * 0.4f);
			}
		}
		if (this.isMulti && this.isMine)
		{
			if ((this.isCompany || Defs.isFlag) && this.myCommand == 0 && this.myTable != null)
			{
				this.myCommand = this.myNetworkStartTable.myCommand;
			}
			if (Defs.isFlag && this.myBaza == null && this.myCommand != 0)
			{
				if (this.myCommand != 1)
				{
					this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
				}
				else
				{
					this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
				}
			}
			if (Defs.isFlag && (this.myFlag == null || this.enemyFlag == null) && this.myCommand != 0)
			{
				this.myFlag = (this.myCommand != 1 ? this.flag2 : this.flag1);
				this.enemyFlag = (this.myCommand != 1 ? this.flag1 : this.flag2);
			}
			if (Defs.isFlag && this.myFlag != null && this.enemyFlag != null)
			{
				if (!this.myFlag.isCapture && !this.myFlag.isBaza && Vector3.SqrMagnitude(this.myPlayerTransform.position - this.myFlag.transform.position) < 2.25f)
				{
					this.photonView.RPC("SendSystemMessegeFromFlagReturnedRPC", PhotonTargets.All, new object[] { this.myFlag.isBlue });
					this.myFlag.GoBaza();
				}
				if (!this.enemyFlag.isCapture && !this.isKilled && this.enemyFlag.GetComponent<FlagController>().flagModel.activeSelf && Vector3.SqrMagnitude(this.myPlayerTransform.position - this.enemyFlag.transform.position) < 2.25f)
				{
					this.enemyFlag.SetCapture(this.photonView.ownerId);
					this.isCaptureFlag = true;
					this.photonView.RPC("SendSystemMessegeFromFlagCaptureRPC", PhotonTargets.All, new object[] { this.enemyFlag.isBlue, this.mySkinName.NickName });
				}
			}
			if (!this.isCaptureFlag || Vector3.SqrMagnitude(this.myPlayerTransform.position - this.myBaza.transform.position) >= 2.25f)
			{
				if (this.inGameGUI.message_returnFlag.activeSelf)
				{
					this.inGameGUI.message_returnFlag.SetActive(false);
				}
			}
			else if (this.myFlag.isBaza)
			{
				if (Defs.isSoundFX)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.flagScoreMyCommandClip);
				}
				if (this.myTable != null)
				{
					this.myNetworkStartTable.AddScore();
				}
				this.countMultyFlag++;
				if (!NetworkStartTable.LocalOrPasswordRoom())
				{
					QuestMediator.NotifyCapture(ConnectSceneNGUIController.RegimGame.FlagCapture);
				}
				PlayerScoreController playerScoreController = this.myScoreController;
				if (this.countMultyFlag != 3)
				{
					scoreEvent = (this.countMultyFlag != 2 ? PlayerEventScoreController.ScoreEvent.flagTouchDown : PlayerEventScoreController.ScoreEvent.flagTouchDouble);
				}
				else
				{
					scoreEvent = PlayerEventScoreController.ScoreEvent.flagTouchDownTriple;
				}
				playerScoreController.AddScoreOnEvent(scoreEvent, 1f);
				this.isCaptureFlag = false;
				this.photonView.RPC("SendSystemMessegeFromFlagAddScoreRPC", PhotonTargets.Others, new object[] { !this.enemyFlag.isBlue, this.mySkinName.NickName });
				this.AddSystemMessage(LocalizationStore.Get("Key_1003"));
				this.enemyFlag.GoBaza();
			}
			else if (!this.inGameGUI.message_returnFlag.activeSelf)
			{
				this.inGameGUI.message_returnFlag.SetActive(true);
			}
			if (Defs.isFlag && this.inGameGUI != null)
			{
				if (this.isCaptureFlag)
				{
					if (!this.inGameGUI.flagRedCaptureTexture.activeSelf)
					{
						this.inGameGUI.flagRedCaptureTexture.SetActive(true);
					}
				}
				else if (this.inGameGUI.flagRedCaptureTexture.activeSelf)
				{
					this.inGameGUI.flagRedCaptureTexture.SetActive(false);
				}
			}
		}
		if (!this.isMulti || this.isMine)
		{
			if (((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).currentAmmoInClip == 0 && !this._changingWeapon && ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).currentAmmoInBackpack > 0 && !this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot") && !this.isReloading)
			{
				this.ReloadPressed();
			}
			if (!this.isHunger || this.hungerGameController.isGo)
			{
				PotionsController.sharedController.Step(Time.deltaTime, this);
			}
		}
		if (this.isHunger && this.isMine)
		{
			this.timeHingerGame += Time.deltaTime;
			bool flag = (InGameGUI.sharedInGameGUI == null ? false : InGameGUI.sharedInGameGUI.pausePanel.activeSelf);
			if (Initializer.players.Count == 1 && this.hungerGameController.isGo && this.timeHingerGame > 10f && !this.isZachetWin && !flag)
			{
				this.isZachetWin = true;
				int num = Storager.getInt(Defs.RatingHunger, false) + 1;
				Storager.setInt(Defs.RatingHunger, num, false);
				num = Storager.getInt("Rating", false) + 1;
				Storager.setInt("Rating", num, false);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.TryIncrementWinCountTimestamp();
				}
				this.myNetworkStartTable.WinInHunger();
			}
		}
		if (!this.isMulti)
		{
			this.inGameTime += Time.deltaTime;
		}
		if ((this.isCompany || Defs.isFlag) && this.myCommand == 0 && this.myTable != null)
		{
			this.myCommand = this.myNetworkStartTable.myCommand;
		}
		if (this.isMulti && this.isMine && this._weaponManager.myPlayer != null)
		{
			GlobalGameController.posMyPlayer = this._weaponManager.myPlayer.transform.position;
			GlobalGameController.rotMyPlayer = this._weaponManager.myPlayer.transform.rotation;
			GlobalGameController.healthMyPlayer = this.CurHealth;
			GlobalGameController.armorMyPlayer = this.curArmor;
		}
		if (!this.isMulti || this.isMine)
		{
			if (this.timerShow[0] > 0f)
			{
				this.timerShow[0] -= Time.deltaTime;
			}
			if (this.timerShow[1] > 0f)
			{
				this.timerShow[1] -= Time.deltaTime;
			}
			if (this.timerShow[2] > 0f)
			{
				this.timerShow[2] -= Time.deltaTime;
			}
		}
		if ((!this.isMulti || this.isMine) && !new Func<bool>(() => (this._pauser == null ? false : this._pauser.paused))() && this.canReceiveSwipes)
		{
			this.isInappWinOpen;
		}
		if (this.GunFlashLifetime > 0f)
		{
			this.GunFlashLifetime -= Time.deltaTime;
			if (this.GunFlashLifetime <= 0f)
			{
				this.GunFlashLifetime = 0f;
				this._SetGunFlashActive(false);
			}
		}
		else if (this.GunFlashLifetime == -1f && JoystickController.IsButtonFireUp())
		{
			this.GunFlashLifetime = 0f;
			this._SetGunFlashActive(false);
		}
		if (Defs.isDaterRegim && this.isPlayerFlying)
		{
			if (!this.isMine)
			{
				if (!this.wingsAnimation.isPlaying)
				{
					this.wingsAnimation.Play();
				}
				if (!this.wingsBearAnimation.isPlaying)
				{
					this.wingsBearAnimation.Play();
				}
			}
			if (Defs.isSoundFX && !this.wingsSound.isPlaying)
			{
				this.wingsSound.Play();
			}
		}
		if (!this.isMulti || this.isMine)
		{
			this.ShootUpdate();
		}
	}

	public void UpdateEffectsForCurrentWeapon(string currentCape, string currentMask, string currentHat)
	{
		if (this.myCurrentWeaponSounds == null)
		{
			return;
		}
		if (!this.isMine)
		{
			this._chanceToIgnoreHeadshot = EffectsController.GetChanceToIgnoreHeadshot(this.myCurrentWeaponSounds.categoryNabor, currentCape, currentMask, currentHat);
		}
		this._currentReloadAnimationSpeed = EffectsController.GetReloadAnimationSpeed(this.myCurrentWeaponSounds.categoryNabor, currentCape, currentMask, currentHat);
		this._protectionShieldValue = 1f;
		bool flag = (this.isMechActive ? false : this.myCurrentWeaponSounds.specialEffect == WeaponSounds.SpecialEffects.PlayerShield);
		if (flag != this.isShieldActivated)
		{
			this.isShieldActivated = flag;
			base.StopCoroutine(this.ToggleShield());
			base.StartCoroutine(this.ToggleShield());
		}
	}

	private void UpdateHealth()
	{
		if (this.isMulti && this.isMine && this.CurHealth + this.curArmor - this.synhHealth > 0.1f)
		{
			this.SendSynhHealth(true, null);
		}
		if (!this.isMulti || this.isMine)
		{
			if (!this.isRegenerationLiveCape)
			{
				this.timerRegenerationLiveCape = this.maxTimerRegenerationLiveCape;
			}
			if (this.isRegenerationLiveCape)
			{
				if (this.timerRegenerationLiveCape <= 0f)
				{
					this.timerRegenerationLiveCape = this.maxTimerRegenerationLiveCape;
					if (this.CurHealth < this.MaxHealth)
					{
						Player_move_c curHealth = this;
						curHealth.CurHealth = curHealth.CurHealth + 1f;
					}
				}
				else
				{
					this.timerRegenerationLiveCape -= Time.deltaTime;
				}
			}
			if (!EffectsController.IsRegeneratingArmor)
			{
				this.timeSettedAfterRegenerationSwitchedOn = false;
			}
			if (EffectsController.IsRegeneratingArmor)
			{
				if (!this.timeSettedAfterRegenerationSwitchedOn)
				{
					this.timeSettedAfterRegenerationSwitchedOn = true;
					this.timerRegenerationArmor = this.maxTimerRegenerationArmor;
				}
				if (this.timerRegenerationArmor <= 0f)
				{
					this.timerRegenerationArmor = this.maxTimerRegenerationArmor;
					if (this.curArmor < this.MaxArmor && Storager.getString(Defs.ArmorNewEquppedSN, false) != Defs.ArmorNewNoneEqupped)
					{
						this.AddArmor(1f);
					}
				}
				else
				{
					this.timerRegenerationArmor -= Time.deltaTime;
				}
			}
			if (!this.isRegenerationLiveZel)
			{
				this.timerRegenerationLiveZel = this.maxTimerRegenerationLiveZel;
			}
			if (this.isRegenerationLiveZel)
			{
				if (this.timerRegenerationLiveZel <= 0f)
				{
					this.timerRegenerationLiveZel = this.maxTimerRegenerationLiveZel;
					if (this.CurHealth < this.MaxHealth)
					{
						Player_move_c playerMoveC = this;
						playerMoveC.CurHealth = playerMoveC.CurHealth + 1f;
					}
				}
				else
				{
					this.timerRegenerationLiveZel -= Time.deltaTime;
				}
			}
			if (this.timerShowUp > 0f)
			{
				this.timerShowUp -= Time.deltaTime;
			}
			if (this.timerShowDown > 0f)
			{
				this.timerShowDown -= Time.deltaTime;
			}
			if (this.timerShowLeft > 0f)
			{
				this.timerShowLeft -= Time.deltaTime;
			}
			if (this.timerShowRight > 0f)
			{
				this.timerShowRight -= Time.deltaTime;
			}
		}
		if ((!this.isMulti || this.isMine) && this.CurHealth <= 0f && !this.isKilled && !this.showRanks && !this.showChat && !ShopNGUIController.GuiActive && !BankController.Instance.uiRoot.gameObject.activeInHierarchy && (this._pauser == null || this._pauser != null && !this._pauser.paused))
		{
			this.countMultyFlag = 0;
			this.ResetMySpotEvent();
			this.ResetHouseKeeperEvent();
			if (this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
			{
				this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Stop();
			}
			if (Mathf.Abs(Time.time - this.timeBuyHealth) < 1.5f)
			{
				int num = Storager.getInt("Coins", false);
				Storager.setInt("Coins", num + Defs.healthInGamePanelPrice, false);
				CoinsMessage.FireCoinsAddedEvent(false, 2);
				this.timeBuyHealth = -10000f;
			}
			if (Defs.isCOOP)
			{
				this.SendImKilled();
				this.SendSynhHealth(false, null);
			}
			this.inGameGUI.ResetDamageTaken();
			if (Defs.isTurretWeapon)
			{
				this.CancelTurret();
				InGameGUI.sharedInGameGUI.HideTurretInterface();
				Defs.isTurretWeapon = false;
			}
			if (this.isGrenadePress)
			{
				this.ReturnWeaponAfterGrenade();
				this.isGrenadePress = false;
			}
			if (this.isZooming)
			{
				this.ZoomPress();
			}
			if (!this.isMulti)
			{
				if (Defs.IsSurvival)
				{
					if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
					{
						GlobalGameController.HasSurvivalRecord = true;
						PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
						PlayerPrefs.Save();
						FriendsController.sharedController.survivalScore = GlobalGameController.Score;
						FriendsController.sharedController.SendOurData(false);
					}
					if (ZombieCreator.sharedCreator != null)
					{
						if (Storager.getInt("SendFirstResaltArena", false) != 1)
						{
							Storager.setInt("SendFirstResaltArena", 1, false);
							AnalyticsStuff.LogArenaFirst(false, (ZombieCreator.sharedCreator.currentWave <= 0 ? false : true));
						}
						AnalyticsStuff.LogArenaWavesPassed(ZombieCreator.sharedCreator.currentWave);
					}
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
					{
						AGSLeaderboardsClient.SubmitScore("best_survival_scores", (long)GlobalGameController.Score, 0);
					}
					else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite && Social.localUser.authenticated)
					{
						Social.ReportScore((long)GlobalGameController.Score, "CgkIr8rGkPIJEAIQCg", (bool success) => UnityEngine.Debug.Log(string.Concat("Player_move_c.Update(): ", (!success ? "Failed to report score." : "Reported score successfully."))));
					}
				}
				else if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
				{
					PlayerPrefs.SetInt(Defs.BestScoreSett, GlobalGameController.Score);
					PlayerPrefs.Save();
				}
				PlayerPrefs.SetInt("IsGameOver", 1);
				LevelCompleteLoader.action = null;
				LevelCompleteLoader.sceneName = "LevelComplete";
				Singleton<SceneLoader>.Instance.LoadScene("LevelToCompleteProm", LoadSceneMode.Single);
			}
			else
			{
				if ((!this.isMulti || this.isMine) && this._player != null && this._player)
				{
					ImpactReceiverTrampoline component = this._player.GetComponent<ImpactReceiverTrampoline>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
				if (Defs.isFlag && this.isCaptureFlag)
				{
					this.isCaptureFlag = false;
					this.photonView.RPC("SendSystemMessegeFromFlagDroppedRPC", PhotonTargets.All, new object[] { this.enemyFlag.isBlue, this.mySkinName.NickName });
					this.enemyFlag.SetNOCapture(this.flagPoint.transform.position, this.flagPoint.transform.rotation);
				}
				this.resetMultyKill();
				this.isKilled = true;
				if (Defs.isCOOP && !this.isSuicided)
				{
					this.killedInMatch = true;
				}
				if (Defs.isMulti && this.isMine && !Defs.isHunger && !this.isSuicided && UnityEngine.Random.Range(0, 100) < 50)
				{
					BonusController bonusController = BonusController.sharedController;
					float single = this.myPlayerTransform.position.x;
					Vector3 vector3 = this.myPlayerTransform.position;
					Vector3 vector31 = this.myPlayerTransform.position;
					bonusController.AddBonusAfterKillPlayer(new Vector3(single, vector3.y - 1f, vector31.z));
				}
				this.isSuicided = false;
				if (this.isHunger && ((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).weaponPrefab.name.Replace("(Clone)", string.Empty) != WeaponManager.KnifeWN)
				{
					BonusController.sharedController.AddWeaponAfterKillPlayer(((Weapon)this._weaponManager.playerWeapons[this._weaponManager.CurrentWeaponIndex]).weaponPrefab.name, this.myPlayerTransform.position);
				}
				if (Defs.isSoundFX)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.deadPlayerSound);
				}
				if (this.isCOOP)
				{
					NetworkStartTable networkStartTable = this._weaponManager.myNetworkStartTable;
					networkStartTable.score = networkStartTable.score - 1000;
					if (this._weaponManager.myNetworkStartTable.score < 0)
					{
						this._weaponManager.myNetworkStartTable.score = 0;
					}
					GlobalGameController.Score = this._weaponManager.myNetworkStartTable.score;
					this._weaponManager.myNetworkStartTable.SynhScore();
				}
				this.isDeadFrame = true;
				AutoFade.fadeKilled(0.5f, (!this.isNeedShowRespawnWindow || Defs.inRespawnWindow ? 1.5f : 0.5f), 0.5f, Color.white);
				base.Invoke("setisDeadFrameFalse", 1f);
				base.StartCoroutine(this.FlashWhenDead());
				if (JoystickController.leftJoystick != null)
				{
					JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
					JoystickController.leftJoystick.SetJoystickActive(false);
				}
				if (JoystickController.leftTouchPad != null)
				{
					JoystickController.leftTouchPad.SetJoystickActive(false);
				}
				if (JoystickController.rightJoystick != null)
				{
					JoystickController.rightJoystick.gameObject.SetActive(false);
					JoystickController.rightJoystick.MakeInactive();
				}
				if (!Defs.inRespawnWindow)
				{
					Vector3 vector32 = this.myPlayerTransform.localPosition;
					TweenParms tweenParm = (new TweenParms()).Prop("localPosition", new Vector3(vector32.x, 100f, vector32.z)).Ease(EaseType.EaseInCubic).OnComplete(() => {
						this.myPlayerTransform.localPosition = new Vector3(0f, -1000f, 0f);
						if (!this.isNeedShowRespawnWindow || Defs.inRespawnWindow)
						{
							Defs.inRespawnWindow = false;
							this.RespawnPlayer();
						}
						else
						{
							this.SetMapCameraActive(true);
							base.StartCoroutine(this.KillCam());
						}
					});
					HOTween.To(this.myPlayerTransform, (!this.isNeedShowRespawnWindow ? 2f : 0.75f), tweenParm);
				}
				else
				{
					Defs.inRespawnWindow = false;
					this.RespawnPlayer();
				}
			}
		}
	}

	private void UpdateImmortalityAlpColor(float _alpha)
	{
		if (Mathf.Abs(_alpha - this.oldAlphaImmortality) < 0.001f)
		{
			return;
		}
		this.oldAlphaImmortality = _alpha;
		if (this.myCurrentWeaponSounds != null)
		{
			this.playerBodyRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, _alpha));
			Shader shader = Shader.Find("Mobile/Diffuse-Color");
			if (shader != null && this.myCurrentWeaponSounds.bonusPrefab != null && this.myCurrentWeaponSounds.bonusPrefab.transform.parent != null)
			{
				this.myCurrentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.shader = shader;
				this.myCurrentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.SetColor("_ColorRili", new Color(1f, 1f, 1f, _alpha));
			}
		}
	}

	private void UpdateKillerInfo(Player_move_c killerPlayerMoveC, int killType)
	{
		float single;
		this._killerInfo.isGrenade = killType == 6;
		this._killerInfo.isMech = killType == 10;
		this._killerInfo.isTurret = killType == 8;
		SkinName skinName = killerPlayerMoveC.mySkinName;
		this._killerInfo.nickname = skinName.NickName;
		if (killerPlayerMoveC.myTable != null)
		{
			NetworkStartTable component = killerPlayerMoveC.myTable.GetComponent<NetworkStartTable>();
			int num = component.myRanks;
			if (num > 0 && num < (int)this.expController.marks.Length)
			{
				this._killerInfo.rankTex = ExperienceController.sharedController.marks[num];
				this._killerInfo.rank = num;
			}
			if (component.myClanTexture != null)
			{
				this._killerInfo.clanLogoTex = component.myClanTexture;
			}
			this._killerInfo.clanName = component.myClanName;
		}
		this._killerInfo.weapon = killerPlayerMoveC.currentWeapon;
		this._killerInfo.skinTex = killerPlayerMoveC._skin;
		this._killerInfo.hat = skinName.currentHat;
		this._killerInfo.mask = skinName.currentMask;
		this._killerInfo.armor = skinName.currentArmor;
		this._killerInfo.cape = skinName.currentCape;
		this._killerInfo.capeTex = skinName.currentCapeTex;
		this._killerInfo.boots = skinName.currentBoots;
		this._killerInfo.mechUpgrade = killerPlayerMoveC.mechUpgrade;
		this._killerInfo.turretUpgrade = killerPlayerMoveC.turretUpgrade;
		this._killerInfo.killerTransform = killerPlayerMoveC.myPlayerTransform;
		KillerInfo killerInfo = this._killerInfo;
		if (!this._killerInfo.isMech)
		{
			single = (killerPlayerMoveC.synhHealth - killerPlayerMoveC.armorSynch <= 0f ? 0f : killerPlayerMoveC.synhHealth - killerPlayerMoveC.armorSynch);
		}
		else
		{
			single = killerPlayerMoveC.liveMech;
		}
		killerInfo.healthValue = Mathf.CeilToInt(single);
		this._killerInfo.armorValue = Mathf.CeilToInt(killerPlayerMoveC.armorSynch);
	}

	private void UpdateNickLabelColor()
	{
		if (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TeamFight && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			if (Defs.isDaterRegim)
			{
				if (this._nickColorInd != 0)
				{
					this.nickLabel.color = Color.white;
					this._nickColorInd = 0;
				}
			}
			else if (Defs.isCOOP)
			{
				if (this._nickColorInd != 1)
				{
					this.nickLabel.color = Color.blue;
					this._nickColorInd = 1;
				}
			}
			else if (this._nickColorInd != 2)
			{
				this.nickLabel.color = Color.red;
				this._nickColorInd = 2;
			}
		}
		else if (WeaponManager.sharedManager.myNetworkStartTable == null || WeaponManager.sharedManager.myNetworkStartTable.myCommand == 0)
		{
			if (this._nickColorInd != 0)
			{
				this.nickLabel.color = Color.white;
				this._nickColorInd = 0;
			}
		}
		else if (WeaponManager.sharedManager.myNetworkStartTable.myCommand == this.myCommand)
		{
			if (this._nickColorInd != 1)
			{
				this.nickLabel.color = Color.blue;
				this._nickColorInd = 1;
			}
		}
		else if (this._nickColorInd != 2)
		{
			this.nickLabel.color = Color.red;
			this._nickColorInd = 2;
		}
	}

	public void UpdateSkin()
	{
		if (!this.isMulti)
		{
			this._skin = SkinsController.currentSkinForPers;
			this._skin.filterMode = FilterMode.Point;
			this.SetTextureForBodyPlayer(this._skin);
		}
	}

	public void WalkAnimation()
	{
		if (!this._singleOrMultiMine() && (!Defs.isDaterRegim || !this.isBearActive))
		{
			return;
		}
		if ((this.isBearActive || this.isMechActive) && !this.mechGunAnimation.IsPlaying("Shoot"))
		{
			this.mechGunAnimation.CrossFade("Walk");
		}
		if (this._weaponManager && this._weaponManager.currentWeaponSounds && this._weaponManager.currentWeaponSounds.animationObject != null)
		{
			this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Walk");
		}
	}

	public void WinFromTimer()
	{
		if (!base.enabled)
		{
			return;
		}
		base.enabled = false;
		InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
		if (Defs.isCompany)
		{
			int num = 0;
			if (this.countKillsCommandBlue > this.countKillsCommandRed)
			{
				num = 1;
			}
			if (this.countKillsCommandRed > this.countKillsCommandBlue)
			{
				num = 2;
			}
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, num, this.countKillsCommandBlue, this.countKillsCommandRed);
			}
		}
		else if (Defs.isCOOP)
		{
			ZombiManager.sharedManager.EndMatch();
		}
		else if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, 0, 0, 0);
		}
	}

	public void ZoomPress()
	{
		if (WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			this.showZoomHint = false;
			HintController.instance.HideHintByName("use_zoom");
		}
		this.isZooming = !this.isZooming;
		if (!this.isZooming)
		{
			if (Defs.isSoundFX && this._weaponManager.currentWeaponSounds.zoomOut != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.zoomOut);
			}
			this.myCamera.fieldOfView = this.stdFov;
			this.gunCamera.fieldOfView = 75f;
			this.gunCamera.gameObject.SetActive(true);
			if (this.inGameGUI != null)
			{
				this.inGameGUI.ResetScope();
			}
		}
		else
		{
			if (Defs.isSoundFX && this._weaponManager.currentWeaponSounds.zoomIn != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.zoomIn);
			}
			this.myCamera.fieldOfView = this._weaponManager.currentWeaponSounds.fieldOfViewZomm;
			this.gunCamera.gameObject.SetActive(false);
			this.inGameGUI.SetScopeForWeapon(this._weaponManager.currentWeaponSounds.scopeNum.ToString());
			Transform vector3 = this.myTransform;
			float single = this.myTransform.localPosition.x;
			float single1 = this.myTransform.localPosition.y;
			Vector3 vector31 = this.myTransform.localPosition;
			vector3.localPosition = new Vector3(single, single1, vector31.z);
		}
		if (this.isMulti && this.isInet)
		{
			this.photonView.RPC("SynhIsZoming", PhotonTargets.All, new object[] { this.isZooming });
		}
	}

	public event Action<float> FreezerFired
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.FreezerFired += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.FreezerFired -= value;
		}
	}

	public event Player_move_c.OnMessagesUpdate messageDelegate
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.messageDelegate += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.messageDelegate -= value;
		}
	}

	public static event Action StopBlinkShop;

	public event EventHandler<EventArgs> WeaponChanged
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.WeaponChanged += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.WeaponChanged -= value;
		}
	}

	public struct MessageChat
	{
		public string text;

		public float time;

		public int ID;

		public int command;

		public bool isClanMessage;

		public Texture clanLogo;

		public string clanID;

		public string clanName;

		public NetworkViewID IDLocal;

		public string iconName;
	}

	public delegate void OnMessagesUpdate();

	public struct RayHitsInfo
	{
		public RaycastHit[] hits;

		public bool obstacleFound;

		public float lenRay;

		public Ray rayReflect;
	}

	public struct SystemMessage
	{
		public string nick1;

		public string message2;

		public string nick2;

		public string message;

		public Color textColor;

		public SystemMessage(string nick1, string message2, string nick2, string message, Color textColor)
		{
			this.nick1 = nick1;
			this.message2 = message2;
			this.nick2 = nick2;
			this.message = message;
			this.textColor = textColor;
		}
	}

	public enum TypeBonuses
	{
		Ammo,
		Health,
		Armor,
		Grenade
	}

	public enum TypeKills
	{
		none,
		himself,
		headshot,
		explosion,
		zoomingshot,
		flag,
		grenade,
		grenade_hell,
		turret,
		killTurret,
		mech,
		like
	}
}