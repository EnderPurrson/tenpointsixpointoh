using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public struct SkinsMemento : IEquatable<SkinsMemento>
	{
		private readonly bool _conflicted;

		[SerializeField]
		private CapeMemento cape;

		[SerializeField]
		private List<SkinMemento> skins;

		[SerializeField]
		private List<string> deletedSkins;

		public CapeMemento Cape
		{
			get
			{
				return this.cape;
			}
		}

		public bool Conflicted
		{
			get
			{
				return this._conflicted;
			}
		}

		public List<string> DeletedSkins
		{
			get
			{
				if (this.deletedSkins == null)
				{
					this.deletedSkins = new List<string>();
				}
				return this.deletedSkins;
			}
		}

		public List<SkinMemento> Skins
		{
			get
			{
				if (this.skins == null)
				{
					this.skins = new List<SkinMemento>();
				}
				return this.skins;
			}
		}

		public SkinsMemento(IEnumerable<SkinMemento> skins, IEnumerable<string> deletedSkins, CapeMemento cape) : this(skins, deletedSkins, cape, false)
		{
		}

		public SkinsMemento(IEnumerable<SkinMemento> skins, IEnumerable<string> deletedSkins, CapeMemento cape, bool conflicted)
		{
			this.skins = (skins != null ? skins.ToList<SkinMemento>() : new List<SkinMemento>());
			this.deletedSkins = (deletedSkins != null ? deletedSkins.ToList<string>() : new List<string>());
			this.cape = cape;
			this._conflicted = conflicted;
		}

		public bool Equals(SkinsMemento other)
		{
			if (!this.Cape.Equals(other.Cape))
			{
				return false;
			}
			if (!this.Skins.SequenceEqual<SkinMemento>(other.Skins))
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SkinsMemento))
			{
				return false;
			}
			return this.Equals((SkinsMemento)obj);
		}

		public override int GetHashCode()
		{
			CapeMemento cape = this.Cape;
			return cape.GetHashCode() ^ this.Skins.GetHashCode() ^ this.DeletedSkins.GetHashCode();
		}

		public Dictionary<string, SkinMemento> GetSkinsAsDictionary()
		{
			Dictionary<string, SkinMemento> strs = new Dictionary<string, SkinMemento>(this.Skins.Count);
			foreach (SkinMemento skin in this.Skins)
			{
				strs[skin.Id] = skin;
			}
			return strs;
		}

		internal static SkinsMemento Merge(SkinsMemento left, SkinsMemento right)
		{
			HashSet<string> strs = new HashSet<string>(left.DeletedSkins.Concat<string>(right.DeletedSkins));
			Dictionary<string, SkinMemento> strs1 = new Dictionary<string, SkinMemento>();
			foreach (SkinMemento skin in left.Skins)
			{
				if (!strs.Contains(skin.Id))
				{
					strs1[skin.Id] = skin;
				}
			}
			foreach (SkinMemento skinMemento in right.Skins)
			{
				if (!strs.Contains(skinMemento.Id))
				{
					strs1[skinMemento.Id] = skinMemento;
				}
			}
			bool flag = (left.Conflicted ? true : right.Conflicted);
			CapeMemento capeMemento = CapeMemento.ChooseCape(left.Cape, right.Cape);
			return new SkinsMemento(strs1.Values.ToList<SkinMemento>(), strs, capeMemento, flag);
		}

		public override string ToString()
		{
			string[] array = (
				from s in this.Skins
				select string.Format("\"{0}\"", s.Name)).ToArray<string>();
			string str = string.Join(",", array);
			string str1 = string.Join(",", this.DeletedSkins.ToArray());
			return string.Format(CultureInfo.InvariantCulture, "{{ \"skins\":[{0}], \"cape\":\"{1}\", \"deletedSkins\":[{2}] }}", new object[] { str, this.cape, str1 });
		}
	}
}