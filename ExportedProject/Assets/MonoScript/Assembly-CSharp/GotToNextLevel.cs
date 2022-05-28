using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class GotToNextLevel : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CBannerTrainingCompleteInvoke_003Ec__AnonStorey275
	{
		internal FacebookController.StoryPriority priority;

		internal GotToNextLevel _003C_003Ef__this;

		internal void _003C_003Em__171()
		{
			FacebookController.PostOpenGraphStory("complete", "tutorial", priority);
		}

		internal void _003C_003Em__172()
		{
			_003C_003Ef__this.GetRewardForTraining();
			ActivityIndicator.IsActiveIndicator = true;
			_003C_003Ef__this.Invoke("LoadPromLevel", 0.4f);
		}
	}

	private Action OnPlayerAddedAct;

	private GameObject _player;

	private Player_move_c _playerMoveC;

	private bool runLoading;

	private IDisposable _backSubscription;

	[CompilerGenerated]
	private static Func<bool> _003C_003Ef__am_0024cache5;

	[CompilerGenerated]
	private static Func<string> _003C_003Ef__am_0024cache6;

	private void Awake()
	{
		OnPlayerAddedAct = _003CAwake_003Em__16F;
		Initializer.PlayerAddedEvent += OnPlayerAddedAct;
	}

	private void OnDestroy()
	{
		Initializer.PlayerAddedEvent -= OnPlayerAddedAct;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(false);
		}
	}

	private void Update()
	{
		if (!(_player == null) && !(_playerMoveC == null) && !runLoading && Vector3.SqrMagnitude(base.transform.position - _player.transform.position) < 2.25f)
		{
			runLoading = true;
			GoToNextLevelInstance();
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(true);
			}
		}
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void HandleEscape()
	{
		if (Application.isEditor)
		{
			Debug.Log("Ignoring [Escape] after touching the portal.");
		}
	}

	private void GoToNextLevelInstance()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Touching the Portal");
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Portal);
			Invoke("BannerTrainingCompleteInvoke", 2f);
			AutoFade.fadeKilled(2.05f, 0f, 0f, Color.white);
		}
		else
		{
			GoToNextLevel();
		}
	}

	private void GetRewardForTraining()
	{
		TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted;
		AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Rewards);
		Storager.setInt("GrenadeID", 5, false);
		PlayerPrefs.Save();
		LevelCompleteLoader.sceneName = Defs.MainMenuScene;
		HashSet<RuntimePlatform> hashSet = new HashSet<RuntimePlatform>();
		hashSet.Add(RuntimePlatform.Android);
		hashSet.Add(RuntimePlatform.IPhonePlayer);
		hashSet.Add(RuntimePlatform.MetroPlayerX64);
		HashSet<RuntimePlatform> hashSet2 = hashSet;
		if (hashSet2.Contains(BuildSettings.BuildTargetPlatform) && !Storager.hasKey(Defs.GotCoinsForTraining))
		{
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003CGetRewardForTraining_003Em__170;
			}
			if (!new Lazy<bool>(_003C_003Ef__am_0024cache5).Value)
			{
				int gemsForTraining = Defs.GemsForTraining;
				BankController.AddGems(gemsForTraining, false);
				FlurryEvents.LogGemsGained("Training", gemsForTraining);
				int coinsForTraining = Defs.CoinsForTraining;
				BankController.AddCoins(coinsForTraining, false);
				FlurryEvents.LogCoinsGained("Training", coinsForTraining);
				AudioClip clip = Resources.Load<AudioClip>("coin_get");
				if (Defs.isSoundFX)
				{
					NGUITools.PlaySound(clip);
				}
				if (Storager.UseSignedPreferences)
				{
					int num = UnityEngine.Random.Range(255, 32767) << 2;
					Defs2.SignedPreferences.Add("Manterry", num.ToString(CultureInfo.InvariantCulture));
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
		TrainingController.TrainingCompletedFlagForLogging = true;
	}

	private void BannerTrainingCompleteInvoke()
	{
		_003CBannerTrainingCompleteInvoke_003Ec__AnonStorey275 _003CBannerTrainingCompleteInvoke_003Ec__AnonStorey = new _003CBannerTrainingCompleteInvoke_003Ec__AnonStorey275();
		_003CBannerTrainingCompleteInvoke_003Ec__AnonStorey._003C_003Ef__this = this;
		GameObject.Find("Background_Training(Clone)").SetActive(false);
		coinsShop.hideCoinsShop();
		if (FriendsController.useBuffSystem)
		{
			Storager.setInt("Training.NoviceArmorUsedKey", 1, false);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/TrainigCompleteNGUI"));
		RewardWindowBase component = gameObject.GetComponent<RewardWindowBase>();
		_003CBannerTrainingCompleteInvoke_003Ec__AnonStorey.priority = FacebookController.StoryPriority.Green;
		component.CollectOnlyNoShare = true;
		component.shareAction = _003CBannerTrainingCompleteInvoke_003Ec__AnonStorey._003C_003Em__171;
		component.customHide = _003CBannerTrainingCompleteInvoke_003Ec__AnonStorey._003C_003Em__172;
		component.priority = _003CBannerTrainingCompleteInvoke_003Ec__AnonStorey.priority;
		if (_003C_003Ef__am_0024cache6 == null)
		{
			_003C_003Ef__am_0024cache6 = _003CBannerTrainingCompleteInvoke_003Em__173;
		}
		component.twitterStatus = _003C_003Ef__am_0024cache6;
		component.EventTitle = "Training Completed";
		component.HasReward = true;
		gameObject.transform.parent = InGameGUI.sharedInGameGUI.transform.GetChild(0);
		InGameGUI.sharedInGameGUI.joystikPanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.interfacePanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.shopPanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.bloodPanel.gameObject.SetActive(false);
		Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("NGUI"));
		gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		GameObject gameObject2 = new GameObject();
		UITexture uITexture = gameObject2.AddComponent<UITexture>();
		string path = ConnectSceneNGUIController.MainLoadingTexture();
		uITexture.mainTexture = Resources.Load<Texture>(path);
		uITexture.SetRect(0f, 0f, 1366f, 768f);
		uITexture.transform.SetParent(InGameGUI.sharedInGameGUI.transform.GetChild(0), false);
		uITexture.transform.localScale = Vector3.one;
		uITexture.transform.localPosition = Vector3.zero;
	}

	private void LoadPromLevel()
	{
		Singleton<SceneLoader>.Instance.LoadScene("LevelToCompleteProm");
	}

	public static void GoToNextLevel()
	{
		LevelCompleteLoader.action = null;
		LevelCompleteLoader.sceneName = "LevelComplete";
		AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
	}

	[CompilerGenerated]
	private void _003CAwake_003Em__16F()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
	}

	[CompilerGenerated]
	private static bool _003CGetRewardForTraining_003Em__170()
	{
		if (!Storager.UseSignedPreferences)
		{
			return false;
		}
		string value;
		if (!Defs2.SignedPreferences.TryGetValue("Manterry", out value))
		{
			return false;
		}
		if (!Defs2.SignedPreferences.Verify("Manterry"))
		{
			return true;
		}
		int result;
		if (!int.TryParse(value, out result))
		{
			return true;
		}
		return result % 2 == 1;
	}

	[CompilerGenerated]
	private static string _003CBannerTrainingCompleteInvoke_003Em__173()
	{
		return "Training completed in @PixelGun3D! Come to play with me! \n#pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
	}
}
