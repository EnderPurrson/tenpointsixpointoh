using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Logic;
using DevToDev.Push.Data.Metrics.Simple;

namespace DevToDev.Push
{
	internal class PushClient
	{
		private static readonly string PUSH_NATIVE_DATA = "push_data";

		public OnPushTokenReceivedHandler OnPushTokenReceived;

		public OnPushTokenFailedHandler OnPushTokenFailed;

		public OnPushReceivedHandler OnPushReceived;

		private PushClientPlatform pushClientPlatform;

		public void onRegisteredForPushNotifications(string token)
		{
			if (token != null)
			{
				SDKClient.Instance.AddEvent(new TokenSendEvent(token));
			}
			OnPushTokenReceived(token);
		}

		public void onFailedToRegisteredForPushNotifications(string errorString)
		{
			OnPushTokenFailed(errorString);
		}

		public void onPushNotificationsReceived(Event metric, string messageString)
		{
			if (metric != null)
			{
				SDKClient.Instance.AddEvent(metric);
			}
			if (messageString != null)
			{
				OnPushReceived(PushType.ToastNotification, (IDictionary<string, string>)(object)DecodePushData(messageString));
			}
		}

		public PushClient()
		{
			pushClientPlatform = new PushClientPlatform();
		}

		public void Initialize()
		{
			pushClientPlatform.Initialize(_003CInitialize_003Eb__0, _003CInitialize_003Eb__1, _003CInitialize_003Eb__2);
		}

		private string GetNativeEvents()
		{
			return pushClientPlatform.GetNativeEvents();
		}

		private void ClearNativeEvents()
		{
			pushClientPlatform.ClearNativeEvents();
		}

		public List<Event> GetEvents()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			List<Event> result = new List<Event>();
			string nativeEvents = GetNativeEvents();
			if (nativeEvents == null)
			{
				return result;
			}
			Log.D("Native push data: " + nativeEvents);
			JSONNode jSONNode = null;
			try
			{
				jSONNode = JSON.Parse(nativeEvents);
			}
			catch (global::System.Exception ex)
			{
				Log.E(ex.get_StackTrace());
				return result;
			}
			if (!(jSONNode is JSONClass))
			{
				return result;
			}
			if (jSONNode[EventConst.EventsInfo.get_Item(EventType.PushOpen).get_Value()] != null)
			{
				JSONArray asArray = jSONNode[EventConst.EventsInfo.get_Item(EventType.PushOpen).get_Value()].AsArray;
				{
					global::System.Collections.IEnumerator enumerator = asArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							JSONNode jSONNode2 = (JSONNode)enumerator.get_Current();
							try
							{
								PushOpenEvent metric = PushOpenEvent.CreateFromJSON(jSONNode2);
								if (jSONNode2[PUSH_NATIVE_DATA] != null)
								{
									onPushNotificationsReceived(metric, jSONNode2[PUSH_NATIVE_DATA].Value);
								}
								else
								{
									onPushNotificationsReceived(metric, null);
								}
							}
							catch
							{
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
			}
			if (jSONNode[EventConst.EventsInfo.get_Item(EventType.PushReceived).get_Value()] != null)
			{
				JSONArray asArray2 = jSONNode[EventConst.EventsInfo.get_Item(EventType.PushReceived).get_Value()].AsArray;
				{
					global::System.Collections.IEnumerator enumerator2 = asArray2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							JSONNode jSONNode3 = (JSONNode)enumerator2.get_Current();
							try
							{
								Log.D(jSONNode3.Value);
								PushReceivedEvent metric2 = PushReceivedEvent.CreateFromJSON(jSONNode3);
								if (jSONNode3[PUSH_NATIVE_DATA] != null)
								{
									onPushNotificationsReceived(metric2, jSONNode3[PUSH_NATIVE_DATA].Value);
								}
								else
								{
									onPushNotificationsReceived(metric2, null);
								}
							}
							catch
							{
							}
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
			ClearNativeEvents();
			return result;
		}

		private Dictionary<string, string> DecodePushData(string pushMessage)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, string> val = new Dictionary<string, string>();
			if (pushMessage == null)
			{
				return val;
			}
			JSONNode jSONNode = null;
			try
			{
				jSONNode = JSON.Parse(pushMessage);
			}
			catch (global::System.Exception ex)
			{
				Log.E(ex.get_StackTrace());
				return val;
			}
			global::System.Collections.IEnumerator enumerator = (jSONNode as JSONClass).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, JSONNode> val2 = (KeyValuePair<string, JSONNode>)enumerator.get_Current();
					val.Add(val2.get_Key(), val2.get_Value().Value);
				}
				return val;
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

		public void SetCustomSmallIcon(string pathToResource)
		{
			pushClientPlatform.SetCustomSmallIcon(pathToResource);
		}

		public void SetCustomLargeIcon(string pathToResource)
		{
			pushClientPlatform.SetCustomLargeIcon(pathToResource);
		}

		[CompilerGenerated]
		private void _003CInitialize_003Eb__0(string token)
		{
			onRegisteredForPushNotifications(token);
		}

		[CompilerGenerated]
		private void _003CInitialize_003Eb__1(string error)
		{
			onFailedToRegisteredForPushNotifications(error);
		}

		[CompilerGenerated]
		private void _003CInitialize_003Eb__2(string data)
		{
			GetEvents();
		}
	}
}
