using Holoville.HOTween;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class InGameGUI : MonoBehaviour
{
	private const string weaponCat = "WeaponCat_";

	public UILabel Wave1_And_Counter;

	public UILabel reloadLabel;

	public GameObject reloadBar;

	public UIPlayTween impactTween;

	public UITexture reloadCircularSprite;

	public UITexture fireCircularSprite;

	public UITexture fireAdditionalCrcualrSprite;

	private UITexture[] circularSprites;

	public GameObject centerAnhor;

	public UILabel newWave;

	public UILabel waveDone;

	public UILabel SurvivalWaveNumber;

	public GameObject deathmatchContainer;

	public GameObject daterContainer;

	public GameObject teamBattleContainer;

	public GameObject timeBattleContainer;

	public GameObject deadlygamesContainer;

	public GameObject flagCaptureContainer;

	public GameObject survivalContainer;

	public GameObject CampaignContainer;

	public GameObject CapturePointContainer;

	public GameObject[] hidesPanelInTurrel;

	public GameObject turretPanel;

	public ButtonHandler runTurrelButton;

	public ButtonHandler cancelTurrelButton;

	[Range(1f, 1000f)]
	public float minLength = 300f;

	[Range(1f, 1000f)]
	public float maxLength = 550f;

	[Range(1f, 1000f)]
	public float defaultPanelLength = 486f;

	public Transform sideObjGearShop;

	public static InGameGUI sharedInGameGUI;

	public GameObject pausePanel;

	public Transform shopPanelForTap;

	public Transform shopPanelForSwipe;

	public Transform shopPanelForTapDater;

	public Transform shopPanelForSwipeDater;

	public Transform swipeWeaponPanel;

	public static Vector3 swipeWeaponPanelPos;

	public static Vector3 shopPanelForTapPos;

	public static Vector3 shopPanelForSwipePos;

	public GameObject blockedCollider;

	public GameObject blockedCollider2;

	public GameObject blockedColliderDater;

	public GameObject zoomButton;

	public GameObject reloadButton;

	public GameObject jumpButton;

	public GameObject fireButton;

	public GameObject fireButtonInJoystick;

	public GameObject joystick;

	public GameObject grenadeButton;

	public UISprite fireButtonSprite;

	public UISprite fireButtonSprite2;

	public GameObject aimPanel;

	public GameObject flagBlueCaptureTexture;

	public GameObject flagRedCaptureTexture;

	public GameObject message_draw;

	public GameObject message_now;

	public GameObject message_wait;

	public GameObject message_returnFlag;

	public float timerShowNow;

	public GameObject interfacePanel;

	public UILabel timerStartHungerLabel;

	public GameObject shopButton;

	public GameObject shopButtonInPause;

	public GameObject enemiesLeftLabel;

	public GameObject duel;

	public GameObject downBloodTexture;

	public GameObject upBloodTexture;

	public GameObject leftBloodTexture;

	public GameObject rightBloodTexture;

	public GameObject aimUp;

	public GameObject aimDown;

	public GameObject aimRight;

	public GameObject aimLeft;

	public GameObject aimCenter;

	public GameObject aimUpLeft;

	public GameObject aimDownLeft;

	public GameObject aimDownRight;

	public GameObject aimUpRight;

	[HideInInspector]
	public UISprite aimUpSprite;

	[HideInInspector]
	public UISprite aimDownSprite;

	[HideInInspector]
	public UISprite aimRightSprite;

	[HideInInspector]
	public UISprite aimLeftSprite;

	[HideInInspector]
	public UISprite aimCenterSprite;

	[HideInInspector]
	public UISprite aimUpLeftSprite;

	[HideInInspector]
	public UISprite aimDownLeftSprite;

	[HideInInspector]
	public UISprite aimDownRightSprite;

	[HideInInspector]
	public UISprite aimUpRightSprite;

	public UISprite aimRect;

	public GameObject topAnchor;

	public GameObject leftAnchor;

	public GameObject rightAnchor;

	public GameObject bottomAnchor;

	public InGameGUI.GetFloatVAlue health;

	public InGameGUI.GetFloatVAlue armor;

	public InGameGUI.GetIntVAlue armorType;

	public InGameGUI.GetString killsToMaxKills;

	public InGameGUI.GetString timeLeft;

	public UIButton gearToogle;

	public UIButton[] weaponCategoriesButtons;

	public UILabel[] ammoCategoriesLabels;

	public UIButton[] weaponCategoriesButtonsDater;

	public UILabel[] ammoCategoriesLabelsDater;

	public GameObject fonBig;

	public GameObject fonSmall;

	public GameObject pointCaptureBar;

	public UISprite teamColorSprite;

	public UISprite captureBarSprite;

	public UILabel pointCaptureName;

	public HeartEffect[] hearts;

	public HeartEffect[] armorShields;

	public HeartEffect[] mechShields;

	public DamageTakenController[] damageTakenControllers;

	private int curDamageTakenController;

	private float timerShowPotion = -1f;

	private float timerShowPotionMax = 10f;

	public SetChatLabelController[] killLabels;

	public GameObject[] chatLabels;

	public UILabel[] messageAddScore;

	public GameObject elixir;

	public GameObject scoreLabel;

	public GameObject enemiesLabel;

	public GameObject timeLabel;

	public GameObject killsLabel;

	public GameObject scopeText;

	public GameObject joystickContainer;

	public GameObject nightVisionEffect;

	public UILabel rulesLabel;

	public Player_move_c playerMoveC;

	private ZombieCreator zombieCreator;

	public UIPanel multyKillPanel;

	public UISprite multyKillSprite;

	private bool isMulti;

	private bool isChatOn;

	private bool isInet;

	private bool isHunger;

	private HungerGameController hungerGameController;

	public GameObject[] upButtonsInShopPanel;

	public GameObject[] upButtonsInShopPanelSwipeRegim;

	public GameObject healthAddButton;

	public GameObject healthAddButtonDater;

	public GameObject ammoAddButton;

	public GameObject ammoAddButtonDater;

	public UITexture[] weaponIcons;

	public UITexture[] weaponIconsDater;

	public GameObject fastShopPanel;

	public UIScrollView changeWeaponScroll;

	public UIWrapContent changeWeaponWrap;

	public GameObject weaponPreviewPrefab;

	public int weaponIndexInScroll;

	public int weaponIndexInScrollOld;

	public int widthWeaponScrollPreview;

	public AudioClip lowResourceBeep;

	public UIPanel joystikPanel;

	public UIPanel shopPanel;

	public UIPanel bloodPanel;

	public UILabel perfectLabels;

	[SerializeField]
	private PrefabHandler _respawnWindowPrefab;

	private LazyObject<RespawnWindow> _lazyRespWindow;

	public UIPanel offGameGuiPanel;

	public UIButton pauseButton;

	public GameObject mineRed;

	public GameObject mineBlue;

	public GameObject winningBlue;

	public GameObject winningRed;

	public GameObject firstPlaceGO;

	public GameObject firstPlaceCoop;

	public UILabel placeDeathmatchLabel;

	public UILabel placeCoopLabel;

	public GameObject bankView;

	public GameObject bankViewLow;

	private IEnumerator _lowResourceBeepRoutine;

	private float timerBlinkNoAmmo;

	private float periodBlink = 2f;

	public UILabel blinkNoAmmoLabel;

	private float timerBlinkNoHeath;

	public UILabel blinkNoHeathLabel;

	public UISprite[] blinkNoHeathFrames;

	private int oldCountHeath;

	public float timerShowScorePict;

	public float maxTimerShowScorePict = 3f;

	public string scorePictName = string.Empty;

	public UISprite ChargeValue;

	[SerializeField]
	private GameObject _subpanelsContainer;

	private bool _kBlockPauseShopButton;

	private bool _disabled;

	private bool crosshairVisible;

	private bool aimRectVisible;

	private Vector2[] aimPositions = new Vector2[6];

	private CrosshairData.aimSprite defaultAimCenter = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(0f, 0f));

	private CrosshairData.aimSprite defaultAimDown = new CrosshairData.aimSprite("pricel_v", new Vector2(12f, 12f), new Vector2(0f, 8f));

	private CrosshairData.aimSprite defaultAimUp = new CrosshairData.aimSprite("pricel_v", new Vector2(12f, 12f), new Vector2(0f, 8f));

	private CrosshairData.aimSprite defaultAimLeftCenter = new CrosshairData.aimSprite("pricel_h", new Vector2(12f, 12f), new Vector2(8f, 0f));

	private CrosshairData.aimSprite defaultAimLeftDown = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(8f, 8f));

	private CrosshairData.aimSprite defaultAimLeftUp = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(8f, 8f));

	private float pastHealth;

	private float pastMechHealth;

	private float pastArmor;

	private bool mechWasActive;

	private int currentHealthStep;

	private int currentMechHealthStep;

	private int currentArmorStep;

	private bool healthInAnim;

	private bool armorInAnim;

	private bool mechInAnim;

	public RespawnWindow respawnWindow
	{
		get
		{
			if (this._lazyRespWindow == null)
			{
				this._lazyRespWindow = new LazyObject<RespawnWindow>(this._respawnWindowPrefab.ResourcePath, this.SubpanelsContainer);
			}
			return this._lazyRespWindow.Value;
		}
	}

	public GameObject SubpanelsContainer
	{
		get
		{
			return this._subpanelsContainer;
		}
	}

	public InGameGUI()
	{
	}

	[DebuggerHidden]
	public IEnumerator _DisableSwiping(float tm)
	{
		InGameGUI.u003c_DisableSwipingu003ec__Iterator8B variable = null;
		return variable;
	}

	public void AddDamageTaken(float alpha)
	{
		this.curDamageTakenController++;
		if (this.curDamageTakenController >= (int)this.damageTakenControllers.Length)
		{
			this.curDamageTakenController = 0;
		}
		this.damageTakenControllers[this.curDamageTakenController].reset(alpha);
	}

	private void AdjustToPlayerHands()
	{
		float single = (float)((!GlobalGameController.LeftHanded ? -1 : 1));
		Vector3[] vector3Array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (vector3Array == null || (int)vector3Array.Length < 7)
		{
			Defs.InitCoordsIphone();
			Transform vector3 = this.zoomButton.transform;
			float zoomButtonY = (float)Defs.ZoomButtonY;
			Vector3 vector31 = this.zoomButton.transform.localPosition;
			vector3.localPosition = new Vector3((float)Defs.ZoomButtonX * single, zoomButtonY, vector31.z);
			Transform transforms = this.reloadButton.transform;
			float reloadButtonY = (float)Defs.ReloadButtonY;
			Vector3 vector32 = this.reloadButton.transform.localPosition;
			transforms.localPosition = new Vector3((float)Defs.ReloadButtonX * single, reloadButtonY, vector32.z);
			Transform transforms1 = this.jumpButton.transform;
			float jumpButtonY = (float)Defs.JumpButtonY;
			Vector3 vector33 = this.jumpButton.transform.localPosition;
			transforms1.localPosition = new Vector3((float)Defs.JumpButtonX * single, jumpButtonY, vector33.z);
			Transform transforms2 = this.fireButton.transform;
			float fireButtonY = (float)Defs.FireButtonY;
			Vector3 vector34 = this.fireButton.transform.localPosition;
			transforms2.localPosition = new Vector3((float)Defs.FireButtonX * single, fireButtonY, vector34.z);
			Transform transforms3 = this.joystick.transform;
			float joyStickY = (float)Defs.JoyStickY;
			Vector3 vector35 = this.joystick.transform.localPosition;
			transforms3.localPosition = new Vector3((float)Defs.JoyStickX * single, joyStickY, vector35.z);
			Transform transforms4 = this.grenadeButton.transform;
			float grenadeY = (float)Defs.GrenadeY;
			Vector3 vector36 = this.grenadeButton.transform.localPosition;
			transforms4.localPosition = new Vector3((float)Defs.GrenadeX * single, grenadeY, vector36.z);
			Transform transforms5 = this.fireButtonInJoystick.transform;
			float fireButton2Y = (float)Defs.FireButton2Y;
			Vector3 vector37 = this.fireButtonInJoystick.transform.localPosition;
			transforms5.localPosition = new Vector3((float)Defs.FireButton2X * single, fireButton2Y, vector37.z);
		}
		else
		{
			for (int i = 0; i < (int)vector3Array.Length; i++)
			{
				vector3Array[i].x *= single;
			}
			this.zoomButton.transform.localPosition = vector3Array[0];
			this.reloadButton.transform.localPosition = vector3Array[1];
			this.jumpButton.transform.localPosition = vector3Array[2];
			this.fireButton.transform.localPosition = vector3Array[3];
			this.joystick.transform.localPosition = vector3Array[4];
			this.grenadeButton.transform.localPosition = vector3Array[5];
			this.fireButtonInJoystick.transform.localPosition = vector3Array[6];
		}
		UISprite[] array = (
			from go in (IEnumerable<GameObject>)(new GameObject[] { this.zoomButton, this.reloadButton, this.jumpButton, this.fireButton, this.joystick, this.grenadeButton, this.fireButtonInJoystick })
			select go.GetComponent<UISprite>()).ToArray<UISprite>();
		object obj = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]"));
		List<object> objs = obj as List<object>;
		if (objs == null)
		{
			objs = new List<object>((int)array.Length);
			UnityEngine.Debug.LogWarning(objs.GetType().FullName);
		}
		int num = Math.Min(objs.Count, (int)array.Length);
		for (int j = 0; j != num; j++)
		{
			int num1 = Convert.ToInt32(objs[j]);
			if (num1 > 0)
			{
				UISprite uISprite = array[j];
				if (uISprite != null)
				{
					array[j].keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
					array[j].width = num1;
					if (uISprite.gameObject == this.joystick)
					{
						UIJoystick component = uISprite.GetComponent<UIJoystick>();
						if (component != null)
						{
							component.ActualRadius = component.radius / 144f * (float)num1;
						}
					}
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator AnimateArmor()
	{
		InGameGUI.u003cAnimateArmoru003ec__Iterator8D variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator AnimateHealth()
	{
		InGameGUI.u003cAnimateHealthu003ec__Iterator8C variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator AnimateMechHealth()
	{
		InGameGUI.u003cAnimateMechHealthu003ec__Iterator8E variable = null;
		return variable;
	}

	private void Awake()
	{
		string str = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[] { base.GetType().Name });
		ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
		try
		{
			InGameGUI.sharedInGameGUI = this;
			this.circularSprites = new UITexture[] { this.reloadCircularSprite, this.fireCircularSprite, this.fireAdditionalCrcualrSprite };
			this.changeWeaponScroll.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, (float)this.widthWeaponScrollPreview * 1.3f, (float)this.widthWeaponScrollPreview * 1.3f);
			this.changeWeaponWrap.itemSize = this.widthWeaponScrollPreview;
			this.HandleChatSettUpdated();
			PauseNGUIController.ChatSettUpdated += new Action(this.HandleChatSettUpdated);
			ControlsSettingsBase.ControlsChanged += new Action(this.AdjustToPlayerHands);
			if (Defs.isDaterRegim)
			{
				this.shopPanelForTap = this.shopPanelForTapDater;
				this.shopPanelForSwipe = this.shopPanelForSwipeDater;
				this.ammoAddButton = this.ammoAddButtonDater;
				this.healthAddButton = this.healthAddButtonDater;
				for (int i = 0; i < (int)this.weaponCategoriesButtons.Length; i++)
				{
					this.weaponCategoriesButtons[i] = this.weaponCategoriesButtonsDater[i];
				}
				for (int j = 0; j < (int)this.ammoCategoriesLabels.Length; j++)
				{
					this.ammoCategoriesLabels[j] = this.ammoCategoriesLabelsDater[j];
				}
				for (int k = 0; k < (int)this.weaponIcons.Length; k++)
				{
					this.weaponIcons[k] = this.weaponIconsDater[k];
				}
			}
			this.shopPanelForTap.gameObject.SetActive(true);
			this.shopPanelForSwipe.gameObject.SetActive(true);
			InGameGUI.swipeWeaponPanelPos = this.swipeWeaponPanel.localPosition;
			InGameGUI.shopPanelForTapPos = this.shopPanelForTap.localPosition;
			InGameGUI.shopPanelForSwipePos = this.shopPanelForSwipe.localPosition;
			this.SetSwitchingWeaponPanel();
			this.isMulti = Defs.isMulti;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				this.centerAnhor.SetActive(false);
			}
			this.isInet = Defs.isInet;
			this.isHunger = Defs.isHunger;
			if (this.isHunger)
			{
				HungerGameController instance = HungerGameController.Instance;
				if (instance != null)
				{
					this.hungerGameController = instance.GetComponent<HungerGameController>();
				}
				else
				{
					UnityEngine.Debug.LogError("hungerGameControllerObject == null");
				}
			}
			this.aimUpSprite = this.aimUp.GetComponent<UISprite>();
			this.aimDownSprite = this.aimDown.GetComponent<UISprite>();
			this.aimRightSprite = this.aimRight.GetComponent<UISprite>();
			this.aimLeftSprite = this.aimLeft.GetComponent<UISprite>();
			this.aimCenterSprite = this.aimCenter.GetComponent<UISprite>();
			this.aimUpLeftSprite = this.aimUpLeft.GetComponent<UISprite>();
			this.aimDownLeftSprite = this.aimDownLeft.GetComponent<UISprite>();
			this.aimDownRightSprite = this.aimDownRight.GetComponent<UISprite>();
			this.aimUpRightSprite = this.aimUpRight.GetComponent<UISprite>();
			this.impactTween.gameObject.SetActive(false);
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}

	public void BlinkNoAmmo(int count)
	{
		if (count == 0)
		{
			this.StopPlayingLowResourceBeep();
		}
		this.timerBlinkNoAmmo = (float)count * this.periodBlink;
		UILabel color = this.blinkNoAmmoLabel;
		float single = this.blinkNoAmmoLabel.color.r;
		float single1 = this.blinkNoAmmoLabel.color.g;
		Color color1 = this.blinkNoAmmoLabel.color;
		color.color = new Color(single, single1, color1.b, 0f);
	}

	private void CancelTurret(object sender, EventArgs e)
	{
		if (this.playerMoveC != null)
		{
			this.playerMoveC.CancelTurret();
		}
		this.HideTurretInterface();
	}

	private void CheckWeaponScrollChanged()
	{
		if (this._disabled)
		{
			return;
		}
		if (this.changeWeaponScroll.transform.localPosition.x <= 0f)
		{
			float single = this.changeWeaponScroll.transform.localPosition.x;
			Vector3 vector3 = this.changeWeaponScroll.transform.localPosition;
			this.weaponIndexInScroll = -1 * Mathf.RoundToInt((single - (float)(Mathf.CeilToInt(vector3.x / (float)this.widthWeaponScrollPreview / (float)this.changeWeaponWrap.transform.childCount) * this.widthWeaponScrollPreview * this.changeWeaponWrap.transform.childCount)) / (float)this.widthWeaponScrollPreview);
		}
		else
		{
			float single1 = this.changeWeaponScroll.transform.localPosition.x;
			Vector3 vector31 = this.changeWeaponScroll.transform.localPosition;
			this.weaponIndexInScroll = Mathf.RoundToInt((single1 - (float)(Mathf.FloorToInt(vector31.x / (float)this.widthWeaponScrollPreview / (float)this.changeWeaponWrap.transform.childCount) * this.widthWeaponScrollPreview * this.changeWeaponWrap.transform.childCount)) / (float)this.widthWeaponScrollPreview);
			this.weaponIndexInScroll = this.changeWeaponWrap.transform.childCount - this.weaponIndexInScroll;
		}
		if (this.weaponIndexInScroll == this.changeWeaponWrap.transform.childCount)
		{
			this.weaponIndexInScroll = 0;
		}
		if (this.weaponIndexInScroll != this.weaponIndexInScrollOld)
		{
			this.SelectWeaponFromCategory(((Weapon)WeaponManager.sharedManager.playerWeapons[this.weaponIndexInScroll]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor, false);
		}
		this.weaponIndexInScrollOld = this.weaponIndexInScroll;
	}

	private void ClickPotionButton(int index)
	{
		this.timerShowPotion = this.timerShowPotionMax;
		ElexirInGameButtonController component = this.upButtonsInShopPanel[index].GetComponent<ElexirInGameButtonController>();
		ElexirInGameButtonController str = this.upButtonsInShopPanelSwipeRegim[index].GetComponent<ElexirInGameButtonController>();
		UIButton uIButton = this.upButtonsInShopPanel[index].GetComponent<UIButton>();
		UIButton component1 = this.upButtonsInShopPanelSwipeRegim[index].GetComponent<UIButton>();
		string str1 = component.myPotion.name;
		string str2 = (!Defs.isDaterRegim ? str1 : GearManager.HolderQuantityForID(component.idForPriceInDaterRegim));
		if (Storager.getInt(str2, false) <= 0)
		{
			string str3 = GearManager.AnalyticsIDForOneItemOfGear(str2 ?? "Potion", true);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(GearManager.OneItemIDForGear(str2, GearManager.CurrentNumberOfUphradesForGear(str2)));
			int price = priceByShopId.Price;
			string currency = priceByShopId.Currency;
			ShopNGUIController.TryToBuy(base.gameObject, priceByShopId, () => {
				Storager.setInt(str2, Storager.getInt(str2, false) + 1, false);
				uIButton.normalSprite = "game_clear";
				uIButton.pressedSprite = "game_clear_n";
				component.myLabelCount.gameObject.SetActive(true);
				component.plusSprite.SetActive(false);
				component.priceLabel.SetActive(false);
				component.myLabelCount.text = Storager.getInt(str2, false).ToString();
				component1.normalSprite = "game_clear";
				component1.pressedSprite = "game_clear_n";
				str.myLabelCount.gameObject.SetActive(true);
				str.plusSprite.SetActive(false);
				str.priceLabel.SetActive(false);
				str.myLabelCount.text = Storager.getInt(str2, false).ToString();
				if (str2 != null)
				{
					FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.GearCategory, GearManager.HolderQuantityForID(str2), 1, true);
					FlurryPluginWrapper.LogGearPurchases(GearManager.HolderQuantityForID(str2), 1, true);
					AnalyticsStuff.LogSales(GearManager.HolderQuantityForID(str2), "Gear", false);
					AnalyticsFacade.InAppPurchase(GearManager.HolderQuantityForID(str2), "Gear", 1, price, currency);
				}
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
				{
					{ "Succeeded", str3 }
				}, true);
				FlurryPluginWrapper.LogFastPurchase(str3);
			}, () => {
				JoystickController.leftJoystick.Reset();
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
				{
					{ "Failed", str3 }
				}, true);
			}, null, null, null, null);
		}
		else
		{
			if (!str1.Equals(GearManager.Turret))
			{
				if (Defs.isDaterRegim)
				{
					Storager.setInt(str2, Storager.getInt(str2, false) - 1, false);
				}
				PotionsController.sharedController.ActivatePotion(str1, this.playerMoveC, new Dictionary<string, object>(), false);
			}
			else
			{
				this.ShowTurretInterface();
			}
			string str4 = Storager.getInt(str2, false).ToString();
			component.myLabelCount.gameObject.SetActive(true);
			component.plusSprite.SetActive(false);
			component.myLabelCount.text = str4;
			component.isActivePotion = true;
			uIButton.isEnabled = false;
			component.myLabelTime.enabled = true;
			component.myLabelTime.gameObject.SetActive(true);
			str.myLabelCount.gameObject.SetActive(true);
			str.plusSprite.SetActive(false);
			str.myLabelCount.text = str4;
			str.isActivePotion = true;
			component1.isEnabled = false;
			str.myLabelTime.enabled = true;
			str.myLabelTime.gameObject.SetActive(true);
		}
	}

	[Obfuscation(Exclude=true)]
	private void GenerateMiganie()
	{
		CoinsMessage.FireCoinsAddedEvent(false, 2);
	}

	private void HandleBackupToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(2, true);
	}

	public void HandleBuyGrenadeClicked(object sender, EventArgs e)
	{
		if (Defs.isDaterRegim)
		{
			string str = GearManager.AnalyticsIDForOneItemOfGear(GearManager.Like, true);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(GearManager.OneItemIDForGear("LikeID", GearManager.CurrentNumberOfUphradesForGear("LikeID")));
			ItemPrice itemPrice = new ItemPrice(priceByShopId.Price * 1, priceByShopId.Currency);
			int price = itemPrice.Price;
			string currency = itemPrice.Currency;
			ShopNGUIController.TryToBuy(base.gameObject, itemPrice, () => {
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					Player_move_c grenadeCount = WeaponManager.sharedManager.myPlayerMoveC;
					grenadeCount.GrenadeCount = grenadeCount.GrenadeCount + 1;
				}
				FlurryPluginWrapper.LogPurchaseByModes(ShopNGUIController.CategoryNames.GearCategory, GearManager.Like, 1, true);
				FlurryPluginWrapper.LogGearPurchases(GearManager.Like, 1, true);
				AnalyticsStuff.LogSales(GearManager.Like, "Gear", false);
				AnalyticsFacade.InAppPurchase(GearManager.Like, "Gear", 1, price, currency);
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
				{
					{ "Succeeded", str }
				}, true);
				FlurryPluginWrapper.LogFastPurchase(str);
			}, () => {
				JoystickController.leftJoystick.Reset();
				FlurryPluginWrapper.LogEventAndDublicateToConsole("Fast Purchase", new Dictionary<string, string>()
				{
					{ "Failed", str }
				}, true);
			}, null, null, null, null);
		}
	}

	private void HandleChatSettUpdated()
	{
		this.isChatOn = Defs.IsChatOn;
	}

	private void HandleGearToogleClicked(object sender, EventArgs e)
	{
		bool component = this.gearToogle.GetComponent<UIToggle>().@value;
		this.fonBig.SetActive(component);
		if (!component)
		{
			this.timerShowPotion = -1f;
		}
		else
		{
			this.timerShowPotion = this.timerShowPotionMax;
		}
		for (int i = 0; i < (int)this.upButtonsInShopPanel.Length; i++)
		{
			this.upButtonsInShopPanel[i].SetActive(component);
		}
	}

	private void HandleMeleeToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(3, true);
	}

	private void HandlePotionClicked(object sender, EventArgs e)
	{
		int num = 0;
		int num1 = 0;
		while (num1 < (int)this.upButtonsInShopPanel.Length)
		{
			if (!this.upButtonsInShopPanel[num1].name.Equals(((ButtonHandler)sender).gameObject.name))
			{
				num1++;
			}
			else
			{
				num = num1;
				break;
			}
		}
		this.ClickPotionButton(num);
	}

	private void HandlePremiumToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(6, true);
	}

	private void HandlePrimaryToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(1, true);
	}

	private void HandleSniperToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(5, true);
	}

	private void HandleSpecialToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(4, true);
	}

	public void HandleWeaponEquipped(int catNabor)
	{
		int num = 0;
		IEnumerator enumerator = WeaponManager.sharedManager.playerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				num++;
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
		for (int i = this.changeWeaponWrap.transform.childCount; i < num; i++)
		{
			GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(this.weaponPreviewPrefab);
			vector3.name = string.Concat("WeaponCat_", i.ToString());
			vector3.transform.parent = this.changeWeaponWrap.transform;
			vector3.transform.localScale = new Vector3(1f, 1f, 1f);
			vector3.GetComponent<UITexture>().width = Mathf.RoundToInt((float)this.widthWeaponScrollPreview * 0.7f);
			vector3.GetComponent<UITexture>().height = Mathf.RoundToInt((float)this.widthWeaponScrollPreview * 0.7f);
			vector3.GetComponent<BoxCollider>().size = new Vector3((float)this.widthWeaponScrollPreview * 1.3f, (float)this.widthWeaponScrollPreview * 1.3f, 1f);
		}
		this.changeWeaponWrap.SortAlphabetically();
		this.changeWeaponWrap.GetComponent<MyCenterOnChild>().enabled = false;
		this.changeWeaponWrap.GetComponent<MyCenterOnChild>().enabled = true;
		int num1 = 0;
		for (int j = 0; j < 6; j++)
		{
			Texture texture = ShopNGUIController.TextureForCat(j);
			if (texture != null)
			{
				this.weaponIcons[j].mainTexture = texture;
				IEnumerator enumerator1 = this.changeWeaponWrap.transform.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						Transform transforms = (Transform)enumerator1.Current;
						if (!transforms.name.Equals(string.Concat("WeaponCat_", num1)))
						{
							continue;
						}
						transforms.GetComponent<UITexture>().mainTexture = texture;
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
				num1++;
			}
		}
		for (int k = 0; k < WeaponManager.sharedManager.playerWeapons.Count; k++)
		{
			this.changeWeaponWrap.transform.GetChild(k).GetComponent<WeaponIconController>().myWeaponSounds = ((Weapon)WeaponManager.sharedManager.playerWeapons[k]).weaponPrefab.GetComponent<WeaponSounds>();
		}
		this.SelectWeaponFromCategory(catNabor + 1, true);
	}

	public void HideTurretInterface()
	{
		if (!GlobalGameController.switchingWeaponSwipe)
		{
			this.shopPanelForTap.gameObject.SetActive(true);
		}
		else
		{
			this.shopPanelForSwipe.gameObject.SetActive(true);
		}
		this.swipeWeaponPanel.gameObject.SetActive(true);
		this.turretPanel.SetActive(false);
		this._kBlockPauseShopButton = false;
	}

	private void OnDestroy()
	{
		this.SetNGUITouchDragThreshold(40f);
		InGameGUI.sharedInGameGUI = null;
		WeaponManager.WeaponEquipped -= new Action<int>(this.HandleWeaponEquipped);
		PauseNGUIController.ChatSettUpdated -= new Action(this.HandleChatSettUpdated);
		PauseNGUIController.PlayerHandUpdated -= new Action(this.AdjustToPlayerHands);
		ControlsSettingsBase.ControlsChanged -= new Action(this.AdjustToPlayerHands);
		PauseNGUIController.SwitchingWeaponsUpdated -= new Action(this.SetSwitchingWeaponPanel);
	}

	public void PlayLowResourceBeep(int count)
	{
		this.StopPlayingLowResourceBeep();
		this._lowResourceBeepRoutine = this.PlayLowResourceBeepCoroutine(count);
		base.StartCoroutine(this._lowResourceBeepRoutine);
	}

	[DebuggerHidden]
	private IEnumerator PlayLowResourceBeepCoroutine(int count)
	{
		InGameGUI.u003cPlayLowResourceBeepCoroutineu003ec__Iterator8A variable = null;
		return variable;
	}

	public void PlayLowResourceBeepIfNotPlaying(int count)
	{
		if (this._lowResourceBeepRoutine != null)
		{
			return;
		}
		this.PlayLowResourceBeep(count);
	}

	[Obfuscation(Exclude=true)]
	private void ReloadAmmo()
	{
		this.reloadLabel.gameObject.SetActive(false);
		this.reloadBar.SetActive(false);
		WeaponManager.sharedManager.ReloadAmmo();
	}

	public void ResetDamageTaken()
	{
		for (int i = 0; i < (int)this.damageTakenControllers.Length; i++)
		{
			this.damageTakenControllers[i].Remove();
		}
	}

	public void ResetScope()
	{
		this.scopeText.GetComponent<UITexture>().mainTexture = null;
		this.scopeText.SetActive(false);
	}

	private void RunCircularSpriteOn(UITexture sprite, float length, Action onComplete = null)
	{
		sprite.fillAmount = 0f;
		HOTween.To(sprite, length, (new TweenParms()).Prop("fillAmount", 1f).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(() => {
			sprite.fillAmount = 0f;
			if (onComplete != null)
			{
				onComplete();
			}
		}));
	}

	private void RunTurret(object sender, EventArgs e)
	{
		if (this.playerMoveC != null)
		{
			this.playerMoveC.RunTurret();
		}
		this.HideTurretInterface();
	}

	private void SelectWeaponFromCategory(int category, bool isUpdateSwipe = true)
	{
		int num = 0;
		while (num < WeaponManager.sharedManager.playerWeapons.Count)
		{
			if (((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor != category)
			{
				num++;
			}
			else
			{
				this.SelectWeaponFromIndex(num, isUpdateSwipe);
				break;
			}
		}
	}

	private void SelectWeaponFromIndex(int _index, bool updateSwipe = true)
	{
		bool[] flagArray = new bool[6];
		for (int i = 0; i < (int)flagArray.Length; i++)
		{
			flagArray[i] = false;
		}
		int num = 0;
		IEnumerator enumerator = WeaponManager.sharedManager.playerWeapons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Weapon current = (Weapon)enumerator.Current;
				int component = current.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
				flagArray[component] = true;
				num++;
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
		for (int j = 0; j < (int)this.weaponCategoriesButtons.Length; j++)
		{
			this.weaponCategoriesButtons[j].isEnabled = flagArray[j];
			if (j != ((Weapon)WeaponManager.sharedManager.playerWeapons[_index]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1)
			{
				this.weaponCategoriesButtons[j].GetComponent<UIToggle>().@value = false;
			}
			else
			{
				this.weaponCategoriesButtons[j].GetComponent<UIToggle>().@value = true;
			}
		}
		this.SetChangeWeapon(_index, updateSwipe);
	}

	private void SetArmor()
	{
		this.currentArmorStep = Mathf.FloorToInt(this.pastArmor);
		for (int i = 0; i < (int)this.armorShields.Length; i++)
		{
			this.armorShields[i].SetIndex(Mathf.CeilToInt((this.pastArmor - (float)i) / 9f), HeartEffect.IndicatorType.Armor);
		}
	}

	private void SetChangeWeapon(int index, bool isUpdateSwipe)
	{
		if (isUpdateSwipe)
		{
			if (index >= this.changeWeaponWrap.transform.childCount)
			{
				UnityEngine.Debug.LogError(string.Concat("InGameGUI: not weapon icon with index ", index));
			}
			else
			{
				this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 1E+11f;
				this.changeWeaponWrap.GetComponent<MyCenterOnChild>().CenterOn(this.changeWeaponWrap.transform.GetChild(index));
				this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 8f;
			}
		}
		if (WeaponManager.sharedManager.CurrentWeaponIndex == index)
		{
			return;
		}
		WeaponManager.sharedManager.CurrentWeaponIndex = index;
		WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
		if (this.playerMoveC != null)
		{
			if (this.playerMoveC.currentWeaponBeforeTurret >= 0)
			{
				this.playerMoveC.currentWeaponBeforeTurret = index;
				return;
			}
			this.playerMoveC.ChangeWeapon(index, false);
			this.playerMoveC.HideChangeWeaponTrainingHint();
		}
	}

	public void SetCrosshair(WeaponSounds weaponSounds)
	{
		WeaponCustomCrosshair component = weaponSounds.GetComponent<WeaponCustomCrosshair>();
		if (component == null)
		{
			this.SetCrosshairPart(this.aimCenterSprite, this.defaultAimCenter, false);
			this.SetCrosshairPart(this.aimDownSprite, this.defaultAimDown, false);
			this.SetCrosshairPart(this.aimUpSprite, this.defaultAimUp, false);
			this.SetCrosshairPart(this.aimLeftSprite, this.defaultAimLeftCenter, false);
			this.SetCrosshairPart(this.aimRightSprite, this.defaultAimLeftCenter, true);
			this.SetCrosshairPart(this.aimUpLeftSprite, this.defaultAimLeftUp, false);
			this.SetCrosshairPart(this.aimUpRightSprite, this.defaultAimLeftUp, true);
			this.SetCrosshairPart(this.aimDownLeftSprite, this.defaultAimLeftDown, false);
			this.SetCrosshairPart(this.aimDownRightSprite, this.defaultAimLeftDown, true);
			this.aimPositions[0] = this.defaultAimCenter.offset;
			this.aimPositions[1] = this.defaultAimDown.offset;
			this.aimPositions[2] = this.defaultAimUp.offset;
			this.aimPositions[3] = this.defaultAimLeftCenter.offset;
			this.aimPositions[4] = this.defaultAimLeftDown.offset;
			this.aimPositions[5] = this.defaultAimLeftUp.offset;
		}
		else
		{
			this.SetCrosshairPart(this.aimCenterSprite, component.Data.center, false);
			this.SetCrosshairPart(this.aimDownSprite, component.Data.down, false);
			this.SetCrosshairPart(this.aimUpSprite, component.Data.up, false);
			this.SetCrosshairPart(this.aimLeftSprite, component.Data.left, false);
			this.SetCrosshairPart(this.aimRightSprite, component.Data.left, true);
			this.SetCrosshairPart(this.aimUpLeftSprite, component.Data.leftUp, false);
			this.SetCrosshairPart(this.aimUpRightSprite, component.Data.leftUp, true);
			this.SetCrosshairPart(this.aimDownLeftSprite, component.Data.leftDown, false);
			this.SetCrosshairPart(this.aimDownRightSprite, component.Data.leftDown, true);
			this.aimPositions[0] = component.Data.center.offset;
			this.aimPositions[1] = component.Data.down.offset;
			this.aimPositions[2] = component.Data.up.offset;
			this.aimPositions[3] = component.Data.left.offset;
			this.aimPositions[4] = component.Data.leftDown.offset;
			this.aimPositions[5] = component.Data.leftUp.offset;
		}
		this.UpdateCrosshairPositions();
	}

	private void SetCrosshairPart(UISprite sprite, CrosshairData.aimSprite param, bool mirror = false)
	{
		if (string.IsNullOrEmpty(param.spriteName))
		{
			sprite.gameObject.SetActive(false);
		}
		else
		{
			sprite.gameObject.SetActive(true);
			sprite.spriteName = param.spriteName;
			sprite.width = Mathf.RoundToInt(param.spriteSize.x);
			sprite.height = Mathf.RoundToInt(param.spriteSize.y);
			sprite.transform.localPosition = (!mirror ? param.offset : new Vector2(param.offset.x, -param.offset.y));
		}
	}

	private void SetCrosshairVisibility(bool visible)
	{
		if (this.crosshairVisible == visible)
		{
			return;
		}
		this.crosshairVisible = visible;
		this.aimCenterSprite.enabled = visible;
		this.aimDownSprite.enabled = visible;
		this.aimUpSprite.enabled = visible;
		this.aimLeftSprite.enabled = visible;
		this.aimRightSprite.enabled = visible;
		this.aimUpLeftSprite.enabled = visible;
		this.aimUpRightSprite.enabled = visible;
		this.aimDownLeftSprite.enabled = visible;
		this.aimDownRightSprite.enabled = visible;
	}

	public void SetEnablePerfectLabel(bool enabled)
	{
		if (this.perfectLabels == null)
		{
			return;
		}
		this.perfectLabels.gameObject.SetActive(enabled);
	}

	private void SetHealth()
	{
		this.currentHealthStep = Mathf.FloorToInt(this.pastHealth);
		for (int i = 0; i < (int)this.hearts.Length; i++)
		{
			this.hearts[i].SetIndex(Mathf.CeilToInt((this.pastHealth - (float)i) / 9f), HeartEffect.IndicatorType.Hearts);
		}
	}

	public void SetInterfaceVisible(bool visible)
	{
		this.interfacePanel.GetComponent<UIPanel>().gameObject.SetActive(visible);
		this.joystikPanel.gameObject.SetActive(visible);
		this.shopPanel.gameObject.SetActive(visible);
		this.bloodPanel.gameObject.SetActive(visible);
	}

	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layerNumber;
		}
	}

	private void SetMechHealth()
	{
		this.currentHealthStep = Mathf.FloorToInt(this.pastMechHealth);
		for (int i = 0; i < (int)this.mechShields.Length; i++)
		{
			this.mechShields[i].SetIndex(Mathf.CeilToInt((this.pastMechHealth - (float)i) / 18f), HeartEffect.IndicatorType.Mech);
		}
	}

	private void SetNGUITouchDragThreshold(float newValue)
	{
		if (!(UICamera.mainCamera != null) || !(UICamera.mainCamera.GetComponent<UICamera>() != null))
		{
			UnityEngine.Debug.LogWarning("UICamera.mainCamera is null");
		}
		else
		{
			UICamera.mainCamera.GetComponent<UICamera>().touchDragThreshold = newValue;
		}
	}

	public void SetScopeForWeapon(string num)
	{
		string str;
		this.scopeText.SetActive(true);
		str = (Device.isWeakDevice || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 ? ResPath.Combine("Scopes", string.Concat("Scope_", num, "_small")) : ResPath.Combine("Scopes", string.Concat("Scope_", num)));
		this.scopeText.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(str);
	}

	public void SetSwipeWeaponPanelVisibility(bool visible)
	{
		this.swipeWeaponPanel.localPosition = (!visible ? InGameGUI.swipeWeaponPanelPos + new Vector3(10000f, 0f, 0f) : InGameGUI.swipeWeaponPanelPos);
	}

	public void SetSwitchingWeaponPanel()
	{
		if (!GlobalGameController.switchingWeaponSwipe)
		{
			Transform vector3 = InGameGUI.sharedInGameGUI.swipeWeaponPanel;
			float single = InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition.y;
			Vector3 vector31 = InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition;
			vector3.localPosition = new Vector3(10000f, single, vector31.z);
			InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(true);
			InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(false);
			for (int i = 0; i < (int)InGameGUI.sharedInGameGUI.upButtonsInShopPanel.Length; i++)
			{
				if (!PotionsController.sharedController.PotionIsActive(InGameGUI.sharedInGameGUI.upButtonsInShopPanel[i].GetComponent<ElexirInGameButtonController>().myPotion.name))
				{
					InGameGUI.sharedInGameGUI.upButtonsInShopPanel[i].GetComponent<ElexirInGameButtonController>().myLabelTime.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition = InGameGUI.swipeWeaponPanelPos;
			InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(false);
			InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(true);
		}
	}

	public void ShowCircularIndicatorOnReload(float length)
	{
		this.StopAllCircularIndicators();
		this.reloadBar.SetActive(true);
		this.reloadLabel.gameObject.SetActive(true);
		base.Invoke("ReloadAmmo", length);
		if (this.playerMoveC != null)
		{
			this.playerMoveC.isReloading = true;
		}
		this.RunCircularSpriteOn(this.reloadCircularSprite, length, () => {
		});
	}

	public void ShowControlSchemeConfigurator()
	{
	}

	public void ShowImpact()
	{
		this.impactTween.gameObject.SetActive(true);
		this.impactTween.Play(true);
		if (Defs.isSoundFX)
		{
			this.impactTween.GetComponent<UIPlaySound>().Play();
		}
	}

	public void ShowTurretInterface()
	{
		this.swipeWeaponPanel.gameObject.SetActive(false);
		this.shopPanelForSwipe.gameObject.SetActive(false);
		this.shopPanelForTap.gameObject.SetActive(false);
		this.runTurrelButton.GetComponent<UIButton>().isEnabled = false;
		this.turretPanel.SetActive(true);
		if (this.playerMoveC != null)
		{
			this.playerMoveC.ChangeWeapon(1001, false);
		}
		this._kBlockPauseShopButton = true;
	}

	private void Start()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.SetSwipeWeaponPanelVisibility(false);
		}
		this.bankView.SetActive(!Device.isPixelGunLow);
		this.bankViewLow.SetActive(Device.isPixelGunLow);
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		if (!Defs.isMulti && !Defs.IsSurvival)
		{
			this.CampaignContainer.SetActive(true);
		}
		if (!Defs.isMulti && Defs.IsSurvival)
		{
			this.survivalContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			if (!Defs.isDaterRegim)
			{
				this.deathmatchContainer.SetActive(true);
			}
			else
			{
				this.daterContainer.SetActive(true);
			}
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			this.timeBattleContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			this.teamBattleContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.flagCaptureContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			this.deadlygamesContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			this.CapturePointContainer.SetActive(true);
		}
		this.turretPanel.SetActive(false);
		if (this.runTurrelButton != null)
		{
			this.runTurrelButton.Clicked += new EventHandler(this.RunTurret);
		}
		if (this.cancelTurrelButton != null)
		{
			this.cancelTurrelButton.Clicked += new EventHandler(this.CancelTurret);
		}
		if (!this.isMulti)
		{
			this.zombieCreator = ZombieCreator.sharedCreator;
		}
		else
		{
			this.enemiesLeftLabel.SetActive(false);
		}
		this.AdjustToPlayerHands();
		PauseNGUIController.PlayerHandUpdated += new Action(this.AdjustToPlayerHands);
		PauseNGUIController.SwitchingWeaponsUpdated += new Action(this.SetSwitchingWeaponPanel);
		WeaponManager.WeaponEquipped += new Action<int>(this.HandleWeaponEquipped);
		int num = (!this.isMulti ? WeaponManager.sharedManager.CurrentWeaponIndex : WeaponManager.sharedManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons());
		this.HandleWeaponEquipped(((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1);
		if (num >= this.changeWeaponWrap.transform.childCount)
		{
			UnityEngine.Debug.LogError(string.Concat("InGameGUI: not weapon icon with index ", ((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1));
		}
		else
		{
			this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 1E+11f;
			this.changeWeaponWrap.GetComponent<MyCenterOnChild>().CenterOn(this.changeWeaponWrap.transform.GetChild(num));
			this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 8f;
		}
		if (this.gearToogle != null)
		{
			this.gearToogle.gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleGearToogleClicked);
		}
		if (this.weaponCategoriesButtons[0] != null)
		{
			this.weaponCategoriesButtons[0].gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandlePrimaryToogleClicked);
		}
		if (this.weaponCategoriesButtons[1] != null)
		{
			this.weaponCategoriesButtons[1].gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleBackupToogleClicked);
		}
		if (this.weaponCategoriesButtons[2] != null)
		{
			this.weaponCategoriesButtons[2].gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleMeleeToogleClicked);
		}
		if (this.weaponCategoriesButtons[3] != null)
		{
			this.weaponCategoriesButtons[3].gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleSpecialToogleClicked);
		}
		if (this.weaponCategoriesButtons[4] != null)
		{
			this.weaponCategoriesButtons[4].gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleSniperToogleClicked);
		}
		if (this.weaponCategoriesButtons[5] != null)
		{
			this.weaponCategoriesButtons[5].gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandlePremiumToogleClicked);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.gearToogle.GetComponent<UIToggle>().@value = false;
			this.HandleGearToogleClicked(null, null);
		}
		for (int i = 0; i < (int)this.upButtonsInShopPanel.Length; i++)
		{
			this.StartUpdatePotionButton(this.upButtonsInShopPanel[i]);
		}
		for (int j = 0; j < (int)this.upButtonsInShopPanelSwipeRegim.Length; j++)
		{
			this.StartUpdatePotionButton(this.upButtonsInShopPanelSwipeRegim[j]);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.fastShopPanel.transform.localPosition = new Vector3(-1000f, -1000f, -1f);
			this.gearToogle.isEnabled = false;
		}
		this.SetNGUITouchDragThreshold(1f);
	}

	public void StartFireCircularIndicators(float length)
	{
		this.StopAllCircularIndicators();
		this.RunCircularSpriteOn(this.fireCircularSprite, length, null);
		this.RunCircularSpriteOn(this.fireAdditionalCrcualrSprite, length, null);
	}

	private void StartUpdatePotionButton(GameObject potionButton)
	{
		if (potionButton != null)
		{
			potionButton.gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandlePotionClicked);
			ElexirInGameButtonController component = potionButton.GetComponent<ElexirInGameButtonController>();
			string str = component.myPotion.name;
			string str1 = (!Defs.isDaterRegim ? str : GearManager.HolderQuantityForID(component.idForPriceInDaterRegim));
			if (PotionsController.sharedController.PotionIsActive(str))
			{
				UIButton uIButton = potionButton.GetComponent<UIButton>();
				component.isActivePotion = true;
				component.myLabelTime.gameObject.SetActive(true);
				component.myLabelTime.enabled = true;
				component.priceLabel.SetActive(false);
				component.myLabelCount.gameObject.SetActive(true);
				component.plusSprite.SetActive(false);
				UILabel uILabel = component.myLabelCount;
				int num = Storager.getInt(str1, false);
				uILabel.text = num.ToString();
				uIButton.isEnabled = false;
			}
		}
	}

	public void StopAllCircularIndicators()
	{
		base.CancelInvoke("ReloadAmmo");
		if (this.playerMoveC != null)
		{
			this.playerMoveC.isReloading = false;
		}
		if (this.circularSprites == null)
		{
			UnityEngine.Debug.LogWarning("Circular sprites is null!");
			return;
		}
		UITexture[] uITextureArray = this.circularSprites;
		for (int i = 0; i < (int)uITextureArray.Length; i++)
		{
			UITexture uITexture = uITextureArray[i];
			HOTween.Kill(uITexture);
			uITexture.fillAmount = 0f;
		}
		this.reloadLabel.gameObject.SetActive(false);
		this.reloadBar.SetActive(false);
	}

	public void StopPlayingLowResourceBeep()
	{
		if (this._lowResourceBeepRoutine != null)
		{
			base.StopCoroutine(this._lowResourceBeepRoutine);
			this._lowResourceBeepRoutine = null;
		}
	}

	private void Update()
	{
		string str;
		string str1;
		object obj;
		this.CheckWeaponScrollChanged();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			this.fastShopPanel.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			this.fastShopPanel.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (this.timerBlinkNoAmmo > 0f)
		{
			this.timerBlinkNoAmmo -= Time.deltaTime;
		}
		if (this.timerBlinkNoAmmo > 0f && this.playerMoveC != null && !this.playerMoveC.isMechActive)
		{
			this.blinkNoAmmoLabel.gameObject.SetActive(true);
			float single = this.timerBlinkNoAmmo % this.periodBlink / this.periodBlink;
			this.blinkNoAmmoLabel.color = new Color(this.blinkNoAmmoLabel.color.r, this.blinkNoAmmoLabel.color.g, this.blinkNoAmmoLabel.color.b, (single >= 0.5f ? (1f - single) * 2f : single * 2f));
		}
		if ((this.timerBlinkNoAmmo < 0f || this.playerMoveC != null && this.playerMoveC.isMechActive) && this.blinkNoAmmoLabel.gameObject.activeSelf)
		{
			this.blinkNoAmmoLabel.gameObject.SetActive(false);
		}
		if (this.playerMoveC != null)
		{
			int num = Mathf.FloorToInt(this.playerMoveC.CurHealth);
			if (num < this.oldCountHeath && this.timerBlinkNoHeath < 0f && num < 3)
			{
				this.timerBlinkNoHeath = this.periodBlink * 3f;
			}
			if (num > 2)
			{
				this.timerBlinkNoHeath = -1f;
			}
			this.oldCountHeath = num;
			if (this.timerBlinkNoHeath > 0f)
			{
				this.timerBlinkNoHeath -= Time.deltaTime;
			}
			if (this.timerBlinkNoHeath > 0f && !this.playerMoveC.isMechActive)
			{
				if (num > 0)
				{
					this.PlayLowResourceBeepIfNotPlaying(1);
				}
				this.blinkNoHeathLabel.gameObject.SetActive(true);
				float single1 = this.timerBlinkNoHeath % this.periodBlink / this.periodBlink;
				float single2 = (single1 >= 0.5f ? (1f - single1) * 2f : single1 * 2f);
				UILabel color = this.blinkNoHeathLabel;
				float single3 = this.blinkNoHeathLabel.color.r;
				float single4 = this.blinkNoHeathLabel.color.g;
				Color color1 = this.blinkNoHeathLabel.color;
				color.color = new Color(single3, single4, color1.b, single2);
				for (int i = 0; i < (int)this.blinkNoHeathFrames.Length; i++)
				{
					this.blinkNoHeathFrames[i].gameObject.SetActive(true);
					this.blinkNoHeathFrames[i].color = new Color(1f, 1f, 1f, single2);
				}
			}
		}
		if ((this.timerBlinkNoHeath < 0f || this.playerMoveC == null || this.playerMoveC != null && this.playerMoveC.isMechActive) && this.blinkNoHeathLabel.gameObject.activeSelf)
		{
			this.blinkNoHeathLabel.gameObject.SetActive(false);
			for (int j = 0; j < (int)this.blinkNoHeathFrames.Length; j++)
			{
				this.blinkNoHeathFrames[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < (int)this.ammoCategoriesLabels.Length; k++)
		{
			if (this.ammoCategoriesLabels[k] != null)
			{
				bool flag = false;
				if (this.weaponCategoriesButtons[k].isEnabled)
				{
					int num1 = 0;
					while (num1 < WeaponManager.sharedManager.playerWeapons.Count)
					{
						Weapon item = (Weapon)WeaponManager.sharedManager.playerWeapons[num1];
						if ((!item.weaponPrefab.GetComponent<WeaponSounds>().isMelee || item.weaponPrefab.GetComponent<WeaponSounds>().isShotMelee) && item.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == k + 1)
						{
							this.ammoCategoriesLabels[k].text = (!item.weaponPrefab.GetComponent<WeaponSounds>().isShotMelee ? string.Concat(item.currentAmmoInClip, "/", item.currentAmmoInBackpack) : (item.currentAmmoInClip + item.currentAmmoInBackpack).ToString());
							flag = true;
							break;
						}
						else
						{
							num1++;
						}
					}
				}
				if (!flag)
				{
					this.ammoCategoriesLabels[k].text = string.Empty;
				}
			}
		}
		if (this.timerShowNow > 0f)
		{
			this.timerShowNow -= Time.deltaTime;
			if (!this.message_now.activeSelf)
			{
				this.message_now.SetActive(true);
			}
		}
		else if (this.message_now.activeSelf)
		{
			this.message_now.SetActive(false);
		}
		if (this.isMulti && this.playerMoveC == null && WeaponManager.sharedManager.myPlayer != null)
		{
			this.playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (!this.isMulti && this.playerMoveC == null)
		{
			this.playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (this.isMulti && this.playerMoveC != null)
		{
			for (int l = 0; l < 3; l++)
			{
				UIPlaySound component = this.messageAddScore[l].GetComponent<UIPlaySound>();
				if (!Defs.isSoundFX)
				{
					obj = null;
				}
				else
				{
					obj = 1;
				}
				component.volume = (float)obj;
				float single5 = 0.3f;
				float single6 = 0.2f;
				if (l == 0)
				{
					float single7 = 1f;
					if (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[l] < single5)
					{
						single7 = 1f + single6 * (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[l]) / single5;
					}
					if (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[l] - single5 < single5)
					{
						single7 = 1f + single6 * (1f - (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[l] - single5) / single5);
					}
					this.messageAddScore[l].transform.localScale = new Vector3(single7, single7, single7);
				}
				if (this.playerMoveC.timerShow[l] <= 0f)
				{
					this.killLabels[l].gameObject.SetActive(false);
				}
				else
				{
					this.killLabels[l].gameObject.SetActive(true);
					this.killLabels[l].SetChatLabelText(this.playerMoveC.killedSpisok[l]);
				}
				if (this.playerMoveC.myScoreController.timerAddScoreShow[l] > 0f)
				{
					if (!this.messageAddScore[l].gameObject.activeSelf)
					{
						this.messageAddScore[l].gameObject.SetActive(true);
					}
					this.messageAddScore[l].text = this.playerMoveC.myScoreController.addScoreString[l];
					this.messageAddScore[l].color = new Color(1f, 1f, 1f, (this.playerMoveC.myScoreController.timerAddScoreShow[l] <= 1f ? this.playerMoveC.myScoreController.timerAddScoreShow[l] : 1f));
				}
				else if (this.messageAddScore[l].gameObject.activeSelf)
				{
					this.messageAddScore[l].gameObject.SetActive(false);
				}
			}
			if (this.isChatOn)
			{
				int num2 = 0;
				for (int m = this.playerMoveC.messages.Count - 1; m >= 0 && this.playerMoveC.messages.Count - m - 1 < 3; m--)
				{
					if (Time.time - this.playerMoveC.messages[m].time >= 10f)
					{
						this.chatLabels[num2].SetActive(false);
					}
					else
					{
						if ((this.isInet || !(this.playerMoveC.messages[m].IDLocal == WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID)) && (!this.isInet || this.playerMoveC.messages[m].ID != WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID))
						{
							if (this.playerMoveC.messages[m].command == 0)
							{
								this.chatLabels[num2].GetComponent<UILabel>().color = new Color(1f, 1f, 0.15f, 1f);
							}
							if (this.playerMoveC.messages[m].command == 1)
							{
								this.chatLabels[num2].GetComponent<UILabel>().color = new Color(0f, 0f, 0.9f, 1f);
							}
							if (this.playerMoveC.messages[m].command == 2)
							{
								this.chatLabels[num2].GetComponent<UILabel>().color = new Color(1f, 0f, 0f, 1f);
							}
						}
						else
						{
							this.chatLabels[num2].GetComponent<UILabel>().color = new Color(0f, 1f, 0.15f, 1f);
						}
						ChatLabel chatLabel = this.chatLabels[num2].GetComponent<ChatLabel>();
						chatLabel.nickLabel.text = this.playerMoveC.messages[m].text;
						chatLabel.iconSprite.spriteName = this.playerMoveC.messages[m].iconName;
						Transform vector3 = chatLabel.iconSprite.transform;
						float single8 = (float)(chatLabel.nickLabel.width + 20);
						float single9 = vector3.localPosition.y;
						Vector3 vector31 = vector3.localPosition;
						vector3.localPosition = new Vector3(single8, single9, vector31.z);
						chatLabel.clanTexture.mainTexture = this.playerMoveC.messages[m].clanLogo;
						this.chatLabels[num2].SetActive(true);
					}
					num2++;
				}
				for (int n = num2; n < 3; n++)
				{
					this.chatLabels[num2].SetActive(false);
				}
			}
			if (this.timerShowScorePict > 0f)
			{
				this.timerShowScorePict -= Time.deltaTime;
			}
			if (!this.isHunger || Initializer.players.Count != 2 || !this.hungerGameController.isGo || this.playerMoveC.timeHingerGame <= 10f)
			{
				if (this.duel.activeSelf)
				{
					this.duel.SetActive(false);
				}
				if (this.timerShowScorePict > 0f)
				{
					if ((!this.multyKillPanel.gameObject.activeSelf || this.scorePictName != this.multyKillSprite.spriteName) && (PauseGUIController.Instance == null || !PauseGUIController.Instance.IsPaused))
					{
						this.multyKillSprite.spriteName = this.scorePictName;
						this.multyKillPanel.gameObject.SetActive(true);
						this.multyKillPanel.GetComponent<MultyKill>().PlayTween();
					}
				}
				else if (this.multyKillPanel.gameObject.activeSelf)
				{
					this.multyKillPanel.gameObject.SetActive(false);
				}
			}
			else
			{
				this.duel.SetActive(true);
				this.multyKillPanel.gameObject.SetActive(false);
			}
			if (this.isHunger && !this.hungerGameController.isGo)
			{
				this.timerStartHungerLabel.gameObject.SetActive(true);
				int num3 = Mathf.FloorToInt(this.hungerGameController.goTimer);
				if (num3 != 0)
				{
					str = string.Concat(string.Empty, num3);
					this.timerStartHungerLabel.color = new Color(1f, 0f, 0f, 1f);
				}
				else
				{
					str = "GO!";
					this.timerStartHungerLabel.color = new Color(0f, 1f, 0f, 1f);
				}
				this.timerStartHungerLabel.text = str;
			}
			else if (!this.isHunger || !this.hungerGameController.isGo || !this.hungerGameController.isShowGo)
			{
				this.timerStartHungerLabel.gameObject.SetActive(false);
			}
			else
			{
				this.timerStartHungerLabel.gameObject.SetActive(true);
				this.timerStartHungerLabel.text = "GO!";
			}
		}
		if (this.playerMoveC != null)
		{
			if (this.playerMoveC.timerShowDown <= 0f || this.playerMoveC.timerShowDown >= this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.downBloodTexture.SetActive(false);
			}
			else
			{
				this.downBloodTexture.SetActive(true);
			}
			if (this.playerMoveC.timerShowUp <= 0f || this.playerMoveC.timerShowUp >= this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.upBloodTexture.SetActive(false);
			}
			else
			{
				this.upBloodTexture.SetActive(true);
			}
			if (this.playerMoveC.timerShowLeft <= 0f || this.playerMoveC.timerShowLeft >= this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.leftBloodTexture.SetActive(false);
			}
			else
			{
				this.leftBloodTexture.SetActive(true);
			}
			if (this.playerMoveC.timerShowRight <= 0f || this.playerMoveC.timerShowRight >= this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.rightBloodTexture.SetActive(false);
			}
			else
			{
				this.rightBloodTexture.SetActive(true);
			}
			if (this.playerMoveC.isZooming || !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.isPressSkip)
			{
				this.SetCrosshairVisibility(false);
			}
			else
			{
				this.SetCrosshairVisibility(true);
				this.UpdateCrosshairPositions();
			}
		}
		bool flag1 = true;
		if (SceneManager.GetActiveScene().name == Defs.TrainingSceneName)
		{
			flag1 = false;
		}
		this.shopButton.GetComponent<UIButton>().isEnabled = (!flag1 ? false : !this.turretPanel.activeSelf);
		this.shopButtonInPause.GetComponent<UIButton>().isEnabled = (!flag1 ? false : !this._kBlockPauseShopButton);
		if (!this.isMulti && this.zombieCreator != null)
		{
			int enemiesToKill = GlobalGameController.EnemiesToKill - this.zombieCreator.NumOfDeadZombies;
			if (Defs.IsSurvival || enemiesToKill != 0)
			{
				this.enemiesLeftLabel.SetActive(false);
			}
			else
			{
				str1 = (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName) ? LocalizationStore.Get("Key_0854") : LocalizationStore.Get("Key_0192"));
				if (this.zombieCreator.bossShowm)
				{
					str1 = LocalizationStore.Get("Key_0855");
				}
				this.enemiesLeftLabel.SetActive((this.perfectLabels == null ? true : !this.perfectLabels.gameObject.activeInHierarchy));
				this.enemiesLeftLabel.GetComponent<UILabel>().text = str1;
			}
		}
		if (!(this.playerMoveC != null) || !this.playerMoveC.isMechActive)
		{
			if (!Defs.isDaterRegim)
			{
				if (this.playerMoveC.respawnedForGUI || this.mechWasActive)
				{
					this.currentMechHealthStep = Mathf.CeilToInt(this.playerMoveC.liveMech);
					this.pastHealth = this.health();
					this.SetHealth();
				}
				else if (!this.healthInAnim && this.pastHealth != this.health())
				{
					base.StartCoroutine(this.AnimateHealth());
				}
				this.pastHealth = this.health();
			}
			else
			{
				for (int o = 0; o < Player_move_c.MaxPlayerGUIHealth; o++)
				{
					this.hearts[o].gameObject.SetActive(false);
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
			{
				for (int p = 0; p < Player_move_c.MaxPlayerGUIHealth; p++)
				{
					this.armorShields[p].gameObject.SetActive(false);
				}
			}
			else if (!Defs.isDaterRegim)
			{
				if (this.playerMoveC.respawnedForGUI || this.mechWasActive)
				{
					this.currentMechHealthStep = Mathf.CeilToInt(this.playerMoveC.liveMech);
					this.pastArmor = this.armor();
					this.SetArmor();
				}
				else if (!this.armorInAnim && this.pastArmor != this.armor())
				{
					base.StartCoroutine(this.AnimateArmor());
				}
				this.pastArmor = this.armor();
			}
			else
			{
				for (int q = 0; q < Player_move_c.MaxPlayerGUIHealth; q++)
				{
					this.armorShields[q].gameObject.SetActive(false);
				}
				this.pastArmor = 0f;
			}
			this.mechWasActive = false;
			this.playerMoveC.respawnedForGUI = false;
		}
		else
		{
			if (!this.mechWasActive)
			{
				this.currentHealthStep = Mathf.CeilToInt(this.health());
				this.currentArmorStep = Mathf.CeilToInt(this.armor());
				this.pastMechHealth = this.playerMoveC.liveMech;
				this.SetMechHealth();
				this.mechWasActive = true;
			}
			else if (!this.mechInAnim && this.pastMechHealth != this.playerMoveC.liveMech)
			{
				base.StartCoroutine(this.AnimateMechHealth());
			}
			this.pastMechHealth = this.playerMoveC.liveMech;
		}
		if (Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight))
		{
			int winningTeam = WeaponManager.sharedManager.myNetworkStartTable.GetWinningTeam();
			this.mineBlue.SetActive(WeaponManager.sharedManager.myNetworkStartTable.myCommand > 0);
			bool flag2 = WeaponManager.sharedManager.myNetworkStartTable.myCommand == winningTeam;
			this.winningBlue.SetActive((winningTeam == 0 ? false : flag2));
			this.winningRed.SetActive((winningTeam == 0 ? false : !flag2));
		}
		if (!Defs.isDaterRegim && Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle))
		{
			int placeInTable = WeaponManager.sharedManager.myNetworkStartTable.GetPlaceInTable();
			this.placeDeathmatchLabel.text = (placeInTable + 1).ToString();
			this.placeCoopLabel.text = (placeInTable + 1).ToString();
			this.firstPlaceGO.SetActive(placeInTable == 0);
			this.firstPlaceCoop.SetActive(placeInTable == 0);
		}
		if (PauseGUIController.Instance != null)
		{
			bool isPaused = !PauseGUIController.Instance.IsPaused;
			if (this.leftAnchor != null && this.leftAnchor.activeInHierarchy != isPaused)
			{
				this.leftAnchor.SetActive(isPaused);
			}
			if (this.swipeWeaponPanel != null && this.swipeWeaponPanel.gameObject.activeInHierarchy != isPaused)
			{
				this.swipeWeaponPanel.gameObject.SetActive(isPaused);
			}
		}
	}

	private void UpdateCrosshairPositions()
	{
		float single;
		if (this.playerMoveC == null || !this.playerMoveC.isMechActive)
		{
			single = WeaponManager.sharedManager.currentWeaponSounds.tekKoof * WeaponManager.sharedManager.currentWeaponSounds.startZone.y * 0.5f;
			this.aimDown.transform.localPosition = new Vector3(0f, -this.aimPositions[1].y - single, 0f);
			this.aimUp.transform.localPosition = new Vector3(0f, this.aimPositions[2].y + single, 0f);
			this.aimLeft.transform.localPosition = new Vector3(-this.aimPositions[3].x - single, 0f, 0f);
			this.aimDownLeft.transform.localPosition = new Vector3(-this.aimPositions[4].x - single, -this.aimPositions[4].y - single, 0f);
			this.aimUpLeft.transform.localPosition = new Vector3(-this.aimPositions[5].x - single, this.aimPositions[5].y + single, 0f);
			this.aimRight.transform.localPosition = new Vector3(this.aimPositions[3].x + single, 0f, 0f);
			this.aimDownRight.transform.localPosition = new Vector3(this.aimPositions[4].x + single, -this.aimPositions[4].y - single, 0f);
			this.aimUpRight.transform.localPosition = new Vector3(this.aimPositions[5].x + single, this.aimPositions[5].y + single, 0f);
		}
		else
		{
			single = 12f + this.playerMoveC.mechWeaponSounds.tekKoof * this.playerMoveC.mechWeaponSounds.startZone.y * 0.5f;
			this.aimUp.transform.localPosition = new Vector3(0f, single, 0f);
			this.aimUpRight.transform.localPosition = new Vector3(single, single, 0f);
			this.aimRight.transform.localPosition = new Vector3(single, 0f, 0f);
			this.aimDownRight.transform.localPosition = new Vector3(single, -single, 0f);
			this.aimDown.transform.localPosition = new Vector3(0f, -single, 0f);
			this.aimDownLeft.transform.localPosition = new Vector3(-single, -single, 0f);
			this.aimLeft.transform.localPosition = new Vector3(-single, 0f, 0f);
			this.aimUpLeft.transform.localPosition = new Vector3(-single, single, 0f);
		}
	}

	public delegate float GetFloatVAlue();

	public delegate int GetIntVAlue();

	public delegate string GetString();
}