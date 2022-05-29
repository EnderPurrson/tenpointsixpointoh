using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour
{
	public static BetterList<UIKeyNavigation> list;

	public UIKeyNavigation.Constraint constraint;

	public GameObject onUp;

	public GameObject onDown;

	public GameObject onLeft;

	public GameObject onRight;

	public GameObject onClick;

	public GameObject onTab;

	public bool startsSelected;

	[NonSerialized]
	private bool mStarted;

	public static int mLastFrame;

	public static UIKeyNavigation current
	{
		get
		{
			GameObject gameObject = UICamera.hoveredObject;
			if (gameObject == null)
			{
				return null;
			}
			return gameObject.GetComponent<UIKeyNavigation>();
		}
	}

	public bool isColliderEnabled
	{
		get
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D collider2D = base.GetComponent<Collider2D>();
			return (collider2D == null ? false : collider2D.enabled);
		}
	}

	static UIKeyNavigation()
	{
		UIKeyNavigation.list = new BetterList<UIKeyNavigation>();
		UIKeyNavigation.mLastFrame = 0;
	}

	public UIKeyNavigation()
	{
	}

	public GameObject Get(Vector3 myDir, float x = 1f, float y = 1f)
	{
		Transform transforms = base.transform;
		myDir = transforms.TransformDirection(myDir);
		Vector3 center = UIKeyNavigation.GetCenter(base.gameObject);
		float single = Single.MaxValue;
		GameObject gameObject = null;
		for (int i = 0; i < UIKeyNavigation.list.size; i++)
		{
			UIKeyNavigation item = UIKeyNavigation.list[i];
			if (!(item == this) && item.constraint != UIKeyNavigation.Constraint.Explicit && item.isColliderEnabled)
			{
				UIWidget component = item.GetComponent<UIWidget>();
				if (!(component != null) || component.alpha != 0f)
				{
					Vector3 vector3 = UIKeyNavigation.GetCenter(item.gameObject) - center;
					if (Vector3.Dot(myDir, vector3.normalized) >= 0.707f)
					{
						vector3 = transforms.InverseTransformDirection(vector3);
						vector3.x *= x;
						vector3.y *= y;
						float single1 = vector3.sqrMagnitude;
						if (single1 <= single)
						{
							gameObject = item.gameObject;
							single = single1;
						}
					}
				}
			}
		}
		return gameObject;
	}

	protected static Vector3 GetCenter(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		UICamera uICamera = UICamera.FindCameraForLayer(go.layer);
		if (uICamera == null)
		{
			if (component == null)
			{
				return go.transform.position;
			}
			Vector3[] vector3Array = component.worldCorners;
			return (vector3Array[0] + vector3Array[2]) * 0.5f;
		}
		Vector3 screenPoint = go.transform.position;
		if (component != null)
		{
			Vector3[] vector3Array1 = component.worldCorners;
			screenPoint = (vector3Array1[0] + vector3Array1[2]) * 0.5f;
		}
		screenPoint = uICamera.cachedCamera.WorldToScreenPoint(screenPoint);
		screenPoint.z = 0f;
		return screenPoint;
	}

	public GameObject GetDown()
	{
		if (UIKeyNavigation.IsActive(this.onDown))
		{
			return this.onDown;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.down, 2f, 1f);
	}

	public GameObject GetLeft()
	{
		if (UIKeyNavigation.IsActive(this.onLeft))
		{
			return this.onLeft;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.left, 1f, 2f);
	}

	public GameObject GetRight()
	{
		if (UIKeyNavigation.IsActive(this.onRight))
		{
			return this.onRight;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.right, 1f, 2f);
	}

	public GameObject GetUp()
	{
		if (UIKeyNavigation.IsActive(this.onUp))
		{
			return this.onUp;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.up, 2f, 1f);
	}

	private static bool IsActive(GameObject go)
	{
		if (!go || !go.activeInHierarchy)
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

	protected virtual void OnClick()
	{
		if (NGUITools.GetActive(this.onClick))
		{
			UICamera.hoveredObject = this.onClick;
		}
	}

	protected virtual void OnDisable()
	{
		UIKeyNavigation.list.Remove(this);
	}

	protected virtual void OnEnable()
	{
		UIKeyNavigation.list.Add(this);
		if (this.mStarted)
		{
			this.Start();
		}
	}

	public virtual void OnKey(KeyCode key)
	{
		if (UIPopupList.isOpen)
		{
			return;
		}
		if (UIKeyNavigation.mLastFrame == Time.frameCount)
		{
			return;
		}
		UIKeyNavigation.mLastFrame = Time.frameCount;
		if (key == KeyCode.Tab)
		{
			GameObject left = this.onTab;
			if (left == null)
			{
				if (UICamera.GetKey(304) || UICamera.GetKey(303))
				{
					left = this.GetLeft();
					if (left == null)
					{
						left = this.GetUp();
					}
					if (left == null)
					{
						left = this.GetDown();
					}
					if (left == null)
					{
						left = this.GetRight();
					}
				}
				else
				{
					left = this.GetRight();
					if (left == null)
					{
						left = this.GetDown();
					}
					if (left == null)
					{
						left = this.GetUp();
					}
					if (left == null)
					{
						left = this.GetLeft();
					}
				}
			}
			if (left != null)
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.hoveredObject = left;
				UIInput component = left.GetComponent<UIInput>();
				if (component != null)
				{
					component.isSelected = true;
				}
			}
		}
	}

	public virtual void OnNavigate(KeyCode key)
	{
		if (UIPopupList.isOpen)
		{
			return;
		}
		if (UIKeyNavigation.mLastFrame == Time.frameCount)
		{
			return;
		}
		UIKeyNavigation.mLastFrame = Time.frameCount;
		GameObject up = null;
		switch (key)
		{
			case KeyCode.UpArrow:
			{
				up = this.GetUp();
				break;
			}
			case KeyCode.DownArrow:
			{
				up = this.GetDown();
				break;
			}
			case KeyCode.RightArrow:
			{
				up = this.GetRight();
				break;
			}
			case KeyCode.LeftArrow:
			{
				up = this.GetLeft();
				break;
			}
		}
		if (up != null)
		{
			UICamera.hoveredObject = up;
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.startsSelected && this.isColliderEnabled)
		{
			UICamera.hoveredObject = base.gameObject;
		}
	}

	public enum Constraint
	{
		None,
		Vertical,
		Horizontal,
		Explicit
	}
}