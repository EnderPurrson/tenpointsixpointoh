using System;
using System.Collections.Generic;

namespace Rilisoft
{
	internal abstract class SignedPreferences : Preferences
	{
		private readonly Preferences _backPreferences;

		protected Preferences BackPreferences
		{
			get
			{
				return this._backPreferences;
			}
		}

		public override int Count
		{
			get
			{
				return this._backPreferences.Count;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return this._backPreferences.IsReadOnly;
			}
		}

		public override ICollection<string> Keys
		{
			get
			{
				return this._backPreferences.Keys;
			}
		}

		public override ICollection<string> Values
		{
			get
			{
				return this._backPreferences.Values;
			}
		}

		protected SignedPreferences(Preferences backPreferences)
		{
			this._backPreferences = backPreferences;
		}

		protected override void AddCore(string key, string value)
		{
			this.AddSignedCore(key, value);
		}

		protected abstract void AddSignedCore(string key, string value);

		public override void Clear()
		{
			this._backPreferences.Clear();
		}

		protected override bool ContainsKeyCore(string key)
		{
			return this._backPreferences.ContainsKey(key);
		}

		protected override void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			this._backPreferences.CopyTo(array, arrayIndex);
		}

		public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this._backPreferences.GetEnumerator();
		}

		protected override bool RemoveCore(string key)
		{
			return this.RemoveSignedCore(key);
		}

		protected abstract bool RemoveSignedCore(string key);

		public override void Save()
		{
			this._backPreferences.Save();
		}

		protected override bool TryGetValueCore(string key, out string value)
		{
			return this._backPreferences.TryGetValue(key, out value);
		}

		public bool Verify(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.VerifyCore(key);
		}

		protected abstract bool VerifyCore(string key);
	}
}