using System;
using UnityEngine;

public class posNGUI : MonoBehaviour
{
	private static UIRoot rootObj;

	public static float scaleNGUI;

	public static float heightScreen;

	public static float nachY;

	public posNGUI()
	{
	}

	private void Awake()
	{
		posNGUI.rootObj = base.GetComponent<UIRoot>();
		posNGUI.heightScreen = (float)Mathf.Min(1366, Mathf.RoundToInt(768f * (float)Screen.height / (float)Screen.width));
		posNGUI.scaleNGUI = 2f / posNGUI.heightScreen;
		posNGUI.nachY = posNGUI.rootObj.transform.position.y;
		posNGUI.rootObj.gameObject.transform.localScale = new Vector3(posNGUI.scaleNGUI, posNGUI.scaleNGUI, posNGUI.scaleNGUI);
	}

	public static Vector3 getEulerZ(float tekUgol)
	{
		return new Vector3(0f, 0f, tekUgol);
	}

	public static Vector3 getPosNGUI(Vector3 tekPos)
	{
		return new Vector3(posNGUI.getPosX(tekPos.x), posNGUI.getPosY(tekPos.y), tekPos.z * posNGUI.scaleNGUI);
	}

	public static float getPosX(float tekPosX)
	{
		return (tekPosX - 384f) * posNGUI.scaleNGUI;
	}

	public static float getPosY(float tekPosY)
	{
		return posNGUI.nachY + (-tekPosY + posNGUI.heightScreen * 0.5f) * posNGUI.scaleNGUI;
	}

	public static Vector3 getSize(Vector3 tekSize)
	{
		return new Vector3(tekSize.x * posNGUI.scaleNGUI, tekSize.y * posNGUI.scaleNGUI, tekSize.z * posNGUI.scaleNGUI);
	}

	public static float getSizeHeight(float tekHeight)
	{
		return tekHeight * posNGUI.scaleNGUI;
	}

	public static float getSizeWidth(float tekWidth)
	{
		return tekWidth * posNGUI.scaleNGUI;
	}

	public static void setFillRect(GameObject thisGameObj)
	{
		thisGameObj.transform.localScale = new Vector3(770f, posNGUI.heightScreen + 2f, 1f);
	}
}