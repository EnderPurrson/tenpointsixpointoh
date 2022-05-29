using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public sealed class ExperienceController : MonoBehaviour
{
	public static int[] MaxExpLevelsDefault;

	public static int[] MaxExpLevels;

	public readonly static float[] HealthByLevel;

	public bool isMenu;

	public bool isConnectScene;

	public int currentLevelForEditor;

	public static int maxLevel;

	public int[,] limitsLeveling = new int[,] { { 1, 6 }, { 7, 11 }, { 12, 16 }, { 17, 21 }, { 22, 26 }, { 27, 31 } };

	public static int[,] accessByLevels;

	public Texture2D[] marks;

	private SaltedInt currentExperience = new SaltedInt(12512238, 0);

	private static int[] _addCoinsFromLevelsDefault;

	private static int[] _addCoinsFromLevels;

	private static int[] _addGemsFromLevelsDefault;

	private static int[] _addGemsFromLevels;

	private static bool _storagerKeysInitialized;

	private bool _isShowRanks = true;

	public bool isShowNextPlashka;

	public Vector2 posRanks = Vector2.zero;

	private int oldCurrentExperience;

	public int oldCurrentLevel;

	public bool isShowAdd;

	public AudioClip exp_1;

	public AudioClip exp_2;

	public AudioClip exp_3;

	public AudioClip Tierup;

	public static ExperienceController sharedController;

	public static int[] addCoinsFromLevels
	{
		get
		{
			return ExperienceController._addCoinsFromLevels;
		}
	}

	public static int[] addGemsFromLevels
	{
		get
		{
			return ExperienceController._addGemsFromLevels;
		}
	}

	public int AddHealthOnCurLevel
	{
		get
		{
			int num = this.currentLevel;
			if ((int)ExperienceController.HealthByLevel.Length <= num || num <= 0)
			{
				return 0;
			}
			return (int)(ExperienceController.HealthByLevel[num] - ExperienceController.HealthByLevel[num - 1]);
		}
	}

	public int CurrentExperience
	{
		get
		{
			return this.currentExperience.Value;
		}
	}

	public int currentLevel
	{
		get
		{
			return this.currentLevelForEditor;
		}
		private set
		{
			bool flag = false;
			if (this.currentLevelForEditor != value)
			{
				flag = true;
			}
			this.currentLevelForEditor = value;
			if (value >= 4)
			{
				ReviewController.CheckActiveReview();
			}
			if (flag && ExperienceController.onLevelChange != null)
			{
				ExperienceController.onLevelChange();
			}
		}
	}

	public bool isShowRanks
	{
		get
		{
			return this._isShowRanks;
		}
		set
		{
			this._isShowRanks = value;
			if (ExpController.Instance != null)
			{
				ExpController.Instance.InterfaceEnabled = value;
			}
		}
	}

	static ExperienceController()
	{
		ExperienceController.MaxExpLevelsDefault = new int[] { 0, 15, 35, 50, 90, 100, 110, 115, 120, 125, 130, 135, 140, 150, 160, 170, 180, 200, 220, 250, 290, 340, 400, 470, 550, 640, 740, 850, 970, 1100, 1240, 100000 };
		ExperienceController.MaxExpLevels = ExperienceController.InitMaxLevelMass(ExperienceController.MaxExpLevelsDefault);
		ExperienceController.HealthByLevel = new float[] { 0f, 9f, 10f, 10f, 11f, 11f, 12f, 13f, 13f, 14f, 14f, 15f, 16f, 16f, 17f, 17f, 18f, 19f, 19f, 20f, 20f, 21f, 22f, 22f, 23f, 23f, 24f, 25f, 25f, 26f, 26f, 27f };
		ExperienceController.maxLevel = 31;
		ExperienceController.accessByLevels = new int[ExperienceController.maxLevel, ExperienceController.maxLevel];
		ExperienceController._addCoinsFromLevelsDefault = new int[] { 0, 5, 10, 15, 20, 25, 25, 25, 30, 30, 30, 35, 35, 40, 40, 40, 45, 45, 50, 50, 50, 55, 55, 60, 60, 60, 65, 65, 70, 70, 70, 0 };
		ExperienceController._addCoinsFromLevels = ExperienceController.InitAddCoinsFromLevels(ExperienceController._addCoinsFromLevelsDefault);
		ExperienceController._addGemsFromLevelsDefault = new int[] { 0, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 0 };
		ExperienceController._addGemsFromLevels = ExperienceController.InitAddGemsFromLevels(ExperienceController._addGemsFromLevelsDefault);
		ExperienceController._storagerKeysInitialized = false;
	}

	public ExperienceController()
	{
		this.currentLevel = 1;
	}

	private void AddCurrenciesForLevelUP()
	{
		int num = ExperienceController.addGemsFromLevels[this.currentLevel - 1];
		BankController.canShowIndication = false;
		BankController.AddGems(num, false, AnalyticsConstants.AccrualType.Earned);
		if (this.currentLevel == 2 && Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
		{
			BankController.AddGems(Defs.abTestBalansStartCapitalGems, false, AnalyticsConstants.AccrualType.Earned);
		}
		FlurryEvents.LogGemsGained(FlurryEvents.GetPlayingMode(), num);
		base.StartCoroutine(BankController.WaitForIndicationGems(true));
		int num1 = ExperienceController.addCoinsFromLevels[this.currentLevel - 1];
		BankController.AddCoins(num1, false, AnalyticsConstants.AccrualType.Earned);
		if (this.currentLevel == 2 && Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
		{
			BankController.AddCoins(Defs.abTestBalansStartCapitalCoins, false, AnalyticsConstants.AccrualType.Earned);
		}
		FlurryEvents.LogCoinsGained(FlurryEvents.GetPlayingMode(), num1);
		base.StartCoroutine(BankController.WaitForIndicationGems(false));
	}

	public void addExperience(int experience)
	{
		object obj;
		if (this.currentLevel == ExperienceController.maxLevel)
		{
			return;
		}
		this.oldCurrentLevel = this.currentLevel;
		this.oldCurrentExperience = this.currentExperience.Value;
		if (this.currentLevel < ExperienceController.maxLevel && experience >= ExperienceController.MaxExpLevels[this.currentLevel] - this.currentExperience.Value + ExperienceController.MaxExpLevels[this.currentLevel + 1])
		{
			experience = ExperienceController.MaxExpLevels[this.currentLevel + 1] + ExperienceController.MaxExpLevels[this.currentLevel] - this.currentExperience.Value - 5;
		}
		string str = string.Concat("Statistics.ExpInMode.Level", ExperienceController.sharedController.currentLevel);
		if (PlayerPrefs.HasKey(str) && Initializer.lastGameMode != -1)
		{
			string str1 = Initializer.lastGameMode.ToString();
			string str2 = PlayerPrefs.GetString(str, "{}");
			try
			{
				Dictionary<string, object> strs = Json.Deserialize(str2) as Dictionary<string, object> ?? new Dictionary<string, object>();
				if (!strs.TryGetValue(str1, out obj))
				{
					strs.Add(str1, experience);
				}
				else
				{
					int num = Convert.ToInt32(obj) + experience;
					strs[str1] = num;
				}
				PlayerPrefs.SetString(str, Json.Serialize(strs));
			}
			catch (OverflowException overflowException1)
			{
				OverflowException overflowException = overflowException1;
				UnityEngine.Debug.LogError(string.Concat("Cannot deserialize exp-in-mode: ", str2));
				UnityEngine.Debug.LogException(overflowException);
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				UnityEngine.Debug.LogError(string.Concat("Unknown exception: ", str2));
				UnityEngine.Debug.LogException(exception);
			}
		}
		this.currentExperience = this.currentExperience.Value + experience;
		Storager.setInt("currentExperience", this.currentExperience.Value, false);
		if (this.currentLevel < ExperienceController.maxLevel && this.currentExperience.Value >= ExperienceController.MaxExpLevels[this.currentLevel])
		{
			DateTime utcNow = DateTime.UtcNow;
			string str3 = string.Concat("Statistics.TimeInRank.Level", this.currentLevel + 1);
			PlayerPrefs.SetString(str3, utcNow.ToString("s"));
			string str4 = string.Concat("Statistics.MatchCount.Level", ExperienceController.sharedController.currentLevel);
			PlayerPrefs.GetInt(str4, 0);
			string str5 = string.Concat("Statistics.WinCount.Level", ExperienceController.sharedController.currentLevel);
			PlayerPrefs.GetInt(str5, 0);
			Dictionary<string, string> strs1 = new Dictionary<string, string>()
			{
				{ "af_level", this.currentLevel.ToString() }
			};
			FlurryPluginWrapper.LogEventToAppsFlyer("af_level_achieved", strs1);
			this.currentExperience = this.currentExperience.Value - ExperienceController.MaxExpLevels[this.currentLevel];
			ExperienceController experienceController = this;
			experienceController.currentLevel = experienceController.currentLevel + 1;
			if (this.currentLevel == 3)
			{
				AnalyticsStuff.TrySendOnceToAppsFlyer("levelup_3");
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.currentLevel > 1)
			{
				if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1)
				{
					Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 1, false);
					if (HintController.instance != null)
					{
						HintController.instance.ShowHintByName("shop_remove_novice_armor", 2.5f);
					}
				}
				TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.FirstMatchCompleted;
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Finished, true);
				if (!Storager.hasKey("Analytics:tutorial_levelup"))
				{
					Storager.setString("Analytics:tutorial_levelup", "{}", false);
					AnalyticsFacade.SendCustomEventToAppsFlyer("tutorial_levelup", new Dictionary<string, string>());
					Storager.setString("Analytics:af_tutorial_completion", "{}", false);
					AnalyticsFacade.SendCustomEventToAppsFlyer("af_tutorial_completion", new Dictionary<string, string>());
				}
				try
				{
					if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null)
					{
						IEnumerable<Weapon> weapons = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>();
						if (weapons.FirstOrDefault<Weapon>((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 3) == null)
						{
							WeaponManager.sharedManager.EquipWeapon(WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().First<Weapon>((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.SimpleFlamethrower_WN), true, false);
						}
						if (weapons.FirstOrDefault<Weapon>((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 5) == null)
						{
							WeaponManager.sharedManager.EquipWeapon(WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().First<Weapon>((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.Rocketnitza_WN), true, false);
						}
					}
				}
				catch (Exception exception2)
				{
					UnityEngine.Debug.LogError(string.Concat("Exception in gequipping flamethrower and rocketniza: ", exception2));
				}
			}
			Storager.setInt(string.Concat("currentLevel", this.currentLevel), 1, true);
			Storager.setInt("currentExperience", this.currentExperience.Value, false);
			ShopNGUIController.SynchronizeAndroidPurchases(string.Concat("Current level: ", this.currentLevel));
			BankController.GiveInitialNumOfCoins();
			this.AddCurrenciesForLevelUP();
			FriendsController.sharedController.rank = this.currentLevel;
			FriendsController.sharedController.SendOurData(false);
			FriendsController.sharedController.UpdatePopularityMaps();
			AnalyticsFacade.LevelUp(this.currentLevel);
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.exp_1);
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.AddExperience(this.oldCurrentLevel, this.oldCurrentExperience, experience, this.exp_2, this.exp_3, this.Tierup);
		}
	}

	private void Awake()
	{
		ExperienceController.sharedController = this;
	}

	private void DoOnGUI()
	{
	}

	public static int GetCurrentLevel()
	{
		int num = 1;
		for (int i = 1; i <= ExperienceController.maxLevel; i++)
		{
			string str = string.Concat("currentLevel", i);
			if (Storager.getInt(str, true) == 1)
			{
				num = i;
				Storager.setInt(str, 1, true);
			}
		}
		return num;
	}

	public static int GetCurrentLevelWithUpdateCorrection()
	{
		ExperienceController.InitializeStoragerKeysIfNeeded();
		int currentLevel = ExperienceController.GetCurrentLevel();
		if (currentLevel < ExperienceController.maxLevel && Storager.getInt("currentExperience", false) >= ExperienceController.MaxExpLevels[currentLevel])
		{
			currentLevel++;
		}
		return currentLevel;
	}

	private void HideNextPlashka()
	{
		this.isShowNextPlashka = false;
		this.isShowAdd = false;
	}

	public static int[] InitAddCoinsFromLevels(int[] _mass)
	{
		int[] numArray = new int[(int)_mass.Length];
		Array.Copy(_mass, numArray, (int)_mass.Length);
		return numArray;
	}

	public static int[] InitAddGemsFromLevels(int[] _mass)
	{
		int[] numArray = new int[(int)_mass.Length];
		Array.Copy(_mass, numArray, (int)_mass.Length);
		return numArray;
	}

	[DebuggerHidden]
	public IEnumerable<float> InitController()
	{
		ExperienceController.u003cInitControlleru003ec__Iterator24 variable = null;
		return variable;
	}

	private static void InitializeStoragerKeysIfNeeded()
	{
		if (ExperienceController._storagerKeysInitialized)
		{
			return;
		}
		if (!Storager.hasKey("currentLevel1"))
		{
			Storager.setInt("currentLevel1", 1, true);
		}
		ExperienceController._storagerKeysInitialized = true;
	}

	public static int[] InitMaxLevelMass(int[] _mass)
	{
		int[] numArray = new int[(int)_mass.Length];
		Array.Copy(_mass, numArray, (int)_mass.Length);
		return numArray;
	}

	public void Refresh()
	{
		this.currentExperience = Storager.getInt("currentExperience", false);
		this.currentLevel = ExperienceController.GetCurrentLevel();
	}

	public static void ResetLevelingOnDefault()
	{
		ExperienceController.MaxExpLevels = ExperienceController.InitMaxLevelMass(ExperienceController.MaxExpLevelsDefault);
		ExperienceController._addCoinsFromLevels = ExperienceController.InitAddCoinsFromLevels(ExperienceController._addCoinsFromLevelsDefault);
		ExperienceController._addGemsFromLevels = ExperienceController.InitAddGemsFromLevels(ExperienceController._addGemsFromLevelsDefault);
	}

	public static void RewriteLevelingParametersForLevel(int _level, int _exp, int _coins, int _gems)
	{
		ExperienceController.MaxExpLevels[_level] = _exp;
		ExperienceController._addCoinsFromLevels[_level] = _coins;
		ExperienceController._addGemsFromLevels[_level] = _gems;
	}

	public static void SendAnalyticsForLevelsFromCloud(int levelBefore)
	{
		if (ExperienceController.sharedController == null)
		{
			UnityEngine.Debug.LogError("SendAnalyticsForLevelsFromCloud ExperienceController.sharedController == null");
			return;
		}
		for (int i = levelBefore + 1; i <= ExperienceController.sharedController.currentLevel; i++)
		{
			AnalyticsFacade.LevelUp(i);
		}
	}

	public void SetCurrentExperience(int _exp)
	{
		this.currentExperience = _exp;
		Storager.setInt("currentExperience", _exp, false);
		UnityEngine.Debug.Log(this.currentExperience.Value);
	}

	public static void SetEnable(bool enable)
	{
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		ExperienceController.sharedController.isShowRanks = enable;
	}

	public static event Action onLevelChange;
}