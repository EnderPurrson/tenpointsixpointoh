using System;
using UnityEngine;

public class UniPasteBoard
{
	private static AndroidJavaClass _javaClass;

	private static AndroidJavaClass JavaClass
	{
		get
		{
			if (UniPasteBoard._javaClass == null)
			{
				try
				{
					UniPasteBoard._javaClass = new AndroidJavaClass("com.onevcat.UniPasteBoard.PasteBoard");
				}
				catch (Exception exception)
				{
					Debug.Log(exception.ToString());
				}
			}
			return UniPasteBoard._javaClass;
		}
	}

	public UniPasteBoard()
	{
	}

	private static string androidGetClipBoardString()
	{
		string str = null;
		if (UniPasteBoard.JavaClass != null)
		{
			str = UniPasteBoard.JavaClass.CallStatic<string>("getClipBoardString", new object[0]);
		}
		return str;
	}

	private static void androidSetClipBoardString(string text)
	{
		if (UniPasteBoard.JavaClass != null)
		{
			UniPasteBoard.JavaClass.CallStatic("setClipBoardString", new object[] { text });
		}
	}

	public static string GetClipBoardString()
	{
		return UniPasteBoard.androidGetClipBoardString();
	}

	public static void SetClipBoardString(string text)
	{
		UniPasteBoard.androidSetClipBoardString(text);
	}
}