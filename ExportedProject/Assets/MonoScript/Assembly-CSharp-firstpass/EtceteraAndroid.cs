using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EtceteraAndroid
{
	private static AndroidJavaObject _plugin;

	static EtceteraAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.EtceteraPlugin"))
		{
			EtceteraAndroid._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public EtceteraAndroid()
	{
	}

	public static void askForReview(int launchesUntilPrompt, int hoursUntilFirstPrompt, int hoursBetweenPrompts, string title, string message, bool isAmazonAppStore = false)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (isAmazonAppStore)
		{
			EtceteraAndroid._plugin.Set<bool>("isAmazonAppStore", true);
		}
		EtceteraAndroid._plugin.Call("askForReview", new object[] { launchesUntilPrompt, hoursUntilFirstPrompt, hoursBetweenPrompts, title, message });
	}

	public static void askForReviewNow(string title, string message, bool isAmazonAppStore = false)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (isAmazonAppStore)
		{
			EtceteraAndroid._plugin.Set<bool>("isAmazonAppStore", true);
		}
		EtceteraAndroid._plugin.Call("askForReviewNow", new object[] { title, message });
	}

	public static void askForReviewSetButtonTitles(string remindMeLaterTitle, string dontAskAgainTitle, string rateItTitle)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("askForReviewSetButtonTitles", new object[] { remindMeLaterTitle, dontAskAgainTitle, rateItTitle });
	}

	public static void cancelAllNotifications()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("cancelAllNotifications", new object[0]);
	}

	public static void cancelNotification(int notificationId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("cancelNotification", new object[] { notificationId });
	}

	public static void checkForNotifications()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("checkForNotifications", new object[0]);
	}

	public static void enableImmersiveMode(bool shouldEnable)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			EtceteraAndroid._plugin.Call("enableImmersiveMode", new object[] { (!shouldEnable ? 0 : 1) });
		}
	}

	public static Vector2 getImageSizeAtPath(string pathToImage)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return Vector2.zero;
		}
		string str = EtceteraAndroid._plugin.Call<string>("getImageSizeAtPath", new object[] { pathToImage });
		string[] strArrays = str.Split(new char[] { ',' });
		return new Vector2((float)int.Parse(strArrays[0]), (float)int.Parse(strArrays[1]));
	}

	public static void hideProgressDialog()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("hideProgressDialog", new object[0]);
	}

	public static void initTTS()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("initTTS", new object[0]);
	}

	public static void inlineWebViewClose()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewClose", new object[0]);
	}

	public static void inlineWebViewSetFrame(int x, int y, int width, int height)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewSetFrame", new object[] { x, y, width, height });
	}

	public static void inlineWebViewSetUrl(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewSetUrl", new object[] { url });
	}

	public static void inlineWebViewShow(string url, int x, int y, int width, int height)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewShow", new object[] { url, x, y, width, height });
	}

	public static bool isSMSComposerAvailable()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return EtceteraAndroid._plugin.Call<bool>("isSMSComposerAvailable", new object[0]);
	}

	public static void loadContacts(int startingIndex, int totalToRetrieve)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("loadContacts", new object[] { startingIndex, totalToRetrieve });
	}

	public static void openReviewPageInPlayStore(bool isAmazonAppStore = false)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (isAmazonAppStore)
		{
			EtceteraAndroid._plugin.Set<bool>("isAmazonAppStore", true);
		}
		EtceteraAndroid._plugin.Call("openReviewPageInPlayStore", new object[0]);
	}

	public static void playMovie(string pathOrUrl, uint bgColor, bool showControls, EtceteraAndroid.ScalingMode scalingMode, bool closeOnTouch)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("playMovie", new object[] { pathOrUrl, (int)bgColor, showControls, (int)scalingMode, closeOnTouch });
	}

	public static void playSilence(long durationInMs, TTSQueueMode queueMode)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("playSilence", new object[] { durationInMs, (int)queueMode });
	}

	public static void promptForPictureFromAlbum(string name)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("promptForPictureFromAlbum", new object[] { name });
	}

	public static void promptToTakePhoto(string name)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("promptToTakePhoto", new object[] { name });
	}

	public static void promptToTakeVideo(string name)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("promptToTakeVideo", new object[] { name });
	}

	public static void resetAskForReview()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("resetAskForReview", new object[0]);
	}

	public static bool saveImageToGallery(string pathToPhoto, string title)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return EtceteraAndroid._plugin.Call<bool>("saveImageToGallery", new object[] { pathToPhoto, title });
	}

	public static void scaleImageAtPath(string pathToImage, float scale)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("scaleImageAtPath", new object[] { pathToImage, scale });
	}

	public static int scheduleNotification(long secondsFromNow, string title, string subtitle, string tickerText, string extraData, int requestCode = -1)
	{
		return EtceteraAndroid.scheduleNotification(secondsFromNow, title, subtitle, tickerText, extraData, null, null, requestCode);
	}

	public static int scheduleNotification(long secondsFromNow, string title, string subtitle, string tickerText, string extraData, string smallIcon, string largeIcon, int requestCode = -1)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return -1;
		}
		if (requestCode == -1)
		{
			requestCode = UnityEngine.Random.Range(0, 2147483647);
		}
		return EtceteraAndroid._plugin.Call<int>("scheduleNotification", new object[] { secondsFromNow, title, subtitle, tickerText, extraData, smallIcon, largeIcon, requestCode });
	}

	public static void setAlertDialogTheme(int theme)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setAlertDialogTheme", new object[] { theme });
	}

	public static void setPitch(float pitch)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setPitch", new object[] { pitch });
	}

	public static void setSpeechRate(float rate)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setSpeechRate", new object[] { rate });
	}

	public static void setSystemUiVisibilityToLowProfile(bool useLowProfile)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setSystemUiVisibilityToLowProfile", new object[] { useLowProfile });
	}

	public static void shareImageWithNativeShareIntent(string pathToImage, string chooserText)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("shareImageWithNativeShareIntent", new object[] { pathToImage, chooserText });
	}

	public static void shareWithNativeShareIntent(string text, string subject, string chooserText, string pathToImage = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("shareWithNativeShareIntent", new object[] { text, subject, chooserText, pathToImage });
	}

	public static void showAlert(string title, string message, string positiveButton)
	{
		EtceteraAndroid.showAlert(title, message, positiveButton, string.Empty);
	}

	public static void showAlert(string title, string message, string positiveButton, string negativeButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showAlert", new object[] { title, message, positiveButton, negativeButton });
	}

	public static void showAlertPrompt(string title, string message, string promptHint, string promptText, string positiveButton, string negativeButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showAlertPrompt", new object[] { title, message, promptHint, promptText, positiveButton, negativeButton });
	}

	public static void showAlertPromptWithTwoFields(string title, string message, string promptHintOne, string promptTextOne, string promptHintTwo, string promptTextTwo, string positiveButton, string negativeButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showAlertPromptWithTwoFields", new object[] { title, message, promptHintOne, promptTextOne, promptHintTwo, promptTextTwo, positiveButton, negativeButton });
	}

	public static void showCustomWebView(string url, bool disableTitle, bool disableBackButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showCustomWebView", new object[] { url, disableTitle, disableBackButton });
	}

	public static void showEmailComposer(string toAddress, string subject, string text, bool isHTML)
	{
		EtceteraAndroid.showEmailComposer(toAddress, subject, text, isHTML, string.Empty);
	}

	public static void showEmailComposer(string toAddress, string subject, string text, bool isHTML, string attachmentFilePath)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showEmailComposer", new object[] { toAddress, subject, text, isHTML, attachmentFilePath });
	}

	public static void showProgressDialog(string title, string message)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showProgressDialog", new object[] { title, message });
	}

	public static void showSMSComposer(string body)
	{
		EtceteraAndroid.showSMSComposer(body, null);
	}

	public static void showSMSComposer(string body, string[] recipients)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string empty = string.Empty;
		if (recipients != null && (int)recipients.Length > 0)
		{
			empty = "smsto:";
			string[] strArrays = recipients;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				empty = string.Concat(empty, strArrays[i], ";");
			}
		}
		EtceteraAndroid._plugin.Call("showSMSComposer", new object[] { empty, body });
	}

	public static void showToast(string text, bool useShortDuration)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showToast", new object[] { text, useShortDuration });
	}

	public static void showWebView(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showWebView", new object[] { url });
	}

	public static void speak(string text)
	{
		EtceteraAndroid.speak(text, TTSQueueMode.Add);
	}

	public static void speak(string text, TTSQueueMode queueMode)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("speak", new object[] { text, (int)queueMode });
	}

	public static void stop()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("stop", new object[0]);
	}

	public static void teardownTTS()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("teardownTTS", new object[0]);
	}

	public static Texture2D textureFromFileAtPath(string filePath)
	{
		byte[] numArray = File.ReadAllBytes(filePath);
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(numArray);
		texture2D.Apply();
		Debug.Log(string.Concat(new object[] { "texture size: ", texture2D.width, "x", texture2D.height }));
		return texture2D;
	}

	public class Contact
	{
		public string name;

		public List<string> emails;

		public List<string> phoneNumbers;

		public Contact()
		{
		}
	}

	public enum ScalingMode
	{
		None,
		AspectFit,
		Fill
	}
}