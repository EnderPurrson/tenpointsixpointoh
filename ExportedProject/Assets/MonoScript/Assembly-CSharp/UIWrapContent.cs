using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
	public int itemSize = 100;

	public bool cullContent = true;

	public int minIndex;

	public int maxIndex;

	public UIWrapContent.OnInitializeItem onInitializeItem;

	protected Transform mTrans;

	protected UIPanel mPanel;

	protected UIScrollView mScroll;

	protected bool mHorizontal;

	protected bool mFirstTime = true;

	protected List<Transform> mChildren = new List<Transform>();

	public UIWrapContent()
	{
	}

	protected bool CacheScrollView()
	{
		this.mTrans = base.transform;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mScroll = this.mPanel.GetComponent<UIScrollView>();
		if (this.mScroll == null)
		{
			return false;
		}
		if (this.mScroll.movement != UIScrollView.Movement.Horizontal)
		{
			if (this.mScroll.movement != UIScrollView.Movement.Vertical)
			{
				return false;
			}
			this.mHorizontal = false;
		}
		else
		{
			this.mHorizontal = true;
		}
		return true;
	}

	protected virtual void OnMove(UIPanel panel)
	{
		this.WrapContent();
	}

	private void OnValidate()
	{
		if (this.maxIndex < this.minIndex)
		{
			this.maxIndex = this.minIndex;
		}
		if (this.minIndex > this.maxIndex)
		{
			this.maxIndex = this.minIndex;
		}
	}

	protected virtual void ResetChildPositions()
	{
		int num = 0;
		int count = this.mChildren.Count;
		while (num < count)
		{
			Transform item = this.mChildren[num];
			item.localPosition = (!this.mHorizontal ? new Vector3(0f, (float)(-num * this.itemSize), 0f) : new Vector3((float)(num * this.itemSize), 0f, 0f));
			this.UpdateItem(item, num);
			num++;
		}
	}

	[ContextMenu("Sort Alphabetically")]
	public virtual void SortAlphabetically()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortByName));
		this.ResetChildPositions();
	}

	[ContextMenu("Sort Based on Scroll Movement")]
	public virtual void SortBasedOnScrollMovement()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		if (!this.mHorizontal)
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortVertical));
		}
		else
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
		}
		this.ResetChildPositions();
	}

	protected virtual void Start()
	{
		this.SortBasedOnScrollMovement();
		this.WrapContent();
		if (this.mScroll != null)
		{
			UIWrapContent uIWrapContent = this;
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(uIWrapContent.OnMove);
		}
		this.mFirstTime = false;
	}

	protected virtual void UpdateItem(Transform item, int index)
	{
		int num;
		if (this.onInitializeItem != null)
		{
			if (this.mScroll.movement != UIScrollView.Movement.Vertical)
			{
				Vector3 vector3 = item.localPosition;
				num = Mathf.RoundToInt(vector3.x / (float)this.itemSize);
			}
			else
			{
				Vector3 vector31 = item.localPosition;
				num = Mathf.RoundToInt(vector31.y / (float)this.itemSize);
			}
			this.onInitializeItem(item.gameObject, index, num);
		}
	}

	public virtual void WrapContent()
	{
		float count = (float)(this.itemSize * this.mChildren.Count) * 0.5f;
		Vector3[] vector3Array = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector3 = vector3Array[i];
			vector3 = this.mTrans.InverseTransformPoint(vector3);
			vector3Array[i] = vector3;
		}
		Vector3 vector31 = Vector3.Lerp(vector3Array[0], vector3Array[2], 0.5f);
		bool flag = true;
		float single = count * 2f;
		if (!this.mHorizontal)
		{
			float single1 = vector3Array[0].y - (float)this.itemSize;
			float single2 = vector3Array[2].y + (float)this.itemSize;
			int num = 0;
			int count1 = this.mChildren.Count;
			while (num < count1)
			{
				Transform item = this.mChildren[num];
				float single3 = item.localPosition.y - vector31.y;
				if (single3 < -count)
				{
					Vector3 vector32 = item.localPosition;
					vector32.y += single;
					single3 = vector32.y - vector31.y;
					int num1 = Mathf.RoundToInt(vector32.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || this.minIndex <= num1 && num1 <= this.maxIndex)
					{
						item.localPosition = vector32;
						this.UpdateItem(item, num);
					}
					else
					{
						flag = false;
					}
				}
				else if (single3 > count)
				{
					Vector3 vector33 = item.localPosition;
					vector33.y -= single;
					single3 = vector33.y - vector31.y;
					int num2 = Mathf.RoundToInt(vector33.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || this.minIndex <= num2 && num2 <= this.maxIndex)
					{
						item.localPosition = vector33;
						this.UpdateItem(item, num);
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(item, num);
				}
				if (this.cullContent)
				{
					float single4 = this.mPanel.clipOffset.y;
					Vector3 vector34 = this.mTrans.localPosition;
					single3 = single3 + (single4 - vector34.y);
					if (!UICamera.IsPressed(item.gameObject))
					{
						NGUITools.SetActive(item.gameObject, (single3 <= single1 ? false : single3 < single2), false);
					}
				}
				num++;
			}
		}
		else
		{
			float single5 = vector3Array[0].x - (float)this.itemSize;
			float single6 = vector3Array[2].x + (float)this.itemSize;
			int num3 = 0;
			int count2 = this.mChildren.Count;
			while (num3 < count2)
			{
				Transform transforms = this.mChildren[num3];
				if (transforms != null)
				{
					float single7 = transforms.localPosition.x - vector31.x;
					if (single7 < -count)
					{
						Vector3 vector35 = transforms.localPosition;
						vector35.x += single;
						single7 = vector35.x - vector31.x;
						int num4 = Mathf.RoundToInt(vector35.x / (float)this.itemSize);
						if (this.minIndex == this.maxIndex || this.minIndex <= num4 && num4 <= this.maxIndex)
						{
							transforms.localPosition = vector35;
							this.UpdateItem(transforms, num3);
						}
						else
						{
							flag = false;
						}
					}
					else if (single7 > count)
					{
						Vector3 vector36 = transforms.localPosition;
						vector36.x -= single;
						single7 = vector36.x - vector31.x;
						int num5 = Mathf.RoundToInt(vector36.x / (float)this.itemSize);
						if (this.minIndex == this.maxIndex || this.minIndex <= num5 && num5 <= this.maxIndex)
						{
							transforms.localPosition = vector36;
							this.UpdateItem(transforms, num3);
						}
						else
						{
							flag = false;
						}
					}
					else if (this.mFirstTime)
					{
						this.UpdateItem(transforms, num3);
					}
					if (this.cullContent)
					{
						float single8 = this.mPanel.clipOffset.x;
						Vector3 vector37 = this.mTrans.localPosition;
						single7 = single7 + (single8 - vector37.x);
						if (!UICamera.IsPressed(transforms.gameObject))
						{
							NGUITools.SetActive(transforms.gameObject, (single7 <= single5 ? false : single7 < single6), false);
						}
					}
				}
				num3++;
			}
		}
		this.mScroll.restrictWithinPanel = !flag;
	}

	public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}