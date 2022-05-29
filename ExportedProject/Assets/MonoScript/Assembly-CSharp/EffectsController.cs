using System;
using System.Collections.Generic;

public static class EffectsController
{
	private static float slowdownCoeff;

	public static float ArmorBonus
	{
		get
		{
			float single = 0f;
			if (Defs.isHunger)
			{
				return single;
			}
			if (Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara"))
			{
				single += 3f;
			}
			if (Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_red"))
			{
				single += 1f;
			}
			if (Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up1"))
			{
				single += 2f;
			}
			if (Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up2"))
			{
				single += 3f;
			}
			if (Storager.getString("MaskEquippedSN", false).Equals("mask_berserk"))
			{
				single += 1f;
			}
			else if (Storager.getString("MaskEquippedSN", false).Equals("mask_berserk_up1"))
			{
				single += 2f;
			}
			else if (Storager.getString("MaskEquippedSN", false).Equals("mask_berserk_up2"))
			{
				single += 3f;
			}
			return single;
		}
	}

	public static float ExplosionImpulseRadiusIncreaseCoef
	{
		get
		{
			if (Defs.isHunger)
			{
				return 0f;
			}
			return (!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_green") ? 0f : 0.05f) + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up1") ? 0f : 0.1f) + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up2") ? 0f : 0.15f) + (!Storager.getString(Defs.HatEquppedSN, false).Equals("league3_hat_afro") ? 0f : 0.08f);
		}
	}

	public static float GrenadeExplosionDamageIncreaseCoef
	{
		get
		{
			if (Defs.isHunger)
			{
				return 0f;
			}
			return (!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_green") ? 0f : 0.1f) + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up1") ? 0f : 0.15f) + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up2") ? 0f : 0.25f);
		}
	}

	public static float GrenadeExplosionRadiusIncreaseCoef
	{
		get
		{
			float single = 1f;
			if (Defs.isHunger)
			{
				return single;
			}
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_RoyalKnight") ? 0f : 0.2f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up1") ? 0f : 0.3f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up2") ? 0f : 0.5f);
			return single;
		}
	}

	public static float IcnreaseEquippedArmorPercentage
	{
		get
		{
			float single = 1f;
			if (Defs.isHunger)
			{
				return single;
			}
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_BloodyDemon") ? 0f : 0.1f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up1") ? 0f : 0.15f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up2") ? 0f : 0.25f);
			return single;
		}
	}

	public static bool IsRegeneratingArmor
	{
		get
		{
			return EffectsController.RegeneratingArmorTime > 0f;
		}
	}

	public static float JumpModifier
	{
		get
		{
			float single = 1f;
			if (Defs.isHunger)
			{
				return single;
			}
			single = single + (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Samurai") ? 0f : 0.05f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Custom") ? 0f : 0.05f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_gray") ? 0f : 0.05f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up1") ? 0f : 0.1f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up2") ? 0f : 0.15f);
			single = single + (!Storager.getString("MaskEquippedSN", false).Equals("mask_demolition") ? 0f : 0.05f);
			single = single + (!Storager.getString("MaskEquippedSN", false).Equals("mask_demolition_up1") ? 0f : 0.1f);
			single = single + (!Storager.getString("MaskEquippedSN", false).Equals("mask_demolition_up2") ? 0f : 0.15f);
			single = single + (!Storager.getString(Defs.HatEquppedSN, false).Equals("league3_hat_afro") ? 0f : 0.08f);
			single = single + (!Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara") ? 0f : 0.08f);
			return single * EffectsController.SlowdownCoeff;
		}
	}

	public static bool NinjaJumpEnabled
	{
		get
		{
			if (Defs.isHunger)
			{
				return false;
			}
			return (Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_tabi") || Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_black") || Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up1") ? true : Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up2"));
		}
	}

	public static float RegeneratingArmorTime
	{
		get
		{
			float single = 0f;
			if (Defs.isHunger)
			{
				return single;
			}
			if (Defs.isHunger)
			{
				return 0f;
			}
			if (Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Archimage"))
			{
				single = 12f;
			}
			if (Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up1"))
			{
				single = 10f;
			}
			if (Storager.getString(Defs.HatEquppedSN, false).Equals("league5_hat_brain"))
			{
				single = 9f;
			}
			if (Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up2"))
			{
				single = 8f;
			}
			return single;
		}
	}

	public static float SelfExplosionDamageDecreaseCoef
	{
		get
		{
			if (Defs.isHunger)
			{
				return 1f;
			}
			return 1f * (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_KingsCrown") ? 1f : 0.8f) * (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_DiamondHelmet") ? 1f : 0.8f) * (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_RoyalKnight") ? 1f : 0.8f) * (!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up1") ? 1f : 0.7f) * (!Storager.getString(Defs.CapeEquppedSN, false).Equals("DemolitionCape_Up2") ? 1f : 0.5f) * (!Storager.getString(Defs.HatEquppedSN, false).Equals("league4_hat_mushroom") ? 1f : 0.5f);
		}
	}

	public static float SlowdownCoeff
	{
		get
		{
			return EffectsController.slowdownCoeff;
		}
		set
		{
			EffectsController.slowdownCoeff = value;
		}
	}

	public static bool WeAreStealth
	{
		get
		{
			if (Defs.isHunger)
			{
				return false;
			}
			return (Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_blue") || Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up1") || Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up2") ? true : Storager.getString(Defs.HatEquppedSN, false).Equals("league4_hat_mushroom"));
		}
	}

	static EffectsController()
	{
		EffectsController.slowdownCoeff = 1f;
	}

	public static float AddingForHeadshot(int cat)
	{
		if (Defs.isHunger)
		{
			return 0f;
		}
		List<float> singles = new List<float>(6);
		for (int i = 0; i < 6; i++)
		{
			singles.Add(0f);
		}
		if (Storager.getString(Defs.HatEquppedSN, false).Equals("league5_hat_brain"))
		{
			for (int j = 0; j < 6; j++)
			{
				List<float> item = singles;
				List<float> singles1 = item;
				int num = j;
				item[num] = singles1[num] + 0.13f;
			}
		}
		List<float> item1 = singles;
		List<float> singles2 = item1;
		int num1 = 4;
		item1[num1] = singles2[num1] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_SkeletonLord") ? 0f : 0.1f);
		List<float> item2 = singles;
		List<float> singles3 = item2;
		int num2 = 4;
		item2[num2] = singles3[num2] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("SniperCape_Up1") ? 0f : 0.15f);
		List<float> item3 = singles;
		List<float> singles4 = item3;
		int num3 = 4;
		item3[num3] = singles4[num3] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("SniperCape_Up2") ? 0f : 0.25f);
		return (cat < 0 || cat >= singles.Count ? 0f : singles[cat]);
	}

	public static float AddingForPotionDuration(string potion)
	{
		float single = 0f;
		if (Defs.isHunger)
		{
			return single;
		}
		if (potion == null)
		{
			return single;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_blue"))
		{
			single += 5f;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up1"))
		{
			single += 10f;
		}
		if (potion.Equals("InvisibilityPotion") && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up2"))
		{
			single += 15f;
		}
		if (!Defs.isDaterRegim)
		{
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Engineer") ? 0f : 10f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Engineer_Up1") ? 0f : 15f);
			single = single + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Engineer_Up2") ? 0f : 20f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots") ? 0f : 10f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up1") ? 0f : 15f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up2") ? 0f : 20f);
			single = single + (!Storager.getString(Defs.HatEquppedSN, false).Equals("league5_hat_brain") ? 0f : 13f);
		}
		return single;
	}

	public static float AmmoModForCategory(int i)
	{
		float single = 1f;
		if (Defs.isHunger)
		{
			return single;
		}
		if (Storager.getString(Defs.HatEquppedSN, false).Equals("league2_hat_cowboyhat"))
		{
			single += 0.13f;
		}
		if (i == 0)
		{
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_gray") ? 0f : 0.1f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up1") ? 0f : 0.15f);
			single = single + (!Storager.getString(Defs.BootsEquppedSN, false).Equals("StormTrooperBoots_Up2") ? 0f : 0.25f);
		}
		else if (i == 3)
		{
			single = single + (!Storager.getString("MaskEquippedSN", false).Equals("mask_engineer") ? 0f : 0.1f);
			single = single + (!Storager.getString("MaskEquippedSN", false).Equals("mask_engineer_up1") ? 0f : 0.15f);
			single = single + (!Storager.getString("MaskEquippedSN", false).Equals("mask_engineer_up2") ? 0f : 0.25f);
		}
		return single;
	}

	public static float DamageModifsByCats(int i)
	{
		List<float> singles = new List<float>(6);
		for (int num = 0; num < 6; num++)
		{
			singles.Add(0f);
		}
		if (Defs.isHunger)
		{
			return (i < 0 || i >= singles.Count ? 0f : singles[i]);
		}
		if (Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara"))
		{
			for (int j = 0; j < singles.Count; j++)
			{
				List<float> item = singles;
				List<float> singles1 = item;
				int num1 = j;
				item[num1] = singles1[num1] + 0.13f;
			}
		}
		List<float> item1 = singles;
		List<float> singles2 = item1;
		int num2 = 0;
		item1[num2] = singles2[num2] + (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Headphones") ? 0f : 0.1f);
		List<float> item2 = singles;
		List<float> singles3 = item2;
		int num3 = 2;
		item2[num3] = singles3[num3] + (!Storager.getString("MaskEquippedSN", false).Equals("hat_ManiacMask") ? 0f : 0.1f);
		List<float> item3 = singles;
		List<float> singles4 = item3;
		int num4 = 2;
		item3[num4] = singles4[num4] + (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Samurai") ? 0f : 0.1f);
		List<float> item4 = singles;
		List<float> singles5 = item4;
		int num5 = 1;
		item4[num5] = singles5[num5] + (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_SeriousManHat") ? 0f : 0.1f);
		List<float> item5 = singles;
		List<float> singles6 = item5;
		int num6 = 0;
		item5[num6] = singles6[num6] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_EliteCrafter") ? 0f : 0.1f);
		List<float> item6 = singles;
		List<float> singles7 = item6;
		int num7 = 0;
		item6[num7] = singles7[num7] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("StormTrooperCape_Up1") ? 0f : 0.15f);
		List<float> item7 = singles;
		List<float> singles8 = item7;
		int num8 = 0;
		item7[num8] = singles8[num8] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("StormTrooperCape_Up2") ? 0f : 0.25f);
		List<float> item8 = singles;
		List<float> singles9 = item8;
		int num9 = 1;
		item8[num9] = singles9[num9] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Archimage") ? 0f : 0.1f);
		List<float> item9 = singles;
		List<float> singles10 = item9;
		int num10 = 1;
		item9[num10] = singles10[num10] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up1") ? 0f : 0.15f);
		List<float> item10 = singles;
		List<float> singles11 = item10;
		int num11 = 1;
		item10[num11] = singles11[num11] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("HitmanCape_Up2") ? 0f : 0.25f);
		List<float> item11 = singles;
		List<float> singles12 = item11;
		int num12 = 2;
		item11[num12] = singles12[num12] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_BloodyDemon") ? 0f : 0.1f);
		List<float> item12 = singles;
		List<float> singles13 = item12;
		int num13 = 2;
		item12[num13] = singles13[num13] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up1") ? 0f : 0.15f);
		List<float> item13 = singles;
		List<float> singles14 = item13;
		int num14 = 2;
		item13[num14] = singles14[num14] + (!Storager.getString(Defs.CapeEquppedSN, false).Equals("BerserkCape_Up2") ? 0f : 0.25f);
		return (i < 0 || i >= singles.Count ? 0f : singles[i]);
	}

	public static float GetChanceToIgnoreHeadshot(int categoryNabor, string currentCape, string currentMask, string currentHat)
	{
		if (currentCape == null)
		{
			currentCape = string.Empty;
		}
		if (currentMask == null)
		{
			currentMask = string.Empty;
		}
		if (currentHat == null)
		{
			currentHat = string.Empty;
		}
		float single = 0f;
		if (Defs.isHunger)
		{
			return single;
		}
		if (currentHat.Equals("league4_hat_mushroom"))
		{
			single += 0.13f;
		}
		switch (categoryNabor - 1)
		{
			case 2:
			{
				if (currentCape.Equals("cape_BloodyDemon"))
				{
					single += 0.1f;
				}
				else if (currentCape.Equals("BerserkCape_Up1"))
				{
					single += 0.15f;
				}
				else if (currentCape.Equals("BerserkCape_Up2"))
				{
					single += 0.25f;
				}
				break;
			}
			case 3:
			{
				break;
			}
			case 4:
			{
				if (currentMask.Equals("mask_sniper"))
				{
					single += 0.1f;
				}
				else if (currentMask.Equals("mask_sniper_up1"))
				{
					single += 0.15f;
				}
				else if (currentMask.Equals("mask_sniper_up2"))
				{
					single += 0.25f;
				}
				break;
			}
			default:
			{
				goto case 3;
			}
		}
		return single;
	}

	public static float GetReloadAnimationSpeed(int categoryNabor, string currentCape, string currentMask, string currentHat)
	{
		if (currentCape == null)
		{
			currentCape = string.Empty;
		}
		if (currentMask == null)
		{
			currentMask = string.Empty;
		}
		if (currentHat == null)
		{
			currentHat = string.Empty;
		}
		float single = 1f;
		if (Defs.isHunger)
		{
			return single;
		}
		if (currentHat.Equals("league2_hat_cowboyhat"))
		{
			single += 0.13f;
		}
		switch (categoryNabor)
		{
			case 1:
			{
				single = single + (!currentCape.Equals("cape_EliteCrafter") ? 0f : 0.1f);
				single = single + (!currentCape.Equals("StormTrooperCape_Up1") ? 0f : 0.15f);
				single = single + (!currentCape.Equals("StormTrooperCape_Up2") ? 0f : 0.25f);
				return single;
			}
			case 2:
			{
				single = single + (!currentMask.Equals("mask_hitman_1") ? 0f : 0.1f);
				single = single + (!currentMask.Equals("mask_hitman_1_up1") ? 0f : 0.15f);
				single = single + (!currentMask.Equals("mask_hitman_1_up2") ? 0f : 0.25f);
				return single;
			}
			case 3:
			{
				return single;
			}
			case 4:
			{
				single = single + (!currentCape.Equals("cape_Engineer") ? 0f : 0.1f);
				single = single + (!currentCape.Equals("cape_Engineer_Up1") ? 0f : 0.15f);
				single = single + (!currentCape.Equals("cape_Engineer_Up2") ? 0f : 0.25f);
				return single;
			}
			case 5:
			{
				single = single + (!currentCape.Equals("cape_SkeletonLord") ? 0f : 0.1f);
				single = single + (!currentCape.Equals("SniperCape_Up1") ? 0f : 0.15f);
				single = single + (!currentCape.Equals("SniperCape_Up2") ? 0f : 0.25f);
				return single;
			}
			case 6:
			{
				single = single + (!currentCape.Equals("cape_RoyalKnight") ? 0f : 0.1f);
				single = single + (!currentCape.Equals("DemolitionCape_Up1") ? 0f : 0.15f);
				single = single + (!currentCape.Equals("DemolitionCape_Up2") ? 0f : 0.25f);
				return single;
			}
			default:
			{
				return single;
			}
		}
	}

	public static float SpeedModifier(int i)
	{
		if (Defs.isHunger || WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
		{
			return 1f;
		}
		float single = WeaponManager.sharedManager.currentWeaponSounds.speedModifier * (!PotionsController.sharedController.PotionIsActive(PotionsController.HastePotion) ? 1f : 1.25f) * (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_KingsCrown") ? 1f : 1.05f) * (!Storager.getString(Defs.HatEquppedSN, false).Equals("hat_Samurai") ? 1f : 1.05f) * (!Storager.getString(Defs.CapeEquppedSN, false).Equals("cape_Custom") ? 1f : 1.05f) * (!Storager.getString(Defs.HatEquppedSN, false).Equals("league6_hat_tiara") ? 1f : 1.08f) * (!Storager.getString(Defs.HatEquppedSN, false).Equals("league3_hat_afro") ? 1f : 1.08f) * EffectsController.SlowdownCoeff;
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_red"))
		{
			single *= 1.05f;
		}
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up1"))
		{
			single *= 1.1f;
		}
		if (i == 1 && Storager.getString(Defs.BootsEquppedSN, false).Equals("HitmanBoots_Up2"))
		{
			single *= 1.15f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_black"))
		{
			single *= 1.05f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up1"))
		{
			single *= 1.1f;
		}
		if (i == 2 && Storager.getString(Defs.BootsEquppedSN, false).Equals("BerserkBoots_Up2"))
		{
			single *= 1.15f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots"))
		{
			single *= 1.05f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up1"))
		{
			single *= 1.1f;
		}
		if (i == 3 && Storager.getString(Defs.BootsEquppedSN, false).Equals("EngineerBoots_Up2"))
		{
			single *= 1.15f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_blue"))
		{
			single *= 1.05f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up1"))
		{
			single *= 1.1f;
		}
		if (i == 4 && Storager.getString(Defs.BootsEquppedSN, false).Equals("SniperBoots_Up2"))
		{
			single *= 1.15f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN, false).Equals("boots_green"))
		{
			single *= 1.05f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up1"))
		{
			single *= 1.1f;
		}
		if (i == 5 && Storager.getString(Defs.BootsEquppedSN, false).Equals("DemolitionBoots_Up2"))
		{
			single *= 1.15f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN", false).Equals("mask_trooper"))
		{
			single *= 1.05f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN", false).Equals("mask_trooper_up1"))
		{
			single *= 1.1f;
		}
		if (i == 0 && Storager.getString("MaskEquippedSN", false).Equals("mask_trooper_up2"))
		{
			single *= 1.15f;
		}
		return single;
	}
}