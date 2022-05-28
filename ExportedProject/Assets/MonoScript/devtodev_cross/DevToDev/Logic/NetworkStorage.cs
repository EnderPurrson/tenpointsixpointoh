using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Aggregated;
using DevToDev.Data.Metrics.Aggregated.CustomEvent;
using DevToDev.Data.Metrics.Simple;

namespace DevToDev.Logic
{
	internal class NetworkStorage : IStorageable<NetworkStorage>
	{
		private static readonly string WORKER = "worker";

		private static readonly string COUNT_FOR_REQUEST = "countForRequest";

		private static readonly string EVENT_PARAMS_COUNT = "eventParamsCount";

		private static readonly string TIME_FOR_REQUEST = "timeForRequest";

		private static readonly string SERVER_TIME = "serverTime";

		private static readonly string CACHE_TIMEOUT = "cacheTimeout";

		private static readonly string CONFIG_VERSION = "configVersion";

		private static readonly string USE_CUSTOM_UDID = "useCustomUDID";

		private static readonly string SESSION_DELAY = "sessionDelay";

		private static readonly string EXCLUDE = "exclude";

		private static readonly string EXCLUDE_ALL_PARAM = "all";

		private static readonly string RESULT = "result";

		private string worker;

		private string configVersion;

		private long serverTime;

		private long sessionDelay = 20L;

		private bool excludeAll;

		private bool isUseCUID;

		private int sendCount = 10;

		private int customEventParamsCount = 10;

		private int timeForRequest = 120;

		private int cacheTimeout;

		private Dictionary<EventType, List<Event>> excludeMetrics;

		private List<string> excludedUserData;

		public string ActiveNodeUrl
		{
			get
			{
				return worker;
			}
		}

		public int TimeForRequest
		{
			get
			{
				return timeForRequest;
			}
		}

		public int CountForRequest
		{
			get
			{
				return sendCount;
			}
		}

		public int EventParamsCount
		{
			get
			{
				return customEventParamsCount;
			}
		}

		public long ServerTime
		{
			get
			{
				return serverTime;
			}
		}

		public int CacheTimeout
		{
			get
			{
				return cacheTimeout;
			}
		}

		public string ConfigVersion
		{
			get
			{
				return configVersion;
			}
		}

		public bool UseCustomUdid
		{
			get
			{
				return isUseCUID;
			}
		}

		public Dictionary<EventType, List<Event>> ExcludeMetrics
		{
			get
			{
				return excludeMetrics;
			}
		}

		public bool ExcludeAll
		{
			get
			{
				return excludeAll;
			}
		}

		public long SessionDelay
		{
			get
			{
				return sessionDelay;
			}
		}

		public List<string> ExcludedUserData
		{
			get
			{
				return excludedUserData;
			}
		}

		public override string StorageName()
		{
			return "networkStorage.dat";
		}

		public override ISaveable GetBlankObject()
		{
			return new NetworkStorage();
		}

		public override ISaveable GetObject(byte[] data)
		{
			return new Formatter<NetworkStorage>().Load(data);
		}

		public override byte[] SaveObject(ISaveable obj)
		{
			return new Formatter<NetworkStorage>().Save(obj as NetworkStorage);
		}

		public NetworkStorage()
		{
		}

		public NetworkStorage(ObjectInfo info)
		{
			try
			{
				worker = info.GetValue("worker", typeof(string)) as string;
				configVersion = info.GetValue("configVersion", typeof(string)) as string;
				sessionDelay = (long)info.GetValue("sessionDelay", typeof(long));
				excludeAll = (bool)info.GetValue("excludeAll", typeof(bool));
				isUseCUID = (bool)info.GetValue("isUseCUID", typeof(bool));
				sendCount = (int)info.GetValue("sendCount", typeof(int));
				customEventParamsCount = (int)info.GetValue("customEventParamsCount", typeof(int));
				timeForRequest = (int)info.GetValue("timeForRequest", typeof(int));
				cacheTimeout = (int)info.GetValue("cacheTimeout", typeof(int));
				excludeMetrics = info.GetValue("excludeMetrics", typeof(Dictionary<EventType, List<Event>>)) as Dictionary<EventType, List<Event>>;
				serverTime = (long)info.GetValue("serverTime", typeof(long));
				excludedUserData = info.GetValue("excludedUserData", typeof(List<string>)) as List<string>;
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("worker", worker);
				info.AddValue("configVersion", configVersion);
				info.AddValue("serverTime", serverTime);
				info.AddValue("sessionDelay", sessionDelay);
				info.AddValue("excludeAll", excludeAll);
				info.AddValue("isUseCUID", isUseCUID);
				info.AddValue("sendCount", sendCount);
				info.AddValue("customEventParamsCount", customEventParamsCount);
				info.AddValue("timeForRequest", timeForRequest);
				info.AddValue("cacheTimeout", cacheTimeout);
				info.AddValue("excludeMetrics", excludeMetrics);
				info.AddValue("excludedUserData", excludedUserData);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public void Load(string jsonParams)
		{
			JSONNode jSONNode = null;
			try
			{
				jSONNode = JSON.Parse(jsonParams);
			}
			catch (global::System.Exception ex)
			{
				Log.D(ex.get_StackTrace());
				return;
			}
			if (jSONNode[WORKER] != null)
			{
				worker = jSONNode[WORKER];
			}
			if (jSONNode[COUNT_FOR_REQUEST] != null)
			{
				sendCount = jSONNode[COUNT_FOR_REQUEST].AsInt;
			}
			if (jSONNode[EVENT_PARAMS_COUNT] != null)
			{
				customEventParamsCount = jSONNode[EVENT_PARAMS_COUNT].AsInt;
			}
			if (jSONNode[TIME_FOR_REQUEST] != null)
			{
				timeForRequest = jSONNode[TIME_FOR_REQUEST].AsInt;
			}
			if (jSONNode[CONFIG_VERSION] != null)
			{
				configVersion = jSONNode[CONFIG_VERSION];
			}
			if (jSONNode[SERVER_TIME] != null)
			{
				serverTime = jSONNode[SERVER_TIME].AsLong;
			}
			if (jSONNode[CACHE_TIMEOUT] != null)
			{
				serverTime = jSONNode[CACHE_TIMEOUT].AsInt;
			}
			if (jSONNode[SESSION_DELAY] != null)
			{
				sessionDelay = jSONNode[SESSION_DELAY].AsInt;
			}
			if (jSONNode[USE_CUSTOM_UDID] != null)
			{
				isUseCUID = jSONNode[USE_CUSTOM_UDID].AsBool;
			}
			else
			{
				isUseCUID = false;
			}
			if (jSONNode[RESULT] != null)
			{
				switch (jSONNode[RESULT].AsInt)
				{
				case 1:
					LoadExcludeData(jSONNode[EXCLUDE]);
					break;
				case 2:
					ClearExcludeData();
					configVersion = null;
					break;
				}
			}
			else if (jSONNode[EXCLUDE] != null)
			{
				LoadExcludeData(jSONNode[EXCLUDE]);
			}
			Log.D("Got worker: " + worker);
		}

		public bool ShouldAddMetric(Event metric)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			if (ExcludeAll)
			{
				return false;
			}
			if (ExcludeMetrics == null)
			{
				return true;
			}
			if (!ExcludeMetrics.ContainsKey(metric.MetricType))
			{
				return true;
			}
			if (ExcludeMetrics.ContainsKey(metric.MetricType) && ExcludeMetrics.get_Item(metric.MetricType) == null)
			{
				return false;
			}
			Enumerator<Event> enumerator = ExcludeMetrics.get_Item(metric.MetricType).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Event current = enumerator.get_Current();
					if (metric.IsEqualToMetric(current))
					{
						return false;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return true;
		}

		private void LoadExcludeData(JSONNode data)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (data is JSONClass)
			{
				excludeAll = false;
				excludeMetrics = new Dictionary<EventType, List<Event>>();
				{
					global::System.Collections.IEnumerator enumerator = ((JSONClass)data).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, JSONNode> val = (KeyValuePair<string, JSONNode>)enumerator.get_Current();
							EventType eventTypeByCode = EventConst.GetEventTypeByCode(val.get_Key());
							if (eventTypeByCode == EventType.UserCard)
							{
								JSONArray asArray = val.get_Value().AsArray;
								if (excludedUserData == null)
								{
									excludedUserData = new List<string>();
								}
								if (asArray == null || asArray.Count == 0)
								{
									if (excludeMetrics == null)
									{
										excludeMetrics = new Dictionary<EventType, List<Event>>();
									}
									excludeMetrics.Add(eventTypeByCode, (List<Event>)default(List<Event>));
									continue;
								}
								{
									global::System.Collections.IEnumerator enumerator2 = asArray.GetEnumerator();
									try
									{
										while (enumerator2.MoveNext())
										{
											JSONNode jSONNode = (JSONNode)enumerator2.get_Current();
											excludedUserData.Add(((object)jSONNode).ToString());
										}
									}
									finally
									{
										global::System.IDisposable disposable2 = enumerator2 as global::System.IDisposable;
										if (disposable2 != null)
										{
											disposable2.Dispose();
										}
									}
								}
							}
							else
							{
								JSONArray asArray2 = val.get_Value().AsArray;
								if (ExcludeMetrics == null)
								{
									excludeMetrics = new Dictionary<EventType, List<Event>>();
								}
								if (asArray2 == null || asArray2.Count == 0)
								{
									excludeMetrics.Add(eventTypeByCode, (List<Event>)default(List<Event>));
									continue;
								}
								List<Event> val2 = new List<Event>();
								AddMetricsToList(eventTypeByCode, val2, asArray2);
								excludeMetrics.Add(eventTypeByCode, val2);
							}
						}
						return;
					}
					finally
					{
						global::System.IDisposable disposable = enumerator as global::System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			if (((object)data).ToString().Equals(EXCLUDE_ALL_PARAM))
			{
				excludeAll = true;
			}
		}

		private void AddMetricsToList(EventType type, List<Event> metricList, JSONArray metricsParams)
		{
			global::System.Collections.IEnumerator enumerator = metricsParams.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode jSONNode = (JSONNode)enumerator.get_Current();
					switch (type)
					{
					case EventType.CustomEvent:
					{
						CustomEventData customEventData = CustomEventData.SingleData(jSONNode, null);
						metricList.Add((Event)new CustomEvent(customEventData));
						break;
					}
					case EventType.Tutorial:
						metricList.Add((Event)new TutorialEvent(jSONNode.AsInt));
						break;
					case EventType.InAppPurchase:
						metricList.Add((Event)new InAppPurchase(jSONNode, "", -1, -1f, ""));
						break;
					case EventType.SocialNetworkConnect:
						metricList.Add((Event)new SocialNetworkConnectEvent(SocialNetwork.Custom(jSONNode)));
						break;
					case EventType.SocialNetworkPost:
						metricList.Add((Event)new SocialNetworkConnectEvent(SocialNetwork.Custom(jSONNode)));
						break;
					}
				}
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private void ClearExcludeData()
		{
			excludeAll = false;
			excludeMetrics = null;
		}
	}
}
