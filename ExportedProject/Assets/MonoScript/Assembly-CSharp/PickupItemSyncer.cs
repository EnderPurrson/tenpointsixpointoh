using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSyncer : Photon.MonoBehaviour
{
	private const float TimeDeltaToIgnore = 0.2f;

	public bool IsWaitingForPickupInit;

	public PickupItemSyncer()
	{
	}

	public void AskForPickupItemSpawnTimes()
	{
		if (this.IsWaitingForPickupInit)
		{
			if ((int)PhotonNetwork.playerList.Length < 2)
			{
				Debug.Log("Cant ask anyone else for PickupItem spawn times.");
				this.IsWaitingForPickupInit = false;
				return;
			}
			PhotonPlayer next = PhotonNetwork.masterClient.GetNext();
			if (next == null || next.Equals(PhotonNetwork.player))
			{
				next = PhotonNetwork.player.GetNext();
			}
			if (next == null || next.Equals(PhotonNetwork.player))
			{
				Debug.Log("No player left to ask");
				this.IsWaitingForPickupInit = false;
			}
			else
			{
				base.photonView.RPC("RequestForPickupItems", next, new object[0]);
			}
		}
	}

	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[] { "Joined Room. isMasterClient: ", PhotonNetwork.isMasterClient, " id: ", PhotonNetwork.player.ID }));
		this.IsWaitingForPickupInit = !PhotonNetwork.isMasterClient;
		if ((int)PhotonNetwork.playerList.Length >= 2)
		{
			base.Invoke("AskForPickupItemSpawnTimes", 2f);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.SendPickedUpItems(newPlayer);
		}
	}

	[PunRPC]
	public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
	{
		this.IsWaitingForPickupInit = false;
		for (int i = 0; i < (int)inactivePickupsAndTimes.Length / 2; i++)
		{
			int num = i * 2;
			int num1 = (int)inactivePickupsAndTimes[num];
			float single = inactivePickupsAndTimes[num + 1];
			PhotonView photonView = PhotonView.Find(num1);
			PickupItem component = photonView.GetComponent<PickupItem>();
			if (single > 0f)
			{
				double num2 = (double)single + timeBase;
				Debug.Log(string.Concat(new object[] { photonView.viewID, " respawn: ", num2, " timeUntilRespawnBasedOnTimeBase:", single, " SecondsBeforeRespawn: ", component.SecondsBeforeRespawn }));
				double num3 = num2 - PhotonNetwork.time;
				if (single <= 0f)
				{
					num3 = 0;
				}
				component.PickedUp((float)num3);
			}
			else
			{
				component.PickedUp(0f);
			}
		}
	}

	[PunRPC]
	public void RequestForPickupItems(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender == null)
		{
			Debug.LogError("Unknown player asked for PickupItems");
			return;
		}
		this.SendPickedUpItems(msgInfo.sender);
	}

	[Obsolete("Use RequestForPickupItems(PhotonMessageInfo msgInfo) with corrected typing instead.")]
	[PunRPC]
	public void RequestForPickupTimes(PhotonMessageInfo msgInfo)
	{
		this.RequestForPickupItems(msgInfo);
	}

	private void SendPickedUpItems(PhotonPlayer targetPlayer)
	{
		if (targetPlayer == null)
		{
			Debug.LogWarning("Cant send PickupItem spawn times to unknown targetPlayer.");
			return;
		}
		double num = PhotonNetwork.time;
		double num1 = num + 0.20000000298023224;
		PickupItem[] pickupItemArray = new PickupItem[PickupItem.DisabledPickupItems.Count];
		PickupItem.DisabledPickupItems.CopyTo(pickupItemArray);
		List<float> singles = new List<float>((int)pickupItemArray.Length * 2);
		for (int i = 0; i < (int)pickupItemArray.Length; i++)
		{
			PickupItem pickupItem = pickupItemArray[i];
			if (pickupItem.SecondsBeforeRespawn > 0f)
			{
				double timeOfRespawn = pickupItem.TimeOfRespawn - PhotonNetwork.time;
				if (pickupItem.TimeOfRespawn > num1)
				{
					Debug.Log(string.Concat(new object[] { pickupItem.ViewID, " respawn: ", pickupItem.TimeOfRespawn, " timeUntilRespawn: ", timeOfRespawn, " (now: ", PhotonNetwork.time, ")" }));
					singles.Add((float)pickupItem.ViewID);
					singles.Add((float)timeOfRespawn);
				}
			}
			else
			{
				singles.Add((float)pickupItem.ViewID);
				singles.Add(0f);
			}
		}
		Debug.Log(string.Concat(new object[] { "Sent count: ", singles.Count, " now: ", num }));
		base.photonView.RPC("PickupItemInit", targetPlayer, new object[] { PhotonNetwork.time, singles.ToArray() });
	}
}