using System;
using UnityEngine;

public class HitStackController : MonoBehaviour
{
	public static HitStackController sharedController;

	public HitParticle[] particles;

	private int currentIndexHole;

	public HitStackController()
	{
	}

	public HitParticle GetCurrentParticle(bool _isUseMine)
	{
		bool flag = true;
		do
		{
			this.currentIndexHole++;
			if (this.currentIndexHole < (int)this.particles.Length)
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
		while (this.particles[this.currentIndexHole].isUseMine && !_isUseMine);
		return this.particles[this.currentIndexHole];
	}

	private void OnDestroy()
	{
		HitStackController.sharedController = null;
	}

	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		HitStackController.sharedController = this;
		Transform transforms = base.transform;
		transforms.position = Vector3.zero;
		int num = transforms.childCount;
		this.particles = new HitParticle[num];
		for (int i = 0; i < num; i++)
		{
			this.particles[i] = transforms.GetChild(i).GetComponent<HitParticle>();
		}
	}
}