using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ExitGames.Client.Photon
{
	[DefaultMember("Item")]
	public class OperationResponse
	{
		public byte OperationCode;

		public short ReturnCode;

		public string DebugMessage;

		public Dictionary<byte, object> Parameters;

		public object this[byte parameterCode]
		{
			get
			{
				object result = default(object);
				Parameters.TryGetValue(parameterCode, ref result);
				return result;
			}
			set
			{
				Parameters.set_Item(parameterCode, value);
			}
		}

		public override string ToString()
		{
			return string.Format("OperationResponse {0}: ReturnCode: {1}.", (object)OperationCode, (object)ReturnCode);
		}

		public string ToStringFull()
		{
			return string.Format("OperationResponse {0}: ReturnCode: {1} ({3}). Parameters: {2}", new object[4]
			{
				OperationCode,
				ReturnCode,
				SupportClass.DictionaryToString((IDictionary)(object)Parameters),
				DebugMessage
			});
		}
	}
}
