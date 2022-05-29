using System;
using UnityEngine;

public class NextPrewSpectratorPlayer : MonoBehaviour
{
	public NextPrewSpectratorPlayer()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().ResetCamPlayer((!base.gameObject.name.Equals("PrewMode") ? 1 : -1));
		}
	}
}