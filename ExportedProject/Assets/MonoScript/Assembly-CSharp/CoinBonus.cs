using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

internal sealed class CoinBonus : MonoBehaviour
{
	public GameObject player;

	public AudioClip CoinItemUpAudioClip;

	private Player_move_c test;

	public VirtualCurrencyBonusType BonusType
	{
		private get;
		set;
	}

	public CoinBonus()
	{
	}

	public static List<string> GetLevelsWhereGotBonus(VirtualCurrencyBonusType bonusType)
	{
		VirtualCurrencyBonusType virtualCurrencyBonusType = bonusType;
		if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Coin)
		{
			return Storager.getString(Defs.LevelsWhereGetCoinS, false).Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
		}
		if (virtualCurrencyBonusType != VirtualCurrencyBonusType.Gem)
		{
			return new List<string>();
		}
		List<object> objs = Json.Deserialize(Storager.getString(Defs.LevelsWhereGotGems, false)) as List<object>;
		if (objs == null)
		{
			return new List<string>();
		}
		return objs.OfType<string>().ToList<string>();
	}

	public static bool SetLevelsWhereGotBonus(string[] levelsWhereGotBonus, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonus == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonus");
		}
		string empty = string.Empty;
		VirtualCurrencyBonusType virtualCurrencyBonusType = bonusType;
		if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Coin)
		{
			empty = string.Join("#", levelsWhereGotBonus);
		}
		else if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Gem)
		{
			empty = Json.Serialize(levelsWhereGotBonus);
		}
		return CoinBonus.SetLevelsWhereGotBonus(empty, bonusType);
	}

	public static bool SetLevelsWhereGotBonus(List<string> levelsWhereGotBonus, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonus == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonus");
		}
		string empty = string.Empty;
		VirtualCurrencyBonusType virtualCurrencyBonusType = bonusType;
		if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Coin)
		{
			empty = string.Join("#", levelsWhereGotBonus.ToArray());
		}
		else if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Gem)
		{
			empty = Json.Serialize(levelsWhereGotBonus);
		}
		return CoinBonus.SetLevelsWhereGotBonus(empty, bonusType);
	}

	internal static bool SetLevelsWhereGotBonus(string levelsWhereGotBonusSerialized, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonusSerialized == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonusAsString");
		}
		VirtualCurrencyBonusType virtualCurrencyBonusType = bonusType;
		if (virtualCurrencyBonusType == VirtualCurrencyBonusType.Coin)
		{
			Storager.setString(Defs.LevelsWhereGetCoinS, levelsWhereGotBonusSerialized, false);
			return true;
		}
		if (virtualCurrencyBonusType != VirtualCurrencyBonusType.Gem)
		{
			return false;
		}
		Storager.setString(Defs.LevelsWhereGotGems, levelsWhereGotBonusSerialized, false);
		return true;
	}

	public void SetPlayer()
	{
		this.test = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		this.player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (this.test == null || this.player == null)
		{
			return;
		}
		if (this.BonusType == VirtualCurrencyBonusType.None)
		{
			return;
		}
		if (Vector3.SqrMagnitude(base.transform.position - this.player.transform.position) > 2.25f)
		{
			return;
		}
		try
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				int num = (PremiumAccountController.Instance == null ? 1 : PremiumAccountController.Instance.RewardCoeff);
				if (!Defs.IsSurvival && !Defs.isMulti)
				{
					num = 1;
				}
				VirtualCurrencyBonusType bonusType = this.BonusType;
				if (bonusType == VirtualCurrencyBonusType.Coin)
				{
					int num1 = Storager.getInt("Coins", false);
					Storager.setInt("Coins", num1 + 1 * num, false);
					AnalyticsFacade.CurrencyAccrual(1 * num, "Coins", AnalyticsConstants.AccrualType.Earned);
					FlurryEvents.LogCoinsGained(FlurryEvents.GetPlayingMode(), 1);
				}
				else if (bonusType == VirtualCurrencyBonusType.Gem)
				{
					int num2 = Storager.getInt("GemsCurrency", false);
					Storager.setInt("GemsCurrency", num2 + 1 * num, false);
					AnalyticsFacade.CurrencyAccrual(1 * num, "GemsCurrency", AnalyticsConstants.AccrualType.Earned);
					FlurryEvents.LogGemsGained(FlurryEvents.GetPlayingMode(), 1);
				}
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
			}
			CoinsMessage.FireCoinsAddedEvent(this.BonusType == VirtualCurrencyBonusType.Gem, 1);
			if (!this.test.isSurvival && TrainingController.TrainingCompleted)
			{
				List<string> levelsWhereGotBonus = CoinBonus.GetLevelsWhereGotBonus(this.BonusType);
				string activeScene = SceneManager.GetActiveScene().name;
				if (!levelsWhereGotBonus.Contains(activeScene))
				{
					levelsWhereGotBonus.Add(activeScene);
					CoinBonus.SetLevelsWhereGotBonus(levelsWhereGotBonus, this.BonusType);
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				TrainingController.isNextStep = TrainingState.GetTheCoin;
				if (CoinBonus.StartBlinkShop != null)
				{
					CoinBonus.StartBlinkShop();
				}
			}
		}
		finally
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static event Action StartBlinkShop;
}