using System;
using UnityEngine;

public class NetworkStartTableStartPlashka : MonoBehaviour
{
	public GameObject plashka;

	public NetworkStartTableStartPlashka()
	{
	}

	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			base.gameObject.SetActive(false);
			this.plashka.SetActive(false);
		}
		else if (Defs.isCOOP)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0555;
		}
		else if (Defs.isHunger)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0556;
		}
		else if (!Defs.isDaterRegim)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0557;
		}
		else
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1539");
		}
	}

	private void Update()
	{
	}
}