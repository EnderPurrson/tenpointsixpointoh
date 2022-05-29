using I2.Loc;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelUpWithOffers : MonoBehaviour
{
	public RewardWindowBase shareScript;

	public UILabel[] rewardGemsPriceLabel;

	public UILabel[] currentRankLabel;

	public UILabel[] rewardPriceLabel;

	public UILabel[] healthLabel;

	public UILabel[] gemsStarterBank;

	public UILabel[] coinsStarterBank;

	public List<UILabel> youReachedLabels;

	public NewAvailableItemInShop[] items;

	public bool isTierLevelUp;

	private float gemsStarterBankValue;

	private float coinsStarterBankValue;

	public LevelUpWithOffers()
	{
	}

	private void Awake()
	{
		if (this.isTierLevelUp)
		{
			FacebookController.StoryPriority storyPriority = FacebookController.StoryPriority.Green;
			this.shareScript.priority = storyPriority;
			this.shareScript.shareAction = () => FacebookController.PostOpenGraphStory("unlock", "new weapon", storyPriority, new Dictionary<string, string>()
			{
				{ "new weapon", (ExpController.Instance.OurTier + 1).ToString() }
			});
			this.shareScript.HasReward = true;
			this.shareScript.twitterStatus = () => "I've unlocked cool new weapons in @PixelGun3D! Let’s try them out! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
			this.shareScript.EventTitle = "Tier-up";
		}
		else
		{
			FacebookController.StoryPriority storyPriority1 = FacebookController.StoryPriority.Red;
			this.shareScript.priority = storyPriority1;
			this.shareScript.shareAction = () => FacebookController.PostOpenGraphStory("reach", "level", storyPriority1, new Dictionary<string, string>()
			{
				{ "level", ExperienceController.sharedController.currentLevel.ToString() }
			});
			this.shareScript.HasReward = true;
			this.shareScript.twitterStatus = () => string.Format("I've reached level {0} in @PixelGun3D! Come to the battle and try to defeat me! #pixelgun3d #pixelgun #3d #pg3d #fps http://goo.gl/8fzL9u", ExperienceController.sharedController.currentLevel);
			this.shareScript.EventTitle = "Level-up";
		}
	}

	public void Close()
	{
		ExpController.Instance.HandleContinueButton(base.gameObject);
	}

	[DebuggerHidden]
	public IEnumerator CoinsStarterAnimation()
	{
		LevelUpWithOffers.u003cCoinsStarterAnimationu003ec__IteratorA8 variable = null;
		return variable;
	}

	[DebuggerHidden]
	public IEnumerator GemsStarterAnimation()
	{
		LevelUpWithOffers.u003cGemsStarterAnimationu003ec__IteratorA7 variable = null;
		return variable;
	}

	private void OnDestroy()
	{
		this.ShowIndicationMoney();
	}

	private void OnDisable()
	{
		this.ShowIndicationMoney();
	}

	[ContextMenu("Update")]
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdatePanelsAndAnchors());
	}

	public void SetAddHealthCount(string count)
	{
		if (this.healthLabel != null)
		{
			for (int i = 0; i < (int)this.healthLabel.Length; i++)
			{
				this.healthLabel[i].text = count;
			}
		}
	}

	private void SetCoinsLabel(int value)
	{
		for (int i = 0; i < (int)this.coinsStarterBank.Length; i++)
		{
			this.coinsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1530"), value);
		}
	}

	public void SetCurrentRank(string currentRank)
	{
		for (int i = 0; i < (int)this.currentRankLabel.Length; i++)
		{
			this.currentRankLabel[i].text = string.Concat(LocalizationStore.Get("Key_0226").ToUpper(), " ", currentRank, "!");
		}
		string empty = string.Empty;
		switch (ProfileController.CurOrderCup)
		{
			case 0:
			{
				empty = ScriptLocalization.Get("Key_1938");
				break;
			}
			case 1:
			{
				empty = ScriptLocalization.Get("Key_1939");
				break;
			}
			case 2:
			{
				empty = ScriptLocalization.Get("Key_1940");
				break;
			}
			case 3:
			{
				empty = ScriptLocalization.Get("Key_1941");
				break;
			}
			case 4:
			{
				empty = ScriptLocalization.Get("Key_1942");
				break;
			}
			case 5:
			{
				empty = ScriptLocalization.Get("Key_1943");
				break;
			}
		}
		foreach (UILabel youReachedLabel in this.youReachedLabels)
		{
			youReachedLabel.text = empty;
		}
	}

	private void SetGemsLabel(int value)
	{
		for (int i = 0; i < (int)this.gemsStarterBank.Length; i++)
		{
			this.gemsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1531"), value);
		}
	}

	public void SetGemsRewardPrice(string gemsReward)
	{
		for (int i = 0; i < (int)this.rewardGemsPriceLabel.Length; i++)
		{
			this.rewardGemsPriceLabel[i].text = gemsReward;
		}
	}

	public void SetItems(List<string> itemTags)
	{
		if (this.items == null || (int)this.items.Length == 0)
		{
			return;
		}
		for (int i = 0; i < (int)this.items.Length; i++)
		{
			this.items[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < itemTags.Count; j++)
		{
			this.items[j].gameObject.SetActive(true);
			string item = itemTags[j];
			int itemCategory = ItemDb.GetItemCategory(item);
			this.items[j]._tag = item;
			this.items[j].category = (ShopNGUIController.CategoryNames)itemCategory;
			this.items[j].itemImage.mainTexture = ItemDb.GetItemIcon(item, (ShopNGUIController.CategoryNames)itemCategory, !this.isTierLevelUp);
			foreach (UILabel itemName in this.items[j].itemName)
			{
				itemName.text = ItemDb.GetItemName(item, (ShopNGUIController.CategoryNames)itemCategory);
			}
			this.items[j].GetComponent<UIButton>().isEnabled = (!Defs.isHunger || item == null ? 0 : (int)(ItemDb.GetByTag(item) != null)) == 0;
		}
	}

	public void SetRewardPrice(string rewardPrice)
	{
		for (int i = 0; i < (int)this.rewardPriceLabel.Length; i++)
		{
			this.rewardPriceLabel[i].text = rewardPrice;
		}
	}

	public void SetStarterBankValues(int gemsReward, int coinsReward)
	{
		this.gemsStarterBankValue = (float)gemsReward;
		this.coinsStarterBankValue = (float)coinsReward;
		this.SetGemsLabel(0);
		this.SetCoinsLabel(0);
	}

	private void ShowIndicationMoney()
	{
		BankController.canShowIndication = true;
		BankController.UpdateAllIndicatorsMoney();
	}

	[DebuggerHidden]
	private IEnumerator UpdatePanelsAndAnchors()
	{
		LevelUpWithOffers.u003cUpdatePanelsAndAnchorsu003ec__IteratorA6 variable = null;
		return variable;
	}

	public struct ItemDesc
	{
		public string tag;

		public ShopNGUIController.CategoryNames category;
	}
}