using System;

public class AdminSettingsController
{
	public static int minScoreDeathMath;

	public static int[][] coinAvardDeathMath;

	public static int[][] expAvardDeathMath;

	public static int minScoreTeamFight;

	public static int[][] coinAvardTeamFight;

	public static int[][] expAvardTeamFight;

	public static int minScoreFlagCapture;

	public static int[][] coinAvardFlagCapture;

	public static int[][] expAvardFlagCapture;

	public static int minScoreTimeBattle;

	public static int[] coinAvardTimeBattle;

	public static int[] expAvardTimeBattle;

	public static int[] coinAvardDeadlyGames;

	public static int[] expAvardDeadlyGames;

	public static int minScoreCapturePoint;

	public static int[][] coinAvardCapturePoint;

	public static int[][] expAvardCapturePoint;

	static AdminSettingsController()
	{
		AdminSettingsController.minScoreDeathMath = 50;
		AdminSettingsController.coinAvardDeathMath = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 2, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 2, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardDeathMath = new int[][] { new int[] { 10, 8, 5, 3, 2, 1, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 3, 2, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 4, 2, 0, 0, 0, 0 } };
		AdminSettingsController.minScoreTeamFight = 50;
		AdminSettingsController.coinAvardTeamFight = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 3, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardTeamFight = new int[][] { new int[] { 10, 8, 5, 3, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 0, 0, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.minScoreFlagCapture = 50;
		AdminSettingsController.coinAvardFlagCapture = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 3, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardFlagCapture = new int[][] { new int[] { 10, 8, 5, 3, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 0, 0, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.minScoreTimeBattle = 2000;
		AdminSettingsController.coinAvardTimeBattle = new int[] { 3, 2, 1, 1, 1, 1, 1, 1, 1, 1 };
		AdminSettingsController.expAvardTimeBattle = new int[] { 20, 15, 10, 5, 5, 5, 5, 5, 5, 5 };
		AdminSettingsController.coinAvardDeadlyGames = new int[] { 0, 2, 3, 4, 5, 6, 8, 10 };
		AdminSettingsController.expAvardDeadlyGames = new int[] { 0, 10, 10, 11, 12, 13, 14, 15 };
		AdminSettingsController.minScoreCapturePoint = 50;
		AdminSettingsController.coinAvardCapturePoint = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 3, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardCapturePoint = new int[][] { new int[] { 10, 8, 5, 3, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 0, 0, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 0, 0, 0, 0, 0, 0 } };
	}

	public AdminSettingsController()
	{
	}

	public static AdminSettingsController.Avard GetAvardAfterMatch(ConnectSceneNGUIController.RegimGame regim, int timeGame, int place, int score, int countKills, bool isWin)
	{
		bool flag;
		AdminSettingsController.Avard multiplyerRewardWithBoostEvent = new AdminSettingsController.Avard()
		{
			coin = 0,
			expierense = 0
		};
		if (regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			if (score < AdminSettingsController.minScoreDeathMath)
			{
				return multiplyerRewardWithBoostEvent;
			}
			switch (timeGame)
			{
				case 4:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardDeathMath[0][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardDeathMath[0][place];
					break;
				}
				case 5:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardDeathMath[1][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardDeathMath[1][place];
					break;
				}
				case 6:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardDeathMath[0][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardDeathMath[0][place];
					break;
				}
				case 7:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardDeathMath[2][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardDeathMath[2][place];
					break;
				}
				default:
				{
					goto case 6;
				}
			}
			multiplyerRewardWithBoostEvent.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
			multiplyerRewardWithBoostEvent.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
			return multiplyerRewardWithBoostEvent;
		}
		if (regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && !isWin)
			{
				place += 5;
			}
			bool flag1 = ExperienceController.sharedController.currentLevel < 2;
			if (FriendsController.useBuffSystem)
			{
				if (!flag1)
				{
					flag = score < AdminSettingsController.minScoreTeamFight;
				}
				else
				{
					flag = (!BuffSystem.instance.haveFirstInteractons ? true : score < 5);
				}
				if (flag)
				{
					if (flag1 && score >= 5)
					{
						multiplyerRewardWithBoostEvent.expierense = 3;
					}
					return multiplyerRewardWithBoostEvent;
				}
			}
			else if (score < (!flag1 ? AdminSettingsController.minScoreTeamFight : 5))
			{
				return multiplyerRewardWithBoostEvent;
			}
			switch (timeGame)
			{
				case 4:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardTeamFight[0][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardTeamFight[0][(!flag1 ? place : 0)];
					break;
				}
				case 5:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardTeamFight[1][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardTeamFight[1][(!flag1 ? place : 0)];
					break;
				}
				case 6:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardTeamFight[0][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardTeamFight[0][(!flag1 ? place : 0)];
					break;
				}
				case 7:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardTeamFight[2][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardTeamFight[2][(!flag1 ? place : 0)];
					break;
				}
				default:
				{
					goto case 6;
				}
			}
			multiplyerRewardWithBoostEvent.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
			multiplyerRewardWithBoostEvent.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
			return multiplyerRewardWithBoostEvent;
		}
		if (regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && !isWin)
			{
				place += 5;
			}
			if (score < AdminSettingsController.minScoreFlagCapture)
			{
				return multiplyerRewardWithBoostEvent;
			}
			switch (timeGame)
			{
				case 4:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardFlagCapture[0][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardFlagCapture[0][place];
					break;
				}
				case 5:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardFlagCapture[1][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardFlagCapture[1][place];
					break;
				}
				case 6:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardFlagCapture[0][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardFlagCapture[0][place];
					break;
				}
				case 7:
				{
					multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardFlagCapture[2][place];
					multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardFlagCapture[2][place];
					break;
				}
				default:
				{
					goto case 6;
				}
			}
			multiplyerRewardWithBoostEvent.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
			multiplyerRewardWithBoostEvent.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
			return multiplyerRewardWithBoostEvent;
		}
		if (regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			if (score < AdminSettingsController.minScoreTimeBattle)
			{
				return multiplyerRewardWithBoostEvent;
			}
			multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardTimeBattle[place] * PremiumAccountController.Instance.RewardCoeff;
			multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardTimeBattle[place] * PremiumAccountController.Instance.RewardCoeff;
			return multiplyerRewardWithBoostEvent;
		}
		if (regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			if (!isWin || countKills < 0)
			{
				return multiplyerRewardWithBoostEvent;
			}
			multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardDeadlyGames[countKills] * PremiumAccountController.Instance.RewardCoeff;
			multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.coinAvardDeadlyGames[countKills] * PremiumAccountController.Instance.RewardCoeff;
			return multiplyerRewardWithBoostEvent;
		}
		if (regim != ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			return multiplyerRewardWithBoostEvent;
		}
		if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && !isWin)
		{
			place += 5;
		}
		if (score < AdminSettingsController.minScoreCapturePoint)
		{
			return multiplyerRewardWithBoostEvent;
		}
		switch (timeGame)
		{
			case 4:
			{
				multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardCapturePoint[0][place];
				multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardCapturePoint[0][place];
				break;
			}
			case 5:
			{
				multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardCapturePoint[1][place];
				multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardCapturePoint[1][place];
				break;
			}
			case 6:
			{
				multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardCapturePoint[0][place];
				multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardCapturePoint[0][place];
				break;
			}
			case 7:
			{
				multiplyerRewardWithBoostEvent.coin = AdminSettingsController.coinAvardCapturePoint[2][place];
				multiplyerRewardWithBoostEvent.expierense = AdminSettingsController.expAvardCapturePoint[2][place];
				break;
			}
			default:
			{
				goto case 6;
			}
		}
		multiplyerRewardWithBoostEvent.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
		multiplyerRewardWithBoostEvent.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
		return multiplyerRewardWithBoostEvent;
	}

	public static int GetMultiplyerRewardWithBoostEvent(bool isMoney)
	{
		int rewardCoeffByActiveOrActiveBeforeMatch = 1;
		PromoActionsManager promoActionsManager = PromoActionsManager.sharedManager;
		PremiumAccountController instance = PremiumAccountController.Instance;
		int num = (!isMoney ? promoActionsManager.DayOfValorMultiplyerForExp : promoActionsManager.DayOfValorMultiplyerForMoney);
		if (promoActionsManager.IsDayOfValorEventActive && instance.IsActiveOrWasActiveBeforeStartMatch())
		{
			rewardCoeffByActiveOrActiveBeforeMatch = num + instance.GetRewardCoeffByActiveOrActiveBeforeMatch();
		}
		else if (promoActionsManager.IsDayOfValorEventActive)
		{
			rewardCoeffByActiveOrActiveBeforeMatch *= num;
		}
		else if (instance.IsActiveOrWasActiveBeforeStartMatch())
		{
			rewardCoeffByActiveOrActiveBeforeMatch *= instance.GetRewardCoeffByActiveOrActiveBeforeMatch();
		}
		return rewardCoeffByActiveOrActiveBeforeMatch;
	}

	public static void ResetAvardSettingsOnDefault()
	{
		AdminSettingsController.minScoreDeathMath = 50;
		AdminSettingsController.coinAvardDeathMath = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 2, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 2, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardDeathMath = new int[][] { new int[] { 10, 8, 5, 3, 2, 1, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 3, 2, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 4, 2, 0, 0, 0, 0 } };
		AdminSettingsController.minScoreTeamFight = 50;
		AdminSettingsController.coinAvardTeamFight = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 3, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardTeamFight = new int[][] { new int[] { 10, 8, 5, 3, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 0, 0, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.minScoreFlagCapture = 50;
		AdminSettingsController.coinAvardFlagCapture = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 3, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardFlagCapture = new int[][] { new int[] { 10, 8, 5, 3, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 0, 0, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.minScoreTimeBattle = 2000;
		AdminSettingsController.coinAvardTimeBattle = new int[] { 3, 2, 1, 1, 1, 1, 1, 1, 1, 1 };
		AdminSettingsController.expAvardTimeBattle = new int[] { 20, 15, 10, 5, 5, 5, 5, 5, 5, 5 };
		AdminSettingsController.coinAvardDeadlyGames = new int[] { 0, 2, 3, 4, 5, 6, 8, 10 };
		AdminSettingsController.expAvardDeadlyGames = new int[] { 0, 10, 10, 11, 12, 13, 14, 15 };
		AdminSettingsController.minScoreCapturePoint = 50;
		AdminSettingsController.coinAvardCapturePoint = new int[][] { new int[] { 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 3, 2, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 4, 3, 0, 0, 0, 0, 0, 0, 0 } };
		AdminSettingsController.expAvardCapturePoint = new int[][] { new int[] { 10, 8, 5, 3, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 10, 6, 4, 0, 0, 0, 0, 0, 0 }, new int[] { 30, 15, 10, 6, 0, 0, 0, 0, 0, 0 } };
	}

	public struct Avard
	{
		public int coin;

		public int expierense;
	}
}