using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PhotonNetwork
{
	public const string versionPUN = "1.73";

	internal const string serverSettingsAssetFile = "PhotonServerSettings";

	internal const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";

	internal readonly static PhotonHandler photonMono;

	internal static NetworkingPeer networkingPeer;

	public readonly static int MAX_VIEW_IDS;

	public static ServerSettings PhotonServerSettings;

	public static bool InstantiateInRoomOnly;

	public static PhotonLogLevel logLevel;

	public static float precisionForVectorSynchronization;

	public static float precisionForQuaternionSynchronization;

	public static float precisionForFloatSynchronization;

	public static bool UseRpcMonoBehaviourCache;

	public static bool UsePrefabCache;

	public static Dictionary<string, GameObject> PrefabCache;

	public static HashSet<GameObject> SendMonoMessageTargets;

	public static Type SendMonoMessageTargetType;

	public static bool StartRpcsAsCoroutine;

	private static bool isOfflineMode;

	private static Room offlineModeRoom;

	[Obsolete("Used for compatibility with Unity networking only.")]
	public static int maxConnections;

	private static bool _mAutomaticallySyncScene;

	private static bool m_autoCleanUpPlayerObjects;

	private static int sendInterval;

	private static int sendIntervalOnSerialize;

	private static bool m_isMessageQueueRunning;

	private static bool UsePreciseTimer;

	private static Stopwatch startupStopwatch;

	public static float BackgroundTimeout;

	public static PhotonNetwork.EventCallback OnEventCall;

	internal static int lastUsedViewSubId;

	internal static int lastUsedViewSubIdStatic;

	internal static List<int> manuallyAllocatedViewIds;

	public static AuthenticationValues AuthValues
	{
		get
		{
			AuthenticationValues authValues;
			if (PhotonNetwork.networkingPeer == null)
			{
				authValues = null;
			}
			else
			{
				authValues = PhotonNetwork.networkingPeer.AuthValues;
			}
			return authValues;
		}
		set
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				PhotonNetwork.networkingPeer.AuthValues = value;
			}
		}
	}

	public static bool autoCleanUpPlayerObjects
	{
		get
		{
			return PhotonNetwork.m_autoCleanUpPlayerObjects;
		}
		set
		{
			if (PhotonNetwork.room == null)
			{
				PhotonNetwork.m_autoCleanUpPlayerObjects = value;
			}
			else
			{
				UnityEngine.Debug.LogError("Setting autoCleanUpPlayerObjects while in a room is not supported.");
			}
		}
	}

	public static bool autoJoinLobby
	{
		get
		{
			return PhotonNetwork.PhotonServerSettings.JoinLobby;
		}
		set
		{
			PhotonNetwork.PhotonServerSettings.JoinLobby = value;
		}
	}

	public static bool automaticallySyncScene
	{
		get
		{
			return PhotonNetwork._mAutomaticallySyncScene;
		}
		set
		{
			PhotonNetwork._mAutomaticallySyncScene = value;
			if (PhotonNetwork._mAutomaticallySyncScene && PhotonNetwork.room != null)
			{
				PhotonNetwork.networkingPeer.LoadLevelIfSynced();
			}
		}
	}

	public static bool connected
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return true;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return false;
			}
			return (PhotonNetwork.networkingPeer.IsInitialConnect || PhotonNetwork.networkingPeer.State == ClientState.PeerCreated || PhotonNetwork.networkingPeer.State == ClientState.Disconnected || PhotonNetwork.networkingPeer.State == ClientState.Disconnecting ? false : PhotonNetwork.networkingPeer.State != ClientState.ConnectingToNameServer);
		}
	}

	public static bool connectedAndReady
	{
		get
		{
			if (!PhotonNetwork.connected)
			{
				return false;
			}
			if (PhotonNetwork.offlineMode)
			{
				return true;
			}
			ClientState clientState = PhotonNetwork.connectionStateDetailed;
			switch (clientState)
			{
				case ClientState.ConnectingToMasterserver:
				case ClientState.Disconnecting:
				case ClientState.Disconnected:
				case ClientState.ConnectingToNameServer:
				case ClientState.Authenticating:
				{
					return false;
				}
				default:
				{
					switch (clientState)
					{
						case ClientState.ConnectingToGameserver:
						case ClientState.Joining:
						{
							return false;
						}
						default:
						{
							if (clientState == ClientState.PeerCreated)
							{
								return false;
							}
							return true;
						}
					}
					break;
				}
			}
		}
	}

	public static bool connecting
	{
		get
		{
			return (!PhotonNetwork.networkingPeer.IsInitialConnect ? false : !PhotonNetwork.offlineMode);
		}
	}

	public static ConnectionState connectionState
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return ConnectionState.Connected;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return ConnectionState.Disconnected;
			}
			PeerStateValue peerState = PhotonNetwork.networkingPeer.PeerState;
			switch (peerState)
			{
				case PeerStateValue.Disconnected:
				{
					return ConnectionState.Disconnected;
				}
				case PeerStateValue.Connecting:
				{
					return ConnectionState.Connecting;
				}
				case PeerStateValue.Connected:
				{
					return ConnectionState.Connected;
				}
				case PeerStateValue.Disconnecting:
				{
					return ConnectionState.Disconnecting;
				}
				default:
				{
					if (peerState == PeerStateValue.InitializingApplication)
					{
						break;
					}
					else
					{
						return ConnectionState.Disconnected;
					}
				}
			}
			return ConnectionState.InitializingApplication;
		}
	}

	public static ClientState connectionStateDetailed
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return (PhotonNetwork.offlineModeRoom == null ? ClientState.ConnectedToMaster : ClientState.Joined);
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return ClientState.Disconnected;
			}
			return PhotonNetwork.networkingPeer.State;
		}
	}

	public static int countOfPlayers
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersInRoomsCount + PhotonNetwork.networkingPeer.PlayersOnMasterCount;
		}
	}

	public static int countOfPlayersInRooms
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersInRoomsCount;
		}
	}

	public static int countOfPlayersOnMaster
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersOnMasterCount;
		}
	}

	public static int countOfRooms
	{
		get
		{
			return PhotonNetwork.networkingPeer.RoomsCount;
		}
	}

	public static bool CrcCheckEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.CrcEnabled;
		}
		set
		{
			if (PhotonNetwork.connected || PhotonNetwork.connecting)
			{
				UnityEngine.Debug.Log(string.Concat("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays ", PhotonNetwork.networkingPeer.CrcEnabled));
			}
			else
			{
				PhotonNetwork.networkingPeer.CrcEnabled = value;
			}
		}
	}

	public static bool EnableLobbyStatistics
	{
		get
		{
			return PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics;
		}
		set
		{
			PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics = value;
		}
	}

	public static List<FriendInfo> Friends
	{
		get;
		internal set;
	}

	public static int FriendsListAge
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null ? 0 : PhotonNetwork.networkingPeer.FriendListAge);
		}
	}

	public static string gameVersion
	{
		get;
		set;
	}

	public static bool inRoom
	{
		get
		{
			return PhotonNetwork.connectionStateDetailed == ClientState.Joined;
		}
	}

	public static bool insideLobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.insideLobby;
		}
	}

	public static bool isMasterClient
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return true;
			}
			return PhotonNetwork.networkingPeer.mMasterClientId == PhotonNetwork.player.ID;
		}
	}

	public static bool isMessageQueueRunning
	{
		get
		{
			return PhotonNetwork.m_isMessageQueueRunning;
		}
		set
		{
			if (value)
			{
				PhotonHandler.StartFallbackSendAckThread();
			}
			PhotonNetwork.networkingPeer.IsSendingOnlyAcks = !value;
			PhotonNetwork.m_isMessageQueueRunning = value;
		}
	}

	public static bool isNonMasterClientInRoom
	{
		get
		{
			return (PhotonNetwork.isMasterClient ? false : PhotonNetwork.room != null);
		}
	}

	public static TypedLobby lobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.lobby;
		}
		set
		{
			PhotonNetwork.networkingPeer.lobby = value;
		}
	}

	public static List<TypedLobbyInfo> LobbyStatistics
	{
		get
		{
			return PhotonNetwork.networkingPeer.LobbyStatistics;
		}
		private set
		{
			PhotonNetwork.networkingPeer.LobbyStatistics = value;
		}
	}

	public static PhotonPlayer masterClient
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.player;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.GetPlayerWithId(PhotonNetwork.networkingPeer.mMasterClientId);
		}
	}

	public static int MaxResendsBeforeDisconnect
	{
		get
		{
			return PhotonNetwork.networkingPeer.SentCountAllowance;
		}
		set
		{
			if (value < 3)
			{
				value = 3;
			}
			if (value > 10)
			{
				value = 10;
			}
			PhotonNetwork.networkingPeer.SentCountAllowance = value;
		}
	}

	public static bool NetworkStatisticsEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.TrafficStatsEnabled;
		}
		set
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = value;
		}
	}

	public static bool offlineMode
	{
		get
		{
			return PhotonNetwork.isOfflineMode;
		}
		set
		{
			if (value == PhotonNetwork.isOfflineMode)
			{
				return;
			}
			if (value && PhotonNetwork.connected)
			{
				UnityEngine.Debug.LogError("Can't start OFFLINE mode while connected!");
				return;
			}
			if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
			{
				PhotonNetwork.networkingPeer.Disconnect();
			}
			PhotonNetwork.isOfflineMode = value;
			if (!PhotonNetwork.isOfflineMode)
			{
				PhotonNetwork.offlineModeRoom = null;
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
			}
			else
			{
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
			}
		}
	}

	public static PhotonPlayer[] otherPlayers
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mOtherPlayerListCopy;
		}
	}

	public static int PacketLossByCrcCheck
	{
		get
		{
			return PhotonNetwork.networkingPeer.PacketLossByCrc;
		}
	}

	public static PhotonPlayer player
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.LocalPlayer;
		}
	}

	public static PhotonPlayer[] playerList
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mPlayerListCopy;
		}
	}

	public static string playerName
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayerName;
		}
		set
		{
			PhotonNetwork.networkingPeer.PlayerName = value;
		}
	}

	public static IPunPrefabPool PrefabPool
	{
		get
		{
			return PhotonNetwork.networkingPeer.ObjectPool;
		}
		set
		{
			PhotonNetwork.networkingPeer.ObjectPool = value;
		}
	}

	public static int QuickResends
	{
		get
		{
			return PhotonNetwork.networkingPeer.QuickResendAttempts;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value > 3)
			{
				value = 3;
			}
			PhotonNetwork.networkingPeer.QuickResendAttempts = (byte)value;
		}
	}

	public static int ResentReliableCommands
	{
		get
		{
			return PhotonNetwork.networkingPeer.ResentReliableCommands;
		}
	}

	public static Room room
	{
		get
		{
			if (PhotonNetwork.isOfflineMode)
			{
				return PhotonNetwork.offlineModeRoom;
			}
			return PhotonNetwork.networkingPeer.CurrentRoom;
		}
	}

	public static int sendRate
	{
		get
		{
			return 1000 / PhotonNetwork.sendInterval;
		}
		set
		{
			PhotonNetwork.sendInterval = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateInterval = PhotonNetwork.sendInterval;
			}
			if (value < PhotonNetwork.sendRateOnSerialize)
			{
				PhotonNetwork.sendRateOnSerialize = value;
			}
		}
	}

	public static int sendRateOnSerialize
	{
		get
		{
			return 1000 / PhotonNetwork.sendIntervalOnSerialize;
		}
		set
		{
			if (value > PhotonNetwork.sendRate)
			{
				UnityEngine.Debug.LogError("Error, can not set the OnSerialize SendRate more often then the overall SendRate");
				value = PhotonNetwork.sendRate;
			}
			PhotonNetwork.sendIntervalOnSerialize = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateIntervalOnSerialize = PhotonNetwork.sendIntervalOnSerialize;
			}
		}
	}

	public static ServerConnection Server
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null ? ServerConnection.NameServer : PhotonNetwork.networkingPeer.Server);
		}
	}

	public static string ServerAddress
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null ? "<not connected>" : PhotonNetwork.networkingPeer.ServerAddress);
		}
	}

	public static int ServerTimestamp
	{
		get
		{
			if (!PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds;
			}
			if (!PhotonNetwork.UsePreciseTimer || PhotonNetwork.startupStopwatch == null || !PhotonNetwork.startupStopwatch.IsRunning)
			{
				return Environment.TickCount;
			}
			return (int)PhotonNetwork.startupStopwatch.ElapsedMilliseconds;
		}
	}

	public static double time
	{
		get
		{
			return (double)((float)PhotonNetwork.ServerTimestamp) / 1000;
		}
	}

	public static int unreliableCommandsLimit
	{
		get
		{
			return PhotonNetwork.networkingPeer.LimitOfUnreliableCommands;
		}
		set
		{
			PhotonNetwork.networkingPeer.LimitOfUnreliableCommands = value;
		}
	}

	static PhotonNetwork()
	{
		PhotonNetwork.MAX_VIEW_IDS = 1000;
		PhotonNetwork.PhotonServerSettings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));
		PhotonNetwork.InstantiateInRoomOnly = true;
		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
		PhotonNetwork.precisionForVectorSynchronization = 9.9E-05f;
		PhotonNetwork.precisionForQuaternionSynchronization = 1f;
		PhotonNetwork.precisionForFloatSynchronization = 0.01f;
		PhotonNetwork.UsePrefabCache = true;
		PhotonNetwork.PrefabCache = new Dictionary<string, GameObject>();
		PhotonNetwork.SendMonoMessageTargetType = typeof(MonoBehaviour);
		PhotonNetwork.StartRpcsAsCoroutine = true;
		PhotonNetwork.isOfflineMode = false;
		PhotonNetwork.offlineModeRoom = null;
		PhotonNetwork._mAutomaticallySyncScene = false;
		PhotonNetwork.m_autoCleanUpPlayerObjects = true;
		PhotonNetwork.sendInterval = 50;
		PhotonNetwork.sendIntervalOnSerialize = 100;
		PhotonNetwork.m_isMessageQueueRunning = true;
		PhotonNetwork.UsePreciseTimer = false;
		PhotonNetwork.BackgroundTimeout = 60f;
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
		PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		Application.runInBackground = true;
		GameObject gameObject = new GameObject();
		PhotonNetwork.photonMono = gameObject.AddComponent<PhotonHandler>();
		gameObject.name = "PhotonMono";
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		ConnectionProtocol protocol = PhotonNetwork.PhotonServerSettings.Protocol;
		PhotonNetwork.networkingPeer = new NetworkingPeer(string.Empty, protocol)
		{
			QuickResendAttempts = 2,
			SentCountAllowance = 7
		};
		if (PhotonNetwork.UsePreciseTimer)
		{
			UnityEngine.Debug.Log("Using Stopwatch as precision timer for PUN.");
			PhotonNetwork.startupStopwatch = new Stopwatch();
			PhotonNetwork.startupStopwatch.Start();
			PhotonNetwork.networkingPeer.LocalMsTimestampDelegate = () => (int)PhotonNetwork.startupStopwatch.ElapsedMilliseconds;
		}
		CustomTypes.Register();
	}

	public static int AllocateSceneViewID()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError("Only the Master Client can AllocateSceneViewID(). Check PhotonNetwork.isMasterClient!");
			return -1;
		}
		int num = PhotonNetwork.AllocateViewID(0);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	private static int[] AllocateSceneViewIDs(int countOfNewViews)
	{
		int[] numArray = new int[countOfNewViews];
		for (int i = 0; i < countOfNewViews; i++)
		{
			numArray[i] = PhotonNetwork.AllocateViewID(0);
		}
		return numArray;
	}

	public static int AllocateViewID()
	{
		int num = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	private static int AllocateViewID(int ownerId)
	{
		if (ownerId == 0)
		{
			int mAXVIEWIDS = PhotonNetwork.lastUsedViewSubIdStatic;
			int num = ownerId * PhotonNetwork.MAX_VIEW_IDS;
			for (int i = 1; i < PhotonNetwork.MAX_VIEW_IDS; i++)
			{
				mAXVIEWIDS = (mAXVIEWIDS + 1) % PhotonNetwork.MAX_VIEW_IDS;
				if (mAXVIEWIDS != 0)
				{
					int num1 = mAXVIEWIDS + num;
					if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num1))
					{
						PhotonNetwork.lastUsedViewSubIdStatic = mAXVIEWIDS;
						return num1;
					}
				}
			}
			throw new Exception(string.Format("AllocateViewID() failed. Room (user {0}) is out of 'scene' viewIDs. It seems all available are in use.", ownerId));
		}
		int mAXVIEWIDS1 = PhotonNetwork.lastUsedViewSubId;
		int num2 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
		for (int j = 1; j < PhotonNetwork.MAX_VIEW_IDS; j++)
		{
			mAXVIEWIDS1 = (mAXVIEWIDS1 + 1) % PhotonNetwork.MAX_VIEW_IDS;
			if (mAXVIEWIDS1 != 0)
			{
				int num3 = mAXVIEWIDS1 + num2;
				if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num3) && !PhotonNetwork.manuallyAllocatedViewIds.Contains(num3))
				{
					PhotonNetwork.lastUsedViewSubId = mAXVIEWIDS1;
					return num3;
				}
			}
		}
		throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of subIds, as all viewIDs are used.", ownerId));
	}

	public static void CacheSendMonoMessageTargets(Type type)
	{
		if (type == null)
		{
			type = PhotonNetwork.SendMonoMessageTargetType;
		}
		PhotonNetwork.SendMonoMessageTargets = PhotonNetwork.FindGameObjectsWithComponent(type);
	}

	public static bool CloseConnection(PhotonPlayer kickPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return false;
		}
		if (!PhotonNetwork.player.isMasterClient)
		{
			UnityEngine.Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
			return false;
		}
		if (kickPlayer == null)
		{
			UnityEngine.Debug.LogError("CloseConnection: No such player connected!");
			return false;
		}
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			TargetActors = new int[] { kickPlayer.ID }
		};
		return PhotonNetwork.networkingPeer.OpRaiseEvent(203, null, true, raiseEventOption);
	}

	public static bool ConnectToBestCloudServer(string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning(string.Concat("ConnectToBestCloudServer() failed. Can only connect while in state 'Disconnected'. Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		CloudRegionCode bestRegionCodeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
		if (bestRegionCodeInPreferences == CloudRegionCode.none)
		{
			return PhotonNetwork.networkingPeer.ConnectToNameServer();
		}
		UnityEngine.Debug.Log(string.Concat("Best region found in PlayerPrefs. Connecting to: ", bestRegionCodeInPreferences));
		return PhotonNetwork.networkingPeer.ConnectToRegionMaster(bestRegionCodeInPreferences);
	}

	public static bool ConnectToMaster(string masterServerAddress, int port, string appID, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning(string.Concat("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			UnityEngine.Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			UnityEngine.Debug.LogWarning("ConnectToMaster() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.SetApp(appID, gameVersion);
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.MasterServerAddress = (port != 0 ? string.Concat(masterServerAddress, ":", port) : masterServerAddress);
		return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
	}

	public static bool ConnectToRegion(CloudRegionCode region, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning(string.Concat("ConnectToRegion() failed. Can only connect while in state 'Disconnected'. Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			UnityEngine.Debug.LogError("Can't connect: ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (region == CloudRegionCode.none)
		{
			return false;
		}
		UnityEngine.Debug.Log(string.Concat("ConnectToRegion: ", region));
		return PhotonNetwork.networkingPeer.ConnectToRegionMaster(region);
	}

	public static bool ConnectUsingSettings(string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning(string.Concat("ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.NotSet)
		{
			UnityEngine.Debug.LogError("You did not select a Hosting Type in your PhotonServerSettings. Please set it up or don't use ConnectUsingSettings().");
			return false;
		}
		PhotonNetwork.SwitchToProtocol(PhotonNetwork.PhotonServerSettings.Protocol);
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			PhotonNetwork.offlineMode = true;
			return true;
		}
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
		}
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		if (PhotonNetwork.PhotonServerSettings.HostType != ServerSettings.HostingOption.SelfHosted)
		{
			if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
			{
				return PhotonNetwork.ConnectToBestCloudServer(gameVersion);
			}
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.PreferredRegion);
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.MasterServerAddress = (PhotonNetwork.PhotonServerSettings.ServerPort != 0 ? string.Concat(PhotonNetwork.PhotonServerSettings.ServerAddress, ":", PhotonNetwork.PhotonServerSettings.ServerPort) : PhotonNetwork.PhotonServerSettings.ServerAddress);
		return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
	}

	public static bool CreateRoom(string roomName)
	{
		return PhotonNetwork.CreateRoom(roomName, null, null, null);
	}

	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		return PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby, null);
	}

	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, roomOptions, true);
			return true;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			UnityEngine.Debug.LogError("CreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		object obj = typedLobby;
		if (obj == null)
		{
			if (!PhotonNetwork.networkingPeer.insideLobby)
			{
				obj = null;
			}
			else
			{
				obj = PhotonNetwork.networkingPeer.lobby;
			}
		}
		typedLobby = (TypedLobby)obj;
		EnterRoomParams enterRoomParam = new EnterRoomParams()
		{
			RoomName = roomName,
			RoomOptions = roomOptions,
			Lobby = typedLobby,
			ExpectedUsers = expectedUsers
		};
		return PhotonNetwork.networkingPeer.OpCreateGame(enterRoomParam);
	}

	public static void Destroy(PhotonView targetView)
	{
		if (targetView == null)
		{
			UnityEngine.Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
		}
		else
		{
			PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetView.gameObject, !PhotonNetwork.inRoom);
		}
	}

	public static void Destroy(GameObject targetGo)
	{
		PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetGo, !PhotonNetwork.inRoom);
	}

	public static void DestroyAll()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
		}
		else
		{
			PhotonNetwork.networkingPeer.DestroyAll(false);
		}
	}

	public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
	{
		if (PhotonNetwork.player == null)
		{
			UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
		}
		PhotonNetwork.DestroyPlayerObjects(targetPlayer.ID);
	}

	public static void DestroyPlayerObjects(int targetPlayerId)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.player.isMasterClient || targetPlayerId == PhotonNetwork.player.ID)
		{
			PhotonNetwork.networkingPeer.DestroyPlayerObjects(targetPlayerId, false);
		}
		else
		{
			UnityEngine.Debug.LogError(string.Concat("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: ", PhotonNetwork.isMasterClient));
		}
	}

	public static void Disconnect()
	{
		if (!PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return;
			}
			PhotonNetwork.networkingPeer.Disconnect();
			return;
		}
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.offlineModeRoom = null;
		PhotonNetwork.networkingPeer.State = ClientState.Disconnecting;
		PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.Disconnect);
	}

	private static void EnterOfflineRoom(string roomName, RoomOptions roomOptions, bool createdRoom)
	{
		PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
		PhotonNetwork.networkingPeer.ChangeLocalID(1);
		PhotonNetwork.offlineModeRoom.masterClientId = 1;
		if (createdRoom)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
		}
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
	}

	public static void FetchServerTimestamp()
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.FetchServerTimestamp();
		}
	}

	public static bool FindFriends(string[] friendsToFind)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.isOfflineMode)
		{
			return false;
		}
		return PhotonNetwork.networkingPeer.OpFindFriends(friendsToFind);
	}

	public static HashSet<GameObject> FindGameObjectsWithComponent(Type type)
	{
		HashSet<GameObject> gameObjects = new HashSet<GameObject>();
		Component[] componentArray = (Component[])UnityEngine.Object.FindObjectsOfType(type);
		for (int i = 0; i < (int)componentArray.Length; i++)
		{
			gameObjects.Add(componentArray[i].gameObject);
		}
		return gameObjects;
	}

	public static int GetPing()
	{
		return PhotonNetwork.networkingPeer.RoundTripTime;
	}

	public static RoomInfo[] GetRoomList()
	{
		if (!PhotonNetwork.offlineMode && PhotonNetwork.networkingPeer != null)
		{
			return PhotonNetwork.networkingPeer.mGameListCopy;
		}
		return new RoomInfo[0];
	}

	[Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
	public static void InitializeSecurity()
	{
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group)
	{
		return PhotonNetwork.Instantiate(prefabName, position, rotation, group, null);
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		GameObject gameObject;
		if (!PhotonNetwork.connected || PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[] { "Failed to Instantiate prefab: ", prefabName, ". Client should be in a room. Current connectionStateDetailed: ", PhotonNetwork.connectionStateDetailed }));
			return null;
		}
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
		{
			gameObject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
			}
		}
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError(string.Concat("Failed to Instantiate prefab: ", prefabName, ". Verify the Prefab is in a Resources folder (and not in a subfolder)"));
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			UnityEngine.Debug.LogError(string.Concat("Failed to Instantiate prefab:", prefabName, ". Prefab must have a PhotonView component."));
			return null;
		}
		int[] numArray = new int[(int)gameObject.GetPhotonViewsInChildren().Length];
		for (int i = 0; i < (int)numArray.Length; i++)
		{
			numArray[i] = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		}
		Hashtable hashtable = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, numArray, data, false);
		return PhotonNetwork.networkingPeer.DoInstantiate(hashtable, PhotonNetwork.networkingPeer.LocalPlayer, gameObject);
	}

	public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		GameObject gameObject;
		if (!PhotonNetwork.connected || PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[] { "Failed to InstantiateSceneObject prefab: ", prefabName, ". Client should be in a room. Current connectionStateDetailed: ", PhotonNetwork.connectionStateDetailed }));
			return null;
		}
		if (!PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError(string.Concat("Failed to InstantiateSceneObject prefab: ", prefabName, ". Client is not the MasterClient in this room."));
			return null;
		}
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
		{
			gameObject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
			}
		}
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError(string.Concat("Failed to InstantiateSceneObject prefab: ", prefabName, ". Verify the Prefab is in a Resources folder (and not in a subfolder)"));
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			UnityEngine.Debug.LogError(string.Concat("Failed to InstantiateSceneObject prefab:", prefabName, ". Prefab must have a PhotonView component."));
			return null;
		}
		Component[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
		int[] numArray = PhotonNetwork.AllocateSceneViewIDs((int)photonViewsInChildren.Length);
		if (numArray != null)
		{
			Hashtable hashtable = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, numArray, data, true);
			return PhotonNetwork.networkingPeer.DoInstantiate(hashtable, PhotonNetwork.networkingPeer.LocalPlayer, gameObject);
		}
		UnityEngine.Debug.LogError(string.Concat(new object[] { "Failed to InstantiateSceneObject prefab: ", prefabName, ". No ViewIDs are free to use. Max is: ", PhotonNetwork.MAX_VIEW_IDS }));
		return null;
	}

	public static bool JoinLobby()
	{
		return PhotonNetwork.JoinLobby(null);
	}

	public static bool JoinLobby(TypedLobby typedLobby)
	{
		if (!PhotonNetwork.connected || PhotonNetwork.Server != ServerConnection.MasterServer)
		{
			return false;
		}
		if (typedLobby == null)
		{
			typedLobby = TypedLobby.Default;
		}
		bool flag = PhotonNetwork.networkingPeer.OpJoinLobby(typedLobby);
		if (flag)
		{
			PhotonNetwork.networkingPeer.lobby = typedLobby;
		}
		return flag;
	}

	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		return PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby, null);
	}

	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, roomOptions, true);
			return true;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			UnityEngine.Debug.LogError("JoinOrCreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			UnityEngine.Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		object obj = typedLobby;
		if (obj == null)
		{
			if (!PhotonNetwork.networkingPeer.insideLobby)
			{
				obj = null;
			}
			else
			{
				obj = PhotonNetwork.networkingPeer.lobby;
			}
		}
		typedLobby = (TypedLobby)obj;
		EnterRoomParams enterRoomParam = new EnterRoomParams()
		{
			RoomName = roomName,
			RoomOptions = roomOptions,
			Lobby = typedLobby,
			CreateIfNotExists = true,
			PlayerProperties = PhotonNetwork.player.customProperties,
			ExpectedUsers = expectedUsers
		};
		return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParam);
	}

	public static bool JoinRandomRoom()
	{
		return PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null, null);
	}

	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
	{
		return PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null, null);
	}

	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers = null)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom("offline room", null, true);
			return true;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			UnityEngine.Debug.LogError("JoinRandomRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		object obj = typedLobby;
		if (obj == null)
		{
			if (!PhotonNetwork.networkingPeer.insideLobby)
			{
				obj = null;
			}
			else
			{
				obj = PhotonNetwork.networkingPeer.lobby;
			}
		}
		typedLobby = (TypedLobby)obj;
		OpJoinRandomRoomParams opJoinRandomRoomParam = new OpJoinRandomRoomParams()
		{
			ExpectedCustomRoomProperties = expectedCustomRoomProperties,
			ExpectedMaxPlayers = expectedMaxPlayers,
			MatchingType = matchingType,
			TypedLobby = typedLobby,
			SqlLobbyFilter = sqlLobbyFilter,
			ExpectedUsers = expectedUsers
		};
		return PhotonNetwork.networkingPeer.OpJoinRandomRoom(opJoinRandomRoomParam);
	}

	public static bool JoinRoom(string roomName)
	{
		return PhotonNetwork.JoinRoom(roomName, null);
	}

	public static bool JoinRoom(string roomName, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				UnityEngine.Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, null, true);
			return true;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			UnityEngine.Debug.LogError("JoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			UnityEngine.Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		EnterRoomParams enterRoomParam = new EnterRoomParams()
		{
			RoomName = roomName,
			ExpectedUsers = expectedUsers
		};
		return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParam);
	}

	public static bool LeaveLobby()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.Server != ServerConnection.MasterServer)
		{
			return false;
		}
		return PhotonNetwork.networkingPeer.OpLeaveLobby();
	}

	public static bool LeaveRoom()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineModeRoom = null;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
			return true;
		}
		if (PhotonNetwork.room == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("PhotonNetwork.room is null. You don't have to call LeaveRoom() when you're not in one. State: ", PhotonNetwork.connectionStateDetailed));
		}
		return PhotonNetwork.networkingPeer.OpLeave();
	}

	public static void LoadLevel(int levelNumber)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelNumber);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		SceneManager.LoadScene(levelNumber);
	}

	public static void LoadLevel(string levelName)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelName);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		SceneManager.LoadScene(levelName);
	}

	public static void NetworkStatisticsReset()
	{
		PhotonNetwork.networkingPeer.TrafficStatsReset();
	}

	public static string NetworkStatisticsToString()
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.offlineMode)
		{
			return "Offline or in OfflineMode. No VitalStats available.";
		}
		return PhotonNetwork.networkingPeer.VitalStatsToString(false);
	}

	public static void OverrideBestCloudServer(CloudRegionCode region)
	{
		PhotonHandler.BestRegionCodeInPreferences = region;
	}

	public static bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
	{
		if (!PhotonNetwork.inRoom || eventCode >= 200)
		{
			UnityEngine.Debug.LogWarning("RaiseEvent() failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
			return false;
		}
		return PhotonNetwork.networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
	}

	public static bool Reconnect()
	{
		if (string.IsNullOrEmpty(PhotonNetwork.networkingPeer.MasterServerAddress))
		{
			UnityEngine.Debug.LogWarning(string.Concat("Reconnect() failed. It seems the client wasn't connected before?! Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Reconnect() failed. Can only connect while in state 'Disconnected'. Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			UnityEngine.Debug.LogWarning("Reconnect() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			UnityEngine.Debug.LogWarning("Reconnect() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = false;
		return PhotonNetwork.networkingPeer.ReconnectToMaster();
	}

	public static bool ReconnectAndRejoin()
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			UnityEngine.Debug.LogWarning(string.Concat("ReconnectAndRejoin() failed. Can only connect while in state 'Disconnected'. Current state: ", PhotonNetwork.networkingPeer.PeerState));
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() disabled the offline mode. No longer offline.");
		}
		if (string.IsNullOrEmpty(PhotonNetwork.networkingPeer.GameServerAddress))
		{
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. It seems the client wasn't connected to a game server before (no address).");
			return false;
		}
		if (PhotonNetwork.networkingPeer.enterRoomParamsCache == null)
		{
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. It seems the client doesn't have any previous room to re-join.");
			return false;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			UnityEngine.Debug.LogWarning("ReconnectAndRejoin() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = false;
		return PhotonNetwork.networkingPeer.ReconnectAndRejoin();
	}

	public static void RefreshCloudServerRating()
	{
		throw new NotImplementedException("not available at the moment");
	}

	public static bool ReJoinRoom(string roomName)
	{
		if (PhotonNetwork.offlineMode)
		{
			UnityEngine.Debug.LogError("ReJoinRoom failed due to offline mode.");
			return false;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			UnityEngine.Debug.LogError("ReJoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			UnityEngine.Debug.LogError("ReJoinRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		EnterRoomParams enterRoomParam = new EnterRoomParams()
		{
			RoomName = roomName,
			RejoinOnly = true,
			PlayerProperties = PhotonNetwork.player.customProperties
		};
		return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParam);
	}

	public static void RemovePlayerCustomProperties(string[] customPropertiesToDelete)
	{
		if (customPropertiesToDelete == null || (int)customPropertiesToDelete.Length == 0 || PhotonNetwork.player.customProperties == null)
		{
			PhotonNetwork.player.customProperties = new Hashtable();
			return;
		}
		for (int i = 0; i < (int)customPropertiesToDelete.Length; i++)
		{
			string str = customPropertiesToDelete[i];
			if (PhotonNetwork.player.customProperties.ContainsKey(str))
			{
				PhotonNetwork.player.customProperties.Remove(str);
			}
		}
	}

	public static void RemoveRPCs(PhotonPlayer targetPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (!targetPlayer.isLocal && !PhotonNetwork.isMasterClient)
		{
			UnityEngine.Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
			return;
		}
		PhotonNetwork.networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
	}

	public static void RemoveRPCs(PhotonView targetPhotonView)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
	}

	public static void RemoveRPCsInGroup(int targetGroup)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.RemoveRPCsInGroup(targetGroup);
	}

	internal static void RPC(PhotonView view, string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.room == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("RPCs can only be sent in rooms. Call of \"", methodName, "\" gets executed locally only, if at all."));
			return;
		}
		if (PhotonNetwork.networkingPeer == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Could not execute RPC ", methodName, ". Possible scene loading in progress?"));
		}
		else if (PhotonNetwork.room.serverSideMasterClient)
		{
			PhotonNetwork.networkingPeer.RPC(view, methodName, target, null, encrypt, parameters);
		}
		else if (!PhotonNetwork.networkingPeer.hasSwitchedMC || target != PhotonTargets.MasterClient)
		{
			PhotonNetwork.networkingPeer.RPC(view, methodName, target, null, encrypt, parameters);
		}
		else
		{
			PhotonNetwork.networkingPeer.RPC(view, methodName, PhotonTargets.Others, PhotonNetwork.masterClient, encrypt, parameters);
		}
	}

	internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, bool encrpyt, params object[] parameters)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.room == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("RPCs can only be sent in rooms. Call of \"", methodName, "\" gets executed locally only, if at all."));
			return;
		}
		if (PhotonNetwork.player == null)
		{
			UnityEngine.Debug.LogError(string.Concat("RPC can't be sent to target PhotonPlayer being null! Did not send \"", methodName, "\" call."));
		}
		if (PhotonNetwork.networkingPeer == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("Could not execute RPC ", methodName, ". Possible scene loading in progress?"));
		}
		else
		{
			PhotonNetwork.networkingPeer.RPC(view, methodName, PhotonTargets.Others, targetPlayer, encrpyt, parameters);
		}
	}

	public static void SendOutgoingCommands()
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		while (PhotonNetwork.networkingPeer.SendOutgoingCommands())
		{
		}
	}

	public static void SetLevelPrefix(short prefix)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetLevelPrefix(prefix);
	}

	public static bool SetMasterClient(PhotonPlayer masterClientPlayer)
	{
		if (!PhotonNetwork.inRoom || !PhotonNetwork.VerifyCanUseNetwork() || PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.logLevel == PhotonLogLevel.Informational)
			{
				UnityEngine.Debug.Log("Can not SetMasterClient(). Not in room or in offlineMode.");
			}
			return false;
		}
		if (!PhotonNetwork.room.serverSideMasterClient)
		{
			if (!PhotonNetwork.isMasterClient)
			{
				return false;
			}
			return PhotonNetwork.networkingPeer.SetMasterClient(masterClientPlayer.ID, true);
		}
		Hashtable hashtable = new Hashtable()
		{
			{ (byte)248, masterClientPlayer.ID }
		};
		Hashtable hashtable1 = hashtable;
		hashtable = new Hashtable()
		{
			{ (byte)248, PhotonNetwork.networkingPeer.mMasterClientId }
		};
		return PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable1, hashtable, false);
	}

	public static void SetPlayerCustomProperties(Hashtable customProperties)
	{
		if (customProperties == null)
		{
			customProperties = new Hashtable();
			foreach (object key in PhotonNetwork.player.customProperties.Keys)
			{
				customProperties[(string)key] = null;
			}
		}
		if (PhotonNetwork.room == null || !PhotonNetwork.room.isLocalClientInside)
		{
			PhotonNetwork.player.InternalCacheProperties(customProperties);
		}
		else
		{
			PhotonNetwork.player.SetCustomProperties(customProperties, null, false);
		}
	}

	public static void SetReceivingEnabled(int group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetReceivingEnabled(group, enabled);
	}

	public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetReceivingEnabled(enableGroups, disableGroups);
	}

	public static void SetSendingEnabled(int group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetSendingEnabled(group, enabled);
	}

	public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetSendingEnabled(enableGroups, disableGroups);
	}

	public static void SwitchToProtocol(ConnectionProtocol cp)
	{
		if (PhotonNetwork.networkingPeer.UsedProtocol == cp)
		{
			return;
		}
		try
		{
			PhotonNetwork.networkingPeer.Disconnect();
			PhotonNetwork.networkingPeer.StopThread();
		}
		catch
		{
		}
		NetworkingPeer networkingPeer = new NetworkingPeer(string.Empty, cp)
		{
			AuthValues = PhotonNetwork.networkingPeer.AuthValues,
			PlayerName = PhotonNetwork.networkingPeer.PlayerName,
			LocalPlayer = PhotonNetwork.networkingPeer.LocalPlayer,
			DebugOut = PhotonNetwork.networkingPeer.DebugOut,
			CrcEnabled = PhotonNetwork.networkingPeer.CrcEnabled,
			QuickResendAttempts = PhotonNetwork.networkingPeer.QuickResendAttempts,
			DisconnectTimeout = PhotonNetwork.networkingPeer.DisconnectTimeout,
			lobby = PhotonNetwork.networkingPeer.lobby,
			LimitOfUnreliableCommands = PhotonNetwork.networkingPeer.LimitOfUnreliableCommands,
			SentCountAllowance = PhotonNetwork.networkingPeer.SentCountAllowance,
			TrafficStatsEnabled = PhotonNetwork.networkingPeer.TrafficStatsEnabled
		};
		PhotonNetwork.networkingPeer = networkingPeer;
		UnityEngine.Debug.LogWarning(string.Concat("Protocol switched to: ", cp, "."));
	}

	public static void UnAllocateViewID(int viewID)
	{
		PhotonNetwork.manuallyAllocatedViewIds.Remove(viewID);
		if (PhotonNetwork.networkingPeer.photonViewList.ContainsKey(viewID))
		{
			UnityEngine.Debug.LogWarning(string.Format("UnAllocateViewID() should be called after the PhotonView was destroyed (GameObject.Destroy()). ViewID: {0} still found in: {1}", viewID, PhotonNetwork.networkingPeer.photonViewList[viewID]));
		}
	}

	private static bool VerifyCanUseNetwork()
	{
		if (PhotonNetwork.connected)
		{
			return true;
		}
		UnityEngine.Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
		return false;
	}

	public static bool WebRpc(string name, object parameters)
	{
		return PhotonNetwork.networkingPeer.WebRpc(name, parameters);
	}

	public delegate void EventCallback(byte eventCode, object content, int senderId);
}