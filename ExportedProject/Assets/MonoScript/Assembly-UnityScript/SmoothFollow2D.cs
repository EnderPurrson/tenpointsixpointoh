using System;
using UnityEngine;

[Serializable]
public class SmoothFollow2D : MonoBehaviour
{
	public Transform target;

	public float smoothTime;

	private Transform thisTransform;

	private Vector2 velocity;

	public SmoothFollow2D()
	{
		this.smoothTime = 0.3f;
	}

	public override void Main()
	{
	}

	public override void Start()
	{
		this.thisTransform = this.transform;
	}

	public override void Update()
	{
		float single = this.thisTransform.position.x;
		Vector3 vector3 = this.target.position;
		float single1 = Mathf.SmoothDamp(single, vector3.x, ref this.velocity.x, this.smoothTime);
		float single2 = single1;
		Vector3 vector31 = this.thisTransform.position;
		Vector3 vector32 = vector31;
		float single3 = single2;
		float single4 = single3;
		vector32.x = single3;
		Vector3 vector33 = vector32;
		Vector3 vector34 = vector33;
		this.thisTransform.position = vector33;
		float single5 = this.thisTransform.position.y;
		Vector3 vector35 = this.target.position;
		float single6 = Mathf.SmoothDamp(single5, vector35.y, ref this.velocity.y, this.smoothTime);
		float single7 = single6;
		Vector3 vector36 = this.thisTransform.position;
		Vector3 vector37 = vector36;
		float single8 = single7;
		float single9 = single8;
		vector37.y = single8;
		Vector3 vector38 = vector37;
		Vector3 vector39 = vector38;
		this.thisTransform.position = vector38;
	}
}