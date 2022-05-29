using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : Photon.MonoBehaviour
{
	public bool DestroyByRpc;

	public OnClickDestroy()
	{
	}

	[DebuggerHidden]
	[PunRPC]
	public IEnumerator DestroyRpc()
	{
		OnClickDestroy.u003cDestroyRpcu003ec__IteratorD5 variable = null;
		return variable;
	}

	public void OnClick()
	{
		if (this.DestroyByRpc)
		{
			base.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered, new object[0]);
		}
		else
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
	}
}