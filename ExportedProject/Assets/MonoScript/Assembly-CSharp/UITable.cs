using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	public int columns;

	public UITable.Direction direction;

	public UITable.Sorting sorting;

	public UIWidget.Pivot pivot;

	public UIWidget.Pivot cellAlignment;

	public bool hideInactive = true;

	public bool keepWithinPanel;

	public Vector2 padding = Vector2.zero;

	public UITable.OnReposition onReposition;

	public Comparison<Transform> onCustomSort;

	protected UIPanel mPanel;

	protected bool mInitDone;

	protected bool mReposition;

	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	public UITable()
	{
	}

	public List<Transform> GetChildList()
	{
		Transform transforms = base.transform;
		List<Transform> transforms1 = new List<Transform>();
		for (int i = 0; i < transforms.childCount; i++)
		{
			Transform child = transforms.GetChild(i);
			if (!this.hideInactive || child && NGUITools.GetActive(child.gameObject))
			{
				transforms1.Add(child);
			}
		}
		if (this.sorting != UITable.Sorting.None)
		{
			if (this.sorting == UITable.Sorting.Alphabetic)
			{
				transforms1.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UITable.Sorting.Horizontal)
			{
				transforms1.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UITable.Sorting.Vertical)
			{
				transforms1.Sort(new Comparison<Transform>(UIGrid.SortVertical));
			}
			else if (this.onCustomSort == null)
			{
				this.Sort(transforms1);
			}
			else
			{
				transforms1.Sort(this.onCustomSort);
			}
		}
		return transforms1;
	}

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	protected virtual void LateUpdate()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transforms = base.transform;
		List<Transform> childList = this.GetChildList();
		if (childList.Count > 0)
		{
			this.RepositionVariableSize(childList);
		}
		if (this.keepWithinPanel && this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transforms, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	protected void RepositionVariableSize(List<Transform> children)
	{
		float single = 0f;
		float single1 = 0f;
		int num = (this.columns <= 0 ? 1 : children.Count / this.columns + 1);
		int num1 = (this.columns <= 0 ? children.Count : this.columns);
		Bounds[,] boundsArray = new Bounds[num, num1];
		Bounds[] boundsArray1 = new Bounds[num1];
		Bounds[] boundsArray2 = new Bounds[num];
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int count = children.Count;
		while (num4 < count)
		{
			Transform item = children[num4];
			Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(item, !this.hideInactive);
			Vector3 vector3 = item.localScale;
			bound.min = Vector3.Scale(bound.min, vector3);
			bound.max = Vector3.Scale(bound.max, vector3);
			boundsArray[num3, num2] = bound;
			boundsArray1[num2].Encapsulate(bound);
			boundsArray2[num3].Encapsulate(bound);
			int num5 = num2 + 1;
			num2 = num5;
			if (num5 >= this.columns && this.columns > 0)
			{
				num2 = 0;
				num3++;
			}
			num4++;
		}
		num2 = 0;
		num3 = 0;
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.cellAlignment);
		int num6 = 0;
		int count1 = children.Count;
		while (num6 < count1)
		{
			Transform transforms = children[num6];
			Bounds bound1 = boundsArray[num3, num2];
			Bounds bound2 = boundsArray1[num2];
			Bounds bound3 = boundsArray2[num3];
			Vector3 vector31 = transforms.localPosition;
			Vector3 vector32 = bound1.extents;
			vector31.x = single + vector32.x - bound1.center.x;
			float single2 = vector31.x;
			float single3 = bound1.max.x;
			Vector3 vector33 = bound1.min;
			float single4 = single3 - vector33.x - bound2.max.x;
			Vector3 vector34 = bound2.min;
			vector31.x = single2 - (Mathf.Lerp(0f, single4 + vector34.x, pivotOffset.x) - this.padding.x);
			if (this.direction != UITable.Direction.Down)
			{
				Vector3 vector35 = bound1.extents;
				vector31.y = single1 + vector35.y - bound1.center.y;
				float single5 = vector31.y;
				float single6 = bound1.max.y;
				Vector3 vector36 = bound1.min;
				float single7 = single6 - vector36.y - bound3.max.y;
				Vector3 vector37 = bound3.min;
				vector31.y = single5 - (Mathf.Lerp(0f, single7 + vector37.y, pivotOffset.y) - this.padding.y);
			}
			else
			{
				Vector3 vector38 = bound1.extents;
				vector31.y = -single1 - vector38.y - bound1.center.y;
				float single8 = vector31.y;
				float single9 = bound1.max.y;
				Vector3 vector39 = bound1.min;
				float single10 = single9 - vector39.y - bound3.max.y;
				Vector3 vector310 = bound3.min;
				vector31.y = single8 + (Mathf.Lerp(single10 + vector310.y, 0f, pivotOffset.y) - this.padding.y);
			}
			Vector3 vector311 = bound2.size;
			single = single + (vector311.x + this.padding.x * 2f);
			transforms.localPosition = vector31;
			int num7 = num2 + 1;
			num2 = num7;
			if (num7 >= this.columns && this.columns > 0)
			{
				num2 = 0;
				num3++;
				single = 0f;
				Vector3 vector312 = bound3.size;
				single1 = single1 + (vector312.y + this.padding.y * 2f);
			}
			num6++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			Bounds bound4 = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			Vector3 vector313 = bound4.size;
			float single11 = Mathf.Lerp(0f, vector313.x, pivotOffset.x);
			Vector3 vector314 = bound4.size;
			float single12 = Mathf.Lerp(-vector314.y, 0f, pivotOffset.y);
			Transform transforms1 = base.transform;
			for (int i = 0; i < transforms1.childCount; i++)
			{
				Transform child = transforms1.GetChild(i);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component == null)
				{
					Vector3 vector315 = child.localPosition;
					vector315.x -= single11;
					vector315.y -= single12;
					child.localPosition = vector315;
				}
				else
				{
					component.target.x -= single11;
					component.target.y -= single12;
				}
			}
		}
	}

	protected virtual void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(UIGrid.SortByName));
	}

	protected virtual void Start()
	{
		this.Init();
		this.Reposition();
		base.enabled = false;
	}

	public enum Direction
	{
		Down,
		Up
	}

	public delegate void OnReposition();

	public enum Sorting
	{
		None,
		Alphabetic,
		Horizontal,
		Vertical,
		Custom
	}
}