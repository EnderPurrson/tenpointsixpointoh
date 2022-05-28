using System;
using System.Runtime.CompilerServices;
using DevToDev.Core.Serialization;

namespace DevToDev.Cheat
{
	internal class CheatData : IStorageable<CheatData>
	{
		public long LocalTime
		{
			[CompilerGenerated]
			get
			{
				return _003CLocalTime_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLocalTime_003Ek__BackingField = value;
			}
		}

		public override string StorageName()
		{
			return "cheat.dat";
		}

		public override ISaveable GetBlankObject()
		{
			return new CheatData();
		}

		public override ISaveable GetObject(byte[] data)
		{
			return new Formatter<CheatData>().Load(data);
		}

		public override byte[] SaveObject(ISaveable obj)
		{
			return new Formatter<CheatData>().Save(obj as CheatData);
		}

		public CheatData()
		{
		}

		public CheatData(ObjectInfo info)
		{
			try
			{
				LocalTime = (long)info.GetValue("LocalTime", typeof(long));
			}
			catch (global::System.Exception)
			{
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("LocalTime", LocalTime);
			}
			catch (global::System.Exception)
			{
			}
		}
	}
}
