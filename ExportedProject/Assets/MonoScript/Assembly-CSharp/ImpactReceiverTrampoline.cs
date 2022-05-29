using System;
using UnityEngine;

public class ImpactReceiverTrampoline : MonoBehaviour
{
	private float mass = 1f;

	private Vector3 impact = Vector3.zero;

	private CharacterController character;

	public ImpactReceiverTrampoline()
	{
	}

	public void AddImpact(Vector3 dir, float force)
	{
		ImpactReceiverTrampoline impactReceiverTrampoline = this;
		impactReceiverTrampoline.impact = impactReceiverTrampoline.impact + ((dir.normalized * force) / this.mass);
	}

	private void Start()
	{
		this.character = base.GetComponent<CharacterController>();
	}

	private void Update()
	{
		if (this.impact.magnitude <= 0.2f)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			this.character.Move(this.impact * Time.deltaTime);
		}
		this.impact = Vector3.Lerp(this.impact, Vector3.zero, 1f * Time.deltaTime);
	}
}