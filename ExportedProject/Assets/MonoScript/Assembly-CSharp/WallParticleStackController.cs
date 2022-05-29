using System;
using UnityEngine;

public class WallParticleStackController : MonoBehaviour
{
	public static WallParticleStackController sharedController;

	public WallBloodParticle[] particles;

	private int currentIndexHole;

	public WallParticleStackController()
	{
	}

	public WallBloodParticle GetCurrentParticle(bool _isUseMine)
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
		WallParticleStackController.sharedController = null;
	}

	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		WallParticleStackController.sharedController = this;
		Transform transforms = base.transform;
		transforms.position = Vector3.zero;
		int num = transforms.childCount;
		this.particles = new WallBloodParticle[num];
		for (int i = 0; i < num; i++)
		{
			this.particles[i] = transforms.GetChild(i).GetComponent<WallBloodParticle>();
		}
	}
}