using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StickerPackScroll : MonoBehaviour
{
	public List<TypePackSticker> listItemData = new List<TypePackSticker>();

	public List<BtnPackItem> listButton = new List<BtnPackItem>();

	public GameObject parentButton;

	public TypePackSticker curShowPack;

	private UIGrid sortScript;

	public StickerPackScroll()
	{
	}

	private void Awake()
	{
		this.listButton.Clear();
		this.listButton.AddRange(base.GetComponentsInChildren<BtnPackItem>(true));
	}

	[DebuggerHidden]
	private IEnumerator crtUpdateListButton()
	{
		StickerPackScroll.u003ccrtUpdateListButtonu003ec__Iterator10B variable = null;
		return variable;
	}

	private void OnDisable()
	{
		StickersController.onBuyPack -= new Action(this.UpdateListButton);
	}

	private void OnEnable()
	{
		this.UpdateListButton();
		StickersController.onBuyPack += new Action(this.UpdateListButton);
	}

	public void ShowPack(TypePackSticker val)
	{
		for (int i = 0; i < this.listButton.Count; i++)
		{
			BtnPackItem item = this.listButton[i];
			if (item.typePack != val)
			{
				item.HidePack();
			}
			else
			{
				item.ShowPack();
				this.curShowPack = item.typePack;
			}
		}
	}

	public void Sort()
	{
		if (this.sortScript != null)
		{
			this.parentButton.SetActive(false);
			this.parentButton.SetActive(true);
			this.sortScript.Reposition();
		}
	}

	public void UpdateListButton()
	{
		base.StartCoroutine(this.crtUpdateListButton());
	}
}