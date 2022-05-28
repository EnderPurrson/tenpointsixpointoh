using System;
using System.Runtime.CompilerServices;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev
{
	public class LocationEventParams : ProgressionEventParams
	{
		private static readonly string DIFFICULTY_KEY = "difficulty";

		private static readonly string SOURCE_KEY = "source";

		private static readonly string DURATION_KEY = "duration";

		internal int? Difficulty
		{
			[CompilerGenerated]
			get
			{
				return _003CDifficulty_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDifficulty_003Ek__BackingField = value;
			}
		}

		internal string SourceLocationId
		{
			[CompilerGenerated]
			get
			{
				return _003CSourceLocationId_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CSourceLocationId_003Ek__BackingField = value;
			}
		}

		internal long? Duration
		{
			[CompilerGenerated]
			get
			{
				return _003CDuration_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDuration_003Ek__BackingField = value;
			}
		}

		public LocationEventParams()
		{
		}

		public LocationEventParams(ObjectInfo info)
			: base(info)
		{
			try
			{
				Difficulty = info.GetValue("Difficulty", typeof(int?)) as int?;
				SourceLocationId = info.GetValue("SourceLocationId", typeof(string)) as string;
				Duration = info.GetValue("Duration", typeof(int?)) as int?;
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			base.GetObjectData(info);
			try
			{
				info.AddValue("Difficulty", Difficulty);
				info.AddValue("SourceLocationId", SourceLocationId);
				info.AddValue("Duration", Duration);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		internal LocationEventParams(LocationEventParams leparams)
		{
			base.Spent = leparams.Spent;
			base.Earned = leparams.Earned;
			Difficulty = leparams.Difficulty;
			Duration = leparams.Duration;
			base.IsSuccessful = leparams.IsSuccessful;
			SourceLocationId = leparams.SourceLocationId;
		}

		internal override void Merge(ProgressionEventParams value)
		{
			base.Merge(value);
			LocationEventParams locationEventParams = value as LocationEventParams;
			if (locationEventParams.Difficulty.get_HasValue())
			{
				Difficulty = locationEventParams.Difficulty;
			}
			if (locationEventParams.SourceLocationId != null)
			{
				SourceLocationId = locationEventParams.SourceLocationId;
			}
			if (locationEventParams.Duration.get_HasValue())
			{
				Duration = locationEventParams.Duration;
			}
		}

		public void SetDifficulty(int difficulty)
		{
			Difficulty = difficulty;
		}

		public void SetSource(string sourceLocationId)
		{
			SourceLocationId = sourceLocationId;
		}

		public void SetDuration(long duration)
		{
			Duration = duration;
		}

		internal override JSONNode ToJson()
		{
			JSONNode jSONNode = base.ToJson();
			JSONNode jSONNode2 = jSONNode[ProgressionEventParams.PARAMS_KEY];
			if (Difficulty.get_HasValue())
			{
				jSONNode2.Add(DIFFICULTY_KEY, new JSONData(Difficulty));
			}
			if (Duration.get_HasValue())
			{
				if (Duration > 0)
				{
					jSONNode2.Add(DURATION_KEY, new JSONData(Duration));
				}
			}
			else
			{
				long num = (base.EventFinish - base.EventStart) / 1000;
				if (num > 0)
				{
					jSONNode2.Add(DURATION_KEY, new JSONData(num));
				}
			}
			if (SourceLocationId != null)
			{
				jSONNode2.Add(SOURCE_KEY, new JSONData(SourceLocationId));
			}
			if (Difficulty.get_HasValue())
			{
				jSONNode2.Add(DIFFICULTY_KEY, new JSONData(Difficulty));
			}
			return jSONNode;
		}

		internal override ProgressionEventParams Clone()
		{
			LocationEventParams locationEventParams = new LocationEventParams();
			locationEventParams.Earned = base.Earned;
			locationEventParams.Spent = base.Spent;
			locationEventParams.EventName = base.EventName;
			locationEventParams.EventStart = base.EventStart;
			locationEventParams.EventFinish = base.EventFinish;
			locationEventParams.Difficulty = Difficulty;
			locationEventParams.SourceLocationId = SourceLocationId;
			locationEventParams.IsSuccessful = base.IsSuccessful;
			locationEventParams.Duration = Duration;
			return locationEventParams;
		}
	}
}
