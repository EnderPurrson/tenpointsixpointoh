namespace FyberPlugin
{
	public sealed class VirtualCurrencyRequester : Requester<VirtualCurrencyRequester, VirtualCurrencyCallback>
	{
		private const string NOTIFY_ON_REWARD_KEY = "notifyUserOnReward";

		private const string TRANSACTION_ID_KEY = "transactionId";

		private const string CURRENCY_ID_KEY = "currencyId";

		private VirtualCurrencyRequester()
		{
		}

		public static VirtualCurrencyRequester Create()
		{
			return new VirtualCurrencyRequester();
		}

		public VirtualCurrencyRequester WithTransactionId(string transactionId)
		{
			requesterAttributes.set_Item("transactionId", (object)transactionId);
			return this;
		}

		public VirtualCurrencyRequester ForCurrencyId(string currencyId)
		{
			requesterAttributes.set_Item("currencyId", (object)currencyId);
			return this;
		}

		public VirtualCurrencyRequester NotifyUserOnReward(bool shouldNotifyUserOnReward)
		{
			requesterAttributes.set_Item("notifyUserOnReward", (object)shouldNotifyUserOnReward);
			return this;
		}

		protected override RequesterType GetRequester()
		{
			return RequesterType.VirtualCurrency;
		}
	}
}
