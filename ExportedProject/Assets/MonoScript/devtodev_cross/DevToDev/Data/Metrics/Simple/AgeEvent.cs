using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class AgeEvent : SimpleEvent
	{
		private static readonly string AGE = "age";

		public AgeEvent()
		{
		}

		public AgeEvent(int age)
			: base(EventType.Age)
		{
			parameters.Remove(Event.TIMESTAMP);
			parameters.Add(AGE, (object)age);
		}

		public AgeEvent(ObjectInfo info)
			: base(info)
		{
		}
	}
}
