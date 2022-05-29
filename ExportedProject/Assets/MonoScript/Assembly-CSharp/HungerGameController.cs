using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class HungerGameController : Photon.MonoBehaviour
{
	public bool isStartGame;

	public bool isStartTimer;

	public float startTimer = 30f;

	public int countPlayers;

	public int maxCountPlayers = 10;

	public bool isRunPlayer;

	public float goTimer = 10.5f;

	public bool isGo;

	private float timeToSynchTimer = 2f;

	public int minCountPlayer = 2;

	public bool isShowGo;

	private float timerShowGo = 1f;

	public float gameTimer = 600f;

	public bool theEnd;

	private static HungerGameController _instance;

	public static HungerGameController Instance
	{
		get
		{
			return HungerGameController._instance;
		}
	}

	public HungerGameController()
	{
	}

	[PunRPC]
	[RPC]
	private void Draw()
	{
		Debug.Log("Draw!!!");
		NetworkStartTable networkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		if (networkStartTable != null)
		{
			base.StartCoroutine(networkStartTable.DrawInHanger());
		}
	}

	[PunRPC]
	[RPC]
	private void Go()
	{
		this.isGo = true;
		this.isShowGo = true;
	}

	private void OnDestroy()
	{
		HungerGameController._instance = null;
	}

	private void Start()
	{
		this.maxCountPlayers = PhotonNetwork.room.maxPlayers;
		this.gameTimer = (float)(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString()) * 60);
		HungerGameController._instance = this;
	}

	[PunRPC]
	[RPC]
	private void StartGame()
	{
		this.isStartGame = true;
	}

	[PunRPC]
	[RPC]
	private void StartTimer(bool _isStartTimer)
	{
		this.isStartTimer = _isStartTimer;
	}

	[PunRPC]
	[RPC]
	private void SynchGameTimer(float _gameTimer)
	{
		this.gameTimer = _gameTimer;
	}

	[PunRPC]
	[RPC]
	private void SynchStartTimer(float _startTimer)
	{
		this.startTimer = _startTimer;
	}

	[PunRPC]
	[RPC]
	private void SynchTimerGo(float _goTimer)
	{
		this.goTimer = _goTimer;
	}

	private void Update()
	{
		if (this.isStartTimer && this.startTimer > 0f)
		{
			this.startTimer -= Time.deltaTime;
		}
		if (this.isStartGame && this.goTimer > 0f)
		{
			this.goTimer -= Time.deltaTime;
		}
		if (this.goTimer < 0f)
		{
			this.goTimer = 0f;
		}
		if (this.isShowGo && this.timerShowGo >= 0f)
		{
			this.timerShowGo -= Time.deltaTime;
		}
		if (this.isShowGo && this.timerShowGo < 0f)
		{
			this.isShowGo = false;
		}
		if (this.isGo && this.gameTimer > 0f && Initializer.players.Count > 0)
		{
			this.gameTimer -= Time.deltaTime;
		}
		if (base.photonView.isMine)
		{
			if (this.gameTimer <= 0f && !this.theEnd)
			{
				this.theEnd = true;
				base.photonView.RPC("Draw", PhotonTargets.AllBuffered, new object[0]);
			}
			this.timeToSynchTimer -= Time.deltaTime;
			if (this.isGo && this.timeToSynchTimer < 0f)
			{
				this.timeToSynchTimer = 0.5f;
				base.photonView.RPC("SynchGameTimer", PhotonTargets.Others, new object[] { this.gameTimer });
			}
			GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("NetworkTable");
			if (this.isStartGame)
			{
				if (this.timeToSynchTimer < 0f)
				{
					this.timeToSynchTimer = 0.5f;
					base.photonView.RPC("SynchTimerGo", PhotonTargets.Others, new object[] { this.goTimer });
				}
				if (!this.isGo && this.goTimer < 0.1f)
				{
					base.photonView.RPC("Go", PhotonTargets.AllBuffered, new object[0]);
				}
			}
			else
			{
				if (!this.isStartTimer && (int)gameObjectArray.Length >= this.minCountPlayer)
				{
					base.photonView.RPC("StartTimer", PhotonTargets.AllBuffered, new object[] { true });
				}
				if (this.timeToSynchTimer < 0f)
				{
					this.timeToSynchTimer = 0.5f;
					base.photonView.RPC("SynchStartTimer", PhotonTargets.Others, new object[] { this.startTimer });
				}
				if (!this.isStartGame && this.isStartTimer && this.startTimer < 0.1f && (int)gameObjectArray.Length >= this.minCountPlayer || !this.isStartGame && this.isStartTimer && (int)gameObjectArray.Length == PhotonNetwork.room.maxPlayers)
				{
					base.photonView.RPC("StartGame", PhotonTargets.AllBuffered, new object[0]);
					PhotonNetwork.room.visible = false;
				}
			}
		}
	}
}