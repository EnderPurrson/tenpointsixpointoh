using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameController
{
	public static bool HasSurvivalRecord;

	public static bool LeftHanded;

	public static bool switchingWeaponSwipe;

	public static bool ShowRec;

	public static List<int> survScoreThresh;

	public static int curThr;

	public static int thrStep;

	public static Font fontHolder;

	public static int EditingLogo;

	public static string TempClanName;

	public static Texture2D LogoToEdit;

	public static List<Texture2D> Logos;

	public readonly static int NumOfLevels;

	private static int _currentLevel;

	private static int _allLevelsCompleted;

	public static bool showTableMyPlayer;

	public static bool imDeadInHungerGame;

	public static bool isFullVersion;

	public static Vector3 posMyPlayer;

	public static Quaternion rotMyPlayer;

	public static float healthMyPlayer;

	public static bool is60FPSEnableInit;

	public static bool _is60FPSEnable;

	public static int numOfCompletedLevels;

	public static int totalNumOfCompletedLevels;

	public static int countKillsBlue;

	public static int countKillsRed;

	public static int EditingCape;

	public static bool EditedCapeSaved;

	private static int? _enemiesToKillOverride;

	private static SaltedInt _saltedScore;

	private static SaltedInt _saltedCountKills;

	private static int _countDaySessionInCurrentVersion;

	public static int coinsBase;

	public static int coinsBaseAdding;

	public static int levelsToGetCoins;

	public readonly static string AppVersion;

	public static int AllLevelsCompleted
	{
		get
		{
			return GlobalGameController._allLevelsCompleted;
		}
		set
		{
			GlobalGameController._allLevelsCompleted = value;
		}
	}

	public static float armorMyPlayer
	{
		get;
		set;
	}

	public static int CountDaySessionInCurrentVersion
	{
		get
		{
			if (GlobalGameController._countDaySessionInCurrentVersion == -1)
			{
				GlobalGameController._countDaySessionInCurrentVersion = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1) - PlayerPrefs.GetInt("countSessionDayOnStartCorrentVersion", 1);
			}
			return GlobalGameController._countDaySessionInCurrentVersion;
		}
		set
		{
			GlobalGameController._countDaySessionInCurrentVersion = value;
		}
	}

	internal static int CountKills
	{
		get
		{
			return GlobalGameController._saltedCountKills.Value;
		}
		set
		{
			GlobalGameController._saltedCountKills.Value = value;
		}
	}

	public static int currentLevel
	{
		get
		{
			return GlobalGameController._currentLevel;
		}
		set
		{
			GlobalGameController._currentLevel = value;
		}
	}

	public static int EnemiesToKill
	{
		get
		{
			if (!TrainingController.TrainingCompleted || Defs.TrainingSceneName.Equals(SceneManager.GetActiveScene().name, StringComparison.OrdinalIgnoreCase))
			{
				return 3;
			}
			if (GlobalGameController._enemiesToKillOverride.HasValue)
			{
				return GlobalGameController._enemiesToKillOverride.Value;
			}
			if (!Defs.IsSurvival)
			{
				return ZombieCreator.GetCountMobsForLevel();
			}
			return 35;
		}
		set
		{
			GlobalGameController._enemiesToKillOverride = new int?(value);
		}
	}

	public static bool is60FPSEnable
	{
		get
		{
			if (!GlobalGameController.is60FPSEnableInit)
			{
				GlobalGameController._is60FPSEnable = PlayerPrefs.GetInt("fps60Enable", (!Device.isPixelGunLow ? 1 : 0)) == 1;
				GlobalGameController.is60FPSEnableInit = true;
			}
			return GlobalGameController._is60FPSEnable;
		}
		set
		{
			GlobalGameController._is60FPSEnable = value;
			PlayerPrefs.SetInt("fps60Enable", (!GlobalGameController._is60FPSEnable ? 0 : 1));
			GlobalGameController.is60FPSEnableInit = true;
			Application.targetFrameRate = (!GlobalGameController._is60FPSEnable ? 30 : 60);
		}
	}

	public static string MultiplayerProtocolVersion
	{
		get
		{
			return "10.6.0";
		}
	}

	internal static bool NewVersionAvailable
	{
		get;
		set;
	}

	internal static int Score
	{
		get
		{
			return GlobalGameController._saltedScore.Value;
		}
		set
		{
			GlobalGameController._saltedScore.Value = value;
		}
	}

	public static int SimultaneousEnemiesOnLevelConstraint
	{
		get
		{
			return 20;
		}
	}

	public static int ZombiesInWave
	{
		get
		{
			return 4;
		}
	}

	static GlobalGameController()
	{
		GlobalGameController.LeftHanded = true;
		GlobalGameController.switchingWeaponSwipe = false;
		GlobalGameController.ShowRec = true;
		GlobalGameController.survScoreThresh = new List<int>();
		GlobalGameController.thrStep = 10000;
		GlobalGameController.fontHolder = null;
		GlobalGameController.EditingLogo = 0;
		GlobalGameController.NumOfLevels = 11;
		GlobalGameController._currentLevel = -1;
		GlobalGameController._allLevelsCompleted = 0;
		GlobalGameController.showTableMyPlayer = false;
		GlobalGameController.imDeadInHungerGame = false;
		GlobalGameController.isFullVersion = true;
		GlobalGameController.is60FPSEnableInit = false;
		GlobalGameController.numOfCompletedLevels = 0;
		GlobalGameController.totalNumOfCompletedLevels = 0;
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		GlobalGameController.EditingCape = 0;
		GlobalGameController.EditedCapeSaved = false;
		GlobalGameController._saltedScore = new SaltedInt(233495534);
		GlobalGameController._saltedCountKills = new SaltedInt(233495534);
		GlobalGameController._countDaySessionInCurrentVersion = -1;
		GlobalGameController.coinsBase = 1;
		GlobalGameController.coinsBaseAdding = 0;
		GlobalGameController.levelsToGetCoins = 1;
		GlobalGameController.AppVersion = "10.6.1";
	}

	public GlobalGameController()
	{
	}

	public static void GoInBattle()
	{
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		FlurryPluginWrapper.LogDeathmatchModePress();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ConnectScene";
		FlurryPluginWrapper.LogEvent("Launch_Multiplayer");
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	public static void ResetParameters()
	{
		GlobalGameController.AllLevelsCompleted = 0;
		GlobalGameController.numOfCompletedLevels = -1;
		GlobalGameController.totalNumOfCompletedLevels = -1;
	}

	public static void SetMultiMode()
	{
		Defs.isMulti = true;
		Defs.isCOOP = false;
		Defs.isHunger = false;
		Defs.isCompany = false;
		Defs.isFlag = false;
		Defs.IsSurvival = false;
		Defs.isCapturePoints = false;
	}

	private static void Swap(IList<int> list, int indexA, int indexB)
	{
		int item = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = item;
	}
}