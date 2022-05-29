using Rilisoft;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePointController : MonoBehaviour
{
	private const float timeCaptureCounter = 5f;

	private const float maxCaptureCounter = 100f;

	public string nameBase;

	private float _captureCounter;

	private Color redTeamColor = Color.red;

	private Color blueTeamColor = Color.blue;

	private Color redCaptureColor = new Color32(212, 0, 0, 130);

	private Color blueCaptureColor = new Color32(0, 0, 225, 130);

	public NickLabelController myLabelController;

	public PhotonView photonView;

	private bool isStartUpdateFromMasterClient;

	private float periodToSynch = 1f;

	private float timerToSynch;

	public List<Player_move_c> capturePlayers = new List<Player_move_c>();

	public bool isBaseActive = true;

	private bool myPlayerOnPoint;

	public GameObject baseRender;

	public LineRenderer rayPoint;

	private bool isSendMessageCaptureBlue;

	private bool isSendMessageCaptureRed;

	private bool isSendMessageRaiderBlue;

	private bool isSendMessageRaiderRed;

	private bool sendScoreEventBlue;

	private bool sendScoreEventRed;

	public BasePointController.TypeCapture captureConmmand;

	public float captureCounter
	{
		get
		{
			return this._captureCounter;
		}
		set
		{
			this._captureCounter = value;
		}
	}

	public BasePointController()
	{
	}

	[PunRPC]
	[RPC]
	private void AddPlayerInCapturePoint(int _viewId, float _time)
	{
		int num = 0;
		while (num < Initializer.players.Count)
		{
			if (Initializer.players[num].photonView.ownerId != _viewId)
			{
				num++;
			}
			else
			{
				if (!this.capturePlayers.Contains(Initializer.players[num]))
				{
					this.capturePlayers.Add(Initializer.players[num]);
					if (this.isBaseActive && PhotonNetwork.time > (double)_time)
					{
						int num1 = 0;
						int num2 = 0;
						for (int i = 0; i < this.capturePlayers.Count; i++)
						{
							if (this.capturePlayers[i].myCommand == 1)
							{
								num1++;
							}
							else if (this.capturePlayers[i].myCommand == 2)
							{
								num2++;
							}
						}
						if (num2 == 0 && num1 == 1)
						{
							BasePointController basePointController = this;
							basePointController.captureCounter = basePointController.captureCounter + ((float)PhotonNetwork.time - _time) * 100f / 5f;
							if (this.captureCounter > 100f)
							{
								this.captureCounter = 100f;
							}
						}
						if (num1 == 0 && num2 == 1)
						{
							BasePointController basePointController1 = this;
							basePointController1.captureCounter = basePointController1.captureCounter - ((float)PhotonNetwork.time - _time) * 100f / 5f;
							if (this.captureCounter < -100f)
							{
								this.captureCounter = -100f;
							}
						}
					}
				}
				break;
			}
		}
	}

	private void AddPlayerInList(GameObject _player)
	{
		Player_move_c component = _player.GetComponent<SkinName>().playerMoveC;
		if (!this.capturePlayers.Contains(component))
		{
			this.capturePlayers.Add(component);
		}
	}

	private void Awake()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.timerToSynch = this.periodToSynch;
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	private void MyPlayerEnterPoint()
	{
		this.myPlayerOnPoint = true;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.pointCaptureBar.SetActive(this.isBaseActive);
		}
	}

	private void MyPlayerLeavePoint()
	{
		this.myPlayerOnPoint = false;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.pointCaptureBar.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void OnDisconnectedFromPhoton()
	{
		this.isStartUpdateFromMasterClient = false;
	}

	public void OnEndMatch()
	{
		this.captureCounter = 0f;
		this.capturePlayers.Clear();
		this.rayPoint.SetColors(Color.gray, Color.gray);
		this.captureConmmand = BasePointController.TypeCapture.none;
		this.isSendMessageCaptureRed = false;
		this.isSendMessageCaptureBlue = false;
		this.isSendMessageRaiderBlue = false;
		this.isSendMessageRaiderRed = false;
		this.MyPlayerLeavePoint();
	}

	private void OnFailedToConnectToPhoton(object parameters)
	{
		this.OnEndMatch();
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (!Defs.isMulti || !Defs.isCapturePoints)
		{
			return;
		}
		this.photonView.RPC("SynchCaptureCounterNewPlayer", player, new object[] { player.ID, PhotonNetwork.isMasterClient, this.captureCounter, (int)this.captureConmmand });
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!Defs.isMulti || !Defs.isCapturePoints)
		{
			return;
		}
		if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Player"))
		{
			this.AddPlayerInList(other.transform.parent.gameObject);
			if (other.transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
			{
				this.MyPlayerEnterPoint();
				this.photonView.RPC("AddPlayerInCapturePoint", PhotonTargets.Others, new object[] { WeaponManager.sharedManager.myPlayerMoveC.photonView.ownerId, (float)PhotonNetwork.time });
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!Defs.isMulti || !Defs.isCapturePoints)
		{
			return;
		}
		if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Player"))
		{
			this.RemoveFromList(other.transform.parent.gameObject);
			if (other.transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
			{
				this.MyPlayerLeavePoint();
				this.photonView.RPC("RemovePlayerInCapturePoint", PhotonTargets.Others, new object[] { WeaponManager.sharedManager.myPlayerMoveC.photonView.ownerId, (float)PhotonNetwork.time });
			}
		}
	}

	private void RemoveFromList(GameObject _player)
	{
		Player_move_c component = _player.GetComponent<SkinName>().playerMoveC;
		if (this.capturePlayers.Contains(component))
		{
			this.capturePlayers.Remove(component);
		}
	}

	[PunRPC]
	[RPC]
	private void RemovePlayerInCapturePoint(int _viewId, float _time)
	{
		int num = 0;
		while (num < Initializer.players.Count)
		{
			if (Initializer.players[num].photonView.ownerId != _viewId)
			{
				num++;
			}
			else
			{
				if (this.capturePlayers.Contains(Initializer.players[num]))
				{
					this.capturePlayers.Remove(Initializer.players[num]);
				}
				break;
			}
		}
	}

	[PunRPC]
	[RPC]
	public void SinchCapture(int command)
	{
		if (command != 1)
		{
			this.captureCounter = -200f;
			this.captureConmmand = BasePointController.TypeCapture.red;
		}
		else
		{
			this.captureCounter = 200f;
			this.captureConmmand = BasePointController.TypeCapture.blue;
		}
	}

	private void Start()
	{
		this.rayPoint.SetColors(Color.gray, Color.gray);
		this.myLabelController = NickLabelStack.sharedStack.GetNextCurrentLabel();
		this.myLabelController.StartShow(NickLabelController.TypeNickLabel.Point, base.transform);
	}

	[PunRPC]
	[RPC]
	private void SynchCaptureCounter(float _captureCounter)
	{
		this.captureCounter = _captureCounter;
	}

	[PunRPC]
	[RPC]
	public void SynchCaptureCounterNewPlayer(int _viewId, bool isMaster, float _captureCounter, int _captureCommand)
	{
		if (this.isStartUpdateFromMasterClient || PhotonNetwork.player.ID != _viewId)
		{
			return;
		}
		this.SynchCaptureCounter(this.captureCounter);
		this.captureConmmand = (BasePointController.TypeCapture)_captureCommand;
		this.isStartUpdateFromMasterClient = isMaster;
	}

	private void Update()
	{
		bool flag;
		Color color;
		float single;
		float single1;
		if (this.isStartUpdateFromMasterClient && !PhotonNetwork.connected)
		{
			this.isStartUpdateFromMasterClient = false;
		}
		int num = 0;
		int num1 = 0;
		bool flag1 = false;
		if (this.isBaseActive)
		{
			for (int i = 0; i < this.capturePlayers.Count; i++)
			{
				if (this.capturePlayers[i].myCommand == 1)
				{
					num++;
				}
				else if (this.capturePlayers[i].myCommand == 2)
				{
					num1++;
				}
				if (this.capturePlayers[i].Equals(WeaponManager.sharedManager.myPlayerMoveC))
				{
					flag1 = true;
				}
			}
			if (num1 == 0 && num > 0)
			{
				if (num == 1)
				{
					single1 = 1f;
				}
				else if (num != 2)
				{
					single1 = (num != 3 ? 1.3f : 1.2f);
				}
				else
				{
					single1 = 1.1f;
				}
				float single2 = single1;
				BasePointController basePointController = this;
				basePointController.captureCounter = basePointController.captureCounter + Time.deltaTime * single2 * 100f / 5f;
				if (this.captureCounter > 100f)
				{
					this.captureCounter = 100f;
				}
			}
			if (num == 0 && num1 > 0)
			{
				if (num1 == 1)
				{
					single = 1f;
				}
				else if (num1 != 2)
				{
					single = (num1 != 3 ? 1.3f : 1.2f);
				}
				else
				{
					single = 1.1f;
				}
				float single3 = single;
				BasePointController basePointController1 = this;
				basePointController1.captureCounter = basePointController1.captureCounter - Time.deltaTime * single3 * 100f / 5f;
				if (this.captureCounter < -100f)
				{
					this.captureCounter = -100f;
				}
			}
		}
		if (WeaponManager.sharedManager.myNetworkStartTable != null)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable.myCommand != 0)
			{
				color = ((this.captureCounter <= 0f || WeaponManager.sharedManager.myNetworkStartTable.myCommand != 1) && (this.captureCounter >= 0f || WeaponManager.sharedManager.myNetworkStartTable.myCommand != 2) ? Color.red : Color.blue);
			}
			else
			{
				color = Color.gray;
			}
			this.myLabelController.pointFillSprite.color = color;
		}
		this.myLabelController.pointFillSprite.fillAmount = Mathf.Abs(this.captureCounter) / 100f;
		if (this.captureCounter > 0f && this.captureConmmand == BasePointController.TypeCapture.red && !this.isSendMessageRaiderBlue)
		{
			this.isSendMessageRaiderBlue = true;
			this.isSendMessageRaiderRed = false;
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1784"), " ", this.nameBase));
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1783"), " ", this.nameBase));
				}
			}
		}
		if (this.captureCounter < 0f && this.captureConmmand == BasePointController.TypeCapture.blue && !this.isSendMessageRaiderRed)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1784"), " ", this.nameBase));
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1783"), " ", this.nameBase));
				}
			}
			this.isSendMessageRaiderBlue = false;
			this.isSendMessageRaiderRed = true;
		}
		if (this.captureCounter > 99.9f)
		{
			if (this.captureConmmand != BasePointController.TypeCapture.blue)
			{
				this.photonView.RPC("SinchCapture", PhotonTargets.Others, new object[] { 1 });
			}
			if (WeaponManager.sharedManager.myPlayerMoveC != null && !this.isSendMessageCaptureBlue)
			{
				this.isSendMessageCaptureRed = false;
				this.isSendMessageCaptureBlue = true;
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1785"), " ", this.nameBase));
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1781"), " ", this.nameBase));
				}
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1 && flag1)
				{
					QuestMediator.NotifyCapture(ConnectSceneNGUIController.RegimGame.CapturePoints);
					if (num != 1)
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.teamCapturePoint, 1f);
					}
					else
					{
						WeaponManager.sharedManager.myPlayerMoveC.SendMySpotEvent();
					}
				}
			}
			if (WeaponManager.sharedManager.myPlayerMoveC == null)
			{
				this.rayPoint.SetColors(Color.white, Color.white);
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1)
			{
				this.rayPoint.SetColors(Color.red, Color.red);
			}
			else
			{
				this.rayPoint.SetColors(Color.blue, Color.blue);
			}
			this.captureConmmand = BasePointController.TypeCapture.blue;
		}
		if (this.captureCounter < -99.9f)
		{
			if (this.captureConmmand != BasePointController.TypeCapture.red)
			{
				this.photonView.RPC("SinchCapture", PhotonTargets.Others, new object[] { 2 });
			}
			if (WeaponManager.sharedManager.myPlayerMoveC != null && !this.isSendMessageCaptureRed)
			{
				this.isSendMessageCaptureRed = true;
				this.isSendMessageCaptureBlue = false;
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2)
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1785"), " ", this.nameBase));
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(LocalizationStore.Get("Key_1781"), " ", this.nameBase));
				}
				if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2 && flag1)
				{
					QuestMediator.NotifyCapture(ConnectSceneNGUIController.RegimGame.CapturePoints);
					if (num1 != 1)
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.teamCapturePoint, 1f);
					}
					else
					{
						WeaponManager.sharedManager.myPlayerMoveC.SendMySpotEvent();
					}
				}
			}
			if (WeaponManager.sharedManager.myPlayerMoveC == null)
			{
				this.rayPoint.SetColors(Color.white, Color.white);
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2)
			{
				this.rayPoint.SetColors(Color.red, Color.red);
			}
			else
			{
				this.rayPoint.SetColors(Color.blue, Color.blue);
			}
			this.captureConmmand = BasePointController.TypeCapture.red;
		}
		if (this.myPlayerOnPoint && InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.pointCaptureBar.SetActive(this.isBaseActive);
			InGameGUI.sharedInGameGUI.pointCaptureName.text = this.nameBase;
			InGameGUI.sharedInGameGUI.captureBarSprite.fillAmount = Mathf.Abs(this.captureCounter) / 100f;
			if (this.captureCounter <= 0f || WeaponManager.sharedManager.myNetworkStartTable.myCommand != 1)
			{
				flag = (this.captureCounter >= 0f ? false : WeaponManager.sharedManager.myNetworkStartTable.myCommand == 2);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			InGameGUI.sharedInGameGUI.captureBarSprite.color = (!flag2 ? this.redTeamColor : this.blueTeamColor);
			InGameGUI.sharedInGameGUI.teamColorSprite.color = (!flag2 ? this.redCaptureColor : this.blueCaptureColor);
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.timerToSynch -= Time.deltaTime;
			if (this.timerToSynch <= 0f)
			{
				this.timerToSynch = this.periodToSynch;
				this.photonView.RPC("SynchCaptureCounter", PhotonTargets.Others, new object[] { this.captureCounter });
			}
		}
	}

	public enum TypeCapture
	{
		none,
		blue,
		red
	}
}