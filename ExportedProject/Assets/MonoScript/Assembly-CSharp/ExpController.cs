using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class ExpController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F
	{
		internal string constTg;

		internal bool _003C_003Em__28B(KeyValuePair<string, string> kvp)
		{
			return kvp.Key.Equals(constTg);
		}
	}

	public const int MaxLobbyLevel = 3;

	public ExpView experienceView;

	private bool starterBannerShowed;

	public static readonly int[] LevelsForTiers = new int[6] { 1, 7, 12, 17, 22, 27 };

	private static ExpController _instance;

	private bool _sameSceneIndicator;

	private bool _inAddingState;

	public static ExpController Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool InAddingState
	{
		get
		{
			return _inAddingState;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			return experienceView != null && experienceView.interfaceHolder != null && experienceView.interfaceHolder.gameObject.activeInHierarchy;
		}
		set
		{
			SetInterfaceEnabled(value);
		}
	}

	public static int LobbyLevel
	{
		get
		{
			return 3;
		}
	}

	public bool IsLevelUpShown { get; private set; }

	public int Rank
	{
		set
		{
			if (!(experienceView == null))
			{
				int rankSprite = Mathf.Clamp(value, 1, ExperienceController.maxLevel);
				experienceView.RankSprite = rankSprite;
			}
		}
	}

	public bool WaitingForLevelUpView { get; private set; }

	public int OurTier
	{
		get
		{
			if (ExperienceController.sharedController != null)
			{
				int currentLevel = ExperienceController.sharedController.currentLevel;
				return TierForLevel(currentLevel);
			}
			return 0;
		}
	}

	private int Experience
	{
		set
		{
			if (!(experienceView == null) && !(ExperienceController.sharedController == null))
			{
				int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
				int num2 = Mathf.Clamp(value, 0, num);
				experienceView.ExperienceLabel = FormatExperienceLabel(num2, num);
				float num3 = (float)num2 / (float)num;
				if (ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel)
				{
					num3 = 1f;
				}
				experienceView.CurrentProgress = num3;
				experienceView.OldProgress = num3;
			}
		}
	}

	public static event Action LevelUpShown;

	public static int OurTierForAnyPlace()
	{
		return (!(Instance != null)) ? GetOurTier() : Instance.OurTier;
	}

	private void SetInterfaceEnabled(bool value)
	{
		if (value && experienceView != null)
		{
			bool flag = Application.loadedLevelName != Defs.MainMenuScene || ShopNGUIController.GuiActive || (MainMenuController.sharedController != null && MainMenuController.sharedController.singleModePanel != null && MainMenuController.sharedController.singleModePanel.activeInHierarchy) || (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled);
			if (experienceView.rankIndicatorContainer.activeSelf != flag)
			{
				experienceView.rankIndicatorContainer.SetActive(flag);
			}
		}
		if (InterfaceEnabled == value || !(experienceView != null) || !(experienceView.interfaceHolder != null))
		{
			return;
		}
		if (!value)
		{
			experienceView.StopAnimation();
		}
		if (ExperienceController.sharedController != null)
		{
			Rank = ExperienceController.sharedController.currentLevel;
			Experience = ExperienceController.sharedController.CurrentExperience;
		}
		experienceView.interfaceHolder.gameObject.SetActive(value);
		if (value && experienceView.experienceCamera != null)
		{
			AudioListener component = experienceView.experienceCamera.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}

	public void HandleContinueButton(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0);
		}
		if (!starterBannerShowed && ExperienceController.sharedController.currentLevel == 2 && Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
		{
			starterBannerShowed = true;
			experienceView.ToBonus(Defs.abTestBalansStartCapitalGems, Defs.abTestBalansStartCapitalCoins);
		}
		else
		{
			starterBannerShowed = false;
			HideTierPanel(tierPanel);
		}
	}

	public void HandleShopButtonFromTierPanel(GameObject tierPanel)
	{
		StartCoroutine(HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	public void HandleNewAvailableItem(GameObject tierPanel, NewAvailableItemInShop itemInfo)
	{
		if (Defs.isHunger)
		{
			return;
		}
		if (CurrentFilterMap() != 0)
		{
			int[] target = new int[0];
			bool flag = true;
			if (itemInfo != null && itemInfo._tag != null)
			{
				flag = ItemDb.GetByTag(itemInfo._tag) != null;
				if (flag)
				{
					target = ItemDb.GetItemFilterMap(itemInfo._tag);
				}
			}
			if (flag && !target.Contains(CurrentFilterMap()))
			{
				HandleShopButtonFromTierPanel(tierPanel);
				return;
			}
		}
		if (ShopNGUIController.sharedShop == null)
		{
			Debug.LogWarning("ShopNGUIController.sharedShop == null");
		}
		else
		{
			if (!(itemInfo == null))
			{
				string text = itemInfo._tag ?? string.Empty;
				Debug.Log("Available item:   " + text + "    " + itemInfo.category);
				StartCoroutine(HandleShopButtonFromNewAvailableItemCoroutine(tierPanel, text, itemInfo.category));
				return;
			}
			Debug.LogWarning("itemInfo == null");
		}
		StartCoroutine(HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	public static int CurrentFilterMap()
	{
		return Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0;
	}

	private IEnumerator HandleShopButtonFromTierPanelCoroutine(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(CurrentFilterMap());
		}
		yield return null;
		ShopNGUIController.sharedShop.resumeAction = null;
		ShopNGUIController.GuiActive = true;
		yield return null;
		HideTierPanel(tierPanel);
	}

	private IEnumerator HandleShopButtonFromNewAvailableItemCoroutine(GameObject tierPanel, string itemTag, ShopNGUIController.CategoryNames category)
	{
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			itemTag = WeaponManager.FirstUnboughtOrForOurTier(itemTag) ?? itemTag;
		}
		ShopNGUIController.sharedShop.SetOfferID(itemTag);
		ShopNGUIController.sharedShop.offerCategory = category;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(Application.loadedLevelName) ? Defs.filterMaps[Application.loadedLevelName] : 0);
		}
		yield return null;
		ShopNGUIController.sharedShop.resumeAction = null;
		ShopNGUIController.GuiActive = true;
		yield return null;
		HideTierPanel(tierPanel);
	}

	public void UpdateLabels()
	{
		if (ExperienceController.sharedController != null)
		{
			Rank = ExperienceController.sharedController.currentLevel;
			Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	public void Refresh()
	{
		if (ExperienceController.sharedController != null)
		{
			Rank = ExperienceController.sharedController.currentLevel;
			Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	public static int TierForLevel(int lev)
	{
		if (lev < LevelsForTiers[1])
		{
			return 0;
		}
		if (lev < LevelsForTiers[2])
		{
			return 1;
		}
		if (lev < LevelsForTiers[3])
		{
			return 2;
		}
		if (lev < LevelsForTiers[4])
		{
			return 3;
		}
		if (lev < LevelsForTiers[5])
		{
			return 4;
		}
		return 5;
	}

	public static int GetOurTier()
	{
		int currentLevelWithUpdateCorrection = ExperienceController.GetCurrentLevelWithUpdateCorrection();
		return TierForLevel(currentLevelWithUpdateCorrection);
	}

	public void AddExperience(int oldLevel, int oldExperience, int addend, AudioClip exp2, AudioClip levelup, AudioClip tierup = null)
	{
		if (experienceView == null || ExperienceController.sharedController == null)
		{
			return;
		}
		int num = oldExperience + addend;
		int num2 = ExperienceController.MaxExpLevels[oldLevel];
		if (num < num2)
		{
			float percentage = GetPercentage(num);
			experienceView.CurrentProgress = percentage;
			experienceView.StartBlinkingWithNewProgress();
			experienceView.WaitAndUpdateOldProgress(exp2);
			experienceView.ExperienceLabel = FormatExperienceLabel(num, num2);
			if (experienceView.currentProgress != null && !experienceView.currentProgress.gameObject.activeInHierarchy)
			{
				experienceView.OldProgress = percentage;
			}
		}
		else
		{
			float num3 = 1f;
			experienceView.CurrentProgress = num3;
			AudioClip sound = levelup;
			if (tierup != null && Array.IndexOf(LevelsForTiers, oldLevel + 1) > 0)
			{
				sound = tierup;
			}
			if (oldLevel < ExperienceController.maxLevel - 1)
			{
				experienceView.StartBlinkingWithNewProgress();
				int num4 = oldLevel + 1;
				int newExperience = num - num2;
				StartCoroutine(WaitAndUpdateExperience(num4, newExperience, ExperienceController.MaxExpLevels[num4], true, sound));
			}
			else if (oldLevel == ExperienceController.maxLevel - 1)
			{
				experienceView.StartBlinkingWithNewProgress();
				int num5 = oldLevel + 1;
				int newExperience2 = ExperienceController.MaxExpLevels[num5];
				StartCoroutine(WaitAndUpdateExperience(num5, newExperience2, ExperienceController.MaxExpLevels[num5], true, sound));
			}
			else
			{
				if (ExperienceController.sharedController.currentLevel == ExperienceController.maxLevel)
				{
					num3 = 1f;
				}
				experienceView.OldProgress = num3;
				experienceView.StartBlinkingWithNewProgress();
				int maxLevel = ExperienceController.maxLevel;
				int newExperience3 = ExperienceController.MaxExpLevels[maxLevel];
				StartCoroutine(WaitAndUpdateExperience(maxLevel, newExperience3, ExperienceController.MaxExpLevels[maxLevel], false, exp2));
			}
		}
		StartCoroutine(SetAddingState());
	}

	public bool IsRenderedWithCamera(Camera c)
	{
		return experienceView != null && experienceView.experienceCamera != null && experienceView.experienceCamera == c;
	}

	private string SubstituteTempGunIfReplaced(string constTg)
	{
		_003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F _003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F = new _003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F();
		_003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F.constTg = constTg;
		if (_003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F.constTg == null)
		{
			return null;
		}
		KeyValuePair<string, string> keyValuePair = WeaponManager.replaceConstWithTemp.Find(_003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F._003C_003Em__28B);
		if (keyValuePair.Key == null || keyValuePair.Value == null)
		{
			return _003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F.constTg;
		}
		if (!TempItemsController.GunsMappingFromTempToConst.ContainsKey(keyValuePair.Value))
		{
			return keyValuePair.Value;
		}
		return _003CSubstituteTempGunIfReplaced_003Ec__AnonStorey29F.constTg;
	}

	private IEnumerator WaitAndUpdateExperience(int newRank, int newExperience, int newBound, bool showLevelUpPanel, AudioClip sound)
	{
		experienceView.RankSprite = newRank;
		experienceView.ExperienceLabel = FormatExperienceLabel(newExperience, newBound);
		WaitingForLevelUpView = showLevelUpPanel;
		yield return new WaitForSeconds(1.2f);
		Experience = newExperience;
		if (showLevelUpPanel && ExperienceController.sharedController != null)
		{
			List<string> itemsToShow = new List<string>();
			int i = Array.BinarySearch(LevelsForTiers, ExperienceController.sharedController.currentLevel);
			if (0 <= i && i < LevelsForTiers.Length)
			{
				switch (i)
				{
				case 1:
					itemsToShow = new List<string>
					{
						"Armor_Steel_1",
						SubstituteTempGunIfReplaced(WeaponTags.Antihero_Rifle_1_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.frank_sheepone_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.PORTABLE_DEATH_MOON_Tag)
					};
					break;
				case 2:
					itemsToShow = new List<string>
					{
						"Armor_Royal_1",
						SubstituteTempGunIfReplaced(WeaponTags.DragonGun_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.charge_rifle_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.loud_piggy_Tag)
					};
					break;
				case 3:
					itemsToShow = new List<string>
					{
						"Armor_Almaz_1",
						SubstituteTempGunIfReplaced(WeaponTags.Dark_Matter_Generator_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.autoaim_bazooka_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Photon_Pistol_Tag)
					};
					break;
				case 4:
					itemsToShow = new List<string>
					{
						SubstituteTempGunIfReplaced(WeaponTags.RailRevolverBuy_3_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.RayMinigun_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Autoaim_RocketlauncherBuy_3_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Impulse_Sniper_RifleBuy_3_Tag)
					};
					break;
				case 5:
					itemsToShow = new List<string>
					{
						SubstituteTempGunIfReplaced(WeaponTags.PX_3000_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.StormHammer_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Sunrise_Tag),
						SubstituteTempGunIfReplaced(WeaponTags.Bastion_Tag)
					};
					break;
				}
				experienceView.LevelUpPanelOptions.ShowTierView = true;
			}
			else
			{
				experienceView.LevelUpPanelOptions.ShowTierView = false;
			}
			experienceView.LevelUpPanelOptions.ShareButtonEnabled = true;
			int oldRank = Math.Max(0, newRank - 1);
			int coinsReward = ExperienceController.addCoinsFromLevels[oldRank];
			int gemsReward = ExperienceController.addGemsFromLevels[oldRank];
			if (NetworkStartTableNGUIController.sharedController != null)
			{
				_sameSceneIndicator = true;
				while (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow)
				{
					yield return null;
				}
				if (!_sameSceneIndicator || NetworkStartTableNGUIController.sharedController == null)
				{
					WaitingForLevelUpView = false;
					yield break;
				}
			}
			else if (Application.loadedLevelName == "LevelComplete")
			{
				_sameSceneIndicator = true;
				while (_sameSceneIndicator && LevelCompleteScript.IsInterfaceBusy)
				{
					yield return null;
				}
				if (!_sameSceneIndicator)
				{
					WaitingForLevelUpView = false;
					yield break;
				}
			}
			experienceView.LevelUpPanelOptions.NewItems = itemsToShow;
			experienceView.LevelUpPanelOptions.CurrentRank = newRank;
			experienceView.LevelUpPanelOptions.CoinsReward = coinsReward;
			experienceView.LevelUpPanelOptions.GemsReward = gemsReward;
			experienceView.ShowLevelUpPanel();
		}
		WaitingForLevelUpView = false;
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(sound);
		}
	}

	private IEnumerator SetAddingState()
	{
		_inAddingState = true;
		yield return new WaitForSeconds(1.2f);
		_inAddingState = false;
	}

	private static float GetPercentage(int experience)
	{
		if (ExperienceController.sharedController == null)
		{
			return 0f;
		}
		int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
		int num2 = Mathf.Clamp(experience, 0, num);
		return (float)num2 / (float)num;
	}

	public static float progressExpInPer()
	{
		return GetPercentage(ExperienceController.sharedController.CurrentExperience);
	}

	private static string FormatExperienceLabel(int xp, int bound)
	{
		string text = LocalizationStore.Get("Key_0928");
		string text2 = string.Format("{0} {1}/{2}", LocalizationStore.Get("Key_0204"), xp, bound);
		return (xp != bound && ExperienceController.sharedController.currentLevel != ExperienceController.maxLevel) ? text2 : text;
	}

	public static string ExpToString()
	{
		int currentLevel = ExperienceController.sharedController.currentLevel;
		int currentExperience = ExperienceController.sharedController.CurrentExperience;
		return FormatExperienceLabel(currentExperience, ExperienceController.MaxExpLevels[currentLevel]);
	}

	private static string FormatLevelLabel(int level)
	{
		return string.Format("{0} {1}", LocalizationStore.Key_0226, level);
	}

	private void OnEnable()
	{
		if (ExperienceController.sharedController != null)
		{
			Rank = ExperienceController.sharedController.currentLevel;
			Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	private void Awake()
	{
		if (!SceneLoader.ActiveSceneName.EndsWith("Workbench"))
		{
			InterfaceEnabled = false;
		}
		LocalizationStore.AddEventCallAfterLocalize(UpdateLabels);
		SceneLoader instance = Singleton<SceneLoader>.Instance;
		instance.OnSceneLoading = (Action<SceneLoadInfo>)Delegate.Combine(instance.OnSceneLoading, new Action<SceneLoadInfo>(_003CAwake_003Em__28C));
	}

	private void Start()
	{
		if (_instance != null)
		{
			Debug.LogWarning("ExpController is not null while starting.");
		}
		_instance = this;
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(UpdateLabels);
		_instance = null;
	}

	private void Update()
	{
		if (ExperienceController.sharedController != null)
		{
			SetInterfaceEnabled(ExperienceController.sharedController.isShowRanks);
		}
	}

	public static void ShowTierPanel(GameObject tierPanel)
	{
		if (!(tierPanel != null))
		{
			return;
		}
		tierPanel.SetActive(true);
		if (Instance != null)
		{
			Instance.IsLevelUpShown = true;
			Action levelUpShown = ExpController.LevelUpShown;
			if (levelUpShown != null)
			{
				levelUpShown();
			}
		}
		MainMenuController.SetInputEnabled(false);
		LevelCompleteScript.SetInputEnabled(false);
	}

	public static void HideTierPanel(GameObject tierPanel)
	{
		if (tierPanel != null)
		{
			tierPanel.SetActive(false);
			if (Instance != null)
			{
				Instance.IsLevelUpShown = false;
			}
			MainMenuController.SetInputEnabled(true);
			LevelCompleteScript.SetInputEnabled(true);
		}
	}

	private void OnLevelWasLoaded(int index)
	{
		_sameSceneIndicator = false;
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__28C(SceneLoadInfo sli)
	{
		if (experienceView != null)
		{
			LevelUpWithOffers currentVisiblePanel = experienceView.CurrentVisiblePanel;
			if (currentVisiblePanel != null)
			{
				HideTierPanel(currentVisiblePanel.gameObject);
			}
		}
		IsLevelUpShown = false;
	}
}
