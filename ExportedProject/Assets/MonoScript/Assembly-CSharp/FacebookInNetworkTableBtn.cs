using Rilisoft;
using System;
using UnityEngine;

public class FacebookInNetworkTableBtn : MonoBehaviour
{
	public FacebookInNetworkTableBtn()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().PostFacebookBtnClick();
		}
	}

	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			base.gameObject.SetActive(false);
		}
	}
}