using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Cheat;
using DevToDev.Core.Network;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated;
using DevToDev.Data.Metrics.Aggregated.CustomEvent;
using DevToDev.Data.Metrics.Aggregated.Progression;
using DevToDev.Data.Metrics.Simple;
using DevToDev.Data.Metrics.Specific;
using DevToDev.Logic.Cross;
using DevToDev.Push.Data.Metrics.Simple;
using UnityEngine;

namespace DevToDev.Logic
{
	internal class SDKClient
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass4
		{
			public SDKClient _003C_003E4__this;

			public long timestamp;

			public void _003CEndSession_003Eb__3()
			{
				Log.D("Executing stop session");
				_003C_003E4__this.UsersStorage.SessionClose(timestamp);
				_003C_003E4__this.SaveAll();
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass7
		{
			public SDKClient _003C_003E4__this;

			public int newLevel;

			public Dictionary<string, int> values;

			public void _003CLevelUp_003Eb__6()
			{
				_003C_003E4__this.UsersStorage.ActiveUser.LevelUp(newLevel, values);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClassa
		{
			public SDKClient _003C_003E4__this;

			public IDictionary<ReferralProperty, string> referralData;

			public void _003CReferral_003Eb__9()
			{
				_003C_003E4__this.AddEvent(new ReferralEvent(referralData));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClassd
		{
			public SDKClient _003C_003E4__this;

			public int step;

			public void _003CTutorial_003Eb__c()
			{
				_003C_003E4__this.AddEvent(new TutorialEvent(step));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass10
		{
			public SDKClient _003C_003E4__this;

			public string transactionId;

			public float inAppPrice;

			public string inAppName;

			public string inAppCurrencyISOCode;

			public void _003CRealPayment_003Eb__f()
			{
				_003C_003E4__this.AddEvent(new RealPayment(transactionId, inAppPrice, inAppName, inAppCurrencyISOCode));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass13
		{
			public SDKClient _003C_003E4__this;

			public SocialNetwork network;

			public void _003CSocialNetworkConnect_003Eb__12()
			{
				_003C_003E4__this.AddEvent(new SocialNetworkConnectEvent(network));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass16
		{
			public SDKClient _003C_003E4__this;

			public SocialNetwork network;

			public string reason;

			public void _003CSocialNetworkPost_003Eb__15()
			{
				_003C_003E4__this.AddEvent(new SocialNetworkPostEvent(network, reason));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass19
		{
			public SDKClient _003C_003E4__this;

			public string purchaseId;

			public string purchaseType;

			public int purchaseAmount;

			public Dictionary<string, int> resources;

			public void _003CInAppPurchase_003Eb__18()
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				bool flag = true;
				Enumerator<string, int> enumerator = resources.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, int> current = enumerator.get_Current();
						_003C_003E4__this.usersStorage.ActiveUser.UpSpend(current.get_Value(), current.get_Key());
						_003C_003E4__this.AddEvent(new InAppPurchase(purchaseId, purchaseType, flag ? purchaseAmount : 0, current.get_Value(), current.get_Key()));
						flag = false;
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1c
		{
			public SDKClient _003C_003E4__this;

			public string currencyName;

			public int amount;

			public AccrualType accrualType;

			public void _003CCurrencyAccrual_003Eb__1b()
			{
				if (accrualType == AccrualType.Purchased)
				{
					Log.D(string.Concat(new object[4] { "Accrual Purchased: ", amount, " of ", currencyName }));
					_003C_003E4__this.usersStorage.ActiveUser.UpBought(amount, currencyName);
				}
				else if (accrualType == AccrualType.Earned)
				{
					Log.D(string.Concat(new object[4] { "Accrual Earned: ", amount, " of ", currencyName }));
					_003C_003E4__this.usersStorage.ActiveUser.UpEarned(amount, currencyName);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1f
		{
			public SDKClient _003C_003E4__this;

			public string eventName;

			public CustomEventParams parameters;

			public void _003CCustomEvent_003Eb__1e()
			{
				_003C_003E4__this.AddEvent(new CustomEvent(CustomEventData.SingleData(eventName, parameters)));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass22
		{
			public SDKClient _003C_003E4__this;

			public string eventId;

			public ProgressionEventParams eventParams;

			public void _003CStartProgressionEvent_003Eb__21()
			{
				eventParams.SetEventName(eventId);
				eventParams.SetStartTime(DeviceHelper.Instance.GetUnixTime());
				_003C_003E4__this.AddEvent(new ProgressionEvent(eventParams));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass25
		{
			public SDKClient _003C_003E4__this;

			public string eventId;

			public ProgressionEventParams eventParams;

			public void _003CEndProgressionEvent_003Eb__24()
			{
				eventParams.SetEventName(eventId);
				eventParams.SetFinishTime(DeviceHelper.Instance.GetUnixTime());
				_003C_003E4__this.AddEvent(new ProgressionEvent(eventParams));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass28
		{
			public SDKClient _003C_003E4__this;

			public int age;

			public void _003CAge_003Eb__27()
			{
				_003C_003E4__this.AddEvent(new AgeEvent(age));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2b
		{
			public SDKClient _003C_003E4__this;

			public Gender gender;

			public void _003CGender_003Eb__2a()
			{
				_003C_003E4__this.AddEvent(new GenderEvent(gender));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2e
		{
			public SDKClient _003C_003E4__this;

			public bool isCheater;

			public void _003CCheater_003Eb__2d()
			{
				_003C_003E4__this.AddEvent(new CheaterEvent(isCheater));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass32
		{
			public SDKClient _003C_003E4__this;

			public int currentLevel;

			public void _003CSetCurrentLevel_003Eb__31()
			{
				_003C_003E4__this.UsersStorage.ActiveUser.Level = currentLevel;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass35
		{
			public SDKClient _003C_003E4__this;

			public string fromUserId;

			public string toUserId;

			public void _003CReplaceUserId_003Eb__34()
			{
				_003C_003E4__this.UsersStorage.ReplaceUserId(fromUserId, toUserId);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass38
		{
			public SDKClient _003C_003E4__this;

			public string value;

			public void _003Cset_ApplicationVersion_003Eb__37()
			{
				_003C_003E4__this.UsersStorage.ApplicationVersion = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass3b
		{
			public SDKClient _003C_003E4__this;

			public string value;

			public void _003Cset_ActiveUserId_003Eb__3a()
			{
				_003C_003E4__this.UsersStorage.ActiveUserId = value;
			}
		}

		private static List<ISaveable> storages;

		private string appKey;

		private string appSecret;

		private NetworkStorage networkStorage;

		private UsersStorage usersStorage;

		private MetricsController metricsController;

		private List<DevToDev.Data.Metrics.Event> futureEvents;

		private List<Action> futureExecutions;

		private List<Action> futureExecutionsAfterInit;

		private bool isInited;

		private bool initWasCalled;

		private People activeUser;

		private static SDKClient instance;

		public NetworkStorage NetworkStorage
		{
			get
			{
				if (networkStorage == null)
				{
					networkStorage = new NetworkStorage().Load() as NetworkStorage;
				}
				return networkStorage;
			}
		}

		public AsyncOperationDispatcher AsyncOperationDispatcher
		{
			get
			{
				return AsyncOperationDispatcher.Create();
			}
		}

		public UsersStorage UsersStorage
		{
			get
			{
				if (usersStorage == null)
				{
					usersStorage = new UsersStorage().Load() as UsersStorage;
					try
					{
						usersStorage.LoadNative();
					}
					catch (global::System.Exception ex)
					{
						Log.E("Error loading native data " + ex.get_Message() + "\r\n" + ex.get_StackTrace());
						new NativeDataLoader().RemoveNativeData();
					}
				}
				return usersStorage;
			}
		}

		public MetricsController MetricsController
		{
			get
			{
				if (metricsController == null)
				{
					metricsController = new MetricsController();
				}
				return metricsController;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return isInited;
			}
		}

		public static SDKClient Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new SDKClient();
				}
				return instance;
			}
		}

		public string ApplicationVersion
		{
			get
			{
				if (!initWasCalled)
				{
					return null;
				}
				return UsersStorage.ApplicationVersion;
			}
			set
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Expected O, but got Unknown
				_003C_003Ec__DisplayClass38 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass38();
				_003C_003Ec__DisplayClass.value = value;
				_003C_003Ec__DisplayClass._003C_003E4__this = this;
				ExecuteFirstUsers(new Action(_003C_003Ec__DisplayClass._003Cset_ApplicationVersion_003Eb__37));
			}
		}

		public string ActiveUserId
		{
			get
			{
				if (!initWasCalled)
				{
					return null;
				}
				return UsersStorage.ActiveUserId;
			}
			set
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Expected O, but got Unknown
				_003C_003Ec__DisplayClass3b _003C_003Ec__DisplayClass3b = new _003C_003Ec__DisplayClass3b();
				_003C_003Ec__DisplayClass3b.value = value;
				_003C_003Ec__DisplayClass3b._003C_003E4__this = this;
				ExecuteFirstUsers(new Action(_003C_003Ec__DisplayClass3b._003Cset_ActiveUserId_003Eb__3a));
			}
		}

		public string AppKey
		{
			get
			{
				return appKey;
			}
		}

		public string AppSecret
		{
			get
			{
				return appSecret;
			}
		}

		public People ActiveUser
		{
			get
			{
				if (activeUser == null)
				{
					activeUser = new People();
				}
				return activeUser;
			}
		}

		private SDKClient()
		{
			isInited = false;
			initWasCalled = false;
			futureEvents = new List<DevToDev.Data.Metrics.Event>();
			futureExecutions = new List<Action>();
			futureExecutionsAfterInit = new List<Action>();
			storages = new List<ISaveable>();
			storages.Add((ISaveable)new RFC4122(new ObjectInfo()));
			storages.Add((ISaveable)new DataStorage(new ObjectInfo()));
			storages.Add((ISaveable)new User(new ObjectInfo()));
			storages.Add((ISaveable)new UsersStorage(new ObjectInfo()));
			storages.Add((ISaveable)new Device(new ObjectInfo()));
			storages.Add((ISaveable)new LevelData(new ObjectInfo()));
			storages.Add((ISaveable)new MetricsStorage(new ObjectInfo()));
			storages.Add((ISaveable)new NetworkStorage(new ObjectInfo()));
			storages.Add((ISaveable)new SessionStorage(new ObjectInfo()));
			storages.Add((ISaveable)new UserMetrics(new ObjectInfo()));
			storages.Add((ISaveable)new AgeEvent(new ObjectInfo()));
			storages.Add((ISaveable)new ApplicationInfoEvent(new ObjectInfo()));
			storages.Add((ISaveable)new CheaterEvent(new ObjectInfo()));
			storages.Add((ISaveable)new DeviceInfoEvent(new ObjectInfo()));
			storages.Add((ISaveable)new GameSessionEvent(new ObjectInfo()));
			storages.Add((ISaveable)new GenderEvent(new ObjectInfo()));
			storages.Add((ISaveable)new GetServerNodeEvent(new ObjectInfo()));
			storages.Add((ISaveable)new LocationEvent(new ObjectInfo()));
			storages.Add((ISaveable)new SocialNetworkConnectEvent(new ObjectInfo()));
			storages.Add((ISaveable)new SocialNetworkPostEvent(new ObjectInfo()));
			storages.Add((ISaveable)new TutorialEvent(new ObjectInfo()));
			storages.Add((ISaveable)new UserInfoEvent(new ObjectInfo()));
			storages.Add((ISaveable)new RealPayment(new ObjectInfo()));
			storages.Add((ISaveable)new InAppPurchase(new ObjectInfo()));
			storages.Add((ISaveable)new CustomEvent(new ObjectInfo()));
			storages.Add((ISaveable)new CustomEventData(new ObjectInfo()));
			storages.Add((ISaveable)new CustomEventParams(new ObjectInfo()));
			storages.Add((ISaveable)new RealPaymentData(new ObjectInfo()));
			storages.Add((ISaveable)new InAppPurchaseData(new ObjectInfo()));
			storages.Add((ISaveable)new TokenSendEvent(new ObjectInfo()));
			storages.Add((ISaveable)new PushOpenEvent(new ObjectInfo()));
			storages.Add((ISaveable)new PushReceivedEvent(new ObjectInfo()));
			storages.Add((ISaveable)new VerifyEvent(new ObjectInfo()));
			storages.Add((ISaveable)new CheatData(new ObjectInfo()));
			storages.Add((ISaveable)new ReferralEvent(new ObjectInfo()));
			storages.Add((ISaveable)new PeopleEvent(new ObjectInfo()));
			storages.Add((ISaveable)new PeopleLogic(new ObjectInfo()));
			storages.Add((ISaveable)new ProgressionEventParams(new ObjectInfo()));
			storages.Add((ISaveable)new LocationEventParams(new ObjectInfo()));
			storages.Add((ISaveable)new ProgressionEvent(new ObjectInfo()));
		}

		public void Initialize(string appKey, string appSecret)
		{
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			Action val = null;
			if (appKey == null || appSecret == null)
			{
				Log.R("Application keys can't be null.");
			}
			else if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(appSecret))
			{
				Log.R("Application keys can't be empty.");
			}
			else
			{
				if (initWasCalled || isInited)
				{
					return;
				}
				initWasCalled = true;
				try
				{
					AsyncOperationDispatcher.Create();
				}
				catch (global::System.Exception)
				{
					throw new global::System.Exception("Initialize method should be called from Unity main thread. Don't worry, this is the only method that must be called from the main thread.");
				}
				this.appKey = appKey;
				this.appSecret = appSecret;
				Log.R(string.Format("Initializing sdk with key {0} and version {1}", (object)this.appKey, (object)Analytics.SDKVersion));
				MetricsController obj = MetricsController;
				obj.PeriodicSendHandler = (EventHandler)global::System.Delegate.Combine((global::System.Delegate)(object)obj.PeriodicSendHandler, (global::System.Delegate)new EventHandler(UsersStorage.OnPeriodicSend));
				MetricsController.Resume();
				Enumerator<Action> enumerator = futureExecutionsAfterInit.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Action current = enumerator.get_Current();
						current.Invoke();
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				futureExecutionsAfterInit.Clear();
				SDKRequests.GetServerNodeV3(OnServerConfigReceived);
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.DTDApplicationLifecycle");
					string text = androidJavaClass.CallStatic<string>("readLostSession", new object[0]);
					Log.D("Last session was lost on " + text);
					long timestamp = 0L;
					if (long.TryParse(text, ref timestamp))
					{
						UsersStorage.SessionClose(timestamp, true);
					}
					StartSession();
					if (val == null)
					{
						val = new Action(_003CInitialize_003Eb__0);
					}
					Execute(val);
				}
				else
				{
					StartSession();
				}
			}
		}

		public void Resume()
		{
			MetricsController.Resume();
			StartSession();
		}

		public void Suspend(long timestamp = 0L)
		{
			EndSession(timestamp);
			MetricsController.Suspend();
		}

		public void StartSession()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			Execute(new Action(_003CStartSession_003Eb__2));
		}

		public bool IsSessionActive()
		{
			return usersStorage.ActiveUser.IsSessionActive();
		}

		public void EndSession(long timestamp = 0L)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClass4 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass4();
			_003C_003Ec__DisplayClass.timestamp = timestamp;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClass._003CEndSession_003Eb__3));
		}

		public void LevelUp(int newLevel, Dictionary<string, int> values)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			_003C_003Ec__DisplayClass7 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass7();
			_003C_003Ec__DisplayClass.newLevel = newLevel;
			_003C_003Ec__DisplayClass.values = values;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClass._003CLevelUp_003Eb__6));
		}

		public void Referral(IDictionary<ReferralProperty, string> referralData)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClassa _003C_003Ec__DisplayClassa = new _003C_003Ec__DisplayClassa();
			_003C_003Ec__DisplayClassa.referralData = referralData;
			_003C_003Ec__DisplayClassa._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClassa._003CReferral_003Eb__9));
		}

		public void Tutorial(int step)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			_003C_003Ec__DisplayClassd _003C_003Ec__DisplayClassd = new _003C_003Ec__DisplayClassd();
			_003C_003Ec__DisplayClassd.step = step;
			_003C_003Ec__DisplayClassd._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClassd.step < TutorialState.Finish)
			{
				Log.R("Tutorial step cannot be less then -2");
			}
			else
			{
				Execute(new Action(_003C_003Ec__DisplayClassd._003CTutorial_003Eb__c));
			}
		}

		public void RealPayment(string transactionId, float inAppPrice, string inAppName, string inAppCurrencyISOCode)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			_003C_003Ec__DisplayClass10 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass10();
			_003C_003Ec__DisplayClass.transactionId = transactionId;
			_003C_003Ec__DisplayClass.inAppPrice = inAppPrice;
			_003C_003Ec__DisplayClass.inAppName = inAppName;
			_003C_003Ec__DisplayClass.inAppCurrencyISOCode = inAppCurrencyISOCode;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass.transactionId != null && _003C_003Ec__DisplayClass.inAppName != null && _003C_003Ec__DisplayClass.inAppCurrencyISOCode != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass._003CRealPayment_003Eb__f));
			}
		}

		public void SocialNetworkConnect(SocialNetwork network)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClass13 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass13();
			_003C_003Ec__DisplayClass.network = network;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClass._003CSocialNetworkConnect_003Eb__12));
		}

		public void SocialNetworkPost(SocialNetwork network, string reason)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			_003C_003Ec__DisplayClass16 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass16();
			_003C_003Ec__DisplayClass.network = network;
			_003C_003Ec__DisplayClass.reason = reason;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass.reason != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass._003CSocialNetworkPost_003Eb__15));
			}
		}

		public void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, Dictionary<string, int> resources)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			_003C_003Ec__DisplayClass19 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass19();
			_003C_003Ec__DisplayClass.purchaseId = purchaseId;
			_003C_003Ec__DisplayClass.purchaseType = purchaseType;
			_003C_003Ec__DisplayClass.purchaseAmount = purchaseAmount;
			_003C_003Ec__DisplayClass.resources = resources;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass.purchaseId != null && _003C_003Ec__DisplayClass.purchaseType != null && _003C_003Ec__DisplayClass.resources != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass._003CInAppPurchase_003Eb__18));
			}
		}

		public void CurrencyAccrual(string currencyName, int amount, AccrualType accrualType)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			_003C_003Ec__DisplayClass1c _003C_003Ec__DisplayClass1c = new _003C_003Ec__DisplayClass1c();
			_003C_003Ec__DisplayClass1c.currencyName = currencyName;
			_003C_003Ec__DisplayClass1c.amount = amount;
			_003C_003Ec__DisplayClass1c.accrualType = accrualType;
			_003C_003Ec__DisplayClass1c._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass1c.currencyName != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass1c._003CCurrencyAccrual_003Eb__1b));
			}
		}

		public void CustomEvent(string eventName, CustomEventParams parameters)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			_003C_003Ec__DisplayClass1f _003C_003Ec__DisplayClass1f = new _003C_003Ec__DisplayClass1f();
			_003C_003Ec__DisplayClass1f.eventName = eventName;
			_003C_003Ec__DisplayClass1f.parameters = parameters;
			_003C_003Ec__DisplayClass1f._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass1f.eventName != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass1f._003CCustomEvent_003Eb__1e));
			}
		}

		public void StartProgressionEvent(string eventId, ProgressionEventParams eventParams)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			_003C_003Ec__DisplayClass22 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass22();
			_003C_003Ec__DisplayClass.eventId = eventId;
			_003C_003Ec__DisplayClass.eventParams = eventParams;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass.eventId != null && _003C_003Ec__DisplayClass.eventParams != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass._003CStartProgressionEvent_003Eb__21));
			}
		}

		public void EndProgressionEvent(string eventId, ProgressionEventParams eventParams)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			_003C_003Ec__DisplayClass25 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass25();
			_003C_003Ec__DisplayClass.eventId = eventId;
			_003C_003Ec__DisplayClass.eventParams = eventParams;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass.eventId != null && _003C_003Ec__DisplayClass.eventParams != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass._003CEndProgressionEvent_003Eb__24));
			}
		}

		public void Age(int age)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClass28 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass28();
			_003C_003Ec__DisplayClass.age = age;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClass._003CAge_003Eb__27));
		}

		public void Gender(Gender gender)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClass2b _003C_003Ec__DisplayClass2b = new _003C_003Ec__DisplayClass2b();
			_003C_003Ec__DisplayClass2b.gender = gender;
			_003C_003Ec__DisplayClass2b._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClass2b._003CGender_003Eb__2a));
		}

		public void Cheater(bool isCheater)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClass2e _003C_003Ec__DisplayClass2e = new _003C_003Ec__DisplayClass2e();
			_003C_003Ec__DisplayClass2e.isCheater = isCheater;
			_003C_003Ec__DisplayClass2e._003C_003E4__this = this;
			Execute(new Action(_003C_003Ec__DisplayClass2e._003CCheater_003Eb__2d));
		}

		public void SendBufferedEvents()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			Execute(new Action(_003CSendBufferedEvents_003Eb__30));
		}

		public void SetCurrentLevel(int currentLevel)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			_003C_003Ec__DisplayClass32 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass32();
			_003C_003Ec__DisplayClass.currentLevel = currentLevel;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			ExecuteFirstLevels(new Action(_003C_003Ec__DisplayClass._003CSetCurrentLevel_003Eb__31));
		}

		public void ReplaceUserId(string fromUserId, string toUserId)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			_003C_003Ec__DisplayClass35 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass35();
			_003C_003Ec__DisplayClass.fromUserId = fromUserId;
			_003C_003Ec__DisplayClass.toUserId = toUserId;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			if (_003C_003Ec__DisplayClass.fromUserId != null && _003C_003Ec__DisplayClass.toUserId != null)
			{
				Execute(new Action(_003C_003Ec__DisplayClass._003CReplaceUserId_003Eb__34));
			}
		}

		private void OnServerConfigReceived(Response response, object callbackData)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if (response.ResponseState == ResponseState.Success && response.ResposeString != null && !response.ResposeString.Equals(string.Empty))
			{
				Log.D("Server config received: " + response.ResposeString);
				NetworkStorage.Load(response.ResposeString);
				networkStorage.Save(networkStorage);
			}
			try
			{
				isInited = true;
				UsersStorage.ActiveUser.StartFastSendSession();
				UsersStorage.OnInitialized(futureEvents);
				futureEvents.Clear();
				Enumerator<Action> enumerator = futureExecutions.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Action current = enumerator.get_Current();
						current.Invoke();
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
				futureExecutions.Clear();
				if (PushManager.PushClient != null)
				{
					List<DevToDev.Data.Metrics.Event> events = PushManager.PushClient.GetEvents();
					UsersStorage.AddEvents(events);
				}
				UsersStorage.ActiveUser.StopFastSendSession();
			}
			catch (global::System.Exception ex)
			{
				Log.E(ex.get_Message() + "\r\n" + ex.get_StackTrace());
			}
		}

		public void SaveAll()
		{
			if (IsInitialized)
			{
				usersStorage.Save(usersStorage);
			}
		}

		public void AddEvent(DevToDev.Data.Metrics.Event eventData)
		{
			if (!IsInitialized)
			{
				futureEvents.Add(eventData);
			}
			else
			{
				UsersStorage.AddEvent(eventData);
			}
		}

		public void Execute(Action action)
		{
			if (!IsInitialized)
			{
				futureExecutions.Add(action);
			}
			else
			{
				action.Invoke();
			}
		}

		public void ExecuteFirstUsers(Action action)
		{
			if (!IsInitialized)
			{
				futureExecutions.Add(action);
			}
			else
			{
				action.Invoke();
			}
		}

		public void ExecuteFirstLevels(Action action)
		{
			if (!IsInitialized)
			{
				futureExecutions.Add(action);
			}
			else
			{
				action.Invoke();
			}
		}

		public void SetActiveLog(bool isActive)
		{
			Log.LogEnabled = isActive;
		}

		[CompilerGenerated]
		private void _003CInitialize_003Eb__0()
		{
			SaveAll();
		}

		[CompilerGenerated]
		private void _003CStartSession_003Eb__2()
		{
			Log.D("Executing start session");
			UsersStorage.SessionOpen();
		}

		[CompilerGenerated]
		private void _003CSendBufferedEvents_003Eb__30()
		{
			UsersStorage.ForceSendEvents();
		}
	}
}
