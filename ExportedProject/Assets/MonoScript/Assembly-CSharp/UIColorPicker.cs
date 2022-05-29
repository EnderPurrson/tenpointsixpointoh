using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class UIColorPicker : MonoBehaviour
{
	public static UIColorPicker current;

	public Color @value = Color.white;

	public UIWidget selectionWidget;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	[NonSerialized]
	private Transform mTrans;

	[NonSerialized]
	private UITexture mUITex;

	[NonSerialized]
	private Texture2D mTex;

	[NonSerialized]
	private UICamera mCam;

	[NonSerialized]
	private Vector2 mPos;

	[NonSerialized]
	private int mWidth;

	[NonSerialized]
	private int mHeight;

	private static AnimationCurve mRed;

	private static AnimationCurve mGreen;

	private static AnimationCurve mBlue;

	public UIColorPicker()
	{
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.mTex);
		this.mTex = null;
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.enabled)
		{
			this.Sample();
		}
	}

	private void OnPan(Vector2 delta)
	{
		if (base.enabled)
		{
			this.mPos.x = Mathf.Clamp01(this.mPos.x + delta.x);
			this.mPos.y = Mathf.Clamp01(this.mPos.y + delta.y);
			this.Select(this.mPos);
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && pressed && UICamera.currentScheme != UICamera.ControlScheme.Controller)
		{
			this.Sample();
		}
	}

	private void Sample()
	{
		Vector3 worldPoint = UICamera.lastEventPosition;
		worldPoint = this.mCam.cachedCamera.ScreenToWorldPoint(worldPoint);
		worldPoint = this.mTrans.InverseTransformPoint(worldPoint);
		Vector3[] vector3Array = this.mUITex.localCorners;
		this.mPos.x = Mathf.Clamp01((worldPoint.x - vector3Array[0].x) / (vector3Array[2].x - vector3Array[0].x));
		this.mPos.y = Mathf.Clamp01((worldPoint.y - vector3Array[0].y) / (vector3Array[2].y - vector3Array[0].y));
		if (this.selectionWidget != null)
		{
			worldPoint.x = Mathf.Lerp(vector3Array[0].x, vector3Array[2].x, this.mPos.x);
			worldPoint.y = Mathf.Lerp(vector3Array[0].y, vector3Array[2].y, this.mPos.y);
			worldPoint = this.mTrans.TransformPoint(worldPoint);
			this.selectionWidget.transform.OverlayPosition(worldPoint, this.mCam.cachedCamera);
		}
		this.@value = UIColorPicker.Sample(this.mPos.x, this.mPos.y);
		UIColorPicker.current = this;
		EventDelegate.Execute(this.onChange);
		UIColorPicker.current = null;
	}

	public static Color Sample(float x, float y)
	{
		if (UIColorPicker.mRed == null)
		{
			UIColorPicker.mRed = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(0.14285715f, 1f), new Keyframe(0.2857143f, 0f), new Keyframe(0.42857143f, 0f), new Keyframe(0.5714286f, 0f), new Keyframe(0.71428573f, 1f), new Keyframe(0.85714287f, 1f), new Keyframe(1f, 0.5f) });
			UIColorPicker.mGreen = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.14285715f, 1f), new Keyframe(0.2857143f, 1f), new Keyframe(0.42857143f, 1f), new Keyframe(0.5714286f, 0f), new Keyframe(0.71428573f, 0f), new Keyframe(0.85714287f, 0f), new Keyframe(1f, 0.5f) });
			UIColorPicker.mBlue = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.14285715f, 0f), new Keyframe(0.2857143f, 0f), new Keyframe(0.42857143f, 1f), new Keyframe(0.5714286f, 1f), new Keyframe(0.71428573f, 1f), new Keyframe(0.85714287f, 0f), new Keyframe(1f, 0.5f) });
		}
		Vector3 vector3 = new Vector3(UIColorPicker.mRed.Evaluate(x), UIColorPicker.mGreen.Evaluate(x), UIColorPicker.mBlue.Evaluate(x));
		if (y >= 0.5f)
		{
			vector3 = Vector3.Lerp(vector3, Vector3.one, y * 2f - 1f);
		}
		else
		{
			y *= 2f;
			vector3.x *= y;
			vector3.y *= y;
			vector3.z *= y;
		}
		return new Color(vector3.x, vector3.y, vector3.z, 1f);
	}

	public void Select(Vector2 v)
	{
		v.x = Mathf.Clamp01(v.x);
		v.y = Mathf.Clamp01(v.y);
		this.mPos = v;
		if (this.selectionWidget != null)
		{
			Vector3[] vector3Array = this.mUITex.localCorners;
			v.x = Mathf.Lerp(vector3Array[0].x, vector3Array[2].x, this.mPos.x);
			v.y = Mathf.Lerp(vector3Array[0].y, vector3Array[2].y, this.mPos.y);
			v = this.mTrans.TransformPoint(v);
			this.selectionWidget.transform.OverlayPosition(v, this.mCam.cachedCamera);
		}
		this.@value = UIColorPicker.Sample(this.mPos.x, this.mPos.y);
		UIColorPicker.current = this;
		EventDelegate.Execute(this.onChange);
		UIColorPicker.current = null;
	}

	public Vector2 Select(Color c)
	{
		Vector3 vector3 = new Vector3();
		if (this.mUITex == null)
		{
			this.@value = c;
			return this.mPos;
		}
		float single = Single.MaxValue;
		for (int i = 0; i < this.mHeight; i++)
		{
			float single1 = ((float)i - 1f) / (float)this.mHeight;
			for (int j = 0; j < this.mWidth; j++)
			{
				float single2 = ((float)j - 1f) / (float)this.mWidth;
				Color color = UIColorPicker.Sample(single2, single1);
				color.r -= c.r;
				color.g -= c.g;
				color.b -= c.b;
				float single3 = color.r * color.r + color.g * color.g + color.b * color.b;
				if (single3 < single)
				{
					single = single3;
					this.mPos.x = single2;
					this.mPos.y = single1;
				}
			}
		}
		if (this.selectionWidget != null)
		{
			Vector3[] vector3Array = this.mUITex.localCorners;
			vector3.x = Mathf.Lerp(vector3Array[0].x, vector3Array[2].x, this.mPos.x);
			vector3.y = Mathf.Lerp(vector3Array[0].y, vector3Array[2].y, this.mPos.y);
			vector3.z = 0f;
			vector3 = this.mTrans.TransformPoint(vector3);
			this.selectionWidget.transform.OverlayPosition(vector3, this.mCam.cachedCamera);
		}
		this.@value = c;
		UIColorPicker.current = this;
		EventDelegate.Execute(this.onChange);
		UIColorPicker.current = null;
		return this.mPos;
	}

	private void Start()
	{
		this.mTrans = base.transform;
		this.mUITex = base.GetComponent<UITexture>();
		this.mCam = UICamera.FindCameraForLayer(base.gameObject.layer);
		this.mWidth = this.mUITex.width;
		this.mHeight = this.mUITex.height;
		Color[] colorArray = new Color[this.mWidth * this.mHeight];
		for (int i = 0; i < this.mHeight; i++)
		{
			float single = ((float)i - 1f) / (float)this.mHeight;
			for (int j = 0; j < this.mWidth; j++)
			{
				float single1 = ((float)j - 1f) / (float)this.mWidth;
				int num = j + i * this.mWidth;
				colorArray[num] = UIColorPicker.Sample(single1, single);
			}
		}
		this.mTex = new Texture2D(this.mWidth, this.mHeight, TextureFormat.RGB24, false);
		this.mTex.SetPixels(colorArray);
		this.mTex.filterMode = FilterMode.Trilinear;
		this.mTex.wrapMode = TextureWrapMode.Clamp;
		this.mTex.Apply();
		this.mUITex.mainTexture = this.mTex;
		this.Select(this.@value);
	}
}