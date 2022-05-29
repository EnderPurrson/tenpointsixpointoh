using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Spring Panel")]
[RequireComponent(typeof(UIPanel))]
public class SpringPanel : MonoBehaviour
{
	public static SpringPanel current;

	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	public SpringPanel.OnFinished onFinished;

	private UIPanel mPanel;

	private Transform mTrans;

	private UIScrollView mDrag;

	public SpringPanel()
	{
	}

	protected virtual void AdvanceTowardsPosition()
	{
		float single = RealTime.deltaTime;
		bool flag = false;
		Vector3 vector3 = this.mTrans.localPosition;
		Vector3 vector31 = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, single);
		if ((vector31 - this.target).sqrMagnitude < 0.01f)
		{
			vector31 = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector31;
		Vector3 vector32 = vector31 - vector3;
		Vector2 vector2 = this.mPanel.clipOffset;
		vector2.x -= vector32.x;
		vector2.y -= vector32.y;
		this.mPanel.clipOffset = vector2;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel component = go.GetComponent<SpringPanel>();
		if (component == null)
		{
			component = go.AddComponent<SpringPanel>();
		}
		component.target = pos;
		component.strength = strength;
		component.onFinished = null;
		component.enabled = true;
		return component;
	}

	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	public delegate void OnFinished();
}