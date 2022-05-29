using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class AddFrendsButtonInTableRangs : MonoBehaviour
{
	public int ID;

	public AddFrendsButtonInTableRangs()
	{
	}

	private void OnPress(bool isDown)
	{
		if (!isDown)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "Added Friends", "AddFrendsButtonInTableRangs" },
				{ "Deleted Friends", "Add" }
			};
			FriendsController.sharedController.SendInvitation(this.ID.ToString(), strs);
			if (!FriendsController.sharedController.notShowAddIds.Contains(this.ID.ToString()))
			{
				FriendsController.sharedController.notShowAddIds.Add(this.ID.ToString());
			}
			base.gameObject.SetActive(false);
		}
	}
}