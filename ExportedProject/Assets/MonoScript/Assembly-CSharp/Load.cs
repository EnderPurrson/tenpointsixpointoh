using System;
using UnityEngine;

public sealed class Load : MonoBehaviour
{
	public Load()
	{
	}

	public static bool LoadBool(string name)
	{
		bool flag;
		if (!PlayerPrefs.HasKey(name))
		{
			return false;
		}
		if (bool.TryParse(PlayerPrefs.GetString(name), out flag))
		{
			return flag;
		}
		return false;
	}

	public static bool[] LoadBoolArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		bool[] flagArray = new bool[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			flagArray[i] = bool.Parse(strArrays[i]);
		}
		return flagArray;
	}

	public static byte LoadByte(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return (byte)0;
		}
		return byte.Parse(PlayerPrefs.GetString(name));
	}

	public static byte[] LoadByteArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		byte[] numArray = new byte[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = byte.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static char LoadChar(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return '\0';
		}
		char chr = '\0';
		char.TryParse(PlayerPrefs.GetString(name), out chr);
		return chr;
	}

	public static char[] LoadCharArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		char[] chrArray = new char[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			char.TryParse(strArrays[i], out chrArray[i]);
		}
		return chrArray;
	}

	public static Color LoadColor(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Color(0f, 0f, 0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Color color = new Color(float.Parse(strArrays[0]), float.Parse(strArrays[1]), float.Parse(strArrays[2]), float.Parse(strArrays[3]));
		return color;
	}

	public static Color[] LoadColorArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		Color[] color = new Color[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			string[] strArrays1 = strArrays[i].Split(new char[] { "&"[0] });
			color[i] = new Color(float.Parse(strArrays1[0]), float.Parse(strArrays1[1]), float.Parse(strArrays1[2]), float.Parse(strArrays1[3]));
		}
		return color;
	}

	public static decimal LoadDecimal(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new decimal(0);
		}
		return decimal.Parse(PlayerPrefs.GetString(name));
	}

	public static decimal[] LoadDecimalArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		decimal[] numArray = new decimal[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = decimal.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static double LoadDouble(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return double.Parse(PlayerPrefs.GetString(name));
	}

	public static double[] LoadDoubleArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		double[] numArray = new double[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = double.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static float LoadFloat(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0f;
		}
		return PlayerPrefs.GetFloat(name);
	}

	public static float[] LoadFloatArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		float[] singleArray = new float[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			singleArray[i] = float.Parse(strArrays[i]);
		}
		return singleArray;
	}

	public static int LoadInt(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return PlayerPrefs.GetInt(name);
	}

	public static int[] LoadIntArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		int[] num = new int[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			num[i] = Convert.ToInt32(strArrays[i]);
		}
		return num;
	}

	public static KeyCode LoadKeyCode(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return KeyCode.Space;
		}
		return (KeyCode)((int)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name)));
	}

	public static KeyCode[] LoadKeyCodeArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		KeyCode[] keyCodeArray = new KeyCode[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			keyCodeArray[i] = (KeyCode)((int)Enum.Parse(typeof(KeyCode), strArrays[i]));
		}
		return keyCodeArray;
	}

	public static long LoadLong(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return (long)0;
		}
		return long.Parse(PlayerPrefs.GetString(name));
	}

	public static long[] LoadLongArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		long[] numArray = new long[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = long.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static void LoadPos(string name, GameObject gameObject)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			gameObject.transform.position = new Vector3(0f, 0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Vector3 vector3 = new Vector3(float.Parse(strArrays[0]), float.Parse(strArrays[1]), float.Parse(strArrays[2]));
		gameObject.transform.position = vector3;
	}

	public static Quaternion LoadQuaternion(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Quaternion(0f, 0f, 0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Quaternion quaternion = new Quaternion(float.Parse(strArrays[0]), float.Parse(strArrays[1]), float.Parse(strArrays[2]), float.Parse(strArrays[3]));
		return quaternion;
	}

	public static Quaternion[] LoadQuaternionArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		Quaternion[] quaternion = new Quaternion[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			string[] strArrays1 = strArrays[i].Split(new char[] { "&"[0] });
			quaternion[i] = new Quaternion(float.Parse(strArrays1[0]), float.Parse(strArrays1[1]), float.Parse(strArrays1[2]), float.Parse(strArrays1[3]));
		}
		return quaternion;
	}

	public static Rect LoadRect(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Rect(0f, 0f, 0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Rect rect = new Rect(float.Parse(strArrays[0]), float.Parse(strArrays[1]), float.Parse(strArrays[2]), float.Parse(strArrays[3]));
		return rect;
	}

	public static Rect[] LoadRectArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		Rect[] rect = new Rect[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			string[] strArrays1 = strArrays[i].Split(new char[] { "&"[0] });
			rect[i] = new Rect(float.Parse(strArrays1[0]), float.Parse(strArrays1[1]), float.Parse(strArrays1[2]), float.Parse(strArrays1[3]));
		}
		return rect;
	}

	public static sbyte LoadSByte(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return sbyte.Parse(PlayerPrefs.GetString(name));
	}

	public static sbyte[] LoadSByteArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		sbyte[] numArray = new sbyte[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = sbyte.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static short LoadShort(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return short.Parse(PlayerPrefs.GetString(name));
	}

	public static short[] LoadShortArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		short[] numArray = new short[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = short.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static string LoadString(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return string.Empty;
		}
		return PlayerPrefs.GetString(name);
	}

	public static string[] LoadStringArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			Debug.Log(string.Concat("LoadStringArray(): Cannot find key ", name));
			return null;
		}
		return PlayerPrefs.GetString(name).Split(new char[] { '#' });
	}

	public static string[] LoadStringArray(string name, char separator)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		return PlayerPrefs.GetString(name).Split(new char[] { separator });
	}

	public static Texture2D LoadTexture2D(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		byte[] numArray = Convert.FromBase64String(strArrays[2]);
		Texture2D texture2D = new Texture2D(int.Parse(strArrays[0]), int.Parse(strArrays[1]));
		texture2D.LoadImage(numArray);
		return texture2D;
	}

	public static uint LoadUInt(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return (uint)0;
		}
		return uint.Parse(PlayerPrefs.GetString(name));
	}

	public static uint[] LoadUIntArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		uint[] num = new uint[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			num[i] = Convert.ToUInt32(strArrays[i]);
		}
		return num;
	}

	public static ulong LoadULong(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return (ulong)0;
		}
		return ulong.Parse(PlayerPrefs.GetString(name));
	}

	public static ulong[] LoadULongArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		ulong[] numArray = new ulong[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = ulong.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static ushort LoadUShort(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return (ushort)0;
		}
		return ushort.Parse(PlayerPrefs.GetString(name));
	}

	public static ushort[] LoadUShortArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		ushort[] numArray = new ushort[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			numArray[i] = ushort.Parse(strArrays[i]);
		}
		return numArray;
	}

	public static Vector2 LoadVector2(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Vector2(0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Vector2 vector2 = new Vector2(float.Parse(strArrays[0]), float.Parse(strArrays[1]));
		return vector2;
	}

	public static Vector2[] LoadVector2Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		Vector2[] vector2 = new Vector2[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			string[] strArrays1 = strArrays[i].Split(new char[] { "&"[0] });
			vector2[i] = new Vector2(float.Parse(strArrays1[0]), float.Parse(strArrays1[1]));
		}
		return vector2;
	}

	public static Vector3 LoadVector3(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Vector3(0f, 0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Vector3 vector3 = new Vector3(float.Parse(strArrays[0]), float.Parse(strArrays[1]), float.Parse(strArrays[2]));
		return vector3;
	}

	public static Vector3[] LoadVector3Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { '#' });
		Vector3[] vector3 = new Vector3[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			string[] strArrays1 = strArrays[i].Split(new char[] { '&' });
			vector3[i] = new Vector3(float.Parse(strArrays1[0]), float.Parse(strArrays1[1]), float.Parse(strArrays1[2]));
		}
		return vector3;
	}

	public static Vector4 LoadVector4(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Vector4(0f, 0f, 0f, 0f);
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "&"[0] });
		Vector4 vector4 = new Vector4(float.Parse(strArrays[0]), float.Parse(strArrays[1]), float.Parse(strArrays[2]), float.Parse(strArrays[3]));
		return vector4;
	}

	public static Vector4[] LoadVector4Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] strArrays = PlayerPrefs.GetString(name).Split(new char[] { "#"[0] });
		Vector4[] vector4 = new Vector4[(int)strArrays.Length - 1];
		for (int i = 0; i < (int)strArrays.Length - 1; i++)
		{
			string[] strArrays1 = strArrays[i].Split(new char[] { "&"[0] });
			vector4[i] = new Vector4(float.Parse(strArrays1[0]), float.Parse(strArrays1[1]), float.Parse(strArrays1[2]), float.Parse(strArrays1[3]));
		}
		return vector4;
	}
}