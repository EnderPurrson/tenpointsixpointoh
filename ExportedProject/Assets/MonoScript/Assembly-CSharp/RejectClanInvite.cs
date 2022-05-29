using Rilisoft;
using System;
using UnityEngine;

public class RejectClanInvite : MonoBehaviour
{
	public RejectClanInvite()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		FriendsController.sharedController.RejectClanInvite(base.transform.parent.GetComponent<Invitation>().recordId, null);
		base.transform.parent.GetComponent<Invitation>().DisableButtons();
	}
}