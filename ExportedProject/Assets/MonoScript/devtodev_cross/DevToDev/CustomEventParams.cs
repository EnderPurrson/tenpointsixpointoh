using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev
{
	public sealed class CustomEventParams : ISaveable
	{
		internal Dictionary<string, double> DoubleParams
		{
			[CompilerGenerated]
			get
			{
				return _003CDoubleParams_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CDoubleParams_003Ek__BackingField = value;
			}
		}

		internal Dictionary<string, int> IntParams
		{
			[CompilerGenerated]
			get
			{
				return _003CIntParams_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CIntParams_003Ek__BackingField = value;
			}
		}

		internal Dictionary<string, long> LongParams
		{
			[CompilerGenerated]
			get
			{
				return _003CLongParams_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CLongParams_003Ek__BackingField = value;
			}
		}

		internal Dictionary<string, string> StringParams
		{
			[CompilerGenerated]
			get
			{
				return _003CStringParams_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				_003CStringParams_003Ek__BackingField = value;
			}
		}

		public int Count
		{
			[CompilerGenerated]
			get
			{
				return _003CCount_003Ek__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				_003CCount_003Ek__BackingField = value;
			}
		}

		internal bool HasNumeric
		{
			get
			{
				return IntParams.get_Count() + LongParams.get_Count() + DoubleParams.get_Count() > 0;
			}
		}

		internal bool HasStrings
		{
			get
			{
				return StringParams.get_Count() > 0;
			}
		}

		public CustomEventParams()
		{
			IntParams = new Dictionary<string, int>();
			LongParams = new Dictionary<string, long>();
			DoubleParams = new Dictionary<string, double>();
			StringParams = new Dictionary<string, string>();
		}

		public CustomEventParams(ObjectInfo info)
		{
			try
			{
				DoubleParams = info.GetValue("DoubleParams", typeof(Dictionary<string, double>)) as Dictionary<string, double>;
				IntParams = info.GetValue("IntParams", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				LongParams = info.GetValue("LongParams", typeof(Dictionary<string, long>)) as Dictionary<string, long>;
				StringParams = info.GetValue("StringParams", typeof(Dictionary<string, string>)) as Dictionary<string, string>;
				Count = (int)info.GetValue("Count", typeof(int));
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
				info.AddValue("DoubleParams", DoubleParams);
				info.AddValue("IntParams", IntParams);
				info.AddValue("LongParams", LongParams);
				info.AddValue("StringParams", StringParams);
				info.AddValue("Count", Count);
			}
			catch (global::System.Exception ex)
			{
				Log.D("Error in sealization: " + ex.get_Message() + "\n" + ex.get_StackTrace());
			}
		}

		public void AddParam(string key, int value)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			if (Count < 20)
			{
				if (HasNumericParam(key))
				{
					RemoveNumeric(key);
					Count--;
				}
				IntParams.Add(key, value);
				Count++;
			}
		}

		public void AddParam(string key, long value)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			if (Count < 20)
			{
				if (HasNumericParam(key))
				{
					RemoveNumeric(key);
					Count--;
				}
				LongParams.Add(key, value);
				Count++;
			}
		}

		public void AddParam(string key, double value)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			if (Count < 20)
			{
				if (HasNumericParam(key))
				{
					RemoveNumeric(key);
					Count--;
				}
				DoubleParams.Add(key, value);
				Count++;
			}
		}

		public void AddParam(string key, string value)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			value = value ?? "null";
			if (Count < 20)
			{
				if (StringParams.ContainsKey(key))
				{
					StringParams.Remove(key);
					Count--;
				}
				StringParams.Add(key, value);
				Count++;
			}
		}

		public void CopyFromAnother(CustomEventParams cep)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string, double> enumerator = cep.DoubleParams.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, double> current = enumerator.get_Current();
					AddParam(current.get_Key(), current.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator).Dispose();
			}
			Enumerator<string, string> enumerator2 = cep.StringParams.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<string, string> current2 = enumerator2.get_Current();
					AddParam(current2.get_Key(), current2.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2).Dispose();
			}
			Enumerator<string, int> enumerator3 = cep.IntParams.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					KeyValuePair<string, int> current3 = enumerator3.get_Current();
					AddParam(current3.get_Key(), current3.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator3).Dispose();
			}
			Enumerator<string, long> enumerator4 = cep.LongParams.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					KeyValuePair<string, long> current4 = enumerator4.get_Current();
					AddParam(current4.get_Key(), current4.get_Value());
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator4).Dispose();
			}
		}

		private bool HasNumericParam(string key)
		{
			if (!IntParams.ContainsKey(key) && !LongParams.ContainsKey(key))
			{
				return DoubleParams.ContainsKey(key);
			}
			return true;
		}

		private void RemoveNumeric(string key)
		{
			if (IntParams.ContainsKey(key))
			{
				IntParams.Remove(key);
			}
			else if (LongParams.ContainsKey(key))
			{
				LongParams.Remove(key);
			}
			else
			{
				DoubleParams.Remove(key);
			}
		}
	}
}
