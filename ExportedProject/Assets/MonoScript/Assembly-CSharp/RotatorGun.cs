using System;
using UnityEngine;

public class RotatorGun : MonoBehaviour
{
	public GameObject playerGun;

	public RotatorGun()
	{
	}

	private void Update()
	{
		this.playerGun.transform.rotation = base.transform.rotation;
	}
}