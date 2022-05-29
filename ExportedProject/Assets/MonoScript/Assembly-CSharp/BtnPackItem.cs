using System;
using UnityEngine;

public class BtnPackItem : MonoBehaviour
{
	public TypePackSticker typePack;

	public GameObject objListSticker;

	public GameObject activeState;

	public GameObject noActiveState;

	private StickerPackScroll scrollPack;

	public BtnPackItem()
	{
	}

	private void Awake()
	{
		this.scrollPack = base.GetComponentInParent<StickerPackScroll>();
	}

	public void HidePack()
	{
		if (this.objListSticker)
		{
			this.objListSticker.SetActive(false);
		}
		if (this.activeState)
		{
			this.activeState.SetActive(false);
		}
		if (this.noActiveState)
		{
			this.noActiveState.SetActive(true);
		}
	}

	private void OnClick()
	{
		if (this.scrollPack)
		{
			ButtonClickSound.Instance.PlayClick();
			this.scrollPack.ShowPack(this.typePack);
		}
	}

	public void ShowPack()
	{
		if (this.objListSticker)
		{
			this.objListSticker.SetActive(true);
			UIGrid component = this.objListSticker.GetComponent<UIGrid>();
			if (component != null)
			{
				component.Reposition();
			}
		}
		if (this.activeState)
		{
			this.activeState.SetActive(true);
		}
		if (this.noActiveState)
		{
			this.noActiveState.SetActive(false);
		}
	}
}