using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/My Center Scroll View on Child")]
public class MyCenterOnChild : MonoBehaviour
{
	public bool endlessScroll;

	public float springStrength = 8f;

	public float nextPageThreshold;

	public SpringPanel.OnFinished onFinished;

	private UIScrollView mScrollView;

	private GameObject mCenteredObject;

	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	public MyCenterOnChild()
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

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	private void OnEnable()
	{
		this.Recenter();
	}

	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

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
			this.mScrollView.onDragFinished = new UIScrollView.OnDragNotification(this.OnDragFinished);
			if (this.mScrollView.horizontalScrollBar != null)
			{
				this.mScrollView.horizontalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				this.mScrollView.verticalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Vector3[] vector3Array = this.mScrollView.panel.worldCorners;
		Vector3 vector3 = (vector3Array[2] + vector3Array[0]) * 0.5f;
		Vector3 vector31 = vector3 - (this.mScrollView.currentMomentum * (this.mScrollView.momentumAmount * 0.1f));
		this.mScrollView.currentMomentum = Vector3.zero;
		float single = Single.MaxValue;
		Transform child = null;
		Transform transforms = base.transform;
		int num = 0;
		int num1 = 0;
		int num2 = transforms.childCount;
		while (num1 < num2)
		{
			Transform child1 = transforms.GetChild(num1);
			float single1 = Vector3.SqrMagnitude(child1.position - vector31);
			if (single1 < single)
			{
				single = single1;
				child = child1;
				num = num1;
			}
			num1++;
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == transforms.GetChild(num))
		{
			Vector2 vector2 = UICamera.currentTouch.totalDelta;
			float single2 = 0f;
			UIScrollView.Movement movement = this.mScrollView.movement;
			if (movement == UIScrollView.Movement.Horizontal)
			{
				single2 = vector2.x;
			}
			else
			{
				single2 = (movement == UIScrollView.Movement.Vertical ? vector2.y : vector2.magnitude);
			}
			if (single2 <= this.nextPageThreshold)
			{
				if (single2 < -this.nextPageThreshold)
				{
					if (num < transforms.childCount - 1)
					{
						child = transforms.GetChild(num + 1);
					}
					else if (this.endlessScroll && transforms.childCount > 0)
					{
						child = transforms.GetChild(0);
					}
				}
			}
			else if (num > 0)
			{
				child = transforms.GetChild(num - 1);
			}
			else if (this.endlessScroll && transforms.childCount > 0 && num == 0)
			{
				child = transforms.GetChild(transforms.childCount - 1);
			}
		}
		this.CenterOn(child, vector3);
	}
}