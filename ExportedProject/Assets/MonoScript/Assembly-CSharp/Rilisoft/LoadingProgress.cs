using System;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class LoadingProgress
	{
		private readonly GUIStyle _labelStyle;

		private readonly Texture2D _backgroundTexture;

		private readonly Texture2D _progressbarTexture;

		private static LoadingProgress _instance;

		public static LoadingProgress Instance
		{
			get
			{
				if (LoadingProgress._instance == null)
				{
					LoadingProgress._instance = new LoadingProgress();
				}
				return LoadingProgress._instance;
			}
		}

		private LoadingProgress()
		{
			this._backgroundTexture = Resources.Load<Texture2D>("line_shadow");
			this._progressbarTexture = Resources.Load<Texture2D>("line");
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				font = Resources.Load<Font>("04B_03"),
				fontSize = Convert.ToInt32(22f * Defs.Coef),
				normal = new GUIStyleState()
				{
					textColor = Color.black
				}
			};
			this._labelStyle = gUIStyle;
		}

		public void Draw(float progress)
		{
			float single = Mathf.Clamp01(progress);
			if (this._backgroundTexture != null)
			{
				float coef = 1.8f * (float)this._backgroundTexture.width * Defs.Coef;
				float coef1 = 1.8f * (float)this._backgroundTexture.height * Defs.Coef;
				Rect rect = new Rect(0.5f * ((float)Screen.width - coef), (float)Screen.height - (21f * Defs.Coef + coef1), coef, coef1);
				float single1 = coef - 7.2f * Defs.Coef;
				float single2 = single1 * single;
				float coef2 = coef1 - 7.2f * Defs.Coef;
				if (this._progressbarTexture != null)
				{
					Rect rect1 = new Rect(rect.xMin + 3.6f * Defs.Coef, rect.yMin + 3.6f * Defs.Coef, single2, coef2);
					GUI.DrawTexture(rect1, this._progressbarTexture);
				}
				GUI.DrawTexture(rect, this._backgroundTexture);
				Rect rect2 = rect;
				rect2.yMin = rect2.yMin - 1.8f * Defs.Coef;
				rect2.y = rect2.y + 1.8f * Defs.Coef;
				int num = Mathf.RoundToInt(single * 100f);
				string str = string.Format("{0}%", num);
				GUI.Label(rect2, str, this._labelStyle);
			}
		}

		public static void Unload()
		{
			if (LoadingProgress._instance != null)
			{
				Resources.UnloadAsset(LoadingProgress._instance._backgroundTexture);
				Resources.UnloadAsset(LoadingProgress._instance._progressbarTexture);
				LoadingProgress._instance = null;
			}
		}
	}
}