using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class TrafficForwardingScript : MonoBehaviour
{
	public EventHandler<TrafficForwardingInfo> Updated;

	private float _trafficForwardingConfigTimestamp;

	private TaskCompletionSource<TrafficForwardingInfo> _trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();

	internal IEnumerator GetTrafficForwardingConfigLoopCoroutine()
	{
		yield return StartCoroutine(GetTrafficForwardingConfigCoroutine());
		float firstDelaySeconds = Math.Max(60f, Defs.timeUpdatePixelbookInfo - 60f);
		yield return new WaitForSeconds(firstDelaySeconds);
		yield return StartCoroutine(GetTrafficForwardingConfigCoroutine());
		float delaySeconds = Defs.timeUpdatePixelbookInfo;
		while (true)
		{
			if (Time.realtimeSinceStartup - _trafficForwardingConfigTimestamp < delaySeconds)
			{
				yield return null;
			}
			else
			{
				yield return StartCoroutine(GetTrafficForwardingConfigCoroutine());
			}
		}
	}

	internal IEnumerator GetTrafficForwardingConfigCoroutine()
	{
		_trafficForwardingConfigTimestamp = Time.realtimeSinceStartup;
		if (_trafficForwardingPromise.Task.IsCompleted)
		{
			_trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.TrafficForwardingConfigUrl);
		yield return response;
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			Updated.Do(((_003CGetTrafficForwardingConfigCoroutine_003Ec__Iterator198)(object)this)._003C_003Em__464);
			string message = ((response != null) ? response.error : "null");
			_trafficForwardingPromise.TrySetException(new InvalidOperationException(message));
			yield break;
		}
		string responseText = URLs.Sanitize(response);
		Dictionary<string, object> responseDict = Json.Deserialize(responseText) as Dictionary<string, object>;
		object trafficForwardingObject;
		if (responseDict == null)
		{
			Updated.Do(((_003CGetTrafficForwardingConfigCoroutine_003Ec__Iterator198)(object)this)._003C_003Em__465);
			_trafficForwardingPromise.TrySetException(new InvalidOperationException("Couldnot deserialize response: " + responseText));
		}
		else if (responseDict.TryGetValue("trafficForwarding_v_10.2.0", out trafficForwardingObject))
		{
			Dictionary<string, object> trafficForwarding = trafficForwardingObject as Dictionary<string, object>;
			if (trafficForwarding != null)
			{
				object urlObject;
				if (trafficForwarding.TryGetValue("url", out urlObject))
				{
					string url2 = Convert.ToString(urlObject);
					url2 = url2 + "&uid=" + FriendsController.sharedController.id + "&device=" + SystemInfo.deviceUniqueIdentifier;
					int minLevel = 0;
					try
					{
						minLevel = Convert.ToInt32(trafficForwarding["minLevel"]);
					}
					catch (Exception ex3)
					{
						Exception ex2 = ex3;
						Debug.LogWarning(ex2.ToString());
					}
					int maxLevel = ExperienceController.maxLevel;
					try
					{
						maxLevel = Convert.ToInt32(trafficForwarding["maxLevel"]);
					}
					catch (Exception ex)
					{
						Debug.LogWarning(ex.ToString());
					}
					TrafficForwardingInfo result = new TrafficForwardingInfo(url2, minLevel, maxLevel);
					Updated.Do(((_003CGetTrafficForwardingConfigCoroutine_003Ec__Iterator198)(object)this)._003C_003Em__466);
					_trafficForwardingPromise.TrySetResult(result);
				}
				else
				{
					Updated.Do(((_003CGetTrafficForwardingConfigCoroutine_003Ec__Iterator198)(object)this)._003C_003Em__467);
					_trafficForwardingPromise.TrySetResult(TrafficForwardingInfo.DisabledInstance);
				}
			}
			else
			{
				Updated.Do(((_003CGetTrafficForwardingConfigCoroutine_003Ec__Iterator198)(object)this)._003C_003Em__468);
				_trafficForwardingPromise.TrySetException(new InvalidOperationException("Couldnot deserialize trafficForwarding node: " + Json.Serialize(trafficForwardingObject)));
			}
		}
		else
		{
			Updated.Do(((_003CGetTrafficForwardingConfigCoroutine_003Ec__Iterator198)(object)this)._003C_003Em__469);
			_trafficForwardingPromise.TrySetException(new InvalidOperationException("Response doesn't contain trafficForwarding node."));
		}
	}

	internal Task<TrafficForwardingInfo> GetTrafficForwardingInfo()
	{
		return _trafficForwardingPromise.Task;
	}
}
