using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class ClanIncomingInviteView : MonoBehaviour
{
	public UIButton acceptButton;

	public UIButton rejectButton;

	public UITexture clanLogo;

	public UILabel clanName;

	private string _clanName = string.Empty;

	private string _clanLogo = string.Empty;

	public string ClanId { get; set; }

	public string ClanCreatorId { get; set; }

	public string ClanName
	{
		get
		{
			return _clanName ?? string.Empty;
		}
		set
		{
			_clanName = value ?? string.Empty;
			clanName.Do(_003Cset_ClanName_003Em__269);
		}
	}

	public string ClanLogo
	{
		get
		{
			return _clanLogo ?? string.Empty;
		}
		set
		{
			_clanLogo = value ?? string.Empty;
			clanLogo.Do(_003Cset_ClanLogo_003Em__26A);
		}
	}

	public void HandleAcceptButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Accept invite to clan {0} ({1})", ClanName, ClanId);
			Debug.Log(message);
		}
		FriendsController.sharedController.Do(_003CHandleAcceptButton_003Em__26B);
		ClanIncomingInvitesController.SetRequestDirty();
	}

	public void HandleRejectButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Reject invite to clan {0} ({1})", ClanName, ClanId);
			Debug.Log(message);
		}
		FriendsController.sharedController.Do(_003CHandleRejectButton_003Em__26C);
		ClanIncomingInvitesController.SetRequestDirty();
	}

	private void Start()
	{
		Refresh();
	}

	private void OnEnable()
	{
		Refresh();
	}

	internal void Refresh()
	{
		if (acceptButton != null && rejectButton != null && FriendsController.sharedController != null)
		{
			bool flag = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			Vector3[] array = (flag ? new Vector3[2]
			{
				rejectButton.transform.localPosition,
				acceptButton.transform.localPosition
			} : new Vector3[2]
			{
				acceptButton.transform.localPosition,
				rejectButton.transform.localPosition
			});
			rejectButton.transform.localPosition = array[0];
			acceptButton.transform.localPosition = array[1];
			acceptButton.gameObject.SetActive(flag);
		}
	}

	private IEnumerator AcceptClanInviteCoroutine()
	{
		if (FriendsController.sharedController == null)
		{
			yield break;
		}
		string playerId = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(playerId))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "accept_invite");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerId);
		form.AddField("id_clan", ClanId ?? string.Empty);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("accept_invite"));
		UIButton o = acceptButton;
		if (_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheC == null)
		{
			_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheC = _003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Em__26D;
		}
		o.Do(_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheC);
		UIButton o2 = rejectButton;
		if (_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheD == null)
		{
			_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheD = _003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Em__26E;
		}
		o2.Do(_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheD);
		try
		{
			WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty);
			if (request == null)
			{
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogError(request.error);
				yield break;
			}
			string responseText = URLs.Sanitize(request);
			if ("fail".Equals(responseText, StringComparison.OrdinalIgnoreCase))
			{
				Debug.LogError("accept_invite failed.");
				yield break;
			}
			FriendsController.sharedController.clanLogo = ClanLogo;
			FriendsController.sharedController.ClanID = ClanId ?? string.Empty;
			FriendsController.sharedController.clanName = ClanName;
			FriendsController.sharedController.clanLeaderID = ClanCreatorId ?? string.Empty;
			if (ClansGUIController.sharedController != null)
			{
				ClansGUIController.sharedController.nameClanLabel.text = FriendsController.sharedController.clanName;
			}
			UIGrid g = base.transform.parent.GetComponent<UIGrid>();
			if (g != null)
			{
				NGUITools.Destroy(base.transform);
				yield return new WaitForEndOfFrame();
				g.Reposition();
				acceptButton = null;
				rejectButton = null;
				ClanIncomingInvitesController componentInParent = g.GetComponentInParent<ClanIncomingInvitesController>();
				if (_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheE == null)
				{
					_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheE = _003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Em__26F;
				}
				componentInParent.Do(_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheE);
			}
			ClanIncomingInviteView[] views = g.GetComponentsInChildren<ClanIncomingInviteView>();
			ClanIncomingInviteView[] array = views;
			foreach (ClanIncomingInviteView view in array)
			{
				view.Refresh();
			}
		}
		finally
		{
			UIButton o3 = acceptButton;
			if (_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheF == null)
			{
				_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheF = _003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Em__270;
			}
			o3.Do(_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cacheF);
			UIButton o4 = rejectButton;
			if (_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cache10 == null)
			{
				_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cache10 = _003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Em__271;
			}
			o4.Do(_003CAcceptClanInviteCoroutine_003Ec__Iterator128._003C_003Ef__am_0024cache10);
		}
	}

	private IEnumerator RejectClanInviteCoroutine()
	{
		FriendsController sharedController = FriendsController.sharedController;
		if (_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cache8 == null)
		{
			_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cache8 = _003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Em__272;
		}
		string playerId = sharedController.Map(_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cache8) ?? string.Empty;
		if (string.IsNullOrEmpty(playerId))
		{
			yield break;
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "reject_invite");
		form.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		form.AddField("id_player", playerId);
		form.AddField("id_clan", ClanId ?? string.Empty);
		form.AddField("id", playerId);
		form.AddField("uniq_id", FriendsController.sharedController.id);
		form.AddField("auth", FriendsController.Hash("reject_invite"));
		UIButton o = acceptButton;
		if (_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cache9 == null)
		{
			_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cache9 = _003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Em__273;
		}
		o.Do(_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cache9);
		UIButton o2 = rejectButton;
		if (_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheA == null)
		{
			_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheA = _003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Em__274;
		}
		o2.Do(_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheA);
		try
		{
			WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty);
			if (request == null)
			{
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				Debug.LogError(request.error);
				yield break;
			}
			string responseText = URLs.Sanitize(request);
			if ("fail".Equals(responseText, StringComparison.OrdinalIgnoreCase))
			{
				Debug.LogError("reject_invite failed.");
				yield break;
			}
			UIGrid g = base.transform.parent.GetComponent<UIGrid>();
			if (g != null)
			{
				NGUITools.Destroy(base.transform);
				yield return new WaitForEndOfFrame();
				g.Reposition();
				acceptButton = null;
				rejectButton = null;
				ClanIncomingInvitesController componentInParent = g.GetComponentInParent<ClanIncomingInvitesController>();
				if (_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheB == null)
				{
					_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheB = _003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Em__275;
				}
				componentInParent.Do(_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheB);
			}
		}
		finally
		{
			UIButton o3 = acceptButton;
			if (_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheC == null)
			{
				_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheC = _003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Em__276;
			}
			o3.Do(_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheC);
			UIButton o4 = rejectButton;
			if (_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheD == null)
			{
				_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheD = _003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Em__277;
			}
			o4.Do(_003CRejectClanInviteCoroutine_003Ec__Iterator129._003C_003Ef__am_0024cacheD);
		}
	}

	[CompilerGenerated]
	private void _003Cset_ClanName_003Em__269(UILabel l)
	{
		l.text = _clanName;
	}

	[CompilerGenerated]
	private void _003Cset_ClanLogo_003Em__26A(UITexture t)
	{
		LeaderboardScript.SetClanLogo(t, _clanLogo);
	}

	[CompilerGenerated]
	private void _003CHandleAcceptButton_003Em__26B(FriendsController f)
	{
		f.StartCoroutine(AcceptClanInviteCoroutine());
	}

	[CompilerGenerated]
	private void _003CHandleRejectButton_003Em__26C(FriendsController f)
	{
		f.StartCoroutine(RejectClanInviteCoroutine());
	}
}
