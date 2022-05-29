using Prime31;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EtceteraAndroidEventListener : MonoBehaviour
{
	public EtceteraAndroidEventListener()
	{
	}

	private void albumChooserCancelledEvent()
	{
		Debug.Log("albumChooserCancelledEvent");
	}

	private void albumChooserSucceededEvent(string imagePath)
	{
		Debug.Log(string.Concat("albumChooserSucceededEvent: ", imagePath));
		Debug.Log(string.Concat("image size: ", EtceteraAndroid.getImageSizeAtPath(imagePath)));
	}

	private void alertButtonClickedEvent(string positiveButton)
	{
		Debug.Log(string.Concat("alertButtonClickedEvent: ", positiveButton));
	}

	private void alertCancelledEvent()
	{
		Debug.Log("alertCancelledEvent");
	}

	private void askForReviewDontAskAgainEvent()
	{
		Debug.Log("askForReviewDontAskAgainEvent");
	}

	private void askForReviewRemindMeLaterEvent()
	{
		Debug.Log("askForReviewRemindMeLaterEvent");
	}

	private void askForReviewWillOpenMarketEvent()
	{
		Debug.Log("askForReviewWillOpenMarketEvent");
	}

	private void contactsLoadedEvent(List<EtceteraAndroid.Contact> contacts)
	{
		Debug.Log("contactsLoadedEvent");
		Utils.logObject(contacts);
	}

	private void inlineWebViewJSCallbackEvent(string message)
	{
		Debug.Log(string.Concat("inlineWebViewJSCallbackEvent: ", message));
	}

	private void notificationReceivedEvent(string extraData)
	{
		Debug.Log(string.Concat("notificationReceivedEvent: ", extraData));
	}

	private void OnDisable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= new Action<string>(this.alertButtonClickedEvent);
		EtceteraAndroidManager.alertCancelledEvent -= new Action(this.alertCancelledEvent);
		EtceteraAndroidManager.promptFinishedWithTextEvent -= new Action<string>(this.promptFinishedWithTextEvent);
		EtceteraAndroidManager.promptCancelledEvent -= new Action(this.promptCancelledEvent);
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent -= new Action<string, string>(this.twoFieldPromptFinishedWithTextEvent);
		EtceteraAndroidManager.twoFieldPromptCancelledEvent -= new Action(this.twoFieldPromptCancelledEvent);
		EtceteraAndroidManager.webViewCancelledEvent -= new Action(this.webViewCancelledEvent);
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent -= new Action<string>(this.inlineWebViewJSCallbackEvent);
		EtceteraAndroidManager.albumChooserCancelledEvent -= new Action(this.albumChooserCancelledEvent);
		EtceteraAndroidManager.albumChooserSucceededEvent -= new Action<string>(this.albumChooserSucceededEvent);
		EtceteraAndroidManager.photoChooserCancelledEvent -= new Action(this.photoChooserCancelledEvent);
		EtceteraAndroidManager.photoChooserSucceededEvent -= new Action<string>(this.photoChooserSucceededEvent);
		EtceteraAndroidManager.videoRecordingCancelledEvent -= new Action(this.videoRecordingCancelledEvent);
		EtceteraAndroidManager.videoRecordingSucceededEvent -= new Action<string>(this.videoRecordingSucceededEvent);
		EtceteraAndroidManager.ttsInitializedEvent -= new Action(this.ttsInitializedEvent);
		EtceteraAndroidManager.ttsFailedToInitializeEvent -= new Action(this.ttsFailedToInitializeEvent);
		EtceteraAndroidManager.askForReviewDontAskAgainEvent -= new Action(this.askForReviewDontAskAgainEvent);
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent -= new Action(this.askForReviewRemindMeLaterEvent);
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent -= new Action(this.askForReviewWillOpenMarketEvent);
		EtceteraAndroidManager.notificationReceivedEvent -= new Action<string>(this.notificationReceivedEvent);
		EtceteraAndroidManager.contactsLoadedEvent -= new Action<List<EtceteraAndroid.Contact>>(this.contactsLoadedEvent);
	}

	private void OnEnable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent += new Action<string>(this.alertButtonClickedEvent);
		EtceteraAndroidManager.alertCancelledEvent += new Action(this.alertCancelledEvent);
		EtceteraAndroidManager.promptFinishedWithTextEvent += new Action<string>(this.promptFinishedWithTextEvent);
		EtceteraAndroidManager.promptCancelledEvent += new Action(this.promptCancelledEvent);
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent += new Action<string, string>(this.twoFieldPromptFinishedWithTextEvent);
		EtceteraAndroidManager.twoFieldPromptCancelledEvent += new Action(this.twoFieldPromptCancelledEvent);
		EtceteraAndroidManager.webViewCancelledEvent += new Action(this.webViewCancelledEvent);
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent += new Action<string>(this.inlineWebViewJSCallbackEvent);
		EtceteraAndroidManager.albumChooserCancelledEvent += new Action(this.albumChooserCancelledEvent);
		EtceteraAndroidManager.albumChooserSucceededEvent += new Action<string>(this.albumChooserSucceededEvent);
		EtceteraAndroidManager.photoChooserCancelledEvent += new Action(this.photoChooserCancelledEvent);
		EtceteraAndroidManager.photoChooserSucceededEvent += new Action<string>(this.photoChooserSucceededEvent);
		EtceteraAndroidManager.videoRecordingCancelledEvent += new Action(this.videoRecordingCancelledEvent);
		EtceteraAndroidManager.videoRecordingSucceededEvent += new Action<string>(this.videoRecordingSucceededEvent);
		EtceteraAndroidManager.ttsInitializedEvent += new Action(this.ttsInitializedEvent);
		EtceteraAndroidManager.ttsFailedToInitializeEvent += new Action(this.ttsFailedToInitializeEvent);
		EtceteraAndroidManager.askForReviewDontAskAgainEvent += new Action(this.askForReviewDontAskAgainEvent);
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent += new Action(this.askForReviewRemindMeLaterEvent);
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent += new Action(this.askForReviewWillOpenMarketEvent);
		EtceteraAndroidManager.notificationReceivedEvent += new Action<string>(this.notificationReceivedEvent);
		EtceteraAndroidManager.contactsLoadedEvent += new Action<List<EtceteraAndroid.Contact>>(this.contactsLoadedEvent);
	}

	private void photoChooserCancelledEvent()
	{
		Debug.Log("photoChooserCancelledEvent");
	}

	private void photoChooserSucceededEvent(string imagePath)
	{
		Debug.Log(string.Concat("photoChooserSucceededEvent: ", imagePath));
		Debug.Log(string.Concat("image size: ", EtceteraAndroid.getImageSizeAtPath(imagePath)));
	}

	private void promptCancelledEvent()
	{
		Debug.Log("promptCancelledEvent");
	}

	private void promptFinishedWithTextEvent(string param)
	{
		Debug.Log(string.Concat("promptFinishedWithTextEvent: ", param));
	}

	private void ttsFailedToInitializeEvent()
	{
		Debug.Log("ttsFailedToInitializeEvent");
	}

	private void ttsInitializedEvent()
	{
		Debug.Log("ttsInitializedEvent");
	}

	private void twoFieldPromptCancelledEvent()
	{
		Debug.Log("twoFieldPromptCancelledEvent");
	}

	private void twoFieldPromptFinishedWithTextEvent(string text1, string text2)
	{
		Debug.Log(string.Concat("twoFieldPromptFinishedWithTextEvent: ", text1, ", ", text2));
	}

	private void videoRecordingCancelledEvent()
	{
		Debug.Log("videoRecordingCancelledEvent");
	}

	private void videoRecordingSucceededEvent(string path)
	{
		Debug.Log(string.Concat("videoRecordingSucceededEvent: ", path));
	}

	private void webViewCancelledEvent()
	{
		Debug.Log("webViewCancelledEvent");
	}
}