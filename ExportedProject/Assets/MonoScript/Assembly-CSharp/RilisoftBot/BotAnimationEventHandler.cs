using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RilisoftBot
{
	public class BotAnimationEventHandler : MonoBehaviour
	{
		private BotAnimationEventHandler.OnDamageEventDelegate OnDamageEvent;

		public BotAnimationEventHandler()
		{
		}

		private void OnApplyShootEffect()
		{
			if (this.OnDamageEvent == null)
			{
				return;
			}
			this.OnDamageEvent();
		}

		public event BotAnimationEventHandler.OnDamageEventDelegate OnDamageEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.OnDamageEvent += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.OnDamageEvent -= value;
			}
		}

		public delegate void OnDamageEventDelegate();
	}
}