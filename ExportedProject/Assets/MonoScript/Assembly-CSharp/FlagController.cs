using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour
{
	public bool isBlue;

	public int masterServerID;

	private PhotonView photonView;

	public bool isCapture;

	private int idCapturePlayer;

	public bool isBaza;

	private GameObject myBaza;

	public GameObject rayBlue;

	public GameObject rayRed;

	public float timerToBaza = 10f;

	private float maxTimerToBaza = 10f;

	public GameObject flagModelRed;

	public GameObject flagModelBlue;

	public Transform targetTrasform;

	private InGameGUI inGameGui;

	public GameObject pointObjTexture;

	private GameObject _objBazaTexture;

	private GameObject _objFlagTexture;

	public GameObject flagModel;

	private FlagPedestalController pedistal;

	private int currentColor = -1;

	public FlagController()
	{
	}

	private void Awake()
	{
		GameObject gameObject = Resources.Load("FlagPedestal") as GameObject;
		if (!this.isBlue)
		{
			this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
			Initializer.flag2 = this;
		}
		else
		{
			this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
			Initializer.flag1 = this;
		}
		GameObject gameObject1 = UnityEngine.Object.Instantiate(gameObject, this.myBaza.transform.position, this.myBaza.transform.rotation) as GameObject;
		this.pedistal = gameObject1.GetComponent<FlagPedestalController>();
		this._objBazaTexture = UnityEngine.Object.Instantiate(Resources.Load("ObjectPictFlag") as GameObject, this.myBaza.transform.position, this.myBaza.transform.rotation) as GameObject;
		this._objFlagTexture = UnityEngine.Object.Instantiate(Resources.Load("ObjectPictFlag") as GameObject, this.myBaza.transform.position, this.myBaza.transform.rotation) as GameObject;
		this._objBazaTexture.GetComponent<ObjectPictFlag>().target = gameObject1.transform.GetChild(0);
		this._objBazaTexture.GetComponent<ObjectPictFlag>().isBaza = true;
		this._objBazaTexture.GetComponent<ObjectPictFlag>().myFlagController = this;
		this._objFlagTexture.GetComponent<ObjectPictFlag>().target = this.pointObjTexture.transform;
		this.SetColor(0);
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	public void GoBaza()
	{
		this.timerToBaza = this.maxTimerToBaza;
		this.photonView.RPC("GoBazaRPC", PhotonTargets.All, new object[0]);
	}

	[PunRPC]
	[RPC]
	public void GoBazaRPC()
	{
		Debug.Log("GoBazaRPC");
		this.isBaza = true;
		this.isCapture = false;
		this.idCapturePlayer = -1;
		this.targetTrasform = null;
		base.transform.position = this.myBaza.transform.position;
		base.transform.rotation = this.myBaza.transform.rotation;
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this._objBazaTexture);
		UnityEngine.Object.Destroy(this._objFlagTexture);
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView == null)
		{
			Debug.Log("FlagController.OnPhotonPlayerConnected():    photonView == null");
			return;
		}
		if (!this.isCapture)
		{
			this.photonView.RPC("SetNOCaptureRPCNewPlayer", player, new object[] { player.ID, base.transform.position, base.transform.rotation, this.isBaza });
		}
		else
		{
			this.photonView.RPC("SetCaptureRPCNewPlayer", player, new object[] { player.ID, this.idCapturePlayer });
		}
	}

	public void SetCapture(int _viewIdCapture)
	{
		this.photonView.RPC("SetCaptureRPC", PhotonTargets.All, new object[] { _viewIdCapture });
	}

	[PunRPC]
	[RPC]
	public void SetCaptureRPC(int _viewIdCapture)
	{
		this.isBaza = false;
		this.idCapturePlayer = _viewIdCapture;
		this.isCapture = true;
		foreach (Player_move_c player in Initializer.players)
		{
			if (player.mySkinName.photonView.ownerId != _viewIdCapture)
			{
				continue;
			}
			this.targetTrasform = player.flagPoint.transform;
			player.isCaptureFlag = true;
		}
	}

	[PunRPC]
	[RPC]
	public void SetCaptureRPCNewPlayer(int idNewPlayer, int _viewIdCapture)
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
		if (PhotonNetwork.player.ID == idNewPlayer)
		{
			this.SetCaptureRPC(_viewIdCapture);
		}
	}

	public void SetColor(int _color)
	{
		if (_color == this.currentColor)
		{
			return;
		}
		this.currentColor = _color;
		this.pedistal.SetColor(_color);
		this.flagModelRed.SetActive(_color == 2);
		this.flagModelBlue.SetActive(_color == 1);
		this.rayRed.SetActive(_color == 2);
		this.rayBlue.SetActive(_color == 1);
		if (_color <= 0)
		{
			this._objBazaTexture.GetComponent<ObjectPictFlag>().SetTexture(null);
			this._objFlagTexture.GetComponent<ObjectPictFlag>().SetTexture(null);
		}
		else
		{
			this._objBazaTexture.GetComponent<ObjectPictFlag>().SetTexture(Resources.Load((_color != 1 ? "red_base" : "blue_base")) as Texture2D);
			this._objFlagTexture.GetComponent<ObjectPictFlag>().SetTexture(Resources.Load((_color != 1 ? "red_flag" : "blue_flag")) as Texture2D);
		}
	}

	[PunRPC]
	[RPC]
	public void SetMasterSeverIDRPC(int _id)
	{
		this.masterServerID = _id;
	}

	public void SetNOCapture(Vector3 pos, Quaternion rot)
	{
		this.photonView.RPC("SetNOCaptureRPC", PhotonTargets.All, new object[] { pos, rot });
		this.timerToBaza = this.maxTimerToBaza;
	}

	[PunRPC]
	[RPC]
	public void SetNOCaptureRPC(Vector3 pos, Quaternion rot)
	{
		this.isCapture = false;
		this.idCapturePlayer = -1;
		if (this.targetTrasform != null)
		{
			this.targetTrasform.parent.GetComponent<SkinName>().playerMoveC.isCaptureFlag = false;
		}
		this.targetTrasform = null;
	}

	[PunRPC]
	[RPC]
	public void SetNOCaptureRPCNewPlayer(int idNewPlayer, Vector3 pos, Quaternion rot, bool _isBaza)
	{
		if (this.photonView == null)
		{
			this.photonView = base.GetComponent<PhotonView>();
		}
		if (this.photonView != null && this.photonView.ownerId == idNewPlayer)
		{
			this.isBaza = _isBaza;
			this.SetNOCaptureRPC(pos, rot);
		}
	}

	private void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.photonView.RPC("SetMasterSeverIDRPC", PhotonTargets.AllBuffered, new object[] { this.photonView.viewID });
	}

	private void Update()
	{
		int num;
		if (this.inGameGui == null)
		{
			this.inGameGui = InGameGUI.sharedInGameGUI;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			num = ((WeaponManager.sharedManager.myPlayerMoveC.myCommand != 1 || !this.isBlue) && (WeaponManager.sharedManager.myPlayerMoveC.myCommand != 2 || this.isBlue) ? 2 : 1);
		}
		else
		{
			num = 0;
		}
		this.SetColor(num);
		if (this.rayBlue.activeInHierarchy == this.isCapture)
		{
			this.rayBlue.SetActive(!this.isCapture);
		}
		if (this.rayRed.activeInHierarchy == this.isCapture)
		{
			this.rayRed.SetActive(!this.isCapture);
		}
		if (this.targetTrasform == null)
		{
			this.isCapture = false;
		}
		else
		{
			base.transform.position = this.targetTrasform.position;
			base.transform.rotation = this.targetTrasform.rotation;
		}
		int num1 = 0;
		int num2 = 0;
		foreach (Player_move_c player in Initializer.players)
		{
			if (player == null)
			{
				continue;
			}
			int num3 = player.myCommand;
			if (num3 == 1)
			{
				num1++;
			}
			if (num3 != 2)
			{
				continue;
			}
			num2++;
		}
		if ((num1 == 0 || num2 == 0) && this.flagModel.activeSelf)
		{
			this.flagModel.SetActive(false);
		}
		if (this.inGameGui != null && (num1 == 0 || num2 == 0) && !this.inGameGui.message_wait.activeSelf)
		{
			this.inGameGui.message_wait.SetActive(true);
			this.inGameGui.timerShowNow = 0f;
		}
		if (this.inGameGui != null && num1 != 0 && num2 != 0 && this.inGameGui.message_wait.activeSelf)
		{
			this.inGameGui.message_wait.SetActive(false);
			this.inGameGui.timerShowNow = 3f;
		}
		if (num1 != 0 && num2 != 0 && !this.flagModel.activeSelf)
		{
			this.flagModel.SetActive(true);
		}
		if ((num1 == 0 || num2 == 0) && this.isCapture)
		{
			foreach (Player_move_c playerMoveC in Initializer.players)
			{
				if (this.idCapturePlayer == playerMoveC.mySkinName.photonView.ownerId)
				{
					playerMoveC.isCaptureFlag = false;
				}
				this.GoBaza();
			}
		}
		if (PhotonNetwork.isMasterClient && !this.isCapture && !this.isBaza)
		{
			this.timerToBaza -= Time.deltaTime;
			if (this.timerToBaza < 0f)
			{
				this.GoBaza();
				if (WeaponManager.sharedManager.myPlayer != null)
				{
					WeaponManager.sharedManager.myPlayerMoveC.SendSystemMessegeFromFlagReturned(this.isBlue);
				}
			}
		}
	}
}