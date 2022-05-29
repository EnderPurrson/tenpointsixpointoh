using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InRoomRoundTimer : MonoBehaviour
{
	private const string StartTimeKey = "st";

	public int SecondsPerTurn = 5;

	public double StartTime;

	public Rect TextPos = new Rect(0f, 80f, 150f, 300f);

	private bool startRoundWhenTimeIsSynced;

	public InRoomRoundTimer()
	{
	}

	public void OnGUI()
	{
		double startTime = PhotonNetwork.time - this.StartTime;
		double secondsPerTurn = (double)this.SecondsPerTurn - startTime % (double)this.SecondsPerTurn;
		int num = (int)(startTime / (double)this.SecondsPerTurn);
		GUILayout.BeginArea(this.TextPos);
		GUILayout.Label(string.Format("elapsed: {0:0.000}", startTime), new GUILayoutOption[0]);
		GUILayout.Label(string.Format("remaining: {0:0.000}", secondsPerTurn), new GUILayoutOption[0]);
		GUILayout.Label(string.Format("turn: {0:0}", num), new GUILayoutOption[0]);
		if (GUILayout.Button("new round", new GUILayoutOption[0]))
		{
			this.StartRoundNow();
		}
		GUILayout.EndArea();
	}

	public void OnJoinedRoom()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			Debug.Log(string.Concat("StartTime already set: ", PhotonNetwork.room.customProperties.ContainsKey("st")));
		}
		else
		{
			this.StartRoundNow();
		}
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!PhotonNetwork.room.customProperties.ContainsKey("st"))
		{
			Debug.Log("The new master starts a new round, cause we didn't start yet.");
			this.StartRoundNow();
		}
	}

	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("st"))
		{
			this.StartTime = (double)propertiesThatChanged["st"];
		}
	}

	private void StartRoundNow()
	{
		if (PhotonNetwork.time < 9.999999747378752E-05)
		{
			this.startRoundWhenTimeIsSynced = true;
			return;
		}
		this.startRoundWhenTimeIsSynced = false;
		Hashtable hashtable = new Hashtable();
		hashtable["st"] = PhotonNetwork.time;
		PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
	}

	private void Update()
	{
		if (this.startRoundWhenTimeIsSynced)
		{
			this.StartRoundNow();
		}
	}
}