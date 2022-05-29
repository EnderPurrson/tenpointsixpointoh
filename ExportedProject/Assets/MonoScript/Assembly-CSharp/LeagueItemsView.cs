using Rilisoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UIGrid))]
public class LeagueItemsView : MonoBehaviour
{
	[SerializeField]
	private UILabel _headerText;

	private UIGrid _grid;

	private LeagueItemStot[] _slots;

	public LeagueItemsView()
	{
	}

	private void Awake()
	{
		this._grid = base.GetComponent<UIGrid>();
		this._slots = base.GetComponentsInChildren<LeagueItemStot>(true);
	}

	private List<Wear.LeagueItemState> GetStatesForItem(string itemId)
	{
		List<Wear.LeagueItemState> leagueItemStates = new List<Wear.LeagueItemState>();
		Dictionary<Wear.LeagueItemState, List<string>> leagueItemStates1 = Wear.LeagueItems();
		RiliExtensions.ForEachEnum<Wear.LeagueItemState>((Wear.LeagueItemState val) => {
			if (leagueItemStates1[val].Contains(itemId))
			{
				leagueItemStates.Add(val);
			}
		});
		return leagueItemStates;
	}

	public void Repaint(RatingSystem.RatingLeague league)
	{
		List<string> item = Wear.LeagueItemsByLeagues()[league];
		this._headerText.gameObject.SetActive(item.Any<string>());
		int num = 0;
		LeagueItemStot[] leagueItemStotArray = this._slots;
		for (int i = 0; i < (int)leagueItemStotArray.Length; i++)
		{
			LeagueItemStot leagueItemStot = leagueItemStotArray[i];
			if (item.Count<string>() <= num)
			{
				leagueItemStot.Hide();
			}
			else
			{
				string str = item[num];
				List<Wear.LeagueItemState> statesForItem = this.GetStatesForItem(str);
				leagueItemStot.Set(str, statesForItem.Contains(Wear.LeagueItemState.Open), statesForItem.Contains(Wear.LeagueItemState.Purchased));
			}
			num++;
		}
		this._grid.Reposition();
	}
}