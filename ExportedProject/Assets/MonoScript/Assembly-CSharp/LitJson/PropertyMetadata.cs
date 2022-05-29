using System;
using System.Reflection;

namespace LitJson
{
	internal struct PropertyMetadata
	{
		public MemberInfo Info;

		public bool IsField;

		public System.Type Type;
	}
}