using Rilisoft;
using System;
using UnityEngine;

public class RatingPanel : MonoBehaviour
{
	public GameObject leaguePanel;

	public UISprite cup;

	public UILabel leagueLabel;

	public UILabel ratingLabel;

	[SerializeField]
	private ButtonHandler _btnOpenProfile;

	public RatingPanel()
	{
	}

	private void OnBtnOpenProfileClicked(object sender, EventArgs e)
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.GoToProfile();
		}
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SetStaticticTab(StatisticHUD.TypeOpenTab.leagues);
		}
	}

	private void OnDisable()
	{
		if (this._btnOpenProfile != null)
		{
			this._btnOpenProfile.Clicked -= new EventHandler(this.OnBtnOpenProfileClicked);
		}
		if (FriendsController.isUseRatingSystem)
		{
			RatingSystem.instance.OnRatingUpdate -= new RatingSystem.RatingUpdate(this.UpdateInfo);
		}
	}

	private void OnEnable()
	{
		this.UpdateInfo();
		if (FriendsController.isUseRatingSystem)
		{
			RatingSystem.instance.OnRatingUpdate += new RatingSystem.RatingUpdate(this.UpdateInfo);
		}
	}

	private void UpdateInfo()
	{
		if (!FriendsController.isUseRatingSystem || !TrainingController.TrainingCompleted)
		{
			this.leaguePanel.SetActive(false);
			return;
		}
		this.leaguePanel.SetActive(true);
		UISprite uISprite = this.cup;
		string str = RatingSystem.instance.currentLeague.ToString();
		int num = 3 - RatingSystem.instance.currentDivision;
		uISprite.spriteName = string.Concat(str, " ", num.ToString());
		if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
		{
			this.leagueLabel.text = LocalizationStore.Get(RatingSystem.leagueLocalizations[(int)RatingSystem.instance.currentLeague]);
		}
		else
		{
			this.leagueLabel.text = string.Concat(LocalizationStore.Get(RatingSystem.leagueLocalizations[(int)RatingSystem.instance.currentLeague]), " ", RatingSystem.divisionByIndex[RatingSystem.instance.currentDivision]);
		}
		this.ratingLabel.text = RatingSystem.instance.currentRating.ToString();
		if (this._btnOpenProfile != null)
		{
			this._btnOpenProfile.Clicked += new EventHandler(this.OnBtnOpenProfileClicked);
		}
	}
}