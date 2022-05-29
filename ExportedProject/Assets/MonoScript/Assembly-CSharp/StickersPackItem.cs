using System;
using UnityEngine;

public class StickersPackItem : MonoBehaviour
{
	public TypePackSticker typePack;

	public UILabel priceLabel;

	public GameObject btnForBuyPack;

	public GameObject btnAvaliablePack;

	private BuySmileBannerController buyPackController;

	public string KeyForBuy
	{
		get
		{
			return StickersController.KeyForBuyPack(this.typePack);
		}
	}

	public StickersPackItem()
	{
	}

	public void CheckStateBtn()
	{
		if (!StickersController.IsBuyPack(this.typePack))
		{
			if (this.btnForBuyPack)
			{
				this.btnForBuyPack.SetActive(true);
			}
			if (this.btnAvaliablePack)
			{
				this.btnAvaliablePack.SetActive(false);
			}
		}
		else
		{
			if (this.btnForBuyPack)
			{
				this.btnForBuyPack.SetActive(false);
			}
			if (this.btnAvaliablePack)
			{
				this.btnAvaliablePack.SetActive(true);
			}
		}
	}

	public void OnBuy()
	{
		this.CheckStateBtn();
		StickersController.EventPackBuy();
	}

	private void OnEnable()
	{
		this.CheckStateBtn();
	}

	private void Start()
	{
		if (this.priceLabel)
		{
			UILabel str = this.priceLabel;
			int price = StickersController.GetPricePack(this.typePack).Price;
			str.text = price.ToString();
		}
		this.buyPackController = base.GetComponentInParent<BuySmileBannerController>();
	}

	public void TryBuyPack()
	{
		if (this.buyPackController != null)
		{
			ButtonClickSound.Instance.PlayClick();
			this.buyPackController.BuyStickersPack(this);
		}
	}
}