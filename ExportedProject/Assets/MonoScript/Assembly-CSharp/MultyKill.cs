using System;
using UnityEngine;

public class MultyKill : MonoBehaviour
{
	public AudioSource multikillSound;

	public UIPlayTween multikillTween;

	public UISprite scorePict;

	private string scorePictTempName;

	public MultyKill()
	{
	}

	private void OnEnable()
	{
		if (Defs.isSoundFX)
		{
			this.multikillSound.Play();
		}
		this.multikillTween.Play(true);
	}

	public void PlayTween()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
		base.transform.GetChild(0).gameObject.SetActive(true);
		if (Defs.isSoundFX)
		{
			this.multikillSound.Stop();
			this.multikillSound.Play();
		}
		this.multikillTween.Play(true);
	}
}