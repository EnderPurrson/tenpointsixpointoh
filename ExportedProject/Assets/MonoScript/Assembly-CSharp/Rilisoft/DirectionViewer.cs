using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class DirectionViewer : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CLookToMe_003Ec__AnonStorey276
		{
			internal DirectionViewerTarget target;

			internal bool _003C_003Em__175(DirectionPointer p)
			{
				return p.Target == target;
			}
		}

		[CompilerGenerated]
		private sealed class _003CForgetMe_003Ec__AnonStorey277
		{
			internal DirectionViewerTarget target;

			internal bool _003C_003Em__176(DirectionPointer p)
			{
				return p.Target == target;
			}
		}

		[SerializeField]
		private UIPanel _panel;

		[SerializeField]
		private float _lookRadius = 50f;

		[SerializeField]
		private float _radius = 200f;

		private Dictionary<DirectionViewTargetType, Queue<DirectionPointer>> _freePointers = new Dictionary<DirectionViewTargetType, Queue<DirectionPointer>>();

		private List<DirectionPointer> _activePointers = new List<DirectionPointer>();

		public static DirectionViewer Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				return;
			}
			Instance = this;
			_activePointers.Clear();
			_freePointers.Clear();
			List<DirectionPointer> list = GetComponentsInChildren<DirectionPointer>(true).ToList();
			foreach (DirectionPointer item in list)
			{
				if (_freePointers.ContainsKey(item.ForPointerType))
				{
					_freePointers[item.ForPointerType].Enqueue(item);
					continue;
				}
				Queue<DirectionPointer> queue = new Queue<DirectionPointer>();
				queue.Enqueue(item);
				_freePointers.Add(item.ForPointerType, queue);
			}
		}

		private void OnDisable()
		{
			_activePointers.ForEach(_003COnDisable_003Em__174);
		}

		private void LateUpdate()
		{
			if (!(WeaponManager.sharedManager == null))
			{
				_activePointers.ForEach(SetPointerState);
			}
		}

		public void LookToMe(DirectionViewerTarget target)
		{
			_003CLookToMe_003Ec__AnonStorey276 _003CLookToMe_003Ec__AnonStorey = new _003CLookToMe_003Ec__AnonStorey276();
			_003CLookToMe_003Ec__AnonStorey.target = target;
			if (!(_003CLookToMe_003Ec__AnonStorey.target == null) && !_activePointers.Any(_003CLookToMe_003Ec__AnonStorey._003C_003Em__175) && _freePointers.ContainsKey(_003CLookToMe_003Ec__AnonStorey.target.Type) && _freePointers[_003CLookToMe_003Ec__AnonStorey.target.Type].Any())
			{
				DirectionPointer directionPointer = _freePointers[_003CLookToMe_003Ec__AnonStorey.target.Type].Dequeue();
				_activePointers.Add(directionPointer);
				directionPointer.TurnOn(_003CLookToMe_003Ec__AnonStorey.target);
			}
		}

		public void ForgetMe(DirectionViewerTarget target)
		{
			_003CForgetMe_003Ec__AnonStorey277 _003CForgetMe_003Ec__AnonStorey = new _003CForgetMe_003Ec__AnonStorey277();
			_003CForgetMe_003Ec__AnonStorey.target = target;
			DirectionPointer directionPointer = _activePointers.FirstOrDefault(_003CForgetMe_003Ec__AnonStorey._003C_003Em__176);
			if (!(directionPointer == null))
			{
				directionPointer.TurnOff();
				_activePointers.Remove(directionPointer);
				_freePointers[directionPointer.ForPointerType].Enqueue(directionPointer);
			}
		}

		private void ForgetPointer(DirectionPointer pointer)
		{
			pointer.TurnOff();
			if (_activePointers.Contains(pointer))
			{
				_activePointers.Remove(pointer);
				_freePointers[pointer.ForPointerType].Enqueue(pointer);
			}
		}

		private bool CheckDistance(DirectionViewerTarget poiner)
		{
			if (WeaponManager.sharedManager == null || poiner == null)
			{
				return false;
			}
			return (WeaponManager.sharedManager.myPlayer.transform.position - poiner.Transform.position).sqrMagnitude < Mathf.Pow(_lookRadius, 2f);
		}

		private void SetPointerState(DirectionPointer pointer)
		{
			if (!CheckDistance(pointer.Target))
			{
				pointer.OutOfRange = true;
				pointer.Hide();
				return;
			}
			if (pointer.OutOfRange)
			{
				pointer.OutOfRange = false;
				pointer.TurnOn(pointer.Target);
			}
			float angle = GetAngle(NickLabelController.currentCamera.transform, pointer.Target.transform.position, Vector3.up);
			Vector3 localPosition = new Vector3(_radius * Mathf.Sin(angle * ((float)Math.PI / 180f)), _radius * Mathf.Cos(angle * ((float)Math.PI / 180f)), pointer.transform.position.z);
			pointer.transform.localPosition = localPosition;
		}

		private float GetAngle(Transform from, Vector3 target, Vector3 n)
		{
			Vector3 forward = from.forward;
			Vector3 rhs = target - from.position;
			return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(forward, rhs)), Vector3.Dot(forward, rhs)) * 57.29578f;
		}

		[CompilerGenerated]
		private void _003COnDisable_003Em__174(DirectionPointer p)
		{
			ForgetPointer(p);
		}
	}
}
