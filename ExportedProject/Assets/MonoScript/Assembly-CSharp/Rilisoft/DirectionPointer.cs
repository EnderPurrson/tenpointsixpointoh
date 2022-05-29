using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(UIWidget))]
	public class DirectionPointer : MonoBehaviour
	{
		[SerializeField]
		private DirectionViewTargetType _forPointerType;

		[Range(0f, 3f)]
		[SerializeField]
		private float _hideTime = 0.3f;

		public bool OutOfRange;

		private UIWidget _widget;

		public DirectionViewTargetType ForPointerType
		{
			get
			{
				return this._forPointerType;
			}
		}

		public bool IsInited
		{
			get
			{
				return this.Target != null;
			}
		}

		public DirectionViewerTarget Target
		{
			get;
			private set;
		}

		public DirectionPointer()
		{
		}

		private void Awake()
		{
			this._widget = base.GetComponent<UIWidget>();
		}

		public void Hide()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(false);
			}
			else
			{
				base.StartCoroutine(this.TurnOffCoroutine());
			}
		}

		public void TurnOff()
		{
			this.Target = null;
			this.OutOfRange = false;
			this.Hide();
		}

		[DebuggerHidden]
		private IEnumerator TurnOffCoroutine()
		{
			DirectionPointer.u003cTurnOffCoroutineu003ec__Iterator7F variable = null;
			return variable;
		}

		public void TurnOn(DirectionViewerTarget pointer)
		{
			this.Target = pointer;
			base.gameObject.SetActive(true);
			this._widget.alpha = 1f;
			this._widget.gameObject.transform.localScale = Vector3.one;
		}
	}
}