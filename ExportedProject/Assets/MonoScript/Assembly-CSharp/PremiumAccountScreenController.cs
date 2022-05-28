using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class PremiumAccountScreenController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHandleRentButtonPressed_003Ec__AnonStorey290
	{
		internal Action<PremiumAccountController.AccountType> provideAcc;

		internal PremiumAccountController.AccountType accType;

		internal PremiumAccountScreenController _003C_003Ef__this;

		internal void _003C_003Em__216(PremiumAccountController.AccountType at)
		{
			if (PremiumAccountController.Instance != null)
			{
				PremiumAccountController.Instance.BuyAccount(at);
			}
			_003C_003Ef__this.UpdateFreeButtons();
		}
	}

	[CompilerGenerated]
	private sealed class _003CHandleRentButtonPressed_003Ec__AnonStorey291
	{
		internal int priceAmount;

		internal string priceCurrency;

		internal _003CHandleRentButtonPressed_003Ec__AnonStorey290 _003C_003Ef__ref_0024656;

		internal PremiumAccountScreenController _003C_003Ef__this;

		internal void _003C_003Em__217()
		{
			_003C_003Ef__ref_0024656.provideAcc(_003C_003Ef__ref_0024656.accType);
			_003C_003Ef__this.LogBuyAccount(_003C_003Ef__ref_0024656.accType.ToString());
			AnalyticsStuff.LogSales(_003C_003Ef__ref_0024656.accType.ToString(), "Premium Account");
			AnalyticsFacade.InAppPurchase(_003C_003Ef__ref_0024656.accType.ToString(), "Premium Account", 1, priceAmount, priceCurrency);
			if (_003C_003Ef__this.InitialFreeAvailable)
			{
				_003C_003Ef__this.SetInitialFreeUsed();
				_003C_003Ef__ref_0024656.provideAcc(PremiumAccountController.AccountType.OneDay);
			}
			_003C_003Ef__this.Hide();
		}
	}

	public GameObject tapToActivate;

	public GameObject window;

	public UIButton[] rentButtons;

	public List<UILabel> headerLabels;

	public static PremiumAccountScreenController Instance;

	private bool ranksBefore;

	public string Header { get; set; }

	private bool InitialFreeAvailable
	{
		get
		{
			return Storager.getInt("PremiumInitialFree1Day", false) == 0;
		}
	}

	private void Start()
	{
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && ExperienceController.sharedController != null)
		{
			ranksBefore = ExperienceController.sharedController.isShowRanks;
			ExperienceController.sharedController.isShowRanks = false;
		}
		UpdateFreeButtons();
		for (int i = 0; i < rentButtons.Length; i++)
		{
			foreach (Transform item in rentButtons[i].transform)
			{
				if (item.name.Equals("GemsIcon"))
				{
					PremiumAccountController.AccountType accountType = (PremiumAccountController.AccountType)i;
					string key = accountType.ToString();
					ItemPrice itemPrice = VirtualCurrencyHelper.Price(key);
					UILabel component = item.GetChild(0).GetComponent<UILabel>();
					component.text = itemPrice.Price.ToString();
					break;
				}
			}
		}
		Instance = this;
	}

	private void LogBuyAccount(string context)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("Purchases", context);
		Dictionary<string, string> dictionary2 = dictionary;
		if (ExperienceController.sharedController != null)
		{
			dictionary2.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
		}
		if (ExpController.Instance != null)
		{
			dictionary2.Add("Tier", ExpController.Instance.OurTier.ToString());
		}
		FlurryPluginWrapper.LogEventAndDublicateToConsole("Premium Account (" + ((!FlurryPluginWrapper.IsPayingUser()) ? "Non paying" : "Paying") + ")", dictionary2);
	}

	public void HandleRentButtonPressed(UIButton button)
	{
		_003CHandleRentButtonPressed_003Ec__AnonStorey290 _003CHandleRentButtonPressed_003Ec__AnonStorey = new _003CHandleRentButtonPressed_003Ec__AnonStorey290();
		_003CHandleRentButtonPressed_003Ec__AnonStorey._003C_003Ef__this = this;
		_003CHandleRentButtonPressed_003Ec__AnonStorey.accType = (PremiumAccountController.AccountType)Array.IndexOf(rentButtons, button);
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(_003CHandleRentButtonPressed_003Ec__AnonStorey.accType.ToString());
		_003CHandleRentButtonPressed_003Ec__AnonStorey.provideAcc = _003CHandleRentButtonPressed_003Ec__AnonStorey._003C_003Em__216;
		if (InitialFreeAvailable && _003CHandleRentButtonPressed_003Ec__AnonStorey.accType == PremiumAccountController.AccountType.OneDay)
		{
			SetInitialFreeUsed();
			LogBuyAccount("FreeDay");
			_003CHandleRentButtonPressed_003Ec__AnonStorey.provideAcc(_003CHandleRentButtonPressed_003Ec__AnonStorey.accType);
			Hide();
		}
		else
		{
			_003CHandleRentButtonPressed_003Ec__AnonStorey291 _003CHandleRentButtonPressed_003Ec__AnonStorey2 = new _003CHandleRentButtonPressed_003Ec__AnonStorey291();
			_003CHandleRentButtonPressed_003Ec__AnonStorey2._003C_003Ef__ref_0024656 = _003CHandleRentButtonPressed_003Ec__AnonStorey;
			_003CHandleRentButtonPressed_003Ec__AnonStorey2._003C_003Ef__this = this;
			_003CHandleRentButtonPressed_003Ec__AnonStorey2.priceAmount = itemPrice.Price;
			_003CHandleRentButtonPressed_003Ec__AnonStorey2.priceCurrency = itemPrice.Currency;
			ShopNGUIController.TryToBuy(window, itemPrice, _003CHandleRentButtonPressed_003Ec__AnonStorey2._003C_003Em__217);
		}
	}

	public void Hide()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void UpdateFreeButtons()
	{
		bool initialFreeAvailable = InitialFreeAvailable;
		foreach (Transform item in rentButtons[0].transform)
		{
			if (item.name.Equals("Free"))
			{
				item.gameObject.SetActive(initialFreeAvailable);
			}
			if (item.name.Equals("GemsIcon"))
			{
				item.gameObject.SetActive(!initialFreeAvailable);
			}
		}
		tapToActivate.SetActive(initialFreeAvailable);
	}

	private void SetInitialFreeUsed()
	{
		Storager.setInt("PremiumInitialFree1Day", 1, false);
	}

	private void OnDestroy()
	{
		Instance = null;
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = ranksBefore;
		}
	}
}
