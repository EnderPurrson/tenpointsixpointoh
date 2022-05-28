using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public struct TrophiesMemento : IEquatable<TrophiesMemento>
	{
		private readonly bool _conflicted;

		[SerializeField]
		private int trophiesNegative;

		[SerializeField]
		private int trophiesPositive;

		public bool Conflicted
		{
			get
			{
				return _conflicted;
			}
		}

		public int TrophiesNegative
		{
			get
			{
				return trophiesNegative;
			}
		}

		public int TrophiesPositive
		{
			get
			{
				return trophiesPositive;
			}
		}

		public int Trophies
		{
			get
			{
				return trophiesPositive - trophiesNegative;
			}
		}

		public TrophiesMemento(int trophiesNegative, int trophiesPositive)
			: this(trophiesNegative, trophiesPositive, false)
		{
		}

		public TrophiesMemento(int trophiesNegative, int trophiesPositive, bool conflicted)
		{
			_conflicted = conflicted;
			this.trophiesNegative = trophiesNegative;
			this.trophiesPositive = trophiesPositive;
		}

		internal static TrophiesMemento Merge(TrophiesMemento left, TrophiesMemento right)
		{
			int num = Math.Max(left.TrophiesNegative, right.TrophiesNegative);
			int num2 = Math.Max(left.TrophiesPositive, right.TrophiesPositive);
			bool conflicted = left.Conflicted || right.Conflicted;
			return new TrophiesMemento(num, num2, conflicted);
		}

		public bool Equals(TrophiesMemento other)
		{
			if (TrophiesNegative != other.TrophiesNegative)
			{
				return false;
			}
			if (TrophiesPositive != other.TrophiesPositive)
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TrophiesMemento))
			{
				return false;
			}
			TrophiesMemento other = (TrophiesMemento)obj;
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return TrophiesNegative.GetHashCode() ^ TrophiesPositive.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ \"negative\":{0},\"positive\":{1} }}", trophiesNegative, trophiesPositive);
		}
	}
}
