using System;
using UnityEngine;

public class DeadEnergyController : MonoBehaviour
{
	public SkinnedMeshRenderer mySkinRenderer;

	public ParticleSystem myParticle;

	public float timeAfteAnim;

	public float startTimerAnim = 1f;

	private float timeAnim = -1f;

	public DeadEnergyController()
	{
	}

	public void StartAnim(Color _color, Texture _skin)
	{
		this.timeAnim = this.startTimerAnim + this.timeAfteAnim;
		this.mySkinRenderer.material.SetColor("_BurnColor", _color);
		this.mySkinRenderer.material.SetTexture("_MainTex", _skin);
		this.myParticle.startColor = _color;
	}

	private void Update()
	{
		if (this.timeAnim > 0f)
		{
			float single = 1.25f;
			this.timeAnim -= Time.deltaTime;
			if (this.timeAnim < this.startTimerAnim)
			{
				single = -0.25f + 1.5f * this.timeAnim / this.startTimerAnim;
			}
			this.mySkinRenderer.material.SetFloat("_Burn", single);
		}
	}
}