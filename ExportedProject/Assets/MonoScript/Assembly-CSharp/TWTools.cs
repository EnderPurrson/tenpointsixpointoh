using System;
using UnityEngine;

public static class TWTools
{
	private static void Activate(Transform t)
	{
		TWTools.SetActiveSelf(t.gameObject, true);
		int num = 0;
		int childCount = t.GetChildCount();
		while (num < childCount)
		{
			if (t.GetChild(num).gameObject.activeSelf)
			{
				return;
			}
			num++;
		}
		int num1 = 0;
		int childCount1 = t.GetChildCount();
		while (num1 < childCount1)
		{
			TWTools.Activate(t.GetChild(num1));
			num1++;
		}
	}

	public static GameObject AddChild(GameObject parent)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transforms = gameObject.transform;
			transforms.parent = parent.transform;
			transforms.localPosition = Vector3.zero;
			transforms.localRotation = Quaternion.identity;
			transforms.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		if (gameObject != null && parent != null)
		{
			Transform transforms = gameObject.transform;
			transforms.parent = parent.transform;
			transforms.localPosition = Vector3.zero;
			transforms.localRotation = Quaternion.identity;
			transforms.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	private static void Deactivate(Transform t)
	{
		TWTools.SetActiveSelf(t.gameObject, false);
	}

	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	public static void DestroyImmediate(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (!Application.isEditor)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
	}

	public static void SetActive(GameObject go, bool state)
	{
		if (!state)
		{
			TWTools.Deactivate(go.transform);
		}
		else
		{
			TWTools.Activate(go.transform);
		}
	}

	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}
}