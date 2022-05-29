using System;

namespace Rilisoft
{
	public struct SaltedInt : IEquatable<SaltedInt>
	{
		private readonly static Random s_prng;

		private readonly int _salt;

		private int _saltedValue;

		public int Value
		{
			get
			{
				return this._salt ^ this._saltedValue;
			}
			set
			{
				this._saltedValue = this._salt ^ value;
			}
		}

		static SaltedInt()
		{
			SaltedInt.s_prng = new Random();
		}

		public SaltedInt(int salt, int value)
		{
			this._salt = salt;
			this._saltedValue = salt ^ value;
		}

		public SaltedInt(int salt) : this(salt, 0)
		{
		}

		public bool Equals(SaltedInt other)
		{
			return this.Value == other.Value;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SaltedInt))
			{
				return false;
			}
			return this.Equals((SaltedInt)obj);
		}

		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		public static implicit operator SaltedInt(int i)
		{
			return new SaltedInt(SaltedInt.s_prng.Next(), i);
		}
	}
}