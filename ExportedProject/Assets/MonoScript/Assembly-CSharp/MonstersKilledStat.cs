using System;
using UnityEngine;

public class MonstersKilledStat : MonoBehaviour
{
	public MonstersKilledStat()
	{
	}

	private void Start()
	{
		UILabel component = base.GetComponent<UILabel>();
		int num = PlayerPrefs.GetInt(Defs.KilledZombiesSett, 0);
		component.text = num.ToString();
	}
}