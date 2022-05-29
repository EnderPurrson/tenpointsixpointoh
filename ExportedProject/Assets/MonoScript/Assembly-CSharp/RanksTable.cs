using System;
using System.Collections.Generic;
using UnityEngine;

public class RanksTable : MonoBehaviour
{
	private const int maxCountInCommandPlusOther = 6;

	private const int maxCountInTeam = 5;

	public GameObject panelRanks;

	public GameObject panelRanksTeam;

	public GameObject tekPanel;

	public GameObject modePC1;

	public GameObject modeFC1;

	public GameObject modeTDM1;

	public ActionInTableButton[] playersButtonsDeathmatch;

	public ActionInTableButton[] playersButtonsTeamFight;

	private List<NetworkStartTable> tabs = new List<NetworkStartTable>();

	private List<NetworkStartTable> tabsBlue = new List<NetworkStartTable>();

	private List<NetworkStartTable> tabsRed = new List<NetworkStartTable>();

	private List<NetworkStartTable> tabsWhite = new List<NetworkStartTable>();

	public bool isShowRanks;

	public bool isShowTableStart;

	public bool isShowTableWin;

	private bool isTeamMode;

	private string othersStr = "Others";

	public int totalBlue;

	public int totalRed;

	public int sumBlue;

	public int sumRed;

	public RanksTable()
	{
	}

	private void Awake()
	{
		this.othersStr = LocalizationStore.Get("Key_1224");
		this.isTeamMode = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture ? true : ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
	}

	private void FillButtonFromOldState(ActionInTableButton button, int tableIndex, bool isBlueTable = true, int team = 0)
	{
		NetworkStartTable networkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		bool flag = false;
		string empty = string.Empty;
		string str = string.Empty;
		string empty1 = string.Empty;
		string str1 = string.Empty;
		int num = 0;
		Texture texture = null;
		if (this.isTeamMode)
		{
			if (!isBlueTable)
			{
				flag = (networkStartTable.oldIndexMy != tableIndex ? false : networkStartTable.myCommandOld == 2);
			}
			else
			{
				flag = (networkStartTable.oldIndexMy != tableIndex ? false : networkStartTable.myCommandOld == 1);
			}
			empty = (!isBlueTable ? networkStartTable.oldCountLilsSpisokRed[tableIndex] : networkStartTable.oldCountLilsSpisokBlue[tableIndex]);
			str = (!isBlueTable ? networkStartTable.oldScoreSpisokRed[tableIndex] : networkStartTable.oldScoreSpisokBlue[tableIndex]);
			empty1 = (!isBlueTable ? networkStartTable.oldSpisokPixelBookIDRed[tableIndex] : networkStartTable.oldSpisokPixelBookIDBlue[tableIndex].ToString());
			str1 = (!isBlueTable ? networkStartTable.oldSpisokNameRed[tableIndex] : networkStartTable.oldSpisokNameBlue[tableIndex]);
			num = (!isBlueTable ? networkStartTable.oldSpisokRanksRed[tableIndex] : networkStartTable.oldSpisokRanksBlue[tableIndex]);
			texture = (!isBlueTable ? networkStartTable.oldSpisokMyClanLogoRed[tableIndex] : networkStartTable.oldSpisokMyClanLogoBlue[tableIndex]);
		}
		else
		{
			flag = networkStartTable.oldIndexMy == tableIndex;
			empty = networkStartTable.oldCountLilsSpisok[tableIndex];
			str = networkStartTable.oldScoreSpisok[tableIndex];
			empty1 = networkStartTable.oldSpisokPixelBookID[tableIndex].ToString();
			str1 = networkStartTable.oldSpisokName[tableIndex];
			num = networkStartTable.oldSpisokRanks[tableIndex];
			texture = networkStartTable.oldSpisokMyClanLogo[tableIndex];
		}
		if (empty == "-1")
		{
			empty = "0";
		}
		if (str == "-1")
		{
			str = "0";
		}
		button.UpdateState(true, tableIndex, flag, team, str1, str, empty, num, texture, empty1);
	}

	private void FillButtonFromTable(ActionInTableButton button, NetworkStartTable table, int tableIndex, int team = 0)
	{
		NetworkStartTable networkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		bool flag = false;
		string empty = string.Empty;
		string str = string.Empty;
		string empty1 = string.Empty;
		string namePlayer = string.Empty;
		int num = 0;
		Texture texture = null;
		flag = table.Equals(networkStartTable);
		empty = table.CountKills.ToString();
		str = table.score.ToString();
		empty1 = table.pixelBookID.ToString();
		namePlayer = table.NamePlayer;
		num = table.myRanks;
		texture = table.myClanTexture;
		if (empty == "-1")
		{
			empty = "0";
		}
		if (str == "-1")
		{
			str = "0";
		}
		button.UpdateState(true, tableIndex, flag, team, namePlayer, str, empty, num, texture, empty1);
	}

	private void FillDeathmatchButtons(bool oldState = false)
	{
		NetworkStartTable networkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		for (int i = 0; i < (int)this.playersButtonsDeathmatch.Length; i++)
		{
			if (!oldState && i < this.tabs.Count)
			{
				this.FillButtonFromTable(this.playersButtonsDeathmatch[i], this.tabs[i], i, 0);
			}
			else if (!oldState || i >= (int)networkStartTable.oldSpisokName.Length)
			{
				this.playersButtonsDeathmatch[i].UpdateState(false, 0, false, 0, string.Empty, string.Empty, string.Empty, 1, null, string.Empty);
			}
			else
			{
				this.FillButtonFromOldState(this.playersButtonsDeathmatch[i], i, true, 0);
			}
		}
	}

	private void FillTeamButtons(bool oldState = false)
	{
		int num;
		int num1;
		int num2;
		NetworkStartTable networkStartTable = WeaponManager.sharedManager.myNetworkStartTable;
		int num3 = Mathf.Max(0, (!oldState ? networkStartTable.myCommand : networkStartTable.myCommandOld));
		this.sumRed = 0;
		this.sumBlue = 0;
		for (int i = 0; i < (int)this.playersButtonsTeamFight.Length / 2; i++)
		{
			if (!oldState && i < Mathf.Min(this.tabsBlue.Count, 5))
			{
				RanksTable ranksTable = this;
				ranksTable.sumBlue = ranksTable.sumBlue + (this.tabsBlue[i].CountKills == -1 ? 0 : this.tabsBlue[i].CountKills);
				this.FillButtonFromTable(this.playersButtonsTeamFight[i + (num3 != 2 ? 0 : 6)], this.tabsBlue[i], i, num3);
			}
			else if (oldState && i < Mathf.Min((int)networkStartTable.oldSpisokNameBlue.Length, 5))
			{
				RanksTable ranksTable1 = this;
				ranksTable1.sumBlue = ranksTable1.sumBlue + int.Parse((networkStartTable.oldCountLilsSpisokBlue[i] == "-1" ? "0" : networkStartTable.oldCountLilsSpisokBlue[i]));
				this.FillButtonFromOldState(this.playersButtonsTeamFight[i + (num3 != 2 ? 0 : 6)], i, true, num3);
			}
			else if (this.totalBlue - this.sumBlue <= 0 || i != 5 || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				this.playersButtonsTeamFight[i + (num3 != 2 ? 0 : 6)].UpdateState(false, 0, false, 0, string.Empty, string.Empty, string.Empty, 1, null, string.Empty);
			}
			else
			{
				ActionInTableButton actionInTableButton = this.playersButtonsTeamFight[i + (num3 != 2 ? 0 : 6)];
				string str = this.othersStr;
				string empty = string.Empty;
				int num4 = this.totalBlue - this.sumBlue;
				actionInTableButton.UpdateState(true, i, false, num3, str, empty, num4.ToString(), -1, null, string.Empty);
			}
			if (!oldState && i < Mathf.Min(this.tabsRed.Count, 5))
			{
				RanksTable ranksTable2 = this;
				ranksTable2.sumRed = ranksTable2.sumRed + (this.tabsRed[i].CountKills == -1 ? 0 : this.tabsRed[i].CountKills);
				ActionInTableButton actionInTableButton1 = this.playersButtonsTeamFight[i + (num3 == 2 ? 0 : 6)];
				NetworkStartTable item = this.tabsRed[i];
				int num5 = i;
				if (num3 != 0)
				{
					num2 = (num3 != 2 ? 2 : 1);
				}
				else
				{
					num2 = 0;
				}
				this.FillButtonFromTable(actionInTableButton1, item, num5, num2);
			}
			else if (oldState && i < Mathf.Min((int)networkStartTable.oldSpisokNameRed.Length, 5))
			{
				RanksTable ranksTable3 = this;
				ranksTable3.sumRed = ranksTable3.sumRed + int.Parse((networkStartTable.oldCountLilsSpisokRed[i] == "-1" ? "0" : networkStartTable.oldCountLilsSpisokRed[i]));
				ActionInTableButton actionInTableButton2 = this.playersButtonsTeamFight[i + (num3 == 2 ? 0 : 6)];
				int num6 = i;
				if (num3 != 0)
				{
					num1 = (num3 != 2 ? 2 : 1);
				}
				else
				{
					num1 = 0;
				}
				this.FillButtonFromOldState(actionInTableButton2, num6, false, num1);
			}
			else if (this.totalRed - this.sumRed <= 0 || i != 5 || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				this.playersButtonsTeamFight[i + (num3 == 2 ? 0 : 6)].UpdateState(false, 0, false, 0, string.Empty, string.Empty, string.Empty, 1, null, string.Empty);
			}
			else
			{
				ActionInTableButton actionInTableButton3 = this.playersButtonsTeamFight[i + (num3 == 2 ? 0 : 6)];
				int num7 = i;
				if (num3 != 0)
				{
					num = (num3 != 2 ? 2 : 1);
				}
				else
				{
					num = 0;
				}
				string str1 = this.othersStr;
				string empty1 = string.Empty;
				int num8 = this.totalRed - this.sumRed;
				actionInTableButton3.UpdateState(true, num7, false, num, str1, empty1, num8.ToString(), -1, null, string.Empty);
			}
		}
		if (oldState && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			if (this.totalBlue < this.sumBlue)
			{
				this.totalBlue = this.sumBlue;
			}
			if (this.totalRed < this.sumRed)
			{
				this.totalRed = this.sumRed;
			}
		}
		for (int j = 0; j < (int)NetworkStartTableNGUIController.sharedController.totalBlue.Length; j++)
		{
			NetworkStartTableNGUIController.sharedController.totalBlue[j].text = (num3 == 2 ? this.totalRed.ToString() : this.totalBlue.ToString());
		}
		for (int k = 0; k < (int)NetworkStartTableNGUIController.sharedController.totalRed.Length; k++)
		{
			NetworkStartTableNGUIController.sharedController.totalRed[k].text = (num3 == 2 ? this.totalBlue.ToString() : this.totalRed.ToString());
		}
	}

	private void ReloadTabsFromReal()
	{
		this.tabsBlue.Clear();
		this.tabsRed.Clear();
		this.tabsWhite.Clear();
		this.tabs.Clear();
		this.tabs.AddRange(Initializer.networkTables);
		for (int i = 1; i < this.tabs.Count; i++)
		{
			NetworkStartTable item = this.tabs[i];
			int num = 0;
			while (num < i)
			{
				NetworkStartTable networkStartTable = this.tabs[num];
				if ((Defs.isFlag || Defs.isCapturePoints || item.score <= networkStartTable.score && (item.score != networkStartTable.score || item.CountKills <= networkStartTable.CountKills)) && (!Defs.isFlag && !Defs.isCapturePoints || item.CountKills <= networkStartTable.CountKills && (item.CountKills != networkStartTable.CountKills || item.score <= networkStartTable.score)))
				{
					num++;
				}
				else
				{
					NetworkStartTable item1 = this.tabs[i];
					for (int j = i - 1; j >= num; j--)
					{
						this.tabs[j + 1] = this.tabs[j];
					}
					this.tabs[num] = item1;
					break;
				}
			}
		}
		if (this.isTeamMode)
		{
			for (int k = 0; k < this.tabs.Count; k++)
			{
				if (this.tabs[k].myCommand == 1)
				{
					this.tabsBlue.Add(this.tabs[k]);
				}
				else if (this.tabs[k].myCommand != 2)
				{
					this.tabsWhite.Add(this.tabs[k]);
				}
				else
				{
					this.tabsRed.Add(this.tabs[k]);
				}
			}
		}
	}

	private void Start()
	{
		if (!this.isTeamMode)
		{
			this.panelRanksTeam.SetActive(false);
			this.panelRanks.SetActive(true);
		}
		else
		{
			this.panelRanksTeam.SetActive(true);
			this.panelRanks.SetActive(false);
			this.modePC1.SetActive(ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
			this.modeFC1.SetActive(ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture);
			this.modeTDM1.SetActive(ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight);
		}
	}

	private void Update()
	{
		if (this.isShowRanks || this.isShowTableStart)
		{
			this.ReloadTabsFromReal();
			this.UpdateRanksFromTabs();
		}
	}

	public void UpdateRanksFromOldSpisok()
	{
		if (!this.isTeamMode)
		{
			this.FillDeathmatchButtons(true);
		}
		else
		{
			this.FillTeamButtons(true);
		}
	}

	private void UpdateRanksFromTabs()
	{
		if (Defs.isCompany)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC == null)
			{
				this.totalBlue = GlobalGameController.countKillsBlue;
				this.totalRed = GlobalGameController.countKillsRed;
			}
			else
			{
				this.totalBlue = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue;
				this.totalRed = WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed;
			}
		}
		if (Defs.isFlag && WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			this.totalBlue = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1;
			this.totalRed = WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2;
		}
		if (Defs.isCapturePoints)
		{
			this.totalBlue = Mathf.RoundToInt(CapturePointController.sharedController.scoreBlue);
			this.totalRed = Mathf.RoundToInt(CapturePointController.sharedController.scoreRed);
		}
		if (!this.isTeamMode)
		{
			this.FillDeathmatchButtons(false);
		}
		else
		{
			this.FillTeamButtons(false);
		}
	}
}