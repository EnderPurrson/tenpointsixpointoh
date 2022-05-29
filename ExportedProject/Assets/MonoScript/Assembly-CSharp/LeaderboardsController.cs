using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class LeaderboardsController : MonoBehaviour
{
	private FriendsGUIController _friendsGuiController;

	private LeaderboardsView _leaderboardsView;

	private string _playerId = string.Empty;

	public FriendsGUIController FriendsGuiController
	{
		private get
		{
			return this._friendsGuiController;
		}
		set
		{
			this._friendsGuiController = value;
		}
	}

	public LeaderboardsView LeaderboardsView
	{
		private get
		{
			return this._leaderboardsView;
		}
		set
		{
			this._leaderboardsView = value;
			if (this._leaderboardsView != null)
			{
				this._leaderboardsView.BackPressed += new EventHandler(this.HandleBackPressed);
			}
		}
	}

	public string PlayerId
	{
		private get
		{
			return this._playerId;
		}
		set
		{
			this._playerId = value ?? string.Empty;
		}
	}

	public LeaderboardsController()
	{
	}

	[DebuggerHidden]
	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		LeaderboardsController.u003cGetLeaderboardsCoroutineu003ec__Iterator15E variable = null;
		return variable;
	}

	private void HandleBackPressed(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("Back pressed.");
		}
		if (this._friendsGuiController != null)
		{
			this._friendsGuiController.leaderboardsView.gameObject.SetActive(false);
			this._friendsGuiController.friendsPanel.gameObject.SetActive(true);
		}
	}

	private void HandleRequestCompleted(WWW request)
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("HandleRequestCompleted()");
		}
		if (request == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			UnityEngine.Debug.LogWarning(request.error);
			return;
		}
		string str = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(str))
		{
			UnityEngine.Debug.LogWarning("Leaderboars response is empty.");
			return;
		}
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			UnityEngine.Debug.LogWarning("Leaderboards response is ill-formed.");
			return;
		}
		if (!strs.Any<KeyValuePair<string, object>>())
		{
			UnityEngine.Debug.LogWarning("Leaderboards response contains no elements.");
			return;
		}
		Func<IList<LeaderboardItemViewModel>, IList<LeaderboardItemViewModel>> func = (IList<LeaderboardItemViewModel> items) => {
			List<LeaderboardItemViewModel> leaderboardItemViewModels = new List<LeaderboardItemViewModel>();
			IOrderedEnumerable<IGrouping<int, LeaderboardItemViewModel>> groupings = 
				from vm in items
				group vm by vm.WinCount into g
				orderby g.Key descending
				select g;
			int num = 1;
			IEnumerator<IGrouping<int, LeaderboardItemViewModel>> enumerator = groupings.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, LeaderboardItemViewModel> current = enumerator.Current;
					IEnumerator<LeaderboardItemViewModel> enumerator1 = (
						from vm in current
						orderby vm.Rank descending
						select vm).GetEnumerator();
					try
					{
						while (enumerator1.MoveNext())
						{
							LeaderboardItemViewModel leaderboardItemViewModel = enumerator1.Current;
							leaderboardItemViewModel.Place = num;
							leaderboardItemViewModels.Add(leaderboardItemViewModel);
						}
					}
					finally
					{
						if (enumerator1 == null)
						{
						}
						enumerator1.Dispose();
					}
					num += current.Count<LeaderboardItemViewModel>();
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			return leaderboardItemViewModels;
		};
		List<LeaderboardItemViewModel> leaderboardItemViewModels1 = LeaderboardsController.ParseLeaderboardEntries(this._playerId, "friends", strs);
		List<LeaderboardItemViewModel> leaderboardItemViewModels2 = leaderboardItemViewModels1;
		LeaderboardItemViewModel leaderboardItemViewModel1 = new LeaderboardItemViewModel()
		{
			Id = this._playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			Rank = ExperienceController.sharedController.currentLevel,
			WinCount = RatingSystem.instance.currentRating,
			Highlight = true,
			ClanLogo = FriendsController.sharedController.clanLogo ?? string.Empty
		};
		leaderboardItemViewModels2.Add(leaderboardItemViewModel1);
		func(leaderboardItemViewModels1);
		List<LeaderboardItemViewModel> leaderboardItemViewModels3 = LeaderboardsController.ParseLeaderboardEntries(this._playerId, "best_players", strs);
		func(leaderboardItemViewModels3);
		List<LeaderboardItemViewModel> leaderboardItemViewModels4 = LeaderboardsController.ParseLeaderboardEntries(this._playerId, "top_clans", strs);
		func(leaderboardItemViewModels4);
	}

	private void OnDestroy()
	{
		if (this._leaderboardsView != null)
		{
			this._leaderboardsView.BackPressed -= new EventHandler(this.HandleBackPressed);
		}
	}

	internal static List<LeaderboardItemViewModel> ParseLeaderboardEntries(string entryId, string leaderboardName, Dictionary<string, object> response)
	{
		object obj;
		object obj1;
		object obj2;
		int num;
		object obj3;
		object obj4;
		object obj5;
		object obj6;
		if (string.IsNullOrEmpty(leaderboardName))
		{
			throw new ArgumentException("Leaderbord should not be empty.", "leaderboardName");
		}
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		List<LeaderboardItemViewModel> leaderboardItemViewModels = new List<LeaderboardItemViewModel>();
		if (response.TryGetValue(leaderboardName, out obj))
		{
			List<object> objs = obj as List<object>;
			if (objs != null)
			{
				IEnumerator<Dictionary<string, object>> enumerator = objs.OfType<Dictionary<string, object>>().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Dictionary<string, object> current = enumerator.Current;
						LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel();
						if (current.TryGetValue("id", out obj1))
						{
							leaderboardItemViewModel.Id = Convert.ToString(obj1);
							leaderboardItemViewModel.Highlight = (string.IsNullOrEmpty(leaderboardItemViewModel.Id) ? false : leaderboardItemViewModel.Id.Equals(entryId));
						}
						if (current.TryGetValue("rank", out obj2) && int.TryParse(obj2 as string, out num))
						{
							leaderboardItemViewModel.Rank = num;
						}
						else if (current.TryGetValue("member_cnt", out obj2))
						{
							try
							{
								leaderboardItemViewModel.Rank = Convert.ToInt32(obj2);
							}
							catch (Exception exception)
							{
								UnityEngine.Debug.LogException(exception);
							}
						}
						if (current.TryGetValue("nick", out obj3))
						{
							leaderboardItemViewModel.Nickname = obj3 as string ?? string.Empty;
						}
						else if (current.TryGetValue("name", out obj3))
						{
							leaderboardItemViewModel.Nickname = obj3 as string ?? string.Empty;
						}
						try
						{
							if (current.TryGetValue("trophies", out obj4))
							{
								leaderboardItemViewModel.WinCount = Convert.ToInt32(obj4);
							}
							else if (current.TryGetValue("wins", out obj4))
							{
								leaderboardItemViewModel.WinCount = Convert.ToInt32(obj4);
							}
							else if (current.TryGetValue("win", out obj4))
							{
								leaderboardItemViewModel.WinCount = Convert.ToInt32(obj4);
							}
						}
						catch (Exception exception1)
						{
							UnityEngine.Debug.LogException(exception1);
						}
						if (!current.TryGetValue("logo", out obj5))
						{
							leaderboardItemViewModel.ClanLogo = string.Empty;
						}
						else
						{
							leaderboardItemViewModel.ClanLogo = obj5 as string ?? string.Empty;
						}
						if (current.TryGetValue("name", out obj6))
						{
							leaderboardItemViewModel.ClanName = obj6 as string ?? string.Empty;
						}
						else if (!current.TryGetValue("clan_name", out obj6))
						{
							leaderboardItemViewModel.ClanName = string.Empty;
						}
						else
						{
							leaderboardItemViewModel.ClanName = obj6 as string ?? string.Empty;
						}
						leaderboardItemViewModels.Add(leaderboardItemViewModel);
					}
				}
				finally
				{
					if (enumerator == null)
					{
					}
					enumerator.Dispose();
				}
			}
		}
		return leaderboardItemViewModels;
	}

	public void RequestLeaderboards()
	{
		if (string.IsNullOrEmpty(this._playerId))
		{
			UnityEngine.Debug.Log("Player id should not be empty.");
		}
		else
		{
			base.StartCoroutine(this.GetLeaderboardsCoroutine(this._playerId));
		}
	}

	private void Start()
	{
		this.RequestLeaderboards();
	}
}