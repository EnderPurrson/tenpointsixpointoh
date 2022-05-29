using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GooglePlayGames.Native
{
	internal class NativeEventClient : IEventsClient
	{
		private readonly EventManager mEventManager;

		internal NativeEventClient(EventManager manager)
		{
			this.mEventManager = Misc.CheckNotNull<EventManager>(manager);
		}

		public void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback)
		{
			Action<ResponseStatus, List<IEvent>> onGameThread = callback;
			Misc.CheckNotNull<Action<ResponseStatus, List<IEvent>>>(onGameThread);
			onGameThread = CallbackUtils.ToOnGameThread<ResponseStatus, List<IEvent>>(onGameThread);
			this.mEventManager.FetchAll(ConversionUtils.AsDataSource(source), (EventManager.FetchAllResponse response) => {
				ResponseStatus responseStatu = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (response.RequestSucceeded())
				{
					onGameThread(responseStatu, response.Data().Cast<IEvent>().ToList<IEvent>());
				}
				else
				{
					onGameThread(responseStatu, new List<IEvent>());
				}
			});
		}

		public void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback)
		{
			Misc.CheckNotNull<string>(eventId);
			Misc.CheckNotNull<Action<ResponseStatus, IEvent>>(callback);
			this.mEventManager.Fetch(ConversionUtils.AsDataSource(source), eventId, (EventManager.FetchResponse response) => {
				ResponseStatus responseStatu = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (response.RequestSucceeded())
				{
					callback(responseStatu, response.Data());
				}
				else
				{
					callback(responseStatu, null);
				}
			});
		}

		public void IncrementEvent(string eventId, uint stepsToIncrement)
		{
			Misc.CheckNotNull<string>(eventId);
			this.mEventManager.Increment(eventId, stepsToIncrement);
		}
	}
}