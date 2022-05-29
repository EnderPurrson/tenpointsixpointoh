using System;
using UnityEngine;

public class FonTableRanksController : MonoBehaviour
{
	public bool isTeamTable;

	public GameObject scoreHead;

	public GameObject countKillsHead;

	public GameObject likeHead;

	public int command;

	public string nameCommand;

	public UISprite fon;

	public UISprite fonHead;

	public UISprite fonUndrhead;

	public UILabel headLabel;

	public UILabel[] undrheadLabels;

	public FonTableRanksController()
	{
	}

	public void SetCommand(int _command)
	{
		if (!this.isTeamTable)
		{
			return;
		}
		if (_command == 0)
		{
			this.fon.spriteName = "GreyTableHead";
			this.fonHead.spriteName = "GreyTableHead";
			this.fonUndrhead.spriteName = "GreyTable";
			UILabel[] uILabelArray = this.undrheadLabels;
			for (int i = 0; i < (int)uILabelArray.Length; i++)
			{
				UILabel color = uILabelArray[i];
				color.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			}
			this.headLabel.text = LocalizationStore.Get(this.nameCommand);
		}
		if (_command == 1)
		{
			this.fon.spriteName = "BlueTeamTableHead";
			this.fonHead.spriteName = "BlueTeamTableHead";
			this.fonUndrhead.spriteName = "BlueTeamTable";
			UILabel[] uILabelArray1 = this.undrheadLabels;
			for (int j = 0; j < (int)uILabelArray1.Length; j++)
			{
				UILabel uILabel = uILabelArray1[j];
				uILabel.color = new Color(0.6f, 0.8f, 1f, 1f);
			}
			this.headLabel.text = LocalizationStore.Get("Key_1771");
		}
		if (_command == 2)
		{
			this.fon.spriteName = "RedTeamTableHead";
			this.fonHead.spriteName = "RedTeamTableHead";
			this.fonUndrhead.spriteName = "RedTeamTable";
			this.headLabel.text = LocalizationStore.Get("Key_1772");
			UILabel[] uILabelArray2 = this.undrheadLabels;
			for (int k = 0; k < (int)uILabelArray2.Length; k++)
			{
				UILabel color1 = uILabelArray2[k];
				color1.color = new Color(1f, 0.7f, 0.7f, 1f);
			}
		}
	}

	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			float single = this.countKillsHead.transform.position.x;
			Transform vector3 = this.countKillsHead.transform;
			float single1 = this.scoreHead.transform.position.x;
			float single2 = this.countKillsHead.transform.position.y;
			Vector3 vector31 = this.countKillsHead.transform.position;
			vector3.position = new Vector3(single1, single2, vector31.z);
			Transform transforms = this.scoreHead.transform;
			float single3 = this.scoreHead.transform.position.y;
			Vector3 vector32 = this.scoreHead.transform.position;
			transforms.position = new Vector3(single, single3, vector32.z);
			this.countKillsHead.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1006");
			if (this.likeHead != null)
			{
				this.likeHead.SetActive(false);
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			Transform transforms1 = this.scoreHead.transform;
			float single4 = this.countKillsHead.transform.position.x;
			float single5 = this.scoreHead.transform.position.y;
			Vector3 vector33 = this.scoreHead.transform.position;
			transforms1.position = new Vector3(single4, single5, vector33.z);
			this.countKillsHead.SetActive(false);
			if (this.likeHead != null)
			{
				this.likeHead.SetActive(false);
			}
		}
		if (Defs.isDaterRegim)
		{
			this.scoreHead.gameObject.SetActive(false);
			this.countKillsHead.gameObject.SetActive(false);
			if (this.likeHead != null)
			{
				this.likeHead.SetActive(true);
			}
		}
		else if (this.likeHead != null)
		{
			this.likeHead.SetActive(false);
		}
	}

	private void Update()
	{
		int num = (WeaponManager.sharedManager.myNetworkStartTable.myCommand > 0 ? WeaponManager.sharedManager.myNetworkStartTable.myCommand : WeaponManager.sharedManager.myNetworkStartTable.myCommandOld);
		if (NetworkStartTable.LocalOrPasswordRoom() && NetworkStartTableNGUIController.IsStartInterfaceShown())
		{
			num = 0;
		}
		this.SetCommand((num > 0 ? this.command : 0));
	}
}