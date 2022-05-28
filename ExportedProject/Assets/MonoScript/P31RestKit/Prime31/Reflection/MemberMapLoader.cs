using System;

namespace Prime31.Reflection
{
	public delegate void MemberMapLoader(global::System.Type type, SafeDictionary<string, CacheResolver.MemberMap> memberMaps);
}
