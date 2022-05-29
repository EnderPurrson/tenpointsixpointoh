using System;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal class GroupCreateResult : ResultBase, IGroupCreateResult, IResult
	{
		public const string IDKey = "id";

		public string GroupId
		{
			get
			{
				return JustDecompileGenerated_get_GroupId();
			}
			set
			{
				JustDecompileGenerated_set_GroupId(value);
			}
		}

		private string JustDecompileGenerated_GroupId_k__BackingField;

		public string JustDecompileGenerated_get_GroupId()
		{
			return this.JustDecompileGenerated_GroupId_k__BackingField;
		}

		private void JustDecompileGenerated_set_GroupId(string value)
		{
			this.JustDecompileGenerated_GroupId_k__BackingField = value;
		}

		public GroupCreateResult(string result) : base(result)
		{
			string str;
			if (this.ResultDictionary != null && this.ResultDictionary.TryGetValue<string>("id", out str))
			{
				this.GroupId = str;
			}
		}
	}
}