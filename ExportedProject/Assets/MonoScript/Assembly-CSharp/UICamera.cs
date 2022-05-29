using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	public static BetterList<UICamera> list;

	public static UICamera.GetKeyStateFunc GetKeyDown;

	public static UICamera.GetKeyStateFunc GetKeyUp;

	public static UICamera.GetKeyStateFunc GetKey;

	public static UICamera.GetAxisFunc GetAxis;

	public static UICamera.GetAnyKeyFunc GetAnyKeyDown;

	public static UICamera.OnScreenResize onScreenResize;

	public UICamera.EventType eventType = UICamera.EventType.UI_3D;

	public bool eventsGoToColliders;

	public LayerMask eventReceiverMask = -1;

	public bool debug;

	public bool useMouse = true;

	public bool useTouch = true;

	public bool allowMultiTouch = true;

	public bool useKeyboard = true;

	public bool useController = true;

	public bool stickyTooltip = true;

	public float tooltipDelay = 1f;

	public bool longPressTooltip;

	public float mouseDragThreshold = 4f;

	public float mouseClickThreshold = 10f;

	public float touchDragThreshold = 40f;

	public float touchClickThreshold = 40f;

	public float rangeDistance = -1f;

	public string horizontalAxisName = "Horizontal";

	public string verticalAxisName = "Vertical";

	public string horizontalPanAxisName;

	public string verticalPanAxisName;

	public string scrollAxisName = "Mouse ScrollWheel";

	public bool commandClick = true;

	public KeyCode submitKey0 = KeyCode.Return;

	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	public KeyCode cancelKey0 = KeyCode.Escape;

	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	public bool autoHideCursor = true;

	public static UICamera.OnCustomInput onCustomInput;

	public static bool showTooltips;

	private static bool mDisableController;

	private static Vector2 mLastPos;

	public static Vector3 lastWorldPosition;

	public static RaycastHit lastHit;

	public static UICamera current;

	public static Camera currentCamera;

	public static UICamera.OnSchemeChange onSchemeChange;

	private static UICamera.ControlScheme mLastScheme;

	public static int currentTouchID;

	private static KeyCode mCurrentKey;

	public static UICamera.MouseOrTouch currentTouch;

	private static bool mInputFocus;

	private static GameObject mGenericHandler;

	public static GameObject fallThrough;

	public static UICamera.VoidDelegate onClick;

	public static UICamera.VoidDelegate onDoubleClick;

	public static UICamera.BoolDelegate onHover;

	public static UICamera.BoolDelegate onPress;

	public static UICamera.BoolDelegate onSelect;

	public static UICamera.FloatDelegate onScroll;

	public static UICamera.VectorDelegate onDrag;

	public static UICamera.VoidDelegate onDragStart;

	public static UICamera.ObjectDelegate onDragOver;

	public static UICamera.ObjectDelegate onDragOut;

	public static UICamera.VoidDelegate onDragEnd;

	public static UICamera.ObjectDelegate onDrop;

	public static UICamera.KeyCodeDelegate onKey;

	public static UICamera.KeyCodeDelegate onNavigate;

	public static UICamera.VectorDelegate onPan;

	public static UICamera.BoolDelegate onTooltip;

	public static UICamera.MoveDelegate onMouseMove;

	private static UICamera.MouseOrTouch[] mMouse;

	public static UICamera.MouseOrTouch controller;

	public static List<UICamera.MouseOrTouch> activeTouches;

	private static List<int> mTouchIDs;

	private static int mWidth;

	private static int mHeight;

	private static GameObject mTooltip;

	private Camera mCam;

	private static float mTooltipTime;

	private float mNextRaycast;

	public static bool isDragging;

	private static GameObject mRayHitObject;

	private static GameObject mHover;

	private static GameObject mSelected;

	private static UICamera.DepthEntry mHit;

	private static BetterList<UICamera.DepthEntry> mHits;

	private static Plane m2DPlane;

	private static float mNextEvent;

	private static int mNotifying;

	private static bool mUsingTouchEvents;

	public static UICamera.GetTouchCountCallback GetInputTouchCount;

	public static UICamera.GetTouchCallback GetInputTouch;

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	public static GameObject controllerNavigationObject
	{
		get
		{
			if (UICamera.controller.current && UICamera.controller.current.activeInHierarchy)
			{
				return UICamera.controller.current;
			}
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.current != null && UICamera.current.useController && UIKeyNavigation.list.size > 0)
			{
				for (int i = 0; i < UIKeyNavigation.list.size; i++)
				{
					UIKeyNavigation item = UIKeyNavigation.list[i];
					if (item && item.constraint != UIKeyNavigation.Constraint.Explicit && item.startsSelected)
					{
						UICamera.hoveredObject = item.gameObject;
						UICamera.controller.current = UICamera.mHover;
						return UICamera.mHover;
					}
				}
				if (UICamera.mHover == null)
				{
					for (int j = 0; j < UIKeyNavigation.list.size; j++)
					{
						UIKeyNavigation uIKeyNavigation = UIKeyNavigation.list[j];
						if (uIKeyNavigation && uIKeyNavigation.constraint != UIKeyNavigation.Constraint.Explicit)
						{
							UICamera.hoveredObject = uIKeyNavigation.gameObject;
							UICamera.controller.current = UICamera.mHover;
							return UICamera.mHover;
						}
					}
				}
			}
			UICamera.controller.current = null;
			return null;
		}
		set
		{
			if (UICamera.controller.current != value && UICamera.controller.current)
			{
				UICamera.Notify(UICamera.controller.current, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.controller.current, false);
				}
				UICamera.controller.current = null;
			}
			UICamera.hoveredObject = value;
		}
	}

	public static KeyCode currentKey
	{
		get
		{
			return UICamera.mCurrentKey;
		}
		set
		{
			if (UICamera.mCurrentKey != value)
			{
				UICamera.ControlScheme controlScheme = UICamera.mLastScheme;
				UICamera.mCurrentKey = value;
				UICamera.mLastScheme = UICamera.currentScheme;
				if (controlScheme != UICamera.mLastScheme)
				{
					UICamera.HideTooltip();
					if (UICamera.mLastScheme == UICamera.ControlScheme.Mouse)
					{
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}
					else if (UICamera.current != null && UICamera.current.autoHideCursor)
					{
						Cursor.visible = false;
						Cursor.lockState = CursorLockMode.Locked;
						UICamera.mMouse[0].ignoreDelta = 2;
					}
					if (UICamera.onSchemeChange != null)
					{
						UICamera.onSchemeChange();
					}
				}
			}
		}
	}

	public static Ray currentRay
	{
		get
		{
			return (!(UICamera.currentCamera != null) || UICamera.currentTouch == null ? new Ray() : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos));
		}
	}

	public static UICamera.ControlScheme currentScheme
	{
		get
		{
			if (UICamera.mCurrentKey == KeyCode.None)
			{
				return UICamera.ControlScheme.Touch;
			}
			if (UICamera.mCurrentKey >= KeyCode.JoystickButton0)
			{
				return UICamera.ControlScheme.Controller;
			}
			if (UICamera.current != null && UICamera.mLastScheme == UICamera.ControlScheme.Controller && (UICamera.mCurrentKey == UICamera.current.submitKey0 || UICamera.mCurrentKey == UICamera.current.submitKey1))
			{
				return UICamera.ControlScheme.Controller;
			}
			return UICamera.ControlScheme.Mouse;
		}
		set
		{
			if (value == UICamera.ControlScheme.Mouse)
			{
				UICamera.currentKey = KeyCode.Mouse0;
			}
			else if (value == UICamera.ControlScheme.Controller)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
			}
			else if (value != UICamera.ControlScheme.Touch)
			{
				UICamera.currentKey = KeyCode.Alpha0;
			}
			else
			{
				UICamera.currentKey = KeyCode.None;
			}
			UICamera.mLastScheme = value;
		}
	}

	public static bool disableController
	{
		get
		{
			return (!UICamera.mDisableController ? false : !UIPopupList.isOpen);
		}
		set
		{
			UICamera.mDisableController = value;
		}
	}

	public static int dragCount
	{
		get
		{
			int num = 0;
			int num1 = 0;
			int count = UICamera.activeTouches.Count;
			while (num1 < count)
			{
				if (UICamera.activeTouches[num1].dragged != null)
				{
					num++;
				}
				num1++;
			}
			for (int i = 0; i < (int)UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uICamera = UICamera.list.buffer[i];
				if (!(uICamera == null) && uICamera.enabled && NGUITools.GetActive(uICamera.gameObject))
				{
					return uICamera;
				}
			}
			return null;
		}
	}

	public static UICamera first
	{
		get
		{
			if (UICamera.list == null || UICamera.list.size == 0)
			{
				return null;
			}
			return UICamera.list[0];
		}
	}

	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	public static GameObject hoveredObject
	{
		get
		{
			if (UICamera.currentTouch != null && UICamera.currentTouch.dragStarted)
			{
				return UICamera.currentTouch.current;
			}
			if (UICamera.mHover && UICamera.mHover.activeInHierarchy)
			{
				return UICamera.mHover;
			}
			UICamera.mHover = null;
			return null;
		}
		set
		{
			Camera camera;
			if (UICamera.mHover == value)
			{
				return;
			}
			bool flag = false;
			UICamera uICamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.ShowTooltip(null);
			if (UICamera.mSelected && UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
				UICamera.mSelected = null;
			}
			if (UICamera.mHover)
			{
				UICamera.Notify(UICamera.mHover, "OnHover", false);
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, false);
				}
			}
			UICamera.mHover = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (UICamera.mHover)
			{
				if (UICamera.mHover != UICamera.controller.current && UICamera.mHover.GetComponent<UIKeyNavigation>() != null)
				{
					UICamera.controller.current = UICamera.mHover;
				}
				if (flag)
				{
					UICamera uICamera1 = (UICamera.mHover == null ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mHover.layer));
					if (uICamera1 != null)
					{
						UICamera.current = uICamera1;
						UICamera.currentCamera = uICamera1.cachedCamera;
					}
				}
				if (UICamera.onHover != null)
				{
					UICamera.onHover(UICamera.mHover, true);
				}
				UICamera.Notify(UICamera.mHover, "OnHover", true);
			}
			if (flag)
			{
				UICamera.current = uICamera;
				if (uICamera == null)
				{
					camera = null;
				}
				else
				{
					camera = uICamera.cachedCamera;
				}
				UICamera.currentCamera = camera;
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	public static bool inputHasFocus
	{
		get
		{
			if (UICamera.mInputFocus)
			{
				if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
				{
					return true;
				}
				UICamera.mInputFocus = false;
			}
			return false;
		}
	}

	public static bool isOverUI
	{
		get
		{
			if (UICamera.currentTouch != null)
			{
				return UICamera.currentTouch.isOverUI;
			}
			int num = 0;
			int count = UICamera.activeTouches.Count;
			while (num < count)
			{
				UICamera.MouseOrTouch item = UICamera.activeTouches[num];
				if (item.pressed != null && item.pressed != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(item.pressed) != null)
				{
					return true;
				}
				num++;
			}
			if (UICamera.mMouse[0].current != null && UICamera.mMouse[0].current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(UICamera.mMouse[0].current) != null)
			{
				return true;
			}
			if (UICamera.controller.pressed != null && UICamera.controller.pressed != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(UICamera.controller.pressed) != null)
			{
				return true;
			}
			return false;
		}
	}

	public static Vector2 lastEventPosition
	{
		get
		{
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				GameObject gameObject = UICamera.hoveredObject;
				if (gameObject != null)
				{
					Bounds bound = NGUIMath.CalculateAbsoluteWidgetBounds(gameObject.transform);
					return NGUITools.FindCameraForLayer(gameObject.layer).WorldToScreenPoint(bound.center);
				}
			}
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	[Obsolete("Use lastEventPosition instead. It handles controller input properly.")]
	public static Vector2 lastTouchPosition
	{
		get
		{
			return UICamera.mLastPos;
		}
		set
		{
			UICamera.mLastPos = value;
		}
	}

	public static Camera mainCamera
	{
		get
		{
			Camera camera;
			UICamera uICamera = UICamera.eventHandler;
			if (uICamera == null)
			{
				camera = null;
			}
			else
			{
				camera = uICamera.cachedCamera;
			}
			return camera;
		}
	}

	public static GameObject selectedObject
	{
		get
		{
			if (UICamera.mSelected && UICamera.mSelected.activeInHierarchy)
			{
				return UICamera.mSelected;
			}
			UICamera.mSelected = null;
			return null;
		}
		set
		{
			Camera camera;
			if (UICamera.mSelected == value)
			{
				UICamera.hoveredObject = value;
				UICamera.controller.current = value;
				return;
			}
			UICamera.ShowTooltip(null);
			bool flag = false;
			UICamera uICamera = UICamera.current;
			if (UICamera.currentTouch == null)
			{
				flag = true;
				UICamera.currentTouchID = -100;
				UICamera.currentTouch = UICamera.controller;
			}
			UICamera.mInputFocus = false;
			if (UICamera.mSelected)
			{
				UICamera.Notify(UICamera.mSelected, "OnSelect", false);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, false);
				}
			}
			UICamera.mSelected = value;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			if (value != null && value.GetComponent<UIKeyNavigation>() != null)
			{
				UICamera.controller.current = value;
			}
			if (UICamera.mSelected && flag)
			{
				UICamera uICamera1 = (UICamera.mSelected == null ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mSelected.layer));
				if (uICamera1 != null)
				{
					UICamera.current = uICamera1;
					UICamera.currentCamera = uICamera1.cachedCamera;
				}
			}
			if (UICamera.mSelected)
			{
				UICamera.mInputFocus = (!UICamera.mSelected.activeInHierarchy ? false : UICamera.mSelected.GetComponent<UIInput>() != null);
				if (UICamera.onSelect != null)
				{
					UICamera.onSelect(UICamera.mSelected, true);
				}
				UICamera.Notify(UICamera.mSelected, "OnSelect", true);
			}
			if (flag)
			{
				UICamera.current = uICamera;
				if (uICamera == null)
				{
					camera = null;
				}
				else
				{
					camera = uICamera.cachedCamera;
				}
				UICamera.currentCamera = camera;
				UICamera.currentTouch = null;
				UICamera.currentTouchID = -100;
			}
		}
	}

	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	public static GameObject tooltipObject
	{
		get
		{
			return UICamera.mTooltip;
		}
	}

	[Obsolete("Use either 'CountInputSources()' or 'activeTouches.Count'")]
	public static int touchCount
	{
		get
		{
			return UICamera.CountInputSources();
		}
	}

	static UICamera()
	{
		UICamera.list = new BetterList<UICamera>();
		UICamera.GetKeyDown = new UICamera.GetKeyStateFunc(Input.GetKeyDown);
		UICamera.GetKeyUp = new UICamera.GetKeyStateFunc(Input.GetKeyUp);
		UICamera.GetKey = new UICamera.GetKeyStateFunc(Input.GetKey);
		UICamera.GetAxis = new UICamera.GetAxisFunc(Input.GetAxis);
		UICamera.showTooltips = true;
		UICamera.mDisableController = false;
		UICamera.mLastPos = Vector2.zero;
		UICamera.lastWorldPosition = Vector3.zero;
		UICamera.current = null;
		UICamera.currentCamera = null;
		UICamera.mLastScheme = UICamera.ControlScheme.Mouse;
		UICamera.currentTouchID = -100;
		UICamera.mCurrentKey = KeyCode.Alpha0;
		UICamera.currentTouch = null;
		UICamera.mInputFocus = false;
		UICamera.mMouse = new UICamera.MouseOrTouch[] { new UICamera.MouseOrTouch(), new UICamera.MouseOrTouch(), new UICamera.MouseOrTouch() };
		UICamera.controller = new UICamera.MouseOrTouch();
		UICamera.activeTouches = new List<UICamera.MouseOrTouch>();
		UICamera.mTouchIDs = new List<int>();
		UICamera.mWidth = 0;
		UICamera.mHeight = 0;
		UICamera.mTooltip = null;
		UICamera.mTooltipTime = 0f;
		UICamera.isDragging = false;
		UICamera.mHit = new UICamera.DepthEntry();
		UICamera.mHits = new BetterList<UICamera.DepthEntry>();
		UICamera.m2DPlane = new Plane(Vector3.back, 0f);
		UICamera.mNextEvent = 0f;
		UICamera.mNotifying = 0;
		UICamera.mUsingTouchEvents = true;
	}

	public UICamera()
	{
	}

	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		UICamera.mMouse[0].pos = Input.mousePosition;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.mLastPos = UICamera.mMouse[0].pos;
	}

	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	public static int CountInputSources()
	{
		int num = 0;
		int num1 = 0;
		int count = UICamera.activeTouches.Count;
		while (num1 < count)
		{
			if (UICamera.activeTouches[num1].pressed != null)
			{
				num++;
			}
			num1++;
		}
		for (int i = 0; i < (int)UICamera.mMouse.Length; i++)
		{
			if (UICamera.mMouse[i].pressed != null)
			{
				num++;
			}
		}
		if (UICamera.controller.pressed != null)
		{
			num++;
		}
		return num;
	}

	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << (layer & 31);
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uICamera = UICamera.list.buffer[i];
			Camera camera = uICamera.cachedCamera;
			if (camera != null && (camera.cullingMask & num) != 0)
			{
				return uICamera;
			}
		}
		return null;
	}

	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody component = trans.GetComponent<Rigidbody>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			UICamera.currentKey = up;
			return 1;
		}
		if (!UICamera.GetKeyDown(down))
		{
			return 0;
		}
		UICamera.currentKey = down;
		return -1;
	}

	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0))
		{
			UICamera.currentKey = up0;
			return 1;
		}
		if (UICamera.GetKeyDown(up1))
		{
			UICamera.currentKey = up1;
			return 1;
		}
		if (UICamera.GetKeyDown(down0))
		{
			UICamera.currentKey = down0;
			return -1;
		}
		if (!UICamera.GetKeyDown(down1))
		{
			return 0;
		}
		UICamera.currentKey = down1;
		return -1;
	}

	private static int GetDirection(string axis)
	{
		float single = RealTime.time;
		if (UICamera.mNextEvent < single && !string.IsNullOrEmpty(axis))
		{
			float getAxis = UICamera.GetAxis(axis);
			if (getAxis > 0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = single + 0.25f;
				return 1;
			}
			if (getAxis < -0.75f)
			{
				UICamera.currentKey = KeyCode.JoystickButton0;
				UICamera.mNextEvent = single + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	public static UICamera.MouseOrTouch GetTouch(int id, bool createIfMissing = false)
	{
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		int num = 0;
		int count = UICamera.mTouchIDs.Count;
		while (num < count)
		{
			if (UICamera.mTouchIDs[num] == id)
			{
				return UICamera.activeTouches[num];
			}
			num++;
		}
		if (!createIfMissing)
		{
			return null;
		}
		UICamera.MouseOrTouch mouseOrTouch = new UICamera.MouseOrTouch()
		{
			pressTime = RealTime.time,
			touchBegan = true
		};
		UICamera.activeTouches.Add(mouseOrTouch);
		UICamera.mTouchIDs.Add(id);
		return mouseOrTouch;
	}

	private bool HasCollider(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		Collider component = go.GetComponent<Collider>();
		if (component != null)
		{
			return component.enabled;
		}
		Collider2D collider2D = go.GetComponent<Collider2D>();
		return (collider2D == null ? false : collider2D.enabled);
	}

	public static bool HideTooltip()
	{
		return UICamera.ShowTooltip(null);
	}

	public static bool IsHighlighted(GameObject go)
	{
		return UICamera.hoveredObject == go;
	}

	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		int num = 0;
		int count = UICamera.activeTouches.Count;
		while (num < count)
		{
			if (UICamera.activeTouches[num].pressed == go)
			{
				return true;
			}
			num++;
		}
		if (UICamera.controller.pressed == go)
		{
			return true;
		}
		return false;
	}

	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		for (UIPanel i = NGUITools.FindInParents<UIPanel>(go); i != null; i = i.parentPanel)
		{
			if (!i.IsVisible(worldPoint))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		for (UIPanel i = NGUITools.FindInParents<UIPanel>(de.go); i != null; i = i.parentPanel)
		{
			if (!i.IsVisible(de.point))
			{
				return false;
			}
		}
		return true;
	}

	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		int num = Screen.width;
		int num1 = Screen.height;
		if (num != UICamera.mWidth || num1 != UICamera.mHeight)
		{
			UICamera.mWidth = num;
			UICamera.mHeight = num1;
			UIRoot.Broadcast("UpdateAnchors");
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
	}

	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying > 10)
		{
			return;
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller && UIPopupList.isOpen && UIPopupList.current.source == go && UIPopupList.isOpen)
		{
			go = UIPopupList.current.gameObject;
		}
		if (go && go.activeInHierarchy)
		{
			UICamera.mNotifying++;
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
			UICamera.mNotifying--;
		}
	}

	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
				UICamera.activeTouches.Add(UICamera.currentTouch);
			}
			Vector2 vector2 = Input.mousePosition;
			UICamera.currentTouch.delta = vector2 - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = vector2;
			UICamera.Raycast(UICamera.currentTouch);
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			UICamera.currentKey = KeyCode.None;
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.activeTouches.Remove(UICamera.currentTouch);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	public void ProcessMouse()
	{
		bool flag = false;
		bool flag1 = false;
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButtonDown(i))
			{
				UICamera.currentKey = (KeyCode)(323 + i);
				flag1 = true;
				flag = true;
			}
			else if (Input.GetMouseButton(i))
			{
				UICamera.currentKey = (KeyCode)(323 + i);
				flag = true;
			}
		}
		if (UICamera.currentScheme == UICamera.ControlScheme.Touch)
		{
			return;
		}
		UICamera.currentTouch = UICamera.mMouse[0];
		Vector2 vector2 = Input.mousePosition;
		if (UICamera.currentTouch.ignoreDelta != 0)
		{
			UICamera.currentTouch.ignoreDelta--;
			UICamera.currentTouch.delta.x = 0f;
			UICamera.currentTouch.delta.y = 0f;
		}
		else
		{
			UICamera.currentTouch.delta = vector2 - UICamera.currentTouch.pos;
		}
		float single = UICamera.currentTouch.delta.sqrMagnitude;
		UICamera.currentTouch.pos = vector2;
		UICamera.mLastPos = vector2;
		bool flag2 = false;
		if (UICamera.currentScheme != UICamera.ControlScheme.Mouse)
		{
			if (single < 0.001f)
			{
				return;
			}
			UICamera.currentKey = KeyCode.Mouse0;
			flag2 = true;
		}
		else if (single > 0.001f)
		{
			flag2 = true;
		}
		for (int j = 1; j < 3; j++)
		{
			UICamera.mMouse[j].pos = UICamera.currentTouch.pos;
			UICamera.mMouse[j].delta = UICamera.currentTouch.delta;
		}
		if (flag || flag2 || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			UICamera.Raycast(UICamera.currentTouch);
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.currentTouch.current;
			}
		}
		bool flag3 = UICamera.currentTouch.last != UICamera.currentTouch.current;
		bool flag4 = UICamera.currentTouch.pressed != null;
		if (!flag4)
		{
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouchID = -1;
		if (flag3)
		{
			UICamera.currentKey = KeyCode.Mouse0;
		}
		if (!flag && flag2 && (!this.stickyTooltip || flag3))
		{
			if (UICamera.mTooltipTime != 0f)
			{
				UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
			}
			else if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
		}
		if (flag2 && UICamera.onMouseMove != null)
		{
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if (flag3 && (flag1 || flag4 && !flag))
		{
			UICamera.hoveredObject = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown || mouseButtonUp)
			{
				UICamera.currentKey = (KeyCode)(323 + l);
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = (KeyCode)(323 + l);
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
		}
		if (!flag && flag3)
		{
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.currentTouchID = -1;
			UICamera.currentKey = KeyCode.Mouse0;
			UICamera.hoveredObject = UICamera.currentTouch.current;
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag1 = false;
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyDown(271))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag1 = true;
		}
		else if (this.submitKey1 != KeyCode.None && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag1 = true;
		}
		else if ((this.submitKey0 == KeyCode.Return || this.submitKey1 == KeyCode.Return) && UICamera.GetKeyUp(271))
		{
			UICamera.currentKey = this.submitKey0;
			flag1 = true;
		}
		if (flag)
		{
			UICamera.currentTouch.pressTime = RealTime.time;
		}
		if ((flag || flag1) && UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			this.ProcessTouch(flag, flag1);
			UICamera.currentTouch.last = UICamera.currentTouch.current;
		}
		KeyCode keyCode = KeyCode.None;
		if (this.useController)
		{
			if (!UICamera.disableController && UICamera.currentScheme == UICamera.ControlScheme.Controller && (UICamera.currentTouch.current == null || !UICamera.currentTouch.current.activeInHierarchy))
			{
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
			}
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				int direction = UICamera.GetDirection(this.verticalAxisName);
				if (direction != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = (direction <= 0 ? KeyCode.DownArrow : KeyCode.UpArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				int num = UICamera.GetDirection(this.horizontalAxisName);
				if (num != 0)
				{
					UICamera.ShowTooltip(null);
					UICamera.currentScheme = UICamera.ControlScheme.Controller;
					UICamera.currentTouch.current = UICamera.controllerNavigationObject;
					if (UICamera.currentTouch.current != null)
					{
						keyCode = (num <= 0 ? KeyCode.LeftArrow : KeyCode.RightArrow);
						if (UICamera.onNavigate != null)
						{
							UICamera.onNavigate(UICamera.currentTouch.current, keyCode);
						}
						UICamera.Notify(UICamera.currentTouch.current, "OnNavigate", keyCode);
					}
				}
			}
			float single = (string.IsNullOrEmpty(this.horizontalPanAxisName) ? 0f : UICamera.GetAxis(this.horizontalPanAxisName));
			float single1 = (string.IsNullOrEmpty(this.verticalPanAxisName) ? 0f : UICamera.GetAxis(this.verticalPanAxisName));
			if (single != 0f || single1 != 0f)
			{
				UICamera.ShowTooltip(null);
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = UICamera.controllerNavigationObject;
				if (UICamera.currentTouch.current != null)
				{
					Vector2 vector2 = new Vector2(single, single1);
					vector2 *= Time.unscaledDeltaTime;
					if (UICamera.onPan != null)
					{
						UICamera.onPan(UICamera.currentTouch.current, vector2);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnPan", vector2);
				}
			}
		}
		if ((UICamera.GetAnyKeyDown == null ? Input.anyKeyDown : UICamera.GetAnyKeyDown()))
		{
			int num1 = 0;
			int length = (int)NGUITools.keys.Length;
			while (num1 < length)
			{
				KeyCode keyCode1 = NGUITools.keys[num1];
				if (keyCode != keyCode1)
				{
					if (UICamera.GetKeyDown(keyCode1))
					{
						if (this.useKeyboard || keyCode1 >= KeyCode.Mouse0)
						{
							if (this.useController || keyCode1 < KeyCode.JoystickButton0)
							{
								if (this.useMouse || keyCode1 < KeyCode.Mouse0 && keyCode1 > KeyCode.Mouse6)
								{
									UICamera.currentKey = keyCode1;
									if (UICamera.onKey != null)
									{
										UICamera.onKey(UICamera.currentTouch.current, keyCode1);
									}
									UICamera.Notify(UICamera.currentTouch.current, "OnKey", keyCode1);
								}
							}
						}
					}
				}
				num1++;
			}
		}
		UICamera.currentTouch = null;
	}

	private void ProcessPress(bool pressed, float click, float drag)
	{
		if (pressed)
		{
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == null && UICamera.currentTouch.current != null)
			{
				UICamera.hoveredObject = UICamera.currentTouch.current;
			}
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null && UICamera.currentTouch.pressed)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.mTooltip != null)
			{
				UICamera.ShowTooltip(null);
			}
			if (UICamera.mSelected != UICamera.currentTouch.pressed)
			{
				UICamera.mInputFocus = false;
				if (UICamera.mSelected)
				{
					UICamera.Notify(UICamera.mSelected, "OnSelect", false);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, false);
					}
				}
				UICamera.mSelected = UICamera.currentTouch.pressed;
				if (UICamera.currentTouch.pressed != null && UICamera.currentTouch.pressed.GetComponent<UIKeyNavigation>() != null)
				{
					UICamera.controller.current = UICamera.currentTouch.pressed;
				}
				if (UICamera.mSelected)
				{
					UICamera.mInputFocus = (!UICamera.mSelected.activeInHierarchy ? false : UICamera.mSelected.GetComponent<UIInput>() != null);
					if (UICamera.onSelect != null)
					{
						UICamera.onSelect(UICamera.mSelected, true);
					}
					UICamera.Notify(UICamera.mSelected, "OnSelect", true);
				}
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.sqrMagnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float single = UICamera.currentTouch.totalDelta.sqrMagnitude;
			bool flag = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && drag < single)
			{
				flag = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.mTooltip != null)
				{
					UICamera.ShowTooltip(null);
				}
				UICamera.isDragging = true;
				bool flag1 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragOut != null)
					{
						UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag1)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
				else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && click < single)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
	}

	private void ProcessRelease(bool isMouse, float drag)
	{
		if (UICamera.currentTouch == null)
		{
			return;
		}
		UICamera.currentTouch.pressStarted = false;
		if (UICamera.currentTouch.pressed != null)
		{
			if (UICamera.currentTouch.dragStarted)
			{
				if (UICamera.onDragOut != null)
				{
					UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
				if (UICamera.onDragEnd != null)
				{
					UICamera.onDragEnd(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
			}
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			if (isMouse && this.HasCollider(UICamera.currentTouch.pressed))
			{
				if (UICamera.mHover != UICamera.currentTouch.current)
				{
					UICamera.hoveredObject = UICamera.currentTouch.current;
				}
				else
				{
					if (UICamera.onHover != null)
					{
						UICamera.onHover(UICamera.currentTouch.current, true);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
			}
			if (UICamera.currentTouch.dragged != UICamera.currentTouch.current && (UICamera.currentScheme == UICamera.ControlScheme.Controller || UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None || UICamera.currentTouch.totalDelta.sqrMagnitude >= drag))
			{
				if (UICamera.currentTouch.dragStarted)
				{
					if (UICamera.onDrop != null)
					{
						UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
				}
			}
			else if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
			{
				UICamera.ShowTooltip(null);
				float single = RealTime.time;
				if (UICamera.onClick != null)
				{
					UICamera.onClick(UICamera.currentTouch.pressed);
				}
				UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
				if (UICamera.currentTouch.clickTime + 0.35f > single)
				{
					if (UICamera.onDoubleClick != null)
					{
						UICamera.onDoubleClick(UICamera.currentTouch.pressed);
					}
					UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
				}
				UICamera.currentTouch.clickTime = single;
			}
		}
		UICamera.currentTouch.dragStarted = false;
		UICamera.currentTouch.pressed = null;
		UICamera.currentTouch.dragged = null;
	}

	public void ProcessTouch(bool pressed, bool released)
	{
		if (pressed)
		{
			UICamera.mTooltipTime = Time.unscaledTime + this.tooltipDelay;
		}
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float single = (!flag ? this.touchDragThreshold : this.mouseDragThreshold);
		float single1 = (!flag ? this.touchClickThreshold : this.mouseClickThreshold);
		single *= single;
		single1 *= single1;
		if (UICamera.currentTouch.pressed != null)
		{
			if (released)
			{
				this.ProcessRelease(flag, single);
			}
			this.ProcessPress(pressed, single1, single);
			if (UICamera.currentTouch.pressed == UICamera.currentTouch.current && UICamera.mTooltipTime != 0f && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && !UICamera.currentTouch.dragStarted && UICamera.currentTouch.deltaTime > this.tooltipDelay)
			{
				UICamera.mTooltipTime = 0f;
				UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				if (this.longPressTooltip)
				{
					UICamera.ShowTooltip(UICamera.currentTouch.pressed);
				}
				UICamera.Notify(UICamera.currentTouch.current, "OnLongPress", null);
			}
		}
		else if (flag || pressed || released)
		{
			this.ProcessPress(pressed, single1, single);
			if (released)
			{
				this.ProcessRelease(flag, single);
			}
		}
	}

	public void ProcessTouches()
	{
		int num;
		TouchPhase touchPhase;
		Vector2 vector2;
		int num1;
		int num2 = (UICamera.GetInputTouchCount != null ? UICamera.GetInputTouchCount() : Input.touchCount);
		int num3 = 0;
		while (num3 < num2)
		{
			float single = 0f;
			float single1 = 1f;
			if (UICamera.GetInputTouch != null)
			{
				UICamera.Touch getInputTouch = UICamera.GetInputTouch(num3);
				touchPhase = getInputTouch.phase;
				num = getInputTouch.fingerId;
				vector2 = getInputTouch.position;
				num1 = getInputTouch.tapCount;
			}
			else
			{
				UnityEngine.Touch touch = Input.GetTouch(num3);
				touchPhase = touch.phase;
				num = touch.fingerId;
				vector2 = touch.position;
				num1 = touch.tapCount;
				single = touch.pressure;
				single1 = touch.maximumPossiblePressure;
			}
			UICamera.currentTouchID = (!this.allowMultiTouch ? 1 : num);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID, true);
			bool flag = (touchPhase == TouchPhase.Began ? true : UICamera.currentTouch.touchBegan);
			bool flag1 = (touchPhase == TouchPhase.Canceled ? true : touchPhase == TouchPhase.Ended);
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.delta = vector2 - UICamera.currentTouch.pos;
			UICamera.currentTouch.pos = vector2;
			UICamera.currentKey = KeyCode.None;
			UICamera.currentTouch.pressure = single;
			UICamera.currentTouch.maxPressure = single1;
			UICamera.Raycast(UICamera.currentTouch);
			if (Defs.touchPressureSupported && Defs.isUse3DTouch && UICamera.currentTouch.current != null)
			{
				UICamera.Notify(UICamera.currentTouch.current, "OnPressure", (!flag1 ? UICamera.currentTouch.pressure / UICamera.currentTouch.maxPressure : 0f));
			}
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (num1 > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag1);
			if (flag1)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (this.allowMultiTouch)
			{
				num3++;
			}
			else
			{
				break;
			}
		}
		if (num2 != 0)
		{
			UICamera.mUsingTouchEvents = true;
		}
		else
		{
			if (UICamera.mUsingTouchEvents)
			{
				UICamera.mUsingTouchEvents = false;
				return;
			}
			if (this.useMouse)
			{
				this.ProcessMouse();
			}
		}
	}

	public static void Raycast(UICamera.MouseOrTouch touch)
	{
		if (!UICamera.Raycast(touch.pos))
		{
			UICamera.mRayHitObject = UICamera.fallThrough;
		}
		if (UICamera.mRayHitObject == null)
		{
			UICamera.mRayHitObject = UICamera.mGenericHandler;
		}
		touch.last = touch.current;
		touch.current = UICamera.mRayHitObject;
		UICamera.mLastPos = touch.pos;
	}

	public static bool Raycast(Vector3 inPos)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uICamera = UICamera.list.buffer[i];
			if (uICamera.enabled && NGUITools.GetActive(uICamera.gameObject))
			{
				UICamera.currentCamera = uICamera.cachedCamera;
				Vector3 viewportPoint = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(viewportPoint.x) && !float.IsNaN(viewportPoint.y))
				{
					if (viewportPoint.x >= 0f && viewportPoint.x <= 1f && viewportPoint.y >= 0f && viewportPoint.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int num = UICamera.currentCamera.cullingMask & uICamera.eventReceiverMask;
						float single = (uICamera.rangeDistance <= 0f ? UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane : uICamera.rangeDistance);
						if (uICamera.eventType == UICamera.EventType.World_3D)
						{
							if (Physics.Raycast(ray, out UICamera.lastHit, single, num))
							{
								UICamera.lastWorldPosition = UICamera.lastHit.point;
								UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
								if (!UICamera.list[0].eventsGoToColliders)
								{
									Rigidbody rigidbody = UICamera.FindRootRigidbody(UICamera.mRayHitObject.transform);
									if (rigidbody != null)
									{
										UICamera.mRayHitObject = rigidbody.gameObject;
									}
								}
								return true;
							}
						}
						else if (uICamera.eventType == UICamera.EventType.UI_3D)
						{
							RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, single, num);
							if ((int)raycastHitArray.Length > 1)
							{
								for (int j = 0; j < (int)raycastHitArray.Length; j++)
								{
									GameObject gameObject = raycastHitArray[j].collider.gameObject;
									UIWidget component = gameObject.GetComponent<UIWidget>();
									if (component == null)
									{
										UIRect uIRect = NGUITools.FindInParents<UIRect>(gameObject);
										if (!(uIRect != null) || uIRect.finalAlpha >= 0.001f)
										{
											goto Label9;
										}
										goto Label8;
									}
									else if (!component.isVisible)
									{
										goto Label8;
									}
									else if (component.hitCheck != null && !component.hitCheck(raycastHitArray[j].point))
									{
										goto Label8;
									}
								Label9:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.hit = raycastHitArray[j];
										UICamera.mHit.point = raycastHitArray[j].point;
										UICamera.mHit.go = raycastHitArray[j].collider.gameObject;
										UICamera.mHits.Add(UICamera.mHit);
									}
								Label8:
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										UICamera.lastHit = UICamera.mHits[k].hit;
										UICamera.mRayHitObject = UICamera.mHits[k].go;
										UICamera.lastWorldPosition = UICamera.mHits[k].point;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if ((int)raycastHitArray.Length == 1)
							{
								GameObject gameObject1 = raycastHitArray[0].collider.gameObject;
								UIWidget uIWidget = gameObject1.GetComponent<UIWidget>();
								if (uIWidget == null)
								{
									UIRect uIRect1 = NGUITools.FindInParents<UIRect>(gameObject1);
									if (!(uIRect1 != null) || uIRect1.finalAlpha >= 0.001f)
									{
										goto Label6;
									}
									goto Label0;
								}
								else if (!uIWidget.isVisible)
								{
									goto Label0;
								}
								else if (uIWidget.hitCheck != null && !uIWidget.hitCheck(raycastHitArray[0].point))
								{
									goto Label0;
								}
							Label6:
								if (UICamera.IsVisible(raycastHitArray[0].point, raycastHitArray[0].collider.gameObject))
								{
									UICamera.lastHit = raycastHitArray[0];
									UICamera.lastWorldPosition = raycastHitArray[0].point;
									UICamera.mRayHitObject = UICamera.lastHit.collider.gameObject;
									return true;
								}
							}
						}
						else if (uICamera.eventType == UICamera.EventType.World_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out single))
							{
								Vector3 point = ray.GetPoint(single);
								Collider2D collider2D = Physics2D.OverlapPoint(point, num);
								if (collider2D)
								{
									UICamera.lastWorldPosition = point;
									UICamera.mRayHitObject = collider2D.gameObject;
									if (!uICamera.eventsGoToColliders)
									{
										Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.mRayHitObject.transform);
										if (rigidbody2D != null)
										{
											UICamera.mRayHitObject = rigidbody2D.gameObject;
										}
									}
									return true;
								}
							}
						}
						else if (uICamera.eventType == UICamera.EventType.UI_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out single))
							{
								UICamera.lastWorldPosition = ray.GetPoint(single);
								Collider2D[] collider2DArray = Physics2D.OverlapPointAll(UICamera.lastWorldPosition, num);
								if ((int)collider2DArray.Length > 1)
								{
									for (int l = 0; l < (int)collider2DArray.Length; l++)
									{
										GameObject gameObject2 = collider2DArray[l].gameObject;
										UIWidget component1 = gameObject2.GetComponent<UIWidget>();
										if (component1 == null)
										{
											UIRect uIRect2 = NGUITools.FindInParents<UIRect>(gameObject2);
											if (!(uIRect2 != null) || uIRect2.finalAlpha >= 0.001f)
											{
												goto Label4;
											}
											goto Label3;
										}
										else if (!component1.isVisible)
										{
											goto Label3;
										}
										else if (component1.hitCheck != null && !component1.hitCheck(UICamera.lastWorldPosition))
										{
											goto Label3;
										}
									Label4:
										UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject2);
										if (UICamera.mHit.depth != 2147483647)
										{
											UICamera.mHit.go = gameObject2;
											UICamera.mHit.point = UICamera.lastWorldPosition;
											UICamera.mHits.Add(UICamera.mHit);
										}
									Label3:
									}
									UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
									for (int m = 0; m < UICamera.mHits.size; m++)
									{
										if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
										{
											UICamera.mRayHitObject = UICamera.mHits[m].go;
											UICamera.mHits.Clear();
											return true;
										}
									}
									UICamera.mHits.Clear();
								}
								else if ((int)collider2DArray.Length == 1)
								{
									GameObject gameObject3 = collider2DArray[0].gameObject;
									UIWidget uIWidget1 = gameObject3.GetComponent<UIWidget>();
									if (uIWidget1 == null)
									{
										UIRect uIRect3 = NGUITools.FindInParents<UIRect>(gameObject3);
										if (!(uIRect3 != null) || uIRect3.finalAlpha >= 0.001f)
										{
											goto Label1;
										}
										goto Label0;
									}
									else if (!uIWidget1.isVisible)
									{
										goto Label0;
									}
									else if (uIWidget1.hitCheck != null && !uIWidget1.hitCheck(UICamera.lastWorldPosition))
									{
										goto Label0;
									}
								Label1:
									if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject3))
									{
										UICamera.mRayHitObject = gameObject3;
										return true;
									}
								}
							}
						}
					}
				}
			}
		Label0:
		}
		return false;
	}

	public static void RemoveTouch(int id)
	{
		int num = 0;
		int count = UICamera.mTouchIDs.Count;
		while (num < count)
		{
			if (UICamera.mTouchIDs[num] == id)
			{
				UICamera.mTouchIDs.RemoveAt(num);
				UICamera.activeTouches.RemoveAt(num);
				return;
			}
			num++;
		}
	}

	public static bool ShowTooltip(GameObject go)
	{
		if (UICamera.mTooltip == go)
		{
			return false;
		}
		if (UICamera.mTooltip != null)
		{
			if (UICamera.onTooltip != null)
			{
				UICamera.onTooltip(UICamera.mTooltip, false);
			}
			UICamera.Notify(UICamera.mTooltip, "OnTooltip", false);
		}
		UICamera.mTooltip = go;
		UICamera.mTooltipTime = 0f;
		if (UICamera.mTooltip != null)
		{
			if (UICamera.onTooltip != null)
			{
				UICamera.onTooltip(UICamera.mTooltip, true);
			}
			UICamera.Notify(UICamera.mTooltip, "OnTooltip", true);
		}
		return true;
	}

	private void Start()
	{
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic)
		{
			this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
		}
		if (Application.isPlaying)
		{
			if (UICamera.fallThrough == null)
			{
				UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				if (uIRoot == null)
				{
					Transform transforms = base.transform;
					UICamera.fallThrough = (transforms.parent == null ? base.gameObject : transforms.parent.gameObject);
				}
				else
				{
					UICamera.fallThrough = uIRoot.gameObject;
				}
			}
			this.cachedCamera.eventMask = 0;
		}
	}

	private void Update()
	{
		this.allowMultiTouch = (!(WeaponManager.sharedManager != null) || !(WeaponManager.sharedManager.myPlayerMoveC != null) || BankController.Instance != null && BankController.Instance.InterfaceEnabled || ShopNGUIController.GuiActive || Pauser.sharedPauser != null && Pauser.sharedPauser.paused || ChatViewrController.sharedController != null ? false : !WeaponManager.sharedManager.myPlayerMoveC.showRanks);
		if (!this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		NGUIDebug.debugRaycast = this.debug;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if ((this.useKeyboard || this.useController) && !UICamera.disableController)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float single = (string.IsNullOrEmpty(this.scrollAxisName) ? 0f : UICamera.GetAxis(this.scrollAxisName));
			if (single != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, single);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", single);
			}
			if (UICamera.showTooltips && UICamera.mTooltipTime != 0f && !UIPopupList.isOpen && UICamera.mMouse[0].dragged == null && (UICamera.mTooltipTime < RealTime.time || UICamera.GetKey(304) || UICamera.GetKey(303)))
			{
				UICamera.currentTouch = UICamera.mMouse[0];
				UICamera.currentTouchID = -1;
				UICamera.ShowTooltip(UICamera.mHover);
			}
		}
		if (UICamera.mTooltip != null && !NGUITools.GetActive(UICamera.mTooltip))
		{
			UICamera.ShowTooltip(null);
		}
		UICamera.current = null;
		UICamera.currentTouchID = -100;
	}

	public delegate void BoolDelegate(GameObject go, bool state);

	public enum ClickNotification
	{
		None,
		Always,
		BasedOnDelta
	}

	public enum ControlScheme
	{
		Mouse,
		Touch,
		Controller
	}

	private struct DepthEntry
	{
		public int depth;

		public RaycastHit hit;

		public Vector3 point;

		public GameObject go;
	}

	public enum EventType
	{
		World_3D,
		UI_3D,
		World_2D,
		UI_2D
	}

	public delegate void FloatDelegate(GameObject go, float delta);

	public delegate bool GetAnyKeyFunc();

	public delegate float GetAxisFunc(string name);

	public delegate bool GetKeyStateFunc(KeyCode key);

	public delegate UICamera.Touch GetTouchCallback(int index);

	public delegate int GetTouchCountCallback();

	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	public class MouseOrTouch
	{
		public KeyCode key;

		public Vector2 pos;

		public Vector2 lastPos;

		public Vector2 delta;

		public Vector2 totalDelta;

		public Camera pressedCam;

		public GameObject last;

		public GameObject current;

		public GameObject pressed;

		public GameObject dragged;

		public float pressTime;

		public float clickTime;

		public UICamera.ClickNotification clickNotification;

		public bool touchBegan;

		public bool pressStarted;

		public bool dragStarted;

		public int ignoreDelta;

		public float pressure;

		public float maxPressure;

		public float deltaTime
		{
			get
			{
				return RealTime.time - this.pressTime;
			}
		}

		public bool isOverUI
		{
			get
			{
				return (!(this.current != null) || !(this.current != UICamera.fallThrough) ? false : NGUITools.FindInParents<UIRoot>(this.current) != null);
			}
		}

		public MouseOrTouch()
		{
		}
	}

	public delegate void MoveDelegate(Vector2 delta);

	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	public delegate void OnCustomInput();

	public delegate void OnSchemeChange();

	public delegate void OnScreenResize();

	public class Touch
	{
		public int fingerId;

		public TouchPhase phase;

		public Vector2 position;

		public int tapCount;

		public Touch()
		{
		}
	}

	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	public delegate void VoidDelegate(GameObject go);
}