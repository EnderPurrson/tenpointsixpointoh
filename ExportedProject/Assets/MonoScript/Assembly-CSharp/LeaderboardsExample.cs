using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class LeaderboardsExample : MonoBehaviour
{
	public LeaderboardsView leaderboardsView;

	public MenuLeaderboardsView menuLeaderboardsView;

	public LeaderboardsExample()
	{
	}

	[DebuggerHidden]
	private IEnumerator PopulateLeaderboards()
	{
		LeaderboardsExample.u003cPopulateLeaderboardsu003ec__Iterator15F variable = null;
		return variable;
	}

	private void Start()
	{
		base.StartCoroutine(this.PopulateLeaderboards());
	}
}