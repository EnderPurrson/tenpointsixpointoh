using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DevToDev.Logic;

namespace DevToDev
{
	public class People
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1
		{
			public Gender value;

			public void _003Cset_Gender_003Eb__0()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Gender = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass4
		{
			public int value;

			public void _003Cset_Age_003Eb__3()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Age = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass7
		{
			public bool value;

			public void _003Cset_Cheater_003Eb__6()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Cheater = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClassa
		{
			public string value;

			public void _003Cset_Name_003Eb__9()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Name = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClassd
		{
			public string value;

			public void _003Cset_Email_003Eb__c()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Email = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass10
		{
			public string value;

			public void _003Cset_Phone_003Eb__f()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Phone = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass13
		{
			public string value;

			public void _003Cset_Photo_003Eb__12()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Photo = value;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass16
		{
			public string key;

			public object value;

			public void _003CSetUserData_003Eb__15()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.SetUserData(key, value);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass19
		{
			public Dictionary<string, object> userData;

			public void _003CSetUserData_003Eb__18()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.SetUserData(userData);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass1e
		{
			public string key;

			public void _003CUnsetUserData_003Eb__1d()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnsetUserData(key);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass21
		{
			public List<string> keysToRemove;

			public void _003CUnsetUserData_003Eb__20()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnsetUserData(keysToRemove);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass24
		{
			public string key;

			public object value;

			public void _003CAppendUserData_003Eb__23()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.AppendUserData(key, value);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass27
		{
			public Dictionary<string, object> userData;

			public void _003CAppendUserData_003Eb__26()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.AppendUserData(userData);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2a
		{
			public string key;

			public object value;

			public void _003CUnionUserData_003Eb__29()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnionUserData(key, value);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass2d
		{
			public Dictionary<string, object> userData;

			public void _003CUnionUserData_003Eb__2c()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnionUserData(userData);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass30
		{
			public string key;

			public object value;

			public void _003CIncrement_003Eb__2f()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Increment(key, value);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass33
		{
			public Dictionary<string, object> userData;

			public void _003CIncrement_003Eb__32()
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Increment(userData);
			}
		}

		[CompilerGenerated]
		private static Action CS_0024_003C_003E9__CachedAnonymousMethodDelegate1c;

		public Gender Gender
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClass1 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass1();
				_003C_003Ec__DisplayClass.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003Cset_Gender_003Eb__0));
			}
		}

		public int Age
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClass4 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass4();
				_003C_003Ec__DisplayClass.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003Cset_Age_003Eb__3));
			}
		}

		public bool Cheater
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClass7 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass7();
				_003C_003Ec__DisplayClass.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003Cset_Cheater_003Eb__6));
			}
		}

		public string Name
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClassa _003C_003Ec__DisplayClassa = new _003C_003Ec__DisplayClassa();
				_003C_003Ec__DisplayClassa.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClassa._003Cset_Name_003Eb__9));
			}
		}

		public string Email
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClassd _003C_003Ec__DisplayClassd = new _003C_003Ec__DisplayClassd();
				_003C_003Ec__DisplayClassd.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClassd._003Cset_Email_003Eb__c));
			}
		}

		public string Phone
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClass10 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass10();
				_003C_003Ec__DisplayClass.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003Cset_Phone_003Eb__f));
			}
		}

		public string Photo
		{
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Expected O, but got Unknown
				_003C_003Ec__DisplayClass13 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass13();
				_003C_003Ec__DisplayClass.value = value;
				SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003Cset_Photo_003Eb__12));
			}
		}

		internal People()
		{
		}

		public void SetUserData(string key, object value)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			_003C_003Ec__DisplayClass16 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass16();
			_003C_003Ec__DisplayClass.key = key;
			_003C_003Ec__DisplayClass.value = value;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CSetUserData_003Eb__15));
		}

		public void SetUserData(Dictionary<string, object> userData)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_003C_003Ec__DisplayClass19 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass19();
			_003C_003Ec__DisplayClass.userData = userData;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CSetUserData_003Eb__18));
		}

		public void ClearUserData()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			SDKClient instance = SDKClient.Instance;
			if (CS_0024_003C_003E9__CachedAnonymousMethodDelegate1c == null)
			{
				CS_0024_003C_003E9__CachedAnonymousMethodDelegate1c = new Action(_003CClearUserData_003Eb__1b);
			}
			instance.Execute(CS_0024_003C_003E9__CachedAnonymousMethodDelegate1c);
		}

		public void UnsetUserData(string key)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_003C_003Ec__DisplayClass1e _003C_003Ec__DisplayClass1e = new _003C_003Ec__DisplayClass1e();
			_003C_003Ec__DisplayClass1e.key = key;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass1e._003CUnsetUserData_003Eb__1d));
		}

		public void UnsetUserData(List<string> keysToRemove)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_003C_003Ec__DisplayClass21 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass21();
			_003C_003Ec__DisplayClass.keysToRemove = keysToRemove;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CUnsetUserData_003Eb__20));
		}

		public void AppendUserData(string key, object value)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			_003C_003Ec__DisplayClass24 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass24();
			_003C_003Ec__DisplayClass.key = key;
			_003C_003Ec__DisplayClass.value = value;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CAppendUserData_003Eb__23));
		}

		public void AppendUserData(Dictionary<string, object> userData)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_003C_003Ec__DisplayClass27 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass27();
			_003C_003Ec__DisplayClass.userData = userData;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CAppendUserData_003Eb__26));
		}

		public void UnionUserData(string key, object value)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			_003C_003Ec__DisplayClass2a _003C_003Ec__DisplayClass2a = new _003C_003Ec__DisplayClass2a();
			_003C_003Ec__DisplayClass2a.key = key;
			_003C_003Ec__DisplayClass2a.value = value;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass2a._003CUnionUserData_003Eb__29));
		}

		public void UnionUserData(Dictionary<string, object> userData)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_003C_003Ec__DisplayClass2d _003C_003Ec__DisplayClass2d = new _003C_003Ec__DisplayClass2d();
			_003C_003Ec__DisplayClass2d.userData = userData;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass2d._003CUnionUserData_003Eb__2c));
		}

		public void Increment(string key, object value)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			_003C_003Ec__DisplayClass30 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass30();
			_003C_003Ec__DisplayClass.key = key;
			_003C_003Ec__DisplayClass.value = value;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CIncrement_003Eb__2f));
		}

		public void Increment(Dictionary<string, object> userData)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			_003C_003Ec__DisplayClass33 _003C_003Ec__DisplayClass = new _003C_003Ec__DisplayClass33();
			_003C_003Ec__DisplayClass.userData = userData;
			SDKClient.Instance.Execute(new Action(_003C_003Ec__DisplayClass._003CIncrement_003Eb__32));
		}

		[CompilerGenerated]
		private static void _003CClearUserData_003Eb__1b()
		{
			SDKClient.Instance.UsersStorage.ActivePeople.ClearUserData();
		}
	}
}
