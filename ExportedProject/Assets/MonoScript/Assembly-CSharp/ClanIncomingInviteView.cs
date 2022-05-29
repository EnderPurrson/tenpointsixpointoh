using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class ClanIncomingInviteView : MonoBehaviour
{
	public UIButton acceptButton;

	public UIButton rejectButton;

	public UITexture clanLogo;

	public UILabel clanName;

	private string _clanName = string.Empty;

	private string _clanLogo = string.Empty;

	public string ClanCreatorId
	{
		get;
		set;
	}

	public string ClanId
	{
		get;
		set;
	}

	public string ClanLogo
	{
		get
		{
			return this._clanLogo ?? string.Empty;
		}
		set
		{
			this._clanLogo = value ?? string.Empty;
			this.clanLogo.Do<UITexture>((UITexture t) => LeaderboardScript.SetClanLogo(t, this._clanLogo));
		}
	}

	public string ClanName
	{
		get
		{
			return this._clanName ?? string.Empty;
		}
		set
		{
			this._clanName = value ?? string.Empty;
			this.clanName.Do<UILabel>((UILabel l) => l.text = this._clanName);
		}
	}

	public ClanIncomingInviteView()
	{
	}

	[DebuggerHidden]
	private IEnumerator AcceptClanInviteCoroutine()
	{
		ClanIncomingInviteView.u003cAcceptClanInviteCoroutineu003ec__Iterator128 variable = null;
		return variable;
	}

	public void HandleAcceptButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			string str = string.Format("Accept invite to clan {0} ({1})", this.ClanName, this.ClanId);
			UnityEngine.Debug.Log(str);
		}
		FriendsController.sharedController.Do<FriendsController>((FriendsController f) => f.StartCoroutine(this.AcceptClanInviteCoroutine()));
		ClanIncomingInvitesController.SetRequestDirty();
	}

	public void HandleRejectButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			string str = string.Format("Reject invite to clan {0} ({1})", this.ClanName, this.ClanId);
			UnityEngine.Debug.Log(str);
		}
		FriendsController.sharedController.Do<FriendsController>((FriendsController f) => f.StartCoroutine(this.RejectClanInviteCoroutine()));
		ClanIncomingInvitesController.SetRequestDirty();
	}

	private void OnEnable()
	{
		this.Refresh();
	}

	internal void Refresh()
	{
		if (this.acceptButton != null && this.rejectButton != null && FriendsController.sharedController != null)
		{
			bool flag = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			Vector3[] vector3Array = (!flag ? new Vector3[] { this.acceptButton.transform.localPosition, this.rejectButton.transform.localPosition } : new Vector3[] { this.rejectButton.transform.localPosition, this.acceptButton.transform.localPosition });
			this.rejectButton.transform.localPosition = vector3Array[0];
			this.acceptButton.transform.localPosition = vector3Array[1];
			this.acceptButton.gameObject.SetActive(flag);
		}
	}

	[DebuggerHidden]
	private IEnumerator RejectClanInviteCoroutine()
	{
		ClanIncomingInviteView.u003cRejectClanInviteCoroutineu003ec__Iterator129 variable = null;
		return variable;
	}

	private void Start()
	{
		this.Refresh();
	}
}