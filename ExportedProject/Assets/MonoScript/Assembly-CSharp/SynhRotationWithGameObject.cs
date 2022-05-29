using System;
using UnityEngine;

public class SynhRotationWithGameObject : MonoBehaviour
{
	public Transform gameObject;

	public bool transformPos;

	private Transform myTransform;

	public Vector3 addpos = Vector3.zero;

	public SynhRotationWithGameObject()
	{
	}

	private void Start()
	{
		this.myTransform = base.transform;
	}

	private void Update()
	{
		this.myTransform.rotation = this.gameObject.rotation;
		if (this.transformPos)
		{
			this.myTransform.position = this.gameObject.TransformPoint(this.addpos);
		}
	}
}