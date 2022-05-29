using System;
using UnityEngine;

public class NoodlePermissionGranter : MonoBehaviour
{
	private const string WRITE_EXTERNAL_STORAGE = "WRITE_EXTERNAL_STORAGE";

	private const string PERMISSION_GRANTED = "PERMISSION_GRANTED";

	private const string PERMISSION_DENIED = "PERMISSION_DENIED";

	private const string NOODLE_PERMISSION_GRANTER = "NoodlePermissionGranter";

	public static Action<bool> PermissionRequestCallback;

	public static EventHandler<EventArgs> PermissionRequestFinished;

	private static NoodlePermissionGranter instance;

	private static bool initialized;

	private static AndroidJavaClass noodlePermissionGranterClass;

	private static AndroidJavaObject activity;

	static NoodlePermissionGranter()
	{
	}

	public NoodlePermissionGranter()
	{
	}

	public void Awake()
	{
		NoodlePermissionGranter.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (base.name != "NoodlePermissionGranter")
		{
			base.name = "NoodlePermissionGranter";
		}
	}

	public static void GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission permission)
	{
		if (!NoodlePermissionGranter.initialized)
		{
			NoodlePermissionGranter.initialize();
		}
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		NoodlePermissionGranter.noodlePermissionGranterClass.CallStatic("grantPermission", new object[] { NoodlePermissionGranter.activity, (int)permission });
	}

	private static void initialize()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (NoodlePermissionGranter.instance == null)
		{
			GameObject gameObject = new GameObject();
			NoodlePermissionGranter.instance = gameObject.AddComponent<NoodlePermissionGranter>();
			gameObject.name = "NoodlePermissionGranter";
		}
		NoodlePermissionGranter.noodlePermissionGranterClass = new AndroidJavaClass("com.noodlecake.unityplugins.NoodlePermissionGranter");
		NoodlePermissionGranter.activity = (new AndroidJavaClass("com.unity3d.player.UnityPlayer")).GetStatic<AndroidJavaObject>("currentActivity");
		NoodlePermissionGranter.initialized = true;
	}

	private void permissionRequestCallbackInternal(string message)
	{
		bool flag = message == "PERMISSION_GRANTED";
		if (NoodlePermissionGranter.PermissionRequestCallback != null)
		{
			NoodlePermissionGranter.PermissionRequestCallback(flag);
		}
	}

	public enum NoodleAndroidPermission
	{
		WRITE_EXTERNAL_STORAGE,
		ACCESS_COARSE_LOCATION,
		CAMERA,
		RECORD_AUDIO
	}
}