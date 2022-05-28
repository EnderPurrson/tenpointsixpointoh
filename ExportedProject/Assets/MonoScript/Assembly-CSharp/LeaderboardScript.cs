using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class LeaderboardScript : MonoBehaviour
{
	private enum GridState
	{
		Empty = 0,
		FillingWithCache = 1,
		Cache = 2,
		FillingWithResponse = 3,
		Response = 4
	}

	[CompilerGenerated]
	private sealed class _003CFillIndividualItem_003Ec__AnonStorey2B8
	{
		private sealed class _003CFillIndividualItem_003Ec__AnonStorey2B6
		{
			internal ClickedEventArgs e;

			internal _003CFillIndividualItem_003Ec__AnonStorey2B8 _003C_003Ef__ref_0024696;

			internal void _003C_003Em__344(EventHandler<ClickedEventArgs> handler)
			{
				handler(_003C_003Ef__ref_0024696._003C_003Ef__this, e);
			}
		}

		internal LeaderboardItemViewModel item;

		internal LeaderboardScript _003C_003Ef__this;

		internal void _003C_003Em__2E5(object sender, ClickedEventArgs e)
		{
			_003CFillIndividualItem_003Ec__AnonStorey2B6 _003CFillIndividualItem_003Ec__AnonStorey2B = new _003CFillIndividualItem_003Ec__AnonStorey2B6();
			_003CFillIndividualItem_003Ec__AnonStorey2B._003C_003Ef__ref_0024696 = this;
			_003CFillIndividualItem_003Ec__AnonStorey2B.e = e;
			LeaderboardScript.PlayerClicked.Do(_003CFillIndividualItem_003Ec__AnonStorey2B._003C_003Em__344);
			if (Application.isEditor && Defs.IsDeveloperBuild)
			{
				Debug.Log(string.Format("Clicked: {0}", _003CFillIndividualItem_003Ec__AnonStorey2B.e.Id));
			}
		}

		internal void _003C_003Em__2EE(UILabel p)
		{
			p.text = ((item.Place <= 3) ? string.Empty : item.Place.ToString(CultureInfo.InvariantCulture));
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillIndividualItem_003Ec__AnonStorey2B7
	{
		internal int p;

		internal _003CFillIndividualItem_003Ec__AnonStorey2B8 _003C_003Ef__ref_0024696;

		internal void _003C_003Em__2EC(Transform l)
		{
			l.gameObject.SetActive(p + 1 == _003C_003Ef__ref_0024696.item.Place && _003C_003Ef__ref_0024696.item.WinCount > 0);
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillClanItem_003Ec__AnonStorey2BA
	{
		internal LeaderboardItemViewModel item;

		internal void _003C_003Em__2F7(UILabel p)
		{
			p.text = ((item.Place <= 3) ? string.Empty : item.Place.ToString(CultureInfo.InvariantCulture));
		}
	}

	[CompilerGenerated]
	private sealed class _003CFillClanItem_003Ec__AnonStorey2B9
	{
		internal int p;

		internal _003CFillClanItem_003Ec__AnonStorey2BA _003C_003Ef__ref_0024698;

		internal void _003C_003Em__2F5(Transform l)
		{
			l.gameObject.SetActive(p + 1 == _003C_003Ef__ref_0024698.item.Place);
		}
	}

	private const int VisibleItemMaxCount = 15;

	private float _expirationTimeSeconds;

	private float _expirationNextUpateTimeSeconds;

	private bool _fillLock;

	private readonly List<LeaderboardItemViewModel> _clansList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _friendsList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _playersList = new List<LeaderboardItemViewModel>(101);

	private readonly List<LeaderboardItemViewModel> _colleaguesList = new List<LeaderboardItemViewModel>(101);

	[SerializeField]
	private GameObject _viewHandler;

	[SerializeField]
	private PrefabHandler _viewPrefab;

	private LazyObject<LeaderboardsView> _view;

	private UIPanel _panelVal;

	private bool _isInit;

	private Lazy<MainMenuController> _mainMenuController;

	private TaskCompletionSource<bool> _returnPromise = new TaskCompletionSource<bool>();

	private bool _profileIsOpened;

	private static TaskCompletionSource<string> _currentRequestPromise;

	private GridState _state;

	[CompilerGenerated]
	private static Func<TaskCompletionSource<string>, Task<string>> _003C_003Ef__am_0024cache13;

	[CompilerGenerated]
	private static Func<Transform, UILabel> _003C_003Ef__am_0024cache14;

	[CompilerGenerated]
	private static Action<UILabel> _003C_003Ef__am_0024cache15;

	[CompilerGenerated]
	private static Func<Transform, UILabel> _003C_003Ef__am_0024cache16;

	[CompilerGenerated]
	private static Action<UILabel> _003C_003Ef__am_0024cache17;

	[CompilerGenerated]
	private static Func<Transform, UILabel> _003C_003Ef__am_0024cache18;

	[CompilerGenerated]
	private static Func<Transform, UILabel> _003C_003Ef__am_0024cache19;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache1A;

	[CompilerGenerated]
	private static Func<UILabel, Transform> _003C_003Ef__am_0024cache1B;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache1C;

	[CompilerGenerated]
	private static Func<UILabel, Transform> _003C_003Ef__am_0024cache1D;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache1E;

	[CompilerGenerated]
	private static Func<UILabel, Transform> _003C_003Ef__am_0024cache1F;

	[CompilerGenerated]
	private static Func<Transform, UILabel> _003C_003Ef__am_0024cache20;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache21;

	[CompilerGenerated]
	private static Func<UILabel, Transform> _003C_003Ef__am_0024cache22;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache23;

	[CompilerGenerated]
	private static Func<UILabel, Transform> _003C_003Ef__am_0024cache24;

	[CompilerGenerated]
	private static Func<UILabel, bool> _003C_003Ef__am_0024cache25;

	[CompilerGenerated]
	private static Func<UILabel, Transform> _003C_003Ef__am_0024cache26;

	[CompilerGenerated]
	private static Func<Transform, UILabel> _003C_003Ef__am_0024cache27;

	[CompilerGenerated]
	private static Func<LeaderboardItemViewModel, int> _003C_003Ef__am_0024cache28;

	[CompilerGenerated]
	private static Func<IGrouping<int, LeaderboardItemViewModel>, int> _003C_003Ef__am_0024cache29;

	[CompilerGenerated]
	private static Func<LeaderboardItemViewModel, int> _003C_003Ef__am_0024cache2A;

	[CompilerGenerated]
	private static Func<MainMenuController> _003C_003Ef__am_0024cache2B;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache2C;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache2D;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache2E;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache2F;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache30;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache31;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache32;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache33;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache34;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache35;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache36;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache37;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache38;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache39;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache3A;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache3B;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache3C;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache3D;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache3E;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache3F;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache40;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache41;

	[CompilerGenerated]
	private static Func<GameObject, UIWrapContent> _003C_003Ef__am_0024cache42;

	[CompilerGenerated]
	private static Action<UIWrapContent> _003C_003Ef__am_0024cache43;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache44;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache45;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache46;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache47;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache48;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache49;

	[CompilerGenerated]
	private static Func<GameObject, UIScrollView> _003C_003Ef__am_0024cache4A;

	[CompilerGenerated]
	private static Action<UIScrollView> _003C_003Ef__am_0024cache4B;

	private LeaderboardsView LeaderboardView
	{
		get
		{
			return _view.Value;
		}
	}

	public UILabel ExpirationLabel
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.expirationLabel;
		}
	}

	private GameObject TopFriendsGrid
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.friendsGrid.gameObject;
		}
	}

	private GameObject TopPlayersGrid
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.bestPlayersGrid.gameObject;
		}
	}

	private GameObject TopClansGrid
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.clansGrid.gameObject;
		}
	}

	private GameObject TopLeagueGrid
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.leagueGrid.gameObject;
		}
	}

	private GameObject TableFooterIndividual
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.tableFooterIndividual;
		}
	}

	private GameObject TableFooterClan
	{
		get
		{
			if (LeaderboardView == null)
			{
				return null;
			}
			return LeaderboardView.tableFooterClan;
		}
	}

	public static LeaderboardScript Instance { get; private set; }

	public UIPanel Panel
	{
		get
		{
			if (_panelVal == null)
			{
				_panelVal = ((_view == null || !_view.ObjectIsLoaded) ? null : _view.Value.gameObject.GetComponent<UIPanel>());
			}
			return _panelVal;
		}
	}

	public bool UIEnabled
	{
		get
		{
			return _view != null && _view.ObjectIsActive;
		}
	}

	private Task<string> CurrentRequest
	{
		get
		{
			TaskCompletionSource<string> currentRequestPromise = _currentRequestPromise;
			if (_003C_003Ef__am_0024cache13 == null)
			{
				_003C_003Ef__am_0024cache13 = _003Cget_CurrentRequest_003Em__2DE;
			}
			return currentRequestPromise.Map(_003C_003Ef__am_0024cache13);
		}
	}

	private static string LeaderboardsResponseCache
	{
		get
		{
			return "Leaderboards.New.ResponseCache";
		}
	}

	private static string LeaderboardsResponseCacheTimestamp
	{
		get
		{
			return "Leaderboards.New.ResponseCacheTimestamp";
		}
	}

	public static event EventHandler<ClickedEventArgs> PlayerClicked;

	private void UpdateLocs()
	{
		if (TableFooterIndividual != null)
		{
			Transform o = TableFooterIndividual.transform.FindChild("LabelPlace");
			if (_003C_003Ef__am_0024cache14 == null)
			{
				_003C_003Ef__am_0024cache14 = _003CUpdateLocs_003Em__2DF;
			}
			UILabel o2 = o.Map(_003C_003Ef__am_0024cache14);
			if (_003C_003Ef__am_0024cache15 == null)
			{
				_003C_003Ef__am_0024cache15 = _003CUpdateLocs_003Em__2E0;
			}
			o2.Do(_003C_003Ef__am_0024cache15);
		}
		if (TableFooterClan != null)
		{
			Transform o3 = TableFooterClan.transform.FindChild("LabelPlace");
			if (_003C_003Ef__am_0024cache16 == null)
			{
				_003C_003Ef__am_0024cache16 = _003CUpdateLocs_003Em__2E1;
			}
			UILabel o4 = o3.Map(_003C_003Ef__am_0024cache16);
			if (_003C_003Ef__am_0024cache17 == null)
			{
				_003C_003Ef__am_0024cache17 = _003CUpdateLocs_003Em__2E2;
			}
			o4.Do(_003C_003Ef__am_0024cache17);
		}
	}

	private IEnumerator FillGrids(string response, string playerId, GridState state)
	{
		while (_fillLock)
		{
			yield return null;
		}
		_fillLock = true;
		try
		{
			if (string.IsNullOrEmpty(playerId))
			{
				throw new ArgumentException("Player id should not be empty", "playerId");
			}
			Dictionary<string, object> d = Json.Deserialize(response ?? string.Empty) as Dictionary<string, object>;
			if (d == null)
			{
				Debug.LogWarning("Leaderboards response is ill-formed.");
				d = new Dictionary<string, object>();
			}
			else if (d.Count == 0)
			{
				Debug.LogWarning("Leaderboards response contains no elements.");
			}
			object expirationTimespanSecondsObject;
			if (d.TryGetValue("leaderboards_generate_next_time", out expirationTimespanSecondsObject))
			{
				try
				{
					float expirationTimespanSeconds = Convert.ToSingle(expirationTimespanSecondsObject);
					_expirationTimeSeconds = Time.realtimeSinceStartup + expirationTimespanSeconds;
					if (state == GridState.FillingWithCache)
					{
						string cacheTimestampString = PlayerPrefs.GetString(LeaderboardsResponseCacheTimestamp, string.Empty);
						DateTime cacheTimestamp;
						if (!string.IsNullOrEmpty(cacheTimestampString) && DateTime.TryParse(cacheTimestampString, out cacheTimestamp))
						{
							float timespanSinceLastCache2 = (float)(DateTime.UtcNow - cacheTimestamp).TotalSeconds;
							timespanSinceLastCache2 = Math.Max(0f, timespanSinceLastCache2);
							_expirationTimeSeconds = Math.Max(0f, _expirationTimeSeconds - timespanSinceLastCache2);
						}
					}
					ExpirationLabel.Do(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__321);
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Debug.LogWarning(ex);
				}
			}
			LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel
			{
				Id = playerId,
				Nickname = ProfileController.GetPlayerNameOrDefault(),
				Rank = ExperienceController.sharedController.currentLevel,
				WinCount = RatingSystem.instance.currentRating,
				Highlight = true
			};
			FriendsController sharedController = FriendsController.sharedController;
			if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2D == null)
			{
				_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2D = _003CFillGrids_003Ec__Iterator157._003C_003Em__322;
			}
			leaderboardItemViewModel.ClanName = sharedController.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2D, string.Empty);
			FriendsController sharedController2 = FriendsController.sharedController;
			if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2E == null)
			{
				_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2E = _003CFillGrids_003Ec__Iterator157._003C_003Em__323;
			}
			leaderboardItemViewModel.ClanLogo = sharedController2.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2E, string.Empty);
			LeaderboardItemViewModel me = leaderboardItemViewModel;
			object meObject;
			if (d.TryGetValue("me", out meObject))
			{
				Dictionary<string, object> meDictionary = meObject as Dictionary<string, object>;
				object myWinCount;
				if (meDictionary.TryGetValue("wins", out myWinCount))
				{
					me.WinCount = Convert.ToInt32(myWinCount);
					PlayerPrefs.SetInt("TotalWinsForLeaderboards", me.WinCount);
				}
			}
			List<LeaderboardItemViewModel> rawFriendsList = LeaderboardsController.ParseLeaderboardEntries(playerId, "friends", d);
			HashSet<string> friendIds = new HashSet<string>(FriendsController.sharedController.friends);
			if (FriendsController.sharedController != null)
			{
				for (int i = rawFriendsList.Count - 1; i >= 0; i--)
				{
					LeaderboardItemViewModel item = rawFriendsList[i];
					Dictionary<string, object> info;
					if (friendIds.Contains(item.Id) && FriendsController.sharedController.friendsInfo.TryGetValue(item.Id, out info))
					{
						try
						{
							Dictionary<string, object> playerDict = info["player"] as Dictionary<string, object>;
							item.Nickname = Convert.ToString(playerDict["nick"]);
							item.Rank = Convert.ToInt32(playerDict["rank"]);
							object clanName;
							if (playerDict.TryGetValue("clan_name", out clanName))
							{
								item.ClanName = Convert.ToString(clanName);
							}
							object clanLogo;
							if (playerDict.TryGetValue("clan_logo", out clanLogo))
							{
								item.ClanLogo = Convert.ToString(clanLogo);
							}
						}
						catch (KeyNotFoundException)
						{
							Debug.LogError("Failed to process cached friend: " + item.Id);
						}
					}
					else
					{
						rawFriendsList.RemoveAt(i);
					}
				}
			}
			rawFriendsList.Add(me);
			yield return StartCoroutine(FillFriendsGrid(list: GroupAndOrder(rawFriendsList), gridGo: TopFriendsGrid, state: state));
			List<LeaderboardItemViewModel> rawColleaguesList = LeaderboardsController.ParseLeaderboardEntries(playerId, "best_players_league", d);
			yield return StartCoroutine(FillColleaguesGrid(list: GroupAndOrder(rawColleaguesList), gridGo: TopLeagueGrid, state: state));
			if (TableFooterIndividual != null && LeaderboardView != null && rawColleaguesList.Any(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__324))
			{
				LeaderboardView.SetLeagueTopFooterActive();
			}
			List<LeaderboardItemViewModel> rawTopPlayersList = LeaderboardsController.ParseLeaderboardEntries(playerId, "best_players", d);
			List<LeaderboardItemViewModel> orderedTopPlayersList = GroupAndOrder(rawTopPlayersList);
			AddCacheInProfileInfo(rawTopPlayersList);
			Coroutine fillPlayersCoroutine = StartCoroutine(FillPlayersGrid(TopPlayersGrid, orderedTopPlayersList, state));
			if (TableFooterIndividual != null)
			{
				if (LeaderboardView != null && rawTopPlayersList.Any(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__325))
				{
					LeaderboardView.SetOverallTopFooterActive();
				}
				Transform o = TableFooterIndividual.transform.FindChild("LabelPlace");
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2F == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2F = _003CFillGrids_003Ec__Iterator157._003C_003Em__326;
				}
				UILabel o2 = o.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache2F);
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache30 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache30 = _003CFillGrids_003Ec__Iterator157._003C_003Em__327;
				}
				o2.Do(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache30);
				Transform o3 = TableFooterIndividual.transform.FindChild("LabelNick");
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache31 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache31 = _003CFillGrids_003Ec__Iterator157._003C_003Em__328;
				}
				o3.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache31).Do(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__329);
				Transform o4 = TableFooterIndividual.transform.FindChild("LabelLevel");
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache32 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache32 = _003CFillGrids_003Ec__Iterator157._003C_003Em__32A;
				}
				o4.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache32).Do(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__32B);
				Transform o5 = TableFooterIndividual.transform.FindChild("LabelWins");
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache33 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache33 = _003CFillGrids_003Ec__Iterator157._003C_003Em__32C;
				}
				o5.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache33).Do(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__32D);
				Transform o6 = TableFooterIndividual.transform.FindChild("LabelClan");
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache34 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache34 = _003CFillGrids_003Ec__Iterator157._003C_003Em__32E;
				}
				UILabel clanLabel2 = o6.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache34);
				clanLabel2.Do(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__32F);
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache35 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache35 = _003CFillGrids_003Ec__Iterator157._003C_003Em__330;
				}
				clanLabel2.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache35).Do(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__331);
			}
			rawTopPlayersList.Clear();
			List<LeaderboardItemViewModel> rawClansList = LeaderboardsController.ParseLeaderboardEntries(playerId, "top_clans", d);
			Coroutine fillClansCoroutine = StartCoroutine(FillClansGrid(list: GroupAndOrder(rawClansList), gridGo: TopClansGrid, state: state));
			if (TableFooterClan != null)
			{
				FriendsController sharedController3 = FriendsController.sharedController;
				if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache36 == null)
				{
					_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache36 = _003CFillGrids_003Ec__Iterator157._003C_003Em__332;
				}
				string clanId = sharedController3.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache36);
				if (string.IsNullOrEmpty(clanId))
				{
					TableFooterClan.SetActive(false);
				}
				else
				{
					LeaderboardItemViewModel myClanInTop = rawClansList.FirstOrDefault(((_003CFillGrids_003Ec__Iterator157)(object)this)._003C_003Em__333);
					TableFooterClan.SetActive(myClanInTop == null);
					if (myClanInTop == null)
					{
						Transform o7 = TableFooterClan.transform.FindChild("LabelPlace");
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache37 == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache37 = _003CFillGrids_003Ec__Iterator157._003C_003Em__334;
						}
						UILabel o8 = o7.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache37);
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache38 == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache38 = _003CFillGrids_003Ec__Iterator157._003C_003Em__335;
						}
						o8.Do(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache38);
						Transform o9 = TableFooterClan.transform.FindChild("LabelMembers");
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache39 == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache39 = _003CFillGrids_003Ec__Iterator157._003C_003Em__336;
						}
						UILabel o10 = o9.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache39);
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3A == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3A = _003CFillGrids_003Ec__Iterator157._003C_003Em__337;
						}
						o10.Do(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3A);
						Transform o11 = TableFooterClan.transform.FindChild("LabelWins");
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3B == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3B = _003CFillGrids_003Ec__Iterator157._003C_003Em__338;
						}
						UILabel o12 = o11.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3B);
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3C == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3C = _003CFillGrids_003Ec__Iterator157._003C_003Em__339;
						}
						o12.Do(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3C);
						Transform o13 = TableFooterClan.transform.FindChild("LabelName");
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3D == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3D = _003CFillGrids_003Ec__Iterator157._003C_003Em__33A;
						}
						UILabel clanLabel = o13.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3D);
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3E == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3E = _003CFillGrids_003Ec__Iterator157._003C_003Em__33B;
						}
						clanLabel.Do(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3E);
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3F == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3F = _003CFillGrids_003Ec__Iterator157._003C_003Em__33C;
						}
						UITexture o14 = clanLabel.Map(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache3F);
						if (_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache40 == null)
						{
							_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache40 = _003CFillGrids_003Ec__Iterator157._003C_003Em__33D;
						}
						o14.Do(_003CFillGrids_003Ec__Iterator157._003C_003Ef__am_0024cache40);
					}
				}
			}
			yield return fillPlayersCoroutine;
			yield return fillClansCoroutine;
		}
		finally
		{
			_fillLock = false;
		}
	}

	private void AddCacheInProfileInfo(List<LeaderboardItemViewModel> _list)
	{
		foreach (LeaderboardItemViewModel item in _list)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("nick", item.Nickname);
			dictionary.Add("rank", item.Rank);
			dictionary.Add("clan_name", item.ClanName);
			dictionary.Add("clan_logo", item.ClanLogo);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("player", dictionary);
			if (!FriendsController.sharedController.profileInfo.ContainsKey(item.Id))
			{
				FriendsController.sharedController.profileInfo.Add(item.Id, dictionary2);
			}
			else
			{
				FriendsController.sharedController.profileInfo[item.Id] = dictionary2;
			}
		}
	}

	private IEnumerator FillClansGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, new UIWrapContent.OnInitializeItem(((_003CFillClansGrid_003Ec__Iterator158)(object)this)._003C_003Em__340));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_clansList.Clear();
		_clansList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(_clansList.Count > 0);
		int bound = Math.Min(15, _clansList.Count);
		for (int i = 0; i != bound; i++)
		{
			LeaderboardItemViewModel item = _clansList[i];
			GameObject newItem;
			if (i < childCount)
			{
				newItem = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillClanItem(gridGo, _clansList, state, i, newItem);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
	}

	private IEnumerator FillPlayersGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, new UIWrapContent.OnInitializeItem(((_003CFillPlayersGrid_003Ec__Iterator159)(object)this)._003C_003Em__341));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_playersList.Clear();
		_playersList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(_playersList.Count > 0);
		int bound = Math.Min(15, _playersList.Count);
		for (int i = 0; i != bound; i++)
		{
			LeaderboardItemViewModel item = _playersList[i];
			GameObject newItem;
			if (i < childCount)
			{
				newItem = gridGo.transform.GetChild(i).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = i.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _playersList, state, i, newItem);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
	}

	private IEnumerator FillColleaguesGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, new UIWrapContent.OnInitializeItem(((_003CFillColleaguesGrid_003Ec__Iterator15A)(object)this)._003C_003Em__342));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogWarning("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_colleaguesList.Clear();
		_colleaguesList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(_colleaguesList.Count > 0);
		int bound = Math.Min(15, _colleaguesList.Count);
		for (int j = 0; j != bound; j++)
		{
			LeaderboardItemViewModel item = _colleaguesList[j];
			GameObject newItem;
			if (j < childCount)
			{
				newItem = gridGo.transform.GetChild(j).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = j.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _colleaguesList, state, j, newItem);
		}
		int newChildCount = gridGo.transform.childCount;
		List<Transform> oddItemsToRemove = new List<Transform>(Math.Max(0, newChildCount - bound));
		for (int i = list.Count; i < newChildCount; i++)
		{
			oddItemsToRemove.Add(gridGo.transform.GetChild(i));
		}
		foreach (Transform item2 in oddItemsToRemove)
		{
			NGUITools.Destroy(item2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
	}

	private IEnumerator FillFriendsGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		UIWrapContent wrap = gridGo.GetComponent<UIWrapContent>();
		if (wrap == null)
		{
			throw new InvalidOperationException("Game object does not contain UIWrapContent component.");
		}
		wrap.minIndex = Math.Min(-list.Count + 1, wrap.maxIndex);
		wrap.onInitializeItem = null;
		wrap.onInitializeItem = (UIWrapContent.OnInitializeItem)Delegate.Combine(wrap.onInitializeItem, new UIWrapContent.OnInitializeItem(((_003CFillFriendsGrid_003Ec__Iterator15B)(object)this)._003C_003Em__343));
		int childCount = gridGo.transform.childCount;
		if (childCount == 0)
		{
			Debug.LogError("No children in grid.");
			yield break;
		}
		Transform itemPrototype = gridGo.transform.GetChild(childCount - 1);
		if (itemPrototype == null)
		{
			Debug.LogError("Cannot find prototype for item.");
			yield break;
		}
		_friendsList.Clear();
		_friendsList.AddRange(list);
		GameObject itemPrototypeGo = itemPrototype.gameObject;
		itemPrototypeGo.SetActive(_friendsList.Count > 0);
		int bound = Math.Min(15, _friendsList.Count);
		for (int j = 0; j != bound; j++)
		{
			LeaderboardItemViewModel item = _friendsList[j];
			GameObject newItem;
			if (j < childCount)
			{
				newItem = gridGo.transform.GetChild(j).gameObject;
			}
			else
			{
				newItem = NGUITools.AddChild(gridGo, itemPrototypeGo);
				newItem.name = j.ToString(CultureInfo.InvariantCulture);
			}
			FillIndividualItem(gridGo, _friendsList, state, j, newItem);
		}
		int newChildCount = gridGo.transform.childCount;
		List<Transform> oddItemsToRemove = new List<Transform>(Math.Max(0, newChildCount - bound));
		for (int i = list.Count; i < newChildCount; i++)
		{
			oddItemsToRemove.Add(gridGo.transform.GetChild(i));
		}
		foreach (Transform item2 in oddItemsToRemove)
		{
			NGUITools.Destroy(item2);
		}
		yield return new WaitForEndOfFrame();
		wrap.SortBasedOnScrollMovement();
		wrap.WrapContent();
		UIScrollView scrollView = gridGo.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (scrollView != null)
		{
			scrollView.enabled = true;
			scrollView.ResetPosition();
			scrollView.UpdatePosition();
		}
	}

	internal void RefreshMyLeaderboardEntries()
	{
		foreach (LeaderboardItemViewModel friends in _friendsList)
		{
			if (friends != null && friends.Id == FriendsController.sharedController.id)
			{
				friends.Nickname = ProfileController.GetPlayerNameOrDefault();
				friends.ClanName = FriendsController.sharedController.clanName ?? string.Empty;
				break;
			}
		}
		Transform o = TableFooterIndividual.transform.FindChild("LabelNick");
		if (_003C_003Ef__am_0024cache18 == null)
		{
			_003C_003Ef__am_0024cache18 = _003CRefreshMyLeaderboardEntries_003Em__2E3;
		}
		UILabel uILabel = o.Map(_003C_003Ef__am_0024cache18);
		if (uILabel != null)
		{
			uILabel.text = ProfileController.GetPlayerNameOrDefault();
		}
		Transform o2 = TableFooterIndividual.transform.FindChild("LabelClan");
		if (_003C_003Ef__am_0024cache19 == null)
		{
			_003C_003Ef__am_0024cache19 = _003CRefreshMyLeaderboardEntries_003Em__2E4;
		}
		UILabel uILabel2 = o2.Map(_003C_003Ef__am_0024cache19);
		if (uILabel2 != null)
		{
			uILabel2.text = FriendsController.sharedController.clanName ?? string.Empty;
		}
	}

	private void FillIndividualItem(GameObject grid, List<LeaderboardItemViewModel> list, GridState state, int index, GameObject newItem)
	{
		_003CFillIndividualItem_003Ec__AnonStorey2B8 _003CFillIndividualItem_003Ec__AnonStorey2B = new _003CFillIndividualItem_003Ec__AnonStorey2B8();
		_003CFillIndividualItem_003Ec__AnonStorey2B._003C_003Ef__this = this;
		if (index >= list.Count)
		{
			return;
		}
		if (index < 0)
		{
			string message = string.Format("Unexpected index {0} in list of {1} leaderboard items.", index, list.Count);
			Debug.LogError(message);
			return;
		}
		_003CFillIndividualItem_003Ec__AnonStorey2B.item = list[index];
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(_003CFillIndividualItem_003Ec__AnonStorey2B.item);
		}
		component.Clicked += _003CFillIndividualItem_003Ec__AnonStorey2B._003C_003Em__2E5;
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] array = new Transform[3];
		if (_003C_003Ef__am_0024cache1A == null)
		{
			_003C_003Ef__am_0024cache1A = _003CFillIndividualItem_003Em__2E6;
		}
		UILabel o = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache1A);
		if (_003C_003Ef__am_0024cache1B == null)
		{
			_003C_003Ef__am_0024cache1B = _003CFillIndividualItem_003Em__2E7;
		}
		array[0] = o.Map(_003C_003Ef__am_0024cache1B);
		if (_003C_003Ef__am_0024cache1C == null)
		{
			_003C_003Ef__am_0024cache1C = _003CFillIndividualItem_003Em__2E8;
		}
		UILabel o2 = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache1C);
		if (_003C_003Ef__am_0024cache1D == null)
		{
			_003C_003Ef__am_0024cache1D = _003CFillIndividualItem_003Em__2E9;
		}
		array[1] = o2.Map(_003C_003Ef__am_0024cache1D);
		if (_003C_003Ef__am_0024cache1E == null)
		{
			_003C_003Ef__am_0024cache1E = _003CFillIndividualItem_003Em__2EA;
		}
		UILabel o3 = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache1E);
		if (_003C_003Ef__am_0024cache1F == null)
		{
			_003C_003Ef__am_0024cache1F = _003CFillIndividualItem_003Em__2EB;
		}
		array[2] = o3.Map(_003C_003Ef__am_0024cache1F);
		Transform[] array2 = array;
		_003CFillIndividualItem_003Ec__AnonStorey2B7 _003CFillIndividualItem_003Ec__AnonStorey2B2 = new _003CFillIndividualItem_003Ec__AnonStorey2B7();
		_003CFillIndividualItem_003Ec__AnonStorey2B2._003C_003Ef__ref_0024696 = _003CFillIndividualItem_003Ec__AnonStorey2B;
		_003CFillIndividualItem_003Ec__AnonStorey2B2.p = 0;
		while (_003CFillIndividualItem_003Ec__AnonStorey2B2.p != array2.Length)
		{
			array2[_003CFillIndividualItem_003Ec__AnonStorey2B2.p].Do(_003CFillIndividualItem_003Ec__AnonStorey2B2._003C_003Em__2EC);
			_003CFillIndividualItem_003Ec__AnonStorey2B2.p++;
		}
		Transform o4 = newItem.transform.FindChild("LabelsPlace");
		if (_003C_003Ef__am_0024cache20 == null)
		{
			_003C_003Ef__am_0024cache20 = _003CFillIndividualItem_003Em__2ED;
		}
		o4.Map(_003C_003Ef__am_0024cache20).Do(_003CFillIndividualItem_003Ec__AnonStorey2B._003C_003Em__2EE);
	}

	private void FillClanItem(GameObject gridGo, List<LeaderboardItemViewModel> list, GridState state, int index, GameObject newItem)
	{
		_003CFillClanItem_003Ec__AnonStorey2BA _003CFillClanItem_003Ec__AnonStorey2BA = new _003CFillClanItem_003Ec__AnonStorey2BA();
		if (index < list.Count)
		{
			_003CFillClanItem_003Ec__AnonStorey2BA.item = list[index];
			LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
			if (component != null)
			{
				component.NewReset(_003CFillClanItem_003Ec__AnonStorey2BA.item);
			}
			UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
			Transform[] array = new Transform[3];
			if (_003C_003Ef__am_0024cache21 == null)
			{
				_003C_003Ef__am_0024cache21 = _003CFillClanItem_003Em__2EF;
			}
			UILabel o = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache21);
			if (_003C_003Ef__am_0024cache22 == null)
			{
				_003C_003Ef__am_0024cache22 = _003CFillClanItem_003Em__2F0;
			}
			array[0] = o.Map(_003C_003Ef__am_0024cache22);
			if (_003C_003Ef__am_0024cache23 == null)
			{
				_003C_003Ef__am_0024cache23 = _003CFillClanItem_003Em__2F1;
			}
			UILabel o2 = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache23);
			if (_003C_003Ef__am_0024cache24 == null)
			{
				_003C_003Ef__am_0024cache24 = _003CFillClanItem_003Em__2F2;
			}
			array[1] = o2.Map(_003C_003Ef__am_0024cache24);
			if (_003C_003Ef__am_0024cache25 == null)
			{
				_003C_003Ef__am_0024cache25 = _003CFillClanItem_003Em__2F3;
			}
			UILabel o3 = componentsInChildren.FirstOrDefault(_003C_003Ef__am_0024cache25);
			if (_003C_003Ef__am_0024cache26 == null)
			{
				_003C_003Ef__am_0024cache26 = _003CFillClanItem_003Em__2F4;
			}
			array[2] = o3.Map(_003C_003Ef__am_0024cache26);
			Transform[] array2 = array;
			_003CFillClanItem_003Ec__AnonStorey2B9 _003CFillClanItem_003Ec__AnonStorey2B = new _003CFillClanItem_003Ec__AnonStorey2B9();
			_003CFillClanItem_003Ec__AnonStorey2B._003C_003Ef__ref_0024698 = _003CFillClanItem_003Ec__AnonStorey2BA;
			_003CFillClanItem_003Ec__AnonStorey2B.p = 0;
			while (_003CFillClanItem_003Ec__AnonStorey2B.p != array2.Length)
			{
				array2[_003CFillClanItem_003Ec__AnonStorey2B.p].Do(_003CFillClanItem_003Ec__AnonStorey2B._003C_003Em__2F5);
				_003CFillClanItem_003Ec__AnonStorey2B.p++;
			}
			Transform o4 = newItem.transform.FindChild("LabelsPlace");
			if (_003C_003Ef__am_0024cache27 == null)
			{
				_003C_003Ef__am_0024cache27 = _003CFillClanItem_003Em__2F6;
			}
			o4.Map(_003C_003Ef__am_0024cache27).Do(_003CFillClanItem_003Ec__AnonStorey2BA._003C_003Em__2F7);
		}
	}

	internal static void SetClanLogo(UITexture s, Texture2D clanLogoTexture)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture mainTexture = s.mainTexture;
		s.mainTexture = ((!(clanLogoTexture != null)) ? null : UnityEngine.Object.Instantiate(clanLogoTexture));
		mainTexture.Do(UnityEngine.Object.Destroy);
	}

	internal static void SetClanLogo(UITexture s, string clanLogo)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture mainTexture = s.mainTexture;
		if (string.IsNullOrEmpty(clanLogo))
		{
			s.mainTexture = null;
		}
		else
		{
			s.mainTexture = LeaderboardItemViewModel.CreateLogoFromBase64String(clanLogo);
		}
		mainTexture.Do(UnityEngine.Object.Destroy);
	}

	private static List<LeaderboardItemViewModel> GroupAndOrder(List<LeaderboardItemViewModel> items)
	{
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		if (_003C_003Ef__am_0024cache28 == null)
		{
			_003C_003Ef__am_0024cache28 = _003CGroupAndOrder_003Em__2F8;
		}
		IEnumerable<IGrouping<int, LeaderboardItemViewModel>> source = items.GroupBy(_003C_003Ef__am_0024cache28);
		if (_003C_003Ef__am_0024cache29 == null)
		{
			_003C_003Ef__am_0024cache29 = _003CGroupAndOrder_003Em__2F9;
		}
		IOrderedEnumerable<IGrouping<int, LeaderboardItemViewModel>> orderedEnumerable = source.OrderByDescending(_003C_003Ef__am_0024cache29);
		int num = 1;
		foreach (IGrouping<int, LeaderboardItemViewModel> item in orderedEnumerable)
		{
			if (_003C_003Ef__am_0024cache2A == null)
			{
				_003C_003Ef__am_0024cache2A = _003CGroupAndOrder_003Em__2FA;
			}
			IOrderedEnumerable<LeaderboardItemViewModel> orderedEnumerable2 = item.OrderByDescending(_003C_003Ef__am_0024cache2A);
			foreach (LeaderboardItemViewModel item2 in orderedEnumerable2)
			{
				item2.Place = num;
				list.Add(item2);
			}
			num += item.Count();
		}
		return list;
	}

	public static int GetLeagueId()
	{
		return (int)RatingSystem.instance.currentLeague;
	}

	internal static void RequestLeaderboards(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("Friends controller is null.");
			return;
		}
		if (_currentRequestPromise != null)
		{
			_currentRequestPromise.TrySetCanceled();
		}
		_currentRequestPromise = new TaskCompletionSource<string>();
		FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _currentRequestPromise));
	}

	private void HandlePlayerClicked(object sender, ClickedEventArgs e)
	{
		if (Panel == null)
		{
			Debug.LogError("Leaderboards panel not found.");
			return;
		}
		Panel.alpha = float.Epsilon;
		Panel.gameObject.SetActive(false);
		Action<bool> onCloseEvent = _003CHandlePlayerClicked_003Em__2FB;
		_profileIsOpened = true;
		FriendsController.ShowProfile(e.Id, ProfileWindowType.other, onCloseEvent);
	}

	private void Awake()
	{
		Instance = this;
		_view = new LazyObject<LeaderboardsView>(_viewPrefab.ResourcePath, _viewHandler);
		if (_003C_003Ef__am_0024cache2B == null)
		{
			_003C_003Ef__am_0024cache2B = _003CAwake_003Em__2FC;
		}
		_mainMenuController = new Lazy<MainMenuController>(_003C_003Ef__am_0024cache2B);
	}

	private void OnDestroy()
	{
		if (_currentRequestPromise != null)
		{
			_currentRequestPromise.TrySetCanceled();
		}
		_currentRequestPromise = null;
		LeaderboardScript.PlayerClicked = null;
		FriendsController.DisposeProfile();
		_mainMenuController.Value.Do(_003COnDestroy_003Em__2FD);
		LocalizationStore.DelEventCallAfterLocalize(UpdateLocs);
	}

	public void Show()
	{
		StartCoroutine(ShowCoroutine());
	}

	private IEnumerator ShowCoroutine()
	{
		if (!_isInit)
		{
			if (LeaderboardView != null && LeaderboardView.backButton != null)
			{
				LeaderboardView.backButton.Clicked += ReturnBack;
			}
			LeaderboardScript.PlayerClicked = (EventHandler<ClickedEventArgs>)Delegate.Combine(LeaderboardScript.PlayerClicked, new EventHandler<ClickedEventArgs>(HandlePlayerClicked));
			LocalizationStore.AddEventCallAfterLocalize(UpdateLocs);
			_isInit = true;
		}
		if (FriendsController.sharedController == null)
		{
			Debug.LogError("Friends controller is null.");
			yield break;
		}
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			Debug.LogError("Player id should not be null.");
			yield break;
		}
		if (_currentRequestPromise == null)
		{
			_currentRequestPromise = new TaskCompletionSource<string>();
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, _currentRequestPromise));
		}
		if (!CurrentRequest.IsCompleted)
		{
			string response2 = PlayerPrefs.GetString(LeaderboardsResponseCache, string.Empty);
			if (string.IsNullOrEmpty(response2))
			{
				yield return StartCoroutine(FillGrids(string.Empty, playerId, _state));
			}
			else
			{
				_state = GridState.FillingWithCache;
				yield return StartCoroutine(FillGrids(response2, playerId, _state));
				_state = GridState.Cache;
			}
		}
		while (!CurrentRequest.IsCompleted)
		{
			yield return null;
		}
		if (CurrentRequest.IsCanceled)
		{
			Debug.LogWarning("Request is cancelled.");
			yield break;
		}
		if (CurrentRequest.IsFaulted)
		{
			Debug.LogException(CurrentRequest.Exception);
			yield break;
		}
		string response = CurrentRequest.Result;
		_state = GridState.FillingWithResponse;
		yield return StartCoroutine(FillGrids(response, playerId, _state));
		_state = GridState.Response;
	}

	private static string FormatExpirationLabel(float expirationTimespanSeconds)
	{
		//Discarded unreachable code: IL_0064, IL_0157
		if (expirationTimespanSeconds < 0f)
		{
			throw new ArgumentOutOfRangeException("expirationTimespanSeconds");
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(expirationTimespanSeconds);
		try
		{
			return string.Format(LocalizationStore.Get("Key_1478"), Convert.ToInt32(Math.Floor(timeSpan.TotalDays)), timeSpan.Hours, timeSpan.Minutes);
		}
		catch
		{
			if (timeSpan.TotalHours < 1.0)
			{
				return string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
			}
			if (timeSpan.TotalDays < 1.0)
			{
				return string.Format("{0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}
			return string.Format("{0}d {1}:{2:00}:{3:00}", Convert.ToInt32(Math.Floor(timeSpan.TotalDays)), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
	}

	private void Update()
	{
		if (!_isInit || !(Time.realtimeSinceStartup > _expirationNextUpateTimeSeconds))
		{
			return;
		}
		_expirationNextUpateTimeSeconds = Time.realtimeSinceStartup + 5f;
		if (!(ExpirationLabel != null))
		{
			return;
		}
		if (_state == GridState.Empty)
		{
			ExpirationLabel.text = LocalizationStore.Key_0348;
			return;
		}
		float num = _expirationTimeSeconds - Time.realtimeSinceStartup;
		if (num <= 0f)
		{
			ExpirationLabel.text = string.Empty;
		}
		else
		{
			ExpirationLabel.text = FormatExpirationLabel(num);
		}
	}

	private static IEnumerator LoadLeaderboardsCoroutine(string playerId, TaskCompletionSource<string> requestPromise)
	{
		if (!TrainingController.TrainingCompleted)
		{
			yield break;
		}
		if (requestPromise == null)
		{
			throw new ArgumentNullException("requestPromise");
		}
		if (requestPromise.Task.IsCanceled)
		{
			yield break;
		}
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be null.", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			throw new InvalidOperationException("Friends controller should not be null.");
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			Debug.LogWarning("Current player id is empty.");
			requestPromise.TrySetException(new InvalidOperationException("Current player id is empty."));
			yield break;
		}
		string leaderboardsResponseCacheTimestampString = PlayerPrefs.GetString(LeaderboardsResponseCacheTimestamp, string.Empty);
		DateTime leaderboardsResponseCacheTimestamp;
		if (DateTime.TryParse(leaderboardsResponseCacheTimestampString, out leaderboardsResponseCacheTimestamp))
		{
			DateTime timeOfNnextRequest = leaderboardsResponseCacheTimestamp + TimeSpan.FromSeconds(Defs.pauseUpdateLeaderboard);
			float secondsTillNextRequest = (float)(timeOfNnextRequest - DateTime.UtcNow).TotalSeconds;
			if (secondsTillNextRequest > 3600f)
			{
				secondsTillNextRequest = 0f;
			}
			yield return new WaitForSeconds(secondsTillNextRequest);
		}
		if (requestPromise.Task.IsCanceled)
		{
			yield break;
		}
		int leagueId = GetLeagueId();
		WWWForm form = new WWWForm();
		form.AddField("action", "get_leaderboards_wins_league");
		form.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
		form.AddField("id", playerId);
		form.AddField("league_id", leagueId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_leaderboards_wins_league"));
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty);
		if (request == null)
		{
			requestPromise.TrySetException(new InvalidOperationException("Request forbidden while connected."));
			TaskCompletionSource<string> newRequestPromise3 = (_currentRequestPromise = new TaskCompletionSource<string>());
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, newRequestPromise3));
			yield break;
		}
		while (!request.isDone)
		{
			if (requestPromise.Task.IsCanceled)
			{
				request.Dispose();
				yield break;
			}
			yield return null;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			requestPromise.TrySetException(new InvalidOperationException(request.error));
			Debug.LogError(request.error);
			TaskCompletionSource<string> newRequestPromise2 = (_currentRequestPromise = new TaskCompletionSource<string>());
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, newRequestPromise2));
			yield break;
		}
		string responseText = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(responseText) || responseText == "fail")
		{
			string message = string.Format("Leaderboars response: '{0}'", responseText);
			requestPromise.TrySetException(new InvalidOperationException(message));
			Debug.LogWarning(message);
			TaskCompletionSource<string> newRequestPromise = (_currentRequestPromise = new TaskCompletionSource<string>());
			yield return new WaitForSeconds(Defs.timeUpdateLeaderboardIfNullResponce);
			FriendsController.sharedController.StartCoroutine(LoadLeaderboardsCoroutine(playerId, newRequestPromise));
		}
		else
		{
			requestPromise.TrySetResult(responseText);
			PlayerPrefs.SetString(LeaderboardsResponseCache, responseText);
			PlayerPrefs.SetString(LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture));
		}
	}

	public Task GetReturnFuture()
	{
		if (_returnPromise.Task.IsCompleted)
		{
			_returnPromise = new TaskCompletionSource<bool>();
		}
		_mainMenuController.Value.Do(_003CGetReturnFuture_003Em__2FE);
		_mainMenuController.Value.Do(_003CGetReturnFuture_003Em__2FF);
		return _returnPromise.Task;
	}

	private void ReturnBack(object sender, EventArgs e)
	{
		if (!_profileIsOpened)
		{
			GameObject topFriendsGrid = TopFriendsGrid;
			if (_003C_003Ef__am_0024cache2C == null)
			{
				_003C_003Ef__am_0024cache2C = _003CReturnBack_003Em__300;
			}
			UIWrapContent o = topFriendsGrid.Map(_003C_003Ef__am_0024cache2C);
			if (_003C_003Ef__am_0024cache2D == null)
			{
				_003C_003Ef__am_0024cache2D = _003CReturnBack_003Em__301;
			}
			o.Do(_003C_003Ef__am_0024cache2D);
			GameObject topPlayersGrid = TopPlayersGrid;
			if (_003C_003Ef__am_0024cache2E == null)
			{
				_003C_003Ef__am_0024cache2E = _003CReturnBack_003Em__302;
			}
			UIWrapContent o2 = topPlayersGrid.Map(_003C_003Ef__am_0024cache2E);
			if (_003C_003Ef__am_0024cache2F == null)
			{
				_003C_003Ef__am_0024cache2F = _003CReturnBack_003Em__303;
			}
			o2.Do(_003C_003Ef__am_0024cache2F);
			GameObject topClansGrid = TopClansGrid;
			if (_003C_003Ef__am_0024cache30 == null)
			{
				_003C_003Ef__am_0024cache30 = _003CReturnBack_003Em__304;
			}
			UIWrapContent o3 = topClansGrid.Map(_003C_003Ef__am_0024cache30);
			if (_003C_003Ef__am_0024cache31 == null)
			{
				_003C_003Ef__am_0024cache31 = _003CReturnBack_003Em__305;
			}
			o3.Do(_003C_003Ef__am_0024cache31);
			GameObject topLeagueGrid = TopLeagueGrid;
			if (_003C_003Ef__am_0024cache32 == null)
			{
				_003C_003Ef__am_0024cache32 = _003CReturnBack_003Em__306;
			}
			UIWrapContent o4 = topLeagueGrid.Map(_003C_003Ef__am_0024cache32);
			if (_003C_003Ef__am_0024cache33 == null)
			{
				_003C_003Ef__am_0024cache33 = _003CReturnBack_003Em__307;
			}
			o4.Do(_003C_003Ef__am_0024cache33);
			GameObject topFriendsGrid2 = TopFriendsGrid;
			if (_003C_003Ef__am_0024cache34 == null)
			{
				_003C_003Ef__am_0024cache34 = _003CReturnBack_003Em__308;
			}
			UIScrollView o5 = topFriendsGrid2.Map(_003C_003Ef__am_0024cache34);
			if (_003C_003Ef__am_0024cache35 == null)
			{
				_003C_003Ef__am_0024cache35 = _003CReturnBack_003Em__309;
			}
			o5.Do(_003C_003Ef__am_0024cache35);
			GameObject topPlayersGrid2 = TopPlayersGrid;
			if (_003C_003Ef__am_0024cache36 == null)
			{
				_003C_003Ef__am_0024cache36 = _003CReturnBack_003Em__30A;
			}
			UIScrollView o6 = topPlayersGrid2.Map(_003C_003Ef__am_0024cache36);
			if (_003C_003Ef__am_0024cache37 == null)
			{
				_003C_003Ef__am_0024cache37 = _003CReturnBack_003Em__30B;
			}
			o6.Do(_003C_003Ef__am_0024cache37);
			GameObject topClansGrid2 = TopClansGrid;
			if (_003C_003Ef__am_0024cache38 == null)
			{
				_003C_003Ef__am_0024cache38 = _003CReturnBack_003Em__30C;
			}
			UIScrollView o7 = topClansGrid2.Map(_003C_003Ef__am_0024cache38);
			if (_003C_003Ef__am_0024cache39 == null)
			{
				_003C_003Ef__am_0024cache39 = _003CReturnBack_003Em__30D;
			}
			o7.Do(_003C_003Ef__am_0024cache39);
			GameObject topLeagueGrid2 = TopLeagueGrid;
			if (_003C_003Ef__am_0024cache3A == null)
			{
				_003C_003Ef__am_0024cache3A = _003CReturnBack_003Em__30E;
			}
			UIScrollView o8 = topLeagueGrid2.Map(_003C_003Ef__am_0024cache3A);
			if (_003C_003Ef__am_0024cache3B == null)
			{
				_003C_003Ef__am_0024cache3B = _003CReturnBack_003Em__30F;
			}
			o8.Do(_003C_003Ef__am_0024cache3B);
			_returnPromise.TrySetResult(true);
			_mainMenuController.Value.Do(_003CReturnBack_003Em__310);
		}
	}

	[CompilerGenerated]
	private static Task<string> _003Cget_CurrentRequest_003Em__2DE(TaskCompletionSource<string> p)
	{
		return p.Task;
	}

	[CompilerGenerated]
	private static UILabel _003CUpdateLocs_003Em__2DF(Transform t)
	{
		return t.gameObject.GetComponent<UILabel>();
	}

	[CompilerGenerated]
	private static void _003CUpdateLocs_003Em__2E0(UILabel n)
	{
		n.text = LocalizationStore.Get("Key_0053");
	}

	[CompilerGenerated]
	private static UILabel _003CUpdateLocs_003Em__2E1(Transform t)
	{
		return t.gameObject.GetComponent<UILabel>();
	}

	[CompilerGenerated]
	private static void _003CUpdateLocs_003Em__2E2(UILabel n)
	{
		n.text = LocalizationStore.Get("Key_0053");
	}

	[CompilerGenerated]
	private static UILabel _003CRefreshMyLeaderboardEntries_003Em__2E3(Transform t)
	{
		return t.gameObject.GetComponent<UILabel>();
	}

	[CompilerGenerated]
	private static UILabel _003CRefreshMyLeaderboardEntries_003Em__2E4(Transform t)
	{
		return t.gameObject.GetComponent<UILabel>();
	}

	[CompilerGenerated]
	private static bool _003CFillIndividualItem_003Em__2E6(UILabel l)
	{
		return l.gameObject.name.Equals("LabelsFirstPlace");
	}

	[CompilerGenerated]
	private static Transform _003CFillIndividualItem_003Em__2E7(UILabel l)
	{
		return l.transform;
	}

	[CompilerGenerated]
	private static bool _003CFillIndividualItem_003Em__2E8(UILabel l)
	{
		return l.gameObject.name.Equals("LabelsSecondPlace");
	}

	[CompilerGenerated]
	private static Transform _003CFillIndividualItem_003Em__2E9(UILabel l)
	{
		return l.transform;
	}

	[CompilerGenerated]
	private static bool _003CFillIndividualItem_003Em__2EA(UILabel l)
	{
		return l.gameObject.name.Equals("LabelsThirdPlace");
	}

	[CompilerGenerated]
	private static Transform _003CFillIndividualItem_003Em__2EB(UILabel l)
	{
		return l.transform;
	}

	[CompilerGenerated]
	private static UILabel _003CFillIndividualItem_003Em__2ED(Transform t)
	{
		return t.gameObject.GetComponent<UILabel>();
	}

	[CompilerGenerated]
	private static bool _003CFillClanItem_003Em__2EF(UILabel l)
	{
		return l.gameObject.name.Equals("LabelsFirstPlace");
	}

	[CompilerGenerated]
	private static Transform _003CFillClanItem_003Em__2F0(UILabel l)
	{
		return l.transform;
	}

	[CompilerGenerated]
	private static bool _003CFillClanItem_003Em__2F1(UILabel l)
	{
		return l.gameObject.name.Equals("LabelsSecondPlace");
	}

	[CompilerGenerated]
	private static Transform _003CFillClanItem_003Em__2F2(UILabel l)
	{
		return l.transform;
	}

	[CompilerGenerated]
	private static bool _003CFillClanItem_003Em__2F3(UILabel l)
	{
		return l.gameObject.name.Equals("LabelsThirdPlace");
	}

	[CompilerGenerated]
	private static Transform _003CFillClanItem_003Em__2F4(UILabel l)
	{
		return l.transform;
	}

	[CompilerGenerated]
	private static UILabel _003CFillClanItem_003Em__2F6(Transform t)
	{
		return t.gameObject.GetComponent<UILabel>();
	}

	[CompilerGenerated]
	private static int _003CGroupAndOrder_003Em__2F8(LeaderboardItemViewModel vm)
	{
		return vm.WinCount;
	}

	[CompilerGenerated]
	private static int _003CGroupAndOrder_003Em__2F9(IGrouping<int, LeaderboardItemViewModel> g)
	{
		return g.Key;
	}

	[CompilerGenerated]
	private static int _003CGroupAndOrder_003Em__2FA(LeaderboardItemViewModel vm)
	{
		return vm.Rank;
	}

	[CompilerGenerated]
	private void _003CHandlePlayerClicked_003Em__2FB(bool needUpdateFriendList)
	{
		Panel.gameObject.SetActive(true);
		GameObject topFriendsGrid = TopFriendsGrid;
		if (_003C_003Ef__am_0024cache3C == null)
		{
			_003C_003Ef__am_0024cache3C = _003CHandlePlayerClicked_003Em__311;
		}
		UIWrapContent o = topFriendsGrid.Map(_003C_003Ef__am_0024cache3C);
		if (_003C_003Ef__am_0024cache3D == null)
		{
			_003C_003Ef__am_0024cache3D = _003CHandlePlayerClicked_003Em__312;
		}
		o.Do(_003C_003Ef__am_0024cache3D);
		GameObject topPlayersGrid = TopPlayersGrid;
		if (_003C_003Ef__am_0024cache3E == null)
		{
			_003C_003Ef__am_0024cache3E = _003CHandlePlayerClicked_003Em__313;
		}
		UIWrapContent o2 = topPlayersGrid.Map(_003C_003Ef__am_0024cache3E);
		if (_003C_003Ef__am_0024cache3F == null)
		{
			_003C_003Ef__am_0024cache3F = _003CHandlePlayerClicked_003Em__314;
		}
		o2.Do(_003C_003Ef__am_0024cache3F);
		GameObject topClansGrid = TopClansGrid;
		if (_003C_003Ef__am_0024cache40 == null)
		{
			_003C_003Ef__am_0024cache40 = _003CHandlePlayerClicked_003Em__315;
		}
		UIWrapContent o3 = topClansGrid.Map(_003C_003Ef__am_0024cache40);
		if (_003C_003Ef__am_0024cache41 == null)
		{
			_003C_003Ef__am_0024cache41 = _003CHandlePlayerClicked_003Em__316;
		}
		o3.Do(_003C_003Ef__am_0024cache41);
		GameObject topLeagueGrid = TopLeagueGrid;
		if (_003C_003Ef__am_0024cache42 == null)
		{
			_003C_003Ef__am_0024cache42 = _003CHandlePlayerClicked_003Em__317;
		}
		UIWrapContent o4 = topLeagueGrid.Map(_003C_003Ef__am_0024cache42);
		if (_003C_003Ef__am_0024cache43 == null)
		{
			_003C_003Ef__am_0024cache43 = _003CHandlePlayerClicked_003Em__318;
		}
		o4.Do(_003C_003Ef__am_0024cache43);
		GameObject topFriendsGrid2 = TopFriendsGrid;
		if (_003C_003Ef__am_0024cache44 == null)
		{
			_003C_003Ef__am_0024cache44 = _003CHandlePlayerClicked_003Em__319;
		}
		UIScrollView o5 = topFriendsGrid2.Map(_003C_003Ef__am_0024cache44);
		if (_003C_003Ef__am_0024cache45 == null)
		{
			_003C_003Ef__am_0024cache45 = _003CHandlePlayerClicked_003Em__31A;
		}
		o5.Do(_003C_003Ef__am_0024cache45);
		GameObject topPlayersGrid2 = TopPlayersGrid;
		if (_003C_003Ef__am_0024cache46 == null)
		{
			_003C_003Ef__am_0024cache46 = _003CHandlePlayerClicked_003Em__31B;
		}
		UIScrollView o6 = topPlayersGrid2.Map(_003C_003Ef__am_0024cache46);
		if (_003C_003Ef__am_0024cache47 == null)
		{
			_003C_003Ef__am_0024cache47 = _003CHandlePlayerClicked_003Em__31C;
		}
		o6.Do(_003C_003Ef__am_0024cache47);
		GameObject topClansGrid2 = TopClansGrid;
		if (_003C_003Ef__am_0024cache48 == null)
		{
			_003C_003Ef__am_0024cache48 = _003CHandlePlayerClicked_003Em__31D;
		}
		UIScrollView o7 = topClansGrid2.Map(_003C_003Ef__am_0024cache48);
		if (_003C_003Ef__am_0024cache49 == null)
		{
			_003C_003Ef__am_0024cache49 = _003CHandlePlayerClicked_003Em__31E;
		}
		o7.Do(_003C_003Ef__am_0024cache49);
		GameObject topLeagueGrid2 = TopLeagueGrid;
		if (_003C_003Ef__am_0024cache4A == null)
		{
			_003C_003Ef__am_0024cache4A = _003CHandlePlayerClicked_003Em__31F;
		}
		UIScrollView o8 = topLeagueGrid2.Map(_003C_003Ef__am_0024cache4A);
		if (_003C_003Ef__am_0024cache4B == null)
		{
			_003C_003Ef__am_0024cache4B = _003CHandlePlayerClicked_003Em__320;
		}
		o8.Do(_003C_003Ef__am_0024cache4B);
		Panel.alpha = 1f;
		_profileIsOpened = false;
	}

	[CompilerGenerated]
	private static MainMenuController _003CAwake_003Em__2FC()
	{
		return MainMenuController.sharedController;
	}

	[CompilerGenerated]
	private void _003COnDestroy_003Em__2FD(MainMenuController m)
	{
		m.BackPressed -= ReturnBack;
	}

	[CompilerGenerated]
	private void _003CGetReturnFuture_003Em__2FE(MainMenuController m)
	{
		m.BackPressed -= ReturnBack;
	}

	[CompilerGenerated]
	private void _003CGetReturnFuture_003Em__2FF(MainMenuController m)
	{
		m.BackPressed += ReturnBack;
	}

	[CompilerGenerated]
	private static UIWrapContent _003CReturnBack_003Em__300(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__301(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIWrapContent _003CReturnBack_003Em__302(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__303(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIWrapContent _003CReturnBack_003Em__304(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__305(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIWrapContent _003CReturnBack_003Em__306(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__307(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIScrollView _003CReturnBack_003Em__308(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__309(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private static UIScrollView _003CReturnBack_003Em__30A(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__30B(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private static UIScrollView _003CReturnBack_003Em__30C(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__30D(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private static UIScrollView _003CReturnBack_003Em__30E(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CReturnBack_003Em__30F(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private void _003CReturnBack_003Em__310(MainMenuController m)
	{
		m.BackPressed -= ReturnBack;
	}

	[CompilerGenerated]
	private static UIWrapContent _003CHandlePlayerClicked_003Em__311(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__312(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIWrapContent _003CHandlePlayerClicked_003Em__313(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__314(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIWrapContent _003CHandlePlayerClicked_003Em__315(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__316(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIWrapContent _003CHandlePlayerClicked_003Em__317(GameObject go)
	{
		return go.GetComponent<UIWrapContent>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__318(UIWrapContent w)
	{
		w.SortBasedOnScrollMovement();
		w.WrapContent();
	}

	[CompilerGenerated]
	private static UIScrollView _003CHandlePlayerClicked_003Em__319(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__31A(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private static UIScrollView _003CHandlePlayerClicked_003Em__31B(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__31C(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private static UIScrollView _003CHandlePlayerClicked_003Em__31D(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__31E(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}

	[CompilerGenerated]
	private static UIScrollView _003CHandlePlayerClicked_003Em__31F(GameObject go)
	{
		return go.GetComponentInParent<UIScrollView>();
	}

	[CompilerGenerated]
	private static void _003CHandlePlayerClicked_003Em__320(UIScrollView s)
	{
		s.ResetPosition();
		s.UpdatePosition();
	}
}
