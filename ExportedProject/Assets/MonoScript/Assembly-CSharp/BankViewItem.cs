using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BankViewItem : MonoBehaviour, IComparable<BankViewItem>
{
	public List<UILabel> inappNameLabels;

	public List<UILabel> countLabelsList;

	public List<UILabel> countLabelsX3List;

	public UILabel countLabel;

	public UILabel countX3Label;

	public UITexture icon;

	public UILabel priceLabel;

	public UILabel priceLabelBestBuy;

	public UISprite discountSprite;

	public UILabel discountPercentsLabel;

	public UIButton btnBuy;

	[NonSerialized]
	public PurchaseEventArgs purchaseInfo;

	public UISprite bestBuy;

	public ChestBonusButtonView bonusButtonView;

	private Animator _bestBuyAnimator;

	private Animator _discountAnimator;

	public int Count
	{
		set
		{
			if (this.countLabelsList != null && this.countLabelsList.Any<UILabel>())
			{
				foreach (UILabel str in this.countLabelsList)
				{
					str.text = value.ToString();
				}
			}
			else if (this.countLabel != null)
			{
				this.countLabel.text = value.ToString();
			}
		}
	}

	public int CountX3
	{
		set
		{
			if (this.countLabelsX3List != null && this.countLabelsX3List.Any<UILabel>())
			{
				foreach (UILabel str in this.countLabelsX3List)
				{
					str.text = value.ToString();
				}
			}
			else if (this.countX3Label != null)
			{
				this.countX3Label.text = value.ToString();
			}
		}
	}

	public string Price
	{
		set
		{
			if (this.priceLabel != null)
			{
				this.priceLabel.text = value ?? string.Empty;
			}
			if (this.priceLabelBestBuy != null)
			{
				this.priceLabelBestBuy.text = value ?? string.Empty;
			}
		}
	}

	public BankViewItem()
	{
	}

	private void Awake()
	{
		Animator component;
		Animator animator;
		if (this.bestBuy != null)
		{
			component = this.bestBuy.GetComponent<Animator>();
		}
		else
		{
			component = null;
		}
		this._bestBuyAnimator = component;
		if (this.discountSprite != null)
		{
			animator = this.discountSprite.GetComponent<Animator>();
		}
		else
		{
			animator = null;
		}
		this._discountAnimator = animator;
		if (this.bonusButtonView != null)
		{
			this.bonusButtonView.Initialize();
			if (this.purchaseInfo != null)
			{
				this.bonusButtonView.UpdateState(this.purchaseInfo);
			}
		}
		PromoActionsManager.BestBuyStateUpdate += new Action(this.UpdateViewBestBuy);
	}

	public int CompareTo(BankViewItem other)
	{
		int num = (other == null ? 0 : other.purchaseInfo.Count);
		return (!BankViewItem.PaymentOccursInLastTwoWeeks() ? this.purchaseInfo.Count.CompareTo(num) : num.CompareTo(this.purchaseInfo.Count));
	}

	private void OnDestroy()
	{
		if (this.bonusButtonView != null)
		{
			this.bonusButtonView.Deinitialize();
		}
		PromoActionsManager.BestBuyStateUpdate -= new Action(this.UpdateViewBestBuy);
	}

	private void OnEnable()
	{
		this.UpdateViewBestBuy();
	}

	private static bool PaymentOccursInLastTwoWeeks()
	{
		DateTime dateTime;
		string str = PlayerPrefs.GetString("Last Payment Time", string.Empty);
		if (string.IsNullOrEmpty(str) || !DateTime.TryParse(str, out dateTime))
		{
			return false;
		}
		TimeSpan utcNow = DateTime.UtcNow - dateTime;
		return utcNow <= TimeSpan.FromDays(14);
	}

	private void UpdateAnimationEventSprite(bool isEventActive)
	{
		PromoActionsManager promoActionsManager = PromoActionsManager.sharedManager;
		if (promoActionsManager != null && promoActionsManager.IsEventX3Active)
		{
			return;
		}
		bool flag = (this.discountSprite == null ? false : this.discountSprite.gameObject.activeSelf);
		if (flag && this._discountAnimator != null)
		{
			if (!isEventActive)
			{
				this._discountAnimator.Play("Idle");
			}
			else
			{
				this._discountAnimator.Play("DiscountAnimation");
			}
		}
		if (isEventActive && this._bestBuyAnimator != null)
		{
			if (!flag)
			{
				this._bestBuyAnimator.Play("Idle");
			}
			else
			{
				this._bestBuyAnimator.Play("BestBuyAnimation");
			}
		}
	}

	public void UpdateViewBestBuy()
	{
		PromoActionsManager promoActionsManager = PromoActionsManager.sharedManager;
		bool flag = (promoActionsManager != null ? promoActionsManager.IsBankItemBestBuy(this.purchaseInfo) : false);
		if (this.priceLabelBestBuy != null)
		{
			if (this.priceLabel != null)
			{
				this.priceLabel.transform.gameObject.SetActive(!flag);
			}
			if (this.priceLabelBestBuy != null)
			{
				this.bestBuy.gameObject.SetActive(flag);
			}
		}
		else
		{
			this.bestBuy.gameObject.SetActive(flag);
			this.UpdateAnimationEventSprite(flag);
		}
	}
}