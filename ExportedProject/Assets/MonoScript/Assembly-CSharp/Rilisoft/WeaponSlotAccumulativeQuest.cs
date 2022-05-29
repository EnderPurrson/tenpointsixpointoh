using System;
using System.Collections.Generic;

namespace Rilisoft
{
	public sealed class WeaponSlotAccumulativeQuest : AccumulativeQuestBase
	{
		private readonly ShopNGUIController.CategoryNames _weaponSlot;

		public ShopNGUIController.CategoryNames WeaponSlot
		{
			get
			{
				return this._weaponSlot;
			}
		}

		public WeaponSlotAccumulativeQuest(string id, long day, int slot, Rilisoft.Difficulty difficulty, Rilisoft.Reward reward, bool active, bool rewarded, int requiredCount, ShopNGUIController.CategoryNames weaponSlot, int initialCount = 0) : base(id, day, slot, difficulty, reward, active, rewarded, requiredCount, initialCount)
		{
			this._weaponSlot = weaponSlot;
		}

		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			base.AppendProperties(properties);
			properties["weaponSlot"] = this._weaponSlot;
		}
	}
}