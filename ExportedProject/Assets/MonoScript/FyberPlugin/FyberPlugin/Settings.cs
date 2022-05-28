using System.Collections.Generic;
using FyberPlugin.LitJson;

namespace FyberPlugin
{
	public class Settings
	{
		private Dictionary<string, object> auxDict;

		internal Settings()
		{
			auxDict = new Dictionary<string, object>(2);
		}

		public Settings NotifyUserOnCompletion(bool shouldNotifyUserOnCompletion)
		{
			RunInBridge("notifyUserOnCompletion", shouldNotifyUserOnCompletion);
			return this;
		}

		public Settings NotifyUserOnReward(bool shouldNotifyUserOnReward)
		{
			RunInBridge("notifyUserOnReward", shouldNotifyUserOnReward);
			return this;
		}

		public Settings CloseOfferWallOnRedirect(bool shouldCloseOfferWallOnRedirect)
		{
			RunInBridge("closeOfferWallOnRedirect", shouldCloseOfferWallOnRedirect);
			return this;
		}

		public Settings AddParameters(Dictionary<string, string> parameters)
		{
			RunInBridge("addParameters", JsonMapper.ToJson(parameters));
			return this;
		}

		public Settings AddParameter(string key, string value)
		{
			RunInBridge("addParameter", string.Format("{\"{0}\":\"{1}\"}", (object)key, (object)value));
			return this;
		}

		public Settings ClearParameters()
		{
			RunInBridge("clearParameters", string.Empty);
			return this;
		}

		public Settings RemoveParameter(string key)
		{
			RunInBridge("removeParameter", key);
			return this;
		}

		private void RunInBridge(string action, object value)
		{
			auxDict.set_Item("action", (object)action);
			auxDict.set_Item("value", value);
			PluginBridge.Settings(JsonMapper.ToJson(auxDict));
		}
	}
}
