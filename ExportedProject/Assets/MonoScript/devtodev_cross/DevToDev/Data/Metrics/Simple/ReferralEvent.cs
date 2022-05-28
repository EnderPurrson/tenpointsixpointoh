using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class ReferralEvent : SimpleEvent
	{
		public ReferralEvent()
		{
		}

		public ReferralEvent(ObjectInfo info)
			: base(info)
		{
		}

		public ReferralEvent(IDictionary<ReferralProperty, string> referralData)
			: base(EventType.Referral)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			global::System.Collections.Generic.IEnumerator<KeyValuePair<ReferralProperty, string>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<ReferralProperty, string>>)referralData).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					KeyValuePair<ReferralProperty, string> current = enumerator.get_Current();
					addParameterIfNotNull(Uri.EscapeDataString(((object)current.get_Key()).ToString()), current.get_Value());
				}
			}
			finally
			{
				if (enumerator != null)
				{
					((global::System.IDisposable)enumerator).Dispose();
				}
			}
		}
	}
}
