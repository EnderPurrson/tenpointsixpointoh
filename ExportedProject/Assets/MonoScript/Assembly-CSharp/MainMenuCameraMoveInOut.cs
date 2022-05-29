using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
internal sealed class MainMenuCameraMoveInOut : MonoBehaviour
{
	private Vector3 _initialPosition;

	private Quaternion _initialRotation;

	private MainMenuCameraMoveInOut.State _currentState = new MainMenuCameraMoveInOut.IdleState();

	internal MainMenuCameraMoveInOut.State CurrentState
	{
		get
		{
			return this._currentState;
		}
	}

	public MainMenuCameraMoveInOut()
	{
	}

	private void Awake()
	{
		this._initialPosition = base.gameObject.transform.position;
		this._initialRotation = base.gameObject.transform.rotation;
	}

	public void HandleBackRequest()
	{
		if (!(this.CurrentState is MainMenuCameraMoveInOut.ActiveState))
		{
			Debug.LogWarning(string.Format("Ignoring click while in {0} state.", this._currentState));
		}
		else
		{
			this._currentState = new MainMenuCameraMoveInOut.TransitionState();
			this._currentState = new MainMenuCameraMoveInOut.IdleState();
		}
	}

	public void HandleClickTrigger()
	{
		if (!(this.CurrentState is MainMenuCameraMoveInOut.IdleState))
		{
			Debug.Log(string.Format("Ignoring click while in {0} state.", this._currentState));
		}
		else
		{
			this._currentState = new MainMenuCameraMoveInOut.TransitionState();
			this._currentState = new MainMenuCameraMoveInOut.ActiveState();
		}
	}

	public void Reset()
	{
		base.gameObject.transform.position = this._initialPosition;
		base.gameObject.transform.rotation = this._initialRotation;
		this._currentState = new MainMenuCameraMoveInOut.IdleState();
	}

	public sealed class ActiveState : MainMenuCameraMoveInOut.State
	{
		public ActiveState()
		{
		}
	}

	public sealed class IdleState : MainMenuCameraMoveInOut.State
	{
		public IdleState()
		{
		}
	}

	public abstract class State
	{
		protected State()
		{
		}
	}

	public sealed class TransitionState : MainMenuCameraMoveInOut.State
	{
		public TransitionState()
		{
		}
	}
}