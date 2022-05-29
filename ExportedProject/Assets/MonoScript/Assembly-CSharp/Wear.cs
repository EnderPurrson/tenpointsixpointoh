using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Wear
{
	public const int IndexOfArmorHatsList = 0;

	public const int NumberOfArmorsPerTier = 3;

	public const string BerserkCape = "cape_BloodyDemon";

	public const string DemolitionCape = "cape_RoyalKnight";

	public const string SniperCape = "cape_SkeletonLord";

	public const string HitmanCape = "cape_Archimage";

	public const string StormTrooperCape = "cape_EliteCrafter";

	public const string cape_Custom = "cape_Custom";

	public const string hat_Headphones = "hat_Headphones";

	public const string hat_ManiacMask = "hat_ManiacMask";

	public const string hat_KingsCrown = "hat_KingsCrown";

	public const string hat_Samurai = "hat_Samurai";

	public const string hat_DiamondHelmet = "hat_DiamondHelmet";

	public const string hat_SeriousManHat = "hat_SeriousManHat";

	public const string hat_AlmazHelmet = "hat_AlmazHelmet";

	public const string hat_ArmyHelmet = "hat_ArmyHelmet";

	public const string hat_GoldHelmet = "hat_GoldHelmet";

	public const string hat_SteelHelmet = "hat_SteelHelmet";

	public const string league2_hat_cowboyhat = "league2_hat_cowboyhat";

	public const string league3_hat_afro = "league3_hat_afro";

	public const string league4_hat_mushroom = "league4_hat_mushroom";

	public const string league5_hat_brain = "league5_hat_brain";

	public const string league6_hat_tiara = "league6_hat_tiara";

	public const string hat_Army_1 = "hat_Army_1";

	public const string hat_Royal_1 = "hat_Royal_1";

	public const string hat_Almaz_1 = "hat_Almaz_1";

	public const string hat_Steel_1 = "hat_Steel_1";

	public const string hat_Army_2 = "hat_Army_2";

	public const string hat_Royal_2 = "hat_Royal_2";

	public const string hat_Almaz_2 = "hat_Almaz_2";

	public const string hat_Steel_2 = "hat_Steel_2";

	public const string hat_Army_3 = "hat_Army_3";

	public const string hat_Royal_3 = "hat_Royal_3";

	public const string hat_Almaz_3 = "hat_Almaz_3";

	public const string hat_Steel_3 = "hat_Steel_3";

	public const string hat_Army_4 = "hat_Army_4";

	public const string hat_Royal_4 = "hat_Royal_4";

	public const string hat_Almaz_4 = "hat_Almaz_4";

	public const string hat_Steel_4 = "hat_Steel_4";

	public const string hat_Rubin_1 = "hat_Rubin_1";

	public const string hat_Rubin_2 = "hat_Rubin_2";

	public const string hat_Rubin_3 = "hat_Rubin_3";

	public const string BerserkBoots = "boots_black";

	public const string SniperBoots = "boots_blue";

	public const string StormTrooperBoots = "boots_gray";

	public const string DemolitionBoots = "boots_green";

	public const string HitmanBoots = "boots_red";

	public const string boots_tabi = "boots_tabi";

	public const string Armor_Steel = "Armor_Steel_1";

	public const string Armor_Steel_2 = "Armor_Steel_2";

	public const string Armor_Steel_3 = "Armor_Steel_3";

	public const string Armor_Steel_4 = "Armor_Steel_4";

	public const string Armor_Royal_1 = "Armor_Royal_1";

	public const string Armor_Royal_2 = "Armor_Royal_2";

	public const string Armor_Royal_3 = "Armor_Royal_3";

	public const string Armor_Royal_4 = "Armor_Royal_4";

	public const string Armor_Almaz_1 = "Armor_Almaz_1";

	public const string Armor_Almaz_2 = "Armor_Almaz_2";

	public const string Armor_Almaz_3 = "Armor_Almaz_3";

	public const string Armor_Almaz_4 = "Armor_Almaz_4";

	public const string Armor_Army_1 = "Armor_Army_1";

	public const string Armor_Army_2 = "Armor_Army_2";

	public const string Armor_Army_3 = "Armor_Army_3";

	public const string Armor_Army_4 = "Armor_Army_4";

	public const string Armor_Rubin_1 = "Armor_Rubin_1";

	public const string Armor_Rubin_2 = "Armor_Rubin_2";

	public const string Armor_Rubin_3 = "Armor_Rubin_3";

	public const string StormTrooperCape_Up1 = "StormTrooperCape_Up1";

	public const string StormTrooperCape_Up2 = "StormTrooperCape_Up2";

	public const string HitmanCape_Up1 = "HitmanCape_Up1";

	public const string HitmanCape_Up2 = "HitmanCape_Up2";

	public const string BerserkCape_Up1 = "BerserkCape_Up1";

	public const string BerserkCape_Up2 = "BerserkCape_Up2";

	public const string SniperCape_Up1 = "SniperCape_Up1";

	public const string SniperCape_Up2 = "SniperCape_Up2";

	public const string EngineerCape = "cape_Engineer";

	public const string EngineerCape_Up1 = "cape_Engineer_Up1";

	public const string EngineerCape_Up2 = "cape_Engineer_Up2";

	public const string DemolitionCape_Up1 = "DemolitionCape_Up1";

	public const string DemolitionCape_Up2 = "DemolitionCape_Up2";

	public const string hat_Headphones_Up1 = "hat_Headphones_Up1";

	public const string hat_Headphones_Up2 = "hat_Headphones_Up2";

	public const string hat_ManiacMask_Up1 = "hat_ManiacMask_Up1";

	public const string hat_ManiacMask_Up2 = "hat_ManiacMask_Up2";

	public const string hat_KingsCrown_Up1 = "hat_KingsCrown_Up1";

	public const string hat_KingsCrown_Up2 = "hat_KingsCrown_Up2";

	public const string hat_Samurai_Up1 = "hat_Samurai_Up1";

	public const string hat_Samurai_Up2 = "hat_Samurai_Up2";

	public const string hat_DiamondHelmet_Up1 = "hat_DiamondHelmet_Up1";

	public const string hat_DiamondHelmet_Up2 = "hat_DiamondHelmet_Up2";

	public const string hat_SeriousManHat_Up1 = "hat_SeriousManHat_Up1";

	public const string hat_SeriousManHat_Up2 = "hat_SeriousManHat_Up2";

	public const string StormTrooperBoots_Up1 = "StormTrooperBoots_Up1";

	public const string StormTrooperBoots_Up2 = "StormTrooperBoots_Up2";

	public const string HitmanBoots_Up1 = "HitmanBoots_Up1";

	public const string HitmanBoots_Up2 = "HitmanBoots_Up2";

	public const string BerserkBoots_Up1 = "BerserkBoots_Up1";

	public const string BerserkBoots_Up2 = "BerserkBoots_Up2";

	public const string SniperBoots_Up1 = "SniperBoots_Up1";

	public const string SniperBoots_Up2 = "SniperBoots_Up2";

	public const string DemolitionBoots_Up1 = "DemolitionBoots_Up1";

	public const string DemolitionBoots_Up2 = "DemolitionBoots_Up2";

	public const string EngineerBoots = "EngineerBoots";

	public const string EngineerBoots_Up1 = "EngineerBoots_Up1";

	public const string EngineerBoots_Up2 = "EngineerBoots_Up2";

	public const string Armor_Novice = "Armor_Novice";

	public const string Armor_Adamant_3 = "Armor_Adamant_3";

	public const string hat_Adamant_3 = "hat_Adamant_3";

	public const string Armor_Adamant_Const_1 = "Armor_Adamant_Const_1";

	public const string Armor_Adamant_Const_2 = "Armor_Adamant_Const_2";

	public const string Armor_Adamant_Const_3 = "Armor_Adamant_Const_3";

	public const string hat_Adamant_Const_1 = "hat_Adamant_Const_1";

	public const string hat_Adamant_Const_2 = "hat_Adamant_Const_2";

	public const string hat_Adamant_Const_3 = "hat_Adamant_Const_3";

	public const string SniperMask = "mask_sniper";

	public const string SniperMask_Up1 = "mask_sniper_up1";

	public const string SniperMask_Up2 = "mask_sniper_up2";

	public const string DemolitionMask = "mask_demolition";

	public const string DemolitionMask_Up1 = "mask_demolition_up1";

	public const string DemolitionMask_Up2 = "mask_demolition_up2";

	public const string BerserkMask = "mask_berserk";

	public const string BerserkMask_Up1 = "mask_berserk_up1";

	public const string BerserkMask_Up2 = "mask_berserk_up2";

	public const string StormTrooperMask = "mask_trooper";

	public const string StormTrooperMask_Up1 = "mask_trooper_up1";

	public const string StormTrooperMask_Up2 = "mask_trooper_up2";

	public const string HitmanMask = "mask_hitman_1";

	public const string HitmanMask_Up1 = "mask_hitman_1_up1";

	public const string HitmanMask_Up2 = "mask_hitman_1_up2";

	public const string EngineerMask = "mask_engineer";

	public const string EngineerMask_Up1 = "mask_engineer_up1";

	public const string EngineerMask_Up2 = "mask_engineer_up2";

	public static Dictionary<string, string> descriptionLocalizationKeys;

	public readonly static Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> wear;

	public static Dictionary<string, float> armorNum;

	public static Dictionary<string, List<float>> armorNumTemp;

	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> bootsMethods;

	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> capesMethods;

	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> hatsMethods;

	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> armorMethods;

	public static Dictionary<string, float> curArmor;

	static Wear()
	{
		Dictionary<string, string> strs = new Dictionary<string, string>()
		{
			{ "boots_tabi", "Key_1816" },
			{ "boots_blue", "Key_1164" },
			{ "SniperBoots_Up1", "Key_1165" },
			{ "SniperBoots_Up2", "Key_1166" },
			{ "boots_green", "Key_1167" },
			{ "DemolitionBoots_Up1", "Key_1168" },
			{ "DemolitionBoots_Up2", "Key_1169" },
			{ "boots_black", "Key_1170" },
			{ "BerserkBoots_Up1", "Key_1171" },
			{ "BerserkBoots_Up2", "Key_1172" },
			{ "boots_gray", "Key_1173" },
			{ "StormTrooperBoots_Up1", "Key_1174" },
			{ "StormTrooperBoots_Up2", "Key_1175" },
			{ "boots_red", "Key_1176" },
			{ "HitmanBoots_Up1", "Key_1177" },
			{ "HitmanBoots_Up2", "Key_1178" },
			{ "EngineerBoots", "Key_1686" },
			{ "EngineerBoots_Up1", "Key_1687" },
			{ "EngineerBoots_Up2", "Key_1688" },
			{ "cape_Custom", "Key_1817" },
			{ "cape_SkeletonLord", "Key_1179" },
			{ "SniperCape_Up1", "Key_1180" },
			{ "SniperCape_Up2", "Key_1181" },
			{ "cape_RoyalKnight", "Key_1182" },
			{ "DemolitionCape_Up1", "Key_1183" },
			{ "DemolitionCape_Up2", "Key_1184" },
			{ "cape_BloodyDemon", "Key_1185" },
			{ "BerserkCape_Up1", "Key_1186" },
			{ "BerserkCape_Up2", "Key_1187" },
			{ "cape_EliteCrafter", "Key_1188" },
			{ "StormTrooperCape_Up1", "Key_1189" },
			{ "StormTrooperCape_Up2", "Key_1190" },
			{ "cape_Archimage", "Key_1191" },
			{ "HitmanCape_Up1", "Key_1192" },
			{ "HitmanCape_Up2", "Key_1193" },
			{ "cape_Engineer", "Key_1683" },
			{ "cape_Engineer_Up1", "Key_1684" },
			{ "cape_Engineer_Up2", "Key_1685" },
			{ "hat_DiamondHelmet", "Key_1822" },
			{ "hat_ManiacMask", "Key_1819" },
			{ "hat_KingsCrown", "Key_1820" },
			{ "hat_Samurai", "Key_1821" },
			{ "hat_SeriousManHat", "Key_1823" },
			{ "hat_Headphones", "Key_1818" },
			{ "league2_hat_cowboyhat", "Key_2174" },
			{ "league3_hat_afro", "Key_2175" },
			{ "league4_hat_mushroom", "Key_2176" },
			{ "league5_hat_brain", "Key_2177" },
			{ "league6_hat_tiara", "Key_2178" },
			{ "mask_sniper", "Key_1845" },
			{ "mask_sniper_up1", "Key_1896" },
			{ "mask_sniper_up2", "Key_1897" },
			{ "mask_demolition", "Key_1846" },
			{ "mask_demolition_up1", "Key_1898" },
			{ "mask_demolition_up2", "Key_1899" },
			{ "mask_berserk", "Key_1847" },
			{ "mask_berserk_up1", "Key_1900" },
			{ "mask_berserk_up2", "Key_1901" },
			{ "mask_trooper", "Key_1848" },
			{ "mask_trooper_up1", "Key_1902" },
			{ "mask_trooper_up2", "Key_1903" },
			{ "mask_hitman_1", "Key_1849" },
			{ "mask_hitman_1_up1", "Key_1904" },
			{ "mask_hitman_1_up2", "Key_1905" },
			{ "mask_engineer", "Key_1850" },
			{ "mask_engineer_up1", "Key_1906" },
			{ "mask_engineer_up2", "Key_1907" }
		};
		Wear.descriptionLocalizationKeys = strs;
		Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> categoryNames = new Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>();
		List<List<string>> lists = new List<List<string>>();
		List<string> strs1 = new List<string>()
		{
			"cape_Custom"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"cape_EliteCrafter",
			"StormTrooperCape_Up1",
			"StormTrooperCape_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"cape_Archimage",
			"HitmanCape_Up1",
			"HitmanCape_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"cape_BloodyDemon",
			"BerserkCape_Up1",
			"BerserkCape_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"cape_Engineer",
			"cape_Engineer_Up1",
			"cape_Engineer_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"cape_SkeletonLord",
			"SniperCape_Up1",
			"SniperCape_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"cape_RoyalKnight",
			"DemolitionCape_Up1",
			"DemolitionCape_Up2"
		};
		lists.Add(strs1);
		categoryNames.Add(ShopNGUIController.CategoryNames.CapesCategory, lists);
		lists = new List<List<string>>();
		strs1 = new List<string>()
		{
			"hat_Army_1",
			"hat_Army_2",
			"hat_Army_3",
			"hat_Steel_1",
			"hat_Steel_2",
			"hat_Steel_3",
			"hat_Royal_1",
			"hat_Royal_2",
			"hat_Royal_3",
			"hat_Almaz_1",
			"hat_Almaz_2",
			"hat_Almaz_3",
			"hat_Rubin_1",
			"hat_Rubin_2",
			"hat_Rubin_3",
			"hat_Adamant_Const_1",
			"hat_Adamant_Const_2",
			"hat_Adamant_Const_3"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"league2_hat_cowboyhat"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"league3_hat_afro"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"league4_hat_mushroom"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"league5_hat_brain"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"league6_hat_tiara"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_Adamant_3"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_Headphones"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_KingsCrown"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_Samurai"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_DiamondHelmet"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_SeriousManHat"
		};
		lists.Add(strs1);
		categoryNames.Add(ShopNGUIController.CategoryNames.HatsCategory, lists);
		lists = new List<List<string>>();
		strs1 = new List<string>()
		{
			"boots_gray",
			"StormTrooperBoots_Up1",
			"StormTrooperBoots_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"boots_red",
			"HitmanBoots_Up1",
			"HitmanBoots_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"boots_black",
			"BerserkBoots_Up1",
			"BerserkBoots_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"EngineerBoots",
			"EngineerBoots_Up1",
			"EngineerBoots_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"boots_blue",
			"SniperBoots_Up1",
			"SniperBoots_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"boots_green",
			"DemolitionBoots_Up1",
			"DemolitionBoots_Up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"boots_tabi"
		};
		lists.Add(strs1);
		categoryNames.Add(ShopNGUIController.CategoryNames.BootsCategory, lists);
		lists = new List<List<string>>();
		strs1 = new List<string>()
		{
			"Armor_Army_1",
			"Armor_Army_2",
			"Armor_Army_3",
			"Armor_Steel_1",
			"Armor_Steel_2",
			"Armor_Steel_3",
			"Armor_Royal_1",
			"Armor_Royal_2",
			"Armor_Royal_3",
			"Armor_Almaz_1",
			"Armor_Almaz_2",
			"Armor_Almaz_3",
			"Armor_Rubin_1",
			"Armor_Rubin_2",
			"Armor_Rubin_3",
			"Armor_Adamant_Const_1",
			"Armor_Adamant_Const_2",
			"Armor_Adamant_Const_3"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"Armor_Novice"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"Armor_Adamant_3"
		};
		lists.Add(strs1);
		categoryNames.Add(ShopNGUIController.CategoryNames.ArmorCategory, lists);
		lists = new List<List<string>>();
		strs1 = new List<string>()
		{
			"mask_trooper",
			"mask_trooper_up1",
			"mask_trooper_up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"mask_hitman_1",
			"mask_hitman_1_up1",
			"mask_hitman_1_up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"mask_berserk",
			"mask_berserk_up1",
			"mask_berserk_up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"mask_engineer",
			"mask_engineer_up1",
			"mask_engineer_up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"mask_sniper",
			"mask_sniper_up1",
			"mask_sniper_up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"mask_demolition",
			"mask_demolition_up1",
			"mask_demolition_up2"
		};
		lists.Add(strs1);
		strs1 = new List<string>()
		{
			"hat_ManiacMask"
		};
		lists.Add(strs1);
		categoryNames.Add(ShopNGUIController.CategoryNames.MaskCategory, lists);
		Wear.wear = categoryNames;
		Wear.armorNum = new Dictionary<string, float>();
		Dictionary<string, List<float>> strs2 = new Dictionary<string, List<float>>();
		List<float> singles = new List<float>()
		{
			5f,
			10f,
			16f,
			20f,
			25f
		};
		strs2.Add("Armor_Adamant_3", singles);
		singles = new List<float>()
		{
			5f,
			10f,
			16f,
			20f,
			25f
		};
		strs2.Add("hat_Adamant_3", singles);
		Wear.armorNumTemp = strs2;
		Wear.bootsMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();
		Wear.capesMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();
		Wear.hatsMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();
		Wear.armorMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();
		Wear.curArmor = new Dictionary<string, float>();
		Wear.bootsMethods.Add("boots_red", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_red), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_red)));
		Wear.bootsMethods.Add("boots_gray", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_grey), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_grey)));
		Wear.bootsMethods.Add("boots_blue", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_blue), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_blue)));
		Wear.bootsMethods.Add("boots_green", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_green), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_green)));
		Wear.bootsMethods.Add("boots_black", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.ActivateBoots_black), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivateBoots_black)));
		Wear.capesMethods.Add("cape_BloodyDemon", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_BloodyDemon), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_BloodyDemon)));
		Wear.capesMethods.Add("cape_RoyalKnight", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_RoyalKnight), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_RoyalKnight)));
		Wear.capesMethods.Add("cape_SkeletonLord", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_SkeletonLord), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_SkeletonLord)));
		Wear.capesMethods.Add("cape_Archimage", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_Archimage), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_Archimage)));
		Wear.capesMethods.Add("cape_EliteCrafter", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_EliteCrafter), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_EliteCrafter)));
		Wear.capesMethods.Add("cape_Custom", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_cape_Custom), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_cape_Custom)));
		Wear.hatsMethods.Add("hat_Adamant_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_EMPTY)));
		Wear.hatsMethods.Add("hat_Headphones", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_EMPTY)));
		Wear.hatsMethods.Add("hat_ManiacMask", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_ManiacMask), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_ManiacMask)));
		Wear.hatsMethods.Add("hat_KingsCrown", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_KingsCrown), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_KingsCrown)));
		Wear.hatsMethods.Add("hat_Samurai", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Samurai), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Samurai)));
		Wear.hatsMethods.Add("hat_DiamondHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_DiamondHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_DiamondHelmet)));
		Wear.hatsMethods.Add("hat_SeriousManHat", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_SeriousManHat), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_SeriousManHat)));
		Wear.hatsMethods.Add("league2_hat_cowboyhat", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league3_hat_afro", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league4_hat_mushroom", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league5_hat_brain", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("league6_hat_tiara", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.hatsMethods.Add("hat_AlmazHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_AlmazHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_AlmazHelmet)));
		Wear.hatsMethods.Add("hat_ArmyHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_ArmyHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_ArmyHelmet)));
		Wear.hatsMethods.Add("hat_GoldHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_GoldHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_GoldHelmet)));
		Wear.hatsMethods.Add("hat_SteelHelmet", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_SteelHelmet), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_SteelHelmet)));
		Wear.hatsMethods.Add("hat_Steel_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Steel_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Steel_1)));
		Wear.hatsMethods.Add("hat_Royal_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Royal_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Royal_1)));
		Wear.hatsMethods.Add("hat_Almaz_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_hat_Almaz_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_hat_Almaz_1)));
		Wear.bootsMethods.Add("boots_tabi", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_boots_tabi), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_boots_tabi)));
		Wear.armorMethods.Add("Armor_Adamant_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.armorMethods.Add("Armor_Steel_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_EMPTY), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_EMPTY)));
		Wear.armorMethods.Add("Armor_Steel_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Steel_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Steel_2)));
		Wear.armorMethods.Add("Armor_Steel_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Steel_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Steel_3)));
		Wear.armorMethods.Add("Armor_Steel_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Steel_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Steel_4)));
		Wear.armorMethods.Add("Armor_Royal_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_1)));
		Wear.armorMethods.Add("Armor_Royal_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_2)));
		Wear.armorMethods.Add("Armor_Royal_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_3)));
		Wear.armorMethods.Add("Armor_Royal_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Royal_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Royal_4)));
		Wear.armorMethods.Add("Armor_Almaz_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_1)));
		Wear.armorMethods.Add("Armor_Almaz_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_2)));
		Wear.armorMethods.Add("Armor_Almaz_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_3)));
		Wear.armorMethods.Add("Armor_Almaz_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_4)));
		Wear.armorMethods.Add("Armor_Novice", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Almaz_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Almaz_2)));
		Wear.armorMethods.Add("Armor_Army_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("Armor_Army_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("Armor_Army_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("Armor_Army_4", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_4), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_4)));
		Wear.armorMethods.Add("Armor_Rubin_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("Armor_Rubin_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("Armor_Rubin_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("Armor_Adamant_Const_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("Armor_Adamant_Const_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("Armor_Adamant_Const_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("hat_Rubin_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("hat_Rubin_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("hat_Rubin_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorMethods.Add("hat_Adamant_Const_1", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_1), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_1)));
		Wear.armorMethods.Add("hat_Adamant_Const_2", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_2), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_2)));
		Wear.armorMethods.Add("hat_Adamant_Const_3", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(Wear.Activate_Armor_Army_3), new Action<Player_move_c, Dictionary<string, object>>(Wear.deActivate_Armor_Army_3)));
		Wear.armorNum.Add("Armor_Novice", 0f);
		Wear.armorNum.Add("Armor_Army_1", 1f);
		Wear.armorNum.Add("Armor_Army_2", 2f);
		Wear.armorNum.Add("Armor_Army_3", 3f);
		Wear.armorNum.Add("Armor_Army_4", 9f);
		Wear.armorNum.Add("Armor_Steel_1", 4f);
		Wear.armorNum.Add("Armor_Steel_2", 5f);
		Wear.armorNum.Add("Armor_Steel_3", 8f);
		Wear.armorNum.Add("Armor_Steel_4", 27f);
		Wear.armorNum.Add("Armor_Royal_1", 9f);
		Wear.armorNum.Add("Armor_Royal_2", 10f);
		Wear.armorNum.Add("Armor_Royal_3", 14f);
		Wear.armorNum.Add("Armor_Royal_4", 63f);
		Wear.armorNum.Add("Armor_Almaz_1", 15f);
		Wear.armorNum.Add("Armor_Almaz_2", 16f);
		Wear.armorNum.Add("Armor_Almaz_3", 18f);
		Wear.armorNum.Add("Armor_Almaz_4", 133f);
		Wear.armorNum.Add("Armor_Rubin_1", 19f);
		Wear.armorNum.Add("Armor_Rubin_2", 20f);
		Wear.armorNum.Add("Armor_Rubin_3", 22f);
		Wear.armorNum.Add("Armor_Adamant_Const_1", 24f);
		Wear.armorNum.Add("Armor_Adamant_Const_2", 26f);
		Wear.armorNum.Add("Armor_Adamant_Const_3", 28f);
		Wear.armorNum.Add("hat_Army_1", 1f);
		Wear.armorNum.Add("hat_Steel_1", 4f);
		Wear.armorNum.Add("hat_Royal_1", 9f);
		Wear.armorNum.Add("hat_Almaz_1", 15f);
		Wear.armorNum.Add("hat_Army_2", 2f);
		Wear.armorNum.Add("hat_Steel_2", 5f);
		Wear.armorNum.Add("hat_Royal_2", 10f);
		Wear.armorNum.Add("hat_Almaz_2", 16f);
		Wear.armorNum.Add("hat_Army_3", 3f);
		Wear.armorNum.Add("hat_Steel_3", 8f);
		Wear.armorNum.Add("hat_Royal_3", 14f);
		Wear.armorNum.Add("hat_Almaz_3", 18f);
		Wear.armorNum.Add("hat_Army_4", 1f);
		Wear.armorNum.Add("hat_Steel_4", 1f);
		Wear.armorNum.Add("hat_Royal_4", 2f);
		Wear.armorNum.Add("hat_Almaz_4", 3f);
		Wear.armorNum.Add("hat_Rubin_1", 19f);
		Wear.armorNum.Add("hat_Rubin_2", 20f);
		Wear.armorNum.Add("hat_Rubin_3", 22f);
		Wear.armorNum.Add("hat_Adamant_Const_1", 24f);
		Wear.armorNum.Add("hat_Adamant_Const_2", 26f);
		Wear.armorNum.Add("hat_Adamant_Const_3", 28f);
	}

	public static void Activate_Armor_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Almaz_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Almaz_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Almaz_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Army_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Army_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Army_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Army_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Royal_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Royal_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Royal_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Steel_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Steel_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_Armor_Steel_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_boots_tabi(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_cape_Archimage(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_cape_BloodyDemon(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_cape_Custom(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		Player_move_c playerMoveC = move;
		playerMoveC.koofDamageWeaponFromPotoins = playerMoveC.koofDamageWeaponFromPotoins + 0.05f;
	}

	public static void Activate_cape_EliteCrafter(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_cape_RoyalKnight(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_cape_SkeletonLord(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_AlmazHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_ArmyHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_DiamondHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_GoldHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_KingsCrown(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		Player_move_c playerMoveC = move;
		playerMoveC.koofDamageWeaponFromPotoins = playerMoveC.koofDamageWeaponFromPotoins + 0.05f;
	}

	public static void Activate_hat_ManiacMask(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_Samurai(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		Player_move_c playerMoveC = move;
		playerMoveC.koofDamageWeaponFromPotoins = playerMoveC.koofDamageWeaponFromPotoins + 0.05f;
	}

	public static void Activate_hat_SeriousManHat(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_Steel_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void Activate_hat_SteelHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void ActivateBoots_black(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void ActivateBoots_blue(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void ActivateBoots_green(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void ActivateBoots_grey(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void ActivateBoots_red(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static List<string> AllWears(ShopNGUIController.CategoryNames category, bool onlyNonLeagueItems = true)
	{
		List<string> strs = new List<string>();
		strs = (
			from l in Wear.wear[category]
			from i in l
			select i).ToList<string>();
		if (onlyNonLeagueItems)
		{
			try
			{
				strs = (
					from item in strs
					where Wear.LeagueForWear(item, category) == 0
					select item).ToList<string>();
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in AllWears filtering onlyNonLeagueItems: ", exception));
			}
		}
		return strs;
	}

	public static List<string> AllWears(ShopNGUIController.CategoryNames category, int tier, bool includePreviousTiers_UNUSED = false, bool withoutUpgrades = false)
	{
		List<int> nums = new List<int>();
		for (int i = 0; i <= tier; i++)
		{
			nums.Add(i);
		}
		List<string> strs = new List<string>();
		foreach (string str in Wear.AllWears(category, true))
		{
			if (!nums.Contains(Wear.TierForWear(str)))
			{
				continue;
			}
			strs.Add(str);
		}
		if (withoutUpgrades)
		{
			List<List<string>> item = Wear.wear[category];
			for (int j = strs.Count; j > 0; j--)
			{
				string item1 = strs[j - 1];
				if (item.All<List<string>>((List<string> l) => l[0] != item1))
				{
					strs.Remove(item1);
				}
			}
		}
		return strs;
	}

	public static string ArmorOrArmorHatAvailableForBuy(ShopNGUIController.CategoryNames category)
	{
		string empty;
		if (category != ShopNGUIController.CategoryNames.ArmorCategory && category != ShopNGUIController.CategoryNames.HatsCategory)
		{
			Debug.LogError(string.Concat("ArmorOrArmorHatAvailableForBuy incorrect category ", category));
			return string.Empty;
		}
		if (category == ShopNGUIController.CategoryNames.ArmorCategory && ShopNGUIController.NoviceArmorAvailable)
		{
			return string.Empty;
		}
		try
		{
			string str = WeaponManager.LastBoughtTag(Wear.wear[category][0][0]);
			string str1 = WeaponManager.FirstUnboughtTag(Wear.wear[category][0][0]);
			if (str == null || !(str == str1))
			{
				empty = (Wear.TierForWear(str1) > ExpController.OurTierForAnyPlace() ? string.Empty : str1);
			}
			else
			{
				empty = string.Empty;
			}
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("ArmorOrArmorHatAvailableForBuy Exception: ", exception));
			empty = string.Empty;
		}
		return empty;
	}

	public static void deActivate_Armor_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Almaz_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Almaz_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Almaz_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Army_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Army_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Army_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Army_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Royal_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Royal_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Royal_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Steel_2(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Steel_3(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_Armor_Steel_4(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_boots_tabi(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_cape_Archimage(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_cape_BloodyDemon(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_cape_Custom(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		Player_move_c playerMoveC = move;
		playerMoveC.koofDamageWeaponFromPotoins = playerMoveC.koofDamageWeaponFromPotoins - 0.05f;
	}

	public static void deActivate_cape_EliteCrafter(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_cape_RoyalKnight(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_cape_SkeletonLord(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_Almaz_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_AlmazHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_ArmyHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_DiamondHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_EMPTY(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_GoldHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_KingsCrown(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		Player_move_c playerMoveC = move;
		playerMoveC.koofDamageWeaponFromPotoins = playerMoveC.koofDamageWeaponFromPotoins - 0.05f;
	}

	public static void deActivate_hat_ManiacMask(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_Royal_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_Samurai(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null || Defs.isHunger)
		{
			return;
		}
		Player_move_c playerMoveC = move;
		playerMoveC.koofDamageWeaponFromPotoins = playerMoveC.koofDamageWeaponFromPotoins - 0.05f;
	}

	public static void deActivate_hat_SeriousManHat(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_Steel_1(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivate_hat_SteelHelmet(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivateBoots_black(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivateBoots_blue(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivateBoots_green(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivateBoots_grey(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static void deActivateBoots_red(Player_move_c move, Dictionary<string, object> p)
	{
		if (move == null)
		{
			return;
		}
	}

	public static int GetArmorCountFor(string armorTag, string hatTag)
	{
		return (int)(Wear.MaxArmorForItem(armorTag, (int)ExpController.LevelsForTiers.Length - 1) + Wear.MaxArmorForItem(hatTag, (int)ExpController.LevelsForTiers.Length - 1));
	}

	public static int LeagueForWear(string name, ShopNGUIController.CategoryNames category)
	{
		if (name == null)
		{
			Debug.LogError("LeagueForWear: name == null");
			return 0;
		}
		ShopPositionParams infoForNonWeaponItem = null;
		try
		{
			infoForNonWeaponItem = ItemDb.GetInfoForNonWeaponItem(name, category);
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("LeagueForWear: Exception: ", exception));
		}
		return (infoForNonWeaponItem == null ? 0 : infoForNonWeaponItem.League);
	}

	public static Dictionary<Wear.LeagueItemState, List<string>> LeagueItems()
	{
		Dictionary<Wear.LeagueItemState, List<string>> leagueItemStates = new Dictionary<Wear.LeagueItemState, List<string>>();
		try
		{
			IEnumerable<string> strs = Wear.wear[ShopNGUIController.CategoryNames.HatsCategory].SelectMany<List<string>, string>((List<string> list) => list).Where<string>((string item) => Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory) > 0);
			leagueItemStates[Wear.LeagueItemState.Open] = (
				from item in strs
				where Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory) <= (int)RatingSystem.instance.currentLeague
				orderby Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)
				select item).ToList<string>();
			leagueItemStates[Wear.LeagueItemState.Closed] = (
				from item in strs.Except<string>(leagueItemStates[Wear.LeagueItemState.Open])
				orderby Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)
				select item).ToList<string>();
			leagueItemStates[Wear.LeagueItemState.Purchased] = (
				from item in strs
				where Storager.getInt(item, true) > 0
				orderby Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)
				select item).ToList<string>();
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Exception in UnboughtLeagueItems: ", exception));
		}
		return leagueItemStates;
	}

	public static Dictionary<RatingSystem.RatingLeague, List<string>> LeagueItemsByLeagues()
	{
		Dictionary<RatingSystem.RatingLeague, List<string>> ratingLeagues = new Dictionary<RatingSystem.RatingLeague, List<string>>();
		try
		{
			ratingLeagues = Wear.wear[ShopNGUIController.CategoryNames.HatsCategory].SelectMany<List<string>, string>((List<string> list) => list).GroupBy<string, int>((string item) => Wear.LeagueForWear(item, ShopNGUIController.CategoryNames.HatsCategory)).Where<IGrouping<int, string>>((IGrouping<int, string> grouping) => grouping.Key > 0).ToDictionary<IGrouping<int, string>, RatingSystem.RatingLeague, List<string>>((IGrouping<int, string> grouping) => (RatingSystem.RatingLeague)grouping.Key, (IGrouping<int, string> grouping) => grouping.ToList<string>());
			ratingLeagues[RatingSystem.RatingLeague.Wood] = new List<string>();
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Exception in LeagueItemsByLeagues: ", exception));
		}
		return ratingLeagues;
	}

	public static float MaxArmorForItem(string armorName, int roomTier)
	{
		float item = 0f;
		if (armorName == null || !TempItemsController.PriceCoefs.ContainsKey(armorName) || !(ExpController.Instance != null))
		{
			int num = Math.Min((ExpController.Instance == null ? (int)ExpController.LevelsForTiers.Length - 1 : ExpController.Instance.OurTier), roomTier);
			bool flag = false;
			List<string> strs = null;
			foreach (List<List<string>> value in Wear.wear.Values)
			{
				foreach (List<string> strs1 in value)
				{
					if (!strs1.Contains(armorName ?? string.Empty))
					{
						continue;
					}
					flag = true;
					strs = strs1;
					break;
				}
				if (!flag)
				{
					continue;
				}
				break;
			}
			if (strs != null)
			{
				if (strs.IndexOf(armorName ?? string.Empty) > 3 * (num + 1) - 1)
				{
					armorName = strs[3 * (num + 1) - 1];
				}
			}
			Wear.armorNum.TryGetValue(armorName ?? string.Empty, out item);
		}
		else if (Wear.armorNumTemp.ContainsKey(armorName) && Wear.armorNumTemp[armorName].Count > ExpController.Instance.OurTier)
		{
			item = Wear.armorNumTemp[armorName][Math.Min(roomTier, ExpController.Instance.OurTier)];
		}
		item *= EffectsController.IcnreaseEquippedArmorPercentage;
		return item;
	}

	public static bool NonArmorHat(string showHatTag)
	{
		bool flag;
		if (showHatTag != null)
		{
			if (!Wear.wear[ShopNGUIController.CategoryNames.HatsCategory].SelectMany<List<string>, string>((List<string> list) => list).Contains<string>(showHatTag) || !(showHatTag != "hat_Adamant_3"))
			{
				flag = false;
				return flag;
			}
			flag = !Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(showHatTag);
			return flag;
		}
		flag = false;
		return flag;
	}

	public static void RemoveTemporaryWear(string item)
	{
		int num = PromoActionsGUIController.CatForTg(item);
		if (num != -1 && item != null)
		{
			if (!Storager.hasKey(ShopNGUIController.SnForWearCategory((ShopNGUIController.CategoryNames)num)))
			{
				Storager.setString(ShopNGUIController.SnForWearCategory((ShopNGUIController.CategoryNames)num), ShopNGUIController.NoneEquippedForWearCategory((ShopNGUIController.CategoryNames)num), false);
			}
			if (Storager.getString(ShopNGUIController.SnForWearCategory((ShopNGUIController.CategoryNames)num), false).Equals(item))
			{
				ShopNGUIController.UnequipCurrentWearInCategory((ShopNGUIController.CategoryNames)num, false);
			}
		}
	}

	public static void RenewCurArmor(int roomTier)
	{
		Wear.curArmor.Clear();
		foreach (string key in Wear.armorNum.Keys)
		{
			Wear.curArmor.Add(key, (!Defs.isHunger ? Wear.MaxArmorForItem(key, roomTier) : 0f));
		}
		foreach (string str in Wear.armorNumTemp.Keys)
		{
			Wear.curArmor.Add(str, (!Defs.isHunger ? Wear.MaxArmorForItem(str, roomTier) : 0f));
		}
	}

	public static int TierForWear(string w)
	{
		int num;
		if (w == null)
		{
			return 0;
		}
		if (Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(w))
		{
			return Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(w) / 3;
		}
		if (Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Contains(w))
		{
			return Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(w) / 3;
		}
		Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>.Enumerator enumerator = Wear.wear.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> current = enumerator.Current;
				List<List<string>>.Enumerator enumerator1 = current.Value.GetEnumerator();
				try
				{
					while (enumerator1.MoveNext())
					{
						List<string> strs = enumerator1.Current;
						if (!strs.Contains(w))
						{
							continue;
						}
						num = (current.Key != ShopNGUIController.CategoryNames.MaskCategory ? strs.IndexOf(w) : strs.IndexOf(w) * 2);
						return num;
					}
				}
				finally
				{
					((IDisposable)(object)enumerator1).Dispose();
				}
			}
			return 0;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return num;
	}

	public static Dictionary<RatingSystem.RatingLeague, List<string>> UnboughtLeagueItemsByLeagues()
	{
		Dictionary<RatingSystem.RatingLeague, List<string>> dictionary = Wear.LeagueItemsByLeagues();
		try
		{
			dictionary = dictionary.ToDictionary<KeyValuePair<RatingSystem.RatingLeague, List<string>>, RatingSystem.RatingLeague, List<string>>((KeyValuePair<RatingSystem.RatingLeague, List<string>> kvp) => kvp.Key, (KeyValuePair<RatingSystem.RatingLeague, List<string>> kvp) => (
				from item in kvp.Value
				where Storager.getInt(item, true) == 0
				select item).ToList<string>());
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Exception in UnboughtLeagueItemsByLeagues: ", exception));
		}
		return dictionary;
	}

	public enum LeagueItemState
	{
		Open,
		Closed,
		Purchased
	}
}