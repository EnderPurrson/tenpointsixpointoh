using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Linq;
using UnityEngine;

namespace RilisoftBot
{
	public class BotHpIndicator : MonoBehaviour
	{
		[SerializeField]
		private GameObject _frame;

		private float _currShowTime;

		[SerializeField]
		private Transform _healthBar;

		[ReadOnly]
		[SerializeField]
		private float _currentScale;

		private float _prevScale = 1f;

		private BotHpIndicator.HealthProvider _hp;

		public BotHpIndicator()
		{
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			BotHpIndicator.u003cStartu003ec__Iterator117 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator UpdateIndicator()
		{
			BotHpIndicator.u003cUpdateIndicatoru003ec__Iterator118 variable = null;
			return variable;
		}

		[DebuggerHidden]
		private IEnumerator WaitHpOwner()
		{
			BotHpIndicator.u003cWaitHpOwneru003ec__Iterator119 variable = null;
			return variable;
		}

		internal class HealthProvider
		{
			private readonly Func<float> _healthGetter;

			private readonly Func<float> _baseHealthGetter;

			public float BaseHealth
			{
				get
				{
					return this._baseHealthGetter();
				}
			}

			public float Health
			{
				get
				{
					return this._healthGetter();
				}
			}

			public HealthProvider(Func<float> healthGetter, Func<float> baseHealthGetter)
			{
				this._healthGetter = healthGetter ?? new Func<float>(() => 0f);
				this._baseHealthGetter = baseHealthGetter ?? new Func<float>(() => 0f);
			}
		}
	}
}