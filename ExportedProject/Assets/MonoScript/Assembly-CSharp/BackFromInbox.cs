using System;
using UnityEngine;

public class BackFromInbox : MonoBehaviour
{
	public BackFromInbox()
	{
	}

	private void OnClick()
	{
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(true);
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().inboxPanel.gameObject.SetActive(false);
		ButtonClickSound.Instance.PlayClick();
	}
}