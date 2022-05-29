using Rilisoft;
using System;
using UnityEngine;

public class DaysOfValorBanner : BannerWindow
{
	public UILabel buttonApplyLabel;

	public UIWidget multiplyerContainer;

	public UIWidget expContainer;

	public UIWidget coinsContainer;

	public UISprite expMultiplyerSprite;

	public UISprite moneyMultiplyerSprite;

	public DaysOfValorBanner()
	{
	}

	private string GetNameSpriteForMultyplayer(int multiplyer)
	{
		return string.Format("{0}x", multiplyer);
	}

	public void HideWindow()
	{
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
		this.UpdateShownCount();
	}

	public void OnClickApplyButton()
	{
		if (SceneLoader.ActiveSceneName.Equals("ConnectScene") || SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox"))
		{
			this.HideWindow();
		}
		else
		{
			this.HideWindow();
			MainMenuController mainMenuController = MainMenuController.sharedController;
			if (mainMenuController != null)
			{
				mainMenuController.OnClickMultiplyerButton();
			}
		}
	}

	private void OnEnable()
	{
		this.SetButtonApplyText();
		this.SetSettingMultiplyerContainer();
	}

	private void SetButtonApplyText()
	{
		if (SceneLoader.ActiveSceneName.Equals("ConnectScene") || SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox"))
		{
			this.buttonApplyLabel.text = LocalizationStore.Get("Key_0012");
		}
		else
		{
			this.buttonApplyLabel.text = LocalizationStore.Get("Key_0085");
		}
	}

	private void SetSettingMultiplyerContainer()
	{
		PromoActionsManager promoActionsManager = PromoActionsManager.sharedManager;
		if (promoActionsManager == null)
		{
			return;
		}
		Transform vector3 = this.expContainer.gameObject.transform;
		Transform transforms = this.coinsContainer.gameObject.transform;
		vector3.localPosition = Vector3.zero;
		transforms.localPosition = Vector3.zero;
		int num = this.expContainer.width / 2;
		int dayOfValorMultiplyerForExp = promoActionsManager.DayOfValorMultiplyerForExp;
		int dayOfValorMultiplyerForMoney = promoActionsManager.DayOfValorMultiplyerForMoney;
		if (dayOfValorMultiplyerForExp > 1 && dayOfValorMultiplyerForMoney > 1)
		{
			vector3.gameObject.SetActive(true);
			transforms.gameObject.SetActive(true);
			vector3.localPosition = new Vector3((float)(-num), 0f, 0f);
			transforms.localPosition = new Vector3((float)num, 0f, 0f);
			this.expMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForExp);
			this.moneyMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForMoney);
		}
		else if (dayOfValorMultiplyerForExp > 1)
		{
			vector3.gameObject.SetActive(true);
			transforms.gameObject.SetActive(false);
			this.expMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForExp);
		}
		else if (dayOfValorMultiplyerForMoney > 1)
		{
			vector3.gameObject.SetActive(false);
			transforms.gameObject.SetActive(true);
			this.moneyMultiplyerSprite.spriteName = this.GetNameSpriteForMultyplayer(dayOfValorMultiplyerForMoney);
		}
	}

	public override void Show()
	{
		base.Show();
		PlayerPrefs.SetString("LastTimeShowDaysOfValor", DateTime.UtcNow.ToString("s"));
		PlayerPrefs.Save();
	}

	private void UpdateShownCount()
	{
		int num = PlayerPrefs.GetInt("DaysOfValorShownCount", 1);
		PlayerPrefs.SetInt("DaysOfValorShownCount", num - 1);
		PlayerPrefs.Save();
	}
}