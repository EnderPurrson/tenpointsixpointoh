using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public sealed class SocialInteractionEventArgs : EventArgs
	{
		public string Kind
		{
			get;
			set;
		}

		public SocialInteractionEventArgs()
		{
		}

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>()
			{
				{ "kind", this.Kind }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}
	}
}