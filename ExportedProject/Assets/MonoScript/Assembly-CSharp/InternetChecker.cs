using System;
using System.IO;
using System.Net;
using UnityEngine;

internal sealed class InternetChecker : MonoBehaviour
{
	public static bool InternetAvailable;

	static InternetChecker()
	{
	}

	public InternetChecker()
	{
	}

	public static void CheckForInternetConn()
	{
		string htmlFromUri = InternetChecker.GetHtmlFromUri("http://google.com");
		if (htmlFromUri == string.Empty)
		{
			InternetChecker.InternetAvailable = false;
		}
		else if (htmlFromUri.Contains("schema.org/WebPage"))
		{
			InternetChecker.InternetAvailable = true;
		}
		else
		{
			InternetChecker.InternetAvailable = false;
		}
	}

	public static string GetHtmlFromUri(string resource)
	{
		string empty;
		string str = string.Empty;
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				if (((int)response.StatusCode >= 299 ? false : response.StatusCode >= HttpStatusCode.OK))
				{
					Debug.Log("Trying to check internet");
					using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
					{
						char[] chrArray = new char[80];
						streamReader.Read(chrArray, 0, (int)chrArray.Length);
						char[] chrArray1 = chrArray;
						for (int i = 0; i < (int)chrArray1.Length; i++)
						{
							str = string.Concat(str, chrArray1[i]);
						}
					}
				}
			}
			return str;
		}
		catch
		{
			empty = string.Empty;
		}
		return empty;
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}