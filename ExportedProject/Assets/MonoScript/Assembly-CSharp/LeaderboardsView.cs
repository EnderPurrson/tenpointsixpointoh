using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class LeaderboardsView : MonoBehaviour
{
	internal const string LeaderboardsTabCache = "Leaderboards.TabCache";

	public UIWrapContent clansGrid;

	public UIWrapContent friendsGrid;

	public UIWrapContent bestPlayersGrid;

	public UIWrapContent leagueGrid;

	public ButtonHandler backButton;

	public UIButton clansButton;

	public UIButton friendsButton;

	public UIButton bestPlayersButton;

	[SerializeField]
	private UIButton leagueButton;

	public UIDragScrollView clansPanel;

	public UIDragScrollView friendsPanel;

	public UIDragScrollView bestPlayersPanel;

	[SerializeField]
	private UIDragScrollView leaguePanel;

	public UIScrollView clansScroll;

	public UIScrollView friendsScroll;

	public UIScrollView bestPlayersScroll;

	[SerializeField]
	private UIScrollView LeagueScroll;

	public GameObject defaultTableHeader;

	public GameObject clansTableHeader;

	public GameObject tableFooterClan;

	public GameObject tableFooterIndividual;

	public UILabel expirationLabel;

	private bool _overallTopFooterActive;

	private bool _leagueTopFooterActive;

	private readonly Lazy<UIPanel> _leaderboardsPanel;

	private bool _prepared;

	private LeaderboardsView.State _currentState;

	private EventHandler BackPressed;

	public LeaderboardsView.State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			bool flag;
			if (this._currentState == value)
			{
				return;
			}
			PlayerPrefs.SetInt("Leaderboards.TabCache", (int)value);
			if (this.clansButton != null)
			{
				this.clansButton.isEnabled = value != LeaderboardsView.State.Clans;
				Transform transforms = this.clansButton.gameObject.transform.FindChild("SpriteLabel");
				if (transforms != null)
				{
					transforms.gameObject.SetActive(value != LeaderboardsView.State.Clans);
				}
				Transform transforms1 = this.clansButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transforms1 != null)
				{
					transforms1.gameObject.SetActive(value == LeaderboardsView.State.Clans);
				}
			}
			if (this.friendsButton != null)
			{
				this.friendsButton.isEnabled = value != LeaderboardsView.State.Friends;
				Transform transforms2 = this.friendsButton.gameObject.transform.FindChild("SpriteLabel");
				if (transforms2 != null)
				{
					transforms2.gameObject.SetActive(value != LeaderboardsView.State.Friends);
				}
				Transform transforms3 = this.friendsButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transforms3 != null)
				{
					transforms3.gameObject.SetActive(value == LeaderboardsView.State.Friends);
				}
			}
			if (this.bestPlayersButton != null)
			{
				this.bestPlayersButton.isEnabled = value != LeaderboardsView.State.BestPlayers;
				Transform transforms4 = this.bestPlayersButton.gameObject.transform.FindChild("SpriteLabel");
				if (transforms4 != null)
				{
					transforms4.gameObject.SetActive(value != LeaderboardsView.State.BestPlayers);
				}
				Transform transforms5 = this.bestPlayersButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transforms5 != null)
				{
					transforms5.gameObject.SetActive(value == LeaderboardsView.State.BestPlayers);
				}
			}
			if (this.leagueButton != null)
			{
				this.leagueButton.isEnabled = value != LeaderboardsView.State.League;
				Transform transforms6 = this.leagueButton.gameObject.transform.FindChild("SpriteLabel");
				if (transforms6 != null)
				{
					transforms6.gameObject.SetActive(value != LeaderboardsView.State.League);
				}
				Transform transforms7 = this.leagueButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transforms7 != null)
				{
					transforms7.gameObject.SetActive(value == LeaderboardsView.State.League);
				}
			}
			if (this.defaultTableHeader != null)
			{
				this.defaultTableHeader.SetActive(value != LeaderboardsView.State.Clans);
				if (this.tableFooterIndividual != null)
				{
					GameObject gameObject = this.tableFooterIndividual;
					if (value != LeaderboardsView.State.BestPlayers || this._overallTopFooterActive)
					{
						flag = (value != LeaderboardsView.State.League ? false : !this._leagueTopFooterActive);
					}
					else
					{
						flag = true;
					}
					gameObject.SetActive(flag);
				}
			}
			if (this.clansTableHeader != null)
			{
				this.clansTableHeader.SetActive(value == LeaderboardsView.State.Clans);
				string str = FriendsController.sharedController.Map<FriendsController, string>((FriendsController c) => c.ClanID);
				this.tableFooterClan.Do<GameObject>((GameObject f) => f.SetActive(!string.IsNullOrEmpty(str)));
			}
			this.bestPlayersGrid.Do<UIWrapContent>((UIWrapContent g) => {
				Vector3 vector3 = g.transform.localPosition;
				vector3.x = (value != LeaderboardsView.State.BestPlayers ? 9000f : 0f);
				g.gameObject.transform.localPosition = vector3;
				!g.gameObject.activeInHierarchy;
			});
			this.friendsGrid.Do<UIWrapContent>((UIWrapContent g) => {
				Vector3 vector3 = g.transform.localPosition;
				vector3.x = (value != LeaderboardsView.State.Friends ? 9000f : 0f);
				g.gameObject.transform.localPosition = vector3;
				!g.gameObject.activeInHierarchy;
			});
			this.clansGrid.Do<UIWrapContent>((UIWrapContent g) => {
				Vector3 vector3 = g.transform.localPosition;
				vector3.x = (value != LeaderboardsView.State.Clans ? 9000f : 0f);
				g.gameObject.transform.localPosition = vector3;
				!g.gameObject.activeInHierarchy;
			});
			if (this.leagueGrid != null)
			{
				UIWrapContent uIWrapContent = this.leagueGrid;
				Vector3 vector31 = uIWrapContent.transform.localPosition;
				vector31.x = (value != LeaderboardsView.State.League ? 9000f : 0f);
				uIWrapContent.gameObject.transform.localPosition = vector31;
			}
			this._currentState = value;
		}
	}

	internal bool Prepared
	{
		get
		{
			return this._prepared;
		}
	}

	public LeaderboardsView()
	{
		this._leaderboardsPanel = new Lazy<UIPanel>(new Func<UIPanel>(this.GetComponent<UIPanel>));
	}

	private void Awake()
	{
		List<UIWrapContent> list = (
			from g in (IEnumerable<UIWrapContent>)(new UIWrapContent[] { this.friendsGrid })
			where g != null
			select g).ToList<UIWrapContent>();
		foreach (UIWrapContent uIWrapContent in list)
		{
			uIWrapContent.gameObject.SetActive(true);
			Vector3 vector3 = uIWrapContent.transform.localPosition;
			vector3.x = 9000f;
			uIWrapContent.gameObject.transform.localPosition = vector3;
		}
		List<UIWrapContent> uIWrapContents = (
			from g in (IEnumerable<UIWrapContent>)(new UIWrapContent[] { this.bestPlayersGrid, this.clansGrid, this.leagueGrid })
			where g != null
			select g).ToList<UIWrapContent>();
		foreach (UIWrapContent uIWrapContent1 in uIWrapContents)
		{
			uIWrapContent1.gameObject.SetActive(true);
			Vector3 vector31 = uIWrapContent1.transform.localPosition;
			vector31.x = 9000f;
			uIWrapContent1.gameObject.transform.localPosition = vector31;
		}
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (this.clansButton != null && gameObject == this.clansButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.Clans;
		}
		else if (this.friendsButton != null && gameObject == this.friendsButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.Friends;
		}
		else if (this.bestPlayersButton != null && gameObject == this.bestPlayersButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.BestPlayers;
		}
		else if (this.leagueButton != null && gameObject == this.leagueButton.gameObject)
		{
			this.CurrentState = LeaderboardsView.State.League;
		}
	}

	private void OnDestroy()
	{
		if (this.backButton != null)
		{
			this.backButton.Clicked -= new EventHandler(this.RaiseBackPressed);
		}
	}

	private void OnDisable()
	{
		this._prepared = false;
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.UpdateGridsAndScrollers());
	}

	private void RaiseBackPressed(object sender, EventArgs e)
	{
		EventHandler backPressed = this.BackPressed;
		if (backPressed != null)
		{
			backPressed(sender, e);
		}
	}

	private void RefreshGrid(UIGrid grid)
	{
		grid.Reposition();
	}

	[DebuggerHidden]
	private static IEnumerator SetGrid(UIGrid grid, IList<LeaderboardItemViewModel> value, string itemPrefabPath)
	{
		LeaderboardsView.u003cSetGridu003ec__Iterator161 variable = null;
		return variable;
	}

	public void SetLeagueTopFooterActive()
	{
		this._leagueTopFooterActive = true;
	}

	public void SetOverallTopFooterActive()
	{
		this._overallTopFooterActive = true;
	}

	[DebuggerHidden]
	private IEnumerator SkipFrameAndExecuteCoroutine(Action a)
	{
		LeaderboardsView.u003cSkipFrameAndExecuteCoroutineu003ec__Iterator160 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		LeaderboardsView.u003cStartu003ec__Iterator163 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator UpdateGridsAndScrollers()
	{
		LeaderboardsView.u003cUpdateGridsAndScrollersu003ec__Iterator162 variable = null;
		return variable;
	}

	public event EventHandler BackPressed
	{
		[MethodImpl(MethodImplOptions.Synchronized)]
		add
		{
			this.BackPressed += value;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		remove
		{
			this.BackPressed -= value;
		}
	}

	public enum State
	{
		None = 0,
		Clans = 1,
		Friends = 2,
		BestPlayers = 3,
		Default = 3,
		League = 4
	}
}