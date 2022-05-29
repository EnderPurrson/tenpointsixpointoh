using Rilisoft;
using System;
using UnityEngine;

public class EnderButton : MonoBehaviour
{
	public EnderButton()
	{
	}

	private void Start()
	{
		bool flag;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			flag = true;
		}
		else
		{
			flag = (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android ? false : Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon);
		}
		if (!flag || !Defs.EnderManAvailable)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
	}
}