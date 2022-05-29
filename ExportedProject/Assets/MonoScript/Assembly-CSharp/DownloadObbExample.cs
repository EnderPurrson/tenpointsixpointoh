using System;
using UnityEngine;

public class DownloadObbExample : MonoBehaviour
{
	public DownloadObbExample()
	{
	}

	private void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		string expansionFilePath = GooglePlayDownloader.GetExpansionFilePath();
		if (expansionFilePath != null)
		{
			string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expansionFilePath);
			string patchOBBPath = GooglePlayDownloader.GetPatchOBBPath(expansionFilePath);
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), string.Concat("Main = ...", (mainOBBPath != null ? mainOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE")));
			GUI.Label(new Rect(10f, 25f, (float)(Screen.width - 10), 20f), string.Concat("Patch = ...", (patchOBBPath != null ? patchOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE")));
			if ((mainOBBPath == null || patchOBBPath == null) && GUI.Button(new Rect(10f, 100f, 100f, 100f), "Fetch OBBs"))
			{
				GooglePlayDownloader.FetchOBB();
			}
		}
		else
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "External storage is not available!");
		}
	}
}