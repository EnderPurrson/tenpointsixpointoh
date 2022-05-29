using System;
using UnityEngine;

[AddComponentMenu("ExplosionScripts/BetCopyer")]
[Serializable]
public class RubbleCreater : MonoBehaviour
{
	public Rigidbody bits;

	public float vel;

	public float up;

	public int amount;

	public int amountRandom;

	public bool useUp;

	private AudioClip soundClip;

	public RubbleCreater()
	{
		this.vel = (float)20;
		this.up = (float)20;
		this.amount = 20;
		this.amountRandom = 5;
	}

	public override void CreateObject()
	{
		this.amount += UnityEngine.Random.Range(-this.amountRandom, this.amountRandom);
		for (int i = 0; i < this.amount; i++)
		{
			Rigidbody rigidbody = (Rigidbody)UnityEngine.Object.Instantiate(this.bits, this.transform.position, this.transform.rotation);
			float single = UnityEngine.Random.Range(-this.vel, this.vel);
			UnityEngine.Random.Range((float)5, this.up);
			float single1 = UnityEngine.Random.Range(-this.vel, this.vel);
			if (!this.useUp)
			{
				rigidbody.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection(single, UnityEngine.Random.Range(-this.up, this.up), single1);
			}
			else
			{
				rigidbody.GetComponent<Rigidbody>().velocity = this.transform.TransformDirection(single, UnityEngine.Random.Range(this.up / (float)2, this.up), single1);
			}
		}
	}

	public override void Main()
	{
	}

	public override void Start()
	{
		this.CreateObject();
	}
}