using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

internal sealed class ProfileController : MonoBehaviour
{
	public const string keyChooseDefaultName = "keyChooseDefaultName";

	private const string NicknameKey = "NamePlayer";

	public static SaltedInt countGameTotalKills;

	public static SaltedInt countGameTotalDeaths;

	public static SaltedInt countGameTotalShoot;

	public static SaltedInt countGameTotalHit;

	public static SaltedInt countLikes;

	public ProfileView profileView;

	public static string[] DefaultKeyNames;

	private static string _defaultPlayerName;

	private static ProfileController _instance;

	private IDisposable _backSubscription;

	private bool _dirty;

	private bool _escapePressed;

	private Action[] _exitCallbacks = new Action[0];

	private float _idleTimeStart;

	private Quaternion _initialLocalRotation;

	private float _lastTime;

	private Rect? _touchZone;

	private Color? _storedAmbientLight;

	private bool _isNicknameSubmit;

	private EventHandler EscapePressed;

	public static int CurOrderCup
	{
		get
		{
			int num = ExperienceController.sharedController.currentLevel;
			for (int i = 0; i < (int)ExpController.LevelsForTiers.Length; i++)
			{
				if (num >= ProfileController.MinLevelTir(i) && num <= ProfileController.MaxLevelTir(i))
				{
					return i;
				}
			}
			return -1;
		}
	}

	public static string defaultPlayerName
	{
		get
		{
			if (!PlayerPrefs.HasKey("keyChooseDefaultName"))
			{
				ProfileController._defaultPlayerName = ProfileController.GetRandomName();
				PlayerPrefs.SetString("keyChooseDefaultName", ProfileController._defaultPlayerName);
			}
			if (ProfileController._defaultPlayerName == null)
			{
				ProfileController._defaultPlayerName = PlayerPrefs.GetString("keyChooseDefaultName");
			}
			return ProfileController._defaultPlayerName;
		}
	}

	public string DesiredWeaponTag
	{
		get;
		set;
	}

	public static ProfileController Instance
	{
		get
		{
			return ProfileController._instance;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			return (!(this.profileView != null) || !(this.profileView.interfaceHolder != null) ? false : this.profileView.interfaceHolder.gameObject.activeInHierarchy);
		}
		private set
		{
			if (this.profileView != null && this.profileView.interfaceHolder != null)
			{
				this.profileView.interfaceHolder.gameObject.SetActive(value);
				if (!value)
				{
					this.DesiredWeaponTag = string.Empty;
					if (this._backSubscription != null)
					{
						this._backSubscription.Dispose();
						this._backSubscription = null;
					}
				}
				else
				{
					this.Refresh(true);
					if (ExperienceController.sharedController != null && ExpController.Instance != null)
					{
						ExperienceController.sharedController.isShowRanks = true;
						ExpController.Instance.InterfaceEnabled = true;
					}
					if (this._backSubscription != null)
					{
						this._backSubscription.Dispose();
					}
					this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Profile Controller");
				}
				FreeAwardShowHandler.CheckShowChest(value);
			}
		}
	}

	static ProfileController()
	{
		ProfileController.countGameTotalKills = new SaltedInt(640178770);
		ProfileController.countGameTotalDeaths = new SaltedInt(371743314);
		ProfileController.countGameTotalShoot = new SaltedInt(623401554);
		ProfileController.countGameTotalHit = new SaltedInt(606624338);
		ProfileController.countLikes = new SaltedInt(606624338);
		ProfileController.DefaultKeyNames = new string[] { "Key_2020", "Key_2021", "Key_2022", "Key_2023", "Key_2024", "Key_2025", "Key_2026", "Key_2027", "Key_2028", "Key_2029", "Key_2030", "Key_2031", "Key_2032", "Key_2033", "Key_2034", "Key_2035", "Key_2036", "Key_2037", "Key_2038" };
		ProfileController._defaultPlayerName = null;
	}

	public ProfileController()
	{
	}

	private void Awake()
	{
		ProfileController._instance = this;
	}

	[DebuggerHidden]
	private IEnumerator ExitCallbacksCoroutine()
	{
		ProfileController.u003cExitCallbacksCoroutineu003ec__Iterator17C variable = null;
		return variable;
	}

	public static float GetPerFillProgress(int order, int lev)
	{
		float single = 0f;
		int num = lev;
		if (order >= (int)ExpController.LevelsForTiers.Length)
		{
			single = 0f;
		}
		else
		{
			int num1 = ProfileController.MinLevelTir(order);
			int num2 = num1;
			num2 = ProfileController.MaxLevelTir(order);
			float single1 = (float)(num - num1);
			float single2 = (float)(num2 - num1);
			single = (single1 <= 0f ? 0f : single1 / single2);
		}
		return single;
	}

	public static string GetPlayerNameOrDefault()
	{
		if (PlayerPrefs.HasKey("NamePlayer"))
		{
			string str = PlayerPrefs.GetString("NamePlayer");
			if (str != null)
			{
				return str;
			}
		}
		string instance = PlayerPrefs.GetString("SocialName", string.Empty);
		if (Social.localUser != null && Social.localUser.authenticated && !string.IsNullOrEmpty(Social.localUser.userName))
		{
			if (!instance.Equals(Social.localUser.userName))
			{
				instance = Social.localUser.userName;
				PlayerPrefs.SetString("SocialName", instance);
			}
			return instance;
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (GameCircleSocial.Instance.localUser.authenticated)
			{
				if (!string.IsNullOrEmpty(GameCircleSocial.Instance.localUser.userName))
				{
					if (!instance.Equals(GameCircleSocial.Instance.localUser.userName))
					{
						instance = GameCircleSocial.Instance.localUser.userName;
						PlayerPrefs.SetString("SocialName", instance);
					}
					return instance;
				}
			}
		}
		if (!string.IsNullOrEmpty(instance))
		{
			return instance;
		}
		return ProfileController.defaultPlayerName;
	}

	private static string GetRandomName()
	{
		if (ProfileController.DefaultKeyNames == null || (int)ProfileController.DefaultKeyNames.Length <= 0)
		{
			return "Player";
		}
		int num = UnityEngine.Random.Range(0, (int)ProfileController.DefaultKeyNames.Length);
		return LocalizationStore.Get(ProfileController.DefaultKeyNames[num]);
	}

	public void HandleAchievementsButton()
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Achievements] button pressed");
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
			case RuntimePlatform.IPhonePlayer:
			{
				if (!Application.isEditor)
				{
					if (!Social.localUser.authenticated)
					{
						GameCenterSingleton.Instance.updateGameCenter();
					}
					else
					{
						Social.ShowAchievementsUI();
					}
				}
				break;
			}
			case RuntimePlatform.PS3:
			case RuntimePlatform.XBOX360:
			{
				break;
			}
			case RuntimePlatform.Android:
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					Social.ShowAchievementsUI();
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					AGSAchievementsClient.ShowAchievementsOverlay();
				}
				break;
			}
			default:
			{
				goto case RuntimePlatform.XBOX360;
			}
		}
	}

	private void HandleBackRequest(object sender, EventArgs e)
	{
		if (this._dirty && FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendOurData(false);
			this._dirty = false;
		}
		Action action = this._exitCallbacks.FirstOrDefault<Action>();
		if (action != null)
		{
			action();
		}
		base.StartCoroutine(this.ExitCallbacksCoroutine());
	}

	public void HandleBankButton()
	{
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("BankController.Instance == null");
		}
		else
		{
			EventHandler instance = null;
			instance = (object sender, EventArgs e) => {
				BankController.Instance.BackRequested -= this.handleBackFromBank;
				BankController.Instance.InterfaceEnabled = false;
				this.u003cu003ef__this.InterfaceEnabled = true;
			};
			BankController.Instance.BackRequested += instance;
			BankController.Instance.InterfaceEnabled = true;
			this.InterfaceEnabled = false;
		}
	}

	private void HandleEscape()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (InfoWindowController.IsActive)
		{
			InfoWindowController.HideCurrentWindow();
			return;
		}
		this._escapePressed = true;
	}

	public void HandleLeaderboardsButton()
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Leaderboards] button pressed");
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
			case RuntimePlatform.IPhonePlayer:
			{
				if (!Application.isEditor)
				{
					if (!Social.localUser.authenticated)
					{
						GameCenterSingleton.Instance.updateGameCenter();
					}
					else
					{
						Social.ShowLeaderboardUI();
					}
				}
				break;
			}
			case RuntimePlatform.PS3:
			case RuntimePlatform.XBOX360:
			{
				break;
			}
			case RuntimePlatform.Android:
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					Social.ShowLeaderboardUI();
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					AGSLeaderboardsClient.ShowLeaderboardsOverlay();
				}
				break;
			}
			default:
			{
				goto case RuntimePlatform.XBOX360;
			}
		}
	}

	private void HandleNicknameInput(object sender, ProfileView.InputEventArgs e)
	{
		this.SaveNamePlayer(e.Input);
	}

	private void HandleOurInfoUpdated()
	{
		if (this.InterfaceEnabled)
		{
			this.Refresh(false);
		}
	}

	private string InitializeSecretBonusCountLabel(VirtualCurrencyBonusType bonusType)
	{
		List<string> levelsWhereGotBonus = CoinBonus.GetLevelsWhereGotBonus(bonusType);
		int num = Math.Min(20, levelsWhereGotBonus.Count);
		return string.Concat(num, '/', 20);
	}

	private string InitializeStarCountLabelForBox(int boxIndex)
	{
		Dictionary<string, int> strs;
		if (boxIndex >= LevelBox.campaignBoxes.Count)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Box index is out of range:    ", boxIndex));
			return string.Empty;
		}
		LevelBox item = LevelBox.campaignBoxes[boxIndex];
		List<CampaignLevel> campaignLevels = item.levels;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(item.name, out strs))
		{
			UnityEngine.Debug.LogWarning(string.Concat("ProfileController: Box not found in dictionary: ", item.name));
			strs = new Dictionary<string, int>();
		}
		int num = 0;
		for (int i = 0; i != campaignLevels.Count; i++)
		{
			string str = campaignLevels[i].sceneName;
			int num1 = 0;
			strs.TryGetValue(str, out num1);
			num += num1;
		}
		return string.Concat(num, '/', campaignLevels.Count * 3);
	}

	private void LateUpdate()
	{
		if (this.profileView != null && this.InterfaceEnabled && !HOTween.IsTweening(this.profileView.characterView.character))
		{
			float single = -120f;
			single = single * (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android ? 0.5f : 2f);
			if (Input.touchCount > 0)
			{
				if (!this._touchZone.HasValue)
				{
					this._touchZone = new Rect?(new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height));
				}
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Moved && this._touchZone.Value.Contains(touch.position))
				{
					this._idleTimeStart = Time.realtimeSinceStartup;
					Transform transforms = this.profileView.characterView.character;
					Vector3 vector3 = Vector3.up;
					Vector2 vector2 = touch.deltaPosition;
					transforms.Rotate(vector3, vector2.x * single * 0.5f * (Time.realtimeSinceStartup - this._lastTime));
				}
			}
			else if (Application.isEditor)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel") * 30f * single * (Time.realtimeSinceStartup - this._lastTime);
				this.profileView.characterView.character.Rotate(Vector3.up, axis);
				if (axis != 0f)
				{
					this._idleTimeStart = Time.realtimeSinceStartup;
				}
			}
			this._lastTime = Time.realtimeSinceStartup;
		}
	}

	public static void LoadStatisticFromKeychain()
	{
		if (!Storager.hasKey("keyGameTotalKills"))
		{
			Storager.setInt("keyGameTotalKills", 0, false);
		}
		if (!Storager.hasKey("keyGameDeath"))
		{
			Storager.setInt("keyGameDeath", 0, false);
		}
		if (!Storager.hasKey("keyGameShoot"))
		{
			Storager.setInt("keyGameShoot", 0, false);
		}
		if (!Storager.hasKey("keyGameHit"))
		{
			Storager.setInt("keyGameHit", 0, false);
		}
		ProfileController.countGameTotalKills.Value = Storager.getInt("keyGameTotalKills", false);
		ProfileController.countGameTotalDeaths.Value = Storager.getInt("keyGameDeath", false);
		ProfileController.countGameTotalShoot.Value = Storager.getInt("keyGameShoot", false);
		ProfileController.countGameTotalHit.Value = Storager.getInt("keyGameHit", false);
		ProfileController.countLikes.Value = Storager.getInt("keyCountLikes", false);
	}

	public static int MaxLevelTir(int curTir)
	{
		if (curTir < 0 || curTir >= (int)ExpController.LevelsForTiers.Length)
		{
			return -1;
		}
		if (curTir == (int)ExpController.LevelsForTiers.Length - 1)
		{
			return ExperienceController.maxLevel;
		}
		return ExpController.LevelsForTiers[curTir + 1];
	}

	public static int MinLevelTir(int curTir)
	{
		if (curTir < 0 || curTir >= (int)ExpController.LevelsForTiers.Length)
		{
			return -1;
		}
		return ExpController.LevelsForTiers[curTir];
	}

	private void OnDestroy()
	{
		FriendsController.OurInfoUpdated -= new Action(this.HandleOurInfoUpdated);
		if (this.profileView != null)
		{
			this.profileView.NicknameInput -= new EventHandler<ProfileView.InputEventArgs>(this.HandleNicknameInput);
			this.profileView.nicknameInput.onFocus -= new UIInputRilisoft.OnFocus(this.OnFocusNickname);
			this.profileView.nicknameInput.onFocusLost -= new UIInputRilisoft.OnFocusLost(this.onFocusLostNickname);
		}
	}

	private void OnEnable()
	{
		this.Refresh(true);
	}

	private void onFocusLostNickname()
	{
		if (!this._isNicknameSubmit && this.profileView != null)
		{
			this.profileView.nicknameInput.@value = ProfileController.GetPlayerNameOrDefault();
		}
	}

	private void OnFocusNickname()
	{
		this._isNicknameSubmit = false;
	}

	public static void OnGameDeath()
	{
		ref SaltedInt value = ref ProfileController.countGameTotalDeaths;
		value.Value = value.Value + 1;
	}

	public static void OnGameHit()
	{
		ref SaltedInt value = ref ProfileController.countGameTotalHit;
		value.Value = value.Value + 1;
	}

	public static void OnGameShoot()
	{
		ref SaltedInt value = ref ProfileController.countGameTotalShoot;
		value.Value = value.Value + 1;
	}

	public static void OnGameTotalKills()
	{
		ref SaltedInt value = ref ProfileController.countGameTotalKills;
		value.Value = value.Value + 1;
	}

	public static void OnGetLike()
	{
		ref SaltedInt value = ref ProfileController.countLikes;
		value.Value = value.Value + 1;
	}

	private void Refresh(bool updateWeapon = true)
	{
		object obj;
		Dictionary<string, object> item;
		if (this.profileView != null)
		{
			this.profileView.Nickname = ProfileController.GetPlayerNameOrDefault();
			if (!(FriendsController.sharedController != null) || FriendsController.sharedController.ourInfo == null || !FriendsController.sharedController.ourInfo.ContainsKey("wincount") || FriendsController.sharedController.ourInfo["wincount"] == null)
			{
				item = null;
			}
			else
			{
				item = FriendsController.sharedController.ourInfo["wincount"] as Dictionary<string, object>;
			}
			Dictionary<string, object> strs = item;
			this.profileView.CheckBtnCopy();
			ProfileView str = this.profileView;
			int num = Storager.getInt(Defs.RatingDeathmatch, false);
			str.DeathmatchWinCount = num.ToString();
			ProfileView profileView = this.profileView;
			int num1 = Storager.getInt(Defs.RatingTeamBattle, false);
			profileView.TeamBattleWinCount = num1.ToString();
			ProfileView str1 = this.profileView;
			int num2 = Storager.getInt(Defs.RatingHunger, false);
			str1.DeadlyGamesWinCount = num2.ToString();
			ProfileView profileView1 = this.profileView;
			int num3 = Storager.getInt(Defs.RatingFlag, false);
			profileView1.FlagCaptureWinCount = num3.ToString();
			ProfileView str2 = this.profileView;
			int num4 = Storager.getInt(Defs.RatingCapturePoint, false);
			str2.CapturePointWinCount = num4.ToString();
			ProfileView profileView2 = this.profileView;
			int num5 = Storager.getInt(Defs.RatingDeathmatch, false) + Storager.getInt(Defs.RatingTeamBattle, false) + Storager.getInt(Defs.RatingHunger, false) + Storager.getInt(Defs.RatingFlag, false) + Storager.getInt(Defs.RatingCapturePoint, false);
			profileView2.TotalWinCount = num5.ToString();
			this.profileView.PixelgunFriendsID = (!(FriendsController.sharedController != null) || FriendsController.sharedController.id == null ? string.Empty : FriendsController.sharedController.id);
			this.profileView.TotalWeeklyWinCount = ((strs == null || !strs.TryGetValue("weekly", out obj) ? (long)0 : (long)obj)).ToString();
			ProfileView str3 = this.profileView;
			int num6 = Storager.getInt(Defs.COOPScore, false);
			str3.CoopTimeSurvivalPointCount = num6.ToString();
			this.profileView.GameTotalKills = ProfileController.countGameTotalKills.Value.ToString();
			float single = 0f;
			single = (ProfileController.countGameTotalDeaths.Value != 0 ? (float)ProfileController.countGameTotalKills.Value / (1f * (float)ProfileController.countGameTotalDeaths.Value) : (float)ProfileController.countGameTotalKills.Value);
			single = (float)Math.Round((double)single, 2);
			this.profileView.GameKillrate = single.ToString();
			float value = 0f;
			if (ProfileController.countGameTotalHit.Value != 0)
			{
				value = (float)(100 * ProfileController.countGameTotalHit.Value) / (1f * (float)ProfileController.countGameTotalShoot.Value);
			}
			value = (float)Math.Round((double)value, 2);
			this.profileView.GameAccuracy = value.ToString();
			this.profileView.GameLikes = ProfileController.countLikes.Value.ToString();
			ProfileView profileView3 = this.profileView;
			int num7 = PlayerPrefs.GetInt(Defs.WavesSurvivedMaxS, 0);
			profileView3.WaveCountLabel = num7.ToString();
			ProfileView str4 = this.profileView;
			int num8 = PlayerPrefs.GetInt(Defs.KilledZombiesMaxSett, 0);
			str4.KilledCountLabel = num8.ToString();
			ProfileView profileView4 = this.profileView;
			int num9 = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
			profileView4.SurvivalScoreLabel = num9.ToString();
			this.profileView.Box1StarsLabel = this.InitializeStarCountLabelForBox(0);
			this.profileView.Box2StarsLabel = this.InitializeStarCountLabelForBox(1);
			this.profileView.Box3StarsLabel = this.InitializeStarCountLabelForBox(2);
			this.profileView.SecretCoinsLabel = this.InitializeSecretBonusCountLabel(VirtualCurrencyBonusType.Coin);
			this.profileView.SecretGemsLabel = this.InitializeSecretBonusCountLabel(VirtualCurrencyBonusType.Gem);
			if (updateWeapon && WeaponManager.sharedManager != null)
			{
				Weapon[] array = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToArray<Weapon>();
				if ((int)array.Length <= 0)
				{
					this.profileView.SetWeaponAndSkin("Knife", false);
				}
				else
				{
					string prefabName = null;
					if (!string.IsNullOrEmpty(this.DesiredWeaponTag))
					{
						ItemRecord byTag = ItemDb.GetByTag(this.DesiredWeaponTag);
						if (byTag != null)
						{
							prefabName = byTag.PrefabName;
						}
					}
					if (string.IsNullOrEmpty(prefabName) || !array.Any<Weapon>((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == prefabName))
					{
						System.Random random = new System.Random(Time.frameCount);
						Weapon weapon = array[random.Next((int)array.Length)];
						this.profileView.SetWeaponAndSkin(ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag, false);
					}
					else
					{
						this.profileView.SetWeaponAndSkin(this.DesiredWeaponTag, false);
					}
				}
			}
			if (Storager.getString(Defs.HatEquppedSN, false) == Defs.HatNoneEqupped)
			{
				this.profileView.RemoveHat();
			}
			else
			{
				this.profileView.UpdateHat(Storager.getString(Defs.HatEquppedSN, false));
			}
			if (Storager.getString("MaskEquippedSN", false) == "MaskNoneEquipped")
			{
				this.profileView.RemoveMask();
			}
			else
			{
				this.profileView.UpdateMask(Storager.getString("MaskEquippedSN", false));
			}
			if (Storager.getString(Defs.BootsEquppedSN, false) == Defs.BootsNoneEqupped)
			{
				this.profileView.RemoveBoots();
			}
			else
			{
				this.profileView.UpdateBoots(Storager.getString(Defs.BootsEquppedSN, false));
			}
			if (Storager.getString(Defs.ArmorNewEquppedSN, false) == Defs.ArmorNoneEqupped)
			{
				this.profileView.RemoveArmor();
			}
			else
			{
				this.profileView.UpdateArmor(Storager.getString(Defs.ArmorNewEquppedSN, false));
			}
			if (Storager.getString(Defs.CapeEquppedSN, false) == Defs.CapeNoneEqupped)
			{
				this.profileView.RemoveCape();
			}
			else
			{
				this.profileView.UpdateCape(Storager.getString(Defs.CapeEquppedSN, false));
			}
			if (FriendsController.sharedController == null)
			{
				this.profileView.SetClanLogo(string.Empty);
			}
			else
			{
				this.profileView.SetClanLogo(FriendsController.sharedController.clanLogo ?? string.Empty);
			}
		}
		this._idleTimeStart = Time.realtimeSinceStartup;
	}

	public static void ResaveStatisticToKeychain()
	{
		Storager.setInt("keyGameTotalKills", ProfileController.countGameTotalKills.Value, false);
		Storager.setInt("keyGameDeath", ProfileController.countGameTotalDeaths.Value, false);
		Storager.setInt("keyGameShoot", ProfileController.countGameTotalShoot.Value, false);
		Storager.setInt("keyGameHit", ProfileController.countGameTotalHit.Value, false);
		Storager.setInt("keyCountLikes", ProfileController.countLikes.Value, false);
	}

	private void ReturnCharacterToInitialState()
	{
		if (this.profileView == null)
		{
			UnityEngine.Debug.LogWarning("profileView == null");
			return;
		}
		int num = HOTween.Kill(this.profileView.characterView.character);
		if (num > 0 && Application.isEditor)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Tweens killed: ", num));
		}
		this._idleTimeStart = Time.realtimeSinceStartup;
		HOTween.To(this.profileView.characterView.character, 0.5f, (new TweenParms()).Prop("localRotation", new PlugQuaternion(this._initialLocalRotation)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(() => this._idleTimeStart = Time.realtimeSinceStartup));
	}

	public void SaveNamePlayer(string namePlayer)
	{
		namePlayer = FilterBadWorld.FilterString(namePlayer);
		if (string.IsNullOrEmpty(namePlayer) || namePlayer.Trim() == string.Empty)
		{
			namePlayer = ProfileController.defaultPlayerName;
			this.profileView.nicknameInput.label.text = namePlayer;
		}
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log(string.Concat("Saving new name:    ", namePlayer));
		}
		PlayerPrefs.SetString("NamePlayer", namePlayer);
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myTable != null)
		{
			NetworkStartTable component = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>();
			if (component != null)
			{
				component.SetNewNick();
			}
		}
		this._dirty = true;
		this._isNicknameSubmit = true;
	}

	public void SetStaticticTab(StatisticHUD.TypeOpenTab tab)
	{
		StatisticHUD componentInChildren = base.GetComponentInChildren<StatisticHUD>(true);
		if (componentInChildren != null)
		{
			componentInChildren.OpenTab(tab);
		}
	}

	public void ShowInterface(params Action[] exitCallbacks)
	{
		FriendsController.sharedController.GetOurWins();
		this.InterfaceEnabled = true;
		this._exitCallbacks = exitCallbacks ?? new Action[0];
	}

	private void Start()
	{
		this.BackRequested += new EventHandler(this.HandleBackRequest);
		if (this.profileView != null)
		{
			this.profileView.nicknameInput.onFocus += new UIInputRilisoft.OnFocus(this.OnFocusNickname);
			this.profileView.nicknameInput.onFocusLost += new UIInputRilisoft.OnFocusLost(this.onFocusLostNickname);
		}
		if (this.profileView != null)
		{
			this.profileView.Nickname = ProfileController.GetPlayerNameOrDefault();
			this.profileView.NicknameInput += new EventHandler<ProfileView.InputEventArgs>(this.HandleNicknameInput);
			this._initialLocalRotation = this.profileView.characterView.character.localRotation;
			switch (BuildSettings.BuildTargetPlatform)
			{
				case RuntimePlatform.IPhonePlayer:
				{
					this.UpdateButton(this.profileView.achievementsButton, "gamecntr");
					this.UpdateButton(this.profileView.leaderboardsButton, "gamecntr");
					break;
				}
				case RuntimePlatform.PS3:
				case RuntimePlatform.XBOX360:
				{
					this.profileView.achievementsButton.gameObject.SetActive(false);
					break;
				}
				case RuntimePlatform.Android:
				{
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
					{
						this.UpdateButton(this.profileView.achievementsButton, "google");
						this.UpdateButton(this.profileView.leaderboardsButton, "google");
					}
					else if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
					{
						this.profileView.achievementsButton.gameObject.SetActive(false);
					}
					else
					{
						this.UpdateButton(this.profileView.achievementsButton, "amazon");
						this.UpdateButton(this.profileView.leaderboardsButton, "amazon");
					}
					break;
				}
				default:
				{
					goto case RuntimePlatform.XBOX360;
				}
			}
		}
		this.InterfaceEnabled = false;
		FriendsController.OurInfoUpdated += new Action(this.HandleOurInfoUpdated);
	}

	private void Update()
	{
		EventHandler escapePressed = this.EscapePressed;
		if (this._escapePressed && escapePressed != null)
		{
			escapePressed(this, EventArgs.Empty);
			this._escapePressed = false;
		}
		if (Time.realtimeSinceStartup - this._idleTimeStart > ShopNGUIController.IdleTimeoutPers)
		{
			this.ReturnCharacterToInitialState();
		}
	}

	private void UpdateButton(UIButton button, string spriteName)
	{
		if (button == null)
		{
			return;
		}
		button.normalSprite = spriteName;
		button.pressedSprite = string.Concat(spriteName, "_n");
		button.hoverSprite = spriteName;
		button.disabledSprite = spriteName;
	}

	public event EventHandler BackRequested
	{
		add
		{
			if (this.profileView != null)
			{
				this.profileView.BackButtonPressed += value;
			}
			this.EscapePressed += value;
		}
		remove
		{
			if (this.profileView != null)
			{
				this.profileView.BackButtonPressed -= value;
			}
			this.EscapePressed -= value;
		}
	}

	private event EventHandler EscapePressed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.EscapePressed += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.EscapePressed -= value;
		}
	}

	public event EventHandler<ProfileView.InputEventArgs> NicknameInput
	{
		add
		{
			if (this.profileView != null)
			{
				this.profileView.NicknameInput += value;
			}
		}
		remove
		{
			if (this.profileView != null)
			{
				this.profileView.NicknameInput -= value;
			}
		}
	}
}