using System;
using UnityEngine;

internal sealed class ZombiManagerSynch : MonoBehaviour
{
	private ThirdPersonCamera cameraScript;

	private ThirdPersonController controllerScript;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	public ZombiManagerSynch()
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
		}
		else
		{
			stream.SendNext(base.transform.position);
		}
	}
}