using System;
using System.Threading;
using UnityEngine;

namespace Prime31
{
	public class LifecycleHelper : MonoBehaviour
	{
		public event Action<bool> onApplicationPausedEvent
		{
			add
			{
				Action<bool> val = this.onApplicationPausedEvent;
				Action<bool> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<bool>>(ref this.onApplicationPausedEvent, (Action<bool>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while (val != val2);
			}
			remove
			{
				Action<bool> val = this.onApplicationPausedEvent;
				Action<bool> val2;
				do
				{
					val2 = val;
					val = Interlocked.CompareExchange<Action<bool>>(ref this.onApplicationPausedEvent, (Action<bool>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value), val);
				}
				while (val != val2);
			}
		}

		private void OnApplicationPause(bool paused)
		{
			if (this.onApplicationPausedEvent != null)
			{
				this.onApplicationPausedEvent.Invoke(paused);
			}
		}
	}
}
