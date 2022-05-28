using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ExitGames.Client.Photon
{
	public class Protocol16 : IProtocol
	{
		public enum GpType : byte
		{
			Unknown = 0,
			Array = 121,
			Boolean = 111,
			Byte = 98,
			ByteArray = 120,
			ObjectArray = 122,
			Short = 107,
			Float = 102,
			Dictionary = 68,
			Double = 100,
			Hashtable = 104,
			Integer = 105,
			IntegerArray = 110,
			Long = 108,
			String = 115,
			StringArray = 97,
			Custom = 99,
			Null = 42,
			EventData = 101,
			OperationRequest = 113,
			OperationResponse = 112
		}

		private readonly byte[] versionBytes = new byte[2] { 1, 6 };

		private readonly byte[] memShort = new byte[2];

		private readonly long[] memLongBlock = new long[1];

		private readonly byte[] memLongBlockBytes = new byte[8];

		private static readonly float[] memFloatBlock = new float[1];

		private static readonly byte[] memFloatBlockBytes = new byte[4];

		private readonly double[] memDoubleBlock = new double[1];

		private readonly byte[] memDoubleBlockBytes = new byte[8];

		private readonly byte[] memInteger = new byte[4];

		private readonly byte[] memLong = new byte[8];

		private readonly byte[] memFloat = new byte[4];

		private readonly byte[] memDouble = new byte[8];

		internal override string protocolType
		{
			get
			{
				return "GpBinaryV16";
			}
		}

		internal override byte[] VersionBytes
		{
			get
			{
				return versionBytes;
			}
		}

		private bool SerializeCustom(StreamBuffer dout, object serObject)
		{
			CustomType customType = default(CustomType);
			if (Protocol.TypeDict.TryGetValue(serObject.GetType(), ref customType))
			{
				if (customType.SerializeStreamFunction == null)
				{
					byte[] array = customType.SerializeFunction(serObject);
					((Stream)dout).WriteByte((byte)99);
					((Stream)dout).WriteByte(customType.Code);
					SerializeShort(dout, (short)array.Length, false);
					((Stream)dout).Write(array, 0, array.Length);
					return true;
				}
				((Stream)dout).WriteByte((byte)99);
				((Stream)dout).WriteByte(customType.Code);
				long position = ((Stream)dout).get_Position();
				((Stream)dout).set_Position(((Stream)dout).get_Position() + 2);
				short num = customType.SerializeStreamFunction(dout, serObject);
				long position2 = ((Stream)dout).get_Position();
				((Stream)dout).set_Position(position);
				SerializeShort(dout, num, false);
				((Stream)dout).set_Position(((Stream)dout).get_Position() + num);
				if (((Stream)dout).get_Position() != position2)
				{
					throw new global::System.Exception(string.Concat(new object[6]
					{
						"Serialization failed. Stream position corrupted. Should be ",
						position2,
						" is now: ",
						((Stream)dout).get_Position(),
						" serializedLength: ",
						num
					}));
				}
				return true;
			}
			return false;
		}

		private object DeserializeCustom(StreamBuffer din, byte customTypeCode)
		{
			short num = DeserializeShort(din);
			CustomType customType = default(CustomType);
			if (Protocol.CodeDict.TryGetValue(customTypeCode, ref customType))
			{
				if (customType.DeserializeStreamFunction == null)
				{
					byte[] array = new byte[num];
					((Stream)din).Read(array, 0, (int)num);
					return customType.DeserializeFunction(array);
				}
				long position = ((Stream)din).get_Position();
				object result = customType.DeserializeStreamFunction(din, num);
				int num2 = (int)(((Stream)din).get_Position() - position);
				if (num2 != num)
				{
					((Stream)din).set_Position(position + num);
				}
				return result;
			}
			return null;
		}

		private global::System.Type GetTypeOfCode(byte typeCode)
		{
			switch (typeCode)
			{
			case 105:
				return typeof(int);
			case 115:
				return typeof(string);
			case 97:
				return typeof(string[]);
			case 120:
				return typeof(byte[]);
			case 110:
				return typeof(int[]);
			case 104:
				return typeof(Hashtable);
			case 68:
				return typeof(IDictionary);
			case 111:
				return typeof(bool);
			case 107:
				return typeof(short);
			case 108:
				return typeof(long);
			case 98:
				return typeof(byte);
			case 102:
				return typeof(float);
			case 100:
				return typeof(double);
			case 121:
				return typeof(global::System.Array);
			case 99:
				return typeof(CustomType);
			case 122:
				return typeof(object[]);
			case 101:
				return typeof(EventData);
			case 113:
				return typeof(OperationRequest);
			case 112:
				return typeof(OperationResponse);
			case 0:
			case 42:
				return typeof(object);
			default:
				Debug.WriteLine(string.Concat((object)"missing type: ", (object)typeCode));
				throw new global::System.Exception(string.Concat((object)"deserialize(): ", (object)typeCode));
			}
		}

		private GpType GetCodeOfType(global::System.Type type)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected I4, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Invalid comparison between Unknown and I4
			TypeCode typeCode = global::System.Type.GetTypeCode(type);
			TypeCode val = typeCode;
			switch (val - 3)
			{
			default:
				if ((int)val != 18)
				{
					break;
				}
				return GpType.String;
			case 3:
				return GpType.Byte;
			case 0:
				return GpType.Boolean;
			case 4:
				return GpType.Short;
			case 6:
				return GpType.Integer;
			case 8:
				return GpType.Long;
			case 10:
				return GpType.Float;
			case 11:
				return GpType.Double;
			case 1:
			case 2:
			case 5:
			case 7:
			case 9:
				break;
			}
			if (type.get_IsArray())
			{
				if (type == typeof(byte[]))
				{
					return GpType.ByteArray;
				}
				return GpType.Array;
			}
			if (type == typeof(Hashtable))
			{
				return GpType.Hashtable;
			}
			if (type.get_IsGenericType() && typeof(Dictionary<, >) == type.GetGenericTypeDefinition())
			{
				return GpType.Dictionary;
			}
			if (type == typeof(EventData))
			{
				return GpType.EventData;
			}
			if (type == typeof(OperationRequest))
			{
				return GpType.OperationRequest;
			}
			if (type == typeof(OperationResponse))
			{
				return GpType.OperationResponse;
			}
			return GpType.Unknown;
		}

		private global::System.Array CreateArrayByType(byte arrayType, short length)
		{
			return global::System.Array.CreateInstance(GetTypeOfCode(arrayType), (int)length);
		}

		private void SerializeOperationRequest(StreamBuffer stream, OperationRequest serObject, bool setType)
		{
			SerializeOperationRequest(stream, serObject.OperationCode, serObject.Parameters, setType);
		}

		public override void SerializeOperationRequest(StreamBuffer stream, byte operationCode, Dictionary<byte, object> parameters, bool setType)
		{
			if (setType)
			{
				((Stream)stream).WriteByte((byte)113);
			}
			((Stream)stream).WriteByte(operationCode);
			SerializeParameterTable(stream, parameters);
		}

		public override OperationRequest DeserializeOperationRequest(StreamBuffer din)
		{
			OperationRequest operationRequest = new OperationRequest();
			operationRequest.OperationCode = DeserializeByte(din);
			operationRequest.Parameters = DeserializeParameterTable(din);
			return operationRequest;
		}

		public override void SerializeOperationResponse(StreamBuffer stream, OperationResponse serObject, bool setType)
		{
			if (setType)
			{
				((Stream)stream).WriteByte((byte)112);
			}
			((Stream)stream).WriteByte(serObject.OperationCode);
			SerializeShort(stream, serObject.ReturnCode, false);
			if (string.IsNullOrEmpty(serObject.DebugMessage))
			{
				((Stream)stream).WriteByte((byte)42);
			}
			else
			{
				SerializeString(stream, serObject.DebugMessage, false);
			}
			SerializeParameterTable(stream, serObject.Parameters);
		}

		public override OperationResponse DeserializeOperationResponse(StreamBuffer stream)
		{
			OperationResponse operationResponse = new OperationResponse();
			operationResponse.OperationCode = DeserializeByte(stream);
			operationResponse.ReturnCode = DeserializeShort(stream);
			operationResponse.DebugMessage = Deserialize(stream, DeserializeByte(stream)) as string;
			operationResponse.Parameters = DeserializeParameterTable(stream);
			return operationResponse;
		}

		public override void SerializeEventData(StreamBuffer stream, EventData serObject, bool setType)
		{
			if (setType)
			{
				((Stream)stream).WriteByte((byte)101);
			}
			((Stream)stream).WriteByte(serObject.Code);
			SerializeParameterTable(stream, serObject.Parameters);
		}

		public override EventData DeserializeEventData(StreamBuffer din)
		{
			EventData eventData = new EventData();
			eventData.Code = DeserializeByte(din);
			eventData.Parameters = DeserializeParameterTable(din);
			return eventData;
		}

		private void SerializeParameterTable(StreamBuffer stream, Dictionary<byte, object> parameters)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (parameters == null || parameters.get_Count() == 0)
			{
				SerializeShort(stream, 0, false);
				return;
			}
			SerializeShort(stream, (short)parameters.get_Count(), false);
			Enumerator<byte, object> enumerator = parameters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<byte, object> current = enumerator.get_Current();
					((Stream)stream).WriteByte(current.get_Key());
					Serialize(stream, current.get_Value(), true);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		private Dictionary<byte, object> DeserializeParameterTable(StreamBuffer stream)
		{
			short num = DeserializeShort(stream);
			Dictionary<byte, object> val = new Dictionary<byte, object>((int)num);
			for (int i = 0; i < num; i++)
			{
				byte b = (byte)((Stream)stream).ReadByte();
				object obj = Deserialize(stream, (byte)((Stream)stream).ReadByte());
				val.set_Item(b, obj);
			}
			return val;
		}

		public override void Serialize(StreamBuffer dout, object serObject, bool setType)
		{
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			if (serObject == null)
			{
				if (setType)
				{
					((Stream)dout).WriteByte((byte)42);
				}
				return;
			}
			switch (GetCodeOfType(serObject.GetType()))
			{
			case GpType.Byte:
				SerializeByte(dout, (byte)serObject, setType);
				break;
			case GpType.String:
				SerializeString(dout, (string)serObject, setType);
				break;
			case GpType.Boolean:
				SerializeBoolean(dout, (bool)serObject, setType);
				break;
			case GpType.Short:
				SerializeShort(dout, (short)serObject, setType);
				break;
			case GpType.Integer:
				SerializeInteger(dout, (int)serObject, setType);
				break;
			case GpType.Long:
				SerializeLong(dout, (long)serObject, setType);
				break;
			case GpType.Float:
				SerializeFloat(dout, (float)serObject, setType);
				break;
			case GpType.Double:
				SerializeDouble(dout, (double)serObject, setType);
				break;
			case GpType.Hashtable:
				SerializeHashTable(dout, (Hashtable)serObject, setType);
				break;
			case GpType.ByteArray:
				SerializeByteArray(dout, (byte[])serObject, setType);
				break;
			case GpType.Array:
				if (serObject is int[])
				{
					SerializeIntArrayOptimized(dout, (int[])serObject, setType);
				}
				else if (serObject.GetType().GetElementType() == typeof(object))
				{
					SerializeObjectArray(dout, serObject as object[], setType);
				}
				else
				{
					SerializeArray(dout, (global::System.Array)serObject, setType);
				}
				break;
			case GpType.Dictionary:
				SerializeDictionary(dout, (IDictionary)serObject, setType);
				break;
			case GpType.EventData:
				SerializeEventData(dout, (EventData)serObject, setType);
				break;
			case GpType.OperationResponse:
				SerializeOperationResponse(dout, (OperationResponse)serObject, setType);
				break;
			case GpType.OperationRequest:
				SerializeOperationRequest(dout, (OperationRequest)serObject, setType);
				break;
			default:
				if (!SerializeCustom(dout, serObject))
				{
					throw new global::System.Exception(string.Concat((object)"cannot serialize(): ", (object)serObject.GetType()));
				}
				break;
			}
		}

		private void SerializeByte(StreamBuffer dout, byte serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)98);
			}
			((Stream)dout).WriteByte(serObject);
		}

		private void SerializeBoolean(StreamBuffer dout, bool serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)111);
			}
			((Stream)dout).WriteByte((byte)(serObject ? 1 : 0));
		}

		public override void SerializeShort(StreamBuffer dout, short serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)107);
			}
			lock (memShort)
			{
				byte[] array = memShort;
				array[0] = (byte)(serObject >> 8);
				array[1] = (byte)serObject;
				((Stream)dout).Write(array, 0, 2);
			}
		}

		private void SerializeInteger(StreamBuffer dout, int serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)105);
			}
			lock (memInteger)
			{
				byte[] array = memInteger;
				array[0] = (byte)(serObject >> 24);
				array[1] = (byte)(serObject >> 16);
				array[2] = (byte)(serObject >> 8);
				array[3] = (byte)serObject;
				((Stream)dout).Write(array, 0, 4);
			}
		}

		private void SerializeLong(StreamBuffer dout, long serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)108);
			}
			lock (memLongBlock)
			{
				memLongBlock[0] = serObject;
				Buffer.BlockCopy((global::System.Array)memLongBlock, 0, (global::System.Array)memLongBlockBytes, 0, 8);
				byte[] array = memLongBlockBytes;
				if (BitConverter.IsLittleEndian)
				{
					byte b = array[0];
					byte b2 = array[1];
					byte b3 = array[2];
					byte b4 = array[3];
					array[0] = array[7];
					array[1] = array[6];
					array[2] = array[5];
					array[3] = array[4];
					array[4] = b4;
					array[5] = b3;
					array[6] = b2;
					array[7] = b;
				}
				((Stream)dout).Write(array, 0, 8);
			}
		}

		private void SerializeFloat(StreamBuffer dout, float serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)102);
			}
			lock (memFloatBlockBytes)
			{
				memFloatBlock[0] = serObject;
				Buffer.BlockCopy((global::System.Array)memFloatBlock, 0, (global::System.Array)memFloatBlockBytes, 0, 4);
				if (BitConverter.IsLittleEndian)
				{
					byte b = memFloatBlockBytes[0];
					byte b2 = memFloatBlockBytes[1];
					memFloatBlockBytes[0] = memFloatBlockBytes[3];
					memFloatBlockBytes[1] = memFloatBlockBytes[2];
					memFloatBlockBytes[2] = b2;
					memFloatBlockBytes[3] = b;
				}
				((Stream)dout).Write(memFloatBlockBytes, 0, 4);
			}
		}

		private void SerializeDouble(StreamBuffer dout, double serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)100);
			}
			lock (memDoubleBlockBytes)
			{
				memDoubleBlock[0] = serObject;
				Buffer.BlockCopy((global::System.Array)memDoubleBlock, 0, (global::System.Array)memDoubleBlockBytes, 0, 8);
				byte[] array = memDoubleBlockBytes;
				if (BitConverter.IsLittleEndian)
				{
					byte b = array[0];
					byte b2 = array[1];
					byte b3 = array[2];
					byte b4 = array[3];
					array[0] = array[7];
					array[1] = array[6];
					array[2] = array[5];
					array[3] = array[4];
					array[4] = b4;
					array[5] = b3;
					array[6] = b2;
					array[7] = b;
				}
				((Stream)dout).Write(array, 0, 8);
			}
		}

		public override void SerializeString(StreamBuffer dout, string serObject, bool setType)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (setType)
			{
				((Stream)dout).WriteByte((byte)115);
			}
			byte[] bytes = Encoding.get_UTF8().GetBytes(serObject);
			if (bytes.Length > 32767)
			{
				throw new NotSupportedException(string.Concat((object)"Strings that exceed a UTF8-encoded byte-length of 32767 (short.MaxValue) are not supported. Yours is: ", (object)bytes.Length));
			}
			SerializeShort(dout, (short)bytes.Length, false);
			((Stream)dout).Write(bytes, 0, bytes.Length);
		}

		private void SerializeArray(StreamBuffer dout, global::System.Array serObject, bool setType)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			if (setType)
			{
				((Stream)dout).WriteByte((byte)121);
			}
			if (serObject.get_Length() > 32767)
			{
				throw new NotSupportedException(string.Concat((object)"String[] that exceed 32767 (short.MaxValue) entries are not supported. Yours is: ", (object)serObject.get_Length()));
			}
			SerializeShort(dout, (short)serObject.get_Length(), false);
			global::System.Type elementType = ((object)serObject).GetType().GetElementType();
			GpType codeOfType = GetCodeOfType(elementType);
			if (codeOfType != 0)
			{
				((Stream)dout).WriteByte((byte)codeOfType);
				if (codeOfType == GpType.Dictionary)
				{
					bool setKeyType;
					bool setValueType;
					SerializeDictionaryHeader(dout, serObject, out setKeyType, out setValueType);
					for (int i = 0; i < serObject.get_Length(); i++)
					{
						object value = serObject.GetValue(i);
						SerializeDictionaryElements(dout, value, setKeyType, setValueType);
					}
				}
				else
				{
					for (int j = 0; j < serObject.get_Length(); j++)
					{
						object value2 = serObject.GetValue(j);
						Serialize(dout, value2, false);
					}
				}
				return;
			}
			CustomType customType = default(CustomType);
			if (Protocol.TypeDict.TryGetValue(elementType, ref customType))
			{
				((Stream)dout).WriteByte((byte)99);
				((Stream)dout).WriteByte(customType.Code);
				for (int k = 0; k < serObject.get_Length(); k++)
				{
					object value3 = serObject.GetValue(k);
					if (customType.SerializeStreamFunction == null)
					{
						byte[] array = customType.SerializeFunction(value3);
						SerializeShort(dout, (short)array.Length, false);
						((Stream)dout).Write(array, 0, array.Length);
						continue;
					}
					long position = ((Stream)dout).get_Position();
					((Stream)dout).set_Position(((Stream)dout).get_Position() + 2);
					short num = customType.SerializeStreamFunction(dout, value3);
					long position2 = ((Stream)dout).get_Position();
					((Stream)dout).set_Position(position);
					SerializeShort(dout, num, false);
					((Stream)dout).set_Position(((Stream)dout).get_Position() + num);
					if (((Stream)dout).get_Position() != position2)
					{
						throw new global::System.Exception(string.Concat(new object[6]
						{
							"Serialization failed. Stream position corrupted. Should be ",
							position2,
							" is now: ",
							((Stream)dout).get_Position(),
							" serializedLength: ",
							num
						}));
					}
				}
				return;
			}
			throw new NotSupportedException(string.Concat((object)"cannot serialize array of type ", (object)elementType));
		}

		private void SerializeByteArray(StreamBuffer dout, byte[] serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)120);
			}
			SerializeInteger(dout, serObject.Length, false);
			((Stream)dout).Write(serObject, 0, serObject.Length);
		}

		private void SerializeIntArrayOptimized(StreamBuffer inWriter, int[] serObject, bool setType)
		{
			if (setType)
			{
				((Stream)inWriter).WriteByte((byte)121);
			}
			SerializeShort(inWriter, (short)serObject.Length, false);
			((Stream)inWriter).WriteByte((byte)105);
			byte[] array = new byte[serObject.Length * 4];
			int num = 0;
			for (int i = 0; i < serObject.Length; i++)
			{
				array[num++] = (byte)(serObject[i] >> 24);
				array[num++] = (byte)(serObject[i] >> 16);
				array[num++] = (byte)(serObject[i] >> 8);
				array[num++] = (byte)serObject[i];
			}
			((Stream)inWriter).Write(array, 0, array.Length);
		}

		private void SerializeStringArray(StreamBuffer dout, string[] serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)97);
			}
			SerializeShort(dout, (short)serObject.Length, false);
			for (int i = 0; i < serObject.Length; i++)
			{
				SerializeString(dout, serObject[i], false);
			}
		}

		private void SerializeObjectArray(StreamBuffer dout, object[] objects, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)122);
			}
			SerializeShort(dout, (short)objects.Length, false);
			foreach (object serObject in objects)
			{
				Serialize(dout, serObject, true);
			}
		}

		private void SerializeHashTable(StreamBuffer dout, Hashtable serObject, bool setType)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (setType)
			{
				((Stream)dout).WriteByte((byte)104);
			}
			SerializeShort(dout, (short)((Dictionary<object, object>)serObject).get_Count(), false);
			KeyCollection<object, object> keys = ((Dictionary<object, object>)serObject).get_Keys();
			Enumerator<object, object> enumerator = keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					Serialize(dout, current, true);
					Serialize(dout, serObject[current], true);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
		}

		private void SerializeDictionary(StreamBuffer dout, IDictionary serObject, bool setType)
		{
			if (setType)
			{
				((Stream)dout).WriteByte((byte)68);
			}
			bool setKeyType;
			bool setValueType;
			SerializeDictionaryHeader(dout, serObject, out setKeyType, out setValueType);
			SerializeDictionaryElements(dout, serObject, setKeyType, setValueType);
		}

		private void SerializeDictionaryHeader(StreamBuffer writer, global::System.Type dictType)
		{
			bool setKeyType;
			bool setValueType;
			SerializeDictionaryHeader(writer, dictType, out setKeyType, out setValueType);
		}

		private void SerializeDictionaryHeader(StreamBuffer writer, object dict, out bool setKeyType, out bool setValueType)
		{
			global::System.Type[] genericArguments = dict.GetType().GetGenericArguments();
			setKeyType = genericArguments[0] == typeof(object);
			setValueType = genericArguments[1] == typeof(object);
			if (setKeyType)
			{
				((Stream)writer).WriteByte((byte)0);
			}
			else
			{
				GpType codeOfType = GetCodeOfType(genericArguments[0]);
				if (codeOfType == GpType.Unknown || codeOfType == GpType.Dictionary)
				{
					throw new global::System.Exception(string.Concat((object)"Unexpected - cannot serialize Dictionary with key type: ", (object)genericArguments[0]));
				}
				((Stream)writer).WriteByte((byte)codeOfType);
			}
			if (setValueType)
			{
				((Stream)writer).WriteByte((byte)0);
				return;
			}
			GpType codeOfType2 = GetCodeOfType(genericArguments[1]);
			if (codeOfType2 == GpType.Unknown)
			{
				throw new global::System.Exception(string.Concat((object)"Unexpected - cannot serialize Dictionary with value type: ", (object)genericArguments[0]));
			}
			((Stream)writer).WriteByte((byte)codeOfType2);
			if (codeOfType2 == GpType.Dictionary)
			{
				SerializeDictionaryHeader(writer, genericArguments[1]);
			}
		}

		private void SerializeDictionaryElements(StreamBuffer writer, object dict, bool setKeyType, bool setValueType)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			IDictionary val = (IDictionary)dict;
			SerializeShort(writer, (short)((global::System.Collections.ICollection)val).get_Count(), false);
			IDictionaryEnumerator enumerator = val.GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					DictionaryEntry val2 = (DictionaryEntry)((global::System.Collections.IEnumerator)enumerator).get_Current();
					if (!setValueType && ((DictionaryEntry)(ref val2)).get_Value() == null)
					{
						throw new global::System.Exception("Can't serialize null in Dictionary with specific value-type.");
					}
					if (!setKeyType && ((DictionaryEntry)(ref val2)).get_Key() == null)
					{
						throw new global::System.Exception("Can't serialize null in Dictionary with specific key-type.");
					}
					Serialize(writer, ((DictionaryEntry)(ref val2)).get_Key(), setKeyType);
					Serialize(writer, ((DictionaryEntry)(ref val2)).get_Value(), setValueType);
				}
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public override object Deserialize(StreamBuffer din, byte type)
		{
			switch (type)
			{
			case 105:
				return DeserializeInteger(din);
			case 115:
				return DeserializeString(din);
			case 97:
				return DeserializeStringArray(din);
			case 120:
				return DeserializeByteArray(din);
			case 110:
				return DeserializeIntArray(din);
			case 104:
				return DeserializeHashTable(din);
			case 68:
				return DeserializeDictionary(din);
			case 111:
				return DeserializeBoolean(din);
			case 107:
				return DeserializeShort(din);
			case 108:
				return DeserializeLong(din);
			case 98:
				return DeserializeByte(din);
			case 102:
				return DeserializeFloat(din);
			case 100:
				return DeserializeDouble(din);
			case 121:
				return DeserializeArray(din);
			case 99:
			{
				byte customTypeCode = (byte)((Stream)din).ReadByte();
				return DeserializeCustom(din, customTypeCode);
			}
			case 122:
				return DeserializeObjectArray(din);
			case 101:
				return DeserializeEventData(din);
			case 113:
				return DeserializeOperationRequest(din);
			case 112:
				return DeserializeOperationResponse(din);
			case 0:
			case 42:
				return null;
			default:
				Debug.WriteLine(string.Concat((object)"missing type: ", (object)type));
				throw new global::System.Exception(string.Concat((object)"deserialize(): ", (object)type));
			}
		}

		public override byte DeserializeByte(StreamBuffer din)
		{
			return (byte)((Stream)din).ReadByte();
		}

		private bool DeserializeBoolean(StreamBuffer din)
		{
			return ((Stream)din).ReadByte() != 0;
		}

		public override short DeserializeShort(StreamBuffer din)
		{
			lock (memShort)
			{
				byte[] array = memShort;
				((Stream)din).Read(array, 0, 2);
				return (short)((array[0] << 8) | array[1]);
			}
		}

		private int DeserializeInteger(StreamBuffer din)
		{
			lock (memInteger)
			{
				byte[] array = memInteger;
				((Stream)din).Read(array, 0, 4);
				return (array[0] << 24) | (array[1] << 16) | (array[2] << 8) | array[3];
			}
		}

		private long DeserializeLong(StreamBuffer din)
		{
			lock (memLong)
			{
				byte[] array = memLong;
				((Stream)din).Read(array, 0, 8);
				if (BitConverter.IsLittleEndian)
				{
					return (long)(((ulong)array[0] << 56) | ((ulong)array[1] << 48) | ((ulong)array[2] << 40) | ((ulong)array[3] << 32) | ((ulong)array[4] << 24) | ((ulong)array[5] << 16) | ((ulong)array[6] << 8) | array[7]);
				}
				return BitConverter.ToInt64(array, 0);
			}
		}

		private float DeserializeFloat(StreamBuffer din)
		{
			lock (memFloat)
			{
				byte[] array = memFloat;
				((Stream)din).Read(array, 0, 4);
				if (BitConverter.IsLittleEndian)
				{
					byte b = array[0];
					byte b2 = array[1];
					array[0] = array[3];
					array[1] = array[2];
					array[2] = b2;
					array[3] = b;
				}
				return BitConverter.ToSingle(array, 0);
			}
		}

		private double DeserializeDouble(StreamBuffer din)
		{
			lock (memDouble)
			{
				byte[] array = memDouble;
				((Stream)din).Read(array, 0, 8);
				if (BitConverter.IsLittleEndian)
				{
					byte b = array[0];
					byte b2 = array[1];
					byte b3 = array[2];
					byte b4 = array[3];
					array[0] = array[7];
					array[1] = array[6];
					array[2] = array[5];
					array[3] = array[4];
					array[4] = b4;
					array[5] = b3;
					array[6] = b2;
					array[7] = b;
				}
				return BitConverter.ToDouble(array, 0);
			}
		}

		private string DeserializeString(StreamBuffer din)
		{
			short num = DeserializeShort(din);
			if (num == 0)
			{
				return "";
			}
			byte[] array = new byte[num];
			((Stream)din).Read(array, 0, array.Length);
			return Encoding.get_UTF8().GetString(array, 0, array.Length);
		}

		private global::System.Array DeserializeArray(StreamBuffer din)
		{
			short num = DeserializeShort(din);
			byte b = (byte)((Stream)din).ReadByte();
			global::System.Array array;
			switch (b)
			{
			case 121:
			{
				global::System.Array array2 = DeserializeArray(din);
				global::System.Type type = ((object)array2).GetType();
				array = global::System.Array.CreateInstance(type, (int)num);
				array.SetValue((object)array2, 0);
				for (short num3 = 1; num3 < num; num3 = (short)(num3 + 1))
				{
					array2 = DeserializeArray(din);
					array.SetValue((object)array2, (int)num3);
				}
				break;
			}
			case 120:
			{
				array = global::System.Array.CreateInstance(typeof(byte[]), (int)num);
				for (short num5 = 0; num5 < num; num5 = (short)(num5 + 1))
				{
					global::System.Array array4 = DeserializeByteArray(din);
					array.SetValue((object)array4, (int)num5);
				}
				break;
			}
			case 99:
			{
				byte b2 = (byte)((Stream)din).ReadByte();
				CustomType customType = default(CustomType);
				if (Protocol.CodeDict.TryGetValue(b2, ref customType))
				{
					array = global::System.Array.CreateInstance(customType.Type, (int)num);
					for (int i = 0; i < num; i++)
					{
						short num4 = DeserializeShort(din);
						if (customType.DeserializeStreamFunction == null)
						{
							byte[] array3 = new byte[num4];
							((Stream)din).Read(array3, 0, (int)num4);
							array.SetValue(customType.DeserializeFunction(array3), i);
						}
						else
						{
							array.SetValue(customType.DeserializeStreamFunction(din, num4), i);
						}
					}
					break;
				}
				throw new global::System.Exception(string.Concat((object)"Cannot find deserializer for custom type: ", (object)b2));
			}
			case 68:
			{
				global::System.Array arrayResult = null;
				DeserializeDictionaryArray(din, num, out arrayResult);
				return arrayResult;
			}
			default:
			{
				array = CreateArrayByType(b, num);
				for (short num2 = 0; num2 < num; num2 = (short)(num2 + 1))
				{
					array.SetValue(Deserialize(din, b), (int)num2);
				}
				break;
			}
			}
			return array;
		}

		private byte[] DeserializeByteArray(StreamBuffer din)
		{
			int num = DeserializeInteger(din);
			byte[] array = new byte[num];
			((Stream)din).Read(array, 0, num);
			return array;
		}

		private int[] DeserializeIntArray(StreamBuffer din)
		{
			int num = DeserializeInteger(din);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = DeserializeInteger(din);
			}
			return array;
		}

		private string[] DeserializeStringArray(StreamBuffer din)
		{
			int num = DeserializeShort(din);
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = DeserializeString(din);
			}
			return array;
		}

		private object[] DeserializeObjectArray(StreamBuffer din)
		{
			short num = DeserializeShort(din);
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				byte type = (byte)((Stream)din).ReadByte();
				array[i] = Deserialize(din, type);
			}
			return array;
		}

		private Hashtable DeserializeHashTable(StreamBuffer din)
		{
			int num = DeserializeShort(din);
			Hashtable hashtable = new Hashtable(num);
			for (int i = 0; i < num; i++)
			{
				object key = Deserialize(din, (byte)((Stream)din).ReadByte());
				object obj2 = (hashtable[key] = Deserialize(din, (byte)((Stream)din).ReadByte()));
			}
			return hashtable;
		}

		private IDictionary DeserializeDictionary(StreamBuffer din)
		{
			byte b = (byte)((Stream)din).ReadByte();
			byte b2 = (byte)((Stream)din).ReadByte();
			int num = DeserializeShort(din);
			bool flag = b == 0 || b == 42;
			bool flag2 = b2 == 0 || b2 == 42;
			global::System.Type typeOfCode = GetTypeOfCode(b);
			global::System.Type typeOfCode2 = GetTypeOfCode(b2);
			global::System.Type type = typeof(Dictionary<, >).MakeGenericType(new global::System.Type[2] { typeOfCode, typeOfCode2 });
			object obj = Activator.CreateInstance(type);
			IDictionary val = (IDictionary)((obj is IDictionary) ? obj : null);
			for (int i = 0; i < num; i++)
			{
				object obj2 = Deserialize(din, flag ? ((byte)((Stream)din).ReadByte()) : b);
				object obj3 = Deserialize(din, flag2 ? ((byte)((Stream)din).ReadByte()) : b2);
				val.Add(obj2, obj3);
			}
			return val;
		}

		private bool DeserializeDictionaryArray(StreamBuffer din, short size, out global::System.Array arrayResult)
		{
			byte keyTypeCode;
			byte valTypeCode;
			global::System.Type type = DeserializeDictionaryType(din, out keyTypeCode, out valTypeCode);
			arrayResult = global::System.Array.CreateInstance(type, (int)size);
			for (short num = 0; num < size; num = (short)(num + 1))
			{
				object obj = Activator.CreateInstance(type);
				IDictionary val = (IDictionary)((obj is IDictionary) ? obj : null);
				if (val == null)
				{
					return false;
				}
				short num2 = DeserializeShort(din);
				for (int i = 0; i < num2; i++)
				{
					object obj2;
					if (keyTypeCode != 0)
					{
						obj2 = Deserialize(din, keyTypeCode);
					}
					else
					{
						byte type2 = (byte)((Stream)din).ReadByte();
						obj2 = Deserialize(din, type2);
					}
					object obj3;
					if (valTypeCode != 0)
					{
						obj3 = Deserialize(din, valTypeCode);
					}
					else
					{
						byte type3 = (byte)((Stream)din).ReadByte();
						obj3 = Deserialize(din, type3);
					}
					val.Add(obj2, obj3);
				}
				arrayResult.SetValue((object)val, (int)num);
			}
			return true;
		}

		private global::System.Type DeserializeDictionaryType(StreamBuffer reader, out byte keyTypeCode, out byte valTypeCode)
		{
			keyTypeCode = (byte)((Stream)reader).ReadByte();
			valTypeCode = (byte)((Stream)reader).ReadByte();
			GpType gpType = (GpType)keyTypeCode;
			GpType gpType2 = (GpType)valTypeCode;
			global::System.Type type = ((gpType != 0) ? GetTypeOfCode(keyTypeCode) : typeof(object));
			global::System.Type type2 = ((gpType2 != 0) ? GetTypeOfCode(valTypeCode) : typeof(object));
			return typeof(Dictionary<, >).MakeGenericType(new global::System.Type[2] { type, type2 });
		}
	}
}
