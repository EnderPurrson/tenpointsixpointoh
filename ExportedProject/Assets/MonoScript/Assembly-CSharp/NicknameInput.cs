using Rilisoft;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class NicknameInput : MonoBehaviour
{
	private const string PlayerNameKey = "NamePlayer";

	public UIInput input;

	private UIButton _okButton;

	public NicknameInput()
	{
	}

	private void HandleOkClicked(object sender, EventArgs e)
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		PlayerPrefs.SetString("NicknameRequested", "1");
		if (this.input != null)
		{
			if (this.input.@value != null)
			{
				string str = this.input.@value.Trim();
				string str1 = (!string.IsNullOrEmpty(str) ? str : "Unnamed");
				PlayerPrefs.SetString("NamePlayer", str1);
				this.input.@value = str1;
			}
			if (this._okButton != null)
			{
				this._okButton.isEnabled = false;
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene, LoadSceneMode.Single);
	}

	private void Start()
	{
		ButtonHandler componentInChildren = base.gameObject.GetComponentInChildren<ButtonHandler>();
		if (componentInChildren != null)
		{
			componentInChildren.Clicked += new EventHandler(this.HandleOkClicked);
			this._okButton = componentInChildren.GetComponent<UIButton>();
		}
		if (ExperienceController.sharedController != null && ExpController.Instance != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
			ExpController.Instance.InterfaceEnabled = false;
		}
		if (this.input != null)
		{
			string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
			this.input.@value = playerNameOrDefault;
		}
	}
}