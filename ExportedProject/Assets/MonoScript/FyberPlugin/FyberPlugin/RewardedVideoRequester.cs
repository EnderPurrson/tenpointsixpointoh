namespace FyberPlugin
{
	public sealed class RewardedVideoRequester : Requester<RewardedVideoRequester, RequestCallback>
	{
		private const string CURRENCY_REQUESTER = "currencyRequester";

		private const string NOTIFY_ON_COMPLETION_KEY = "notifyUserOnCompletion";

		private VirtualCurrencyRequester vcRequester;

		private RewardedVideoRequester()
		{
		}

		public static RewardedVideoRequester Create()
		{
			return new RewardedVideoRequester();
		}

		public RewardedVideoRequester WithVirtualCurrencyRequester(VirtualCurrencyRequester requester)
		{
			vcRequester = requester;
			return this;
		}

		public RewardedVideoRequester NotifyUserOnCompletion(bool shouldNotifyUserOnCompletion)
		{
			requesterAttributes.set_Item("notifyUserOnCompletion", (object)shouldNotifyUserOnCompletion);
			return this;
		}

		public override void Request()
		{
			if (vcRequester != null)
			{
				requesterAttributes.set_Item("currencyRequester", (object)vcRequester.GetRequesterAttributes());
			}
			base.Request();
		}

		protected override RequesterType GetRequester()
		{
			return RequesterType.RewardedVideos;
		}
	}
}
