using System;
using System.Runtime.CompilerServices;
using System.Threading;
using DevToDev.Core.Network;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Logic
{
	internal class MetricsController
	{
		internal class WebTimer
		{
			private long currentTimerValue;

			private long interval;

			private object timerData;

			private Action<object> functionOnTick;

			public WebTimer(Action<object> functionOnTick, object timerData, long interval)
			{
				currentTimerValue = DeviceHelper.Instance.GetUnixTime();
				this.interval = interval;
				this.timerData = timerData;
				this.functionOnTick = functionOnTick;
			}

			public void WebTimerTick()
			{
				long unixTime = DeviceHelper.Instance.GetUnixTime();
				if (unixTime >= currentTimerValue + interval)
				{
					functionOnTick.Invoke(timerData);
					currentTimerValue = unixTime;
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2
		{
			public MetricsController _003C_003E4__this;

			public object state;

			public void _003CTimerTick_003Eb__0()
			{
				try
				{
					_003C_003E4__this.PeriodicSendHandler.Invoke(state, (EventArgs)default(EventArgs));
				}
				catch
				{
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass6
		{
			public MetricsStorage metricStorage;

			public OnRequestSend onSendCallback;

			public void _003CProceedMetricsStorage_003Eb__4()
			{
				SDKRequests.SendStorage(metricStorage, onSendCallback);
			}
		}

		public EventHandler PeriodicSendHandler;

		private Timer timer;

		private WebTimer webTimer;

		public void WebTimerTick()
		{
			if (webTimer != null)
			{
				webTimer.WebTimerTick();
			}
		}

		public void Resume()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			int num = SDKClient.Instance.NetworkStorage.TimeForRequest * 1000;
			if (UnityPlayerPlatform.isUnityWebPlatform())
			{
				if (webTimer == null)
				{
					webTimer = new WebTimer(TimerTick, null, num);
				}
			}
			else if (timer == null)
			{
				timer = new Timer(new TimerCallback(TimerTick), (object)default(object), num, num);
			}
			Log.Resume();
		}

		public void Suspend()
		{
			if (!UnityPlayerPlatform.isUnityWebPlatform() && timer != null)
			{
				timer.Dispose();
				timer = null;
			}
			Log.Suspend();
		}

		private void TimerTick(object state)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			Action val = null;
			_003C_003Ec__DisplayClass2 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass2();
			_003C_003Ec__DisplayClass.state = state;
			_003C_003Ec__DisplayClass._003C_003E4__this = this;
			Log.D("timer send");
			try
			{
				AsyncOperationDispatcher asyncOperationDispatcher = SDKClient.Instance.AsyncOperationDispatcher;
				if (val == null)
				{
					val = new Action(_003C_003Ec__DisplayClass._003CTimerTick_003Eb__0);
				}
				asyncOperationDispatcher.DispatchOnMainThread(val);
			}
			catch
			{
			}
		}

		public void ProceedMetricsStorage(MetricsStorage metricStorage, OnRequestSend onSendCallback)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			Action val = null;
			_003C_003Ec__DisplayClass6 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass6();
			_003C_003Ec__DisplayClass.metricStorage = metricStorage;
			_003C_003Ec__DisplayClass.onSendCallback = onSendCallback;
			Log.D("Proceed");
			lock (_003C_003Ec__DisplayClass.metricStorage)
			{
				AsyncOperationDispatcher asyncOperationDispatcher = SDKClient.Instance.AsyncOperationDispatcher;
				if (val == null)
				{
					val = new Action(_003C_003Ec__DisplayClass._003CProceedMetricsStorage_003Eb__4);
				}
				asyncOperationDispatcher.DispatchOnMainThread(val);
			}
		}
	}
}
