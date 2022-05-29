using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HockeyAppIOS : MonoBehaviour
{
	protected const string HOCKEYAPP_BASEURL = "https://rink.hockeyapp.net/";

	protected const string HOCKEYAPP_CRASHESPATH = "api/2/apps/[APPID]/crashes/upload";

	protected const string LOG_FILE_DIR = "/logs/";

	protected const int MAX_CHARS = 199800;

	[Header("HockeyApp Setup")]
	public string appID = "your-hockey-app-id";

	public string serverURL = "your-custom-server-url";

	[Header("Authentication")]
	public HockeyAppIOS.AuthenticatorType authenticatorType;

	public string secret = "your-hockey-app-secret";

	[Header("Crashes & Exceptions")]
	public bool autoUploadCrashes;

	public bool exceptionLogging = true;

	[Header("Metrics")]
	public bool userMetrics = true;

	[Header("Version Updates")]
	public bool updateAlert = true;

	public HockeyAppIOS()
	{
	}

	private void Awake()
	{
	}

	protected virtual WWWForm CreateForm(string log)
	{
		return new WWWForm();
	}

	private void GameViewLoaded(string message)
	{
	}

	protected virtual string GetAuthenticatorTypeString()
	{
		return string.Empty;
	}

	protected virtual string GetBaseURL()
	{
		return string.Empty;
	}

	protected virtual List<string> GetLogFiles()
	{
		return new List<string>();
	}

	protected virtual List<string> GetLogHeaders()
	{
		return new List<string>();
	}

	protected virtual void HandleException(string logString, string stackTrace)
	{
	}

	protected virtual bool IsConnected()
	{
		return false;
	}

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
	}

	public void OnHandleLogCallback(string logString, string stackTrace, LogType type)
	{
	}

	[DebuggerHidden]
	protected virtual IEnumerator SendLogs(List<string> logs)
	{
		HockeyAppIOS.u003cSendLogsu003ec__IteratorE9 variable = null;
		return variable;
	}

	protected virtual void WriteLogToDisk(string logString, string stackTrace)
	{
	}

	public enum AuthenticatorType
	{
		Anonymous,
		Device,
		HockeyAppUser,
		HockeyAppEmail,
		WebAuth
	}
}