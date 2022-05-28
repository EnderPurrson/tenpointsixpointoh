using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

public sealed class ExpView : MonoBehaviour
{
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
		instance.OnSceneLoading = (Action<SceneLoadInfo>)Delegate.Combine(instance.OnSceneLoading, (Action<SceneLoadInfo>)delegate
		{
			_levelUpPanel.DestroyValue();
			_levelUpPanelTier.DestroyValue();
		});
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
		if (currentProgress == null || !currentProgress.gameObject.activeInHierarchy)
		{
			Debug.LogWarning("(currentProgress == null || !currentProgress.gameObject.activeInHierarchy)");
			return new ActionDisposable(delegate
			{
			});
		}
		currentProgress.StopAllCoroutines();
		IEnumerator c = StartBlinkingCoroutine();
		currentProgress.StartCoroutine(c);
		return new ActionDisposable(delegate
		{
			currentProgress.StopCoroutine(c);
			if (currentProgress != null)
			{
				currentProgress.enabled = true;
			}
		});
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
			IEnumerable<UIButton> source = from b in UnityEngine.Object.FindObjectsOfType<UIButton>()
				where b.gameObject.name.Equals("Profile")
				select b;
			_profileButton = source.FirstOrDefault();
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
}
