using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class EveryplayHudCamera : MonoBehaviour
{
	protected const int EPSR = 1162892114;

	public EveryplayHudCamera()
	{
	}

	private void Awake()
	{
		EveryplayHudCamera.EveryplayUnityPluginInterfaceInitialize();
	}

	[DllImport("everyplay", CharSet=CharSet.None, ExactSpelling=false)]
	private static extern void EveryplayUnityPluginInterfaceInitialize();

	private void OnPreRender()
	{
		GL.IssuePluginEvent(1162892114);
	}
}