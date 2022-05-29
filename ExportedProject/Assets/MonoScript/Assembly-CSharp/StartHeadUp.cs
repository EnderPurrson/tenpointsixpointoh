using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class StartHeadUp : MonoBehaviour
{
	public StartHeadUp()
	{
	}

	private void Start()
	{
		if (Defs.isDaterRegim)
		{
			base.GetComponent<UILabel>().text = string.Empty;
			return;
		}
		if (!Defs.isInet || Defs.isInet && PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty))
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0560;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0561;
		}
		else if (!Defs.isCOOP)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0563;
		}
		else
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0562;
		}
	}
}