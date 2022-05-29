using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class ClanIncomingInvitesController : MonoBehaviour
{
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

	internal static Task<List<object>> CurrentRequest
	{
		get
		{
			return ClanIncomingInvitesController._currentRequest;
		}
	}

	public ClanIncomingInvitesController()
	{
	}

	internal static void FetchClanIncomingInvites(string playerId)
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
		ClanIncomingInvitesController._cts = new CancellationTokenSource();
		ClanIncomingInvitesController._currentRequest = ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, ClanIncomingInvitesController._cts.Token);
	}

	public void HandleBackFromInboxPressed()
	{
		if (this._back != null)
		{
			this._back();
		}
	}

	public void HandleInboxPressed()
	{
		Func<FriendsController, string> func = null;
		Action<GameObject> action = null;
		ClansGUIController.State state = this.clansGui.Map<ClansGUIController, ClansGUIController.State>((ClansGUIController c) => c.CurrentState);
		this.inboxPanel.Do<GameObject>((GameObject i) => i.SetActive(true));
		this.clanPanel.Do<GameObject>((GameObject i) => i.SetActive(false));
		this.noClanPanel.Do<GameObject>((GameObject i) => i.SetActive(false));
		this.clansGui.Do<ClansGUIController>((ClansGUIController c) => c.CurrentState = ClansGUIController.State.Inbox);
		base.StartCoroutine(this.RepositionAfterPause());
		this._back = () => {
			FriendsController friendsController = FriendsController.sharedController;
			if (func == null)
			{
				func = (FriendsController f) => f.ClanID;
			}
			bool flag = !string.IsNullOrEmpty(friendsController.Map<FriendsController, string>(func));
			GameObject u003cu003ef_this = this.inboxPanel;
			if (action == null)
			{
				action = (GameObject i) => i.SetActive(false);
			}
			u003cu003ef_this.Do<GameObject>(action);
			this.clanPanel.Do<GameObject>((GameObject i) => i.SetActive(flag));
			this.noClanPanel.Do<GameObject>((GameObject i) => i.SetActive(!flag));
			this.clansGui.Do<ClansGUIController>((ClansGUIController c) => c.CurrentState = state);
		};
	}

	internal void Refresh()
	{
		bool flag;
		bool flag1;
		if (this.clanIncomingInvitesGrid != null && this.noClanIncomingInvitesLabel != null)
		{
			bool flag2 = this.clanIncomingInvitesGrid.transform.childCount == 0;
			GameObject gameObject = this.clansGui.Map<ClansGUIController, GameObject>((ClansGUIController c) => c.receivingPlashka);
			if (gameObject != null)
			{
				flag1 = (!flag2 ? false : !gameObject.activeInHierarchy);
			}
			else
			{
				flag1 = flag2;
			}
			this.noClanIncomingInvitesLabel.gameObject.SetActive(flag1);
		}
		if (this.cannotAcceptClanIncomingInvitesLabel != null)
		{
			bool flag3 = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			if (this.noClanIncomingInvitesLabel != null)
			{
				flag = (flag3 ? false : !this.noClanIncomingInvitesLabel.activeInHierarchy);
			}
			else
			{
				flag = !flag3;
			}
			this.cannotAcceptClanIncomingInvitesLabel.SetActive(flag);
		}
	}

	[DebuggerHidden]
	private IEnumerator RepositionAfterPause()
	{
		ClanIncomingInvitesController.u003cRepositionAfterPauseu003ec__Iterator12C variable = null;
		return variable;
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
		FriendsController.sharedController.StartCoroutine(ClanIncomingInvitesController.RequestClanIncomingInvitesCoroutine(playerId, delay, taskCompletionSource, ct));
		return taskCompletionSource.Task;
	}

	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId, CancellationToken ct)
	{
		return ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 0f, ct);
	}

	internal static Task<List<object>> RequestClanIncomingInvitesAsync(string playerId)
	{
		return ClanIncomingInvitesController.RequestClanIncomingInvitesAsync(playerId, 0f, CancellationToken.None);
	}

	[DebuggerHidden]
	private static IEnumerator RequestClanIncomingInvitesCoroutine(string playerId, float delay, TaskCompletionSource<List<object>> promise, CancellationToken ct)
	{
		ClanIncomingInvitesController.u003cRequestClanIncomingInvitesCoroutineu003ec__Iterator12A variable = null;
		return variable;
	}

	internal static void SetRequestDirty()
	{
		if (ClanIncomingInvitesController._currentRequest == null)
		{
			return;
		}
		if (!ClanIncomingInvitesController._currentRequest.IsCompleted)
		{
			ClanIncomingInvitesController._cts.Do<CancellationTokenSource>((CancellationTokenSource c) => c.Cancel());
		}
		ClanIncomingInvitesController._cts = new CancellationTokenSource();
		ClanIncomingInvitesController._currentRequest = null;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		ClanIncomingInvitesController.u003cStartu003ec__Iterator12B variable = null;
		return variable;
	}
}