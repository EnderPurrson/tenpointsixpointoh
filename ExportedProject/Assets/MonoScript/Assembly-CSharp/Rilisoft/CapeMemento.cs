using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public struct CapeMemento : IEquatable<CapeMemento>
	{
		[SerializeField]
		private long id;

		[SerializeField]
		private string cape;

		private int? _capeHashCode;

		public string Cape
		{
			get
			{
				return this.cape ?? string.Empty;
			}
		}

		public long Id
		{
			get
			{
				return this.id;
			}
		}

		public CapeMemento(long id, string cape)
		{
			this.id = id;
			this.cape = cape ?? string.Empty;
			this._capeHashCode = null;
		}

		internal static CapeMemento ChooseCape(CapeMemento left, CapeMemento right)
		{
			if (string.IsNullOrEmpty(left.Cape) && string.IsNullOrEmpty(right.Cape))
			{
				return (left.Id > right.Id ? left : right);
			}
			if (string.IsNullOrEmpty(left.Cape) || string.IsNullOrEmpty(right.Cape))
			{
				if (!string.IsNullOrEmpty(left.Cape))
				{
					return left;
				}
				return right;
			}
			return (left.Id > right.Id ? left : right);
		}

		public bool Equals(CapeMemento other)
		{
			if (this.Id != other.Id)
			{
				return false;
			}
			if (this.GetCapeHashCode() != other.GetCapeHashCode())
			{
				return false;
			}
			if (this.Cape != other.Cape)
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SkinMemento))
			{
				return false;
			}
			return this.Equals((SkinMemento)obj);
		}

		private int GetCapeHashCode()
		{
			if (!this._capeHashCode.HasValue)
			{
				this._capeHashCode = new int?(this.Cape.GetHashCode());
			}
			return this._capeHashCode.Value;
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode() ^ this.GetCapeHashCode();
		}

		public override string ToString()
		{
			string str = (this.Cape.Length > 4 ? this.Cape.Substring(this.Cape.Length - 4) : this.Cape);
			return string.Format(CultureInfo.InvariantCulture, "{{ \"id\":{0},\"cape\":\"{1}\" }}", new object[] { this.Id, str });
		}
	}
}