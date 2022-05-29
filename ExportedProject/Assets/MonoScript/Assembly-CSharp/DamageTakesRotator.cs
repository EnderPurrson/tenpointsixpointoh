using System;
using UnityEngine;

public sealed class DamageTakesRotator : MonoBehaviour
{
	private Transform thisTransform;

	public InGameGUI inGameGUI;

	private GameObject myPlayer;

	public DamageTakesRotator()
	{
	}

	private void Start()
	{
		this.thisTransform = base.transform;
	}

	private void Update()
	{
		if (this.myPlayer == null)
		{
			if (!Defs.isMulti)
			{
				this.myPlayer = GameObject.FindGameObjectWithTag("Player");
			}
			else
			{
				this.myPlayer = WeaponManager.sharedManager.myPlayer;
			}
		}
		if (this.myPlayer == null)
		{
			return;
		}
		Transform transforms = this.thisTransform;
		Vector3 vector3 = this.myPlayer.transform.localRotation.eulerAngles;
		transforms.localRotation = Quaternion.Euler(new Vector3(0f, 0f, vector3.y));
	}
}