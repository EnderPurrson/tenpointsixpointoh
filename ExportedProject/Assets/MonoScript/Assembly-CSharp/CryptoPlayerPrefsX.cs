using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoPlayerPrefsX
{
	private static int endianDiff1;

	private static int endianDiff2;

	private static int idx;

	private static byte[] byteBlock;

	public CryptoPlayerPrefsX()
	{
	}

	private static float ConvertBytesToFloat(byte[] bytes)
	{
		CryptoPlayerPrefsX.ConvertFrom4Bytes(bytes);
		return BitConverter.ToSingle(CryptoPlayerPrefsX.byteBlock, 0);
	}

	private static int ConvertBytesToInt32(byte[] bytes)
	{
		CryptoPlayerPrefsX.ConvertFrom4Bytes(bytes);
		return BitConverter.ToInt32(CryptoPlayerPrefsX.byteBlock, 0);
	}

	private static void ConvertFloatToBytes(float f, byte[] bytes)
	{
		CryptoPlayerPrefsX.byteBlock = BitConverter.GetBytes(f);
		CryptoPlayerPrefsX.ConvertTo4Bytes(bytes);
	}

	private static void ConvertFrom4Bytes(byte[] bytes)
	{
		CryptoPlayerPrefsX.byteBlock[CryptoPlayerPrefsX.endianDiff1] = bytes[CryptoPlayerPrefsX.idx];
		CryptoPlayerPrefsX.byteBlock[1 + CryptoPlayerPrefsX.endianDiff2] = bytes[CryptoPlayerPrefsX.idx + 1];
		CryptoPlayerPrefsX.byteBlock[2 - CryptoPlayerPrefsX.endianDiff2] = bytes[CryptoPlayerPrefsX.idx + 2];
		CryptoPlayerPrefsX.byteBlock[3 - CryptoPlayerPrefsX.endianDiff1] = bytes[CryptoPlayerPrefsX.idx + 3];
		CryptoPlayerPrefsX.idx += 4;
	}

	private static void ConvertFromColor(Color[] array, byte[] bytes, int i)
	{
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].r, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].g, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].b, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].a, bytes);
	}

	private static void ConvertFromFloat(float[] array, byte[] bytes, int i)
	{
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i], bytes);
	}

	private static void ConvertFromInt(int[] array, byte[] bytes, int i)
	{
		CryptoPlayerPrefsX.ConvertInt32ToBytes(array[i], bytes);
	}

	private static void ConvertFromQuaternion(Quaternion[] array, byte[] bytes, int i)
	{
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].z, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].w, bytes);
	}

	private static void ConvertFromVector2(Vector2[] array, byte[] bytes, int i)
	{
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
	}

	private static void ConvertFromVector3(Vector3[] array, byte[] bytes, int i)
	{
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].x, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].y, bytes);
		CryptoPlayerPrefsX.ConvertFloatToBytes(array[i].z, bytes);
	}

	private static void ConvertInt32ToBytes(int i, byte[] bytes)
	{
		CryptoPlayerPrefsX.byteBlock = BitConverter.GetBytes(i);
		CryptoPlayerPrefsX.ConvertTo4Bytes(bytes);
	}

	private static void ConvertTo4Bytes(byte[] bytes)
	{
		bytes[CryptoPlayerPrefsX.idx] = CryptoPlayerPrefsX.byteBlock[CryptoPlayerPrefsX.endianDiff1];
		bytes[CryptoPlayerPrefsX.idx + 1] = CryptoPlayerPrefsX.byteBlock[1 + CryptoPlayerPrefsX.endianDiff2];
		bytes[CryptoPlayerPrefsX.idx + 2] = CryptoPlayerPrefsX.byteBlock[2 - CryptoPlayerPrefsX.endianDiff2];
		bytes[CryptoPlayerPrefsX.idx + 3] = CryptoPlayerPrefsX.byteBlock[3 - CryptoPlayerPrefsX.endianDiff1];
		CryptoPlayerPrefsX.idx += 4;
	}

	private static void ConvertToColor(List<Color> list, byte[] bytes)
	{
		list.Add(new Color(CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToFloat(List<float> list, byte[] bytes)
	{
		list.Add(CryptoPlayerPrefsX.ConvertBytesToFloat(bytes));
	}

	private static void ConvertToInt(List<int> list, byte[] bytes)
	{
		list.Add(CryptoPlayerPrefsX.ConvertBytesToInt32(bytes));
	}

	private static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
	{
		list.Add(new Quaternion(CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToVector2(List<Vector2> list, byte[] bytes)
	{
		list.Add(new Vector2(CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToVector3(List<Vector3> list, byte[] bytes)
	{
		list.Add(new Vector3(CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes), CryptoPlayerPrefsX.ConvertBytesToFloat(bytes)));
	}

	public static bool GetBool(string name)
	{
		return CryptoPlayerPrefs.GetInt(name, 0) == 1;
	}

	public static bool GetBool(string name, bool defaultValue)
	{
		if (!CryptoPlayerPrefs.HasKey(name))
		{
			return defaultValue;
		}
		return CryptoPlayerPrefsX.GetBool(name);
	}

	public static bool[] GetBoolArray(string key)
	{
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return new bool[0];
		}
		byte[] numArray = Convert.FromBase64String(CryptoPlayerPrefs.GetString(key, string.Empty));
		if ((int)numArray.Length < 6)
		{
			Debug.LogError(string.Concat("Corrupt preference file for ", key));
			return new bool[0];
		}
		if (numArray[0] != 2)
		{
			Debug.LogError(string.Concat(key, " is not a boolean array"));
			return new bool[0];
		}
		CryptoPlayerPrefsX.Initialize();
		byte[] numArray1 = new byte[(int)numArray.Length - 5];
		Array.Copy(numArray, 5, numArray1, 0, (int)numArray1.Length);
		BitArray bitArrays = new BitArray(numArray1)
		{
			Length = CryptoPlayerPrefsX.ConvertBytesToInt32(numArray)
		};
		bool[] flagArray = new bool[bitArrays.Count];
		bitArrays.CopyTo(flagArray, 0);
		return flagArray;
	}

	public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetBoolArray(key);
		}
		bool[] flagArray = new bool[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			flagArray[i] = defaultValue;
		}
		return flagArray;
	}

	public static Color GetColor(string key)
	{
		float[] floatArray = CryptoPlayerPrefsX.GetFloatArray(key);
		if ((int)floatArray.Length < 4)
		{
			return new Color(0f, 0f, 0f, 0f);
		}
		return new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	}

	public static Color GetColor(string key, Color defaultValue)
	{
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return defaultValue;
		}
		return CryptoPlayerPrefsX.GetColor(key);
	}

	public static Color[] GetColorArray(string key)
	{
		List<Color> colors = new List<Color>();
		CryptoPlayerPrefsX.GetValue<List<Color>>(key, colors, CryptoPlayerPrefsX.ArrayType.Color, 4, new Action<List<Color>, byte[]>(CryptoPlayerPrefsX.ConvertToColor));
		return colors.ToArray();
	}

	public static Color[] GetColorArray(string key, Color defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetColorArray(key);
		}
		Color[] colorArray = new Color[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			colorArray[i] = defaultValue;
		}
		return colorArray;
	}

	public static float[] GetFloatArray(string key)
	{
		List<float> singles = new List<float>();
		CryptoPlayerPrefsX.GetValue<List<float>>(key, singles, CryptoPlayerPrefsX.ArrayType.Float, 1, new Action<List<float>, byte[]>(CryptoPlayerPrefsX.ConvertToFloat));
		return singles.ToArray();
	}

	public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetFloatArray(key);
		}
		float[] singleArray = new float[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			singleArray[i] = defaultValue;
		}
		return singleArray;
	}

	public static int[] GetIntArray(string key)
	{
		List<int> nums = new List<int>();
		CryptoPlayerPrefsX.GetValue<List<int>>(key, nums, CryptoPlayerPrefsX.ArrayType.Int32, 1, new Action<List<int>, byte[]>(CryptoPlayerPrefsX.ConvertToInt));
		return nums.ToArray();
	}

	public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetIntArray(key);
		}
		int[] numArray = new int[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			numArray[i] = defaultValue;
		}
		return numArray;
	}

	public static Quaternion GetQuaternion(string key)
	{
		float[] floatArray = CryptoPlayerPrefsX.GetFloatArray(key);
		if ((int)floatArray.Length < 4)
		{
			return Quaternion.identity;
		}
		return new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
	}

	public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
	{
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return defaultValue;
		}
		return CryptoPlayerPrefsX.GetQuaternion(key);
	}

	public static Quaternion[] GetQuaternionArray(string key)
	{
		List<Quaternion> quaternions = new List<Quaternion>();
		CryptoPlayerPrefsX.GetValue<List<Quaternion>>(key, quaternions, CryptoPlayerPrefsX.ArrayType.Quaternion, 4, new Action<List<Quaternion>, byte[]>(CryptoPlayerPrefsX.ConvertToQuaternion));
		return quaternions.ToArray();
	}

	public static Quaternion[] GetQuaternionArray(string key, Quaternion defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetQuaternionArray(key);
		}
		Quaternion[] quaternionArray = new Quaternion[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			quaternionArray[i] = defaultValue;
		}
		return quaternionArray;
	}

	public static string[] GetStringArray(string key)
	{
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return new string[0];
		}
		string str = CryptoPlayerPrefs.GetString(key, string.Empty);
		int num = str.IndexOf("|"[0]);
		if (num < 4)
		{
			Debug.LogError(string.Concat("Corrupt preference file for ", key));
			return new string[0];
		}
		byte[] numArray = Convert.FromBase64String(str.Substring(0, num));
		if (numArray[0] != 3)
		{
			Debug.LogError(string.Concat(key, " is not a string array"));
			return new string[0];
		}
		CryptoPlayerPrefsX.Initialize();
		int length = (int)numArray.Length - 1;
		string[] strArrays = new string[length];
		int num1 = num + 1;
		for (int i = 0; i < length; i++)
		{
			int num2 = CryptoPlayerPrefsX.idx;
			CryptoPlayerPrefsX.idx = num2 + 1;
			int num3 = numArray[num2];
			if (num1 + num3 > str.Length)
			{
				Debug.LogError(string.Concat("Corrupt preference file for ", key));
				return new string[0];
			}
			strArrays[i] = str.Substring(num1, num3);
			num1 += num3;
		}
		return strArrays;
	}

	public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetStringArray(key);
		}
		string[] strArrays = new string[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			strArrays[i] = defaultValue;
		}
		return strArrays;
	}

	private static void GetValue<T>(string key, T list, CryptoPlayerPrefsX.ArrayType arrayType, int vectorNumber, Action<T, byte[]> convert)
	where T : IList
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			byte[] numArray = Convert.FromBase64String(CryptoPlayerPrefs.GetString(key, string.Empty));
			if (((int)numArray.Length - 1) % (vectorNumber * 4) != 0)
			{
				Debug.LogError(string.Concat("Corrupt preference file for ", key));
				return;
			}
			if (numArray[0] != (byte)arrayType)
			{
				Debug.LogError(string.Concat(key, " is not a ", arrayType.ToString(), " array"));
				return;
			}
			CryptoPlayerPrefsX.Initialize();
			int length = ((int)numArray.Length - 1) / (vectorNumber * 4);
			for (int i = 0; i < length; i++)
			{
				convert(list, numArray);
			}
		}
	}

	private static Vector2 GetVector2(string key)
	{
		float[] floatArray = CryptoPlayerPrefsX.GetFloatArray(key);
		if ((int)floatArray.Length < 2)
		{
			return Vector2.zero;
		}
		return new Vector2(floatArray[0], floatArray[1]);
	}

	public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return defaultValue;
		}
		return CryptoPlayerPrefsX.GetVector2(key);
	}

	public static Vector2[] GetVector2Array(string key)
	{
		List<Vector2> vector2s = new List<Vector2>();
		CryptoPlayerPrefsX.GetValue<List<Vector2>>(key, vector2s, CryptoPlayerPrefsX.ArrayType.Vector2, 2, new Action<List<Vector2>, byte[]>(CryptoPlayerPrefsX.ConvertToVector2));
		return vector2s.ToArray();
	}

	public static Vector2[] GetVector2Array(string key, Vector2 defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetVector2Array(key);
		}
		Vector2[] vector2Array = new Vector2[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			vector2Array[i] = defaultValue;
		}
		return vector2Array;
	}

	public static Vector3 GetVector3(string key)
	{
		float[] floatArray = CryptoPlayerPrefsX.GetFloatArray(key);
		if ((int)floatArray.Length < 3)
		{
			return Vector3.zero;
		}
		return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
	}

	public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
		if (!CryptoPlayerPrefs.HasKey(key))
		{
			return defaultValue;
		}
		return CryptoPlayerPrefsX.GetVector3(key);
	}

	public static Vector3[] GetVector3Array(string key)
	{
		List<Vector3> vector3s = new List<Vector3>();
		CryptoPlayerPrefsX.GetValue<List<Vector3>>(key, vector3s, CryptoPlayerPrefsX.ArrayType.Vector3, 3, new Action<List<Vector3>, byte[]>(CryptoPlayerPrefsX.ConvertToVector3));
		return vector3s.ToArray();
	}

	public static Vector3[] GetVector3Array(string key, Vector3 defaultValue, int defaultSize)
	{
		if (CryptoPlayerPrefs.HasKey(key))
		{
			return CryptoPlayerPrefsX.GetVector3Array(key);
		}
		Vector3[] vector3Array = new Vector3[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			vector3Array[i] = defaultValue;
		}
		return vector3Array;
	}

	private static void Initialize()
	{
		if (!BitConverter.IsLittleEndian)
		{
			CryptoPlayerPrefsX.endianDiff1 = 3;
			CryptoPlayerPrefsX.endianDiff2 = 1;
		}
		else
		{
			CryptoPlayerPrefsX.endianDiff1 = 0;
			CryptoPlayerPrefsX.endianDiff2 = 0;
		}
		if (CryptoPlayerPrefsX.byteBlock == null)
		{
			CryptoPlayerPrefsX.byteBlock = new byte[4];
		}
		CryptoPlayerPrefsX.idx = 1;
	}

	private static bool SaveBytes(string key, byte[] bytes)
	{
		bool flag;
		try
		{
			CryptoPlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
			return true;
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	public static bool SetBool(string name, bool value)
	{
		bool flag;
		try
		{
			CryptoPlayerPrefs.SetInt(name, (!value ? 0 : 1));
			return true;
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	public static bool SetBoolArray(string key, bool[] boolArray)
	{
		if ((int)boolArray.Length == 0)
		{
			Debug.LogError(string.Concat("The bool array cannot have 0 entries when setting ", key));
			return false;
		}
		byte[] num = new byte[((int)boolArray.Length + 7) / 8 + 5];
		num[0] = Convert.ToByte(CryptoPlayerPrefsX.ArrayType.Bool);
		(new BitArray(boolArray)).CopyTo(num, 5);
		CryptoPlayerPrefsX.Initialize();
		CryptoPlayerPrefsX.ConvertInt32ToBytes((int)boolArray.Length, num);
		return CryptoPlayerPrefsX.SaveBytes(key, num);
	}

	public static bool SetColor(string key, Color color)
	{
		return CryptoPlayerPrefsX.SetFloatArray(key, new float[] { color.r, color.g, color.b, color.a });
	}

	public static bool SetColorArray(string key, Color[] colorArray)
	{
		return CryptoPlayerPrefsX.SetValue<Color[]>(key, colorArray, CryptoPlayerPrefsX.ArrayType.Color, 4, new Action<Color[], byte[], int>(CryptoPlayerPrefsX.ConvertFromColor));
	}

	public static bool SetFloatArray(string key, float[] floatArray)
	{
		return CryptoPlayerPrefsX.SetValue<float[]>(key, floatArray, CryptoPlayerPrefsX.ArrayType.Float, 1, new Action<float[], byte[], int>(CryptoPlayerPrefsX.ConvertFromFloat));
	}

	public static bool SetIntArray(string key, int[] intArray)
	{
		return CryptoPlayerPrefsX.SetValue<int[]>(key, intArray, CryptoPlayerPrefsX.ArrayType.Int32, 1, new Action<int[], byte[], int>(CryptoPlayerPrefsX.ConvertFromInt));
	}

	public static bool SetQuaternion(string key, Quaternion vector)
	{
		return CryptoPlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y, vector.z, vector.w });
	}

	public static bool SetQuaternionArray(string key, Quaternion[] quaternionArray)
	{
		return CryptoPlayerPrefsX.SetValue<Quaternion[]>(key, quaternionArray, CryptoPlayerPrefsX.ArrayType.Quaternion, 4, new Action<Quaternion[], byte[], int>(CryptoPlayerPrefsX.ConvertFromQuaternion));
	}

	public static bool SetStringArray(string key, string[] stringArray)
	{
		bool flag;
		if ((int)stringArray.Length == 0)
		{
			Debug.LogError(string.Concat("The string array cannot have 0 entries when setting ", key));
			return false;
		}
		byte[] num = new byte[(int)stringArray.Length + 1];
		num[0] = Convert.ToByte(CryptoPlayerPrefsX.ArrayType.String);
		CryptoPlayerPrefsX.Initialize();
		for (int i = 0; i < (int)stringArray.Length; i++)
		{
			if (stringArray[i] == null)
			{
				Debug.LogError(string.Concat("Can't save null entries in the string array when setting ", key));
				return false;
			}
			if (stringArray[i].Length > 255)
			{
				Debug.LogError(string.Concat("Strings cannot be longer than 255 characters when setting ", key));
				return false;
			}
			int num1 = CryptoPlayerPrefsX.idx;
			CryptoPlayerPrefsX.idx = num1 + 1;
			num[num1] = (byte)stringArray[i].Length;
		}
		try
		{
			CryptoPlayerPrefs.SetString(key, string.Concat(Convert.ToBase64String(num), "|", string.Join(string.Empty, stringArray)));
			return true;
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	private static bool SetValue<T>(string key, T array, CryptoPlayerPrefsX.ArrayType arrayType, int vectorNumber, Action<T, byte[], int> convert)
	where T : IList
	{
		if (array.Count == 0)
		{
			Debug.LogError(string.Concat("The ", arrayType.ToString(), " array cannot have 0 entries when setting ", key));
			return false;
		}
		byte[] num = new byte[4 * array.Count * vectorNumber + 1];
		num[0] = Convert.ToByte(arrayType);
		CryptoPlayerPrefsX.Initialize();
		for (int i = 0; i < array.Count; i++)
		{
			convert(array, num, i);
		}
		return CryptoPlayerPrefsX.SaveBytes(key, num);
	}

	public static bool SetVector2(string key, Vector2 vector)
	{
		return CryptoPlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y });
	}

	public static bool SetVector2Array(string key, Vector2[] vector2Array)
	{
		return CryptoPlayerPrefsX.SetValue<Vector2[]>(key, vector2Array, CryptoPlayerPrefsX.ArrayType.Vector2, 2, new Action<Vector2[], byte[], int>(CryptoPlayerPrefsX.ConvertFromVector2));
	}

	public static bool SetVector3(string key, Vector3 vector)
	{
		return CryptoPlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y, vector.z });
	}

	public static bool SetVector3Array(string key, Vector3[] vector3Array)
	{
		return CryptoPlayerPrefsX.SetValue<Vector3[]>(key, vector3Array, CryptoPlayerPrefsX.ArrayType.Vector3, 3, new Action<Vector3[], byte[], int>(CryptoPlayerPrefsX.ConvertFromVector3));
	}

	public static void ShowArrayType(string key)
	{
		byte[] numArray = Convert.FromBase64String(CryptoPlayerPrefs.GetString(key, string.Empty));
		if ((int)numArray.Length > 0)
		{
			CryptoPlayerPrefsX.ArrayType arrayType = (CryptoPlayerPrefsX.ArrayType)numArray[0];
			Debug.Log(string.Concat(key, " is a ", arrayType.ToString(), " array"));
		}
	}

	private enum ArrayType
	{
		Float,
		Int32,
		Bool,
		String,
		Vector2,
		Vector3,
		Quaternion,
		Color
	}
}