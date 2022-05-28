using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
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

	[SerializeField]
	[ReadOnly]
	private List<ProfileCup> _cups;

	[ReadOnly]
	[SerializeField]
	private LeagueItemsView _itemsView;

	private ProfileCup _selectedCup;

	private readonly Dictionary<RatingSystem.RatingLeague, string> _leaguesLKeys = new Dictionary<RatingSystem.RatingLeague, string>
	{
		{
			RatingSystem.RatingLeague.Wood,
			"Key_1953"
		},
		{
			RatingSystem.RatingLeague.Steel,
			"Key_1954"
		},
		{
			RatingSystem.RatingLeague.Gold,
			"Key_1955"
		},
		{
			RatingSystem.RatingLeague.Crystal,
			"Key_1956"
		},
		{
			RatingSystem.RatingLeague.Ruby,
			"Key_1957"
		},
		{
			RatingSystem.RatingLeague.Adamant,
			"Key_1958"
		}
	};

	[CompilerGenerated]
	private static Func<ProfileCup, bool> _003C_003Ef__am_0024cacheB;

	private void OnEnable()
	{
		_cups = GetComponentsInChildren<ProfileCup>(true).ToList();
		_itemsView = GetComponentInChildren<LeagueItemsView>(true);
		Reposition();
	}

	private void Reposition()
	{
		List<ProfileCup> cups = _cups;
		if (_003C_003Ef__am_0024cacheB == null)
		{
			_003C_003Ef__am_0024cacheB = _003CReposition_003Em__397;
		}
		_selectedCup = cups.FirstOrDefault(_003C_003Ef__am_0024cacheB);
		StartCoroutine(PositionToCurrentLeagueCoroutine());
	}

	private IEnumerator PositionToCurrentLeagueCoroutine()
	{
		yield return null;
		List<ProfileCup> cups = _cups;
		if (_003CPositionToCurrentLeagueCoroutine_003Ec__Iterator17B._003C_003Ef__am_0024cache4 == null)
		{
			_003CPositionToCurrentLeagueCoroutine_003Ec__Iterator17B._003C_003Ef__am_0024cache4 = _003CPositionToCurrentLeagueCoroutine_003Ec__Iterator17B._003C_003Em__398;
		}
		ProfileCup to = cups.FirstOrDefault(_003CPositionToCurrentLeagueCoroutine_003Ec__Iterator17B._003C_003Ef__am_0024cache4);
		_centerOnChild.CenterOn(to.gameObject.transform);
		yield return null;
		SetInfoFromLeague(to.League);
	}

	public void CupCentered(ProfileCup cup)
	{
		_selectedCup = cup;
		SetInfoFromLeague(cup.League);
		_itemsView.Repaint(cup.League);
	}

	private void SetInfoFromLeague(RatingSystem.RatingLeague league)
	{
		string text = LocalizationStore.Get(_leaguesLKeys[league]);
		_lblLeagueName.text = text;
		_lblLeagueNameOutline.text = text;
		if (league < RatingSystem.instance.currentLeague)
		{
			_progressGO.SetActive(false);
			_progressTextLabel.gameObject.SetActive(true);
			_progressTextLabel.text = LocalizationStore.Get("Key_2173");
		}
		else if (league > RatingSystem.instance.currentLeague)
		{
			_progressGO.SetActive(false);
			_progressTextLabel.gameObject.SetActive(true);
			int num = RatingSystem.instance.MaxRatingInLeague(league - 1) - RatingSystem.instance.currentRating;
			_progressTextLabel.text = string.Format(LocalizationStore.Get("Key_2172"), num);
		}
		else if (league == (RatingSystem.RatingLeague)RiliExtensions.EnumNumbers<RatingSystem.RatingLeague>().Max())
		{
			_progressGO.SetActive(false);
			_progressTextLabel.gameObject.SetActive(true);
			_progressTextLabel.text = LocalizationStore.Get("Key_2249");
		}
		else
		{
			_progressGO.SetActive(true);
			_progressTextLabel.gameObject.SetActive(false);
			int num2 = RatingSystem.instance.MaxRatingInLeague(league);
			_lblScore.text = string.Format("{0}/{1}", RatingSystem.instance.currentRating, num2);
			_sprScoreBar.fillAmount = (float)RatingSystem.instance.currentRating / (float)num2;
		}
	}

	[CompilerGenerated]
	private static bool _003CReposition_003Em__397(ProfileCup c)
	{
		return c.League == RatingSystem.instance.currentLeague;
	}
}
