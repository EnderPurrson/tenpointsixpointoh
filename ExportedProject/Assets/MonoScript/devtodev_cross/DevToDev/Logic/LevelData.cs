using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics;

namespace DevToDev.Logic
{
	[Serializable]
	internal class LevelData : ISaveable
	{
		private static readonly string EARNED = "earned";

		private static readonly string SPENT = "spent";

		private static readonly string BOUGHT = "bought";

		private static readonly string BALANCE = "balance";

		private int level = 1;

		private bool isNew;

		private long timestamp;

		private long localDuration;

		private long absoluteDuration;

		private Dictionary<string, int> balance;

		private Dictionary<string, int> spent;

		private Dictionary<string, int> earned;

		private Dictionary<string, int> bought;

		internal int Level
		{
			get
			{
				return level;
			}
			set
			{
				level = value;
			}
		}

		internal long TimeStamp
		{
			get
			{
				return timestamp;
			}
			set
			{
				timestamp = value;
			}
		}

		internal long LocalDuration
		{
			get
			{
				return localDuration;
			}
			set
			{
				localDuration = value;
			}
		}

		internal long AbsoluteDuration
		{
			get
			{
				return absoluteDuration;
			}
			set
			{
				absoluteDuration = value;
			}
		}

		public Dictionary<string, int> Balance
		{
			get
			{
				return balance;
			}
			set
			{
				if (value != null)
				{
					balance = value;
				}
			}
		}

		public bool IsNew
		{
			get
			{
				return isNew;
			}
			set
			{
				isNew = value;
				if (isNew)
				{
					timestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
				}
				else
				{
					timestamp = 0L;
				}
			}
		}

		public JSONClass DataToSend
		{
			get
			{
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_012c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0131: Unknown result type (might be due to invalid IL or missing references)
				//IL_018e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0193: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_019e: Unknown result type (might be due to invalid IL or missing references)
				JSONClass jSONClass = new JSONClass();
				if (timestamp > 0)
				{
					jSONClass.Add(Event.TIMESTAMP, new JSONData(timestamp));
				}
				if (earned.get_Count() > 0)
				{
					JSONClass jSONClass2 = new JSONClass();
					jSONClass.Add(EARNED, jSONClass2);
					Enumerator<string, int> enumerator = earned.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, int> current = enumerator.get_Current();
							jSONClass2.Add(current.get_Key(), new JSONData(current.get_Value()));
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator).Dispose();
					}
				}
				if (spent.get_Count() > 0)
				{
					JSONClass jSONClass3 = new JSONClass();
					jSONClass.Add(SPENT, jSONClass3);
					Enumerator<string, int> enumerator2 = spent.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, int> current2 = enumerator2.get_Current();
							jSONClass3.Add(current2.get_Key(), new JSONData(current2.get_Value()));
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2).Dispose();
					}
				}
				if (bought.get_Count() > 0)
				{
					JSONClass jSONClass4 = new JSONClass();
					jSONClass.Add(BOUGHT, jSONClass4);
					Enumerator<string, int> enumerator3 = bought.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<string, int> current3 = enumerator3.get_Current();
							jSONClass4.Add(current3.get_Key(), new JSONData(current3.get_Value()));
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator3).Dispose();
					}
				}
				if (balance.get_Count() > 0)
				{
					JSONClass jSONClass5 = new JSONClass();
					jSONClass.Add(BALANCE, jSONClass5);
					Enumerator<string, int> enumerator4 = balance.GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							KeyValuePair<string, int> current4 = enumerator4.get_Current();
							jSONClass5.Add(current4.get_Key(), new JSONData(current4.get_Value()));
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator4).Dispose();
					}
				}
				if (jSONClass.Count == 0)
				{
					return null;
				}
				return jSONClass;
			}
		}

		public LevelData()
		{
			balance = new Dictionary<string, int>();
			spent = new Dictionary<string, int>();
			earned = new Dictionary<string, int>();
			bought = new Dictionary<string, int>();
		}

		public LevelData(ObjectInfo info)
		{
			try
			{
				balance = info.GetValue("balance", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				spent = info.GetValue("spent", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				earned = info.GetValue("earned", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				bought = info.GetValue("bought", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				level = (int)info.GetValue("level", typeof(int));
				timestamp = (long)info.GetValue("timestamp", typeof(long));
				localDuration = (long)info.GetValue("localDuration", typeof(long));
				absoluteDuration = (long)info.GetValue("absoluteDuration", typeof(long));
				isNew = (bool)info.GetValue("isNew", typeof(bool));
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in desealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("balance", balance);
				info.AddValue("spent", spent);
				info.AddValue("earned", earned);
				info.AddValue("bought", bought);
				info.AddValue("level", level);
				info.AddValue("timestamp", timestamp);
				info.AddValue("localDuration", localDuration);
				info.AddValue("absoluteDuration", absoluteDuration);
				info.AddValue("isNew", isNew);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public LevelData(int level, bool isNew)
			: this()
		{
			this.level = level;
			IsNew = isNew;
		}

		public void upSend(string key, int value)
		{
			if (spent.ContainsKey(key))
			{
				int num = spent.get_Item(key);
				spent.set_Item(key, num + value);
			}
			else
			{
				spent.Add(key, value);
			}
		}

		public void upEarned(string key, int value)
		{
			if (earned.ContainsKey(key))
			{
				int num = earned.get_Item(key);
				earned.set_Item(key, num + value);
			}
			else
			{
				earned.Add(key, value);
			}
		}

		public void upBought(string key, int value)
		{
			if (bought.ContainsKey(key))
			{
				int num = bought.get_Item(key);
				bought.set_Item(key, num + value);
			}
			else
			{
				bought.Add(key, value);
			}
		}
	}
}
