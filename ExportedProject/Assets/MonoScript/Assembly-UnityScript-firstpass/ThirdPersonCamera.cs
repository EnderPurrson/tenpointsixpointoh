using System;
using UnityEngine;

[Serializable]
public class ThirdPersonCamera : MonoBehaviour
{
	public Transform cameraTransform;

	private Transform _target;

	public float distance;

	public float height;

	public float angularSmoothLag;

	public float angularMaxSpeed;

	public float heightSmoothLag;

	public float snapSmoothLag;

	public float snapMaxSpeed;

	public float clampHeadPositionScreenSpace;

	public float lockCameraTimeout;

	private Vector3 headOffset;

	private Vector3 centerOffset;

	private float heightVelocity;

	private float angleVelocity;

	private bool snap;

	private ThirdPersonController controller;

	private float targetHeight;

	public ThirdPersonCamera()
	{
		this.distance = 7f;
		this.height = 3f;
		this.angularSmoothLag = 0.3f;
		this.angularMaxSpeed = 15f;
		this.heightSmoothLag = 0.3f;
		this.snapSmoothLag = 0.2f;
		this.snapMaxSpeed = 720f;
		this.clampHeadPositionScreenSpace = 0.75f;
		this.lockCameraTimeout = 0.2f;
		this.headOffset = Vector3.zero;
		this.centerOffset = Vector3.zero;
		this.targetHeight = 100000f;
	}

	public override float AngleDistance(float a, float b)
	{
		a = Mathf.Repeat(a, (float)360);
		b = Mathf.Repeat(b, (float)360);
		return Mathf.Abs(b - a);
	}

	public override void Apply(Transform dummyTarget, Vector3 dummyCenter)
	{
		if (this.controller)
		{
			Vector3 vector3 = this._target.position + this.centerOffset;
			Vector3 vector31 = this._target.position + this.headOffset;
			float single = this._target.eulerAngles.y;
			float single1 = this.cameraTransform.eulerAngles.y;
			float single2 = single;
			if (Input.GetButton("Fire2"))
			{
				this.snap = true;
			}
			if (!this.snap)
			{
				if (this.controller.GetLockCameraTimer() < this.lockCameraTimeout)
				{
					single2 = single1;
				}
				if (this.AngleDistance(single1, single2) > (float)160 && this.controller.IsMovingBackwards())
				{
					single2 += (float)180;
				}
				single1 = Mathf.SmoothDampAngle(single1, single2, ref this.angleVelocity, this.angularSmoothLag, this.angularMaxSpeed);
			}
			else
			{
				if (this.AngleDistance(single1, single) < 3f)
				{
					this.snap = false;
				}
				single1 = Mathf.SmoothDampAngle(single1, single2, ref this.angleVelocity, this.snapSmoothLag, this.snapMaxSpeed);
			}
			if (!this.controller.IsJumping())
			{
				this.targetHeight = vector3.y + this.height;
			}
			else
			{
				float single3 = vector3.y + this.height;
				if (single3 < this.targetHeight || single3 - this.targetHeight > (float)5)
				{
					this.targetHeight = vector3.y + this.height;
				}
			}
			float single4 = this.cameraTransform.position.y;
			single4 = Mathf.SmoothDamp(single4, this.targetHeight, ref this.heightVelocity, this.heightSmoothLag);
			Quaternion quaternion = Quaternion.Euler((float)0, single1, (float)0);
			this.cameraTransform.position = vector3;
			this.cameraTransform.position = this.cameraTransform.position + ((quaternion * Vector3.back) * this.distance);
			float single5 = single4;
			float single6 = single5;
			Vector3 vector32 = this.cameraTransform.position;
			Vector3 vector33 = vector32;
			float single7 = single6;
			float single8 = single7;
			vector33.y = single7;
			Vector3 vector34 = vector33;
			Vector3 vector35 = vector34;
			this.cameraTransform.position = vector34;
			this.SetUpRotation(vector3, vector31);
		}
	}

	public override void Awake()
	{
		if (!this.cameraTransform && Camera.main)
		{
			this.cameraTransform = Camera.main.transform;
		}
		if (!this.cameraTransform)
		{
			Debug.Log("Please assign a camera to the ThirdPersonCamera script.");
			this.enabled = false;
		}
		this._target = this.transform;
		if (this._target)
		{
			this.controller = (ThirdPersonController)this._target.GetComponent(typeof(ThirdPersonController));
		}
		if (!this.controller)
		{
			Debug.Log("Please assign a target to the camera that has a ThirdPersonController script attached.");
		}
		else
		{
			CharacterController component = (CharacterController)this._target.GetComponent<Collider>();
			Bounds bound = component.bounds;
			this.centerOffset = bound.center - this._target.position;
			this.headOffset = this.centerOffset;
			float single = component.bounds.max.y;
			Vector3 vector3 = this._target.position;
			this.headOffset.y = single - vector3.y;
		}
		this.Cut(this._target, this.centerOffset);
	}

	public override void Cut(Transform dummyTarget, Vector3 dummyCenter)
	{
		float single = this.heightSmoothLag;
		float single1 = this.snapMaxSpeed;
		float single2 = this.snapSmoothLag;
		this.snapMaxSpeed = (float)10000;
		this.snapSmoothLag = 0.001f;
		this.heightSmoothLag = 0.001f;
		this.snap = true;
		this.Apply(this.transform, Vector3.zero);
		this.heightSmoothLag = single;
		this.snapMaxSpeed = single1;
		this.snapSmoothLag = single2;
	}

	public override void DebugDrawStuff()
	{
		Debug.DrawLine(this._target.position, this._target.position + this.headOffset);
	}

	public override Vector3 GetCenterOffset()
	{
		return this.centerOffset;
	}

	public override void LateUpdate()
	{
		this.Apply(this.transform, Vector3.zero);
	}

	public override void Main()
	{
	}

	public override void SetUpRotation(Vector3 centerPos, Vector3 headPos)
	{
		Vector3 vector3 = centerPos - this.cameraTransform.position;
		Quaternion quaternion = Quaternion.LookRotation(new Vector3(vector3.x, (float)0, vector3.z));
		Vector3 vector31 = (Vector3.forward * this.distance) + (Vector3.down * this.height);
		this.cameraTransform.rotation = quaternion * Quaternion.LookRotation(vector31);
		Ray ray = this.cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, (float)1));
		Ray ray1 = this.cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, this.clampHeadPositionScreenSpace, (float)1));
		Vector3 point = ray.GetPoint(this.distance);
		Vector3 point1 = ray1.GetPoint(this.distance);
		float single = Vector3.Angle(ray.direction, ray1.direction);
		float single1 = single / (point.y - point1.y);
		float single2 = single1 * (point.y - centerPos.y);
		if (single2 >= single)
		{
			single2 -= single;
			this.cameraTransform.rotation = this.cameraTransform.rotation * Quaternion.Euler(-single2, (float)0, (float)0);
		}
		else
		{
			single2 = (float)0;
		}
	}
}