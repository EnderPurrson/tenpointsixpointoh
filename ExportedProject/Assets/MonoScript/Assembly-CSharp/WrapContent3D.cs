using System;
using UnityEngine;

public class WrapContent3D : MonoBehaviour
{
	public Transform[] wrappedObjects;

	public float maxDistance;

	public float deltaX;

	public Transform center;

	public WrapContent3D()
	{
	}

	private void Update()
	{
		for (int i = 0; i < (int)this.wrappedObjects.Length; i++)
		{
			float single = this.wrappedObjects[i].position.x;
			Vector3 vector3 = this.center.position;
			float single1 = (single - vector3.x) / 0.002604167f;
			float single2 = Mathf.Clamp01((this.maxDistance - Mathf.Abs(single1)) / this.maxDistance);
			this.wrappedObjects[i].localScale = Vector3.Lerp(Vector3.one, Vector3.zero, 0.7f - single2);
			this.wrappedObjects[i].gameObject.SetActive(Mathf.Abs(single1) < this.maxDistance);
		}
	}
}