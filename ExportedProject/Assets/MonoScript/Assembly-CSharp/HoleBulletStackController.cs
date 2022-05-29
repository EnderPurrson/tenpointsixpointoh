using System;
using UnityEngine;

public class HoleBulletStackController : MonoBehaviour
{
	public static HoleBulletStackController sharedController;

	public HoleScript[] holes;

	private int currentIndexHole;

	public HoleBulletStackController()
	{
	}

	public HoleScript GetCurrentHole(bool _isUseMine)
	{
		bool flag = true;
		do
		{
			this.currentIndexHole++;
			if (this.currentIndexHole < (int)this.holes.Length)
			{
				continue;
			}
			if (!flag)
			{
				return null;
			}
			this.currentIndexHole = 0;
			flag = false;
		}
		while (this.holes[this.currentIndexHole].isUseMine && !_isUseMine);
		return this.holes[this.currentIndexHole];
	}

	private void OnDestroy()
	{
		HoleBulletStackController.sharedController = null;
	}

	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		HoleBulletStackController.sharedController = this;
		base.transform.position = Vector3.zero;
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("HoleBullet");
		this.holes = new HoleScript[(int)gameObjectArray.Length];
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			this.holes[i] = gameObjectArray[i].GetComponent<HoleScript>();
			this.holes[i].Init();
		}
	}
}