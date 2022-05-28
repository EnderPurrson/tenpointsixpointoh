using System;

namespace Prime31
{
	public static class DateTimeExtensions
	{
		public static long toEpochTime(this global::System.DateTime self)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1);
			TimeSpan val = self - dateTime;
			return Convert.ToInt64(((TimeSpan)(ref val)).get_TotalSeconds());
		}

		public static global::System.DateTime fromEpochTime(this long unixTime)
		{
			return new global::System.DateTime(1970, 1, 1, 0, 0, 0, (DateTimeKind)1).AddSeconds((double)unixTime);
		}
	}
}
