using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ExitGames.Client.Photon
{
	[DefaultMember("Item")]
	public class EventData
	{
		public byte Code;

		public Dictionary<byte, object> Parameters;

		public object this[byte key]
		{
			get
			{
				object result = default(object);
				Parameters.TryGetValue(key, ref result);
				return result;
			}
			set
			{
				Parameters.set_Item(key, value);
			}
		}

		public override string ToString()
		{
			return string.Format("Event {0}.", (object)Code.ToString());
		}

		public string ToStringFull()
		{
			return string.Format("Event {0}: {1}", (object)Code, (object)SupportClass.DictionaryToString((IDictionary)(object)Parameters));
		}
	}
}
