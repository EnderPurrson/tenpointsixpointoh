using System;

namespace Rilisoft
{
	public class SaltedFloat
	{
		private float[] _values = new float[5];

		private int _index;

		public float @value
		{
			get
			{
				return this._values[this._index];
			}
			set
			{
				SaltedFloat saltedFloat = this;
				int num = saltedFloat._index + 1;
				int num1 = num;
				saltedFloat._index = num;
				if (num1 >= (int)this._values.Length)
				{
					this._index = 0;
				}
				this._values[this._index] = value;
			}
		}

		public SaltedFloat(float value)
		{
			this.@value = value;
		}

		public SaltedFloat() : this(0f)
		{
		}
	}
}