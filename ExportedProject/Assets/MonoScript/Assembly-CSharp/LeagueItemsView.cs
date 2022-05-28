using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

[RequireComponent(typeof(UIGrid))]
public class LeagueItemsView : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CGetStatesForItem_003Ec__AnonStorey2CD
	{
		internal Dictionary<Wear.LeagueItemState, List<string>> items;

		internal string itemId;

		internal List<Wear.LeagueItemState> res;

		internal void _003C_003Em__396(Wear.LeagueItemState val)
		{
			if (items[val].Contains(itemId))
			{
				res.Add(val);
			}
		}
	}

	[SerializeField]
	private UILabel _headerText;

	private UIGrid _grid;

	private LeagueItemStot[] _slots;

	private void Awake()
	{
		_grid = GetComponent<UIGrid>();
		_slots = GetComponentsInChildren<LeagueItemStot>(true);
	}

	public void Repaint(RatingSystem.RatingLeague league)
	{
		List<string> list = Wear.LeagueItemsByLeagues()[league];
		_headerText.gameObject.SetActive(list.Any());
		int num = 0;
		LeagueItemStot[] slots = _slots;
		foreach (LeagueItemStot leagueItemStot in slots)
		{
			if (list.Count() > num)
			{
				string itemId = list[num];
				List<Wear.LeagueItemState> statesForItem = GetStatesForItem(itemId);
				leagueItemStot.Set(itemId, statesForItem.Contains(Wear.LeagueItemState.Open), statesForItem.Contains(Wear.LeagueItemState.Purchased));
			}
			else
			{
				leagueItemStot.Hide();
			}
			num++;
		}
		_grid.Reposition();
	}

	private List<Wear.LeagueItemState> GetStatesForItem(string itemId)
	{
		_003CGetStatesForItem_003Ec__AnonStorey2CD _003CGetStatesForItem_003Ec__AnonStorey2CD = new _003CGetStatesForItem_003Ec__AnonStorey2CD();
		_003CGetStatesForItem_003Ec__AnonStorey2CD.itemId = itemId;
		_003CGetStatesForItem_003Ec__AnonStorey2CD.res = new List<Wear.LeagueItemState>();
		_003CGetStatesForItem_003Ec__AnonStorey2CD.items = Wear.LeagueItems();
		RiliExtensions.ForEachEnum<Wear.LeagueItemState>(_003CGetStatesForItem_003Ec__AnonStorey2CD._003C_003Em__396);
		return _003CGetStatesForItem_003Ec__AnonStorey2CD.res;
	}
}
