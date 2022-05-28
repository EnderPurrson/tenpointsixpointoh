using System;
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

		[CompilerGenerated]
		private static Func<LevelProgressMemento, string> _003C_003Ef__am_0024cache2;

		[CompilerGenerated]
		private static Func<LevelProgressMemento, bool> _003C_003Ef__am_0024cache3;

		internal bool Conflicted
		{
			get
			{
				return _conflicted;
			}
		}

		internal List<LevelProgressMemento> Levels
		{
			get
			{
				if (levels == null)
				{
					levels = new List<LevelProgressMemento>();
				}
				return levels;
			}
		}

		internal CampaignProgressMemento(bool conflicted)
		{
			levels = new List<LevelProgressMemento>();
			_conflicted = conflicted;
		}

		internal Dictionary<string, LevelProgressMemento> GetLevelsAsDictionary()
		{
			Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>(Levels.Count);
			foreach (LevelProgressMemento level in Levels)
			{
				LevelProgressMemento value;
				if (dictionary.TryGetValue(level.LevelId, out value))
				{
					dictionary[value.LevelId] = LevelProgressMemento.Merge(level, value);
				}
				else
				{
					dictionary.Add(level.LevelId, level);
				}
			}
			return dictionary;
		}

		internal void SetConflicted()
		{
			_conflicted = true;
		}

		public bool Equals(CampaignProgressMemento other)
		{
			EqualityComparer<List<LevelProgressMemento>> @default = EqualityComparer<List<LevelProgressMemento>>.Default;
			if (!@default.Equals(Levels, other.Levels))
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
			CampaignProgressMemento other = (CampaignProgressMemento)obj;
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return Levels.GetHashCode();
		}

		public override string ToString()
		{
			List<LevelProgressMemento> source = Levels;
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = _003CToString_003Em__573;
			}
			string[] value = source.Select(_003C_003Ef__am_0024cache2).ToArray();
			return string.Format(CultureInfo.InvariantCulture, "[{0}]", string.Join(",", value));
		}

		internal static CampaignProgressMemento Merge(CampaignProgressMemento left, CampaignProgressMemento right)
		{
			Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>();
			IEnumerable<LevelProgressMemento> source = left.Levels.Concat(right.Levels);
			if (_003C_003Ef__am_0024cache3 == null)
			{
				_003C_003Ef__am_0024cache3 = _003CMerge_003Em__574;
			}
			IEnumerable<LevelProgressMemento> enumerable = source.Where(_003C_003Ef__am_0024cache3);
			foreach (LevelProgressMemento item in enumerable)
			{
				LevelProgressMemento value;
				if (dictionary.TryGetValue(item.LevelId, out value))
				{
					dictionary[item.LevelId] = LevelProgressMemento.Merge(value, item);
				}
				else
				{
					dictionary.Add(item.LevelId, item);
				}
			}
			bool conflicted = left.Conflicted || right.Conflicted;
			CampaignProgressMemento result = new CampaignProgressMemento(conflicted);
			result.Levels.AddRange(dictionary.Values);
			return result;
		}

		[CompilerGenerated]
		private static string _003CToString_003Em__573(LevelProgressMemento l)
		{
			return '"' + l.LevelId + '"';
		}

		[CompilerGenerated]
		private static bool _003CMerge_003Em__574(LevelProgressMemento l)
		{
			return l != null;
		}
	}
}
