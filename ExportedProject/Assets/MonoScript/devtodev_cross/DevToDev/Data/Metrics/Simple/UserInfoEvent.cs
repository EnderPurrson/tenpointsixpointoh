using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Logic;

namespace DevToDev.Data.Metrics.Simple
{
	internal class UserInfoEvent : SimpleEvent
	{
		private static readonly string LOCALE = "locale";

		private static readonly string IP = "ip";

		private static readonly string CROSS_UID = "crossUid";

		private static readonly string REGISTERED = "registered";

		private static readonly string CARIER = "carrier";

		private static readonly string USER_AGENT = "userAgent";

		private static readonly string IS_ROOTED = "isRooted";

		public UserInfoEvent(ObjectInfo info)
			: base(info)
		{
		}

		public UserInfoEvent()
			: base(EventType.UserInfo)
		{
			DeviceHelper instance = DeviceHelper.Instance;
			parameters.Remove(Event.TIMESTAMP);
			parameters.Add(LOCALE, (object)instance.GetLocale());
			string mobileOperator = instance.GetMobileOperator();
			if (mobileOperator != null && !mobileOperator.Equals("0"))
			{
				parameters.Add(CARIER, (object)instance.GetMobileOperator());
			}
			parameters.Add(USER_AGENT, (object)instance.GetUserAgentSring());
			parameters.Add(IS_ROOTED, (object)instance.IsRooted());
			SDKClient instance2 = SDKClient.Instance;
			string userId = instance2.UsersStorage.ActiveUser.UserId;
			parameters.Add(CROSS_UID, (object)userId);
			long registredTime = instance2.UsersStorage.ActiveUser.RegistredTime;
			if (registredTime != 0)
			{
				parameters.Add(REGISTERED, (object)registredTime);
			}
		}
	}
}
