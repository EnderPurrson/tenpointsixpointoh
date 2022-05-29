using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal class ShareResult : ResultBase, IResult, IShareResult
	{
		public string PostId
		{
			get
			{
				return JustDecompileGenerated_get_PostId();
			}
			set
			{
				JustDecompileGenerated_set_PostId(value);
			}
		}

		private string JustDecompileGenerated_PostId_k__BackingField;

		public string JustDecompileGenerated_get_PostId()
		{
			return this.JustDecompileGenerated_PostId_k__BackingField;
		}

		private void JustDecompileGenerated_set_PostId(string value)
		{
			this.JustDecompileGenerated_PostId_k__BackingField = value;
		}

		internal ShareResult(string result) : base(result)
		{
			object obj;
			if (this.ResultDictionary != null && this.ResultDictionary.TryGetValue("id", out obj))
			{
				this.PostId = obj as string;
			}
		}
	}
}