using System;
using System.IO;
using UnityEngine;

public sealed class SkinsManager
{
	public static string _PathBase
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	public SkinsManager()
	{
	}

	private static void _WriteImageAtPathToGal(string pathToImage)
	{
		try
		{
		}
		catch (Exception exception)
		{
			Debug.Log(string.Concat("Exception in _ScreenshotWriteToAlbum: ", exception));
		}
	}

	public static bool DeleteTexture(string nm)
	{
		try
		{
			File.Delete(Path.Combine(SkinsManager._PathBase, nm));
		}
		catch (Exception exception)
		{
			Debug.Log(exception);
		}
		return true;
	}

	public static byte[] ReadAllBytes(string path)
	{
		byte[] numArray;
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			int num = 0;
			long length = fileStream.Length;
			if (length > (long)2147483647)
			{
				throw new IOException("File is too long.");
			}
			int num1 = (int)length;
			numArray = new byte[num1];
			while (num1 > 0)
			{
				int num2 = fileStream.Read(numArray, num, num1);
				if (num2 == 0)
				{
					throw new EndOfStreamException("Read beyond end of file.");
				}
				num += num2;
				num1 -= num2;
			}
		}
		return numArray;
	}

	public static void SaveTextureToGallery(Texture2D t, string nm)
	{
		SkinsManager._WriteImageAtPathToGal(Path.Combine(SkinsManager._PathBase, nm));
	}

	public static bool SaveTextureWithName(Texture2D t, string nm, bool writeToGallery = true)
	{
		string str = Path.Combine(SkinsManager._PathBase, nm);
		try
		{
			byte[] pNG = t.EncodeToPNG();
			if (File.Exists(str))
			{
				File.Delete(str);
			}
			using (FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(pNG, 0, (int)pNG.Length);
			}
		}
		catch (Exception exception)
		{
			Debug.Log(exception);
		}
		if (writeToGallery)
		{
			SkinsManager._WriteImageAtPathToGal(str);
		}
		return true;
	}

	public static Texture2D TextureForName(string nm, int w = 64, int h = 32, bool disableMimMap = false)
	{
		Texture2D texture2D = (!disableMimMap ? new Texture2D(w, h) : new Texture2D(w, h, TextureFormat.ARGB32, false));
		string str = Path.Combine(SkinsManager._PathBase, nm);
		try
		{
			texture2D.LoadImage(SkinsManager.ReadAllBytes(str));
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Format("Failed to read bytes from {0}\n{1}", str, exception));
		}
		return texture2D;
	}
}