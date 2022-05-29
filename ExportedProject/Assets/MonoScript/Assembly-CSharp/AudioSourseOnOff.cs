using System;
using UnityEngine;

public class AudioSourseOnOff : MonoBehaviour
{
	private AudioSource myAudioSourse;

	public AudioSourseOnOff()
	{
	}

	private void Awake()
	{
		this.myAudioSourse = base.GetComponent<AudioSource>();
		if (this.myAudioSourse != null)
		{
			this.myAudioSourse.enabled = Defs.isSoundFX;
		}
	}

	private void Update()
	{
		if (this.myAudioSourse != null && this.myAudioSourse.enabled != Defs.isSoundFX)
		{
			this.myAudioSourse.enabled = Defs.isSoundFX;
		}
	}
}