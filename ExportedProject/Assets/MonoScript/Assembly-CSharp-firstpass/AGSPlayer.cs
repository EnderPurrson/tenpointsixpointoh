using System;
using System.Collections;

public class AGSPlayer
{
	private const string aliasKey = "alias";

	private const string playerIdKey = "playerId";

	private const string avatarUrlKey = "avatarUrl";

	public readonly string @alias;

	public readonly string playerId;

	public readonly string avatarUrl;

	private AGSPlayer(string alias, string playerId, string avatarUrl)
	{
		this.@alias = alias;
		this.playerId = playerId;
		this.avatarUrl = avatarUrl;
	}

	public static AGSPlayer BlankPlayerWithID(string playerId)
	{
		return new AGSPlayer(string.Empty, playerId, string.Empty);
	}

	public static AGSPlayer fromHashtable(Hashtable playerDataAsHashtable)
	{
		AGSPlayer aGSPlayer;
		try
		{
			aGSPlayer = new AGSPlayer((!playerDataAsHashtable.ContainsKey("alias") ? string.Empty : playerDataAsHashtable["alias"].ToString()), (!playerDataAsHashtable.ContainsKey("playerId") ? string.Empty : playerDataAsHashtable["playerId"].ToString()), (!playerDataAsHashtable.ContainsKey("avatarUrl") ? string.Empty : playerDataAsHashtable["avatarUrl"].ToString()));
		}
		catch (Exception exception)
		{
			AGSClient.LogGameCircleError(string.Concat("Returning blank player due to exception getting player from hashtable: ", exception.ToString()));
			aGSPlayer = AGSPlayer.GetBlankPlayer();
		}
		return aGSPlayer;
	}

	public static AGSPlayer GetBlankPlayer()
	{
		return new AGSPlayer(string.Empty, string.Empty, string.Empty);
	}

	public override string ToString()
	{
		return string.Format("alias: {0}, playerId: {1}, avatarUrl: {2}", this.@alias, this.playerId, this.avatarUrl);
	}
}