using System;

public class TypedLobbyInfo : TypedLobby
{
	public int PlayerCount;

	public int RoomCount;

	public TypedLobbyInfo()
	{
	}

	public override string ToString()
	{
		return string.Format("TypedLobbyInfo '{0}'[{1}] rooms: {2} players: {3}", new object[] { this.Name, this.Type, this.RoomCount, this.PlayerCount });
	}
}