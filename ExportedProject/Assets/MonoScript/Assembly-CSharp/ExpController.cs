using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ExpController : MonoBehaviour
{
	public const int MaxLobbyLevel = 3;

	public ExpView experienceView;

	private bool starterBannerShowed;

	public readonly static int[] LevelsForTiers;

	private static ExpController _instance;

	private bool _sameSceneIndicator;

	private bool _inAddingState;

	private int Experience
	{
		set
		{
			if (this.experienceView == null)
			{
				return;
			}
			if (ExperienceController.sharedController == null)
			{
				return;
			}
			int maxExpLevels = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
			int num = Mathf.Clamp(value, 0, maxExpLevels);
			this.experienceView.ExperienceLabel = ExpController.FormatExperienceLabel(num, maxExpLevels);
			float single = (float)num / (float)maxExpLevels;
			if (ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel)
			{
				single = 1f;
			}
			this.experienceView.CurrentProgress = single;
			this.experienceView.OldProgress = single;
		}
	}

	public bool InAddingState
	{
		get
		{
			return this._inAddingState;
		}
	}

	public static ExpController Instance
	{
		get
		{
			return ExpController._instance;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			return (!(this.experienceView != null) || !(this.experienceView.interfaceHolder != null) ? false : this.experienceView.interfaceHolder.gameObject.activeInHierarchy);
		}
		set
		{
			this.SetInterfaceEnabled(value);
		}
	}

	public bool IsLevelUpShown
	{
		get;
		private set;
	}

	public static int LobbyLevel
	{
		get
		{
			return 3;
		}
	}

	public int OurTier
	{
		get
		{
			if (ExperienceController.sharedController == null)
			{
				return 0;
			}
			return ExpController.TierForLevel(ExperienceController.sharedController.currentLevel);
		}
	}

	public int Rank
	{
		set
		{
			if (this.experienceView == null)
			{
				return;
			}
			int num = Mathf.Clamp(value, 1, ExperienceController.maxLevel);
			this.experienceView.RankSprite = num;
		}
	}

	public bool WaitingForLevelUpView
	{
		get;
		private set;
	}

	static ExpController()
	{
		ExpController.LevelsForTiers = new int[] { 1, 7, 12, 17, 22, 27 };
	}

	public ExpController()
	{
	}

	public void AddExperience(int oldLevel, int oldExperience, int addend, AudioClip exp2, AudioClip levelup, AudioClip tierup = null)
	{
		if (this.experienceView == null)
		{
			return;
		}
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		int num = oldExperience + addend;
		int maxExpLevels = ExperienceController.MaxExpLevels[oldLevel];
		if (num >= maxExpLevels)
		{
			float single = 1f;
			this.experienceView.CurrentProgress = single;
			AudioClip audioClip = levelup;
			if (tierup != null && Array.IndexOf<int>(ExpController.LevelsForTiers, oldLevel + 1) > 0)
			{
				audioClip = tierup;
			}
			if (oldLevel < ExperienceController.maxLevel - 1)
			{
				this.experienceView.StartBlinkingWithNewProgress();
				int num1 = oldLevel + 1;
				int num2 = num - maxExpLevels;
				base.StartCoroutine(this.WaitAndUpdateExperience(num1, num2, ExperienceController.MaxExpLevels[num1], true, audioClip));
			}
			else if (oldLevel != ExperienceController.maxLevel - 1)
			{
				if (ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel)
				{
					single = 1f;
				}
				this.experienceView.OldProgress = single;
				this.experienceView.StartBlinkingWithNewProgress();
				int num3 = ExperienceController.maxLevel;
				int maxExpLevels1 = ExperienceController.MaxExpLevels[num3];
				base.StartCoroutine(this.WaitAndUpdateExperience(num3, maxExpLevels1, ExperienceController.MaxExpLevels[num3], false, exp2));
			}
			else
			{
				this.experienceView.StartBlinkingWithNewProgress();
				int num4 = oldLevel + 1;
				int maxExpLevels2 = ExperienceController.MaxExpLevels[num4];
				base.StartCoroutine(this.WaitAndUpdateExperience(num4, maxExpLevels2, ExperienceController.MaxExpLevels[num4], true, audioClip));
			}
		}
		else
		{
			float percentage = ExpController.GetPercentage(num);
			this.experienceView.CurrentProgress = percentage;
			this.experienceView.StartBlinkingWithNewProgress();
			this.experienceView.WaitAndUpdateOldProgress(exp2);
			this.experienceView.ExperienceLabel = ExpController.FormatExperienceLabel(num, maxExpLevels);
			if (this.experienceView.currentProgress != null && !this.experienceView.currentProgress.gameObject.activeInHierarchy)
			{
				this.experienceView.OldProgress = percentage;
			}
		}
		base.StartCoroutine(this.SetAddingState());
	}

	private void Awake()
	{
		if (!SceneLoader.ActiveSceneName.EndsWith("Workbench"))
		{
			this.InterfaceEnabled = false;
		}
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLabels));
		Singleton<SceneLoader>.Instance.OnSceneLoading += new Action<SceneLoadInfo>((SceneLoadInfo sli) => {
			if (this.experienceView != null)
			{
				LevelUpWithOffers currentVisiblePanel = this.experienceView.CurrentVisiblePanel;
				if (currentVisiblePanel != null)
				{
					ExpController.HideTierPanel(currentVisiblePanel.gameObject);
				}
			}
			this.IsLevelUpShown = false;
		});
	}

	public static int CurrentFilterMap()
	{
		return (!Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? 0 : Defs.filterMaps[Application.loadedLevelName]);
	}

	public static string ExpToString()
	{
		int num = ExperienceController.sharedController.currentLevel;
		int currentExperience = ExperienceController.sharedController.CurrentExperience;
		return ExpController.FormatExperienceLabel(currentExperience, ExperienceController.MaxExpLevels[num]);
	}

	private static string FormatExperienceLabel(int xp, int bound)
	{
		string str = LocalizationStore.Get("Key_0928");
		string str1 = string.Format("{0} {1}/{2}", LocalizationStore.Get("Key_0204"), xp, bound);
		return (xp == bound || ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel ? str : str1);
	}

	private static string FormatLevelLabel(int level)
	{
		return string.Format("{0} {1}", LocalizationStore.Key_0226, level);
	}

	public static int GetOurTier()
	{
		return ExpController.TierForLevel(ExperienceController.GetCurrentLevelWithUpdateCorrection());
	}

	private static float GetPercentage(int experience)
	{
		if (ExperienceController.sharedController == null)
		{
			return 0f;
		}
		int maxExpLevels = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
		int num = Mathf.Clamp(experience, 0, maxExpLevels);
		return (float)num / (float)maxExpLevels;
	}

	public void HandleContinueButton(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? 0 : Defs.filterMaps[Application.loadedLevelName]));
		}
		if (this.starterBannerShowed || ExperienceController.sharedController.currentLevel != 2 || Defs.abTestBalansCohort != Defs.ABTestCohortsType.B)
		{
			this.starterBannerShowed = false;
			ExpController.HideTierPanel(tierPanel);
		}
		else
		{
			this.starterBannerShowed = true;
			this.experienceView.ToBonus(Defs.abTestBalansStartCapitalGems, Defs.abTestBalansStartCapitalCoins);
		}
	}

	public void HandleNewAvailableItem(GameObject tierPanel, NewAvailableItemInShop itemInfo)
	{
		if (Defs.isHunger)
		{
			return;
		}
		if (ExpController.CurrentFilterMap() != 0)
		{
			int[] itemFilterMap = new int[0];
			bool byTag = true;
			if (itemInfo != null && itemInfo._tag != null)
			{
				byTag = ItemDb.GetByTag(itemInfo._tag) != null;
				if (byTag)
				{
					itemFilterMap = ItemDb.GetItemFilterMap(itemInfo._tag);
				}
			}
			if (byTag && !itemFilterMap.Contains(ExpController.CurrentFilterMap()))
			{
				this.HandleShopButtonFromTierPanel(tierPanel);
				return;
			}
		}
		if (ShopNGUIController.sharedShop != null)
		{
			if (itemInfo != null)
			{
				string str = itemInfo._tag ?? string.Empty;
				UnityEngine.Debug.Log(string.Concat(new object[] { "Available item:   ", str, "    ", itemInfo.category }));
				base.StartCoroutine(this.HandleShopButtonFromNewAvailableItemCoroutine(tierPanel, str, itemInfo.category));
				return;
			}
			UnityEngine.Debug.LogWarning("itemInfo == null");
		}
		else
		{
			UnityEngine.Debug.LogWarning("ShopNGUIController.sharedShop == null");
		}
		base.StartCoroutine(this.HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	[DebuggerHidden]
	private IEnumerator HandleShopButtonFromNewAvailableItemCoroutine(GameObject tierPanel, string itemTag, ShopNGUIController.CategoryNames category)
	{
		ExpController.u003cHandleShopButtonFromNewAvailableItemCoroutineu003ec__Iterator134 variable = null;
		return variable;
	}

	public void HandleShopButtonFromTierPanel(GameObject tierPanel)
	{
		base.StartCoroutine(this.HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	[DebuggerHidden]
	private IEnumerator HandleShopButtonFromTierPanelCoroutine(GameObject tierPanel)
	{
		ExpController.u003cHandleShopButtonFromTierPanelCoroutineu003ec__Iterator133 variable = null;
		return variable;
	}

	public static void HideTierPanel(GameObject tierPanel)
	{
		if (tierPanel != null)
		{
			tierPanel.SetActive(false);
			if (ExpController.Instance != null)
			{
				ExpController.Instance.IsLevelUpShown = false;
			}
			MainMenuController.SetInputEnabled(true);
			LevelCompleteScript.SetInputEnabled(true);
		}
	}

	public bool IsRenderedWithCamera(Camera c)
	{
		return (!(this.experienceView != null) || !(this.experienceView.experienceCamera != null) ? false : this.experienceView.experienceCamera == c);
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLabels));
		ExpController._instance = null;
	}

	private void OnEnable()
	{
		if (ExperienceController.sharedController != null)
		{
			this.Rank = ExperienceController.sharedController.currentLevel;
			this.Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	private void OnLevelWasLoaded(int index)
	{
		this._sameSceneIndicator = false;
	}

	public static int OurTierForAnyPlace()
	{
		return (ExpController.Instance == null ? ExpController.GetOurTier() : ExpController.Instance.OurTier);
	}

	public static float progressExpInPer()
	{
		return ExpController.GetPercentage(ExperienceController.sharedController.CurrentExperience);
	}

	public void Refresh()
	{
		if (ExperienceController.sharedController != null)
		{
			this.Rank = ExperienceController.sharedController.currentLevel;
			this.Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	[DebuggerHidden]
	private IEnumerator SetAddingState()
	{
		ExpController.u003cSetAddingStateu003ec__Iterator136 variable = null;
		return variable;
	}

	private void SetInterfaceEnabled(bool value)
	{
		bool flag;
		if (value && this.experienceView != null)
		{
			if (Application.loadedLevelName != Defs.MainMenuScene || ShopNGUIController.GuiActive || MainMenuController.sharedController != null && MainMenuController.sharedController.singleModePanel != null && MainMenuController.sharedController.singleModePanel.activeInHierarchy)
			{
				flag = true;
			}
			else
			{
				flag = (ProfileController.Instance == null ? false : ProfileController.Instance.InterfaceEnabled);
			}
			bool flag1 = flag;
			if (this.experienceView.rankIndicatorContainer.activeSelf != flag1)
			{
				this.experienceView.rankIndicatorContainer.SetActive(flag1);
			}
		}
		if (this.InterfaceEnabled == value)
		{
			return;
		}
		if (this.experienceView != null && this.experienceView.interfaceHolder != null)
		{
			if (!value)
			{
				this.experienceView.StopAnimation();
			}
			if (ExperienceController.sharedController != null)
			{
				this.Rank = ExperienceController.sharedController.currentLevel;
				this.Experience = ExperienceController.sharedController.CurrentExperience;
			}
			this.experienceView.interfaceHolder.gameObject.SetActive(value);
			if (value && this.experienceView.experienceCamera != null)
			{
				AudioListener component = this.experienceView.experienceCamera.GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}

	public static void ShowTierPanel(GameObject tierPanel)
	{
		if (tierPanel != null)
		{
			tierPanel.SetActive(true);
			if (ExpController.Instance != null)
			{
				ExpController.Instance.IsLevelUpShown = true;
				Action action = ExpController.LevelUpShown;
				if (action != null)
				{
					action();
				}
			}
			MainMenuController.SetInputEnabled(false);
			LevelCompleteScript.SetInputEnabled(false);
		}
	}

	private void Start()
	{
		if (ExpController._instance != null)
		{
			UnityEngine.Debug.LogWarning("ExpController is not null while starting.");
		}
		ExpController._instance = this;
	}

	private string SubstituteTempGunIfReplaced(string constTg)
	{
		if (constTg == null)
		{
			return null;
		}
		KeyValuePair<string, string> keyValuePair = WeaponManager.replaceConstWithTemp.Find((KeyValuePair<string, string> kvp) => kvp.Key.Equals(constTg));
		if (keyValuePair.Key == null || keyValuePair.Value == null)
		{
			return constTg;
		}
		if (!TempItemsController.GunsMappingFromTempToConst.ContainsKey(keyValuePair.Value))
		{
			return keyValuePair.Value;
		}
		return constTg;
	}

	public static int TierForLevel(int lev)
	{
		if (lev < ExpController.LevelsForTiers[1])
		{
			return 0;
		}
		if (lev < ExpController.LevelsForTiers[2])
		{
			return 1;
		}
		if (lev < ExpController.LevelsForTiers[3])
		{
			return 2;
		}
		if (lev < ExpController.LevelsForTiers[4])
		{
			return 3;
		}
		if (lev < ExpController.LevelsForTiers[5])
		{
			return 4;
		}
		return 5;
	}

	private void Update()
	{
		if (ExperienceController.sharedController != null)
		{
			this.SetInterfaceEnabled(ExperienceController.sharedController.isShowRanks);
		}
	}

	public void UpdateLabels()
	{
		if (ExperienceController.sharedController != null)
		{
			this.Rank = ExperienceController.sharedController.currentLevel;
			this.Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitAndUpdateExperience(int newRank, int newExperience, int newBound, bool showLevelUpPanel, AudioClip sound)
	{
		ExpController.u003cWaitAndUpdateExperienceu003ec__Iterator135 variable = null;
		return variable;
	}

	public static event Action LevelUpShown;
}