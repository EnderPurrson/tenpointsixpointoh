using Prime31;
using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public sealed class RemotePushNotificationController : MonoBehaviour
{
	private const string RemotePushRegistrationKey = "RemotePushRegistration";

	public static RemotePushNotificationController Instance;

	private bool _isResponceRuning;

	private bool _isStartUpdateRecive;

	private string UrlPushNotificationServer
	{
		get
		{
			return "https://secure.pixelgunserver.com/push_service";
		}
	}

	public RemotePushNotificationController()
	{
	}

	private static bool CheckIfExpired(RemotePushRegistrationMemento remotePushRegistrationMemento)
	{
		DateTime dateTime;
		if (DateTime.TryParse(remotePushRegistrationMemento.RegistrationTime, out dateTime) && (DateTime.UtcNow - dateTime) < TimeSpan.FromDays(2))
		{
			return false;
		}
		return true;
	}

	internal static string GetRemotePushNotificationToken()
	{
		RemotePushRegistrationMemento remotePushRegistrationMemento = RemotePushNotificationController.LoadRemotePushRegistrationMemento();
		if (RemotePushNotificationController.CheckIfExpired(remotePushRegistrationMemento))
		{
			return string.Empty;
		}
		return remotePushRegistrationMemento.RegistrationId;
	}

	private void HandleError(string error)
	{
		UnityEngine.Debug.LogError(error);
	}

	private void HandleRegistered(string registrationId)
	{
		string str = string.Format("{0}.HandleRegistered('{1}')", base.GetType().Name, registrationId);
		ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
		try
		{
			if (!string.IsNullOrEmpty(registrationId))
			{
				base.StartCoroutine(this.ReciveUpdateDataToServer(registrationId));
			}
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}

	private static bool IsDeviceRegistred()
	{
		return !string.IsNullOrEmpty(RemotePushNotificationController.GetRemotePushNotificationToken());
	}

	private static RemotePushRegistrationMemento LoadRemotePushRegistrationMemento()
	{
		return RemotePushNotificationController.ParseRemotePushRegistrationMemento(PlayerPrefs.GetString("RemotePushRegistration", "{}"));
	}

	private static RemotePushRegistrationMemento ParseRemotePushRegistrationMemento(string remotePushRegistrationJson)
	{
		RemotePushRegistrationMemento remotePushRegistrationMemento;
		try
		{
			remotePushRegistrationMemento = JsonUtility.FromJson<RemotePushRegistrationMemento>(remotePushRegistrationJson);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogWarning(exception);
			remotePushRegistrationMemento = new RemotePushRegistrationMemento(string.Empty, DateTime.MinValue, string.Empty);
		}
		return remotePushRegistrationMemento;
	}

	[DebuggerHidden]
	private IEnumerator ReciveUpdateDataToServer(string deviceToken)
	{
		RemotePushNotificationController.u003cReciveUpdateDataToServeru003ec__Iterator100 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		RemotePushNotificationController.u003cStartu003ec__IteratorFF variable = null;
		return variable;
	}
}