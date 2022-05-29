using DevToDev;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class AnalyticsFacade
	{
		private static bool _initialized;

		private static Rilisoft.DevToDevFacade _devToDevFacade;

		private static Rilisoft.AppsFlyerFacade _appsFlyerFacade;

		private readonly static Lazy<string> _simpleEventFormat;

		private readonly static Lazy<string> _parametrizedEventFormat;

		internal static Rilisoft.AppsFlyerFacade AppsFlyerFacade
		{
			get
			{
				return AnalyticsFacade._appsFlyerFacade;
			}
		}

		internal static Rilisoft.DevToDevFacade DevToDevFacade
		{
			get
			{
				return AnalyticsFacade._devToDevFacade;
			}
		}

		public static bool DuplicateToConsoleByDefault
		{
			get;
			set;
		}

		public static bool LoggingEnabled
		{
			set
			{
				Rilisoft.DevToDevFacade.LoggingEnabled = value;
				Rilisoft.AppsFlyerFacade.LoggingEnabled = value;
			}
		}

		static AnalyticsFacade()
		{
			AnalyticsFacade._initialized = false;
			AnalyticsFacade._simpleEventFormat = new Lazy<string>(new Func<string>(AnalyticsFacade.InitializeSimpleEventFormat));
			AnalyticsFacade._parametrizedEventFormat = new Lazy<string>(new Func<string>(AnalyticsFacade.InitializeParametrizedEventFormat));
		}

		public AnalyticsFacade()
		{
		}

		public static void CurrencyAccrual(int amount, string currencyName, AnalyticsConstants.AccrualType accrualType = 0)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.CurrencyAccrual(amount, currencyName, accrualType, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void CurrencyAccrual(int amount, string currencyName, AnalyticsConstants.AccrualType accrualType, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AccrualType accrualType1 = AccrualType.Earned;
				if (accrualType == AnalyticsConstants.AccrualType.Purchased)
				{
					accrualType1 = AccrualType.Purchased;
				}
				AnalyticsFacade._devToDevFacade.CurrencyAccrual(amount, currencyName, accrualType1);
			}
			if (duplicateToConsole)
			{
				string value = AnalyticsFacade._parametrizedEventFormat.Value;
				object[] objArray = new object[] { "CURRENCY_ACCRUAL_BUILTIN", null };
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "amount", amount.ToString() },
					{ "currencyName", currencyName },
					{ "accrualType", accrualType.ToString() }
				};
				objArray[1] = Json.Serialize(strs);
				Debug.LogFormat(value, objArray);
			}
		}

		public static void Flush()
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.SendBufferedEvents();
			}
		}

		public static void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.InAppPurchase(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchaseCurrency, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.InAppPurchase(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchaseCurrency);
			}
			if (duplicateToConsole)
			{
				string value = AnalyticsFacade._parametrizedEventFormat.Value;
				object[] objArray = new object[] { "IN_APP_PURCHASE_BUILTIN", null };
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "purchaseId", purchaseId },
					{ "purchaseType", purchaseType },
					{ "purchaseAmount", purchaseAmount.ToString() },
					{ "purchasePrice", purchasePrice.ToString() },
					{ "purchaseCurrency", purchaseCurrency }
				};
				objArray[1] = Json.Serialize(strs);
				Debug.LogFormat(value, objArray);
			}
		}

		public static void Initialize()
		{
			RuntimePlatform buildTargetPlatform;
			if (AnalyticsFacade._initialized)
			{
				return;
			}
			if (MiscAppsMenu.Instance == null)
			{
				Debug.LogError("MiscAppsMenu.Instance == null");
				return;
			}
			if (MiscAppsMenu.Instance.misc == null)
			{
				Debug.LogError("MiscAppsMenu.Instance.misc == null");
				return;
			}
			try
			{
				HiddenSettings instance = MiscAppsMenu.Instance.misc;
				AnalyticsFacade.DuplicateToConsoleByDefault = Defs.IsDeveloperBuild;
				AnalyticsFacade.LoggingEnabled = Defs.IsDeveloperBuild;
				string empty = string.Empty;
				string str = string.Empty;
				if (Defs.IsDeveloperBuild || Application.isEditor)
				{
					buildTargetPlatform = BuildSettings.BuildTargetPlatform;
					switch (buildTargetPlatform)
					{
						case RuntimePlatform.IPhonePlayer:
						{
							empty = "92002d69-82d8-067e-997d-88d1c5e804f7";
							str = "tQ4zhKGBvyFVObPUofaiHj7pSAcWn3Mw";
							goto case RuntimePlatform.XBOX360;
						}
						case RuntimePlatform.PS3:
						case RuntimePlatform.XBOX360:
						{
							break;
						}
						case RuntimePlatform.Android:
						{
							empty = "8517441f-d330-04c5-b621-5d88e92f50e3";
							str = "xkjaPTLIgGQKs5MftquXrEHDW0y8OBAS";
							goto case RuntimePlatform.XBOX360;
						}
						default:
						{
							goto case RuntimePlatform.XBOX360;
						}
					}
				}
				else
				{
					buildTargetPlatform = BuildSettings.BuildTargetPlatform;
					switch (buildTargetPlatform)
					{
						case RuntimePlatform.IPhonePlayer:
						{
							empty = "3c77b196-8042-0dab-a5dc-92eb4377aa8e";
							str = instance.devtodevSecretIos;
							break;
						}
						case RuntimePlatform.Android:
						{
							if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
							{
								empty = "8d1482db-5181-0647-a80e-decf21db619f";
								str = instance.devtodevSecretGoogle;
							}
							else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
							{
								empty = "531e6d54-b959-06c1-8a38-6dfdfbf309eb";
								str = instance.devtodevSecretAmazon;
							}
							break;
						}
						default:
						{
							if (buildTargetPlatform == RuntimePlatform.MetroPlayerX64)
							{
								empty = "cd19ad66-971e-09b2-b449-ba84d3fb52d8";
								str = instance.devtodevSecretWsa;
								break;
							}
							else
							{
								break;
							}
						}
					}
				}
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Initializing DevtoDev {0}; appId: '*{1}', appSecret: '*{2}'...", new object[] { Rilisoft.DevToDevFacade.Version, empty.Substring(Math.Max(empty.Length - 4, 0)), str.Substring(Math.Max(str.Length - 4, 0)) });
				}
				AnalyticsFacade.InitializeDevToDev(empty, str);
				string empty1 = string.Empty;
				string str1 = instance.appsFlyerAppKey;
				if (!Defs.IsDeveloperBuild && !Application.isEditor)
				{
					buildTargetPlatform = BuildSettings.BuildTargetPlatform;
					switch (buildTargetPlatform)
					{
						case RuntimePlatform.IPhonePlayer:
						{
							empty1 = "ecd1e376-8e2f-45e4-a9dc-9e938f999d20";
							break;
						}
						case RuntimePlatform.Android:
						{
							if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
							{
								empty1 = "com.pixel.gun3d";
							}
							else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
							{
								empty1 = "com.PixelGun.a3D";
							}
							break;
						}
					}
				}
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Initializing AppsFlyer; appsFlyerAppKey: '*{0}', appsFlyerAppId: '*{1}'...", new object[] { str1.Substring(Math.Max(str1.Length - 4, 0)), empty1.Substring(Math.Max(empty1.Length - 4, 0)) });
				}
				AnalyticsFacade.InitializeAppsFlyer(str1, empty1);
				AnalyticsFacade._initialized = true;
			}
			catch (Exception exception)
			{
				Debug.LogError(string.Concat("Exception in AnalyticsFacade.Initialize: ", exception));
			}
		}

		private static void InitializeAppsFlyer(string appKey, string appId)
		{
			AnalyticsFacade._appsFlyerFacade = new Rilisoft.AppsFlyerFacade(appKey, appId);
			AnalyticsFacade._appsFlyerFacade.TrackAppLaunch();
		}

		private static void InitializeDevToDev(string appKey, string secretKey)
		{
			AnalyticsFacade._devToDevFacade = new Rilisoft.DevToDevFacade(appKey, secretKey);
		}

		private static string InitializeParametrizedEventFormat()
		{
			return (!Application.isEditor ? "\"{0}\": {1}" : "<color=magenta>\"{0}\": {1}</color>");
		}

		private static string InitializeSimpleEventFormat()
		{
			return (!Application.isEditor ? "\"{0}\"" : "<color=magenta>\"{0}\"</color>");
		}

		public static void LevelUp(int level)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.LevelUp(level, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void LevelUp(int level, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			Dictionary<string, int> strs = new Dictionary<string, int>()
			{
				{ "Coins", Storager.getInt("Coins", false) },
				{ "GemsCurrency", Storager.getInt("GemsCurrency", false) }
			};
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.LevelUp(level, strs);
			}
			if (duplicateToConsole)
			{
				string value = AnalyticsFacade._parametrizedEventFormat.Value;
				object[] objArray = new object[] { "LEVELUP_BUILTIN", null };
				Dictionary<string, object> strs1 = new Dictionary<string, object>()
				{
					{ "level", level.ToString() },
					{ "resources", strs }
				};
				objArray[1] = Json.Serialize(strs1);
				Debug.LogFormat(value, objArray);
			}
		}

		public static void RealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.RealPayment(paymentId, inAppPrice, inAppName, currencyIsoCode, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void RealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.RealPayment(paymentId, inAppPrice, inAppName, currencyIsoCode);
			}
			Lazy<Dictionary<string, string>> lazy = new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>(4)
			{
				{ "af_revenue", inAppPrice.ToString("0.00", CultureInfo.InvariantCulture) },
				{ "af_content_id", inAppName },
				{ "af_currency", currencyIsoCode },
				{ "af_receipt_id", paymentId }
			});
			if (AnalyticsFacade._appsFlyerFacade != null)
			{
				AnalyticsFacade._appsFlyerFacade.TrackRichEvent("af_purchase", lazy.Value);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[] { "REAL_PAYMENT_BUILTIN", Json.Serialize(lazy.Value) });
			}
		}

		public static void SendCustomEvent(string eventName)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEvent(eventName, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void SendCustomEvent(string eventName, IDictionary<string, object> eventParams)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEvent(eventName, eventParams, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void SendCustomEvent(string eventName, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.SendCustomEvent(eventName);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._simpleEventFormat.Value, new object[] { eventName });
			}
		}

		public static void SendCustomEvent(string eventName, IDictionary<string, object> eventParams, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.SendCustomEvent(eventName, eventParams);
			}
			if (duplicateToConsole)
			{
				string str = Json.Serialize(eventParams);
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[] { eventName, str });
			}
		}

		public static void SendCustomEventToAppsFlyer(string eventName, Dictionary<string, string> eventParams)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, eventParams, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void SendCustomEventToAppsFlyer(string eventName, Dictionary<string, string> eventParams, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (AnalyticsFacade._appsFlyerFacade != null)
			{
				AnalyticsFacade._appsFlyerFacade.TrackRichEvent(eventName, eventParams);
			}
			if (duplicateToConsole)
			{
				string str = Json.Serialize(eventParams);
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[] { eventName, str });
			}
		}

		public static void SendFirstTimeRealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendFirstTimeRealPayment(paymentId, inAppPrice, inAppName, currencyIsoCode, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void SendFirstTimeRealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			Lazy<Dictionary<string, string>> lazy = new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>(4)
			{
				{ "af_revenue", inAppPrice.ToString("0.00", CultureInfo.InvariantCulture) },
				{ "af_content_id", inAppName },
				{ "af_currency", currencyIsoCode },
				{ "af_receipt_id", paymentId }
			});
			if (AnalyticsFacade._appsFlyerFacade != null)
			{
				AnalyticsFacade._appsFlyerFacade.TrackRichEvent("first_buy", lazy.Value);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[] { "First time real payment", Json.Serialize(lazy.Value) });
			}
		}

		public static void Tutorial(AnalyticsConstants.TutorialState step)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.Tutorial(step, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		public static void Tutorial(AnalyticsConstants.TutorialState step, bool duplicateToConsole)
		{
			int start;
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				if (step == AnalyticsConstants.TutorialState.Started)
				{
					start = TutorialState.Start;
				}
				else if (step != AnalyticsConstants.TutorialState.Finished)
				{
					start = (int)step;
				}
				else
				{
					start = TutorialState.Finish;
				}
				AnalyticsFacade._devToDevFacade.Tutorial(start);
			}
			if (duplicateToConsole)
			{
				string value = AnalyticsFacade._parametrizedEventFormat.Value;
				object[] objArray = new object[] { "TUTORIAL_BUILTIN", null };
				Dictionary<string, object> strs = new Dictionary<string, object>()
				{
					{ "step", step.ToString() }
				};
				objArray[1] = Json.Serialize(strs);
				Debug.LogFormat(value, objArray);
			}
		}
	}
}