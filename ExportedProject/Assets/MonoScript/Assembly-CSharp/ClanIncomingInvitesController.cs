using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class ClanIncomingInvitesController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHandleInboxPressed_003Ec__AnonStorey29E
	{
		private sealed class _003CHandleInboxPressed_003Ec__AnonStorey29D
		{
			internal bool inClan;

			internal _003CHandleInboxPressed_003Ec__AnonStorey29E _003C_003Ef__ref_0024670;

			internal void _003C_003Em__282(GameObject i)
			{
				i.SetActive(inClan);
			}

			internal void _003C_003Em__283(GameObject i)
			{
				i.SetActive(!inClan);
			}

			internal void _003C_003Em__284(ClansGUIController c)
			{
				c.CurrentState = _003C_003Ef__ref_0024670.previousState;
			}
		}

		internal ClansGUIController.State previousState;

		internal ClanIncomingInvitesController _003C_003Ef__this;

		private static Func<FriendsController, string> _003C_003Ef__am_0024cache2;

		private static Action<GameObject> _003C_003Ef__am_0024cache3;

		internal void _003C_003Em__27D()
		{
			_003CHandleInboxPressed_003Ec__AnonStorey29D _003CHandleInboxPressed_003Ec__AnonStorey29D = new _003CHandleInboxPressed_003Ec__AnonStorey29D();
			_003CHandleInboxPressed_003Ec__AnonStorey29D._003C_003Ef__ref_0024670 = this;
			FriendsController sharedController = FriendsController.sharedController;
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003C_003Em__280;
			}
			_003CHandleInboxPressed_003Ec__AnonStorey29D.inClan = !string.IsNullOrEmpty(sharedController.Map(_003C_003Ef__am_0024cache2));
			GameObject inboxPanel = _003C_003Ef__this.inboxPanel;
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003C_003Em__281;
			}
			inboxPanel.Do(_003C_003Ef__am_0024cache3);
			_003C_003Ef__this.clanPanel.Do(_003CHandleInboxPressed_003Ec__AnonStorey29D._003C_003Em__282);
			_003C_003Ef__this.noClanPanel.Do(_003CHandleInboxPressed_003Ec__AnonStorey29D._003C_003Em__283);
			_003C_003Ef__this.clansGui.Do(_003CHandleInboxPressed_003Ec__AnonStorey29D._003C_003Em__284);
		}

		private static string _003C_003Em__280(FriendsController f)
		{
			return f.ClanID;
		}

		private static void _003C_003Em__281(GameObject i)
		{
			i.SetActive(false);
		}
	}

	public ClansGUIController clansGui;

	public UIGrid clanIncomingInvitesGrid;

	public ClanIncomingInviteView clanIncomingInviteViewPrototype;

	public GameObject clanPanel;

	public GameObject noClanPanel;

	public GameObject inboxPanel;

	public GameObject noClanIncomingInvitesLabel;

	public GameObject cannotAcceptClanIncomingInvitesLabel;

	private Action _back;

	private static Task<List<object>> _currentRequest;

	private static CancellationTokenSource _cts;

	[CompilerGenerated]
	private static Func<ClansGUIController, ClansGUIController.State> _003C_003Ef__am_0024cacheB;

	[CompilerGenerated]
	private static Action<GameObject> _003C_003Ef__am_0024cacheC;

	[CompilerGenerated]
	private static Action<GameObject> _003C_003Ef__am_0024cacheD;

	[CompilerGenerated]
	private static Action<GameObject> _003C_003Ef__am_0024cacheE;

	[CompilerGenerated]
	private static Action<ClansGUIController> _003C_003Ef__am_0024cacheF;

	[CompilerGenerated]
	private static Func<ClansGUIController, GameObject> _003C_003Ef__am_0024cache10;

	[CompilerGenerated]
	private static Action<CancellationTokenSource> _003C_003Ef__am_0024cache11;

	internal static Task<List<object>> CurrentRequest
	{
		get
		{
			return _currentRequest;
		}
	}

	public void HandleInboxPressed()
	{
		_003CHandleInboxPressed_003Ec__AnonStorey29E _003CHandleInboxPressed_003Ec__AnonStorey29E = new _003CHandleInboxPressed_003Ec__AnonStorey29E();
		_003CHandleInboxPressed_003Ec__AnonStorey29E._003C_003Ef__this = this;
		ClansGUIController o = clansGui;
		if (_003C_003Ef__am_0024cacheB == null)
		{
			_003C_003Ef__am_0024cacheB = _003CHandleInboxPressed_003Em__278;
		}
		_003CHandleInboxPressed_003Ec__AnonStorey29E.previousState = o.Map(_003C_003Ef__am_0024cacheB);
		GameObject o2 = inboxPanel;
		if (_003C_003Ef__am_0024cacheC == null)
		{
			_003C_003Ef__am_0024cacheC = _003CHandleInboxPressed_003Em__279;
		}
		o2.Do(_003C_003Ef__am_0024cacheC);
		GameObject o3 = clanPanel;
		if (_003C_003Ef__am_0024cacheD == null)
		{
			_003C_003Ef__am_0024cacheD = _003CHandleInboxPressed_003Em__27A;
		}
		o3.Do(_003C_003Ef__am_0024cacheD);
		GameObject o4 = noClanPanel;
		if (_003C_003Ef__am_0024cacheE == null)
		{
			_003C_003Ef__am_0024cacheE = _003CHandleInboxPressed_003Em__27B;
		}
		o4.Do(_003C_003Ef__am_0024cacheE);
		ClansGUIController o5 = clansGui;
		if (_003C_003Ef__am_0024cacheF == null)
		{
			_003C_003Ef__am_0024cacheF = _003CHandleInboxPressed_003Em__27C;
		}
		o5.Do(_003C_003Ef__am_0024cacheF);
		StartCoroutine(RepositionAfterPause());
		_back = _003CHandleInboxPressed_003Ec__AnonStorey29E._003C_003Em__27D;
	}

	public void HandleBackFromInboxPressed()
	{
		if (_back != null)
		{
			_back();
		}
	}

	internal static void FetchClanIncomingInvites(string playerId)
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
		_cts = new CancellationTokenSource();
		_currentRequest = RequestClanIncomingInvitesAsync(playerId, _cts.Token);
	}

	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId, float delay, CancellationToken ct)
	{
		if (string.IsNullOrEmpty(playerId))
		{
			throw new ArgumentException("Player id should not be empty", "playerId");
		}
		if (FriendsController.sharedController == null)
		{
			throw new InvalidOperationException("FriendsController instance should not be null.");
		}
		TaskCompletionSource<List<object>> taskCompletionSource = new TaskCompletionSource<List<object>>();
		FriendsController.sharedController.StartCoroutine(RequestClanIncomingInvitesCoroutine(playerId, delay, taskCompletionSource, ct));
		return taskCompletionSource.Task;
	}

	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId, CancellationToken ct)
	{
		return RequestClanIncomingInvitesAsync(playerId, 0f, ct);
	}

	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId)
	{
		return RequestClanIncomingInvitesAsync(playerId, 0f, CancellationToken.None);
	}

	internal void Refresh()
	{
		if (clanIncomingInvitesGrid != null && noClanIncomingInvitesLabel != null)
		{
			bool flag = clanIncomingInvitesGrid.transform.childCount == 0;
			ClansGUIController o = clansGui;
			if (_003C_003Ef__am_0024cache10 == null)
			{
				_003C_003Ef__am_0024cache10 = _003CRefresh_003Em__27E;
			}
			GameObject gameObject = o.Map(_003C_003Ef__am_0024cache10);
			bool active = ((gameObject == null) ? flag : (flag && !gameObject.activeInHierarchy));
			noClanIncomingInvitesLabel.gameObject.SetActive(active);
		}
		if (cannotAcceptClanIncomingInvitesLabel != null)
		{
			bool flag2 = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			bool active2 = ((noClanIncomingInvitesLabel == null) ? (!flag2) : (!flag2 && !noClanIncomingInvitesLabel.activeInHierarchy));
			cannotAcceptClanIncomingInvitesLabel.SetActive(active2);
		}
	}

	private static IEnumerator RequestClanIncomingInvitesCoroutine(string playerId, float delay, TaskCompletionSource<List<object>> promise, CancellationToken ct)
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		if (ct.IsCancellationRequested)
		{
			promise.TrySetCanceled();
			yield break;
		}
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			Debug.LogWarning("Current player id is empty.");
			promise.TrySetException(new InvalidOperationException("Current player id is empty."));
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "get_clan_incoming_invites");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("get_clan_incoming_invites"));
		WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty);
		if (request == null)
		{
			promise.TrySetException(new InvalidOperationException("Request was not performed because player is connected to Photon."));
			_currentRequest = RequestClanIncomingInvitesAsync(playerId, 10f, ct);
			yield break;
		}
		while (!request.isDone)
		{
			yield return null;
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogError(request.error);
			promise.TrySetException(new InvalidOperationException(request.error));
			_currentRequest = RequestClanIncomingInvitesAsync(playerId, 10f, ct);
			yield break;
		}
		string responseText = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("Clan incoming invites response is empty.");
			promise.TrySetException(new InvalidOperationException("Clan incoming invites response is empty."));
			_currentRequest = RequestClanIncomingInvitesAsync(playerId, 10f, ct);
			yield break;
		}
		Dictionary<string, object> d = Json.Deserialize(responseText) as Dictionary<string, object>;
		object invites;
		if (d != null && d.TryGetValue("clans_invites", out invites))
		{
			List<object> result = invites as List<object>;
			if (invites == null)
			{
				promise.TrySetException(new InvalidOperationException("“clans_invites” could not be parsed."));
			}
			else
			{
				promise.TrySetResult(result);
			}
		}
		else
		{
			promise.TrySetException(new InvalidOperationException("“clans_invites” not found."));
		}
	}

	internal static void SetRequestDirty()
	{
		if (_currentRequest == null)
		{
			return;
		}
		if (!_currentRequest.IsCompleted)
		{
			CancellationTokenSource cts = _cts;
			if (_003C_003Ef__am_0024cache11 == null)
			{
				_003C_003Ef__am_0024cache11 = _003CSetRequestDirty_003Em__27F;
			}
			cts.Do(_003C_003Ef__am_0024cache11);
		}
		_cts = new CancellationTokenSource();
		_currentRequest = null;
	}

	private IEnumerator Start()
	{
		Refresh();
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
		if (CurrentRequest == null)
		{
			_cts = new CancellationTokenSource();
			_currentRequest = RequestClanIncomingInvitesAsync(playerId, _cts.Token);
		}
		else if (CurrentRequest.IsCompleted && (CurrentRequest.IsCanceled || CurrentRequest.IsFaulted))
		{
			_cts = new CancellationTokenSource();
			_currentRequest = RequestClanIncomingInvitesAsync(playerId, _cts.Token);
		}
		if (!CurrentRequest.IsCompleted)
		{
		}
		while (!CurrentRequest.IsCompleted)
		{
			yield return null;
		}
		if (CurrentRequest.IsCanceled)
		{
			Debug.LogWarning("Request is cancelled.");
		}
		else if (CurrentRequest.IsFaulted)
		{
			Debug.LogException(CurrentRequest.Exception);
		}
		else if (clanIncomingInviteViewPrototype != null && clanIncomingInvitesGrid != null)
		{
			List<object> inviteList = CurrentRequest.Result;
			if (inviteList != null)
			{
				List<Dictionary<string, object>> invites = inviteList.OfType<Dictionary<string, object>>().ToList();
				clanIncomingInviteViewPrototype.gameObject.SetActive(invites.Count > 0);
				foreach (Dictionary<string, object> invite in invites)
				{
					GameObject newItem = NGUITools.AddChild(clanIncomingInvitesGrid.gameObject, clanIncomingInviteViewPrototype.gameObject);
					ClanIncomingInviteView c = newItem.GetComponent<ClanIncomingInviteView>();
					if (c != null)
					{
						object clanId;
						if (invite.TryGetValue("id", out clanId))
						{
							c.ClanId = Convert.ToString(clanId);
						}
						else
						{
							c.ClanId = string.Empty;
						}
						object clanName;
						if (invite.TryGetValue("name", out clanName))
						{
							c.ClanName = Convert.ToString(clanName);
						}
						else
						{
							c.ClanName = string.Empty;
						}
						object clanLogo;
						if (invite.TryGetValue("logo", out clanLogo))
						{
							c.ClanLogo = Convert.ToString(clanLogo);
						}
						else
						{
							c.ClanLogo = string.Empty;
						}
						object clanCreatorId;
						if (invite.TryGetValue("creator_id", out clanCreatorId))
						{
							c.ClanCreatorId = Convert.ToString(clanLogo);
						}
						else
						{
							c.ClanCreatorId = string.Empty;
						}
					}
				}
				UIScrollView component = clanIncomingInvitesGrid.transform.parent.GetComponent<UIScrollView>();
				if (_003CStart_003Ec__Iterator12B._003C_003Ef__am_0024cacheE == null)
				{
					_003CStart_003Ec__Iterator12B._003C_003Ef__am_0024cacheE = _003CStart_003Ec__Iterator12B._003C_003Em__285;
				}
				component.Do(_003CStart_003Ec__Iterator12B._003C_003Ef__am_0024cacheE);
			}
			clanIncomingInviteViewPrototype.gameObject.SetActive(false);
			yield return new WaitForEndOfFrame();
			clanIncomingInvitesGrid.Reposition();
		}
		Refresh();
	}

	private IEnumerator RepositionAfterPause()
	{
		yield return new WaitForEndOfFrame();
		UIGrid o = clanIncomingInvitesGrid;
		if (_003CRepositionAfterPause_003Ec__Iterator12C._003C_003Ef__am_0024cache3 == null)
		{
			_003CRepositionAfterPause_003Ec__Iterator12C._003C_003Ef__am_0024cache3 = _003CRepositionAfterPause_003Ec__Iterator12C._003C_003Em__286;
		}
		o.Do(_003CRepositionAfterPause_003Ec__Iterator12C._003C_003Ef__am_0024cache3);
		Refresh();
	}

	[CompilerGenerated]
	private static ClansGUIController.State _003CHandleInboxPressed_003Em__278(ClansGUIController c)
	{
		return c.CurrentState;
	}

	[CompilerGenerated]
	private static void _003CHandleInboxPressed_003Em__279(GameObject i)
	{
		i.SetActive(true);
	}

	[CompilerGenerated]
	private static void _003CHandleInboxPressed_003Em__27A(GameObject i)
	{
		i.SetActive(false);
	}

	[CompilerGenerated]
	private static void _003CHandleInboxPressed_003Em__27B(GameObject i)
	{
		i.SetActive(false);
	}

	[CompilerGenerated]
	private static void _003CHandleInboxPressed_003Em__27C(ClansGUIController c)
	{
		c.CurrentState = ClansGUIController.State.Inbox;
	}

	[CompilerGenerated]
	private static GameObject _003CRefresh_003Em__27E(ClansGUIController c)
	{
		return c.receivingPlashka;
	}

	[CompilerGenerated]
	private static void _003CSetRequestDirty_003Em__27F(CancellationTokenSource c)
	{
		c.Cancel();
	}
}
