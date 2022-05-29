using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class EventDelegate
{
	[SerializeField]
	private MonoBehaviour mTarget;

	[SerializeField]
	private string mMethodName;

	[SerializeField]
	private EventDelegate.Parameter[] mParameters;

	public bool oneShot;

	[NonSerialized]
	private EventDelegate.Callback mCachedCallback;

	[NonSerialized]
	private bool mRawDelegate;

	[NonSerialized]
	private bool mCached;

	[NonSerialized]
	private MethodInfo mMethod;

	[NonSerialized]
	private ParameterInfo[] mParameterInfos;

	[NonSerialized]
	private object[] mArgs;

	private static int s_Hash;

	public bool isEnabled
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRawDelegate && this.mCachedCallback != null)
			{
				return true;
			}
			if (this.mTarget == null)
			{
				return false;
			}
			MonoBehaviour monoBehaviour = this.mTarget;
			return (monoBehaviour == null ? true : monoBehaviour.enabled);
		}
	}

	public bool isValid
	{
		get
		{
			bool flag;
			if (!this.mCached)
			{
				this.Cache();
			}
			if (!this.mRawDelegate || this.mCachedCallback == null)
			{
				flag = (this.mTarget == null ? false : !string.IsNullOrEmpty(this.mMethodName));
			}
			else
			{
				flag = true;
			}
			return flag;
		}
	}

	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mParameters = null;
		}
	}

	public EventDelegate.Parameter[] parameters
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			return this.mParameters;
		}
	}

	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
			this.mCached = false;
			this.mMethod = null;
			this.mParameterInfos = null;
			this.mParameters = null;
		}
	}

	static EventDelegate()
	{
		EventDelegate.s_Hash = "EventDelegate".GetHashCode();
	}

	public EventDelegate()
	{
	}

	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		return EventDelegate.Add(list, callback, false);
	}

	public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list == null)
		{
			Debug.LogWarning("Attempting to add a callback to a list that's null");
			return null;
		}
		int num = 0;
		int count = list.Count;
		while (num < count)
		{
			EventDelegate item = list[num];
			if (item != null && item.Equals(callback))
			{
				return item;
			}
			num++;
		}
		EventDelegate eventDelegate = new EventDelegate(callback)
		{
			oneShot = oneShot
		};
		list.Add(eventDelegate);
		return eventDelegate;
	}

	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, ev.oneShot);
	}

	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
		{
			EventDelegate.Add(list, ev.mCachedCallback, oneShot);
		}
		else if (list == null)
		{
			Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
		else
		{
			int num = 0;
			int count = list.Count;
			while (num < count)
			{
				EventDelegate item = list[num];
				if (item != null && item.Equals(ev))
				{
					return;
				}
				num++;
			}
			EventDelegate eventDelegate = new EventDelegate(ev.target, ev.methodName)
			{
				oneShot = oneShot
			};
			if (ev.mParameters != null && (int)ev.mParameters.Length > 0)
			{
				eventDelegate.mParameters = new EventDelegate.Parameter[(int)ev.mParameters.Length];
				for (int i = 0; i < (int)ev.mParameters.Length; i++)
				{
					eventDelegate.mParameters[i] = ev.mParameters[i];
				}
			}
			list.Add(eventDelegate);
		}
	}

	private void Cache()
	{
		this.mCached = true;
		if (this.mRawDelegate)
		{
			return;
		}
		if ((this.mCachedCallback == null || this.mCachedCallback.Target as MonoBehaviour != this.mTarget || EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName) && this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName))
		{
			Type type = this.mTarget.GetType();
			this.mMethod = null;
			while (type != null)
			{
				try
				{
					this.mMethod = type.GetMethod(this.mMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (this.mMethod != null)
					{
						break;
					}
				}
				catch (Exception exception)
				{
				}
				type = type.BaseType;
			}
			if (this.mMethod == null)
			{
				Debug.LogError(string.Concat(new object[] { "Could not find method '", this.mMethodName, "' on ", this.mTarget.GetType() }), this.mTarget);
				return;
			}
			if (this.mMethod.ReturnType != typeof(void))
			{
				Debug.LogError(string.Concat(new object[] { this.mTarget.GetType(), ".", this.mMethodName, " must have a 'void' return type." }), this.mTarget);
				return;
			}
			this.mParameterInfos = this.mMethod.GetParameters();
			if ((int)this.mParameterInfos.Length == 0)
			{
				this.mCachedCallback = (EventDelegate.Callback)Delegate.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget, this.mMethodName);
				this.mArgs = null;
				this.mParameters = null;
				return;
			}
			this.mCachedCallback = null;
			if (this.mParameters == null || (int)this.mParameters.Length != (int)this.mParameterInfos.Length)
			{
				this.mParameters = new EventDelegate.Parameter[(int)this.mParameterInfos.Length];
				int num = 0;
				int length = (int)this.mParameters.Length;
				while (num < length)
				{
					this.mParameters[num] = new EventDelegate.Parameter();
					num++;
				}
			}
			int num1 = 0;
			int length1 = (int)this.mParameters.Length;
			while (num1 < length1)
			{
				this.mParameters[num1].expectedType = this.mParameterInfos[num1].ParameterType;
				num1++;
			}
		}
	}

	public void Clear()
	{
		this.mTarget = null;
		this.mMethodName = null;
		this.mRawDelegate = false;
		this.mCachedCallback = null;
		this.mParameters = null;
		this.mCached = false;
		this.mMethod = null;
		this.mParameterInfos = null;
		this.mArgs = null;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (!(obj is EventDelegate.Callback))
		{
			if (!(obj is EventDelegate))
			{
				return false;
			}
			EventDelegate eventDelegate = obj as EventDelegate;
			return (this.mTarget != eventDelegate.mTarget ? false : string.Equals(this.mMethodName, eventDelegate.mMethodName));
		}
		EventDelegate.Callback callback = obj as EventDelegate.Callback;
		if (callback.Equals(this.mCachedCallback))
		{
			return true;
		}
		MonoBehaviour target = callback.Target as MonoBehaviour;
		return (this.mTarget != target ? false : string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback)));
	}

	public bool Execute()
	{
		if (!this.mCached)
		{
			this.Cache();
		}
		if (this.mCachedCallback != null)
		{
			this.mCachedCallback();
			return true;
		}
		if (this.mMethod == null)
		{
			return false;
		}
		if ((this.mParameters == null ? 0 : (int)this.mParameters.Length) != 0)
		{
			if (this.mArgs == null || (int)this.mArgs.Length != (int)this.mParameters.Length)
			{
				this.mArgs = new object[(int)this.mParameters.Length];
			}
			int num = 0;
			int length = (int)this.mParameters.Length;
			while (num < length)
			{
				this.mArgs[num] = this.mParameters[num].@value;
				num++;
			}
			try
			{
				this.mMethod.Invoke(this.mTarget, this.mArgs);
			}
			catch (ArgumentException argumentException1)
			{
				ArgumentException argumentException = argumentException1;
				string str = "Error calling ";
				if (this.mTarget != null)
				{
					string str1 = str;
					str = string.Concat(new object[] { str1, this.mTarget.GetType(), ".", this.mMethod.Name });
				}
				else
				{
					str = string.Concat(str, this.mMethod.Name);
				}
				str = string.Concat(str, ": ", argumentException.Message);
				str = string.Concat(str, "\n  Expected: ");
				if ((int)this.mParameterInfos.Length != 0)
				{
					str = string.Concat(str, this.mParameterInfos[0]);
					for (int i = 1; i < (int)this.mParameterInfos.Length; i++)
					{
						str = string.Concat(str, ", ", this.mParameterInfos[i].ParameterType);
					}
				}
				else
				{
					str = string.Concat(str, "no arguments");
				}
				str = string.Concat(str, "\n  Received: ");
				if ((int)this.mParameters.Length != 0)
				{
					str = string.Concat(str, this.mParameters[0].type);
					for (int j = 1; j < (int)this.mParameters.Length; j++)
					{
						str = string.Concat(str, ", ", this.mParameters[j].type);
					}
				}
				else
				{
					str = string.Concat(str, "no arguments");
				}
				str = string.Concat(str, "\n");
				Debug.LogError(str);
			}
			int num1 = 0;
			int length1 = (int)this.mArgs.Length;
			while (num1 < length1)
			{
				if (this.mParameterInfos[num1].IsIn || this.mParameterInfos[num1].IsOut)
				{
					this.mParameters[num1].@value = this.mArgs[num1];
				}
				this.mArgs[num1] = null;
				num1++;
			}
		}
		else
		{
			this.mMethod.Invoke(this.mTarget, null);
		}
		return true;
	}

	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			int num = 0;
			while (num < list.Count)
			{
				EventDelegate item = list[num];
				if (item != null)
				{
					try
					{
						item.Execute();
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						if (exception.InnerException == null)
						{
							Debug.LogError(exception.Message);
						}
						else
						{
							Debug.LogError(exception.InnerException.Message);
						}
					}
					if (num >= list.Count)
					{
						break;
					}
					else if (list[num] != item)
					{
						continue;
					}
					else if (item.oneShot)
					{
						list.RemoveAt(num);
						continue;
					}
				}
				num++;
			}
		}
	}

	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	private static string GetMethodName(EventDelegate.Callback callback)
	{
		return callback.Method.Name;
	}

	private static bool IsValid(EventDelegate.Callback callback)
	{
		return (callback == null ? false : callback.Method != null);
	}

	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int num = 0;
			int count = list.Count;
			while (num < count)
			{
				EventDelegate item = list[num];
				if (item != null && item.isValid)
				{
					return true;
				}
				num++;
			}
		}
		return false;
	}

	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int num = 0;
			int count = list.Count;
			while (num < count)
			{
				EventDelegate item = list[num];
				if (item != null && item.Equals(callback))
				{
					list.RemoveAt(num);
					return true;
				}
				num++;
			}
		}
		return false;
	}

	public static bool Remove(List<EventDelegate> list, EventDelegate ev)
	{
		if (list != null)
		{
			int num = 0;
			int count = list.Count;
			while (num < count)
			{
				EventDelegate item = list[num];
				if (item != null && item.Equals(ev))
				{
					list.RemoveAt(num);
					return true;
				}
				num++;
			}
		}
		return false;
	}

	private void Set(EventDelegate.Callback call)
	{
		this.Clear();
		if (call != null && EventDelegate.IsValid(call))
		{
			this.mTarget = call.Target as MonoBehaviour;
			if (this.mTarget != null)
			{
				this.mMethodName = EventDelegate.GetMethodName(call);
				this.mRawDelegate = false;
			}
			else
			{
				this.mRawDelegate = true;
				this.mCachedCallback = call;
				this.mMethodName = null;
			}
		}
	}

	public void Set(MonoBehaviour target, string methodName)
	{
		this.Clear();
		this.mTarget = target;
		this.mMethodName = methodName;
	}

	public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list == null)
		{
			return null;
		}
		EventDelegate eventDelegate = new EventDelegate(callback);
		list.Clear();
		list.Add(eventDelegate);
		return eventDelegate;
	}

	public static void Set(List<EventDelegate> list, EventDelegate del)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(del);
		}
	}

	public override string ToString()
	{
		string str;
		if (this.mTarget == null)
		{
			if (!this.mRawDelegate)
			{
				str = null;
			}
			else
			{
				str = "[delegate]";
			}
			return str;
		}
		string str1 = this.mTarget.GetType().ToString();
		int num = str1.LastIndexOf('.');
		if (num > 0)
		{
			str1 = str1.Substring(num + 1);
		}
		if (string.IsNullOrEmpty(this.methodName))
		{
			return string.Concat(str1, "/[delegate]");
		}
		return string.Concat(str1, "/", this.methodName);
	}

	public delegate void Callback();

	[Serializable]
	public class Parameter
	{
		public UnityEngine.Object obj;

		public string field;

		[NonSerialized]
		private object mValue;

		[NonSerialized]
		public Type expectedType;

		[NonSerialized]
		public bool cached;

		[NonSerialized]
		public PropertyInfo propInfo;

		[NonSerialized]
		public FieldInfo fieldInfo;

		public Type type
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue.GetType();
				}
				if (this.obj == null)
				{
					return typeof(void);
				}
				return this.obj.GetType();
			}
		}

		public object @value
		{
			get
			{
				if (this.mValue != null)
				{
					return this.mValue;
				}
				if (!this.cached)
				{
					this.cached = true;
					this.fieldInfo = null;
					this.propInfo = null;
					if (this.obj != null && !string.IsNullOrEmpty(this.field))
					{
						Type type = this.obj.GetType();
						this.propInfo = type.GetProperty(this.field);
						if (this.propInfo == null)
						{
							this.fieldInfo = type.GetField(this.field);
						}
					}
				}
				if (this.propInfo != null)
				{
					return this.propInfo.GetValue(this.obj, null);
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.GetValue(this.obj);
				}
				if (this.obj != null)
				{
					return this.obj;
				}
				if (this.expectedType != null && this.expectedType.IsValueType)
				{
					return null;
				}
				return Convert.ChangeType(null, this.expectedType);
			}
			set
			{
				this.mValue = value;
			}
		}

		public Parameter()
		{
		}

		public Parameter(UnityEngine.Object obj, string field)
		{
			this.obj = obj;
			this.field = field;
		}

		public Parameter(object val)
		{
			this.mValue = val;
		}
	}
}