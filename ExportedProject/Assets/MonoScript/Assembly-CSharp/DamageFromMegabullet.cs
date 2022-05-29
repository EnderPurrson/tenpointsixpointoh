using System;
using UnityEngine;

public class DamageFromMegabullet : MonoBehaviour
{
	public Rocket myRocketScript;

	public DamageFromMegabullet()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!this.myRocketScript.isRun)
		{
			return;
		}
		if (Defs.isMulti && !this.myRocketScript.isMine)
		{
			return;
		}
		if (other.gameObject.name.Equals("DamageCollider"))
		{
			return;
		}
		if (other.gameObject.CompareTag("CapturePoint"))
		{
			return;
		}
		if (!Defs.isMulti && (other.gameObject.tag.Equals("Player") || other.transform.parent != null && other.transform.parent.gameObject.tag.Equals("Player")))
		{
			return;
		}
		if (Defs.isMulti && (other.gameObject.tag.Equals("Player") && other.gameObject == WeaponManager.sharedManager.myPlayer || other.transform.parent != null && other.transform.parent.gameObject.tag.Equals("Player") && other.transform.parent.gameObject == WeaponManager.sharedManager.myPlayer))
		{
			return;
		}
		if ((!(other.gameObject.transform.parent != null) || !other.gameObject.transform.parent.gameObject.CompareTag("Untagged")) && (!(other.gameObject.transform.parent == null) || !other.gameObject.CompareTag("Untagged")))
		{
			this.myRocketScript.Hit(other);
		}
	}
}