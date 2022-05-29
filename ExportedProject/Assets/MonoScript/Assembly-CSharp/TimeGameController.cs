using ExitGames.Client.Photon;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TimeGameController : MonoBehaviour
{
	public static TimeGameController sharedController;

	public double timeEndMatch;

	public double timerToEndMatch;

	public double networkTime;

	public PhotonView photonView;

	public double timeLocalServer;

	public string ipServera;

	private long pauseTime;

	private bool paused;

	private bool wasPaused;

	public bool isEndMatch = true;

	private bool matchEnding;

	private int matchEndingPos;

	private int writtedMatchEndingPos;

	public TimeGameController()
	{
	}

	private void Awake()
	{
		if (!Defs.isMulti || Defs.isHunger)
		{
			base.enabled = false;
			return;
		}
		base.StartCoroutine(this.FetchServerTimestamp());
	}

	private void CheckPause()
	{
		this.paused = false;
		long currentUnixTime = Tools.CurrentUnixTime;
		if (this.pauseTime > currentUnixTime || this.pauseTime + (long)60 < currentUnixTime)
		{
			PhotonNetwork.Disconnect();
		}
	}

	[DebuggerHidden]
	private IEnumerator FetchServerTimestamp()
	{
		return new TimeGameController.u003cFetchServerTimestampu003ec__Iterator1D6();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (PhotonNetwork.connected)
		{
			if (!pauseStatus)
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					this.CheckPause();
				}
				PhotonNetwork.FetchServerTimestamp();
			}
			else if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PhotonNetwork.Disconnect();
			}
			else
			{
				this.paused = true;
				this.wasPaused = true;
				PhotonNetwork.isMessageQueueRunning = false;
			}
		}
	}

	private void OnDestroy()
	{
		TimeGameController.sharedController = null;
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (Network.isServer)
		{
			base.GetComponent<NetworkView>().RPC("SynchTimeEnd", RPCMode.Others, new object[] { (float)this.timeEndMatch });
			base.GetComponent<NetworkView>().RPC("SynchTimeServer", RPCMode.Others, new object[] { (float)Network.time });
		}
	}

	[DebuggerHidden]
	private IEnumerator OnUnpause()
	{
		return new TimeGameController.u003cOnUnpauseu003ec__Iterator1D7();
	}

	[Obfuscation(Exclude=true)]
	public void SinchServerTimeInvoke()
	{
		base.GetComponent<NetworkView>().RPC("SynchTimeServer", RPCMode.Others, new object[] { (float)Network.time });
	}

	private void Start()
	{
		TimeGameController.sharedController = this;
		if (Defs.isMulti && !Defs.isInet && Network.isServer)
		{
			base.InvokeRepeating("SinchServerTimeInvoke", 0.1f, 2f);
			UnityEngine.Debug.Log("TimeGameController: Start synch server time");
		}
	}

	public void StartMatch()
	{
		bool flag = false;
		this.matchEnding = false;
		if (CapturePointController.sharedController != null)
		{
			CapturePointController.sharedController.isEndMatch = false;
		}
		if ((Defs.isCapturePoints || Defs.isFlag) && Convert.ToDouble(PhotonNetwork.room.customProperties["TimeMatchEnd"]) < -5000000)
		{
			flag = true;
		}
		if (Defs.isInet && (this.timeEndMatch < PhotonNetwork.time && !Defs.isFlag || Initializer.players.Count == 0 || Defs.isFlag && flag))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			double num = PhotonNetwork.time + (double)((!Defs.isCOOP ? (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty] : 4) * 60);
			if (num > 4294967 && PhotonNetwork.time < 4294967)
			{
				num = 4294967;
			}
			hashtable["TimeMatchEnd"] = num;
			hashtable[ConnectSceneNGUIController.endingProperty] = 0;
			PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
			this.matchEndingPos = 0;
			this.timerToEndMatch = num - PhotonNetwork.time;
		}
		if (!Defs.isInet && (this.timeEndMatch < this.networkTime || Initializer.players.Count == 0))
		{
			this.timeEndMatch = this.networkTime + (double)((PlayerPrefs.GetString("MaxKill", "9").Equals(string.Empty) ? 5 : int.Parse(PlayerPrefs.GetString("MaxKill", "5"))) * 60);
			base.GetComponent<NetworkView>().RPC("SynchTimeEnd", RPCMode.Others, new object[] { (float)this.timeEndMatch });
		}
	}

	[PunRPC]
	[RPC]
	private void SynchTimeEnd(float synchTime)
	{
		this.timeEndMatch = (double)synchTime;
	}

	[PunRPC]
	[RPC]
	private void SynchTimeServer(float synchTime)
	{
		if (this.networkTime < (double)synchTime)
		{
			this.networkTime = (double)synchTime;
		}
	}

	private void Update()
	{
		if (this.paused && Defs.isInet && Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.CheckPause();
			if (!PhotonNetwork.connected)
			{
				return;
			}
		}
		this.ipServera = PhotonNetwork.ServerAddress;
		if (Defs.isInet && PhotonNetwork.room != null && PhotonNetwork.room.customProperties["TimeMatchEnd"] != null)
		{
			double num = this.networkTime - PhotonNetwork.time;
			if (WeaponManager.sharedManager.myPlayerMoveC != null && num > 6 && num < 600)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[] { "Speedhack detected! Delta: ", num, ", Photon time: ", PhotonNetwork.time, ", Last time: ", this.networkTime }));
				PhotonNetwork.Disconnect();
			}
			this.networkTime = PhotonNetwork.time;
			if (this.networkTime < 0.1)
			{
				return;
			}
			this.timeEndMatch = Convert.ToDouble(PhotonNetwork.room.customProperties["TimeMatchEnd"]);
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null && this.timeEndMatch > PhotonNetwork.time + 1500)
			{
				Initializer.Instance.goToConnect();
			}
			if (PhotonNetwork.room.customProperties.ContainsKey(ConnectSceneNGUIController.endingProperty))
			{
				this.matchEndingPos = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.endingProperty];
			}
			this.writtedMatchEndingPos = this.matchEndingPos;
			switch (this.matchEndingPos)
			{
				case 0:
				{
					if (this.timeEndMatch < PhotonNetwork.time + (double)((!PhotonNetwork.isMasterClient ? 110 : 130)))
					{
						this.matchEndingPos = 2;
						UnityEngine.Debug.Log("two minutes remain");
					}
					goto case 1;
				}
				case 1:
				{
					if (this.writtedMatchEndingPos != this.matchEndingPos)
					{
						UnityEngine.Debug.Log(string.Concat("Write ending: ", this.matchEndingPos));
						ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
						hashtable[ConnectSceneNGUIController.endingProperty] = this.matchEndingPos;
						PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
					}
					if (this.timeEndMatch > 4290000 && this.networkTime < 2000000)
					{
						ExitGames.Client.Photon.Hashtable hashtable1 = new ExitGames.Client.Photon.Hashtable();
						hashtable1["TimeMatchEnd"] = this.networkTime + 60;
						PhotonNetwork.room.SetCustomProperties(hashtable1, null, false);
					}
					if (this.timeEndMatch <= 0)
					{
						this.timerToEndMatch = -1;
						break;
					}
					else
					{
						this.timerToEndMatch = this.timeEndMatch - this.networkTime;
						break;
					}
				}
				case 2:
				{
					if (this.timeEndMatch < PhotonNetwork.time + (double)((!PhotonNetwork.isMasterClient ? 50 : 70)))
					{
						UnityEngine.Debug.Log("one minute remain");
						this.matchEndingPos = 1;
					}
					goto case 1;
				}
				default:
				{
					goto case 1;
				}
			}
		}
		if (!Defs.isInet)
		{
			if (!Network.isServer)
			{
				this.networkTime += (double)Time.deltaTime;
			}
			else
			{
				this.networkTime = Network.time;
			}
			this.timerToEndMatch = this.timeEndMatch - this.networkTime;
		}
		if (this.timerToEndMatch >= 0 || Defs.isFlag)
		{
			this.isEndMatch = false;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			if (CapturePointController.sharedController != null && !this.isEndMatch)
			{
				CapturePointController.sharedController.EndMatch();
				this.isEndMatch = true;
			}
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			if (!this.isEndMatch)
			{
				ZombiManager.sharedManager.EndMatch();
			}
		}
		else if (WeaponManager.sharedManager.myPlayer == null)
		{
			GlobalGameController.countKillsRed = 0;
			GlobalGameController.countKillsBlue = 0;
		}
		else
		{
			WeaponManager.sharedManager.myPlayerMoveC.WinFromTimer();
		}
		if (this.wasPaused)
		{
			this.wasPaused = false;
			base.StartCoroutine(this.OnUnpause());
		}
		this.pauseTime = Tools.CurrentUnixTime;
	}
}