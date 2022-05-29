using System;
using UnityEngine;

public class DamageTakenController : MonoBehaviour
{
	private float time;

	private float maxTime = 3f;

	public UISprite mySprite;

	public DamageTakenController()
	{
	}

	public void Remove()
	{
		this.time = -1f;
		this.mySprite.color = new Color(1f, 1f, 1f, 0f);
	}

	public void reset(float alpha)
	{
		this.time = this.maxTime;
		base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -alpha));
	}

	private void Start()
	{
		this.mySprite.color = new Color(1f, 1f, 1f, 0f);
	}

	private void Update()
	{
		if (this.time > 0f)
		{
			this.mySprite.color = new Color(1f, 1f, 1f, this.time / this.maxTime);
			this.time -= Time.deltaTime;
			if (this.time < 0f)
			{
				this.mySprite.color = new Color(1f, 1f, 1f, 0f);
			}
		}
	}
}