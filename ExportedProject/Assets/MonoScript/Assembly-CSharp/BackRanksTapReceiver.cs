using Rilisoft;
using System;
using UnityEngine;

public sealed class BackRanksTapReceiver : MonoBehaviour
{
	public NetworkStartTableNGUIController networkStartTableNGUIController;

	private IDisposable _backSubscription;

	public BackRanksTapReceiver()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		this.networkStartTableNGUIController.BackPressFromRanksTable(true);
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
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Back Ranks");
	}
}