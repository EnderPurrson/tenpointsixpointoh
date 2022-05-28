namespace FyberPlugin
{
	public sealed class InstallReporter : Reporter<InstallReporter>
	{
		private InstallReporter(string appId)
		{
			ValidateAppId(appId);
			nativeDict.set_Item("appId", (object)appId);
		}

		public static InstallReporter Create(string appId)
		{
			return new InstallReporter(appId);
		}

		protected override InstallReporter GetThis()
		{
			return this;
		}
	}
}
