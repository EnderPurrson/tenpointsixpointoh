using System;

namespace Prime31
{
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class P31DeserializeableFieldAttribute : global::System.Attribute
	{
		public readonly string key;

		public readonly bool isCollection;

		public global::System.Type type;

		public P31DeserializeableFieldAttribute(string key)
		{
			this.key = key;
		}

		public P31DeserializeableFieldAttribute(string key, global::System.Type type)
			: this(key)
		{
			this.type = type;
		}

		public P31DeserializeableFieldAttribute(string key, global::System.Type type, bool isCollection)
			: this(key, type)
		{
			this.isCollection = isCollection;
		}
	}
}
