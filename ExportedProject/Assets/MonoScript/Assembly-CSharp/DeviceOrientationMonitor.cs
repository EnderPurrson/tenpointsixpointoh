using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeviceOrientationMonitor : MonoBehaviour
{
	public static float CheckDelay = 0.5f;

	[CompilerGenerated]
	private static Action<DeviceOrientation> _003C_003Ef__am_0024cache3;

	public static DeviceOrientation CurrentOrientation { get; private set; }

	public static event Action<DeviceOrientation> OnOrientationChange;

	static DeviceOrientationMonitor()
	{
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = _003COnOrientationChange_003Em__289;
		}
		DeviceOrientationMonitor.OnOrientationChange = _003C_003Ef__am_0024cache3;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		StartCoroutine(CheckForChange());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator CheckForChange()
	{
		CurrentOrientation = Input.deviceOrientation;
		while (true)
		{
			DeviceOrientation deviceOrientation = Input.deviceOrientation;
			if ((deviceOrientation == DeviceOrientation.LandscapeLeft || deviceOrientation == DeviceOrientation.LandscapeRight) && CurrentOrientation != Input.deviceOrientation)
			{
				CurrentOrientation = Input.deviceOrientation;
				DeviceOrientationMonitor.OnOrientationChange(CurrentOrientation);
			}
			yield return new WaitForSeconds(CheckDelay);
		}
	}

	[CompilerGenerated]
	private static void _003COnOrientationChange_003Em__289(DeviceOrientation o)
	{
	}
}
