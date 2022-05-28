namespace FyberPlugin
{
	public interface IPluginBridge
	{
		void StartSDK(string json);

		void Cache(string action);

		void Request(string json);

		void StartAd(string json);

		bool Banner(string json);

		void Report(string json);

		void Settings(string json);

		void EnableLogging(bool shouldLog);
	}
}
