using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PromoActionsGUIController : MonoBehaviour
{
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

	public PromoActionsGUIController()
	{
	}

	public static int CatForTg(string tg)
	{
		int key = -1;
		if (WeaponManager.sharedManager == null)
		{
			return key;
		}
		string prefabName = null;
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag != null)
		{
			prefabName = byTag.PrefabName;
		}
		if (prefabName != null)
		{
			foreach (WeaponSounds weaponSound in WeaponManager.AllWrapperPrefabs())
			{
				if (weaponSound != null)
				{
					if (weaponSound.name != prefabName)
					{
						continue;
					}
					if (weaponSound != null)
					{
						key = weaponSound.categoryNabor - 1;
					}
					break;
				}
			}
		}
		if (key == -1)
		{
			bool flag = false;
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
			{
				foreach (List<string> value in keyValuePair.Value)
				{
					if (!value.Contains(tg))
					{
						continue;
					}
					flag = true;
					key = (int)keyValuePair.Key;
					break;
				}
				if (!flag)
				{
					continue;
				}
				break;
			}
		}
		if (key == -1 && (SkinsController.skinsNamesForPers.ContainsKey(tg) || tg.Equals("CustomSkinID")))
		{
			key = 8;
		}
		return key;
	}

	public static string FilterForLoadings(string tg, List<string> alreadyUsed)
	{
		string str;
		string tag;
		GameObject item;
		if (tg == null || alreadyUsed == null)
		{
			return null;
		}
		string str1 = WeaponManager.FirstUnboughtTag(tg);
		string empty = string.Empty;
		try
		{
			empty = WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[str1]];
			goto Label0;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.Log(string.Concat("Exception in FilterForLoadings:  idefFirstUnobught = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnobught] ]:  ", exception));
			str = null;
		}
		return str;
	Label0:
		if (!Storager.hasKey(empty))
		{
			Storager.setInt(empty, 0, false);
		}
		bool num = Storager.getInt(empty, true) > 0;
		WeaponSounds component = null;
		UnityEngine.Object[] objArray = WeaponManager.sharedManager.weaponsInGame;
		int num1 = 0;
		while (num1 < (int)objArray.Length)
		{
			GameObject gameObject = (GameObject)objArray[num1];
			if (!ItemDb.GetByPrefabName(gameObject.name).Tag.Equals(str1))
			{
				num1++;
			}
			else
			{
				component = gameObject.GetComponent<WeaponSounds>();
				break;
			}
		}
		if (component == null)
		{
			return null;
		}
		if (!num && component.tier <= ExpController.Instance.OurTier && !alreadyUsed.Contains(ItemDb.GetByPrefabName(component.name.Replace("(Clone)", string.Empty)).Tag))
		{
			return str1;
		}
		WeaponSounds weaponSound = component;
		if (!num)
		{
			string str2 = WeaponManager.LastBoughtTag(str1);
			if (str2 != null)
			{
				List<string> strs = null;
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (!upgrade.Contains(str2))
					{
						continue;
					}
					strs = upgrade;
					break;
				}
				int num2 = strs.IndexOf(str2);
				while (num2 >= 0)
				{
					bool flag = false;
					UnityEngine.Object[] objArray1 = WeaponManager.sharedManager.weaponsInGame;
					for (int i = 0; i < (int)objArray1.Length; i++)
					{
						GameObject gameObject1 = (GameObject)objArray1[i];
						if (ItemDb.GetByPrefabName(gameObject1.name).Tag.Equals(strs[num2]))
						{
							WeaponSounds component1 = gameObject1.GetComponent<WeaponSounds>();
							if (component1.tier <= ExpController.Instance.OurTier)
							{
								flag = true;
								weaponSound = component1;
								break;
							}
						}
					}
					if (!flag)
					{
						num2--;
					}
					else
					{
						break;
					}
				}
			}
		}
		float single1 = 1f;
		if (weaponSound != null)
		{
			single1 = weaponSound.DamageByTier[weaponSound.tier] / weaponSound.lengthForShot;
		}
		float single2 = single1;
		if (num && component.tier > ExpController.Instance.OurTier && weaponSound != null)
		{
			try
			{
				single2 = single1 * (weaponSound.DamageByTier[ExpController.Instance.OurTier] / weaponSound.DamageByTier[weaponSound.tier]);
			}
			catch (Exception exception1)
			{
				UnityEngine.Debug.Log(string.Concat("Exception in FilterForLoadings:  if (bought && ws.tier > ExpController.Instance.OurTier && lastBoughtInOurTierWS != null):  ", exception1));
			}
		}
		List<string> strs1 = new List<string>()
		{
			tg
		};
		foreach (List<string> upgrade1 in WeaponUpgrades.upgrades)
		{
			if (!upgrade1.Contains(tg))
			{
				continue;
			}
			strs1 = upgrade1;
			break;
		}
		List<string> strs2 = new List<string>();
		List<GameObject> gameObjects = new List<GameObject>();
		UnityEngine.Object[] objArray2 = WeaponManager.sharedManager.weaponsInGame;
		for (int j = 0; j < (int)objArray2.Length; j++)
		{
			GameObject gameObject2 = (GameObject)objArray2[j];
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(gameObject2.name);
			if (!strs1.Contains(byPrefabName.Tag) && gameObject2.GetComponent<WeaponSounds>().tier <= ExpController.Instance.OurTier && !gameObject2.GetComponent<WeaponSounds>().campaignOnly && !gameObject2.name.Equals(WeaponManager.AlienGunWN) && !gameObject2.name.Equals(WeaponManager.BugGunWN) && !gameObject2.name.Equals(WeaponManager.SimpleFlamethrower_WN) && !gameObject2.name.Equals(WeaponManager.CampaignRifle_WN) && !gameObject2.name.Equals(WeaponManager.Rocketnitza_WN) && !gameObject2.name.Equals(WeaponManager.PistolWN) && !gameObject2.name.Equals(WeaponManager.SocialGunWN) && !gameObject2.name.Equals(WeaponManager.DaterFreeWeaponPrefabName) && !gameObject2.name.Equals(WeaponManager.KnifeWN) && !gameObject2.name.Equals(WeaponManager.ShotgunWN) && !gameObject2.name.Equals(WeaponManager.MP5WN) && !ItemDb.IsTemporaryGun(byPrefabName.Tag) && (byPrefabName.Tag == null || !WeaponManager.GotchaGuns.Contains(byPrefabName.Tag)))
			{
				string str3 = WeaponManager.FirstUnboughtTag(byPrefabName.Tag);
				if (!alreadyUsed.Contains(str3) && !strs2.Contains(str3))
				{
					bool flag1 = false;
					UnityEngine.Object[] objArray3 = WeaponManager.sharedManager.weaponsInGame;
					int num3 = 0;
					while (num3 < (int)objArray3.Length)
					{
						GameObject gameObject3 = (GameObject)objArray3[num3];
						if (!ItemDb.GetByPrefabName(gameObject3.name).Tag.Equals(str3))
						{
							num3++;
						}
						else
						{
							flag1 = gameObject3.GetComponent<WeaponSounds>().tier > ExpController.Instance.OurTier;
							break;
						}
					}
					if (!flag1)
					{
						string empty1 = string.Empty;
						try
						{
							if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[str3]], true) > 0)
							{
								goto Label1;
							}
						}
						catch (Exception exception2)
						{
							UnityEngine.Debug.Log(string.Concat("Exception in FilterForLoadings:  defFirstUnobughtOther = WeaponManager.storeIDtoDefsSNMapping[ WeaponManager.tagToStoreIDMapping[firstUnboughtOthers] ]:  ", exception2));
						}
						strs2.Add(str3);
						UnityEngine.Object[] objArray4 = WeaponManager.sharedManager.weaponsInGame;
						int num4 = 0;
						while (num4 < (int)objArray4.Length)
						{
							GameObject gameObject4 = (GameObject)objArray4[num4];
							if (!ItemDb.GetByPrefabName(gameObject4.name).Tag.Equals(str3))
							{
								num4++;
							}
							else
							{
								gameObjects.Add(gameObject4);
								break;
							}
						}
					}
				}
			}
		Label1:
		}
		gameObjects.Sort((GameObject go1, GameObject go2) => {
			float damageByTier = 1f;
			float single = 1f;
			damageByTier = go1.GetComponent<WeaponSounds>().DamageByTier[go1.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
			single = go2.GetComponent<WeaponSounds>().DamageByTier[go2.GetComponent<WeaponSounds>().tier] / go1.GetComponent<WeaponSounds>().lengthForShot;
			return (int)(damageByTier - single);
		});
		GameObject gameObject5 = gameObjects.Find((GameObject obj) => {
			float damageByTier = 1f;
			damageByTier = obj.GetComponent<WeaponSounds>().DamageByTier[obj.GetComponent<WeaponSounds>().tier] / obj.GetComponent<WeaponSounds>().lengthForShot;
			return damageByTier >= single2;
		});
		if (gameObject5 == null)
		{
			if (gameObjects.Count <= 0)
			{
				item = null;
			}
			else
			{
				item = gameObjects[gameObjects.Count - 1];
			}
			gameObject5 = item;
		}
		if (gameObject5 == null)
		{
			tag = null;
		}
		else
		{
			tag = ItemDb.GetByPrefabName(gameObject5.name).Tag;
		}
		return tag;
	}

	public static List<string> FilterPurchases(IEnumerable<string> input, bool filterNextTierUpgrades = false, bool filterWeapons = true, bool filterRentedTempWeapons = false, bool checkWear = true)
	{
		WeaponSounds weaponSound;
		bool flag;
		List<string> strs = new List<string>();
		Dictionary<string, WeaponSounds> dictionary = (
			from w in (IEnumerable<UnityEngine.Object>)WeaponManager.sharedManager.weaponsInGame
			select ((GameObject)w).GetComponent<WeaponSounds>()).ToDictionary<WeaponSounds, string, WeaponSounds>((WeaponSounds ws) => ws.name, (WeaponSounds ws) => ws);
		HashSet<string> strs1 = new HashSet<string>(WeaponManager.sharedManager.FilteredShopLists.SelectMany<List<GameObject>, string>((List<GameObject> l) => 
			from g in l
			select g.name.Replace("(Clone)", string.Empty)));
		IEnumerator<string> enumerator = input.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(current) || current == "Armor_Novice")
				{
					strs.Add(current);
				}
				else
				{
					ItemRecord byTag = ItemDb.GetByTag(current);
					bool flag1 = (byTag == null ? false : byTag.TemporaryGun);
					bool flag2 = true;
					if ((byTag == null ? true : !strs1.Contains(byTag.PrefabName)) && WeaponManager.tagToStoreIDMapping.ContainsKey(current))
					{
						flag2 = false;
					}
					if (filterWeapons && (!flag2 || flag2 && !flag1 && WeaponManager.tagToStoreIDMapping.ContainsKey(current) && WeaponManager.storeIDtoDefsSNMapping.ContainsKey(WeaponManager.tagToStoreIDMapping[current]) && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[current]], true) > 0 || filterRentedTempWeapons && flag2 && flag1 && TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(current)))
					{
						strs.Add(current);
					}
					bool flag3 = false;
					bool flag4 = false;
					if (checkWear)
					{
						Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>.Enumerator enumerator1 = Wear.wear.GetEnumerator();
						try
						{
							while (enumerator1.MoveNext())
							{
								KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair = enumerator1.Current;
								List<List<string>>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										List<string> current1 = enumerator2.Current;
										if (!current1.Contains(current))
										{
											continue;
										}
										flag4 = true;
										if (!TempItemsController.PriceCoefs.ContainsKey(current))
										{
											int num = current1.IndexOf(current);
											bool num1 = Storager.getInt(current, true) == 0;
											bool flag5 = Wear.TierForWear(current) <= ExpController.OurTierForAnyPlace();
											bool flag6 = Wear.LeagueForWear(current, keyValuePair.Key) <= (int)RatingSystem.instance.currentLeague;
											if (num != 0 || !num1 || !flag5)
											{
												if (num > 0 && num1 && Storager.getInt(current1[num - 1], true) > 0)
												{
													if ((!filterNextTierUpgrades ? true : flag5))
													{
														goto Label1;
													}
												}
												flag = false;
												goto Label0;
											}
										Label1:
											flag = flag6;
										Label0:
											flag3 = flag;
											break;
										}
										else
										{
											break;
										}
									}
								}
								finally
								{
									((IDisposable)(object)enumerator2).Dispose();
								}
							}
						}
						finally
						{
							((IDisposable)(object)enumerator1).Dispose();
						}
					}
					if (!flag3 && (SkinsController.skinsNamesForPers.ContainsKey(current) || current.Equals("CustomSkinID")))
					{
						flag4 = true;
						flag3 = false;
					}
					if (flag4 && !flag3 && !TempItemsController.PriceCoefs.ContainsKey(current))
					{
						strs.Add(current);
					}
					if (!(WeaponManager.sharedManager == null) && !(ExpController.Instance == null))
					{
						if (filterWeapons && byTag != null && dictionary.TryGetValue(byTag.PrefabName, out weaponSound) && weaponSound != null)
						{
							if (weaponSound.tier > ExpController.Instance.OurTier)
							{
								strs.Add(current);
							}
							if (SceneLoader.ActiveSceneName.Equals("Sniper") && (!weaponSound.IsAvalibleFromFilter(2) || weaponSound.name == WeaponManager.PistolWN || weaponSound.name == WeaponManager.KnifeWN))
							{
								strs.Add(current);
							}
							if (SceneLoader.ActiveSceneName.Equals("Knife") && weaponSound.categoryNabor != 3)
							{
								strs.Add(current);
							}
							if (SceneLoader.ActiveSceneName.Equals("LoveIsland") && !weaponSound.IsAvalibleFromFilter(3))
							{
								strs.Add(current);
							}
						}
						if (!flag4 && !WeaponManager.tagToStoreIDMapping.ContainsKey(current))
						{
							strs.Add(current);
						}
						if (!TempItemsController.PriceCoefs.ContainsKey(current))
						{
							continue;
						}
						strs.Add(current);
					}
				}
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		try
		{
			if (ShopNGUIController.NoviceArmorAvailable)
			{
				strs = strs.Union<string>(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory].SelectMany<List<string>, string>((List<string> list) => list)).ToList<string>();
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(string.Concat("Exception in FilterPurchases removing all armor: ", exception));
		}
		return strs;
	}

	[DebuggerHidden]
	private IEnumerator HandleUpdateCoroutine()
	{
		PromoActionsGUIController.u003cHandleUpdateCoroutineu003ec__IteratorF0 variable = null;
		return variable;
	}

	private void HandleUpdated()
	{
		base.StartCoroutine(this.HandleUpdateCoroutine());
	}

	public static string IconNameForKey(string key, int cat)
	{
		bool flag;
		string empty = string.Empty;
		string item = key;
		if (cat > -1)
		{
			ShopNGUIController.CategoryNames categoryName = (ShopNGUIController.CategoryNames)cat;
			bool flag1 = TempItemsController.PriceCoefs.ContainsKey(item);
			if (ShopNGUIController.IsWeaponCategory(categoryName))
			{
				foreach (List<string> upgrade in WeaponUpgrades.upgrades)
				{
					if (!upgrade.Contains(key))
					{
						continue;
					}
					item = upgrade[0];
					break;
				}
			}
			if (item != null)
			{
				int num = 1;
				if (ShopNGUIController.IsWeaponCategory(categoryName))
				{
					ItemRecord byTag = ItemDb.GetByTag(key);
					if (byTag == null || !byTag.UseImagesFromFirstUpgrade)
					{
						num = 1 + (!flag1 ? ShopNGUIController._CurrentNumberOfUpgrades(item, out flag, categoryName, false) : 0);
					}
				}
				if (ShopNGUIController.IsWeaponCategory(categoryName) && WeaponManager.GotchaGuns.Contains(item))
				{
					num = 1;
				}
				empty = string.Concat(item, "_icon", num.ToString(), "_big");
			}
		}
		return empty;
	}

	public void MarkUpdateOnEnable()
	{
		this.updateOnEnable = true;
		if (base.gameObject.activeInHierarchy)
		{
			this.HandleUpdated();
		}
	}

	private void OnDestroy()
	{
		PromoActionsManager.ActionsUUpdated -= new Action(this.UpdateAfterDelayHandler);
		ShopNGUIController.GunBought -= new Action(this.MarkUpdateOnEnable);
		WeaponManager.TryGunExpired -= new Action(this.MarkUpdateOnEnable);
		StickersController.onBuyPack -= new Action(this.MarkUpdateOnEnable);
	}

	private void OnEnable()
	{
		if (this.updateOnEnable || !this.initiallyUpdated)
		{
			base.StartCoroutine(this.UpdateAfterDelay());
		}
		this.updateOnEnable = false;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		PromoActionsGUIController.u003cStartu003ec__IteratorEE variable = null;
		return variable;
	}

	private void Update()
	{
		Transform transforms = this.wrapContent.transform;
		if (transforms.childCount > 0)
		{
			UIPanel component = transforms.parent.GetComponent<UIPanel>();
			if (transforms.childCount <= 3)
			{
				float current = 0f;
				IEnumerator enumerator = transforms.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						current += ((Transform)enumerator.Current).localPosition.x;
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
				current /= (float)transforms.childCount;
				if (component != null)
				{
					this.wrapContent.WrapContent();
					component.GetComponent<UIScrollView>().SetDragAmount(0.5f, 0f, false);
				}
			}
			if (this.refreshPromoPanelCntr % 10 == 0)
			{
				component.Refresh();
			}
			this.refreshPromoPanelCntr++;
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateAfterDelay()
	{
		PromoActionsGUIController.u003cUpdateAfterDelayu003ec__IteratorEF variable = null;
		return variable;
	}

	private void UpdateAfterDelayHandler()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.UpdateAfterDelay());
		}
	}
}