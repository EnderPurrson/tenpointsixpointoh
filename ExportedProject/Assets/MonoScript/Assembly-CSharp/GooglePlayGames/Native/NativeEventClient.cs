using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	internal class NativeEventClient : IEventsClient
	{
		[CompilerGenerated]
		private sealed class _003CFetchAllEvents_003Ec__AnonStorey221
		{
			internal Action<ResponseStatus, List<IEvent>> callback;

			internal void _003C_003Em__B6(EventManager.FetchAllResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, new List<IEvent>());
				}
				else
				{
					callback(arg, response.Data().Cast<IEvent>().ToList());
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CFetchEvent_003Ec__AnonStorey222
		{
			internal Action<ResponseStatus, IEvent> callback;

			internal void _003C_003Em__B7(EventManager.FetchResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data());
				}
			}
		}

		private readonly EventManager mEventManager;

		internal NativeEventClient(EventManager manager)
		{
			mEventManager = Misc.CheckNotNull(manager);
		}

		public void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback)
		{
			_003CFetchAllEvents_003Ec__AnonStorey221 _003CFetchAllEvents_003Ec__AnonStorey = new _003CFetchAllEvents_003Ec__AnonStorey221();
			_003CFetchAllEvents_003Ec__AnonStorey.callback = callback;
			Misc.CheckNotNull(_003CFetchAllEvents_003Ec__AnonStorey.callback);
			_003CFetchAllEvents_003Ec__AnonStorey.callback = CallbackUtils.ToOnGameThread(_003CFetchAllEvents_003Ec__AnonStorey.callback);
			mEventManager.FetchAll(ConversionUtils.AsDataSource(source), _003CFetchAllEvents_003Ec__AnonStorey._003C_003Em__B6);
		}

		public void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback)
		{
			_003CFetchEvent_003Ec__AnonStorey222 _003CFetchEvent_003Ec__AnonStorey = new _003CFetchEvent_003Ec__AnonStorey222();
			_003CFetchEvent_003Ec__AnonStorey.callback = callback;
			Misc.CheckNotNull(eventId);
			Misc.CheckNotNull(_003CFetchEvent_003Ec__AnonStorey.callback);
			mEventManager.Fetch(ConversionUtils.AsDataSource(source), eventId, _003CFetchEvent_003Ec__AnonStorey._003C_003Em__B7);
		}

		public void IncrementEvent(string eventId, uint stepsToIncrement)
		{
			Misc.CheckNotNull(eventId);
			mEventManager.Increment(eventId, stepsToIncrement);
		}
	}
}
