using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class KillOtherPlayerEventArgs : EventArgs
	{
		public bool Grenade
		{
			get;
			set;
		}

		public bool Headshot
		{
			get;
			set;
		}

		public ConnectSceneNGUIController.RegimGame Mode
		{
			get;
			set;
		}

		public bool Revenge
		{
			get;
			set;
		}

		public bool VictimIsFlagCarrier
		{
			get;
			set;
		}

		public ShopNGUIController.CategoryNames WeaponSlot
		{
			get;
			set;
		}

		public KillOtherPlayerEventArgs()
		{
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "mode", this.Mode },
				{ "weaponSlot", this.WeaponSlot },
				{ "headshot", this.Headshot },
				{ "grenade", this.Grenade },
				{ "revenge", this.Revenge },
				{ "victimIsFlagCarrier", this.VictimIsFlagCarrier }
			};
			return strs;
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}