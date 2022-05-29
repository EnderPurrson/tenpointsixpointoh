using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class DirectionViewerTarget : MonoBehaviour
	{
		[SerializeField]
		private DirectionViewTargetType _Type;

		[ReadOnly]
		[SerializeField]
		private Rocket _rocketComponent;

		public UnityEngine.Transform Transform
		{
			get
			{
				return base.gameObject.transform;
			}
		}

		public DirectionViewTargetType Type
		{
			get
			{
				return this._Type;
			}
		}

		public DirectionViewerTarget()
		{
		}

		private void HidePointer()
		{
			if (DirectionViewer.Instance != null)
			{
				DirectionViewer.Instance.ForgetMe(this);
			}
		}

		private void OnDisable()
		{
			this.HidePointer();
		}

		private void OnEnable()
		{
			this._rocketComponent = base.gameObject.GetComponentInParent<Rocket>();
			if (this._rocketComponent == null)
			{
				throw new Exception("rocket component not found");
			}
			base.StartCoroutine(this.RocketMonitorCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator RocketMonitorCoroutine()
		{
			DirectionViewerTarget.u003cRocketMonitorCoroutineu003ec__Iterator80 variable = null;
			return variable;
		}

		private void ShowPointer()
		{
			if (DirectionViewer.Instance != null)
			{
				DirectionViewer.Instance.LookToMe(this);
			}
		}
	}
}