using System;
using UnityEngine;

internal sealed class DeveloperConsoleView : MonoBehaviour
{
	public static DeveloperConsoleView instance;

	public UIToggle strongDeivceCheckbox;

	public UIInput gemsInput;

	public UIToggle set60FpsCheckbox;

	public UIToggle mouseCOntrollCheckbox;

	public UIToggle spectatorModeCheckbox;

	public UIToggle fbRewardCheckbox;

	public UIToggle tempGunCheckbox;

	public UILabel levelLabel;

	public UILabel experienceLabel;

	public UISlider experienceSlider;

	public UISlider levelSlider;

	public UILabel ratingLabel;

	public UISlider ratingSlider;

	public UIInput coinsInput;

	public UIInput enemyCountInSurvivalWave;

	public UIInput enemiesInCampaignInput;

	public UIToggle trainingCheckbox;

	public UIToggle downgradeResolutionCheckbox;

	public UIToggle isPayingCheckbox;

	public UIToggle isDebugGuiVisibleCheckbox;

	public UIToggle isEventX3ForcedCheckbox;

	public UIToggle adIdCheckbox;

	public UIInput marathonCurrentDay;

	public UIToggle marathonTestMode;

	public UIToggle gameGUIOffMode;

	public UILabel deviceInfo;

	public UILabel diagonalInfo;

	public UIInput devicePushTokenInput;

	public UIInput playerIdInput;

	public UILabel starterPackLive;

	public UILabel starterPackCooldown;

	public UILabel socialUsername;

	public UIInput oneDayPreminAccount;

	public UIInput threeDayPreminAccount;

	public UIInput sevenDayPreminAccount;

	public UIInput monthDayPreminAccount;

	public UIToggle memoryCheckbox;

	public UIToggle isPixelGunLowCheckbox;

	public UIToggle reviewCheckbox;

	public int CoinsInput
	{
		set
		{
			if (this.coinsInput != null)
			{
				this.coinsInput.@value = value.ToString();
			}
		}
	}

	public string DevicePushTokenInput
	{
		set
		{
			if (this.devicePushTokenInput != null)
			{
				this.devicePushTokenInput.@value = value;
			}
		}
	}

	public int EnemiesInCampaignInput
	{
		set
		{
			if (this.enemiesInCampaignInput != null)
			{
				this.enemiesInCampaignInput.@value = value.ToString();
			}
		}
	}

	public int EnemiesInSurvivalWaveInput
	{
		set
		{
			if (this.enemyCountInSurvivalWave != null)
			{
				this.enemyCountInSurvivalWave.@value = value.ToString();
			}
		}
	}

	public string ExperienceLabel
	{
		set
		{
			if (this.experienceLabel == null)
			{
				return;
			}
			this.experienceLabel.text = value;
		}
	}

	public float ExperiencePercentage
	{
		get
		{
			return (this.experienceSlider == null ? 0f : this.experienceSlider.@value);
		}
		set
		{
			if (this.experienceSlider == null)
			{
				return;
			}
			this.experienceSlider.@value = Mathf.Clamp01(value);
		}
	}

	public bool GameGUIOffMode
	{
		set
		{
			if (this.gameGUIOffMode != null)
			{
				this.gameGUIOffMode.@value = value;
			}
		}
	}

	public int GemsInput
	{
		set
		{
			if (this.gemsInput != null)
			{
				this.gemsInput.@value = value.ToString();
			}
		}
	}

	public bool IsPayingUser
	{
		set
		{
			if (this.isPayingCheckbox != null)
			{
				this.isPayingCheckbox.@value = value;
			}
		}
	}

	public string LevelLabel
	{
		set
		{
			if (this.levelLabel == null)
			{
				return;
			}
			this.levelLabel.text = value;
		}
	}

	public float LevelPercentage
	{
		get
		{
			return (this.levelSlider == null ? 0f : this.levelSlider.@value);
		}
		set
		{
			if (this.levelSlider == null)
			{
				return;
			}
			this.levelSlider.@value = Mathf.Clamp01(value);
		}
	}

	public int MarathonDayInput
	{
		set
		{
			if (this.marathonCurrentDay != null)
			{
				this.marathonCurrentDay.@value = value.ToString();
			}
		}
	}

	public bool MarathonTestMode
	{
		set
		{
			if (this.marathonTestMode != null)
			{
				this.marathonTestMode.@value = value;
			}
		}
	}

	public bool MemoryInfoActive
	{
		get
		{
			if (!this.memoryCheckbox)
			{
				return false;
			}
			return this.memoryCheckbox.@value;
		}
		set
		{
			if (this.memoryCheckbox)
			{
				this.memoryCheckbox.@value = value;
			}
		}
	}

	public string PlayerIdInput
	{
		set
		{
			if (this.playerIdInput != null)
			{
				this.playerIdInput.@value = value;
			}
		}
	}

	public string RatingLabel
	{
		set
		{
			if (this.ratingLabel == null)
			{
				return;
			}
			this.ratingLabel.text = value;
		}
	}

	public float RatingPercentage
	{
		get
		{
			return (this.ratingSlider == null ? 0f : this.ratingSlider.@value);
		}
		set
		{
			if (this.ratingSlider == null)
			{
				return;
			}
			this.ratingSlider.@value = Mathf.Clamp01(value);
		}
	}

	public bool ReviewActive
	{
		get
		{
			if (!this.reviewCheckbox)
			{
				return false;
			}
			return this.reviewCheckbox.@value;
		}
		set
		{
			if (this.reviewCheckbox)
			{
				this.reviewCheckbox.@value = value;
			}
		}
	}

	public bool Set60FPSActive
	{
		set
		{
			if (this.set60FpsCheckbox != null)
			{
				this.set60FpsCheckbox.@value = value;
			}
		}
	}

	public bool SetFBReward
	{
		set
		{
			if (this.fbRewardCheckbox != null)
			{
				this.fbRewardCheckbox.@value = value;
			}
		}
	}

	public bool SetMouseControll
	{
		set
		{
			if (this.mouseCOntrollCheckbox != null)
			{
				this.mouseCOntrollCheckbox.@value = value;
			}
		}
	}

	public bool SetSpectatorMode
	{
		set
		{
			if (this.spectatorModeCheckbox != null)
			{
				this.spectatorModeCheckbox.@value = value;
			}
		}
	}

	public string SocialUserName
	{
		set
		{
			if (this.socialUsername != null)
			{
				this.socialUsername.text = value;
			}
		}
	}

	public bool StrongDevice
	{
		set
		{
			if (this.strongDeivceCheckbox != null)
			{
				this.strongDeivceCheckbox.@value = value;
			}
		}
	}

	public bool TempGunActive
	{
		set
		{
			if (this.tempGunCheckbox != null)
			{
				this.tempGunCheckbox.@value = value;
			}
		}
	}

	public bool TrainingCompleted
	{
		set
		{
			if (this.trainingCheckbox != null)
			{
				this.trainingCheckbox.@value = value;
			}
		}
	}

	public DeveloperConsoleView()
	{
	}

	private void Awake()
	{
		DeveloperConsoleView.instance = this;
		this.diagonalInfo.text = string.Concat("Диагональ: ", Defs.ScreenDiagonal);
	}

	private void OnDestroy()
	{
		DeveloperConsoleView.instance = null;
	}
}