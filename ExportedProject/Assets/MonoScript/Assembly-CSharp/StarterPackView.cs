using System;
using System.Collections.Generic;
using UnityEngine;

public class StarterPackView : BannerWindow
{
	public UILabel[] timerEvent;

	public StarterPackItem[] items;

	public UILabel buttonLabel;

	public UILabel[] title;

	public UIWidget backgroundItems;

	public UIWidget backgroundMoney;

	public UIWidget itemsCentralPanel;

	public UIWidget moneyCentralPanel;

	public UITexture moneyLeftSprite;

	public UITexture moneyRightSprite;

	public UILabel[] moneyCountLabel;

	public UILabel[] moneySaleLabel;

	public UILabel[] moneySaveSaleLabel;

	public UIWidget buttonMoneyContainer;

	public UILabel buttonMoneyDescription;

	public UISprite buttonMoneyIcon;

	public UILabel buttonMoneyCount;

	public StarterPackView()
	{
	}

	private void CenterItems(int countHideElements)
	{
		if ((int)this.items.Length < 2 || countHideElements == 0)
		{
			return;
		}
		float single = (float)countHideElements / 2f;
		float single1 = this.items[1].transform.localPosition.x;
		Vector3 vector3 = this.items[0].transform.localPosition;
		float single2 = (single1 - vector3.x) * single;
		int length = (int)this.items.Length - countHideElements;
		for (int i = 0; i < length; i++)
		{
			Vector3 vector31 = this.items[i].transform.localPosition;
			this.items[i].transform.localPosition = new Vector3(vector31.x + single2, vector31.y, vector31.z);
		}
	}

	public void HideWindow()
	{
		StarterPackController.Get.CheckSendEventChangeEnabled();
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController != null)
		{
			sharedController.HideBannerWindow();
			return;
		}
		base.Hide();
	}

	public void OnButtonBuyClick()
	{
		StarterPackController get = StarterPackController.Get;
		if (get == null)
		{
			return;
		}
		if (!get.IsPackSellForGameMoney())
		{
			get.CheckBuyRealMoney();
			this.HideWindow();
		}
		else
		{
			get.CheckBuyPackForGameMoney(this);
		}
	}

	private void SetButtonText()
	{
		if (!StarterPackController.Get.IsPackSellForGameMoney())
		{
			this.buttonMoneyContainer.gameObject.SetActive(false);
			this.buttonLabel.gameObject.SetActive(true);
			string priceLabelForCurrentPack = StarterPackController.Get.GetPriceLabelForCurrentPack();
			this.buttonLabel.text = string.Format("{0} {1}", LocalizationStore.Get("Key_1043"), priceLabelForCurrentPack);
		}
		else
		{
			ItemPrice priceDataForItemsPack = StarterPackController.Get.GetPriceDataForItemsPack();
			if (priceDataForItemsPack == null)
			{
				return;
			}
			this.buttonMoneyContainer.gameObject.SetActive(true);
			this.buttonLabel.gameObject.SetActive(false);
			this.buttonMoneyDescription.text = LocalizationStore.Get("Key_1043");
			this.buttonMoneyIcon.spriteName = (priceDataForItemsPack.Currency != "GemsCurrency" ? "coin_znachek" : "gem_znachek");
			this.buttonMoneyIcon.MakePixelPerfect();
			this.buttonMoneyCount.text = priceDataForItemsPack.Price.ToString();
		}
	}

	private void SetCountMoneyLabel(int count, bool isCoins)
	{
		string empty = string.Empty;
		empty = (!isCoins ? LocalizationStore.Get("Key_0771") : LocalizationStore.Get("Key_0936"));
		for (int i = 0; i < (int)this.moneyCountLabel.Length; i++)
		{
			this.moneyCountLabel[i].text = string.Format("{0}\n{1}", count, empty);
		}
	}

	private void SetMoneyData(bool isCoins)
	{
		StarterPackData currentPackData = StarterPackController.Get.GetCurrentPackData();
		string empty = string.Empty;
		int num = 0;
		if (!isCoins)
		{
			empty = "Textures/Bank/Coins_Shop_Gem_5";
			num = currentPackData.gemsCount;
		}
		else
		{
			empty = "Textures/Bank/Coins_Shop_5";
			num = currentPackData.coinsCount;
		}
		Texture texture = Resources.Load<Texture>(empty);
		this.moneyLeftSprite.mainTexture = texture;
		this.moneyRightSprite.mainTexture = texture;
		this.SetCountMoneyLabel(num, isCoins);
		this.SetSaleLabel(currentPackData.sale);
	}

	private void SetSaleLabel(int sale)
	{
		for (int i = 0; i < (int)this.moneySaleLabel.Length; i++)
		{
			this.moneySaleLabel[i].text = string.Format("{0}% {1}", sale, LocalizationStore.Get("Key_0276"));
		}
	}

	private void SetSaleSaveText(string text)
	{
		for (int i = 0; i < (int)this.moneySaveSaleLabel.Length; i++)
		{
			this.moneySaveSaleLabel[i].text = text;
		}
	}

	private void SetTitleText(string text)
	{
		for (int i = 0; i < (int)this.title.Length; i++)
		{
			this.title[i].text = text;
		}
	}

	private void SetupItemsData()
	{
		if ((int)this.items.Length == 0)
		{
			return;
		}
		StarterPackData currentPackData = StarterPackController.Get.GetCurrentPackData();
		if (currentPackData == null)
		{
			return;
		}
		if (currentPackData.items == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < (int)this.items.Length; i++)
		{
			if (i < currentPackData.items.Count)
			{
				this.items[i].SetData(currentPackData.items[i]);
			}
			else
			{
				this.items[i].gameObject.SetActive(false);
				num++;
			}
		}
		this.CenterItems(num);
	}

	public override void Show()
	{
		base.Show();
		StarterPackController.Get.CheckFindStoreKitEventListner();
		StarterPackController.Get.UpdateCountShownWindowByShowCondition();
		this.ShowCustomInterface();
	}

	private void ShowCustomInterface()
	{
		StarterPackModel.TypePack currentPackType = StarterPackController.Get.GetCurrentPackType();
		bool flag = currentPackType == StarterPackModel.TypePack.Items;
		bool flag1 = currentPackType == StarterPackModel.TypePack.Coins;
		this.itemsCentralPanel.gameObject.SetActive(flag);
		this.backgroundItems.gameObject.SetActive(flag);
		this.moneyCentralPanel.gameObject.SetActive(!flag);
		this.backgroundMoney.gameObject.SetActive(!flag);
		if (!flag)
		{
			this.SetMoneyData(flag1);
		}
		else
		{
			this.SetupItemsData();
		}
		this.SetSaleSaveText(StarterPackController.Get.GetSavingMoneyByCarrentPack());
		this.SetTitleText(StarterPackController.Get.GetCurrentPackName());
		this.SetButtonText();
	}

	private void Update()
	{
		string timeToEndEvent = StarterPackController.Get.GetTimeToEndEvent();
		for (int i = 0; i < (int)this.timerEvent.Length; i++)
		{
			this.timerEvent[i].text = timeToEndEvent;
		}
	}
}