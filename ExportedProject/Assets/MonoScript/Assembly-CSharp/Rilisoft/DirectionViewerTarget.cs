using System;
using System.Collections;
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

		public DirectionViewTargetType Type
		{
			get
			{
				return _Type;
			}
		}

		public Transform Transform
		{
			get
			{
				return base.gameObject.transform;
			}
		}

		private void OnEnable()
		{
			_rocketComponent = base.gameObject.GetComponentInParent<Rocket>();
			if (_rocketComponent == null)
			{
				throw new Exception("rocket component not found");
			}
			StartCoroutine(RocketMonitorCoroutine());
		}

		private void OnDisable()
		{
			HidePointer();
		}

		private IEnumerator RocketMonitorCoroutine()
		{
			while (!_rocketComponent.isRun)
			{
				yield return null;
			}
			ShowPointer();
			while (_rocketComponent.isRun)
			{
				yield return null;
			}
			HidePointer();
		}

		private void ShowPointer()
		{
			if (DirectionViewer.Instance != null)
			{
				DirectionViewer.Instance.LookToMe(this);
			}
		}

		private void HidePointer()
		{
			if (DirectionViewer.Instance != null)
			{
				DirectionViewer.Instance.ForgetMe(this);
			}
		}
	}
}
