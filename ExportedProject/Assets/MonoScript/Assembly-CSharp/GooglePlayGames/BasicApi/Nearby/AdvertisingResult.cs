using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	public struct AdvertisingResult
	{
		private readonly ResponseStatus mStatus;

		private readonly string mLocalEndpointName;

		public string LocalEndpointName
		{
			get
			{
				return this.mLocalEndpointName;
			}
		}

		public ResponseStatus Status
		{
			get
			{
				return this.mStatus;
			}
		}

		public bool Succeeded
		{
			get
			{
				return this.mStatus == ResponseStatus.Success;
			}
		}

		public AdvertisingResult(ResponseStatus status, string localEndpointName)
		{
			this.mStatus = status;
			this.mLocalEndpointName = Misc.CheckNotNull<string>(localEndpointName);
		}
	}
}