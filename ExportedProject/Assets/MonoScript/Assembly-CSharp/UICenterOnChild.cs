using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
	public float springStrength = 8f;

	public float nextPageThreshold;

	public SpringPanel.OnFinished onFinished;

	public UICenterOnChild.OnCenterCallback onCenter;

	private UIScrollView mScrollView;

	private GameObject mCenteredObject;

	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	public UICenterOnChild()
	{
	}

	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (!(target != null) || !(this.mScrollView != null) || !(this.mScrollView.panel != null))
		{
			this.mCenteredObject = null;
		}
		else
		{
			Transform transforms = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 vector3 = transforms.InverseTransformPoint(target.position);
			Vector3 vector31 = vector3 - transforms.InverseTransformPoint(panelCenter);
			if (!this.mScrollView.canMoveHorizontally)
			{
				vector31.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				vector31.y = 0f;
			}
			vector31.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, transforms.localPosition - vector31, this.springStrength).onFinished = this.onFinished;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] vector3Array = this.mScrollView.panel.worldCorners;
			Vector3 vector3 = (vector3Array[2] + vector3Array[0]) * 0.5f;
			this.CenterOn(target, vector3);
		}
	}

	private void OnDisable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = null;
		}
	}

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	private void OnEnable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = this;
			this.Recenter();
		}
	}

	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	[ContextMenu("Execute")]
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[] { base.GetType(), " requires ", typeof(UIScrollView), " on a parent object in order to work" }), this);
				base.enabled = false;
				return;
			}
			if (this.mScrollView)
			{
				this.mScrollView.centerOnChild = this;
				this.mScrollView.onDragFinished += new UIScrollView.OnDragNotification(this.OnDragFinished);
			}
			if (this.mScrollView.horizontalScrollBar != null)
			{
				this.mScrollView.horizontalScrollBar.onDragFinished += new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				this.mScrollView.verticalScrollBar.onDragFinished += new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transforms = base.transform;
		if (transforms.childCount == 0)
		{
			return;
		}
		Vector3[] vector3Array = this.mScrollView.panel.worldCorners;
		Vector3 vector3 = (vector3Array[2] + vector3Array[0]) * 0.5f;
		Vector3 vector31 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 vector32 = NGUIMath.SpringDampen(ref vector31, 9f, 2f);
		Vector3 vector33 = vector3 - (vector32 * 0.01f);
		float single = Single.MaxValue;
		Transform item = null;
		int num = 0;
		int num1 = 0;
		UIGrid component = base.GetComponent<UIGrid>();
		List<Transform> childList = null;
		if (component == null)
		{
			int num2 = 0;
			int num3 = transforms.childCount;
			int num4 = 0;
			while (num2 < num3)
			{
				Transform child = transforms.GetChild(num2);
				if (child.gameObject.activeInHierarchy)
				{
					float single1 = Vector3.SqrMagnitude(child.position - vector33);
					if (single1 < single)
					{
						single = single1;
						item = child;
						num = num2;
						num1 = num4;
					}
					num4++;
				}
				num2++;
			}
		}
		else
		{
			childList = component.GetChildList();
			int num5 = 0;
			int count = childList.Count;
			int num6 = 0;
			while (num5 < count)
			{
				Transform item1 = childList[num5];
				if (item1.gameObject.activeInHierarchy)
				{
					float single2 = Vector3.SqrMagnitude(item1.position - vector33);
					if (single2 < single)
					{
						single = single2;
						item = item1;
						num = num5;
						num1 = num6;
					}
					num6++;
				}
				num5++;
			}
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null)
		{
			if (this.mCenteredObject.transform == (childList == null ? transforms.GetChild(num) : childList[num]))
			{
				Vector3 vector34 = UICamera.currentTouch.totalDelta;
				vector34 = base.transform.rotation * vector34;
				float single3 = 0f;
				UIScrollView.Movement movement = this.mScrollView.movement;
				if (movement == UIScrollView.Movement.Horizontal)
				{
					single3 = vector34.x;
				}
				else
				{
					single3 = (movement == UIScrollView.Movement.Vertical ? vector34.y : vector34.magnitude);
				}
				if (Mathf.Abs(single3) > this.nextPageThreshold)
				{
					if (single3 <= this.nextPageThreshold)
					{
						if (single3 < -this.nextPageThreshold)
						{
							if (childList != null)
							{
								if (num1 >= childList.Count - 1)
								{
									item = (base.GetComponent<UIWrapContent>() != null ? childList[0] : childList[childList.Count - 1]);
								}
								else
								{
									item = childList[num1 + 1];
								}
							}
							else if (num1 >= transforms.childCount - 1)
							{
								item = (base.GetComponent<UIWrapContent>() != null ? transforms.GetChild(0) : transforms.GetChild(transforms.childCount - 1));
							}
							else
							{
								item = transforms.GetChild(num1 + 1);
							}
						}
					}
					else if (childList != null)
					{
						if (num1 <= 0)
						{
							item = (base.GetComponent<UIWrapContent>() != null ? childList[childList.Count - 1] : childList[0]);
						}
						else
						{
							item = childList[num1 - 1];
						}
					}
					else if (num1 <= 0)
					{
						item = (base.GetComponent<UIWrapContent>() != null ? transforms.GetChild(transforms.childCount - 1) : transforms.GetChild(0));
					}
					else
					{
						item = transforms.GetChild(num1 - 1);
					}
				}
			}
		}
		this.CenterOn(item, vector3);
	}

	private void Start()
	{
		this.Recenter();
	}

	public delegate void OnCenterCallback(GameObject centeredObject);
}