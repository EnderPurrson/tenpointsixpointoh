using System;
using UnityEngine;

public class DeadExplosionController : MonoBehaviour
{
	public SkinnedMeshRenderer mySkinRenderer;

	public float timeAfteAnim = 0.5f;

	public float startTimerAnim = 0.5f;

	private float timeAnim = -1f;

	public DeadExplosionController()
	{
	}

	public void StartAnim()
	{
		this.timeAnim = this.startTimerAnim + this.timeAfteAnim;
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