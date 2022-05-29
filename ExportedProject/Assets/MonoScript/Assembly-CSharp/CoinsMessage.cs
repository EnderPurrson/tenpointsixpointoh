using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CoinsMessage : MonoBehaviour
{
	public GUIStyle labelStyle;

	public Rect rect = Tools.SuccessMessageRect();

	public string message = "Purchases restored";

	public Texture texture;

	public int depth = -2;

	public bool singleMessage;

	public Texture youveGotCoin;

	public Texture passNextLevels;

	private int coinsToShow;

	private int coinsForNextLevels;

	private double startTime;

	private float _time = 2f;

	public Texture plashka;

	public CoinsMessage()
	{
	}

	public static void FireCoinsAddedEvent(bool isGems = false, int count = 2)
	{
		if (CoinsMessage.CoinsLabelDisappeared != null)
		{
			CoinsMessage.CoinsLabelDisappeared(isGems, count);
		}
	}

	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.coinsToShow = Storager.getInt(Defs.EarnedCoins, false);
		Storager.setInt(Defs.EarnedCoins, 0, false);
		if (this.coinsToShow <= 1)
		{
			this.plashka = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", "got_coin"));
		}
		else
		{
			this.plashka = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", "got_prize"));
		}
		this.startTime = (double)Time.realtimeSinceStartup;
	}

	public static event CoinsMessage.CoinsLabelDisappearedDelegate CoinsLabelDisappeared;

	public delegate void CoinsLabelDisappearedDelegate(bool isGems, int count);
}