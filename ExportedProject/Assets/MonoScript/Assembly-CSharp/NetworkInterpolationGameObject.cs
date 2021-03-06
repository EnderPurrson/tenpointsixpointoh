using System;
using UnityEngine;

public class NetworkInterpolationGameObject : MonoBehaviour
{
	private Quaternion correctPlayerRot = Quaternion.identity;

	public NetworkInterpolationGameObject()
	{
	}

	private void Awake()
	{
		if (!Defs.isMulti || Defs.isInet)
		{
			base.enabled = false;
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!stream.isWriting)
		{
			Quaternion quaternion = Quaternion.identity;
			stream.Serialize(ref quaternion);
			this.correctPlayerRot = quaternion;
		}
		else
		{
			Quaternion quaternion1 = base.transform.localRotation;
			stream.Serialize(ref quaternion1);
		}
	}

	private void Update()
	{
		if (!base.GetComponent<NetworkView>().isMine)
		{
			base.transform.localRotation = this.correctPlayerRot;
		}
	}
}