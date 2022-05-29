using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public abstract class AccumulativeQuestBase : QuestBase
	{
		private readonly int _requiredCount;

		private int _currentCount;

		public int CurrentCount
		{
			get
			{
				return this._currentCount;
			}
		}

		public int RequiredCount
		{
			get
			{
				return this._requiredCount;
			}
		}

		public AccumulativeQuestBase(string id, long day, int slot, Rilisoft.Difficulty difficulty, Rilisoft.Reward reward, bool active, bool rewarded, int requiredCount, int initialCound) : base(id, day, slot, difficulty, reward, active, rewarded)
		{
			if (requiredCount < 1)
			{
				throw new ArgumentOutOfRangeException("requiredCount", (object)requiredCount, "Requires at least 1.");
			}
			this._requiredCount = requiredCount;
			this._currentCount = Mathf.Clamp(initialCound, 0, requiredCount);
		}

		protected override void AppendProperties(Dictionary<string, object> properties)
		{
			properties["currentCount"] = this._currentCount;
		}

		protected override void ApppendDifficultyProperties(Dictionary<string, object> difficultyProperties)
		{
			difficultyProperties["parameter"] = this._requiredCount;
		}

		public override decimal CalculateProgress()
		{
			return this._currentCount / this.RequiredCount;
		}

		public void Increment(int count = 1)
		{
			this.IncrementIf(true, count);
		}

		public void IncrementIf(bool condition, int count = 1)
		{
			if (!condition)
			{
				return;
			}
			decimal num = this.CalculateProgress();
			this._currentCount = Mathf.Clamp(this._currentCount + count, 0, this._requiredCount);
			if (num < new decimal(1))
			{
				base.SetDirty();
			}
		}
	}
}