using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class KillMonsterEventArgs : EventArgs
	{
		public bool Campaign
		{
			get;
			set;
		}

		public ShopNGUIController.CategoryNames WeaponSlot
		{
			get;
			set;
		}

		public KillMonsterEventArgs()
		{
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "weaponSlot", this.WeaponSlot },
				{ "campaign", this.Campaign }
			};
			return strs;
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}