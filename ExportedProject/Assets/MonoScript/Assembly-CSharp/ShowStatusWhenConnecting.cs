using System;
using UnityEngine;

public class ShowStatusWhenConnecting : MonoBehaviour
{
	public GUISkin Skin;

	public ShowStatusWhenConnecting()
	{
	}

	private string GetConnectingDots()
	{
		string empty = string.Empty;
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad * 3f % 4f);
		for (int i = 0; i < num; i++)
		{
			empty = string.Concat(empty, " .");
		}
		return empty;
	}

	private void OnGUI()
	{
		if (this.Skin != null)
		{
			GUI.skin = this.Skin;
		}
		float single = 400f;
		float single1 = 100f;
		Rect rect = new Rect(((float)Screen.width - single) / 2f, ((float)Screen.height - single1) / 2f, single, single1);
		GUILayout.BeginArea(rect, GUI.skin.box);
		GUILayout.Label(string.Concat("Connecting", this.GetConnectingDots()), GUI.skin.customStyles[0], new GUILayoutOption[0]);
		GUILayout.Label(string.Concat("Status: ", PhotonNetwork.connectionStateDetailed), new GUILayoutOption[0]);
		GUILayout.EndArea();
		if (PhotonNetwork.inRoom)
		{
			base.enabled = false;
		}
	}
}