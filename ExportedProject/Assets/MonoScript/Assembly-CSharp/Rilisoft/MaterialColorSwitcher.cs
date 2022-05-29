using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[RequireComponent(typeof(Renderer))]
	public class MaterialColorSwitcher : MonoBehaviour
	{
		public List<Color> Colors = new List<Color>();

		public float ToColorTime = 1f;

		private Material _mat;

		private int _colorIdx = -1;

		private bool _changed = true;

		public MaterialColorSwitcher()
		{
		}

		private void Awake()
		{
			this._mat = base.GetComponent<Renderer>().material;
		}

		[DebuggerHidden]
		private IEnumerator ChangeColor(int toIdx, float time)
		{
			MaterialColorSwitcher.u003cChangeColoru003ec__Iterator170 variable = null;
			return variable;
		}

		private void OnEnable()
		{
			base.StopAllCoroutines();
			this._changed = true;
		}

		private void Update()
		{
			if (this._changed)
			{
				this._changed = false;
				this._colorIdx = (this.Colors.Count - 1 <= this._colorIdx ? 0 : this._colorIdx + 1);
				base.StartCoroutine(this.ChangeColor(this._colorIdx, this.ToColorTime));
			}
		}
	}
}