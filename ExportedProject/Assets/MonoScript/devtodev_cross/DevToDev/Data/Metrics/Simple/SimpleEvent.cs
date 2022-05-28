using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Simple
{
	internal abstract class SimpleEvent : Event
	{
		public SimpleEvent()
		{
		}

		protected SimpleEvent(EventType type)
			: base(type)
		{
		}

		public SimpleEvent(ObjectInfo info)
			: base(info)
		{
		}

		public override JSONNode GetAdditionalDataJson()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			JSONClass jSONClass = new JSONClass();
			Enumerator<string, object> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> current = enumerator.get_Current();
					if (!current.get_Key().Equals(Event.IN_PROGRESS))
					{
						if (current.get_Value() is JSONNode)
						{
							jSONClass.Add(current.get_Key(), current.get_Value() as JSONNode);
						}
						else
						{
							if (current.get_Value() == null)
							{
								continue;
							}
							if (current.get_Value() is string)
							{
								if (!current.get_Key().Equals("token") && !current.get_Key().Equals("receipt"))
								{
									jSONClass.Add(current.get_Key(), new JSONData(Uri.EscapeDataString(current.get_Value().ToString())));
								}
								else
								{
									jSONClass.Add(current.get_Key(), new JSONData(current.get_Value().ToString()));
								}
							}
							else
							{
								jSONClass.Add(current.get_Key(), new JSONData(current.get_Value()));
							}
						}
					}
					else
					{
						AddPendingToJSON(jSONClass);
					}
				}
				return jSONClass;
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		protected void addParameterIfNotNull(string key, object value)
		{
			if (value != null)
			{
				parameters.Add(key, value);
			}
		}
	}
}
