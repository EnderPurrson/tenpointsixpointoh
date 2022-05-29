using Rilisoft;
using System;
using UnityEngine;

internal sealed class BestPlayersPresser : MonoBehaviour
{
	public BestPlayersPresser()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		FriendsGUIController component = NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>();
		component.friendsPanel.gameObject.SetActive(false);
		component.leaderboardsView.gameObject.SetActive(true);
		component.RequestLeaderboards();
	}

	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}
}