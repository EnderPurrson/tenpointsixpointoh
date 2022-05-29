using System;
using UnityEngine;

public class CreateClanButton : MonoBehaviour
{
	public CreateClanButton()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		NGUITools.GetRoot(base.gameObject).GetComponent<ClansGUIController>().CreateClanPanel.SetActive(true);
	}
}