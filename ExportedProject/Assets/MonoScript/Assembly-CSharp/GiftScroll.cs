using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GiftScroll : MonoBehaviour
{
	private List<SlotInfo> listItemData = new List<SlotInfo>();

	public List<GiftHUDItem> listButton = new List<GiftHUDItem>();

	public GiftHUDItem exampleBut;

	public GameObject parentButton;

	public UIWrapContent wrapScript;

	public UIScrollView scView;

	public BoxCollider scrollAreaCollider;

	public static bool canReCreateSlots;

	static GiftScroll()
	{
		GiftScroll.canReCreateSlots = true;
	}

	public GiftScroll()
	{
	}

	public void AnimScrollGift(int num)
	{
		if (this.listButton.Count > num)
		{
			this.listButton[num].InCenter(true, this.listButton.Count);
		}
	}

	private void Awake()
	{
		if (this.exampleBut)
		{
			this.exampleBut.gameObject.SetActive(false);
		}
		this.scView = base.GetComponentInParent<UIScrollView>();
	}

	private GiftHUDItem CreateButton()
	{
		GameObject vector3 = UnityEngine.Object.Instantiate(this.exampleBut.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		vector3.SetActive(true);
		GiftHUDItem component = vector3.GetComponent<GiftHUDItem>();
		vector3.transform.parent = this.parentButton.transform;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		return component;
	}

	[DebuggerHidden]
	private IEnumerator CrtSort()
	{
		GiftScroll.u003cCrtSortu003ec__Iterator14F variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator crtUpdateListButton()
	{
		GiftScroll.u003ccrtUpdateListButtonu003ec__Iterator14E variable = null;
		return variable;
	}

	private void OnDisable()
	{
		GiftController.OnChangeSlots -= new Action(this.UpdateListButton);
	}

	private void OnEnable()
	{
		GiftController.OnChangeSlots += new Action(this.UpdateListButton);
		this.UpdateListButton();
	}

	private void SetButtonCount(int needCount)
	{
		if (this.listButton.Count < needCount)
		{
			for (int i = this.listButton.Count; i < needCount; i++)
			{
				GiftHUDItem giftHUDItem = this.CreateButton();
				this.listButton.Add(giftHUDItem);
			}
		}
		else if (this.listButton.Count > needCount)
		{
			int count = this.listButton.Count - needCount;
			for (int j = 0; j < count; j++)
			{
				GameObject item = this.listButton[this.listButton.Count - 1].gameObject;
				this.listButton[this.listButton.Count - 1] = null;
				this.listButton.RemoveAt(this.listButton.Count - 1);
				UnityEngine.Object.Destroy(item);
			}
		}
	}

	public void SetCanDraggable(bool val)
	{
		if (this.scrollAreaCollider)
		{
			this.scrollAreaCollider.enabled = val;
		}
		for (int i = 0; i < this.listButton.Count; i++)
		{
			this.listButton[i].colliderForDrag.enabled = val;
		}
	}

	public void Sort()
	{
		if (GiftScroll.canReCreateSlots)
		{
			base.StartCoroutine(this.CrtSort());
		}
	}

	[ContextMenu("Center main gift")]
	private void TestCenterGift()
	{
		if (this.listButton.Count > 6)
		{
			this.listButton[0].InCenter(false, 1);
		}
	}

	[ContextMenu("Sort gift")]
	private void TestSortGift()
	{
		this.Sort();
	}

	public void UpdateListButton()
	{
		if (GiftScroll.canReCreateSlots && base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.crtUpdateListButton());
		}
	}
}