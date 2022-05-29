using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Rilisoft
{
	public class AreaFog : AreaBase
	{
		[SerializeField]
		private float animationTime = 1f;

		[SerializeField]
		private FogSettings _settings;

		[ReadOnly]
		[SerializeField]
		private FogSettings _prevSettings;

		private CancellationTokenSource _tokenSource = new CancellationTokenSource();

		public AreaFog()
		{
		}

		private new void Awake()
		{
			this._prevSettings = (new FogSettings()).FromCurrent();
		}

		[DebuggerHidden]
		private IEnumerator Change(FogSettings to, float time, CancellationToken token)
		{
			AreaFog.u003cChangeu003ec__Iterator105 variable = null;
			return variable;
		}

		public override void CheckIn(GameObject to)
		{
			base.CheckIn(to);
			this._tokenSource.Cancel();
			this._tokenSource = new CancellationTokenSource();
			base.StartCoroutine(this.Change(this._settings, this.animationTime, this._tokenSource.Token));
		}

		public override void CheckOut(GameObject from)
		{
			base.CheckOut(from);
			this._tokenSource.Cancel();
			this._tokenSource = new CancellationTokenSource();
			base.StartCoroutine(this.Change(this._prevSettings, this.animationTime, this._tokenSource.Token));
		}
	}
}