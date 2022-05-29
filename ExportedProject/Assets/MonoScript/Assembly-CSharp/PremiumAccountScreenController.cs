using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PremiumAccountScreenController : MonoBehaviour
{
	public GameObject tapToActivate;

	public GameObject window;

	public UIButton[] rentButtons;

	public List<UILabel> headerLabels;

	public static PremiumAccountScreenController Instance;

	private bool ranksBefore;

	public string Header
	{
		get;
		set;
	}

	private bool InitialFreeAvailable
	{
		get
		{
			return Storager.getInt("PremiumInitialFree1Day", false) == 0;
		}
	}

	public PremiumAccountScreenController()
	{
	}

	public void HandleRentButtonPressed(UIButton button)
	{
		PremiumAccountController.AccountType accountType = (PremiumAccountController.AccountType)Array.IndexOf<UIButton>(this.rentButtons, button);
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(accountType.ToString());
		Action<PremiumAccountController.AccountType> instance = (PremiumAccountController.AccountType at) => {
			if (PremiumAccountController.Instance != null)
			{
				PremiumAccountController.Instance.BuyAccount(at);
			}
			this.UpdateFreeButtons();
		};
		if (!this.InitialFreeAvailable || accountType != PremiumAccountController.AccountType.OneDay)
		{
			int price = itemPrice.Price;
			string currency = itemPrice.Currency;
			ShopNGUIController.TryToBuy(this.window, itemPrice, () => {
				instance(accountType);
				this.LogBuyAccount(accountType.ToString());
				AnalyticsStuff.LogSales(accountType.ToString(), "Premium Account", false);
				AnalyticsFacade.InAppPurchase(accountType.ToString(), "Premium Account", 1, price, currency);
				if (this.InitialFreeAvailable)
				{
					this.SetInitialFreeUsed();
					instance(0);
				}
				this.Hide();
			}, null, null, null, null, null);
		}
		else
		{
			this.SetInitialFreeUsed();
			this.LogBuyAccount("FreeDay");
			instance(accountType);
			this.Hide();
		}
	}

	public void Hide()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void LogBuyAccount(string context)
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "Purchases", context }
		};
		if (ExperienceController.sharedController != null)
		{
			strs.Add("Level", ExperienceController.sharedController.currentLevel.ToString());
		}
		if (ExpController.Instance != null)
		{
			strs.Add("Tier", ExpController.Instance.OurTier.ToString());
		}
		FlurryPluginWrapper.LogEventAndDublicateToConsole(string.Concat("Premium Account (", (!FlurryPluginWrapper.IsPayingUser() ? "Non paying" : "Paying"), ")"), strs, true);
	}

	private void OnDestroy()
	{
		PremiumAccountScreenController.Instance = null;
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = this.ranksBefore;
		}
	}

	private void SetInitialFreeUsed()
	{
		Storager.setInt("PremiumInitialFree1Day", 1, false);
	}

	private void Start()
	{
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene) && ExperienceController.sharedController != null)
		{
			this.ranksBefore = ExperienceController.sharedController.isShowRanks;
			ExperienceController.sharedController.isShowRanks = false;
		}
		this.UpdateFreeButtons();
		for (int i = 0; i < (int)this.rentButtons.Length; i++)
		{
			IEnumerator enumerator = this.rentButtons[i].transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform current = (Transform)enumerator.Current;
					if (!current.name.Equals("GemsIcon"))
					{
						continue;
					}
					ItemPrice itemPrice = VirtualCurrencyHelper.Price(i.ToString());
					UILabel component = current.GetChild(0).GetComponent<UILabel>();
					component.text = itemPrice.Price.ToString();
					break;
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}
		PremiumAccountScreenController.Instance = this;
	}

	private void UpdateFreeButtons()
	{
		bool initialFreeAvailable = this.InitialFreeAvailable;
		IEnumerator enumerator = this.rentButtons[0].transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform current = (Transform)enumerator.Current;
				if (current.name.Equals("Free"))
				{
					current.gameObject.SetActive(initialFreeAvailable);
				}
				if (!current.name.Equals("GemsIcon"))
				{
					continue;
				}
				current.gameObject.SetActive(!initialFreeAvailable);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		this.tapToActivate.SetActive(initialFreeAvailable);
	}
}