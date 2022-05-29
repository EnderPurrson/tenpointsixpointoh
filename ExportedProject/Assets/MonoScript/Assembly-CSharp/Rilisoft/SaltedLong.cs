using System;

namespace Rilisoft
{
	public struct SaltedLong
	{
		private readonly long _salt;

		private long _saltedValue;

		public long Value
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

		public SaltedLong(long salt, long value)
		{
			this._salt = salt;
			this._saltedValue = salt ^ value;
		}

		public SaltedLong(long salt) : this(salt, (long)0)
		{
		}
	}
}