using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class PhotonStatsGui : MonoBehaviour
{
	public bool statsWindowOn = true;

	public bool statsOn = true;

	public bool healthStatsVisible;

	public bool trafficStatsOn;

	public bool buttonsOn;

	public Rect statsRect = new Rect(0f, 100f, 200f, 50f);

	public int WindowId = 100;

	public PhotonStatsGui()
	{
	}

	public void OnGUI()
	{
		if (PhotonNetwork.networkingPeer.TrafficStatsEnabled != this.statsOn)
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = this.statsOn;
		}
		if (!this.statsWindowOn)
		{
			return;
		}
		this.statsRect = GUILayout.Window(this.WindowId, this.statsRect, new GUI.WindowFunction(this.TrafficStatsWindow), "Messages (shift+tab)", new GUILayoutOption[0]);
	}

	public void Start()
	{
		if (this.statsRect.x <= 0f)
		{
			this.statsRect.x = (float)Screen.width - this.statsRect.width;
		}
	}

	public void TrafficStatsWindow(int windowID)
	{
		bool flag = false;
		TrafficStatsGameLevel trafficStatsGameLevel = PhotonNetwork.networkingPeer.TrafficStatsGameLevel;
		long trafficStatsElapsedMs = PhotonNetwork.networkingPeer.TrafficStatsElapsedMs / (long)1000;
		if (trafficStatsElapsedMs == 0)
		{
			trafficStatsElapsedMs = (long)1;
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.buttonsOn = GUILayout.Toggle(this.buttonsOn, "buttons", new GUILayoutOption[0]);
		this.healthStatsVisible = GUILayout.Toggle(this.healthStatsVisible, "health", new GUILayoutOption[0]);
		this.trafficStatsOn = GUILayout.Toggle(this.trafficStatsOn, "traffic", new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		string str = string.Format("Out|In|Sum:\t{0,4} | {1,4} | {2,4}", trafficStatsGameLevel.TotalOutgoingMessageCount, trafficStatsGameLevel.TotalIncomingMessageCount, trafficStatsGameLevel.TotalMessageCount);
		string str1 = string.Format("{0}sec average:", trafficStatsElapsedMs);
		string str2 = string.Format("Out|In|Sum:\t{0,4} | {1,4} | {2,4}", (long)trafficStatsGameLevel.TotalOutgoingMessageCount / trafficStatsElapsedMs, (long)trafficStatsGameLevel.TotalIncomingMessageCount / trafficStatsElapsedMs, (long)trafficStatsGameLevel.TotalMessageCount / trafficStatsElapsedMs);
		GUILayout.Label(str, new GUILayoutOption[0]);
		GUILayout.Label(str1, new GUILayoutOption[0]);
		GUILayout.Label(str2, new GUILayoutOption[0]);
		if (this.buttonsOn)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.statsOn = GUILayout.Toggle(this.statsOn, "stats on", new GUILayoutOption[0]);
			if (GUILayout.Button("Reset", new GUILayoutOption[0]))
			{
				PhotonNetwork.networkingPeer.TrafficStatsReset();
				PhotonNetwork.networkingPeer.TrafficStatsEnabled = true;
			}
			flag = GUILayout.Button("To Log", new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		string empty = string.Empty;
		string empty1 = string.Empty;
		if (this.trafficStatsOn)
		{
			empty = string.Concat("Incoming: ", PhotonNetwork.networkingPeer.TrafficStatsIncoming.ToString());
			empty1 = string.Concat("Outgoing: ", PhotonNetwork.networkingPeer.TrafficStatsOutgoing.ToString());
			GUILayout.Label(empty, new GUILayoutOption[0]);
			GUILayout.Label(empty1, new GUILayoutOption[0]);
		}
		string empty2 = string.Empty;
		if (this.healthStatsVisible)
		{
			empty2 = string.Format("ping: {6}[+/-{7}]ms resent:{8}\nmax ms between\nsend: {0,4} dispatch: {1,4}\nlongest dispatch for:\nev({3}):{2,3}ms op({5}):{4,3}ms", new object[] { trafficStatsGameLevel.LongestDeltaBetweenSending, trafficStatsGameLevel.LongestDeltaBetweenDispatching, trafficStatsGameLevel.LongestEventCallback, trafficStatsGameLevel.LongestEventCallbackCode, trafficStatsGameLevel.LongestOpResponseCallback, trafficStatsGameLevel.LongestOpResponseCallbackOpCode, PhotonNetwork.networkingPeer.RoundTripTime, PhotonNetwork.networkingPeer.RoundTripTimeVariance, PhotonNetwork.networkingPeer.ResentReliableCommands });
			GUILayout.Label(empty2, new GUILayoutOption[0]);
		}
		if (flag)
		{
			Debug.Log(string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", new object[] { str, str1, str2, empty, empty1, empty2 }));
		}
		if (GUI.changed)
		{
			this.statsRect.height = 100f;
		}
		GUI.DragWindow();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
		{
			this.statsWindowOn = !this.statsWindowOn;
			this.statsOn = true;
		}
	}
}