using Rilisoft;
using System;
using UnityEngine;

public class SoundFXOnOff : MonoBehaviour
{
	private GameObject soundFX;

	private bool _isWeakdevice;

	public SoundFXOnOff()
	{
	}

	private void FixedUpdate()
	{
		if (!this._isWeakdevice && this.soundFX.activeSelf != Defs.isSoundFX)
		{
			this.soundFX.SetActive(Defs.isSoundFX);
		}
	}

	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			this._isWeakdevice = Device.isWeakDevice;
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			this._isWeakdevice = true;
		}
		else
		{
			this._isWeakdevice = Device.IsLoweMemoryDevice;
		}
		if (!this._isWeakdevice || Application.isEditor)
		{
			this.soundFX = base.transform.GetChild(0).gameObject;
			if (Defs.isSoundFX)
			{
				this.soundFX.SetActive(true);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}