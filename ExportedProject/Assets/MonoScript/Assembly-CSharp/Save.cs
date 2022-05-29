using System;
using UnityEngine;

public class Save : MonoBehaviour
{
	public Save()
	{
	}

	public static void Delete(string name)
	{
		PlayerPrefs.DeleteKey(name);
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public static void SaveBool(string name, bool variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveBoolArray(string name, bool[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveByte(string name, byte variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveByteArray(string name, byte[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveChar(string name, char variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveCharArray(string name, char[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveCharArray(string name, char[] variable, char separator)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), separator.ToString());
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveColor(string name, Color variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new string[] { variable.r.ToString(), "&", variable.g.ToString(), "&", variable.b.ToString(), "&", variable.a.ToString() }));
	}

	public static void SaveColorArray(string name, Color[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			string str = empty;
			empty = string.Concat(new string[] { str, variable[i].r.ToString(), "&", variable[i].g.ToString(), "&", variable[i].b.ToString(), "&", variable[i].a.ToString(), "#" });
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveDecimal(string name, decimal variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveDecimalArray(string name, decimal[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveDouble(string name, double variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveDoubleArray(string name, double[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveFloat(string name, float variable)
	{
		PlayerPrefs.SetFloat(name, variable);
	}

	public static void SaveFloatArray(string name, float[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveInt(string name, int variable)
	{
		PlayerPrefs.SetInt(name, variable);
	}

	public static void SaveIntArray(string name, int[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveKeyCode(string name, KeyCode variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveKeyCodeArray(string name, KeyCode[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveLong(string name, long variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveLongArray(string name, long[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SavePos(string name, GameObject gameObject)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[] { gameObject.transform.position.x, "&", gameObject.transform.position.y, "&", gameObject.transform.position.z }));
	}

	public static void SaveQuaternion(string name, Quaternion variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[] { variable.x, "&", variable.y, "&", variable.z, "&", variable.w }));
	}

	public static void SaveQuaternionArray(string name, Quaternion[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			string str = empty;
			empty = string.Concat(new object[] { str, variable[i].x, "&", variable[i].y, "&", variable[i].z, "&", variable[i].w, "#" });
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveRect(string name, Rect variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new string[] { variable.x.ToString(), "&", variable.y.ToString(), "&", variable.width.ToString(), "&", variable.height.ToString() }));
	}

	public static void SaveRectArray(string name, Rect[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			string str = empty;
			string[] strArrays = new string[] { str, null, null, null, null, null, null, null, null };
			float single = variable[i].x;
			strArrays[1] = single.ToString();
			strArrays[2] = "&";
			float single1 = variable[i].y;
			strArrays[3] = single1.ToString();
			strArrays[4] = "&";
			float single2 = variable[i].width;
			strArrays[5] = single2.ToString();
			strArrays[6] = "&";
			float single3 = variable[i].height;
			strArrays[7] = single3.ToString();
			strArrays[8] = "#";
			empty = string.Concat(strArrays);
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveSByte(string name, sbyte variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveSByteArray(string name, sbyte[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveShort(string name, short variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveShortArray(string name, short[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveString(string name, string variable)
	{
		PlayerPrefs.SetString(name, variable);
	}

	public static void SaveStringArray(string name, string[] variable)
	{
		Debug.Log(string.Concat(new object[] { "SaveStringArray name: ", name, "  variable=", variable }));
		PlayerPrefs.SetString(name, string.Join("#", variable));
	}

	public static void SaveStringArray(string name, string[] variable, char separator)
	{
		PlayerPrefs.SetString(name, string.Join(separator.ToString(), variable));
	}

	public static void SaveTexture2D(string name, Texture2D variable)
	{
		byte[] pNG = variable.EncodeToPNG();
		int num = variable.width;
		int num1 = variable.height;
		PlayerPrefs.SetString(name, string.Concat(new string[] { num.ToString(), "&", num1.ToString(), "&", Convert.ToBase64String(pNG) }));
	}

	public static void SaveUInt(string name, uint variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveUIntArray(string name, uint[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveULong(string name, ulong variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveULongArray(string name, ulong[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveUShort(string name, ushort variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	public static void SaveUShortArray(string name, ushort[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			empty = string.Concat(empty, variable[i].ToString(), "#");
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveVector2(string name, Vector2 variable)
	{
		PlayerPrefs.SetString(name, string.Concat(variable.x, "&", variable.y));
	}

	public static void SaveVector2Array(string name, Vector2[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			string str = empty;
			empty = string.Concat(new object[] { str, variable[i].x, "&", variable[i].y, "#" });
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveVector3(string name, Vector3 variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[] { variable.x, "&", variable.y, "&", variable.z }));
	}

	public static void SaveVector3Array(string name, Vector3[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			string str = empty;
			empty = string.Concat(new object[] { str, variable[i].x, "&", variable[i].y, "&", variable[i].z, "#" });
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}

	public static void SaveVector4(string name, Vector4 variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[] { variable.x, "&", variable.y, "&", variable.z, "&", variable.w }));
	}

	public static void SaveVector4Array(string name, Vector4[] variable)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)variable.Length; i++)
		{
			string str = empty;
			empty = string.Concat(new object[] { str, variable[i].x, "&", variable[i].y, "&", variable[i].z, "&", variable[i].w, "#" });
		}
		PlayerPrefs.SetString(name, empty.ToString());
	}
}