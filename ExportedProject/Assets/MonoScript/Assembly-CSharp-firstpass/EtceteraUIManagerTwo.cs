using Prime31;
using System;
using UnityEngine;

public class EtceteraUIManagerTwo : MonoBehaviourGUI
{
	private int _fiveSecondNotificationId;

	private int _tenSecondNotificationId;

	public EtceteraUIManagerTwo()
	{
	}

	private void OnGUI()
	{
		base.beginColumn();
		if (GUILayout.Button("Show Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewShow("http://prime31.com/", 160, 430, Screen.width - 160, Screen.height - 100);
		}
		if (GUILayout.Button("Close Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewClose();
		}
		if (GUILayout.Button("Set Url of Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewSetUrl("http://google.com");
		}
		if (GUILayout.Button("Set Frame of Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewSetFrame(80, 50, 300, 400);
		}
		if (GUILayout.Button("Get First 25 Contacts", new GUILayoutOption[0]))
		{
			EtceteraAndroid.loadContacts(0, 25);
		}
		base.endColumn(true);
		if (GUILayout.Button("Schedule Notification in 5 Seconds", new GUILayoutOption[0]))
		{
			this._fiveSecondNotificationId = EtceteraAndroid.scheduleNotification((long)5, "Notification Title - 5 Seconds", "The subtitle of the notification", "Ticker text gets ticked", "five-second-note", -1);
			Debug.Log(string.Concat("notificationId: ", this._fiveSecondNotificationId));
		}
		if (GUILayout.Button("Schedule Notification in 10 Seconds", new GUILayoutOption[0]))
		{
			this._tenSecondNotificationId = EtceteraAndroid.scheduleNotification((long)10, "Notiifcation Title - 10 Seconds", "The subtitle of the notification", "Ticker text gets ticked", "ten-second-note", -1);
			Debug.Log(string.Concat("notificationId: ", this._tenSecondNotificationId));
		}
		if (GUILayout.Button("Cancel 5 Second Notification", new GUILayoutOption[0]))
		{
			EtceteraAndroid.cancelNotification(this._fiveSecondNotificationId);
		}
		if (GUILayout.Button("Cancel 10 Second Notification", new GUILayoutOption[0]))
		{
			EtceteraAndroid.cancelNotification(this._tenSecondNotificationId);
		}
		if (GUILayout.Button("Check for Notifications", new GUILayoutOption[0]))
		{
			EtceteraAndroid.checkForNotifications();
		}
		if (GUILayout.Button("Cancel All Notifications", new GUILayoutOption[0]))
		{
			EtceteraAndroid.cancelAllNotifications();
		}
		if (GUILayout.Button("Quit App", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		base.endColumn();
		if (base.bottomRightButton("Previous Scene", 150f))
		{
			Application.LoadLevel("EtceteraTestScene");
		}
	}
}