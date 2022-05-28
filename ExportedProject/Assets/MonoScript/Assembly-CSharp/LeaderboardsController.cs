using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

internal sealed class LeaderboardsController : MonoBehaviour
{
	private FriendsGUIController _friendsGuiController;

	private LeaderboardsView _leaderboardsView;

	private string _playerId = string.Empty;

	[CompilerGenerated]
	private static Func<IList<LeaderboardItemViewModel>, IList<LeaderboardItemViewModel>> _003C_003Ef__am_0024cache3;

	[CompilerGenerated]
	private static Func<LeaderboardItemViewModel, int> _003C_003Ef__am_0024cache4;

	[CompilerGenerated]
	private static Func<IGrouping<int, LeaderboardItemViewModel>, int> _003C_003Ef__am_0024cache5;

	[CompilerGenerated]
	private static Func<LeaderboardItemViewModel, int> _003C_003Ef__am_0024cache6;

	public LeaderboardsView LeaderboardsView
	{
		private get
		{
			return _leaderboardsView;
		}
		set
		{
			_leaderboardsView = value;
			if (_leaderboardsView != null)
			{
				_leaderboardsView.BackPressed += HandleBackPressed;
			}
		}
	}

	public FriendsGUIController FriendsGuiController
	{
		private get
		{
			return _friendsGuiController;
		}
		set
		{
			_friendsGuiController = value;
		}
	}

	public string PlayerId
	{
		private get
		{
			return _playerId;
		}
		set
		{
			_playerId = value ?? string.Empty;
		}
	}

	public void RequestLeaderboards()
	{
		if (!string.IsNullOrEmpty(_playerId))
		{
			StartCoroutine(GetLeaderboardsCoroutine(_playerId));
		}
		else
		{
			Debug.Log("Player id should not be empty.");
		}
	}

	internal static List<LeaderboardItemViewModel> ParseLeaderboardEntries(string entryId, string leaderboardName, Dictionary<string, object> response)
	{
		if (string.IsNullOrEmpty(leaderboardName))
		{
			throw new ArgumentException("Leaderbord should not be empty.", "leaderboardName");
		}
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		object value;
		if (response.TryGetValue(leaderboardName, out value))
		{
			List<object> list2 = value as List<object>;
			if (list2 != null)
			{
				IEnumerable<Dictionary<string, object>> enumerable = list2.OfType<Dictionary<string, object>>();
				{
					foreach (Dictionary<string, object> item in enumerable)
					{
						LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel();
						object value2;
						if (item.TryGetValue("id", out value2))
						{
							leaderboardItemViewModel.Id = Convert.ToString(value2);
							leaderboardItemViewModel.Highlight = !string.IsNullOrEmpty(leaderboardItemViewModel.Id) && leaderboardItemViewModel.Id.Equals(entryId);
						}
						object value3;
						int result;
						if (item.TryGetValue("rank", out value3) && int.TryParse(value3 as string, out result))
						{
							leaderboardItemViewModel.Rank = result;
						}
						else if (item.TryGetValue("member_cnt", out value3))
						{
							try
							{
								leaderboardItemViewModel.Rank = Convert.ToInt32(value3);
							}
							catch (Exception exception)
							{
								Debug.LogException(exception);
							}
						}
						object value4;
						if (item.TryGetValue("nick", out value4))
						{
							leaderboardItemViewModel.Nickname = (value4 as string) ?? string.Empty;
						}
						else if (item.TryGetValue("name", out value4))
						{
							leaderboardItemViewModel.Nickname = (value4 as string) ?? string.Empty;
						}
						try
						{
							object value5;
							if (item.TryGetValue("trophies", out value5))
							{
								leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
							}
							else if (item.TryGetValue("wins", out value5))
							{
								leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
							}
							else if (item.TryGetValue("win", out value5))
							{
								leaderboardItemViewModel.WinCount = Convert.ToInt32(value5);
							}
						}
						catch (Exception exception2)
						{
							Debug.LogException(exception2);
						}
						object value6;
						if (item.TryGetValue("logo", out value6))
						{
							leaderboardItemViewModel.ClanLogo = (value6 as string) ?? string.Empty;
						}
						else
						{
							leaderboardItemViewModel.ClanLogo = string.Empty;
						}
						object value7;
						if (item.TryGetValue("name", out value7))
						{
							leaderboardItemViewModel.ClanName = (value7 as string) ?? string.Empty;
						}
						else if (item.TryGetValue("clan_name", out value7))
						{
							leaderboardItemViewModel.ClanName = (value7 as string) ?? string.Empty;
						}
						else
						{
							leaderboardItemViewModel.ClanName = string.Empty;
						}
						list.Add(leaderboardItemViewModel);
					}
					return list;
				}
			}
		}
		return list;
	}

	private void OnDestroy()
	{
		if (_leaderboardsView != null)
		{
			_leaderboardsView.BackPressed -= HandleBackPressed;
		}
	}

	private void Start()
	{
		RequestLeaderboards();
	}

	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			Debug.LogWarning("Player id should not be empty.");
			yield break;
		}
		Debug.Log("LeaderboardsController.GetLeaderboardsCoroutine(" + playerId + ")");
		WWWForm form = new WWWForm();
		form.AddField("action", "get_leaderboards_league");
		form.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
		form.AddField("id", playerId);
		form.AddField("league_id", LeaderboardScript.GetLeagueId());
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_leaderboards_league"));
		if (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
		{
			Debug.Log("Waiting previous leaderboards request...");
			while (FriendsController.sharedController.NumberOfBestPlayersRequests > 0)
			{
				yield return null;
			}
		}
		FriendsController.sharedController.NumberOfBestPlayersRequests++;
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty);
		yield return request;
		FriendsController.sharedController.NumberOfBestPlayersRequests--;
		HandleRequestCompleted(request);
	}

	private void HandleBackPressed(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("Back pressed.");
		}
		if (_friendsGuiController != null)
		{
			_friendsGuiController.leaderboardsView.gameObject.SetActive(false);
			_friendsGuiController.friendsPanel.gameObject.SetActive(true);
		}
	}

	private void HandleRequestCompleted(WWW request)
	{
		if (Application.isEditor)
		{
			Debug.Log("HandleRequestCompleted()");
		}
		if (request == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogWarning(request.error);
			return;
		}
		string text = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogWarning("Leaderboars response is empty.");
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			Debug.LogWarning("Leaderboards response is ill-formed.");
			return;
		}
		if (!dictionary.Any())
		{
			Debug.LogWarning("Leaderboards response contains no elements.");
			return;
		}
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = _003CHandleRequestCompleted_003Em__351;
		}
		Func<IList<LeaderboardItemViewModel>, IList<LeaderboardItemViewModel>> func = _003C_003Ef__am_0024cache3;
		List<LeaderboardItemViewModel> list = ParseLeaderboardEntries(_playerId, "friends", dictionary);
		list.Add(new LeaderboardItemViewModel
		{
			Id = _playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			Rank = ExperienceController.sharedController.currentLevel,
			WinCount = RatingSystem.instance.currentRating,
			Highlight = true,
			ClanLogo = (FriendsController.sharedController.clanLogo ?? string.Empty)
		});
		IList<LeaderboardItemViewModel> list2 = func(list);
		List<LeaderboardItemViewModel> arg = ParseLeaderboardEntries(_playerId, "best_players", dictionary);
		IList<LeaderboardItemViewModel> list3 = func(arg);
		List<LeaderboardItemViewModel> arg2 = ParseLeaderboardEntries(_playerId, "top_clans", dictionary);
		IList<LeaderboardItemViewModel> list4 = func(arg2);
	}

	[CompilerGenerated]
	private static IList<LeaderboardItemViewModel> _003CHandleRequestCompleted_003Em__351(IList<LeaderboardItemViewModel> items)
	{
		List<LeaderboardItemViewModel> list = new List<LeaderboardItemViewModel>();
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = _003CHandleRequestCompleted_003Em__352;
		}
		IEnumerable<IGrouping<int, LeaderboardItemViewModel>> source = items.GroupBy(_003C_003Ef__am_0024cache4);
		if (_003C_003Ef__am_0024cache5 == null)
		{
			_003C_003Ef__am_0024cache5 = _003CHandleRequestCompleted_003Em__353;
		}
		IOrderedEnumerable<IGrouping<int, LeaderboardItemViewModel>> orderedEnumerable = source.OrderByDescending(_003C_003Ef__am_0024cache5);
		int num = 1;
		foreach (IGrouping<int, LeaderboardItemViewModel> item in orderedEnumerable)
		{
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003CHandleRequestCompleted_003Em__354;
			}
			IOrderedEnumerable<LeaderboardItemViewModel> orderedEnumerable2 = item.OrderByDescending(_003C_003Ef__am_0024cache6);
			foreach (LeaderboardItemViewModel item2 in orderedEnumerable2)
			{
				item2.Place = num;
				list.Add(item2);
			}
			num += item.Count();
		}
		return list;
	}

	[CompilerGenerated]
	private static int _003CHandleRequestCompleted_003Em__352(LeaderboardItemViewModel vm)
	{
		return vm.WinCount;
	}

	[CompilerGenerated]
	private static int _003CHandleRequestCompleted_003Em__353(IGrouping<int, LeaderboardItemViewModel> g)
	{
		return g.Key;
	}

	[CompilerGenerated]
	private static int _003CHandleRequestCompleted_003Em__354(LeaderboardItemViewModel vm)
	{
		return vm.Rank;
	}
}
