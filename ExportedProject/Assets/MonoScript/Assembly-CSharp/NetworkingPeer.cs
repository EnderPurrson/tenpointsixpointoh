using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

internal class NetworkingPeer : LoadBalancingPeer, IPhotonPeerListener
{
	public const string NameServerHost = "ns.exitgames.com";

	public const string NameServerHttp = "http://ns.exitgamescloud.com:80/photon/n";

	protected internal const string CurrentSceneProperty = "curScn";

	public const int SyncViewId = 0;

	public const int SyncCompressed = 1;

	public const int SyncNullValues = 2;

	public const int SyncFirstValue = 3;

	private string tokenCache;

	protected internal string AppId;

	private readonly static Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort;

	public bool IsInitialConnect;

	public bool insideLobby;

	protected internal List<TypedLobbyInfo> LobbyStatistics = new List<TypedLobbyInfo>();

	public Dictionary<string, RoomInfo> mGameList = new Dictionary<string, RoomInfo>();

	public RoomInfo[] mGameListCopy = new RoomInfo[0];

	private string playername = string.Empty;

	private bool mPlayernameHasToBeUpdated;

	private Room currentRoom;

	private JoinType lastJoinType;

	protected internal EnterRoomParams enterRoomParamsCache;

	private bool didAuthenticate;

	private string[] friendListRequested;

	private int friendListTimestamp;

	private bool isFetchingFriendList;

	public Dictionary<int, PhotonPlayer> mActors = new Dictionary<int, PhotonPlayer>();

	public PhotonPlayer[] mOtherPlayerListCopy = new PhotonPlayer[0];

	public PhotonPlayer[] mPlayerListCopy = new PhotonPlayer[0];

	public bool hasSwitchedMC;

	private HashSet<int> allowedReceivingGroups = new HashSet<int>();

	private HashSet<int> blockSendingGroups = new HashSet<int>();

	protected internal Dictionary<int, PhotonView> photonViewList = new Dictionary<int, PhotonView>();

	private readonly PhotonStream pStream = new PhotonStream(true, null);

	private readonly Dictionary<int, ExitGames.Client.Photon.Hashtable> dataPerGroupReliable = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();

	private readonly Dictionary<int, ExitGames.Client.Photon.Hashtable> dataPerGroupUnreliable = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();

	protected internal short currentLevelPrefix;

	protected internal bool loadingLevelAndPausedNetwork;

	public static bool UsePrefabCache;

	internal IPunPrefabPool ObjectPool;

	public static Dictionary<string, GameObject> PrefabCache;

	private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();

	private readonly Dictionary<string, int> rpcShortcuts;

	private Dictionary<int, object[]> tempInstantiationData = new Dictionary<int, object[]>();

	protected internal string AppVersion
	{
		get
		{
			return string.Format("{0}_{1}", PhotonNetwork.gameVersion, "1.73");
		}
	}

	public AuthenticationValues AuthValues
	{
		get;
		set;
	}

	public List<Region> AvailableRegions
	{
		get;
		protected internal set;
	}

	public CloudRegionCode CloudRegion
	{
		get;
		protected internal set;
	}

	public Room CurrentRoom
	{
		get
		{
			if (this.currentRoom == null || !this.currentRoom.isLocalClientInside)
			{
				return null;
			}
			return this.currentRoom;
		}
		private set
		{
			this.currentRoom = value;
		}
	}

	protected internal int FriendListAge
	{
		get
		{
			return (this.isFetchingFriendList || this.friendListTimestamp == 0 ? 0 : Environment.TickCount - this.friendListTimestamp);
		}
	}

	public string GameServerAddress
	{
		get;
		protected internal set;
	}

	public bool IsAuthorizeSecretAvailable
	{
		get
		{
			return (this.AuthValues == null ? false : !string.IsNullOrEmpty(this.AuthValues.Token));
		}
	}

	public bool IsUsingNameServer
	{
		get;
		protected internal set;
	}

	public TypedLobby lobby
	{
		get;
		set;
	}

	public PhotonPlayer LocalPlayer
	{
		get;
		internal set;
	}

	public string MasterServerAddress
	{
		get;
		protected internal set;
	}

	public int mMasterClientId
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return this.LocalPlayer.ID;
			}
			return (this.CurrentRoom != null ? this.CurrentRoom.masterClientId : 0);
		}
		private set
		{
			if (this.CurrentRoom != null)
			{
				this.CurrentRoom.masterClientId = value;
			}
		}
	}

	public string NameServerAddress
	{
		get
		{
			return this.GetNameServerAddress();
		}
	}

	public string PlayerName
	{
		get
		{
			return this.playername;
		}
		set
		{
			if (string.IsNullOrEmpty(value) || value.Equals(this.playername))
			{
				return;
			}
			if (this.LocalPlayer != null)
			{
				this.LocalPlayer.name = value;
			}
			this.playername = value;
			if (this.CurrentRoom != null)
			{
				this.SendPlayerName();
			}
		}
	}

	public int PlayersInRoomsCount
	{
		get;
		internal set;
	}

	public int PlayersOnMasterCount
	{
		get;
		internal set;
	}

	private bool requestLobbyStatistics
	{
		get
		{
			return (!PhotonNetwork.EnableLobbyStatistics ? false : this.Server == ServerConnection.MasterServer);
		}
	}

	public int RoomsCount
	{
		get;
		internal set;
	}

	protected internal ServerConnection Server
	{
		get;
		private set;
	}

	public ClientState State
	{
		get;
		internal set;
	}

	static NetworkingPeer()
	{
		Dictionary<ConnectionProtocol, int> connectionProtocols = new Dictionary<ConnectionProtocol, int>()
		{
			{ ConnectionProtocol.Udp, 5058 },
			{ ConnectionProtocol.Tcp, 4533 },
			{ ConnectionProtocol.WebSocket, 9093 },
			{ ConnectionProtocol.WebSocketSecure, 19093 }
		};
		NetworkingPeer.ProtocolToNameServerPort = connectionProtocols;
		NetworkingPeer.UsePrefabCache = true;
		NetworkingPeer.PrefabCache = new Dictionary<string, GameObject>();
	}

	public NetworkingPeer(string playername, ConnectionProtocol connectionProtocol) : base(connectionProtocol)
	{
		base.Listener = this;
		if (PhotonHandler.PingImplementation == null)
		{
			PhotonHandler.PingImplementation = typeof(PingMono);
		}
		base.LimitOfUnreliableCommands = 40;
		this.lobby = TypedLobby.Default;
		this.PlayerName = playername;
		this.LocalPlayer = new PhotonPlayer(true, -1, this.playername);
		this.AddNewPlayer(this.LocalPlayer.ID, this.LocalPlayer);
		this.rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
		for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
		{
			string item = PhotonNetwork.PhotonServerSettings.RpcList[i];
			this.rpcShortcuts[item] = i;
		}
		this.State = ClientState.PeerCreated;
	}

	private void AddNewPlayer(int ID, PhotonPlayer player)
	{
		if (this.mActors.ContainsKey(ID))
		{
			Debug.LogError(string.Concat("Adding player twice: ", ID));
		}
		else
		{
			this.mActors[ID] = player;
			this.RebuildPlayerListCopies();
		}
	}

	private bool AlmostEquals(object[] lastData, object[] currentContent)
	{
		if (lastData == null && currentContent == null)
		{
			return true;
		}
		if (lastData == null || currentContent == null || (int)lastData.Length != (int)currentContent.Length)
		{
			return false;
		}
		for (int i = 0; i < (int)currentContent.Length; i++)
		{
			if (!this.ObjectIsSameWithInprecision(currentContent[i], lastData[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void ChangeLocalID(int newID)
	{
		if (this.LocalPlayer == null)
		{
			Debug.LogWarning(string.Format("LocalPlayer is null or not in mActors! LocalPlayer: {0} mActors==null: {1} newID: {2}", this.LocalPlayer, this.mActors == null, newID));
		}
		if (this.mActors.ContainsKey(this.LocalPlayer.ID))
		{
			this.mActors.Remove(this.LocalPlayer.ID);
		}
		this.LocalPlayer.InternalChangeLocalID(newID);
		this.mActors[this.LocalPlayer.ID] = this.LocalPlayer;
		this.RebuildPlayerListCopies();
	}

	private void CheckMasterClient(int leavingPlayerId)
	{
		int d;
		bool flag = this.mMasterClientId == leavingPlayerId;
		bool flag1 = leavingPlayerId > 0;
		if (flag1 && !flag)
		{
			return;
		}
		if (this.mActors.Count > 1)
		{
			d = 2147483647;
			foreach (int key in this.mActors.Keys)
			{
				if (key >= d || key == leavingPlayerId)
				{
					continue;
				}
				d = key;
			}
		}
		else
		{
			d = this.LocalPlayer.ID;
		}
		this.mMasterClientId = d;
		if (flag1)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[] { this.GetPlayerWithId(d) });
		}
	}

	private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
	{
		if ((int)methodParameters.Length < (int)callParameterTypes.Length)
		{
			return false;
		}
		for (int i = 0; i < (int)callParameterTypes.Length; i++)
		{
			Type parameterType = methodParameters[i].ParameterType;
			if (callParameterTypes[i] != null && !parameterType.IsAssignableFrom(callParameterTypes[i]) && (!parameterType.IsEnum || !Enum.GetUnderlyingType(parameterType).IsAssignableFrom(callParameterTypes[i])))
			{
				return false;
			}
		}
		return true;
	}

	public void CleanRpcBufferIfMine(PhotonView view)
	{
		if (view.ownerId == this.LocalPlayer.ID || this.LocalPlayer.isMasterClient)
		{
			this.OpCleanRpcBuffer(view);
			return;
		}
		Debug.LogError(string.Concat(new object[] { "Cannot remove cached RPCs on a PhotonView thats not ours! ", view.owner, " scene: ", view.isSceneView }));
	}

	public override bool Connect(string serverAddress, string applicationName)
	{
		Debug.LogError("Avoid using this directly. Thanks.");
		return false;
	}

	public bool Connect(string serverAddress, ServerConnection type)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		if (this.State == ClientState.Disconnecting)
		{
			Debug.LogError(string.Concat("Connect() failed. Can't connect while disconnecting (still). Current state: ", PhotonNetwork.connectionStateDetailed));
			return false;
		}
		bool flag = base.Connect(serverAddress, string.Empty);
		if (flag)
		{
			switch (type)
			{
				case ServerConnection.MasterServer:
				{
					this.State = ClientState.ConnectingToMasterserver;
					break;
				}
				case ServerConnection.GameServer:
				{
					this.State = ClientState.ConnectingToGameserver;
					break;
				}
				case ServerConnection.NameServer:
				{
					this.State = ClientState.ConnectingToNameServer;
					break;
				}
			}
		}
		return flag;
	}

	public bool ConnectToNameServer()
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = CloudRegionCode.none;
		if (this.State == ClientState.ConnectedToNameServer)
		{
			return true;
		}
		if (!base.Connect(this.NameServerAddress, "ns"))
		{
			return false;
		}
		this.State = ClientState.ConnectingToNameServer;
		return true;
	}

	public bool ConnectToRegionMaster(CloudRegionCode region)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = region;
		if (this.State != ClientState.ConnectedToNameServer)
		{
			if (!base.Connect(this.NameServerAddress, "ns"))
			{
				return false;
			}
			this.State = ClientState.ConnectingToNameServer;
			return true;
		}
		AuthenticationValues authValues = this.AuthValues ?? new AuthenticationValues()
		{
			UserId = this.PlayerName
		};
		return this.OpAuthenticate(this.AppId, this.AppVersion, authValues, region.ToString(), this.requestLobbyStatistics);
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		if (level == DebugLevel.ERROR)
		{
			Debug.LogError(message);
		}
		else if (level == DebugLevel.WARNING)
		{
			Debug.LogWarning(message);
		}
		else if (level == DebugLevel.INFO && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(message);
		}
		else if (level == DebugLevel.ALL && PhotonNetwork.logLevel == PhotonLogLevel.Full)
		{
			Debug.Log(message);
		}
	}

	private object[] DeltaCompressionRead(object[] lastOnSerializeDataReceived, object[] incomingData)
	{
		if (!(bool)incomingData[1])
		{
			return incomingData;
		}
		if (lastOnSerializeDataReceived == null)
		{
			return null;
		}
		int[] numArray = incomingData[2] as int[];
		for (int i = 3; i < (int)incomingData.Length; i++)
		{
			if (numArray == null || !numArray.Contains(i))
			{
				if (incomingData[i] == null)
				{
					incomingData[i] = lastOnSerializeDataReceived[i];
				}
			}
		}
		return incomingData;
	}

	private object[] DeltaCompressionWrite(object[] previousContent, object[] currentContent)
	{
		if (currentContent == null || previousContent == null || (int)previousContent.Length != (int)currentContent.Length)
		{
			return currentContent;
		}
		if ((int)currentContent.Length <= 3)
		{
			return null;
		}
		object[] array = previousContent;
		array[1] = false;
		int num = 0;
		Queue<int> nums = null;
		for (int i = 3; i < (int)currentContent.Length; i++)
		{
			object obj = currentContent[i];
			if (!this.ObjectIsSameWithInprecision(obj, previousContent[i]))
			{
				array[i] = obj;
				if (obj == null)
				{
					if (nums == null)
					{
						nums = new Queue<int>((int)currentContent.Length);
					}
					nums.Enqueue(i);
				}
			}
			else
			{
				num++;
				array[i] = null;
			}
		}
		if (num > 0)
		{
			if (num == (int)currentContent.Length - 3)
			{
				return null;
			}
			array[1] = true;
			if (nums != null)
			{
				array[2] = nums.ToArray();
			}
		}
		array[0] = currentContent[0];
		return array;
	}

	public void DestroyAll(bool localOnly)
	{
		if (!localOnly)
		{
			this.OpRemoveCompleteCache();
			this.SendDestroyOfAll();
		}
		this.LocalCleanupAnythingInstantiated(true);
	}

	public void DestroyPlayerObjects(int playerId, bool localOnly)
	{
		if (playerId <= 0)
		{
			Debug.LogError(string.Concat("Failed to Destroy objects of playerId: ", playerId));
			return;
		}
		if (!localOnly)
		{
			this.OpRemoveFromServerInstantiationsOfPlayer(playerId);
			this.OpCleanRpcBuffer(playerId);
			this.SendDestroyOfPlayer(playerId);
		}
		HashSet<GameObject> gameObjects = new HashSet<GameObject>();
		foreach (PhotonView value in this.photonViewList.Values)
		{
			if (value.CreatorActorNr != playerId)
			{
				continue;
			}
			gameObjects.Add(value.gameObject);
		}
		foreach (GameObject gameObject in gameObjects)
		{
			this.RemoveInstantiatedGO(gameObject, true);
		}
		foreach (PhotonView creatorActorNr in this.photonViewList.Values)
		{
			if (creatorActorNr.ownerId != playerId)
			{
				continue;
			}
			creatorActorNr.ownerId = creatorActorNr.CreatorActorNr;
		}
	}

	public override void Disconnect()
	{
		if (base.PeerState != PeerStateValue.Disconnected)
		{
			this.State = ClientState.Disconnecting;
			base.Disconnect();
			return;
		}
		if (!PhotonHandler.AppQuits)
		{
			Debug.LogWarning(string.Format("Can't execute Disconnect() while not connected. Nothing changed. State: {0}", this.State));
		}
	}

	private void DisconnectToReconnect()
	{
		switch (this.Server)
		{
			case ServerConnection.MasterServer:
			{
				this.State = ClientState.DisconnectingFromMasterserver;
				base.Disconnect();
				break;
			}
			case ServerConnection.GameServer:
			{
				this.State = ClientState.DisconnectingFromGameserver;
				base.Disconnect();
				break;
			}
			case ServerConnection.NameServer:
			{
				this.State = ClientState.DisconnectingFromNameServer;
				base.Disconnect();
				break;
			}
		}
	}

	internal GameObject DoInstantiate(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		Vector3 vector3;
		int[] numArray;
		object[] item;
		string str = (string)evData[(byte)0];
		int num = (int)evData[(byte)6];
		int item1 = (int)evData[(byte)7];
		vector3 = (!evData.ContainsKey((byte)1) ? Vector3.zero : (Vector3)evData[(byte)1]);
		Quaternion quaternion = Quaternion.identity;
		if (evData.ContainsKey((byte)2))
		{
			quaternion = (Quaternion)evData[(byte)2];
		}
		int num1 = 0;
		if (evData.ContainsKey((byte)3))
		{
			num1 = (int)evData[(byte)3];
		}
		short item2 = 0;
		if (evData.ContainsKey((byte)8))
		{
			item2 = (short)evData[(byte)8];
		}
		numArray = (!evData.ContainsKey((byte)4) ? new int[] { item1 } : (int[])evData[(byte)4]);
		if (!evData.ContainsKey((byte)5))
		{
			item = null;
		}
		else
		{
			item = (object[])evData[(byte)5];
		}
		if (num1 != 0 && !this.allowedReceivingGroups.Contains(num1))
		{
			return null;
		}
		if (this.ObjectPool != null)
		{
			GameObject gameObject = this.ObjectPool.Instantiate(str, vector3, quaternion);
			PhotonView[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
			if ((int)photonViewsInChildren.Length != (int)numArray.Length)
			{
				throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
			}
			for (int i = 0; i < (int)photonViewsInChildren.Length; i++)
			{
				photonViewsInChildren[i].didAwake = false;
				photonViewsInChildren[i].viewID = 0;
				photonViewsInChildren[i].prefix = item2;
				photonViewsInChildren[i].instantiationId = item1;
				photonViewsInChildren[i].isRuntimeInstantiated = true;
				photonViewsInChildren[i].instantiationDataField = item;
				photonViewsInChildren[i].didAwake = true;
				photonViewsInChildren[i].viewID = numArray[i];
			}
			gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, num, null), SendMessageOptions.DontRequireReceiver);
			return gameObject;
		}
		if (resourceGameObject == null)
		{
			if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(str, out resourceGameObject))
			{
				resourceGameObject = (GameObject)Resources.Load(str, typeof(GameObject));
				if (NetworkingPeer.UsePrefabCache)
				{
					NetworkingPeer.PrefabCache.Add(str, resourceGameObject);
				}
			}
			if (resourceGameObject == null)
			{
				Debug.LogError(string.Concat("PhotonNetwork error: Could not Instantiate the prefab [", str, "]. Please verify you have this gameobject in a Resources folder."));
				return null;
			}
		}
		PhotonView[] photonViewArray = resourceGameObject.GetPhotonViewsInChildren();
		if ((int)photonViewArray.Length != (int)numArray.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int j = 0; j < (int)numArray.Length; j++)
		{
			photonViewArray[j].viewID = numArray[j];
			photonViewArray[j].prefix = item2;
			photonViewArray[j].instantiationId = item1;
			photonViewArray[j].isRuntimeInstantiated = true;
		}
		this.StoreInstantiationData(item1, item);
		GameObject gameObject1 = (GameObject)UnityEngine.Object.Instantiate(resourceGameObject, vector3, quaternion);
		for (int k = 0; k < (int)numArray.Length; k++)
		{
			photonViewArray[k].viewID = 0;
			photonViewArray[k].prefix = -1;
			photonViewArray[k].prefixBackup = -1;
			photonViewArray[k].instantiationId = -1;
			photonViewArray[k].isRuntimeInstantiated = false;
		}
		this.RemoveInstantiationData(item1);
		gameObject1.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, num, null), SendMessageOptions.DontRequireReceiver);
		return gameObject1;
	}

	protected internal void ExecuteRpc(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
	{
		string item;
		if (rpcData == null || !rpcData.ContainsKey((byte)0))
		{
			Debug.LogError(string.Concat("Malformed RPC; this should never occur. Content: ", SupportClass.DictionaryToString(rpcData)));
			return;
		}
		int num = (int)rpcData[(byte)0];
		int item1 = 0;
		if (rpcData.ContainsKey((byte)1))
		{
			item1 = (short)rpcData[(byte)1];
		}
		if (!rpcData.ContainsKey((byte)5))
		{
			item = (string)rpcData[(byte)3];
		}
		else
		{
			int num1 = (byte)rpcData[(byte)5];
			if (num1 > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
			{
				Debug.LogError(string.Concat("Could not find RPC with index: ", num1, ". Going to ignore! Check PhotonServerSettings.RpcList"));
				return;
			}
			item = PhotonNetwork.PhotonServerSettings.RpcList[num1];
		}
		if (Defs.inComingMessagesCounter > 5 && Defs.unimportantRPCList.Contains(item))
		{
			return;
		}
		object[] objArray = null;
		if (rpcData.ContainsKey((byte)4))
		{
			objArray = (object[])rpcData[(byte)4];
		}
		if (objArray == null)
		{
			objArray = new object[0];
		}
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			int mAXVIEWIDS = num / PhotonNetwork.MAX_VIEW_IDS;
			bool d = mAXVIEWIDS == this.LocalPlayer.ID;
			bool flag = mAXVIEWIDS == sender.ID;
			if (!d)
			{
				object[] d1 = new object[] { "Received RPC \"", item, "\" for viewID ", num, " but this PhotonView does not exist! Was remote PV.", null, null, null, null };
				d1[5] = (!flag ? " Remote called." : " Owner called.");
				d1[6] = " By: ";
				d1[7] = sender.ID;
				d1[8] = " Maybe GO was destroyed but RPC not cleaned up.";
				Debug.LogWarning(string.Concat(d1));
			}
			else
			{
				object[] objArray1 = new object[] { "Received RPC \"", item, "\" for viewID ", num, " but this PhotonView does not exist! View was/is ours.", null, null, null };
				objArray1[5] = (!flag ? " Remote called." : " Owner called.");
				objArray1[6] = " By: ";
				objArray1[7] = sender.ID;
				Debug.LogWarning(string.Concat(objArray1));
			}
			return;
		}
		if (photonView.prefix != item1)
		{
			Debug.LogError(string.Concat(new object[] { "Received RPC \"", item, "\" on viewID ", num, " with a prefix of ", item1, ", our prefix is ", photonView.prefix, ". The RPC has been ignored." }));
			return;
		}
		if (string.IsNullOrEmpty(item))
		{
			Debug.LogError(string.Concat("Malformed RPC; this should never occur. Content: ", SupportClass.DictionaryToString(rpcData)));
			return;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log(string.Concat("Received RPC: ", item));
		}
		if (photonView.@group != 0 && !this.allowedReceivingGroups.Contains(photonView.@group))
		{
			return;
		}
		Type[] type = new Type[0];
		if ((int)objArray.Length > 0)
		{
			type = new Type[(int)objArray.Length];
			int num2 = 0;
			for (int i = 0; i < (int)objArray.Length; i++)
			{
				object obj = objArray[i];
				if (obj != null)
				{
					type[num2] = obj.GetType();
				}
				else
				{
					type[num2] = null;
				}
				num2++;
			}
		}
		int num3 = 0;
		int num4 = 0;
		if (!PhotonNetwork.UseRpcMonoBehaviourCache || photonView.RpcMonoBehaviours == null || (int)photonView.RpcMonoBehaviours.Length == 0)
		{
			photonView.RefreshRpcMonoBehaviourCache();
		}
		for (int j = 0; j < (int)photonView.RpcMonoBehaviours.Length; j++)
		{
			MonoBehaviour rpcMonoBehaviours = photonView.RpcMonoBehaviours[j];
			if (rpcMonoBehaviours != null)
			{
				Type type1 = rpcMonoBehaviours.GetType();
				List<MethodInfo> methodInfos = null;
				if (!this.monoRPCMethodsCache.TryGetValue(type1, out methodInfos))
				{
					List<MethodInfo> methods = SupportClass.GetMethods(type1, typeof(PunRPC));
					this.monoRPCMethodsCache[type1] = methods;
					methodInfos = methods;
				}
				if (methodInfos != null)
				{
					for (int k = 0; k < methodInfos.Count; k++)
					{
						MethodInfo methodInfo = methodInfos[k];
						if (methodInfo.Name.Equals(item))
						{
							num4++;
							ParameterInfo[] cachedParemeters = methodInfo.GetCachedParemeters();
							if ((int)cachedParemeters.Length == (int)type.Length)
							{
								if (this.CheckTypeMatch(cachedParemeters, type))
								{
									num3++;
									object obj1 = methodInfo.Invoke(rpcMonoBehaviours, objArray);
									if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
									{
										rpcMonoBehaviours.StartCoroutine((IEnumerator)obj1);
									}
								}
							}
							else if ((int)cachedParemeters.Length - 1 == (int)type.Length)
							{
								if (this.CheckTypeMatch(cachedParemeters, type) && cachedParemeters[(int)cachedParemeters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
								{
									num3++;
									int item2 = (int)rpcData[(byte)2];
									object[] photonMessageInfo = new object[(int)objArray.Length + 1];
									objArray.CopyTo(photonMessageInfo, 0);
									photonMessageInfo[(int)photonMessageInfo.Length - 1] = new PhotonMessageInfo(sender, item2, photonView);
									object obj2 = methodInfo.Invoke(rpcMonoBehaviours, photonMessageInfo);
									if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
									{
										rpcMonoBehaviours.StartCoroutine((IEnumerator)obj2);
									}
								}
							}
							else if ((int)cachedParemeters.Length == 1 && cachedParemeters[0].ParameterType.IsArray)
							{
								num3++;
								object obj3 = methodInfo.Invoke(rpcMonoBehaviours, new object[] { objArray });
								if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
								{
									rpcMonoBehaviours.StartCoroutine((IEnumerator)obj3);
								}
							}
						}
					}
				}
			}
			else
			{
				Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
			}
		}
		if (num3 != 1)
		{
			string empty = string.Empty;
			for (int l = 0; l < (int)type.Length; l++)
			{
				Type type2 = type[l];
				if (empty != string.Empty)
				{
					empty = string.Concat(empty, ", ");
				}
				empty = (type2 != null ? string.Concat(empty, type2.Name) : string.Concat(empty, "null"));
			}
			if (num3 != 0)
			{
				Debug.LogError(string.Concat(new object[] { "PhotonView with ID ", num, " has ", num3, " methods \"", item, "\" that takes ", (int)type.Length, " argument(s): ", empty, ". Should be just one?" }));
			}
			else if (num4 != 0)
			{
				Debug.LogError(string.Concat(new object[] { "PhotonView with ID ", num, " has no method \"", item, "\" that takes ", (int)type.Length, " argument(s): ", empty }));
			}
			else
			{
				Debug.LogError(string.Concat(new object[] { "PhotonView with ID ", num, " has no method \"", item, "\" marked with the [PunRPC](C#) or @PunRPC(JS) property! Args: ", empty }));
			}
		}
	}

	public object[] FetchInstantiationData(int instantiationId)
	{
		object[] objArray = null;
		if (instantiationId == 0)
		{
			return null;
		}
		this.tempInstantiationData.TryGetValue(instantiationId, out objArray);
		return objArray;
	}

	private void GameEnteredOnGameServer(OperationResponse operationResponse)
	{
		if (operationResponse.ReturnCode != 0)
		{
			switch (operationResponse.OperationCode)
			{
				case 225:
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log(string.Concat("Join failed on GameServer. Changing back to MasterServer. Msg: ", operationResponse.DebugMessage));
						if (operationResponse.ReturnCode == 32758)
						{
							Debug.Log("Most likely the game became empty during the switch to GameServer.");
						}
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					break;
				}
				case 226:
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log(string.Concat("Join failed on GameServer. Changing back to MasterServer. Msg: ", operationResponse.DebugMessage));
						if (operationResponse.ReturnCode == 32758)
						{
							Debug.Log("Most likely the game became empty during the switch to GameServer.");
						}
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					break;
				}
				case 227:
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log(string.Concat("Create failed on GameServer. Changing back to MasterServer. Msg: ", operationResponse.DebugMessage));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					break;
				}
			}
			this.DisconnectToReconnect();
			return;
		}
		Room room = new Room(this.enterRoomParamsCache.RoomName, null)
		{
			isLocalClientInside = true
		};
		this.CurrentRoom = room;
		this.State = ClientState.Joined;
		if (operationResponse.Parameters.ContainsKey(252))
		{
			this.UpdatedActorList((int[])operationResponse.Parameters[252]);
		}
		this.ChangeLocalID((int)operationResponse[254]);
		ExitGames.Client.Photon.Hashtable item = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
		this.ReadoutProperties((ExitGames.Client.Photon.Hashtable)operationResponse[248], item, 0);
		if (!this.CurrentRoom.serverSideMasterClient)
		{
			this.CheckMasterClient(-1);
		}
		if (this.mPlayernameHasToBeUpdated)
		{
			this.SendPlayerName();
		}
		switch (operationResponse.OperationCode)
		{
			case 227:
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
				break;
			}
		}
	}

	public int GetInstantiatedObjectsId(GameObject go)
	{
		int num = -1;
		if (go == null)
		{
			Debug.LogError("GetInstantiatedObjectsId() for GO == null.");
			return num;
		}
		PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
		if (photonViewsInChildren != null && (int)photonViewsInChildren.Length > 0 && photonViewsInChildren[0] != null)
		{
			return photonViewsInChildren[0].instantiationId;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Concat("GetInstantiatedObjectsId failed for GO: ", go));
		}
		return num;
	}

	private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
	{
		if (PhotonNetwork.player != null)
		{
			return PhotonNetwork.player.allProperties;
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)255] = this.PlayerName;
		return hashtable;
	}

	protected internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
	{
		mi = null;
		if (monob == null || string.IsNullOrEmpty(methodType))
		{
			return false;
		}
		List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
		for (int i = 0; i < methods.Count; i++)
		{
			MethodInfo item = methods[i];
			if (item.Name.Equals(methodType))
			{
				mi = item;
				return true;
			}
		}
		return false;
	}

	private string GetNameServerAddress()
	{
		ConnectionProtocol usedProtocol = base.UsedProtocol;
		int num = 0;
		NetworkingPeer.ProtocolToNameServerPort.TryGetValue(usedProtocol, out num);
		string empty = string.Empty;
		if (usedProtocol == ConnectionProtocol.WebSocket)
		{
			empty = "ws://";
		}
		else if (usedProtocol == ConnectionProtocol.WebSocketSecure)
		{
			empty = "wss://";
		}
		return string.Format("{0}{1}:{2}", empty, "ns.exitgames.com", num);
	}

	public PhotonView GetPhotonView(int viewID)
	{
		PhotonView photonView = null;
		this.photonViewList.TryGetValue(viewID, out photonView);
		if (photonView == null)
		{
			PhotonView[] photonViewArray = UnityEngine.Object.FindObjectsOfType(typeof(PhotonView)) as PhotonView[];
			for (int i = 0; i < (int)photonViewArray.Length; i++)
			{
				PhotonView photonView1 = photonViewArray[i];
				if (photonView1.viewID == viewID)
				{
					if (photonView1.didAwake)
					{
						Debug.LogWarning(string.Concat("Had to lookup view that wasn't in photonViewList: ", photonView1));
					}
					return photonView1;
				}
			}
		}
		return photonView;
	}

	protected internal PhotonPlayer GetPlayerWithId(int number)
	{
		if (this.mActors == null)
		{
			return null;
		}
		PhotonPlayer photonPlayer = null;
		this.mActors.TryGetValue(number, out photonPlayer);
		return photonPlayer;
	}

	public bool GetRegions()
	{
		if (this.Server != ServerConnection.NameServer)
		{
			return false;
		}
		bool flag = this.OpGetRegions(this.AppId);
		if (flag)
		{
			this.AvailableRegions = null;
		}
		return flag;
	}

	private void HandleEventLeave(int actorID, EventData evLeave)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Concat(new object[] { "HandleEventLeave for player ID: ", actorID, " evLeave: ", evLeave.ToStringFull() }));
		}
		PhotonPlayer playerWithId = this.GetPlayerWithId(actorID);
		if (playerWithId == null)
		{
			Debug.LogError(string.Format("Received event Leave for unknown player ID: {0}", actorID));
			return;
		}
		bool flag = playerWithId.isInactive;
		if (evLeave.Parameters.ContainsKey(233))
		{
			playerWithId.isInactive = (bool)evLeave.Parameters[233];
			if (playerWithId.isInactive)
			{
				Debug.LogWarning(string.Concat(new object[] { "HandleEventLeave for player ID: ", actorID, " isInactive: ", playerWithId.isInactive, ". Stopping handling if inactive." }));
				return;
			}
		}
		if (evLeave.Parameters.ContainsKey(203))
		{
			if ((int)evLeave[203] != 0)
			{
				this.mMasterClientId = (int)evLeave[203];
				this.UpdateMasterClient();
			}
		}
		else if (!this.CurrentRoom.serverSideMasterClient)
		{
			this.CheckMasterClient(actorID);
		}
		if (playerWithId.isInactive && !flag)
		{
			return;
		}
		if (this.CurrentRoom != null && this.CurrentRoom.autoCleanUp)
		{
			this.DestroyPlayerObjects(actorID, true);
		}
		this.RemovePlayer(actorID, playerWithId);
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, new object[] { playerWithId });
	}

	private void LeftLobbyCleanup()
	{
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		if (this.insideLobby)
		{
			this.insideLobby = false;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby, new object[0]);
		}
	}

	private void LeftRoomCleanup()
	{
		bool currentRoom = this.CurrentRoom != null;
		bool flag = (this.CurrentRoom == null ? PhotonNetwork.autoCleanUpPlayerObjects : this.CurrentRoom.autoCleanUp);
		this.hasSwitchedMC = false;
		this.CurrentRoom = null;
		this.mActors = new Dictionary<int, PhotonPlayer>();
		this.mPlayerListCopy = new PhotonPlayer[0];
		this.mOtherPlayerListCopy = new PhotonPlayer[0];
		this.allowedReceivingGroups = new HashSet<int>();
		this.blockSendingGroups = new HashSet<int>();
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		this.isFetchingFriendList = false;
		this.ChangeLocalID(-1);
		if (flag)
		{
			this.LocalCleanupAnythingInstantiated(true);
			PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		}
		if (currentRoom)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
		}
	}

	protected internal void LoadLevelIfSynced()
	{
		if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (!PhotonNetwork.room.customProperties.ContainsKey("curScn"))
		{
			return;
		}
		object item = PhotonNetwork.room.customProperties["curScn"];
		if (item is int)
		{
			if (SceneManagerHelper.ActiveSceneBuildIndex != (int)item)
			{
				PhotonNetwork.LoadLevel((int)item);
			}
		}
		else if (item is string && SceneManagerHelper.ActiveSceneName != (string)item)
		{
			PhotonNetwork.LoadLevel((string)item);
		}
	}

	public bool LocalCleanPhotonView(PhotonView view)
	{
		view.removedFromLocalViewList = true;
		return this.photonViewList.Remove(view.viewID);
	}

	protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
	{
		if (this.tempInstantiationData.Count > 0)
		{
			Debug.LogWarning("It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
		}
		if (destroyInstantiatedGameObjects)
		{
			HashSet<GameObject> gameObjects = new HashSet<GameObject>();
			foreach (PhotonView value in this.photonViewList.Values)
			{
				if (!value.isRuntimeInstantiated)
				{
					continue;
				}
				gameObjects.Add(value.gameObject);
			}
			foreach (GameObject gameObject in gameObjects)
			{
				this.RemoveInstantiatedGO(gameObject, true);
			}
		}
		this.tempInstantiationData.Clear();
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
	}

	private string LogObjectArray(object[] data)
	{
		if (data == null)
		{
			return "null";
		}
		string[] strArrays = new string[(int)data.Length];
		for (int i = 0; i < (int)data.Length; i++)
		{
			object obj = data[i];
			strArrays[i] = (obj == null ? "null" : obj.ToString());
		}
		return string.Join(", ", strArrays);
	}

	public void NewSceneLoaded()
	{
		if (this.loadingLevelAndPausedNetwork)
		{
			this.loadingLevelAndPausedNetwork = false;
			PhotonNetwork.isMessageQueueRunning = true;
		}
		List<int> nums = new List<int>();
		foreach (PhotonView value in this.photonViewList.Values)
		{
			if (value != null)
			{
				continue;
			}
			nums.Add(value.viewID);
		}
		for (int i = 0; i < nums.Count; i++)
		{
			int item = nums[i];
			this.photonViewList.Remove(item);
		}
		if (nums.Count > 0 && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Concat("New level loaded. Removed ", nums.Count, " scene view IDs from last level."));
		}
	}

	private bool ObjectIsSameWithInprecision(object one, object two)
	{
		if (one == null || two == null)
		{
			return (one != null ? false : two == null);
		}
		if (one.Equals(two))
		{
			return true;
		}
		if (one is Vector3)
		{
			if (((Vector3)one).AlmostEquals((Vector3)two, PhotonNetwork.precisionForVectorSynchronization))
			{
				return true;
			}
		}
		else if (one is Vector2)
		{
			if (((Vector2)one).AlmostEquals((Vector2)two, PhotonNetwork.precisionForVectorSynchronization))
			{
				return true;
			}
		}
		else if (one is Quaternion)
		{
			if (((Quaternion)one).AlmostEquals((Quaternion)two, PhotonNetwork.precisionForQuaternionSynchronization))
			{
				return true;
			}
		}
		else if (one is float && ((float)one).AlmostEquals((float)two, PhotonNetwork.precisionForFloatSynchronization))
		{
			return true;
		}
		return false;
	}

	public void OnEvent(EventData photonEvent)
	{
		ExitGames.Client.Photon.Hashtable item;
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Format("OnEvent: {0}", photonEvent.ToString()));
		}
		int num = -1;
		PhotonPlayer playerWithId = null;
		if (photonEvent.Parameters.ContainsKey(254))
		{
			num = (int)photonEvent[254];
			playerWithId = this.GetPlayerWithId(num);
		}
		byte code = photonEvent.Code;
		switch (code)
		{
			case 200:
			{
				this.ExecuteRpc(photonEvent[245] as ExitGames.Client.Photon.Hashtable, playerWithId);
				break;
			}
			case 201:
			case 206:
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
				int item1 = (int)hashtable[(byte)0];
				short num1 = -1;
				short num2 = 10;
				int num3 = 1;
				if (hashtable.ContainsKey((byte)1))
				{
					num1 = (short)hashtable[(byte)1];
					num3 = 2;
				}
				for (short i = num2; i - num2 < hashtable.Count - num3; i = (short)(i + 1))
				{
					this.OnSerializeRead(hashtable[i] as object[], playerWithId, item1, num1);
				}
				break;
			}
			case 202:
			{
				this.DoInstantiate((ExitGames.Client.Photon.Hashtable)photonEvent[245], playerWithId, null);
				break;
			}
			case 203:
			{
				if (playerWithId == null || !playerWithId.isMasterClient)
				{
					Debug.LogError(string.Concat("Error: Someone else(", playerWithId, ") then the masterserver requests a disconnect!"));
				}
				else
				{
					PhotonNetwork.LeaveRoom();
				}
				break;
			}
			case 204:
			{
				item = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
				int item2 = (int)item[(byte)0];
				PhotonView photonView = null;
				if (this.photonViewList.TryGetValue(item2, out photonView))
				{
					this.RemoveInstantiatedGO(photonView.gameObject, true);
				}
				else if (base.DebugOut >= DebugLevel.ERROR)
				{
					Debug.LogError(string.Concat(new object[] { "Ev Destroy Failed. Could not find PhotonView with instantiationId ", item2, ". Sent by actorNr: ", num }));
				}
				break;
			}
			case 207:
			{
				item = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
				int item3 = (int)item[(byte)0];
				if (item3 < 0)
				{
					if (base.DebugOut >= DebugLevel.INFO)
					{
						Debug.Log(string.Concat("Ev DestroyAll! By PlayerId: ", num));
					}
					this.DestroyAll(true);
				}
				else
				{
					this.DestroyPlayerObjects(item3, true);
				}
				break;
			}
			case 208:
			{
				item = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
				int item4 = (int)item[(byte)1];
				this.SetMasterClient(item4, false);
				break;
			}
			case 209:
			{
				int[] numArray = (int[])photonEvent.Parameters[245];
				int num4 = numArray[0];
				int num5 = numArray[1];
				Debug.Log(string.Concat(new object[] { "Ev OwnershipRequest: ", photonEvent.Parameters.ToStringFull(), " ViewID: ", num4, " from: ", num5, " Time: ", Environment.TickCount % 1000 }));
				PhotonView photonView1 = PhotonView.Find(num4);
				if (photonView1 != null)
				{
					Debug.Log(string.Concat(new object[] { "Ev OwnershipRequest PhotonView.ownershipTransfer: ", photonView1.ownershipTransfer, " .ownerId: ", photonView1.ownerId, " isOwnerActive: ", photonView1.isOwnerActive, ". This client's player: ", PhotonNetwork.player.ToStringFull() }));
					switch (photonView1.ownershipTransfer)
					{
						case OwnershipOption.Fixed:
						{
							Debug.LogWarning("Ownership mode == fixed. Ignoring request.");
							break;
						}
						case OwnershipOption.Takeover:
						{
							if (num5 == photonView1.ownerId)
							{
								photonView1.ownerId = num;
							}
							break;
						}
						case OwnershipOption.Request:
						{
							if ((num5 == PhotonNetwork.player.ID || PhotonNetwork.player.isMasterClient) && (photonView1.ownerId == PhotonNetwork.player.ID || PhotonNetwork.player.isMasterClient && !photonView1.isOwnerActive))
							{
								NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipRequest, new object[] { photonView1, playerWithId });
							}
							break;
						}
					}
					break;
				}
				else
				{
					Debug.LogWarning(string.Concat("Can't find PhotonView of incoming OwnershipRequest. ViewId not found: ", num4));
					break;
				}
			}
			case 210:
			{
				int[] numArray1 = (int[])photonEvent.Parameters[245];
				Debug.Log(string.Concat(new object[] { "Ev OwnershipTransfer. ViewID ", numArray1[0], " to: ", numArray1[1], " Time: ", Environment.TickCount % 1000 }));
				int num6 = numArray1[0];
				int num7 = numArray1[1];
				PhotonView photonView2 = PhotonView.Find(num6);
				if (photonView2 != null)
				{
					photonView2.ownerId = num7;
				}
				break;
			}
			default:
			{
				switch (code)
				{
					case 224:
					{
						string[] strArrays = photonEvent[213] as string[];
						byte[] numArray2 = photonEvent[212] as byte[];
						int[] numArray3 = photonEvent[229] as int[];
						int[] numArray4 = photonEvent[228] as int[];
						this.LobbyStatistics.Clear();
						for (int j = 0; j < (int)strArrays.Length; j++)
						{
							TypedLobbyInfo typedLobbyInfo = new TypedLobbyInfo()
							{
								Name = strArrays[j],
								Type = (LobbyType)numArray2[j],
								PlayerCount = numArray3[j],
								RoomCount = numArray4[j]
							};
							this.LobbyStatistics.Add(typedLobbyInfo);
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLobbyStatisticsUpdate, new object[0]);
						break;
					}
					case 226:
					{
						this.PlayersInRoomsCount = (int)photonEvent[229];
						this.PlayersOnMasterCount = (int)photonEvent[227];
						this.RoomsCount = (int)photonEvent[228];
						break;
					}
					case 228:
					{
						break;
					}
					case 229:
					{
						ExitGames.Client.Photon.Hashtable hashtable1 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
						foreach (object key in hashtable1.Keys)
						{
							string str = (string)key;
							RoomInfo roomInfo = new RoomInfo(str, (ExitGames.Client.Photon.Hashtable)hashtable1[key]);
							if (!roomInfo.removedFromList)
							{
								this.mGameList[str] = roomInfo;
							}
							else
							{
								this.mGameList.Remove(str);
							}
						}
						this.mGameListCopy = new RoomInfo[this.mGameList.Count];
						this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
						break;
					}
					case 230:
					{
						this.mGameList = new Dictionary<string, RoomInfo>();
						ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
						foreach (object obj in hashtable2.Keys)
						{
							string roomInfo1 = (string)obj;
							this.mGameList[roomInfo1] = new RoomInfo(roomInfo1, (ExitGames.Client.Photon.Hashtable)hashtable2[obj]);
						}
						this.mGameListCopy = new RoomInfo[this.mGameList.Count];
						this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
						break;
					}
					default:
					{
						switch (code)
						{
							case 251:
							{
								if (PhotonNetwork.OnEventCall == null)
								{
									Debug.LogWarning("Warning: Unhandled Event ErrorInfo (251). Set PhotonNetwork.OnEventCall to the method PUN should call for this event.");
								}
								else
								{
									object obj1 = photonEvent[245];
									PhotonNetwork.OnEventCall(photonEvent.Code, obj1, num);
								}
								break;
							}
							case 252:
							{
								if (photonEvent.Code < 200)
								{
									if (PhotonNetwork.OnEventCall == null)
									{
										Debug.LogWarning(string.Concat("Warning: Unhandled event ", photonEvent, ". Set PhotonNetwork.OnEventCall."));
									}
									else
									{
										object obj2 = photonEvent[245];
										PhotonNetwork.OnEventCall(photonEvent.Code, obj2, num);
									}
								}
								break;
							}
							case 253:
							{
								int item5 = (int)photonEvent[253];
								ExitGames.Client.Photon.Hashtable hashtable3 = null;
								ExitGames.Client.Photon.Hashtable hashtable4 = null;
								if (item5 != 0)
								{
									hashtable4 = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
								}
								else
								{
									hashtable3 = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
								}
								this.ReadoutProperties(hashtable3, hashtable4, item5);
								break;
							}
							case 254:
							{
								this.HandleEventLeave(num, photonEvent);
								break;
							}
							case 255:
							{
								ExitGames.Client.Photon.Hashtable hashtable5 = (ExitGames.Client.Photon.Hashtable)photonEvent[249];
								if (playerWithId != null)
								{
									playerWithId.InternalCacheProperties(hashtable5);
									playerWithId.isInactive = false;
								}
								else
								{
									bool d = this.LocalPlayer.ID == num;
									this.AddNewPlayer(num, new PhotonPlayer(d, num, hashtable5));
									this.ResetPhotonViewsOnSerialize();
								}
								if (num != this.LocalPlayer.ID)
								{
									NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, new object[] { this.mActors[num] });
								}
								else
								{
									this.UpdatedActorList((int[])photonEvent[252]);
									if (this.lastJoinType == JoinType.JoinOrCreateRoom && this.LocalPlayer.ID == 1)
									{
										NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
									}
									NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
								}
								break;
							}
							default:
							{
								goto case 252;
							}
						}
						break;
					}
				}
				break;
			}
		}
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
		if (PhotonNetwork.networkingPeer.State == ClientState.Disconnecting)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(string.Concat("OperationResponse ignored while disconnecting. Code: ", operationResponse.OperationCode));
			}
			return;
		}
		if (operationResponse.ReturnCode == 0)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(operationResponse.ToString());
			}
		}
		else if (operationResponse.ReturnCode == -3)
		{
			Debug.LogError(string.Concat("Operation ", operationResponse.OperationCode, " could not be executed (yet). Wait for state JoinedLobby or ConnectedToMaster and their callbacks before calling operations. WebRPCs need a server-side configuration. Enum OperationCode helps identify the operation."));
		}
		else if (operationResponse.ReturnCode == 32752)
		{
			Debug.LogError(string.Concat(new object[] { "Operation ", operationResponse.OperationCode, " failed in a server-side plugin. Check the configuration in the Dashboard. Message from server-plugin: ", operationResponse.DebugMessage }));
		}
		else if (operationResponse.ReturnCode != 32760)
		{
			Debug.LogError(string.Concat(new object[] { "Operation failed: ", operationResponse.ToStringFull(), " Server: ", this.Server }));
		}
		else
		{
			Debug.LogWarning(string.Concat("Operation failed: ", operationResponse.ToStringFull()));
		}
		if (operationResponse.Parameters.ContainsKey(221))
		{
			if (this.AuthValues == null)
			{
				this.AuthValues = new AuthenticationValues();
			}
			this.AuthValues.Token = operationResponse[221] as string;
			this.tokenCache = this.AuthValues.Token;
		}
		byte operationCode = operationResponse.OperationCode;
		switch (operationCode)
		{
			case 219:
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, new object[] { operationResponse });
				break;
			}
			case 220:
			{
				if (operationResponse.ReturnCode == 32767)
				{
					Debug.LogError(string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.", new object[0]));
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { DisconnectCause.InvalidAuthentication });
					this.State = ClientState.Disconnecting;
					this.Disconnect();
					break;
				}
				else if (operationResponse.ReturnCode == 0)
				{
					string[] item = operationResponse[210] as string[];
					string[] strArrays = operationResponse[230] as string[];
					if (item == null || strArrays == null || (int)item.Length != (int)strArrays.Length)
					{
						Debug.LogError(string.Concat(new object[] { "The region arrays from Name Server are not ok. Must be non-null and same length. ", item == null, " ", strArrays == null, "\n", operationResponse.ToStringFull() }));
						break;
					}
					else
					{
						this.AvailableRegions = new List<Region>((int)item.Length);
						for (int i = 0; i < (int)item.Length; i++)
						{
							string lower = item[i];
							if (!string.IsNullOrEmpty(lower))
							{
								lower = lower.ToLower();
								CloudRegionCode cloudRegionCode = Region.Parse(lower);
								bool enabledRegions = true;
								if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion && (int)PhotonNetwork.PhotonServerSettings.EnabledRegions != 0)
								{
									CloudRegionFlag cloudRegionFlag = Region.ParseFlag(lower);
									enabledRegions = (int)(PhotonNetwork.PhotonServerSettings.EnabledRegions & cloudRegionFlag) != 0;
									if (!enabledRegions && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
									{
										Debug.Log(string.Concat("Skipping region because it's not in PhotonServerSettings.EnabledRegions: ", cloudRegionCode));
									}
								}
								if (enabledRegions)
								{
									List<Region> availableRegions = this.AvailableRegions;
									Region region = new Region()
									{
										Code = cloudRegionCode,
										HostAndPort = strArrays[i]
									};
									availableRegions.Add(region);
								}
							}
						}
						if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
						{
							PhotonHandler.PingAvailableRegionsAndConnectToBest();
						}
						break;
					}
				}
				else
				{
					Debug.LogError(string.Concat(new object[] { "GetRegions failed. Can't provide regions list. Error: ", operationResponse.ReturnCode, ": ", operationResponse.DebugMessage }));
					break;
				}
			}
			case 222:
			{
				bool[] flagArray = operationResponse[1] as bool[];
				string[] item1 = operationResponse[2] as string[];
				if (flagArray == null || item1 == null || this.friendListRequested == null || (int)flagArray.Length != (int)this.friendListRequested.Length)
				{
					Debug.LogError("FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
				}
				else
				{
					List<FriendInfo> friendInfos = new List<FriendInfo>((int)this.friendListRequested.Length);
					for (int j = 0; j < (int)this.friendListRequested.Length; j++)
					{
						FriendInfo friendInfo = new FriendInfo()
						{
							Name = this.friendListRequested[j],
							Room = item1[j],
							IsOnline = flagArray[j]
						};
						friendInfos.Insert(j, friendInfo);
					}
					PhotonNetwork.Friends = friendInfos;
				}
				this.friendListRequested = null;
				this.isFetchingFriendList = false;
				this.friendListTimestamp = Environment.TickCount;
				if (this.friendListTimestamp == 0)
				{
					this.friendListTimestamp = 1;
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList, new object[0]);
				break;
			}
			case 225:
			{
				if (operationResponse.ReturnCode == 0)
				{
					string str = (string)operationResponse[255];
					this.enterRoomParamsCache.RoomName = str;
					this.GameServerAddress = (string)operationResponse[230];
					this.DisconnectToReconnect();
					break;
				}
				else
				{
					if (operationResponse.ReturnCode == 32760)
					{
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
						{
							Debug.Log("JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
						}
					}
					else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogWarning(string.Format("JoinRandom failed: {0}.", operationResponse.ToStringFull()));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					break;
				}
			}
			case 226:
			{
				if (this.Server == ServerConnection.GameServer)
				{
					this.GameEnteredOnGameServer(operationResponse);
				}
				else if (operationResponse.ReturnCode == 0)
				{
					this.GameServerAddress = (string)operationResponse[230];
					this.DisconnectToReconnect();
				}
				else
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log(string.Format("JoinRoom failed (room maybe closed by now). Client stays on masterserver: {0}. State: {1}", operationResponse.ToStringFull(), this.State));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					break;
				}
				break;
			}
			case 227:
			{
				if (this.Server == ServerConnection.GameServer)
				{
					this.GameEnteredOnGameServer(operationResponse);
				}
				else if (operationResponse.ReturnCode == 0)
				{
					string str1 = (string)operationResponse[255];
					if (!string.IsNullOrEmpty(str1))
					{
						this.enterRoomParamsCache.RoomName = str1;
					}
					this.GameServerAddress = (string)operationResponse[230];
					this.DisconnectToReconnect();
				}
				else
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogWarning(string.Format("CreateRoom failed, client stays on masterserver: {0}.", operationResponse.ToStringFull()));
					}
					this.State = (!this.insideLobby ? ClientState.ConnectedToMaster : ClientState.JoinedLobby);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					break;
				}
				break;
			}
			case 228:
			{
				this.State = ClientState.Authenticated;
				this.LeftLobbyCleanup();
				break;
			}
			case 229:
			{
				this.State = ClientState.JoinedLobby;
				this.insideLobby = true;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby, new object[0]);
				break;
			}
			case 230:
			{
				if (operationResponse.ReturnCode == 0)
				{
					if (this.Server == ServerConnection.NameServer || this.Server == ServerConnection.MasterServer)
					{
						if (operationResponse.Parameters.ContainsKey(225))
						{
							string item2 = (string)operationResponse.Parameters[225];
							if (!string.IsNullOrEmpty(item2))
							{
								if (this.AuthValues == null)
								{
									this.AuthValues = new AuthenticationValues();
								}
								this.AuthValues.UserId = item2;
								PhotonNetwork.player.userId = item2;
								if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
								{
									this.DebugReturn(DebugLevel.INFO, string.Format("Received your UserID from server. Updating local value to: {0}", item2));
								}
							}
						}
						if (operationResponse.Parameters.ContainsKey(202))
						{
							this.playername = (string)operationResponse.Parameters[202];
							if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
							{
								this.DebugReturn(DebugLevel.INFO, string.Format("Received your NickName from server. Updating local value to: {0}", this.playername));
							}
						}
					}
					if (this.Server == ServerConnection.NameServer)
					{
						this.MasterServerAddress = operationResponse[230] as string;
						this.DisconnectToReconnect();
					}
					else if (this.Server == ServerConnection.MasterServer)
					{
						if (!PhotonNetwork.autoJoinLobby)
						{
							this.State = ClientState.ConnectedToMaster;
							NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
						}
						else
						{
							this.State = ClientState.Authenticated;
							this.OpJoinLobby(this.lobby);
						}
					}
					else if (this.Server == ServerConnection.GameServer)
					{
						this.State = ClientState.Joining;
						this.enterRoomParamsCache.PlayerProperties = this.GetLocalActorProperties();
						this.enterRoomParamsCache.OnGameServer = true;
						if (this.lastJoinType == JoinType.JoinRoom || this.lastJoinType == JoinType.JoinRandomRoom || this.lastJoinType == JoinType.JoinOrCreateRoom)
						{
							this.OpJoinRoom(this.enterRoomParamsCache);
						}
						else if (this.lastJoinType == JoinType.CreateRoom)
						{
							this.OpCreateGame(this.enterRoomParamsCache);
						}
					}
					if (operationResponse.Parameters.ContainsKey(245))
					{
						Dictionary<string, object> strs = (Dictionary<string, object>)operationResponse.Parameters[245];
						if (strs != null)
						{
							NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationResponse, new object[] { strs });
						}
					}
					break;
				}
				else
				{
					if (operationResponse.ReturnCode == -2)
					{
						Debug.LogError(string.Format(string.Concat("If you host Photon yourself, make sure to start the 'Instance LoadBalancing' ", base.ServerAddress), new object[0]));
					}
					else if (operationResponse.ReturnCode == 32767)
					{
						Debug.LogError(string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.", new object[0]));
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { DisconnectCause.InvalidAuthentication });
					}
					else if (operationResponse.ReturnCode != 32755)
					{
						Debug.LogError(string.Format("Authentication failed: '{0}' Code: {1}", operationResponse.DebugMessage, operationResponse.ReturnCode));
					}
					else
					{
						Debug.LogError(string.Format("Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()", new object[0]));
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, new object[] { operationResponse.DebugMessage });
					}
					this.State = ClientState.Disconnecting;
					this.Disconnect();
					if (operationResponse.ReturnCode == 32757)
					{
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.LogWarning(string.Format("Currently, the limit of users is reached for this title. Try again later. Disconnecting", new object[0]));
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached, new object[0]);
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { DisconnectCause.MaxCcuReached });
					}
					else if (operationResponse.ReturnCode == 32756)
					{
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.LogError(string.Format("The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting.", new object[0]));
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { DisconnectCause.InvalidRegion });
					}
					else if (operationResponse.ReturnCode == 32753)
					{
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.LogError(string.Format("The authentication ticket expired. You need to connect (and authenticate) again. Disconnecting.", new object[0]));
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { DisconnectCause.AuthenticationTicketExpired });
					}
					break;
				}
			}
			default:
			{
				switch (operationCode)
				{
					case 251:
					{
						ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
						this.ReadoutProperties((ExitGames.Client.Photon.Hashtable)operationResponse[248], hashtable, 0);
						break;
					}
					case 252:
					{
						break;
					}
					case 253:
					{
						break;
					}
					case 254:
					{
						this.DisconnectToReconnect();
						break;
					}
					default:
					{
						Debug.LogWarning(string.Format("OperationResponse unhandled: {0}", operationResponse.ToString()));
						break;
					}
				}
				break;
			}
		}
	}

	private void OnSerializeRead(object[] data, PhotonPlayer sender, int networkTime, short correctPrefix)
	{
		int num = (int)data[0];
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			Debug.LogWarning(string.Concat(new object[] { "Received OnSerialization for view ID ", num, ". We have no such PhotonView! Ignored this if you're leaving a room. State: ", this.State }));
			return;
		}
		if (photonView.prefix > 0 && correctPrefix != photonView.prefix)
		{
			Debug.LogError(string.Concat(new object[] { "Received OnSerialization for view ID ", num, " with prefix ", correctPrefix, ". Our prefix is ", photonView.prefix }));
			return;
		}
		if (photonView.@group != 0 && !this.allowedReceivingGroups.Contains(photonView.@group))
		{
			return;
		}
		if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			object[] objArray = this.DeltaCompressionRead(photonView.lastOnSerializeDataReceived, data);
			if (objArray == null)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.Log(string.Concat(new object[] { "Skipping packet for ", photonView.name, " [", photonView.viewID, "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game." }));
				}
				return;
			}
			photonView.lastOnSerializeDataReceived = objArray;
			data = objArray;
		}
		PhotonStream photonStream = new PhotonStream(false, data)
		{
			currentItem = 3
		};
		photonView.DeserializeView(photonStream, new PhotonMessageInfo(sender, networkTime, photonView));
	}

	private object[] OnSerializeWrite(PhotonView view)
	{
		if (view.synchronization == ViewSynchronization.Off)
		{
			return null;
		}
		PhotonMessageInfo photonMessageInfo = new PhotonMessageInfo(this.LocalPlayer, PhotonNetwork.ServerTimestamp, view);
		this.pStream.ResetWriteStream();
		this.pStream.SendNext(view.viewID);
		this.pStream.SendNext(false);
		this.pStream.SendNext(null);
		view.SerializeView(this.pStream, photonMessageInfo);
		if (this.pStream.Count <= 3)
		{
			return null;
		}
		if (view.synchronization == ViewSynchronization.Unreliable)
		{
			return this.pStream.ToArray();
		}
		object[] array = this.pStream.ToArray();
		if (view.synchronization != ViewSynchronization.UnreliableOnChange)
		{
			if (view.synchronization != ViewSynchronization.ReliableDeltaCompressed)
			{
				return null;
			}
			object[] objArray = this.DeltaCompressionWrite(view.lastOnSerializeDataSent, array);
			view.lastOnSerializeDataSent = array;
			return objArray;
		}
		if (!this.AlmostEquals(array, view.lastOnSerializeDataSent))
		{
			view.mixedModeIsReliable = false;
			view.lastOnSerializeDataSent = array;
		}
		else
		{
			if (view.mixedModeIsReliable)
			{
				return null;
			}
			view.mixedModeIsReliable = true;
			view.lastOnSerializeDataSent = array;
		}
		return array;
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
		DisconnectCause disconnectCause;
		AuthenticationValues authenticationValue;
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Format("OnStatusChanged: {0}", statusCode.ToString()));
		}
		switch (statusCode)
		{
			case StatusCode.SecurityExceptionOnConnect:
			case StatusCode.ExceptionOnConnect:
			{
				this.State = ClientState.PeerCreated;
				if (this.AuthValues != null)
				{
					this.AuthValues.Token = null;
				}
				disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				break;
			}
			case StatusCode.Connect:
			{
				if (this.State == ClientState.ConnectingToNameServer)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("Connected to NameServer.");
					}
					this.Server = ServerConnection.NameServer;
					if (this.AuthValues != null)
					{
						this.AuthValues.Token = null;
					}
				}
				if (this.State == ClientState.ConnectingToGameserver)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("Connected to gameserver.");
					}
					this.Server = ServerConnection.GameServer;
					this.State = ClientState.ConnectedToGameserver;
				}
				if (this.State == ClientState.ConnectingToMasterserver)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("Connected to masterserver.");
					}
					this.Server = ServerConnection.MasterServer;
					this.State = ClientState.Authenticating;
					if (this.IsInitialConnect)
					{
						this.IsInitialConnect = false;
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton, new object[0]);
					}
				}
				if (base.IsProtocolSecure)
				{
					Debug.Log("Skipping EstablishEncryption. Protocol is secure.");
				}
				else
				{
					base.EstablishEncryption();
				}
				if (this.IsAuthorizeSecretAvailable || base.IsProtocolSecure)
				{
					AuthenticationValues authValues = this.AuthValues;
					if (authValues == null)
					{
						authenticationValue = new AuthenticationValues()
						{
							UserId = this.PlayerName
						};
						authValues = authenticationValue;
					}
					AuthenticationValues authenticationValue1 = authValues;
					this.didAuthenticate = this.OpAuthenticate(this.AppId, this.AppVersion, authenticationValue1, this.CloudRegion.ToString(), this.requestLobbyStatistics);
					if (this.didAuthenticate)
					{
						this.State = ClientState.Authenticating;
					}
				}
				break;
			}
			case StatusCode.Disconnect:
			{
				this.didAuthenticate = false;
				this.isFetchingFriendList = false;
				if (this.Server == ServerConnection.GameServer)
				{
					this.LeftRoomCleanup();
				}
				if (this.Server == ServerConnection.MasterServer)
				{
					this.LeftLobbyCleanup();
				}
				if (this.State == ClientState.DisconnectingFromMasterserver)
				{
					if (this.Connect(this.GameServerAddress, ServerConnection.GameServer))
					{
						this.State = ClientState.ConnectingToGameserver;
					}
				}
				else if (this.State != ClientState.DisconnectingFromGameserver && this.State != ClientState.DisconnectingFromNameServer)
				{
					if (this.AuthValues != null)
					{
						this.AuthValues.Token = null;
					}
					this.State = ClientState.PeerCreated;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton, new object[0]);
				}
				else if (this.Connect(this.MasterServerAddress, ServerConnection.MasterServer))
				{
					this.State = ClientState.ConnectingToMasterserver;
				}
				break;
			}
			case StatusCode.Exception:
			{
				if (!this.IsInitialConnect)
				{
					this.State = ClientState.PeerCreated;
					disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { disconnectCause });
				}
				else
				{
					Debug.LogError(string.Concat("Exception while connecting to: ", base.ServerAddress, ". Check if the server is available."));
					if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
					{
						Debug.LogWarning("The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
						if (base.ServerAddress == this.GameServerAddress)
						{
							Debug.LogWarning("This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
						}
					}
					this.State = ClientState.PeerCreated;
					disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				}
				this.Disconnect();
				break;
			}
			case StatusCode.QueueOutgoingReliableWarning:
			case StatusCode.QueueOutgoingUnreliableWarning:
			case StatusCode.QueueOutgoingAcksWarning:
			case StatusCode.QueueSentWarning:
			{
				break;
			}
			case 1028:
			case 1032:
			case StatusCode.Connect | StatusCode.Exception:
			case 1036:
			case StatusCode.Connect | StatusCode.Exception | StatusCode.SendError:
			case StatusCode.TcpRouterResponseOk:
			case StatusCode.TcpRouterResponseNodeIdUnknown:
			case StatusCode.TcpRouterResponseEndpointUnknown:
			case StatusCode.TcpRouterResponseNodeNotReady:
			{
				Debug.LogError(string.Concat("Received unknown status code: ", statusCode));
				break;
			}
			case StatusCode.SendError:
			{
				break;
			}
			case StatusCode.QueueIncomingReliableWarning:
			case StatusCode.QueueIncomingUnreliableWarning:
			{
				Debug.Log(string.Concat(statusCode, ". This client buffers many incoming messages. This is OK temporarily. With lots of these warnings, check if you send too much or execute messages too slow. ", (!PhotonNetwork.isMessageQueueRunning ? "Your isMessageQueueRunning is false. This can cause the issue temporarily." : string.Empty)));
				break;
			}
			case StatusCode.ExceptionOnReceive:
			case StatusCode.DisconnectByServer:
			case StatusCode.DisconnectByServerUserLimit:
			case StatusCode.DisconnectByServerLogic:
			{
				if (!this.IsInitialConnect)
				{
					disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { disconnectCause });
				}
				else
				{
					Debug.LogWarning(string.Concat(new object[] { statusCode, " while connecting to: ", base.ServerAddress, ". Check if the server is available." }));
					disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				}
				if (this.AuthValues != null)
				{
					this.AuthValues.Token = null;
				}
				this.Disconnect();
				break;
			}
			case StatusCode.TimeoutDisconnect:
			{
				if (!this.IsInitialConnect)
				{
					disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { disconnectCause });
				}
				else
				{
					Debug.LogWarning(string.Concat(new object[] { statusCode, " while connecting to: ", base.ServerAddress, ". Check if the server is available." }));
					disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				}
				if (this.AuthValues != null)
				{
					this.AuthValues.Token = null;
				}
				if (base.ServerAddress.Equals(this.GameServerAddress))
				{
					this.GameServerAddress = null;
				}
				if (base.ServerAddress.Equals(this.MasterServerAddress))
				{
					base.ServerAddress = null;
				}
				this.Disconnect();
				break;
			}
			case StatusCode.EncryptionEstablished:
			{
				if (this.Server == ServerConnection.NameServer)
				{
					this.State = ClientState.ConnectedToNameServer;
					if (!this.didAuthenticate && this.CloudRegion == CloudRegionCode.none)
					{
						this.OpGetRegions(this.AppId);
					}
				}
				if (!this.didAuthenticate && (!this.IsUsingNameServer || this.CloudRegion != CloudRegionCode.none))
				{
					AuthenticationValues authValues1 = this.AuthValues;
					if (authValues1 == null)
					{
						authenticationValue = new AuthenticationValues()
						{
							UserId = this.PlayerName
						};
						authValues1 = authenticationValue;
					}
					AuthenticationValues authenticationValue2 = authValues1;
					this.didAuthenticate = this.OpAuthenticate(this.AppId, this.AppVersion, authenticationValue2, this.CloudRegion.ToString(), this.requestLobbyStatistics);
					if (this.didAuthenticate)
					{
						this.State = ClientState.Authenticating;
					}
				}
				break;
			}
			case StatusCode.EncryptionFailedToEstablish:
			{
				Debug.LogError(string.Concat("Encryption wasn't established: ", statusCode, ". Going to authenticate anyways."));
				AuthenticationValues authValues2 = this.AuthValues;
				if (authValues2 == null)
				{
					authenticationValue = new AuthenticationValues()
					{
						UserId = this.PlayerName
					};
					authValues2 = authenticationValue;
				}
				AuthenticationValues authenticationValue3 = authValues2;
				this.OpAuthenticate(this.AppId, this.AppVersion, authenticationValue3, this.CloudRegion.ToString(), this.requestLobbyStatistics);
				break;
			}
			default:
			{
				goto case StatusCode.TcpRouterResponseNodeNotReady;
			}
		}
	}

	public void OpCleanRpcBuffer(int actorNumber)
	{
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { actorNumber }
		};
		this.OpRaiseEvent(200, null, true, raiseEventOption);
	}

	public void OpCleanRpcBuffer(PhotonView view)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
	}

	public bool OpCreateGame(EnterRoomParams enterRoomParams)
	{
		bool server = this.Server == ServerConnection.GameServer;
		enterRoomParams.OnGameServer = server;
		enterRoomParams.PlayerProperties = this.GetLocalActorProperties();
		if (!server)
		{
			this.enterRoomParamsCache = enterRoomParams;
		}
		this.lastJoinType = JoinType.CreateRoom;
		return base.OpCreateRoom(enterRoomParams);
	}

	public override bool OpFindFriends(string[] friendsToFind)
	{
		if (this.isFetchingFriendList)
		{
			return false;
		}
		this.friendListRequested = friendsToFind;
		this.isFetchingFriendList = true;
		return base.OpFindFriends(friendsToFind);
	}

	public override bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
	{
		this.enterRoomParamsCache = new EnterRoomParams()
		{
			Lobby = opJoinRandomRoomParams.TypedLobby
		};
		this.lastJoinType = JoinType.JoinRandomRoom;
		return base.OpJoinRandomRoom(opJoinRandomRoomParams);
	}

	public override bool OpJoinRoom(EnterRoomParams opParams)
	{
		bool server = this.Server == ServerConnection.GameServer;
		opParams.OnGameServer = server;
		if (!server)
		{
			this.enterRoomParamsCache = opParams;
		}
		this.lastJoinType = (!opParams.CreateIfNotExists ? JoinType.JoinRoom : JoinType.JoinOrCreateRoom);
		return base.OpJoinRoom(opParams);
	}

	public virtual bool OpLeave()
	{
		if (this.State == ClientState.Joined)
		{
			return this.OpCustom(254, null, true, 0);
		}
		Debug.LogWarning(string.Concat("Not sending leave operation. State is not 'Joined': ", this.State));
		return false;
	}

	public override bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
	}

	public void OpRemoveCompleteCache()
	{
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			Receivers = ReceiverGroup.MasterClient
		};
		this.OpRaiseEvent(0, null, true, raiseEventOption);
	}

	public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
	{
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { actorNumber }
		};
		this.OpRaiseEvent(0, null, true, raiseEventOption);
	}

	private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
	{
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { actorNr }
		};
		this.OpRaiseEvent(202, null, true, raiseEventOption);
	}

	private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
	{
		if (pActorProperties != null && pActorProperties.Count > 0)
		{
			if (targetActorNr <= 0)
			{
				foreach (object key in pActorProperties.Keys)
				{
					int num = (int)key;
					ExitGames.Client.Photon.Hashtable item = (ExitGames.Client.Photon.Hashtable)pActorProperties[key];
					string str = (string)item[(byte)255];
					PhotonPlayer playerWithId = this.GetPlayerWithId(num);
					if (playerWithId == null)
					{
						playerWithId = new PhotonPlayer(false, num, str);
						this.AddNewPlayer(num, playerWithId);
					}
					playerWithId.InternalCacheProperties(item);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[] { playerWithId, item });
				}
			}
			else
			{
				PhotonPlayer photonPlayer = this.GetPlayerWithId(targetActorNr);
				if (photonPlayer != null)
				{
					ExitGames.Client.Photon.Hashtable hashtable = this.ReadoutPropertiesForActorNr(pActorProperties, targetActorNr);
					photonPlayer.InternalCacheProperties(hashtable);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[] { photonPlayer, hashtable });
				}
			}
		}
		if (this.CurrentRoom != null && gameProperties != null)
		{
			this.CurrentRoom.InternalCacheProperties(gameProperties);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[] { gameProperties });
			if (PhotonNetwork.automaticallySyncScene)
			{
				this.LoadLevelIfSynced();
			}
		}
	}

	private ExitGames.Client.Photon.Hashtable ReadoutPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
	{
		if (!actorProperties.ContainsKey(actorNr))
		{
			return actorProperties;
		}
		return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
	}

	private void RebuildPlayerListCopies()
	{
		this.mPlayerListCopy = new PhotonPlayer[this.mActors.Count];
		this.mActors.Values.CopyTo(this.mPlayerListCopy, 0);
		List<PhotonPlayer> photonPlayers = new List<PhotonPlayer>();
		for (int i = 0; i < (int)this.mPlayerListCopy.Length; i++)
		{
			PhotonPlayer photonPlayer = this.mPlayerListCopy[i];
			if (!photonPlayer.isLocal)
			{
				photonPlayers.Add(photonPlayer);
			}
		}
		this.mOtherPlayerListCopy = photonPlayers.ToArray();
	}

	public bool ReconnectAndRejoin()
	{
		if (this.AuthValues == null)
		{
			Debug.LogWarning("ReconnectAndRejoin() with AuthValues == null is not correct!");
			this.AuthValues = new AuthenticationValues();
		}
		this.AuthValues.Token = this.tokenCache;
		if (string.IsNullOrEmpty(this.GameServerAddress) || this.enterRoomParamsCache == null)
		{
			return false;
		}
		this.lastJoinType = JoinType.JoinRoom;
		this.enterRoomParamsCache.RejoinOnly = true;
		return this.Connect(this.GameServerAddress, ServerConnection.GameServer);
	}

	public bool ReconnectToMaster()
	{
		if (this.AuthValues == null)
		{
			Debug.LogWarning("ReconnectToMaster() with AuthValues == null is not correct!");
			this.AuthValues = new AuthenticationValues();
		}
		this.AuthValues.Token = this.tokenCache;
		return this.Connect(this.MasterServerAddress, ServerConnection.MasterServer);
	}

	public void RegisterPhotonView(PhotonView netView)
	{
		if (!Application.isPlaying)
		{
			this.photonViewList = new Dictionary<int, PhotonView>();
			return;
		}
		if (netView.viewID == 0)
		{
			Debug.Log(string.Concat("PhotonView register is ignored, because viewID is 0. No id assigned yet to: ", netView));
			return;
		}
		PhotonView photonView = null;
		if (this.photonViewList.TryGetValue(netView.viewID, out photonView))
		{
			if (netView == photonView)
			{
				return;
			}
			Debug.LogError(string.Format("PhotonView ID duplicate found: {0}. New: {1} old: {2}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.", netView.viewID, netView, photonView));
			this.RemoveInstantiatedGO(photonView.gameObject, true);
		}
		this.photonViewList.Add(netView.viewID, netView);
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log(string.Concat("Registered PhotonView: ", netView.viewID));
		}
	}

	private void RemoveCacheOfLeftPlayers()
	{
		Dictionary<byte, object> nums = new Dictionary<byte, object>();
		nums[244] = (byte)0;
		nums[247] = (byte)7;
		this.OpCustom(253, nums, true, 0);
	}

	protected internal void RemoveInstantiatedGO(GameObject go, bool localOnly)
	{
		if (go == null)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>(true);
		if (componentsInChildren == null || (int)componentsInChildren.Length <= 0)
		{
			Debug.LogError(string.Concat("Failed to 'network-remove' GameObject because has no PhotonView components: ", go));
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int creatorActorNr = photonView.CreatorActorNr;
		int num = photonView.instantiationId;
		if (!localOnly)
		{
			if (!photonView.isMine)
			{
				Debug.LogError(string.Concat("Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: ", photonView));
				return;
			}
			if (num < 1)
			{
				Debug.LogError(string.Concat("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: ", photonView, ". Not Destroying GameObject or PhotonViews!"));
				return;
			}
		}
		if (!localOnly)
		{
			this.ServerCleanInstantiateAndDestroy(num, creatorActorNr, photonView.isRuntimeInstantiated);
		}
		for (int i = (int)componentsInChildren.Length - 1; i >= 0; i--)
		{
			PhotonView photonView1 = componentsInChildren[i];
			if (photonView1 != null)
			{
				if (photonView1.instantiationId >= 1)
				{
					this.LocalCleanPhotonView(photonView1);
				}
				if (!localOnly)
				{
					this.OpCleanRpcBuffer(photonView1);
				}
			}
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log(string.Concat("Network destroy Instantiated GO: ", go.name));
		}
		if (this.ObjectPool == null)
		{
			UnityEngine.Object.Destroy(go);
		}
		else
		{
			PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
			for (int j = 0; j < (int)photonViewsInChildren.Length; j++)
			{
				photonViewsInChildren[j].viewID = 0;
			}
			this.ObjectPool.Destroy(go);
		}
	}

	private void RemoveInstantiationData(int instantiationId)
	{
		this.tempInstantiationData.Remove(instantiationId);
	}

	private void RemovePlayer(int ID, PhotonPlayer player)
	{
		this.mActors.Remove(ID);
		if (!player.isLocal)
		{
			this.RebuildPlayerListCopies();
		}
	}

	public void RemoveRPCsInGroup(int group)
	{
		foreach (PhotonView value in this.photonViewList.Values)
		{
			if (value.@group != group)
			{
				continue;
			}
			this.CleanRpcBufferIfMine(value);
		}
	}

	protected internal void RequestOwnership(int viewID, int fromOwner)
	{
		Debug.Log(string.Concat(new object[] { "RequestOwnership(): ", viewID, " from: ", fromOwner, " Time: ", Environment.TickCount % 1000 }));
		int[] numArray = new int[] { viewID, fromOwner };
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			Receivers = ReceiverGroup.All
		};
		this.OpRaiseEvent(209, numArray, true, raiseEventOption);
	}

	private void ResetPhotonViewsOnSerialize()
	{
		foreach (PhotonView value in this.photonViewList.Values)
		{
			value.lastOnSerializeDataSent = null;
		}
	}

	private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
	{
		if (players == null || (int)players.Length == 0)
		{
			return -1;
		}
		int d = 2147483647;
		for (int i = 0; i < (int)players.Length; i++)
		{
			PhotonPlayer photonPlayer = players[i];
			if (photonPlayer.ID != playerIdToIgnore)
			{
				if (photonPlayer.ID < d)
				{
					d = photonPlayer.ID;
				}
			}
		}
		return d;
	}

	internal void RPC(PhotonView view, string methodName, PhotonTargets target, PhotonPlayer player, bool encrypt, params object[] parameters)
	{
		RaiseEventOptions raiseEventOption;
		if (this.blockSendingGroups.Contains(view.@group))
		{
			return;
		}
		if (view.viewID < 1)
		{
			Debug.LogError(string.Concat(new object[] { "Illegal view ID:", view.viewID, " method: ", methodName, " GO:", view.gameObject.name }));
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log(string.Concat(new object[] { "Sending RPC \"", methodName, "\" to target: ", target, " or player:", player, "." }));
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		if (view.prefix > 0)
		{
			hashtable[(byte)1] = (short)view.prefix;
		}
		hashtable[(byte)2] = PhotonNetwork.ServerTimestamp;
		int num = 0;
		if (!this.rpcShortcuts.TryGetValue(methodName, out num))
		{
			hashtable[(byte)3] = methodName;
		}
		else
		{
			hashtable[(byte)5] = (byte)num;
		}
		if (parameters != null && (int)parameters.Length > 0)
		{
			hashtable[(byte)4] = parameters;
		}
		if (player != null)
		{
			if (this.LocalPlayer.ID != player.ID)
			{
				raiseEventOption = new RaiseEventOptions()
				{
					TargetActors = new int[] { player.ID },
					Encrypt = encrypt
				};
				this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
			}
			else
			{
				this.ExecuteRpc(hashtable, player);
			}
			return;
		}
		if (target == PhotonTargets.All)
		{
			raiseEventOption = new RaiseEventOptions()
			{
				InterestGroup = (byte)view.@group,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
			this.ExecuteRpc(hashtable, this.LocalPlayer);
		}
		else if (target == PhotonTargets.Others)
		{
			raiseEventOption = new RaiseEventOptions()
			{
				InterestGroup = (byte)view.@group,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
		}
		else if (target == PhotonTargets.AllBuffered)
		{
			raiseEventOption = new RaiseEventOptions()
			{
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
			this.ExecuteRpc(hashtable, this.LocalPlayer);
		}
		else if (target == PhotonTargets.OthersBuffered)
		{
			raiseEventOption = new RaiseEventOptions()
			{
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
		}
		else if (target == PhotonTargets.MasterClient)
		{
			if (this.mMasterClientId != this.LocalPlayer.ID)
			{
				raiseEventOption = new RaiseEventOptions()
				{
					Receivers = ReceiverGroup.MasterClient,
					Encrypt = encrypt
				};
				this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
			}
			else
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer);
			}
		}
		else if (target == PhotonTargets.AllViaServer)
		{
			raiseEventOption = new RaiseEventOptions()
			{
				InterestGroup = (byte)view.@group,
				Receivers = ReceiverGroup.All,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
			if (PhotonNetwork.offlineMode)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer);
			}
		}
		else if (target != PhotonTargets.AllBufferedViaServer)
		{
			Debug.LogError(string.Concat("Unsupported target enum: ", target));
		}
		else
		{
			raiseEventOption = new RaiseEventOptions()
			{
				InterestGroup = (byte)view.@group,
				Receivers = ReceiverGroup.All,
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOption);
			if (PhotonNetwork.offlineMode)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer);
			}
		}
	}

	public void RunViewUpdate()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || this.mActors == null)
		{
			return;
		}
		if (this.mActors.Count <= 1)
		{
			return;
		}
		int num = 0;
		foreach (PhotonView value in this.photonViewList.Values)
		{
			if (value.synchronization != ViewSynchronization.Off && value.isMine && value.gameObject.activeInHierarchy)
			{
				if (!this.blockSendingGroups.Contains(value.@group))
				{
					object[] objArray = this.OnSerializeWrite(value);
					if (objArray != null)
					{
						if (value.synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable)
						{
							ExitGames.Client.Photon.Hashtable hashtable = null;
							if (!this.dataPerGroupReliable.TryGetValue(value.@group, out hashtable))
							{
								hashtable = new ExitGames.Client.Photon.Hashtable(10);
								this.dataPerGroupReliable[value.@group] = hashtable;
							}
							hashtable.Add((short)(hashtable.Count + 10), objArray);
							num++;
						}
						else
						{
							ExitGames.Client.Photon.Hashtable hashtable1 = null;
							if (!this.dataPerGroupUnreliable.TryGetValue(value.@group, out hashtable1))
							{
								hashtable1 = new ExitGames.Client.Photon.Hashtable(10);
								this.dataPerGroupUnreliable[value.@group] = hashtable1;
							}
							hashtable1.Add((short)(hashtable1.Count + 10), objArray);
							num++;
						}
					}
				}
			}
		}
		if (num == 0)
		{
			return;
		}
		RaiseEventOptions raiseEventOption = new RaiseEventOptions();
		foreach (int key in this.dataPerGroupReliable.Keys)
		{
			raiseEventOption.InterestGroup = (byte)key;
			ExitGames.Client.Photon.Hashtable item = this.dataPerGroupReliable[key];
			if (item.Count != 0)
			{
				item[(byte)0] = PhotonNetwork.ServerTimestamp;
				if (this.currentLevelPrefix >= 0)
				{
					item[(byte)1] = this.currentLevelPrefix;
				}
				this.OpRaiseEvent(206, item, true, raiseEventOption);
				item.Clear();
			}
		}
		foreach (int key1 in this.dataPerGroupUnreliable.Keys)
		{
			raiseEventOption.InterestGroup = (byte)key1;
			ExitGames.Client.Photon.Hashtable serverTimestamp = this.dataPerGroupUnreliable[key1];
			if (serverTimestamp.Count != 0)
			{
				serverTimestamp[(byte)0] = PhotonNetwork.ServerTimestamp;
				if (this.currentLevelPrefix >= 0)
				{
					serverTimestamp[(byte)1] = this.currentLevelPrefix;
				}
				this.OpRaiseEvent(201, serverTimestamp, false, raiseEventOption);
				serverTimestamp.Clear();
			}
		}
	}

	private void SendDestroyOfAll()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = -1;
		this.OpRaiseEvent(207, hashtable, true, null);
	}

	private void SendDestroyOfPlayer(int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = actorNr;
		this.OpRaiseEvent(207, hashtable, true, null);
	}

	internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, int group, int[] viewIDs, object[] data, bool isGlobalObject)
	{
		int num = viewIDs[0];
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[(byte)1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[(byte)2] = rotation;
		}
		if (group != 0)
		{
			hashtable[(byte)3] = group;
		}
		if ((int)viewIDs.Length > 1)
		{
			hashtable[(byte)4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[(byte)5] = data;
		}
		if (this.currentLevelPrefix > 0)
		{
			hashtable[(byte)8] = this.currentLevelPrefix;
		}
		hashtable[(byte)6] = PhotonNetwork.ServerTimestamp;
		hashtable[(byte)7] = num;
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = (!isGlobalObject ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal)
		};
		this.OpRaiseEvent(202, hashtable, true, raiseEventOption);
		return hashtable;
	}

	public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<GameObject> gameObjects;
		object obj;
		gameObjects = (PhotonNetwork.SendMonoMessageTargets == null ? PhotonNetwork.FindGameObjectsWithComponent(PhotonNetwork.SendMonoMessageTargetType) : PhotonNetwork.SendMonoMessageTargets);
		string str = methodString.ToString();
		if (parameters == null || (int)parameters.Length != 1)
		{
			obj = parameters;
		}
		else
		{
			obj = parameters[0];
		}
		object obj1 = obj;
		foreach (GameObject gameObject in gameObjects)
		{
			if (gameObject == null)
			{
				continue;
			}
			gameObject.SendMessage(str, obj1, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SendPlayerName()
	{
		if (this.State == ClientState.Joining)
		{
			this.mPlayernameHasToBeUpdated = true;
			return;
		}
		if (this.LocalPlayer != null)
		{
			this.LocalPlayer.name = this.PlayerName;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)255] = this.PlayerName;
			if (this.LocalPlayer.ID > 0)
			{
				base.OpSetPropertiesOfActor(this.LocalPlayer.ID, hashtable, null, false);
				this.mPlayernameHasToBeUpdated = false;
			}
		}
	}

	private void SendVacantViewIds()
	{
		Debug.Log("SendVacantViewIds()");
		List<int> nums = new List<int>();
		foreach (PhotonView value in this.photonViewList.Values)
		{
			if (value.isOwnerActive)
			{
				continue;
			}
			nums.Add(value.viewID);
		}
		Debug.Log(string.Concat("Sending vacant view IDs. Length: ", nums.Count));
		this.OpRaiseEvent(211, nums.ToArray(), true, null);
	}

	private void ServerCleanInstantiateAndDestroy(int instantiateId, int creatorId, bool isRuntimeInstantiated)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)7] = instantiateId;
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { creatorId }
		};
		RaiseEventOptions raiseEventOption1 = raiseEventOption;
		this.OpRaiseEvent(202, hashtable, true, raiseEventOption1);
		ExitGames.Client.Photon.Hashtable hashtable1 = new ExitGames.Client.Photon.Hashtable();
		hashtable1[(byte)0] = instantiateId;
		raiseEventOption1 = null;
		if (!isRuntimeInstantiated)
		{
			raiseEventOption1 = new RaiseEventOptions()
			{
				CachingOption = EventCaching.AddToRoomCacheGlobal
			};
			Debug.Log(string.Concat("Destroying GO as global. ID: ", instantiateId));
		}
		this.OpRaiseEvent(204, hashtable1, true, raiseEventOption1);
	}

	public void SetApp(string appId, string gameVersion)
	{
		this.AppId = appId.Trim();
		if (!string.IsNullOrEmpty(gameVersion))
		{
			PhotonNetwork.gameVersion = gameVersion.Trim();
		}
	}

	protected internal void SetLevelInPropsIfSynced(object levelId)
	{
		if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (levelId == null)
		{
			Debug.LogError("Parameter levelId can't be null!");
			return;
		}
		if (PhotonNetwork.room.customProperties.ContainsKey("curScn"))
		{
			object item = PhotonNetwork.room.customProperties["curScn"];
			if (item is int && SceneManagerHelper.ActiveSceneBuildIndex == (int)item)
			{
				return;
			}
			if (item is string && SceneManagerHelper.ActiveSceneName != null && SceneManagerHelper.ActiveSceneName.Equals((string)item))
			{
				return;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (levelId is int)
		{
			hashtable["curScn"] = (int)levelId;
		}
		else if (!(levelId is string))
		{
			Debug.LogError("Parameter levelId must be int or string!");
		}
		else
		{
			hashtable["curScn"] = (string)levelId;
		}
		PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		this.SendOutgoingCommands();
	}

	public void SetLevelPrefix(short prefix)
	{
		this.currentLevelPrefix = prefix;
	}

	protected internal bool SetMasterClient(int playerId, bool sync)
	{
		if (this.mMasterClientId == playerId || !this.mActors.ContainsKey(playerId))
		{
			return false;
		}
		if (sync)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
			{
				{ (byte)1, playerId }
			};
			if (!this.OpRaiseEvent(208, hashtable, true, null))
			{
				return false;
			}
		}
		this.hasSwitchedMC = true;
		this.CurrentRoom.masterClientId = playerId;
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[] { this.GetPlayerWithId(playerId) });
		return true;
	}

	public bool SetMasterClient(int nextMasterId)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
		{
			{ (byte)248, nextMasterId }
		};
		ExitGames.Client.Photon.Hashtable hashtable1 = hashtable;
		hashtable = new ExitGames.Client.Photon.Hashtable()
		{
			{ (byte)248, this.mMasterClientId }
		};
		return base.OpSetPropertiesOfRoom(hashtable1, hashtable, false);
	}

	public void SetReceivingEnabled(int group, bool enabled)
	{
		if (group <= 0)
		{
			Debug.LogError(string.Concat("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: ", group, ". The group number should be at least 1."));
			return;
		}
		if (enabled)
		{
			if (!this.allowedReceivingGroups.Contains(group))
			{
				this.allowedReceivingGroups.Add(group);
				this.OpChangeGroups(null, new byte[] { (byte)group });
			}
		}
		else if (this.allowedReceivingGroups.Contains(group))
		{
			this.allowedReceivingGroups.Remove(group);
			this.OpChangeGroups(new byte[] { (byte)group }, null);
		}
	}

	public void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		byte[] array;
		byte[] numArray;
		List<byte> nums = new List<byte>();
		List<byte> nums1 = new List<byte>();
		if (enableGroups != null)
		{
			for (int i = 0; i < (int)enableGroups.Length; i++)
			{
				int num = enableGroups[i];
				if (num <= 0)
				{
					Debug.LogError(string.Concat("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: ", num, ". The group number should be at least 1."));
				}
				else if (!this.allowedReceivingGroups.Contains(num))
				{
					this.allowedReceivingGroups.Add(num);
					nums.Add((byte)num);
				}
			}
		}
		if (disableGroups != null)
		{
			for (int j = 0; j < (int)disableGroups.Length; j++)
			{
				int num1 = disableGroups[j];
				if (num1 <= 0)
				{
					Debug.LogError(string.Concat("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: ", num1, ". The group number should be at least 1."));
				}
				else if (nums.Contains((byte)num1))
				{
					Debug.LogError(string.Concat("Error: PhotonNetwork.SetReceivingEnabled disableGroups contains a group that is also in the enableGroups: ", num1, "."));
				}
				else if (this.allowedReceivingGroups.Contains(num1))
				{
					this.allowedReceivingGroups.Remove(num1);
					nums1.Add((byte)num1);
				}
			}
		}
		if (nums1.Count <= 0)
		{
			array = null;
		}
		else
		{
			array = nums1.ToArray();
		}
		if (nums.Count <= 0)
		{
			numArray = null;
		}
		else
		{
			numArray = nums.ToArray();
		}
		this.OpChangeGroups(array, numArray);
	}

	public void SetSendingEnabled(int group, bool enabled)
	{
		if (enabled)
		{
			this.blockSendingGroups.Remove(group);
		}
		else
		{
			this.blockSendingGroups.Add(group);
		}
	}

	public void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (enableGroups != null)
		{
			int[] numArray = enableGroups;
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				int num = numArray[i];
				if (this.blockSendingGroups.Contains(num))
				{
					this.blockSendingGroups.Remove(num);
				}
			}
		}
		if (disableGroups != null)
		{
			int[] numArray1 = disableGroups;
			for (int j = 0; j < (int)numArray1.Length; j++)
			{
				int num1 = numArray1[j];
				if (!this.blockSendingGroups.Contains(num1))
				{
					this.blockSendingGroups.Add(num1);
				}
			}
		}
	}

	private void StoreInstantiationData(int instantiationId, object[] instantiationData)
	{
		this.tempInstantiationData[instantiationId] = instantiationData;
	}

	protected internal void TransferOwnership(int viewID, int playerID)
	{
		Debug.Log(string.Concat(new object[] { "TransferOwnership() view ", viewID, " to: ", playerID, " Time: ", Environment.TickCount % 1000 }));
		int[] numArray = new int[] { viewID, playerID };
		RaiseEventOptions raiseEventOption = new RaiseEventOptions()
		{
			Receivers = ReceiverGroup.All
		};
		this.OpRaiseEvent(210, numArray, true, raiseEventOption);
	}

	protected internal void UpdatedActorList(int[] actorsInRoom)
	{
		for (int i = 0; i < (int)actorsInRoom.Length; i++)
		{
			int num = actorsInRoom[i];
			if (this.LocalPlayer.ID != num && !this.mActors.ContainsKey(num))
			{
				this.AddNewPlayer(num, new PhotonPlayer(false, num, string.Empty));
			}
		}
	}

	protected internal void UpdateMasterClient()
	{
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[] { PhotonNetwork.masterClient });
	}

	public bool WebRpc(string uriPath, object parameters)
	{
		Dictionary<byte, object> nums = new Dictionary<byte, object>()
		{
			{ 209, uriPath },
			{ 208, parameters }
		};
		return this.OpCustom(219, nums, true);
	}
}