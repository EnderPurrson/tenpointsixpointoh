using System;

public interface IPunTurnManagerCallbacks
{
	void OnPlayerFinished(PhotonPlayer player, int turn, object move);

	void OnPlayerMove(PhotonPlayer player, int turn, object move);

	void OnTurnBegins(int turn);

	void OnTurnCompleted(int turn);

	void OnTurnTimeEnds(int turn);
}