using System;
using UnityEngine;

public class HeadShotStackController : MonoBehaviour
{
	public static HeadShotStackController sharedController;

	public HitParticle[] particles;

	public Material mobHeadShotMaterial;

	private int currentIndexHole;

	public HeadShotStackController()
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
		HeadShotStackController.sharedController = null;
	}

	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		HeadShotStackController.sharedController = this;
		Transform transforms = base.transform;
		transforms.position = Vector3.zero;
		int num = transforms.childCount;
		this.particles = new HitParticle[num];
		for (int i = 0; i < num; i++)
		{
			this.particles[i] = transforms.GetChild(i).GetComponent<HitParticle>();
			if (!Defs.isMulti || Defs.isCOOP)
			{
				ParticleSystem particleSystem = this.particles[i].myParticleSystem;
				particleSystem.GetComponent<Renderer>().material = this.mobHeadShotMaterial;
			}
		}
	}
}