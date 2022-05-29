using Rilisoft;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class CampaignLoading : MonoBehaviour
{
	public readonly static string DesignersTestMap;

	public UITexture backgroundUiTexture;

	public GameObject survivalNotesOverlay;

	public GameObject campaignNotesOverlay;

	public GameObject trainingNotesOverlay;

	public GameObject ordinaryAwardLabel;

	public GameObject stackOfCoinsLabel;

	public Texture loadingNote;

	private Texture fonToDraw;

	private Texture plashkaCoins;

	private Rect plashkaCoinsRect;

	static CampaignLoading()
	{
		CampaignLoading.DesignersTestMap = "Coliseum";
	}

	public CampaignLoading()
	{
	}

	[Obfuscation(Exclude=true)]
	private void Load()
	{
		if (!Defs.IsSurvival)
		{
			Singleton<SceneLoader>.Instance.LoadScene((TrainingController.TrainingCompleted ? CurrentCampaignGame.levelSceneName : "Training"), LoadSceneMode.Single);
		}
		else
		{
			Singleton<SceneLoader>.Instance.LoadScene(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % (int)Defs.SurvivalMaps.Length], LoadSceneMode.Single);
		}
	}

	private void Start()
	{
		string str;
		string str1;
		ActivityIndicator.IsActiveIndicator = true;
		if (Defs.IsSurvival)
		{
			str = "gey_surv";
			if (this.survivalNotesOverlay != null)
			{
				this.survivalNotesOverlay.SetActive(true);
			}
		}
		else if (!TrainingController.TrainingCompleted)
		{
			str = "Restore";
			if (this.trainingNotesOverlay != null)
			{
				this.trainingNotesOverlay.SetActive(true);
			}
		}
		else
		{
			int num = 0;
			LevelBox levelBox = null;
			foreach (LevelBox campaignBox in LevelBox.campaignBoxes)
			{
				if (!campaignBox.name.Equals(CurrentCampaignGame.boXName))
				{
					continue;
				}
				levelBox = campaignBox;
				foreach (CampaignLevel level in campaignBox.levels)
				{
					if (!level.sceneName.Equals(CurrentCampaignGame.levelSceneName))
					{
						continue;
					}
					num = campaignBox.levels.IndexOf(level);
					break;
				}
			}
			bool count = false;
			count = num >= levelBox.levels.Count - 1;
			bool flag = false;
			if (!CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName].ContainsKey(CurrentCampaignGame.levelSceneName))
			{
				flag = true;
			}
			bool flag1 = (!flag ? false : count);
			str = (!flag1 ? "gey_1" : "gey_15");
			if (this.ordinaryAwardLabel != null)
			{
				this.ordinaryAwardLabel.SetActive(!flag1);
			}
			if (this.stackOfCoinsLabel != null)
			{
				this.stackOfCoinsLabel.SetActive(flag1);
			}
			if (this.campaignNotesOverlay != null)
			{
				this.campaignNotesOverlay.SetActive(true);
			}
		}
		this.plashkaCoins = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", str));
		float coef = (float)((!TrainingController.TrainingCompleted ? 484 : 500)) * Defs.Coef;
		float single = (float)((!TrainingController.TrainingCompleted ? 279 : 244)) * Defs.Coef;
		this.plashkaCoinsRect = new Rect(((float)Screen.width - coef) / 2f, (float)Screen.height * 0.8f - single / 2f, coef, single);
		if (TrainingController.TrainingCompleted)
		{
			str1 = (!Defs.IsSurvival ? CurrentCampaignGame.levelSceneName : Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % (int)Defs.SurvivalMaps.Length]);
		}
		else
		{
			str1 = "Training";
		}
		string str2 = string.Concat("Loading_", str1);
		this.fonToDraw = Resources.Load<Texture>(ResPath.Combine(string.Concat(Switcher.LoadingInResourcesPath, (!Device.isRetinaAndStrong ? string.Empty : "/Hi")), str2));
		if (this.backgroundUiTexture != null)
		{
			this.backgroundUiTexture.mainTexture = this.fonToDraw;
		}
		base.Invoke("Load", 2f);
	}
}