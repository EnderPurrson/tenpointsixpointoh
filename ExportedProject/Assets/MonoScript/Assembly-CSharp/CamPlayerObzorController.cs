using System;
using UnityEngine;

internal sealed class CamPlayerObzorController : MonoBehaviour
{
	private bool isMine;

	public GameObject playerGameObject;

	public CamPlayerObzorController()
	{
	}

	private void Start()
	{
		if (Defs.isMulti && Defs.isInet && !base.transform.parent.GetComponent<PhotonView>().isMine)
		{
			this.isMine = true;
		}
		if (!this.isMine)
		{
			base.enabled = false;
		}
		else
		{
			base.SendMessage("SetActiveFalse");
		}
		this.playerGameObject = base.transform.parent.GetComponent<SkinName>().playerGameObject;
	}

	private void Update()
	{
		Transform transforms = base.transform;
		float single = this.playerGameObject.transform.rotation.x;
		float single1 = base.transform.rotation.y;
		Quaternion quaternion = base.transform.rotation;
		transforms.rotation = Quaternion.Euler(new Vector3(single, single1, quaternion.z));
	}
}