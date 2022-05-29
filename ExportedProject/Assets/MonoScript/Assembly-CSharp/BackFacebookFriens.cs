using Rilisoft;
using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class BackFacebookFriens : MonoBehaviour
{
	private IDisposable _backSubscription;

	public BackFacebookFriens()
	{
	}

	private void OnClick()
	{
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(true);
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().facebookFriensPanel.gameObject.SetActive(false);
		FriendsController.sharedController.facebookFriendsInfo.Clear();
		FacebookFriendsGUIController.sharedController._infoRequested = false;
		ButtonClickSound.Instance.PlayClick();
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Facebook Friends");
	}
}