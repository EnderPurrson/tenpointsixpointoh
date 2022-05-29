using System;
using UnityEngine;

public class RotatorKillCam : MonoBehaviour
{
	public static bool isDraggin;

	static RotatorKillCam()
	{
	}

	public RotatorKillCam()
	{
	}

	private void OnDrag(Vector2 delta)
	{
		if (RPG_Camera.instance == null)
		{
			return;
		}
		RPG_Camera.instance.deltaMouseX += delta.x;
	}

	private void OnEnable()
	{
		RotatorKillCam.isDraggin = false;
		this.ReturnCameraToDefaultOrientation();
	}

	private void OnPress(bool isDown)
	{
		RotatorKillCam.isDraggin = isDown;
	}

	private void ReturnCameraToDefaultOrientation()
	{
		if (RPG_Camera.instance == null)
		{
			return;
		}
		RPG_Camera.instance.deltaMouseX = 0f;
		RPG_Camera.instance.mouseY = 15f;
	}

	private void Start()
	{
		RotatorKillCam.isDraggin = false;
		this.ReturnCameraToDefaultOrientation();
	}
}