using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rilisoft
{
	public abstract class QuestBase
	{
		private readonly string _id;

		private long _day;

		private readonly int _slot;

		private readonly Rilisoft.Difficulty _difficulty;

		private readonly Rilisoft.Reward _reward;

		private bool _dirty;

		private bool _active;

		private bool _rewarded;

		private EventHandler Changed;

		public long Day
		{
			get
			{
				return this._day;
			}
		}

		public Rilisoft.Difficulty Difficulty
		{
			get
			{
				return this._difficulty;
			}
		}

		public bool Dirty
		{
			get
			{
				return this._dirty;
			}
		}

		public string Id
		{
			get
			{
				return this._id;
			}
		}

		public Rilisoft.Reward Reward
		{
			get
			{
				return this._reward;
			}
		}

		public bool Rewarded
		{
			get
			{
				return this._rewarded;
			}
		}

		public int Slot
		{
			get
			{
				return this._slot;
			}
		}

		public QuestBase(string id, long day, int slot, Rilisoft.Difficulty difficulty, Rilisoft.Reward reward, bool active, bool rewarded)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("Id should not be empty.");
			}
			this._id = id;
			this._day = day;
			this._slot = slot;
			this._difficulty = difficulty;
			this._reward = reward;
			this._active = active;
			this._rewarded = rewarded;
		}

		protected virtual void AppendProperties(Dictionary<string, object> properties)
		{
		}

		protected virtual void ApppendDifficultyProperties(Dictionary<string, object> difficultyProperties)
		{
		}

		public abstract decimal CalculateProgress();

		internal void DebugSetDay(long day)
		{
			this._day = day;
			this._dirty = true;
		}

		public bool SetActive()
		{
			if (this._active)
			{
				return false;
			}
			this._active = true;
			this._dirty = true;
			return true;
		}

		public void SetClean()
		{
			this._dirty = false;
		}

		protected void SetDirty()
		{
			this._dirty = true;
			this.Changed.Do<EventHandler>((EventHandler h) => h(this, EventArgs.Empty));
		}

		public void SetRewarded()
		{
			this._rewarded = true;
			this._dirty = true;
		}

		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, object> strs = new Dictionary<string, object>(2)
			{
				{ "reward", this.Reward.ToJson() }
			};
			Dictionary<string, object> strs1 = strs;
			this.ApppendDifficultyProperties(strs1);
			strs = new Dictionary<string, object>(3)
			{
				{ "id", this.Id },
				{ "day", this.Day },
				{ "slot", this.Slot },
				{ QuestConstants.GetDifficultyKey(this.Difficulty), strs1 },
				{ "active", Convert.ToInt32(this._active) },
				{ "rewarded", Convert.ToInt32(this.Rewarded) }
			};
			Dictionary<string, object> strs2 = strs;
			this.AppendProperties(strs2);
			return strs2;
		}

		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}

		public event EventHandler Changed
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Changed += value;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Changed -= value;
			}
		}
	}
}