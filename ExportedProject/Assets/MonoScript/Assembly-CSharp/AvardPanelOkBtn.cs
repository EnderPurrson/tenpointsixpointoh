using Rilisoft;
using System;
using UnityEngine;

internal sealed class AvardPanelOkBtn : MonoBehaviour
{
	private IDisposable _backSubscription;

	public AvardPanelOkBtn()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideAvardPanel();
		}
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
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnClick), "Award Panel");
	}
}