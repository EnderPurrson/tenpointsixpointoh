using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	internal struct CampaignProgressMemento : IEquatable<CampaignProgressMemento>
	{
		[SerializeField]
		private List<LevelProgressMemento> levels;

		private bool _conflicted;

		internal bool Conflicted
		{
			get
			{
				return this._conflicted;
			}
		}

		internal List<LevelProgressMemento> Levels
		{
			get
			{
				if (this.levels == null)
				{
					this.levels = new List<LevelProgressMemento>();
				}
				return this.levels;
			}
		}

		internal CampaignProgressMemento(bool conflicted)
		{
			this.levels = new List<LevelProgressMemento>();
			this._conflicted = conflicted;
		}

		public bool Equals(CampaignProgressMemento other)
		{
			if (!EqualityComparer<List<LevelProgressMemento>>.Default.Equals(this.Levels, other.Levels))
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is CampaignProgressMemento))
			{
				return false;
			}
			return this.Equals((CampaignProgressMemento)obj);
		}

		public override int GetHashCode()
		{
			return this.Levels.GetHashCode();
		}

		internal Dictionary<string, LevelProgressMemento> GetLevelsAsDictionary()
		{
			LevelProgressMemento levelProgressMemento;
			Dictionary<string, LevelProgressMemento> strs = new Dictionary<string, LevelProgressMemento>(this.Levels.Count);
			foreach (LevelProgressMemento level in this.Levels)
			{
				if (!strs.TryGetValue(level.LevelId, out levelProgressMemento))
				{
					strs.Add(level.LevelId, level);
				}
				else
				{
					strs[levelProgressMemento.LevelId] = LevelProgressMemento.Merge(level, levelProgressMemento);
				}
			}
			return strs;
		}

		internal static CampaignProgressMemento Merge(CampaignProgressMemento left, CampaignProgressMemento right)
		{
			LevelProgressMemento levelProgressMemento;
			Dictionary<string, LevelProgressMemento> strs = new Dictionary<string, LevelProgressMemento>();
			IEnumerable<LevelProgressMemento> levelProgressMementos = 
				from l in left.Levels.Concat<LevelProgressMemento>(right.Levels)
				where l != null
				select l;
			IEnumerator<LevelProgressMemento> enumerator = levelProgressMementos.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					LevelProgressMemento current = enumerator.Current;
					if (!strs.TryGetValue(current.LevelId, out levelProgressMemento))
					{
						strs.Add(current.LevelId, current);
					}
					else
					{
						strs[current.LevelId] = LevelProgressMemento.Merge(levelProgressMemento, current);
					}
				}
			}
			finally
			{
				if (enumerator == null)
				{
				}
				enumerator.Dispose();
			}
			CampaignProgressMemento campaignProgressMemento = new CampaignProgressMemento((left.Conflicted ? true : right.Conflicted));
			campaignProgressMemento.Levels.AddRange(strs.Values);
			return campaignProgressMemento;
		}

		internal void SetConflicted()
		{
			this._conflicted = true;
		}

		public override string ToString()
		{
			string[] array = (
				from l in this.Levels
				select string.Concat('\"', l.LevelId, '\"')).ToArray<string>();
			return string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[] { string.Join(",", array) });
		}
	}
}