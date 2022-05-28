using System;
using System.Collections.Generic;

namespace DevToDev.Core.Data.Consts
{
	internal static class EventConst
	{
		public static readonly Dictionary<EventType, KeyValuePair<string, string>> EventsInfo;

		public static EventType GetEventTypeByCode(string code)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<EventType, KeyValuePair<string, string>> enumerator = EventsInfo.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<EventType, KeyValuePair<string, string>> current = enumerator.get_Current();
					if (current.get_Value().get_Value().ToUpper()
						.Equals(code.ToUpper()))
					{
						return current.get_Key();
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return EventType.Undefined;
		}

		public static bool IsFastSend(EventType eventType)
		{
			if (eventType != EventType.RealPayment && eventType != EventType.ReceiptValidation && eventType != EventType.GameSession && eventType != EventType.BannerClick && eventType != EventType.BannerShow && eventType != EventType.PushToken && eventType != EventType.PushReceived)
			{
				return eventType == EventType.PushOpen;
			}
			return true;
		}

		public static bool IsReplacing(EventType eventType)
		{
			if (eventType != EventType.ApplicationInfo && eventType != EventType.DeviceInfo && eventType != EventType.UserInfo)
			{
				return eventType == EventType.PushToken;
			}
			return true;
		}

		static EventConst()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<EventType, KeyValuePair<string, string>> val = new Dictionary<EventType, KeyValuePair<string, string>>();
			val.Add(EventType.ReceiptValidation, new KeyValuePair<string, string>("Receipt", "rc"));
			val.Add(EventType.Age, new KeyValuePair<string, string>("Age", "ag"));
			val.Add(EventType.ApplicationInfo, new KeyValuePair<string, string>("Application Info", "ai"));
			val.Add(EventType.Cheater, new KeyValuePair<string, string>("Cheater", "ch"));
			val.Add(EventType.GameSession, new KeyValuePair<string, string>("Game Session", "gs"));
			val.Add(EventType.Gender, new KeyValuePair<string, string>("Gender", "gr"));
			val.Add(EventType.Location, new KeyValuePair<string, string>("Location", "cr"));
			val.Add(EventType.SocialNetworkConnect, new KeyValuePair<string, string>("Connect To Social Network", "sc"));
			val.Add(EventType.SocialNetworkPost, new KeyValuePair<string, string>("Social Network Post", "sp"));
			val.Add(EventType.Tutorial, new KeyValuePair<string, string>("Tutorial", "tr"));
			val.Add(EventType.DeviceInfo, new KeyValuePair<string, string>("Device Info", "di"));
			val.Add(EventType.UserInfo, new KeyValuePair<string, string>("User Info", "ui"));
			val.Add(EventType.InAppPurchase, new KeyValuePair<string, string>("In App Purchase", "ip"));
			val.Add(EventType.RealPayment, new KeyValuePair<string, string>("Real Payment", "rp"));
			val.Add(EventType.CustomEvent, new KeyValuePair<string, string>("Custom event", "ce"));
			val.Add(EventType.ApplicationList, new KeyValuePair<string, string>("ApplicationsList", "al"));
			val.Add(EventType.BannerClick, new KeyValuePair<string, string>("Banner Click", "bc"));
			val.Add(EventType.BannerShow, new KeyValuePair<string, string>("Banner Show", "bs"));
			val.Add(EventType.FacebookInfo, new KeyValuePair<string, string>("Facebook Info", "fb"));
			val.Add(EventType.InstallEvent, new KeyValuePair<string, string>("Install Event", "it"));
			val.Add(EventType.SelfInfo, new KeyValuePair<string, string>("Self Info", "si"));
			val.Add(EventType.PushToken, new KeyValuePair<string, string>("Push Token", "pt"));
			val.Add(EventType.PushReceived, new KeyValuePair<string, string>("Push Received", "pr"));
			val.Add(EventType.PushOpen, new KeyValuePair<string, string>("Push Open", "po"));
			val.Add(EventType.GetServerNode, new KeyValuePair<string, string>("Get Server Node", "sn"));
			val.Add(EventType.Referral, new KeyValuePair<string, string>("Referral", "rf"));
			val.Add(EventType.UserCard, new KeyValuePair<string, string>("User Card", "pl"));
			val.Add(EventType.ProgressionEvent, new KeyValuePair<string, string>("Progression Event", "pe"));
			EventsInfo = val;
		}
	}
}
