using System;
using UnityEngine;

public class BonusItemDetailInfo : MonoBehaviour
{
	public UILabel title;

	public UILabel title1;

	public UILabel title2;

	public UILabel description;

	public UITexture imageHolder;

	public BonusItemDetailInfo()
	{
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void SetDescription(string text)
	{
		this.description.text = text;
	}

	public void SetImage(Texture2D image)
	{
		this.imageHolder.mainTexture = image;
	}

	public void SetTitle(string text)
	{
		this.title.text = text;
		this.title1.text = text;
		this.title2.text = text;
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}
}