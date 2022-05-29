using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class FogSettings
	{
		public bool Active;

		public FogMode Mode;

		public UnityEngine.Color Color;

		public float Start;

		public float End;

		public FogSettings()
		{
		}

		public FogSettings FromCurrent()
		{
			this.Active = RenderSettings.fog;
			this.Mode = RenderSettings.fogMode;
			this.Color = RenderSettings.fogColor;
			this.Start = RenderSettings.fogStartDistance;
			this.End = RenderSettings.fogEndDistance;
			return this;
		}

		public void SetToCurrent()
		{
			RenderSettings.fog = this.Active;
			RenderSettings.fogMode = this.Mode;
			RenderSettings.fogColor = this.Color;
			RenderSettings.fogStartDistance = this.Start;
			RenderSettings.fogEndDistance = this.End;
		}
	}
}