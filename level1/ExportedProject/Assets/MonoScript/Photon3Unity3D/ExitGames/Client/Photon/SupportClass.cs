using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ExitGames.Client.Photon
{
	public class SupportClass
	{
		public delegate int IntegerMillisecondsDelegate();

		public class ThreadSafeRandom
		{
			private static readonly Random _r = new Random();

			public static int Next()
			{
				lock (_r)
				{
					return _r.Next();
				}
			}
		}

		protected internal static IntegerMillisecondsDelegate IntegerMilliseconds = () => Environment.get_TickCount();

		public static uint CalculateCrc(byte[] buffer, int length)
		{
			uint num = 4294967295u;
			uint num2 = 3988292384u;
			byte b = 0;
			for (int i = 0; i < length; i++)
			{
				b = buffer[i];
				num ^= b;
				for (int j = 0; j < 8; j++)
				{
					num = (((num & 1) == 0) ? (num >> 1) : ((num >> 1) ^ num2));
				}
			}
			return num;
		}

		public static List<MethodInfo> GetMethods(global::System.Type type, global::System.Type attribute)
		{
			List<MethodInfo> val = new List<MethodInfo>();
			if (type == null)
			{
				return val;
			}
			MethodInfo[] methods = type.GetMethods((BindingFlags)52);
			MethodInfo[] array = methods;
			foreach (MethodInfo val2 in array)
			{
				if (attribute == null || ((MemberInfo)val2).IsDefined(attribute, false))
				{
					val.Add(val2);
				}
			}
			return val;
		}

		public static int GetTickCount()
		{
			return IntegerMilliseconds();
		}

		public static void CallInBackground(Func<bool> myThread)
		{
			CallInBackground(myThread, 100);
		}

		public static void CallInBackground(Func<bool> myThread, int millisecondsInterval)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			_003C_003Ec__DisplayClass6_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass6_0();
			CS_0024_003C_003E8__locals0.millisecondsInterval = millisecondsInterval;
			CS_0024_003C_003E8__locals0.myThread = myThread;
			Thread val = new Thread((ThreadStart)delegate
			{
				while (CS_0024_003C_003E8__locals0.myThread.Invoke())
				{
					Thread.Sleep(CS_0024_003C_003E8__locals0.millisecondsInterval);
				}
			});
			val.set_IsBackground(true);
			val.Start();
		}

		public static void WriteStackTrace(global::System.Exception throwable, TextWriter stream)
		{
			if (stream != null)
			{
				stream.WriteLine(((object)throwable).ToString());
				stream.WriteLine(throwable.get_StackTrace());
				stream.Flush();
			}
			else
			{
				Debug.WriteLine(((object)throwable).ToString());
				Debug.WriteLine(throwable.get_StackTrace());
			}
		}

		public static void WriteStackTrace(global::System.Exception throwable)
		{
			WriteStackTrace(throwable, null);
		}

		public static string DictionaryToString(IDictionary dictionary)
		{
			return DictionaryToString(dictionary, true);
		}

		public static string DictionaryToString(IDictionary dictionary, bool includeTypes)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			if (dictionary == null)
			{
				return "null";
			}
			StringBuilder val = new StringBuilder();
			val.Append("{");
			global::System.Collections.IEnumerator enumerator = ((global::System.Collections.IEnumerable)dictionary.get_Keys()).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					if (val.get_Length() > 1)
					{
						val.Append(", ");
					}
					global::System.Type type;
					string text;
					if (dictionary.get_Item(current) == null)
					{
						type = typeof(object);
						text = "null";
					}
					else
					{
						type = dictionary.get_Item(current).GetType();
						text = dictionary.get_Item(current).ToString();
					}
					if (typeof(IDictionary) == type || typeof(Hashtable) == type)
					{
						text = DictionaryToString((IDictionary)dictionary.get_Item(current));
					}
					if (typeof(string[]) == type)
					{
						text = string.Format("{{{0}}}", (object)string.Join(",", (string[])dictionary.get_Item(current)));
					}
					if (includeTypes)
					{
						val.AppendFormat("({0}){1}=({2}){3}", new object[4]
						{
							((MemberInfo)current.GetType()).get_Name(),
							current,
							((MemberInfo)type).get_Name(),
							text
						});
					}
					else
					{
						val.AppendFormat("{0}={1}", current, (object)text);
					}
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
			val.Append("}");
			return ((object)val).ToString();
		}

		[Obsolete("Use DictionaryToString() instead.")]
		public static string HashtableToString(Hashtable hash)
		{
			return DictionaryToString((IDictionary)(object)hash);
		}

		public static string ByteArrayToString(byte[] list)
		{
			if (list == null)
			{
				return string.Empty;
			}
			return BitConverter.ToString(list);
		}
	}
}
