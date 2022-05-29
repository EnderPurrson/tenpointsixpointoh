using Rilisoft;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotToNextLevel : MonoBehaviour
{
	private Action OnPlayerAddedAct;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private bool runLoading;

	private IDisposable _backSubscription;

	public GotToNextLevel()
	{
	}

	private void Awake()
	{
		this.OnPlayerAddedAct = () => {
			this._player = GameObject.FindGameObjectWithTag("Player");
			this._playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		};
		Initializer.PlayerAddedEvent += this.OnPlayerAddedAct;
	}

	private void BannerTrainingCompleteInvoke()
	{
		GameObject.Find("Background_Training(Clone)").SetActive(false);
		coinsShop.hideCoinsShop();
		if (FriendsController.useBuffSystem)
		{
			Storager.setInt("Training.NoviceArmorUsedKey", 1, false);
		}
		GameObject child = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/TrainigCompleteNGUI"));
		RewardWindowBase component = child.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority storyPriority = FacebookController.StoryPriority.Green;
		component.CollectOnlyNoShare = true;
		component.shareAction = () => FacebookController.PostOpenGraphStory("complete", "tutorial", storyPriority, null);
		component.customHide = () => {
			this.GetRewardForTraining();
			ActivityIndicator.IsActiveIndicator = true;
			base.Invoke("LoadPromLevel", 0.4f);
		};
		component.priority = storyPriority;
		component.twitterStatus = () => "Training completed in @PixelGun3D! Come to play with me! \n#pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
		component.EventTitle = "Training Completed";
		component.HasReward = true;
		child.transform.parent = InGameGUI.sharedInGameGUI.transform.GetChild(0);
		InGameGUI.sharedInGameGUI.joystikPanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.interfacePanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.shopPanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.bloodPanel.gameObject.SetActive(false);
		Player_move_c.SetLayerRecursively(child, LayerMask.NameToLayer("NGUI"));
		child.transform.localPosition = new Vector3(0f, 0f, -130f);
		child.transform.localRotation = Quaternion.identity;
		child.transform.localScale = new Vector3(1f, 1f, 1f);
		UITexture uITexture = (new GameObject()).AddComponent<UITexture>();
		uITexture.mainTexture = Resources.Load<Texture>(ConnectSceneNGUIController.MainLoadingTexture());
		uITexture.SetRect(0f, 0f, 1366f, 768f);
		uITexture.transform.SetParent(InGameGUI.sharedInGameGUI.transform.GetChild(0), false);
		uITexture.transform.localScale = Vector3.one;
		uITexture.transform.localPosition = Vector3.zero;
	}

	private void GetRewardForTraining()
	{
		TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted;
		AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Rewards, true);
		Storager.setInt("GrenadeID", 5, false);
		PlayerPrefs.Save();
		LevelCompleteLoader.sceneName = Defs.MainMenuScene;
		HashSet<RuntimePlatform> runtimePlatforms = new HashSet<RuntimePlatform>();
		runtimePlatforms.Add(RuntimePlatform.Android);
		runtimePlatforms.Add(RuntimePlatform.IPhonePlayer);
		runtimePlatforms.Add(RuntimePlatform.MetroPlayerX64);
		if (runtimePlatforms.Contains(BuildSettings.BuildTargetPlatform) && !Storager.hasKey(Defs.GotCoinsForTraining))
		{
			if (!(new Lazy<bool>(() => {
				string str;
				int num;
				if (!Storager.UseSignedPreferences)
				{
					return false;
				}
				if (!Defs2.SignedPreferences.TryGetValue("Manterry", out str))
				{
					return false;
				}
				if (!Defs2.SignedPreferences.Verify("Manterry"))
				{
					return true;
				}
				if (!int.TryParse(str, out num))
				{
					return true;
				}
				return num % 2 == 1;
			})).Value)
			{
				int gemsForTraining = Defs.GemsForTraining;
				BankController.AddGems(gemsForTraining, false, AnalyticsConstants.AccrualType.Earned);
				FlurryEvents.LogGemsGained("Training", gemsForTraining);
				int coinsForTraining = Defs.CoinsForTraining;
				BankController.AddCoins(coinsForTraining, false, AnalyticsConstants.AccrualType.Earned);
				FlurryEvents.LogCoinsGained("Training", coinsForTraining);
				AudioClip audioClip = Resources.Load<AudioClip>("coin_get");
				if (Defs.isSoundFX)
				{
					NGUITools.PlaySound(audioClip);
				}
				if (Storager.UseSignedPreferences)
				{
					int num1 = UnityEngine.Random.Range(255, 32767) << 2;
					Defs2.SignedPreferences.Add("Manterry", num1.ToString(CultureInfo.InvariantCulture));
				}
			}
			else if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Skipping reward since it has been already claimed.");
			}
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.addExperience(Defs.ExpForTraining);
			}
		}
		MainMenuController.trainingCompleted = true;
		ShopNGUIController.GiveArmorArmy1OrNoviceArmor();
		FlurryPluginWrapper.LogEventToAppsFlyer("Training complete", new Dictionary<string, string>());
		FlurryEvents.LogTrainingProgress("Complete");
		TrainingController.TrainingCompletedFlagForLogging = new bool?(true);
	}

	public static void GoToNextLevel()
	{
		LevelCompleteLoader.action = null;
		LevelCompleteLoader.sceneName = "LevelComplete";
		AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
	}

	private void GoToNextLevelInstance()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Touching the Portal");
		if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
		{
			GotToNextLevel.GoToNextLevel();
		}
		else
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Portal, true);
			base.Invoke("BannerTrainingCompleteInvoke", 2f);
			AutoFade.fadeKilled(2.05f, 0f, 0f, Color.white);
		}
	}

	private void HandleEscape()
	{
		if (Application.isEditor)
		{
			Debug.Log("Ignoring [Escape] after touching the portal.");
		}
	}

	private void LoadPromLevel()
	{
		Singleton<SceneLoader>.Instance.LoadScene("LevelToCompleteProm", LoadSceneMode.Single);
	}

	private void OnDestroy()
	{
		Initializer.PlayerAddedEvent -= this.OnPlayerAddedAct;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(false);
		}
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void Update()
	{
		if (this._player == null || this._playerMoveC == null)
		{
			return;
		}
		if (this.runLoading)
		{
			return;
		}
		if (Vector3.SqrMagnitude(base.transform.position - this._player.transform.position) < 2.25f)
		{
			this.runLoading = true;
			this.GoToNextLevelInstance();
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(true);
			}
		}
	}
}