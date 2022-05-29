using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Prime31
{
	public class GoogleCloudMessagingManager : AbstractManager
	{
		static GoogleCloudMessagingManager()
		{
			AbstractManager.initialize(typeof(GoogleCloudMessagingManager));
		}

		public GoogleCloudMessagingManager()
		{
		}

		public void notificationReceived(string json)
		{
			GoogleCloudMessagingManager.notificationReceivedEvent.fire<Dictionary<string, object>>(json.dictionaryFromJson());
		}

		public void registrationFailed(string error)
		{
			GoogleCloudMessagingManager.registrationFailedEvent.fire<string>(error);
		}

		public void registrationSucceeded(string registrationId)
		{
			GoogleCloudMessagingManager.registrationSucceededEvent.fire<string>(registrationId);
		}

		public void unregistrationFailed(string param)
		{
			GoogleCloudMessagingManager.unregistrationFailedEvent.fire<string>(param);
		}

		public void unregistrationSucceeded(string empty)
		{
			GoogleCloudMessagingManager.unregistrationSucceededEvent.fire();
		}

		public static event Action<Dictionary<string, object>> notificationReceivedEvent;

		public static event Action<string> registrationFailedEvent;

		public static event Action<string> registrationSucceededEvent;

		public static event Action<string> unregistrationFailedEvent;

		public static event Action unregistrationSucceededEvent;
	}
}