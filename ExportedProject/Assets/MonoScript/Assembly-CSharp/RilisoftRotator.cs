using System;
using UnityEngine;

public class RilisoftRotator : MonoBehaviour
{
	public float rate = 10f;

	private Transform _transform;

	public RilisoftRotator()
	{
	}

	private void Start()
	{
		this._transform = base.transform;
	}

	private void Update()
	{
		this._transform.Rotate(Vector3.forward, this.rate * Time.deltaTime, Space.Self);
	}
}