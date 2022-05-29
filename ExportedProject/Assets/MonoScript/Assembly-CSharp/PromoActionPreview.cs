using I2.Loc;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PromoActionPreview : MonoBehaviour
{
	public UIButton button;

	public UITexture stickerTexture;

	public GameObject stickersLabel;

	public UISprite currencyImage;

	public string tg;

	public UITexture icon;

	public UILabel topSeller;

	public UILabel newItem;

	public UILabel sale;

	public UILabel coins;

	public Texture unpressed;

	public Texture pressed;

	public int Discount
	{
		get;
		set;
	}

	public PromoActionPreview()
	{
	}

	private void HandleLocalizationChanged()
	{
		this.SetSaleText();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private void OnEnable()
	{
		UIButton[] componentsInChildren = base.GetComponentsInChildren<UIButton>(true);
		for (int i = 0; i < (int)componentsInChildren.Length; i++)
		{
			componentsInChildren[i].isEnabled = TrainingController.TrainingCompleted;
		}
		this.SetSaleText();
	}

	private void SetSaleText()
	{
		if (this.Discount > 0 && this.sale != null)
		{
			this.sale.text = string.Format("{0}\n{1}%", LocalizationStore.Key_0419, this.Discount);
		}
	}

	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}
}