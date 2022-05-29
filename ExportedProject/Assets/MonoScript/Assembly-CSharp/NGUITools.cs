using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class NGUITools
{
	private static AudioListener mListener;

	private static bool mLoaded;

	private static float mGlobalVolume;

	private static float mLastTimestamp;

	private static AudioClip mLastClip;

	private static Vector3[] mSides;

	public static KeyCode[] keys;

	public static string clipboard
	{
		get
		{
			TextEditor textEditor = new TextEditor();
			textEditor.Paste();
			return textEditor.content.text;
		}
		set
		{
			TextEditor textEditor = new TextEditor()
			{
				content = new GUIContent(value)
			};
			textEditor.OnFocus();
			textEditor.Copy();
		}
	}

	public static bool fileAccess
	{
		get
		{
			return (Application.platform == RuntimePlatform.WindowsWebPlayer ? false : Application.platform != RuntimePlatform.OSXWebPlayer);
		}
	}

	public static Vector2 screenSize
	{
		get
		{
			return new Vector2((float)Screen.width, (float)Screen.height);
		}
	}

	public static float soundVolume
	{
		get
		{
			if (!NGUITools.mLoaded)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
			}
			return NGUITools.mGlobalVolume;
		}
		set
		{
			if (NGUITools.mGlobalVolume != value)
			{
				NGUITools.mLoaded = true;
				NGUITools.mGlobalVolume = value;
				PlayerPrefs.SetFloat("Sound", value);
			}
		}
	}

	static NGUITools()
	{
		NGUITools.mLoaded = false;
		NGUITools.mGlobalVolume = 1f;
		NGUITools.mLastTimestamp = 0f;
		NGUITools.mSides = new Vector3[4];
		NGUITools.keys = new KeyCode[] { KeyCode.Backspace, KeyCode.Tab, KeyCode.Clear, KeyCode.Return, KeyCode.Pause, KeyCode.Escape, KeyCode.Space, KeyCode.Exclaim, KeyCode.DoubleQuote, KeyCode.Hash, KeyCode.Dollar, KeyCode.Ampersand, KeyCode.Quote, KeyCode.LeftParen, KeyCode.RightParen, KeyCode.Asterisk, KeyCode.Plus, KeyCode.Comma, KeyCode.Minus, KeyCode.Period, KeyCode.Slash, KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Colon, KeyCode.Semicolon, KeyCode.Less, KeyCode.Equals, KeyCode.Greater, KeyCode.Question, KeyCode.At, KeyCode.LeftBracket, KeyCode.Backslash, KeyCode.RightBracket, KeyCode.Caret, KeyCode.Underscore, KeyCode.BackQuote, KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.Delete, KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.KeypadPeriod, KeyCode.KeypadDivide, KeyCode.KeypadMultiply, KeyCode.KeypadMinus, KeyCode.KeypadPlus, KeyCode.KeypadEnter, KeyCode.KeypadEquals, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Insert, KeyCode.Home, KeyCode.End, KeyCode.PageUp, KeyCode.PageDown, KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12, KeyCode.F13, KeyCode.F14, KeyCode.F15, KeyCode.Numlock, KeyCode.CapsLock, KeyCode.ScrollLock, KeyCode.RightShift, KeyCode.LeftShift, KeyCode.RightControl, KeyCode.LeftControl, KeyCode.RightAlt, KeyCode.LeftAlt, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6, KeyCode.JoystickButton0, KeyCode.JoystickButton1, KeyCode.JoystickButton2, KeyCode.JoystickButton3, KeyCode.JoystickButton4, KeyCode.JoystickButton5, KeyCode.JoystickButton6, KeyCode.JoystickButton7, KeyCode.JoystickButton8, KeyCode.JoystickButton9, KeyCode.JoystickButton10, KeyCode.JoystickButton11, KeyCode.JoystickButton12, KeyCode.JoystickButton13, KeyCode.JoystickButton14, KeyCode.JoystickButton15, KeyCode.JoystickButton16, KeyCode.JoystickButton17, KeyCode.JoystickButton18, KeyCode.JoystickButton19 };
	}

	private static void Activate(Transform t)
	{
		NGUITools.Activate(t, false);
	}

	private static void Activate(Transform t, bool compatibilityMode)
	{
		NGUITools.SetActiveSelf(t.gameObject, true);
		if (compatibilityMode)
		{
			int num = 0;
			int num1 = t.childCount;
			while (num < num1)
			{
				if (t.GetChild(num).gameObject.activeSelf)
				{
					return;
				}
				num++;
			}
			int num2 = 0;
			int num3 = t.childCount;
			while (num2 < num3)
			{
				NGUITools.Activate(t.GetChild(num2), true);
				num2++;
			}
		}
	}

	public static GameObject AddChild(GameObject parent)
	{
		return NGUITools.AddChild(parent, true);
	}

	public static GameObject AddChild(GameObject parent, bool undo)
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

	public static T AddChild<T>(GameObject parent)
	where T : Component
	{
		GameObject typeName = NGUITools.AddChild(parent);
		typeName.name = NGUITools.GetTypeName<T>();
		return typeName.AddComponent<T>();
	}

	public static T AddChild<T>(GameObject parent, bool undo)
	where T : Component
	{
		GameObject typeName = NGUITools.AddChild(parent, undo);
		typeName.name = NGUITools.GetTypeName<T>();
		return typeName.AddComponent<T>();
	}

	public static T AddMissingComponent<T>(this GameObject go)
	where T : Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
		{
			component = go.AddComponent<T>();
		}
		return component;
	}

	public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName, int depth = 2147483647)
	{
		UISpriteData sprite;
		if (atlas == null)
		{
			sprite = null;
		}
		else
		{
			sprite = atlas.GetSprite(spriteName);
		}
		UISpriteData uISpriteDatum = sprite;
		UISprite uISprite = NGUITools.AddWidget<UISprite>(go, depth);
		uISprite.type = (uISpriteDatum == null || !uISpriteDatum.hasBorder ? UIBasicSprite.Type.Simple : UIBasicSprite.Type.Sliced);
		uISprite.atlas = atlas;
		uISprite.spriteName = spriteName;
		return uISprite;
	}

	public static T AddWidget<T>(GameObject go, int depth = 2147483647)
	where T : UIWidget
	{
		if (depth == 2147483647)
		{
			depth = NGUITools.CalculateNextDepth(go);
		}
		T t = NGUITools.AddChild<T>(go);
		t.width = 100;
		t.height = 100;
		t.depth = depth;
		return t;
	}

	public static void AddWidgetCollider(GameObject go)
	{
		NGUITools.AddWidgetCollider(go, false);
	}

	public static void AddWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			Collider component = go.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
				return;
			}
			if (component != null)
			{
				return;
			}
			BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
			if (boxCollider2D != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			UICamera uICamera = UICamera.FindCameraForLayer(go.layer);
			if (uICamera != null && (uICamera.eventType == UICamera.EventType.World_2D || uICamera.eventType == UICamera.EventType.UI_2D))
			{
				boxCollider2D = go.AddComponent<BoxCollider2D>();
				boxCollider2D.isTrigger = true;
				UIWidget uIWidget = go.GetComponent<UIWidget>();
				if (uIWidget != null)
				{
					uIWidget.autoResizeBoxCollider = true;
				}
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
				return;
			}
			boxCollider = go.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			UIWidget component1 = go.GetComponent<UIWidget>();
			if (component1 != null)
			{
				component1.autoResizeBoxCollider = true;
			}
			NGUITools.UpdateWidgetCollider(boxCollider, considerInactive);
		}
	}

	public static int AdjustDepth(GameObject go, int adjustment)
	{
		if (go == null)
		{
			return 0;
		}
		UIPanel component = go.GetComponent<UIPanel>();
		if (component != null)
		{
			UIPanel[] componentsInChildren = go.GetComponentsInChildren<UIPanel>(true);
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				UIPanel uIPanel = componentsInChildren[i];
				uIPanel.depth = uIPanel.depth + adjustment;
			}
			return 1;
		}
		component = NGUITools.FindInParents<UIPanel>(go);
		if (component == null)
		{
			return 0;
		}
		UIWidget[] uIWidgetArray = go.GetComponentsInChildren<UIWidget>(true);
		int num = 0;
		int length = (int)uIWidgetArray.Length;
		while (num < length)
		{
			UIWidget uIWidget = uIWidgetArray[num];
			if (uIWidget.panel == component)
			{
				uIWidget.depth = uIWidget.depth + adjustment;
			}
			num++;
		}
		return 2;
	}

	public static Color ApplyPMA(Color c)
	{
		if (c.a != 1f)
		{
			c.r *= c.a;
			c.g *= c.a;
			c.b *= c.a;
		}
		return c;
	}

	public static void BringForward(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, 1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] gameObjectArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int num = 0;
		int length = (int)gameObjectArray.Length;
		while (num < length)
		{
			gameObjectArray[num].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			num++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] gameObjectArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int num = 0;
		int length = (int)gameObjectArray.Length;
		while (num < length)
		{
			gameObjectArray[num].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			num++;
		}
	}

	public static int CalculateNextDepth(GameObject go)
	{
		if (!go)
		{
			return 0;
		}
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int num1 = 0;
		int length = (int)componentsInChildren.Length;
		while (num1 < length)
		{
			num = Mathf.Max(num, componentsInChildren[num1].depth);
			num1++;
		}
		return num + 1;
	}

	public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
	{
		if (!go || !ignoreChildrenWithColliders)
		{
			return NGUITools.CalculateNextDepth(go);
		}
		int num = -1;
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		int num1 = 0;
		int length = (int)componentsInChildren.Length;
		while (num1 < length)
		{
			UIWidget uIWidget = componentsInChildren[num1];
			if (!(uIWidget.cachedGameObject != go) || !(uIWidget.GetComponent<Collider>() != null) && !(uIWidget.GetComponent<Collider2D>() != null))
			{
				num = Mathf.Max(num, uIWidget.depth);
			}
			num1++;
		}
		return num + 1;
	}

	public static int CalculateRaycastDepth(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			return component.raycastDepth;
		}
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
		if ((int)componentsInChildren.Length == 0)
		{
			return 0;
		}
		int num = 2147483647;
		int num1 = 0;
		int length = (int)componentsInChildren.Length;
		while (num1 < length)
		{
			if (componentsInChildren[num1].enabled)
			{
				num = Mathf.Min(num, componentsInChildren[num1].raycastDepth);
			}
			num1++;
		}
		return num;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CallCreatePanel(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.CreatePanel();
		}
		int num = 0;
		int num1 = t.childCount;
		while (num < num1)
		{
			NGUITools.CallCreatePanel(t.GetChild(num));
			num++;
		}
	}

	public static UIPanel CreateUI(bool advanced3D)
	{
		return NGUITools.CreateUI(null, advanced3D, -1);
	}

	public static UIPanel CreateUI(bool advanced3D, int layer)
	{
		return NGUITools.CreateUI(null, advanced3D, layer);
	}

	public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
	{
		UIRoot uIRoot;
		if (trans == null)
		{
			uIRoot = null;
		}
		else
		{
			uIRoot = NGUITools.FindInParents<UIRoot>(trans.gameObject);
		}
		UIRoot uIRoot1 = uIRoot;
		if (uIRoot1 == null && UIRoot.list.Count > 0)
		{
			foreach (UIRoot uIRoot2 in UIRoot.list)
			{
				if (uIRoot2.gameObject.layer != layer)
				{
					continue;
				}
				uIRoot1 = uIRoot2;
				break;
			}
		}
		if (uIRoot1 == null)
		{
			int num = 0;
			int count = UIPanel.list.Count;
			while (num < count)
			{
				UIPanel item = UIPanel.list[num];
				GameObject gameObject = item.gameObject;
				if (gameObject.hideFlags == HideFlags.None && gameObject.layer == layer)
				{
					trans.parent = item.transform;
					trans.localScale = Vector3.one;
					return item;
				}
				num++;
			}
		}
		if (uIRoot1 != null)
		{
			UICamera componentInChildren = uIRoot1.GetComponentInChildren<UICamera>();
			if (componentInChildren != null && componentInChildren.GetComponent<Camera>().orthographic == advanced3D)
			{
				trans = null;
				uIRoot1 = null;
			}
		}
		if (uIRoot1 == null)
		{
			GameObject gameObject1 = NGUITools.AddChild(null, false);
			uIRoot1 = gameObject1.AddComponent<UIRoot>();
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("UI");
			}
			if (layer == -1)
			{
				layer = LayerMask.NameToLayer("2D UI");
			}
			gameObject1.layer = layer;
			if (!advanced3D)
			{
				gameObject1.name = "UI Root";
				uIRoot1.scalingStyle = UIRoot.Scaling.Flexible;
			}
			else
			{
				gameObject1.name = "UI Root (3D)";
				uIRoot1.scalingStyle = UIRoot.Scaling.Constrained;
			}
		}
		UIPanel uIPanel = uIRoot1.GetComponentInChildren<UIPanel>();
		if (uIPanel == null)
		{
			Camera[] cameraArray = NGUITools.FindActive<Camera>();
			float single = -1f;
			bool flag = false;
			int num1 = 1 << (uIRoot1.gameObject.layer & 31);
			for (int i = 0; i < (int)cameraArray.Length; i++)
			{
				Camera camera = cameraArray[i];
				if (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox)
				{
					flag = true;
				}
				single = Mathf.Max(single, camera.depth);
				camera.cullingMask = camera.cullingMask & ~num1;
			}
			Camera vector3 = NGUITools.AddChild<Camera>(uIRoot1.gameObject, false);
			vector3.gameObject.AddComponent<UICamera>();
			vector3.clearFlags = (!flag ? CameraClearFlags.Color : CameraClearFlags.Depth);
			vector3.backgroundColor = Color.grey;
			vector3.cullingMask = num1;
			vector3.depth = single + 1f;
			if (!advanced3D)
			{
				vector3.orthographic = true;
				vector3.orthographicSize = 1f;
				vector3.nearClipPlane = -10f;
				vector3.farClipPlane = 10f;
			}
			else
			{
				vector3.nearClipPlane = 0.1f;
				vector3.farClipPlane = 4f;
				vector3.transform.localPosition = new Vector3(0f, 0f, -700f);
			}
			AudioListener[] audioListenerArray = NGUITools.FindActive<AudioListener>();
			if (audioListenerArray == null || (int)audioListenerArray.Length == 0)
			{
				vector3.gameObject.AddComponent<AudioListener>();
			}
			uIPanel = uIRoot1.gameObject.AddComponent<UIPanel>();
		}
		if (trans != null)
		{
			while (trans.parent != null)
			{
				trans = trans.parent;
			}
			if (!NGUITools.IsChild(trans, uIPanel.transform))
			{
				trans.parent = uIPanel.transform;
				trans.localScale = Vector3.one;
				trans.localPosition = Vector3.zero;
				NGUITools.SetChildLayer(uIPanel.cachedTransform, uIPanel.cachedGameObject.layer);
			}
			else
			{
				uIPanel = trans.gameObject.AddComponent<UIPanel>();
			}
		}
		return uIPanel;
	}

	private static void Deactivate(Transform t)
	{
		NGUITools.SetActiveSelf(t.gameObject, false);
	}

	public static void Destroy(UnityEngine.Object obj)
	{
		if (obj)
		{
			if (obj is Transform)
			{
				Transform transforms = obj as Transform;
				GameObject gameObject = transforms.gameObject;
				if (!Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
				else
				{
					transforms.parent = null;
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			else if (obj is GameObject)
			{
				GameObject gameObject1 = obj as GameObject;
				Transform transforms1 = gameObject1.transform;
				if (!Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(gameObject1);
				}
				else
				{
					transforms1.parent = null;
					UnityEngine.Object.Destroy(gameObject1);
				}
			}
			else if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	public static void DestroyChildren(this Transform t)
	{
		bool flag = Application.isPlaying;
		while (t.childCount != 0)
		{
			Transform child = t.GetChild(0);
			if (!flag)
			{
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
			else
			{
				child.parent = null;
				UnityEngine.Object.Destroy(child.gameObject);
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

	[Obsolete("Use NGUIText.EncodeColor instead")]
	public static string EncodeColor(Color c)
	{
		return NGUIText.EncodeColor24(c);
	}

	public static void Execute<T>(GameObject go, string funcName)
	where T : Component
	{
		T[] components = go.GetComponents<T>();
		for (int i = 0; i < (int)components.Length; i++)
		{
			T t = components[i];
			MethodInfo method = t.GetType().GetMethod(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				method.Invoke(t, null);
			}
		}
	}

	public static void ExecuteAll<T>(GameObject root, string funcName)
	where T : Component
	{
		NGUITools.Execute<T>(root, funcName);
		Transform transforms = root.transform;
		int num = 0;
		int num1 = transforms.childCount;
		while (num < num1)
		{
			NGUITools.ExecuteAll<T>(transforms.GetChild(num).gameObject, funcName);
			num++;
		}
	}

	public static T[] FindActive<T>()
	where T : Component
	{
		return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	}

	public static Camera FindCameraForLayer(int layer)
	{
		Camera camera;
		int num = 1 << (layer & 31);
		for (int i = 0; i < UICamera.list.size; i++)
		{
			camera = UICamera.list.buffer[i].cachedCamera;
			if (camera && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		camera = Camera.main;
		if (camera && (camera.cullingMask & num) != 0)
		{
			return camera;
		}
		Camera[] cameraArray = new Camera[Camera.allCamerasCount];
		int allCameras = Camera.GetAllCameras(cameraArray);
		for (int j = 0; j < allCameras; j++)
		{
			camera = cameraArray[j];
			if (camera && camera.enabled && (camera.cullingMask & num) != 0)
			{
				return camera;
			}
		}
		return null;
	}

	public static T FindInParents<T>(GameObject go)
	where T : Component
	{
		if (go == null)
		{
			return (T)null;
		}
		T component = go.GetComponent<T>();
		if (component == null)
		{
			for (Transform i = go.transform.parent; i != null && component == null; i = i.parent)
			{
				component = i.gameObject.GetComponent<T>();
			}
		}
		return component;
	}

	public static T FindInParents<T>(Transform trans)
	where T : Component
	{
		if (trans == null)
		{
			return (T)null;
		}
		return trans.GetComponentInParent<T>();
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(Behaviour mb)
	{
		return (!mb || !mb.enabled ? false : mb.gameObject.activeInHierarchy);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static bool GetActive(GameObject go)
	{
		return (!go ? false : go.activeInHierarchy);
	}

	public static string GetFuncName(object obj, string method)
	{
		if (obj == null)
		{
			return "<null>";
		}
		string str = obj.GetType().ToString();
		int num = str.LastIndexOf('/');
		if (num > 0)
		{
			str = str.Substring(num + 1);
		}
		return (!string.IsNullOrEmpty(method) ? string.Concat(str, "/", method) : str);
	}

	public static string GetHierarchy(GameObject obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		string str = obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			str = string.Concat(obj.name, "\\", str);
		}
		return str;
	}

	public static GameObject GetRoot(GameObject go)
	{
		Transform transforms = go.transform;
		while (true)
		{
			Transform transforms1 = transforms.parent;
			if (transforms1 == null)
			{
				break;
			}
			transforms = transforms1;
		}
		return transforms.gameObject;
	}

	public static Vector3[] GetSides(this Camera cam)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
	}

	public static Vector3[] GetSides(this Camera cam, float depth)
	{
		return cam.GetSides(depth, null);
	}

	public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
	{
		return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
	{
		if (!cam.orthographic)
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
		}
		else
		{
			float single = cam.orthographicSize;
			float single1 = -single;
			float single2 = single;
			float single3 = -single;
			float single4 = single;
			Rect rect = cam.rect;
			Vector2 vector2 = NGUITools.screenSize;
			float single5 = vector2.x / vector2.y;
			single5 = single5 * (rect.width / rect.height);
			single1 *= single5;
			single2 *= single5;
			Transform transforms = cam.transform;
			Quaternion quaternion = transforms.rotation;
			Vector3 vector3 = transforms.position;
			int num = Mathf.RoundToInt(vector2.x);
			int num1 = Mathf.RoundToInt(vector2.y);
			if ((num & 1) == 1)
			{
				vector3.x = vector3.x - 1f / vector2.x;
			}
			if ((num1 & 1) == 1)
			{
				vector3.y = vector3.y + 1f / vector2.y;
			}
			NGUITools.mSides[0] = (quaternion * new Vector3(single1, 0f, depth)) + vector3;
			NGUITools.mSides[1] = (quaternion * new Vector3(0f, single4, depth)) + vector3;
			NGUITools.mSides[2] = (quaternion * new Vector3(single2, 0f, depth)) + vector3;
			NGUITools.mSides[3] = (quaternion * new Vector3(0f, single3, depth)) + vector3;
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static string GetTypeName<T>()
	{
		string str = typeof(T).ToString();
		if (str.StartsWith("UI"))
		{
			str = str.Substring(2);
		}
		else if (str.StartsWith("UnityEngine."))
		{
			str = str.Substring(12);
		}
		return str;
	}

	public static string GetTypeName(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			return "Null";
		}
		string str = obj.GetType().ToString();
		if (str.StartsWith("UI"))
		{
			str = str.Substring(2);
		}
		else if (str.StartsWith("UnityEngine."))
		{
			str = str.Substring(12);
		}
		return str;
	}

	public static Vector3[] GetWorldCorners(this Camera cam)
	{
		float single = Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f);
		return cam.GetWorldCorners(single, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth)
	{
		return cam.GetWorldCorners(depth, null);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
	{
		return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
	}

	public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
	{
		if (!cam.orthographic)
		{
			NGUITools.mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
			NGUITools.mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
			NGUITools.mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
			NGUITools.mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
		}
		else
		{
			float single = cam.orthographicSize;
			float single1 = -single;
			float single2 = single;
			float single3 = -single;
			float single4 = single;
			Rect rect = cam.rect;
			Vector2 vector2 = NGUITools.screenSize;
			float single5 = vector2.x / vector2.y;
			single5 = single5 * (rect.width / rect.height);
			single1 *= single5;
			single2 *= single5;
			Transform transforms = cam.transform;
			Quaternion quaternion = transforms.rotation;
			Vector3 vector3 = transforms.position;
			NGUITools.mSides[0] = (quaternion * new Vector3(single1, single3, depth)) + vector3;
			NGUITools.mSides[1] = (quaternion * new Vector3(single1, single4, depth)) + vector3;
			NGUITools.mSides[2] = (quaternion * new Vector3(single2, single4, depth)) + vector3;
			NGUITools.mSides[3] = (quaternion * new Vector3(single2, single3, depth)) + vector3;
		}
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				NGUITools.mSides[i] = relativeTo.InverseTransformPoint(NGUITools.mSides[i]);
			}
		}
		return NGUITools.mSides;
	}

	public static void ImmediatelyCreateDrawCalls(GameObject root)
	{
		NGUITools.ExecuteAll<UIWidget>(root, "Start");
		NGUITools.ExecuteAll<UIPanel>(root, "Start");
		NGUITools.ExecuteAll<UIWidget>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "Update");
		NGUITools.ExecuteAll<UIPanel>(root, "LateUpdate");
	}

	[Obsolete("Use NGUITools.GetActive instead")]
	public static bool IsActive(Behaviour mb)
	{
		return (!(mb != null) || !mb.enabled ? false : mb.gameObject.activeInHierarchy);
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	public static string KeyToCaption(KeyCode key)
	{
		KeyCode keyCode = key;
		switch (keyCode)
		{
			case KeyCode.None:
			{
				return null;
			}
			case KeyCode.Backspace:
			{
				return "BS";
			}
			case KeyCode.Tab:
			{
				return "Tab";
			}
			case KeyCode.Clear:
			{
				return "Clr";
			}
			case KeyCode.Return:
			{
				return "NT";
			}
			case KeyCode.Pause:
			{
				return "PS";
			}
			case KeyCode.Escape:
			{
				return "Esc";
			}
			case KeyCode.Space:
			{
				return "SP";
			}
			case KeyCode.Exclaim:
			{
				return "!";
			}
			case KeyCode.DoubleQuote:
			{
				return "\"";
			}
			case KeyCode.Hash:
			{
				return "#";
			}
			case KeyCode.Dollar:
			{
				return "$";
			}
			case KeyCode.Ampersand:
			{
				return "&";
			}
			case KeyCode.Quote:
			{
				return "'";
			}
			case KeyCode.LeftParen:
			{
				return "(";
			}
			case KeyCode.RightParen:
			{
				return ")";
			}
			case KeyCode.Asterisk:
			{
				return "*";
			}
			case KeyCode.Plus:
			{
				return "+";
			}
			case KeyCode.Comma:
			{
				return ",";
			}
			case KeyCode.Minus:
			{
				return "-";
			}
			case KeyCode.Period:
			{
				return ".";
			}
			case KeyCode.Slash:
			{
				return "/";
			}
			case KeyCode.Alpha0:
			{
				return "0";
			}
			case KeyCode.Alpha1:
			{
				return "1";
			}
			case KeyCode.Alpha2:
			{
				return "2";
			}
			case KeyCode.Alpha3:
			{
				return "3";
			}
			case KeyCode.Alpha4:
			{
				return "4";
			}
			case KeyCode.Alpha5:
			{
				return "5";
			}
			case KeyCode.Alpha6:
			{
				return "6";
			}
			case KeyCode.Alpha7:
			{
				return "7";
			}
			case KeyCode.Alpha8:
			{
				return "8";
			}
			case KeyCode.Alpha9:
			{
				return "9";
			}
			case KeyCode.Colon:
			{
				return ":";
			}
			case KeyCode.Semicolon:
			{
				return ";";
			}
			case KeyCode.Less:
			{
				return "<";
			}
			case KeyCode.Equals:
			{
				return "=";
			}
			case KeyCode.Greater:
			{
				return ">";
			}
			case KeyCode.Question:
			{
				return "?";
			}
			case KeyCode.At:
			{
				return "@";
			}
			case KeyCode.LeftBracket:
			{
				return "[";
			}
			case KeyCode.Backslash:
			{
				return "\\";
			}
			case KeyCode.RightBracket:
			{
				return "]";
			}
			case KeyCode.Caret:
			{
				return "^";
			}
			case KeyCode.Underscore:
			{
				return "_";
			}
			case KeyCode.BackQuote:
			{
				return "`";
			}
			case KeyCode.A:
			{
				return "A";
			}
			case KeyCode.B:
			{
				return "B";
			}
			case KeyCode.C:
			{
				return "C";
			}
			case KeyCode.D:
			{
				return "D";
			}
			case KeyCode.E:
			{
				return "E";
			}
			case KeyCode.F:
			{
				return "F";
			}
			case KeyCode.G:
			{
				return "G";
			}
			case KeyCode.H:
			{
				return "H";
			}
			case KeyCode.I:
			{
				return "I";
			}
			case KeyCode.J:
			{
				return "J";
			}
			case KeyCode.K:
			{
				return "K";
			}
			case KeyCode.L:
			{
				return "L";
			}
			case KeyCode.M:
			{
				return "M";
			}
			case KeyCode.N:
			{
				return "N0";
			}
			case KeyCode.O:
			{
				return "O";
			}
			case KeyCode.P:
			{
				return "P";
			}
			case KeyCode.Q:
			{
				return "Q";
			}
			case KeyCode.R:
			{
				return "R";
			}
			case KeyCode.S:
			{
				return "S";
			}
			case KeyCode.T:
			{
				return "T";
			}
			case KeyCode.U:
			{
				return "U";
			}
			case KeyCode.V:
			{
				return "V";
			}
			case KeyCode.W:
			{
				return "W";
			}
			case KeyCode.X:
			{
				return "X";
			}
			case KeyCode.Y:
			{
				return "Y";
			}
			case KeyCode.Z:
			{
				return "Z";
			}
			case KeyCode.Delete:
			{
				return "Del";
			}
			default:
			{
				switch (keyCode)
				{
					case KeyCode.Keypad0:
					{
						return "K0";
					}
					case KeyCode.Keypad1:
					{
						return "K1";
					}
					case KeyCode.Keypad2:
					{
						return "K2";
					}
					case KeyCode.Keypad3:
					{
						return "K3";
					}
					case KeyCode.Keypad4:
					{
						return "K4";
					}
					case KeyCode.Keypad5:
					{
						return "K5";
					}
					case KeyCode.Keypad6:
					{
						return "K6";
					}
					case KeyCode.Keypad7:
					{
						return "K7";
					}
					case KeyCode.Keypad8:
					{
						return "K8";
					}
					case KeyCode.Keypad9:
					{
						return "K9";
					}
					case KeyCode.KeypadPeriod:
					{
						return ".";
					}
					case KeyCode.KeypadDivide:
					{
						return "/";
					}
					case KeyCode.KeypadMultiply:
					{
						return "*";
					}
					case KeyCode.KeypadMinus:
					{
						return "-";
					}
					case KeyCode.KeypadPlus:
					{
						return "+";
					}
					case KeyCode.KeypadEnter:
					{
						return "NT";
					}
					case KeyCode.KeypadEquals:
					{
						return "=";
					}
					case KeyCode.UpArrow:
					{
						return "UP";
					}
					case KeyCode.DownArrow:
					{
						return "DN";
					}
					case KeyCode.RightArrow:
					{
						return "LT";
					}
					case KeyCode.LeftArrow:
					{
						return "RT";
					}
					case KeyCode.Insert:
					{
						return "Ins";
					}
					case KeyCode.Home:
					{
						return "Home";
					}
					case KeyCode.End:
					{
						return "End";
					}
					case KeyCode.PageUp:
					{
						return "PU";
					}
					case KeyCode.PageDown:
					{
						return "PD";
					}
					case KeyCode.F1:
					{
						return "F1";
					}
					case KeyCode.F2:
					{
						return "F2";
					}
					case KeyCode.F3:
					{
						return "F3";
					}
					case KeyCode.F4:
					{
						return "F4";
					}
					case KeyCode.F5:
					{
						return "F5";
					}
					case KeyCode.F6:
					{
						return "F6";
					}
					case KeyCode.F7:
					{
						return "F7";
					}
					case KeyCode.F8:
					{
						return "F8";
					}
					case KeyCode.F9:
					{
						return "F9";
					}
					case KeyCode.F10:
					{
						return "F10";
					}
					case KeyCode.F11:
					{
						return "F11";
					}
					case KeyCode.F12:
					{
						return "F12";
					}
					case KeyCode.F13:
					{
						return "F13";
					}
					case KeyCode.F14:
					{
						return "F14";
					}
					case KeyCode.F15:
					{
						return "F15";
					}
					case KeyCode.Backspace | KeyCode.Tab | KeyCode.Space | KeyCode.Keypad0 | KeyCode.Keypad1 | KeyCode.Keypad8 | KeyCode.Keypad9 | KeyCode.F7 | KeyCode.F8 | KeyCode.F15 | KeyCode.Exclaim | KeyCode.LeftParen | KeyCode.RightParen:
					case KeyCode.Backspace | KeyCode.Space | KeyCode.Keypad0 | KeyCode.Keypad2 | KeyCode.Keypad8 | KeyCode.KeypadPeriod | KeyCode.F7 | KeyCode.F9 | KeyCode.F15 | KeyCode.DoubleQuote | KeyCode.LeftParen | KeyCode.Asterisk:
					case KeyCode.Backspace | KeyCode.Tab | KeyCode.Space | KeyCode.Keypad0 | KeyCode.Keypad1 | KeyCode.Keypad2 | KeyCode.Keypad3 | KeyCode.Keypad8 | KeyCode.Keypad9 | KeyCode.KeypadPeriod | KeyCode.KeypadDivide | KeyCode.F7 | KeyCode.F8 | KeyCode.F9 | KeyCode.F10 | KeyCode.F15 | KeyCode.Exclaim | KeyCode.DoubleQuote | KeyCode.Hash | KeyCode.LeftParen | KeyCode.RightParen | KeyCode.Asterisk | KeyCode.Plus:
					case KeyCode.RightCommand:
					case KeyCode.LeftCommand:
					case KeyCode.LeftWindows:
					case KeyCode.RightWindows:
					case KeyCode.AltGr:
					case KeyCode.Backspace | KeyCode.Space | KeyCode.Keypad0 | KeyCode.Keypad2 | KeyCode.Keypad8 | KeyCode.KeypadPeriod | KeyCode.KeypadEquals | KeyCode.DownArrow | KeyCode.PageUp | KeyCode.F1 | KeyCode.F7 | KeyCode.F9 | KeyCode.F15 | KeyCode.Alpha0 | KeyCode.Alpha2 | KeyCode.Alpha8 | KeyCode.DoubleQuote | KeyCode.LeftParen | KeyCode.Asterisk | KeyCode.Colon | KeyCode.LeftShift | KeyCode.LeftControl | KeyCode.RightWindows:
					case KeyCode.Help:
					case KeyCode.Print:
					case KeyCode.SysReq:
					case KeyCode.Break:
					case KeyCode.Menu:
					case KeyCode.Keypad0 | KeyCode.At:
					case KeyCode.Keypad0 | KeyCode.Keypad1 | KeyCode.At:
					case KeyCode.Keypad0 | KeyCode.Keypad2 | KeyCode.At:
					{
						return null;
					}
					case KeyCode.Numlock:
					{
						return "Num";
					}
					case KeyCode.CapsLock:
					{
						return "Cap";
					}
					case KeyCode.ScrollLock:
					{
						return "Scr";
					}
					case KeyCode.RightShift:
					{
						return "RS";
					}
					case KeyCode.LeftShift:
					{
						return "LS";
					}
					case KeyCode.RightControl:
					{
						return "RC";
					}
					case KeyCode.LeftControl:
					{
						return "LC";
					}
					case KeyCode.RightAlt:
					{
						return "RA";
					}
					case KeyCode.LeftAlt:
					{
						return "LA";
					}
					case KeyCode.Mouse0:
					{
						return "M0";
					}
					case KeyCode.Mouse1:
					{
						return "M1";
					}
					case KeyCode.Mouse2:
					{
						return "M2";
					}
					case KeyCode.Mouse3:
					{
						return "M3";
					}
					case KeyCode.Mouse4:
					{
						return "M4";
					}
					case KeyCode.Mouse5:
					{
						return "M5";
					}
					case KeyCode.Mouse6:
					{
						return "M6";
					}
					case KeyCode.JoystickButton0:
					{
						return "(A)";
					}
					case KeyCode.JoystickButton1:
					{
						return "(B)";
					}
					case KeyCode.JoystickButton2:
					{
						return "(X)";
					}
					case KeyCode.JoystickButton3:
					{
						return "(Y)";
					}
					case KeyCode.JoystickButton4:
					{
						return "(RB)";
					}
					case KeyCode.JoystickButton5:
					{
						return "(LB)";
					}
					case KeyCode.JoystickButton6:
					{
						return "(Back)";
					}
					case KeyCode.JoystickButton7:
					{
						return "(Start)";
					}
					case KeyCode.JoystickButton8:
					{
						return "(LS)";
					}
					case KeyCode.JoystickButton9:
					{
						return "(RS)";
					}
					case KeyCode.JoystickButton10:
					{
						return "J10";
					}
					case KeyCode.JoystickButton11:
					{
						return "J11";
					}
					case KeyCode.JoystickButton12:
					{
						return "J12";
					}
					case KeyCode.JoystickButton13:
					{
						return "J13";
					}
					case KeyCode.JoystickButton14:
					{
						return "J14";
					}
					case KeyCode.JoystickButton15:
					{
						return "J15";
					}
					case KeyCode.JoystickButton16:
					{
						return "J16";
					}
					case KeyCode.JoystickButton17:
					{
						return "J17";
					}
					case KeyCode.JoystickButton18:
					{
						return "J18";
					}
					case KeyCode.JoystickButton19:
					{
						return "J19";
					}
					default:
					{
						return null;
					}
				}
				break;
			}
		}
	}

	public static byte[] Load(string fileName)
	{
		if (!NGUITools.fileAccess)
		{
			return null;
		}
		string str = string.Concat(Application.persistentDataPath, "/", fileName);
		if (!File.Exists(str))
		{
			return null;
		}
		return File.ReadAllBytes(str);
	}

	public static void MakePixelPerfect(Transform t)
	{
		UIWidget component = t.GetComponent<UIWidget>();
		if (component != null)
		{
			component.MakePixelPerfect();
		}
		if (t.GetComponent<UIAnchor>() == null && t.GetComponent<UIRoot>() == null)
		{
			t.localPosition = NGUITools.Round(t.localPosition);
			t.localScale = NGUITools.Round(t.localScale);
		}
		int num = 0;
		int num1 = t.childCount;
		while (num < num1)
		{
			NGUITools.MakePixelPerfect(t.GetChild(num));
			num++;
		}
	}

	public static void MarkParentAsChanged(GameObject go)
	{
		UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
		int num = 0;
		int length = (int)componentsInChildren.Length;
		while (num < length)
		{
			componentsInChildren[num].ParentHasChanged();
			num++;
		}
	}

	public static void NormalizeDepths()
	{
		NGUITools.NormalizeWidgetDepths();
		NGUITools.NormalizePanelDepths();
	}

	public static void NormalizePanelDepths()
	{
		UIPanel[] uIPanelArray = NGUITools.FindActive<UIPanel>();
		int length = (int)uIPanelArray.Length;
		if (length > 0)
		{
			Array.Sort<UIPanel>(uIPanelArray, new Comparison<UIPanel>(UIPanel.CompareFunc));
			int num = 0;
			int num1 = uIPanelArray[0].depth;
			for (int i = 0; i < length; i++)
			{
				UIPanel uIPanel = uIPanelArray[i];
				if (uIPanel.depth != num1)
				{
					num1 = uIPanel.depth;
					int num2 = num + 1;
					num = num2;
					uIPanel.depth = num2;
				}
				else
				{
					uIPanel.depth = num;
				}
			}
		}
	}

	public static void NormalizeWidgetDepths()
	{
		NGUITools.NormalizeWidgetDepths(NGUITools.FindActive<UIWidget>());
	}

	public static void NormalizeWidgetDepths(GameObject go)
	{
		NGUITools.NormalizeWidgetDepths(go.GetComponentsInChildren<UIWidget>());
	}

	public static void NormalizeWidgetDepths(UIWidget[] list)
	{
		int length = (int)list.Length;
		if (length > 0)
		{
			Array.Sort<UIWidget>(list, new Comparison<UIWidget>(UIWidget.FullCompareFunc));
			int num = 0;
			int num1 = list[0].depth;
			for (int i = 0; i < length; i++)
			{
				UIWidget uIWidget = list[i];
				if (uIWidget.depth != num1)
				{
					num1 = uIWidget.depth;
					int num2 = num + 1;
					num = num2;
					uIWidget.depth = num2;
				}
				else
				{
					uIWidget.depth = num;
				}
			}
		}
	}

	[Obsolete("Use NGUIText.ParseColor instead")]
	public static Color ParseColor(string text, int offset)
	{
		return NGUIText.ParseColor24(text, offset);
	}

	public static AudioSource PlaySound(AudioClip clip)
	{
		return NGUITools.PlaySound(clip, 1f, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume)
	{
		return NGUITools.PlaySound(clip, volume, 1f);
	}

	public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
	{
		float single = RealTime.time;
		if (NGUITools.mLastClip == clip && NGUITools.mLastTimestamp + 0.1f > single)
		{
			return null;
		}
		NGUITools.mLastClip = clip;
		NGUITools.mLastTimestamp = single;
		volume *= NGUITools.soundVolume;
		if (clip != null && volume > 0.01f)
		{
			if (NGUITools.mListener == null || !NGUITools.GetActive(NGUITools.mListener))
			{
				AudioListener[] audioListenerArray = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
				if (audioListenerArray != null)
				{
					int num = 0;
					while (num < (int)audioListenerArray.Length)
					{
						if (!NGUITools.GetActive(audioListenerArray[num]))
						{
							num++;
						}
						else
						{
							NGUITools.mListener = audioListenerArray[num];
							break;
						}
					}
				}
				if (NGUITools.mListener == null)
				{
					Camera camera = Camera.main;
					if (camera == null)
					{
						camera = UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera;
					}
					if (camera != null)
					{
						NGUITools.mListener = camera.gameObject.AddComponent<AudioListener>();
					}
				}
			}
			if (NGUITools.mListener != null && NGUITools.mListener.enabled && NGUITools.GetActive(NGUITools.mListener.gameObject))
			{
				AudioSource component = NGUITools.mListener.GetComponent<AudioSource>();
				if (component == null)
				{
					component = NGUITools.mListener.gameObject.AddComponent<AudioSource>();
				}
				component.priority = 50;
				component.pitch = pitch;
				component.PlayOneShot(clip, volume);
				return component;
			}
		}
		return null;
	}

	public static void PushBack(GameObject go)
	{
		int num = NGUITools.AdjustDepth(go, -1000);
		if (num == 1)
		{
			NGUITools.NormalizePanelDepths();
		}
		else if (num == 2)
		{
			NGUITools.NormalizeWidgetDepths();
		}
	}

	public static int RandomRange(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		return UnityEngine.Random.Range(min, max + 1);
	}

	public static void RegisterUndo(UnityEngine.Object obj, string name)
	{
	}

	public static Vector3 Round(Vector3 v)
	{
		v.x = Mathf.Round(v.x);
		v.y = Mathf.Round(v.y);
		v.z = Mathf.Round(v.z);
		return v;
	}

	public static bool Save(string fileName, byte[] bytes)
	{
		bool flag;
		if (!NGUITools.fileAccess)
		{
			return false;
		}
		string str = string.Concat(Application.persistentDataPath, "/", fileName);
		if (bytes == null)
		{
			if (File.Exists(str))
			{
				File.Delete(str);
			}
			return true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(str);
			fileStream.Write(bytes, 0, (int)bytes.Length);
			fileStream.Close();
			return true;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError(exception.Message);
			flag = false;
		}
		return flag;
	}

	public static void SetActive(GameObject go, bool state)
	{
		NGUITools.SetActive(go, state, true);
	}

	public static void SetActive(GameObject go, bool state, bool compatibilityMode)
	{
		if (go)
		{
			if (!state)
			{
				NGUITools.Deactivate(go.transform);
			}
			else
			{
				NGUITools.Activate(go.transform, compatibilityMode);
				NGUITools.CallCreatePanel(go.transform);
			}
		}
	}

	public static void SetActiveChildren(GameObject go, bool state)
	{
		Transform transforms = go.transform;
		if (!state)
		{
			int num = 0;
			int num1 = transforms.childCount;
			while (num < num1)
			{
				NGUITools.Deactivate(transforms.GetChild(num));
				num++;
			}
		}
		else
		{
			int num2 = 0;
			int num3 = transforms.childCount;
			while (num2 < num3)
			{
				NGUITools.Activate(transforms.GetChild(num2));
				num2++;
			}
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	public static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			NGUITools.SetChildLayer(child, layer);
		}
	}

	public static void SetDirty(UnityEngine.Object obj)
	{
	}

	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		Transform transforms = go.transform;
		int num = 0;
		int num1 = transforms.childCount;
		while (num < num1)
		{
			NGUITools.SetLayer(transforms.GetChild(num).gameObject, layer);
			num++;
		}
	}

	[Obsolete("Use NGUIText.StripSymbols instead")]
	public static string StripSymbols(string text)
	{
		return NGUIText.StripSymbols(text);
	}

	public static void UpdateWidgetCollider(GameObject go)
	{
		NGUITools.UpdateWidgetCollider(go, false);
	}

	public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
	{
		if (go != null)
		{
			BoxCollider component = go.GetComponent<BoxCollider>();
			if (component != null)
			{
				NGUITools.UpdateWidgetCollider(component, considerInactive);
				return;
			}
			BoxCollider2D boxCollider2D = go.GetComponent<BoxCollider2D>();
			if (boxCollider2D != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider2D, considerInactive);
			}
		}
	}

	public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component == null)
			{
				Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.center = bound.center;
				float single = bound.size.x;
				Vector3 vector3 = bound.size;
				box.size = new Vector3(single, vector3.y, 0f);
			}
			else
			{
				Vector4 vector4 = component.drawRegion;
				if (vector4.x != 0f || vector4.y != 0f || vector4.z != 1f || vector4.w != 1f)
				{
					Vector4 vector41 = component.drawingDimensions;
					box.center = new Vector3((vector41.x + vector41.z) * 0.5f, (vector41.y + vector41.w) * 0.5f);
					box.size = new Vector3(vector41.z - vector41.x, vector41.w - vector41.y);
				}
				else
				{
					Vector3[] vector3Array = component.localCorners;
					box.center = Vector3.Lerp(vector3Array[0], vector3Array[2], 0.5f);
					box.size = vector3Array[2] - vector3Array[0];
				}
			}
		}
	}

	public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
	{
		if (box != null)
		{
			GameObject gameObject = box.gameObject;
			UIWidget component = gameObject.GetComponent<UIWidget>();
			if (component == null)
			{
				Bounds bound = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
				box.offset = bound.center;
				box.size = new Vector2(bound.size.x, bound.size.y);
			}
			else
			{
				Vector3[] vector3Array = component.localCorners;
				box.offset = Vector3.Lerp(vector3Array[0], vector3Array[2], 0.5f);
				box.size = vector3Array[2] - vector3Array[0];
			}
		}
	}
}