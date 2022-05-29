using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PersistentPreferences : Preferences
	{
		private const string KeyElement = "Key";

		private const string PreferenceElement = "Preference";

		private const string RootElement = "Preferences";

		private const string ValueElement = "Value";

		private readonly XDocument _doc;

		private readonly static string _path;

		public override int Count
		{
			get
			{
				return this._doc.Root.Elements().Count<XElement>();
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public override ICollection<string> Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		internal static string Path
		{
			get
			{
				return PersistentPreferences._path;
			}
		}

		public override ICollection<string> Values
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		static PersistentPreferences()
		{
			PersistentPreferences._path = System.IO.Path.Combine(Application.persistentDataPath, "com.P3D.Pixlgun.Settings.xml");
		}

		public PersistentPreferences()
		{
			try
			{
				this._doc = XDocument.Load(PersistentPreferences._path);
			}
			catch (Exception exception)
			{
				this._doc = new XDocument(new object[] { new XElement("Preferences") });
				this._doc.Save(PersistentPreferences._path);
			}
		}

		protected override void AddCore(string key, string value)
		{
			XElement xElement = this._doc.Root.Elements("Preference").FirstOrDefault<XElement>((XElement e) => (e.Element("Key") == null ? false : e.Element("Key").Value.Equals(key)));
			if (xElement != null)
			{
				xElement.Remove();
			}
			XElement xElement1 = new XElement("Preference", new object[] { new XElement("Key", key), new XElement("Value", value) });
			this._doc.Root.Add(xElement1);
			this._doc.Save(PersistentPreferences._path);
		}

		public override void Clear()
		{
			this._doc.Root.RemoveNodes();
			this._doc.Save(PersistentPreferences._path);
		}

		protected override bool ContainsKeyCore(string key)
		{
			return this._doc.Root.Elements("Preference").Any<XElement>((XElement e) => (e.Element("Key") == null ? false : e.Element("Key").Value.Equals(key)));
		}

		protected override void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			throw new NotSupportedException();
		}

		protected override bool RemoveCore(string key)
		{
			XElement xElement = this._doc.Root.Elements("Preference").FirstOrDefault<XElement>((XElement e) => (e.Element("Key") == null ? false : e.Element("Key").Value.Equals(key)));
			if (xElement == null)
			{
				return false;
			}
			xElement.Remove();
			this._doc.Save(PersistentPreferences._path);
			return true;
		}

		public override void Save()
		{
			this._doc.Save(PersistentPreferences._path);
		}

		protected override bool TryGetValueCore(string key, out string value)
		{
			XElement xElement = this._doc.Root.Elements("Preference").FirstOrDefault<XElement>((XElement e) => (e.Element("Key") == null ? false : e.Element("Key").Value.Equals(key)));
			if (xElement != null)
			{
				XElement xElement1 = xElement.Element("Value");
				if (xElement1 != null)
				{
					value = xElement1.Value;
					return true;
				}
			}
			value = null;
			return false;
		}
	}
}