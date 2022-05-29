using System;
using UnityEngine;

public class RePositionPlayersButton : MonoBehaviour
{
	public Vector3 positionInCommand;

	public RePositionPlayersButton()
	{
	}

	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			Transform transforms = base.transform;
			transforms.localPosition = transforms.localPosition + this.positionInCommand;
		}
	}
}