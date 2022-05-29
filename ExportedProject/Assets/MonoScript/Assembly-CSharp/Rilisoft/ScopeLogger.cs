using System;
using UnityEngine;

namespace Rilisoft
{
	internal struct ScopeLogger : IDisposable
	{
		private string _callee;

		private string _caller;

		private bool _enabled;

		private bool _initialized;

		private readonly int _startFrame;

		private readonly float _startTime;

		public ScopeLogger(string caller, string callee, bool enabled)
		{
			this._caller = caller ?? string.Empty;
			this._callee = callee ?? string.Empty;
			this._enabled = enabled;
			if (!this._enabled)
			{
				this._startTime = 0f;
				this._startFrame = 0;
			}
			else
			{
				this._startTime = Time.realtimeSinceStartup;
				this._startFrame = Time.frameCount;
				string str = (!string.IsNullOrEmpty(this._caller) ? "{0} > {1}: {2:f3}, {3}" : "> {1}: {2:f3}, {3}");
				string str1 = (!Application.isEditor ? str : string.Concat("<color=orange>", str, "</color>"));
				Debug.LogFormat(str1, new object[] { this._caller, this._callee, this._startTime, this._startFrame });
			}
			this._initialized = true;
		}

		public ScopeLogger(string callee, bool enabled) : this(string.Empty, callee, enabled)
		{
		}

		public void Dispose()
		{
			if (!this._initialized)
			{
				return;
			}
			if (this._enabled)
			{
				string str = (!string.IsNullOrEmpty(this._caller) ? "{0} < {1}: +{2:f3}, +{3}" : "< {1}: +{2:f3}, +{3}");
				string str1 = (!Application.isEditor ? str : string.Concat("<color=orange>", str, "</color>"));
				Debug.LogFormat(str1, new object[] { this._caller, this._callee, Time.realtimeSinceStartup - this._startTime, Time.frameCount - this._startFrame });
			}
			this._callee = string.Empty;
			this._caller = string.Empty;
			this._initialized = false;
		}
	}
}