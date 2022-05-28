using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

namespace RilisoftBot
{
	public class BotHpIndicator : MonoBehaviour
	{
		internal class HealthProvider
		{
			private readonly Func<float> _healthGetter;

			private readonly Func<float> _baseHealthGetter;

			[CompilerGenerated]
			private static Func<float> _003C_003Ef__am_0024cache2;

			[CompilerGenerated]
			private static Func<float> _003C_003Ef__am_0024cache3;

			public float Health
			{
				get
				{
					return _healthGetter();
				}
			}

			public float BaseHealth
			{
				get
				{
					return _baseHealthGetter();
				}
			}

			public HealthProvider(Func<float> healthGetter, Func<float> baseHealthGetter)
			{
				Func<float> func = healthGetter;
				if (func == null)
				{
					if (_003C_003Ef__am_0024cache2 == null)
					{
						_003C_003Ef__am_0024cache2 = _003CHealthProvider_003Em__261;
					}
					func = _003C_003Ef__am_0024cache2;
				}
				_healthGetter = func;
				Func<float> func2 = baseHealthGetter;
				if (func2 == null)
				{
					if (_003C_003Ef__am_0024cache3 == null)
					{
						_003C_003Ef__am_0024cache3 = _003CHealthProvider_003Em__262;
					}
					func2 = _003C_003Ef__am_0024cache3;
				}
				_baseHealthGetter = func2;
			}

			[CompilerGenerated]
			private static float _003CHealthProvider_003Em__261()
			{
				return 0f;
			}

			[CompilerGenerated]
			private static float _003CHealthProvider_003Em__262()
			{
				return 0f;
			}
		}

		[SerializeField]
		private GameObject _frame;

		private float _currShowTime;

		[SerializeField]
		private Transform _healthBar;

		[ReadOnly]
		[SerializeField]
		private float _currentScale;

		private float _prevScale = 1f;

		private HealthProvider _hp;

		private IEnumerator Start()
		{
			_frame.SetActive(false);
			yield return new WaitForSeconds(0.2f);
			yield return WaitHpOwner();
			yield return UpdateIndicator();
		}

		private IEnumerator UpdateIndicator()
		{
			while (true)
			{
				if (_hp == null || _healthBar == null || Math.Abs(_hp.BaseHealth) < 0.0001f)
				{
					yield return null;
				}
				if (_hp.Health <= 0f && _frame.activeInHierarchy)
				{
					_frame.SetActive(false);
					yield return null;
				}
				_currentScale = _hp.Health / _hp.BaseHealth;
				if (Math.Abs(_currentScale - _prevScale) > 0.0001f)
				{
					_frame.SetActive(true);
					_currShowTime = 2f;
					_healthBar.localScale = new Vector3(_currentScale, _healthBar.localScale.y, _healthBar.localScale.z);
				}
				_prevScale = _currentScale;
				if (_currShowTime > 0f)
				{
					_currShowTime -= Time.deltaTime;
				}
				else
				{
					_currShowTime = 0f;
					_frame.SetActive(false);
				}
				yield return null;
			}
		}

		private IEnumerator WaitHpOwner()
		{
			bool setted = false;
			while (!setted)
			{
				foreach (GameObject pr2 in base.gameObject.AncestorsAndSelf())
				{
					BaseBot bot = pr2.GetComponent<BaseBot>();
					if (bot != null)
					{
						_hp = new HealthProvider(((_003CWaitHpOwner_003Ec__Iterator119)(object)this)._003C_003Em__25D, ((_003CWaitHpOwner_003Ec__Iterator119)(object)this)._003C_003Em__25E);
						setted = true;
						break;
					}
				}
				foreach (GameObject pr in base.gameObject.AncestorsAndSelf())
				{
					TrainingEnemy dummy = pr.GetComponent<TrainingEnemy>();
					if (dummy != null)
					{
						_hp = new HealthProvider(((_003CWaitHpOwner_003Ec__Iterator119)(object)this)._003C_003Em__25F, ((_003CWaitHpOwner_003Ec__Iterator119)(object)this)._003C_003Em__260);
						setted = true;
						break;
					}
				}
				yield return null;
			}
		}
	}
}
