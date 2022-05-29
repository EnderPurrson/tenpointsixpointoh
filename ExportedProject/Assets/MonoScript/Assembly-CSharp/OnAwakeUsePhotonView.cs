using Photon;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class OnAwakeUsePhotonView : Photon.MonoBehaviour
{
	public OnAwakeUsePhotonView()
	{
	}

	private void Awake()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[0]);
	}

	[PunRPC]
	public void OnAwakeRPC()
	{
		Debug.Log(string.Concat("RPC: 'OnAwakeRPC' PhotonView: ", base.photonView));
	}

	[PunRPC]
	public void OnAwakeRPC(byte myParameter)
	{
		Debug.Log(string.Concat(new object[] { "RPC: 'OnAwakeRPC' Parameter: ", myParameter, " PhotonView: ", base.photonView }));
	}

	private void Start()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[] { (byte)1 });
	}
}