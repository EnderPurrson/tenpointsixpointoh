using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public static class InAppData
{
	public static Dictionary<int, KeyValuePair<string, string>> inAppData;

	public static Dictionary<string, string> inappReadableNames;

	static InAppData()
	{
		InAppData.inAppData = new Dictionary<int, KeyValuePair<string, string>>();
		InAppData.inappReadableNames = new Dictionary<string, string>();
		InAppData.inAppData.Add(5, new KeyValuePair<string, string>(StoreKitEventListener.endmanskin, Defs.endmanskinBoughtSett));
		InAppData.inAppData.Add(11, new KeyValuePair<string, string>(StoreKitEventListener.chief, Defs.chiefBoughtSett));
		InAppData.inAppData.Add(12, new KeyValuePair<string, string>(StoreKitEventListener.spaceengineer, Defs.spaceengineerBoughtSett));
		InAppData.inAppData.Add(13, new KeyValuePair<string, string>(StoreKitEventListener.nanosoldier, Defs.nanosoldierBoughtSett));
		InAppData.inAppData.Add(14, new KeyValuePair<string, string>(StoreKitEventListener.steelman, Defs.steelmanBoughtSett));
		InAppData.inAppData.Add(15, new KeyValuePair<string, string>(StoreKitEventListener.CaptainSkin, Defs.captainSett));
		InAppData.inAppData.Add(16, new KeyValuePair<string, string>(StoreKitEventListener.HawkSkin, Defs.hawkSett));
		InAppData.inAppData.Add(17, new KeyValuePair<string, string>(StoreKitEventListener.GreenGuySkin, Defs.greenGuySett));
		InAppData.inAppData.Add(18, new KeyValuePair<string, string>(StoreKitEventListener.TunderGodSkin, Defs.TunderGodSett));
		InAppData.inAppData.Add(19, new KeyValuePair<string, string>(StoreKitEventListener.GordonSkin, Defs.gordonSett));
		InAppData.inAppData.Add(23, new KeyValuePair<string, string>(StoreKitEventListener.magicGirl, Defs.magicGirlSett));
		InAppData.inAppData.Add(24, new KeyValuePair<string, string>(StoreKitEventListener.braveGirl, Defs.braveGirlSett));
		InAppData.inAppData.Add(25, new KeyValuePair<string, string>(StoreKitEventListener.glamDoll, Defs.glamGirlSett));
		InAppData.inAppData.Add(26, new KeyValuePair<string, string>(StoreKitEventListener.kittyGirl, Defs.kityyGirlSett));
		InAppData.inAppData.Add(27, new KeyValuePair<string, string>(StoreKitEventListener.famosBoy, Defs.famosBoySett));
		for (int i = 0; i < 11; i++)
		{
			InAppData.inAppData.Add(29 + i - 1, new KeyValuePair<string, string>(string.Concat("newskin_", i), string.Concat("newskin_", i)));
		}
		for (int j = 11; j < 19; j++)
		{
			InAppData.inAppData.Add(29 + j - 1, new KeyValuePair<string, string>(string.Concat("newskin_", j), string.Concat("newskin_", j)));
		}
		InAppData.inAppData.Add(47, new KeyValuePair<string, string>(StoreKitEventListener.skin810_1, Defs.skin810_1));
		InAppData.inAppData.Add(48, new KeyValuePair<string, string>(StoreKitEventListener.skin810_2, Defs.skin810_2));
		InAppData.inAppData.Add(49, new KeyValuePair<string, string>(StoreKitEventListener.skin810_3, Defs.skin810_3));
		InAppData.inAppData.Add(50, new KeyValuePair<string, string>(StoreKitEventListener.skin810_4, Defs.skin810_4));
		InAppData.inAppData.Add(51, new KeyValuePair<string, string>(StoreKitEventListener.skin810_5, Defs.skin810_5));
		InAppData.inAppData.Add(52, new KeyValuePair<string, string>(StoreKitEventListener.skin810_6, Defs.skin810_6));
		InAppData.inAppData.Add(53, new KeyValuePair<string, string>(StoreKitEventListener.skin931_1, Defs.skin931_1));
		InAppData.inAppData.Add(54, new KeyValuePair<string, string>(StoreKitEventListener.skin931_2, Defs.skin931_2));
		for (int k = 0; k < (int)StoreKitEventListener.Skins_11_040915.Length; k++)
		{
			InAppData.inAppData.Add(55 + k, new KeyValuePair<string, string>(StoreKitEventListener.Skins_11_040915[k], Defs.Skins_11_040915[k]));
		}
		InAppData.inAppData.Add(61, new KeyValuePair<string, string>("super_socialman", "super_socialman"));
		InAppData.inAppData.Add(62, new KeyValuePair<string, string>(StoreKitEventListener.skin_tiger, Defs.skin_tiger));
		InAppData.inAppData.Add(63, new KeyValuePair<string, string>(StoreKitEventListener.skin_pitbull, Defs.skin_pitbull));
		InAppData.inAppData.Add(64, new KeyValuePair<string, string>(StoreKitEventListener.skin_santa, Defs.skin_santa));
		InAppData.inAppData.Add(65, new KeyValuePair<string, string>(StoreKitEventListener.skin_elf_new_year, Defs.skin_elf_new_year));
		InAppData.inAppData.Add(66, new KeyValuePair<string, string>(StoreKitEventListener.skin_girl_new_year, Defs.skin_girl_new_year));
		InAppData.inAppData.Add(67, new KeyValuePair<string, string>(StoreKitEventListener.skin_cookie_new_year, Defs.skin_cookie_new_year));
		InAppData.inAppData.Add(68, new KeyValuePair<string, string>(StoreKitEventListener.skin_snowman_new_year, Defs.skin_snowman_new_year));
		InAppData.inAppData.Add(69, new KeyValuePair<string, string>(StoreKitEventListener.skin_jetti_hnight, Defs.skin_jetti_hnight));
		InAppData.inAppData.Add(70, new KeyValuePair<string, string>(StoreKitEventListener.skin_startrooper, Defs.skin_startrooper));
		InAppData.inAppData.Add(71, new KeyValuePair<string, string>(StoreKitEventListener.easter_skin1, Defs.easter_skin1));
		InAppData.inAppData.Add(72, new KeyValuePair<string, string>(StoreKitEventListener.easter_skin2, Defs.easter_skin2));
		InAppData.inAppData.Add(73, new KeyValuePair<string, string>(StoreKitEventListener.skin_rapid_girl, Defs.skin_rapid_girl));
		InAppData.inAppData.Add(74, new KeyValuePair<string, string>(StoreKitEventListener.skin_silent_killer, Defs.skin_silent_killer));
		InAppData.inAppData.Add(75, new KeyValuePair<string, string>(StoreKitEventListener.skin_daemon_fighter, Defs.skin_daemon_fighter));
		InAppData.inAppData.Add(76, new KeyValuePair<string, string>(StoreKitEventListener.skin_scary_demon, Defs.skin_scary_demon));
		InAppData.inAppData.Add(77, new KeyValuePair<string, string>(StoreKitEventListener.skin_orc_warrior, Defs.skin_orc_warrior));
		InAppData.inAppData.Add(78, new KeyValuePair<string, string>(StoreKitEventListener.skin_kung_fu_master, Defs.skin_kung_fu_master));
		InAppData.inAppData.Add(79, new KeyValuePair<string, string>(StoreKitEventListener.skin_fire_wizard, Defs.skin_fire_wizard));
		InAppData.inAppData.Add(80, new KeyValuePair<string, string>(StoreKitEventListener.skin_ice_wizard, Defs.skin_ice_wizard));
		InAppData.inAppData.Add(81, new KeyValuePair<string, string>(StoreKitEventListener.skin_storm_wizard, Defs.skin_storm_wizard));
		InAppData.inAppData.Add(82, new KeyValuePair<string, string>(StoreKitEventListener.skin_may1, Defs.skin_may1));
		InAppData.inAppData.Add(83, new KeyValuePair<string, string>(StoreKitEventListener.skin_may2, Defs.skin_may2));
		InAppData.inAppData.Add(84, new KeyValuePair<string, string>(StoreKitEventListener.skin_may3, Defs.skin_may3));
		InAppData.inAppData.Add(85, new KeyValuePair<string, string>(StoreKitEventListener.skin_may4, Defs.skin_may4));
		InAppData.inAppData.Add(86, new KeyValuePair<string, string>("skin_july1", "skin_july1"));
		InAppData.inAppData.Add(87, new KeyValuePair<string, string>("skin_july2", "skin_july2"));
		InAppData.inAppData.Add(88, new KeyValuePair<string, string>("skin_july3", "skin_july3"));
		InAppData.inAppData.Add(89, new KeyValuePair<string, string>("skin_july4", "skin_july4"));
		InAppData.inappReadableNames.Add("bigammopack", "Big Pack of Ammo");
		InAppData.inappReadableNames.Add("Fullhealth", "Full Health");
		InAppData.inappReadableNames.Add(StoreKitEventListener.elixirID, "Elixir of Resurrection");
		InAppData.inappReadableNames.Add(StoreKitEventListener.armor, "Armor");
		InAppData.inappReadableNames.Add(StoreKitEventListener.armor2, "Armor2");
		InAppData.inappReadableNames.Add(StoreKitEventListener.armor3, "Armor3");
		InAppData.inappReadableNames.Add(StoreKitEventListener.endmanskin, "End Man Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.chief, "Chief Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.spaceengineer, "Space Engineer Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.nanosoldier, "Nano Soldier Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.steelman, "Steel Man Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.CaptainSkin, "Captain Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.HawkSkin, "Hawk Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.TunderGodSkin, "Thunder God Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.GreenGuySkin, "Green Guy Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.GordonSkin, "Gordon Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.magicGirl, "Magic Girl Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.braveGirl, "Brave Girl Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.glamDoll, "Glam Doll Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.kittyGirl, "Kitty Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.famosBoy, "Famos Boy Skin for 40 coins");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin810_1, "skin810_1");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin810_2, "skin810_2");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin810_3, "skin810_3");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin810_4, "skin810_4");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin810_5, "skin810_5");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin810_6, "skin810_6");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_santa, "skin_santa");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_elf_new_year, "skin_elf_new_year");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_girl_new_year, "skin_girl_new_year");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_cookie_new_year, "skin_cookie_new_year");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_snowman_new_year, "skin_snowman_new_year");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_jetti_hnight, FlurryPluginWrapper.ConvertFromBase64("c2tpbiBqZWRpIGtuaWdodA=="));
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_startrooper, FlurryPluginWrapper.ConvertFromBase64("c2tpbiBzdGFyd2FycyBzdG9ybXRyb29wZXI="));
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin931_1, "skin931_1");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin931_2, "skin931_2");
		for (int l = 0; l < (int)StoreKitEventListener.Skins_11_040915.Length; l++)
		{
			InAppData.inappReadableNames.Add(StoreKitEventListener.Skins_11_040915[l], StoreKitEventListener.Skins_11_040915[l]);
		}
		InAppData.inappReadableNames.Add("super_socialman", "super_socialman");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_tiger, StoreKitEventListener.skin_tiger);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_pitbull, StoreKitEventListener.skin_pitbull);
		InAppData.inappReadableNames.Add(StoreKitEventListener.easter_skin1, "easter_skin1");
		InAppData.inappReadableNames.Add(StoreKitEventListener.easter_skin2, "easter_skin2");
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_rapid_girl, StoreKitEventListener.skin_rapid_girl);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_daemon_fighter, StoreKitEventListener.skin_daemon_fighter);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_scary_demon, StoreKitEventListener.skin_scary_demon);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_silent_killer, StoreKitEventListener.skin_silent_killer);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_orc_warrior, StoreKitEventListener.skin_orc_warrior);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_kung_fu_master, StoreKitEventListener.skin_kung_fu_master);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_fire_wizard, StoreKitEventListener.skin_fire_wizard);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_ice_wizard, StoreKitEventListener.skin_ice_wizard);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_storm_wizard, StoreKitEventListener.skin_storm_wizard);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_may1, StoreKitEventListener.skin_may1);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_may2, StoreKitEventListener.skin_may2);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_may3, StoreKitEventListener.skin_may3);
		InAppData.inappReadableNames.Add(StoreKitEventListener.skin_may4, StoreKitEventListener.skin_may4);
		InAppData.inappReadableNames.Add("skin_july1", "skin_july1");
		InAppData.inappReadableNames.Add("skin_july2", "skin_july2");
		InAppData.inappReadableNames.Add("skin_july3", "skin_july3");
		InAppData.inappReadableNames.Add("skin_july4", "skin_july4");
		InAppData.inappReadableNames.Add("cape_Archimage", "Archimage Cape");
		InAppData.inappReadableNames.Add("cape_BloodyDemon", "Bloody Demon Cape");
		InAppData.inappReadableNames.Add("cape_RoyalKnight", "Royal Knight Cape");
		InAppData.inappReadableNames.Add("cape_SkeletonLord", "Skeleton Lord Cape");
		InAppData.inappReadableNames.Add("cape_EliteCrafter", "Elite Crafter Cape");
		InAppData.inappReadableNames.Add("cape_Custom", "Custom Cape");
		InAppData.inappReadableNames.Add("HitmanCape_Up1", "HitmanCape_Up1");
		InAppData.inappReadableNames.Add("BerserkCape_Up1", "BerserkCape_Up1");
		InAppData.inappReadableNames.Add("DemolitionCape_Up1", "DemolitionCape_Up1");
		InAppData.inappReadableNames.Add("cape_Engineer", "EngineerCape");
		InAppData.inappReadableNames.Add("cape_Engineer_Up1", "EngineerCape_Up1");
		InAppData.inappReadableNames.Add("cape_Engineer_Up2", "EngineerCape_Up2");
		InAppData.inappReadableNames.Add("SniperCape_Up1", "SniperCape_Up1");
		InAppData.inappReadableNames.Add("StormTrooperCape_Up1", "StormTrooperCape_Up1");
		InAppData.inappReadableNames.Add("HitmanCape_Up2", "HitmanCape_Up2");
		InAppData.inappReadableNames.Add("BerserkCape_Up2", "BerserkCape_Up2");
		InAppData.inappReadableNames.Add("DemolitionCape_Up2", "DemolitionCape_Up2");
		InAppData.inappReadableNames.Add("SniperCape_Up2", "SniperCape_Up2");
		InAppData.inappReadableNames.Add("StormTrooperCape_Up2", "StormTrooperCape_Up2");
		InAppData.inappReadableNames.Add("hat_Adamant_3", "hat_Adamant_3");
		InAppData.inappReadableNames.Add("hat_DiamondHelmet", "Diamond Helmet");
		InAppData.inappReadableNames.Add("hat_Headphones", "Headphones");
		InAppData.inappReadableNames.Add("hat_KingsCrown", "King's Crown");
		InAppData.inappReadableNames.Add("hat_SeriousManHat", "Leprechaun's Hat");
		InAppData.inappReadableNames.Add("hat_Samurai", "Samurais Helm");
		InAppData.inappReadableNames.Add("league2_hat_cowboyhat", "league2_hat_cowboyhat");
		InAppData.inappReadableNames.Add("league3_hat_afro", "league3_hat_afro");
		InAppData.inappReadableNames.Add("league4_hat_mushroom", "league4_hat_mushroom");
		InAppData.inappReadableNames.Add("league5_hat_brain", "league5_hat_brain");
		InAppData.inappReadableNames.Add("league6_hat_tiara", "league6_hat_tiara");
		InAppData.inappReadableNames.Add("hat_AlmazHelmet", "hat_AlmazHelmet");
		InAppData.inappReadableNames.Add("hat_ArmyHelmet", "hat_ArmyHelmet");
		InAppData.inappReadableNames.Add("hat_SteelHelmet", "hat_SteelHelmet");
		InAppData.inappReadableNames.Add("hat_GoldHelmet", "hat_GoldHelmet");
		InAppData.inappReadableNames.Add("hat_Army_1", "hat_Army_1");
		InAppData.inappReadableNames.Add("hat_Almaz_1", "hat_Almaz_1");
		InAppData.inappReadableNames.Add("hat_Steel_1", "hat_Steel_1");
		InAppData.inappReadableNames.Add("hat_Royal_1", "hat_Royal_1");
		InAppData.inappReadableNames.Add("hat_Army_2", "hat_Army_2");
		InAppData.inappReadableNames.Add("hat_Almaz_2", "hat_Almaz_2");
		InAppData.inappReadableNames.Add("hat_Steel_2", "hat_Steel_2");
		InAppData.inappReadableNames.Add("hat_Royal_2", "hat_Royal_2");
		InAppData.inappReadableNames.Add("hat_Army_3", "hat_Army_3");
		InAppData.inappReadableNames.Add("hat_Almaz_3", "hat_Almaz_3");
		InAppData.inappReadableNames.Add("hat_Steel_3", "hat_Steel_3");
		InAppData.inappReadableNames.Add("hat_Royal_3", "hat_Royal_3");
		InAppData.inappReadableNames.Add("hat_Army_4", "hat_Army_4");
		InAppData.inappReadableNames.Add("hat_Almaz_4", "hat_Almaz_4");
		InAppData.inappReadableNames.Add("hat_Steel_4", "hat_Steel_4");
		InAppData.inappReadableNames.Add("hat_Royal_4", "hat_Royal_4");
		InAppData.inappReadableNames.Add("hat_Rubin_1", "hat_Rubin_1");
		InAppData.inappReadableNames.Add("hat_Rubin_2", "hat_Rubin_2");
		InAppData.inappReadableNames.Add("hat_Rubin_3", "hat_Rubin_3");
		InAppData.inappReadableNames.Add("Armor_Steel_1", "Armor_Steel_1");
		InAppData.inappReadableNames.Add("Armor_Steel_2", "Armor_Steel_2");
		InAppData.inappReadableNames.Add("Armor_Steel_3", "Armor_Steel_3");
		InAppData.inappReadableNames.Add("Armor_Steel_4", "Armor_Steel_4");
		InAppData.inappReadableNames.Add("Armor_Royal_1", "Armor_Royal_1");
		InAppData.inappReadableNames.Add("Armor_Royal_2", "Armor_Royal_2");
		InAppData.inappReadableNames.Add("Armor_Royal_3", "Armor_Royal_3");
		InAppData.inappReadableNames.Add("Armor_Royal_4", "Armor_Royal_4");
		InAppData.inappReadableNames.Add("Armor_Almaz_1", "Armor_Almaz_1");
		InAppData.inappReadableNames.Add("Armor_Almaz_2", "Armor_Almaz_2");
		InAppData.inappReadableNames.Add("Armor_Almaz_3", "Armor_Almaz_3");
		InAppData.inappReadableNames.Add("Armor_Almaz_4", "Armor_Almaz_4");
		InAppData.inappReadableNames.Add("Armor_Army_1", "Armor_Army_1");
		InAppData.inappReadableNames.Add("Armor_Army_2", "Armor_Army_2");
		InAppData.inappReadableNames.Add("Armor_Army_3", "Armor_Army_3");
		InAppData.inappReadableNames.Add("Armor_Army_4", "Armor_Army_4");
		InAppData.inappReadableNames.Add("Armor_Novice", "Armor_Novice");
		InAppData.inappReadableNames.Add("Armor_Rubin_1", "Armor_Rubin_1");
		InAppData.inappReadableNames.Add("Armor_Rubin_2", "Armor_Rubin_2");
		InAppData.inappReadableNames.Add("Armor_Rubin_3", "Armor_Rubin_3");
		InAppData.inappReadableNames.Add("Armor_Adamant_Const_1", "Armor_Adamant_Const_1");
		InAppData.inappReadableNames.Add("Armor_Adamant_Const_2", "Armor_Adamant_Const_2");
		InAppData.inappReadableNames.Add("Armor_Adamant_Const_3", "Armor_Adamant_Const_3");
		InAppData.inappReadableNames.Add("hat_Adamant_Const_1", "hat_Adamant_Const_1");
		InAppData.inappReadableNames.Add("hat_Adamant_Const_2", "hat_Adamant_Const_2");
		InAppData.inappReadableNames.Add("hat_Adamant_Const_3", "hat_Adamant_Const_3");
		InAppData.inappReadableNames.Add("Armor_Adamant_3", "Armor_Adamant_3");
		string[] strArrays = PotionsController.potions;
		for (int m = 0; m < (int)strArrays.Length; m++)
		{
			string str = strArrays[m];
			InAppData.inappReadableNames.Add(str, str);
		}
		InAppData.inappReadableNames.Add("boots_red", "boots_red");
		InAppData.inappReadableNames.Add("boots_gray", "boots_gray");
		InAppData.inappReadableNames.Add("boots_blue", "boots_blue");
		InAppData.inappReadableNames.Add("boots_green", "boots_green");
		InAppData.inappReadableNames.Add("boots_black", "boots_black");
		InAppData.inappReadableNames.Add("boots_tabi", "boots ninja");
		InAppData.inappReadableNames.Add("HitmanBoots_Up1", "HitmanBoots_Up1");
		InAppData.inappReadableNames.Add("StormTrooperBoots_Up1", "StormTrooperBoots_Up1");
		InAppData.inappReadableNames.Add("SniperBoots_Up1", "SniperBoots_Up1");
		InAppData.inappReadableNames.Add("DemolitionBoots_Up1", "DemolitionBoots_Up1");
		InAppData.inappReadableNames.Add("BerserkBoots_Up1", "BerserkBoots_Up1");
		InAppData.inappReadableNames.Add("HitmanBoots_Up2", "HitmanBoots_Up2");
		InAppData.inappReadableNames.Add("StormTrooperBoots_Up2", "StormTrooperBoots_Up2");
		InAppData.inappReadableNames.Add("SniperBoots_Up2", "SniperBoots_Up2");
		InAppData.inappReadableNames.Add("DemolitionBoots_Up2", "DemolitionBoots_Up2");
		InAppData.inappReadableNames.Add("BerserkBoots_Up2", "BerserkBoots_Up2");
		InAppData.inappReadableNames.Add("EngineerBoots", "EngineerBoots");
		InAppData.inappReadableNames.Add("EngineerBoots_Up1", "EngineerBoots_Up1");
		InAppData.inappReadableNames.Add("EngineerBoots_Up2", "EngineerBoots_Up2");
		IEnumerator<string> enumerator = Wear.wear[ShopNGUIController.CategoryNames.MaskCategory].SelectMany<List<string>, string>((List<string> list) => list).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (current != "hat_ManiacMask")
				{
					InAppData.inappReadableNames.Add(current, current);
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
		InAppData.inappReadableNames.Add("hat_ManiacMask", "Maniac Mask");
		for (int n = 0; n < 11; n++)
		{
			InAppData.inappReadableNames.Add(string.Concat("newskin_", n), string.Concat("newskin_", n));
		}
		for (int o = 11; o < 19; o++)
		{
			InAppData.inappReadableNames.Add(string.Concat("newskin_", o), string.Concat("newskin_", o));
		}
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 1), string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 2), string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 3), string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 4), string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 5), string.Concat("InvisibilityPotion", GearManager.UpgradeSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 1), string.Concat("GrenadeID", GearManager.UpgradeSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 2), string.Concat("GrenadeID", GearManager.UpgradeSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 3), string.Concat("GrenadeID", GearManager.UpgradeSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 4), string.Concat("GrenadeID", GearManager.UpgradeSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.UpgradeSuffix, 5), string.Concat("GrenadeID", GearManager.UpgradeSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 1), string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 2), string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 3), string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 4), string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 5), string.Concat(GearManager.Turret, GearManager.UpgradeSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 1), string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 2), string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 3), string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 4), string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 5), string.Concat(GearManager.Mech, GearManager.UpgradeSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 1), string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 2), string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 3), string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 4), string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 5), string.Concat(GearManager.Jetpack, GearManager.UpgradeSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 0), string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 0));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 1), string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 2), string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 3), string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 4), string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 5), string.Concat("InvisibilityPotion", GearManager.OneItemSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.OneItemSuffix, 0), string.Concat("GrenadeID", GearManager.OneItemSuffix, 0));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.OneItemSuffix, 1), string.Concat("GrenadeID", GearManager.OneItemSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.OneItemSuffix, 2), string.Concat("GrenadeID", GearManager.OneItemSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.OneItemSuffix, 3), string.Concat("GrenadeID", GearManager.OneItemSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.OneItemSuffix, 4), string.Concat("GrenadeID", GearManager.OneItemSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat("GrenadeID", GearManager.OneItemSuffix, 5), string.Concat("GrenadeID", GearManager.OneItemSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 0), string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 0));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 1), string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 2), string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 3), string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 4), string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 5), string.Concat(GearManager.Turret, GearManager.OneItemSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 0), string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 0));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 1), string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 2), string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 3), string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 4), string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 5), string.Concat(GearManager.Mech, GearManager.OneItemSuffix, 5));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 0), string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 0));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 1), string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 1));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 2), string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 2));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 3), string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 3));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 4), string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 4));
		InAppData.inappReadableNames.Add(string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 5), string.Concat(GearManager.Jetpack, GearManager.OneItemSuffix, 5));
	}
}