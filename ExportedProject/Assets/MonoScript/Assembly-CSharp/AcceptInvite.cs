using Rilisoft;
using System;
using UnityEngine;

internal sealed class AcceptInvite : MonoBehaviour
{
	public AcceptInvite()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		Invitation component = base.transform.parent.GetComponent<Invitation>();
		if (component == null)
		{
			Debug.LogWarning("invitation == null");
			return;
		}
		FriendsController.sharedController.AcceptInvite(component.id, null);
		component.DisableButtons();
	}
}