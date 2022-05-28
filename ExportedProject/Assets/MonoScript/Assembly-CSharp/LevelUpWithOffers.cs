using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using I2.Loc;
using Rilisoft;
using UnityEngine;

public class LevelUpWithOffers : MonoBehaviour
{
	public struct ItemDesc
	{
		public string tag;

		public ShopNGUIController.CategoryNames category;
	}

	[CompilerGenerated]
	private sealed class _003CAwake_003Ec__AnonStorey27F
	{
		internal FacebookController.StoryPriority levelupPriority;

		internal void _003C_003Em__195()
		{
			FacebookController.PostOpenGraphStory("reach", "level", levelupPriority, new Dictionary<string, string> { 
			{
				"level",
				ExperienceController.sharedController.currentLevel.ToString()
			} });
		}
	}

	[CompilerGenerated]
	private sealed class _003CAwake_003Ec__AnonStorey280
	{
		internal FacebookController.StoryPriority tierupPriority;

		internal void _003C_003Em__197()
		{
			FacebookController.PostOpenGraphStory("unlock", "new weapon", tierupPriority, new Dictionary<string, string> { 
			{
				"new weapon",
				(ExpController.Instance.OurTier + 1).ToString()
			} });
		}
	}

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

	[CompilerGenerated]
	private static Func<string> _003C_003Ef__am_0024cacheC;

	[CompilerGenerated]
	private static Func<string> _003C_003Ef__am_0024cacheD;

	private IEnumerator UpdatePanelsAndAnchors()
	{
		yield return new WaitForEndOfFrame();
		GameObject obj = base.transform.parent.parent.parent.gameObject;
		if (_003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Ef__am_0024cache3 == null)
		{
			_003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Ef__am_0024cache3 = _003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Em__199;
		}
		Player_move_c.PerformActionRecurs(obj, _003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Ef__am_0024cache3);
		GameObject obj2 = base.transform.parent.parent.parent.gameObject;
		if (_003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Ef__am_0024cache4 == null)
		{
			_003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Ef__am_0024cache4 = _003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Em__19A;
		}
		Player_move_c.PerformActionRecurs(obj2, _003CUpdatePanelsAndAnchors_003Ec__IteratorA6._003C_003Ef__am_0024cache4);
	}

	private void Awake()
	{
		if (!isTierLevelUp)
		{
			_003CAwake_003Ec__AnonStorey27F _003CAwake_003Ec__AnonStorey27F = new _003CAwake_003Ec__AnonStorey27F();
			_003CAwake_003Ec__AnonStorey27F.levelupPriority = FacebookController.StoryPriority.Red;
			shareScript.priority = _003CAwake_003Ec__AnonStorey27F.levelupPriority;
			shareScript.shareAction = _003CAwake_003Ec__AnonStorey27F._003C_003Em__195;
			shareScript.HasReward = true;
			RewardWindowBase rewardWindowBase = shareScript;
			if (_003C_003Ef__am_0024cacheC == null)
			{
				_003C_003Ef__am_0024cacheC = _003CAwake_003Em__196;
			}
			rewardWindowBase.twitterStatus = _003C_003Ef__am_0024cacheC;
			shareScript.EventTitle = "Level-up";
		}
		else
		{
			_003CAwake_003Ec__AnonStorey280 _003CAwake_003Ec__AnonStorey = new _003CAwake_003Ec__AnonStorey280();
			_003CAwake_003Ec__AnonStorey.tierupPriority = FacebookController.StoryPriority.Green;
			shareScript.priority = _003CAwake_003Ec__AnonStorey.tierupPriority;
			shareScript.shareAction = _003CAwake_003Ec__AnonStorey._003C_003Em__197;
			shareScript.HasReward = true;
			RewardWindowBase rewardWindowBase2 = shareScript;
			if (_003C_003Ef__am_0024cacheD == null)
			{
				_003C_003Ef__am_0024cacheD = _003CAwake_003Em__198;
			}
			rewardWindowBase2.twitterStatus = _003C_003Ef__am_0024cacheD;
			shareScript.EventTitle = "Tier-up";
		}
	}

	[ContextMenu("Update")]
	public void OnEnable()
	{
		StartCoroutine(UpdatePanelsAndAnchors());
	}

	private void OnDisable()
	{
		ShowIndicationMoney();
	}

	private void OnDestroy()
	{
		ShowIndicationMoney();
	}

	private void ShowIndicationMoney()
	{
		BankController.canShowIndication = true;
		BankController.UpdateAllIndicatorsMoney();
	}

	public void SetCurrentRank(string currentRank)
	{
		for (int i = 0; i < currentRankLabel.Length; i++)
		{
			currentRankLabel[i].text = LocalizationStore.Get("Key_0226").ToUpper() + " " + currentRank + "!";
		}
		string text = string.Empty;
		switch (ProfileController.CurOrderCup)
		{
		case 0:
			text = ScriptLocalization.Get("Key_1938");
			break;
		case 1:
			text = ScriptLocalization.Get("Key_1939");
			break;
		case 2:
			text = ScriptLocalization.Get("Key_1940");
			break;
		case 3:
			text = ScriptLocalization.Get("Key_1941");
			break;
		case 4:
			text = ScriptLocalization.Get("Key_1942");
			break;
		case 5:
			text = ScriptLocalization.Get("Key_1943");
			break;
		}
		foreach (UILabel youReachedLabel in youReachedLabels)
		{
			youReachedLabel.text = text;
		}
	}

	public void SetRewardPrice(string rewardPrice)
	{
		for (int i = 0; i < rewardPriceLabel.Length; i++)
		{
			rewardPriceLabel[i].text = rewardPrice;
		}
	}

	public void SetGemsRewardPrice(string gemsReward)
	{
		for (int i = 0; i < rewardGemsPriceLabel.Length; i++)
		{
			rewardGemsPriceLabel[i].text = gemsReward;
		}
	}

	public void SetAddHealthCount(string count)
	{
		if (healthLabel != null)
		{
			for (int i = 0; i < healthLabel.Length; i++)
			{
				healthLabel[i].text = count;
			}
		}
	}

	private void SetGemsLabel(int value)
	{
		for (int i = 0; i < gemsStarterBank.Length; i++)
		{
			gemsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1531"), value);
		}
	}

	private void SetCoinsLabel(int value)
	{
		for (int i = 0; i < coinsStarterBank.Length; i++)
		{
			coinsStarterBank[i].text = string.Format(LocalizationStore.Get("Key_1530"), value);
		}
	}

	public IEnumerator GemsStarterAnimation()
	{
		float seconds = 0f;
		SetGemsLabel(0);
		while (seconds < 1f)
		{
			for (int i = 0; i < gemsStarterBank.Length; i++)
			{
				SetGemsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, gemsStarterBankValue, seconds)));
			}
			seconds += Time.deltaTime;
			yield return null;
		}
		SetGemsLabel(Mathf.RoundToInt(gemsStarterBankValue));
	}

	public IEnumerator CoinsStarterAnimation()
	{
		float seconds = 0f;
		SetCoinsLabel(0);
		while (seconds < 1f)
		{
			for (int i = 0; i < coinsStarterBank.Length; i++)
			{
				SetCoinsLabel(Mathf.RoundToInt(Mathf.Lerp(0f, coinsStarterBankValue, seconds)));
			}
			seconds += Time.deltaTime;
			yield return null;
		}
		SetCoinsLabel(Mathf.RoundToInt(coinsStarterBankValue));
	}

	public void SetStarterBankValues(int gemsReward, int coinsReward)
	{
		gemsStarterBankValue = gemsReward;
		coinsStarterBankValue = coinsReward;
		SetGemsLabel(0);
		SetCoinsLabel(0);
	}

	public void SetItems(List<string> itemTags)
	{
		if (items == null || items.Length == 0)
		{
			return;
		}
		for (int i = 0; i < items.Length; i++)
		{
			items[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < itemTags.Count; j++)
		{
			items[j].gameObject.SetActive(true);
			string text = itemTags[j];
			int itemCategory = ItemDb.GetItemCategory(text);
			items[j]._tag = text;
			items[j].category = (ShopNGUIController.CategoryNames)itemCategory;
			items[j].itemImage.mainTexture = ItemDb.GetItemIcon(text, (ShopNGUIController.CategoryNames)itemCategory, !isTierLevelUp);
			foreach (UILabel item in items[j].itemName)
			{
				item.text = ItemDb.GetItemName(text, (ShopNGUIController.CategoryNames)itemCategory);
			}
			items[j].GetComponent<UIButton>().isEnabled = !Defs.isHunger || text == null || ItemDb.GetByTag(text) == null;
		}
	}

	public void Close()
	{
		ExpController.Instance.HandleContinueButton(base.gameObject);
	}

	[CompilerGenerated]
	private static string _003CAwake_003Em__196()
	{
		return string.Format("I've reached level {0} in @PixelGun3D! Come to the battle and try to defeat me! #pixelgun3d #pixelgun #3d #pg3d #fps http://goo.gl/8fzL9u", ExperienceController.sharedController.currentLevel);
	}

	[CompilerGenerated]
	private static string _003CAwake_003Em__198()
	{
		return "I've unlocked cool new weapons in @PixelGun3D! Let’s try them out! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
	}
}
