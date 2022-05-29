using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TrainingController : MonoBehaviour
{
	public const string NewTrainingStageHolderKey = "TrainingController.NewTrainingStageHolderKey";

	public static TrainingController sharedController;

	private static bool _trainingCompletedInitialized;

	private static bool cachedTrainingComplete;

	private static bool _comletedTrainingStageInitialized;

	private static TrainingController.NewTrainingCompletedStage _completedTrainingStage;

	public GameObject swipeToRotateOverlay;

	public GameObject dragToMoveOverlay;

	public GameObject pickupGunOverlay;

	public GameObject touch3dPressGun;

	public GameObject touch3dPressFire;

	public GameObject wellDoneOverlay;

	public GameObject getCoinOverlay;

	public GameObject enterShopOverlay;

	public GameObject shopArrowOverlay;

	public GameObject swipeToChangeWeaponOverlay;

	public GameObject tapToChangeWeaponOverlay;

	public GameObject shootReloadOverlay;

	public GameObject selectGrenadeOverlay;

	public GameObject buyGrenadeArrowOverlay;

	public GameObject throwGrenadeOverlay;

	public GameObject throwGrenadeArrowOverlay;

	public GameObject killZombiesOverlay;

	public GameObject overlaysRoot;

	public GameObject joystickFingerOverlay;

	public GameObject joystickShadowOverlay;

	public GameObject touchpadOverlay;

	public GameObject touchpadFingerOverlay;

	public GameObject swipeWeaponFingerOverlay;

	public GameObject tapWeaponArrowOverlay;

	public GameObject enemyPrototype;

	public GameObject[] enemies;

	public Transform teleportTransform;

	public Transform weaponTransform;

	public Transform playerTransform;

	private readonly static Vector3 _playerDefaultPosition;

	private GameObject[] _overlays = new GameObject[0];

	internal static TrainingState stepTraining;

	internal static Dictionary<string, TrainingState> stepTrainingList;

	internal static TrainingState isNextStep;

	public static bool isPressSkip;

	public static bool isPause;

	private Rect animTextureRect;

	private static bool nextStepAfterSkipTraining;

	private GameObject coinsPrefab;

	private Texture2D[] animTextures;

	private static int stepAnim;

	private static int maxStepAnim;

	private static bool isCanceled;

	private float speedAnim;

	private static TrainingState setNextStepInd;

	private Texture2D shop;

	private Texture2D shop_n;

	private bool isAnimShop;

	private static TrainingState oldStepTraning;

	private UIButton shopButton;

	private static bool? _trainingCompleted;

	private ActionDisposable _weaponChangedSubscription = new ActionDisposable(new Action(() => {
	}));

	private readonly List<TrainingEnemy> _enemies = new List<TrainingEnemy>(3);

	private UIButton _pauseButton;

	private int _weaponChangingCount;

	public static float timeShowJump;

	public static float timeShow3dTouchJump;

	private bool isShow3dTouchJump;

	public static float timeShowFire;

	public static float timeShow3dTouchFire;

	private bool isShow3dTouchFire;

	private GameObject weapon;

	private readonly Lazy<PlayerArrowToPortalController> _directionArrow;

	public static TrainingController.NewTrainingCompletedStage CompletedTrainingStage
	{
		get
		{
			if (!TrainingController._comletedTrainingStageInitialized)
			{
				TrainingController._comletedTrainingStageInitialized = true;
				TrainingController._completedTrainingStage = (TrainingController.NewTrainingCompletedStage)Storager.getInt("TrainingController.NewTrainingStageHolderKey", false);
			}
			return TrainingController._completedTrainingStage;
		}
		set
		{
			TrainingController._comletedTrainingStageInitialized = true;
			if (TrainingController._completedTrainingStage != value)
			{
				TrainingController._completedTrainingStage = value;
				Storager.setInt("TrainingController.NewTrainingStageHolderKey", (int)TrainingController._completedTrainingStage, false);
				if (TrainingController._completedTrainingStage == TrainingController.NewTrainingCompletedStage.FirstMatchCompleted)
				{
					Action action = TrainingController.onChangeTraining;
					if (action != null)
					{
						action();
					}
				}
			}
		}
	}

	public static bool FireButtonEnabled
	{
		get
		{
			return TrainingController.stepTraining >= TrainingState.KillZombie;
		}
	}

	public static Vector3 PlayerDefaultPosition
	{
		get
		{
			return TrainingController._playerDefaultPosition;
		}
	}

	public Vector3 PlayerDesiredPosition
	{
		get
		{
			return (this.playerTransform == null ? TrainingController._playerDefaultPosition : this.playerTransform.position);
		}
	}

	public static bool TrainingCompleted
	{
		get
		{
			if (TrainingController.cachedTrainingComplete)
			{
				return true;
			}
			if (!TrainingController._trainingCompletedInitialized)
			{
				if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) == 0 && PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 1)
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("Trying to set TrainingCompleted flag...");
					}
					TrainingController.OnGetProgress();
				}
				if (TrainingController.onChangeTraining != null)
				{
					TrainingController.onChangeTraining();
				}
				TrainingController._trainingCompletedInitialized = true;
			}
			TrainingController.cachedTrainingComplete = (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) > 0 ? true : TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.FirstMatchCompleted);
			return TrainingController.cachedTrainingComplete;
		}
	}

	public static bool? TrainingCompletedFlagForLogging
	{
		get
		{
			return TrainingController._trainingCompleted;
		}
		set
		{
			TrainingController._trainingCompleted = value;
		}
	}

	static TrainingController()
	{
		TrainingController.sharedController = null;
		TrainingController._trainingCompletedInitialized = false;
		TrainingController.cachedTrainingComplete = false;
		TrainingController._comletedTrainingStageInitialized = false;
		TrainingController._completedTrainingStage = TrainingController.NewTrainingCompletedStage.None;
		TrainingController._playerDefaultPosition = new Vector3(-0.72f, 1.75f, -13.23f);
		TrainingController.stepTraining = TrainingState.SwipeToRotate | TrainingState.TapToMove | TrainingState.GetTheGun | TrainingState.WellDone | TrainingState.KillZombie | TrainingState.GoToPortal | TrainingState.GetTheCoin | TrainingState.WellDoneCoin | TrainingState.EnterTheShop | TrainingState.Shop | TrainingState.TapToSelectWeapon | TrainingState.TapToShoot | TrainingState.TapToThrowGrenade;
		TrainingController.stepTrainingList = new Dictionary<string, TrainingState>(10);
		TrainingController.isNextStep = TrainingState.None;
		TrainingController.isPause = false;
		TrainingController.nextStepAfterSkipTraining = false;
		TrainingController.setNextStepInd = TrainingState.None;
		TrainingController.timeShowJump = 0f;
		TrainingController.timeShow3dTouchJump = 0f;
		TrainingController.timeShowFire = 0f;
		TrainingController.timeShow3dTouchFire = 0f;
		TrainingController.stepTrainingList.Add("TapToMove", TrainingState.TapToMove);
		TrainingController.stepTrainingList.Add("GetTheGun", TrainingState.GetTheGun);
		TrainingController.stepTrainingList.Add("WellDone", TrainingState.WellDone);
		TrainingController.stepTrainingList.Add("Shop", TrainingState.Shop);
		TrainingController.stepTrainingList.Add("TapToSelectWeapon", TrainingState.TapToSelectWeapon);
		TrainingController.stepTrainingList.Add("TapToShoot", TrainingState.TapToShoot);
		TrainingController.stepTrainingList.Add("TapToThrowGrenade", TrainingState.TapToThrowGrenade);
		TrainingController.stepTrainingList.Add("KillZombi", TrainingState.KillZombie);
		TrainingController.stepTrainingList.Add("GoToPortal", TrainingState.GoToPortal);
	}

	public TrainingController()
	{
		this._directionArrow = new Lazy<PlayerArrowToPortalController>(() => {
			GameObject gameObject = GameObject.FindWithTag("Player");
			if (gameObject == null)
			{
				return null;
			}
			return gameObject.GetComponent<PlayerArrowToPortalController>();
		});
	}

	private void AdjustGrenadeLabelAndArrow()
	{
		TrainingArrow component;
		TrainingArrow trainingArrow;
		Vector3 vector3 = Vector3.zero;
		Vector3[] vector3Array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (vector3Array == null || (int)vector3Array.Length < 6)
		{
			float single = (float)((!GlobalGameController.LeftHanded ? -1 : 1));
			vector3 = new Vector3((float)Defs.GrenadeX * single, (float)Defs.GrenadeY, 0f);
		}
		else
		{
			vector3 = vector3Array[5];
		}
		if (this.buyGrenadeArrowOverlay != null)
		{
			component = this.buyGrenadeArrowOverlay.GetComponent<TrainingArrow>();
		}
		else
		{
			component = null;
		}
		TrainingArrow trainingArrow1 = component;
		if (trainingArrow1 != null)
		{
			trainingArrow1.SetAnchoredPosition(vector3 - new Vector3(64f, 0f, 0f));
		}
		if (this.throwGrenadeArrowOverlay != null)
		{
			trainingArrow = this.throwGrenadeArrowOverlay.GetComponent<TrainingArrow>();
		}
		else
		{
			trainingArrow = null;
		}
		TrainingArrow trainingArrow2 = trainingArrow;
		if (trainingArrow2 != null)
		{
			trainingArrow2.SetAnchoredPosition(vector3 - new Vector3(90f, -60f, 0f));
		}
		if (this.selectGrenadeOverlay != null)
		{
			this.selectGrenadeOverlay.transform.localPosition = vector3 - new Vector3(120f, 0f, 0f);
		}
		if (this.throwGrenadeOverlay != null)
		{
			this.throwGrenadeOverlay.transform.localPosition = vector3 - new Vector3(400f, -120f, 0f);
		}
	}

	private void AdjustJoystickAreaAndFinger()
	{
		TrainingFinger component;
		float single = (float)((!GlobalGameController.LeftHanded ? -1 : 1));
		Vector3 vector3 = new Vector3((float)Defs.JoyStickX * single, (float)Defs.JoyStickY, 0f);
		if (this.dragToMoveOverlay != null)
		{
			this.dragToMoveOverlay.transform.localPosition = vector3 + new Vector3(30f, 120f, 0f);
		}
		Vector3[] vector3Array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (vector3Array != null && (int)vector3Array.Length > 4)
		{
			vector3 = vector3Array[4];
		}
		if (this.joystickShadowOverlay != null)
		{
			this.joystickShadowOverlay.GetComponent<RectTransform>().anchoredPosition = vector3;
		}
		if (this.joystickFingerOverlay != null)
		{
			component = this.joystickFingerOverlay.GetComponent<TrainingFinger>();
		}
		else
		{
			component = null;
		}
		TrainingFinger trainingFinger = component;
		if (trainingFinger != null)
		{
			trainingFinger.GetComponent<RectTransform>().anchoredPosition = vector3 + new Vector3(20f, 20f, 0f);
		}
	}

	private void AdjustShootReloadLabel()
	{
		bool num = PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1;
		if (this.shootReloadOverlay != null && num)
		{
			this.shootReloadOverlay.transform.localPosition = this.shootReloadOverlay.transform.localPosition - new Vector3(120f, 0f, 0f);
		}
	}

	[Obfuscation(Exclude=true)]
	private void AnimShop()
	{
		this.isAnimShop = !this.isAnimShop;
		bool flag = TrainingController.stepTraining == TrainingState.EnterTheShop;
		string str = this.shopButton.normalSprite;
		string str1 = this.shopButton.pressedSprite;
		this.shopButton.pressedSprite = str;
		this.shopButton.normalSprite = str1;
		if (flag)
		{
			base.Invoke("AnimShop", 0.3f);
		}
	}

	public static void CancelSkipTraining()
	{
		TrainingController.isCanceled = false;
		TrainingController.isPressSkip = false;
		TrainingController.stepTraining = TrainingController.oldStepTraning;
		TrainingController component = GameObject.FindGameObjectWithTag("TrainingController").GetComponent<TrainingController>();
		if (TrainingController.nextStepAfterSkipTraining)
		{
			TrainingController.nextStepAfterSkipTraining = false;
			component.StartNextStepTraning();
		}
		if (TrainingController.stepAnim != 0)
		{
			component.NextStepAnim();
		}
		else
		{
			component.FirstStep();
		}
	}

	[Obfuscation(Exclude=true)]
	private void FirstStep()
	{
		TrainingController.isCanceled = false;
		TrainingController.stepAnim = 0;
		this.NextStepAnim();
	}

	private void HandleWeaponChanged(object sender, EventArgs e)
	{
		if (this._weaponChangingCount > 0 && TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			TrainingController.isNextStep = TrainingState.TapToSelectWeapon;
		}
		this._weaponChangingCount++;
	}

	public void Hide3dTouchFire()
	{
		if (this.touch3dPressFire.activeSelf)
		{
			this.touch3dPressFire.SetActive(false);
		}
	}

	public void Hide3dTouchJump()
	{
		if (this.touch3dPressGun.activeSelf)
		{
			this.touch3dPressGun.SetActive(false);
		}
	}

	private void LateUpdate()
	{
		this.RefreshOverlays();
	}

	[Obfuscation(Exclude=true)]
	private void NextStepAnim()
	{
		base.CancelInvoke("NextStepAnim");
		if (TrainingController.isCanceled)
		{
			return;
		}
		TrainingController.stepAnim++;
		if (TrainingController.stepTraining == TrainingState.WellDone && TrainingController.stepAnim >= TrainingController.maxStepAnim)
		{
			TrainingController.isNextStep = TrainingState.WellDone;
			return;
		}
		if (TrainingController.stepTraining == TrainingState.WellDoneCoin && TrainingController.stepAnim >= TrainingController.maxStepAnim)
		{
			TrainingController.isNextStep = TrainingState.WellDoneCoin;
			return;
		}
		base.Invoke("NextStepAnim", this.speedAnim);
	}

	private void OnApplicationPause(bool pause)
	{
		FlurryEvents.LogTrainingProgress(string.Format("Pause ({0})", pause));
	}

	private void OnApplicationQuit()
	{
		FlurryEvents.LogTrainingProgress("Exit");
	}

	private void OnDestroy()
	{
		TrainingController.sharedController = null;
		if (this._pauseButton != null)
		{
			this._pauseButton.isEnabled = true;
		}
		this._weaponChangedSubscription.Dispose();
	}

	public static void OnGetProgress()
	{
		if (ShopNGUIController.NoviceArmorAvailable)
		{
			ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
			ShopNGUIController.ProvideShopItemOnStarterPackBoguht(ShopNGUIController.CategoryNames.ArmorCategory, "Armor_Army_1", 1, false, 0, null, null, true, false, false);
		}
		Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 0, false);
		Storager.setInt(Defs.TrainingCompleted_4_4_Sett, 1, false);
		if (!Defs.isABTestBalansNoneSkip)
		{
			FriendsController.ResetABTestsBalans();
		}
		FriendsController.ResetABTestSandBox();
		FriendsController.ResetABTestSpecialOffers();
		FriendsController.ResetABTestQuestSystem();
		if (!FriendsController.useBuffSystem)
		{
			KillRateCheck.instance.OnGetProgress();
		}
		else
		{
			BuffSystem.instance.OnGetProgress();
		}
	}

	private void OnLevelWasLoaded(int unused)
	{
		if (SceneManager.GetActiveScene().name == Defs.TrainingSceneName)
		{
			GC.Collect();
			this.weapon = BonusCreator._CreateBonus(WeaponManager.MP5WN, new Vector3(0f, -10000f, 0f));
		}
	}

	private void RefreshOverlays()
	{
		if (TrainingController.isPause)
		{
			return;
		}
		GameObject gameObject = null;
		if (TrainingController.stepTraining == TrainingState.SwipeToRotate)
		{
			gameObject = this.swipeToRotateOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.TapToMove)
		{
			gameObject = this.dragToMoveOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.GetTheGun)
		{
			if (Defs.touchPressureSupported || Application.isEditor)
			{
				TrainingController.timeShowJump += Time.deltaTime;
				if (this.touch3dPressGun.activeSelf)
				{
					TrainingController.timeShow3dTouchJump += Time.deltaTime;
					if (TrainingController.timeShow3dTouchJump > 5f)
					{
						this.Hide3dTouchJump();
					}
				}
				if (!this.isShow3dTouchJump && TrainingController.timeShowJump > 3f)
				{
					this.isShow3dTouchJump = true;
					HintController.instance.HideHintByName("press_jump");
					this.touch3dPressGun.SetActive(true);
				}
			}
			gameObject = this.pickupGunOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.WellDone || TrainingController.stepTraining == TrainingState.WellDoneCoin)
		{
			gameObject = this.wellDoneOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.GetTheCoin)
		{
			gameObject = this.getCoinOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.EnterTheShop)
		{
			gameObject = this.enterShopOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			gameObject = this.shootReloadOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			gameObject = this.throwGrenadeOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.KillZombie)
		{
			if (Defs.touchPressureSupported || Application.isEditor)
			{
				TrainingController.timeShowFire += Time.deltaTime;
				if (this.touch3dPressFire.activeSelf)
				{
					TrainingController.timeShow3dTouchFire += Time.deltaTime;
					if (TrainingController.timeShow3dTouchFire > 5f)
					{
						this.Hide3dTouchFire();
					}
				}
				if (!this.isShow3dTouchFire && TrainingController.timeShowFire > 3f)
				{
					this.isShow3dTouchFire = true;
					HintController.instance.HideHintByName("press_fire");
					this.touch3dPressFire.SetActive(true);
				}
			}
			gameObject = this.killZombiesOverlay;
		}
		IEnumerator<GameObject> enumerator = (
			from o in (IEnumerable<GameObject>)this._overlays
			where null != o
			select o).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.SetActive(object.ReferenceEquals(gameObject, current));
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		bool num = PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1;
		if (this.swipeToChangeWeaponOverlay != null)
		{
			this.swipeToChangeWeaponOverlay.SetActive((TrainingController.stepTraining != TrainingState.TapToSelectWeapon ? false : num));
		}
		if (this.tapToChangeWeaponOverlay != null)
		{
			this.tapToChangeWeaponOverlay.SetActive((TrainingController.stepTraining != TrainingState.TapToSelectWeapon ? false : !num));
		}
		if (this.shopArrowOverlay != null)
		{
			this.shopArrowOverlay.SetActive(TrainingController.stepTraining == TrainingState.EnterTheShop);
		}
		if (this.throwGrenadeArrowOverlay != null)
		{
			this.throwGrenadeArrowOverlay.SetActive(TrainingController.stepTraining == TrainingController.stepTrainingList["TapToThrowGrenade"]);
		}
		if (this.joystickFingerOverlay != null)
		{
			this.joystickFingerOverlay.SetActive(TrainingController.stepTraining == TrainingController.stepTrainingList["TapToMove"]);
		}
		if (this.joystickShadowOverlay != null)
		{
			this.joystickShadowOverlay.SetActive(TrainingController.stepTraining == TrainingController.stepTrainingList["TapToMove"]);
		}
		if (this.touchpadOverlay != null)
		{
			this.touchpadOverlay.SetActive(TrainingController.stepTraining == TrainingState.SwipeToRotate);
		}
		if (this.touchpadFingerOverlay != null)
		{
			this.touchpadFingerOverlay.SetActive(TrainingController.stepTraining == TrainingState.SwipeToRotate);
		}
		if (this.swipeWeaponFingerOverlay != null)
		{
			this.swipeWeaponFingerOverlay.SetActive((TrainingController.stepTraining != TrainingState.TapToSelectWeapon ? false : num));
		}
		if (this.tapWeaponArrowOverlay != null)
		{
			this.tapWeaponArrowOverlay.SetActive((TrainingController.stepTraining != TrainingState.TapToSelectWeapon ? false : !num));
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.CampaignContainer.SetActive(TrainingController.stepTraining == TrainingState.KillZombie);
			InGameGUI.sharedInGameGUI.leftAnchor.SetActive(false);
			InGameGUI.sharedInGameGUI.rightAnchor.SetActive(false);
		}
	}

	public static void SkipTraining()
	{
		TrainingController.oldStepTraning = TrainingController.stepTraining;
		TrainingController.stepTraining = TrainingState.None;
		TrainingController.isPressSkip = true;
		TrainingController.isCanceled = true;
		TrainingController._trainingCompleted = new bool?(false);
		FlurryPluginWrapper.LogEventToAppsFlyer("Training complete", new Dictionary<string, string>());
		FlurryEvents.LogTrainingProgress("Skip");
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		TrainingController.u003cStartu003ec__Iterator1D8 variable = null;
		return variable;
	}

	[Obfuscation(Exclude=true)]
	public void StartNextStepTraning()
	{
		TrainingState trainingState;
		if (TrainingController.isPressSkip)
		{
			TrainingController.nextStepAfterSkipTraining = true;
			return;
		}
		TrainingController.stepTraining += TrainingState.SwipeToRotate;
		Vector2 vector2 = Vector2.zero;
		if (TrainingController.stepTraining == TrainingState.SwipeToRotate)
		{
			this.AdjustJoystickAreaAndFinger();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 13;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			if (this.enemies != null && (int)this.enemies.Length > 0)
			{
				GameObject[] gameObjectArray = this.enemies;
				for (int i = 0; i < (int)gameObjectArray.Length; i++)
				{
					GameObject gameObject = gameObjectArray[i];
					TrainingEnemy component = gameObject.GetComponent<TrainingEnemy>() ?? gameObject.AddComponent<TrainingEnemy>();
					this._enemies.Add(component);
				}
			}
			else if (this.enemyPrototype != null)
			{
				Behaviour[] behaviourArray = new Behaviour[] { this.enemyPrototype.GetComponent<BotAiController>(), this.enemyPrototype.GetComponent<MeleeBot>(), this.enemyPrototype.GetComponent<NavMeshAgent>() };
				for (int j = 0; j < (int)behaviourArray.Length; j++)
				{
					Behaviour behaviour = behaviourArray[j];
					if (behaviour != null)
					{
						behaviour.enabled = false;
						UnityEngine.Object.Destroy(behaviour);
					}
				}
				GameObject vector3 = new GameObject("DynamicEnemies");
				vector3.transform.localPosition = new Vector3(-2f, 0f, 15f);
				int enemiesToKill = GlobalGameController.EnemiesToKill;
				for (int k = 0; k < enemiesToKill; k++)
				{
					GameObject vector31 = UnityEngine.Object.Instantiate<GameObject>(this.enemyPrototype);
					vector31.transform.parent = vector3.transform;
					Vector2 vector21 = UnityEngine.Random.insideUnitCircle;
					vector31.transform.localPosition = new Vector3((float)(3 * k) + vector21.x, 0f, vector21.y);
					vector31.transform.localRotation = Quaternion.AngleAxis(180f + UnityEngine.Random.Range(-60f, 60f), Vector3.up);
					TrainingEnemy trainingEnemy = vector31.GetComponent<TrainingEnemy>() ?? vector31.AddComponent<TrainingEnemy>();
					this._enemies.Add(trainingEnemy);
				}
			}
		}
		if (TrainingController.stepTraining == TrainingState.TapToMove)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Controls_Overview, true);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 19;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			for (int l = 0; l != (int)this.animTextures.Length; l++)
			{
				this.animTextures[l] = null;
			}
			if (this.animTextures[0] != null)
			{
				vector2 = new Vector2(-10f * Defs.Coef, (float)Screen.height - ((float)this.animTextures[0].height - 51f) * Defs.Coef);
			}
		}
		if (TrainingController.stepTraining == TrainingState.GetTheGun)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Controls_Move, true);
			HintController.instance.ShowHintByName("press_jump", 0f);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 0.2f;
			TrainingController.stepAnim = 0;
			Vector3 vector32 = (this.weaponTransform == null ? new Vector3(-1.6f, 1.75f, -2.6f) : this.weaponTransform.position);
			if (this.weaponTransform != null)
			{
				UnityEngine.Object.Destroy(this.weaponTransform.gameObject);
			}
			if (this.weapon != null)
			{
				this.weapon.transform.position = vector32;
			}
			else
			{
				this.weapon = BonusCreator._CreateBonus(WeaponManager.MP5WN, vector32);
			}
			if (this._directionArrow.Value != null)
			{
				this._directionArrow.Value.RemovePointOfInterest();
				this._directionArrow.Value.SetPointOfInterest(this.weapon.transform);
			}
		}
		if (TrainingController.stepTraining == TrainingState.WellDone || TrainingController.stepTraining == TrainingState.WellDoneCoin)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Controls_Jump, true);
			HintController.instance.HideHintByName("press_jump");
			HintController.instance.ShowHintByName("press_fire", 0f);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 1;
			this.speedAnim = 1f;
			TrainingController.stepAnim = 0;
			if (this._directionArrow.Value != null)
			{
				this._directionArrow.Value.RemovePointOfInterest();
			}
		}
		if (TrainingController.stepTraining == TrainingState.GetTheCoin)
		{
			if (this.coinsPrefab != null)
			{
				this.coinsPrefab.SetActive(true);
				this.coinsPrefab.GetComponent<CoinBonus>().SetPlayer();
			}
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 3f;
			TrainingController.stepAnim = 0;
		}
		if (TrainingController.stepTraining == TrainingState.EnterTheShop)
		{
			this.isAnimShop = false;
			this.AnimShop();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 13;
			this.speedAnim = 0.3f;
			TrainingController.stepAnim = 0;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		if (TrainingController.stepTraining != TrainingState.TapToSelectWeapon)
		{
			this._weaponChangedSubscription.Dispose();
		}
		else
		{
			InGameGUI.sharedInGameGUI.SetSwipeWeaponPanelVisibility(PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1);
			Player_move_c playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
			if (playerMoveC != null)
			{
				playerMoveC.WeaponChanged += new EventHandler<EventArgs>(this.HandleWeaponChanged);
				this._weaponChangedSubscription = new ActionDisposable(() => {
					playerMoveC.WeaponChanged -= new EventHandler<EventArgs>(this.HandleWeaponChanged);
					this._weaponChangingCount = 0;
				});
			}
		}
		if (TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			this.AdjustShootReloadLabel();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 3f;
			TrainingController.stepAnim = 0;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		if ((!TrainingController.stepTrainingList.TryGetValue("SwipeWeapon", out trainingState) ? false : trainingState == TrainingController.stepTraining))
		{
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 13;
			this.speedAnim = 0.3f;
			TrainingController.stepAnim = 0;
			for (int m = 0; m != (int)this.animTextures.Length; m++)
			{
				this.animTextures[m] = null;
			}
			if (this.animTextures[0] != null)
			{
				vector2 = new Vector2((float)Screen.width - (float)this.animTextures[0].width * Defs.Coef, 0f);
			}
		}
		if (TrainingController.stepTraining == TrainingState.KillZombie)
		{
			if (this._enemies.Count <= 0)
			{
				GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<ZombieCreator>().BeganCreateEnemies();
			}
			else
			{
				foreach (TrainingEnemy _enemy in this._enemies)
				{
					_enemy.WakeUp(UnityEngine.Random.@value);
				}
			}
			InGameGUI.sharedInGameGUI.centerAnhor.SetActive(true);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 3f;
			TrainingController.stepAnim = 0;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		if (TrainingController.stepTraining == TrainingState.GoToPortal)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Kill_Enemy, true);
			if (this._directionArrow.Value != null)
			{
				this._directionArrow.Value.RemovePointOfInterest();
				this._directionArrow.Value.SetPointOfInterest(this.teleportTransform);
			}
			PlayerPrefs.SetInt("PendingGooglePlayGamesSync", 1);
		}
		if (TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 19;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			this.animTextures[0] = Resources.Load<Texture2D>("Training/ob_change_0");
			this.animTextures[1] = Resources.Load<Texture2D>("Training/ob_change_1");
			if (this.animTextures[0] != null)
			{
				vector2 = new Vector2((float)Screen.width * 0.5f - 164f * Defs.Coef - (float)this.animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height - (112f + (float)this.animTextures[0].height) * Defs.Coef);
			}
		}
		if (TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount = 10;
			}
			this.AdjustGrenadeLabelAndArrow();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 19;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			for (int n = 0; n != (int)this.animTextures.Length; n++)
			{
				this.animTextures[n] = null;
			}
			Defs.InitCoordsIphone();
			if (this.animTextures[0] != null)
			{
				vector2 = new Vector2((float)Screen.width - ((float)(-Defs.GrenadeX + this.animTextures[0].width) + 80f) * Defs.Coef, (float)Screen.height - ((float)(Defs.GrenadeY + this.animTextures[0].height) - 80f) * Defs.Coef);
			}
		}
		if (this.animTextures[0] != null)
		{
			this.animTextureRect = new Rect(vector2.x, vector2.y, (float)this.animTextures[0].width * Defs.Coef, (float)this.animTextures[0].height * Defs.Coef);
		}
		base.Invoke("FirstStep", 1f);
	}

	private void Update()
	{
		if (this.coinsPrefab == null && TrainingController.stepTraining < TrainingState.GetTheCoin)
		{
			this.coinsPrefab = GameObject.FindGameObjectWithTag("CoinBonus");
			if (this.coinsPrefab != null)
			{
				this.coinsPrefab.SetActive(false);
			}
		}
		if (TrainingController.isNextStep > TrainingController.setNextStepInd)
		{
			TrainingController.setNextStepInd = TrainingController.isNextStep;
			if (TrainingController.stepTraining == TrainingState.SwipeToRotate || TrainingController.stepTraining == TrainingState.TapToMove)
			{
				base.Invoke("StartNextStepTraning", 1.5f);
			}
			else if (TrainingController.stepTraining != TrainingState.TapToShoot)
			{
				this.StartNextStepTraning();
			}
			else
			{
				base.Invoke("StartNextStepTraning", 3f);
			}
		}
		if (ShopNGUIController.GuiActive || TrainingController.isPause)
		{
			if (this.shopArrowOverlay != null)
			{
				this.shopArrowOverlay.SetActive(false);
			}
			if (this.buyGrenadeArrowOverlay != null)
			{
				this.buyGrenadeArrowOverlay.SetActive(false);
			}
			if (this.throwGrenadeArrowOverlay != null)
			{
				this.throwGrenadeArrowOverlay.SetActive(false);
			}
			if (this.joystickFingerOverlay != null)
			{
				this.joystickFingerOverlay.SetActive(false);
			}
			if (this.joystickShadowOverlay != null)
			{
				this.joystickShadowOverlay.SetActive(false);
			}
			if (this.touchpadOverlay != null)
			{
				this.touchpadOverlay.SetActive(false);
			}
			if (this.touchpadFingerOverlay != null)
			{
				this.touchpadFingerOverlay.SetActive(false);
			}
			if (this.swipeWeaponFingerOverlay != null)
			{
				this.swipeWeaponFingerOverlay.SetActive(false);
			}
			if (this.tapWeaponArrowOverlay != null)
			{
				this.tapWeaponArrowOverlay.SetActive(false);
			}
		}
	}

	public static event Action onChangeTraining;

	public enum NewTrainingCompletedStage
	{
		None,
		ShootingRangeCompleted,
		ShopCompleted,
		FirstMatchCompleted
	}
}