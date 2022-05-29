using ExitGames.Client.Photon;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal class PhotonHandler : Photon.MonoBehaviour
{
	private const string PlayerPrefsKey = "PUNCloudBestRegion";

	public static PhotonHandler SP;

	public int updateInterval;

	public int updateIntervalOnSerialize;

	private int nextSendTickCount;

	private int nextSendTickCountOnSerialize;

	private static bool sendThreadShouldRun;

	private static Stopwatch timerToStopConnectionInBackground;

	protected internal static bool AppQuits;

	protected internal static Type PingImplementation;

	internal static CloudRegionCode BestRegionCodeCurrently;

	internal static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			string str = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			if (string.IsNullOrEmpty(str))
			{
				return CloudRegionCode.none;
			}
			return Region.Parse(str);
		}
		set
		{
			if (value != CloudRegionCode.none)
			{
				PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
			}
			else
			{
				PlayerPrefs.DeleteKey("PUNCloudBestRegion");
			}
		}
	}

	static PhotonHandler()
	{
		PhotonHandler.BestRegionCodeCurrently = CloudRegionCode.none;
	}

	public PhotonHandler()
	{
	}

	protected void Awake()
	{
		if (PhotonHandler.SP != null && PhotonHandler.SP != this && PhotonHandler.SP.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(PhotonHandler.SP.gameObject);
		}
		PhotonHandler.SP = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.updateInterval = 1000 / PhotonNetwork.sendRate;
		this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
		PhotonHandler.StartFallbackSendAckThread();
	}

	public static bool FallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
		{
			if (PhotonHandler.timerToStopConnectionInBackground != null && PhotonNetwork.BackgroundTimeout > 0.001f && (float)PhotonHandler.timerToStopConnectionInBackground.ElapsedMilliseconds > PhotonNetwork.BackgroundTimeout * 1000f)
			{
				return PhotonHandler.sendThreadShouldRun;
			}
			PhotonNetwork.networkingPeer.SendAcksOnly();
		}
		return PhotonHandler.sendThreadShouldRun;
	}

	protected void OnApplicationPause(bool pause)
	{
		if (PhotonNetwork.BackgroundTimeout > 0.001f)
		{
			if (PhotonHandler.timerToStopConnectionInBackground == null)
			{
				PhotonHandler.timerToStopConnectionInBackground = new Stopwatch();
			}
			PhotonHandler.timerToStopConnectionInBackground.Reset();
			if (!pause)
			{
				PhotonHandler.timerToStopConnectionInBackground.Stop();
			}
			else
			{
				PhotonHandler.timerToStopConnectionInBackground.Start();
			}
		}
	}

	protected void OnApplicationQuit()
	{
		PhotonHandler.AppQuits = true;
		PhotonHandler.StopFallbackSendAckThread();
		PhotonNetwork.Disconnect();
	}

	protected void OnCreatedRoom()
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
	}

	protected void OnDestroy()
	{
		PhotonHandler.StopFallbackSendAckThread();
	}

	protected void OnJoinedRoom()
	{
		PhotonNetwork.networkingPeer.LoadLevelIfSynced();
	}

	protected void OnLevelWasLoaded(int level)
	{
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
	}

	protected internal static void PingAvailableRegionsAndConnectToBest()
	{
		PhotonHandler.SP.StartCoroutine(PhotonHandler.SP.PingAvailableRegionsCoroutine(true));
	}

	[DebuggerHidden]
	internal IEnumerator PingAvailableRegionsCoroutine(bool connectToBest)
	{
		PhotonHandler.u003cPingAvailableRegionsCoroutineu003ec__IteratorD2 variable = null;
		return variable;
	}

	public static void StartFallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun)
		{
			return;
		}
		PhotonHandler.sendThreadShouldRun = true;
		SupportClass.CallInBackground(new Func<bool>(PhotonHandler.FallbackSendAckThread));
	}

	public static void StopFallbackSendAckThread()
	{
		PhotonHandler.sendThreadShouldRun = false;
	}

	protected void Update()
	{
		if (PhotonNetwork.networkingPeer == null)
		{
			UnityEngine.Debug.LogError("NetworkPeer broke!");
			return;
		}
		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated || PhotonNetwork.connectionStateDetailed == ClientState.Disconnected || PhotonNetwork.offlineMode)
		{
			return;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			return;
		}
		Defs.inComingMessagesCounter = 0;
		bool flag = true;
		while (PhotonNetwork.isMessageQueueRunning && flag)
		{
			flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
			Defs.inComingMessagesCounter++;
		}
		Defs.inComingMessagesCounter = 0;
		int num = (int)(Time.realtimeSinceStartup * 1000f);
		if (PhotonNetwork.isMessageQueueRunning && num > this.nextSendTickCountOnSerialize)
		{
			PhotonNetwork.networkingPeer.RunViewUpdate();
			this.nextSendTickCountOnSerialize = num + this.updateIntervalOnSerialize;
			this.nextSendTickCount = 0;
		}
		num = (int)(Time.realtimeSinceStartup * 1000f);
		if (num > this.nextSendTickCount)
		{
			bool flag1 = true;
			while (PhotonNetwork.isMessageQueueRunning && flag1)
			{
				flag1 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
			}
			this.nextSendTickCount = num + this.updateInterval;
		}
	}
}