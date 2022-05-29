using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public sealed class AdvertisementController : MonoBehaviour
	{
		public bool updateBanner;

		public bool updateFromMultiBanner;

		private Texture2D _advertisementTexture;

		private AdvertisementController.State _currentState;

		private WWW _checkingRequest;

		private float _disabledTimeStamp;

		private WWW _imageRequest;

		private readonly HashSet<AdvertisementController.State> _permittedStatesForRun;

		public Texture2D AdvertisementTexture
		{
			get
			{
				return this._advertisementTexture;
			}
		}

		public AdvertisementController.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		public AdvertisementController()
		{
			HashSet<AdvertisementController.State> states = new HashSet<AdvertisementController.State>();
			states.Add(AdvertisementController.State.Idle);
			states.Add(AdvertisementController.State.Disabled);
			states.Add(AdvertisementController.State.Closed);
			this._permittedStatesForRun = states;
			base();
		}

		public void Close()
		{
			if (this._currentState != AdvertisementController.State.Complete)
			{
				Debug.LogError(string.Concat("AdvertisementController cannot be started in ", this._currentState, " state."));
				return;
			}
			this._advertisementTexture = null;
			if (this._imageRequest != null)
			{
				this._imageRequest.Dispose();
				this._imageRequest = null;
			}
			this._currentState = AdvertisementController.State.Closed;
		}

		private void OnDestroy()
		{
			if (this._checkingRequest != null)
			{
				this._checkingRequest.Dispose();
			}
			if (this._imageRequest != null)
			{
				this._imageRequest.Dispose();
			}
		}

		public void Run()
		{
			if (!this._permittedStatesForRun.Contains(this._currentState))
			{
				Debug.LogError(string.Concat("AdvertisementController cannot be started in ", this._currentState, " state."));
				return;
			}
			if (Debug.isDebugBuild)
			{
				Debug.Log("Start checking advertisement.");
			}
			if (this._imageRequest != null)
			{
				this._imageRequest.Dispose();
				this._imageRequest = null;
			}
			if (this._checkingRequest != null)
			{
				this._checkingRequest.Dispose();
			}
			if (string.IsNullOrEmpty(PromoActionsManager.Advert.imageUrl) || !PromoActionsManager.Advert.enabled)
			{
				return;
			}
			if (this.updateBanner || this.updateFromMultiBanner)
			{
				this._advertisementTexture = Resources.Load<Texture2D>("update_available");
				this._currentState = AdvertisementController.State.Complete;
			}
			else
			{
				if (Application.isEditor && FriendsController.isDebugLogWWW)
				{
					Debug.Log(string.Format("<color=yellow><size=14>{0}</size></color>", "WWW PromoActionsManager.Advert.imageUrl"));
				}
				this._checkingRequest = Tools.CreateWwwIfNotConnected(PromoActionsManager.Advert.imageUrl);
				this._currentState = AdvertisementController.State.Checking;
			}
		}

		private void Update()
		{
			switch (this._currentState)
			{
				case AdvertisementController.State.Idle:
				{
					break;
				}
				case AdvertisementController.State.Checking:
				{
					if (this._checkingRequest == null)
					{
						Debug.LogError("Checking request is null.");
						this._currentState = AdvertisementController.State.Idle;
					}
					else if (!string.IsNullOrEmpty(this._checkingRequest.error))
					{
						Debug.LogWarning(this._checkingRequest.error);
						this._checkingRequest.Dispose();
						this._checkingRequest = null;
						this._disabledTimeStamp = Time.time;
						this._currentState = AdvertisementController.State.Disabled;
					}
					else if (this._checkingRequest.isDone)
					{
						if (Debug.isDebugBuild)
						{
							Debug.Log("Complete checking advertisement.");
						}
						this._advertisementTexture = null;
						this._imageRequest = this._checkingRequest;
						this._checkingRequest = null;
						this._currentState = AdvertisementController.State.Downloading;
					}
					break;
				}
				case AdvertisementController.State.Disabled:
				{
					if (Time.time - this._disabledTimeStamp > 300f)
					{
						this._disabledTimeStamp = 0f;
						this.Run();
					}
					break;
				}
				case AdvertisementController.State.Downloading:
				{
					if (this._imageRequest == null)
					{
						Debug.LogError("Image request is null.");
						this._currentState = AdvertisementController.State.Idle;
					}
					else if (!string.IsNullOrEmpty(this._imageRequest.error))
					{
						Debug.LogWarning(this._imageRequest.error);
						this._currentState = AdvertisementController.State.Error;
					}
					else if (this._imageRequest.isDone)
					{
						if (Debug.isDebugBuild)
						{
							Debug.Log("Complete downloading advertisement.");
						}
						this._advertisementTexture = this._imageRequest.texture;
						this._currentState = AdvertisementController.State.Complete;
					}
					break;
				}
				case AdvertisementController.State.Error:
				{
					break;
				}
				case AdvertisementController.State.Complete:
				{
					break;
				}
				case AdvertisementController.State.Closed:
				{
					break;
				}
				default:
				{
					Debug.LogError("Unknown state.");
					break;
				}
			}
		}

		public enum State
		{
			Idle,
			Checking,
			Disabled,
			Downloading,
			Error,
			Complete,
			Closed
		}
	}
}