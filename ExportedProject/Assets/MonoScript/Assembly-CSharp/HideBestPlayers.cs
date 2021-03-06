using System;
using UnityEngine;

internal sealed class HideBestPlayers : MonoBehaviour
{
	public HideBestPlayers()
	{
	}

	private void OnClick()
	{
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().leaderboardsView.gameObject.SetActive(false);
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(true);
		ButtonClickSound.Instance.PlayClick();
	}
}