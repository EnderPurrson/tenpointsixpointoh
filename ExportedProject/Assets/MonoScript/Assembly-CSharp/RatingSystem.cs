using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RatingSystem : MonoBehaviour
{
	private static RatingSystem _instance;

	public readonly static string[] divisionByIndex;

	public readonly static string[] leagueChangeLocalizations;

	public readonly static string[] leagueLocalizations;

	private float[] winRaitingFactorByPlace = new float[] { 1.2f, 1.1f, 1f, 0.9f, 0.8f };

	private float[] looseRaitingFactorByPlace = new float[] { 0.8f, 0.9f, 1f, 1.1f, 1.2f };

	private float form_kd_a = 5f;

	private float form_kd_b = 5f;

	private float form_kd_top = 2f;

	private float form_place_coeff = 3f;

	private float form_hunger_a = 0.5f;

	private float form_hunger_b = 5f;

	private int[] hungerLeagueCoefs = new int[] { 40, 40, 40, 35, 35, 35, 30, 30, 30, 25, 25, 25, 20, 20, 20, 20 };

	private int form_min = 1;

	private int form_max = 1;

	private int[] leagueCoefs = new int[] { 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15 };

	private int[] leagueRatings = new int[] { 0, 200, 400, 600, 800, 1000, 1200, 1400, 1600, 1800, 2000, 2200, 2400, 2600, 2800, 3000 };

	public RatingSystem.RatingLeague currentLeague;

	public int currentDivision;

	public SaltedInt lastRatingChange = new SaltedInt(210674148);

	public bool ratingMatch = true;

	public RatingSystem.RatingUpdate OnRatingUpdate;

	public int currentRating
	{
		get
		{
			return this.positiveRating - this.negativeRating;
		}
	}

	public RatingSystem.RatingChange currentRatingChange
	{
		get
		{
			return new RatingSystem.RatingChange(this.currentLeague, this.currentDivision, this.currentRating);
		}
	}

	private int form_coef
	{
		get
		{
			return this.leagueCoefs[Mathf.Min((int)this.currentLeague * (int)RatingSystem.RatingLeague.Crystal + this.currentDivision, (int)this.leagueCoefs.Length - 1)];
		}
	}

	private int form_hungerCoef
	{
		get
		{
			return this.hungerLeagueCoefs[Mathf.Min((int)this.currentLeague * (int)RatingSystem.RatingLeague.Crystal + this.currentDivision, (int)this.hungerLeagueCoefs.Length - 1)];
		}
	}

	public static RatingSystem instance
	{
		get
		{
			if (RatingSystem._instance == null)
			{
				GameObject gameObject = new GameObject("RatingSystem");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				RatingSystem._instance = gameObject.AddComponent<RatingSystem>();
			}
			return RatingSystem._instance;
		}
	}

	public int negativeRating
	{
		get
		{
			if (!Storager.hasKey("RatingNegative"))
			{
				return 0;
			}
			return Storager.getInt("RatingNegative", false);
		}
		set
		{
			Storager.setInt("RatingNegative", value, false);
		}
	}

	public int positiveRating
	{
		get
		{
			if (!Storager.hasKey("RatingPositive"))
			{
				return 0;
			}
			return Storager.getInt("RatingPositive", false);
		}
		set
		{
			Storager.setInt("RatingPositive", value, false);
		}
	}

	static RatingSystem()
	{
		RatingSystem.divisionByIndex = new string[] { "III", "II", "I" };
		RatingSystem.leagueChangeLocalizations = new string[] { "Key_2139", "Key_2140", "Key_2141", "Key_2142", "Key_2143", "Key_2144" };
		RatingSystem.leagueLocalizations = new string[] { "Key_1953", "Key_1954", "Key_1955", "Key_1956", "Key_1957", "Key_1958" };
	}

	public RatingSystem()
	{
	}

	private void AddToRating(int rating)
	{
		if (rating <= 0)
		{
			RatingSystem ratingSystem = this;
			ratingSystem.negativeRating = ratingSystem.negativeRating - rating;
			this.negativeRating = Mathf.Min(this.negativeRating, this.positiveRating);
		}
		else
		{
			RatingSystem ratingSystem1 = this;
			ratingSystem1.positiveRating = ratingSystem1.positiveRating + rating;
		}
		this.UpdateLeague(rating > 0);
		this.SaveValues();
		TrophiesSynchronizer.Instance.Push();
		base.StartCoroutine(FriendsController.sharedController.SynchRating(this.currentRating));
		Debug.Log(string.Format("<color=yellow>Add {0} rating.</color>", rating));
		Debug.Log(string.Format("<color=yellow>I'm in {0} league, division: {1}. Rating: {2}</color>", this.currentLeague.ToString(), 3 - this.currentDivision, this.currentRating));
	}

	private void Awake()
	{
		this.LoadValues();
		this.ParseConfig();
		TrophiesSynchronizer.Instance.Updated += new EventHandler(this.UpdateLeagueEvent);
	}

	public void BackupLastRatingTake()
	{
		if (this.lastRatingChange.Value >= 0)
		{
			return;
		}
		RatingSystem value = this;
		value.negativeRating = value.negativeRating + this.lastRatingChange.Value;
		this.UpdateLeague(true);
		this.SaveValues();
		Debug.Log(string.Format("<color=yellow>Rating backup: {0} rating.</color>", this.lastRatingChange.Value));
		Debug.Log(string.Format("<color=yellow>I'm in {0} league, division: {1}. Rating: {2}</color>", this.currentLeague.ToString(), 3 - this.currentDivision, this.currentRating));
		this.lastRatingChange.Value = 0;
	}

	public RatingSystem.RatingChange CalculateRating(int playersCount, int place, float matchKillrate, bool deadheat = false)
	{
		RatingSystem.RatingChange ratingChange = this.currentRatingChange;
		int num = (matchKillrate > this.form_kd_top ? Mathf.RoundToInt(this.form_kd_b) : Mathf.RoundToInt(matchKillrate * this.form_kd_b - this.form_kd_a));
		int num1 = Mathf.Max(playersCount - 1, 1);
		int num2 = Mathf.RoundToInt((float)this.form_coef * (((float)num1 / 2f - (float)place) / ((float)num1 / this.form_place_coeff)));
		if (num2 >= 0 && num2 + num >= 0)
		{
			num2 += num;
		}
		if (deadheat)
		{
			num2 = num;
		}
		this.AddToRating(num2);
		ratingChange = ratingChange.AddChange(this.currentLeague, this.currentDivision, this.currentRating);
		this.lastRatingChange.Value = ratingChange.addRating;
		return ratingChange;
	}

	public RatingSystem.RatingChange CalculateRatingDeadlyGames(bool win, int killcount)
	{
		RatingSystem.RatingChange ratingChange = this.currentRatingChange;
		int num = this.currentRating;
		int num1 = (!win ? -1 * Mathf.RoundToInt((float)this.form_hungerCoef * this.form_hunger_a / this.form_hunger_b) : Mathf.RoundToInt((float)this.form_hungerCoef * this.form_hunger_a));
		if (num1 <= 0)
		{
			RatingSystem ratingSystem = this;
			ratingSystem.negativeRating = ratingSystem.negativeRating - num1;
			this.negativeRating = Mathf.Min(this.negativeRating, this.positiveRating);
		}
		else
		{
			RatingSystem ratingSystem1 = this;
			ratingSystem1.positiveRating = ratingSystem1.positiveRating + num1;
		}
		this.UpdateLeague(num1 > 0);
		this.SaveValues();
		base.StartCoroutine(FriendsController.sharedController.SynchRating(this.currentRating));
		Debug.Log(string.Format("<color=yellow>Add {0} rating (Hunger).</color>", num1));
		Debug.Log(string.Format("<color=yellow>I'm in {0} league, division: {1}. Rating: {2}</color>", this.currentLeague.ToString(), 3 - this.currentDivision, this.currentRating));
		ratingChange = ratingChange.AddChange(this.currentLeague, this.currentDivision, this.currentRating);
		this.lastRatingChange.Value = ratingChange.addRating;
		TrophiesSynchronizer.Instance.Push();
		return ratingChange;
	}

	public RatingSystem.RatingChange CalculateRatingOld(bool win, int place, bool deadheat, int[] enemiesRating)
	{
		float single;
		RatingSystem.RatingChange ratingChange = this.currentRatingChange;
		int num = this.currentRating;
		if (!win)
		{
			single = (!deadheat ? 0f : 0.5f);
		}
		else
		{
			single = 1f;
		}
		float single1 = single;
		int num1 = 0;
		for (int i = 0; i < (int)enemiesRating.Length; i++)
		{
			num1 += enemiesRating[i];
		}
		int num2 = Mathf.RoundToInt((float)this.form_coef * (single1 - 1f / (1f + Mathf.Pow(10f, (float)((num1 / (int)enemiesRating.Length - num) / 400)))));
		if (num2 == 0 && single1 != 0.5f)
		{
			num2 = (single1 <= 0.5f ? -this.form_min : this.form_max);
		}
		num2 = (win || deadheat ? Mathf.RoundToInt((float)num2 * this.winRaitingFactorByPlace[(place >= 5 ? 4 : place)]) : Mathf.RoundToInt((float)num2 * this.looseRaitingFactorByPlace[(place >= 5 ? 4 : place)]));
		if (num2 <= 0)
		{
			RatingSystem ratingSystem = this;
			ratingSystem.negativeRating = ratingSystem.negativeRating - num2;
			this.negativeRating = Mathf.Min(this.negativeRating, this.positiveRating);
		}
		else
		{
			RatingSystem ratingSystem1 = this;
			ratingSystem1.positiveRating = ratingSystem1.positiveRating + num2;
		}
		this.UpdateLeague(num2 > 0);
		this.SaveValues();
		base.StartCoroutine(FriendsController.sharedController.SynchRating(this.currentRating));
		Debug.Log(string.Format("<color=yellow>Add {0} rating.</color>", num2));
		Debug.Log(string.Format("<color=yellow>I'm in {0} league, division: {1}. Rating: {2}</color>", this.currentLeague.ToString(), 3 - this.currentDivision, this.currentRating));
		ratingChange = ratingChange.AddChange(this.currentLeague, this.currentDivision, this.currentRating);
		this.lastRatingChange.Value = ratingChange.addRating;
		TrophiesSynchronizer.Instance.Push();
		return ratingChange;
	}

	public int DivisionInLeague(RatingSystem.RatingLeague league)
	{
		if (league < this.currentLeague)
		{
			return 2;
		}
		if (league > this.currentLeague)
		{
			return 0;
		}
		return this.currentDivision;
	}

	public float GetRatingAmountForLeague(RatingSystem.RatingLeague league)
	{
		float single = (float)this.leagueRatings[Mathf.Clamp((int)league * (int)RatingSystem.RatingLeague.Crystal, 0, (int)this.leagueRatings.Length - 1)];
		float single1 = (float)this.leagueRatings[Mathf.Clamp(((int)league + (int)RatingSystem.RatingLeague.Steel) * (int)RatingSystem.RatingLeague.Crystal, 0, (int)this.leagueRatings.Length - 1)];
		float single2 = 0.03f;
		if (this.currentRating == 0)
		{
			single2 = 0f;
		}
		return Mathf.Max(single2, Mathf.Clamp01((float)((float)this.currentRating - single) / (float)(single1 - single)));
	}

	private void LoadValues()
	{
		string str = Storager.getString("RatingSystem", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs != null)
		{
			if (strs.ContainsKey("League"))
			{
				this.currentLeague = (RatingSystem.RatingLeague)Convert.ToInt32(strs["League"]);
			}
			if (strs.ContainsKey("Division"))
			{
				this.currentDivision = Convert.ToInt32(strs["Division"]);
			}
		}
	}

	public int MaxRatingInDivision(RatingSystem.RatingLeague league, int division)
	{
		if ((int)league * (int)RatingSystem.RatingLeague.Crystal + division + (int)RatingSystem.RatingLeague.Steel >= (int)this.leagueRatings.Length)
		{
			return 2147483647;
		}
		return this.leagueRatings[(int)league * (int)RatingSystem.RatingLeague.Crystal + division + (int)RatingSystem.RatingLeague.Steel];
	}

	public int MaxRatingInLeague(RatingSystem.RatingLeague league)
	{
		return this.MaxRatingInDivision(league, 2);
	}

	public void OnGetCloudValues(int ratingPositive, int ratingNegative)
	{
		this.positiveRating = ratingPositive;
		this.negativeRating = ratingNegative;
		this.UpdateLeagueByRating();
		this.SaveValues();
	}

	public void ParseConfig()
	{
		string str = Storager.getString("rSCKey", false);
		Dictionary<string, object> strs = Json.Deserialize(str) as Dictionary<string, object>;
		if (strs == null)
		{
			return;
		}
		if (strs.ContainsKey("settingRatingSystem"))
		{
			Dictionary<string, object> item = strs["settingRatingSystem"] as Dictionary<string, object>;
			if (item.ContainsKey("min"))
			{
				this.form_min = Convert.ToInt32(item["min"]);
			}
			if (item.ContainsKey("max"))
			{
				this.form_max = Convert.ToInt32(item["max"]);
			}
			if (item.ContainsKey("leveling"))
			{
				List<object> objs = item["leagueRatings"] as List<object>;
				for (int i = 0; i < (int)this.leagueRatings.Length; i++)
				{
					if (objs.Count < i)
					{
						this.leagueRatings[i] = Convert.ToInt32(objs[i]);
					}
				}
			}
			if (item.ContainsKey("leagueCoefs"))
			{
				List<object> item1 = item["leagueCoefs"] as List<object>;
				for (int j = 0; j < (int)this.leagueCoefs.Length; j++)
				{
					if (item1.Count < j)
					{
						this.leagueCoefs[j] = Convert.ToInt32(item1[j]);
					}
				}
			}
			if (item.ContainsKey("form_kd_a"))
			{
				this.form_kd_a = (float)Convert.ToDouble(item["form_kd_a"]);
			}
			if (item.ContainsKey("form_kd_b"))
			{
				this.form_kd_b = (float)Convert.ToInt32(item["form_kd_b"]);
			}
			if (item.ContainsKey("form_kd_top"))
			{
				this.form_kd_top = (float)Convert.ToInt32(item["form_kd_top"]);
			}
			if (item.ContainsKey("form_place_coeff"))
			{
				this.form_min = Convert.ToInt32(item["form_place_coeff"]);
			}
		}
	}

	private void SaveValues()
	{
		Dictionary<string, object> strs = new Dictionary<string, object>();
		strs["League"] = (int)this.currentLeague;
		strs["Division"] = this.currentDivision;
		Storager.setString("RatingSystem", Json.Serialize(strs), false);
	}

	private void UpdateLeague(bool up)
	{
		int num = this.currentRating;
		int num1 = (int)this.currentLeague * (int)RatingSystem.RatingLeague.Crystal + this.currentDivision;
		int num2 = (!up ? 0 : num1);
		while (true)
		{
			if (num2 >= (!up ? num1 + 1 : (int)this.leagueRatings.Length))
			{
				break;
			}
			if (num >= this.leagueRatings[num2] + (!up ? -100 : 0))
			{
				this.currentLeague = (RatingSystem.RatingLeague)Mathf.FloorToInt((float)num2 / 3f);
				this.currentDivision = num2 - (int)this.currentLeague * (int)RatingSystem.RatingLeague.Crystal;
			}
			num2++;
		}
	}

	private void UpdateLeagueByRating()
	{
		int num = this.currentRating;
		for (int i = 0; i < (int)this.leagueRatings.Length; i++)
		{
			if (num >= this.leagueRatings[i])
			{
				this.currentLeague = (RatingSystem.RatingLeague)Mathf.FloorToInt((float)i / 3f);
				this.currentDivision = i - (int)this.currentLeague * (int)RatingSystem.RatingLeague.Crystal;
			}
		}
	}

	private void UpdateLeagueEvent(object o, EventArgs arg)
	{
		this.UpdateLeagueByRating();
		this.SaveValues();
		if (this.OnRatingUpdate != null)
		{
			this.OnRatingUpdate();
		}
	}

	public struct RatingChange
	{
		public RatingSystem.RatingLeague oldLeague;

		public RatingSystem.RatingLeague newLeague;

		public int oldDivision;

		public int newDivision;

		public int oldRating;

		public int newRating;

		public int addRating
		{
			get
			{
				return this.newRating - this.oldRating;
			}
		}

		public bool divisionChanged
		{
			get
			{
				return this.oldDivision != this.newDivision;
			}
		}

		public bool isDown
		{
			get
			{
				return this.GetNewLeagueIndex() < this.GetOldLeagueIndex();
			}
		}

		public bool isUp
		{
			get
			{
				return this.GetNewLeagueIndex() > this.GetOldLeagueIndex();
			}
		}

		public bool leagueChanged
		{
			get
			{
				return this.oldLeague != this.newLeague;
			}
		}

		public int maxRating
		{
			get
			{
				return RatingSystem.instance.MaxRatingInDivision(this.oldLeague, this.oldDivision);
			}
		}

		public float newRatingAmount
		{
			get
			{
				float single = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex(), 0, (int)RatingSystem.instance.leagueRatings.Length - 1)];
				float single1 = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex() + 1, 0, (int)RatingSystem.instance.leagueRatings.Length - 1)];
				float single2 = (this.newRating <= this.oldRating ? 0.015f : 0.03f);
				if ((float)this.newRating - (single - 100f) < 0f)
				{
					single2 = 0f;
				}
				return Mathf.Max(single2, Mathf.Clamp01((float)((float)this.newRating - single) / (float)(single1 - single)));
			}
		}

		public float oldRatingAmount
		{
			get
			{
				float single = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex(), 0, (int)RatingSystem.instance.leagueRatings.Length - 1)];
				float single1 = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex() + 1, 0, (int)RatingSystem.instance.leagueRatings.Length - 1)];
				float single2 = (this.oldRating <= this.newRating ? 0.015f : 0.03f);
				if (this.oldRating == 0)
				{
					single2 = 0f;
				}
				return Mathf.Max(single2, Mathf.Clamp01((float)((float)this.oldRating - single) / (float)(single1 - single)));
			}
		}

		public RatingChange(RatingSystem.RatingLeague currentLeague, int currentDivision, int currentRating)
		{
			this.oldLeague = currentLeague;
			this.oldDivision = currentDivision;
			this.oldRating = currentRating;
			this.newLeague = currentLeague;
			this.newDivision = currentDivision;
			this.newRating = currentRating;
		}

		public RatingChange(RatingSystem.RatingLeague oldLeague, RatingSystem.RatingLeague newLeague, int oldDivision, int newDivision, int oldRating, int newRating)
		{
			this.oldLeague = oldLeague;
			this.oldDivision = oldDivision;
			this.oldRating = oldRating;
			this.newLeague = newLeague;
			this.newDivision = newDivision;
			this.newRating = newRating;
		}

		public RatingSystem.RatingChange AddChange(RatingSystem.RatingLeague league, int division, int rating)
		{
			return new RatingSystem.RatingChange(this.oldLeague, league, this.oldDivision, division, this.oldRating, rating);
		}

		private int GetNewLeagueIndex()
		{
			return (int)this.newLeague * (int)RatingSystem.RatingLeague.Crystal + this.newDivision;
		}

		private int GetOldLeagueIndex()
		{
			return (int)this.oldLeague * (int)RatingSystem.RatingLeague.Crystal + this.oldDivision;
		}
	}

	public enum RatingLeague
	{
		Wood,
		Steel,
		Gold,
		Crystal,
		Ruby,
		Adamant
	}

	public delegate void RatingUpdate();
}