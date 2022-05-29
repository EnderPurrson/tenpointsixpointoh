using System;
using System.Collections.Generic;

public static class PixelGunLowWeaponsTable
{
	public static Dictionary<WeaponManager.WeaponTypeForLow, List<string>> table;

	static PixelGunLowWeaponsTable()
	{
		Dictionary<WeaponManager.WeaponTypeForLow, List<string>> weaponTypeForLows = new Dictionary<WeaponManager.WeaponTypeForLow, List<string>>();
		List<string> strs = new List<string>()
		{
			"Weapon3",
			"Weapon309",
			"Weapon309",
			"Weapon62",
			"Weapon62",
			"Weapon220"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.AssaultRifle_1, strs);
		strs = new List<string>()
		{
			"Weapon142",
			"Weapon142",
			"Weapon263",
			"Weapon263",
			"Weapon330",
			"Weapon330"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.AssaultRifle_2, strs);
		strs = new List<string>()
		{
			"Weapon2",
			"Weapon163",
			"Weapon163",
			"Weapon177",
			"Weapon177",
			"Weapon231"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Shotgun_1, strs);
		strs = new List<string>()
		{
			"Weapon167",
			"Weapon167",
			"Weapon172",
			"Weapon172",
			"Weapon173",
			"Weapon173"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Shotgun_2, strs);
		strs = new List<string>()
		{
			"Weapon127",
			"Weapon141",
			"Weapon207",
			"Weapon148",
			"Weapon159",
			"Weapon232"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Machinegun, strs);
		strs = new List<string>()
		{
			"Weapon1",
			"Weapon164",
			"Weapon164",
			"Weapon71",
			"Weapon71",
			"Weapon223"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Pistol_1, strs);
		strs = new List<string>()
		{
			"Weapon364",
			"Weapon364",
			"Weapon150",
			"Weapon150",
			"Weapon152",
			"Weapon152"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Pistol_2, strs);
		strs = new List<string>()
		{
			"Weapon160",
			"Weapon160",
			"Weapon54",
			"Weapon54",
			"Weapon175",
			"Weapon175"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Submachinegun, strs);
		strs = new List<string>()
		{
			"Weapon9",
			"Weapon9",
			"Weapon9",
			"Weapon9",
			"Weapon9",
			"Weapon9"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Knife, strs);
		strs = new List<string>()
		{
			"Weapon131",
			"Weapon32",
			"Weapon30",
			"Weapon90",
			"Weapon155",
			"Weapon238"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Sword, strs);
		strs = new List<string>()
		{
			"Weapon333",
			"Weapon336",
			"Weapon336",
			"Weapon239",
			"Weapon283",
			"Weapon338"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Flamethrower_1, strs);
		strs = new List<string>()
		{
			"Weapon281",
			"Weapon281",
			"Weapon81",
			"Weapon81",
			"Weapon124",
			"Weapon124"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Flamethrower_2, strs);
		strs = new List<string>()
		{
			"Weapon67",
			"Weapon339",
			"Weapon339",
			"Weapon340",
			"Weapon340",
			"Weapon221"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.SniperRifle_1, strs);
		strs = new List<string>()
		{
			"Weapon61",
			"Weapon188",
			"Weapon346",
			"Weapon192",
			"Weapon211",
			"Weapon242"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.SniperRifle_2, strs);
		strs = new List<string>()
		{
			"Weapon256",
			"Weapon27",
			"Weapon268",
			"Weapon268",
			"Weapon269",
			"Weapon269"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Bow, strs);
		strs = new List<string>()
		{
			"Weapon162",
			"Weapon162",
			"Weapon162",
			"Weapon254",
			"Weapon254",
			"Weapon254"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.RocketLauncher_1, strs);
		strs = new List<string>()
		{
			"Weapon75",
			"Weapon76",
			"Weapon76",
			"Weapon76",
			"Weapon76",
			"Weapon248"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.RocketLauncher_2, strs);
		strs = new List<string>()
		{
			"Weapon82",
			"Weapon82",
			"Weapon157",
			"Weapon157",
			"Weapon158",
			"Weapon158"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.RocketLauncher_3, strs);
		strs = new List<string>()
		{
			"Weapon80",
			"Weapon143",
			"Weapon253",
			"Weapon140",
			"Weapon274",
			"Weapon222"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.GrenadeLauncher, strs);
		strs = new List<string>()
		{
			"Weapon166",
			"Weapon261",
			"Weapon168",
			"Weapon272",
			"Weapon169",
			"Weapon273"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Snaryad, strs);
		strs = new List<string>()
		{
			"Weapon303",
			"Weapon303",
			"Weapon304",
			"Weapon304",
			"Weapon305",
			"Weapon305"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Snaryad_Otskok, strs);
		strs = new List<string>()
		{
			"Weapon331",
			"Weapon331",
			"Weapon260",
			"Weapon260",
			"Weapon349",
			"Weapon349"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Snaryad_Disk, strs);
		strs = new List<string>()
		{
			"Weapon77",
			"Weapon77",
			"Weapon121",
			"Weapon121",
			"Weapon122",
			"Weapon122"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Railgun, strs);
		strs = new List<string>()
		{
			"Weapon178",
			"Weapon105",
			"Weapon133",
			"Weapon156",
			"Weapon306",
			"Weapon243"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Ray, strs);
		strs = new List<string>()
		{
			"Weapon297",
			"Weapon278",
			"Weapon324",
			"Weapon279",
			"Weapon325",
			"Weapon224"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.AOE, strs);
		strs = new List<string>()
		{
			"Weapon427",
			"Weapon427",
			"Weapon427",
			"Weapon251",
			"Weapon429",
			"Weapon271"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.Instant_Area_Damage, strs);
		strs = new List<string>()
		{
			"Weapon255",
			"Weapon255",
			"Weapon255",
			"Weapon255",
			"Weapon275",
			"Weapon275"
		};
		weaponTypeForLows.Add(WeaponManager.WeaponTypeForLow.X3_Snaryad, strs);
		PixelGunLowWeaponsTable.table = weaponTypeForLows;
	}
}