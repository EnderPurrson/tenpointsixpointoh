using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Network;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Simple;
using DevToDev.Data.Metrics.Specific;
using UnityEngine;

namespace DevToDev.Logic
{
	internal class User : ISaveable
	{
		private bool fastSendSession;

		private List<string> userIds;

		private long registeredTime;

		private DataStorage dataStorage;

		private MetricsStorage metricsStorage;

		private SessionStorage sessionInfo;

		private List<MetricsStorage> failedStorages;

		private PeopleLogic activePeople;

		private bool referralSent;

		public PeopleLogic ActivePeople
		{
			get
			{
				return activePeople;
			}
		}

		public long RegistredTime
		{
			get
			{
				return registeredTime;
			}
			set
			{
				if (registeredTime == 0)
				{
					registeredTime = value;
				}
			}
		}

		public bool ReferralSent
		{
			get
			{
				return referralSent;
			}
			set
			{
				referralSent = value;
			}
		}

		public string UserId
		{
			get
			{
				if (userIds.get_Count() > 0)
				{
					return userIds.get_Item(userIds.get_Count() - 1);
				}
				return null;
			}
		}

		public string PrevId
		{
			get
			{
				if (userIds.get_Count() > 1)
				{
					return userIds.get_Item(userIds.get_Count() - 2);
				}
				return null;
			}
		}

		public DataStorage DataStorage
		{
			get
			{
				return dataStorage;
			}
		}

		public MetricsStorage MetricStorage
		{
			get
			{
				return metricsStorage;
			}
		}

		public MetricsStorage NextFailedStorage
		{
			get
			{
				if (failedStorages.get_Count() > 0)
				{
					Log.D("Found failed storage: " + failedStorages.get_Count());
					MetricsStorage metricsStorage = failedStorages.get_Item(0);
					failedStorages.Remove(metricsStorage);
					return metricsStorage;
				}
				return null;
			}
		}

		public SessionStorage SessionInfo
		{
			get
			{
				return sessionInfo;
			}
		}

		public int Level
		{
			get
			{
				return dataStorage.Level;
			}
			set
			{
				if (value != dataStorage.Level)
				{
					metricsStorage.ClearLevelData();
					dataStorage.Level = value;
					metricsStorage.setLevel(value, null, false);
					Log.R("Current level: " + dataStorage.Level);
				}
			}
		}

		public User()
		{
		}

		public User(string userId)
		{
			userIds = new List<string>();
			userIds.Add(userId);
			dataStorage = new DataStorage();
			metricsStorage = new MetricsStorage();
			sessionInfo = new SessionStorage();
			failedStorages = new List<MetricsStorage>();
			activePeople = new PeopleLogic();
			registeredTime = 0L;
			referralSent = false;
		}

		public User(ObjectInfo info)
		{
			try
			{
				userIds = info.GetValue("userIds", typeof(List<string>)) as List<string>;
				dataStorage = info.GetValue("dataStorage", typeof(DataStorage)) as DataStorage;
				metricsStorage = info.GetValue("metricsStorage", typeof(MetricsStorage)) as MetricsStorage;
				sessionInfo = info.GetValue("sessionInfo", typeof(SessionStorage)) as SessionStorage;
				registeredTime = (long)info.GetValue("registeredTime", typeof(long));
				failedStorages = info.GetValue("failedStorages", typeof(List<MetricsStorage>)) as List<MetricsStorage>;
				activePeople = info.GetValue("activePeopleLogic", typeof(PeopleLogic)) as PeopleLogic;
				referralSent = (bool)info.GetValue("referralSent", typeof(bool));
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
			if (activePeople == null)
			{
				activePeople = new PeopleLogic();
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("userIds", userIds);
				info.AddValue("dataStorage", dataStorage);
				info.AddValue("metricsStorage", metricsStorage);
				info.AddValue("sessionInfo", sessionInfo);
				info.AddValue("registeredTime", registeredTime);
				info.AddValue("failedStorages", failedStorages);
				info.AddValue("activePeopleLogic", activePeople);
				info.AddValue("referralSent", referralSent);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public void AddFailedStorage(MetricsStorage failedStorage)
		{
			failedStorages.Add(failedStorage);
		}

		public void LevelUp(int level, Dictionary<string, int> resources)
		{
			dataStorage.Level = level;
			metricsStorage.setLevel(level, resources, true);
			Log.R("Level up. New level: " + level);
		}

		private void SendFailedStorages()
		{
			MetricsStorage metricsStorage = null;
			while ((metricsStorage = NextFailedStorage) != null)
			{
				SDKClient.Instance.MetricsController.ProceedMetricsStorage(metricsStorage, OnSendCallBack);
			}
		}

		public void OnInitialized(List<DevToDev.Data.Metrics.Event> futureEvents)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<DevToDev.Data.Metrics.Event> enumerator = futureEvents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DevToDev.Data.Metrics.Event current = enumerator.get_Current();
					AddEvent(current);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			AddSelfInfo();
			MergePeopleEventData(activePeople);
			SendFailedStorages();
		}

		public void AddEvents(List<DevToDev.Data.Metrics.Event> events)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<DevToDev.Data.Metrics.Event> enumerator = events.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					DevToDev.Data.Metrics.Event current = enumerator.get_Current();
					AddEvent(current);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public void MergePeopleEventData(PeopleLogic peopleEvent)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			List<PeopleEvent> val = new List<PeopleEvent>();
			Enumerator<MetricsStorage> enumerator = failedStorages.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MetricsStorage current = enumerator.get_Current();
					PeopleEvent removePeopleEvent = current.GetRemovePeopleEvent();
					if (removePeopleEvent != null)
					{
						val.Add(removePeopleEvent);
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			val.Reverse();
			peopleEvent.Merge(val);
		}

		public void AddEvent(DevToDev.Data.Metrics.Event eventData)
		{
			if (!NeedToAddEvent(eventData))
			{
				return;
			}
			if (eventData.MetricType == DevToDev.Core.Data.Consts.EventType.Referral)
			{
				if (referralSent)
				{
					Log.R("Referral event for current user was already sent.");
					return;
				}
				referralSent = true;
			}
			if (EventConst.IsReplacing(eventData.MetricType))
			{
				metricsStorage.RemoveAllMetricsByType(eventData.MetricType);
			}
			metricsStorage.AddMetric(dataStorage.Level, eventData);
			if (UnityPlayerPlatform.isUnityWebPlatform())
			{
				SDKClient.Instance.SaveAll();
			}
			if (EventConst.IsFastSend(eventData.MetricType) && !fastSendSession)
			{
				ForceSendEvents();
			}
			if (metricsStorage.Size >= SDKClient.Instance.NetworkStorage.CountForRequest)
			{
				ForceSendEvents();
			}
		}

		private bool NeedToAddEvent(DevToDev.Data.Metrics.Event eventData)
		{
			if (eventData is TutorialEvent)
			{
				if (!SDKClient.Instance.NetworkStorage.ShouldAddMetric(eventData))
				{
					return false;
				}
				int tutorStep = (eventData as TutorialEvent).TutorStep;
				if (!dataStorage.ContainsTutorialStep(tutorStep))
				{
					dataStorage.AddTutorialStep(tutorStep);
					return true;
				}
				Log.R(string.Concat((object)"Tutorial step ", (object)tutorStep, (object)" has been allready added. Skipping..."));
				return false;
			}
			return SDKClient.Instance.NetworkStorage.ShouldAddMetric(eventData);
		}

		public void OnPeriodicSend(object sender, EventArgs e)
		{
			ForceSendEvents();
		}

		public void ForceSendEvents()
		{
			if (!SDKClient.Instance.IsInitialized)
			{
				return;
			}
			NetworkStorage networkStorage = SDKClient.Instance.NetworkStorage;
			if (this.metricsStorage.Size != 0 || activePeople.NeedToSend(networkStorage.ExcludedUserData))
			{
				MetricsStorage metricsStorage = this.metricsStorage;
				this.metricsStorage = new MetricsStorage(dataStorage.Level, metricsStorage);
				MergePeopleEventData(activePeople);
				if (activePeople.NeedToSend(networkStorage.ExcludedUserData) && networkStorage.ShouldAddMetric(activePeople.PeopleEvent))
				{
					metricsStorage.AddPeopleEvent(dataStorage.Level, activePeople.PeopleEvent);
					activePeople.PeopleEventSent();
				}
				SDKClient.Instance.MetricsController.ProceedMetricsStorage(metricsStorage, OnSendCallBack);
			}
		}

		public void StartFastSendSession()
		{
			fastSendSession = true;
		}

		public void StopFastSendSession()
		{
			fastSendSession = false;
			ForceSendEvents();
		}

		private void OnSendCallBack(Response response, object state)
		{
			Log.D(string.Concat(new object[4] { "Response: ", response, " State: ", state }));
			if ((response == null || response.ResponseState == ResponseState.Failure) && state != null)
			{
				MetricsStorage failedStorage = state as MetricsStorage;
				AddFailedStorage(failedStorage);
				Log.R("Sending Metric Storage failed");
				if (Application.platform == RuntimePlatform.Android || UnityPlayerPlatform.isUnityWebPlatform())
				{
					SDKClient.Instance.SaveAll();
				}
			}
			else
			{
				Log.R("Metric Storage has been sent sucsessfully");
				if (Application.platform == RuntimePlatform.Android || UnityPlayerPlatform.isUnityWebPlatform())
				{
					SDKClient.Instance.SaveAll();
				}
				SendFailedStorages();
			}
		}

		public void ReplaceId(string toUserId)
		{
			StartFastSendSession();
			if (!UserId.Equals(toUserId))
			{
				userIds.Add(toUserId);
			}
			AddSelfInfo();
			StopFastSendSession();
		}

		public void ReplaceIdSilent(string toUserId)
		{
			if (!UserId.Equals(toUserId))
			{
				userIds.Add(toUserId);
			}
		}

		public void AddSelfInfo()
		{
			AddEvent(new ApplicationInfoEvent());
			AddEvent(new UserInfoEvent());
			AddEvent(new DeviceInfoEvent());
			AddReferral();
		}

		private void AddReferral()
		{
			if (ReferralSent)
			{
				return;
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				string referral = InitializationPlatform.GetReferral();
				if (referral == null)
				{
					return;
				}
				string text = Uri.UnescapeDataString(referral);
				Match val = null;
				Dictionary<ReferralProperty, string> val2 = new Dictionary<ReferralProperty, string>();
				val2.Add(ReferralProperty.Custom("referral"), referral);
				if (((Group)(val = Regex.Match(text, "utm_source=[^&]*&?"))).get_Success())
				{
					string text2 = ((Capture)val).get_Value().Split(new char[1] { '=' })[1].Replace("&", "");
					Log.D("utm_source: " + text2);
					val2.Add(ReferralProperty.Source, text2);
				}
				if (((Group)(val = Regex.Match(text, "utm_medium=[^&]*&?"))).get_Success())
				{
					string text3 = ((Capture)val).get_Value().Split(new char[1] { '=' })[1].Replace("&", "");
					Log.D("utm_medium: " + text3);
					val2.Add(ReferralProperty.Medium, text3);
				}
				if (((Group)(val = Regex.Match(text, "utm_term=[^&]*&?"))).get_Success())
				{
					string text4 = ((Capture)val).get_Value().Split(new char[1] { '=' })[1].Replace("&", "");
					Log.D("utm_term: " + text4);
					val2.Add(ReferralProperty.Term, text4);
				}
				if (((Group)(val = Regex.Match(text, "utm_content=[^&]*&?"))).get_Success())
				{
					string text5 = ((Capture)val).get_Value().Split(new char[1] { '=' })[1].Replace("&", "");
					Log.D("utm_content: " + text5);
					val2.Add(ReferralProperty.Content, text5);
				}
				if (((Group)(val = Regex.Match(text, "utm_campaign=[^&]*&?"))).get_Success())
				{
					string text6 = ((Capture)val).get_Value().Split(new char[1] { '=' })[1].Replace("&", "");
					Log.D("utm_campaign: " + text6);
					val2.Add(ReferralProperty.Campaign, text6);
				}
				if (val2.get_Count() > 0)
				{
					AddEvent(new ReferralEvent((IDictionary<ReferralProperty, string>)(object)val2));
				}
			}
			if (!UnityPlayerPlatform.isUnityWebPlatform())
			{
				return;
			}
			string absoluteURL = Application.absoluteURL;
			Log.D("Referral URL: " + absoluteURL);
			if (string.IsNullOrEmpty(absoluteURL))
			{
				return;
			}
			string[] array = absoluteURL.Split(new char[1] { '?' });
			if (array != null && array.Length > 1)
			{
				absoluteURL = array[1];
				Dictionary<ReferralProperty, string> val3 = new Dictionary<ReferralProperty, string>();
				DeepParseReferrer(absoluteURL, val3);
				if (val3.get_Count() > 0)
				{
					AddEvent(new ReferralEvent((IDictionary<ReferralProperty, string>)(object)val3));
				}
			}
		}

		private void DeepParseReferrer(string referrer, Dictionary<ReferralProperty, string> referralProps)
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			string[] array = referrer.Split(new char[1] { '&' });
			List<string> val = new List<string>();
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.Contains("\""))
				{
					val.AddRange((global::System.Collections.Generic.IEnumerable<string>)text.Split(new char[1] { '"' }));
				}
				else if (text.Contains("'"))
				{
					val.AddRange((global::System.Collections.Generic.IEnumerable<string>)text.Split(new char[1] { '\'' }));
				}
				else
				{
					val.Add(text);
				}
			}
			Enumerator<string> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					string[] array3 = current.Split(new char[1] { '=' });
					if (array3 == null || array3.Length <= 1)
					{
						continue;
					}
					if (array3[0].ToLower().Contains("utm"))
					{
						referralProps.Add(ReferralProperty.Custom(array3[0]), array3[1]);
					}
					else if (!string.IsNullOrEmpty(array3[1]))
					{
						string text2 = Uri.UnescapeDataString(array3[1]);
						if (!string.IsNullOrEmpty(text2))
						{
							DeepParseReferrer(text2, referralProps);
						}
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		public void UpSpend(int amount, string currencyName)
		{
			metricsStorage.upSpend(dataStorage.Level, amount, currencyName);
		}

		public void UpEarned(int amount, string currencyName)
		{
			metricsStorage.upEarned(dataStorage.Level, amount, currencyName);
		}

		public void UpBought(int amount, string currencyName)
		{
			metricsStorage.upBought(dataStorage.Level, amount, currencyName);
		}

		public void SessionOpen()
		{
			if (!sessionInfo.IsActive)
			{
				sessionInfo.IsActive = true;
				if (sessionInfo == null)
				{
					sessionInfo = new SessionStorage();
					sessionInfo.StartLevel = dataStorage.Level;
					sessionInfo.IsActive = true;
				}
				else if (sessionInfo.EndTime + SDKClient.Instance.NetworkStorage.SessionDelay < DeviceHelper.Instance.GetUnixTime() / 1000)
				{
					GameSession();
					sessionInfo = new SessionStorage();
					sessionInfo.StartLevel = dataStorage.Level;
					sessionInfo.IsActive = true;
				}
				else
				{
					sessionInfo.TimeDifference += Math.Min(Math.Abs(sessionInfo.EndTime - DeviceHelper.Instance.GetUnixTime() / 1000), SDKClient.Instance.NetworkStorage.SessionDelay);
				}
			}
		}

		public bool IsSessionActive()
		{
			return sessionInfo.IsActive;
		}

		public void SessionClose(long timestamp, bool inActive = false)
		{
			if (inActive || sessionInfo.IsActive)
			{
				sessionInfo.IsActive = false;
				sessionInfo.EndLevel = dataStorage.Level;
				sessionInfo.EndTime = ((timestamp != 0) ? timestamp : (DeviceHelper.Instance.GetUnixTime() / 1000));
			}
		}

		private void GameSession()
		{
			long num = sessionInfo.EndTime - sessionInfo.TimeDifference;
			if (num - sessionInfo.StartTime > 0)
			{
				AddEvent(new GameSessionEvent(sessionInfo.StartTime, num));
			}
		}

		public bool CheckDeviceChanges(Device device)
		{
			if (registeredTime == 0 && !UserId.Equals(device.DeviceId))
			{
				userIds.Add(device.DeviceId);
				return true;
			}
			return false;
		}

		public void LoadNativeData(JSONNode data)
		{
			if (data["level"] != null)
			{
				int num = (Level = data["level"].AsInt);
			}
			if (data["registredTime"] != null)
			{
				long num2 = (registeredTime = data["registredTime"].AsLong);
			}
			if (!(data["tutorialSteps"] != null))
			{
				return;
			}
			JSONArray asArray = data["tutorialSteps"].AsArray;
			global::System.Collections.IEnumerator enumerator = asArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JSONNode jSONNode = (JSONNode)enumerator.get_Current();
					dataStorage.AddTutorialStep(jSONNode.AsInt);
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
	}
}
