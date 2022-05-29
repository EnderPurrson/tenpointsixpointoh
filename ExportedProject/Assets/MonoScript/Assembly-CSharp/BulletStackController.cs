using System;
using UnityEngine;

public class BulletStackController : MonoBehaviour
{
	public static BulletStackController sharedController;

	public Transform standartBulletStack;

	public Transform redBulletStack;

	public Transform for252BulletStack;

	public Transform turquoiseBulletStack;

	public Transform greenBulletStack;

	public Transform violetBulletStack;

	public GameObject[][] bullets;

	private int[] currentIndexBullet = new int[6];

	public BulletStackController()
	{
	}

	public GameObject GetCurrentBullet(int type = 0)
	{
		if (type < 0)
		{
			return null;
		}
		this.currentIndexBullet[type]++;
		if (this.currentIndexBullet[type] >= (int)this.bullets[type].Length)
		{
			this.currentIndexBullet[type] = 0;
		}
		return this.bullets[type][this.currentIndexBullet[type]];
	}

	private void OnDestroy()
	{
		BulletStackController.sharedController = null;
	}

	private void Start()
	{
		BulletStackController.sharedController = this;
		base.transform.position = Vector3.zero;
		for (int i = 0; i < 6; i++)
		{
			this.currentIndexBullet[i] = 0;
		}
		this.bullets = new GameObject[][] { new GameObject[this.standartBulletStack.childCount], null, null, null, null, null };
		for (int j = 0; j < (int)this.bullets[0].Length; j++)
		{
			this.bullets[0][j] = this.standartBulletStack.GetChild(j).gameObject;
		}
		this.bullets[1] = new GameObject[this.redBulletStack.childCount];
		for (int k = 0; k < (int)this.bullets[1].Length; k++)
		{
			this.bullets[1][k] = this.redBulletStack.GetChild(k).gameObject;
		}
		this.bullets[2] = new GameObject[this.for252BulletStack.childCount];
		for (int l = 0; l < (int)this.bullets[2].Length; l++)
		{
			this.bullets[2][l] = this.for252BulletStack.GetChild(l).gameObject;
		}
		this.bullets[3] = new GameObject[this.turquoiseBulletStack.childCount];
		for (int m = 0; m < (int)this.bullets[3].Length; m++)
		{
			this.bullets[3][m] = this.turquoiseBulletStack.GetChild(m).gameObject;
		}
		this.bullets[4] = new GameObject[this.greenBulletStack.childCount];
		for (int n = 0; n < (int)this.bullets[4].Length; n++)
		{
			this.bullets[4][n] = this.greenBulletStack.GetChild(n).gameObject;
		}
		this.bullets[5] = new GameObject[this.violetBulletStack.childCount];
		for (int o = 0; o < (int)this.bullets[5].Length; o++)
		{
			this.bullets[5][o] = this.violetBulletStack.GetChild(o).gameObject;
		}
	}
}