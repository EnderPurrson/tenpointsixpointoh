using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FyberPlugin.LitJson;
using UnityEngine;

namespace FyberPlugin
{
	public class Ad
	{
		protected bool Started;

		private AdCallback adCallback
		{
			[CompilerGenerated]
			get
			{
				return _003CadCallback_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CadCallback_003Ek__BackingField = value;
			}
		}

		public AdFormat AdFormat
		{
			[CompilerGenerated]
			get
			{
				return _003CAdFormat_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CAdFormat_003Ek__BackingField = value;
			}
		}

		public string PlacementId
		{
			[CompilerGenerated]
			get
			{
				return _003CPlacementId_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CPlacementId_003Ek__BackingField = value;
			}
		}

		internal Ad()
		{
		}

		public Ad WithCallback(AdCallback callback)
		{
			adCallback = callback;
			return this;
		}

		public void Start()
		{
			if (!Started && CanStart())
			{
				Started = true;
				string guid = GetGuid();
				Debug.Log("Start - " + guid);
				RegisterCallbacks(guid);
				Dictionary<string, object> val = new Dictionary<string, object>(3);
				val.set_Item("id", (object)guid);
				val.set_Item("ad", (object)AdFormat);
				if (!string.IsNullOrEmpty(PlacementId))
				{
					val.set_Item("placementId", (object)PlacementId);
				}
				Debug.Log(string.Concat((object)"dict - ", val.get_Item("id")));
				AddExtraInfo(val);
				PluginBridge.StartAd(JsonMapper.ToJson(val));
			}
			else
			{
				FyberCallback.Instance.OnNativeError("This ad was already shown. You should request a new one.");
			}
		}

		protected virtual bool CanStart()
		{
			return true;
		}

		protected virtual string GetGuid()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Guid val = Guid.NewGuid();
			return ((Guid)(ref val)).ToString();
		}

		protected virtual void RegisterCallbacks(string guid)
		{
			if (adCallback == null)
			{
				adCallback = FyberCallback.Instance;
			}
			FyberCallbacksManager.AddCallback(guid, adCallback);
			FyberCallbacksManager.AddAd(guid, this);
		}

		protected virtual void AddExtraInfo(Dictionary<string, object> dict)
		{
		}
	}
}
