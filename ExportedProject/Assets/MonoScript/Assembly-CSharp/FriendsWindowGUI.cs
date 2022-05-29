using Rilisoft;
using System;
using UnityEngine;

public sealed class FriendsWindowGUI : MonoBehaviour
{
	public static FriendsWindowGUI Instance;

	public GameObject cameraObject;

	public Action OnExitCallback;

	public FriendsWindowController friendsWindow;

	private IDisposable _escapeSubscription;

	public bool InterfaceEnabled
	{
		get
		{
			return this.cameraObject.activeSelf;
		}
		private set
		{
			IDisposable disposable;
			this.cameraObject.SetActive(value);
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
			}
			if (!value)
			{
				disposable = null;
			}
			else
			{
				IDisposable disposable1 = BackSystem.Instance.Register(new Action(this.HandleEscape), "Friends");
				disposable = disposable1;
			}
			this._escapeSubscription = disposable;
		}
	}

	static FriendsWindowGUI()
	{
	}

	public FriendsWindowGUI()
	{
	}

	private void Awake()
	{
		FriendsWindowGUI.Instance = this;
		this.InterfaceEnabled = false;
	}

	private void HandleEscape()
	{
		this.HideInterface();
	}

	public void HideInterface()
	{
		if (this.OnExitCallback != null)
		{
			this.OnExitCallback();
		}
		ActivityIndicator.IsActiveIndicator = false;
		this.friendsWindow.SetCancelState();
		this.InterfaceEnabled = false;
	}

	private void OnDestroy()
	{
		FriendsWindowGUI.Instance = null;
	}

	public void ShowInterface(Action _exitCallback = null)
	{
		this.InterfaceEnabled = true;
		this.OnExitCallback = _exitCallback;
		this.friendsWindow.SetStartState();
	}
}