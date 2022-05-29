using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class LogHandler : MonoBehaviour
{
	private bool _cancelled;

	private bool _registered;

	private string _logString = string.Empty;

	private string _stackTrace = string.Empty;

	public LogHandler()
	{
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			this._logString = logString;
			this._stackTrace = stackTrace;
		}
	}

	private void OnDisable()
	{
		this._cancelled = true;
		if (this._registered)
		{
			Application.RegisterLogCallback(null);
		}
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.RegisterLogCallbackCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator RegisterLogCallbackCoroutine()
	{
		LogHandler.u003cRegisterLogCallbackCoroutineu003ec__Iterator16D variable = null;
		return variable;
	}

	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}