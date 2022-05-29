using System;
using UnityEngine;

public class RanksSetPlace : MonoBehaviour
{
	public UILabel placeLabel;

	public GameObject cupGold;

	public GameObject cupSilver;

	public GameObject cupBronze;

	public bool isShowCups;

	public RanksSetPlace()
	{
	}

	public void SetPlace(int place)
	{
		if (place > 3 || !this.isShowCups)
		{
			this.cupGold.SetActive(false);
			this.cupSilver.SetActive(false);
			this.cupBronze.SetActive(false);
			this.placeLabel.text = place.ToString();
		}
		else
		{
			this.placeLabel.text = string.Empty;
			this.cupGold.SetActive(place == 1);
			this.cupSilver.SetActive(place == 2);
			this.cupBronze.SetActive(place == 3);
		}
	}
}