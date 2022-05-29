using System;
using System.Runtime.CompilerServices;

namespace GooglePlayGames.BasicApi
{
	public class PlayerStats
	{
		private static float UNSET_VALUE;

		public float AvgSessonLength
		{
			get;
			set;
		}

		public float ChurnProbability
		{
			get;
			set;
		}

		public int DaysSinceLastPlayed
		{
			get;
			set;
		}

		public int NumberOfPurchases
		{
			get;
			set;
		}

		public int NumberOfSessions
		{
			get;
			set;
		}

		public float SessPercentile
		{
			get;
			set;
		}

		public float SpendPercentile
		{
			get;
			set;
		}

		public bool Valid
		{
			get;
			set;
		}

		static PlayerStats()
		{
			PlayerStats.UNSET_VALUE = -1f;
		}

		public PlayerStats()
		{
			this.Valid = false;
		}

		public bool HasAvgSessonLength()
		{
			return this.AvgSessonLength != PlayerStats.UNSET_VALUE;
		}

		public bool HasChurnProbability()
		{
			return this.ChurnProbability != PlayerStats.UNSET_VALUE;
		}

		public bool HasDaysSinceLastPlayed()
		{
			return this.DaysSinceLastPlayed != (int)PlayerStats.UNSET_VALUE;
		}

		public bool HasNumberOfPurchases()
		{
			return this.NumberOfPurchases != (int)PlayerStats.UNSET_VALUE;
		}

		public bool HasNumberOfSessions()
		{
			return this.NumberOfSessions != (int)PlayerStats.UNSET_VALUE;
		}

		public bool HasSessPercentile()
		{
			return this.SessPercentile != PlayerStats.UNSET_VALUE;
		}

		public bool HasSpendPercentile()
		{
			return this.SpendPercentile != PlayerStats.UNSET_VALUE;
		}
	}
}