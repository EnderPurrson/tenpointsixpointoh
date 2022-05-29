using System;
using UnityEngine;

public class BestScoresStat : MonoBehaviour
{
	public BestScoresStat()
	{
	}

	private void Start()
	{
		UILabel component = base.GetComponent<UILabel>();
		int num = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0);
		component.text = num.ToString();
	}
}