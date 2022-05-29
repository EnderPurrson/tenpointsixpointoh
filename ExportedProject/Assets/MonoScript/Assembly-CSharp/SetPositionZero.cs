using System;
using UnityEngine;

public class SetPositionZero : MonoBehaviour
{
	private Transform myTransform;

	public SetPositionZero()
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.localPosition = Vector3.zero;
	}

	private void Update()
	{
		this.myTransform.localPosition = Vector3.zero;
	}
}