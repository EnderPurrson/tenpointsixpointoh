using System;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class ProgressBounds
	{
		private float _lowerBound;

		private float _upperBound = 1f;

		public float LowerBound
		{
			get
			{
				return this._lowerBound;
			}
		}

		public float UpperBound
		{
			get
			{
				return this._upperBound;
			}
		}

		public ProgressBounds()
		{
		}

		public float Clamp(float progress)
		{
			return Mathf.Clamp(progress, this._lowerBound, this._upperBound);
		}

		public float Lerp(float progress, float time)
		{
			return Mathf.Lerp(this.Clamp(progress), this.UpperBound, time);
		}

		public void SetBounds(float lowerBound, float upperBound)
		{
			lowerBound = Mathf.Clamp01(lowerBound);
			upperBound = Mathf.Clamp01(upperBound);
			if (lowerBound > upperBound)
			{
				throw new ArgumentException("Bounds are not ordered.");
			}
			this._lowerBound = lowerBound;
			this._upperBound = upperBound;
		}
	}
}