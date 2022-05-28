using System;

namespace DevToDev.Core.Serialization
{
	public abstract class ISaveable
	{
		public static int SerialVersionId = 1;

		public abstract void GetObjectData(ObjectInfo info);

		public ISaveable()
		{
		}

		public ISaveable(ObjectInfo info)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			throw new NotImplementedException("Override this method in your storage");
		}
	}
}
