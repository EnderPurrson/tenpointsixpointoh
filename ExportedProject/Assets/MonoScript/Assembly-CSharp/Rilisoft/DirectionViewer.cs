using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class DirectionViewer : MonoBehaviour
	{
		[SerializeField]
		private UIPanel _panel;

		[SerializeField]
		private float _lookRadius = 50f;

		[SerializeField]
		private float _radius = 200f;

		private Dictionary<DirectionViewTargetType, Queue<DirectionPointer>> _freePointers = new Dictionary<DirectionViewTargetType, Queue<DirectionPointer>>();

		private List<DirectionPointer> _activePointers = new List<DirectionPointer>();

		public static DirectionViewer Instance
		{
			get;
			private set;
		}

		public DirectionViewer()
		{
		}

		private void Awake()
		{
			if (DirectionViewer.Instance != null)
			{
				return;
			}
			DirectionViewer.Instance = this;
			this._activePointers.Clear();
			this._freePointers.Clear();
			foreach (DirectionPointer list in base.GetComponentsInChildren<DirectionPointer>(true).ToList<DirectionPointer>())
			{
				if (!this._freePointers.ContainsKey(list.ForPointerType))
				{
					Queue<DirectionPointer> directionPointers = new Queue<DirectionPointer>();
					directionPointers.Enqueue(list);
					this._freePointers.Add(list.ForPointerType, directionPointers);
				}
				else
				{
					this._freePointers[list.ForPointerType].Enqueue(list);
				}
			}
		}

		private bool CheckDistance(DirectionViewerTarget poiner)
		{
			if (WeaponManager.sharedManager == null || poiner == null)
			{
				return false;
			}
			Vector3 transform = WeaponManager.sharedManager.myPlayer.transform.position - poiner.Transform.position;
			return transform.sqrMagnitude < Mathf.Pow(this._lookRadius, 2f);
		}

		public void ForgetMe(DirectionViewerTarget target)
		{
			DirectionPointer directionPointer = this._activePointers.FirstOrDefault<DirectionPointer>((DirectionPointer p) => p.Target == target);
			if (directionPointer == null)
			{
				return;
			}
			directionPointer.TurnOff();
			this._activePointers.Remove(directionPointer);
			this._freePointers[directionPointer.ForPointerType].Enqueue(directionPointer);
		}

		private void ForgetPointer(DirectionPointer pointer)
		{
			pointer.TurnOff();
			if (!this._activePointers.Contains(pointer))
			{
				return;
			}
			this._activePointers.Remove(pointer);
			this._freePointers[pointer.ForPointerType].Enqueue(pointer);
		}

		private float GetAngle(Transform from, Vector3 target, Vector3 n)
		{
			Vector3 vector3 = from.forward;
			Vector3 vector31 = target - from.position;
			return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(vector3, vector31)), Vector3.Dot(vector3, vector31)) * 57.29578f;
		}

		private void LateUpdate()
		{
			if (WeaponManager.sharedManager == null)
			{
				return;
			}
			this._activePointers.ForEach(new Action<DirectionPointer>(this.SetPointerState));
		}

		public void LookToMe(DirectionViewerTarget target)
		{
			if (target == null || this._activePointers.Any<DirectionPointer>((DirectionPointer p) => p.Target == target))
			{
				return;
			}
			if (!this._freePointers.ContainsKey(target.Type) || !this._freePointers[target.Type].Any<DirectionPointer>())
			{
				return;
			}
			DirectionPointer directionPointer = this._freePointers[target.Type].Dequeue();
			this._activePointers.Add(directionPointer);
			directionPointer.TurnOn(target);
		}

		private void OnDisable()
		{
			this._activePointers.ForEach((DirectionPointer p) => this.ForgetPointer(p));
		}

		private void SetPointerState(DirectionPointer pointer)
		{
			if (!this.CheckDistance(pointer.Target))
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
			float angle = this.GetAngle(NickLabelController.currentCamera.transform, pointer.Target.transform.position, Vector3.up);
			float single = this._radius * Mathf.Sin(angle * 0.017453292f);
			float single1 = this._radius * Mathf.Cos(angle * 0.017453292f);
			Vector3 vector3 = pointer.transform.position;
			Vector3 vector31 = new Vector3(single, single1, vector3.z);
			pointer.transform.localPosition = vector31;
		}
	}
}