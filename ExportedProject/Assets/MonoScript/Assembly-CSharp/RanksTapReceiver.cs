using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RanksTapReceiver : MonoBehaviour
{
	public RanksTapReceiver()
	{
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if (RanksTapReceiver.RanksClicked != null)
		{
			RanksTapReceiver.RanksClicked();
		}
	}

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
	}

	public static event Action RanksClicked;
}