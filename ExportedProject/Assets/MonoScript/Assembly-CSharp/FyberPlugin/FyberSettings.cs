using System;
using UnityEngine;

namespace FyberPlugin
{
	public class FyberSettings : ScriptableObject
	{
		private const string fyberSettingsAssetName = "FyberSettings";

		private const string fyberSettingsPath = "Fyber/Resources";

		private const string fyberSettingsAssetExtension = ".asset";

		private static FyberSettings instance;

		[HideInInspector]
		[SerializeField]
		private string bundlesJson;

		[HideInInspector]
		[SerializeField]
		private string configJson;

		[HideInInspector]
		[SerializeField]
		private int bundlesCount;

		public static FyberSettings Instance
		{
			get
			{
				return FyberSettings.GetInstance();
			}
		}

		public FyberSettings()
		{
		}

		internal string BundlesConfigJson()
		{
			return this.configJson;
		}

		internal int BundlesCount()
		{
			return this.bundlesCount;
		}

		internal string BundlesInfoJson()
		{
			return this.bundlesJson;
		}

		private static FyberSettings GetInstance()
		{
			if (FyberSettings.instance == null)
			{
				PluginBridge.bridge = new PluginBridgeComponent();
				FyberSettings.instance = Resources.Load("FyberSettings") as FyberSettings;
				if (FyberSettings.instance == null)
				{
					FyberSettings.instance = ScriptableObject.CreateInstance<FyberSettings>();
				}
			}
			return FyberSettings.instance;
		}

		private void OnEnable()
		{
			FyberSettings.GetInstance();
		}
	}
}