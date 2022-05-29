using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCompletedRewardWindowSettings : MonoBehaviour
{
	public List<UILabel> armorNameLabels;

	public UITexture armorTexture;

	public List<UILabel> exp;

	public List<UILabel> gems;

	public List<UILabel> coins;

	public TrainingCompletedRewardWindowSettings()
	{
	}

	private void Awake()
	{
		foreach (UILabel uILabel in this.exp)
		{
			uILabel.text = string.Format(LocalizationStore.Get("Key_1532"), Defs.ExpForTraining);
		}
		foreach (UILabel gem in this.gems)
		{
			gem.text = string.Format(LocalizationStore.Get("Key_1531"), Defs.GemsForTraining);
		}
		foreach (UILabel coin in this.coins)
		{
			coin.text = string.Format(LocalizationStore.Get("Key_1530"), Defs.CoinsForTraining);
		}
		foreach (UILabel armorNameLabel in this.armorNameLabels)
		{
			armorNameLabel.text = LocalizationStore.Get((Storager.getInt("Training.NoviceArmorUsedKey", false) != 1 ? "Key_0724" : "Key_2045"));
		}
		if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1)
		{
			this.armorTexture.mainTexture = Resources.Load<Texture2D>("OfferIcons/Armor_Novice_icon1_big");
		}
	}
}