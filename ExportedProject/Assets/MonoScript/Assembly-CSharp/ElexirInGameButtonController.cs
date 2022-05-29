using Rilisoft;
using System;
using System.Linq;
using UnityEngine;

public class ElexirInGameButtonController : MonoBehaviour
{
	public bool isActivePotion;

	public UIButton myButton;

	public UILabel myLabelTime;

	public UILabel myLabelCount;

	public int price = 10;

	public GameObject plusSprite;

	public GameObject myPotion;

	public GameObject priceLabel;

	public GameObject lockSprite;

	private bool isKnifeMap;

	public string idForPriceInDaterRegim;

	public ElexirInGameButtonController()
	{
	}

	private void Awake()
	{
		string str = (!Defs.isDaterRegim ? this.myPotion.name : this.idForPriceInDaterRegim);
		string str1 = this.myPotion.name;
		if (GearManager.Gear.Contains<string>(str))
		{
			str = GearManager.OneItemIDForGear(str, GearManager.CurrentNumberOfUphradesForGear(str));
		}
		this.isKnifeMap = SceneLoader.ActiveSceneName.Equals("Knife");
		if (Defs.isHunger || this.isKnifeMap)
		{
			this.myButton.disabledSprite = "game_clear";
			this.myButton.isEnabled = false;
			this.lockSprite.SetActive(true);
		}
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(str);
		if (str1 != null && str1.Equals(GearManager.Grenade))
		{
			itemPrice = new ItemPrice(itemPrice.Price * GearManager.ItemsInPackForGear(GearManager.Grenade), itemPrice.Currency);
		}
		this.priceLabel.GetComponent<UILabel>().text = itemPrice.Price.ToString();
		PotionsController.PotionDisactivated += new Action<string>(this.HandlePotionDisactivated);
	}

	private void HandlePotionDisactivated(string obj)
	{
		if (!obj.Equals(this.myPotion.name))
		{
			return;
		}
		this.myButton.isEnabled = true;
		this.myLabelTime.text = string.Empty;
		int num = Storager.getInt((!Defs.isDaterRegim ? obj : GearManager.HolderQuantityForID(this.idForPriceInDaterRegim)), false);
		this.myLabelTime.enabled = false;
		this.isActivePotion = false;
		this.myLabelTime.gameObject.SetActive(base.gameObject.activeSelf);
		if (num != 0)
		{
			this.SetStateUse();
		}
		else
		{
			this.SetStateBuy();
		}
	}

	private void OnDestroy()
	{
		PotionsController.PotionDisactivated -= new Action<string>(this.HandlePotionDisactivated);
	}

	private void OnDisable()
	{
		if (!this.isActivePotion)
		{
			this.myLabelTime.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		int num = Storager.getInt((!Defs.isDaterRegim ? this.myPotion.name : GearManager.HolderQuantityForID(this.idForPriceInDaterRegim)), false);
		this.myLabelCount.text = num.ToString();
		this.myLabelTime.gameObject.SetActive(true);
		if (num != 0)
		{
			this.SetStateUse();
		}
		else if (!this.isActivePotion)
		{
			this.SetStateBuy();
		}
	}

	private void SetStateBuy()
	{
		this.myButton.normalSprite = "game_clear_yellow";
		this.myButton.pressedSprite = "game_clear_yellow_n";
		this.priceLabel.SetActive(true);
		this.myLabelCount.gameObject.SetActive(false);
		this.plusSprite.SetActive(true);
		this.myLabelTime.enabled = false;
	}

	private void SetStateUse()
	{
		this.myLabelCount.gameObject.SetActive(true);
		this.plusSprite.SetActive(false);
		this.myButton.normalSprite = "game_clear";
		this.myButton.pressedSprite = "game_clear_n";
		this.priceLabel.SetActive(false);
		if (!this.isActivePotion)
		{
			this.myLabelTime.enabled = false;
		}
	}

	private void Start()
	{
		this.OnEnable();
	}

	private void Update()
	{
	}
}