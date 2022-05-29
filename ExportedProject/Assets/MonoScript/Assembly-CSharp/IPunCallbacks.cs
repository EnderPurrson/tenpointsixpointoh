using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;

public interface IPunCallbacks
{
	void OnConnectedToMaster();

	void OnConnectedToPhoton();

	void OnConnectionFail(DisconnectCause cause);

	void OnCreatedRoom();

	void OnCustomAuthenticationFailed(string debugMessage);

	void OnCustomAuthenticationResponse(Dictionary<string, object> data);

	void OnDisconnectedFromPhoton();

	void OnFailedToConnectToPhoton(DisconnectCause cause);

	void OnJoinedLobby();

	void OnJoinedRoom();

	void OnLeftLobby();

	void OnLeftRoom();

	void OnLobbyStatisticsUpdate();

	void OnMasterClientSwitched(PhotonPlayer newMasterClient);

	void OnOwnershipRequest(object[] viewAndPlayer);

	void OnPhotonCreateRoomFailed(object[] codeAndMsg);

	void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged);

	void OnPhotonInstantiate(PhotonMessageInfo info);

	void OnPhotonJoinRoomFailed(object[] codeAndMsg);

	void OnPhotonMaxCccuReached();

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer);

	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer);

	void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps);

	void OnPhotonRandomJoinFailed(object[] codeAndMsg);

	void OnReceivedRoomListUpdate();

	void OnUpdatedFriendList();

	void OnWebRpcResponse(OperationResponse response);
}