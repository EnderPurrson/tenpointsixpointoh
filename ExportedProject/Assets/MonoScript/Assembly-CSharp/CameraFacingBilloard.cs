using System;
using UnityEngine;

public class CameraFacingBilloard : MonoBehaviour
{
	private Transform thisTransform;

	public bool Invert;

	public CameraFacingBilloard()
	{
	}

	private void Awake()
	{
		this.thisTransform = base.transform;
	}

	private void Update()
	{
		if (NickLabelController.currentCamera != null)
		{
			this.thisTransform.rotation = NickLabelController.currentCamera.transform.rotation;
			if (this.Invert)
			{
				Transform quaternion = this.thisTransform;
				quaternion.rotation = quaternion.rotation * new Quaternion(1f, 180f, 1f, 1f);
			}
		}
	}
}