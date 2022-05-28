using System;

namespace ExitGames.Client.Photon
{
	internal class CustomType
	{
		public readonly byte Code;

		public readonly global::System.Type Type;

		public readonly SerializeMethod SerializeFunction;

		public readonly DeserializeMethod DeserializeFunction;

		public readonly SerializeStreamMethod SerializeStreamFunction;

		public readonly DeserializeStreamMethod DeserializeStreamFunction;

		public CustomType(global::System.Type type, byte code, SerializeMethod serializeFunction, DeserializeMethod deserializeFunction)
		{
			Type = type;
			Code = code;
			SerializeFunction = serializeFunction;
			DeserializeFunction = deserializeFunction;
		}

		public CustomType(global::System.Type type, byte code, SerializeStreamMethod serializeFunction, DeserializeStreamMethod deserializeFunction)
		{
			Type = type;
			Code = code;
			SerializeStreamFunction = serializeFunction;
			DeserializeStreamFunction = deserializeFunction;
		}
	}
}
