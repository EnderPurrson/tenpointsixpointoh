using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	public static List<UIRoot> list;

	public UIRoot.Scaling scalingStyle;

	public int manualWidth = 1280;

	public int manualHeight = 720;

	public int minimumHeight = 320;

	public int maximumHeight = 1536;

	public bool fitWidth;

	public bool fitHeight = true;

	public bool adjustByDPI;

	public bool shrinkPortraitUI;

	private Transform mTrans;

	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 vector2 = NGUITools.screenSize;
				float single = vector2.x / vector2.y;
				if (vector2.y < (float)this.minimumHeight)
				{
					vector2.y = (float)this.minimumHeight;
					vector2.x = vector2.y * single;
				}
				else if (vector2.y > (float)this.maximumHeight)
				{
					vector2.y = (float)this.maximumHeight;
					vector2.x = vector2.y * single;
				}
				int num = Mathf.RoundToInt((!this.shrinkPortraitUI || vector2.y <= vector2.x ? vector2.y : vector2.y / single));
				return (!this.adjustByDPI ? num : NGUIMath.AdjustByDPI((float)num));
			}
			UIRoot.Constraint constraint = this.constraint;
			if (constraint == UIRoot.Constraint.FitHeight)
			{
				return this.manualHeight;
			}
			Vector2 vector21 = NGUITools.screenSize;
			float single1 = vector21.x / vector21.y;
			float single2 = (float)this.manualWidth / (float)this.manualHeight;
			switch (constraint)
			{
				case UIRoot.Constraint.Fit:
				{
					return (single2 <= single1 ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / single1));
				}
				case UIRoot.Constraint.Fill:
				{
					return (single2 >= single1 ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / single1));
				}
				case UIRoot.Constraint.FitWidth:
				{
					return Mathf.RoundToInt((float)this.manualWidth / single1);
				}
			}
			return this.manualHeight;
		}
	}

	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Constrained;
			}
			return scaling;
		}
	}

	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			if (this.fitHeight)
			{
				return UIRoot.Constraint.FitHeight;
			}
			return UIRoot.Constraint.Fill;
		}
	}

	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			return (num != -1 ? this.GetPixelSizeAdjustment(num) : 1f);
		}
	}

	static UIRoot()
	{
		UIRoot.list = new List<UIRoot>();
	}

	public UIRoot()
	{
	}

	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	public static void Broadcast(string funcName)
	{
		int num = 0;
		int count = UIRoot.list.Count;
		while (num < count)
		{
			UIRoot item = UIRoot.list[num];
			if (item != null)
			{
				item.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			num++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		if (param != null)
		{
			int num = 0;
			int count = UIRoot.list.Count;
			while (num < count)
			{
				UIRoot item = UIRoot.list[num];
				if (item != null)
				{
					item.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
				}
				num++;
			}
		}
		else
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
	}

	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(go);
		return (uIRoot == null ? 1f : uIRoot.pixelSizeAdjustment);
	}

	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height <= this.maximumHeight)
		{
			return 1f;
		}
		return (float)this.maximumHeight / (float)height;
	}

	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren == null)
		{
			this.UpdateScale(false);
		}
		else
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
	}

	private void Update()
	{
		this.UpdateScale(true);
	}

	public void UpdateScale(bool updateAnchors = true)
	{
		if (this.mTrans != null)
		{
			float single = (float)this.activeHeight;
			if (single > 0f)
			{
				float single1 = 2f / single;
				Vector3 vector3 = this.mTrans.localScale;
				if (Mathf.Abs(vector3.x - single1) > 1E-45f || Mathf.Abs(vector3.y - single1) > 1E-45f || Mathf.Abs(vector3.z - single1) > 1E-45f)
				{
					this.mTrans.localScale = new Vector3(single1, single1, single1);
					if (updateAnchors)
					{
						base.BroadcastMessage("UpdateAnchors");
					}
				}
			}
		}
	}

	public enum Constraint
	{
		Fit,
		Fill,
		FitWidth,
		FitHeight
	}

	public enum Scaling
	{
		Flexible,
		Constrained,
		ConstrainedOnMobiles
	}
}