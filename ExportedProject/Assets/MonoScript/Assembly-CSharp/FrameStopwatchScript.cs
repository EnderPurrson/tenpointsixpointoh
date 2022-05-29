using System;
using System.Diagnostics;
using UnityEngine;

internal sealed class FrameStopwatchScript : MonoBehaviour
{
	private readonly Stopwatch _stopwatch = new Stopwatch();

	public FrameStopwatchScript()
	{
	}

	public float GetSecondsSinceFrameStarted()
	{
		return (float)this._stopwatch.ElapsedMilliseconds / 1000f;
	}

	internal void Start()
	{
		this._stopwatch.Start();
	}

	internal void Update()
	{
		this._stopwatch.Reset();
		this._stopwatch.Start();
	}
}