using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class WinEventArgs : EventArgs
	{
		public string Map
		{
			get;
			set;
		}

		public ConnectSceneNGUIController.RegimGame Mode
		{
			get;
			set;
		}

		public WinEventArgs()
		{
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "mode", this.Mode },
				{ "map", this.Map }
			};
			return strs;
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}