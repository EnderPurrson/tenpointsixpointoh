using Rilisoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class VirtualCurrencyHelper
{
	private static int[] _coinInappsQuantityDefault;

	public static int[] _coinInappsQuantityStaticBank;

	public static string[] coinInappsLocalizationKeys;

	private static int[] _coinInappsQuantity;

	private static int[] _gemsInappsQuantityDefault;

	public static int[] _gemsInappsQuantityStaticBank;

	public static string[] gemsInappsLocalizationKeys;

	private static int[] _gemsInappsQuantity;

	public readonly static int[] coinPriceIds;

	public readonly static int[] gemsPriceIds;

	public readonly static int[] starterPackFakePrice;

	private static Dictionary<string, SaltedInt> prices;

	private static System.Random _prng;

	private static WeakReference _referencePricesInUsd;

	private static Dictionary<string, ItemPrice> _armorPricesDefault;

	public static int[] coinInappsQuantity
	{
		get
		{
			return (!FriendsController.isShowStaticBank ? VirtualCurrencyHelper._coinInappsQuantity : VirtualCurrencyHelper._coinInappsQuantityStaticBank);
		}
	}

	public static int[] gemsInappsQuantity
	{
		get
		{
			return (!FriendsController.isShowStaticBank ? VirtualCurrencyHelper._gemsInappsQuantity : VirtualCurrencyHelper._gemsInappsQuantityStaticBank);
		}
	}

	internal static Dictionary<string, decimal> ReferencePricesInUsd
	{
		get
		{
			if (VirtualCurrencyHelper._referencePricesInUsd != null && VirtualCurrencyHelper._referencePricesInUsd.IsAlive)
			{
				return (Dictionary<string, decimal>)VirtualCurrencyHelper._referencePricesInUsd.Target;
			}
			Dictionary<string, decimal> strs = VirtualCurrencyHelper.InitializeReferencePrices();
			VirtualCurrencyHelper._referencePricesInUsd = new WeakReference(strs, false);
			return strs;
		}
	}

	static VirtualCurrencyHelper()
	{
		VirtualCurrencyHelper._coinInappsQuantityDefault = new int[] { 15, 45, 80, 165, 335, 865, 2000 };
		VirtualCurrencyHelper._coinInappsQuantityStaticBank = new int[] { 15, 50, 90, 185, 390, 1050, 2250 };
		VirtualCurrencyHelper.coinInappsLocalizationKeys = new string[] { "Key_2106", "Key_2107", "Key_2108", "Key_2109", "Key_2110", "Key_2111", "Key_2112" };
		VirtualCurrencyHelper._coinInappsQuantity = VirtualCurrencyHelper.InitCoinInappsQuantity(VirtualCurrencyHelper._coinInappsQuantityDefault);
		VirtualCurrencyHelper._gemsInappsQuantityDefault = new int[] { 9, 27, 48, 100, 200, 517, 1200 };
		VirtualCurrencyHelper._gemsInappsQuantityStaticBank = new int[] { 9, 30, 60, 120, 260, 700, 1500 };
		VirtualCurrencyHelper.gemsInappsLocalizationKeys = new string[] { "Key_2113", "Key_2114", "Key_2115", "Key_2116", "Key_2117", "Key_2118", "Key_2119" };
		VirtualCurrencyHelper._gemsInappsQuantity = VirtualCurrencyHelper.InitGemsInappsQuantity(VirtualCurrencyHelper._gemsInappsQuantityDefault);
		VirtualCurrencyHelper.coinPriceIds = new int[] { 1, 3, 5, 10, 20, 50, 100 };
		VirtualCurrencyHelper.gemsPriceIds = new int[] { 1, 3, 5, 10, 20, 50, 100 };
		VirtualCurrencyHelper.starterPackFakePrice = new int[] { 6, 5, 4, 3, 2, 1, 1, 1 };
		VirtualCurrencyHelper.prices = new Dictionary<string, SaltedInt>();
		VirtualCurrencyHelper._prng = new System.Random(4103);
		VirtualCurrencyHelper._armorPricesDefault = new Dictionary<string, ItemPrice>();
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.OneDay.ToString(), 5);
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.ThreeDay.ToString(), 10);
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.SevenDays.ToString(), 20);
		VirtualCurrencyHelper.AddPrice(PremiumAccountController.AccountType.Month.ToString(), 60);
		VirtualCurrencyHelper.AddPrice("crystalsword", 185);
		VirtualCurrencyHelper.AddPrice("Fullhealth", 15);
		VirtualCurrencyHelper.AddPrice("bigammopack", 15);
		VirtualCurrencyHelper.AddPrice("MinerWeapon", 35);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.elixirID, 15);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.chief, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.nanosoldier, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.endmanskin, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.spaceengineer, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.steelman, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.CaptainSkin, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.HawkSkin, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.TunderGodSkin, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.GreenGuySkin, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.GordonSkin, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.armor, 10);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.armor2, 15);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.armor3, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.magicGirl, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.braveGirl, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.glamDoll, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.kittyGirl, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.famosBoy, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin810_1, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin810_2, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin810_3, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin810_4, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin810_5, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin810_6, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_rapid_girl, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_silent_killer, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_daemon_fighter, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_scary_demon, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_orc_warrior, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_kung_fu_master, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_fire_wizard, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_ice_wizard, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_storm_wizard, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin931_1, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin931_2, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_may1, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_may2, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_may3, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_may4, 20);
		VirtualCurrencyHelper.AddPrice("skin_july1", 20);
		VirtualCurrencyHelper.AddPrice("skin_july2", 20);
		VirtualCurrencyHelper.AddPrice("skin_july3", 20);
		VirtualCurrencyHelper.AddPrice("skin_july4", 20);
		for (int i = 0; i < (int)StoreKitEventListener.Skins_11_040915.Length; i++)
		{
			VirtualCurrencyHelper.AddPrice(StoreKitEventListener.Skins_11_040915[i], 20);
		}
		VirtualCurrencyHelper.AddPrice("super_socialman", 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_tiger, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_pitbull, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_santa, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_elf_new_year, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_girl_new_year, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_cookie_new_year, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_snowman_new_year, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_jetti_hnight, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.skin_startrooper, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.easter_skin1, 20);
		VirtualCurrencyHelper.AddPrice(StoreKitEventListener.easter_skin2, 20);
		VirtualCurrencyHelper.AddPrice("CustomSkinID", Defs.skinsMakerPrice);
		VirtualCurrencyHelper.AddPrice("cape_Archimage", 60);
		VirtualCurrencyHelper.AddPrice("cape_BloodyDemon", 60);
		VirtualCurrencyHelper.AddPrice("cape_RoyalKnight", 60);
		VirtualCurrencyHelper.AddPrice("cape_SkeletonLord", 60);
		VirtualCurrencyHelper.AddPrice("cape_EliteCrafter", 60);
		VirtualCurrencyHelper.AddPrice("cape_Custom", 75);
		VirtualCurrencyHelper.AddPrice("HitmanCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("BerserkCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("DemolitionCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("SniperCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("StormTrooperCape_Up1", 30);
		VirtualCurrencyHelper.AddPrice("HitmanCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("BerserkCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("DemolitionCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("SniperCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("StormTrooperCape_Up2", 45);
		VirtualCurrencyHelper.AddPrice("cape_Engineer", 60);
		VirtualCurrencyHelper.AddPrice("cape_Engineer_Up1", 30);
		VirtualCurrencyHelper.AddPrice("cape_Engineer_Up2", 45);
		VirtualCurrencyHelper.AddPrice("hat_DiamondHelmet", 65);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_3", 3);
		VirtualCurrencyHelper.AddPrice("hat_Headphones", 50);
		VirtualCurrencyHelper.AddPrice("hat_ManiacMask", 65);
		VirtualCurrencyHelper.AddPrice("hat_KingsCrown", 150);
		VirtualCurrencyHelper.AddPrice("hat_SeriousManHat", 50);
		VirtualCurrencyHelper.AddPrice("hat_Samurai", 95);
		VirtualCurrencyHelper.AddPrice("league2_hat_cowboyhat", 100);
		VirtualCurrencyHelper.AddPrice("league3_hat_afro", 150);
		VirtualCurrencyHelper.AddPrice("league4_hat_mushroom", 200);
		VirtualCurrencyHelper.AddPrice("league5_hat_brain", 250);
		VirtualCurrencyHelper.AddPrice("league6_hat_tiara", 300);
		VirtualCurrencyHelper.AddPrice("hat_AlmazHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_ArmyHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_SteelHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_GoldHelmet", 95);
		VirtualCurrencyHelper.AddPrice("hat_Army_1", 70);
		VirtualCurrencyHelper.AddPrice("hat_Army_2", 70);
		VirtualCurrencyHelper.AddPrice("hat_Army_3", 70);
		VirtualCurrencyHelper.AddPrice("hat_Army_4", 70);
		VirtualCurrencyHelper.AddPrice("hat_Steel_1", 85);
		VirtualCurrencyHelper.AddPrice("hat_Steel_2", 85);
		VirtualCurrencyHelper.AddPrice("hat_Steel_3", 85);
		VirtualCurrencyHelper.AddPrice("hat_Steel_4", 85);
		VirtualCurrencyHelper.AddPrice("hat_Royal_1", 100);
		VirtualCurrencyHelper.AddPrice("hat_Royal_2", 100);
		VirtualCurrencyHelper.AddPrice("hat_Royal_3", 100);
		VirtualCurrencyHelper.AddPrice("hat_Royal_4", 100);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_1", 120);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_2", 120);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_3", 120);
		VirtualCurrencyHelper.AddPrice("hat_Almaz_4", 120);
		VirtualCurrencyHelper.AddPrice(PotionsController.HastePotion, 2);
		VirtualCurrencyHelper.AddPrice(PotionsController.MightPotion, 2);
		VirtualCurrencyHelper.AddPrice(PotionsController.RegenerationPotion, 5);
		VirtualCurrencyHelper.AddPrice(GearManager.UpgradeIDForGear("InvisibilityPotion", 1), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 2), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 3), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 4), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 5), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 1), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 2), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 3), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 4), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 5), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 1), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 2), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 3), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 4), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 5), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 1), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 2), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 3), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 4), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 5), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 1), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 2), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 3), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 4), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 5), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Wings, GearManager.OneItemSuffix, 0), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Bear, GearManager.OneItemSuffix, 0), 2);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.BigHeadPotion, GearManager.OneItemSuffix, 0), 1);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.MusicBox, GearManager.OneItemSuffix, 0), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Like, GearManager.OneItemSuffix, 0), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 0), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 1), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 2), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 3), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 4), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 5), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.OneItemSuffix, 0), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.OneItemSuffix, 1), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.OneItemSuffix, 2), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.OneItemSuffix, 3), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.OneItemSuffix, 4), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat("GrenadeID", GearManager.OneItemSuffix, 5), 3);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 0), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 1), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 2), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 3), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 4), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 5), 5);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 0), 7);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 1), 7);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 2), 7);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 3), 7);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 4), 7);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 5), 7);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 0), 4);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 1), 4);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 2), 4);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 3), 4);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 4), 4);
		VirtualCurrencyHelper.AddPrice(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 5), 4);
		VirtualCurrencyHelper.AddPrice("boots_red", 50);
		VirtualCurrencyHelper.AddPrice("boots_gray", 50);
		VirtualCurrencyHelper.AddPrice("boots_blue", 50);
		VirtualCurrencyHelper.AddPrice("boots_green", 50);
		VirtualCurrencyHelper.AddPrice("boots_black", 50);
		VirtualCurrencyHelper.AddPrice("boots_tabi", 120);
		VirtualCurrencyHelper.AddPrice("HitmanBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("StormTrooperBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("SniperBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("DemolitionBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("BerserkBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("HitmanBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("StormTrooperBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("SniperBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("DemolitionBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("BerserkBoots_Up2", 40);
		VirtualCurrencyHelper.AddPrice("mask_sniper", 40);
		VirtualCurrencyHelper.AddPrice("mask_sniper_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_sniper_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_demolition", 40);
		VirtualCurrencyHelper.AddPrice("mask_demolition_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_demolition_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_hitman_1", 40);
		VirtualCurrencyHelper.AddPrice("mask_hitman_1_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_hitman_1_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_berserk", 40);
		VirtualCurrencyHelper.AddPrice("mask_berserk_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_berserk_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_trooper", 40);
		VirtualCurrencyHelper.AddPrice("mask_trooper_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_trooper_up2", 30);
		VirtualCurrencyHelper.AddPrice("mask_engineer", 40);
		VirtualCurrencyHelper.AddPrice("mask_engineer_up1", 15);
		VirtualCurrencyHelper.AddPrice("mask_engineer_up2", 30);
		VirtualCurrencyHelper.AddPrice("EngineerBoots", 50);
		VirtualCurrencyHelper.AddPrice("EngineerBoots_Up1", 25);
		VirtualCurrencyHelper.AddPrice("EngineerBoots_Up2", 40);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_1", 70);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_2", 70);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_3", 70);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_1", 85);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_2", 85);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_3", 85);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_1", 100);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_2", 100);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_3", 100);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_1", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_2", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_3", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Novice", 10);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Rubin_1", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Rubin_2", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Rubin_3", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_Const_1", 170);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_Const_2", 170);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_Const_3", 170);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Army_4", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Steel_4", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Royal_4", 135);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Almaz_4", 120);
		VirtualCurrencyHelper.AddPriceForArmor("Armor_Adamant_3", 3);
		VirtualCurrencyHelper.AddPrice("hat_Rubin_1", 135);
		VirtualCurrencyHelper.AddPrice("hat_Rubin_2", 135);
		VirtualCurrencyHelper.AddPrice("hat_Rubin_3", 135);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_Const_1", 170);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_Const_2", 170);
		VirtualCurrencyHelper.AddPrice("hat_Adamant_Const_3", 170);
		VirtualCurrencyHelper.AddPrice(StickersController.KeyForBuyPack(TypePackSticker.classic), 20);
		VirtualCurrencyHelper.AddPrice(StickersController.KeyForBuyPack(TypePackSticker.christmas), 30);
		VirtualCurrencyHelper.AddPrice(StickersController.KeyForBuyPack(TypePackSticker.easter), 40);
		VirtualCurrencyHelper.AddPrice(Defs.BuyGiftKey, 5);
		for (int j = 0; j < 11; j++)
		{
			VirtualCurrencyHelper.AddPrice(string.Concat("newskin_", j), 20);
		}
		for (int k = 11; k < 19; k++)
		{
			VirtualCurrencyHelper.AddPrice(string.Concat("newskin_", k), 20);
		}
		VirtualCurrencyHelper._prng = null;
	}

	public VirtualCurrencyHelper()
	{
	}

	private static void AddPrice(string key, int value)
	{
		VirtualCurrencyHelper.prices.Add(key, new SaltedInt(VirtualCurrencyHelper._prng.Next(), value));
	}

	private static void AddPriceForArmor(string armor, int amount)
	{
		if (armor == null)
		{
			Debug.LogError("AddPriceForArmor armor == null");
			return;
		}
		VirtualCurrencyHelper._armorPricesDefault.Add(armor, new ItemPrice(amount, (armor != "Armor_Adamant_3" ? "Coins" : "GemsCurrency")));
	}

	private static int CoefInappsQuantity()
	{
		if (PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active)
		{
			return 3;
		}
		return 1;
	}

	public static int GetCoinInappsQuantity(int inappIndex)
	{
		if (PromoActionsManager.sharedManager == null)
		{
			Debug.LogError("GetCoinInappsQuantity: PromoActionsManager.sharedManager == null when calculating");
		}
		return VirtualCurrencyHelper.CoefInappsQuantity() * VirtualCurrencyHelper.coinInappsQuantity[inappIndex];
	}

	public static int GetGemsInappsQuantity(int inappIndex)
	{
		if (PromoActionsManager.sharedManager == null)
		{
			Debug.LogError("GetGemsInappsQuantity: PromoActionsManager.sharedManager == null when calculating");
		}
		return VirtualCurrencyHelper.CoefInappsQuantity() * VirtualCurrencyHelper.gemsInappsQuantity[inappIndex];
	}

	public static int[] InitCoinInappsQuantity(int[] _mass)
	{
		int[] numArray = new int[(int)_mass.Length];
		Array.Copy(_mass, numArray, (int)_mass.Length);
		return numArray;
	}

	public static int[] InitGemsInappsQuantity(int[] _mass)
	{
		int[] numArray = new int[(int)_mass.Length];
		Array.Copy(_mass, numArray, (int)_mass.Length);
		return numArray;
	}

	private static Dictionary<string, decimal> InitializeReferencePrices()
	{
		Dictionary<string, decimal> strs = new Dictionary<string, decimal>()
		{
			{ "coin1", new decimal(99, 0, 0, false, 2) },
			{ "coin7", new decimal(299, 0, 0, false, 2) },
			{ "coin2", new decimal(499, 0, 0, false, 2) },
			{ "coin4", new decimal(1999, 0, 0, false, 2) },
			{ "coin5", new decimal(4999, 0, 0, false, 2) },
			{ "coin8", new decimal(9999, 0, 0, false, 2) },
			{ "gem1", new decimal(99, 0, 0, false, 2) },
			{ "gem2", new decimal(299, 0, 0, false, 2) },
			{ "gem3", new decimal(499, 0, 0, false, 2) },
			{ "gem4", new decimal(999, 0, 0, false, 2) },
			{ "gem5", new decimal(1999, 0, 0, false, 2) },
			{ "gem6", new decimal(4999, 0, 0, false, 2) },
			{ "gem7", new decimal(9999, 0, 0, false, 2) },
			{ "starterpack8", new decimal(99, 0, 0, false, 2) },
			{ "starterpack7", new decimal(99, 0, 0, false, 2) },
			{ "starterpack5", new decimal(199, 0, 0, false, 2) },
			{ "starterpack3", new decimal(399, 0, 0, false, 2) },
			{ "starterpack1", new decimal(599, 0, 0, false, 2) }
		};
		Dictionary<string, decimal> strs1 = strs;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			strs1.Add("coin3.", new decimal(999, 0, 0, false, 2));
			strs1.Add("starterpack6", new decimal(99, 0, 0, false, 2));
			strs1.Add("starterpack4", new decimal(299, 0, 0, false, 2));
			strs1.Add("starterpack2", new decimal(499, 0, 0, false, 2));
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			strs1.Add("coin3", new decimal(999, 0, 0, false, 2));
			strs1.Add("starterpack10", new decimal(299, 0, 0, false, 2));
			strs1.Add("starterpack9", new decimal(499, 0, 0, false, 2));
		}
		return strs1;
	}

	public static ItemPrice Price(string key)
	{
		if (key == null)
		{
			return null;
		}
		if (Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory].SelectMany<List<string>, string>((List<string> list) => list).ToList<string>().Contains(key))
		{
			if (Defs2.ArmorPricesFromServer != null && Defs2.ArmorPricesFromServer.ContainsKey(key))
			{
				ItemPrice item = Defs2.ArmorPricesFromServer[key];
				if (item != null)
				{
					return item;
				}
				Debug.LogError(string.Concat("armorPriceFromServer == null, armor = ", key));
			}
			return VirtualCurrencyHelper._armorPricesDefault[key];
		}
		if (!VirtualCurrencyHelper.prices.ContainsKey(key))
		{
			return null;
		}
		int value = VirtualCurrencyHelper.prices[key].Value;
		string str = "Coins";
		string str1 = GearManager.HolderQuantityForID(key);
		bool flag = false;
		flag = (str1 == null || !GearManager.Gear.Contains<string>(str1) && !GearManager.DaterGear.Contains<string>(str1) || key.Contains(GearManager.UpgradeSuffix) ? false : !str1.Equals(GearManager.Grenade));
		if (key == "cape_Archimage" || key == "cape_BloodyDemon" || key == "cape_RoyalKnight" || key == "cape_SkeletonLord" || key == "cape_EliteCrafter" || key == "HitmanCape_Up1" || key == "BerserkCape_Up1" || key == "DemolitionCape_Up1" || key == "SniperCape_Up1" || key == "StormTrooperCape_Up1" || key == "HitmanCape_Up2" || key == "BerserkCape_Up2" || key == "DemolitionCape_Up2" || key == "SniperCape_Up2" || key == "StormTrooperCape_Up2" || key == "cape_Engineer" || key == "cape_Engineer_Up1" || key == "cape_Engineer_Up2")
		{
			flag = true;
		}
		if (key == "boots_red" || key == "boots_gray" || key == "boots_blue" || key == "boots_green" || key == "boots_black" || key == "HitmanBoots_Up1" || key == "StormTrooperBoots_Up1" || key == "SniperBoots_Up1" || key == "DemolitionBoots_Up1" || key == "BerserkBoots_Up1" || key == "HitmanBoots_Up2" || key == "StormTrooperBoots_Up2" || key == "SniperBoots_Up2" || key == "DemolitionBoots_Up2" || key == "BerserkBoots_Up2" || key == "EngineerBoots" || key == "EngineerBoots_Up1" || key == "EngineerBoots_Up2")
		{
			flag = true;
		}
		IEnumerable<string> strs = Wear.wear[ShopNGUIController.CategoryNames.MaskCategory].SelectMany<List<string>, string>((List<string> l) => l);
		if (key != "hat_ManiacMask" && strs.Contains<string>(key))
		{
			flag = true;
		}
		if (key == StickersController.KeyForBuyPack(TypePackSticker.classic))
		{
			flag = true;
		}
		if (key == StickersController.KeyForBuyPack(TypePackSticker.christmas))
		{
			flag = true;
		}
		if (key == StickersController.KeyForBuyPack(TypePackSticker.easter))
		{
			flag = true;
		}
		if (key == Defs.BuyGiftKey)
		{
			flag = true;
		}
		if (TempItemsController.PriceCoefs.ContainsKey(key))
		{
			flag = true;
		}
		if (key != null && (key.Equals(PremiumAccountController.AccountType.OneDay.ToString()) || key.Equals(PremiumAccountController.AccountType.ThreeDay.ToString()) || key.Equals(PremiumAccountController.AccountType.SevenDays.ToString()) || key.Equals(PremiumAccountController.AccountType.Month.ToString())))
		{
			flag = true;
		}
		if (flag)
		{
			str = "GemsCurrency";
		}
		return new ItemPrice(value, str);
	}

	public static void ResetInappsQuantityOnDefault()
	{
		VirtualCurrencyHelper._gemsInappsQuantity = VirtualCurrencyHelper.InitGemsInappsQuantity(VirtualCurrencyHelper._gemsInappsQuantityDefault);
		VirtualCurrencyHelper._coinInappsQuantity = VirtualCurrencyHelper.InitCoinInappsQuantity(VirtualCurrencyHelper._coinInappsQuantityDefault);
	}

	public static void RewriteInappsQuantity(int _priceId, int _coinQuantity, int _gemsQuantity, int _bonusCoins, int _bonusGems)
	{
		for (int i = 0; i < (int)VirtualCurrencyHelper.coinPriceIds.Length; i++)
		{
			if (VirtualCurrencyHelper.coinPriceIds[i] == _priceId)
			{
				VirtualCurrencyHelper._coinInappsQuantity[i] = _coinQuantity;
				VirtualCurrencyHelper._gemsInappsQuantity[i] = _gemsQuantity;
				BankView.discountsCoins[i] = _bonusCoins;
				BankView.discountsGems[i] = _bonusGems;
				return;
			}
		}
	}
}