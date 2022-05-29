using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.SceneUtils
{
	public class SlowMoButton : MonoBehaviour
	{
		public Sprite FullSpeedTex;

		public Sprite SlowSpeedTex;

		public float fullSpeed = 1f;

		public float slowSpeed = 0.3f;

		public Button button;

		private bool m_SlowMo;

		public SlowMoButton()
		{
		}

		public void ChangeSpeed()
		{
			this.m_SlowMo = !this.m_SlowMo;
			Image image = this.button.targetGraphic as Image;
			if (image != null)
			{
				image.sprite = (!this.m_SlowMo ? this.FullSpeedTex : this.SlowSpeedTex);
			}
			this.button.targetGraphic = image;
			Time.timeScale = (!this.m_SlowMo ? this.fullSpeed : this.slowSpeed);
		}

		private void OnDestroy()
		{
			Time.timeScale = 1f;
		}

		private void Start()
		{
			this.m_SlowMo = false;
		}
	}
}