using System.Reflection;

namespace System
{
	public static class TypeExtensions
	{
		public static System.Type AsType(this System.Type type)
		{
			return type;
		}

		public static TypeInfo GetTypeInfo(this System.Type type)
		{
			return TypeInfo.FromType(type);
		}
	}
}
