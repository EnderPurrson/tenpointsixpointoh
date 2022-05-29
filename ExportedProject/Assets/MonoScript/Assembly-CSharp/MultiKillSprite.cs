using System;
using UnityEngine;

public class MultiKillSprite : MonoBehaviour
{
	public MultiKillSprite()
	{
	}

	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			Transform vector3 = base.transform;
			float single = base.transform.localPosition.x;
			Vector3 vector31 = base.transform.localPosition;
			vector3.localPosition = new Vector3(single, -195f, vector31.z);
		}
	}

	private void Update()
	{
	}
}