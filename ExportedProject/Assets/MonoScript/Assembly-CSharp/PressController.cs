using System;
using UnityEngine;

public class PressController : MonoBehaviour
{
	public bool isPrimary;

	public PressController primaryPress;

	private GameObject firstCollision;

	[HideInInspector]
	public GameObject secondCollision;

	public PressController()
	{
	}

	public void CheckSmash()
	{
		if (this.firstCollision == this.secondCollision)
		{
			this.firstCollision.GetComponent<SkinName>().playerMoveC.KillSelf();
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.transform.gameObject == WeaponManager.sharedManager.myPlayer)
		{
			if (!this.isPrimary)
			{
				this.primaryPress.secondCollision = col.transform.gameObject;
				this.primaryPress.CheckSmash();
			}
			else
			{
				this.firstCollision = col.transform.gameObject;
				this.CheckSmash();
			}
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.transform.gameObject == WeaponManager.sharedManager.myPlayer)
		{
			if (!this.isPrimary)
			{
				this.primaryPress.secondCollision = null;
			}
			else
			{
				this.firstCollision = null;
			}
		}
	}

	private void Update()
	{
	}
}