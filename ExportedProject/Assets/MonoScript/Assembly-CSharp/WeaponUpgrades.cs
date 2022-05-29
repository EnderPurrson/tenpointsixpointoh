using System;
using System.Collections.Generic;

public static class WeaponUpgrades
{
	public static List<List<string>> upgrades;

	static WeaponUpgrades()
	{
		WeaponUpgrades.upgrades = new List<List<string>>();
		List<string> strs = new List<string>()
		{
			WeaponTags.Fire_orb_Tag,
			WeaponTags.Fire_orb_2_Tag,
			WeaponTags.Fire_orb_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.Hand_dragon_Tag,
			WeaponTags.Hand_dragon_2_Tag,
			WeaponTags.Hand_dragon_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.Tesla_Cannon_Tag,
			WeaponTags.Tesla_Cannon_2_Tag,
			WeaponTags.Tesla_Cannon_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.Hydra_Tag,
			WeaponTags.Hydra_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.Dark_Matter_Generator_Tag,
			WeaponTags.Dark_Matter_Generator_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.Devostator_Tag,
			WeaponTags.Devostator_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.TacticalBow_Tag,
			WeaponTags.TacticalBow_2_Tag,
			WeaponTags.TacticalBow_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.LaserDiscThower_Tag,
			WeaponTags.LaserDiscThower_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.ElectroBlastRifle_Tag,
			WeaponTags.ElectroBlastRifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.Photon_Pistol_Tag,
			WeaponTags.Photon_Pistol_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.RapidFireRifle_Tag,
			WeaponTags.RapidFireRifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.PlasmaShotgun_Tag,
			WeaponTags.PlasmaShotgun_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		strs = new List<string>()
		{
			WeaponTags.FutureRifle_Tag,
			WeaponTags.FutureRifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs);
		List<string> strs1 = new List<string>()
		{
			WeaponTags.DragonGun_Tag,
			WeaponTags.DragonGun_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs1);
		List<string> strs2 = new List<string>()
		{
			WeaponTags.Bazooka_2_3_Tag,
			WeaponTags.Bazooka_2_4tag
		};
		WeaponUpgrades.upgrades.Add(strs2);
		List<string> strs3 = new List<string>()
		{
			WeaponTags.buddy_Tag,
			WeaponTags.bigbuddy_2tag,
			WeaponTags.bigbuddy_3tag
		};
		WeaponUpgrades.upgrades.Add(strs3);
		List<string> strs4 = new List<string>()
		{
			WeaponTags.barret_3_Tag,
			WeaponTags.Barret_4tag
		};
		WeaponUpgrades.upgrades.Add(strs4);
		List<string> strs5 = new List<string>()
		{
			WeaponTags.Flamethrower_3_Tag,
			WeaponTags.Flamethrower_4tag,
			WeaponTags.Flamethrower_5tag
		};
		WeaponUpgrades.upgrades.Add(strs5);
		List<string> strs6 = new List<string>()
		{
			WeaponTags.katana_3_Tag,
			WeaponTags.katana_4tag
		};
		WeaponUpgrades.upgrades.Add(strs6);
		List<string> strs7 = new List<string>()
		{
			WeaponTags.SparklyBlasterTag,
			WeaponTags.SparklyBlaster_2tag,
			WeaponTags.SparklyBlaster_3tag
		};
		WeaponUpgrades.upgrades.Add(strs7);
		List<string> strs8 = new List<string>()
		{
			WeaponTags.Thompson_2_Tag,
			WeaponTags.StateDefender_2_tag
		};
		WeaponUpgrades.upgrades.Add(strs8);
		List<string> strs9 = new List<string>()
		{
			WeaponTags.plazma_3_Tag,
			WeaponTags.PlasmaRifle_2_Tag,
			WeaponTags.PlasmaRifle_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs9);
		List<string> strs10 = new List<string>()
		{
			WeaponTags._3_shotgun_3_Tag,
			WeaponTags.SteamPower_2_Tag,
			WeaponTags.SteamPower_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs10);
		List<string> strs11 = new List<string>()
		{
			WeaponTags.MinersWeaponTag,
			WeaponTags.GoldenPickTag,
			WeaponTags.CrystalPickTag
		};
		WeaponUpgrades.upgrades.Add(strs11);
		List<string> strs12 = new List<string>()
		{
			WeaponTags.Sword_2_3_Tag,
			WeaponTags.Sword_2_4_Tag,
			WeaponTags.Sword_2_5_Tag
		};
		WeaponUpgrades.upgrades.Add(strs12);
		List<string> strs13 = new List<string>()
		{
			WeaponTags.RailgunTag,
			WeaponTags.railgun_2_Tag,
			WeaponTags.railgun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs13);
		List<string> strs14 = new List<string>()
		{
			WeaponTags.SteelAxeTag,
			WeaponTags.GoldenAxeTag,
			WeaponTags.CrystalAxeTag
		};
		WeaponUpgrades.upgrades.Add(strs14);
		List<string> strs15 = new List<string>()
		{
			WeaponTags.IronSwordTag,
			WeaponTags.GoldenSwordTag,
			WeaponTags.CrystalSwordTag
		};
		WeaponUpgrades.upgrades.Add(strs15);
		List<string> strs16 = new List<string>()
		{
			WeaponTags.Red_Stone_3_Tag,
			WeaponTags.Red_Stone_4_Tag,
			WeaponTags.Red_Stone_5tag
		};
		WeaponUpgrades.upgrades.Add(strs16);
		List<string> strs17 = new List<string>()
		{
			WeaponTags.SPASTag,
			WeaponTags.GoldenSPASTag,
			WeaponTags.CrystalSPASTag
		};
		WeaponUpgrades.upgrades.Add(strs17);
		List<string> strs18 = new List<string>()
		{
			WeaponTags.SteelCrossbowTag,
			WeaponTags.CrossbowTag,
			WeaponTags.CrystalCrossbowTag
		};
		WeaponUpgrades.upgrades.Add(strs18);
		List<string> strs19 = new List<string>()
		{
			WeaponTags.minigun_3_Tag,
			WeaponTags.Minigun_4_Tag,
			WeaponTags.Minigun_5_Tag
		};
		WeaponUpgrades.upgrades.Add(strs19);
		List<string> strs20 = new List<string>()
		{
			WeaponTags.LightSword_3_Tag,
			WeaponTags.LightSword_4_Tag,
			WeaponTags.LightSword_5tag
		};
		WeaponUpgrades.upgrades.Add(strs20);
		List<string> strs21 = new List<string>()
		{
			WeaponTags.FAMASTag,
			WeaponTags.SandFamasTag,
			WeaponTags.NavyFamasTag
		};
		WeaponUpgrades.upgrades.Add(strs21);
		List<string> strs22 = new List<string>()
		{
			WeaponTags.FreezeGunTag,
			WeaponTags.FreezeGun_2_Tag,
			WeaponTags.FreezeGun_3tag
		};
		WeaponUpgrades.upgrades.Add(strs22);
		List<string> strs23 = new List<string>()
		{
			WeaponTags.BerettaTag,
			WeaponTags.WhiteBerettaTag,
			WeaponTags.BlackBerettaTag
		};
		WeaponUpgrades.upgrades.Add(strs23);
		List<string> strs24 = new List<string>()
		{
			WeaponTags.EagleTag,
			WeaponTags.BlackEagleTag,
			WeaponTags.eagle_3Tag
		};
		WeaponUpgrades.upgrades.Add(strs24);
		List<string> strs25 = new List<string>()
		{
			WeaponTags.GlockTag,
			WeaponTags.GoldenGlockTag,
			WeaponTags.CrystalGlockTag
		};
		WeaponUpgrades.upgrades.Add(strs25);
		List<string> strs26 = new List<string>()
		{
			WeaponTags.svdTag,
			WeaponTags.svd_2Tag,
			WeaponTags.svd_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs26);
		List<string> strs27 = new List<string>()
		{
			WeaponTags.m16Tag,
			WeaponTags.m16_3_Tag,
			WeaponTags.m16_4_Tag
		};
		WeaponUpgrades.upgrades.Add(strs27);
		List<string> strs28 = new List<string>()
		{
			WeaponTags.TreeTag,
			WeaponTags.Tree_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs28);
		List<string> strs29 = new List<string>()
		{
			WeaponTags.revolver_2_3_Tag,
			WeaponTags.Revolver5_Tag,
			WeaponTags.Revolver6_Tag
		};
		WeaponUpgrades.upgrades.Add(strs29);
		List<string> strs30 = new List<string>()
		{
			WeaponTags.FreezeGun_0_Tag,
			WeaponTags.FreezeGun_0_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs30);
		List<string> strs31 = new List<string>()
		{
			WeaponTags.TeslaTag,
			WeaponTags.Tesla_2Tag,
			WeaponTags.tesla_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs31);
		List<string> strs32 = new List<string>()
		{
			WeaponTags.Easter_Bazooka_Tag,
			WeaponTags.Easter_Bazooka_2_Tag,
			WeaponTags.Easter_Bazooka_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs32);
		List<string> strs33 = new List<string>()
		{
			WeaponTags.Bazooka_3Tag,
			WeaponTags.Bazooka_3_2_Tag,
			WeaponTags.Bazooka_3_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs33);
		List<string> strs34 = new List<string>()
		{
			WeaponTags.GrenadeLuancher_2Tag,
			WeaponTags.grenade_launcher_3_Tag,
			WeaponTags.m32_1_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs34);
		List<string> strs35 = new List<string>()
		{
			WeaponTags.BazookaTag,
			WeaponTags.Bazooka_2Tag,
			WeaponTags.Bazooka_1_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs35);
		List<string> strs36 = new List<string>()
		{
			WeaponTags.AUGTag,
			WeaponTags.AUG_2Tag,
			WeaponTags.AUG_3tag
		};
		WeaponUpgrades.upgrades.Add(strs36);
		List<string> strs37 = new List<string>()
		{
			WeaponTags.AK74Tag,
			WeaponTags.AK74_2_Tag,
			WeaponTags.AK74_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs37);
		List<string> strs38 = new List<string>()
		{
			WeaponTags.GravigunTag,
			WeaponTags.gravity_2_Tag,
			WeaponTags.gravity_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs38);
		List<string> strs39 = new List<string>()
		{
			WeaponTags.XM8_1_Tag,
			WeaponTags.XM8_2_Tag,
			WeaponTags.XM8_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs39);
		List<string> strs40 = new List<string>()
		{
			WeaponTags.SnowballMachingun_Tag,
			WeaponTags.SnowballMachingun_2_Tag,
			WeaponTags.SnowballMachingun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs40);
		List<string> strs41 = new List<string>()
		{
			WeaponTags.SnowballGun_Tag,
			WeaponTags.SnowballGun_2_Tag,
			WeaponTags.SnowballGun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs41);
		List<string> strs42 = new List<string>()
		{
			WeaponTags.HeavyShotgun_Tag,
			WeaponTags.HeavyShotgun_2_Tag,
			WeaponTags.HeavyShotgun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs42);
		List<string> strs43 = new List<string>()
		{
			WeaponTags.TwoBolters_Tag,
			WeaponTags.TwoBolters_2_Tag,
			WeaponTags.TwoBolters_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs43);
		List<string> strs44 = new List<string>()
		{
			WeaponTags.TwoRevolvers_Tag,
			WeaponTags.TwoRevolvers_2_Tag,
			WeaponTags.TwoRevolvers_3tag
		};
		WeaponUpgrades.upgrades.Add(strs44);
		List<string> strs45 = new List<string>()
		{
			WeaponTags.AutoShotgun_Tag,
			WeaponTags.AutoShotgun_2_Tag,
			WeaponTags.AutoShotgun_3tag
		};
		WeaponUpgrades.upgrades.Add(strs45);
		List<string> strs46 = new List<string>()
		{
			WeaponTags.Solar_Ray_Tag,
			WeaponTags.Solar_Ray_2_Tag,
			WeaponTags.Solar_Ray_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs46);
		List<string> strs47 = new List<string>()
		{
			WeaponTags.Water_Pistol_Tag,
			WeaponTags.Water_Pistol_2_Tag,
			WeaponTags.Water_Pistol_3tag
		};
		WeaponUpgrades.upgrades.Add(strs47);
		List<string> strs48 = new List<string>()
		{
			WeaponTags.Solar_Power_Cannon_Tag,
			WeaponTags.Solar_Power_Cannon_2_Tag,
			WeaponTags.Solar_Power_Cannon_3tag
		};
		WeaponUpgrades.upgrades.Add(strs48);
		List<string> strs49 = new List<string>()
		{
			WeaponTags.Water_Rifle_Tag,
			WeaponTags.Water_Rifle_2_Tag,
			WeaponTags.Water_Rifle_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs49);
		List<string> strs50 = new List<string>()
		{
			WeaponTags.Valentine_Shotgun_Tag,
			WeaponTags.Valentine_Shotgun_2_Tag,
			WeaponTags.Valentine_Shotgun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs50);
		List<string> strs51 = new List<string>()
		{
			WeaponTags.Needle_Throw_Tag,
			WeaponTags.Needle_Throw_2_Tag,
			WeaponTags.Needle_Throw_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs51);
		List<string> strs52 = new List<string>()
		{
			WeaponTags.Carrot_Sword_Tag,
			WeaponTags.Carrot_Sword_2_Tag,
			WeaponTags.Carrot_Sword_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs52);
		List<string> strs53 = new List<string>()
		{
			WeaponTags.RailRevolverBuy_Tag,
			WeaponTags.RailRevolverBuy_2_Tag,
			WeaponTags.RailRevolverBuy_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs53);
		List<string> strs54 = new List<string>()
		{
			WeaponTags.Assault_Machine_GunBuy_Tag,
			WeaponTags.Assault_Machine_GunBuy_2_Tag,
			WeaponTags.Assault_Machine_GunBuy_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs54);
		List<string> strs55 = new List<string>()
		{
			WeaponTags.Impulse_Sniper_RifleBuy_Tag,
			WeaponTags.Impulse_Sniper_RifleBuy_2_Tag,
			WeaponTags.Impulse_Sniper_RifleBuy_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs55);
		List<string> strs56 = new List<string>()
		{
			WeaponTags.Autoaim_RocketlauncherBuy_Tag,
			WeaponTags.Autoaim_RocketlauncherBuy_2_Tag,
			WeaponTags.Autoaim_RocketlauncherBuy_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs56);
		List<string> strs57 = new List<string>()
		{
			WeaponTags.DualUzi_Tag,
			WeaponTags.DualUzi_2_Tag,
			WeaponTags.DualUzi_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs57);
		List<string> strs58 = new List<string>()
		{
			WeaponTags.Alligator_Tag,
			WeaponTags.Alligator_2_Tag,
			WeaponTags.Alligator_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs58);
		List<string> strs59 = new List<string>()
		{
			WeaponTags.Hippo_Tag,
			WeaponTags.Hippo_2_Tag,
			WeaponTags.Hippo_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs59);
		List<string> strs60 = new List<string>()
		{
			WeaponTags.Alien_Cannon_Tag,
			WeaponTags.Alien_Cannon_2_Tag,
			WeaponTags.Alien_Cannon_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs60);
		List<string> strs61 = new List<string>()
		{
			WeaponTags.Alien_Laser_Pistol_Tag,
			WeaponTags.Alien_Laser_Pistol_2_Tag,
			WeaponTags.Alien_Laser_Pistol_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs61);
		List<string> strs62 = new List<string>()
		{
			WeaponTags.Alien_rifle_Tag,
			WeaponTags.Alien_rifle_2_Tag,
			WeaponTags.Alien_rifle_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs62);
		List<string> strs63 = new List<string>()
		{
			WeaponTags.Tiger_gun_Tag,
			WeaponTags.Tiger_gun_2_Tag,
			WeaponTags.Tiger_gun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs63);
		List<string> strs64 = new List<string>()
		{
			WeaponTags.Pit_Bull_Tag,
			WeaponTags.Pit_Bull_2_Tag,
			WeaponTags.Pit_Bull_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs64);
		List<string> strs65 = new List<string>()
		{
			WeaponTags.Range_Rifle_Tag,
			WeaponTags.Range_Rifle_2_Tag,
			WeaponTags.Range_Rifle_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs65);
		List<string> strs66 = new List<string>()
		{
			WeaponTags.Dater_Bow_Tag,
			WeaponTags.Dater_Bow_2_Tag,
			WeaponTags.Dater_Bow_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs66);
		List<string> strs67 = new List<string>()
		{
			WeaponTags.Dater_DJ_Tag,
			WeaponTags.Dater_DJ_2_Tag,
			WeaponTags.Dater_DJ_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs67);
		List<string> strs68 = new List<string>()
		{
			WeaponTags.Dater_Flowers_Tag,
			WeaponTags.Dater_Flowers_2_Tag,
			WeaponTags.Dater_Flowers_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs68);
		List<string> strs69 = new List<string>()
		{
			WeaponTags.Balloon_Cannon_Tag,
			WeaponTags.Balloon_Cannon_2_Tag,
			WeaponTags.Balloon_Cannon_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs69);
		List<string> strs70 = new List<string>()
		{
			WeaponTags.Fireworks_Launcher_Tag,
			WeaponTags.Fireworks_Launcher_2_Tag,
			WeaponTags.Fireworks_Launcher_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs70);
		List<string> strs71 = new List<string>()
		{
			WeaponTags.PumpkinGun_1_Tag,
			WeaponTags.PumpkinGun_2_Tag,
			WeaponTags.PumpkinGun_5_Tag
		};
		WeaponUpgrades.upgrades.Add(strs71);
		List<string> strs72 = new List<string>()
		{
			WeaponTags.Laser_Crossbow_Tag,
			WeaponTags.Laser_Crossbow2_Tag,
			WeaponTags.Laser_Crossbow3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs72);
		List<string> strs73 = new List<string>()
		{
			WeaponTags.SPACE_RIFLE_Tag,
			WeaponTags.SPACE_RIFLE_UP1_Tag,
			WeaponTags.SPACE_RIFLE_UP2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs73);
		List<string> strs74 = new List<string>()
		{
			WeaponTags.Nutcracker_Tag,
			WeaponTags.Nutcracker2_Tag,
			WeaponTags.Nutcracker3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs74);
		List<string> strs75 = new List<string>()
		{
			WeaponTags.Shuriken_Thrower_Tag,
			WeaponTags.Shuriken_Thrower2_Tag,
			WeaponTags.Shuriken_Thrower3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs75);
		List<string> strs76 = new List<string>()
		{
			WeaponTags.Icicle_Generator_Tag,
			WeaponTags.Icicle_Generator2_Tag,
			WeaponTags.Icicle_Generator3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs76);
		List<string> strs77 = new List<string>()
		{
			WeaponTags.Snowball_Tag,
			WeaponTags.Snowball2_Tag,
			WeaponTags.Snowball3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs77);
		List<string> strs78 = new List<string>()
		{
			WeaponTags.PORTABLE_DEATH_MOON_Tag,
			WeaponTags.PORTABLE_DEATH_MOON_UP1_Tag,
			WeaponTags.PORTABLE_DEATH_MOON_UP2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs78);
		List<string> strs79 = new List<string>()
		{
			WeaponTags.MysticOreEmitter_Tag,
			WeaponTags.MysticOreEmitter_UP1_Tag,
			WeaponTags.MysticOreEmitter_UP2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs79);
		List<string> strs80 = new List<string>()
		{
			WeaponTags.Hockey_stick_Tag,
			WeaponTags.Hockey_stick_UP1_Tag,
			WeaponTags.Hockey_stick_UP2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs80);
		List<string> strs81 = new List<string>()
		{
			WeaponTags.Space_blaster_Tag,
			WeaponTags.Space_blaster_UP1_Tag,
			WeaponTags.Space_blaster_UP2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs81);
		List<string> strs82 = new List<string>()
		{
			WeaponTags.Dynamite_Gun_1_Tag,
			WeaponTags.Dynamite_Gun_2_Tag,
			WeaponTags.Dynamite_Gun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs82);
		List<string> strs83 = new List<string>()
		{
			WeaponTags.Dual_shotguns_1_Tag,
			WeaponTags.Dual_shotguns_2_Tag,
			WeaponTags.Dual_shotguns_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs83);
		List<string> strs84 = new List<string>()
		{
			WeaponTags.Antihero_Rifle_1_Tag,
			WeaponTags.Antihero_Rifle_2_Tag,
			WeaponTags.Antihero_Rifle_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs84);
		List<string> strs85 = new List<string>()
		{
			WeaponTags.HarpoonGun_1_Tag,
			WeaponTags.HarpoonGun_2_Tag,
			WeaponTags.HarpoonGun_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs85);
		List<string> strs86 = new List<string>()
		{
			WeaponTags.Red_twins_pistols_1_Tag,
			WeaponTags.Red_twins_pistols_2_Tag,
			WeaponTags.Red_twins_pistols_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs86);
		List<string> strs87 = new List<string>()
		{
			WeaponTags.Toxic_sniper_rifle_1_Tag,
			WeaponTags.Toxic_sniper_rifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs87);
		List<string> strs88 = new List<string>()
		{
			WeaponTags.NuclearRevolver_1_Tag,
			WeaponTags.NuclearRevolver_2_Tag,
			WeaponTags.NuclearRevolver_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs88);
		List<string> strs89 = new List<string>()
		{
			WeaponTags.NAIL_MINIGUN_1_Tag,
			WeaponTags.NAIL_MINIGUN_2_Tag,
			WeaponTags.NAIL_MINIGUN_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs89);
		List<string> strs90 = new List<string>()
		{
			WeaponTags.DUAL_MACHETE_1_Tag,
			WeaponTags.DUAL_MACHETE_2_Tag,
			WeaponTags.DUAL_MACHETE_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs90);
		List<string> strs91 = new List<string>()
		{
			WeaponTags.Fighter_1_Tag,
			WeaponTags.Fighter_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs91);
		List<string> strs92 = new List<string>()
		{
			WeaponTags.Gas_spreader_Tag,
			WeaponTags.Gas_spreader_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs92);
		List<string> strs93 = new List<string>()
		{
			WeaponTags.LaserBouncer_1_Tag,
			WeaponTags.LaserBouncer_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs93);
		List<string> strs94 = new List<string>()
		{
			WeaponTags.magicbook_fireball_Tag,
			WeaponTags.magicbook_fireball_2_Tag,
			WeaponTags.magicbook_fireball_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs94);
		List<string> strs95 = new List<string>()
		{
			WeaponTags.magicbook_frostbeam_Tag,
			WeaponTags.magicbook_frostbeam_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs95);
		List<string> strs96 = new List<string>()
		{
			WeaponTags.magicbook_thunder_Tag,
			WeaponTags.magicbook_thunder_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs96);
		List<string> strs97 = new List<string>()
		{
			WeaponTags.TurboPistols_1_Tag,
			WeaponTags.TurboPistols_2_Tag,
			WeaponTags.TurboPistols_3_Tag
		};
		WeaponUpgrades.upgrades.Add(strs97);
		List<string> strs98 = new List<string>()
		{
			WeaponTags.Laser_Bow_1_Tag,
			WeaponTags.Laser_Bow_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs98);
		List<string> strs99 = new List<string>()
		{
			WeaponTags.loud_piggy_Tag,
			WeaponTags.loud_piggy_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs99);
		List<string> strs100 = new List<string>()
		{
			WeaponTags.Trapper_1_Tag,
			WeaponTags.Trapper_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs100);
		List<string> strs101 = new List<string>()
		{
			WeaponTags.chainsaw_sword_1_Tag,
			WeaponTags.chainsaw_sword_2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs101);
		List<string> strs102 = new List<string>()
		{
			WeaponTags.dark_star_Tag,
			WeaponTags.dark_star_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs102);
		List<string> strs103 = new List<string>()
		{
			WeaponTags.toy_bomber_Tag,
			WeaponTags.toy_bomber_up1_Tag,
			WeaponTags.toy_bomber_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs103);
		List<string> strs104 = new List<string>()
		{
			WeaponTags.zombie_head_Tag,
			WeaponTags.zombie_head_up1_Tag,
			WeaponTags.zombie_head_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs104);
		List<string> strs105 = new List<string>()
		{
			WeaponTags.mr_squido_Tag,
			WeaponTags.mr_squido_up1_Tag,
			WeaponTags.mr_squido_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs105);
		List<string> strs106 = new List<string>()
		{
			WeaponTags.RocketCrossbow_Tag,
			WeaponTags.RocketCrossbow_up1_Tag,
			WeaponTags.RocketCrossbow_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs106);
		List<string> strs107 = new List<string>()
		{
			WeaponTags.zombie_slayer_Tag,
			WeaponTags.zombie_slayer_up1_Tag,
			WeaponTags.zombie_slayer_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs107);
		List<string> strs108 = new List<string>()
		{
			WeaponTags.AcidCannon_Tag,
			WeaponTags.AcidCannon_up1_Tag,
			WeaponTags.AcidCannon_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs108);
		List<string> strs109 = new List<string>()
		{
			WeaponTags.frank_sheepone_Tag,
			WeaponTags.frank_sheepone_up1_Tag,
			WeaponTags.frank_sheepone_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs109);
		List<string> strs110 = new List<string>()
		{
			WeaponTags.Ghost_Lantern_Tag,
			WeaponTags.Ghost_Lantern_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs110);
		List<string> strs111 = new List<string>()
		{
			WeaponTags.autoaim_bazooka_Tag,
			WeaponTags.autoaim_bazooka_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs111);
		List<string> strs112 = new List<string>()
		{
			WeaponTags.Semiauto_sniper_Tag,
			WeaponTags.Semiauto_sniper_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs112);
		List<string> strs113 = new List<string>()
		{
			WeaponTags.Chain_electro_cannon_Tag,
			WeaponTags.Chain_electro_cannon_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs113);
		List<string> strs114 = new List<string>()
		{
			WeaponTags.Demoman_Tag,
			WeaponTags.Demoman_up1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs114);
		List<string> strs115 = new List<string>()
		{
			WeaponTags.Barier_Generator_Tag,
			WeaponTags.Barier_Generator_up1_Tag,
			WeaponTags.Barier_Generator_up2_Tag
		};
		WeaponUpgrades.upgrades.Add(strs115);
		List<string> strs116 = new List<string>()
		{
			WeaponTags.charge_rifle_Tag,
			WeaponTags.charge_rifle_UP1_Tag
		};
		WeaponUpgrades.upgrades.Add(strs116);
	}

	public static List<string> ChainForTag(string tg)
	{
		List<string> strs;
		if (tg == null)
		{
			return null;
		}
		List<List<string>>.Enumerator enumerator = WeaponUpgrades.upgrades.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				List<string> current = enumerator.Current;
				if (!current.Contains(tg))
				{
					continue;
				}
				strs = current;
				return strs;
			}
			return null;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return strs;
	}

	public static string TagOfFirstUpgrade(string tg)
	{
		string item;
		if (tg == null)
		{
			return null;
		}
		List<List<string>>.Enumerator enumerator = WeaponUpgrades.upgrades.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				List<string> current = enumerator.Current;
				if (!current.Contains(tg))
				{
					continue;
				}
				item = current[0];
				return item;
			}
			return tg;
		}
		finally
		{
			((IDisposable)(object)enumerator).Dispose();
		}
		return item;
	}
}