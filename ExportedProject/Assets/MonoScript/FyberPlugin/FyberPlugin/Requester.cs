using System;
using System.Collections.Generic;
using FyberPlugin.LitJson;

namespace FyberPlugin
{
	public abstract class Requester<T, V> where T : Requester<T, V> where V : class, Callback
	{
		protected enum RequesterType
		{
			OfferWall = 0,
			RewardedVideos = 1,
			Interstitials = 2,
			Banners = 3,
			VirtualCurrency = 4
		}

		protected const string CUSTOM_PARAMS_KEY = "parameters";

		private const string REQUEST_ID = "requestId";

		private const string REQUESTER = "requester";

		protected const string PLACEMENT_ID_KEY = "placementId";

		protected Dictionary<string, object> requesterAttributes;

		protected V requestCallback;

		public Requester()
		{
			requesterAttributes = new Dictionary<string, object>();
		}

		public virtual void Request()
		{
			GetRequesterAttributes();
			string json = JsonMapper.ToJson(requesterAttributes);
			PluginBridge.Request(json);
		}

		protected abstract RequesterType GetRequester();

		internal Dictionary<string, object> GetRequesterAttributes()
		{
			requesterAttributes.set_Item("requestId", (object)GenerateUidAndAddToQueue());
			requesterAttributes.set_Item("requester", (object)GetRequester());
			return requesterAttributes;
		}

		internal string GenerateUidAndAddToQueue()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Guid val = Guid.NewGuid();
			string text = ((Guid)(ref val)).ToString();
			if (requestCallback == null)
			{
				requestCallback = FyberCallback.Instance as V;
			}
			FyberCallbacksManager.AddCallback(text, requestCallback);
			return text;
		}

		public T WithPlacementId(string placementId)
		{
			requesterAttributes.set_Item("placementId", (object)placementId);
			return this as T;
		}

		public T WithCallback(V requestCallback)
		{
			this.requestCallback = requestCallback;
			return this as T;
		}

		public T AddParameters(Dictionary<string, string> parameters)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, string> customParameters = GetCustomParameters();
			Enumerator<string, string> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					customParameters.set_Item(current.get_Key(), current.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			return this as T;
		}

		public T AddParameter(string key, string value)
		{
			Dictionary<string, string> customParameters = GetCustomParameters();
			customParameters.set_Item(key, value);
			return this as T;
		}

		public T ClearParameters()
		{
			GetCustomParameters().Clear();
			return this as T;
		}

		public T RemoveParameter(string key)
		{
			GetCustomParameters().Remove(key);
			return this as T;
		}

		protected Dictionary<string, string> GetCustomParameters()
		{
			if (!requesterAttributes.ContainsKey("parameters"))
			{
				requesterAttributes.set_Item("parameters", (object)new Dictionary<string, string>());
			}
			return requesterAttributes.get_Item("parameters") as Dictionary<string, string>;
		}
	}
}
