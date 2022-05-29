using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KillRateCheck
{
	public const int DiscountTryGun = 50;

	public const int TryGunPromoDuration = 3600;

	private static KillRateCheck _instance;

	private static float lastConfigCheck;

	private bool activeFromServer;

	private KillRateCheck.ParamByTier[] paramsByTier;

	private int startRounds = 5;

	private int roundsForCheckL1 = 3;

	private int roundsForCheckL2 = 2;

	private int roundsForL2Buff = 3;

	private float starterHighKillrate = 1.2f;

	private int roundsForGunLow = 3;

	private int roundsForGunMiddle = 2;

	private int roundsForGunHigh = 2;

	private float debuffL1 = 0.85f;

	private float debuffL2 = 0.7f;

	private int debuffRoundsL1 = 2;

	private int debuffRoundsL2 = 2;

	private float debuffVal = 1.2f;

	private int debuffRoundsAfterGun = 1;

	private float debuffAfterGun = 0.75f;

	public float timeForDiscount = 3600f;

	public int discountValue = 50;

	private KillRateCheck.CheckStatus status;

	private bool writeKill;

	private int[] kills = new int[30];

	private int[] deaths = new int[30];

	private int roundCount;

	private int allRoundsCount;

	private float killRateVal;

	private int starterBuff;

	private float nextBuffCheck;

	private int killRateLength;

	private int killCount;

	private int deathCount;

	public bool buffEnabled;

	public float damageBuff = 1f;

	public float healthBuff = 1f;

	public bool giveWeapon;

	public bool active;

	private bool calcbuff;

	private bool isFirstRounds;

	private bool configLoaded;

	public static KillRateCheck instance
	{
		get
		{
			if (KillRateCheck._instance == null)
			{
				KillRateCheck._instance = new KillRateCheck();
			}
			if (!KillRateCheck._instance.configLoaded && Time.time > KillRateCheck.lastConfigCheck)
			{
				KillRateCheck.lastConfigCheck = Time.time + 20f;
				KillRateCheck._instance.LoadParameters();
				Debug.LogWarning("KillRateCheck config not loaded: try loading");
			}
			return KillRateCheck._instance;
		}
	}

	private KillRateCheck.ParamByTier tierParam
	{
		get
		{
			return this.paramsByTier[ExpController.Instance.OurTier];
		}
	}

	public KillRateCheck()
	{
		this.LoadParameters();
		string str = Storager.getString("killRateValues", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null)
		{
			if (strs.ContainsKey("roundCount"))
			{
				this.roundCount = Convert.ToInt32(strs["roundCount"]);
			}
			if (strs.ContainsKey("allRoundsCount"))
			{
				this.allRoundsCount = Convert.ToInt32(strs["allRoundsCount"]);
			}
			if (strs.ContainsKey("isFirstRounds"))
			{
				this.isFirstRounds = Convert.ToInt32(strs["isFirstRounds"]) == 1;
			}
			if (strs.ContainsKey("status"))
			{
				this.status = (KillRateCheck.CheckStatus)Convert.ToInt32(strs["status"]);
			}
			if (strs.ContainsKey("killRateVal"))
			{
				this.killRateVal = Convert.ToSingle(strs["killRateVal"]);
			}
			if (strs.ContainsKey("nextBuffCheck"))
			{
				this.nextBuffCheck = Convert.ToSingle(strs["nextBuffCheck"]);
			}
			if (strs.ContainsKey("StarterBuff"))
			{
				this.starterBuff = Convert.ToInt32(strs["StarterBuff"]);
			}
			this.CheckForBuff();
		}
		if (this.status == KillRateCheck.CheckStatus.None)
		{
			if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) == 1)
			{
				this.status = KillRateCheck.CheckStatus.Starter;
			}
			else
			{
				this.status = KillRateCheck.CheckStatus.StarterBuff;
				this.starterBuff = 2;
				this.isFirstRounds = true;
			}
			this.SaveValues();
			this.CheckForBuff();
		}
		string str1 = Storager.getString("LastKillRates", false);
		List<object> objs = Json.Deserialize(str1) as List<object>;
		if (objs != null && objs.Count == 2)
		{
			this.kills = (
				from o in objs[0] as List<object>
				select Convert.ToInt32(o)).ToArray<int>();
			this.deaths = (
				from o in objs[1] as List<object>
				select Convert.ToInt32(o)).ToArray<int>();
		}
	}

	private void CheckForBuff()
	{
		this.damageBuff = 1f;
		this.healthBuff = 1f;
		this.buffEnabled = false;
		if (this.status == KillRateCheck.CheckStatus.StarterBuff || this.status == KillRateCheck.CheckStatus.StarterBuff2 || this.status == KillRateCheck.CheckStatus.HardPlayer)
		{
			int num = this.starterBuff;
			if (num == 1)
			{
				this.buffEnabled = true;
				this.damageBuff = this.tierParam.buffL2;
				this.healthBuff = this.tierParam.buffL2;
			}
			else if (num == 2)
			{
				this.buffEnabled = true;
				this.damageBuff = this.tierParam.buffL1;
				this.healthBuff = this.tierParam.buffL1;
			}
		}
		if (this.active)
		{
			if (this.status == KillRateCheck.CheckStatus.HardDebuff)
			{
				this.buffEnabled = true;
				this.damageBuff = this.debuffL1;
				this.healthBuff = this.debuffL1;
			}
			else if (this.status == KillRateCheck.CheckStatus.HardDebuff2)
			{
				this.buffEnabled = true;
				this.damageBuff = this.debuffL2;
				this.healthBuff = this.debuffL2;
			}
			else if (this.status == KillRateCheck.CheckStatus.DebuffAfterGun)
			{
				this.buffEnabled = true;
				this.damageBuff = this.debuffAfterGun;
				this.healthBuff = this.debuffAfterGun;
			}
			else if (this.killRateVal >= this.tierParam.lowKillRate)
			{
				if (this.killRateVal < this.tierParam.highKillRate && (this.status == KillRateCheck.CheckStatus.GetGun || this.status == KillRateCheck.CheckStatus.BuyedGun && this.roundCount < this.tierParam.buffRoundsL2))
				{
					this.buffEnabled = true;
					this.damageBuff = this.tierParam.buffL2;
					this.healthBuff = this.tierParam.buffL2;
				}
			}
			else if (this.status == KillRateCheck.CheckStatus.GetGun || this.status == KillRateCheck.CheckStatus.BuyedGun && this.roundCount < this.tierParam.buffRoundsL1)
			{
				this.buffEnabled = true;
				this.damageBuff = this.tierParam.buffL1;
				this.healthBuff = this.tierParam.buffL1;
			}
			else if (this.status == KillRateCheck.CheckStatus.BuyedGun && this.roundCount < this.tierParam.buffRoundsL1 + this.tierParam.buffRoundsL2)
			{
				this.buffEnabled = true;
				this.damageBuff = this.tierParam.buffL2;
				this.healthBuff = this.tierParam.buffL2;
			}
		}
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetupBuffParameters(this.damageBuff, this.healthBuff);
		}
	}

	public void CheckKillRate()
	{
		if (this.writeKill)
		{
			this.WriteKillRate();
		}
		float killRate = this.GetKillRate();
		Debug.Log(killRate);
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesKillRate(this.allRoundsCount, killRate);
		}
		this.allRoundsCount++;
		if (this.allRoundsCount == 3)
		{
			AnalyticsStuff.TrySendOnceToAppsFlyer("third_round_complete");
		}
		if (this.allRoundsCount > 9)
		{
			this.isFirstRounds = false;
		}
		switch (this.status)
		{
			case KillRateCheck.CheckStatus.Starter:
			{
				if (!this.calcbuff)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount >= this.startRounds)
				{
					if (killRate >= this.tierParam.lowKillRate)
					{
						this.roundCount = 0;
						this.status = KillRateCheck.CheckStatus.CoolDown;
					}
					else
					{
						this.GiveWeaponByKillRate(killRate);
					}
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.StarterBuff:
			{
				if (!this.calcbuff)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount < this.roundsForCheckL1 || killRate < this.tierParam.highKillRate)
				{
					if (this.roundCount >= this.startRounds)
					{
						if (killRate < this.tierParam.lowKillRate)
						{
							this.GiveWeaponByKillRate(killRate);
						}
						else if (killRate >= this.tierParam.highKillRate || this.roundCount != this.startRounds)
						{
							this.roundCount = 0;
							this.status = KillRateCheck.CheckStatus.CoolDown;
						}
						else
						{
							this.roundCount = 0;
							this.starterBuff = 1;
							this.status = KillRateCheck.CheckStatus.StarterBuff2;
						}
					}
					this.SaveValues();
					this.CheckForBuff();
					return;
				}
				else
				{
					this.roundCount = 0;
					this.starterBuff = 1;
					this.status = KillRateCheck.CheckStatus.HardPlayer;
					this.SaveValues();
					this.CheckForBuff();
					return;
				}
			}
			case KillRateCheck.CheckStatus.WaitGetGun:
			{
				if (!this.active)
				{
					return;
				}
				this.giveWeapon = true;
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.GetGun:
			{
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.BuyedGun:
			{
				if (!this.calcbuff)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount >= this.tierParam.buffRoundsL1 + this.tierParam.buffRoundsL2)
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.CoolDown;
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.CoolDown:
			{
				if (!this.active)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount >= this.GetCooldownForGun(killRate))
				{
					if (killRate <= this.debuffVal)
					{
						this.GiveWeaponByKillRate(killRate);
					}
					else
					{
						this.roundCount = 0;
						this.status = KillRateCheck.CheckStatus.HardDebuff;
					}
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.StarterBuff2:
			{
				if (!this.calcbuff)
				{
					return;
				}
				this.roundCount++;
				if (killRate < this.tierParam.lowKillRate)
				{
					this.GiveWeaponByKillRate(killRate);
				}
				else if (this.roundCount >= this.roundsForL2Buff)
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.CoolDown;
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.HardPlayer:
			{
				if (!this.calcbuff)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount == this.roundsForCheckL2)
				{
					if (killRate < this.tierParam.highKillRate)
					{
						this.roundCount = 0;
						this.status = KillRateCheck.CheckStatus.StarterBuff2;
					}
					else
					{
						this.roundCount = 0;
						this.status = KillRateCheck.CheckStatus.CoolDown;
					}
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.HardDebuff:
			{
				if (!this.active)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount >= this.debuffRoundsL1)
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.HardDebuff2;
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.HardDebuff2:
			{
				if (!this.active)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount >= this.debuffRoundsL2)
				{
					this.roundCount = 0;
					this.GiveWeaponByKillRate(killRate);
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			case KillRateCheck.CheckStatus.DebuffAfterGun:
			{
				if (!this.active)
				{
					return;
				}
				this.roundCount++;
				if (this.roundCount >= this.debuffRoundsAfterGun)
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.CoolDown;
				}
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
			default:
			{
				this.SaveValues();
				this.CheckForBuff();
				return;
			}
		}
	}

	private int GetCooldownForGun(float value)
	{
		if (value < this.tierParam.lowKillRate)
		{
			return this.tierParam.cooldownRoundsLow;
		}
		if (value < this.tierParam.highKillRate)
		{
			return this.tierParam.cooldownRoundsMiddle;
		}
		return this.tierParam.cooldownRoundsHigh;
	}

	public float GetKillRate()
	{
		int num = 0;
		int num1 = 0;
		for (int i = 0; i < Mathf.Min(this.killRateLength, (int)this.kills.Length); i++)
		{
			num += this.kills[i];
		}
		for (int j = 0; j < Mathf.Min(this.killRateLength, (int)this.deaths.Length); j++)
		{
			num1 += this.deaths[j];
		}
		if (num1 == 0)
		{
			return (float)num;
		}
		return (float)num / (float)num1;
	}

	public int GetRoundsForGun()
	{
		if (this.killRateVal < this.tierParam.lowKillRate)
		{
			return this.roundsForGunLow;
		}
		if (this.killRateVal < this.tierParam.highKillRate)
		{
			return this.roundsForGunMiddle;
		}
		return this.roundsForGunHigh;
	}

	private void GiveWeaponByKillRate(float rateValue)
	{
		if (!this.active)
		{
			return;
		}
		this.giveWeapon = true;
		this.roundCount = 0;
		this.status = KillRateCheck.CheckStatus.WaitGetGun;
		this.killRateVal = rateValue;
		this.SaveValues();
	}

	public void IncrementDeath()
	{
		if (!this.active)
		{
			return;
		}
		this.deathCount++;
	}

	public void IncrementKills()
	{
		if (!this.active)
		{
			return;
		}
		this.killCount++;
	}

	private void LoadParameters()
	{
		if (!Storager.hasKey("BuffsParam"))
		{
			this.WriteDefaultParameters();
			return;
		}
		string str = Storager.getString("BuffsParam", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null || !strs.ContainsKey("killRate"))
		{
			this.WriteDefaultParameters();
			return;
		}
		try
		{
			Dictionary<string, object> item = strs["killRate"] as Dictionary<string, object>;
			this.activeFromServer = Convert.ToInt32(item["active"]) == 1;
			this.startRounds = Convert.ToInt32(item["startRounds"]);
			this.roundsForCheckL1 = Convert.ToInt32(item["roundsForCheckL1"]);
			this.roundsForCheckL2 = Convert.ToInt32(item["roundsForCheckL2"]);
			this.roundsForL2Buff = Convert.ToInt32(item["roundsForL2Buff"]);
			this.starterHighKillrate = Convert.ToSingle(item["starterHighKillrate"]);
			this.roundsForGunLow = Convert.ToInt32(item["roundsForGunLow"]);
			this.roundsForGunMiddle = Convert.ToInt32(item["roundsForGunMiddle"]);
			this.roundsForGunHigh = Convert.ToInt32(item["roundsForGunHigh"]);
			this.killRateLength = Convert.ToInt32(item["killRateLength"]);
			this.timeForDiscount = Convert.ToSingle(item["timeForDiscount"]);
			this.discountValue = Convert.ToInt32(item["discountValue"]);
			this.debuffL1 = Convert.ToSingle(item["debuffL1"]);
			this.debuffL2 = Convert.ToSingle(item["debuffL2"]);
			this.debuffRoundsL1 = Convert.ToInt32(item["debuffRoundsL1"]);
			this.debuffRoundsL2 = Convert.ToInt32(item["debuffRoundsL2"]);
			this.debuffVal = Convert.ToSingle(item["killrateForDebuff"]);
			this.debuffRoundsAfterGun = Convert.ToInt32(item["debuffRoundsAfterGun"]);
			this.debuffAfterGun = Convert.ToSingle(item["debuffAfterGun"]);
			List<object> objs = item["tierParams"] as List<object>;
			if (objs == null)
			{
				Debug.LogWarning("Error Deserialize JSON: tierParams");
				return;
			}
			else
			{
				this.paramsByTier = (
					from e in objs
					select new KillRateCheck.ParamByTier(e as Dictionary<string, object>)).ToArray<KillRateCheck.ParamByTier>();
			}
		}
		catch (Exception exception)
		{
			Debug.LogWarning(string.Concat("Error Deserialize JSON: BuffsParam: ", exception));
			this.WriteDefaultParameters();
			return;
		}
		this.configLoaded = true;
	}

	public void LogFirstBattlesResult(bool isWinner)
	{
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesResult(this.allRoundsCount, isWinner);
		}
	}

	public void MakeRemoveGunBuff()
	{
		if (this.status != KillRateCheck.CheckStatus.GetGun)
		{
			return;
		}
		KillRateCheck.instance.WriteStatusAndResetCounter(KillRateCheck.CheckStatus.CoolDown, 0);
	}

	public void OnGetProgress()
	{
		if (this.status == KillRateCheck.CheckStatus.StarterBuff)
		{
			this.status = KillRateCheck.CheckStatus.Starter;
			this.buffEnabled = false;
			this.damageBuff = 1f;
			this.healthBuff = 1f;
		}
		this.isFirstRounds = false;
		this.SaveValues();
	}

	public static void OnGunTakeOff()
	{
		KillRateCheck.instance.WriteStatusAndResetCounter(KillRateCheck.CheckStatus.DebuffAfterGun, -1);
	}

	public static void OnTryGunBuyed()
	{
		KillRateCheck.instance.WriteStatusAndResetCounter(KillRateCheck.CheckStatus.BuyedGun, 0);
	}

	public static void RemoveGunBuff()
	{
		KillRateCheck.instance.MakeRemoveGunBuff();
	}

	private void SaveKillRates()
	{
		Storager.setString("LastKillRates", Json.Serialize(new int[][] { this.kills, this.deaths }), false);
	}

	private void SaveValues()
	{
		Dictionary<string, object> strs = new Dictionary<string, object>();
		if (this.roundCount > 0)
		{
			strs["roundCount"] = this.roundCount;
		}
		if (this.allRoundsCount > 0)
		{
			strs["allRoundsCount"] = this.allRoundsCount;
		}
		if (this.isFirstRounds)
		{
			strs["isFirstRounds"] = 1;
		}
		if (this.killRateVal > 0f)
		{
			strs["killRateVal"] = this.killRateVal;
		}
		if (this.nextBuffCheck > 0f)
		{
			strs["nextBuffCheck"] = this.nextBuffCheck;
		}
		if (this.status == KillRateCheck.CheckStatus.StarterBuff && this.starterBuff > 0)
		{
			strs["StarterBuff"] = this.starterBuff;
		}
		strs["status"] = (int)this.status;
		Storager.setString("killRateValues", Json.Serialize(strs), false);
	}

	public void SetActive(bool isAcitve, bool roundMore30Sec)
	{
		this.active = (!isAcitve || !this.activeFromServer || !roundMore30Sec ? false : this.configLoaded);
		this.writeKill = (!isAcitve ? false : roundMore30Sec);
		this.calcbuff = (!roundMore30Sec ? false : this.configLoaded);
	}

	public void SetGetWeapon()
	{
		this.giveWeapon = false;
		if (!this.active)
		{
			return;
		}
		this.roundCount = 0;
		this.status = KillRateCheck.CheckStatus.GetGun;
		this.SaveValues();
		this.CheckForBuff();
	}

	private void WriteDefaultParameters()
	{
		this.active = false;
		if (this.paramsByTier == null)
		{
			this.paramsByTier = new KillRateCheck.ParamByTier[6];
			for (int i = 0; i < (int)this.paramsByTier.Length; i++)
			{
				this.paramsByTier[i] = new KillRateCheck.ParamByTier();
			}
		}
	}

	private void WriteKillRate()
	{
		for (int i = (int)this.kills.Length - 2; i >= 0; i--)
		{
			this.kills[i + 1] = this.kills[i];
		}
		this.kills[0] = this.killCount;
		for (int j = (int)this.deaths.Length - 2; j >= 0; j--)
		{
			this.deaths[j + 1] = this.deaths[j];
		}
		this.deaths[0] = this.deathCount;
		this.killCount = 0;
		this.deathCount = 0;
		this.SaveKillRates();
	}

	private void WriteStatusAndResetCounter(KillRateCheck.CheckStatus stat, int round)
	{
		this.roundCount = round;
		this.status = stat;
		this.SaveValues();
		this.CheckForBuff();
	}

	private enum CheckStatus
	{
		None,
		Starter,
		StarterBuff,
		WaitGetGun,
		GetGun,
		BuyedGun,
		CoolDown,
		StarterBuff2,
		HardPlayer,
		HardDebuff,
		HardDebuff2,
		DebuffAfterGun
	}

	public class ParamByTier
	{
		public int cooldownRoundsLow;

		public int cooldownRoundsMiddle;

		public int cooldownRoundsHigh;

		public float lowKillRate;

		public float highKillRate;

		public float buffL1;

		public float buffL2;

		public int buffRoundsL1;

		public int buffRoundsL2;

		public ParamByTier()
		{
			this.cooldownRoundsLow = 10;
			this.cooldownRoundsMiddle = 15;
			this.cooldownRoundsHigh = 20;
			this.lowKillRate = 0.5f;
			this.highKillRate = 1.2f;
			this.buffL1 = 1.4f;
			this.buffL2 = 1.2f;
			this.buffRoundsL1 = 2;
			this.buffRoundsL2 = 2;
		}

		public ParamByTier(Dictionary<string, object> dictionary)
		{
			this.cooldownRoundsLow = Convert.ToInt32(dictionary["cooldownRoundsLow"]);
			this.cooldownRoundsMiddle = Convert.ToInt32(dictionary["cooldownRoundsMiddle"]);
			this.cooldownRoundsHigh = Convert.ToInt32(dictionary["cooldownRoundsHigh"]);
			this.lowKillRate = Convert.ToSingle(dictionary["lowKillRate"]);
			this.highKillRate = Convert.ToSingle(dictionary["highKillRate"]);
			this.buffL1 = Convert.ToSingle(dictionary["buffL1"]);
			this.buffL2 = Convert.ToSingle(dictionary["buffL2"]);
			this.buffRoundsL1 = Convert.ToInt32(dictionary["buffRoundsL1"]);
			this.buffRoundsL2 = Convert.ToInt32(dictionary["buffRoundsL2"]);
		}
	}
}