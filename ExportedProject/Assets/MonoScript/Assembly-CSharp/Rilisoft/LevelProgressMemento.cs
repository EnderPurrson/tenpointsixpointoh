using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	internal sealed class LevelProgressMemento : IEquatable<LevelProgressMemento>
	{
		[SerializeField]
		private string levelId;

		[SerializeField]
		private int coinCount;

		[SerializeField]
		private int gemCount;

		[SerializeField]
		private int starCount;

		internal int CoinCount
		{
			get
			{
				return this.coinCount;
			}
			set
			{
				this.coinCount = value;
			}
		}

		internal int GemCount
		{
			get
			{
				return this.gemCount;
			}
			set
			{
				this.gemCount = value;
			}
		}

		internal string LevelId
		{
			get
			{
				if (this.levelId == null)
				{
					this.levelId = string.Empty;
				}
				return this.levelId;
			}
			set
			{
				this.levelId = value ?? string.Empty;
			}
		}

		internal int StarCount
		{
			get
			{
				return this.starCount;
			}
			set
			{
				this.starCount = value;
			}
		}

		public LevelProgressMemento(string levelId)
		{
			if (levelId == null)
			{
				throw new ArgumentNullException("levelId");
			}
			this.levelId = levelId;
		}

		public bool Equals(LevelProgressMemento other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.CoinCount != other.CoinCount)
			{
				return false;
			}
			if (this.GemCount != other.GemCount)
			{
				return false;
			}
			if (this.StarCount != other.StarCount)
			{
				return false;
			}
			if (this.LevelId != other.LevelId)
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			LevelProgressMemento levelProgressMemento = obj as LevelProgressMemento;
			if (object.ReferenceEquals(levelProgressMemento, null))
			{
				return false;
			}
			return this.Equals(levelProgressMemento);
		}

		public override int GetHashCode()
		{
			int hashCode = this.LevelId.GetHashCode() ^ this.CoinCount.GetHashCode();
			int gemCount = this.GemCount;
			return hashCode ^ gemCount.GetHashCode() ^ this.StarCount.GetHashCode();
		}

		internal static LevelProgressMemento Merge(LevelProgressMemento left, LevelProgressMemento right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			if (left.LevelId != right.LevelId)
			{
				throw new ArgumentException("Level ids shoud be equal.");
			}
			LevelProgressMemento levelProgressMemento = new LevelProgressMemento(left.LevelId)
			{
				CoinCount = Math.Max(left.CoinCount, right.CoinCount),
				GemCount = Math.Max(left.GemCount, right.GemCount),
				StarCount = Math.Max(left.StarCount, right.StarCount)
			};
			return levelProgressMemento;
		}

		public override string ToString()
		{
			return JsonUtility.ToJson(this);
		}
	}
}