using I2.Loc;
using System;
using UnityEngine;

public class ChestBonusButtonView : MonoBehaviour
{
	public UILabel timeOrCountLabel;

	public UITexture itemTexture;

	private PurchaseEventArgs _purchaseInfo;

	public ChestBonusButtonView()
	{
	}

	private void Awake()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private void CheckBonusButtonUpdate()
	{
		bool flag = (this._purchaseInfo == null ? false : ChestBonusController.Get.IsBonusActiveForItem(this._purchaseInfo));
		base.gameObject.SetActive(flag);
		if (flag)
		{
			this.SetViewData(this._purchaseInfo);
		}
	}

	public void Deinitialize()
	{
		ChestBonusController.OnChestBonusChange -= new ChestBonusController.OnChestBonusEnabledDelegate(this.CheckBonusButtonUpdate);
		this._purchaseInfo = null;
	}

	private void HandleLocalizationChanged()
	{
		this.CheckBonusButtonUpdate();
	}

	public void Initialize()
	{
		ChestBonusController.OnChestBonusChange += new ChestBonusController.OnChestBonusEnabledDelegate(this.CheckBonusButtonUpdate);
	}

	public void OnButtonClick()
	{
		ChestBonusController.Get.ShowBonusWindowForItem(this._purchaseInfo);
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	private void SetViewData(PurchaseEventArgs purchaseInfo)
	{
		ChestBonusData bonusData = ChestBonusController.Get.GetBonusData(purchaseInfo);
		this.timeOrCountLabel.text = bonusData.GetItemCountOrTime();
		this.itemTexture.mainTexture = bonusData.GetImage();
	}

	public void UpdateState(PurchaseEventArgs purchaseInfo)
	{
		this._purchaseInfo = purchaseInfo;
		this.CheckBonusButtonUpdate();
	}
}