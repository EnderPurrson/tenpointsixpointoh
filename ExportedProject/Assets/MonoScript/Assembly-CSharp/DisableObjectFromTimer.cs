using System;
using UnityEngine;

public class DisableObjectFromTimer : MonoBehaviour
{
	public float timer = -1f;

	public bool isDestroy;

	public DisableObjectFromTimer()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.timer >= 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				if (!this.isDestroy)
				{
					base.gameObject.SetActive(false);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
	}
}