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
				return this._conflicted;
			}
		}

		public int Trophies
		{
			get
			{
				return this.trophiesPositive - this.trophiesNegative;
			}
		}

		public int TrophiesNegative
		{
			get
			{
				return this.trophiesNegative;
			}
		}

		public int TrophiesPositive
		{
			get
			{
				return this.trophiesPositive;
			}
		}

		public TrophiesMemento(int trophiesNegative, int trophiesPositive) : this(trophiesNegative, trophiesPositive, false)
		{
		}

		public TrophiesMemento(int trophiesNegative, int trophiesPositive, bool conflicted)
		{
			this._conflicted = conflicted;
			this.trophiesNegative = trophiesNegative;
			this.trophiesPositive = trophiesPositive;
		}

		public bool Equals(TrophiesMemento other)
		{
			if (this.TrophiesNegative != other.TrophiesNegative)
			{
				return false;
			}
			if (this.TrophiesPositive != other.TrophiesPositive)
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
			return this.Equals((TrophiesMemento)obj);
		}

		public override int GetHashCode()
		{
			return this.TrophiesNegative.GetHashCode() ^ this.TrophiesPositive.GetHashCode();
		}

		internal static TrophiesMemento Merge(TrophiesMemento left, TrophiesMemento right)
		{
			int num = Math.Max(left.TrophiesNegative, right.TrophiesNegative);
			int num1 = Math.Max(left.TrophiesPositive, right.TrophiesPositive);
			return new TrophiesMemento(num, num1, (left.Conflicted ? true : right.Conflicted));
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ \"negative\":{0},\"positive\":{1} }}", new object[] { this.trophiesNegative, this.trophiesPositive });
		}
	}
}