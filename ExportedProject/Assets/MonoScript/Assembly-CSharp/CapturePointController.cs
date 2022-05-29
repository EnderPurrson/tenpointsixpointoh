using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CapturePointController : MonoBehaviour
{
	public static CapturePointController sharedController;

	public PhotonView photonView;

	public float scoreBlue;

	public float scoreRed;

	private float speedScoreAdd = 3f;

	private float maxScoreCommands = 1000f;

	private bool isStartUpdateFromMasterClient;

	private float periodToSynch = 1f;

	private float timerToSynch;

	public BasePointController[] basePointControllers;

	private Transform myTransform;

	public bool isEndMatch;

	public CapturePointController()
	{
	}

	private void Awake()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	public void EndMatch()
	{
		Debug.Log("EndMatch");
		int num = 0;
		if (this.scoreBlue > this.scoreRed)
		{
			num = 1;
		}
		if (this.scoreRed > this.scoreBlue)
		{
			num = 2;
		}
		if (TimeGameController.sharedController.isEndMatch || this.isEndMatch)
		{
			Debug.Log("EndMatch in table!");
		}
		else
		{
			this.isEndMatch = true;
			if (WeaponManager.sharedManager.myTable != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, num, Mathf.RoundToInt(this.scoreBlue), Mathf.RoundToInt(this.scoreRed));
			}
		}
		this.scoreRed = 0f;
		this.scoreBlue = 0f;
		for (int i = 0; i < (int)this.basePointControllers.Length; i++)
		{
			this.basePointControllers[i].OnEndMatch();
		}
	}

	private void OnDestroy()
	{
		CapturePointController.sharedController = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		this.photonView.RPC("SynchScoresCommandsNewPlayerRPC", player, new object[] { player.ID, PhotonNetwork.isMasterClient, this.scoreBlue, this.scoreRed });
	}

	private void Start()
	{
		if (!Defs.isCapturePoints || !Defs.isMulti)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (PhotonNetwork.room != null)
		{
			int item = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty];
			if (item == 3)
			{
				this.speedScoreAdd = 3f;
			}
			if (item == 5)
			{
				this.speedScoreAdd = 2f;
			}
			if (item == 7)
			{
				this.speedScoreAdd = 1.5f;
			}
		}
		CapturePointController.sharedController = this;
		this.myTransform = base.transform;
		this.basePointControllers = new BasePointController[this.myTransform.childCount];
		for (int i = 0; i < this.myTransform.childCount; i++)
		{
			this.basePointControllers[i] = this.myTransform.GetChild(i).GetComponent<BasePointController>();
		}
	}

	[PunRPC]
	[RPC]
	public void SynchScoresCommandsNewPlayerRPC(int _viewId, bool isMaster, float _scoreBlue, float _scoreRed)
	{
		if (this.isStartUpdateFromMasterClient || PhotonNetwork.player.ID != _viewId)
		{
			return;
		}
		this.SynchScoresCommandsRPC(_scoreBlue, _scoreRed);
		this.isStartUpdateFromMasterClient = isMaster;
	}

	[PunRPC]
	[RPC]
	public void SynchScoresCommandsRPC(float _scoreBlue, float _scoreRed)
	{
		this.scoreBlue = _scoreBlue;
		this.scoreRed = _scoreRed;
	}

	private void Update()
	{
		if (this.isStartUpdateFromMasterClient && !PhotonNetwork.connected)
		{
			this.isStartUpdateFromMasterClient = false;
		}
		if (Initializer.bluePlayers.Count == 0 || Initializer.redPlayers.Count == 0)
		{
			if (InGameGUI.sharedInGameGUI != null && !InGameGUI.sharedInGameGUI.message_wait.activeSelf)
			{
				InGameGUI.sharedInGameGUI.message_wait.SetActive(true);
			}
			for (int i = 0; i < (int)this.basePointControllers.Length; i++)
			{
				this.basePointControllers[i].isBaseActive = false;
				if (this.basePointControllers[i].baseRender != null && this.basePointControllers[i].baseRender.activeSelf)
				{
					this.basePointControllers[i].baseRender.SetActive(false);
				}
			}
			return;
		}
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.message_wait.activeSelf)
		{
			InGameGUI.sharedInGameGUI.message_wait.SetActive(false);
		}
		for (int j = 0; j < (int)this.basePointControllers.Length; j++)
		{
			this.basePointControllers[j].isBaseActive = true;
			if (this.basePointControllers[j].baseRender != null && !this.basePointControllers[j].baseRender.activeSelf)
			{
				this.basePointControllers[j].baseRender.SetActive(true);
			}
		}
		int num = 0;
		int num1 = 0;
		for (int k = 0; k < (int)this.basePointControllers.Length; k++)
		{
			if (this.basePointControllers[k].captureConmmand == BasePointController.TypeCapture.blue)
			{
				num++;
			}
			if (this.basePointControllers[k].captureConmmand == BasePointController.TypeCapture.red)
			{
				num1++;
			}
		}
		CapturePointController capturePointController = this;
		capturePointController.scoreBlue = capturePointController.scoreBlue + Time.deltaTime * this.speedScoreAdd * (float)num;
		CapturePointController capturePointController1 = this;
		capturePointController1.scoreRed = capturePointController1.scoreRed + Time.deltaTime * this.speedScoreAdd * (float)num1;
		if (this.scoreBlue > this.maxScoreCommands)
		{
			this.scoreBlue = this.maxScoreCommands;
			this.EndMatch();
		}
		if (this.scoreRed > this.maxScoreCommands)
		{
			this.scoreRed = this.maxScoreCommands;
			this.EndMatch();
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.timerToSynch -= Time.deltaTime;
			if (this.timerToSynch <= 0f)
			{
				this.timerToSynch = this.periodToSynch;
				this.photonView.RPC("SynchScoresCommandsRPC", PhotonTargets.All, new object[] { this.scoreBlue, this.scoreRed });
			}
		}
	}
}