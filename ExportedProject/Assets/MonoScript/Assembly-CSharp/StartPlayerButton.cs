using System;
using UnityEngine;

public class StartPlayerButton : MonoBehaviour
{
	public StartPlayerButton.TypeButton command;

	public BlueRedButtonController buttonController;

	private float timeEnable;

	public StartPlayerButton()
	{
	}

	private void Awake()
	{
		bool flag = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture ? true : ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
		bool flag1 = NetworkStartTable.LocalOrPasswordRoom();
		if (!flag && this.command != StartPlayerButton.TypeButton.Start || flag && (this.command == StartPlayerButton.TypeButton.Start || (this.command == StartPlayerButton.TypeButton.RandomBtn || this.command == StartPlayerButton.TypeButton.Team2 || this.command == StartPlayerButton.TypeButton.Team1) && !flag1 || this.command == StartPlayerButton.TypeButton.TeamBattle && flag1) || Defs.isHunger)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnClick()
	{
		if (Time.time - NotificationController.timeStartApp < 3f || Defs.isCapturePoints && Time.realtimeSinceStartup - this.timeEnable < 1.5f)
		{
			return;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (WeaponManager.sharedManager.myTable != null)
		{
			int num = WeaponManager.sharedManager.myNetworkStartTable.myCommand;
			if (num <= 0)
			{
				num = (int)this.command;
				if ((this.command == StartPlayerButton.TypeButton.RandomBtn || this.command == StartPlayerButton.TypeButton.TeamBattle) && this.buttonController != null)
				{
					if (this.buttonController.countRed >= this.buttonController.countBlue)
					{
						num = (this.buttonController.countRed <= this.buttonController.countBlue ? UnityEngine.Random.Range(1, 3) : 1);
					}
					else
					{
						num = 2;
					}
				}
			}
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().StartPlayerButtonClick(num);
		}
	}

	private void OnEnable()
	{
		this.timeEnable = Time.realtimeSinceStartup;
	}

	private void Start()
	{
		if ((this.command == StartPlayerButton.TypeButton.Start || this.command == StartPlayerButton.TypeButton.TeamBattle) && Defs.isRegimVidosDebug)
		{
			base.gameObject.SetActive(false);
			base.GetComponent<UIButton>().enabled = false;
		}
	}

	private void Update()
	{
		base.GetComponent<UIButton>().isEnabled = (!Defs.isFlag ? true : Initializer.flag1 != null);
	}

	public enum TypeButton
	{
		Start,
		Team1,
		Team2,
		RandomBtn,
		TeamBattle
	}
}