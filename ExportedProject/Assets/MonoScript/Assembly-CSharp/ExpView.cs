using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class ExpView : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A0
	{
		internal IEnumerator c;

		internal ExpView _003C_003Ef__this;

		internal void _003C_003Em__28F()
		{
			_003C_003Ef__this.currentProgress.StopCoroutine(c);
			if (_003C_003Ef__this.currentProgress != null)
			{
				_003C_003Ef__this.currentProgress.enabled = true;
			}
		}
	}

	public GameObject rankIndicatorContainer;

	public UIRoot interfaceHolder;

	public Camera experienceCamera;

	public UISprite experienceFrame;

	public UILabel experienceLabel;

	public UISprite currentProgress;

	public UISprite oldProgress;

	public UISprite rankSprite;

	[SerializeField]
	private PrefabHandler _levelUpPanelPrefab;

	[SerializeField]
	private PrefabHandler _levelUpPanelTierPrefab;

	public GameObject objHUD;

	[SerializeField]
	private GameObject _levelUpPanelsContainer;

	private LazyObject<LevelUpWithOffers> _levelUpPanelValue;

	private LazyObject<LevelUpWithOffers> _levelUpPanelTierValue;

	private LevelUpWithOffers _currentLevelUpPanel;

	private LeveUpPanelShowOptions _levelUpPanelOptions;

	private UIButton _profileButton;

	[CompilerGenerated]
	private static Action _003C_003Ef__am_0024cache11;

	[CompilerGenerated]
	private static Func<UIButton, bool> _003C_003Ef__am_0024cache12;

	public bool LevelUpPanelOpened
	{
		get
		{
			return _levelUpPanel.ObjectIsActive || _levelUpPanelTier.ObjectIsActive;
		}
	}

	public LevelUpWithOffers CurrentVisiblePanel
	{
		get
		{
			if (_levelUpPanel.ObjectIsActive)
			{
				return _levelUpPanel.Value;
			}
			if (_levelUpPanelTier.ObjectIsActive)
			{
				return _levelUpPanelTier.Value;
			}
			return null;
		}
	}

	public LazyObject<LevelUpWithOffers> _levelUpPanel
	{
		get
		{
			if (_levelUpPanelValue == null)
			{
				_levelUpPanelValue = new LazyObject<LevelUpWithOffers>(_levelUpPanelPrefab.ResourcePath, _levelUpPanelsContainer);
			}
			return _levelUpPanelValue;
		}
	}

	public LazyObject<LevelUpWithOffers> _levelUpPanelTier
	{
		get
		{
			if (_levelUpPanelTierValue == null)
			{
				_levelUpPanelTierValue = new LazyObject<LevelUpWithOffers>(_levelUpPanelTierPrefab.ResourcePath, _levelUpPanelsContainer);
			}
			return _levelUpPanelTierValue;
		}
	}

	public bool VisibleHUD
	{
		get
		{
			return objHUD.activeSelf;
		}
		set
		{
			objHUD.SetActive(value);
		}
	}

	public string ExperienceLabel
	{
		get
		{
			return (!(experienceLabel != null)) ? string.Empty : experienceLabel.text;
		}
		set
		{
			if (experienceLabel != null)
			{
				experienceLabel.text = value ?? string.Empty;
			}
		}
	}

	public float CurrentProgress
	{
		get
		{
			return (!(currentProgress != null)) ? 0f : currentProgress.fillAmount;
		}
		set
		{
			if (currentProgress != null)
			{
				currentProgress.fillAmount = value;
			}
		}
	}

	public float OldProgress
	{
		get
		{
			return (!(oldProgress != null)) ? 0f : oldProgress.fillAmount;
		}
		set
		{
			if (oldProgress != null)
			{
				oldProgress.fillAmount = value;
			}
		}
	}

	public int RankSprite
	{
		get
		{
			if (rankSprite == null)
			{
				return 1;
			}
			string s = rankSprite.spriteName.Replace("Rank_", string.Empty);
			int result = 0;
			return (!int.TryParse(s, out result)) ? 1 : result;
		}
		set
		{
			if (rankSprite != null)
			{
				string spriteName = string.Format("Rank_{0}", value);
				rankSprite.spriteName = spriteName;
			}
		}
	}

	public LeveUpPanelShowOptions LevelUpPanelOptions
	{
		get
		{
			if (_levelUpPanelOptions == null)
			{
				_levelUpPanelOptions = new LeveUpPanelShowOptions();
			}
			return _levelUpPanelOptions;
		}
	}

	private void Awake()
	{
		SceneLoader instance = Singleton<SceneLoader>.Instance;
		instance.OnSceneLoading = (Action<SceneLoadInfo>)Delegate.Combine(instance.OnSceneLoading, new Action<SceneLoadInfo>(_003CAwake_003Em__28D));
	}

	public void ShowLevelUpPanel()
	{
		_currentLevelUpPanel = ((!LevelUpPanelOptions.ShowTierView) ? _levelUpPanel.Value : _levelUpPanelTier.Value);
		_currentLevelUpPanel.SetCurrentRank(LevelUpPanelOptions.CurrentRank.ToString());
		_currentLevelUpPanel.SetRewardPrice("+" + LevelUpPanelOptions.CoinsReward + "\n" + LocalizationStore.Get("Key_0275"));
		_currentLevelUpPanel.SetGemsRewardPrice("+" + LevelUpPanelOptions.GemsReward + "\n" + LocalizationStore.Get("Key_0951"));
		_currentLevelUpPanel.SetAddHealthCount(string.Format(LocalizationStore.Get("Key_1856"), ExperienceController.sharedController.AddHealthOnCurLevel.ToString()));
		_currentLevelUpPanel.SetItems(LevelUpPanelOptions.NewItems);
		_currentLevelUpPanel.shareScript.share.IsChecked = LevelUpPanelOptions.ShareButtonEnabled;
		ExpController.ShowTierPanel(_currentLevelUpPanel.gameObject);
	}

	public void ToBonus(int starterGemsReward, int starterCoinsReward)
	{
		if (_currentLevelUpPanel != null)
		{
			_currentLevelUpPanel.SetStarterBankValues(starterGemsReward, starterCoinsReward);
			_currentLevelUpPanel.shareScript.animatorLevel.SetTrigger("Bonus");
		}
	}

	public void StopAnimation()
	{
		if (currentProgress.gameObject.activeInHierarchy)
		{
			currentProgress.StopAllCoroutines();
		}
		if (oldProgress != null && oldProgress.gameObject.activeInHierarchy)
		{
			oldProgress.StopAllCoroutines();
			oldProgress.enabled = true;
			oldProgress.fillAmount = currentProgress.fillAmount;
		}
	}

	public IDisposable StartBlinkingWithNewProgress()
	{
		_003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A0 _003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A = new _003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A0();
		_003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A._003C_003Ef__this = this;
		if (currentProgress == null || !currentProgress.gameObject.activeInHierarchy)
		{
			Debug.LogWarning("(currentProgress == null || !currentProgress.gameObject.activeInHierarchy)");
			if (_003C_003Ef__am_0024cache11 == null)
			{
				_003C_003Ef__am_0024cache11 = _003CStartBlinkingWithNewProgress_003Em__28E;
			}
			return new ActionDisposable(_003C_003Ef__am_0024cache11);
		}
		currentProgress.StopAllCoroutines();
		_003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A.c = StartBlinkingCoroutine();
		currentProgress.StartCoroutine(_003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A.c);
		return new ActionDisposable(_003CStartBlinkingWithNewProgress_003Ec__AnonStorey2A._003C_003Em__28F);
	}

	public void WaitAndUpdateOldProgress(AudioClip sound)
	{
		if (!(oldProgress == null) && oldProgress.gameObject.activeInHierarchy)
		{
			oldProgress.StopAllCoroutines();
			oldProgress.StartCoroutine(WaitAndUpdateCoroutine(sound));
		}
	}

	private void OnEnable()
	{
		if (_profileButton == null)
		{
			UIButton[] source = UnityEngine.Object.FindObjectsOfType<UIButton>();
			if (_003C_003Ef__am_0024cache12 == null)
			{
				_003C_003Ef__am_0024cache12 = _003COnEnable_003Em__290;
			}
			IEnumerable<UIButton> source2 = source.Where(_003C_003Ef__am_0024cache12);
			_profileButton = source2.FirstOrDefault();
		}
	}

	private void OnDisable()
	{
		oldProgress.enabled = true;
		oldProgress.fillAmount = currentProgress.fillAmount;
		if (currentProgress != null && currentProgress.gameObject.activeInHierarchy)
		{
			currentProgress.StopAllCoroutines();
		}
	}

	private IEnumerator StartBlinkingCoroutine()
	{
		for (int i = 0; i != 4; i++)
		{
			currentProgress.enabled = false;
			yield return new WaitForSeconds(0.15f);
			currentProgress.enabled = true;
			yield return new WaitForSeconds(0.15f);
		}
	}

	private IEnumerator WaitAndUpdateCoroutine(AudioClip sound)
	{
		yield return new WaitForSeconds(1.2f);
		if (currentProgress != null)
		{
			oldProgress.fillAmount = currentProgress.fillAmount;
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(sound);
		}
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__28D(SceneLoadInfo loadInfo)
	{
		_levelUpPanel.DestroyValue();
		_levelUpPanelTier.DestroyValue();
	}

	[CompilerGenerated]
	private static void _003CStartBlinkingWithNewProgress_003Em__28E()
	{
	}

	[CompilerGenerated]
	private static bool _003COnEnable_003Em__290(UIButton b)
	{
		return b.gameObject.name.Equals("Profile");
	}
}
