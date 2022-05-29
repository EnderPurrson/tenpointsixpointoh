using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Google.Developers
{
	public class JavaObjWrapper
	{
		private IntPtr raw;

		public IntPtr RawObject
		{
			get
			{
				return this.raw;
			}
		}

		protected JavaObjWrapper()
		{
		}

		public JavaObjWrapper(string clazzName)
		{
			this.raw = AndroidJNI.AllocObject(AndroidJNI.FindClass(clazzName));
		}

		public JavaObjWrapper(IntPtr rawObject)
		{
			this.raw = rawObject;
		}

		protected static jvalue[] ConstructArgArray(object[] theArgs)
		{
			object[] objArray = new object[(int)theArgs.Length];
			for (int i = 0; i < (int)theArgs.Length; i++)
			{
				if (!(theArgs[i] is JavaObjWrapper))
				{
					objArray[i] = theArgs[i];
				}
				else
				{
					objArray[i] = ((JavaObjWrapper)theArgs[i]).raw;
				}
			}
			jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(objArray);
			for (int j = 0; j < (int)theArgs.Length; j++)
			{
				if (theArgs[j] is JavaObjWrapper)
				{
					jvalueArray[j].l = ((JavaObjWrapper)theArgs[j]).raw;
				}
				else if (theArgs[j] is JavaInterfaceProxy)
				{
					IntPtr intPtr = AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy)theArgs[j]);
					jvalueArray[j].l = intPtr;
				}
			}
			if ((int)jvalueArray.Length == 1)
			{
				for (int k = 0; k < (int)jvalueArray.Length; k++)
				{
					Debug.Log(string.Concat(new object[] { "---- [", k, "] -- ", jvalueArray[k].l }));
				}
			}
			return jvalueArray;
		}

		public void CreateInstance(string clazzName, params object[] args)
		{
			if (this.raw != IntPtr.Zero)
			{
				throw new Exception("Java object already set");
			}
			IntPtr intPtr = AndroidJNI.FindClass(clazzName);
			IntPtr constructorID = AndroidJNIHelper.GetConstructorID(intPtr, args);
			jvalue[] jvalueArray = JavaObjWrapper.ConstructArgArray(args);
			this.raw = AndroidJNI.NewObject(intPtr, constructorID, jvalueArray);
		}

		public static float GetStaticFloatField(string clsName, string name)
		{
			IntPtr intPtr = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, name, "F");
			return AndroidJNI.GetStaticFloatField(intPtr, staticFieldID);
		}

		public static int GetStaticIntField(string clsName, string name)
		{
			IntPtr intPtr = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, name, "I");
			return AndroidJNI.GetStaticIntField(intPtr, staticFieldID);
		}

		public static T GetStaticObjectField<T>(string clsName, string name, string sig)
		{
			IntPtr intPtr = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, name, sig);
			IntPtr staticObjectField = AndroidJNI.GetStaticObjectField(intPtr, staticFieldID);
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { staticObjectField.GetType() });
			if (constructor == null)
			{
				return (T)Marshal.PtrToStructure(staticObjectField, typeof(T));
			}
			return (T)constructor.Invoke(new object[] { staticObjectField });
		}

		public static string GetStaticStringField(string clsName, string name)
		{
			IntPtr intPtr = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, name, "Ljava/lang/String;");
			return AndroidJNI.GetStaticStringField(intPtr, staticFieldID);
		}

		public T InvokeCall<T>(string name, string sig, params object[] args)
		{
			Type type = typeof(T);
			IntPtr objectClass = AndroidJNI.GetObjectClass(this.raw);
			IntPtr methodID = AndroidJNI.GetMethodID(objectClass, name, sig);
			jvalue[] jvalueArray = JavaObjWrapper.ConstructArgArray(args);
			if (objectClass == IntPtr.Zero)
			{
				Debug.LogError("Cannot get rawClass object!");
				throw new Exception("Cannot get rawClass object");
			}
			if (methodID == IntPtr.Zero)
			{
				Debug.LogError(string.Concat("Cannot get method for ", name));
				throw new Exception(string.Concat("Cannot get method for ", name));
			}
			if (type == typeof(bool))
			{
				return (T)(object)AndroidJNI.CallBooleanMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(string))
			{
				return (T)AndroidJNI.CallStringMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(int))
			{
				return (T)(object)AndroidJNI.CallIntMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(float))
			{
				return (T)(object)AndroidJNI.CallFloatMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(double))
			{
				return (T)(object)AndroidJNI.CallDoubleMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(byte))
			{
				return (T)(object)AndroidJNI.CallByteMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(char))
			{
				return (T)(object)AndroidJNI.CallCharMethod(this.raw, methodID, jvalueArray);
			}
			if (type == typeof(long))
			{
				return (T)(object)AndroidJNI.CallLongMethod(this.raw, methodID, jvalueArray);
			}
			if (type != typeof(short))
			{
				return this.InvokeObjectCall<T>(name, sig, args);
			}
			return (T)(object)AndroidJNI.CallShortMethod(this.raw, methodID, jvalueArray);
		}

		public void InvokeCallVoid(string name, string sig, params object[] args)
		{
			IntPtr objectClass = AndroidJNI.GetObjectClass(this.raw);
			IntPtr methodID = AndroidJNI.GetMethodID(objectClass, name, sig);
			jvalue[] jvalueArray = JavaObjWrapper.ConstructArgArray(args);
			AndroidJNI.CallVoidMethod(this.raw, methodID, jvalueArray);
		}

		public T InvokeObjectCall<T>(string name, string sig, params object[] theArgs)
		{
			IntPtr objectClass = AndroidJNI.GetObjectClass(this.raw);
			IntPtr methodID = AndroidJNI.GetMethodID(objectClass, name, sig);
			jvalue[] jvalueArray = JavaObjWrapper.ConstructArgArray(theArgs);
			IntPtr intPtr = AndroidJNI.CallObjectMethod(this.raw, methodID, jvalueArray);
			if (intPtr.Equals(IntPtr.Zero))
			{
				return default(T);
			}
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { intPtr.GetType() });
			if (constructor == null)
			{
				return (T)Marshal.PtrToStructure(intPtr, typeof(T));
			}
			return (T)constructor.Invoke(new object[] { intPtr });
		}

		public static T StaticInvokeCall<T>(string type, string name, string sig, params object[] args)
		{
			Type type1 = typeof(T);
			IntPtr intPtr = AndroidJNI.FindClass(type);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(intPtr, name, sig);
			jvalue[] jvalueArray = JavaObjWrapper.ConstructArgArray(args);
			if (type1 == typeof(bool))
			{
				return (T)(object)AndroidJNI.CallStaticBooleanMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(string))
			{
				return (T)AndroidJNI.CallStaticStringMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(int))
			{
				return (T)(object)AndroidJNI.CallStaticIntMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(float))
			{
				return (T)(object)AndroidJNI.CallStaticFloatMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(double))
			{
				return (T)(object)AndroidJNI.CallStaticDoubleMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(byte))
			{
				return (T)(object)AndroidJNI.CallStaticByteMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(char))
			{
				return (T)(object)AndroidJNI.CallStaticCharMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 == typeof(long))
			{
				return (T)(object)AndroidJNI.CallStaticLongMethod(intPtr, staticMethodID, jvalueArray);
			}
			if (type1 != typeof(short))
			{
				return JavaObjWrapper.StaticInvokeObjectCall<T>(type, name, sig, args);
			}
			return (T)(object)AndroidJNI.CallStaticShortMethod(intPtr, staticMethodID, jvalueArray);
		}

		public static void StaticInvokeCallVoid(string type, string name, string sig, params object[] args)
		{
			IntPtr intPtr = AndroidJNI.FindClass(type);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(intPtr, name, sig);
			AndroidJNI.CallStaticVoidMethod(intPtr, staticMethodID, JavaObjWrapper.ConstructArgArray(args));
		}

		public static T StaticInvokeObjectCall<T>(string type, string name, string sig, params object[] args)
		{
			IntPtr intPtr = AndroidJNI.FindClass(type);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(intPtr, name, sig);
			jvalue[] jvalueArray = JavaObjWrapper.ConstructArgArray(args);
			IntPtr intPtr1 = AndroidJNI.CallStaticObjectMethod(intPtr, staticMethodID, jvalueArray);
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { intPtr1.GetType() });
			if (constructor != null)
			{
				return (T)constructor.Invoke(new object[] { intPtr1 });
			}
			if (typeof(T).IsArray)
			{
				return AndroidJNIHelper.ConvertFromJNIArray<T>(intPtr1);
			}
			Debug.Log("Trying cast....");
			return (T)Marshal.PtrToStructure(intPtr1, typeof(T));
		}
	}
}