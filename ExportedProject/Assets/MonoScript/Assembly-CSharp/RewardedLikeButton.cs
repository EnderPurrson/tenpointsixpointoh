using Facebook.Unity;
using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class RewardedLikeButton : MonoBehaviour
{
	private const int RewardGemsCount = 10;

	internal const string RewardKey = "RewardForLikeGained";

	public UIButton rewardedLikeButton;

	public UILabel rewardedLikeCaption;

	public RewardedLikeButton()
	{
	}

	public void OnClick()
	{
		Application.OpenURL("https://www.facebook.com/PixelGun3DOfficial");
		try
		{
			TutorialQuestManager.Instance.AddFulfilledQuest("likeFacebook");
			QuestMediator.NotifySocialInteraction("likeFacebook");
			if (Storager.getInt("RewardForLikeGained", true) <= 0)
			{
				Storager.setInt("RewardForLikeGained", 1, true);
				int num = Storager.getInt("GemsCurrency", false);
				Storager.setInt("GemsCurrency", num + 10, false);
				AnalyticsFacade.CurrencyAccrual(10, "GemsCurrency", AnalyticsConstants.AccrualType.Earned);
				FlurryEvents.LogGemsGained(FlurryEvents.GetPlayingMode(), 10);
				AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>()
				{
					{ "Like Facebook Page", "Likes" }
				});
				CoinsMessage.FireCoinsAddedEvent(true, 2);
			}
		}
		finally
		{
			this.Refresh();
		}
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.RefreshRewardedLikeButton));
	}

	private void OnEnable()
	{
		this.Refresh();
	}

	internal void Refresh()
	{
		if (!FacebookController.FacebookSupported)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (this.rewardedLikeButton == null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (Storager.hasKey("RewardForLikeGained") && Storager.getInt("RewardForLikeGained", true) > 0)
		{
			UnityEngine.Object.Destroy(this.rewardedLikeButton.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (!FB.IsLoggedIn)
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		if (!Storager.hasKey(Defs.IsFacebookLoginRewardaGained) || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0)
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		if (ExpController.LobbyLevel <= 1)
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		if (MainMenuController.SavedShwonLobbyLevelIsLessThanActual())
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		this.RefreshRewardedLikeButton();
		this.rewardedLikeButton.gameObject.SetActive(true);
	}

	private void RefreshRewardedLikeButton()
	{
		if (this.rewardedLikeCaption == null)
		{
			UnityEngine.Debug.LogError("rewardedLikeCaption == null");
			return;
		}
		try
		{
			string str = LocalizationStore.Get("Key_1653");
			this.rewardedLikeCaption.text = string.Format(str, 10);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		RewardedLikeButton.u003cStartu003ec__Iterator189 variable = null;
		return variable;
	}
}