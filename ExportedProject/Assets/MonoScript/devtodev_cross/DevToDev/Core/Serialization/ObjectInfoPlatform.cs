using System.Reflection;

namespace DevToDev.Core.Serialization
{
	public class ObjectInfoPlatform
	{
		public static FieldInfo GetFieldInfo(object saveableObject, string fieldName)
		{
			return saveableObject.GetType().GetField(fieldName, (BindingFlags)24);
		}

		public static bool IsPrimitive(object data)
		{
			return data.GetType().get_IsPrimitive();
		}
	}
}
