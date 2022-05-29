using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

	public LazyObject<LevelUpWithOffers> _levelUpPanel
	{
		get
		{
			if (this._levelUpPanelValue == null)
			{
				this._levelUpPanelValue = new LazyObject<LevelUpWithOffers>(this._levelUpPanelPrefab.ResourcePath, this._levelUpPanelsContainer);
			}
			return this._levelUpPanelValue;
		}
	}

	public LazyObject<LevelUpWithOffers> _levelUpPanelTier
	{
		get
		{
			if (this._levelUpPanelTierValue == null)
			{
				this._levelUpPanelTierValue = new LazyObject<LevelUpWithOffers>(this._levelUpPanelTierPrefab.ResourcePath, this._levelUpPanelsContainer);
			}
			return this._levelUpPanelTierValue;
		}
	}

	public float CurrentProgress
	{
		get
		{
			return (this.currentProgress == null ? 0f : this.currentProgress.fillAmount);
		}
		set
		{
			if (this.currentProgress != null)
			{
				this.currentProgress.fillAmount = value;
			}
		}
	}

	public LevelUpWithOffers CurrentVisiblePanel
	{
		get
		{
			if (this._levelUpPanel.ObjectIsActive)
			{
				return this._levelUpPanel.Value;
			}
			if (!this._levelUpPanelTier.ObjectIsActive)
			{
				return null;
			}
			return this._levelUpPanelTier.Value;
		}
	}

	public string ExperienceLabel
	{
		get
		{
			return (this.experienceLabel == null ? string.Empty : this.experienceLabel.text);
		}
		set
		{
			if (this.experienceLabel != null)
			{
				this.experienceLabel.text = value ?? string.Empty;
			}
		}
	}

	public bool LevelUpPanelOpened
	{
		get
		{
			return (this._levelUpPanel.ObjectIsActive ? true : this._levelUpPanelTier.ObjectIsActive);
		}
	}

	public LeveUpPanelShowOptions LevelUpPanelOptions
	{
		get
		{
			if (this._levelUpPanelOptions == null)
			{
				this._levelUpPanelOptions = new LeveUpPanelShowOptions();
			}
			return this._levelUpPanelOptions;
		}
	}

	public float OldProgress
	{
		get
		{
			return (this.oldProgress == null ? 0f : this.oldProgress.fillAmount);
		}
		set
		{
			if (this.oldProgress != null)
			{
				this.oldProgress.fillAmount = value;
			}
		}
	}

	public int RankSprite
	{
		get
		{
			if (this.rankSprite == null)
			{
				return 1;
			}
			string str = this.rankSprite.spriteName.Replace("Rank_", string.Empty);
			int num = 0;
			return (!int.TryParse(str, out num) ? 1 : num);
		}
		set
		{
			if (this.rankSprite != null)
			{
				string str = string.Format("Rank_{0}", value);
				this.rankSprite.spriteName = str;
			}
		}
	}

	public bool VisibleHUD
	{
		get
		{
			return this.objHUD.activeSelf;
		}
		set
		{
			this.objHUD.SetActive(value);
		}
	}

	public ExpView()
	{
	}

	private void Awake()
	{
		Singleton<SceneLoader>.Instance.OnSceneLoading += new Action<SceneLoadInfo>((SceneLoadInfo loadInfo) => {
			this._levelUpPanel.DestroyValue();
			this._levelUpPanelTier.DestroyValue();
		});
	}

	private void OnDisable()
	{
		this.oldProgress.enabled = true;
		this.oldProgress.fillAmount = this.currentProgress.fillAmount;
		if (this.currentProgress != null && this.currentProgress.gameObject.activeInHierarchy)
		{
			this.currentProgress.StopAllCoroutines();
		}
	}

	private void OnEnable()
	{
		if (this._profileButton == null)
		{
			IEnumerable<UIButton> uIButtons = 
				from b in (IEnumerable<UIButton>)UnityEngine.Object.FindObjectsOfType<UIButton>()
				where b.gameObject.name.Equals("Profile")
				select b;
			this._profileButton = uIButtons.FirstOrDefault<UIButton>();
		}
	}

	public void ShowLevelUpPanel()
	{
		this._currentLevelUpPanel = (!this.LevelUpPanelOptions.ShowTierView ? this._levelUpPanel.Value : this._levelUpPanelTier.Value);
		this._currentLevelUpPanel.SetCurrentRank(this.LevelUpPanelOptions.CurrentRank.ToString());
		this._currentLevelUpPanel.SetRewardPrice(string.Concat(new object[] { "+", this.LevelUpPanelOptions.CoinsReward, "\n", LocalizationStore.Get("Key_0275") }));
		this._currentLevelUpPanel.SetGemsRewardPrice(string.Concat(new object[] { "+", this.LevelUpPanelOptions.GemsReward, "\n", LocalizationStore.Get("Key_0951") }));
		LevelUpWithOffers levelUpWithOffer = this._currentLevelUpPanel;
		string str = LocalizationStore.Get("Key_1856");
		int addHealthOnCurLevel = ExperienceController.sharedController.AddHealthOnCurLevel;
		levelUpWithOffer.SetAddHealthCount(string.Format(str, addHealthOnCurLevel.ToString()));
		this._currentLevelUpPanel.SetItems(this.LevelUpPanelOptions.NewItems);
		this._currentLevelUpPanel.shareScript.share.IsChecked = this.LevelUpPanelOptions.ShareButtonEnabled;
		ExpController.ShowTierPanel(this._currentLevelUpPanel.gameObject);
	}

	[DebuggerHidden]
	private IEnumerator StartBlinkingCoroutine()
	{
		ExpView.u003cStartBlinkingCoroutineu003ec__Iterator137 variable = null;
		return variable;
	}

	public IDisposable StartBlinkingWithNewProgress()
	{
		if (this.currentProgress == null || !this.currentProgress.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogWarning("(currentProgress == null || !currentProgress.gameObject.activeInHierarchy)");
			return new ActionDisposable(() => {
			});
		}
		this.currentProgress.StopAllCoroutines();
		IEnumerator enumerator = this.StartBlinkingCoroutine();
		this.currentProgress.StartCoroutine(enumerator);
		return new ActionDisposable(() => {
			this.currentProgress.StopCoroutine(enumerator);
			if (this.currentProgress != null)
			{
				this.currentProgress.enabled = true;
			}
		});
	}

	public void StopAnimation()
	{
		if (this.currentProgress.gameObject.activeInHierarchy)
		{
			this.currentProgress.StopAllCoroutines();
		}
		if (this.oldProgress != null && this.oldProgress.gameObject.activeInHierarchy)
		{
			this.oldProgress.StopAllCoroutines();
			this.oldProgress.enabled = true;
			this.oldProgress.fillAmount = this.currentProgress.fillAmount;
		}
	}

	public void ToBonus(int starterGemsReward, int starterCoinsReward)
	{
		if (this._currentLevelUpPanel != null)
		{
			this._currentLevelUpPanel.SetStarterBankValues(starterGemsReward, starterCoinsReward);
			this._currentLevelUpPanel.shareScript.animatorLevel.SetTrigger("Bonus");
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitAndUpdateCoroutine(AudioClip sound)
	{
		ExpView.u003cWaitAndUpdateCoroutineu003ec__Iterator138 variable = null;
		return variable;
	}

	public void WaitAndUpdateOldProgress(AudioClip sound)
	{
		if (this.oldProgress == null || !this.oldProgress.gameObject.activeInHierarchy)
		{
			return;
		}
		this.oldProgress.StopAllCoroutines();
		this.oldProgress.StartCoroutine(this.WaitAndUpdateCoroutine(sound));
	}
}