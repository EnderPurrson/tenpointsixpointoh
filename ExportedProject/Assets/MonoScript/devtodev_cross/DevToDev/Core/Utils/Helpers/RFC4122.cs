using System;
using DevToDev.Core.Serialization;

namespace DevToDev.Core.Utils.Helpers
{
	[Serializable]
	internal class RFC4122 : IStorageable<RFC4122>
	{
		private static RFC4122 rfc4122;

		private string guid;

		public string GUID
		{
			get
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				if (guid == null)
				{
					Guid val = Guid.NewGuid();
					guid = ((object)(Guid)(ref val)).ToString();
					Save(this);
				}
				return guid;
			}
		}

		public override string StorageName()
		{
			return "guid.dat";
		}

		public override ISaveable GetBlankObject()
		{
			return new RFC4122();
		}

		public override ISaveable GetObject(byte[] data)
		{
			return new Formatter<RFC4122>().Load(data);
		}

		public override byte[] SaveObject(ISaveable obj)
		{
			return new Formatter<RFC4122>().Save(obj as RFC4122);
		}

		public static string Get()
		{
			if (rfc4122 == null)
			{
				rfc4122 = new RFC4122().Load() as RFC4122;
			}
			return rfc4122.GUID;
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("guid", guid);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public RFC4122()
		{
		}

		public RFC4122(ObjectInfo info)
		{
			try
			{
				guid = info.GetValue("guid", typeof(string)) as string;
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}
	}
}
