using System;
using UnityEngine;

public class RemovePanelAuth : MonoBehaviour
{
	public float lifetime = 0.7f;

	private float startTime;

	private bool isDestroyed;

	public RemovePanelAuth()
	{
	}

	private void Start()
	{
		this.startTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (!this.isDestroyed && Time.realtimeSinceStartup - this.startTime >= this.lifetime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			this.isDestroyed = true;
		}
	}
}