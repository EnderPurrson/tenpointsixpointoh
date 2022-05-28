using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class PromoActionsGUIController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CFilterForLoadings_003Ec__AnonStorey292
	{
		internal float initialDPS;

		internal bool _003C_003Em__21B(GameObject obj)
		{
			float num = 1f;
			num = obj.GetComponent<WeaponSounds>().DamageByTier[obj.GetComponent<WeaponSounds>().tier] / obj.GetComponent<WeaponSounds>().lengthForShot;
			return num >= initialDPS;
		}
	}

	public const int ITEMS_COUNT_NEWS = 5;

	public const int ITEMS_COUNT_TOPSELLER = 2;

	public const int ITEMS_COUNT_DISCOUNT = 3;

	public const string StickersPromoActionsPanelKey = "StickersPromoActionsPanelKey";

	public UIWrapContent wrapContent;

	public UILabel noOffersLabel;

	public UILabel checkInternetLabel;

	public GameObject fonPromoPanel;

	private bool initiallyUpdated;

	private bool updateOnEnable;

	private int refreshPromoPanelCntr;

	[CompilerGenerated]
	private static Comparison<GameObject> _003C_003Ef__am_0024cache7;

	[CompilerGenerated]
	private static Func<UnityEngine.Object, WeaponSounds> _003C_003Ef__am_0024cache8;

	[CompilerGenerated]
	private static Func<WeaponSounds, string> _003C_003Ef__am_0024cache9;

	[CompilerGenerated]
	private static Func<WeaponSounds, WeaponSounds> _003C_003Ef__am_0024cacheA;

	[CompilerGenerated]
	private static Func<List<GameObject>, IEnumerable<string>> _003C_003Ef__am_0024cacheB;

	[CompilerGenerated]
	private static Func<List<string>, IEnumerable<string>> _003C_003Ef__am_0024cacheC;

	[CompilerGenerated]
	private static Func<GameObject, string> _003C_003Ef__am_0024cacheD;

	private IEnumerator Start()
	{
		PromoActionsManager.ActionsUUpdated += UpdateAfterDelayHandler;
		ShopNGUIController.GunBought += MarkUpdateOnEnable;
		WeaponManager.TryGunExpired += MarkUpdateOnEnable;
		StickersController.onBuyPack += MarkUpdateOnEnable;
		yield return null;
		initiallyUpdated = true;
	}

	private void OnEnable()
	{
		if (updateOnEnable || !initiallyUpdated)
		{
			StartCoroutine(UpdateAfterDelay());
		}
		updateOnEnable = false;
	}

	private void UpdateAfterDelayHandler()
	{
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(UpdateAfterDelay());
		}
	}

	private IEnumerator UpdateAfterDelay()
	{
		yield return null;
		HandleUpdated();
	}

	private void OnDestroy()
	{
		PromoActionsManager.ActionsUUpdated -= UpdateAfterDelayHandler;
		ShopNGUIController.GunBought -= MarkUpdateOnEnable;
		WeaponManager.TryGunExpired -= MarkUpdateOnEnable;
		StickersController.onBuyPack -= MarkUpdateOnEnable;
	}

	private void Update()
	{
		Transform transform = wrapContent.transform;
		if (transform.childCount <= 0)
		{
			return;
		}
		UIPanel component = transform.parent.GetComponent<UIPanel>();
		if (transform.childCount <= 3)
		{
			float num = 0f;
			foreach (Transform item in transform)
			{
				num += item.localPosition.x;
			}
			num /= (float)transform.childCount;
			if (component != null)
			{
				wrapContent.WrapContent();
				component.GetComponent<UIScrollView>().SetDragAmount(0.5f, 0f, false);
			}
		}
		if (refreshPromoPanelCntr % 10 == 0)
		{
			component.Refresh();
		}
		refreshPromoPanelCntr++;
	}

	public void MarkUpdateOnEnable()
	{
		updateOnEnable = true;
		if (base.gameObject.activeInHierarchy)
		{
			HandleUpdated();
		}
	}

	private void HandleUpdated()
	{
		StartCoroutine(HandleUpdateCoroutine());
	}

	public static string FilterForLoadings(string tg, List<string> alreadyUsed)
	{
		//Discarded unreachable code: IL_0056
		_003CFilterForLoadings_003Ec__AnonStorey292 _003CFilterForLoadings_003Ec__AnonStorey = new _003CFilterForLoadings_003Ec__AnonStorey292();
		if (tg == null || alreadyUsed == null)
		{
			return null;
		}
		string text = WeaponManager.FirstUnboughtTag(tg);
		string empty = string.Empty;
		try
		{
			empty = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text]];
		}
		catch (Exception ex)
		{
			Debug.Log("Exception in FilterForLoadings:  idefFirstUnobught = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnobught] ]:  " + ex);
			return null;
		}
		if (!Storager.hasKey(empty))
		{
			Storager.setInt(empty, 0, false);
		}
		bool flag = Storager.getInt(empty, true) > 0;
		WeaponSounds weaponSounds = null;
		UnityEngine.Object[] weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
		for (int i = 0; i < weaponsInGame.Length; i++)
		{
			GameObject gameObject = (GameObject)weaponsInGame[i];
			if (ItemDb.GetByPrefabName(gameObject.name).Tag.Equals(text))
			{
				weaponSounds = gameObject.GetComponent<WeaponSounds>();
				break;
			}
		}
		if (weaponSounds == null)
		{
			return null;
		}
		if (!flag && weaponSounds.tier <= ExpController.Instance.OurTier && !alreadyUsed.Contains(ItemDb.GetByPrefabName(weaponSounds.name.Replace("(Clone)", string.Empty)).Tag))
		{
			return text;
		}
		WeaponSounds weaponSounds2 = weaponSounds;
		if (!flag)
		{
			string text2 = WeaponManager.LastBoughtTag(text);
			if (text2 != null)
			{
				List<string> list = null;
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (upgrade.Contains(text2))
					{
						list = upgrade;
						break;
					}
				}
				for (int num = list.IndexOf(text2); num >= 0; num--)
				{
					bool flag2 = false;
					UnityEngine.Object[] weaponsInGame2 = WeaponManager.sharedManager.weaponsInGame;
					for (int j = 0; j < weaponsInGame2.Length; j++)
					{
						GameObject gameObject2 = (GameObject)weaponsInGame2[j];
						if (ItemDb.GetByPrefabName(gameObject2.name).Tag.Equals(list[num]))
						{
							WeaponSounds component = gameObject2.GetComponent<WeaponSounds>();
							if (component.tier <= ExpController.Instance.OurTier)
							{
								flag2 = true;
								weaponSounds2 = component;
								break;
							}
						}
					}
					if (flag2)
					{
						break;
					}
				}
			}
		}
		float num2 = 1f;
		if (weaponSounds2 != null)
		{
			num2 = weaponSounds2.DamageByTier[weaponSounds2.tier] / weaponSounds2.lengthForShot;
		}
		_003CFilterForLoadings_003Ec__AnonStorey.initialDPS = num2;
		if (flag && weaponSounds.tier > ExpController.Instance.OurTier && weaponSounds2 != null)
		{
			try
			{
				_003CFilterForLoadings_003Ec__AnonStorey.initialDPS = num2 * (weaponSounds2.DamageByTier[ExpController.Instance.OurTier] / weaponSounds2.DamageByTier[weaponSounds2.tier]);
			}
			catch (Exception ex2)
			{
				Debug.Log("Exception in FilterForLoadings:  if (bought && ws.tier > ExpController.Instance.OurTier && lastBoughtInOurTierWS != null):  " + ex2);
			}
		}
		List<string> list2 = new List<string>();
		list2.Add(tg);
		List<string> list3 = list2;
		foreach (List<string> upgrade2 in WeaponUpgrades.upgrades)
		{
			if (upgrade2.Contains(tg))
			{
				list3 = upgrade2;
				break;
			}
		}
		List<string> list4 = new List<string>();
		List<GameObject> list5 = new List<GameObject>();
		UnityEngine.Object[] weaponsInGame3 = WeaponManager.sharedManager.weaponsInGame;
		for (int k = 0; k < weaponsInGame3.Length; k++)
		{
			GameObject gameObject3 = (GameObject)weaponsInGame3[k];
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(gameObject3.name);
			if (list3.Contains(byPrefabName.Tag) || gameObject3.GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier || gameObject3.GetComponent<WeaponSounds>().campaignOnly || gameObject3.name.Equals(WeaponManager.AlienGunWN) || gameObject3.name.Equals(WeaponManager.BugGunWN) || gameObject3.name.Equals(WeaponManager.SimpleFlamethrower_WN) || gameObject3.name.Equals(WeaponManager.CampaignRifle_WN) || gameObject3.name.Equals(WeaponManager.Rocketnitza_WN) || gameObject3.name.Equals(WeaponManager.PistolWN) || gameObject3.name.Equals(WeaponManager.SocialGunWN) || gameObject3.name.Equals(WeaponManager.DaterFreeWeaponPrefabName) || gameObject3.name.Equals(WeaponManager.KnifeWN) || gameObject3.name.Equals(WeaponManager.ShotgunWN) || gameObject3.name.Equals(WeaponManager.MP5WN) || ItemDb.IsTemporaryGun(byPrefabName.Tag) || (byPrefabName.Tag != null && WeaponManager.GotchaGuns.Contains(byPrefabName.Tag)))
			{
				continue;
			}
			string text3 = WeaponManager.FirstUnboughtTag(byPrefabName.Tag);
			if (alreadyUsed.Contains(text3) || list4.Contains(text3))
			{
				continue;
			}
			bool flag3 = false;
			UnityEngine.Object[] weaponsInGame4 = WeaponManager.sharedManager.weaponsInGame;
			for (int l = 0; l < weaponsInGame4.Length; l++)
			{
				GameObject gameObject4 = (GameObject)weaponsInGame4[l];
				if (ItemDb.GetByPrefabName(gameObject4.name).Tag.Equals(text3))
				{
					flag3 = gameObject4.GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier;
					break;
				}
			}
			if (flag3)
			{
				continue;
			}
			string empty2 = string.Empty;
			try
			{
				empty2 = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[text3]];
				if (Storager.getInt(empty2, true) > 0)
				{
					continue;
				}
			}
			catch (Exception ex3)
			{
				Debug.Log("Exception in FilterForLoadings:  defFirstUnobughtOther = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnboughtOthers] ]:  " + ex3);
			}
			list4.Add(text3);
			UnityEngine.Object[] weaponsInGame5 = WeaponManager.sharedManager.weaponsInGame;
			for (int m = 0; m < weaponsInGame5.Length; m++)
			{
				GameObject gameObject5 = (GameObject)weaponsInGame5[m];
				if (ItemDb.GetByPrefabName(gameObject5.name).Tag.Equals(text3))
				{
					list5.Add(gameObject5);
					break;
				}
			}
		}
		if (_003C_003Ef__am_0024cache7 == null)
		{
			_003C_003Ef__am_0024cache7 = _003CFilterForLoadings_003Em__21A;
		}
		list5.Sort(_003C_003Ef__am_0024cache7);
		GameObject gameObject6 = list5.Find(_003CFilterForLoadings_003Ec__AnonStorey._003C_003Em__21B);
		if (gameObject6 == null)
		{
			gameObject6 = ((list5.Count <= 0) ? null : list5[list5.Count - 1]);
		}
		return (!(gameObject6 != null)) ? null : ItemDb.GetByPrefabName(gameObject6.name).Tag;
	}

	public static List<string> FilterPurchases(IEnumerable<string> input, bool filterNextTierUpgrades = false, bool filterWeapons = true, bool filterRentedTempWeapons = false, bool checkWear = true)
	{
		List<string> list = new List<string>();
		UnityEngine.Object[] weaponsInGame = WeaponManager.sharedManager.weaponsInGame;
		if (_003C_003Ef__am_0024cache8 == null)
		{
			_003C_003Ef__am_0024cache8 = _003CFilterPurchases_003Em__21C;
		}
		IEnumerable<WeaponSounds> source = weaponsInGame.Select(_003C_003Ef__am_0024cache8);
		if (_003C_003Ef__am_0024cache9 == null)
		{
			_003C_003Ef__am_0024cache9 = _003CFilterPurchases_003Em__21D;
		}
		Func<WeaponSounds, string> keySelector = _003C_003Ef__am_0024cache9;
		if (_003C_003Ef__am_0024cacheA == null)
		{
			_003C_003Ef__am_0024cacheA = _003CFilterPurchases_003Em__21E;
		}
		Dictionary<string, WeaponSounds> dictionary = source.ToDictionary(keySelector, _003C_003Ef__am_0024cacheA);
		List<List<GameObject>> filteredShopLists = WeaponManager.sharedManager.FilteredShopLists;
		if (_003C_003Ef__am_0024cacheB == null)
		{
			_003C_003Ef__am_0024cacheB = _003CFilterPurchases_003Em__21F;
		}
		HashSet<string> hashSet = new HashSet<string>(filteredShopLists.SelectMany(_003C_003Ef__am_0024cacheB));
		foreach (string item in input)
		{
			if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(item) || item == "Armor_Novice")
			{
				list.Add(item);
				continue;
			}
			ItemRecord byTag = ItemDb.GetByTag(item);
			bool flag = byTag != null && byTag.TemporaryGun;
			bool flag2 = true;
			if ((byTag == null || !hashSet.Contains(byTag.PrefabName)) && WeaponManager.tagToStoreIDMapping.ContainsKey(item))
			{
				flag2 = false;
			}
			if (filterWeapons && (!flag2 || (flag2 && !flag && WeaponManager.tagToStoreIDMapping.ContainsKey(item) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[item]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[item]], true) > 0) || (filterRentedTempWeapons && flag2 && flag && TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(item))))
			{
				list.Add(item);
			}
			bool flag3 = false;
			bool flag4 = false;
			if (checkWear)
			{
				foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> item2 in Wear.wear)
				{
					foreach (List<string> item3 in item2.Value)
					{
						if (item3.Contains(item))
						{
							flag4 = true;
							if (!TempItemsController.PriceCoefs.ContainsKey(item))
							{
								int num = item3.IndexOf(item);
								bool flag5 = Storager.getInt(item, true) == 0;
								bool flag6 = Wear.TierForWear(item) <= ExpController.OurTierForAnyPlace();
								bool flag7 = Wear.LeagueForWear(item, item2.Key) <= (int)RatingSystem.instance.currentLeague;
								flag3 = ((num == 0 && flag5 && flag6) || (num > 0 && flag5 && Storager.getInt(item3[num - 1], true) > 0 && (!filterNextTierUpgrades || flag6))) && flag7;
							}
							break;
						}
					}
				}
			}
			if (!flag3 && (SkinsController.skinsNamesForPers.ContainsKey(item) || item.Equals("CustomSkinID")))
			{
				flag4 = true;
				flag3 = false;
			}
			if (flag4 && !flag3 && !TempItemsController.PriceCoefs.ContainsKey(item))
			{
				list.Add(item);
			}
			if (WeaponManager.sharedManager == null || ExpController.Instance == null)
			{
				continue;
			}
			WeaponSounds value;
			if (filterWeapons && byTag != null && dictionary.TryGetValue(byTag.PrefabName, out value) && value != null)
			{
				if (value.tier > ExpController.Instance.OurTier)
				{
					list.Add(item);
				}
				if (SceneLoader.ActiveSceneName.Equals("Sniper") && (!value.IsAvalibleFromFilter(2) || value.name == WeaponManager.PistolWN || value.name == WeaponManager.KnifeWN))
				{
					list.Add(item);
				}
				if (SceneLoader.ActiveSceneName.Equals("Knife") && value.categoryNabor != 3)
				{
					list.Add(item);
				}
				if (SceneLoader.ActiveSceneName.Equals("LoveIsland") && !value.IsAvalibleFromFilter(3))
				{
					list.Add(item);
				}
			}
			if (!flag4 && !WeaponManager.tagToStoreIDMapping.ContainsKey(item))
			{
				list.Add(item);
			}
			if (TempItemsController.PriceCoefs.ContainsKey(item))
			{
				list.Add(item);
			}
		}
		try
		{
			if (ShopNGUIController.NoviceArmorAvailable)
			{
				List<string> first = list;
				List<List<string>> source2 = Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory];
				if (_003C_003Ef__am_0024cacheC == null)
				{
					_003C_003Ef__am_0024cacheC = _003CFilterPurchases_003Em__220;
				}
				list = first.Union(source2.SelectMany(_003C_003Ef__am_0024cacheC)).ToList();
				return list;
			}
			return list;
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception in FilterPurchases removing all armor: " + ex);
			return list;
		}
	}

	public static string IconNameForKey(string key, int cat)
	{
		string result = string.Empty;
		string text = key;
		if (cat > -1)
		{
			bool flag = TempItemsController.PriceCoefs.ContainsKey(text);
			if (ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)cat))
			{
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (upgrade.Contains(key))
					{
						text = upgrade[0];
						break;
					}
				}
			}
			if (text != null)
			{
				int num = 1;
				if (ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)cat))
				{
					ItemRecord byTag = ItemDb.GetByTag(key);
					if (byTag == null || !byTag.UseImagesFromFirstUpgrade)
					{
						bool maxUpgrade;
						num = 1 + ((!flag) ? ShopNGUIController._CurrentNumberOfUpgrades(text, out maxUpgrade, (ShopNGUIController.CategoryNames)cat, false) : 0);
					}
				}
				if (ShopNGUIController.IsWeaponCategory((ShopNGUIController.CategoryNames)cat) && WeaponManager.GotchaGuns.Contains(text))
				{
					num = 1;
				}
				result = text + "_icon" + num + "_big";
			}
		}
		return result;
	}

	private IEnumerator HandleUpdateCoroutine()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			yield break;
		}
		PromoActionPreview[] componentsInChildren = wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
		foreach (PromoActionPreview pa in componentsInChildren)
		{
			pa.transform.parent = null;
			UnityEngine.Object.Destroy(pa.gameObject);
		}
		wrapContent.SortAlphabetically();
		if (!TrainingController.TrainingCompleted)
		{
			if (fonPromoPanel.activeSelf)
			{
				fonPromoPanel.SetActive(false);
			}
			yield break;
		}
		List<string> idsToAdd = new List<string>();
		List<string> allNews = PromoActionsManager.sharedManager.news;
		List<string> allTopSellers = PromoActionsManager.sharedManager.topSellers;
		List<string> allDiscounts = PromoActionsManager.sharedManager.discounts.Keys.Union(WeaponManager.sharedManager.TryGunPromos.Keys).ToList();
		List<string> news = allNews.Except(FilterPurchases(allNews)).Random(5).ToList();
		idsToAdd.AddRange(news);
		List<string> topSellers = allTopSellers.Except(idsToAdd).ToList();
		topSellers = topSellers.Except(FilterPurchases(topSellers)).Random(2).ToList();
		idsToAdd.AddRange(topSellers);
		List<string> discounts = allDiscounts.Except(idsToAdd).ToList();
		discounts = RiliExtensions.Random(count: 8 - news.Count, source: discounts.Except(FilterPurchases(discounts))).ToList();
		idsToAdd.AddRange(discounts);
		Dictionary<string, PromoActionMenu> pas = new Dictionary<string, PromoActionMenu>();
		foreach (string tg in idsToAdd)
		{
			PromoActionMenu pam = new PromoActionMenu
			{
				tg = tg
			};
			if (allNews.Contains(pam.tg))
			{
				pam.isNew = true;
			}
			if (allTopSellers.Contains(pam.tg))
			{
				pam.isTopSeller = true;
			}
			if (allDiscounts.Contains(pam.tg))
			{
				pam.isDiscounted = true;
				try
				{
					bool unused;
					pam.discount = ShopNGUIController.DiscountFor(pam.tg, out unused);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("Exception in pam.discount = ShopNGUIController.DiscountFor(key,out unused): " + e);
				}
				pam.price = ShopNGUIController.currentPrice(pam.tg, (ShopNGUIController.CategoryNames)CatForTg(pam.tg)).Price;
			}
			pas.Add(tg, pam);
		}
		string discountLocalize = LocalizationStore.Key_0419;
		foreach (string key in pas.Keys)
		{
			GameObject f = UnityEngine.Object.Instantiate(Resources.Load("PromoAction") as GameObject);
			f.transform.parent = wrapContent.transform;
			f.transform.localScale = new Vector3(1f, 1f, 1f);
			PromoActionPreview pap = f.GetComponent<PromoActionPreview>();
			ItemPrice price = null;
			if (!(key == "StickersPromoActionsPanelKey"))
			{
				string shopId = ItemDb.GetShopIdByTag(key) ?? key;
				if (!string.IsNullOrEmpty(shopId))
				{
					price = ItemDb.GetPriceByShopId(shopId);
				}
			}
			if (pas[key].isDiscounted)
			{
				pap.sale.gameObject.SetActive(true);
				pap.Discount = pas[key].discount;
				pap.sale.text = string.Format("{0}\n{1}%", discountLocalize, pas[key].discount);
				pap.coins.text = pas[key].price.ToString();
			}
			else if (price != null)
			{
				pap.coins.text = price.Price.ToString();
			}
			if (price != null)
			{
				pap.currencyImage.spriteName = ((!price.Currency.Equals("Coins")) ? "gem_znachek" : "ingame_coin");
				pap.currencyImage.width = ((!price.Currency.Equals("Coins")) ? 34 : 30);
				pap.currencyImage.height = ((!price.Currency.Equals("Coins")) ? 24 : 30);
				pap.coins.color = ((!price.Currency.Equals("Coins")) ? new Color(0.3176f, 0.8117f, 1f) : new Color(1f, 0.8627f, 0f));
			}
			else
			{
				pap.coins.gameObject.SetActive(false);
				pap.currencyImage.gameObject.SetActive(false);
			}
			if (key == "StickersPromoActionsPanelKey")
			{
				pap.stickersLabel.SetActive(true);
			}
			pap.topSeller.gameObject.SetActive(pas[key].isTopSeller);
			pap.newItem.gameObject.SetActive(pas[key].isNew);
			if (key == "StickersPromoActionsPanelKey")
			{
				pap.button.tweenTarget = pap.stickerTexture.gameObject;
				pap.icon.mainTexture = null;
				pap.icon = pap.stickerTexture;
				pap.pressed = pap.stickerTexture.mainTexture;
				pap.unpressed = pap.stickerTexture.mainTexture;
			}
			else
			{
				string imageName2 = string.Empty;
				int cat = CatForTg(key);
				imageName2 = "OfferIcons/" + IconNameForKey(key, cat);
				Texture t = Resources.Load<Texture>(imageName2);
				if (t != null)
				{
					pap.unpressed = t;
					pap.icon.mainTexture = t;
				}
				if (t != null)
				{
					pap.pressed = t;
				}
			}
			pap.tg = key;
		}
		noOffersLabel.gameObject.SetActive(pas.Count == 0 && PromoActionsManager.ActionsAvailable);
		checkInternetLabel.gameObject.SetActive(pas.Count == 0 && !PromoActionsManager.ActionsAvailable);
		if (fonPromoPanel.activeSelf != (pas.Count != 0))
		{
			fonPromoPanel.SetActive(pas.Count != 0);
		}
		yield return null;
		PromoActionPreview[] paps = wrapContent.GetComponentsInChildren<PromoActionPreview>(true);
		if (paps == null)
		{
			paps = new PromoActionPreview[0];
		}
		if (_003CHandleUpdateCoroutine_003Ec__IteratorF0._003C_003Ef__am_0024cache25 == null)
		{
			_003CHandleUpdateCoroutine_003Ec__IteratorF0._003C_003Ef__am_0024cache25 = _003CHandleUpdateCoroutine_003Ec__IteratorF0._003C_003Em__222;
		}
		Comparison<PromoActionPreview> comp = _003CHandleUpdateCoroutine_003Ec__IteratorF0._003C_003Ef__am_0024cache25;
		Array.Sort(paps, comp);
		for (int i = 0; i < paps.Length; i++)
		{
			paps[i].gameObject.name = i.ToString("D7");
		}
		wrapContent.SortAlphabetically();
		wrapContent.WrapContent();
		Transform lookAtTransform = null;
		if (paps.Length > 0)
		{
			lookAtTransform = paps[0].transform;
		}
		if (lookAtTransform != null)
		{
			float x = lookAtTransform.localPosition.x - 9f;
			Transform scrollViewTr = wrapContent.transform.parent;
			if (scrollViewTr != null)
			{
				UIPanel scrollViewPanel = scrollViewTr.GetComponent<UIPanel>();
				if (scrollViewPanel != null)
				{
					scrollViewPanel.clipOffset = new Vector2(x, scrollViewPanel.clipOffset.y);
					scrollViewTr.localPosition = new Vector3(0f - x, scrollViewTr.localPosition.y, scrollViewTr.localPosition.z);
				}
			}
		}
		wrapContent.SortAlphabetically();
		wrapContent.WrapContent();
		wrapContent.transform.parent.GetComponent<UIScrollView>().enabled = wrapContent.transform.childCount <= 0 || wrapContent.transform.childCount > 3;
		yield return null;
		wrapContent.SortAlphabetically();
		wrapContent.WrapContent();
		wrapContent.transform.GetComponent<MyCenterOnChild>().Recenter();
	}

	public static int CatForTg(string tg)
	{
		int num = -1;
		if (WeaponManager.sharedManager == null)
		{
			return num;
		}
		string text = null;
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag != null)
		{
			text = byTag.PrefabName;
		}
		if (text != null)
		{
			foreach (WeaponSounds item in WeaponManager.AllWrapperPrefabs())
			{
				if (item == null || !(item.name == text))
				{
					continue;
				}
				if (item != null)
				{
					num = item.categoryNabor - 1;
				}
				break;
			}
		}
		if (num == -1)
		{
			bool flag = false;
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> item2 in Wear.wear)
			{
				foreach (List<string> item3 in item2.Value)
				{
					if (item3.Contains(tg))
					{
						flag = true;
						num = (int)item2.Key;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (num == -1 && (SkinsController.skinsNamesForPers.ContainsKey(tg) || tg.Equals("CustomSkinID")))
		{
			num = 8;
		}
		return num;
	}

	[CompilerGenerated]
	private static int _003CFilterForLoadings_003Em__21A(GameObject go1, GameObject go2)
	{
		float num = 1f;
		float num2 = 1f;
		num = go1.GetComponent<WeaponSounds>().DamageByTier[go1.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
		num2 = go2.GetComponent<WeaponSounds>().DamageByTier[go2.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
		return (int)(num - num2);
	}

	[CompilerGenerated]
	private static WeaponSounds _003CFilterPurchases_003Em__21C(UnityEngine.Object w)
	{
		return ((GameObject)w).GetComponent<WeaponSounds>();
	}

	[CompilerGenerated]
	private static string _003CFilterPurchases_003Em__21D(WeaponSounds ws)
	{
		return ws.name;
	}

	[CompilerGenerated]
	private static WeaponSounds _003CFilterPurchases_003Em__21E(WeaponSounds ws)
	{
		return ws;
	}

	[CompilerGenerated]
	private static IEnumerable<string> _003CFilterPurchases_003Em__21F(List<GameObject> l)
	{
		if (_003C_003Ef__am_0024cacheD == null)
		{
			_003C_003Ef__am_0024cacheD = _003CFilterPurchases_003Em__221;
		}
		return l.Select(_003C_003Ef__am_0024cacheD);
	}

	[CompilerGenerated]
	private static IEnumerable<string> _003CFilterPurchases_003Em__220(List<string> list)
	{
		return list;
	}

	[CompilerGenerated]
	private static string _003CFilterPurchases_003Em__221(GameObject g)
	{
		return g.name.Replace("(Clone)", string.Empty);
	}
}
