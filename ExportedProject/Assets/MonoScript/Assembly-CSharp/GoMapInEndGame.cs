using System;
using UnityEngine;

public class GoMapInEndGame : MonoBehaviour
{
	public int mapIndex;

	public UITexture mapTexture;

	public UILabel mapLabel;

	private float enableTime;

	public GoMapInEndGame()
	{
	}

	public void OnClick()
	{
		if (Time.time - this.enableTime < 2f)
		{
			return;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.mapIndex);
		Defs.typeDisconnectGame = Defs.DisconectGameType.SelectNewMap;
		Initializer.Instance.goMapName = infoScene.NameScene;
		GlobalGameController.countKillsRed = 0;
		GlobalGameController.countKillsBlue = 0;
		PhotonNetwork.LeaveRoom();
	}

	private void OnEnable()
	{
		this.enableTime = Time.time;
	}

	public void SetMap(SceneInfo scInfo)
	{
		this.mapIndex = scInfo.indexMap;
		this.mapTexture.mainTexture = Resources.Load<Texture>(string.Concat("LevelLoadingsSmall/Loading_", scInfo.NameScene));
		if (scInfo != null)
		{
			this.mapLabel.text = scInfo.TranslateName;
		}
	}

	private void Start()
	{
		if (!Defs.isInet || Defs.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
	}
}