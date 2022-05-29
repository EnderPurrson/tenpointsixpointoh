using System;
using System.Diagnostics;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class TimeMeasurement
	{
		private readonly string _context;

		private readonly int _startFrame;

		private readonly float _startTime;

		private int _frameCount;

		private float _timeBrutto;

		private readonly Stopwatch _timeNetto;

		public string Context
		{
			get
			{
				return this._context;
			}
		}

		public int FrameCount
		{
			get
			{
				return this._frameCount;
			}
		}

		public float TimeBrutto
		{
			get
			{
				return this._timeBrutto;
			}
		}

		public TimeSpan TimeNetto
		{
			get
			{
				return this._timeNetto.Elapsed;
			}
		}

		public TimeMeasurement(string context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this._context = context;
			this._startFrame = Time.frameCount;
			this._startTime = Time.realtimeSinceStartup;
			this._timeNetto = new Stopwatch();
		}

		public void Start()
		{
			this._timeNetto.Start();
		}

		public void Stop()
		{
			this._timeNetto.Stop();
			this._frameCount = Time.frameCount - this._startFrame;
			this._timeBrutto = Time.realtimeSinceStartup - this._startTime;
		}
	}
}