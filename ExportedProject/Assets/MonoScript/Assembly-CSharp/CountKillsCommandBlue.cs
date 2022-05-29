using System;
using UnityEngine;

public class CountKillsCommandBlue : MonoBehaviour
{
	public static float localScaleForLabels;

	private UILabel _label;

	public bool isEnemyCommandLabel;

	private WeaponManager _weaponManager;

	public GameObject myBackground;

	private Color goldColor = new Color(1f, 1f, 0f);

	static CountKillsCommandBlue()
	{
		CountKillsCommandBlue.localScaleForLabels = 1.25f;
	}

	public CountKillsCommandBlue()
	{
	}

	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		InGameGUI inGameGUI = InGameGUI.sharedInGameGUI;
		this._label = base.GetComponent<UILabel>();
	}

	private void Update()
	{
		if (this._weaponManager && this._weaponManager.myPlayer)
		{
			string str = "0";
			bool flag = false;
			if (Defs.isFlag)
			{
				if (WeaponManager.sharedManager.myTable != null)
				{
					if (this.isEnemyCommandLabel != (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2))
					{
						str = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2.ToString();
						flag = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1;
					}
					else
					{
						str = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1.ToString();
						flag = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2;
					}
				}
			}
			else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				if (this.isEnemyCommandLabel != (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2))
				{
					int num = Mathf.RoundToInt(CapturePointController.sharedController.scoreRed);
					str = num.ToString();
					flag = CapturePointController.sharedController.scoreRed > CapturePointController.sharedController.scoreBlue;
				}
				else
				{
					int num1 = Mathf.RoundToInt(CapturePointController.sharedController.scoreBlue);
					str = num1.ToString();
					flag = CapturePointController.sharedController.scoreBlue > CapturePointController.sharedController.scoreRed;
				}
			}
			else if (this.isEnemyCommandLabel != (WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2))
			{
				str = this._weaponManager.myPlayerMoveC.countKillsCommandRed.ToString();
				flag = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue;
			}
			else
			{
				str = this._weaponManager.myPlayerMoveC.countKillsCommandBlue.ToString();
				flag = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed;
			}
			this._label.text = str;
			this._label.color = (!flag ? Color.white : this.goldColor);
		}
	}
}