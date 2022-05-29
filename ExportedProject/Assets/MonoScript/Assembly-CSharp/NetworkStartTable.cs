using ExitGames.Client.Photon;
using FyberPlugin;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public sealed class NetworkStartTable : MonoBehaviour
{
	public string pixelBookID = "-1";

	private SaltedInt _scoreCommandFlag1 = new SaltedInt(818919);

	private SaltedInt _scoreCommandFlag2 = new SaltedInt(823016);

	public double timerFlag;

	private float maxTimerFlag = 150f;

	private float timerUpdateTimerFlag;

	private float maxTimerUpdateTimerFlag = 1f;

	public bool isShowAvard;

	private bool isShowFinished;

	private bool isEndInHunger;

	private int addExperience;

	public GameObject guiObj;

	public NetworkStartTableNGUIController networkStartTableNGUIController;

	public bool isRegimVidos;

	private int numberPlayerCun;

	private int numberPlayerCunId;

	public Player_move_c currentPlayerMoveCVidos;

	private bool oldIsZomming;

	private InGameGUI inGameGUI;

	public string playerVidosNick;

	public string playerVidosClanName;

	public Texture playerVidosClanTexture;

	public GameObject currentCamPlayer;

	public GameObject currentFPSPlayer;

	private GameObject currentBodyMech;

	public GameObject currentGameObjectPlayer;

	public bool isGoRandomRoom;

	public Texture mySkin;

	public List<GameObject> zombiePrefabs = new List<GameObject>();

	private GameObject _playerPrefab;

	public GameObject tempCam;

	public GameObject zombieManagerPrefab;

	public Texture2D serverLeftTheGame;

	public ExperienceController experienceController;

	private int addCoins;

	public bool isDeadInHungerGame;

	private bool showMessagFacebook;

	private bool clickButtonFacebook;

	public bool isIwin;

	public int myCommand;

	public int myCommandOld;

	private bool isLocal;

	private bool isMine;

	private bool isCOOP;

	private bool isServer;

	private bool isCompany;

	private bool isMulti;

	private bool isInet;

	private float timeNotRunZombiManager;

	private bool isSendZaprosZombiManager;

	private bool isGetZaprosZombiManager;

	private ExperienceController expController;

	public Texture myClanTexture;

	public string myClanID = string.Empty;

	public string myClanName = string.Empty;

	public string myClanLeaderID = string.Empty;

	private LANBroadcastService lanScan;

	private bool isSetNewMapButton;

	public bool isDrawInHanger;

	public List<NetworkStartTable.infoClient> players = new List<NetworkStartTable.infoClient>();

	public GUIStyle labelStyle;

	public GUIStyle messagesStyle;

	public GUIStyle ozidanieStyle;

	private Vector2 scrollPosition = Vector2.zero;

	private float koofScreen = (float)Screen.height / 768f;

	public WeaponManager _weaponManager;

	public bool _showTable;

	public string nickPobeditelya;

	public bool _isShowNickTable;

	public bool runGame = true;

	public GameObject[] zoneCreatePlayer;

	private GameObject _cam;

	public bool isDrawInDeathMatch;

	public bool showDisconnectFromServer;

	public bool showDisconnectFromMasterServer;

	private float timerShow = -1f;

	public string NamePlayer = "Player";

	public int CountKills;

	public int oldCountKills;

	public string[] oldSpisokName;

	public string[] oldCountLilsSpisok;

	public string[] oldScoreSpisok;

	public int[] oldSpisokRanks;

	public string[] oldSpisokNameBlue;

	public string[] oldCountLilsSpisokBlue;

	public int[] oldSpisokRanksBlue;

	public string[] oldSpisokNameRed;

	public string[] oldCountLilsSpisokRed;

	public string[] oldScoreSpisokRed;

	public string[] oldScoreSpisokBlue;

	public int[] oldSpisokRanksRed;

	public bool[] oldIsDeadInHungerGame;

	public string[] oldSpisokPixelBookID;

	public string[] oldSpisokPixelBookIDBlue;

	public string[] oldSpisokPixelBookIDRed;

	public Texture[] oldSpisokMyClanLogo;

	public Texture[] oldSpisokMyClanLogoBlue;

	public Texture[] oldSpisokMyClanLogoRed;

	public int oldIndexMy;

	private GameObject tc;

	public int scoreOld;

	private PhotonView photonView;

	private float timeTomig = 0.5f;

	private float timerSynchScore = -1f;

	private int countMigZagolovok;

	private int commandWinner;

	private bool isMigZag;

	private HungerGameController hungerGameController;

	private bool _canUserUseFacebookComposer;

	private bool _hasPublishPermission;

	private bool _hasPublishActions;

	private SaltedInt _score = new SaltedInt();

	private static System.Random _prng;

	public int myRanks = 1;

	public Player_move_c myPlayerMoveC;

	private bool isHunger;

	private int _gameRating = -1;

	private ShopNGUIController _shopInstance;

	private int playerCountInHunger;

	private bool isStartPlayerCoroutine;

	private string waitingPlayerLocalize;

	private string matchLocalize;

	private string preparingLocalize;

	private Pauser _pauser;

	private Stopwatch _matchStopwatch = new Stopwatch();

	private int killCountMatch;

	private int deathCountMatch;

	public static Vector2 ExperiencePosRanks
	{
		get
		{
			return new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		}
	}

	public int gameRating
	{
		get
		{
			return (!Defs.isMulti || !this.isMine ? this._gameRating : RatingSystem.instance.currentRating);
		}
		set
		{
			this._gameRating = value;
		}
	}

	public bool isShowNickTable
	{
		get
		{
			return this._isShowNickTable;
		}
		set
		{
			this._isShowNickTable = value;
			if (this.isMine)
			{
				Defs.showNickTableInNetworkStartTable = value;
			}
		}
	}

	public int score
	{
		get
		{
			return this._score.Value;
		}
		set
		{
			this._score = new SaltedInt(NetworkStartTable._prng.Next(), value);
		}
	}

	public int scoreCommandFlag1
	{
		get
		{
			return this._scoreCommandFlag1.Value;
		}
		set
		{
			this._scoreCommandFlag1 = value;
		}
	}

	public int scoreCommandFlag2
	{
		get
		{
			return this._scoreCommandFlag2.Value;
		}
		set
		{
			this._scoreCommandFlag2 = value;
		}
	}

	public bool showTable
	{
		get
		{
			return this._showTable;
		}
		set
		{
			this._showTable = value;
			if (this.isMine)
			{
				Defs.showTableInNetworkStartTable = value;
			}
		}
	}

	static NetworkStartTable()
	{
		NetworkStartTable._prng = new System.Random(19937);
	}

	public NetworkStartTable()
	{
	}

	private string _SocialMessage()
	{
		int num = Storager.getInt(Defs.COOPScore, false);
		bool flag = Defs.isCOOP;
		int num1 = Storager.getInt("Rating", false);
		string str = "http://goo.gl/dQMf4n";
		if (this.isIwin)
		{
			return (!flag ? string.Format("I won the match in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", num1, str) : string.Format(" Now I have {0} score in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", num, str));
		}
		return (!flag ? string.Format("I played a match in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", num1, str) : string.Format("I received {0} points in @PixelGun3D ! Try it right now!\n#pixelgun3d #pg3d #pixelgun", num, str));
	}

	private string _SocialSentSuccess(string SocialName)
	{
		return string.Concat("Message was sent to ", SocialName);
	}

	private void AddFlag()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
		GameObject gameObject1 = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
		PhotonNetwork.InstantiateSceneObject("Flags/Flag1", gameObject.transform.position, gameObject.transform.rotation, 0, null);
		PhotonNetwork.InstantiateSceneObject("Flags/Flag2", gameObject1.transform.position, gameObject1.transform.rotation, 0, null);
	}

	[PunRPC]
	[RPC]
	private void AddPaticleBazeRPC(int _command)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag(string.Concat("BazaZoneCommand", _command));
		UnityEngine.Object obj = Resources.Load((_command != WeaponManager.sharedManager.myNetworkStartTable.myCommand ? "Ring_Particle_Red" : "Ring_Particle_Blue"));
		float single = gameObject.transform.position.x;
		Vector3 vector3 = gameObject.transform.position;
		Vector3 vector31 = gameObject.transform.position;
		UnityEngine.Object.Instantiate(obj, new Vector3(single, vector3.y + 0.22f, vector31.z), gameObject.transform.rotation);
	}

	public void AddScore()
	{
		this.CountKills++;
		GlobalGameController.CountKills = this.CountKills;
		this.photonView.RPC("AddPaticleBazeRPC", PhotonTargets.All, new object[] { this.myCommand });
		if (this.myCommand != 1)
		{
			this.photonView.RPC("SynchScoreCommandRPC", PhotonTargets.All, new object[] { 2, this.scoreCommandFlag2 + 1 });
		}
		else
		{
			this.photonView.RPC("SynchScoreCommandRPC", PhotonTargets.All, new object[] { 1, this.scoreCommandFlag1 + 1 });
		}
		this.SynhCountKills(null);
	}

	private void Awake()
	{
		this.isLocal = !Defs.isInet;
		this.isInet = Defs.isInet;
		this.isCOOP = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle;
		if (!this.isInet)
		{
			this.isServer = PlayerPrefs.GetString("TypeGame").Equals("server");
		}
		else
		{
			this.isServer = PhotonNetwork.isMasterClient;
		}
		this.isMulti = Defs.isMulti;
		this.isCompany = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight;
		this.isHunger = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames;
		this.experienceController = GameObject.FindGameObjectWithTag("ExperienceController").GetComponent<ExperienceController>();
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			string[] strArrays = null;
			strArrays = new string[] { "1", "15", "14", "2", "3", "9", "11", "12", "10", "16" };
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				GameObject gameObject = Resources.Load(string.Concat("Enemies/Enemy", strArrays[i], "_go")) as GameObject;
				this.zombiePrefabs.Add(gameObject);
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.maxTimerFlag = (float)int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString()) * 60f;
		}
		this.photonView = PhotonView.Get(this);
		Initializer.networkTables.Add(this);
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
	}

	public void BackButtonPress()
	{
		if (ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		NetworkStartTableNGUIController networkStartTableNGUIController = NetworkStartTableNGUIController.sharedController;
		if (networkStartTableNGUIController != null && networkStartTableNGUIController.CheckHideInternalPanel())
		{
			return;
		}
		this.networkStartTableNGUIController.shopAnchor.SetActive(false);
		this.RemoveShop(true);
		if (this.isInet)
		{
			ActivityIndicator.IsActiveIndicator = false;
			Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
			PhotonNetwork.Disconnect();
		}
		else
		{
			if (this.isServer)
			{
				Network.Disconnect(200);
				GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
			}
			else if ((int)Network.connections.Length == 1)
			{
				UnityEngine.Debug.Log(string.Concat(new object[] { "Disconnecting: ", Network.connections[0].ipAddress, ":", Network.connections[0].port }));
				Network.CloseConnection(Network.connections[0], true);
			}
			ActivityIndicator.IsActiveIndicator = false;
			ConnectSceneNGUIController.Local();
		}
		if (EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Paused || EveryplayWrapper.Instance.CurrentState == EveryplayWrapper.State.Recording)
		{
			EveryplayWrapper.Instance.Stop();
		}
	}

	public RatingSystem.RatingChange CalculateMatchRating(bool disconnecting)
	{
		RatingSystem.RatingChange ratingChange = RatingSystem.instance.currentRatingChange;
		if (NetworkStartTable.LocalOrPasswordRoom() || !RatingSystem.instance.ratingMatch || !FriendsController.isUseRatingSystem)
		{
			return ratingChange;
		}
		if (Defs.isHunger && this.isDeadInHungerGame)
		{
			return ratingChange;
		}
		if (Defs.isDaterRegim)
		{
			return ratingChange;
		}
		int placeInTable = this.GetPlaceInTable();
		int count = Initializer.networkTables.Count;
		int winningTeam = this.GetWinningTeam();
		bool num = this.CheckForWin(placeInTable, winningTeam, this.CountKills, this.score, false);
		if (this.myPlayerMoveC != null && (Defs.isHunger || this.score > 0 || this.myPlayerMoveC.killedInMatch))
		{
			List<int> nums = new List<int>();
			if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
			{
				int num1 = 0;
				int num2 = 0;
				for (int i = 0; i < Initializer.networkTables.Count; i++)
				{
					if (Initializer.networkTables[i].myCommand != 1)
					{
						num1++;
					}
					else
					{
						num2++;
					}
				}
				int num3 = (num1 <= num2 ? num2 : num1);
				count = num3 * 2;
				placeInTable = placeInTable + (num ? 0 : num3);
			}
			else if (Defs.isHunger)
			{
				placeInTable = (!num ? Initializer.players.Count - 1 : 0);
				num = placeInTable < Mathf.CeilToInt((float)(this.playerCountInHunger / 2));
			}
			else
			{
				num = placeInTable < Mathf.CeilToInt((float)(count / 2));
			}
			UnityEngine.Debug.Log(string.Format("<color=orange>My place: {0}, team winner: {1}, rating winner - {2}</color>", placeInTable.ToString(), winningTeam.ToString(), num.ToString()));
			if (!num && !Defs.isHunger && this.myPlayerMoveC.liveTime < 60f)
			{
				return ratingChange;
			}
			if (disconnecting && num)
			{
				return ratingChange;
			}
			if (Defs.isHunger && this.CountKills <= 0 && num)
			{
				return ratingChange;
			}
			if (!Defs.isHunger)
			{
				ratingChange = (this.isCompany || Defs.isFlag || Defs.isCapturePoints ? RatingSystem.instance.CalculateRating(count, placeInTable, this.GetMatchKillrate(), winningTeam == 0) : RatingSystem.instance.CalculateRating(count, placeInTable, this.GetMatchKillrate(), false));
			}
			else
			{
				ratingChange = RatingSystem.instance.CalculateRating(this.playerCountInHunger, Mathf.Clamp(placeInTable, 0, this.playerCountInHunger - 1), this.GetMatchKillrate(), false);
			}
		}
		return ratingChange;
	}

	public RatingSystem.RatingChange CalculateMatchRatingOld(bool disconnecting)
	{
		RatingSystem.RatingChange ratingChange = RatingSystem.instance.currentRatingChange;
		if (NetworkStartTable.LocalOrPasswordRoom() || !RatingSystem.instance.ratingMatch || !FriendsController.isUseRatingSystem)
		{
			return ratingChange;
		}
		if (Defs.isHunger && this.isDeadInHungerGame)
		{
			return ratingChange;
		}
		int placeInTable = this.GetPlaceInTable();
		int winningTeam = this.GetWinningTeam();
		bool flag = this.CheckForWin(placeInTable, winningTeam, this.CountKills, this.score, false);
		if (this.myPlayerMoveC != null && (Defs.isHunger || this.score > 0 || this.myPlayerMoveC.killedInMatch))
		{
			List<int> nums = new List<int>();
			if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
			{
				for (int i = 0; i < Initializer.networkTables.Count; i++)
				{
					if (Initializer.networkTables[i] != this)
					{
						nums.Add((Initializer.networkTables[i].gameRating == -1 ? this.gameRating : Initializer.networkTables[i].gameRating));
					}
				}
				if (nums.Count == 0)
				{
					nums.Add(this.gameRating);
				}
			}
			else if (!Defs.isHunger)
			{
				for (int j = 0; j < Initializer.networkTables.Count; j++)
				{
					if (Initializer.networkTables[j] != this)
					{
						nums.Add((Initializer.networkTables[j].gameRating == -1 ? this.gameRating : Initializer.networkTables[j].gameRating));
					}
				}
				if (nums.Count == 0)
				{
					return ratingChange;
				}
				float count = (float)Initializer.networkTables.Count / 2f;
				flag = (float)(placeInTable + 1) <= count;
				if (!flag)
				{
					placeInTable -= Mathf.FloorToInt(count);
				}
			}
			UnityEngine.Debug.Log(string.Format("<color=orange>My place: {0}, team winner: {1}, rating winner - {2}</color>", placeInTable.ToString(), winningTeam.ToString(), flag.ToString()));
			if (!flag && !Defs.isHunger && this.myPlayerMoveC.liveTime < 60f)
			{
				return ratingChange;
			}
			if (disconnecting && flag)
			{
				return ratingChange;
			}
		}
		return ratingChange;
	}

	public void CalculateMatchRatingOnDisconnect()
	{
		if (this.myPlayerMoveC != null && (!Defs.isCOOP && !Defs.isCompany && !Defs.isFlag && !Defs.isCapturePoints || this.myPlayerMoveC.liveTime > 90f))
		{
			this.CalculateMatchRating(true);
		}
	}

	private bool CheckForWin(int myPlace, int winnerTeam, int killCount, int myscore, bool scoreMatterForTeam = true)
	{
		bool flag;
		switch (ConnectSceneNGUIController.regim)
		{
			case ConnectSceneNGUIController.RegimGame.Deathmatch:
			case ConnectSceneNGUIController.RegimGame.TimeBattle:
			{
				return (myPlace != 0 ? false : myscore > 0);
			}
			case ConnectSceneNGUIController.RegimGame.TeamFight:
			case ConnectSceneNGUIController.RegimGame.FlagCapture:
			case ConnectSceneNGUIController.RegimGame.CapturePoints:
			{
				if (this.myCommand != winnerTeam)
				{
					flag = false;
				}
				else
				{
					flag = (myscore > 0 ? true : !scoreMatterForTeam);
				}
				return flag;
			}
			case ConnectSceneNGUIController.RegimGame.DeadlyGames:
			{
				return (killCount <= 0 ? false : this.isIwin);
			}
		}
		return false;
	}

	public void ClearKillrate()
	{
		this.killCountMatch = 0;
		this.deathCountMatch = 0;
	}

	[Obfuscation(Exclude=true)]
	public void ClearScoreCommandInFlagGame()
	{
		this.photonView.RPC("ClearScoreCommandInFlagGameRPC", PhotonTargets.Others, new object[0]);
	}

	[PunRPC]
	[RPC]
	public void ClearScoreCommandInFlagGameRPC()
	{
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().scoreCommandFlag1 = 0;
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().scoreCommandFlag2 = 0;
		}
	}

	private void completionHandler(string error, object result)
	{
		if (error == null)
		{
			Prime31.Utils.logObject(result);
			this.showMessagFacebook = true;
			base.Invoke("hideMessag", 3f);
		}
		else
		{
			UnityEngine.Debug.LogError(error);
		}
	}

	[PunRPC]
	[RPC]
	public void CreateChestRPC(Vector3 pos, Quaternion rot)
	{
		PhotonNetwork.InstantiateSceneObject("HungerGames/Chest", pos, rot, 0, null);
	}

	[Obfuscation(Exclude=true)]
	private void DestroyMyPlayer()
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			Network.RemoveRPCs(this._weaponManager.myPlayer.GetComponent<NetworkView>().viewID);
			Network.Destroy(this._weaponManager.myPlayer);
		}
	}

	public void DestroyPlayer()
	{
		this.isShowFinished = false;
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		if (this._cam != null)
		{
			this._cam.SetActive(true);
			this._cam.GetComponent<RPG_Camera>().enabled = false;
		}
		if (!this.isInet)
		{
			this.DestroyMyPlayer();
		}
		else if (this._weaponManager && this._weaponManager.myPlayer)
		{
			PhotonNetwork.Destroy(this._weaponManager.myPlayer);
		}
	}

	[DebuggerHidden]
	public IEnumerator DrawInHanger()
	{
		NetworkStartTable.u003cDrawInHangeru003ec__IteratorC4 variable = null;
		return variable;
	}

	[PunRPC]
	[RPC]
	public void DrawInHangerRPC()
	{
	}

	private void finishTable()
	{
		this.playersTable();
	}

	public float GetMatchKillrate()
	{
		if (Defs.isCOOP)
		{
			return 1f;
		}
		if (this.deathCountMatch == 0)
		{
			return (float)this.killCountMatch;
		}
		return (float)this.killCountMatch / (float)this.deathCountMatch;
	}

	private int GetMyCommandOnStart()
	{
		if (this.myCommand > 0)
		{
			return this.myCommand;
		}
		int num = 0;
		int num1 = 0;
		for (int i = 0; i < Initializer.networkTables.Count; i++)
		{
			if (Initializer.networkTables[i].myCommand == 1)
			{
				num++;
			}
			if (Initializer.networkTables[i].myCommand == 2)
			{
				num1++;
			}
		}
		if (num1 < num)
		{
			return 2;
		}
		if (num1 > num)
		{
			return 1;
		}
		float single = (!FriendsController.useBuffSystem ? KillRateCheck.instance.GetKillRate() : BuffSystem.instance.GetKillrateByInteractions());
		int winningTeam = this.GetWinningTeam();
		if (winningTeam == 0)
		{
			return UnityEngine.Random.Range(1, 3);
		}
		if (single < 1f)
		{
			return winningTeam;
		}
		return (winningTeam != 1 ? 1 : 2);
	}

	private void GetMyTeam()
	{
		if (this.isMine && !NetworkStartTable.LocalOrPasswordRoom() && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture))
		{
			this.myCommand = this.GetMyCommandOnStart();
			this.SynhCommand(null);
		}
	}

	public int GetPlaceInTable()
	{
		int num = 0;
		NetworkStartTable[] array = Initializer.networkTables.ToArray();
		for (int i = 1; i < (int)array.Length; i++)
		{
			NetworkStartTable networkStartTable = array[i];
			int num1 = 0;
			while (num1 < i)
			{
				NetworkStartTable networkStartTable1 = array[num1];
				if ((Defs.isFlag || Defs.isCapturePoints || networkStartTable.score <= networkStartTable1.score && (networkStartTable.score != networkStartTable1.score || networkStartTable.CountKills <= networkStartTable1.CountKills)) && (!Defs.isFlag && !Defs.isCapturePoints || networkStartTable.CountKills <= networkStartTable1.CountKills && (networkStartTable.CountKills != networkStartTable1.CountKills || networkStartTable.score <= networkStartTable1.score)))
				{
					num1++;
				}
				else
				{
					NetworkStartTable networkStartTable2 = array[i];
					for (int j = i - 1; j >= num1; j--)
					{
						array[j + 1] = array[j];
					}
					array[num1] = networkStartTable2;
					break;
				}
			}
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int k = 0; k < (int)array.Length; k++)
		{
			if (array[k].myCommand == 0)
			{
				if (array[k] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					num = num2;
				}
				num2++;
			}
			if (array[k].myCommand == 1)
			{
				if (array[k] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					num = num3;
				}
				num3++;
			}
			if (array[k].myCommand == 2)
			{
				if (array[k] == WeaponManager.sharedManager.myNetworkStartTable)
				{
					num = num4;
				}
				num4++;
			}
		}
		return num;
	}

	public int GetWinningTeam()
	{
		int num = 0;
		if (Defs.isFlag)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				if (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2)
				{
					num = 1;
				}
				else if (WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag2 > WeaponManager.sharedManager.myNetworkStartTable.scoreCommandFlag1)
				{
					num = 2;
				}
			}
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			if (CapturePointController.sharedController.scoreBlue > CapturePointController.sharedController.scoreRed)
			{
				num = 1;
			}
			else if (CapturePointController.sharedController.scoreRed > CapturePointController.sharedController.scoreBlue)
			{
				num = 2;
			}
		}
		else if (this.myPlayerMoveC != null)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed)
			{
				num = 1;
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandRed > WeaponManager.sharedManager.myPlayerMoveC.countKillsCommandBlue)
			{
				num = 2;
			}
		}
		else if (GlobalGameController.countKillsBlue > GlobalGameController.countKillsRed)
		{
			num = 1;
		}
		else if (GlobalGameController.countKillsRed > GlobalGameController.countKillsBlue)
		{
			num = 2;
		}
		return num;
	}

	public void HandleResumeFromShop()
	{
		if (this._shopInstance != null)
		{
			this.expController.isShowRanks = true;
			ShopNGUIController.GuiActive = false;
			this._shopInstance.resumeAction = () => {
			};
			this._shopInstance = null;
		}
	}

	public void HandleShopButton()
	{
		if (this._shopInstance == null && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled))
		{
			FlurryPluginWrapper.LogEvent("Shop");
			this._shopInstance = ShopNGUIController.sharedShop;
			if (this._shopInstance == null)
			{
				UnityEngine.Debug.LogWarning("sharedShop == null");
			}
			else
			{
				this._shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
			}
		}
	}

	[Obfuscation(Exclude=true)]
	private void hideMessag()
	{
		this.showMessagFacebook = false;
	}

	public void ImDeadInHungerGames()
	{
		if (Defs.isInet && NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons(true);
			this.isSetNewMapButton = true;
		}
		this._matchStopwatch.Stop();
		int num = PlayerPrefs.GetInt("CountMatch", 0);
		PlayerPrefs.SetInt("CountMatch", num + 1);
		if (ExperienceController.sharedController != null)
		{
			string str = string.Concat("Statistics.MatchCount.Level", ExperienceController.sharedController.currentLevel);
			int num1 = PlayerPrefs.GetInt(str, 0);
			PlayerPrefs.SetInt(str, num1 + 1);
			FlurryPluginWrapper.LogMatchCompleted(ConnectSceneNGUIController.regim.ToString());
		}
		NetworkStartTable.IncreaseTimeInMode(3, this._matchStopwatch.Elapsed.TotalMinutes);
		StoreKitEventListener.State.PurchaseKey = "End match";
		if (this._cam != null)
		{
			this._cam.SetActive(true);
		}
		NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
		this.showTable = true;
		RatingSystem.RatingChange ratingChange = this.CalculateMatchRating(false);
		this.photonView.RPC("ImDeadInHungerGamesRPC", PhotonTargets.Others, new object[0]);
		this.isDeadInHungerGame = true;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.ShowEndInterfaceDeadInHunger(LocalizationStore.Get("Key_1116"), ratingChange);
		}
		this.inGameGUI.ResetScope();
	}

	[PunRPC]
	[RPC]
	public void ImDeadInHungerGamesRPC()
	{
		this.isDeadInHungerGame = true;
	}

	public static void IncreaseTimeInMode(int mode, double minutes)
	{
		object obj;
		object obj1;
		if (ExperienceController.sharedController != null)
		{
			string str = mode.ToString();
			string str1 = string.Concat("Statistics.TimeInMode.Level", ExperienceController.sharedController.currentLevel);
			if (PlayerPrefs.HasKey(str1))
			{
				string str2 = PlayerPrefs.GetString(str1, "{}");
				UnityEngine.Debug.Log(string.Concat("Time in mode string:    ", str2));
				try
				{
					Dictionary<string, object> strs = Rilisoft.MiniJson.Json.Deserialize(str2) as Dictionary<string, object> ?? new Dictionary<string, object>();
					if (!strs.TryGetValue(str, out obj))
					{
						strs.Add(str, minutes);
					}
					else
					{
						double num = Convert.ToDouble(obj) + minutes;
						strs[str] = num;
					}
					PlayerPrefs.SetString(str1, Rilisoft.MiniJson.Json.Serialize(strs));
				}
				catch (OverflowException overflowException1)
				{
					OverflowException overflowException = overflowException1;
					UnityEngine.Debug.LogError(string.Concat("Cannot deserialize time-in-mode:    ", str2));
					UnityEngine.Debug.LogException(overflowException);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					UnityEngine.Debug.LogError(string.Concat("Unknown exception:    ", str2));
					UnityEngine.Debug.LogException(exception);
				}
			}
			string str3 = string.Concat("Statistics.RoundsInMode.Level", ExperienceController.sharedController.currentLevel);
			if (PlayerPrefs.HasKey(str3))
			{
				Dictionary<string, object> strs1 = Rilisoft.MiniJson.Json.Deserialize(PlayerPrefs.GetString(str3)) as Dictionary<string, object> ?? new Dictionary<string, object>();
				if (!strs1.TryGetValue(str, out obj1))
				{
					strs1.Add(str, 1);
				}
				else
				{
					int num1 = Convert.ToInt32(obj1) + 1;
					strs1[str] = num1;
				}
				PlayerPrefs.SetString(str3, Rilisoft.MiniJson.Json.Serialize(strs1));
			}
			PlayerPrefs.Save();
		}
	}

	public void IncrementDeath()
	{
		this.deathCountMatch++;
	}

	public void IncrementKills()
	{
		this.killCountMatch++;
	}

	public static bool LocalOrPasswordRoom()
	{
		bool flag;
		if (!Defs.isInet)
		{
			flag = true;
		}
		else
		{
			flag = (PhotonNetwork.room == null ? false : !PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty));
		}
		return flag;
	}

	[DebuggerHidden]
	private IEnumerator LockInterfaceCoroutine()
	{
		return new NetworkStartTable.u003cLockInterfaceCoroutineu003ec__IteratorC6();
	}

	public void MyOnGUI()
	{
		if (this.experienceController.isShowAdd)
		{
			GUI.enabled = false;
		}
		if (this.showDisconnectFromServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)this.serverLeftTheGame.width * 0.5f * this.koofScreen, (float)(Screen.height / 2) - (float)this.serverLeftTheGame.height * 0.5f * this.koofScreen, (float)this.serverLeftTheGame.width * this.koofScreen, (float)this.serverLeftTheGame.height * this.koofScreen), this.serverLeftTheGame);
			GUI.enabled = false;
		}
		if (this.showDisconnectFromMasterServer)
		{
			GUI.DrawTexture(new Rect((float)(Screen.width / 2) - (float)this.serverLeftTheGame.width * 0.5f * this.koofScreen, (float)(Screen.height / 2) - (float)this.serverLeftTheGame.height * 0.5f * this.koofScreen, (float)this.serverLeftTheGame.width * this.koofScreen, (float)this.serverLeftTheGame.height * this.koofScreen), this.serverLeftTheGame);
		}
		if (this.showTable)
		{
			this.playersTable();
		}
		if (this.isShowNickTable)
		{
			this.finishTable();
		}
		if (this.showMessagFacebook)
		{
			this.labelStyle.fontSize = Player_move_c.FontSizeForMessages;
			GUI.Label(Tools.SuccessMessageRect(), this._SocialSentSuccess("Facebook"), this.labelStyle);
		}
		GUI.enabled = true;
	}

	private void OnDestroy()
	{
		if (this.isMine)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
			this.RemoveShop(false);
			if (this.networkStartTableNGUIController != null && !this.networkStartTableNGUIController.isRewardShow)
			{
				UnityEngine.Object.Destroy(this.networkStartTableNGUIController.gameObject);
			}
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.resumeAction = null;
			}
		}
		if (!this.isMine && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(this.NamePlayer, " ", LocalizationStore.Get("Key_0996")), new Color(1f, 0f, 0f));
		}
		if (Initializer.networkTables.Contains(this))
		{
			Initializer.networkTables.Remove(this);
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		UnityEngine.Debug.Log("OnDisconnectedFromServer");
		this.showDisconnectFromServer = true;
		this.timerShow = 3f;
	}

	private void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		UnityEngine.Debug.Log(string.Concat("Could not connect to master server: ", info));
		this.showDisconnectFromMasterServer = true;
		this.timerShow = 3f;
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView && this.photonView.isMine)
		{
			if (Defs.isFlag && !this.isShowFinished)
			{
				this.photonView.RPC("SynchScoreCommandRPC", player, new object[] { 1, this.scoreCommandFlag1 });
				this.photonView.RPC("SynchScoreCommandRPC", player, new object[] { 2, this.scoreCommandFlag2 });
			}
			this.SynhCommand(player);
			this.SynhCountKills(player);
			this.SendSynhScore(player);
			if (Defs.isMulti && Defs.isInet && this.isMine)
			{
				this.photonView.RPC("SynchGameRating", player, new object[] { this.gameRating });
			}
		}
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (base.GetComponent<NetworkView>().isMine)
		{
			this.SynhCommand(null);
			this.SynhCountKills(null);
			this.SynhScore();
		}
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		foreach (Player_move_c playerMoveC in Initializer.players)
		{
			if (!player.ipAddress.Equals(playerMoveC.myIp) || !(NickLabelStack.sharedStack != null))
			{
				continue;
			}
			NickLabelController[] nickLabelControllerArray = NickLabelStack.sharedStack.lables;
			int num = 0;
			while (num < (int)nickLabelControllerArray.Length)
			{
				NickLabelController nickLabelController = nickLabelControllerArray[num];
				if (nickLabelController.target != playerMoveC.transform)
				{
					num++;
				}
				else
				{
					nickLabelController.target = null;
					break;
				}
			}
			UnityEngine.Object.Destroy(playerMoveC.mySkinName.gameObject);
		}
	}

	private void playersTable()
	{
		bool flag;
		if (!this.isShowAvard)
		{
			ShopTapReceiver.AddClickHndIfNotExist(new Action(this.HandleShopButton));
			GameObject gameObject = this.networkStartTableNGUIController.shopAnchor;
			if (this.isShowFinished || this.isHunger || !(this._shopInstance == null))
			{
				flag = false;
			}
			else
			{
				flag = (this.expController == null ? true : !this.expController.isShowNextPlashka);
			}
			gameObject.SetActive(flag);
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				return;
			}
		}
	}

	public void PostFacebookBtnClick()
	{
		UnityEngine.Debug.Log("show facebook dialog");
		FlurryPluginWrapper.LogFacebook();
		FacebookController.ShowPostDialog();
	}

	public void PostTwitterBtnClick()
	{
		FlurryPluginWrapper.LogTwitter();
		if (TwitterController.Instance != null)
		{
			TwitterController.Instance.PostStatusUpdate(this._SocialMessage(), null);
		}
	}

	public void RandomRoomClickBtnInHunger()
	{
		this.isGoRandomRoom = true;
		if (this.isRegimVidos)
		{
			this.isRegimVidos = false;
			if (this.inGameGUI != null)
			{
				this.inGameGUI.ResetScope();
			}
		}
		Defs.typeDisconnectGame = Defs.DisconectGameType.RandomGameInHunger;
		PhotonNetwork.LeaveRoom();
	}

	public void RemoveShop(bool disable = true)
	{
		ShopTapReceiver.ShopClicked -= new Action(this.HandleShopButton);
		if (this._shopInstance != null)
		{
			if (disable)
			{
				ShopNGUIController.GuiActive = false;
			}
			this._shopInstance.resumeAction = () => {
			};
			this._shopInstance = null;
		}
	}

	private void ReplaceCommand()
	{
		this.myCommand = (this.myCommand != 1 ? 1 : 2);
		this.SynhCommand(null);
		this.score = 0;
		this.CountKills = 0;
		GlobalGameController.Score = 0;
		GlobalGameController.CountKills = 0;
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.countKills = 0;
			WeaponManager.sharedManager.myPlayerMoveC.myCommand = this.myCommand;
			WeaponManager.sharedManager.myPlayerMoveC.myBaza = null;
			WeaponManager.sharedManager.myPlayerMoveC.myFlag = null;
			WeaponManager.sharedManager.myPlayerMoveC.enemyFlag = null;
			if (Initializer.redPlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC) && this.myCommand == 1)
			{
				Initializer.redPlayers.Remove(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (Initializer.bluePlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC) && this.myCommand == 2)
			{
				Initializer.bluePlayers.Remove(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (this.myCommand == 1 && !Initializer.bluePlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC))
			{
				Initializer.bluePlayers.Add(WeaponManager.sharedManager.myPlayerMoveC);
			}
			if (this.myCommand == 2 && !Initializer.redPlayers.Contains(WeaponManager.sharedManager.myPlayerMoveC))
			{
				Initializer.redPlayers.Add(WeaponManager.sharedManager.myPlayerMoveC);
			}
		}
	}

	public void ResetCamPlayer(int _nextPrev = 0)
	{
		if (_nextPrev != 0 && Initializer.players.Count == 1)
		{
			return;
		}
		if (Initializer.players.Count <= 0)
		{
			this._cam.SetActive(true);
			this.showTable = true;
			this.isRegimVidos = false;
			NickLabelController.currentCamera = this._cam.GetComponent<Camera>();
			if (this.inGameGUI != null)
			{
				this.inGameGUI.ResetScope();
			}
		}
		else
		{
			if (_nextPrev == 0)
			{
				this.numberPlayerCun = UnityEngine.Random.Range(0, Initializer.players.Count);
				this.numberPlayerCunId = Initializer.players[this.numberPlayerCun].mySkinName.photonView.ownerId;
			}
			if (_nextPrev == 1)
			{
				int num = 10000000;
				int item = Initializer.players[0].mySkinName.photonView.ownerId;
				foreach (Player_move_c player in Initializer.players)
				{
					int num1 = player.mySkinName.photonView.ownerId;
					if (num1 < item)
					{
						item = num1;
					}
					if (num1 <= this.numberPlayerCunId || num1 >= num)
					{
						continue;
					}
					num = num1;
				}
				if (num != 10000000)
				{
					this.numberPlayerCunId = num;
				}
				else
				{
					this.numberPlayerCunId = item;
				}
				int num2 = 0;
				while (num2 < Initializer.players.Count)
				{
					if (Initializer.players[num2].mySkinName.photonView.ownerId != this.numberPlayerCunId)
					{
						num2++;
					}
					else
					{
						this.numberPlayerCun = num2;
						break;
					}
				}
			}
			if (_nextPrev == -1)
			{
				int num3 = -1;
				int item1 = Initializer.players[0].mySkinName.photonView.ownerId;
				foreach (Player_move_c playerMoveC in Initializer.players)
				{
					int num4 = playerMoveC.mySkinName.photonView.ownerId;
					if (num4 > item1)
					{
						item1 = num4;
					}
					if (num4 >= this.numberPlayerCunId)
					{
						continue;
					}
					num3 = num4;
				}
				if (num3 != -1)
				{
					this.numberPlayerCunId = num3;
				}
				else
				{
					this.numberPlayerCunId = item1;
				}
				int num5 = 0;
				while (num5 < Initializer.players.Count)
				{
					if (Initializer.players[num5].mySkinName.photonView.ownerId != this.numberPlayerCunId)
					{
						num5++;
					}
					else
					{
						this.numberPlayerCun = num5;
						break;
					}
				}
			}
			if (this.currentCamPlayer != null)
			{
				this.currentCamPlayer.SetActive(false);
				if (!this.currentPlayerMoveCVidos.isMechActive)
				{
					this.currentFPSPlayer.SetActive(true);
				}
				this.currentBodyMech.SetActive(true);
				Player_move_c.SetLayerRecursively(this.currentGameObjectPlayer.transform.GetChild(0).gameObject, 0);
				this.currentGameObjectPlayer.GetComponent<InterolationGameObject>().sglajEnabled = false;
				this.currentCamPlayer.transform.parent.GetComponent<ThirdPersonNetwork1>().sglajEnabledVidos = false;
				this.currentCamPlayer = null;
				this.currentFPSPlayer = null;
				this.currentBodyMech = null;
				this.currentGameObjectPlayer = null;
				this.currentPlayerMoveCVidos = null;
			}
			SkinName skinName = Initializer.players[this.numberPlayerCun].mySkinName;
			skinName.camPlayer.SetActive(true);
			this.playerVidosNick = skinName.NickName;
			this.playerVidosClanName = skinName.playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanName;
			this.playerVidosClanTexture = skinName.playerMoveC.myTable.GetComponent<NetworkStartTable>().myClanTexture;
			this.currentPlayerMoveCVidos = skinName.playerMoveC;
			this.currentCamPlayer = skinName.camPlayer;
			this.currentFPSPlayer = skinName.FPSplayerObject;
			this.currentBodyMech = skinName.playerMoveC.mechBody;
			Initializer.players[this.numberPlayerCun].myPersonNetwork.sglajEnabledVidos = true;
			this.currentGameObjectPlayer = skinName.playerGameObject;
			this.currentGameObjectPlayer.GetComponent<InterolationGameObject>().sglajEnabled = true;
			this.currentFPSPlayer.SetActive(false);
			this.currentBodyMech.SetActive(false);
			NickLabelController.currentCamera = skinName.camPlayer.GetComponent<Camera>();
			Player_move_c.SetLayerRecursively(this.currentGameObjectPlayer.transform.GetChild(0).gameObject, 9);
		}
	}

	public void ResetOldScore()
	{
		this.scoreOld = 0;
		this.score = 0;
		this.SynhScore();
		this.oldCountKills = 0;
		this.CountKills = 0;
		this.SynhCountKills(null);
		this.GetMyTeam();
	}

	[PunRPC]
	[RPC]
	private void RunGame()
	{
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("NetworkTable");
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			gameObjectArray[i].GetComponent<NetworkStartTable>().runGame = true;
		}
	}

	public void sendMySkin()
	{
		this.mySkin = SkinsController.currentSkinForPers;
		byte[] pNG = (this.mySkin as Texture2D).EncodeToPNG();
		if (!this.isInet)
		{
			string base64String = Convert.ToBase64String(pNG);
			base.GetComponent<NetworkView>().RPC("setMySkinLocal", RPCMode.AllBuffered, new object[] { base64String.Substring(0, base64String.Length / 2), base64String.Substring(base64String.Length / 2, base64String.Length / 2) });
		}
		else
		{
			this.photonView.RPC("setMySkin", PhotonTargets.AllBuffered, new object[] { pNG });
		}
	}

	public void SendSynhScore(PhotonPlayer player = null)
	{
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SynhScoreRPC", RPCMode.Others, new object[] { this.score, this.scoreOld });
		}
		else if (player != null)
		{
			this.photonView.RPC("SynhScoreRPC", player, new object[] { this.score, this.scoreOld });
		}
		else
		{
			this.photonView.RPC("SynhScoreRPC", PhotonTargets.Others, new object[] { this.score, this.scoreOld });
		}
	}

	[PunRPC]
	[RPC]
	private void SetMyClanTexture(string str, string _clanID, string _clanName, string _clanLeaderId)
	{
		try
		{
			byte[] numArray = Convert.FromBase64String(str);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(numArray);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			this.myClanTexture = texture2D;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.Log(exception);
		}
		this.myClanID = _clanID;
		this.myClanName = _clanName;
		this.myClanLeaderID = _clanLeaderId;
	}

	[PunRPC]
	[RPC]
	private void setMySkin(byte[] _skinByte)
	{
		if (this.photonView == null || !Defs.isMulti)
		{
			return;
		}
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.mySkin = texture2D;
		foreach (Player_move_c player in Initializer.players)
		{
			if (player.mySkinName.photonView.owner == null || !player.mySkinName.photonView.owner.Equals(this.photonView.owner))
			{
				continue;
			}
			if (player.myNetworkStartTable != null)
			{
				player._skin = this.mySkin;
				player.SetTextureForBodyPlayer(player._skin);
			}
			else
			{
				player.setMyTamble(base.gameObject);
			}
		}
	}

	[PunRPC]
	[RPC]
	private void setMySkinLocal(string str1, string str2)
	{
		byte[] numArray = Convert.FromBase64String(string.Concat(str1, str2));
		Texture2D texture2D = new Texture2D(64, 32);
		texture2D.LoadImage(numArray);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		this.mySkin = texture2D;
		if (base.GetComponent<NetworkView>().isMine && WeaponManager.sharedManager.myPlayer != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetIDMyTable(base.GetComponent<NetworkView>().viewID.ToString());
		}
	}

	public void SetNewNick()
	{
		this.NamePlayer = ProfileController.GetPlayerNameOrDefault();
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SynhNickNameRPC", RPCMode.OthersBuffered, new object[] { this.NamePlayer });
		}
		else
		{
			PhotonNetwork.playerName = this.NamePlayer;
			this.photonView.RPC("SynhNickNameRPC", PhotonTargets.OthersBuffered, new object[] { this.NamePlayer });
		}
	}

	[PunRPC]
	[RPC]
	private void SetPixelBookID(string _pixelBookID)
	{
		this.pixelBookID = _pixelBookID;
	}

	public void SetRanks()
	{
		this.myRanks = this.expController.currentLevel;
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SynhRanksRPC", RPCMode.OthersBuffered, new object[] { this.myRanks });
		}
		else
		{
			this.photonView.RPC("SynhRanksRPC", PhotonTargets.OthersBuffered, new object[] { this.myRanks });
		}
	}

	public void SetRegimVidos(bool _isRegimVidos)
	{
		bool flag = this.isRegimVidos;
		this.isRegimVidos = _isRegimVidos;
		if (this.isRegimVidos != flag && !this.isRegimVidos && this.inGameGUI != null)
		{
			this.inGameGUI.ResetScope();
		}
	}

	public void setScoreFromGlobalGameController()
	{
		this.score = GlobalGameController.Score;
		this.SynhScore();
	}

	private void Start()
	{
		this.waitingPlayerLocalize = LocalizationStore.Key_0565;
		this.matchLocalize = LocalizationStore.Key_0566;
		this.preparingLocalize = LocalizationStore.Key_0567;
		this.lanScan = base.GetComponent<LANBroadcastService>();
		try
		{
			this.StartUnsafe();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(exception);
		}
		if (this.isMine && !TrainingController.TrainingCompleted)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Table_Deathmatch, true);
		}
	}

	[Obfuscation(Exclude=true)]
	public void startPlayer()
	{
		base.StartCoroutine(this.StartPlayerCoroutine());
	}

	public void StartPlayerButtonClick(int _command)
	{
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.HideEndInterface();
		}
		this.isShowNickTable = false;
		this.CountKills = 0;
		this.score = 0;
		GlobalGameController.Score = 0;
		GlobalGameController.CountKills = 0;
		this.myCommand = _command;
		this.SynhCommand(null);
		this.SynhCountKills(null);
		this.SynhScore();
		this.startPlayer();
		this.countMigZagolovok = 0;
		this.timeTomig = 0.7f;
		this.isMigZag = false;
	}

	[DebuggerHidden]
	private IEnumerator StartPlayerCoroutine()
	{
		NetworkStartTable.u003cStartPlayerCoroutineu003ec__IteratorC3 variable = null;
		return variable;
	}

	private void StartUnsafe()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (this.isMulti)
		{
			if (!this.isLocal)
			{
				this.isMine = this.photonView.isMine;
			}
			else
			{
				this.isMine = base.GetComponent<NetworkView>().isMine;
			}
		}
		if (this.isMine)
		{
			this.networkStartTableNGUIController = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("NetworkStartTableNGUI"))).GetComponent<NetworkStartTableNGUIController>();
			this._cam = GameObject.FindGameObjectWithTag("CamTemp");
			StoreKitEventListener.State.PurchaseKey = "Start table";
			if (FriendsController.sharedController.clanLogo != null && !string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
			{
				byte[] numArray = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
				Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
				texture2D.LoadImage(numArray);
				texture2D.filterMode = FilterMode.Point;
				texture2D.Apply();
				this.myClanTexture = texture2D;
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("SetMyClanTexture", RPCMode.AllBuffered, new object[] { FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName, FriendsController.sharedController.clanLeaderID });
				}
				else
				{
					this.photonView.RPC("SetMyClanTexture", PhotonTargets.AllBuffered, new object[] { FriendsController.sharedController.clanLogo, FriendsController.sharedController.ClanID, FriendsController.sharedController.clanName, FriendsController.sharedController.clanLeaderID });
				}
			}
			base.Invoke("GetMyTeam", 1.5f);
			this.photonView.RPC("SynchGameRating", PhotonTargets.Others, new object[] { this.gameRating });
		}
		if (this.isHunger)
		{
			this.hungerGameController = HungerGameController.Instance;
		}
		this.expController = ExperienceController.sharedController;
		this.expController.posRanks = NetworkStartTable.ExperiencePosRanks;
		this._weaponManager = WeaponManager.sharedManager;
		if (!this.isMulti || !this.isMine)
		{
			this.showTable = false;
		}
		else
		{
			if (PlayerPrefs.GetInt("StartAfterDisconnect") != 0)
			{
				this.showTable = GlobalGameController.showTableMyPlayer;
				this.isDeadInHungerGame = GlobalGameController.imDeadInHungerGame;
				if (!this.showTable && !this.isEndInHunger)
				{
					if (NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.HideStartInterface();
					}
					base.Invoke("startPlayer", 0.1f);
				}
				else if (!this.isDeadInHungerGame && !this.isEndInHunger)
				{
					if (NetworkStartTableNGUIController.sharedController != null)
					{
						NetworkStartTableNGUIController.sharedController.ShowStartInterface();
					}
				}
				else if (NetworkStartTableNGUIController.sharedController != null)
				{
					NetworkStartTableNGUIController.sharedController.ShowEndInterface(string.Empty, 0, false);
				}
			}
			else
			{
				if (NetworkStartTableNGUIController.sharedController != null)
				{
					NetworkStartTableNGUIController.sharedController.ShowStartInterface();
				}
				this.showTable = true;
			}
			NickLabelController.currentCamera = Initializer.Instance.tc.GetComponent<Camera>();
			this.tempCam.SetActive(true);
			this.NamePlayer = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			this.pixelBookID = FriendsController.sharedController.id;
			if (this.isInet)
			{
				this.photonView.RPC("SetPixelBookID", PhotonTargets.OthersBuffered, new object[] { this.pixelBookID });
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SetPixelBookID", RPCMode.OthersBuffered, new object[] { this.pixelBookID });
			}
			if (!this.isServer || this.isInet)
			{
				this.lanScan.enabled = false;
			}
			else
			{
				this.lanScan.serverMessage.name = PlayerPrefs.GetString("ServerName");
				this.lanScan.serverMessage.map = PlayerPrefs.GetString("MapName");
				this.lanScan.serverMessage.connectedPlayers = 0;
				this.lanScan.serverMessage.playerLimit = int.Parse(PlayerPrefs.GetString("PlayersLimits"));
				this.lanScan.serverMessage.comment = PlayerPrefs.GetString("MaxKill");
				this.lanScan.serverMessage.regim = (int)ConnectSceneNGUIController.regim;
				this.lanScan.StartAnnounceBroadCasting();
				UnityEngine.Debug.Log(string.Concat("lanScan.serverMessage.regim=", this.lanScan.serverMessage.regim));
			}
			if (PlayerPrefs.GetInt("StartAfterDisconnect") != 1)
			{
				this.CountKills = -1;
				this.score = -1;
				GlobalGameController.CountKills = 0;
				GlobalGameController.Score = 0;
				base.Invoke("synchState", 1f);
			}
			else
			{
				this.CountKills = GlobalGameController.CountKills;
				this.score = GlobalGameController.Score;
				base.Invoke("synchState", 1f);
			}
			this.expController = ExperienceController.sharedController;
			this.SetNewNick();
			this.SetRanks();
			this.SynhCountKills(null);
			this.SynhScore();
			this.sendMySkin();
			ShopNGUIController.sharedShop.onEquipSkinAction = (string id) => this.sendMySkin();
		}
		stopwatch.Stop();
	}

	[PunRPC]
	[RPC]
	private void SynchGameRating(int _rating)
	{
		this.gameRating = _rating;
	}

	[PunRPC]
	[RPC]
	private void SynchScoreCommandRPC(int _command, int _score)
	{
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("NetworkTable");
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			if (_command != 1)
			{
				gameObjectArray[i].GetComponent<NetworkStartTable>().scoreCommandFlag2 = _score;
			}
			else
			{
				gameObjectArray[i].GetComponent<NetworkStartTable>().scoreCommandFlag1 = _score;
			}
		}
	}

	public void SynhCommand(PhotonPlayer player = null)
	{
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SynhCommandRPC", RPCMode.Others, new object[] { this.myCommand, this.myCommandOld });
		}
		else if (player != null)
		{
			this.photonView.RPC("SynhCommandRPC", player, new object[] { this.myCommand, this.myCommandOld });
		}
		else
		{
			this.photonView.RPC("SynhCommandRPC", PhotonTargets.Others, new object[] { this.myCommand, this.myCommandOld });
		}
	}

	[PunRPC]
	[RPC]
	private void SynhCommandRPC(int _command, int _oldCommand)
	{
		this.myCommand = _command;
		this.myCommandOld = _oldCommand;
		if (this.myPlayerMoveC != null)
		{
			this.myPlayerMoveC.myCommand = this.myCommand;
			if (Initializer.redPlayers.Contains(this.myPlayerMoveC) && this.myCommand == 1)
			{
				Initializer.redPlayers.Remove(this.myPlayerMoveC);
			}
			if (Initializer.bluePlayers.Contains(this.myPlayerMoveC) && this.myCommand == 2)
			{
				Initializer.bluePlayers.Remove(this.myPlayerMoveC);
			}
			if (this.myCommand == 1 && !Initializer.bluePlayers.Contains(this.myPlayerMoveC))
			{
				Initializer.bluePlayers.Add(this.myPlayerMoveC);
			}
			if (this.myCommand == 2 && !Initializer.redPlayers.Contains(this.myPlayerMoveC))
			{
				Initializer.redPlayers.Add(this.myPlayerMoveC);
			}
			if (this.myPlayerMoveC.myNickLabelController != null)
			{
				this.myPlayerMoveC.myNickLabelController.SetCommandColor(this.myCommand);
			}
		}
	}

	public void SynhCountKills(PhotonPlayer player = null)
	{
		if (!Defs.isInet)
		{
			base.GetComponent<NetworkView>().RPC("SynhCountKillsRPC", RPCMode.Others, new object[] { this.CountKills, this.oldCountKills });
		}
		else if (player != null)
		{
			this.photonView.RPC("SynhCountKillsRPC", player, new object[] { this.CountKills, this.oldCountKills });
		}
		else
		{
			this.photonView.RPC("SynhCountKillsRPC", PhotonTargets.Others, new object[] { this.CountKills, this.oldCountKills });
		}
	}

	[PunRPC]
	[RPC]
	private void SynhCountKillsRPC(int _countKills, int _oldCountKills)
	{
		this.CountKills = _countKills;
		this.oldCountKills = _oldCountKills;
	}

	[PunRPC]
	[RPC]
	private void SynhNickNameRPC(string _nick)
	{
		if (!this.isMine && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Concat(_nick, " ", LocalizationStore.Get("Key_0995")), new Color(1f, 0.7f, 0f));
		}
		this.NamePlayer = _nick;
	}

	[PunRPC]
	[RPC]
	private void SynhRanksRPC(int _ranks)
	{
		this.myRanks = _ranks;
	}

	public void SynhScore()
	{
		if (this.timerSynchScore < 0f)
		{
			this.timerSynchScore = 1f;
		}
	}

	[PunRPC]
	[RPC]
	private void SynhScoreRPC(int _score, int _oldScore)
	{
		this.score = _score;
		this.scoreOld = _oldScore;
	}

	private void Update()
	{
		bool flag;
		if (this.isMine)
		{
			if (this.inGameGUI == null)
			{
				this.inGameGUI = InGameGUI.sharedInGameGUI;
			}
			if (this.timerSynchScore > 0f)
			{
				this.timerSynchScore -= Time.deltaTime;
				if (this.timerSynchScore < 0f)
				{
					this.SendSynhScore(null);
				}
			}
			bool flag1 = (this.isShowNickTable || this.showDisconnectFromServer || this.showDisconnectFromMasterServer || this.showTable ? true : this.showMessagFacebook);
			if (this.guiObj.activeSelf != flag1)
			{
				this.guiObj.SetActive(flag1);
			}
			if (this.inGameGUI == null)
			{
				this.inGameGUI = InGameGUI.sharedInGameGUI;
			}
			if (this._pauser == null)
			{
				this._pauser = Pauser.sharedPauser;
			}
			if (ShopNGUIController.GuiActive || ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled)
			{
				this.expController.isShowRanks = (SkinEditorController.sharedController != null ? false : (BankController.Instance == null ? 0 : (int)BankController.Instance.InterfaceEnabled) == 0);
			}
			else if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
			{
				this.expController.isShowRanks = false;
			}
			else if (this._pauser != null && this._pauser.paused)
			{
				if (PauseNGUIController.sharedController != null)
				{
					this.expController.isShowRanks = !PauseNGUIController.sharedController.SettingsJoysticksPanel.activeInHierarchy;
				}
			}
			else if ((this.showTable || this.isShowNickTable) && !this.isRegimVidos && this._shopInstance == null && !LoadingInAfterGame.isShowLoading && !this.isGoRandomRoom)
			{
				ExperienceController experienceController = this.expController;
				if (this.isShowFinished)
				{
					flag = false;
				}
				else
				{
					flag = (this.networkStartTableNGUIController == null ? true : this.networkStartTableNGUIController.rentScreenPoint.childCount == 0);
				}
				experienceController.isShowRanks = flag;
			}
			else
			{
				this.expController.isShowRanks = false;
			}
			if (this.isRegimVidos && this.isDeadInHungerGame && this._cam.activeInHierarchy && Initializer.players.Count > 0)
			{
				this._cam.SetActive(false);
				this.ResetCamPlayer(0);
			}
			if (this.isRegimVidos && this.isDeadInHungerGame && this.currentCamPlayer == null)
			{
				this.ResetCamPlayer(0);
			}
			if (!this.isRegimVidos && this.isDeadInHungerGame && this.currentCamPlayer != null)
			{
				this.currentCamPlayer.SetActive(false);
				if (!this.currentPlayerMoveCVidos.isMechActive)
				{
					this.currentFPSPlayer.SetActive(true);
				}
				this.currentBodyMech.SetActive(true);
				this.currentCamPlayer = null;
				this.currentFPSPlayer = null;
				this.currentBodyMech = null;
				this._cam.SetActive(true);
			}
			if (this.isRegimVidos && this.inGameGUI != null && this.currentPlayerMoveCVidos.isZooming != this.oldIsZomming)
			{
				this.oldIsZomming = this.currentPlayerMoveCVidos.isZooming;
				if (!this.oldIsZomming)
				{
					this.currentPlayerMoveCVidos.myCamera.fieldOfView = 44f;
					this.currentPlayerMoveCVidos.gunCamera.fieldOfView = 75f;
					this.inGameGUI.ResetScope();
				}
				else
				{
					string empty = string.Empty;
					float component = 60f;
					if (this.currentGameObjectPlayer.transform.childCount > 0)
					{
						try
						{
							empty = ItemDb.GetByPrefabName(this.currentGameObjectPlayer.transform.GetChild(0).name.Replace("(Clone)", string.Empty)).Tag;
						}
						catch (Exception exception1)
						{
							Exception exception = exception1;
							if (Application.isEditor)
							{
								UnityEngine.Debug.LogWarning(string.Concat("Exception  tagWeapon = ItemDb.GetByPrefabName(currentGameObjectPlayer.transform.GetChild(0).name.Replace(\"(Clone)\",\"\")).Tag:  ", exception));
							}
						}
						component = this.currentGameObjectPlayer.transform.GetChild(0).GetComponent<WeaponSounds>().fieldOfViewZomm;
					}
					if (!empty.Equals(string.Empty))
					{
						this.inGameGUI.SetScopeForWeapon(string.Concat(string.Empty, this.currentGameObjectPlayer.transform.GetChild(0).GetComponent<WeaponSounds>().scopeNum));
					}
					this.currentPlayerMoveCVidos.myCamera.fieldOfView = component;
					this.currentPlayerMoveCVidos.gunCamera.fieldOfView = 1f;
				}
			}
			if (Defs.isFlag || Defs.isCompany || Defs.isCapturePoints)
			{
				if (Defs.isInet && this.myCommand > 0)
				{
					int num = 0;
					for (int i = 0; i < Initializer.networkTables.Count; i++)
					{
						if (Initializer.networkTables[i] != null && Initializer.networkTables[i].myCommand == this.myCommand)
						{
							num++;
						}
					}
					if (num > 5)
					{
						int item = -1;
						for (int j = 0; j < Initializer.networkTables.Count; j++)
						{
							if (Initializer.networkTables[j] != null && Initializer.networkTables[j].myCommand == this.myCommand && Initializer.networkTables[j].photonView.ownerId > item)
							{
								item = Initializer.networkTables[j].photonView.ownerId;
							}
						}
						if (item == this.photonView.ownerId)
						{
							this.ReplaceCommand();
						}
					}
				}
				if (Defs.isFlag)
				{
					this.timerFlag = TimeGameController.sharedController.timerToEndMatch;
					if (this.timerFlag < 0)
					{
						this.timerFlag = 0;
					}
					if (this.timerFlag < 0.10000000149011612)
					{
						if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.enabled)
						{
							WeaponManager.sharedManager.myPlayerMoveC.enabled = false;
							InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
							base.Invoke("ClearScoreCommandInFlagGame", 0.5f);
							ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
							hashtable["TimeMatchEnd"] = -9000000;
							PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
							if (this.scoreCommandFlag1 > this.scoreCommandFlag2)
							{
								this.win(string.Empty, 1, this.scoreCommandFlag1, this.scoreCommandFlag2);
							}
							else if (this.scoreCommandFlag1 >= this.scoreCommandFlag2)
							{
								this.win(string.Empty, 0, this.scoreCommandFlag1, this.scoreCommandFlag2);
							}
							else
							{
								this.win(string.Empty, 2, this.scoreCommandFlag1, this.scoreCommandFlag2);
							}
						}
					}
					else if (this.inGameGUI != null && this.inGameGUI.message_draw.activeSelf)
					{
						this.inGameGUI.message_draw.SetActive(false);
					}
				}
			}
			if (this.isHunger && this.hungerGameController != null && this.hungerGameController.isStartGame && !this.hungerGameController.isRunPlayer && !this.isEndInHunger)
			{
				UnityEngine.Debug.Log("Start hunger player");
				this.hungerGameController.isRunPlayer = true;
				this.isShowNickTable = false;
				this.CountKills = 0;
				this.score = 0;
				GlobalGameController.Score = 0;
				this.isDrawInHanger = false;
				this.startPlayer();
				this.countMigZagolovok = 0;
				this.timeTomig = 0.7f;
				this.isMigZag = false;
				this.SynhCountKills(null);
				this.SynhScore();
				return;
			}
			if (this.isHunger && this.hungerGameController != null && !this.hungerGameController.isStartGame)
			{
				string str = string.Empty;
				if (this.hungerGameController.isStartTimer)
				{
					if (this.hungerGameController.startTimer > 0f && !this.hungerGameController.isStartGame)
					{
						float single = this.hungerGameController.startTimer;
						object[] objArray = new object[] { this.matchLocalize, " ", Mathf.FloorToInt(single / 60f), ":", null, null };
						objArray[4] = (Mathf.FloorToInt(single - (float)(Mathf.FloorToInt(single / 60f) * 60)) >= 10 ? string.Empty : "0");
						objArray[5] = Mathf.FloorToInt(single - (float)(Mathf.FloorToInt(single / 60f) * 60));
						str = string.Concat(objArray);
					}
					if (this.hungerGameController.startTimer < 0f && !this.hungerGameController.isStartGame)
					{
						str = this.preparingLocalize;
					}
				}
				else
				{
					str = this.waitingPlayerLocalize;
				}
				if (NetworkStartTableNGUIController.sharedController != null)
				{
					NetworkStartTableNGUIController.sharedController.HungerStartLabel.text = str;
				}
			}
			if (Defs.isFlag && this.isInet && PhotonNetwork.isMasterClient && Initializer.flag1 == null)
			{
				this.AddFlag();
			}
		}
		if (!this.isLocal && this.isMine)
		{
			GlobalGameController.showTableMyPlayer = this.showTable;
			GlobalGameController.imDeadInHungerGame = this.isDeadInHungerGame;
		}
		if (this.isLocal && this.isServer && this.lanScan != null)
		{
			this.lanScan.serverMessage.connectedPlayers = (int)GameObject.FindGameObjectsWithTag("NetworkTable").Length;
		}
		if (this.timerShow >= 0f)
		{
			this.timerShow -= Time.deltaTime;
			if (this.timerShow < 0f)
			{
				ActivityIndicator.IsActiveIndicator = false;
				ConnectSceneNGUIController.Local();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitInterstitialRequestAndShowCoroutine(Task<Ad> request)
	{
		NetworkStartTable.u003cWaitInterstitialRequestAndShowCoroutineu003ec__IteratorC5 variable = null;
		return variable;
	}

	public void win(string winner, int _commandWin = 0, int blueCount = 0, int redCount = 0)
	{
		string key0568;
		string str;
		int num;
		if (NetworkStartTableNGUIController.sharedController.isRewardShow || this.isShowFinished)
		{
			return;
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (Defs.isInet)
		{
			PhotonNetwork.FetchServerTimestamp();
		}
		this._matchStopwatch.Stop();
		double totalMinutes = this._matchStopwatch.Elapsed.TotalMinutes;
		if (Defs.isHunger)
		{
			this.isEndInHunger = true;
		}
		if (Defs.isDaterRegim)
		{
			Storager.setInt("DaterDayLived", Storager.getInt("DaterDayLived", false) + 1, false);
		}
		if (Defs.isDaterRegim)
		{
			int item = 5;
			if (!Defs.isInet)
			{
				item = (PlayerPrefs.GetString("MaxKill", "9").Equals(string.Empty) ? 5 : int.Parse(PlayerPrefs.GetString("MaxKill", "5")));
			}
			else
			{
				item = (int)PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty];
			}
			AnalyticsStuff.LogSandboxTimeGamePopularity(item, false);
		}
		StoreKitEventListener.State.PurchaseKey = "End match";
		if (!Defs.isHunger)
		{
			int num1 = PlayerPrefs.GetInt("CountMatch", 0);
			PlayerPrefs.SetInt("CountMatch", num1 + 1);
			if (ExperienceController.sharedController != null)
			{
				string str1 = string.Concat("Statistics.MatchCount.Level", ExperienceController.sharedController.currentLevel);
				int num2 = PlayerPrefs.GetInt(str1, 0);
				PlayerPrefs.SetInt(str1, num2 + 1);
				FlurryPluginWrapper.LogMatchCompleted(ConnectSceneNGUIController.regim.ToString());
			}
			ConnectSceneNGUIController.RegimGame regimGame = ConnectSceneNGUIController.regim;
			TimeSpan elapsed = this._matchStopwatch.Elapsed;
			NetworkStartTable.IncreaseTimeInMode((int)regimGame, elapsed.TotalMinutes);
			this._matchStopwatch.Reset();
		}
		this.isShowAvard = false;
		this.commandWinner = _commandWin;
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		this.nickPobeditelya = winner;
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("NetworkTable");
		List<GameObject> gameObjects = new List<GameObject>();
		List<GameObject> gameObjects1 = new List<GameObject>();
		List<GameObject> gameObjects2 = new List<GameObject>();
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			this.isDrawInDeathMatch = true;
			for (int i = 0; i < (int)gameObjectArray.Length; i++)
			{
				if (gameObjectArray[i].GetComponent<NetworkStartTable>().score >= AdminSettingsController.minScoreDeathMath)
				{
					this.isDrawInDeathMatch = false;
				}
			}
		}
		for (int j = 1; j < (int)gameObjectArray.Length; j++)
		{
			NetworkStartTable component = gameObjectArray[j].GetComponent<NetworkStartTable>();
			int num3 = 0;
			while (num3 < j)
			{
				NetworkStartTable networkStartTable = gameObjectArray[num3].GetComponent<NetworkStartTable>();
				if ((Defs.isFlag || Defs.isCapturePoints || component.score <= networkStartTable.score && (component.score != networkStartTable.score || component.CountKills <= networkStartTable.CountKills)) && (!Defs.isFlag && !Defs.isCapturePoints || component.CountKills <= networkStartTable.CountKills && (component.CountKills != networkStartTable.CountKills || component.score <= networkStartTable.score)))
				{
					num3++;
				}
				else
				{
					GameObject gameObject = gameObjectArray[j];
					for (int k = j - 1; k >= num3; k--)
					{
						gameObjectArray[k + 1] = gameObjectArray[k];
					}
					gameObjectArray[num3] = gameObject;
					break;
				}
			}
		}
		int count = 0;
		for (int l = 0; l < (int)gameObjectArray.Length; l++)
		{
			int component1 = gameObjectArray[l].GetComponent<NetworkStartTable>().myCommand;
			if (component1 == -1)
			{
				component1 = gameObjectArray[l].GetComponent<NetworkStartTable>().myCommandOld;
			}
			if (component1 == 0)
			{
				if (gameObjectArray[l].Equals(base.gameObject))
				{
					count = gameObjects2.Count;
				}
				gameObjects2.Add(gameObjectArray[l]);
			}
			if (component1 == 1)
			{
				if (gameObjectArray[l].Equals(base.gameObject))
				{
					count = gameObjects.Count;
				}
				gameObjects.Add(gameObjectArray[l]);
			}
			if (component1 == 2)
			{
				if (gameObjectArray[l].Equals(base.gameObject))
				{
					count = gameObjects1.Count;
				}
				gameObjects1.Add(gameObjectArray[l]);
			}
		}
		this.oldSpisokName = new string[gameObjects2.Count];
		this.oldScoreSpisok = new string[gameObjects2.Count];
		this.oldCountLilsSpisok = new string[gameObjects2.Count];
		this.oldSpisokRanks = new int[gameObjects2.Count];
		this.oldIsDeadInHungerGame = new bool[gameObjects2.Count];
		this.oldSpisokPixelBookID = new string[gameObjects2.Count];
		this.oldSpisokMyClanLogo = new Texture[gameObjects2.Count];
		this.oldSpisokNameBlue = new string[gameObjects.Count];
		this.oldCountLilsSpisokBlue = new string[gameObjects.Count];
		this.oldSpisokRanksBlue = new int[gameObjects.Count];
		this.oldSpisokPixelBookIDBlue = new string[gameObjects.Count];
		this.oldSpisokMyClanLogoBlue = new Texture[gameObjects.Count];
		this.oldScoreSpisokBlue = new string[gameObjects.Count];
		this.oldSpisokNameRed = new string[gameObjects1.Count];
		this.oldCountLilsSpisokRed = new string[gameObjects1.Count];
		this.oldSpisokRanksRed = new int[gameObjects1.Count];
		this.oldSpisokPixelBookIDRed = new string[gameObjects1.Count];
		this.oldSpisokMyClanLogoRed = new Texture[gameObjects1.Count];
		this.oldScoreSpisokRed = new string[gameObjects1.Count];
		this.addCoins = 0;
		this.addExperience = 0;
		bool flag = this.CheckForWin(count, _commandWin, this.CountKills, this.score, true);
		bool flag1 = this.CheckForWin(count, _commandWin, this.CountKills, this.score, false);
		KillRateCheck.instance.LogFirstBattlesResult(flag);
		RatingSystem.RatingChange ratingChange = this.CalculateMatchRating(false);
		if (this.isInet)
		{
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B || this.myCommand == _commandWin || !Defs.isCompany && !Defs.isFlag && !Defs.isCapturePoints || ExperienceController.sharedController.currentLevel < 2)
			{
				if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && (Defs.isCompany || Defs.isFlag || Defs.isCapturePoints))
				{
					this.isIwin = this.myCommand == _commandWin;
				}
				int num4 = int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString());
				AdminSettingsController.Avard avardAfterMatch = AdminSettingsController.GetAvardAfterMatch(ConnectSceneNGUIController.regim, num4, count, this.score, this.CountKills, this.isIwin);
				this.addCoins = avardAfterMatch.coin;
				this.addExperience = avardAfterMatch.expierense;
			}
			if (!TrainingController.TrainingCompleted)
			{
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Deathmatch_Completed, this.myCommand == _commandWin);
			}
			bool minMatchDurationMinutes = totalMinutes > PromoActionsManager.MobileAdvert.MinMatchDurationMinutes;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log(string.Format("[Rilisoft] Match duration: {0:F2}, threshold: {1:F2}", totalMinutes, PromoActionsManager.MobileAdvert.MinMatchDurationMinutes));
			}
			if (this.isMine && !ShopNGUIController.GuiActive && minMatchDurationMinutes && MobileAdManager.AdIsApplicable(MobileAdManager.Type.Image, true))
			{
				if (flag)
				{
					if (PromoActionsManager.MobileAdvert.ShowInterstitialAfterMatchWinner)
					{
						Task<Ad> task = FyberFacade.Instance.RequestImageInterstitial("NetworkStartTable.win(Winner)");
						FyberFacade.Instance.Requests.AddLast(task);
						base.StartCoroutine(this.WaitInterstitialRequestAndShowCoroutine(task));
						base.StartCoroutine(this.LockInterfaceCoroutine());
					}
					else if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("showInterstitialAfterMatchWinner: false");
					}
				}
				else if (PromoActionsManager.MobileAdvert.ShowInterstitialAfterMatchLoser)
				{
					Task<Ad> task1 = FyberFacade.Instance.RequestImageInterstitial("NetworkStartTable.win(Loser)");
					FyberFacade.Instance.Requests.AddLast(task1);
					base.StartCoroutine(this.WaitInterstitialRequestAndShowCoroutine(task1));
					base.StartCoroutine(this.LockInterfaceCoroutine());
				}
				else if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("showInterstitialAfterMatchLoser: false");
				}
			}
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
			}
			if (flag)
			{
				if (!NetworkStartTable.LocalOrPasswordRoom())
				{
					QuestMediator.NotifyWin(ConnectSceneNGUIController.regim, Application.loadedLevelName);
				}
				if (Defs.isFlag)
				{
					int num5 = Storager.getInt(Defs.RatingFlag, false) + 1;
					Storager.setInt(Defs.RatingFlag, num5, false);
				}
				if (Defs.isCompany)
				{
					int num6 = Storager.getInt(Defs.RatingTeamBattle, false) + 1;
					Storager.setInt(Defs.RatingTeamBattle, num6, false);
				}
				if (Defs.isCapturePoints)
				{
					int num7 = Storager.getInt(Defs.RatingCapturePoint, false) + 1;
					Storager.setInt(Defs.RatingCapturePoint, num7, false);
				}
				if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
				{
					int num8 = Storager.getInt(Defs.RatingDeathmatch, false) + 1;
					Storager.setInt(Defs.RatingDeathmatch, num8, false);
				}
				if (ExperienceController.sharedController != null)
				{
					string str2 = string.Concat("Statistics.WinCount.Level", ExperienceController.sharedController.currentLevel);
					int num9 = PlayerPrefs.GetInt(str2, 0);
					PlayerPrefs.SetInt(str2, num9 + 1);
					FlurryPluginWrapper.LogWinInMatch(ConnectSceneNGUIController.regim.ToString());
				}
				if (!Defs.isCOOP)
				{
					FriendsController.sharedController.SendRoundWon();
					if (PlayerPrefs.GetInt("LogCountMatch", 0) == 1)
					{
						PlayerPrefs.SetInt("LogCountMatch", 0);
						int num10 = PlayerPrefs.GetInt("CountMatch", 0);
						Dictionary<string, string> strs = new Dictionary<string, string>()
						{
							{ "Count matchs", num10.ToString() }
						};
						FlurryPluginWrapper.LogEventAndDublicateToConsole("First WIN in Multiplayer", strs, true);
						if (Social.localUser.authenticated)
						{
							Social.ReportProgress("CgkIr8rGkPIJEAIQAg", 100, (bool success) => UnityEngine.Debug.Log(string.Concat("Achievement First Win completed: ", success)));
						}
					}
				}
			}
			if (this.addCoins > 0 || ExperienceController.sharedController.currentLevel < 31 && this.addExperience > 0)
			{
				this.isShowAvard = true;
				if (PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty))
				{
					this.addCoins = 0;
					this.addExperience = 0;
					this.isShowAvard = false;
				}
			}
		}
		bool flag2 = false;
		int num11 = 0;
		NetworkStartTable networkStartTable1 = null;
		for (int m = 0; m < gameObjects2.Count; m++)
		{
			if (this._weaponManager && gameObjects2[m].Equals(this._weaponManager.myTable))
			{
				this.oldIndexMy = m;
			}
			NetworkStartTable component2 = gameObjects2[m].GetComponent<NetworkStartTable>();
			this.oldSpisokName[m] = component2.NamePlayer;
			this.oldSpisokRanks[m] = component2.myRanks;
			this.oldSpisokPixelBookID[m] = component2.pixelBookID;
			this.oldSpisokMyClanLogo[m] = component2.myClanTexture;
			this.oldScoreSpisok[m] = (component2.score == -1 ? component2.scoreOld.ToString() : component2.score.ToString());
			int num12 = (component2.CountKills == -1 ? component2.oldCountKills : component2.CountKills);
			this.oldCountLilsSpisok[m] = num12.ToString();
			this.oldIsDeadInHungerGame[m] = component2.isDeadInHungerGame;
			if (Defs.isDaterRegim)
			{
				if (num12 > num11)
				{
					networkStartTable1 = component2;
					flag2 = false;
					num11 = num12;
				}
				else if (num12 > 0 && num12 == num11)
				{
					flag2 = true;
				}
			}
		}
		for (int n = 0; n < gameObjects.Count; n++)
		{
			if (this._weaponManager && gameObjects[n].Equals(this._weaponManager.myTable))
			{
				this.oldIndexMy = n;
			}
			this.oldSpisokNameBlue[n] = gameObjects[n].GetComponent<NetworkStartTable>().NamePlayer;
			this.oldSpisokRanksBlue[n] = gameObjects[n].GetComponent<NetworkStartTable>().myRanks;
			this.oldSpisokPixelBookIDBlue[n] = gameObjects[n].GetComponent<NetworkStartTable>().pixelBookID;
			this.oldSpisokMyClanLogoBlue[n] = gameObjects[n].GetComponent<NetworkStartTable>().myClanTexture;
			this.oldScoreSpisokBlue[n] = (gameObjects[n].GetComponent<NetworkStartTable>().score == -1 ? string.Concat(string.Empty, gameObjects[n].GetComponent<NetworkStartTable>().scoreOld) : string.Concat(string.Empty, gameObjects[n].GetComponent<NetworkStartTable>().score));
			this.oldCountLilsSpisokBlue[n] = (gameObjects[n].GetComponent<NetworkStartTable>().CountKills == -1 ? string.Concat(string.Empty, gameObjects[n].GetComponent<NetworkStartTable>().oldCountKills) : string.Concat(string.Empty, gameObjects[n].GetComponent<NetworkStartTable>().CountKills));
		}
		for (int o = 0; o < gameObjects1.Count; o++)
		{
			if (this._weaponManager && gameObjects1[o].Equals(this._weaponManager.myTable))
			{
				this.oldIndexMy = o;
			}
			this.oldSpisokNameRed[o] = gameObjects1[o].GetComponent<NetworkStartTable>().NamePlayer;
			this.oldSpisokRanksRed[o] = gameObjects1[o].GetComponent<NetworkStartTable>().myRanks;
			this.oldSpisokPixelBookIDRed[o] = gameObjects1[o].GetComponent<NetworkStartTable>().pixelBookID;
			this.oldSpisokMyClanLogoRed[o] = gameObjects1[o].GetComponent<NetworkStartTable>().myClanTexture;
			this.oldScoreSpisokRed[o] = (gameObjects1[o].GetComponent<NetworkStartTable>().score == -1 ? string.Concat(string.Empty, gameObjects1[o].GetComponent<NetworkStartTable>().scoreOld) : string.Concat(string.Empty, gameObjects1[o].GetComponent<NetworkStartTable>().score));
			this.oldCountLilsSpisokRed[o] = (gameObjects1[o].GetComponent<NetworkStartTable>().CountKills == -1 ? string.Concat(string.Empty, gameObjects1[o].GetComponent<NetworkStartTable>().oldCountKills) : string.Concat(string.Empty, gameObjects1[o].GetComponent<NetworkStartTable>().CountKills));
		}
		this.myCommandOld = this.myCommand;
		this.oldCountKills = this.CountKills;
		this.scoreOld = this.score;
		this.score = -1;
		GlobalGameController.Score = -1;
		this.scoreCommandFlag1 = 0;
		this.scoreCommandFlag2 = 0;
		this.CountKills = -1;
		if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			this.myCommand = -1;
		}
		this.SynhCommand(null);
		this.SynhCountKills(null);
		this.SynhScore();
		if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.showRanks)
		{
			NetworkStartTableNGUIController.sharedController.BackPressFromRanksTable(true);
		}
		GameObject gameObject1 = GameObject.FindGameObjectWithTag("DamageFrame");
		if (gameObject1 != null)
		{
			UnityEngine.Object.Destroy(gameObject1);
		}
		int num13 = 0;
		if (Defs.isDaterRegim)
		{
			if (networkStartTable1 == null)
			{
				key0568 = LocalizationStore.Get("Key_1427");
			}
			else if (flag2)
			{
				key0568 = LocalizationStore.Get("Key_1764");
			}
			else
			{
				key0568 = (!networkStartTable1.Equals(this) ? string.Format(LocalizationStore.Get("Key_1763"), networkStartTable1.NamePlayer) : LocalizationStore.Get("Key_1762"));
			}
		}
		else if (this.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			string key0571 = LocalizationStore.Key_0571;
			if (this.commandWinner != 0)
			{
				str = (this.commandWinner != this.myCommandOld ? LocalizationStore.Get("Key_1794") : LocalizationStore.Get("Key_1793"));
			}
			else
			{
				str = key0571;
			}
			key0568 = str;
			if (this.commandWinner != 0)
			{
				num = (this.commandWinner != this.myCommandOld ? 2 : 1);
			}
			else
			{
				num = 0;
			}
			num13 = num;
		}
		else if ((!this.isHunger || !this.isDrawInHanger) && !this.isDrawInDeathMatch)
		{
			key0568 = (!flag ? LocalizationStore.Get("Key_1116") : LocalizationStore.Get("Key_1115"));
		}
		else
		{
			key0568 = LocalizationStore.Key_0568;
		}
		this.isShowFinished = true;
		if (NetworkStartTableNGUIController.sharedController != null)
		{
			if (!Defs.isDaterRegim && Defs.isInet && !this.isSetNewMapButton)
			{
				NetworkStartTableNGUIController.sharedController.UpdateGoMapButtons(true);
			}
			if (!Defs.isHunger || !this.isDeadInHungerGame)
			{
				NetworkStartTableNGUIController.sharedController.StartCoroutine(NetworkStartTableNGUIController.sharedController.MatchFinishedInterface(key0568, ratingChange, this.isShowAvard, this.addCoins, this.addExperience, NetworkStartTable.LocalOrPasswordRoom(), (!Defs.isHunger ? count == 0 : this.isIwin), flag1, num13, blueCount, redCount, false));
			}
			else
			{
				NetworkStartTableNGUIController.sharedController.MathFinishedDeadInHunger();
			}
		}
		this.isShowAvard = false;
		this.showTable = false;
		this.isShowNickTable = true;
	}

	public void WinInHunger()
	{
		this.isIwin = true;
		this.photonView.RPC("winInHungerRPC", PhotonTargets.AllBuffered, new object[] { this.NamePlayer });
	}

	[PunRPC]
	[RPC]
	public void winInHungerRPC(string winner)
	{
		this.isEndInHunger = true;
		if (this._weaponManager != null && this._weaponManager.myTable != null)
		{
			this._weaponManager.myTable.GetComponent<NetworkStartTable>().win(winner, 0, 0, 0);
		}
	}

	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}
}