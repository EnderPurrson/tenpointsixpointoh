using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EtceteraUIManager : MonoBehaviourGUI
{
	public GameObject testPlane;

	public EtceteraUIManager()
	{
	}

	private void hideProgress()
	{
		EtceteraAndroid.hideProgressDialog();
	}

	public void imageLoaded(string imagePath)
	{
		EtceteraAndroid.scaleImageAtPath(imagePath, 0.1f);
		this.testPlane.GetComponent<Renderer>().material.mainTexture = EtceteraAndroid.textureFromFileAtPath(imagePath);
	}

	private void OnDisable()
	{
		EtceteraAndroidManager.albumChooserSucceededEvent -= new Action<string>(this.imageLoaded);
		EtceteraAndroidManager.photoChooserSucceededEvent -= new Action<string>(this.imageLoaded);
	}

	private void OnEnable()
	{
		EtceteraAndroidManager.albumChooserSucceededEvent += new Action<string>(this.imageLoaded);
		EtceteraAndroidManager.photoChooserSucceededEvent += new Action<string>(this.imageLoaded);
	}

	private void OnGUI()
	{
		base.beginColumn();
		if (GUILayout.Button("Show Toast", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showToast("Hi. Something just happened in the game and I want to tell you but not interrupt you", true);
		}
		if (GUILayout.Button("Play Video", new GUILayoutOption[0]))
		{
			EtceteraAndroid.playMovie("http://techslides.com/demos/sample-videos/small.3gp", 16711680, false, EtceteraAndroid.ScalingMode.AspectFit, true);
		}
		if (GUILayout.Button("Show Alert", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showAlert("Alert Title Here", "Something just happened.  Do you want to have a snack?", "Yes", "Not Now");
		}
		if (GUILayout.Button("Single Field Prompt", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showAlertPrompt("Enter Digits", "I'll call you if you give me your number", "phone number", "867-5309", "Send", "Not a Chance");
		}
		if (GUILayout.Button("Two Field Prompt", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showAlertPromptWithTwoFields("Need Info", "Enter your credentials:", "username", "harry_potter", "password", string.Empty, "OK", "Cancel");
		}
		if (GUILayout.Button("Show Progress Dialog", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showProgressDialog("Progress is happening", "it will be over in just a second...");
			base.Invoke("hideProgress", 1f);
		}
		if (GUILayout.Button("Text to Speech Speak", new GUILayoutOption[0]))
		{
			EtceteraAndroid.setPitch((float)UnityEngine.Random.Range(0, 5));
			EtceteraAndroid.setSpeechRate(UnityEngine.Random.Range(0.5f, 1.5f));
			EtceteraAndroid.speak("Howdy. Im a robot voice");
		}
		if (GUILayout.Button("Prompt for Video", new GUILayoutOption[0]))
		{
			EtceteraAndroid.promptToTakeVideo("fancyVideo");
		}
		base.endColumn(true);
		if (GUILayout.Button("Show Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showWebView("http://prime31.com");
		}
		if (GUILayout.Button("Email Composer", new GUILayoutOption[0]))
		{
			base.StartCoroutine(this.saveScreenshotToSDCard((string path) => EtceteraAndroid.showEmailComposer("noone@nothing.com", "Message subject", "click <a href='http://somelink.com'>here</a> for a present", true, path)));
		}
		if (GUILayout.Button("SMS Composer", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showSMSComposer("I did something really cool in this game!");
		}
		if (GUILayout.Button("Share Image Natively", new GUILayoutOption[0]))
		{
			base.StartCoroutine(this.saveScreenshotToSDCard((string path) => EtceteraAndroid.shareImageWithNativeShareIntent(path, "Sharing a screenshot...")));
		}
		if (GUILayout.Button("Share Text and Image Natively", new GUILayoutOption[0]))
		{
			base.StartCoroutine(this.saveScreenshotToSDCard((string path) => EtceteraAndroid.shareWithNativeShareIntent("Check this out!", "Some Subject", "Sharing a screenshot and text...", path)));
		}
		if (GUILayout.Button("Prompt to Take Photo", new GUILayoutOption[0]))
		{
			EtceteraAndroid.promptToTakePhoto("photo.jpg");
		}
		if (GUILayout.Button("Prompt for Album Image", new GUILayoutOption[0]))
		{
			EtceteraAndroid.promptForPictureFromAlbum("albumImage.jpg");
		}
		if (GUILayout.Button("Save Image to Gallery", new GUILayoutOption[0]))
		{
			base.StartCoroutine(this.saveScreenshotToSDCard((string path) => UnityEngine.Debug.Log(string.Concat("did save to gallery: ", EtceteraAndroid.saveImageToGallery(path, "My image from Unity")))));
		}
		if (GUILayout.Button("Ask For Review", new GUILayoutOption[0]))
		{
			EtceteraAndroid.resetAskForReview();
			EtceteraAndroid.askForReviewNow("Please rate my app!", "It will really make me happy if you do...", false);
		}
		base.endColumn();
		if (base.bottomRightButton("Next Scene", 150f))
		{
			Application.LoadLevel("EtceteraTestSceneTwo");
		}
	}

	[DebuggerHidden]
	private IEnumerator saveScreenshotToSDCard(Action<string> completionHandler)
	{
		EtceteraUIManager.u003csaveScreenshotToSDCardu003ec__Iterator3 variable = null;
		return variable;
	}

	private void Start()
	{
		EtceteraAndroid.initTTS();
		EtceteraAndroid.setAlertDialogTheme(3);
	}
}