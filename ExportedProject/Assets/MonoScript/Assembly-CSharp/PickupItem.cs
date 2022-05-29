using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItem : Photon.MonoBehaviour, IPunObservable
{
	public float SecondsBeforeRespawn = 2f;

	public bool PickupOnTrigger;

	public bool PickupIsMine;

	public UnityEngine.MonoBehaviour OnPickedUpCall;

	public bool SentPickup;

	public double TimeOfRespawn;

	public static HashSet<PickupItem> DisabledPickupItems;

	public int ViewID
	{
		get
		{
			return base.photonView.viewID;
		}
	}

	static PickupItem()
	{
		PickupItem.DisabledPickupItems = new HashSet<PickupItem>();
	}

	public PickupItem()
	{
	}

	public void Drop()
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[0]);
		}
	}

	public void Drop(Vector3 newPosition)
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[] { newPosition });
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!stream.isWriting || this.SecondsBeforeRespawn > 0f)
		{
			Vector3 vector3 = (Vector3)stream.ReceiveNext();
			base.gameObject.transform.position = vector3;
		}
		else
		{
			stream.SendNext(base.gameObject.transform.position);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnTrigger && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	internal void PickedUp(float timeUntilRespawn)
	{
		base.gameObject.SetActive(false);
		PickupItem.DisabledPickupItems.Add(this);
		this.TimeOfRespawn = 0;
		if (timeUntilRespawn > 0f)
		{
			this.TimeOfRespawn = PhotonNetwork.time + (double)timeUntilRespawn;
			base.Invoke("PunRespawn", timeUntilRespawn);
		}
	}

	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickup", PhotonTargets.AllViaServer, new object[0]);
	}

	[PunRPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender.isLocal)
		{
			this.SentPickup = false;
		}
		if (!base.gameObject.GetActive())
		{
			Debug.Log(string.Concat(new object[] { "Ignored PU RPC, cause item is inactive. ", base.gameObject, " SecondsBeforeRespawn: ", this.SecondsBeforeRespawn, " TimeOfRespawn: ", this.TimeOfRespawn, " respawn in future: ", this.TimeOfRespawn > PhotonNetwork.time }));
			return;
		}
		this.PickupIsMine = msgInfo.sender.isLocal;
		if (this.OnPickedUpCall != null)
		{
			this.OnPickedUpCall.SendMessage("OnPickedUp", this);
		}
		if (this.SecondsBeforeRespawn > 0f)
		{
			double num = PhotonNetwork.time - msgInfo.timestamp;
			double secondsBeforeRespawn = (double)this.SecondsBeforeRespawn - num;
			if (secondsBeforeRespawn > 0)
			{
				this.PickedUp((float)secondsBeforeRespawn);
			}
		}
		else
		{
			this.PickedUp(0f);
		}
	}

	[PunRPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		this.PunRespawn();
		base.gameObject.transform.position = pos;
	}

	[PunRPC]
	internal void PunRespawn()
	{
		PickupItem.DisabledPickupItems.Remove(this);
		this.TimeOfRespawn = 0;
		this.PickupIsMine = false;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}
}