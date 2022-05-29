using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeaguesGUIController : MonoBehaviour
{
	[SerializeField]
	private UICenterOnChild _centerOnChild;

	[SerializeField]
	private UILabel _lblLeagueName;

	[SerializeField]
	private UILabel _lblLeagueNameOutline;

	[SerializeField]
	private UILabel _lblScore;

	[SerializeField]
	private UISprite _sprScoreBar;

	[SerializeField]
	private GameObject _progressGO;

	[SerializeField]
	private UILabel _progressTextLabel;

	[ReadOnly]
	[SerializeField]
	private List<ProfileCup> _cups;

	[ReadOnly]
	[SerializeField]
	private LeagueItemsView _itemsView;

	private ProfileCup _selectedCup;

	private readonly Dictionary<RatingSystem.RatingLeague, string> _leaguesLKeys = new Dictionary<RatingSystem.RatingLeague, string>()
	{
		{ RatingSystem.RatingLeague.Wood, "Key_1953" },
		{ RatingSystem.RatingLeague.Steel, "Key_1954" },
		{ RatingSystem.RatingLeague.Gold, "Key_1955" },
		{ RatingSystem.RatingLeague.Crystal, "Key_1956" },
		{ RatingSystem.RatingLeague.Ruby, "Key_1957" },
		{ RatingSystem.RatingLeague.Adamant, "Key_1958" }
	};

	public LeaguesGUIController()
	{
	}

	public void CupCentered(ProfileCup cup)
	{
		this._selectedCup = cup;
		this.SetInfoFromLeague(cup.League);
		this._itemsView.Repaint(cup.League);
	}

	private void OnEnable()
	{
		this._cups = base.GetComponentsInChildren<ProfileCup>(true).ToList<ProfileCup>();
		this._itemsView = base.GetComponentInChildren<LeagueItemsView>(true);
		this.Reposition();
	}

	[DebuggerHidden]
	private IEnumerator PositionToCurrentLeagueCoroutine()
	{
		LeaguesGUIController.u003cPositionToCurrentLeagueCoroutineu003ec__Iterator17B variable = null;
		return variable;
	}

	private void Reposition()
	{
		this._selectedCup = this._cups.FirstOrDefault<ProfileCup>((ProfileCup c) => c.League == RatingSystem.instance.currentLeague);
		base.StartCoroutine(this.PositionToCurrentLeagueCoroutine());
	}

	private void SetInfoFromLeague(RatingSystem.RatingLeague league)
	{
		string str = LocalizationStore.Get(this._leaguesLKeys[league]);
		this._lblLeagueName.text = str;
		this._lblLeagueNameOutline.text = str;
		if (league < RatingSystem.instance.currentLeague)
		{
			this._progressGO.SetActive(false);
			this._progressTextLabel.gameObject.SetActive(true);
			this._progressTextLabel.text = LocalizationStore.Get("Key_2173");
		}
		else if (league > RatingSystem.instance.currentLeague)
		{
			this._progressGO.SetActive(false);
			this._progressTextLabel.gameObject.SetActive(true);
			int num = RatingSystem.instance.MaxRatingInLeague((int)league - (int)RatingSystem.RatingLeague.Steel) - RatingSystem.instance.currentRating;
			this._progressTextLabel.text = string.Format(LocalizationStore.Get("Key_2172"), num);
		}
		else if ((int)league != RiliExtensions.EnumNumbers<RatingSystem.RatingLeague>().Max())
		{
			this._progressGO.SetActive(true);
			this._progressTextLabel.gameObject.SetActive(false);
			int num1 = RatingSystem.instance.MaxRatingInLeague(league);
			this._lblScore.text = string.Format("{0}/{1}", RatingSystem.instance.currentRating, num1);
			this._sprScoreBar.fillAmount = (float)RatingSystem.instance.currentRating / (float)num1;
		}
		else
		{
			this._progressGO.SetActive(false);
			this._progressTextLabel.gameObject.SetActive(true);
			this._progressTextLabel.text = LocalizationStore.Get("Key_2249");
		}
	}
}