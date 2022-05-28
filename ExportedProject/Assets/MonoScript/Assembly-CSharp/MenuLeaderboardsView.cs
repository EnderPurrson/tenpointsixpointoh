using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

internal sealed class MenuLeaderboardsView : MonoBehaviour
{
	public enum State
	{
		Friends = 0,
		BestPlayers = 1,
		Clans = 2
	}

	public UIGrid friendsGrid;

	public UIGrid bestPlayersGrid;

	public UIGrid clansGrid;

	public UIButton friendsButton;

	public UIButton bestPlayersButton;

	public UIButton clansButton;

	public UIDragScrollView friendsPanel;

	public UIDragScrollView bestPlayersPanel;

	public UIDragScrollView clansPanel;

	public UIScrollView friendsScroll;

	public UIScrollView bestPlayersScroll;

	public UIScrollView clansScroll;

	public LeaderboardItemView footer;

	public LeaderboardItemView clanFooter;

	public UISprite temporaryBackground;

	public UISprite bestPlayersDefaultSprite;

	public UISprite clansDefaultSprite;

	public UILabel nickOrClanName;

	public ToggleButton btnLeaderboards;

	public GameObject opened;

	private State _currentState;

	private Vector3 _desiredPosition = Vector3.zero;

	private Vector3 _outOfScreenPosition = Vector3.zero;

	[CompilerGenerated]
	private static Func<UIButton, bool> _003C_003Ef__am_0024cache17;

	[CompilerGenerated]
	private static Func<UIScrollView, bool> _003C_003Ef__am_0024cache18;

	public State CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			friendsButton.isEnabled = value != State.Friends;
			Transform transform = friendsButton.transform.FindChild("IdleLabel");
			Transform transform2 = friendsButton.transform.FindChild("ActiveLabel");
			if (transform != null && (bool)transform2)
			{
				transform.gameObject.SetActive(value != State.Friends);
				transform2.gameObject.SetActive(value == State.Friends);
			}
			bestPlayersButton.isEnabled = value != State.BestPlayers;
			Transform transform3 = bestPlayersButton.transform.FindChild("IdleLabel");
			Transform transform4 = bestPlayersButton.transform.FindChild("ActiveLabel");
			if (transform3 != null && (bool)transform4)
			{
				transform3.gameObject.SetActive(value != State.BestPlayers);
				transform4.gameObject.SetActive(value == State.BestPlayers);
			}
			clansButton.isEnabled = value != State.Clans;
			Transform transform5 = clansButton.transform.FindChild("IdleLabel");
			Transform transform6 = clansButton.transform.FindChild("ActiveLabel");
			if (transform5 != null && (bool)transform6)
			{
				transform5.gameObject.SetActive(value != State.Clans);
				transform6.gameObject.SetActive(value == State.Clans);
			}
			if (nickOrClanName != null)
			{
				nickOrClanName.text = ((value != State.Clans) ? LocalizationStore.Get("Key_0071") : LocalizationStore.Get("Key_0257"));
			}
			friendsPanel.transform.localPosition = ((value != 0) ? _outOfScreenPosition : _desiredPosition);
			bestPlayersPanel.transform.localPosition = ((value != State.BestPlayers) ? _outOfScreenPosition : _desiredPosition);
			clansPanel.transform.localPosition = ((value != State.Clans) ? _outOfScreenPosition : _desiredPosition);
			_currentState = value;
		}
	}

	public static bool IsNeedShow
	{
		get
		{
			bool hasFriends = FriendsController.HasFriends;
			return false;
		}
	}

	public static int PageSize
	{
		get
		{
			return 9;
		}
	}

	public IList<LeaderboardItemViewModel> FriendsList
	{
		set
		{
			StartCoroutine(SetGrid(friendsGrid, value, temporaryBackground));
		}
	}

	public IList<LeaderboardItemViewModel> BestPlayersList
	{
		set
		{
			StartCoroutine(SetGrid(bestPlayersGrid, value, temporaryBackground));
			if (bestPlayersDefaultSprite != null)
			{
				bestPlayersDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(bestPlayersDefaultSprite);
				bestPlayersDefaultSprite = null;
			}
		}
	}

	public IList<LeaderboardItemViewModel> ClansList
	{
		set
		{
			StartCoroutine(SetGrid(clansGrid, value, temporaryBackground));
			if (clansDefaultSprite != null)
			{
				clansDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(clansDefaultSprite);
				clansDefaultSprite = null;
			}
		}
	}

	public LeaderboardItemViewModel SelfStats
	{
		set
		{
			footer.Reset(value);
			footer.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	public LeaderboardItemViewModel SelfClanStats
	{
		set
		{
			clanFooter.Reset(value);
			clanFooter.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	private void OnEnable()
	{
		StartCoroutine(UpdateGridsAndScrollers());
	}

	private void Awake()
	{
		footer.gameObject.SetActive(false);
		clanFooter.gameObject.SetActive(false);
		if (bestPlayersDefaultSprite != null)
		{
			bestPlayersDefaultSprite.gameObject.SetActive(true);
		}
		if (clansDefaultSprite != null)
		{
			clansDefaultSprite.gameObject.SetActive(true);
		}
		temporaryBackground.gameObject.SetActive(false);
	}

	private void Start()
	{
		_desiredPosition = friendsPanel.transform.localPosition;
		_outOfScreenPosition = new Vector3(9000f, _desiredPosition.y, _desiredPosition.z);
		UIButton[] source = new UIButton[3] { friendsButton, bestPlayersButton, clansButton };
		if (_003C_003Ef__am_0024cache17 == null)
		{
			_003C_003Ef__am_0024cache17 = _003CStart_003Em__369;
		}
		IEnumerable<UIButton> enumerable = source.Where(_003C_003Ef__am_0024cache17);
		foreach (UIButton item in enumerable)
		{
			ButtonHandler component = item.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleTabPressed;
			}
		}
		UIScrollView[] source2 = new UIScrollView[3] { friendsScroll, bestPlayersScroll, clansScroll };
		if (_003C_003Ef__am_0024cache18 == null)
		{
			_003C_003Ef__am_0024cache18 = _003CStart_003Em__36A;
		}
		IEnumerable<UIScrollView> enumerable2 = source2.Where(_003C_003Ef__am_0024cache18);
		foreach (UIScrollView item2 in enumerable2)
		{
			item2.ResetPosition();
		}
		CurrentState = State.BestPlayers;
		bool isNeedShow = IsNeedShow;
		Show(isNeedShow, false);
		btnLeaderboards.IsChecked = false;
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (gameObject == friendsButton.gameObject)
		{
			CurrentState = State.Friends;
		}
		else if (gameObject == bestPlayersButton.gameObject)
		{
			CurrentState = State.BestPlayers;
		}
		else if (gameObject == clansButton.gameObject)
		{
			CurrentState = State.Clans;
		}
	}

	private static IEnumerator SetGrid(UIGrid grid, IList<LeaderboardItemViewModel> value, UISprite temporaryBackground)
	{
		temporaryBackground.gameObject.SetActive(true);
		try
		{
			if (grid == null)
			{
				yield break;
			}
			while (!grid.gameObject.activeInHierarchy)
			{
				yield return null;
			}
			IEnumerable<LeaderboardItemViewModel> enumerable2;
			if (value == null)
			{
				IEnumerable<LeaderboardItemViewModel> enumerable = new List<LeaderboardItemViewModel>();
				enumerable2 = enumerable;
			}
			else
			{
				if (_003CSetGrid_003Ec__Iterator167._003C_003Ef__am_0024cache10 == null)
				{
					_003CSetGrid_003Ec__Iterator167._003C_003Ef__am_0024cache10 = _003CSetGrid_003Ec__Iterator167._003C_003Em__36B;
				}
				enumerable2 = value.Where(_003CSetGrid_003Ec__Iterator167._003C_003Ef__am_0024cache10);
			}
			IEnumerable<LeaderboardItemViewModel> filteredList = enumerable2;
			List<Transform> list = grid.GetChildList();
			for (int i = 0; i != list.Count; i++)
			{
				UnityEngine.Object.Destroy(list[i].gameObject);
			}
			list.Clear();
			grid.Reposition();
			foreach (LeaderboardItemViewModel item in filteredList)
			{
				GameObject o = ((!item.Highlight) ? (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardItem")) as GameObject) : (UnityEngine.Object.Instantiate(Resources.Load("Leaderboards/MenuLeaderboardSelectedItem")) as GameObject));
				if (o != null)
				{
					LeaderboardItemView liv = o.GetComponent<LeaderboardItemView>();
					if (liv != null)
					{
						liv.Reset(item);
						o.transform.parent = grid.transform;
						grid.AddChild(o.transform);
						o.transform.localScale = Vector3.one;
					}
				}
			}
			grid.Reposition();
			UIScrollView scrollView = grid.transform.parent.gameObject.GetComponent<UIScrollView>();
			if (scrollView != null)
			{
				scrollView.enabled = true;
				yield return null;
				scrollView.ResetPosition();
				scrollView.UpdatePosition();
				yield return null;
				scrollView.enabled = value.Count >= 10;
			}
		}
		finally
		{
			temporaryBackground.gameObject.SetActive(false);
		}
	}

	private IEnumerator UpdateGridsAndScrollers()
	{
		UIGrid[] source = new UIGrid[3] { friendsGrid, bestPlayersGrid, clansGrid };
		if (_003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Ef__am_0024cache9 == null)
		{
			_003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Ef__am_0024cache9 = _003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Em__36C;
		}
		IEnumerable<UIGrid> grids = source.Where(_003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Ef__am_0024cache9);
		foreach (UIGrid g in grids)
		{
			g.Reposition();
		}
		yield return null;
		UIScrollView[] source2 = new UIScrollView[3] { friendsScroll, bestPlayersScroll, clansScroll };
		if (_003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Ef__am_0024cacheA == null)
		{
			_003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Ef__am_0024cacheA = _003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Em__36D;
		}
		IEnumerable<UIScrollView> scrolls = source2.Where(_003CUpdateGridsAndScrollers_003Ec__Iterator168._003C_003Ef__am_0024cacheA);
		foreach (UIScrollView s in scrolls)
		{
			s.ResetPosition();
			s.UpdatePosition();
		}
	}

	public void Show(bool needShow, bool animate)
	{
	}

	[CompilerGenerated]
	private static bool _003CStart_003Em__369(UIButton b)
	{
		return b != null;
	}

	[CompilerGenerated]
	private static bool _003CStart_003Em__36A(UIScrollView s)
	{
		return s != null;
	}
}
