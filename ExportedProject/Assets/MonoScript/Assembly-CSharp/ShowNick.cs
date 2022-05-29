using System;
using UnityEngine;

public class ShowNick : MonoBehaviour
{
	public string nick;

	public bool isShowNick;

	public GUIStyle labelStyle;

	private float koofHeight;

	public ShowNick()
	{
	}

	private void OnGUI()
	{
	}

	private void Start()
	{
		this.koofHeight = (float)Screen.height / 768f;
		this.labelStyle.fontSize = Mathf.RoundToInt(20f * this.koofHeight);
	}

	private void Update()
	{
	}
}