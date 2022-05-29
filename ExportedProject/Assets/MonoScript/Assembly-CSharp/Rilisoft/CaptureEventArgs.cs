using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class CaptureEventArgs : EventArgs
	{
		public ConnectSceneNGUIController.RegimGame Mode
		{
			get;
			set;
		}

		public CaptureEventArgs()
		{
		}

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>()
			{
				{ "mode", this.Mode }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}