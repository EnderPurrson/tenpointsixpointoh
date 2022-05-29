using System;
using UnityEngine;

public sealed class ActionInTableButton : MonoBehaviour
{
	public UIButton buttonScript;

	public UISprite backgroundSprite;

	public UISprite rankSprite;

	public GameObject check;

	public UILabel namesPlayers;

	public Vector3 defaultNameLabelPos;

	public UILabel scorePlayers;

	public UILabel countKillsPlayers;

	public UITexture clanTexture;

	public string pixelbookID;

	public string nickPlayer;

	public NetworkStartTableNGUIController networkStartTableNGUIController;

	public bool isMine;

	public GameObject isMineSprite;

	public ActionInTableButton()
	{
	}

	private void OnClick()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		if (this.isMine)
		{
			return;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.networkStartTableNGUIController.ShowActionPanel(this.pixelbookID, this.nickPlayer);
	}

	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			float single = this.countKillsPlayers.transform.position.x;
			Transform vector3 = this.countKillsPlayers.transform;
			float single1 = this.scorePlayers.transform.position.x;
			float single2 = this.countKillsPlayers.transform.position.y;
			Vector3 vector31 = this.countKillsPlayers.transform.position;
			vector3.position = new Vector3(single1, single2, vector31.z);
			Transform transforms = this.scorePlayers.transform;
			float single3 = this.scorePlayers.transform.position.y;
			Vector3 vector32 = this.scorePlayers.transform.position;
			transforms.position = new Vector3(single, single3, vector32.z);
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			Transform transforms1 = this.scorePlayers.transform;
			float single4 = this.countKillsPlayers.transform.position.x;
			float single5 = this.scorePlayers.transform.position.y;
			Vector3 vector33 = this.scorePlayers.transform.position;
			transforms1.position = new Vector3(single4, single5, vector33.z);
			this.countKillsPlayers.gameObject.SetActive(false);
		}
		if (Defs.isDaterRegim)
		{
			this.countKillsPlayers.gameObject.SetActive(true);
			this.scorePlayers.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		this.UpdateAddState();
	}

	public void UpdateAddState()
	{
		if (!string.IsNullOrEmpty(this.pixelbookID) && FriendsController.sharedController.IsShowAdd(this.pixelbookID))
		{
			if (!this.buttonScript.enabled)
			{
				this.buttonScript.enabled = true;
				this.buttonScript.tweenTarget.SetActive(true);
				this.check.SetActive(true);
			}
			if (!this.check.activeSelf)
			{
				this.check.SetActive(true);
			}
		}
		else if (string.IsNullOrEmpty(this.pixelbookID) || this.pixelbookID.Equals("0") || this.pixelbookID.Equals("-1") || this.pixelbookID.Equals(FriendsController.sharedController.id) || !Defs2.IsAvalibleAddFrends() || string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			if (this.buttonScript.enabled)
			{
				this.buttonScript.enabled = false;
				this.buttonScript.tweenTarget.SetActive(false);
				this.check.SetActive(false);
			}
			if (this.check.activeSelf)
			{
				this.check.SetActive(false);
			}
		}
		else
		{
			if (this.buttonScript.enabled)
			{
				this.buttonScript.enabled = false;
				this.buttonScript.tweenTarget.SetActive(false);
			}
			if (!this.check.activeSelf)
			{
				this.check.SetActive(true);
			}
		}
	}

	public void UpdateState(bool isActive, int placeIndex = 0, bool _isMine = false, int command = 0, string nick = "", string score = "", string countKills = "", int rank = 1, Texture clanLogo = null, string _pixelbookID = "")
	{
		this.pixelbookID = _pixelbookID;
		this.nickPlayer = nick;
		this.isMine = _isMine;
		if (isActive)
		{
			Color color = Color.white;
			if (!this.isMine)
			{
				this.isMineSprite.SetActive(false);
				if ((ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture ? true : ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints))
				{
					if (command != 0)
					{
						color = (command != 1 ? new Color(1f, 0.7f, 0.7f, 1f) : new Color(0.6f, 0.8f, 1f, 1f));
					}
					else
					{
						color = new Color(0.6f, 0.6f, 0.6f, 1f);
					}
				}
			}
			else
			{
				this.isMineSprite.SetActive(true);
				if (this.buttonScript.enabled)
				{
					this.buttonScript.enabled = false;
					this.buttonScript.tweenTarget.SetActive(false);
					this.check.SetActive(false);
				}
				if (this.check.activeSelf)
				{
					this.check.SetActive(false);
				}
				color = new Color(1f, 1f, 1f, 1f);
			}
			base.gameObject.SetActive(true);
			this.namesPlayers.text = nick;
			if (this.defaultNameLabelPos == Vector3.zero)
			{
				this.defaultNameLabelPos = this.namesPlayers.transform.localPosition;
			}
			if (clanLogo != null)
			{
				this.namesPlayers.transform.localPosition = this.defaultNameLabelPos + (Vector3.right * 34f);
			}
			else
			{
				this.namesPlayers.transform.localPosition = this.defaultNameLabelPos;
			}
			this.namesPlayers.color = color;
			this.scorePlayers.text = score;
			this.scorePlayers.color = color;
			this.countKillsPlayers.text = countKills;
			this.countKillsPlayers.color = color;
			this.rankSprite.spriteName = string.Concat("Rank_", rank);
			this.clanTexture.mainTexture = clanLogo;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}
}