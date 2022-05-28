using System.Collections.Generic;
using FyberPlugin.LitJson;
using UnityEngine;

namespace FyberPlugin
{
	public class BannerAd : Ad
	{
		public const int POSITION_TOP = 1;

		public const int POSITION_BOTTOM = 0;

		private int? position;

		private BannerAdCallback bannerAdCallback;

		private string guid;

		internal BannerAd(string guid)
		{
			base.AdFormat = AdFormat.BANNER;
			this.guid = guid;
		}

		public BannerAd WithBannerAdCallback(BannerAdCallback callback)
		{
			if (!Started)
			{
				bannerAdCallback = callback;
			}
			return this;
		}

		public BannerAd WithPosition(int position)
		{
			if (!Started)
			{
				this.position = position;
			}
			return this;
		}

		public BannerAd DisplayAtTop()
		{
			return WithPosition(1);
		}

		public BannerAd DisplayAtBottom()
		{
			return WithPosition(0);
		}

		public void Hide()
		{
			if (Started)
			{
				Dictionary<string, object> val = new Dictionary<string, object>(3);
				val.set_Item("id", (object)guid);
				val.set_Item("ad", (object)base.AdFormat);
				val.set_Item("action", (object)"hide");
				PluginBridge.Banner(JsonMapper.ToJson(val));
			}
			else
			{
				Debug.Log("This instance has not been started yet, the \"Hide()\" method will not do anything");
			}
		}

		public void Show()
		{
			if (Started)
			{
				Dictionary<string, object> val = new Dictionary<string, object>(3);
				val.set_Item("id", (object)guid);
				val.set_Item("ad", (object)base.AdFormat);
				val.set_Item("action", (object)"show");
				PluginBridge.Banner(JsonMapper.ToJson(val));
			}
			else
			{
				Debug.Log("This instance has not been started yet, the \"Show()\" method will not do anything");
			}
		}

		public void Destroy()
		{
			if (Started)
			{
				Dictionary<string, object> val = new Dictionary<string, object>(3);
				val.set_Item("id", (object)guid);
				val.set_Item("ad", (object)base.AdFormat);
				val.set_Item("action", (object)"destroy");
				PluginBridge.Banner(JsonMapper.ToJson(val));
				FyberCallbacksManager.Instance.ClearBannerCallback(guid);
			}
			else
			{
				Debug.Log("This instance has not been started yet, the \"Destroy()\" method will not do anything");
			}
		}

		protected override bool CanStart()
		{
			Dictionary<string, object> val = new Dictionary<string, object>(3);
			val.set_Item("id", (object)guid);
			val.set_Item("ad", (object)base.AdFormat);
			val.set_Item("action", (object)"canStart");
			return PluginBridge.Banner(JsonMapper.ToJson(val));
		}

		protected override void RegisterCallbacks(string guid)
		{
			base.RegisterCallbacks(guid);
			if (bannerAdCallback == null)
			{
				bannerAdCallback = FyberCallback.Instance;
			}
			FyberCallbacksManager.AddBannerCallback(guid, bannerAdCallback);
		}

		protected override string GetGuid()
		{
			Debug.Log("GetGuid - " + guid);
			return guid;
		}

		protected override void AddExtraInfo(Dictionary<string, object> dict)
		{
			int? num = position;
			if (num.get_HasValue())
			{
				dict.set_Item("position", (object)position);
			}
		}
	}
}
