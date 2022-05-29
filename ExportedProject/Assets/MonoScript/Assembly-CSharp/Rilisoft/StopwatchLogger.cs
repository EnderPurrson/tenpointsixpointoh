using System;
using System.Diagnostics;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class StopwatchLogger : IDisposable
	{
		private readonly Stopwatch _stopwatch;

		private readonly string _text;

		private readonly bool _verbose;

		public StopwatchLogger(string text, bool verbose)
		{
			this._verbose = verbose;
			this._text = text ?? string.Empty;
			if (this._verbose)
			{
				UnityEngine.Debug.Log(string.Format("{0}: started.", this._text));
			}
			this._stopwatch = Stopwatch.StartNew();
		}

		public StopwatchLogger(string text) : this(text, true)
		{
		}

		public void Dispose()
		{
			this._stopwatch.Stop();
			if (this._verbose)
			{
				UnityEngine.Debug.Log(string.Format("{0}: finished at {1:0.00}", this._text, this._stopwatch.ElapsedMilliseconds));
			}
		}
	}
}