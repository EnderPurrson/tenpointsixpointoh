using Rilisoft;
using System;
using UnityEngine;

internal sealed class PostSocialBtn : MonoBehaviour
{
	public bool isFacebook;

	public PostSocialBtn()
	{
	}

	private void OnClick()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Debug.Log("PostSocialBtn");
		ButtonClickSound.Instance.PlayClick();
		if (this.isFacebook)
		{
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().PostFacebookBtnClick();
			}
		}
		else if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().PostTwitterBtnClick();
		}
	}

	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}
}