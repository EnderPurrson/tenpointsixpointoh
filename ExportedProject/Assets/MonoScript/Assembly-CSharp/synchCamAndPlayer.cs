using System;
using UnityEngine;

internal sealed class synchCamAndPlayer : MonoBehaviour
{
	private bool isMine;

	private PhotonView photonView;

	public Transform gameObjectPlayerTrasform;

	private bool isMulti;

	private bool isInet;

	private Transform myTransform;

	public synchCamAndPlayer()
	{
	}

	private void invokeStart()
	{
	}

	public void setSynh(bool _isActive)
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.photonView = base.transform.parent.GetComponent<PhotonView>();
		if (this.isMulti)
		{
			if (this.isInet)
			{
				this.isMine = this.photonView.isMine;
			}
			else
			{
				this.isMine = base.transform.parent.GetComponent<NetworkView>().isMine;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			base.enabled = false;
		}
		else
		{
			base.SendMessage("SetActiveFalse");
		}
	}

	private void Update()
	{
		this.myTransform.rotation = this.gameObjectPlayerTrasform.rotation;
	}
}