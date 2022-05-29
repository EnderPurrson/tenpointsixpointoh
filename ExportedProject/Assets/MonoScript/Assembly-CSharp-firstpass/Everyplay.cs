using EveryplayMiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class Everyplay : MonoBehaviour
{
	private const string nativeMethodSource = "__Internal";

	private static string clientId;

	private static bool appIsClosing;

	private static bool hasMethods;

	private static bool seenInitialization;

	private static bool readyForRecording;

	private static EveryplayLegacy everyplayLegacy;

	private static Everyplay everyplayInstance;

	private static Texture2D currentThumbnailTargetTexture;

	private static AndroidJavaObject everyplayUnity;

	private static Everyplay EveryplayInstance
	{
		get
		{
			Everyplay everyplay;
			if (Everyplay.everyplayInstance == null && !Everyplay.appIsClosing)
			{
				EveryplaySettings everyplaySetting = (EveryplaySettings)Resources.Load("EveryplaySettings");
				if (everyplaySetting != null && everyplaySetting.IsEnabled)
				{
					GameObject gameObject = new GameObject("Everyplay");
					if (gameObject != null)
					{
						Everyplay.everyplayInstance = gameObject.AddComponent<Everyplay>();
						if (Everyplay.everyplayInstance != null)
						{
							Everyplay.clientId = everyplaySetting.clientId;
							Everyplay.hasMethods = true;
							try
							{
								Everyplay.InitEveryplay(everyplaySetting.clientId, everyplaySetting.clientSecret, everyplaySetting.redirectURI);
								Everyplay.seenInitialization;
								Everyplay.seenInitialization = true;
								if (everyplaySetting.testButtonsEnabled)
								{
									Everyplay.AddTestButtons(gameObject);
								}
								UnityEngine.Object.DontDestroyOnLoad(gameObject);
								return Everyplay.everyplayInstance;
							}
							catch (DllNotFoundException dllNotFoundException)
							{
								Everyplay.hasMethods = false;
								Everyplay.everyplayInstance.OnApplicationQuit();
								everyplay = null;
							}
							catch (EntryPointNotFoundException entryPointNotFoundException)
							{
								Everyplay.hasMethods = false;
								Everyplay.everyplayInstance.OnApplicationQuit();
								everyplay = null;
							}
							return everyplay;
						}
					}
				}
			}
			return Everyplay.everyplayInstance;
		}
	}

	[Obsolete("Calling Everyplay with SharedInstance is deprecated, you may remove SharedInstance.")]
	public static EveryplayLegacy SharedInstance
	{
		get
		{
			if (Everyplay.EveryplayInstance != null && Everyplay.everyplayLegacy == null)
			{
				Everyplay.everyplayLegacy = Everyplay.everyplayInstance.gameObject.AddComponent<EveryplayLegacy>();
			}
			return Everyplay.everyplayLegacy;
		}
	}

	static Everyplay()
	{
		Everyplay.hasMethods = true;
	}

	public Everyplay()
	{
	}

	public static string AccessToken()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayAccountAccessToken();
		}
		return null;
	}

	private static void AddTestButtons(GameObject gameObject)
	{
		Texture2D texture2D = (Texture2D)Resources.Load("everyplay-test-buttons", typeof(Texture2D));
		if (texture2D != null)
		{
			EveryplayRecButtons everyplayRecButton = gameObject.AddComponent<EveryplayRecButtons>();
			if (everyplayRecButton != null)
			{
				everyplayRecButton.atlasTexture = texture2D;
			}
		}
	}

	private void AsyncMakeRequest(string method, string url, Dictionary<string, object> data, Everyplay.RequestReadyDelegate readyDelegate, Everyplay.RequestFailedDelegate failedDelegate)
	{
		base.StartCoroutine(this.MakeRequestEnumerator(method, url, data, readyDelegate, failedDelegate));
	}

	public static string EveryplayAccountAccessToken()
	{
		return Everyplay.everyplayUnity.Call<string>("getAccessToken", new object[0]);
	}

	public static float EveryplayFaceCamAudioPeakLevel()
	{
		return Everyplay.everyplayUnity.Call<float>("faceCamAudioPeakLevel", new object[0]);
	}

	public static float EveryplayFaceCamAudioPowerLevel()
	{
		return Everyplay.everyplayUnity.Call<float>("faceCamAudioPowerLevel", new object[0]);
	}

	public static bool EveryplayFaceCamIsAudioRecordingSupported()
	{
		return Everyplay.everyplayUnity.Call<bool>("faceCamIsAudioRecordingSupported", new object[0]);
	}

	public static bool EveryplayFaceCamIsHeadphonesPluggedIn()
	{
		return Everyplay.everyplayUnity.Call<bool>("faceCamIsHeadphonesPluggedIn", new object[0]);
	}

	public static bool EveryplayFaceCamIsRecordingPermissionGranted()
	{
		return Everyplay.everyplayUnity.Call<bool>("faceCamIsRecordingPermissionGranted", new object[0]);
	}

	public static bool EveryplayFaceCamIsSessionRunning()
	{
		return Everyplay.everyplayUnity.Call<bool>("faceCamIsSessionRunning", new object[0]);
	}

	public static bool EveryplayFaceCamIsVideoRecordingSupported()
	{
		return Everyplay.everyplayUnity.Call<bool>("faceCamIsVideoRecordingSupported", new object[0]);
	}

	private void EveryplayFaceCamRecordingPermission(string jsonMsg)
	{
		bool flag;
		if (Everyplay.FaceCamRecordingPermission != null && EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg).TryGetValue<bool>("granted", out flag))
		{
			Everyplay.FaceCamRecordingPermission(flag);
		}
	}

	public static void EveryplayFaceCamRequestRecordingPermission()
	{
		Everyplay.everyplayUnity.Call("faceCamRequestRecordingPermission", new object[0]);
	}

	private void EveryplayFaceCamSessionStarted(string msg)
	{
		if (Everyplay.FaceCamSessionStarted != null)
		{
			Everyplay.FaceCamSessionStarted();
		}
	}

	private void EveryplayFaceCamSessionStopped(string msg)
	{
		if (Everyplay.FaceCamSessionStopped != null)
		{
			Everyplay.FaceCamSessionStopped();
		}
	}

	public static void EveryplayFaceCamSetAudioOnly(bool audioOnly)
	{
		Everyplay.everyplayUnity.Call("faceCamSetAudioOnly", new object[] { audioOnly });
	}

	public static void EveryplayFaceCamSetMonitorAudioLevels(bool enabled)
	{
		Everyplay.everyplayUnity.Call("faceCamSetSetMonitorAudioLevels", new object[] { enabled });
	}

	public static void EveryplayFaceCamSetPreviewBorderColor(float r, float g, float b, float a)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewBorderColor", new object[] { r, g, b, a });
	}

	public static void EveryplayFaceCamSetPreviewBorderWidth(int width)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewBorderWidth", new object[] { width });
	}

	public static void EveryplayFaceCamSetPreviewOrigin(int origin)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewOrigin", new object[] { origin });
	}

	public static void EveryplayFaceCamSetPreviewPositionX(int x)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewPositionX", new object[] { x });
	}

	public static void EveryplayFaceCamSetPreviewPositionY(int y)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewPositionY", new object[] { y });
	}

	public static void EveryplayFaceCamSetPreviewScaleRetina(bool autoScale)
	{
		UnityEngine.Debug.Log(string.Concat(MethodBase.GetCurrentMethod().Name, " not available on Android"));
	}

	public static void EveryplayFaceCamSetPreviewSideWidth(int width)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewSideWidth", new object[] { width });
	}

	public static void EveryplayFaceCamSetPreviewVisible(bool visible)
	{
		Everyplay.everyplayUnity.Call("faceCamSetPreviewVisible", new object[] { visible });
	}

	public static void EveryplayFaceCamSetRecordingMode(int mode)
	{
		Everyplay.everyplayUnity.Call("faceCamSetRecordingMode", new object[] { mode });
	}

	public static void EveryplayFaceCamSetTargetTextureHeight(int textureHeight)
	{
		Everyplay.everyplayUnity.Call("faceCamSetTargetTextureHeight", new object[] { textureHeight });
	}

	public static void EveryplayFaceCamSetTargetTextureId(int textureId)
	{
		Everyplay.everyplayUnity.Call("faceCamSetTargetTextureId", new object[] { textureId });
	}

	public static void EveryplayFaceCamSetTargetTextureWidth(int textureWidth)
	{
		Everyplay.everyplayUnity.Call("faceCamSetTargetTextureWidth", new object[] { textureWidth });
	}

	public static void EveryplayFaceCamStartSession()
	{
		Everyplay.everyplayUnity.Call("faceCamStartSession", new object[0]);
	}

	public static void EveryplayFaceCamStopSession()
	{
		Everyplay.everyplayUnity.Call("faceCamStopSession", new object[0]);
	}

	public static int EveryplayGetUserInterfaceIdiom()
	{
		return Everyplay.everyplayUnity.Call<int>("getUserInterfaceIdiom", new object[0]);
	}

	private void EveryplayHidden(string msg)
	{
		if (Everyplay.WasClosed != null)
		{
			Everyplay.WasClosed();
		}
	}

	public static bool EveryplayIsPaused()
	{
		return Everyplay.everyplayUnity.Call<bool>("isPaused", new object[0]);
	}

	public static bool EveryplayIsRecording()
	{
		return Everyplay.everyplayUnity.Call<bool>("isRecording", new object[0]);
	}

	public static bool EveryplayIsRecordingSupported()
	{
		return Everyplay.everyplayUnity.Call<bool>("isRecordingSupported", new object[0]);
	}

	public static bool EveryplayIsSingleCoreDevice()
	{
		return Everyplay.everyplayUnity.Call<bool>("isSingleCoreDevice", new object[0]);
	}

	public static bool EveryplayIsSupported()
	{
		return Everyplay.everyplayUnity.Call<bool>("isSupported", new object[0]);
	}

	public static void EveryplayPauseRecording()
	{
		Everyplay.everyplayUnity.Call("pauseRecording", new object[0]);
	}

	public static void EveryplayPlayLastRecording()
	{
		Everyplay.everyplayUnity.Call("playLastRecording", new object[0]);
	}

	public static void EveryplayPlayVideoWithDictionary(string dic)
	{
		Everyplay.everyplayUnity.Call("playVideoWithDictionary", new object[] { dic });
	}

	public static void EveryplayPlayVideoWithURL(string url)
	{
		Everyplay.everyplayUnity.Call("playVideoWithURL", new object[] { url });
	}

	private void EveryplayReadyForRecording(string jsonMsg)
	{
		bool flag;
		if (Everyplay.ReadyForRecording != null && EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg).TryGetValue<bool>("enabled", out flag))
		{
			Everyplay.readyForRecording = flag;
			Everyplay.ReadyForRecording(flag);
		}
	}

	private void EveryplayRecordingStarted(string msg)
	{
		if (Everyplay.RecordingStarted != null)
		{
			Everyplay.RecordingStarted();
		}
	}

	private void EveryplayRecordingStopped(string msg)
	{
		if (Everyplay.RecordingStopped != null)
		{
			Everyplay.RecordingStopped();
		}
	}

	public static void EveryplayResumeRecording()
	{
		Everyplay.everyplayUnity.Call("resumeRecording", new object[0]);
	}

	public static void EveryplaySetDisableSingleCoreDevices(bool state)
	{
		Everyplay.everyplayUnity.Call("setDisableSingleCoreDevices", new object[] { (!state ? 0 : 1) });
	}

	public static void EveryplaySetLowMemoryDevice(bool state)
	{
		Everyplay.everyplayUnity.Call("setLowMemoryDevice", new object[] { (!state ? 0 : 1) });
	}

	public static void EveryplaySetMaxRecordingMinutesLength(int minutes)
	{
		Everyplay.everyplayUnity.Call("setMaxRecordingMinutesLength", new object[] { minutes });
	}

	public static void EveryplaySetMetadata(string json)
	{
		Everyplay.everyplayUnity.Call("setMetadata", new object[] { json });
	}

	public static void EveryplaySetMotionFactor(int factor)
	{
		Everyplay.everyplayUnity.Call("setMotionFactor", new object[] { factor });
	}

	public static void EveryplaySetTargetFPS(int fps)
	{
		Everyplay.everyplayUnity.Call("setTargetFPS", new object[] { fps });
	}

	public static void EveryplaySetThumbnailTargetTextureHeight(int textureHeight)
	{
		Everyplay.everyplayUnity.Call("setThumbnailTargetTextureHeight", new object[] { textureHeight });
	}

	public static void EveryplaySetThumbnailTargetTextureId(int textureId)
	{
		Everyplay.everyplayUnity.Call("setThumbnailTargetTextureId", new object[] { textureId });
	}

	public static void EveryplaySetThumbnailTargetTextureWidth(int textureWidth)
	{
		Everyplay.everyplayUnity.Call("setThumbnailTargetTextureWidth", new object[] { textureWidth });
	}

	public static void EveryplayShow()
	{
		Everyplay.everyplayUnity.Call<bool>("showEveryplay", new object[0]);
	}

	public static void EveryplayShowSharingModal()
	{
		Everyplay.everyplayUnity.Call("showSharingModal", new object[0]);
	}

	public static void EveryplayShowWithPath(string path)
	{
		Everyplay.everyplayUnity.Call<bool>("showEveryplay", new object[] { path });
	}

	public static bool EveryplaySnapshotRenderbuffer()
	{
		return Everyplay.everyplayUnity.Call<bool>("snapshotRenderbuffer", new object[0]);
	}

	public static void EveryplayStartRecording()
	{
		Everyplay.everyplayUnity.Call("startRecording", new object[0]);
	}

	public static void EveryplayStopRecording()
	{
		Everyplay.everyplayUnity.Call("stopRecording", new object[0]);
	}

	public static void EveryplayTakeThumbnail()
	{
		Everyplay.everyplayUnity.Call("takeThumbnail", new object[0]);
	}

	private void EveryplayThumbnailReadyAtTextureId(string jsonMsg)
	{
		int num;
		bool flag;
		if (Everyplay.ThumbnailReadyAtTextureId != null || Everyplay.ThumbnailTextureReady != null)
		{
			Dictionary<string, object> dictionary = EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg);
			if (dictionary.TryGetValue<int>("textureId", out num) && dictionary.TryGetValue<bool>("portrait", out flag))
			{
				if (Everyplay.ThumbnailReadyAtTextureId != null)
				{
					Everyplay.ThumbnailReadyAtTextureId(num, flag);
				}
				if (Everyplay.ThumbnailTextureReady != null && Everyplay.currentThumbnailTargetTexture != null && Everyplay.currentThumbnailTargetTexture.GetNativeTextureID() == num)
				{
					Everyplay.ThumbnailTextureReady(Everyplay.currentThumbnailTargetTexture, flag);
				}
			}
		}
	}

	private void EveryplayThumbnailTextureReady(string jsonMsg)
	{
		long num;
		bool flag;
		if (Everyplay.ThumbnailTextureReady != null)
		{
			Dictionary<string, object> dictionary = EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg);
			if (Everyplay.currentThumbnailTargetTexture != null && dictionary.TryGetValue<long>("texturePtr", out num) && dictionary.TryGetValue<bool>("portrait", out flag) && (long)Everyplay.currentThumbnailTargetTexture.GetNativeTexturePtr() == num)
			{
				Everyplay.ThumbnailTextureReady(Everyplay.currentThumbnailTargetTexture, flag);
			}
		}
	}

	private void EveryplayUploadDidComplete(string jsonMsg)
	{
		int num;
		if (Everyplay.UploadDidComplete != null && EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg).TryGetValue<int>("videoId", out num))
		{
			Everyplay.UploadDidComplete(num);
		}
	}

	private void EveryplayUploadDidProgress(string jsonMsg)
	{
		int num;
		double num1;
		if (Everyplay.UploadDidProgress != null)
		{
			Dictionary<string, object> dictionary = EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg);
			if (dictionary.TryGetValue<int>("videoId", out num) && dictionary.TryGetValue<double>("progress", out num1))
			{
				Everyplay.UploadDidProgress(num, (float)num1);
			}
		}
	}

	private void EveryplayUploadDidStart(string jsonMsg)
	{
		int num;
		if (Everyplay.UploadDidStart != null && EveryplayDictionaryExtensions.JsonToDictionary(jsonMsg).TryGetValue<int>("videoId", out num))
		{
			Everyplay.UploadDidStart(num);
		}
	}

	public static float FaceCamAudioPeakLevel()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamAudioPeakLevel();
		}
		return 0f;
	}

	public static float FaceCamAudioPowerLevel()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamAudioPowerLevel();
		}
		return 0f;
	}

	public static bool FaceCamIsAudioRecordingSupported()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamIsAudioRecordingSupported();
		}
		return false;
	}

	public static bool FaceCamIsHeadphonesPluggedIn()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamIsHeadphonesPluggedIn();
		}
		return false;
	}

	public static bool FaceCamIsRecordingPermissionGranted()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamIsRecordingPermissionGranted();
		}
		return false;
	}

	public static bool FaceCamIsSessionRunning()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamIsSessionRunning();
		}
		return false;
	}

	public static bool FaceCamIsVideoRecordingSupported()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayFaceCamIsVideoRecordingSupported();
		}
		return false;
	}

	public static void FaceCamRequestRecordingPermission()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamRequestRecordingPermission();
		}
	}

	public static void FaceCamSetAudioOnly(bool audioOnly)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetAudioOnly(audioOnly);
		}
	}

	public static void FaceCamSetMonitorAudioLevels(bool enabled)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetMonitorAudioLevels(enabled);
		}
	}

	public static void FaceCamSetPreviewBorderColor(float r, float g, float b, float a)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewBorderColor(r, g, b, a);
		}
	}

	public static void FaceCamSetPreviewBorderWidth(int width)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewBorderWidth(width);
		}
	}

	public static void FaceCamSetPreviewOrigin(Everyplay.FaceCamPreviewOrigin origin)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewOrigin((int)origin);
		}
	}

	public static void FaceCamSetPreviewPositionX(int x)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewPositionX(x);
		}
	}

	public static void FaceCamSetPreviewPositionY(int y)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewPositionY(y);
		}
	}

	public static void FaceCamSetPreviewScaleRetina(bool autoScale)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewScaleRetina(autoScale);
		}
	}

	public static void FaceCamSetPreviewSideWidth(int width)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewSideWidth(width);
		}
	}

	public static void FaceCamSetPreviewVisible(bool visible)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetPreviewVisible(visible);
		}
	}

	public static void FaceCamSetRecordingMode(Everyplay.FaceCamRecordingMode mode)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetRecordingMode((int)mode);
		}
	}

	public static void FaceCamSetTargetTexture(Texture2D texture)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			if (texture == null)
			{
				Everyplay.EveryplayFaceCamSetTargetTextureId(0);
			}
			else
			{
				Everyplay.EveryplayFaceCamSetTargetTextureId(texture.GetNativeTextureID());
				Everyplay.EveryplayFaceCamSetTargetTextureWidth(texture.width);
				Everyplay.EveryplayFaceCamSetTargetTextureHeight(texture.height);
			}
		}
	}

	[Obsolete("Defining texture height is no longer required when FaceCamSetTargetTexture(Texture2D texture) is used.")]
	public static void FaceCamSetTargetTextureHeight(int textureHeight)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetTargetTextureHeight(textureHeight);
		}
	}

	[Obsolete("Use FaceCamSetTargetTexture(Texture2D texture) instead.")]
	public static void FaceCamSetTargetTextureId(int textureId)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetTargetTextureId(textureId);
		}
	}

	[Obsolete("Defining texture width is no longer required when FaceCamSetTargetTexture(Texture2D texture) is used.")]
	public static void FaceCamSetTargetTextureWidth(int textureWidth)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamSetTargetTextureWidth(textureWidth);
		}
	}

	public static void FaceCamStartSession()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamStartSession();
		}
	}

	public static void FaceCamStopSession()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayFaceCamStopSession();
		}
	}

	public static int GetUserInterfaceIdiom()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayGetUserInterfaceIdiom();
		}
		return 0;
	}

	public static void InitEveryplay(string clientId, string clientSecret, string redirectURI)
	{
		AndroidJavaObject @static = (new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
		Everyplay.everyplayUnity = new AndroidJavaObject("com.everyplay.Everyplay.unity.EveryplayUnity3DWrapper", new object[0]);
		Everyplay.everyplayUnity.Call("initEveryplay", new object[] { @static, clientId, clientSecret, redirectURI });
	}

	public static void Initialize()
	{
		if (Everyplay.EveryplayInstance == null)
		{
			UnityEngine.Debug.Log("Unable to initialize Everyplay. Everyplay might be disabled for this platform or the app is closing.");
		}
	}

	public static bool IsPaused()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayIsPaused();
		}
		return false;
	}

	public static bool IsReadyForRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.readyForRecording;
		}
		return false;
	}

	public static bool IsRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayIsRecording();
		}
		return false;
	}

	public static bool IsRecordingSupported()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayIsRecordingSupported();
		}
		return false;
	}

	public static bool IsSingleCoreDevice()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayIsSingleCoreDevice();
		}
		return false;
	}

	public static bool IsSupported()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplayIsSupported();
		}
		return false;
	}

	public static void MakeRequest(string method, string url, Dictionary<string, object> data, Everyplay.RequestReadyDelegate readyDelegate, Everyplay.RequestFailedDelegate failedDelegate)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayInstance.AsyncMakeRequest(method, url, data, readyDelegate, failedDelegate);
		}
	}

	[DebuggerHidden]
	private IEnumerator MakeRequestEnumerator(string method, string url, Dictionary<string, object> data, Everyplay.RequestReadyDelegate readyDelegate, Everyplay.RequestFailedDelegate failedDelegate)
	{
		Everyplay.u003cMakeRequestEnumeratoru003ec__Iterator6 variable = null;
		return variable;
	}

	private void OnApplicationQuit()
	{
		Everyplay.Reset();
		if (Everyplay.currentThumbnailTargetTexture != null)
		{
			Everyplay.SetThumbnailTargetTexture(null);
			Everyplay.currentThumbnailTargetTexture = null;
		}
		Everyplay.RemoveAllEventHandlers();
		Everyplay.appIsClosing = true;
		Everyplay.everyplayInstance = null;
	}

	public static void PauseRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayPauseRecording();
		}
	}

	public static void PlayLastRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayPlayLastRecording();
		}
	}

	public static void PlayVideoWithDictionary(Dictionary<string, object> dict)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayPlayVideoWithDictionary(Json.Serialize(dict));
		}
	}

	public static void PlayVideoWithURL(string url)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayPlayVideoWithURL(url);
		}
	}

	private static void RemoveAllEventHandlers()
	{
		Everyplay.WasClosed = null;
		Everyplay.ReadyForRecording = null;
		Everyplay.RecordingStarted = null;
		Everyplay.RecordingStopped = null;
		Everyplay.FaceCamSessionStarted = null;
		Everyplay.FaceCamRecordingPermission = null;
		Everyplay.FaceCamSessionStopped = null;
		Everyplay.ThumbnailReadyAtTextureId = null;
		Everyplay.ThumbnailTextureReady = null;
		Everyplay.UploadDidStart = null;
		Everyplay.UploadDidProgress = null;
		Everyplay.UploadDidComplete = null;
	}

	private static void Reset()
	{
	}

	public static void ResumeRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayResumeRecording();
		}
	}

	public static void SetDisableSingleCoreDevices(bool state)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetDisableSingleCoreDevices(state);
		}
	}

	public static void SetLowMemoryDevice(bool state)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetLowMemoryDevice(state);
		}
	}

	public static void SetMaxRecordingMinutesLength(int minutes)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetMaxRecordingMinutesLength(minutes);
		}
	}

	public static void SetMetadata(string key, object val)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods && key != null && val != null)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ key, val }
			};
			Everyplay.EveryplaySetMetadata(Json.Serialize(strs));
		}
	}

	public static void SetMetadata(Dictionary<string, object> dict)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods && dict != null && dict.Count > 0)
		{
			Everyplay.EveryplaySetMetadata(Json.Serialize(dict));
		}
	}

	public static void SetMotionFactor(int factor)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetMotionFactor(factor);
		}
	}

	public static void SetTargetFPS(int fps)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetTargetFPS(fps);
		}
	}

	public static void SetThumbnailTargetTexture(Texture2D texture)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.currentThumbnailTargetTexture = texture;
			if (texture == null)
			{
				Everyplay.EveryplaySetThumbnailTargetTextureId(0);
			}
			else
			{
				Everyplay.EveryplaySetThumbnailTargetTextureId(Everyplay.currentThumbnailTargetTexture.GetNativeTextureID());
				Everyplay.EveryplaySetThumbnailTargetTextureWidth(Everyplay.currentThumbnailTargetTexture.width);
				Everyplay.EveryplaySetThumbnailTargetTextureHeight(Everyplay.currentThumbnailTargetTexture.height);
			}
		}
	}

	[Obsolete("Defining texture height is no longer required when SetThumbnailTargetTexture(Texture2D texture) is used.")]
	public static void SetThumbnailTargetTextureHeight(int textureHeight)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetThumbnailTargetTextureHeight(textureHeight);
		}
	}

	[Obsolete("Use SetThumbnailTargetTexture(Texture2D texture) instead.")]
	public static void SetThumbnailTargetTextureId(int textureId)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetThumbnailTargetTextureId(textureId);
		}
	}

	[Obsolete("Defining texture width is no longer required when SetThumbnailTargetTexture(Texture2D texture) is used.")]
	public static void SetThumbnailTargetTextureWidth(int textureWidth)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplaySetThumbnailTargetTextureWidth(textureWidth);
		}
	}

	public static void Show()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayShow();
		}
	}

	public static void ShowSharingModal()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayShowSharingModal();
		}
	}

	public static void ShowWithPath(string path)
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayShowWithPath(path);
		}
	}

	public static bool SnapshotRenderbuffer()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			return Everyplay.EveryplaySnapshotRenderbuffer();
		}
		return false;
	}

	public static void StartRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayStartRecording();
		}
	}

	public static void StopRecording()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayStopRecording();
		}
	}

	public static void TakeThumbnail()
	{
		if (Everyplay.EveryplayInstance != null && Everyplay.hasMethods)
		{
			Everyplay.EveryplayTakeThumbnail();
		}
	}

	public static event Everyplay.FaceCamRecordingPermissionDelegate FaceCamRecordingPermission;

	public static event Everyplay.FaceCamSessionStartedDelegate FaceCamSessionStarted;

	public static event Everyplay.FaceCamSessionStoppedDelegate FaceCamSessionStopped;

	public static event Everyplay.ReadyForRecordingDelegate ReadyForRecording;

	public static event Everyplay.RecordingStartedDelegate RecordingStarted;

	public static event Everyplay.RecordingStoppedDelegate RecordingStopped;

	[Obsolete("Use ThumbnailTextureReady instead.")]
	public static event Everyplay.ThumbnailReadyAtTextureIdDelegate ThumbnailReadyAtTextureId;

	public static event Everyplay.ThumbnailTextureReadyDelegate ThumbnailTextureReady;

	public static event Everyplay.UploadDidCompleteDelegate UploadDidComplete;

	public static event Everyplay.UploadDidProgressDelegate UploadDidProgress;

	public static event Everyplay.UploadDidStartDelegate UploadDidStart;

	public static event Everyplay.WasClosedDelegate WasClosed;

	public enum FaceCamPreviewOrigin
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	public enum FaceCamRecordingMode
	{
		RecordAudio,
		RecordVideo,
		PassThrough
	}

	public delegate void FaceCamRecordingPermissionDelegate(bool granted);

	public delegate void FaceCamSessionStartedDelegate();

	public delegate void FaceCamSessionStoppedDelegate();

	public delegate void ReadyForRecordingDelegate(bool enabled);

	public delegate void RecordingStartedDelegate();

	public delegate void RecordingStoppedDelegate();

	public delegate void RequestFailedDelegate(string error);

	public delegate void RequestReadyDelegate(string response);

	[Obsolete("Use ThumbnailTextureReadyDelegate(Texture2D texture,bool portrait) instead.")]
	public delegate void ThumbnailReadyAtTextureIdDelegate(int textureId, bool portrait);

	public delegate void ThumbnailTextureReadyDelegate(Texture2D texture, bool portrait);

	public delegate void UploadDidCompleteDelegate(int videoId);

	public delegate void UploadDidProgressDelegate(int videoId, float progress);

	public delegate void UploadDidStartDelegate(int videoId);

	public enum UserInterfaceIdiom
	{
		iPhone = 0,
		Phone = 0,
		iPad = 1,
		Tablet = 1,
		TV = 2
	}

	public delegate void WasClosedDelegate();
}