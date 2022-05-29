using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class ChooseLevel : MonoBehaviour
{
	public GameObject BonusGun3Box;

	public GameObject panel;

	public GameObject[] starEnabledPrototypes;

	public GameObject[] starDisabledPrototypes;

	public GameObject gainedStarCountLabel;

	public GameObject backButton;

	public GameObject shopButton;

	public ButtonHandler nextButton;

	public GameObject[] boxOneLevelButtons;

	public GameObject[] boxTwoLevelButtons;

	public GameObject[] boxThreeLevelButtons;

	public AudioClip shopButtonSound;

	public GameObject backgroundHolder;

	public GameObject backgroundHolder_2;

	public GameObject backgroundHolder_3;

	public GameObject[] boxContents;

	public static ChooseLevel sharedChooseLevel;

	private float _timeStarted;

	private IDisposable _backSubscription;

	private int _boxIndex;

	private GameObject[] _boxLevelButtons;

	private string _gainedStarCount = string.Empty;

	private IList<ChooseLevel.LevelInfo> _levelInfos = new List<ChooseLevel.LevelInfo>();

	public ShopNGUIController _shopInstance;

	private float _timeWhenShopWasClosed;

	static ChooseLevel()
	{
	}

	public ChooseLevel()
	{
	}

	public static string GetGainStarsString()
	{
		return ChooseLevel.InitializeGainStarCount(ChooseLevel.InitializeLevelInfos(false));
	}

	private void HandleBackButton(object sender, EventArgs args)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (Time.time - this._timeWhenShopWasClosed < 1f)
		{
			return;
		}
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "CampaignChooseBox";
		LoadConnectScene.noteToShow = null;
		Application.LoadLevel(Defs.PromSceneName);
	}

	private void HandleLevelButton(string levelName)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._timeStarted < 0.15f)
		{
			return;
		}
		CurrentCampaignGame.levelSceneName = levelName;
		WeaponManager.sharedManager.Reset(0);
		FlurryPluginWrapper.LogLevelPressed(CurrentCampaignGame.levelSceneName);
		LevelArt.endOfBox = false;
		Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.ShouldShowArts ? "CampaignLoading" : "LevelArt"), LoadSceneMode.Single);
	}

	private void HandleResumeFromShop()
	{
		this.panel.gameObject.SetActive(true);
		if (this._shopInstance != null)
		{
			ShopNGUIController.GuiActive = false;
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
			}
			this._shopInstance.resumeAction = () => {
			};
			this._shopInstance = null;
			this._timeWhenShopWasClosed = Time.time;
		}
	}

	private void HandleShopButton(object sender, EventArgs args)
	{
		if (this._shopInstance == null)
		{
			this._shopInstance = ShopNGUIController.sharedShop;
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				if (this.shopButtonSound != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(this.shopButtonSound);
				}
				this.panel.gameObject.SetActive(false);
				this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
			}
		}
	}

	private void InitializeFixedDisplay()
	{
		if (this.backButton != null)
		{
			this.backButton.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleBackButton);
		}
		if (this.shopButton != null)
		{
			this.shopButton.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleShopButton);
		}
		if (this.gainedStarCountLabel != null)
		{
			this.gainedStarCountLabel.GetComponent<UILabel>().text = this._gainedStarCount;
		}
	}

	private static string InitializeGainStarCount(IList<ChooseLevel.LevelInfo> levelInfos)
	{
		int count = 3 * levelInfos.Count;
		int starGainedCount = 0;
		IEnumerator<ChooseLevel.LevelInfo> enumerator = levelInfos.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				starGainedCount += enumerator.Current.StarGainedCount;
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		return string.Format("{0}/{1}", starGainedCount, count);
	}

	private void InitializeLevelButtons()
	{
		if (this.starEnabledPrototypes != null)
		{
			GameObject[] gameObjectArray = this.starEnabledPrototypes;
			for (int i = 0; i < (int)gameObjectArray.Length; i++)
			{
				GameObject gameObject = gameObjectArray[i];
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (this.starDisabledPrototypes != null)
		{
			GameObject[] gameObjectArray1 = this.starDisabledPrototypes;
			for (int j = 0; j < (int)gameObjectArray1.Length; j++)
			{
				GameObject gameObject1 = gameObjectArray1[j];
				if (gameObject1 != null)
				{
					gameObject1.SetActive(false);
				}
			}
		}
		if (this.boxContents == null)
		{
			throw new InvalidOperationException("boxContents == 0");
		}
		for (int k = 0; k != (int)this.boxContents.Length; k++)
		{
			this.boxContents[k].SetActive(k == this._boxIndex);
		}
		if (this._boxLevelButtons == null)
		{
			throw new InvalidOperationException("Box level buttons are null.");
		}
		GameObject[] gameObjectArray2 = this._boxLevelButtons;
		for (int l = 0; l < (int)gameObjectArray2.Length; l++)
		{
			GameObject gameObject2 = gameObjectArray2[l];
			if (gameObject2 != null)
			{
				UIButton component = gameObject2.GetComponent<UIButton>();
				if (component != null)
				{
					component.isEnabled = false;
				}
			}
		}
		int num = Math.Min(this._levelInfos.Count, (int)this._boxLevelButtons.Length);
		for (int m = 0; m != num; m++)
		{
			ChooseLevel.LevelInfo item = this._levelInfos[m];
			GameObject enabled = this._boxLevelButtons[m];
			enabled.transform.parent = enabled.transform.parent;
			enabled.GetComponent<UIButton>().isEnabled = item.Enabled;
			UISprite componentInChildren = enabled.GetComponentInChildren<UISprite>();
			if (componentInChildren != null)
			{
				UILabel uILabel = componentInChildren.GetComponentInChildren<UILabel>();
				if (uILabel != null)
				{
					uILabel.applyGradient = item.Enabled;
				}
				else
				{
					Debug.LogWarning("Could not find caption of level button.");
				}
			}
			else
			{
				Debug.LogWarning("Could not find background of level button.");
			}
			enabled.AddComponent<ButtonHandler>();
			string name = item.Name;
			enabled.GetComponent<ButtonHandler>().Clicked += new EventHandler((object sender, EventArgs e) => this.HandleLevelButton(name));
			enabled.SetActive(true);
			for (int n = 0; n != (int)this.starEnabledPrototypes.Length; n++)
			{
				if (item.Enabled)
				{
					GameObject gameObject3 = this.starEnabledPrototypes[n];
					if (gameObject3 != null)
					{
						GameObject starGainedCount = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
						starGainedCount.transform.parent = enabled.transform;
						starGainedCount.GetComponent<UIToggle>().@value = n < item.StarGainedCount;
						starGainedCount.transform.localPosition = gameObject3.transform.localPosition;
						starGainedCount.transform.localScale = gameObject3.transform.localScale;
						starGainedCount.SetActive(true);
					}
				}
			}
		}
		GameObject[] gameObjectArray3 = this.starEnabledPrototypes;
		for (int o = 0; o < (int)gameObjectArray3.Length; o++)
		{
			GameObject gameObject4 = gameObjectArray3[o];
			if (gameObject4 != null)
			{
				UnityEngine.Object.Destroy(gameObject4);
			}
		}
		GameObject[] gameObjectArray4 = this.starDisabledPrototypes;
		for (int p = 0; p < (int)gameObjectArray4.Length; p++)
		{
			GameObject gameObject5 = gameObjectArray4[p];
			if (gameObject5 != null)
			{
				UnityEngine.Object.Destroy(gameObject5);
			}
		}
	}

	private static IList<ChooseLevel.LevelInfo> InitializeLevelInfos(bool draggableLayout = false)
	{
		Dictionary<string, int> strs;
		List<ChooseLevel.LevelInfo> levelInfos = new List<ChooseLevel.LevelInfo>();
		string str = CurrentCampaignGame.boXName;
		int num = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == str);
		if (num == -1)
		{
			Debug.LogWarning("Box not found in list!");
			return levelInfos;
		}
		List<CampaignLevel> item = LevelBox.campaignBoxes[num].levels;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(str, out strs))
		{
			Debug.LogWarning(string.Concat("Box not found in dictionary: ", str));
			strs = new Dictionary<string, int>();
		}
		for (int i = 0; i != item.Count; i++)
		{
			string item1 = item[i].sceneName;
			int num1 = 0;
			strs.TryGetValue(item1, out num1);
			ChooseLevel.LevelInfo levelInfo = new ChooseLevel.LevelInfo()
			{
				Enabled = i <= strs.Count,
				Name = item1,
				StarGainedCount = num1
			};
			levelInfos.Add(levelInfo);
		}
		return levelInfos;
	}

	private static IList<ChooseLevel.LevelInfo> InitializeLevelInfosWithTestData(bool draggableLayout = false)
	{
		List<ChooseLevel.LevelInfo> levelInfos = new List<ChooseLevel.LevelInfo>();
		ChooseLevel.LevelInfo levelInfo = new ChooseLevel.LevelInfo()
		{
			Enabled = true,
			Name = "Cementery",
			StarGainedCount = 1
		};
		levelInfos.Add(levelInfo);
		levelInfo = new ChooseLevel.LevelInfo()
		{
			Enabled = true,
			Name = "City",
			StarGainedCount = 3
		};
		levelInfos.Add(levelInfo);
		levelInfo = new ChooseLevel.LevelInfo()
		{
			Enabled = false,
			Name = "Hospital"
		};
		levelInfos.Add(levelInfo);
		return levelInfos;
	}

	private void InitializeNextButton(IList<ChooseLevel.LevelInfo> levels, ButtonHandler nextButton)
	{
		if (levels == null)
		{
			throw new ArgumentNullException("levels");
		}
		if (nextButton == null)
		{
			throw new ArgumentNullException("nextButton");
		}
		ChooseLevel.LevelInfo levelInfo = levels.LastOrDefault<ChooseLevel.LevelInfo>((ChooseLevel.LevelInfo l) => (!l.Enabled ? false : l.StarGainedCount == 0));
		nextButton.gameObject.SetActive(levelInfo != null);
		if (levelInfo != null)
		{
			nextButton.Clicked += new EventHandler((object sender, EventArgs e) => this.HandleLevelButton(levelInfo.Name));
		}
	}

	private void OnDestroy()
	{
		ChooseLevel.sharedChooseLevel = null;
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(() => {
			if (this._shopInstance == null)
			{
				this.HandleBackButton(this, EventArgs.Empty);
			}
		}, "Choose Level");
	}

	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = false;
		WeaponManager.RefreshExpControllers();
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		StoreKitEventListener.State.PurchaseKey = "In Map";
		StoreKitEventListener.State.Parameters.Clear();
		ChooseLevel.sharedChooseLevel = this;
		this._timeStarted = Time.realtimeSinceStartup;
		bool flag = false;
		this._boxIndex = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == CurrentCampaignGame.boXName);
		if (this._boxIndex == -1)
		{
			Debug.LogWarning("Box not found in list!");
			throw new InvalidOperationException("Box not found in list!");
		}
		this._levelInfos = (false ? ChooseLevel.InitializeLevelInfosWithTestData(flag) : ChooseLevel.InitializeLevelInfos(flag));
		this._gainedStarCount = ChooseLevel.InitializeGainStarCount(this._levelInfos);
		if (CurrentCampaignGame.boXName == "Real")
		{
			this._boxLevelButtons = this.boxOneLevelButtons;
			this.backgroundHolder.SetActive(true);
		}
		else if (CurrentCampaignGame.boXName == "minecraft")
		{
			this._boxLevelButtons = this.boxTwoLevelButtons;
			this.backgroundHolder_2.SetActive(true);
		}
		else if (CurrentCampaignGame.boXName != "Crossed")
		{
			Debug.LogWarning(string.Concat("Unknown box: ", CurrentCampaignGame.boXName));
		}
		else
		{
			this._boxLevelButtons = this.boxThreeLevelButtons;
			this.backgroundHolder_3.SetActive(true);
			string[] empty = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[] { '#' });
			for (int i = 0; i < (int)empty.Length; i++)
			{
				if (empty[i] == null)
				{
					empty[i] = string.Empty;
				}
			}
			this.BonusGun3Box.SetActive((empty == null ? 0 : (int)empty.Contains<string>(WeaponManager.BugGunWN)) == 0);
		}
		this.InitializeLevelButtons();
		this.InitializeFixedDisplay();
		this.InitializeNextButton(this._levelInfos, this.nextButton);
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	private sealed class LevelInfo
	{
		public bool Enabled
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int StarGainedCount
		{
			get;
			set;
		}

		public LevelInfo()
		{
		}
	}
}