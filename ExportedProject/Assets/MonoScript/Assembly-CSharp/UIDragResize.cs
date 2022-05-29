using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
	public UIWidget target;

	public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;

	public int minWidth = 100;

	public int minHeight = 100;

	public int maxWidth = 100000;

	public int maxHeight = 100000;

	public bool updateAnchors;

	private Plane mPlane;

	private Vector3 mRayPos;

	private Vector3 mLocalPos;

	private int mWidth;

	private int mHeight;

	private bool mDragging;

	public UIDragResize()
	{
	}

	private void OnDrag(Vector2 delta)
	{
		float single;
		if (this.mDragging && this.target != null)
		{
			Ray ray = UICamera.currentRay;
			if (this.mPlane.Raycast(ray, out single))
			{
				Transform transforms = this.target.cachedTransform;
				transforms.localPosition = this.mLocalPos;
				this.target.width = this.mWidth;
				this.target.height = this.mHeight;
				Vector3 point = ray.GetPoint(single) - this.mRayPos;
				transforms.position = transforms.position + point;
				Vector3 vector3 = Quaternion.Inverse(transforms.localRotation) * (transforms.localPosition - this.mLocalPos);
				transforms.localPosition = this.mLocalPos;
				NGUIMath.ResizeWidget(this.target, this.pivot, vector3.x, vector3.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
				if (this.updateAnchors)
				{
					this.target.BroadcastMessage("UpdateAnchors");
				}
			}
		}
	}

	private void OnDragEnd()
	{
		this.mDragging = false;
	}

	private void OnDragStart()
	{
		float single;
		if (this.target != null)
		{
			Vector3[] vector3Array = this.target.worldCorners;
			this.mPlane = new Plane(vector3Array[0], vector3Array[1], vector3Array[3]);
			Ray ray = UICamera.currentRay;
			if (this.mPlane.Raycast(ray, out single))
			{
				this.mRayPos = ray.GetPoint(single);
				this.mLocalPos = this.target.cachedTransform.localPosition;
				this.mWidth = this.target.width;
				this.mHeight = this.target.height;
				this.mDragging = true;
			}
		}
	}
}