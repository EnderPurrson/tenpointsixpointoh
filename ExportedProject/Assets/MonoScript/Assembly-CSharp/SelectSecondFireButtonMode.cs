using System;
using UnityEngine;

public class SelectSecondFireButtonMode : MonoBehaviour
{
	public UIToggle sniperModeButton;

	public UIToggle onModeButton;

	public UIToggle offSniperModeButton;

	public SelectSecondFireButtonMode()
	{
	}

	private void HandleOffClicked(object sender, EventArgs e)
	{
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.Off;
		PlayerPrefs.SetInt("GameSecondFireButtonMode", (int)Defs.gameSecondFireButtonMode);
	}

	private void HandleOnClicked(object sender, EventArgs e)
	{
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.On;
		PlayerPrefs.SetInt("GameSecondFireButtonMode", (int)Defs.gameSecondFireButtonMode);
	}

	private void HandleSniperClicked(object sender, EventArgs e)
	{
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.Sniper;
		PlayerPrefs.SetInt("GameSecondFireButtonMode", (int)Defs.gameSecondFireButtonMode);
	}

	private void Start()
	{
		this.sniperModeButton.gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleSniperClicked);
		this.onModeButton.gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleOnClicked);
		this.offSniperModeButton.gameObject.GetComponent<ButtonHandler>().Clicked += new EventHandler(this.HandleOffClicked);
		this.sniperModeButton.@value = Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Sniper;
		this.onModeButton.@value = Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.On;
		this.offSniperModeButton.@value = Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Off;
	}
}