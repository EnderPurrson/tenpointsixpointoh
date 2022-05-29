using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class TrafficForwardingScript : MonoBehaviour
{
	public EventHandler<TrafficForwardingInfo> Updated;

	private float _trafficForwardingConfigTimestamp;

	private TaskCompletionSource<TrafficForwardingInfo> _trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();

	public TrafficForwardingScript()
	{
	}

	[DebuggerHidden]
	internal IEnumerator GetTrafficForwardingConfigCoroutine()
	{
		TrafficForwardingScript.u003cGetTrafficForwardingConfigCoroutineu003ec__Iterator198 variable = null;
		return variable;
	}

	[DebuggerHidden]
	internal IEnumerator GetTrafficForwardingConfigLoopCoroutine()
	{
		TrafficForwardingScript.u003cGetTrafficForwardingConfigLoopCoroutineu003ec__Iterator197 variable = null;
		return variable;
	}

	internal Task<TrafficForwardingInfo> GetTrafficForwardingInfo()
	{
		return this._trafficForwardingPromise.Task;
	}
}