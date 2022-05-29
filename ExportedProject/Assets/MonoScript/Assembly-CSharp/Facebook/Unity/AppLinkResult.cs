using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Facebook.Unity
{
	internal class AppLinkResult : ResultBase, IAppLinkResult, IResult
	{
		public IDictionary<string, object> Extras
		{
			get
			{
				return JustDecompileGenerated_get_Extras();
			}
			set
			{
				JustDecompileGenerated_set_Extras(value);
			}
		}

		private IDictionary<string, object> JustDecompileGenerated_Extras_k__BackingField;

		public IDictionary<string, object> JustDecompileGenerated_get_Extras()
		{
			return this.JustDecompileGenerated_Extras_k__BackingField;
		}

		private void JustDecompileGenerated_set_Extras(IDictionary<string, object> value)
		{
			this.JustDecompileGenerated_Extras_k__BackingField = value;
		}

		public string Ref
		{
			get
			{
				return JustDecompileGenerated_get_Ref();
			}
			set
			{
				JustDecompileGenerated_set_Ref(value);
			}
		}

		private string JustDecompileGenerated_Ref_k__BackingField;

		public string JustDecompileGenerated_get_Ref()
		{
			return this.JustDecompileGenerated_Ref_k__BackingField;
		}

		private void JustDecompileGenerated_set_Ref(string value)
		{
			this.JustDecompileGenerated_Ref_k__BackingField = value;
		}

		public string TargetUrl
		{
			get
			{
				return JustDecompileGenerated_get_TargetUrl();
			}
			set
			{
				JustDecompileGenerated_set_TargetUrl(value);
			}
		}

		private string JustDecompileGenerated_TargetUrl_k__BackingField;

		public string JustDecompileGenerated_get_TargetUrl()
		{
			return this.JustDecompileGenerated_TargetUrl_k__BackingField;
		}

		private void JustDecompileGenerated_set_TargetUrl(string value)
		{
			this.JustDecompileGenerated_TargetUrl_k__BackingField = value;
		}

		public string Url
		{
			get
			{
				return JustDecompileGenerated_get_Url();
			}
			set
			{
				JustDecompileGenerated_set_Url(value);
			}
		}

		private string JustDecompileGenerated_Url_k__BackingField;

		public string JustDecompileGenerated_get_Url()
		{
			return this.JustDecompileGenerated_Url_k__BackingField;
		}

		private void JustDecompileGenerated_set_Url(string value)
		{
			this.JustDecompileGenerated_Url_k__BackingField = value;
		}

		public AppLinkResult(string result) : base(result)
		{
			string str;
			string str1;
			string str2;
			IDictionary<string, object> strs;
			if (this.ResultDictionary != null)
			{
				if (this.ResultDictionary.TryGetValue<string>("url", out str))
				{
					this.Url = str;
				}
				if (this.ResultDictionary.TryGetValue<string>("target_url", out str1))
				{
					this.TargetUrl = str1;
				}
				if (this.ResultDictionary.TryGetValue<string>("ref", out str2))
				{
					this.Ref = str2;
				}
				if (this.ResultDictionary.TryGetValue<IDictionary<string, object>>("extras", out strs))
				{
					this.Extras = strs;
				}
			}
		}
	}
}