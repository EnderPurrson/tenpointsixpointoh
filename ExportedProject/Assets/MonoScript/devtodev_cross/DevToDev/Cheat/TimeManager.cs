using System;
using System.Runtime.CompilerServices;
using DevToDev.Core.Network;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Builders;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Consts;
using DevToDev.Logic;

namespace DevToDev.Cheat
{
	internal class TimeManager
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2
		{
			public TimeManager _003C_003E4__this;

			public OnTimeVerifyCallback callback;

			public void _003CCheckTime_003Eb__0()
			{
				callback(_003C_003E4__this.GetTimeStatus());
			}
		}

		private static readonly string TIME = "time";

		private static TimeManager instance;

		private long savedTimestamp;

		private long serverTimestamp;

		private long currentTimestamp;

		private long deltaTime;

		public long SavedTimestamp
		{
			get
			{
				return savedTimestamp;
			}
		}

		public long ServerTimestamp
		{
			get
			{
				return serverTimestamp;
			}
			set
			{
				serverTimestamp = value;
			}
		}

		public long CurrentTimestamp
		{
			get
			{
				return currentTimestamp;
			}
		}

		public long DeltaTime
		{
			get
			{
				return deltaTime;
			}
			set
			{
				deltaTime = value;
			}
		}

		public static TimeManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TimeManager();
				}
				return instance;
			}
		}

		private TimeManager()
		{
			currentTimestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
		}

		public void CheckTime(OnTimeVerifyCallback callback)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			Action val = null;
			_003C_003Ec__DisplayClass2 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass2();
			_003C_003Ec__DisplayClass.callback = callback;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			savedTimestamp = LoadSavedTimestamp();
			SaveTimestamp();
			if (_003C_003Ec__DisplayClass.callback == null)
			{
				return;
			}
			if (serverTimestamp == 0)
			{
				LoadServerTimestamp(_003C_003Ec__DisplayClass.callback);
				return;
			}
			AsyncOperationDispatcher asyncOperationDispatcher = SDKClient.Instance.AsyncOperationDispatcher;
			if (val == null)
			{
				val = new Action(_003C_003Ec__DisplayClass._003CCheckTime_003Eb__0);
			}
			asyncOperationDispatcher.DispatchOnMainThread(val);
		}

		private void OnTimeGot(Response response, object state)
		{
			if (response.ResponseState != 0)
			{
				return;
			}
			try
			{
				JSONClass jSONClass = JSON.Parse(response.ResposeString) as JSONClass;
				if (jSONClass == null)
				{
					Log.E("Server error occured in time request.");
					return;
				}
				long num = (serverTimestamp = jSONClass[TIME].AsLong);
			}
			catch (global::System.Exception)
			{
				Log.E("Server error occured in time request.");
			}
			CheckTime(state as OnTimeVerifyCallback);
		}

		private void LoadServerTimestamp(OnTimeVerifyCallback callback)
		{
			Request request = new RequestBuilder().Url(NetworkConsts.MAIN_SERVER + NetworkConsts.WEB).AddParameter(RequestParam.ID, SDKClient.Instance.AppKey).AddParameter(RequestParam.UID, SDKClient.Instance.UsersStorage.Device.DeviceId)
				.AddParameter(RequestParam.FUNCTION, CheatNetworkConst.CHECK_TIMESTAMP)
				.Secret(SDKClient.Instance.AppSecret)
				.NeedSigned(true)
				.Build();
			Log.D("Send: " + request.Url);
			NetworkClient networkClient = new NetworkClient(OnTimeGot);
			networkClient.Send(request, callback);
		}

		private long LoadSavedTimestamp()
		{
			CheatData cheatData = new CheatData().Load() as CheatData;
			return cheatData.LocalTime;
		}

		private void SaveTimestamp()
		{
			new CheatData().Save(new CheatData
			{
				LocalTime = savedTimestamp
			});
		}

		private TimeVerificationStatus GetTimeStatus()
		{
			currentTimestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
			bool flag = savedTimestamp > currentTimestamp + 3600;
			bool flag2 = serverTimestamp > currentTimestamp + 3600;
			if (flag || flag2)
			{
				return TimeVerificationStatus.TimeRewind;
			}
			if (serverTimestamp + 3600 < currentTimestamp)
			{
				return TimeVerificationStatus.TimeForward;
			}
			return TimeVerificationStatus.TimeValid;
		}
	}
}
