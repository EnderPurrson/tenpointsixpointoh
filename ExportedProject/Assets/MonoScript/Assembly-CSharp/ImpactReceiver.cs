using System;
using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
	private float mass = 1f;

	private Vector3 impact = Vector3.zero;

	private CharacterController character;

	public ImpactReceiver()
	{
	}

	public void AddImpact(Vector3 dir, float force)
	{
		dir.Normalize();
		if (dir.y < 0f)
		{
			dir.y = -dir.y;
		}
		ImpactReceiver impactReceiver = this;
		impactReceiver.impact = impactReceiver.impact + ((dir.normalized * force) / this.mass);
	}

	private void Start()
	{
		this.character = base.GetComponent<CharacterController>();
	}

	private void Update()
	{
		if (this.impact.magnitude > 0.2f)
		{
			this.character.Move(this.impact * Time.deltaTime);
		}
		this.impact = Vector3.Lerp(this.impact, Vector3.zero, 5f * Time.deltaTime);
	}
}