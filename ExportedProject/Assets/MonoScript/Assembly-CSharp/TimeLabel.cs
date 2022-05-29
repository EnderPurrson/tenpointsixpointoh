using System;
using UnityEngine;

public class TimeLabel : MonoBehaviour
{
	private UILabel _label;

	public UISprite timerBackground;

	public AudioSource timerSound;

	public ParticleSystem timerParticles;

	private Vector3 targetScale = Vector3.one;

	private bool blink;

	private float startTime = 11f;

	public TimeLabel()
	{
	}

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
		this._label = base.GetComponent<UILabel>();
	}

	private void Update()
	{
		if (InGameGUI.sharedInGameGUI && this._label)
		{
			this._label.text = InGameGUI.sharedInGameGUI.timeLeft();
			if (!Defs.isHunger)
			{
				float single = (float)TimeGameController.sharedController.timerToEndMatch;
				if (single > this.startTime)
				{
					this.timerParticles.gameObject.SetActive(false);
					this.timerSound.enabled = false;
					this._label.color = Color.white;
					this._label.transform.localScale = Vector3.one;
					this._label.GetComponentInChildren<TweenRotation>().ResetToBeginning();
					this._label.GetComponentInChildren<TweenRotation>().enabled = false;
				}
				else
				{
					float single1 = Mathf.Round(single) - single;
					this.blink = single1 > 0f;
					this._label.transform.localScale = Vector3.MoveTowards(this._label.transform.localScale, (!this.blink ? Vector3.one : Vector3.one * Mathf.Min(1.4f + (this.startTime - single) / 20f, 2f)), (!this.blink ? 2.4f * Time.deltaTime : 12f * Time.deltaTime));
					this._label.color = (!this.blink ? Color.white : Color.red);
					this._label.GetComponentInChildren<TweenRotation>().enabled = true;
					this._label.GetComponentInChildren<TweenRotation>().PlayForward();
					if (Defs.isSoundFX)
					{
						this.timerSound.enabled = true;
					}
					this.timerSound.loop = true;
					if (!(PauseGUIController.Instance != null) || !PauseGUIController.Instance.IsPaused)
					{
						this.timerParticles.gameObject.SetActive(true);
					}
					else
					{
						this.timerParticles.gameObject.SetActive(false);
					}
					ParticleSystem.TextureSheetAnimationModule minMaxCurve = this.timerParticles.textureSheetAnimation;
					ParticleSystemCurveMode particleSystemCurveMode = minMaxCurve.frameOverTime.mode;
					minMaxCurve.frameOverTime = new ParticleSystem.MinMaxCurve((single - 1f) / 9f);
					if (single < 1f)
					{
						this.timerParticles.gameObject.SetActive(false);
					}
				}
			}
		}
	}
}