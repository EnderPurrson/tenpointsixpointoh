using Prime31;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EtceteraAndroidManager : AbstractManager
{
	static EtceteraAndroidManager()
	{
		AbstractManager.initialize(typeof(EtceteraAndroidManager));
	}

	public EtceteraAndroidManager()
	{
	}

	public void albumChooserCancelled(string empty)
	{
		if (EtceteraAndroidManager.albumChooserCancelledEvent != null)
		{
			EtceteraAndroidManager.albumChooserCancelledEvent();
		}
	}

	public void albumChooserSucceeded(string path)
	{
		if (EtceteraAndroidManager.albumChooserSucceededEvent != null)
		{
			if (File.Exists(path))
			{
				EtceteraAndroidManager.albumChooserSucceededEvent(path);
			}
			else if (EtceteraAndroidManager.albumChooserCancelledEvent != null)
			{
				EtceteraAndroidManager.albumChooserCancelledEvent();
			}
		}
	}

	public void alertButtonClicked(string positiveButton)
	{
		if (EtceteraAndroidManager.alertButtonClickedEvent != null)
		{
			EtceteraAndroidManager.alertButtonClickedEvent(positiveButton);
		}
	}

	public void alertCancelled(string empty)
	{
		if (EtceteraAndroidManager.alertCancelledEvent != null)
		{
			EtceteraAndroidManager.alertCancelledEvent();
		}
	}

	public void askForReviewDontAskAgain(string empty)
	{
		if (EtceteraAndroidManager.askForReviewDontAskAgainEvent != null)
		{
			EtceteraAndroidManager.askForReviewDontAskAgainEvent();
		}
	}

	public void askForReviewRemindMeLater(string empty)
	{
		if (EtceteraAndroidManager.askForReviewRemindMeLaterEvent != null)
		{
			EtceteraAndroidManager.askForReviewRemindMeLaterEvent();
		}
	}

	public void askForReviewWillOpenMarket(string empty)
	{
		if (EtceteraAndroidManager.askForReviewWillOpenMarketEvent != null)
		{
			EtceteraAndroidManager.askForReviewWillOpenMarketEvent();
		}
	}

	private void contactsLoaded(string json)
	{
		if (EtceteraAndroidManager.contactsLoadedEvent != null)
		{
			List<EtceteraAndroid.Contact> contacts = Json.decode<List<EtceteraAndroid.Contact>>(json, null);
			EtceteraAndroidManager.contactsLoadedEvent(contacts);
		}
	}

	public void inlineWebViewJSCallback(string message)
	{
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent.fire<string>(message);
	}

	public void notificationReceived(string extraData)
	{
		EtceteraAndroidManager.notificationReceivedEvent.fire<string>(extraData);
	}

	public void photoChooserCancelled(string empty)
	{
		if (EtceteraAndroidManager.photoChooserCancelledEvent != null)
		{
			EtceteraAndroidManager.photoChooserCancelledEvent();
		}
	}

	public void photoChooserSucceeded(string path)
	{
		if (EtceteraAndroidManager.photoChooserSucceededEvent != null)
		{
			if (File.Exists(path))
			{
				EtceteraAndroidManager.photoChooserSucceededEvent(path);
			}
			else if (EtceteraAndroidManager.photoChooserCancelledEvent != null)
			{
				EtceteraAndroidManager.photoChooserCancelledEvent();
			}
		}
	}

	public void promptCancelled(string empty)
	{
		if (EtceteraAndroidManager.promptCancelledEvent != null)
		{
			EtceteraAndroidManager.promptCancelledEvent();
		}
	}

	public void promptFinishedWithText(string text)
	{
		string[] strArrays = text.Split(new string[] { "|||" }, StringSplitOptions.None);
		if ((int)strArrays.Length == 1 && EtceteraAndroidManager.promptFinishedWithTextEvent != null)
		{
			EtceteraAndroidManager.promptFinishedWithTextEvent(strArrays[0]);
		}
		if ((int)strArrays.Length == 2 && EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent != null)
		{
			EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent(strArrays[0], strArrays[1]);
		}
	}

	public void ttsInitialized(string result)
	{
		bool flag = result == "1";
		if (flag && EtceteraAndroidManager.ttsInitializedEvent != null)
		{
			EtceteraAndroidManager.ttsInitializedEvent();
		}
		if (!flag && EtceteraAndroidManager.ttsFailedToInitializeEvent != null)
		{
			EtceteraAndroidManager.ttsFailedToInitializeEvent();
		}
	}

	public void ttsUtteranceCompleted(string utteranceId)
	{
		Debug.Log(string.Concat("utterance completed: ", utteranceId));
	}

	public void twoFieldPromptCancelled(string empty)
	{
		if (EtceteraAndroidManager.twoFieldPromptCancelledEvent != null)
		{
			EtceteraAndroidManager.twoFieldPromptCancelledEvent();
		}
	}

	public void videoRecordingCancelled(string empty)
	{
		if (EtceteraAndroidManager.videoRecordingCancelledEvent != null)
		{
			EtceteraAndroidManager.videoRecordingCancelledEvent();
		}
	}

	public void videoRecordingSucceeded(string path)
	{
		if (EtceteraAndroidManager.videoRecordingSucceededEvent != null)
		{
			EtceteraAndroidManager.videoRecordingSucceededEvent(path);
		}
	}

	public void webViewCancelled(string empty)
	{
		if (EtceteraAndroidManager.webViewCancelledEvent != null)
		{
			EtceteraAndroidManager.webViewCancelledEvent();
		}
	}

	public static event Action albumChooserCancelledEvent;

	public static event Action<string> albumChooserSucceededEvent;

	public static event Action<string> alertButtonClickedEvent;

	public static event Action alertCancelledEvent;

	public static event Action askForReviewDontAskAgainEvent;

	public static event Action askForReviewRemindMeLaterEvent;

	public static event Action askForReviewWillOpenMarketEvent;

	public static event Action<List<EtceteraAndroid.Contact>> contactsLoadedEvent;

	public static event Action<string> inlineWebViewJSCallbackEvent;

	public static event Action<string> notificationReceivedEvent;

	public static event Action photoChooserCancelledEvent;

	public static event Action<string> photoChooserSucceededEvent;

	public static event Action promptCancelledEvent;

	public static event Action<string> promptFinishedWithTextEvent;

	public static event Action ttsFailedToInitializeEvent;

	public static event Action ttsInitializedEvent;

	public static event Action twoFieldPromptCancelledEvent;

	public static event Action<string, string> twoFieldPromptFinishedWithTextEvent;

	public static event Action videoRecordingCancelledEvent;

	public static event Action<string> videoRecordingSucceededEvent;

	public static event Action webViewCancelledEvent;
}