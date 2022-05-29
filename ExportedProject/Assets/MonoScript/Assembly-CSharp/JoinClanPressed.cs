using Rilisoft;
using System;
using UnityEngine;

public sealed class JoinClanPressed : MonoBehaviour
{
	public JoinClanPressed()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		Invitation component = base.transform.parent.GetComponent<Invitation>();
		if (component == null)
		{
			return;
		}
		FriendsController.sharedController.AcceptClanInvite(component.recordId);
		component.DisableButtons();
		component.KeepClanData();
	}
}