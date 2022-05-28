using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FyberPlugin.LitJson;

namespace FyberPlugin
{
	public class Fyber
	{
		public const string Version = "8.3.0";

		private static Fyber fyber;

		private Dictionary<string, string> startParams;

		private Dictionary<string, object> startOptions;

		private bool initialized
		{
			[CompilerGenerated]
			get
			{
				return _003Cinitialized_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003Cinitialized_003Ek__BackingField = value;
			}
		}

		private Settings settings
		{
			[CompilerGenerated]
			get
			{
				return _003Csettings_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003Csettings_003Ek__BackingField = value;
			}
		}

		private Fyber()
		{
			startOptions = new Dictionary<string, object>(4);
			settings = new Settings();
		}

		public static Fyber With(string appId)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(appId))
			{
				throw new ArgumentException("App ID cannot be null nor empty");
			}
			if (fyber == null)
			{
				fyber = new Fyber();
			}
			if (!fyber.initialized)
			{
				fyber.startOptions.set_Item("appId", (object)appId);
			}
			return fyber;
		}

		public Fyber WithUserId(string userId)
		{
			if (!initialized)
			{
				startOptions.set_Item("userId", (object)userId);
			}
			return this;
		}

		public Fyber WithManualPrecaching()
		{
			if (!initialized)
			{
				CacheManager.PauseDownloads();
				startOptions.set_Item("startVideoPrecaching", (object)false);
			}
			return this;
		}

		public Fyber WithSecurityToken(string securityToken)
		{
			if (!initialized)
			{
				startOptions.set_Item("securityToken", (object)securityToken);
			}
			return this;
		}

		public Fyber WithParameters(Dictionary<string, string> parameters)
		{
			if (!initialized)
			{
				startParams = new Dictionary<string, string>((IDictionary<string, string>)(object)parameters);
			}
			return this;
		}

		public Settings Start()
		{
			if (!initialized)
			{
				if (startParams != null)
				{
					startOptions.set_Item("parameters", (object)startParams);
				}
				string json = JsonMapper.ToJson(startOptions);
				PluginBridge.Start(json);
			}
			return settings;
		}
	}
}
