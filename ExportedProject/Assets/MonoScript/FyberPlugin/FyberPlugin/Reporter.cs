using System;
using System.Collections.Generic;
using FyberPlugin.LitJson;

namespace FyberPlugin
{
	public abstract class Reporter<T> where T : Reporter<T>
	{
		protected const string APP_ID = "appId";

		protected Dictionary<string, object> nativeDict = new Dictionary<string, object>(3);

		protected abstract T GetThis();

		public T AddParameters(Dictionary<string, string> parameters)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, string> reportParameters = GetReportParameters();
			Enumerator<string, string> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					reportParameters.set_Item(current.get_Key(), current.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return GetThis();
		}

		public T AddParameter(string key, string value)
		{
			Dictionary<string, string> reportParameters = GetReportParameters();
			reportParameters.set_Item(key, value);
			return GetThis();
		}

		public void Report()
		{
			PluginBridge.Report(JsonMapper.ToJson(nativeDict));
		}

		private Dictionary<string, string> GetReportParameters()
		{
			if (nativeDict.get_Item("parameters") == null)
			{
				nativeDict.set_Item("parameters", (object)new Dictionary<string, string>());
			}
			return nativeDict.get_Item("parameters") as Dictionary<string, string>;
		}

		protected void ValidateAppId(string appId)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(appId))
			{
				throw new ArgumentException("App ID cannot be null nor empty.");
			}
		}
	}
}
