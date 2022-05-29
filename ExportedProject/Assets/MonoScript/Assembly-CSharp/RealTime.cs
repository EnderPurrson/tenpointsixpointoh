using System;
using UnityEngine;

public class RealTime : MonoBehaviour
{
	public static float deltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}

	public static float time
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	public RealTime()
	{
	}
}