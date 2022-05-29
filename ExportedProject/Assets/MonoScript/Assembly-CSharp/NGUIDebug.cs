using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	private static bool mRayDebug;

	private static List<string> mLines;

	private static NGUIDebug mInstance;

	public static bool debugRaycast
	{
		get
		{
			return NGUIDebug.mRayDebug;
		}
		set
		{
			NGUIDebug.mRayDebug = value;
			if (value && Application.isPlaying)
			{
				NGUIDebug.CreateInstance();
			}
		}
	}

	static NGUIDebug()
	{
		NGUIDebug.mRayDebug = false;
		NGUIDebug.mLines = new List<string>();
		NGUIDebug.mInstance = null;
	}

	public NGUIDebug()
	{
	}

	public static void Clear()
	{
		NGUIDebug.mLines.Clear();
	}

	public static void CreateInstance()
	{
		if (NGUIDebug.mInstance == null)
		{
			GameObject gameObject = new GameObject("_NGUI Debug");
			NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	public static void DrawBounds(Bounds b)
	{
		Vector3 vector3 = b.center;
		Vector3 vector31 = b.center - b.extents;
		Vector3 vector32 = b.center + b.extents;
		Debug.DrawLine(new Vector3(vector31.x, vector31.y, vector3.z), new Vector3(vector32.x, vector31.y, vector3.z), Color.red);
		Debug.DrawLine(new Vector3(vector31.x, vector31.y, vector3.z), new Vector3(vector31.x, vector32.y, vector3.z), Color.red);
		Debug.DrawLine(new Vector3(vector32.x, vector31.y, vector3.z), new Vector3(vector32.x, vector32.y, vector3.z), Color.red);
		Debug.DrawLine(new Vector3(vector31.x, vector32.y, vector3.z), new Vector3(vector32.x, vector32.y, vector3.z), Color.red);
	}

	public static void Log(params object[] objs)
	{
		string empty = string.Empty;
		for (int i = 0; i < (int)objs.Length; i++)
		{
			empty = (i != 0 ? string.Concat(empty, ", ", objs[i].ToString()) : string.Concat(empty, objs[i].ToString()));
		}
		NGUIDebug.LogString(empty);
	}

	private static void LogString(string text)
	{
		if (!Application.isPlaying)
		{
			Debug.Log(text);
		}
		else
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			NGUIDebug.CreateInstance();
		}
	}

	private void OnGUI()
	{
		Rect rect = new Rect(5f, 5f, 1000f, 22f);
		if (NGUIDebug.mRayDebug)
		{
			string str = string.Concat("Scheme: ", UICamera.currentScheme);
			GUI.color = Color.black;
			GUI.Label(rect, str);
			rect.y = rect.y - 1f;
			rect.x = rect.x - 1f;
			GUI.color = Color.white;
			GUI.Label(rect, str);
			rect.y = rect.y + 18f;
			rect.x = rect.x + 1f;
			str = string.Concat("Hover: ", NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", string.Empty));
			GUI.color = Color.black;
			GUI.Label(rect, str);
			rect.y = rect.y - 1f;
			rect.x = rect.x - 1f;
			GUI.color = Color.white;
			GUI.Label(rect, str);
			rect.y = rect.y + 18f;
			rect.x = rect.x + 1f;
			str = string.Concat("Selection: ", NGUITools.GetHierarchy(UICamera.selectedObject).Replace("\"", string.Empty));
			GUI.color = Color.black;
			GUI.Label(rect, str);
			rect.y = rect.y - 1f;
			rect.x = rect.x - 1f;
			GUI.color = Color.white;
			GUI.Label(rect, str);
			rect.y = rect.y + 18f;
			rect.x = rect.x + 1f;
			str = string.Concat("Controller: ", NGUITools.GetHierarchy(UICamera.controllerNavigationObject).Replace("\"", string.Empty));
			GUI.color = Color.black;
			GUI.Label(rect, str);
			rect.y = rect.y - 1f;
			rect.x = rect.x - 1f;
			GUI.color = Color.white;
			GUI.Label(rect, str);
			rect.y = rect.y + 18f;
			rect.x = rect.x + 1f;
			str = string.Concat("Active events: ", UICamera.CountInputSources());
			if (UICamera.disableController)
			{
				str = string.Concat(str, ", disabled controller");
			}
			if (UICamera.inputHasFocus)
			{
				str = string.Concat(str, ", input focus");
			}
			GUI.color = Color.black;
			GUI.Label(rect, str);
			rect.y = rect.y - 1f;
			rect.x = rect.x - 1f;
			GUI.color = Color.white;
			GUI.Label(rect, str);
			rect.y = rect.y + 18f;
			rect.x = rect.x + 1f;
		}
		int num = 0;
		int count = NGUIDebug.mLines.Count;
		while (num < count)
		{
			GUI.color = Color.black;
			GUI.Label(rect, NGUIDebug.mLines[num]);
			rect.y = rect.y - 1f;
			rect.x = rect.x - 1f;
			GUI.color = Color.white;
			GUI.Label(rect, NGUIDebug.mLines[num]);
			rect.y = rect.y + 18f;
			rect.x = rect.x + 1f;
			num++;
		}
	}
}