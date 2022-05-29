using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	public UIGrid.Arrangement arrangement;

	public UIGrid.Sorting sorting;

	public UIWidget.Pivot pivot;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool animateSmoothly;

	public bool hideInactive;

	public bool keepWithinPanel;

	public UIGrid.OnReposition onReposition;

	public Comparison<Transform> onCustomSort;

	[HideInInspector]
	[SerializeField]
	private bool sorted;

	protected bool mReposition;

	protected UIPanel mPanel;

	protected bool mInitDone;

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

	public UIGrid()
	{
	}

	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	public Transform GetChild(int index)
	{
		Transform item;
		List<Transform> childList = this.GetChildList();
		if (index >= childList.Count)
		{
			item = null;
		}
		else
		{
			item = childList[index];
		}
		return item;
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
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				transforms1.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				transforms1.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
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

	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (!childList.Remove(t))
		{
			return false;
		}
		this.ResetPosition(childList);
		return true;
	}

	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(base.gameObject))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		this.ResetPosition(this.GetChildList());
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	protected virtual void ResetPosition(List<Transform> list)
	{
		float single;
		float single1;
		this.mReposition = false;
		int num = 0;
		int num1 = 0;
		int num2 = 0;
		int num3 = 0;
		Transform transforms = base.transform;
		int num4 = 0;
		int count = list.Count;
		while (num4 < count)
		{
			Transform item = list[num4];
			Vector3 vector3 = item.localPosition;
			float single2 = vector3.z;
			if (this.arrangement != UIGrid.Arrangement.CellSnap)
			{
				vector3 = (this.arrangement != UIGrid.Arrangement.Horizontal ? new Vector3(this.cellWidth * (float)num1, -this.cellHeight * (float)num, single2) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num1, single2));
			}
			else
			{
				if (this.cellWidth > 0f)
				{
					vector3.x = Mathf.Round(vector3.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector3.y = Mathf.Round(vector3.y / this.cellHeight) * this.cellHeight;
				}
			}
			if (!this.animateSmoothly || !Application.isPlaying || Vector3.SqrMagnitude(item.localPosition - vector3) < 0.0001f)
			{
				item.localPosition = vector3;
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(item.gameObject, vector3, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			num2 = Mathf.Max(num2, num);
			num3 = Mathf.Max(num3, num1);
			int num5 = num + 1;
			num = num5;
			if (num5 >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num1++;
			}
			num4++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			if (this.arrangement != UIGrid.Arrangement.Horizontal)
			{
				single = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				single1 = Mathf.Lerp((float)(-num2) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				single = Mathf.Lerp(0f, (float)num2 * this.cellWidth, pivotOffset.x);
				single1 = Mathf.Lerp((float)(-num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int i = 0; i < transforms.childCount; i++)
			{
				Transform child = transforms.GetChild(i);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component == null)
				{
					Vector3 vector31 = child.localPosition;
					vector31.x -= single;
					vector31.y -= single1;
					child.localPosition = vector31;
				}
				else
				{
					component.target.x -= single;
					component.target.y -= single1;
				}
			}
		}
	}

	protected virtual void Sort(List<Transform> list)
	{
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	public static int SortHorizontal(Transform a, Transform b)
	{
		Vector3 vector3 = a.localPosition;
		Vector3 vector31 = b.localPosition;
		return vector3.x.CompareTo(vector31.x);
	}

	public static int SortVertical(Transform a, Transform b)
	{
		Vector3 vector3 = b.localPosition;
		Vector3 vector31 = a.localPosition;
		return vector3.y.CompareTo(vector31.y);
	}

	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	protected virtual void Update()
	{
		this.Reposition();
		base.enabled = false;
	}

	public enum Arrangement
	{
		Horizontal,
		Vertical,
		CellSnap
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