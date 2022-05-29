using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
	public AnimationCurve xRotation;

	public AnimationCurve yRotation;

	public Transform sun;

	public Transform yAxis;

	private float matchTime;

	private float matchTimeDelta;

	public SunRotation()
	{
	}

	private void LateUpdate()
	{
		if (TimeGameController.sharedController != null && PhotonNetwork.room != null && !string.IsNullOrEmpty(ConnectSceneNGUIController.maxKillProperty) && PhotonNetwork.room.customProperties.ContainsKey(ConnectSceneNGUIController.maxKillProperty))
		{
			int num = -1;
			int.TryParse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString(), out num);
			if (num < 0)
			{
				return;
			}
			this.matchTime = (float)num * 60f;
			if ((float)TimeGameController.sharedController.timerToEndMatch < this.matchTime)
			{
				this.matchTimeDelta = this.matchTime - (float)TimeGameController.sharedController.timerToEndMatch;
				if (Camera.main)
				{
					this.sun.LookAt(Camera.main.transform);
				}
				Quaternion vector3 = new Quaternion()
				{
					eulerAngles = new Vector3(this.xRotation.Evaluate(this.matchTimeDelta / this.matchTime), 0f, 0f)
				};
				base.transform.localRotation = vector3;
				vector3.eulerAngles = new Vector3(0f, this.yRotation.Evaluate(this.matchTimeDelta / this.matchTime), 0f);
				this.yAxis.localRotation = vector3;
			}
		}
	}
}