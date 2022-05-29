using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

internal sealed class MenuLeaderboardsController : MonoBehaviour
{
	private const string MenuLeaderboardsResponseCache = "MenuLeaderboardsFriendsCache";

	internal const bool NewLeaderboards = true;

	public static MenuLeaderboardsController sharedController;

	private MenuLeaderboardsView _menuLeaderboardsView;

	private string _playerId = string.Empty;

	public bool IsOpened
	{
		get
		{
			return this.menuLeaderboardsView.opened.activeSelf;
		}
	}

	public MenuLeaderboardsView menuLeaderboardsView
	{
		get
		{
			return this._menuLeaderboardsView;
		}
	}

	static MenuLeaderboardsController()
	{
	}

	public MenuLeaderboardsController()
	{
	}

	private void FillListsWithResponseText(string responseText)
	{
		IEnumerator<float> enumerator = this.FillListsWithResponseTextAsync(responseText).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				float current = (float)enumerator.Current;
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

	[DebuggerHidden]
	private IEnumerable<float> FillListsWithResponseTextAsync(string responseText)
	{
		MenuLeaderboardsController.u003cFillListsWithResponseTextAsyncu003ec__Iterator165 variable = null;
		return variable;
	}

	private static LeaderboardItemViewModel FulfillSelfStats(LeaderboardItemViewModel selfStats, Dictionary<string, object> response)
	{
		object obj;
		LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel()
		{
			Id = selfStats.Id,
			Nickname = selfStats.Nickname,
			WinCount = selfStats.WinCount,
			Place = -2147483648,
			Highlight = true
		};
		LeaderboardItemViewModel num = leaderboardItemViewModel;
		if (response.TryGetValue("me", out obj))
		{
			Dictionary<string, object> strs = obj as Dictionary<string, object>;
			if (strs != null)
			{
				try
				{
					num.WinCount = Convert.ToInt32(strs["wins"]);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
		}
		return num;
	}

	[DebuggerHidden]
	private IEnumerator GetLeaderboardsCoroutine(string playerId)
	{
		MenuLeaderboardsController.u003cGetLeaderboardsCoroutineu003ec__Iterator166 variable = null;
		return variable;
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
		PlayerPrefs.SetString("MenuLeaderboardsFriendsCache", str);
		this.FillListsWithResponseText(str);
	}

	public void OnBtnLeaderboardsOffClick()
	{
	}

	public void OnBtnLeaderboardsOnClick()
	{
	}

	private void OnDestroy()
	{
		MenuLeaderboardsController.sharedController = null;
	}

	public void RefreshWithCache()
	{
		if (PlayerPrefs.HasKey("MenuLeaderboardsFriendsCache"))
		{
			this.FillListsWithResponseText(PlayerPrefs.GetString("MenuLeaderboardsFriendsCache"));
		}
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		MenuLeaderboardsController.u003cStartu003ec__Iterator164 variable = null;
		return variable;
	}

	private void TransitToFallbackState()
	{
		LeaderboardItemViewModel leaderboardItemViewModel = new LeaderboardItemViewModel()
		{
			Id = this._playerId,
			Nickname = ProfileController.GetPlayerNameOrDefault(),
			WinCount = RatingSystem.instance.currentRating,
			Place = 1,
			Highlight = true
		};
		IList<LeaderboardItemViewModel> leaderboardItemViewModels = new List<LeaderboardItemViewModel>(MenuLeaderboardsView.PageSize)
		{
			leaderboardItemViewModel
		};
		for (int i = 0; i < MenuLeaderboardsView.PageSize - 1; i++)
		{
			leaderboardItemViewModels.Add(LeaderboardItemViewModel.Empty);
		}
		this._menuLeaderboardsView.FriendsList = leaderboardItemViewModels;
	}
}