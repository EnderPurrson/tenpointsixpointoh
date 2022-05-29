using System;
using UnityEngine;

internal sealed class ZombiSynchPhoton : MonoBehaviour
{
	private ThirdPersonCamera cameraScript;

	private ThirdPersonController controllerScript;

	private PhotonView photonView;

	public int сountUpdate;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private Transform myTransform;

	public ZombiSynchPhoton()
	{
	}

	private void Awake()
	{
		try
		{
			if (!Defs.isMulti || !Defs.isInet)
			{
				base.enabled = false;
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!stream.isWriting)
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
		else
		{
			stream.SendNext(this.myTransform.position);
			stream.SendNext(this.myTransform.rotation);
		}
	}

	private void Start()
	{
		try
		{
			this.myTransform = base.transform;
			this.photonView = PhotonView.Get(this);
			this.correctPlayerPos = this.myTransform.position;
			this.correctPlayerRot = this.myTransform.rotation;
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	private void Update()
	{
		try
		{
			if (!this.photonView.isMine)
			{
				this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.correctPlayerPos, Time.deltaTime * 5f);
				this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);
				if (this.сountUpdate < 10)
				{
					this.сountUpdate++;
				}
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}
}