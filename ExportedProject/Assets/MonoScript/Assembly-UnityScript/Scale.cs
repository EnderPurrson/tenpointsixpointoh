using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Scale : MonoBehaviour
{
	public ParticleEmitter[] particleEmitters;

	public float scale;

	[HideInInspector]
	[SerializeField]
	private float[] minsize;

	[HideInInspector]
	[SerializeField]
	private float[] maxsize;

	[HideInInspector]
	[SerializeField]
	private Vector3[] worldvelocity;

	[HideInInspector]
	[SerializeField]
	private Vector3[] localvelocity;

	[HideInInspector]
	[SerializeField]
	private Vector3[] rndvelocity;

	[HideInInspector]
	[SerializeField]
	private Vector3[] scaleBackUp;

	[HideInInspector]
	[SerializeField]
	private bool firstUpdate;

	public Scale()
	{
		this.scale = (float)1;
		this.firstUpdate = true;
	}

	public override void Main()
	{
	}

	public override void UpdateScale()
	{
		int num = Extensions[this.particleEmitters];
		if (this.firstUpdate)
		{
			this.minsize = new float[num];
			this.maxsize = new float[num];
			this.worldvelocity = new Vector3[num];
			this.localvelocity = new Vector3[num];
			this.rndvelocity = new Vector3[num];
			this.scaleBackUp = new Vector3[num];
		}
		for (int i = 0; i < Extensions[this.particleEmitters]; i++)
		{
			if (this.firstUpdate)
			{
				this.minsize[i] = this.particleEmitters[i].minSize;
				this.maxsize[i] = this.particleEmitters[i].maxSize;
				this.worldvelocity[i] = this.particleEmitters[i].worldVelocity;
				this.localvelocity[i] = this.particleEmitters[i].localVelocity;
				this.rndvelocity[i] = this.particleEmitters[i].rndVelocity;
				this.scaleBackUp[i] = this.particleEmitters[i].transform.localScale;
			}
			this.particleEmitters[i].minSize = this.minsize[i] * this.scale;
			this.particleEmitters[i].maxSize = this.maxsize[i] * this.scale;
			this.particleEmitters[i].worldVelocity = this.worldvelocity[i] * this.scale;
			this.particleEmitters[i].localVelocity = this.localvelocity[i] * this.scale;
			this.particleEmitters[i].rndVelocity = this.rndvelocity[i] * this.scale;
			this.particleEmitters[i].transform.localScale = this.scaleBackUp[i] * this.scale;
		}
		this.firstUpdate = false;
	}
}