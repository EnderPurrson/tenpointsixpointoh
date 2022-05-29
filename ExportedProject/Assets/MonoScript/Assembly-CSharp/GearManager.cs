using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GearManager
{
	public const int MaxGrenadeCount = 10;

	public const int MaxGearCount = 1000000;

	public const int NumberOfGearInPack = 3;

	public readonly static string InvisibilityPotion;

	public readonly static string Grenade;

	public readonly static string Like;

	public readonly static string Jetpack;

	public readonly static string Turret;

	public readonly static string Mech;

	public readonly static string BigHeadPotion;

	public readonly static string Wings;

	public readonly static string Bear;

	public readonly static string MusicBox;

	public readonly static string[] Gear;

	public readonly static string[] DaterGear;

	public static List<string> AllGear
	{
		get
		{
			return GearManager.Gear.Concat<string>(GearManager.DaterGear).ToList<string>();
		}
	}

	public static int NumOfGearUpgrades
	{
		get
		{
			return (int)ExpController.LevelsForTiers.Length - 1;
		}
	}

	public static string OneItemSuffix
	{
		get
		{
			return "_OneItem_";
		}
	}

	public static string UpgradeSuffix
	{
		get
		{
			return "_Up_";
		}
	}

	static GearManager()
	{
		GearManager.InvisibilityPotion = "InvisibilityPotion";
		GearManager.Grenade = "GrenadeID";
		GearManager.Like = "LikeID";
		GearManager.Jetpack = "Jetpack";
		GearManager.Turret = "Turret";
		GearManager.Mech = "Mech";
		GearManager.BigHeadPotion = "BigHeadPotion";
		GearManager.Wings = "Wings";
		GearManager.Bear = "Bear";
		GearManager.MusicBox = "MusicBox";
		GearManager.Gear = new string[] { GearManager.Grenade, GearManager.InvisibilityPotion, GearManager.Jetpack, GearManager.Turret, GearManager.Mech };
		GearManager.DaterGear = new string[] { GearManager.BigHeadPotion, GearManager.Wings, GearManager.MusicBox, GearManager.Bear };
	}

	public static string AnalyticsIDForOneItemOfGear(string itemName, bool changeGrenade = false)
	{
		if (itemName == null)
		{
			return string.Empty;
		}
		string str = GearManager.HolderQuantityForID(itemName);
		if (str == null || !GearManager.AllGear.Contains(str))
		{
			return string.Empty;
		}
		int num = GearManager.CurrentNumberOfUphradesForGear(str);
		string str1 = str;
		if (changeGrenade && str.Equals(GearManager.Grenade) && num == 0)
		{
			str1 = "Grenade";
		}
		if (num > 0)
		{
			str1 = string.Concat(str1, "_", num);
		}
		return str1;
	}

	public static int CurrentNumberOfUphradesForGear(string id)
	{
		if (Defs.isDaterRegim)
		{
			return 0;
		}
		int ourTier = 0;
		if (ExpController.Instance != null)
		{
			ourTier = ExpController.Instance.OurTier;
		}
		return Mathf.Min(ourTier, GearManager.NumOfGearUpgrades);
	}

	public static string HolderQuantityForID(string id)
	{
		string str;
		if (id == null)
		{
			return string.Empty;
		}
		List<string>.Enumerator enumerator = GearManager.AllGear.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (!GearManager.ItemIsGear(id, current))
				{
					continue;
				}
				str = current;
				return str;
			}
			return id;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return str;
	}

	public static bool IsItemGear(string tag)
	{
		if (tag == null)
		{
			return false;
		}
		for (int i = 0; i < GearManager.AllGear.Count; i++)
		{
			if (GearManager.ItemIsGear(tag, GearManager.AllGear[i]))
			{
				return true;
			}
		}
		return false;
	}

	private static bool ItemIsGear(string item, string gear)
	{
		int num;
		bool flag;
		if (gear == null || item == null)
		{
			return false;
		}
		string str = string.Concat(gear, GearManager.UpgradeSuffix);
		string str1 = string.Concat(gear, GearManager.OneItemSuffix);
		if (item == gear || item.StartsWith(str) && item.Length > str.Length && int.TryParse(string.Concat(string.Empty, item[str.Length]), out num))
		{
			flag = true;
		}
		else
		{
			flag = (!item.StartsWith(str1) || item.Length <= str1.Length ? false : int.TryParse(string.Concat(string.Empty, item[str1.Length]), out num));
		}
		return flag;
	}

	public static int ItemsInPackForGear(string id)
	{
		return (id == null || !id.Equals(GearManager.Grenade) ? 3 : 5);
	}

	public static int MaxCountForGear(string id)
	{
		return (id == null || !id.Equals(GearManager.Grenade) ? 1000000 : 10);
	}

	public static string NameForUpgrade(string item, int num)
	{
		return string.Concat(item, GearManager.UpgradeSuffix, num);
	}

	public static string OneItemIDForGear(string id, int i)
	{
		if (id == null)
		{
			return null;
		}
		return string.Concat(id, GearManager.OneItemSuffix, i);
	}

	public static string UpgradeIDForGear(string id, int i)
	{
		if (id == null)
		{
			return null;
		}
		return string.Concat(id, GearManager.UpgradeSuffix, i);
	}
}