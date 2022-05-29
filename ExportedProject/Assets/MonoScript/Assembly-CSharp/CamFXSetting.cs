using System;
using UnityEngine;

public sealed class CamFXSetting : MonoBehaviour
{
	public GameObject CamFX;

	public CamFXSetting()
	{
	}

	private void Start()
	{
		this.CamFX = base.transform.GetChild(0).gameObject;
		this.CamFX.SetActive(false);
	}
}