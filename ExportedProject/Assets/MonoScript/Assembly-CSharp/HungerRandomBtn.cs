using System;
using UnityEngine;

public class HungerRandomBtn : MonoBehaviour
{
	public HungerRandomBtn()
	{
	}

	private void OnClick()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().RandomRoomClickBtnInHunger();
		}
	}
}