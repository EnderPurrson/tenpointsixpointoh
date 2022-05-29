using System;
using UnityEngine;

public class testMem : MonoBehaviour
{
	public testMem()
	{
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, (float)(Screen.height - 50), 180f, 40f), "Get MemInfo"))
		{
			meminfo.getMemInfo();
		}
		if (GUI.Button(new Rect(200f, (float)(Screen.height - 50), 180f, 40f), "native Gc Collect"))
		{
			meminfo.gc_Collect();
		}
		GUI.Label(new Rect(50f, 10f, 250f, 40f), string.Concat("memtotal: ", meminfo.minf.memtotal.ToString(), " kb"));
		GUI.Label(new Rect(50f, 50f, 250f, 40f), string.Concat("memfree: ", meminfo.minf.memfree.ToString(), " kb"));
		GUI.Label(new Rect(50f, 90f, 250f, 40f), string.Concat("active: ", meminfo.minf.active.ToString(), " kb"));
		GUI.Label(new Rect(50f, 130f, 250f, 40f), string.Concat("inactive: ", meminfo.minf.inactive.ToString(), " kb"));
		GUI.Label(new Rect(50f, 170f, 250f, 40f), string.Concat("cached: ", meminfo.minf.cached.ToString(), " kb"));
		GUI.Label(new Rect(50f, 210f, 250f, 40f), string.Concat("swapcached: ", meminfo.minf.swapcached.ToString(), " kb"));
		GUI.Label(new Rect(50f, 250f, 250f, 40f), string.Concat("swaptotal: ", meminfo.minf.swaptotal.ToString(), " kb"));
		GUI.Label(new Rect(50f, 290f, 250f, 40f), string.Concat("swapfree: ", meminfo.minf.swapfree.ToString(), " kb"));
	}

	private void Start()
	{
		if (!meminfo.getMemInfo())
		{
			Debug.Log("Could not get Memory Info");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}