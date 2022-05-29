using System;
using UnityEngine;

public class RotationWeaponGameObject : MonoBehaviour
{
	public RotationWeaponGameObject.ConstraintAxis axis;

	public float min;

	public float max;

	public Transform playerGun;

	public Transform mechGun;

	public Player_move_c player_move_c;

	private Transform thisTransform;

	private Vector3 rotateAround;

	private Quaternion minQuaternion;

	private Quaternion maxQuaternion;

	private float range;

	public RotationWeaponGameObject()
	{
	}

	private void LateUpdate()
	{
		float single;
		Quaternion quaternion = this.thisTransform.localRotation;
		if (this.axis != RotationWeaponGameObject.ConstraintAxis.X)
		{
			single = (this.axis != RotationWeaponGameObject.ConstraintAxis.Y ? quaternion.eulerAngles.z : quaternion.eulerAngles.y);
		}
		else
		{
			single = quaternion.eulerAngles.x;
		}
		Quaternion quaternion1 = Quaternion.AngleAxis(single, this.rotateAround);
		float single1 = Quaternion.Angle(quaternion1, this.minQuaternion);
		float single2 = Quaternion.Angle(quaternion1, this.maxQuaternion);
		if (single1 <= this.range && single2 <= this.range)
		{
			this.playerGun.rotation = this.thisTransform.rotation;
			this.playerGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
			this.mechGun.rotation = this.thisTransform.rotation;
			this.mechGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
			return;
		}
		Vector3 vector3 = quaternion.eulerAngles;
		vector3 = (single1 <= single2 ? new Vector3((this.axis != RotationWeaponGameObject.ConstraintAxis.X ? vector3.x : this.minQuaternion.eulerAngles.x), (this.axis != RotationWeaponGameObject.ConstraintAxis.Y ? vector3.y : this.minQuaternion.eulerAngles.y), (this.axis != RotationWeaponGameObject.ConstraintAxis.Z ? vector3.z : this.minQuaternion.eulerAngles.z)) : new Vector3((this.axis != RotationWeaponGameObject.ConstraintAxis.X ? vector3.x : this.maxQuaternion.eulerAngles.x), (this.axis != RotationWeaponGameObject.ConstraintAxis.Y ? vector3.y : this.maxQuaternion.eulerAngles.y), (this.axis != RotationWeaponGameObject.ConstraintAxis.Z ? vector3.z : this.maxQuaternion.eulerAngles.z)));
		this.thisTransform.localEulerAngles = vector3;
		this.playerGun.rotation = this.thisTransform.rotation;
		this.playerGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
		this.mechGun.rotation = this.thisTransform.rotation;
		this.mechGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
	}

	private void SetActiveFalse()
	{
		base.enabled = false;
	}

	private void Start()
	{
		float single;
		this.thisTransform = base.transform;
		switch (this.axis)
		{
			case RotationWeaponGameObject.ConstraintAxis.X:
			{
				this.rotateAround = Vector3.right;
				break;
			}
			case RotationWeaponGameObject.ConstraintAxis.Y:
			{
				this.rotateAround = Vector3.up;
				break;
			}
			case RotationWeaponGameObject.ConstraintAxis.Z:
			{
				this.rotateAround = Vector3.forward;
				break;
			}
		}
		if (this.axis != RotationWeaponGameObject.ConstraintAxis.X)
		{
			single = (this.axis != RotationWeaponGameObject.ConstraintAxis.Y ? this.thisTransform.localRotation.eulerAngles.z : this.thisTransform.localRotation.eulerAngles.y);
		}
		else
		{
			single = this.thisTransform.localRotation.eulerAngles.x;
		}
		Quaternion quaternion = Quaternion.AngleAxis(single, this.rotateAround);
		this.minQuaternion = quaternion * Quaternion.AngleAxis(this.min, this.rotateAround);
		this.maxQuaternion = quaternion * Quaternion.AngleAxis(this.max, this.rotateAround);
		this.range = this.max - this.min;
	}

	public enum ConstraintAxis
	{
		X,
		Y,
		Z
	}
}