using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class SpriteAnimation : UISpriteAnimation
	{
		public bool SnapPixels
		{
			get
			{
				return this.mSnap;
			}
			set
			{
				this.mSnap = value;
			}
		}

		public SpriteAnimation()
		{
		}

		protected override void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!base.isPlaying)
			{
				return;
			}
			if (base.frames < 2)
			{
				return;
			}
			if ((float)base.framesPerSecond <= 0f)
			{
				return;
			}
			int num = Mathf.FloorToInt(Time.realtimeSinceStartup * (float)base.framesPerSecond);
			this.mIndex = num % base.frames;
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}
}