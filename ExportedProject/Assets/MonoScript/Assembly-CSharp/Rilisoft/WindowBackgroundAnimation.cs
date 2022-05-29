using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class WindowBackgroundAnimation : MonoBehaviour
	{
		public GameObject[] Arrows;

		public GameObject[] ShineNodes;

		public bool PlayOnEnable = true;

		private int _currentBgArrowPrefabIndex = -1;

		private GameObject[] _bgArrowRows;

		private UIRoot interfaceHolderValue;

		private UIRoot interfaceHolder
		{
			get
			{
				if (this.interfaceHolderValue == null)
				{
					this.interfaceHolderValue = base.gameObject.GetComponentInParents<UIRoot>();
				}
				return this.interfaceHolderValue;
			}
		}

		public WindowBackgroundAnimation()
		{
		}

		[DebuggerHidden]
		private IEnumerator LoopBackgroundAnimation()
		{
			WindowBackgroundAnimation.u003cLoopBackgroundAnimationu003ec__Iterator1A2 variable = null;
			return variable;
		}

		private void OnEnable()
		{
			if (this.PlayOnEnable)
			{
				this.Play();
			}
		}

		public void Play()
		{
			this._currentBgArrowPrefabIndex = -1;
			base.StartCoroutine(this.LoopBackgroundAnimation());
		}

		private void ResetBackgroundArrows(Transform target)
		{
			float single;
			for (int i = 0; i < (int)this._bgArrowRows.Length; i++)
			{
				Transform transforms = this._bgArrowRows[i].transform;
				transforms.parent = target.parent;
				transforms.localScale = Vector3.one;
				Transform vector3 = transforms;
				float single1 = target.localPosition.x;
				single = (i % 2 != 1 ? 0f : 90f);
				Vector3 vector31 = target.localPosition;
				float single2 = vector31.y - 110f * (float)i;
				Vector3 vector32 = target.localPosition;
				vector3.localPosition = new Vector3(single1 + single, single2, vector32.z);
				transforms.localRotation = target.localRotation;
			}
		}
	}
}