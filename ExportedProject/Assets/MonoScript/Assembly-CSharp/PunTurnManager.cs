using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PunTurnManager : PunBehaviour
{
	public const byte TurnManagerEventOffset = 0;

	public const byte EvMove = 1;

	public const byte EvFinalMove = 2;

	public float TurnDuration = 20f;

	public IPunTurnManagerCallbacks TurnManagerListener;

	private readonly HashSet<PhotonPlayer> finishedPlayers = new HashSet<PhotonPlayer>();

	private bool _isOverCallProcessed;

	public float ElapsedTimeInTurn
	{
		get
		{
			return (float)(PhotonNetwork.ServerTimestamp - PhotonNetwork.room.GetTurnStart()) / 1000f;
		}
	}

	public bool IsCompletedByAll
	{
		get
		{
			return (PhotonNetwork.room == null || this.Turn <= 0 ? false : this.finishedPlayers.Count == PhotonNetwork.room.playerCount);
		}
	}

	public bool IsFinishedByMe
	{
		get
		{
			return this.finishedPlayers.Contains(PhotonNetwork.player);
		}
	}

	public bool IsOver
	{
		get
		{
			return this.RemainingSecondsInTurn <= 0f;
		}
	}

	public float RemainingSecondsInTurn
	{
		get
		{
			return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn);
		}
	}

	public int Turn
	{
		get
		{
			return PhotonNetwork.room.GetTurn();
		}
		private set
		{
			this._isOverCallProcessed = false;
			PhotonNetwork.room.SetTurn(value, true);
		}
	}

	public PunTurnManager()
	{
	}

	public void BeginTurn()
	{
		this.Turn = this.Turn + 1;
	}

	public bool GetPlayerFinishedTurn(PhotonPlayer player)
	{
		if (player != null && this.finishedPlayers != null && this.finishedPlayers.Contains(player))
		{
			return true;
		}
		return false;
	}

	public void OnEvent(byte eventCode, object content, int senderId)
	{
		PhotonPlayer photonPlayer = PhotonPlayer.Find(senderId);
		byte num = eventCode;
		if (num == 1)
		{
			Hashtable hashtable = content as Hashtable;
			int item = (int)hashtable["turn"];
			object obj = hashtable["move"];
			this.TurnManagerListener.OnPlayerMove(photonPlayer, item, obj);
		}
		else if (num == 2)
		{
			Hashtable hashtable1 = content as Hashtable;
			int item1 = (int)hashtable1["turn"];
			object obj1 = hashtable1["move"];
			if (item1 == this.Turn)
			{
				this.finishedPlayers.Add(photonPlayer);
				this.TurnManagerListener.OnPlayerFinished(photonPlayer, item1, obj1);
			}
			if (this.IsCompletedByAll)
			{
				this.TurnManagerListener.OnTurnCompleted(this.Turn);
			}
		}
	}

	public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("Turn"))
		{
			this._isOverCallProcessed = false;
			this.finishedPlayers.Clear();
			this.TurnManagerListener.OnTurnBegins(this.Turn);
		}
	}

	public void SendMove(object move, bool finished)
	{
		if (this.IsFinishedByMe)
		{
			Debug.LogWarning("Can't SendMove. Turn is finished by this player.");
			return;
		}
		Hashtable hashtable = new Hashtable()
		{
			{ "turn", this.Turn },
			{ "move", move }
		};
		byte num = (byte)((!finished ? 1 : 2));
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.AddToRoomCache
		};
		PhotonNetwork.RaiseEvent(num, hashtable, true, raiseEventOption);
		if (finished)
		{
			PhotonNetwork.player.SetFinishedTurn(this.Turn);
		}
		this.OnEvent(num, hashtable, PhotonNetwork.player.ID);
	}

	private void Start()
	{
		PhotonNetwork.OnEventCall = new PhotonNetwork.EventCallback(this.OnEvent);
	}

	private void Update()
	{
		if (this.Turn > 0 && this.IsOver && !this._isOverCallProcessed)
		{
			this._isOverCallProcessed = true;
			this.TurnManagerListener.OnTurnTimeEnds(this.Turn);
		}
	}
}