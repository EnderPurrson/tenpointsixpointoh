using System;
using System.Collections;
using System.Collections.Generic;

public class AGSScore
{
	public AGSPlayer player;

	public int rank;

	public string scoreString;

	public long scoreValue;

	public AGSScore()
	{
	}

	public static List<AGSScore> fromArrayList(ArrayList list)
	{
		List<AGSScore> aGSScores = new List<AGSScore>();
		IEnumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				aGSScores.Add(AGSScore.fromHashtable((Hashtable)enumerator.Current));
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable == null)
			{
			}
			disposable.Dispose();
		}
		return aGSScores;
	}

	public static AGSScore fromHashtable(Hashtable scoreHashTable)
	{
		AGSScore aGSScore = new AGSScore()
		{
			player = AGSPlayer.fromHashtable(scoreHashTable["player"] as Hashtable),
			rank = int.Parse(scoreHashTable["rank"].ToString()),
			scoreString = scoreHashTable["scoreString"].ToString(),
			scoreValue = long.Parse(scoreHashTable["score"].ToString())
		};
		return aGSScore;
	}

	public override string ToString()
	{
		return string.Format("player: {0}, rank: {1}, scoreValue: {2}, scoreString: {3}", new object[] { this.player.ToString(), this.rank, this.scoreValue, this.scoreString });
	}
}