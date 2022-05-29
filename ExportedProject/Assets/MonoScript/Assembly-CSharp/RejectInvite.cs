using Rilisoft;
using System;
using UnityEngine;

internal sealed class RejectInvite : MonoBehaviour
{
	public RejectInvite()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		Invitation component = base.transform.parent.GetComponent<Invitation>();
		if (component == null)
		{
			Debug.LogWarning("invitation == null");
		}
		else
		{
			FriendsController.sharedController.RejectInvite(component.id, null);
			base.transform.parent.GetComponent<Invitation>().DisableButtons();
		}
	}
}