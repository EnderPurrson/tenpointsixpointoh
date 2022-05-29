using System;
using UnityEngine;

public sealed class PortalForPlayerController : MonoBehaviour
{
	public PortalForPlayerController myDublicatePortal;

	private Transform myPointOut;

	public PortalForPlayerController()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.Equals("BodyCollider") && other.transform.parent != null && other.transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
		{
			WeaponManager.sharedManager.myPlayer.transform.position = this.myDublicatePortal.myPointOut.position;
			WeaponManager.sharedManager.myPlayerMoveC.myPersonNetwork.isTeleported = true;
			float single = this.myPointOut.transform.rotation.eulerAngles.y;
			float single1 = this.myDublicatePortal.myPointOut.transform.rotation.eulerAngles.y;
			if (single1 < single)
			{
				single1 += 360f;
			}
			float single2 = single1 - single - 180f;
			WeaponManager.sharedManager.myPlayer.transform.Rotate(0f, single2, 0f);
			WeaponManager.sharedManager.myPlayerMoveC.PlayPortalSound();
		}
	}

	private void Start()
	{
		this.myPointOut = base.transform.GetChild(0);
	}
}