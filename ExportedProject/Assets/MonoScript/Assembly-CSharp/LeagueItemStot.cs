using System;
using UnityEngine;

public class LeagueItemStot : MonoBehaviour
{
	[SerializeField]
	private UITexture _texture;

	[SerializeField]
	private GameObject _lockIndicator;

	private Color _baseTextureColor;

	public LeagueItemStot()
	{
	}

	private void Awake()
	{
		this._baseTextureColor = this._texture.color;
		base.gameObject.SetActive(false);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void Set(string itemId, bool opened, bool purchased)
	{
		base.gameObject.SetActive(true);
		if (opened || purchased)
		{
			this._texture.color = this._baseTextureColor;
			this._lockIndicator.gameObject.SetActive(false);
		}
		else
		{
			this._texture.color = Color.white;
			this._lockIndicator.gameObject.SetActive(true);
		}
		int? nullable = null;
		this._texture.mainTexture = ItemDb.GetItemIcon(itemId, ShopNGUIController.CategoryNames.HatsCategory, nullable);
	}
}