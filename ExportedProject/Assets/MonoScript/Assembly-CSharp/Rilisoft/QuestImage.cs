using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal struct QuestImage
	{
		private HashSet<string> _arenaQuests;

		private HashSet<string> _campaignQuests;

		private HashSet<string> _modeQuests;

		private HashSet<string> _weaponQuests;

		private Dictionary<ConnectSceneNGUIController.RegimGame, string> _mapModeToSpriteName;

		private Dictionary<ShopNGUIController.CategoryNames, string> _mapWeaponToSpriteName;

		private readonly static Color s_defaultColor;

		private readonly static QuestImage s_instance;

		private HashSet<string> ArenaQuests
		{
			get
			{
				if (this._arenaQuests == null)
				{
					this._arenaQuests = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					this._arenaQuests.Add("surviveWavesInArena");
				}
				return this._arenaQuests;
			}
		}

		private HashSet<string> CampaignQuests
		{
			get
			{
				if (this._campaignQuests == null)
				{
					this._campaignQuests = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					this._campaignQuests.Add("killInCampaign");
					this._campaignQuests.Add("killNpcWithWeapon");
				}
				return this._campaignQuests;
			}
		}

		public static QuestImage Instance
		{
			get
			{
				return QuestImage.s_instance;
			}
		}

		private Dictionary<ConnectSceneNGUIController.RegimGame, string> MapModeToSpriteName
		{
			get
			{
				if (this._mapModeToSpriteName == null)
				{
					Dictionary<ConnectSceneNGUIController.RegimGame, string> regimGames = new Dictionary<ConnectSceneNGUIController.RegimGame, string>()
					{
						{ ConnectSceneNGUIController.RegimGame.Deathmatch, "mode_death_znachek" },
						{ ConnectSceneNGUIController.RegimGame.TimeBattle, "mode_time_znachek" },
						{ ConnectSceneNGUIController.RegimGame.TeamFight, "mode_team_znachek" },
						{ ConnectSceneNGUIController.RegimGame.DeadlyGames, "mode_deadly_games_znachek" },
						{ ConnectSceneNGUIController.RegimGame.FlagCapture, "mode_flag_znachek" },
						{ ConnectSceneNGUIController.RegimGame.CapturePoints, "mode_capture_point" }
					};
					this._mapModeToSpriteName = regimGames;
				}
				return this._mapModeToSpriteName;
			}
		}

		private Dictionary<ShopNGUIController.CategoryNames, string> MapWeaponToSpriteName
		{
			get
			{
				if (this._mapWeaponToSpriteName == null)
				{
					Dictionary<ShopNGUIController.CategoryNames, string> categoryNames = new Dictionary<ShopNGUIController.CategoryNames, string>()
					{
						{ ShopNGUIController.CategoryNames.BackupCategory, "shop_icons_backup" },
						{ ShopNGUIController.CategoryNames.MeleeCategory, "shop_icons_melee" },
						{ ShopNGUIController.CategoryNames.PremiumCategory, "shop_icons_premium" },
						{ ShopNGUIController.CategoryNames.PrimaryCategory, "shop_icons_primary" },
						{ ShopNGUIController.CategoryNames.SniperCategory, "shop_icons_sniper" },
						{ ShopNGUIController.CategoryNames.SpecilCategory, "shop_icons_special" }
					};
					this._mapWeaponToSpriteName = categoryNames;
				}
				return this._mapWeaponToSpriteName;
			}
		}

		static QuestImage()
		{
			QuestImage.s_defaultColor = new Color32(0, 253, 53, 255);
			QuestImage.s_instance = new QuestImage();
		}

		public Color GetColor(QuestBase quest)
		{
			if (quest == null)
			{
				return QuestImage.s_defaultColor;
			}
			if (this.CampaignQuests.Contains(quest.Id))
			{
				return new Color32(255, 184, 0, 255);
			}
			if (!this.ArenaQuests.Contains(quest.Id))
			{
				return QuestImage.s_defaultColor;
			}
			return new Color32(255, 121, 0, 255);
		}

		public string GetSpriteName(QuestBase quest)
		{
			if (quest == null)
			{
				return this.GetSpriteNameForMultiplayer();
			}
			ModeAccumulativeQuest modeAccumulativeQuest = quest as ModeAccumulativeQuest;
			if (modeAccumulativeQuest != null)
			{
				return this.GetSpriteNameForMultiplayer(modeAccumulativeQuest.Mode);
			}
			WeaponSlotAccumulativeQuest weaponSlotAccumulativeQuest = quest as WeaponSlotAccumulativeQuest;
			if (weaponSlotAccumulativeQuest != null)
			{
				if (this.CampaignQuests.Contains(quest.Id))
				{
					return this.GetSpriteNameForCampaign(weaponSlotAccumulativeQuest.WeaponSlot);
				}
				return this.GetSpriteNameForMultiplayer(weaponSlotAccumulativeQuest.WeaponSlot);
			}
			if (this.ArenaQuests.Contains(quest.Id))
			{
				return this.GetSpriteNameForArena();
			}
			if (this.CampaignQuests.Contains(quest.Id))
			{
				return this.GetSpriteNameForCampaign();
			}
			return this.GetSpriteNameForMultiplayer();
		}

		private string GetSpriteNameForArena()
		{
			return "mode_arena";
		}

		private string GetSpriteNameForCampaign()
		{
			return "star";
		}

		private string GetSpriteNameForCampaign(ShopNGUIController.CategoryNames weapon)
		{
			string str;
			if (this.MapWeaponToSpriteName.TryGetValue(weapon, out str))
			{
				return str;
			}
			return this.GetSpriteNameForCampaign();
		}

		private string GetSpriteNameForMultiplayer()
		{
			return "battle_now_znachek";
		}

		private string GetSpriteNameForMultiplayer(ConnectSceneNGUIController.RegimGame mode)
		{
			string str;
			if (this.MapModeToSpriteName.TryGetValue(mode, out str))
			{
				return str;
			}
			return this.GetSpriteNameForMultiplayer();
		}

		private string GetSpriteNameForMultiplayer(ShopNGUIController.CategoryNames weapon)
		{
			string str;
			if (this.MapWeaponToSpriteName.TryGetValue(weapon, out str))
			{
				return str;
			}
			return this.GetSpriteNameForMultiplayer();
		}
	}
}