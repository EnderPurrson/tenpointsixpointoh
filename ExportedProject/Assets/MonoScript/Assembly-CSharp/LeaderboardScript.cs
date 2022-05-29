using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class LeaderboardScript : MonoBehaviour
{
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

	private LeaderboardScript.GridState _state;

	private Task<string> CurrentRequest
	{
		get
		{
			return LeaderboardScript._currentRequestPromise.Map<TaskCompletionSource<string>, Task<string>>((TaskCompletionSource<string> p) => p.Task);
		}
	}

	public UILabel ExpirationLabel
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.expirationLabel;
		}
	}

	public static LeaderboardScript Instance
	{
		get;
		private set;
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

	private LeaderboardsView LeaderboardView
	{
		get
		{
			return this._view.Value;
		}
	}

	public UIPanel Panel
	{
		get
		{
			UIPanel component;
			if (this._panelVal == null)
			{
				if (this._view == null || !this._view.ObjectIsLoaded)
				{
					component = null;
				}
				else
				{
					component = this._view.Value.gameObject.GetComponent<UIPanel>();
				}
				this._panelVal = component;
			}
			return this._panelVal;
		}
	}

	private GameObject TableFooterClan
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.tableFooterClan;
		}
	}

	private GameObject TableFooterIndividual
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.tableFooterIndividual;
		}
	}

	private GameObject TopClansGrid
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.clansGrid.gameObject;
		}
	}

	private GameObject TopFriendsGrid
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.friendsGrid.gameObject;
		}
	}

	private GameObject TopLeagueGrid
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.leagueGrid.gameObject;
		}
	}

	private GameObject TopPlayersGrid
	{
		get
		{
			if (this.LeaderboardView == null)
			{
				return null;
			}
			return this.LeaderboardView.bestPlayersGrid.gameObject;
		}
	}

	public bool UIEnabled
	{
		get
		{
			return (this._view == null ? false : this._view.ObjectIsActive);
		}
	}

	public LeaderboardScript()
	{
	}

	private void AddCacheInProfileInfo(List<LeaderboardItemViewModel> _list)
	{
		foreach (LeaderboardItemViewModel leaderboardItemViewModel in _list)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "nick", leaderboardItemViewModel.Nickname },
				{ "rank", leaderboardItemViewModel.Rank },
				{ "clan_name", leaderboardItemViewModel.ClanName },
				{ "clan_logo", leaderboardItemViewModel.ClanLogo }
			};
			Dictionary<string, object> strs1 = new Dictionary<string, object>()
			{
				{ "player", strs }
			};
			if (FriendsController.sharedController.profileInfo.ContainsKey(leaderboardItemViewModel.Id))
			{
				FriendsController.sharedController.profileInfo[leaderboardItemViewModel.Id] = strs1;
			}
			else
			{
				FriendsController.sharedController.profileInfo.Add(leaderboardItemViewModel.Id, strs1);
			}
		}
	}

	private void Awake()
	{
		LeaderboardScript.Instance = this;
		this._view = new LazyObject<LeaderboardsView>(this._viewPrefab.ResourcePath, this._viewHandler);
		this._mainMenuController = new Lazy<MainMenuController>(() => MainMenuController.sharedController);
	}

	private void FillClanItem(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state, int index, GameObject newItem)
	{
		if (index >= list.Count)
		{
			return;
		}
		LeaderboardItemViewModel item = list[index];
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(item);
		}
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] transformArrays = new Transform[] { ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => l.gameObject.name.Equals("LabelsFirstPlace")).Map<UILabel, Transform>((UILabel l) => l.transform), ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => l.gameObject.name.Equals("LabelsSecondPlace")).Map<UILabel, Transform>((UILabel l) => l.transform), ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => l.gameObject.name.Equals("LabelsThirdPlace")).Map<UILabel, Transform>((UILabel l) => l.transform) };
		for (int i = 0; i != (int)transformArrays.Length; i++)
		{
			transformArrays[i].Do<Transform>((Transform l) => l.gameObject.SetActive(i + 1 == item.Place));
		}
		newItem.transform.FindChild("LabelsPlace").Map<Transform, UILabel>((Transform t) => t.gameObject.GetComponent<UILabel>()).Do<UILabel>((UILabel p) => p.text = (item.Place <= 3 ? string.Empty : item.Place.ToString(CultureInfo.InvariantCulture)));
	}

	[DebuggerHidden]
	private IEnumerator FillClansGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		LeaderboardScript.u003cFillClansGridu003ec__Iterator158 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator FillColleaguesGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		LeaderboardScript.u003cFillColleaguesGridu003ec__Iterator15A variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator FillFriendsGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		LeaderboardScript.u003cFillFriendsGridu003ec__Iterator15B variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator FillGrids(string response, string playerId, LeaderboardScript.GridState state)
	{
		LeaderboardScript.u003cFillGridsu003ec__Iterator157 variable = null;
		return variable;
	}

	private void FillIndividualItem(GameObject grid, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state, int index, GameObject newItem)
	{
		if (index >= list.Count)
		{
			return;
		}
		if (index < 0)
		{
			string str = string.Format("Unexpected index {0} in list of {1} leaderboard items.", index, list.Count);
			UnityEngine.Debug.LogError(str);
			return;
		}
		LeaderboardItemViewModel item = list[index];
		LeaderboardItemView component = newItem.GetComponent<LeaderboardItemView>();
		if (component != null)
		{
			component.NewReset(item);
		}
		component.Clicked += new EventHandler<ClickedEventArgs>((object sender, ClickedEventArgs e) => {
			LeaderboardScript.PlayerClicked.Do<EventHandler<ClickedEventArgs>>((EventHandler<ClickedEventArgs> handler) => handler(this, e));
			if (Application.isEditor && Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log(string.Format("Clicked: {0}", e.Id));
			}
		});
		UILabel[] componentsInChildren = newItem.GetComponentsInChildren<UILabel>(true);
		Transform[] transformArrays = new Transform[] { ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => l.gameObject.name.Equals("LabelsFirstPlace")).Map<UILabel, Transform>((UILabel l) => l.transform), ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => l.gameObject.name.Equals("LabelsSecondPlace")).Map<UILabel, Transform>((UILabel l) => l.transform), ((IEnumerable<UILabel>)componentsInChildren).FirstOrDefault<UILabel>((UILabel l) => l.gameObject.name.Equals("LabelsThirdPlace")).Map<UILabel, Transform>((UILabel l) => l.transform) };
		for (int i = 0; i != (int)transformArrays.Length; i++)
		{
			transformArrays[i].Do<Transform>((Transform l) => l.gameObject.SetActive((i + 1 != item.Place ? false : item.WinCount > 0)));
		}
		newItem.transform.FindChild("LabelsPlace").Map<Transform, UILabel>((Transform t) => t.gameObject.GetComponent<UILabel>()).Do<UILabel>((UILabel p) => p.text = (item.Place <= 3 ? string.Empty : item.Place.ToString(CultureInfo.InvariantCulture)));
	}

	[DebuggerHidden]
	private IEnumerator FillPlayersGrid(GameObject gridGo, List<LeaderboardItemViewModel> list, LeaderboardScript.GridState state)
	{
		LeaderboardScript.u003cFillPlayersGridu003ec__Iterator159 variable = null;
		return variable;
	}

	private static string FormatExpirationLabel(float expirationTimespanSeconds)
	{
		string str;
		if (expirationTimespanSeconds < 0f)
		{
			throw new ArgumentOutOfRangeException("expirationTimespanSeconds");
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)expirationTimespanSeconds);
		try
		{
			string str1 = string.Format(LocalizationStore.Get("Key_1478"), Convert.ToInt32(Math.Floor(timeSpan.TotalDays)), timeSpan.Hours, timeSpan.Minutes);
			str = str1;
		}
		catch
		{
			if (timeSpan.TotalHours < 1)
			{
				string str2 = string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
				str = str2;
			}
			else if (timeSpan.TotalDays >= 1)
			{
				str = string.Format("{0}d {1}:{2:00}:{3:00}", new object[] { Convert.ToInt32(Math.Floor(timeSpan.TotalDays)), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds });
			}
			else
			{
				string str3 = string.Format("{0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				str = str3;
			}
		}
		return str;
	}

	public static int GetLeagueId()
	{
		return (int)RatingSystem.instance.currentLeague;
	}

	public Task GetReturnFuture()
	{
		if (this._returnPromise.Task.IsCompleted)
		{
			this._returnPromise = new TaskCompletionSource<bool>();
		}
		this._mainMenuController.Value.Do<MainMenuController>((MainMenuController m) => m.BackPressed -= new EventHandler(this.ReturnBack));
		this._mainMenuController.Value.Do<MainMenuController>((MainMenuController m) => m.BackPressed += new EventHandler(this.ReturnBack));
		return this._returnPromise.Task;
	}

	private static List<LeaderboardItemViewModel> GroupAndOrder(List<LeaderboardItemViewModel> items)
	{
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
				IOrderedEnumerable<LeaderboardItemViewModel> rank = 
					from vm in current
					orderby vm.Rank descending
					select vm;
				IEnumerator<LeaderboardItemViewModel> enumerator1 = rank.GetEnumerator();
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
	}

	private void HandlePlayerClicked(object sender, ClickedEventArgs e)
	{
		if (this.Panel == null)
		{
			UnityEngine.Debug.LogError("Leaderboards panel not found.");
			return;
		}
		this.Panel.alpha = 1E-45f;
		this.Panel.gameObject.SetActive(false);
		Action<bool> action = (bool needUpdateFriendList) => {
			this.Panel.gameObject.SetActive(true);
			this.TopFriendsGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TopPlayersGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TopClansGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TopLeagueGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
				w.SortBasedOnScrollMovement();
				w.WrapContent();
			});
			this.TopFriendsGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.TopPlayersGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.TopClansGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.TopLeagueGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
				s.ResetPosition();
				s.UpdatePosition();
			});
			this.Panel.alpha = 1f;
			this._profileIsOpened = false;
		};
		this._profileIsOpened = true;
		FriendsController.ShowProfile(e.Id, ProfileWindowType.other, action);
	}

	[DebuggerHidden]
	private static IEnumerator LoadLeaderboardsCoroutine(string playerId, TaskCompletionSource<string> requestPromise)
	{
		LeaderboardScript.u003cLoadLeaderboardsCoroutineu003ec__Iterator15D variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		if (LeaderboardScript._currentRequestPromise != null)
		{
			LeaderboardScript._currentRequestPromise.TrySetCanceled();
		}
		LeaderboardScript._currentRequestPromise = null;
		LeaderboardScript.PlayerClicked = null;
		FriendsController.DisposeProfile();
		this._mainMenuController.Value.Do<MainMenuController>((MainMenuController m) => m.BackPressed -= new EventHandler(this.ReturnBack));
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLocs));
	}

	internal void RefreshMyLeaderboardEntries()
	{
		foreach (LeaderboardItemViewModel playerNameOrDefault in this._friendsList)
		{
			if (playerNameOrDefault == null || !(playerNameOrDefault.Id == FriendsController.sharedController.id))
			{
				continue;
			}
			playerNameOrDefault.Nickname = ProfileController.GetPlayerNameOrDefault();
			playerNameOrDefault.ClanName = FriendsController.sharedController.clanName ?? string.Empty;
			break;
		}
		UILabel uILabel = this.TableFooterIndividual.transform.FindChild("LabelNick").Map<Transform, UILabel>((Transform t) => t.gameObject.GetComponent<UILabel>());
		if (uILabel != null)
		{
			uILabel.text = ProfileController.GetPlayerNameOrDefault();
		}
		UILabel empty = this.TableFooterIndividual.transform.FindChild("LabelClan").Map<Transform, UILabel>((Transform t) => t.gameObject.GetComponent<UILabel>());
		if (empty != null)
		{
			empty.text = FriendsController.sharedController.clanName ?? string.Empty;
		}
	}

	internal static void RequestLeaderboards(string playerId)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			UnityEngine.Debug.LogError("Friends controller is null.");
			return;
		}
		if (LeaderboardScript._currentRequestPromise != null)
		{
			LeaderboardScript._currentRequestPromise.TrySetCanceled();
		}
		LeaderboardScript._currentRequestPromise = new TaskCompletionSource<string>();
		FriendsController.sharedController.StartCoroutine(LeaderboardScript.LoadLeaderboardsCoroutine(playerId, LeaderboardScript._currentRequestPromise));
	}

	private void ReturnBack(object sender, EventArgs e)
	{
		if (this._profileIsOpened)
		{
			return;
		}
		this.TopFriendsGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TopPlayersGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TopClansGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TopLeagueGrid.Map<GameObject, UIWrapContent>((GameObject go) => go.GetComponent<UIWrapContent>()).Do<UIWrapContent>((UIWrapContent w) => {
			w.SortBasedOnScrollMovement();
			w.WrapContent();
		});
		this.TopFriendsGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
			s.ResetPosition();
			s.UpdatePosition();
		});
		this.TopPlayersGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
			s.ResetPosition();
			s.UpdatePosition();
		});
		this.TopClansGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
			s.ResetPosition();
			s.UpdatePosition();
		});
		this.TopLeagueGrid.Map<GameObject, UIScrollView>((GameObject go) => go.GetComponentInParent<UIScrollView>()).Do<UIScrollView>((UIScrollView s) => {
			s.ResetPosition();
			s.UpdatePosition();
		});
		this._returnPromise.TrySetResult(true);
		this._mainMenuController.Value.Do<MainMenuController>((MainMenuController m) => m.BackPressed -= new EventHandler(this.ReturnBack));
	}

	internal static void SetClanLogo(UITexture s, Texture2D clanLogoTexture)
	{
		Texture texture;
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture texture1 = s.mainTexture;
		UITexture uITexture = s;
		if (clanLogoTexture == null)
		{
			texture = null;
		}
		else
		{
			texture = UnityEngine.Object.Instantiate<Texture2D>(clanLogoTexture);
		}
		uITexture.mainTexture = texture;
		texture1.Do<Texture>(new Action<Texture>(UnityEngine.Object.Destroy));
	}

	internal static void SetClanLogo(UITexture s, string clanLogo)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		Texture texture = s.mainTexture;
		if (!string.IsNullOrEmpty(clanLogo))
		{
			s.mainTexture = LeaderboardItemViewModel.CreateLogoFromBase64String(clanLogo);
		}
		else
		{
			s.mainTexture = null;
		}
		texture.Do<Texture>(new Action<Texture>(UnityEngine.Object.Destroy));
	}

	public void Show()
	{
		base.StartCoroutine(this.ShowCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator ShowCoroutine()
	{
		LeaderboardScript.u003cShowCoroutineu003ec__Iterator15C variable = null;
		return variable;
	}

	private void Update()
	{
		if (!this._isInit)
		{
			return;
		}
		if (Time.realtimeSinceStartup > this._expirationNextUpateTimeSeconds)
		{
			this._expirationNextUpateTimeSeconds = Time.realtimeSinceStartup + 5f;
			if (this.ExpirationLabel != null)
			{
				if (this._state != LeaderboardScript.GridState.Empty)
				{
					float single = this._expirationTimeSeconds - Time.realtimeSinceStartup;
					if (single > 0f)
					{
						this.ExpirationLabel.text = LeaderboardScript.FormatExpirationLabel(single);
					}
					else
					{
						this.ExpirationLabel.text = string.Empty;
					}
				}
				else
				{
					this.ExpirationLabel.text = LocalizationStore.Key_0348;
				}
			}
		}
	}

	private void UpdateLocs()
	{
		if (this.TableFooterIndividual != null)
		{
			this.TableFooterIndividual.transform.FindChild("LabelPlace").Map<Transform, UILabel>((Transform t) => t.gameObject.GetComponent<UILabel>()).Do<UILabel>((UILabel n) => n.text = LocalizationStore.Get("Key_0053"));
		}
		if (this.TableFooterClan != null)
		{
			this.TableFooterClan.transform.FindChild("LabelPlace").Map<Transform, UILabel>((Transform t) => t.gameObject.GetComponent<UILabel>()).Do<UILabel>((UILabel n) => n.text = LocalizationStore.Get("Key_0053"));
		}
	}

	public static event EventHandler<ClickedEventArgs> PlayerClicked;

	private enum GridState
	{
		Empty,
		FillingWithCache,
		Cache,
		FillingWithResponse,
		Response
	}
}