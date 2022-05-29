using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class ChooseBox : MonoBehaviour
{
	public static ChooseBox instance;

	private Vector2 pressPoint;

	private Vector2 startPoint;

	private Vector2 pointMap;

	private bool isVozvratMap;

	private Vector2 sizeMap = new Vector2(823f, 736f);

	private bool isMoveMap;

	private bool isSetMap;

	private List<Texture> boxPreviews = new List<Texture>();

	private List<Texture> closedBoxPreviews = new List<Texture>();

	public ChooseBoxNGUIController nguiController;

	public Transform gridTransform;

	private bool _escapePressed;

	private IDisposable _backSubscription;

	static ChooseBox()
	{
	}

	public ChooseBox()
	{
	}

	private int CalculateStarsLeftToOpenTheBox(int boxIndex)
	{
		Dictionary<string, int> strs;
		if (boxIndex >= LevelBox.campaignBoxes.Count)
		{
			throw new ArgumentOutOfRangeException("boxIndex");
		}
		int num = 0;
		for (int i = 0; i < boxIndex; i++)
		{
			LevelBox item = LevelBox.campaignBoxes[i];
			if (CampaignProgress.boxesLevelsAndStars.TryGetValue(item.name, out strs))
			{
				foreach (CampaignLevel level in item.levels)
				{
					int num1 = 0;
					if (!strs.TryGetValue(level.sceneName, out num1))
					{
						continue;
					}
					num += num1;
				}
			}
		}
		return LevelBox.campaignBoxes[boxIndex].starsToOpen - num;
	}

	private void HandleBackClicked(object sender, EventArgs e)
	{
		this._escapePressed = true;
	}

	private void HandleStartClicked(object sender, EventArgs e)
	{
		if (this.nguiController.selectIndexMap == 0 || this.CalculateStarsLeftToOpenTheBox(this.nguiController.selectIndexMap) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(this.nguiController.selectIndexMap))
		{
			this.StartNBox(this.nguiController.selectIndexMap);
		}
	}

	private bool IsCompliteAllLevelsToOpenTheBox(int boxIndex)
	{
		Dictionary<string, int> strs;
		if (boxIndex == 0)
		{
			return true;
		}
		bool flag = false;
		LevelBox item = LevelBox.campaignBoxes[boxIndex - 1];
		if (CampaignProgress.boxesLevelsAndStars.TryGetValue(item.name, out strs))
		{
			if (boxIndex == 1 && strs.Count >= 9)
			{
				flag = true;
			}
			if (boxIndex == 2 && strs.Count >= 6)
			{
				flag = true;
			}
			if (boxIndex == 3 && strs.Count >= 5)
			{
				flag = true;
			}
		}
		return flag;
	}

	private void LoadBoxPreviews()
	{
		for (int i = 0; i < LevelBox.campaignBoxes.Count; i++)
		{
			Texture texture = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)) as Texture;
			this.boxPreviews.Add(texture);
			Texture texture1 = Resources.Load(ResPath.Combine("Boxes", string.Concat(LevelBox.campaignBoxes[i].PreviewNAme, "_closed"))) as Texture;
			this.closedBoxPreviews.Add(texture1);
		}
	}

	private void OnDestroy()
	{
		ChooseBox.instance = null;
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		this.UnloadBoxPreviews();
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
		this._backSubscription = BackSystem.Instance.Register(() => this._escapePressed = true, "Choose Box");
	}

	private void Start()
	{
		string str;
		ChooseBox.instance = this;
		if (this.nguiController.startButton != null)
		{
			ButtonHandler component = this.nguiController.startButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += new EventHandler(this.HandleStartClicked);
			}
		}
		if (this.nguiController.backButton != null)
		{
			ButtonHandler buttonHandler = this.nguiController.backButton.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleBackClicked);
			}
		}
		StoreKitEventListener.State.Mode = "Campaign";
		StoreKitEventListener.State.Parameters.Clear();
		Debug.Log("start choosbox");
		CampaignProgressSynchronizer.Instance.Sync();
		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			ProgressSynchronizer.Instance.AuthenticateAndSynchronize(() => WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap), true);
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			ProgressSynchronizer.Instance.SynchronizeIosProgress();
			WeaponManager.sharedManager.Reset(0);
		}
		this.pointMap = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		int num = Math.Min(LevelBox.campaignBoxes.Count, this.gridTransform.childCount);
		for (int i = 0; i < num; i++)
		{
			bool flag = (this.CalculateStarsLeftToOpenTheBox(i) > 0 ? false : this.IsCompliteAllLevelsToOpenTheBox(i));
			Texture texture = (!flag ? Resources.Load<Texture>(ResPath.Combine("Boxes", string.Concat(LevelBox.campaignBoxes[i].PreviewNAme, "_closed"))) ?? Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)) : Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)));
			Transform child = this.gridTransform.GetChild(i);
			child.GetComponent<UITexture>().mainTexture = texture;
			Transform transforms = child.FindChild("NeedMoreStarsLabel");
			if (transforms == null)
			{
				Debug.LogWarning("Could not find “NeedMoreStarsLabel”.");
			}
			else if (flag || i >= LevelBox.campaignBoxes.Count - 1)
			{
				transforms.gameObject.SetActive(false);
			}
			else
			{
				transforms.gameObject.SetActive(true);
				int openTheBox = this.CalculateStarsLeftToOpenTheBox(i);
				if (this.IsCompliteAllLevelsToOpenTheBox(i) || openTheBox <= 0)
				{
					str = (openTheBox <= 0 ? LocalizationStore.Get("Key_1366") : string.Format(LocalizationStore.Get("Key_1367"), openTheBox));
				}
				else
				{
					str = string.Format(LocalizationStore.Get("Key_0241"), openTheBox);
				}
				transforms.GetComponent<UILabel>().text = str;
			}
			Transform transforms1 = child.FindChild("CaptionLabel");
			if (transforms1 == null)
			{
				Debug.LogWarning("Could not find “CaptionLabel”.");
			}
			else
			{
				transforms1.gameObject.SetActive((flag ? true : i == LevelBox.campaignBoxes.Count - 1));
			}
		}
	}

	public void StartNameBox(string _nameBox)
	{
		if (_nameBox.Equals("Box_1"))
		{
			this.StartNBox(0);
			return;
		}
		if (_nameBox.Equals("Box_2"))
		{
			if (this.CalculateStarsLeftToOpenTheBox(1) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(1))
			{
				this.StartNBox(1);
			}
			return;
		}
		if (!_nameBox.Equals("Box_3"))
		{
			return;
		}
		if (this.CalculateStarsLeftToOpenTheBox(2) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(2))
		{
			this.StartNBox(2);
		}
	}

	public void StartNBox(int n)
	{
		ButtonClickSound.Instance.PlayClick();
		CurrentCampaignGame.boXName = LevelBox.campaignBoxes[n].name;
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ChooseLevel";
		LoadConnectScene.noteToShow = null;
		Application.LoadLevel(Defs.PromSceneName);
	}

	private void UnloadBoxPreviews()
	{
		this.boxPreviews.Clear();
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		bool flag;
		if (this._escapePressed)
		{
			ButtonClickSound.Instance.PlayClick();
			FlurryPluginWrapper.LogEvent("Back to Main Menu");
			Resources.UnloadUnusedAssets();
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.noteToShow = null;
			LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
			Application.LoadLevel(Defs.PromSceneName);
			this._escapePressed = false;
		}
		if (this.nguiController.startButton != null)
		{
			GameObject gameObject = this.nguiController.startButton.gameObject;
			if (this.nguiController.selectIndexMap == 0)
			{
				flag = true;
			}
			else
			{
				flag = (this.CalculateStarsLeftToOpenTheBox(this.nguiController.selectIndexMap) > 0 ? false : this.IsCompliteAllLevelsToOpenTheBox(this.nguiController.selectIndexMap));
			}
			gameObject.SetActive(flag);
		}
	}
}