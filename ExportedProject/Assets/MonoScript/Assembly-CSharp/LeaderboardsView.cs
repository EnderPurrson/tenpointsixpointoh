using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class LeaderboardsView : MonoBehaviour
{
	public enum State
	{
		None = 0,
		Clans = 1,
		Friends = 2,
		BestPlayers = 3,
		League = 4,
		Default = 3
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__AnonStorey2C1
	{
		internal State value;

		internal void _003C_003Em__357(UIWrapContent g)
		{
			Vector3 localPosition = g.transform.localPosition;
			localPosition.x = ((value != State.BestPlayers) ? 9000f : 0f);
			g.gameObject.transform.localPosition = localPosition;
			if (!g.gameObject.activeInHierarchy)
			{
			}
		}

		internal void _003C_003Em__358(UIWrapContent g)
		{
			Vector3 localPosition = g.transform.localPosition;
			localPosition.x = ((value != State.Friends) ? 9000f : 0f);
			g.gameObject.transform.localPosition = localPosition;
			if (!g.gameObject.activeInHierarchy)
			{
			}
		}

		internal void _003C_003Em__359(UIWrapContent g)
		{
			Vector3 localPosition = g.transform.localPosition;
			localPosition.x = ((value != State.Clans) ? 9000f : 0f);
			g.gameObject.transform.localPosition = localPosition;
			if (!g.gameObject.activeInHierarchy)
			{
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__AnonStorey2C0
	{
		internal string clanId;

		internal void _003C_003Em__356(GameObject f)
		{
			f.SetActive(!string.IsNullOrEmpty(clanId));
		}
	}

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

	private State _currentState;

	[CompilerGenerated]
	private static Func<FriendsController, string> _003C_003Ef__am_0024cache1C;

	[CompilerGenerated]
	private static Func<UIWrapContent, bool> _003C_003Ef__am_0024cache1D;

	[CompilerGenerated]
	private static Func<UIWrapContent, bool> _003C_003Ef__am_0024cache1E;

	public State CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			_003C_003Ec__AnonStorey2C1 _003C_003Ec__AnonStorey2C = new _003C_003Ec__AnonStorey2C1();
			_003C_003Ec__AnonStorey2C.value = value;
			if (_currentState == _003C_003Ec__AnonStorey2C.value)
			{
				return;
			}
			PlayerPrefs.SetInt("Leaderboards.TabCache", (int)_003C_003Ec__AnonStorey2C.value);
			if (clansButton != null)
			{
				clansButton.isEnabled = _003C_003Ec__AnonStorey2C.value != State.Clans;
				Transform transform = clansButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform != null)
				{
					transform.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value != State.Clans);
				}
				Transform transform2 = clansButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value == State.Clans);
				}
			}
			if (friendsButton != null)
			{
				friendsButton.isEnabled = _003C_003Ec__AnonStorey2C.value != State.Friends;
				Transform transform3 = friendsButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value != State.Friends);
				}
				Transform transform4 = friendsButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value == State.Friends);
				}
			}
			if (bestPlayersButton != null)
			{
				bestPlayersButton.isEnabled = _003C_003Ec__AnonStorey2C.value != State.BestPlayers;
				Transform transform5 = bestPlayersButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value != State.BestPlayers);
				}
				Transform transform6 = bestPlayersButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value == State.BestPlayers);
				}
			}
			if (leagueButton != null)
			{
				leagueButton.isEnabled = _003C_003Ec__AnonStorey2C.value != State.League;
				Transform transform7 = leagueButton.gameObject.transform.FindChild("SpriteLabel");
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value != State.League);
				}
				Transform transform8 = leagueButton.gameObject.transform.FindChild("ChekmarkLabel");
				if (transform8 != null)
				{
					transform8.gameObject.SetActive(_003C_003Ec__AnonStorey2C.value == State.League);
				}
			}
			if (defaultTableHeader != null)
			{
				defaultTableHeader.SetActive(_003C_003Ec__AnonStorey2C.value != State.Clans);
				if (tableFooterIndividual != null)
				{
					tableFooterIndividual.SetActive((_003C_003Ec__AnonStorey2C.value == State.BestPlayers && !_overallTopFooterActive) || (_003C_003Ec__AnonStorey2C.value == State.League && !_leagueTopFooterActive));
				}
			}
			if (clansTableHeader != null)
			{
				_003C_003Ec__AnonStorey2C0 _003C_003Ec__AnonStorey2C2 = new _003C_003Ec__AnonStorey2C0();
				clansTableHeader.SetActive(_003C_003Ec__AnonStorey2C.value == State.Clans);
				FriendsController sharedController = FriendsController.sharedController;
				if (_003C_003Ef__am_0024cache1C == null)
				{
					_003C_003Ef__am_0024cache1C = _003Cset_CurrentState_003Em__355;
				}
				_003C_003Ec__AnonStorey2C2.clanId = sharedController.Map(_003C_003Ef__am_0024cache1C);
				tableFooterClan.Do(_003C_003Ec__AnonStorey2C2._003C_003Em__356);
			}
			bestPlayersGrid.Do(_003C_003Ec__AnonStorey2C._003C_003Em__357);
			friendsGrid.Do(_003C_003Ec__AnonStorey2C._003C_003Em__358);
			clansGrid.Do(_003C_003Ec__AnonStorey2C._003C_003Em__359);
			if (leagueGrid != null)
			{
				UIWrapContent uIWrapContent = leagueGrid;
				Vector3 localPosition = uIWrapContent.transform.localPosition;
				localPosition.x = ((_003C_003Ec__AnonStorey2C.value != State.League) ? 9000f : 0f);
				uIWrapContent.gameObject.transform.localPosition = localPosition;
			}
			_currentState = _003C_003Ec__AnonStorey2C.value;
		}
	}

	internal bool Prepared
	{
		get
		{
			return _prepared;
		}
	}

	public event EventHandler BackPressed;

	public LeaderboardsView()
	{
		_leaderboardsPanel = new Lazy<UIPanel>(base.GetComponent<UIPanel>);
	}

	public void SetOverallTopFooterActive()
	{
		_overallTopFooterActive = true;
	}

	public void SetLeagueTopFooterActive()
	{
		_leagueTopFooterActive = true;
	}

	private void RefreshGrid(UIGrid grid)
	{
		grid.Reposition();
	}

	private IEnumerator SkipFrameAndExecuteCoroutine(Action a)
	{
		if (a != null)
		{
			yield return new WaitForEndOfFrame();
			a();
		}
	}

	private void HandleTabPressed(object sender, EventArgs e)
	{
		GameObject gameObject = ((ButtonHandler)sender).gameObject;
		if (clansButton != null && gameObject == clansButton.gameObject)
		{
			CurrentState = State.Clans;
		}
		else if (friendsButton != null && gameObject == friendsButton.gameObject)
		{
			CurrentState = State.Friends;
		}
		else if (bestPlayersButton != null && gameObject == bestPlayersButton.gameObject)
		{
			CurrentState = State.BestPlayers;
		}
		else if (leagueButton != null && gameObject == leagueButton.gameObject)
		{
			CurrentState = State.League;
		}
	}

	private void RaiseBackPressed(object sender, EventArgs e)
	{
		EventHandler backPressed = this.BackPressed;
		if (backPressed != null)
		{
			backPressed(sender, e);
		}
	}

	private static IEnumerator SetGrid(UIGrid grid, IList<LeaderboardItemViewModel> value, string itemPrefabPath)
	{
		if (string.IsNullOrEmpty(itemPrefabPath))
		{
			throw new ArgumentException("itemPrefabPath");
		}
		if (!(grid != null))
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
			if (_003CSetGrid_003Ec__Iterator161._003C_003Ef__am_0024cache10 == null)
			{
				_003CSetGrid_003Ec__Iterator161._003C_003Ef__am_0024cache10 = _003CSetGrid_003Ec__Iterator161._003C_003Em__35C;
			}
			enumerable2 = value.Where(_003CSetGrid_003Ec__Iterator161._003C_003Ef__am_0024cache10);
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
			GameObject o = UnityEngine.Object.Instantiate(Resources.Load(itemPrefabPath)) as GameObject;
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

	private IEnumerator UpdateGridsAndScrollers()
	{
		_prepared = false;
		yield return new WaitForEndOfFrame();
		UIWrapContent[] source = new UIWrapContent[4] { friendsGrid, bestPlayersGrid, clansGrid, leagueGrid };
		if (_003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Ef__am_0024cache9 == null)
		{
			_003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Ef__am_0024cache9 = _003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Em__35D;
		}
		IEnumerable<UIWrapContent> wraps = source.Where(_003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Ef__am_0024cache9);
		foreach (UIWrapContent w in wraps)
		{
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		}
		yield return null;
		UIScrollView[] source2 = new UIScrollView[4] { clansScroll, friendsScroll, bestPlayersScroll, LeagueScroll };
		if (_003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Ef__am_0024cacheA == null)
		{
			_003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Ef__am_0024cacheA = _003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Em__35E;
		}
		IEnumerable<UIScrollView> scrolls = source2.Where(_003CUpdateGridsAndScrollers_003Ec__Iterator162._003C_003Ef__am_0024cacheA);
		foreach (UIScrollView s in scrolls)
		{
			s.ResetPosition();
			s.UpdatePosition();
		}
		_prepared = true;
	}

	private void OnDestroy()
	{
		if (backButton != null)
		{
			backButton.Clicked -= RaiseBackPressed;
		}
	}

	private void OnEnable()
	{
		StartCoroutine(UpdateGridsAndScrollers());
	}

	private void OnDisable()
	{
		_prepared = false;
	}

	private void Awake()
	{
		UIWrapContent[] source = new UIWrapContent[1] { friendsGrid };
		if (_003C_003Ef__am_0024cache1D == null)
		{
			_003C_003Ef__am_0024cache1D = _003CAwake_003Em__35A;
		}
		List<UIWrapContent> list = source.Where(_003C_003Ef__am_0024cache1D).ToList();
		foreach (UIWrapContent item in list)
		{
			item.gameObject.SetActive(true);
			Vector3 localPosition = item.transform.localPosition;
			localPosition.x = 9000f;
			item.gameObject.transform.localPosition = localPosition;
		}
		UIWrapContent[] source2 = new UIWrapContent[3] { bestPlayersGrid, clansGrid, leagueGrid };
		if (_003C_003Ef__am_0024cache1E == null)
		{
			_003C_003Ef__am_0024cache1E = _003CAwake_003Em__35B;
		}
		List<UIWrapContent> list2 = source2.Where(_003C_003Ef__am_0024cache1E).ToList();
		foreach (UIWrapContent item2 in list2)
		{
			item2.gameObject.SetActive(true);
			Vector3 localPosition2 = item2.transform.localPosition;
			localPosition2.x = 9000f;
			item2.gameObject.transform.localPosition = localPosition2;
		}
	}

	private IEnumerator Start()
	{
		UIButton[] source = new UIButton[4] { clansButton, friendsButton, bestPlayersButton, leagueButton };
		if (_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheC == null)
		{
			_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheC = _003CStart_003Ec__Iterator163._003C_003Em__35F;
		}
		IEnumerable<UIButton> buttons = source.Where(_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheC);
		foreach (UIButton b in buttons)
		{
			ButtonHandler bh = b.GetComponent<ButtonHandler>();
			if (bh != null)
			{
				bh.Clicked += HandleTabPressed;
			}
		}
		if (backButton != null)
		{
			backButton.Clicked += RaiseBackPressed;
		}
		UIScrollView[] source2 = new UIScrollView[4] { clansScroll, friendsScroll, bestPlayersScroll, LeagueScroll };
		if (_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheD == null)
		{
			_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheD = _003CStart_003Ec__Iterator163._003C_003Em__360;
		}
		IEnumerable<UIScrollView> scrollViews = source2.Where(_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheD);
		foreach (UIScrollView scrollView in scrollViews)
		{
			scrollView.ResetPosition();
		}
		yield return null;
		UIWrapContent o = friendsGrid;
		if (_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheE == null)
		{
			_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheE = _003CStart_003Ec__Iterator163._003C_003Em__361;
		}
		o.Do(_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheE);
		UIWrapContent o2 = bestPlayersGrid;
		if (_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheF == null)
		{
			_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheF = _003CStart_003Ec__Iterator163._003C_003Em__362;
		}
		o2.Do(_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cacheF);
		UIWrapContent o3 = clansGrid;
		if (_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cache10 == null)
		{
			_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cache10 = _003CStart_003Ec__Iterator163._003C_003Em__363;
		}
		o3.Do(_003CStart_003Ec__Iterator163._003C_003Ef__am_0024cache10);
		if (leagueGrid != null)
		{
			leagueGrid.SortBasedOnScrollMovement();
			leagueGrid.WrapContent();
		}
		yield return null;
		int stateInt = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
		State state = ((!Enum.IsDefined(typeof(State), stateInt)) ? State.BestPlayers : ((State)stateInt));
		CurrentState = ((state == State.None) ? State.BestPlayers : state);
	}

	[CompilerGenerated]
	private static string _003Cset_CurrentState_003Em__355(FriendsController c)
	{
		return c.ClanID;
	}

	[CompilerGenerated]
	private static bool _003CAwake_003Em__35A(UIWrapContent g)
	{
		return g != null;
	}

	[CompilerGenerated]
	private static bool _003CAwake_003Em__35B(UIWrapContent g)
	{
		return g != null;
	}
}
