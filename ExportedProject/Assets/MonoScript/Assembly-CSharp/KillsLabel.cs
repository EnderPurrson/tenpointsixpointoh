using System;
using UnityEngine;

public class KillsLabel : MonoBehaviour
{
	private UILabel _label;

	private InGameGUI _inGameGUI;

	public KillsLabel()
	{
	}

	private void Start()
	{
		base.gameObject.SetActive((!Defs.isMulti || ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.Deathmatch ? Defs.isDaterRegim : true));
		this._label = base.GetComponent<UILabel>();
		this._inGameGUI = InGameGUI.sharedInGameGUI;
	}

	private void Update()
	{
		if (this._inGameGUI && this._label)
		{
			if (Defs.isDaterRegim)
			{
				this._label.text = GlobalGameController.CountKills.ToString();
			}
			else if (this._inGameGUI != null)
			{
				this._label.text = this._inGameGUI.killsToMaxKills();
			}
		}
	}
}