using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class TestAddFriends : MonoBehaviour
{
	public TestAddFriends()
	{
	}

	private void OnClick()
	{
		Dictionary<string, object> strs = new Dictionary<string, object>()
		{
			{ "Added Friends", "Test" },
			{ "Deleted Friends", "Add" }
		};
		FriendsController.sharedController.SendInvitation("123", strs);
	}
}