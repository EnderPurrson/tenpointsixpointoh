using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	public const int DiscountTryGun = 50;

	public const int TryGunPromoDuration = 3600;

	private static BuffSystem _instance;

	private bool[] interactionBuffs = new bool[] { typeof(u003cPrivateImplementationDetailsu003e).GetField("$$field-32").FieldHandle };

	private BuffSystem.ParamByTier[] paramsByTier;

	private Dictionary<BuffSystem.SituationBuffType, BuffSystem.BuffParameter> buffParamByType;

	private List<BuffSystem.SituationBuff> situationBuffs = new List<BuffSystem.SituationBuff>();

	private BuffSystem.SituationBuff currentBuff;

	private BuffSystem.SituationBuff weaponBuff;

	private bool configLoaded;

	private bool loadValuesCalled;

	private BuffSystem.CheckStatus status;

	private BuffSystem.InteractionType[] interactions = new BuffSystem.InteractionType[30];

	private bool interactionsChanged;

	private bool buffsActive;

	private int interactionCounter;

	private BuffSystem.BuffParameter waitingForPurchaseBuff;

	private float waitingForPurchaseTime;

	private float lastGiveGunTime;

	private bool readyToGiveGun;

	public bool giveTryGun;

	public float timeForDiscount = 3600f;

	public int discountValue = 50;

	private int roundsForGunLow = 3;

	private int roundsForGunMiddle = 2;

	private int roundsForGunHigh = 2;

	private float debuffKillrateForGun = 0.8f;

	private float firstBuffArmor = 8f;

	private float firstBuffNoArmor = 2f;

	private int interactionCountForOldPlayer = 10;

	private bool isFirstRounds;

	private int allRoundsCount;

	private float damageBuff = 1f;

	private float healthBuff = 1f;

	private float killRateCached = -1f;

	private readonly BuffSystem.SituationBuffType[] gemsBuffByIndex = new BuffSystem.SituationBuffType[] { BuffSystem.SituationBuffType.Gem1, BuffSystem.SituationBuffType.Gem2, BuffSystem.SituationBuffType.Gem3, BuffSystem.SituationBuffType.Gem4, BuffSystem.SituationBuffType.Gem5, BuffSystem.SituationBuffType.Gem6, BuffSystem.SituationBuffType.Gem7 };

	private readonly BuffSystem.SituationBuffType[] coinsBuffByIndex = new BuffSystem.SituationBuffType[] { BuffSystem.SituationBuffType.Coin1, BuffSystem.SituationBuffType.Coin7, BuffSystem.SituationBuffType.Coin2, BuffSystem.SituationBuffType.Coin3, BuffSystem.SituationBuffType.Coin4, BuffSystem.SituationBuffType.Coin5, BuffSystem.SituationBuffType.Coin8 };

	public bool haveAllInteractons
	{
		get
		{
			return this.status != BuffSystem.CheckStatus.NewPlayer;
		}
	}

	public bool haveFirstInteractons
	{
		get
		{
			return this.interactionCounter >= 4;
		}
	}

	public static BuffSystem instance
	{
		get
		{
			if (BuffSystem._instance == null)
			{
				GameObject gameObject = new GameObject("BuffSystem");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				BuffSystem._instance = gameObject.AddComponent<BuffSystem>();
			}
			return BuffSystem._instance;
		}
	}

	private BuffSystem.ParamByTier tierParam
	{
		get
		{
			return this.paramsByTier[ExpController.Instance.OurTier];
		}
	}

	public float weaponBuffValue
	{
		get
		{
			float single;
			object obj;
			if (this.weaponBuff == null)
			{
				single = 1f;
			}
			else
			{
				float single1 = this.weaponBuff.param.damageBuff;
				if (this.GetKillrateByInteractions() >= 0.8f)
				{
					obj = null;
				}
				else
				{
					obj = 1;
				}
				single = Mathf.Clamp(single1, (float)obj, (float)((this.GetKillrateByInteractions() <= 2f ? 2 : 1)));
			}
			return single;
		}
	}

	public BuffSystem()
	{
	}

	private void AddSituationBuff(BuffSystem.SituationBuffType type, string buffForWeapon = "")
	{
		this.situationBuffs.Add(new BuffSystem.SituationBuff(this.buffParamByType[type], buffForWeapon));
		this.SaveValues();
		this.CheckForPlayerBuff();
	}

	private void Awake()
	{
		this.TryLoadConfig();
		this.LoadValues();
		this.CheckForPlayerBuff();
		ShopNGUIController.GunOrArmorBought += new Action(this.OnGunBuyed);
	}

	public void BuffsActive(bool value)
	{
		this.buffsActive = value;
		this.CheckExpiredBuffs();
	}

	private void CheckAndWriteInteraction(BuffSystem.InteractionType value)
	{
		if (!this.buffsActive)
		{
			return;
		}
		switch (this.status)
		{
			case BuffSystem.CheckStatus.NewPlayer:
			{
				if (this.interactionCounter < (int)this.interactionBuffs.Length && !this.interactionBuffs[this.interactionCounter])
				{
					this.WriteInteraction(value);
				}
				this.interactionCounter++;
				if (this.interactionCounter >= (int)this.interactionBuffs.Length)
				{
					this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
					this.status = BuffSystem.CheckStatus.Regular;
				}
				break;
			}
			case BuffSystem.CheckStatus.OldPlayer:
			{
				this.WriteInteraction(value);
				this.interactionCounter++;
				if (this.interactionCounter >= this.interactionCountForOldPlayer)
				{
					this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
					this.status = BuffSystem.CheckStatus.Regular;
				}
				break;
			}
			case BuffSystem.CheckStatus.Regular:
			{
				this.WriteInteraction(value);
				this.interactionCounter++;
				break;
			}
		}
		this.CheckForPlayerBuff();
	}

	private void CheckExpiredBuffs()
	{
		if (this.buffsActive)
		{
			for (int i = 0; i < this.situationBuffs.Count; i++)
			{
				if (this.situationBuffs[i].expired && this.situationBuffs[i].param.type != BuffSystem.SituationBuffType.TryGunBuff)
				{
					if (this.situationBuffs[i].param.type == BuffSystem.SituationBuffType.DebuffBeforeGun)
					{
						this.GiveTryGunToPlayer();
					}
					this.situationBuffs.RemoveAt(i);
					i--;
				}
			}
		}
	}

	public void CheckForPlayerBuff()
	{
		object obj;
		object obj1;
		this.damageBuff = 1f;
		this.healthBuff = 1f;
		if (this.buffsActive)
		{
			float killrateByInteractions = this.GetKillrateByInteractions();
			this.currentBuff = null;
			this.weaponBuff = null;
			for (int i = 0; i < this.situationBuffs.Count; i++)
			{
				if (!string.IsNullOrEmpty(this.situationBuffs[i].weapon))
				{
					if (this.weaponBuff == null || this.weaponBuff.param.priority < this.situationBuffs[i].param.priority)
					{
						this.weaponBuff = this.situationBuffs[i];
					}
					if (this.weaponBuff != null)
					{
						Debug.Log(string.Format("<color=green>Weapon buff active: {0}</color>", this.weaponBuffValue));
					}
				}
				else
				{
					if (this.currentBuff == null || this.currentBuff.param.priority < this.situationBuffs[i].param.priority)
					{
						this.currentBuff = this.situationBuffs[i];
					}
					if (this.currentBuff != null)
					{
						Debug.Log(string.Format("<color=green>Buff active: {0}</color>", this.currentBuff.param.type.ToString()));
					}
				}
			}
			switch (this.status)
			{
				case BuffSystem.CheckStatus.NewPlayer:
				{
					if (this.interactionCounter < (int)this.interactionBuffs.Length && this.interactionBuffs[this.interactionCounter])
					{
						this.healthBuff = (ExperienceController.sharedController.currentLevel == 1 || ShopNGUIController.NoviceArmorAvailable ? this.firstBuffArmor : this.firstBuffNoArmor);
					}
					goto case BuffSystem.CheckStatus.OldPlayer;
				}
				case BuffSystem.CheckStatus.OldPlayer:
				{
					if (this.status == BuffSystem.CheckStatus.NewPlayer)
					{
						break;
					}
					float single = this.damageBuff;
					if (killrateByInteractions >= 0.8f)
					{
						obj = null;
					}
					else
					{
						obj = 1;
					}
					this.damageBuff = Mathf.Clamp(single, (float)obj, (float)((killrateByInteractions <= 2f ? 2 : 1)));
					float single1 = this.healthBuff;
					if (killrateByInteractions >= 0.8f)
					{
						obj1 = null;
					}
					else
					{
						obj1 = 1;
					}
					this.healthBuff = Mathf.Clamp(single1, (float)obj1, (float)((killrateByInteractions <= 2f ? 2 : 1)));
					break;
				}
				case BuffSystem.CheckStatus.Regular:
				{
					if (this.currentBuff == null)
					{
						float buffPercentByKillRate = 1f + 0.01f * this.GetBuffPercentByKillRate(killrateByInteractions);
						this.damageBuff = buffPercentByKillRate;
						this.healthBuff = buffPercentByKillRate;
					}
					else
					{
						this.damageBuff = this.currentBuff.param.damageBuff;
						this.healthBuff = this.currentBuff.param.healthBuff;
					}
					goto case BuffSystem.CheckStatus.OldPlayer;
				}
				default:
				{
					goto case BuffSystem.CheckStatus.OldPlayer;
				}
			}
		}
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetupBuffParameters(this.damageBuff, this.healthBuff);
		}
	}

	private void ClearBuffOfType(BuffSystem.SituationBuffType type)
	{
		for (int i = 0; i < this.situationBuffs.Count; i++)
		{
			if (this.situationBuffs[i].param.type == type)
			{
				this.situationBuffs.RemoveAt(i);
				i--;
			}
		}
	}

	private void ClearDebuffs()
	{
		for (int i = 0; i < this.situationBuffs.Count; i++)
		{
			if (this.situationBuffs[i].isDebuff)
			{
				this.situationBuffs.RemoveAt(i);
				i--;
			}
		}
	}

	public void DeathInteraction()
	{
		this.CheckAndWriteInteraction(BuffSystem.InteractionType.Death);
		this.SaveInteractions();
	}

	public void EndRound()
	{
		float killrateByInteractions = this.GetKillrateByInteractions();
		Debug.Log(killrateByInteractions);
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesKillRate(this.allRoundsCount, killrateByInteractions);
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
		if (this.buffsActive && this.configLoaded)
		{
			this.CheckExpiredBuffs();
			if (this.status != BuffSystem.CheckStatus.NewPlayer && this.status != BuffSystem.CheckStatus.OldPlayer)
			{
				if (this.lastGiveGunTime + this.GetTimeForGun() < NotificationController.instance.currentPlayTimeMatch)
				{
					this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
					if (this.GetKillrateByInteractions() <= this.debuffKillrateForGun)
					{
						this.GiveTryGunToPlayer();
					}
					else
					{
						this.AddSituationBuff(BuffSystem.SituationBuffType.DebuffBeforeGun, string.Empty);
					}
				}
				if (this.readyToGiveGun && WeaponManager.sharedManager._currentFilterMap == 0)
				{
					this.giveTryGun = true;
				}
			}
		}
		this.SaveValues();
		this.CheckForPlayerBuff();
	}

	private float GetBuffPercentByKillRate(float value)
	{
		float single = Mathf.Round(10f * value) / 10f;
		Debug.Log(string.Format("<color=green>Killrate: {0}</color>", single));
		if (this.tierParam.midbottom < single && single < this.tierParam.midtop)
		{
			return 0f;
		}
		return this.tierParam.b - Mathf.Clamp(single, this.tierParam.bottom, this.tierParam.top) * this.tierParam.a;
	}

	public float GetKillrateByInteractions()
	{
		if (this.killRateCached != -1f)
		{
			return this.killRateCached;
		}
		int num = 0;
		int num1 = 0;
		for (int i = 0; i < (int)this.interactions.Length; i++)
		{
			BuffSystem.InteractionType interactionType = this.interactions[i];
			if (interactionType == BuffSystem.InteractionType.Kill)
			{
				num++;
			}
			else if (interactionType == BuffSystem.InteractionType.Death)
			{
				num1++;
			}
		}
		if (num1 == 0)
		{
			this.killRateCached = (float)num;
		}
		else
		{
			this.killRateCached = (float)num / (float)num1;
		}
		return this.killRateCached;
	}

	public int GetRoundsForGun()
	{
		float killrateByInteractions = this.GetKillrateByInteractions();
		if (killrateByInteractions < this.tierParam.lowKillRate)
		{
			return this.roundsForGunLow;
		}
		if (killrateByInteractions < this.tierParam.highKillRate)
		{
			return this.roundsForGunMiddle;
		}
		return this.roundsForGunHigh;
	}

	private float GetTimeForGun()
	{
		float killrateByInteractions = this.GetKillrateByInteractions();
		if (killrateByInteractions < this.tierParam.lowKillRate)
		{
			return this.tierParam.timeToGetGunLow;
		}
		if (killrateByInteractions < this.tierParam.highKillRate)
		{
			return this.tierParam.timeToGetGunMiddle;
		}
		return this.tierParam.timeToGetGunHigh;
	}

	private void GiveTryGunToPlayer()
	{
		this.readyToGiveGun = true;
	}

	public bool haveBuffForWeapon(string weapon)
	{
		return (this.weaponBuff == null || string.IsNullOrEmpty(weapon) ? false : this.weaponBuff.weapon == weapon);
	}

	public void KillInteraction()
	{
		this.CheckAndWriteInteraction(BuffSystem.InteractionType.Kill);
	}

	private void LoadValues()
	{
		this.loadValuesCalled = true;
		string str = Storager.getString("buffsValues", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null)
		{
			if (strs.ContainsKey("interactionCount"))
			{
				this.interactionCounter = Convert.ToInt32(strs["interactionCount"]);
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
				this.status = (BuffSystem.CheckStatus)Convert.ToInt32(strs["status"]);
			}
			if (strs.ContainsKey("lastGiveGunTime"))
			{
				this.lastGiveGunTime = Convert.ToSingle(strs["lastGiveGunTime"]);
			}
			if (strs.ContainsKey("giveGun"))
			{
				this.readyToGiveGun = Convert.ToInt32(strs["giveGun"]) == 1;
			}
			if (strs.ContainsKey("waitTime"))
			{
				this.waitingForPurchaseTime = Convert.ToSingle(strs["waitTime"]);
			}
			if (strs.ContainsKey("waitBuff"))
			{
				BuffSystem.SituationBuffType num = (BuffSystem.SituationBuffType)Convert.ToInt32(strs["waitBuff"]);
				if (this.buffParamByType.ContainsKey(num))
				{
					this.waitingForPurchaseBuff = this.buffParamByType[num];
				}
			}
			if (strs.ContainsKey("buffs"))
			{
				List<object> item = strs["buffs"] as List<object>;
				for (int i = 0; i < item.Count; i++)
				{
					Dictionary<string, object> item1 = item[i] as Dictionary<string, object>;
					BuffSystem.SituationBuffType situationBuffType = (BuffSystem.SituationBuffType)Convert.ToInt32(item1["type"]);
					string str1 = (!item1.ContainsKey("weapon") ? string.Empty : Convert.ToString(item1["weapon"]));
					float single = Convert.ToSingle(item1["expire"]);
					BuffSystem.SituationBuff situationBuff = new BuffSystem.SituationBuff(this.buffParamByType[situationBuffType], str1, single);
					this.situationBuffs.Add(situationBuff);
				}
			}
		}
		if (this.status == BuffSystem.CheckStatus.None)
		{
			if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) == 1)
			{
				this.status = BuffSystem.CheckStatus.OldPlayer;
			}
			else
			{
				this.status = BuffSystem.CheckStatus.NewPlayer;
				this.isFirstRounds = true;
			}
			this.SaveValues();
		}
		string str2 = Storager.getString("buffsPlayerInteractions", false);
		List<object> objs = Json.Deserialize(str2) as List<object>;
		if (objs != null)
		{
			this.interactions = (
				from o in objs
				select (BuffSystem.InteractionType)Convert.ToInt32(o)).ToArray<BuffSystem.InteractionType>();
		}
	}

	public void LogFirstBattlesResult(bool isWinner)
	{
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesResult(this.allRoundsCount, isWinner);
		}
	}

	public void OnCurrencyBuyed(bool isGems, int index)
	{
		BuffSystem.SituationBuffType situationBuffType;
		if (!isGems)
		{
			if (index >= (int)this.coinsBuffByIndex.Length)
			{
				return;
			}
			situationBuffType = this.coinsBuffByIndex[index];
		}
		else
		{
			if (index >= (int)this.gemsBuffByIndex.Length)
			{
				return;
			}
			situationBuffType = this.gemsBuffByIndex[index];
		}
		if (!this.buffParamByType.ContainsKey(situationBuffType))
		{
			return;
		}
		BuffSystem.BuffParameter item = this.buffParamByType[situationBuffType];
		if (this.waitingForPurchaseBuff == null || this.waitingForPurchaseTime < item.timeForPurchase + NotificationController.instance.currentPlayTime)
		{
			this.waitingForPurchaseBuff = item;
			this.waitingForPurchaseTime = this.waitingForPurchaseBuff.timeForPurchase + NotificationController.instance.currentPlayTime;
		}
		this.SaveValues();
	}

	public void OnGetProgress()
	{
		if (this.status == BuffSystem.CheckStatus.None || this.status == BuffSystem.CheckStatus.NewPlayer)
		{
			this.status = BuffSystem.CheckStatus.OldPlayer;
			this.damageBuff = 1f;
			this.healthBuff = 1f;
			this.isFirstRounds = false;
		}
		this.SaveValues();
	}

	public void OnGunBuyed()
	{
		this.ClearDebuffs();
		this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
		this.SaveValues();
		this.CheckForPlayerBuff();
	}

	public void OnGunTakeOff()
	{
		this.ClearBuffOfType(BuffSystem.SituationBuffType.TryGunBuff);
		this.AddSituationBuff(BuffSystem.SituationBuffType.DebuffAfterGun, string.Empty);
	}

	public void OnSomethingPurchased()
	{
		if (this.waitingForPurchaseBuff != null)
		{
			if (this.waitingForPurchaseTime <= NotificationController.instance.currentPlayTime)
			{
				this.waitingForPurchaseBuff = null;
				this.waitingForPurchaseTime = 0f;
				this.SaveValues();
			}
			else
			{
				BuffSystem.SituationBuffType situationBuffType = this.waitingForPurchaseBuff.type;
				this.waitingForPurchaseBuff = null;
				this.waitingForPurchaseTime = 0f;
				this.ClearDebuffs();
				this.AddSituationBuff(situationBuffType, string.Empty);
			}
		}
	}

	public void OnTierLvlUp()
	{
		this.AddSituationBuff(BuffSystem.SituationBuffType.TierLvlUp, string.Empty);
	}

	public void OnTryGunBuyed(string weaponName)
	{
		this.ClearDebuffs();
		this.AddSituationBuff(BuffSystem.SituationBuffType.BuyedTryGun, weaponName);
	}

	public void PlayerLeaved()
	{
		this.CheckExpiredBuffs();
		this.SaveValues();
	}

	public void RemoveGunBuff()
	{
		this.ClearBuffOfType(BuffSystem.SituationBuffType.TryGunBuff);
	}

	private void SaveInteractions()
	{
		if (this.interactionsChanged)
		{
			this.interactionsChanged = false;
			Storager.setString("buffsPlayerInteractions", Json.Serialize((
				from o in (IEnumerable<BuffSystem.InteractionType>)this.interactions
				select (int)o).ToArray<int>()), false);
		}
	}

	private void SaveValues()
	{
		if (!this.loadValuesCalled)
		{
			return;
		}
		Dictionary<string, object> strs = new Dictionary<string, object>();
		if (this.interactionCounter > 0)
		{
			strs["interactionCount"] = this.interactionCounter;
		}
		if (this.isFirstRounds)
		{
			strs["isFirstRounds"] = 1;
		}
		if (this.readyToGiveGun)
		{
			strs["giveGun"] = 1;
		}
		if (this.allRoundsCount > 0)
		{
			strs["allRoundsCount"] = this.allRoundsCount;
		}
		if (this.situationBuffs != null && this.situationBuffs.Count > 0)
		{
			strs["buffs"] = (
				from o in this.situationBuffs
				select o.Serialize()).ToArray<Dictionary<string, object>>();
		}
		if (this.lastGiveGunTime > 0f)
		{
			strs["lastGiveGunTime"] = this.lastGiveGunTime;
		}
		if (this.waitingForPurchaseTime > 0f)
		{
			strs["waitTime"] = this.waitingForPurchaseTime;
		}
		if (this.waitingForPurchaseBuff != null)
		{
			strs["waitBuff"] = (int)this.waitingForPurchaseBuff.type;
		}
		strs["status"] = (int)this.status;
		Storager.setString("buffsValues", Json.Serialize(strs), false);
		this.SaveInteractions();
	}

	public void SetGetTryGun(string weaponName)
	{
		this.giveTryGun = false;
		this.readyToGiveGun = false;
		this.ClearDebuffs();
		this.AddSituationBuff(BuffSystem.SituationBuffType.TryGunBuff, weaponName);
	}

	public void TryLoadConfig()
	{
		if (this.configLoaded)
		{
			return;
		}
		if (!Storager.hasKey("BuffsParam"))
		{
			this.WriteDefaultParameters();
			return;
		}
		string str = Storager.getString("BuffsParam", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null || !strs.ContainsKey("buffSettings"))
		{
			this.WriteDefaultParameters();
			return;
		}
		Dictionary<string, object> item = strs["buffSettings"] as Dictionary<string, object>;
		string empty = string.Empty;
		if (!item.ContainsKey("roundsForGunLow"))
		{
			empty = "get roundsForGunLow";
		}
		else
		{
			this.roundsForGunLow = Convert.ToInt32(item["roundsForGunLow"]);
		}
		if (!item.ContainsKey("roundsForGunMiddle"))
		{
			empty = "get roundsForGunMiddle";
		}
		else
		{
			this.roundsForGunMiddle = Convert.ToInt32(item["roundsForGunMiddle"]);
		}
		if (!item.ContainsKey("roundsForGunHigh"))
		{
			empty = "get roundsForGunHigh";
		}
		else
		{
			this.roundsForGunHigh = Convert.ToInt32(item["roundsForGunHigh"]);
		}
		if (!item.ContainsKey("timeForDiscount"))
		{
			empty = "get timeForDiscount";
		}
		else
		{
			this.timeForDiscount = Convert.ToSingle(item["timeForDiscount"]);
		}
		if (!item.ContainsKey("discountValue"))
		{
			empty = "get discountValue";
		}
		else
		{
			this.discountValue = Convert.ToInt32(item["discountValue"]);
		}
		if (!item.ContainsKey("debuffKillrateForGun"))
		{
			empty = "get debuffKillrateForGun";
		}
		else
		{
			this.debuffKillrateForGun = Convert.ToSingle(item["debuffKillrateForGun"]);
		}
		if (!item.ContainsKey("firstBuffArmor"))
		{
			empty = "get firstBuffArmor";
		}
		else
		{
			this.firstBuffArmor = Convert.ToSingle(item["firstBuffArmor"]);
		}
		if (!item.ContainsKey("firstBuffNoArmor"))
		{
			empty = "get firstBuffNoArmor";
		}
		else
		{
			this.firstBuffNoArmor = Convert.ToSingle(item["firstBuffNoArmor"]);
		}
		if (!item.ContainsKey("tierParams"))
		{
			empty = "get tierParams";
		}
		else
		{
			List<object> objs = item["tierParams"] as List<object>;
			if (objs == null)
			{
				empty = "tierParams == null";
			}
			else
			{
				this.paramsByTier = (
					from e in objs
					select new BuffSystem.ParamByTier(e as Dictionary<string, object>)).ToArray<BuffSystem.ParamByTier>();
			}
		}
		if (!item.ContainsKey("buffsParams"))
		{
			empty = "get buffsParams";
		}
		else
		{
			List<object> item1 = item["buffsParams"] as List<object>;
			this.buffParamByType = new Dictionary<BuffSystem.SituationBuffType, BuffSystem.BuffParameter>();
			for (int i = 0; i < item1.Count; i++)
			{
				Dictionary<string, object> strs1 = item1[i] as Dictionary<string, object>;
				BuffSystem.SituationBuffType situationBuffType = (BuffSystem.SituationBuffType)((int)Enum.Parse(typeof(BuffSystem.SituationBuffType), Convert.ToString(strs1["type"])));
				this.buffParamByType.Add(situationBuffType, new BuffSystem.BuffParameter(strs1));
			}
		}
		if (string.IsNullOrEmpty(empty))
		{
			this.configLoaded = true;
		}
		else
		{
			Debug.LogError(string.Concat("Error Deserialize JSON: buffSettings - ", empty));
			this.WriteDefaultParameters();
		}
	}

	private void WriteDefaultParameters()
	{
		this.configLoaded = false;
		if (this.paramsByTier == null)
		{
			this.paramsByTier = new BuffSystem.ParamByTier[6];
			for (int i = 0; i < (int)this.paramsByTier.Length; i++)
			{
				this.paramsByTier[i] = new BuffSystem.ParamByTier();
			}
		}
		this.buffParamByType = new Dictionary<BuffSystem.SituationBuffType, BuffSystem.BuffParameter>()
		{
			{ BuffSystem.SituationBuffType.DebuffBeforeGun, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.DebuffBeforeGun, 1f, 0.5f, 540f, -1) },
			{ BuffSystem.SituationBuffType.DebuffAfterGun, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.DebuffAfterGun, 1f, 0.5f, 180f, -1) },
			{ BuffSystem.SituationBuffType.TierLvlUp, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.TierLvlUp, 1f, 0.7f, 240f, -1) },
			{ BuffSystem.SituationBuffType.TryGunBuff, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.TryGunBuff, 1f, 1.25f, 0f, 1) },
			{ BuffSystem.SituationBuffType.BuyedTryGun, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.BuyedTryGun, 1f, 1.25f, 600f, 2) },
			{ BuffSystem.SituationBuffType.Coin1, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin1, 1f, 1.25f, 300f, 3) },
			{ BuffSystem.SituationBuffType.Coin7, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin7, 1f, 1.25f, 400f, 3) },
			{ BuffSystem.SituationBuffType.Coin2, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin2, 1f, 1.25f, 500f, 3) },
			{ BuffSystem.SituationBuffType.Coin3, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin3, 1f, 1.25f, 600f, 3) },
			{ BuffSystem.SituationBuffType.Coin4, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin4, 1f, 1.25f, 700f, 3) },
			{ BuffSystem.SituationBuffType.Coin5, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin5, 1f, 1.25f, 800f, 3) },
			{ BuffSystem.SituationBuffType.Coin8, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin8, 1f, 1.25f, 900f, 3) },
			{ BuffSystem.SituationBuffType.Gem1, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem1, 1f, 1.25f, 300f, 4) },
			{ BuffSystem.SituationBuffType.Gem2, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem2, 1f, 1.25f, 400f, 4) },
			{ BuffSystem.SituationBuffType.Gem3, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem3, 1f, 1.25f, 500f, 4) },
			{ BuffSystem.SituationBuffType.Gem4, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem4, 1f, 1.25f, 600f, 4) },
			{ BuffSystem.SituationBuffType.Gem5, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem5, 1f, 1.25f, 700f, 4) },
			{ BuffSystem.SituationBuffType.Gem6, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem6, 1f, 1.25f, 800f, 4) },
			{ BuffSystem.SituationBuffType.Gem7, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem7, 1f, 1.25f, 900f, 4) }
		};
	}

	private void WriteInteraction(BuffSystem.InteractionType value)
	{
		this.interactionsChanged = true;
		this.killRateCached = -1f;
		for (int i = (int)this.interactions.Length - 2; i >= 0; i--)
		{
			this.interactions[i + 1] = this.interactions[i];
		}
		this.interactions[0] = value;
	}

	private class BuffParameter
	{
		public int priority;

		public BuffSystem.SituationBuffType type;

		public float healthBuff;

		public float damageBuff;

		public float time;

		public float timeForPurchase;

		public BuffParameter(BuffSystem.SituationBuffType type, float healthBuff, float damageBuff, float time, int priority)
		{
			this.type = type;
			this.healthBuff = healthBuff;
			this.damageBuff = damageBuff;
			this.priority = priority;
			this.time = time;
			this.timeForPurchase = 1800f;
		}

		public BuffParameter(Dictionary<string, object> dictionary)
		{
			this.type = (BuffSystem.SituationBuffType)((int)Enum.Parse(typeof(BuffSystem.SituationBuffType), Convert.ToString(dictionary["type"])));
			if (!dictionary.ContainsKey("health"))
			{
				this.healthBuff = 1f;
			}
			else
			{
				this.healthBuff = Convert.ToSingle(dictionary["health"]);
			}
			if (!dictionary.ContainsKey("damage"))
			{
				this.damageBuff = 1f;
			}
			else
			{
				this.damageBuff = Convert.ToSingle(dictionary["damage"]);
			}
			if (!dictionary.ContainsKey("timeToBuy"))
			{
				this.timeForPurchase = 0f;
			}
			else
			{
				this.timeForPurchase = Convert.ToSingle(dictionary["timeToBuy"]);
			}
			this.priority = Convert.ToInt32(dictionary["prior"]);
			this.time = Convert.ToSingle(dictionary["time"]);
		}
	}

	private enum CheckStatus
	{
		None,
		NewPlayer,
		OldPlayer,
		Regular
	}

	private enum InteractionType
	{
		None,
		Kill,
		Death
	}

	public class ParamByTier
	{
		public float timeToGetGunLow;

		public float timeToGetGunMiddle;

		public float timeToGetGunHigh;

		public float lowKillRate;

		public float highKillRate;

		public float a;

		public float b;

		public float midbottom;

		public float midtop;

		public float top;

		public float bottom;

		public ParamByTier()
		{
			this.timeToGetGunLow = 2400f;
			this.timeToGetGunMiddle = 3600f;
			this.timeToGetGunHigh = 4800f;
			this.lowKillRate = 0.5f;
			this.highKillRate = 1.2f;
			this.a = 50f;
			this.b = 50f;
			this.midbottom = 0.8f;
			this.midtop = 1.2f;
			this.top = 2f;
			this.bottom = 0f;
		}

		public ParamByTier(Dictionary<string, object> dictionary)
		{
			this.timeToGetGunLow = Convert.ToSingle(dictionary["timeGunLow"]);
			this.timeToGetGunMiddle = Convert.ToSingle(dictionary["timeGunMiddle"]);
			this.timeToGetGunHigh = Convert.ToSingle(dictionary["timeGunHigh"]);
			this.lowKillRate = Convert.ToSingle(dictionary["lowKillRate"]);
			this.highKillRate = Convert.ToSingle(dictionary["highKillRate"]);
			this.a = Convert.ToSingle(dictionary["form_a"]);
			this.b = Convert.ToSingle(dictionary["form_b"]);
			this.midbottom = Convert.ToSingle(dictionary["form_midbottom"]);
			this.midtop = Convert.ToSingle(dictionary["form_midtop"]);
			this.top = Convert.ToSingle(dictionary["form_top"]);
			if (dictionary.ContainsKey("form_bottom"))
			{
				this.bottom = Convert.ToSingle(dictionary["form_bottom"]);
			}
		}
	}

	private class SituationBuff
	{
		public BuffSystem.BuffParameter param;

		private float expireTime;

		public string weapon;

		public bool expired
		{
			get
			{
				return this.expireTime < NotificationController.instance.currentPlayTimeMatch;
			}
		}

		public bool isDebuff
		{
			get
			{
				return (this.param.healthBuff < 1f ? true : this.param.damageBuff < 1f);
			}
		}

		public SituationBuff(BuffSystem.BuffParameter param, string weaponBuff)
		{
			this.param = param;
			this.weapon = weaponBuff;
			this.expireTime = NotificationController.instance.currentPlayTimeMatch + param.time;
		}

		public SituationBuff(BuffSystem.BuffParameter param, string weaponBuff, float savedTime)
		{
			this.param = param;
			this.weapon = weaponBuff;
			this.expireTime = savedTime;
		}

		public Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>();
			strs["type"] = (int)this.param.type;
			strs["expire"] = this.expireTime;
			if (!string.IsNullOrEmpty(this.weapon))
			{
				strs["weapon"] = this.weapon;
			}
			return strs;
		}
	}

	private enum SituationBuffType
	{
		DebuffBeforeGun,
		DebuffAfterGun,
		TierLvlUp,
		TryGunBuff,
		BuyedTryGun,
		Coin1,
		Coin7,
		Coin2,
		Coin3,
		Coin4,
		Coin5,
		Coin8,
		Gem1,
		Gem2,
		Gem3,
		Gem4,
		Gem5,
		Gem6,
		Gem7,
		Count
	}
}