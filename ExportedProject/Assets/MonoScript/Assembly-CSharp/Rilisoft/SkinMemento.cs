using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public struct SkinMemento : IEquatable<SkinMemento>
	{
		[SerializeField]
		private string id;

		[SerializeField]
		private string name;

		[SerializeField]
		private string skin;

		private int? _skinHashCode;

		public string Id
		{
			get
			{
				return this.id ?? string.Empty;
			}
		}

		public string Name
		{
			get
			{
				return this.name ?? string.Empty;
			}
		}

		public string Skin
		{
			get
			{
				return this.skin ?? string.Empty;
			}
		}

		public SkinMemento(string id, string name, string skin)
		{
			this.id = id ?? string.Empty;
			this.name = name ?? string.Empty;
			this.skin = skin ?? string.Empty;
			this._skinHashCode = null;
		}

		public bool Equals(SkinMemento other)
		{
			if (this.Id != other.Id)
			{
				return false;
			}
			if (this.Name != other.Name)
			{
				return false;
			}
			if (this.GetSkinHashCode() != other.GetSkinHashCode())
			{
				return false;
			}
			if (this.Skin != other.Skin)
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

		public override int GetHashCode()
		{
			return this.Id.GetHashCode() ^ this.Name.GetHashCode() ^ this.GetSkinHashCode();
		}

		private int GetSkinHashCode()
		{
			if (!this._skinHashCode.HasValue)
			{
				this._skinHashCode = new int?(this.Skin.GetHashCode());
			}
			return this._skinHashCode.Value;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ \"id\":{0},\"name\":{1},\"skin\":{2} }}", new object[] { this.Id, this.Name, this.Skin });
		}
	}
}