using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class MenuLeaderboardsView : MonoBehaviour
{
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

	private MenuLeaderboardsView.State _currentState;

	private Vector3 _desiredPosition = Vector3.zero;

	private Vector3 _outOfScreenPosition = Vector3.zero;

	public IList<LeaderboardItemViewModel> BestPlayersList
	{
		set
		{
			base.StartCoroutine(MenuLeaderboardsView.SetGrid(this.bestPlayersGrid, value, this.temporaryBackground));
			if (this.bestPlayersDefaultSprite != null)
			{
				this.bestPlayersDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(this.bestPlayersDefaultSprite);
				this.bestPlayersDefaultSprite = null;
			}
		}
	}

	public IList<LeaderboardItemViewModel> ClansList
	{
		set
		{
			base.StartCoroutine(MenuLeaderboardsView.SetGrid(this.clansGrid, value, this.temporaryBackground));
			if (this.clansDefaultSprite != null)
			{
				this.clansDefaultSprite.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(this.clansDefaultSprite);
				this.clansDefaultSprite = null;
			}
		}
	}

	public MenuLeaderboardsView.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			this.friendsButton.isEnabled = value != MenuLeaderboardsView.State.Friends;
			Transform transforms = this.friendsButton.transform.FindChild("IdleLabel");
			Transform transforms1 = this.friendsButton.transform.FindChild("ActiveLabel");
			if (transforms != null && transforms1)
			{
				transforms.gameObject.SetActive(value != MenuLeaderboardsView.State.Friends);
				transforms1.gameObject.SetActive(value == MenuLeaderboardsView.State.Friends);
			}
			this.bestPlayersButton.isEnabled = value != MenuLeaderboardsView.State.BestPlayers;
			Transform transforms2 = this.bestPlayersButton.transform.FindChild("IdleLabel");
			Transform transforms3 = this.bestPlayersButton.transform.FindChild("ActiveLabel");
			if (transforms2 != null && transforms3)
			{
				transforms2.gameObject.SetActive(value != MenuLeaderboardsView.State.BestPlayers);
				transforms3.gameObject.SetActive(value == MenuLeaderboardsView.State.BestPlayers);
			}
			this.clansButton.isEnabled = value != MenuLeaderboardsView.State.Clans;
			Transform transforms4 = this.clansButton.transform.FindChild("IdleLabel");
			Transform transforms5 = this.clansButton.transform.FindChild("ActiveLabel");
			if (transforms4 != null && transforms5)
			{
				transforms4.gameObject.SetActive(value != MenuLeaderboardsView.State.Clans);
				transforms5.gameObject.SetActive(value == MenuLeaderboardsView.State.Clans);
			}
			if (this.nickOrClanName != null)
			{
				this.nickOrClanName.text = (value != MenuLeaderboardsView.State.Clans ? LocalizationStore.Get("Key_0071") : LocalizationStore.Get("Key_0257"));
			}
			this.friendsPanel.transform.localPosition = (value != MenuLeaderboardsView.State.Friends ? this._outOfScreenPosition : this._desiredPosition);
			this.bestPlayersPanel.transform.localPosition = (value != MenuLeaderboardsView.State.BestPlayers ? this._outOfScreenPosition : this._desiredPosition);
			this.clansPanel.transform.localPosition = (value != MenuLeaderboardsView.State.Clans ? this._outOfScreenPosition : this._desiredPosition);
			this._currentState = value;
		}
	}

	public IList<LeaderboardItemViewModel> FriendsList
	{
		set
		{
			base.StartCoroutine(MenuLeaderboardsView.SetGrid(this.friendsGrid, value, this.temporaryBackground));
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

	public LeaderboardItemViewModel SelfClanStats
	{
		set
		{
			this.clanFooter.Reset(value);
			this.clanFooter.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	public LeaderboardItemViewModel SelfStats
	{
		set
		{
			this.footer.Reset(value);
			this.footer.gameObject.SetActive(value != LeaderboardItemViewModel.Empty);
		}
	}

	public MenuLeaderboardsView()
	{
	}

	private void Awake()
	{
		this.footer.gameObject.SetActive(false);
		this.clanFooter.gameObject.SetActive(false);
		if (this.bestPlayersDefaultSprite != null)
		{
			this.bestPlayersDefaultSprite.gameObject.SetActive(true);
		}
		if (this.clansDefaultSprite != null)
		{
			this.clansDefaultSprite.gameObject.SetActive(true);
		}
		this.temporaryBackground.gameObject.SetActive(false);
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (gameObject == this.friendsButton.gameObject)
		{
			this.CurrentState = MenuLeaderboardsView.State.Friends;
		}
		else if (gameObject == this.bestPlayersButton.gameObject)
		{
			this.CurrentState = MenuLeaderboardsView.State.BestPlayers;
		}
		else if (gameObject == this.clansButton.gameObject)
		{
			this.CurrentState = MenuLeaderboardsView.State.Clans;
		}
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.UpdateGridsAndScrollers());
	}

	[DebuggerHidden]
	private static IEnumerator SetGrid(UIGrid grid, IList<LeaderboardItemViewModel> value, UISprite temporaryBackground)
	{
		MenuLeaderboardsView.u003cSetGridu003ec__Iterator167 variable = null;
		return variable;
	}

	public void Show(bool needShow, bool animate)
	{
	}

	private void Start()
	{
		this._desiredPosition = this.friendsPanel.transform.localPosition;
		this._outOfScreenPosition = new Vector3(9000f, this._desiredPosition.y, this._desiredPosition.z);
		IEnumerable<UIButton> uIButtons = 
			from b in (IEnumerable<UIButton>)(new UIButton[] { this.friendsButton, this.bestPlayersButton, this.clansButton })
			where b != null
			select b;
		IEnumerator<UIButton> enumerator = uIButtons.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ButtonHandler component = enumerator.Current.GetComponent<ButtonHandler>();
				if (component == null)
				{
					continue;
				}
				component.Clicked += new EventHandler(this.HandleTabPressed);
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		IEnumerable<UIScrollView> uIScrollViews = 
			from s in (IEnumerable<UIScrollView>)(new UIScrollView[] { this.friendsScroll, this.bestPlayersScroll, this.clansScroll })
			where s != null
			select s;
		IEnumerator<UIScrollView> enumerator1 = uIScrollViews.GetEnumerator();
		try
		{
			while (enumerator1.MoveNext())
			{
				enumerator1.Current.ResetPosition();
			}
		}
		finally
		{
			if (enumerator1 == null)
			{
			}
			enumerator1.Dispose();
		}
		this.CurrentState = MenuLeaderboardsView.State.BestPlayers;
		this.Show(MenuLeaderboardsView.IsNeedShow, false);
		this.btnLeaderboards.IsChecked = false;
	}

	[DebuggerHidden]
	private IEnumerator UpdateGridsAndScrollers()
	{
		MenuLeaderboardsView.u003cUpdateGridsAndScrollersu003ec__Iterator168 variable = null;
		return variable;
	}

	public enum State
	{
		Friends,
		BestPlayers,
		Clans
	}
}