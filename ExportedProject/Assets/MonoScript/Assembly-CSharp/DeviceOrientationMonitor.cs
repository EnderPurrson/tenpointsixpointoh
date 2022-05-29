using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeviceOrientationMonitor : MonoBehaviour
{
	public static float CheckDelay;

	public static DeviceOrientation CurrentOrientation
	{
		get;
		private set;
	}

	static DeviceOrientationMonitor()
	{
		DeviceOrientationMonitor.CheckDelay = 0.5f;
		DeviceOrientationMonitor.OnOrientationChange = (DeviceOrientation o) => {
		};
	}

	public DeviceOrientationMonitor()
	{
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	[DebuggerHidden]
	private IEnumerator CheckForChange()
	{
		return new DeviceOrientationMonitor.u003cCheckForChangeu003ec__Iterator131();
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	private void OnEnable()
	{
		base.StartCoroutine(this.CheckForChange());
	}

	public static event Action<DeviceOrientation> OnOrientationChange;
}