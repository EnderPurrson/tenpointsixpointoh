using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class NGUIMath
{
	public static int AdjustByDPI(float height)
	{
		float single = Screen.dpi;
		RuntimePlatform runtimePlatform = Application.platform;
		if (single == 0f)
		{
			single = (runtimePlatform == RuntimePlatform.Android || runtimePlatform == RuntimePlatform.IPhonePlayer ? 160f : 96f);
		}
		int num = Mathf.RoundToInt(height * (96f / single));
		if ((num & 1) == 1)
		{
			num++;
		}
		return num;
	}

	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top)
	{
		NGUIMath.AdjustWidget(w, left, bottom, right, top, 2, 2, 100000, 100000);
	}

	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight)
	{
		NGUIMath.AdjustWidget(w, left, bottom, right, top, minWidth, minHeight, 100000, 100000);
	}

	public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight, int maxWidth, int maxHeight)
	{
		Vector2 vector2 = w.pivotOffset;
		Transform transforms = w.cachedTransform;
		Quaternion quaternion = transforms.localRotation;
		int num = Mathf.FloorToInt(left + 0.5f);
		int num1 = Mathf.FloorToInt(bottom + 0.5f);
		int num2 = Mathf.FloorToInt(right + 0.5f);
		int num3 = Mathf.FloorToInt(top + 0.5f);
		if (vector2.x == 0.5f && (num == 0 || num2 == 0))
		{
			num = num >> 1 << 1;
			num2 = num2 >> 1 << 1;
		}
		if (vector2.y == 0.5f && (num1 == 0 || num3 == 0))
		{
			num1 = num1 >> 1 << 1;
			num3 = num3 >> 1 << 1;
		}
		Vector3 vector3 = quaternion * new Vector3((float)num, (float)num3);
		Vector3 vector31 = quaternion * new Vector3((float)num2, (float)num3);
		Vector3 vector32 = quaternion * new Vector3((float)num, (float)num1);
		Vector3 vector33 = quaternion * new Vector3((float)num2, (float)num1);
		Vector3 vector34 = quaternion * new Vector3((float)num, 0f);
		Vector3 vector35 = quaternion * new Vector3((float)num2, 0f);
		Vector3 vector36 = quaternion * new Vector3(0f, (float)num3);
		Vector3 vector37 = quaternion * new Vector3(0f, (float)num1);
		Vector3 vector38 = Vector3.zero;
		if (vector2.x == 0f && vector2.y == 1f)
		{
			vector38.x = vector3.x;
			vector38.y = vector3.y;
		}
		else if (vector2.x == 1f && vector2.y == 0f)
		{
			vector38.x = vector33.x;
			vector38.y = vector33.y;
		}
		else if (vector2.x == 0f && vector2.y == 0f)
		{
			vector38.x = vector32.x;
			vector38.y = vector32.y;
		}
		else if (vector2.x == 1f && vector2.y == 1f)
		{
			vector38.x = vector31.x;
			vector38.y = vector31.y;
		}
		else if (vector2.x == 0f && vector2.y == 0.5f)
		{
			vector38.x = vector34.x + (vector36.x + vector37.x) * 0.5f;
			vector38.y = vector34.y + (vector36.y + vector37.y) * 0.5f;
		}
		else if (vector2.x == 1f && vector2.y == 0.5f)
		{
			vector38.x = vector35.x + (vector36.x + vector37.x) * 0.5f;
			vector38.y = vector35.y + (vector36.y + vector37.y) * 0.5f;
		}
		else if (vector2.x == 0.5f && vector2.y == 1f)
		{
			vector38.x = vector36.x + (vector34.x + vector35.x) * 0.5f;
			vector38.y = vector36.y + (vector34.y + vector35.y) * 0.5f;
		}
		else if (vector2.x == 0.5f && vector2.y == 0f)
		{
			vector38.x = vector37.x + (vector34.x + vector35.x) * 0.5f;
			vector38.y = vector37.y + (vector34.y + vector35.y) * 0.5f;
		}
		else if (vector2.x == 0.5f && vector2.y == 0.5f)
		{
			vector38.x = (vector34.x + vector35.x + vector36.x + vector37.x) * 0.5f;
			vector38.y = (vector36.y + vector37.y + vector34.y + vector35.y) * 0.5f;
		}
		minWidth = Mathf.Max(minWidth, w.minWidth);
		minHeight = Mathf.Max(minHeight, w.minHeight);
		int num4 = w.width + num2 - num;
		int num5 = w.height + num3 - num1;
		Vector3 vector39 = Vector3.zero;
		int num6 = num4;
		if (num4 < minWidth)
		{
			num6 = minWidth;
		}
		else if (num4 > maxWidth)
		{
			num6 = maxWidth;
		}
		if (num4 != num6)
		{
			if (num == 0)
			{
				vector39.x += Mathf.Lerp(0f, (float)(num6 - num4), vector2.x);
			}
			else
			{
				vector39.x -= Mathf.Lerp((float)(num6 - num4), 0f, vector2.x);
			}
			num4 = num6;
		}
		int num7 = num5;
		if (num5 < minHeight)
		{
			num7 = minHeight;
		}
		else if (num5 > maxHeight)
		{
			num7 = maxHeight;
		}
		if (num5 != num7)
		{
			if (num1 == 0)
			{
				vector39.y += Mathf.Lerp(0f, (float)(num7 - num5), vector2.y);
			}
			else
			{
				vector39.y -= Mathf.Lerp((float)(num7 - num5), 0f, vector2.y);
			}
			num5 = num7;
		}
		if (vector2.x == 0.5f)
		{
			num4 = num4 >> 1 << 1;
		}
		if (vector2.y == 0.5f)
		{
			num5 = num5 >> 1 << 1;
		}
		Vector3 vector310 = (transforms.localPosition + vector38) + (quaternion * vector39);
		transforms.localPosition = vector310;
		w.SetDimensions(num4, num5);
		if (w.isAnchored)
		{
			transforms = transforms.parent;
			float single = vector310.x - vector2.x * (float)num4;
			float single1 = vector310.y - vector2.y * (float)num5;
			if (w.leftAnchor.target)
			{
				w.leftAnchor.SetHorizontal(transforms, single);
			}
			if (w.rightAnchor.target)
			{
				w.rightAnchor.SetHorizontal(transforms, single + (float)num4);
			}
			if (w.bottomAnchor.target)
			{
				w.bottomAnchor.SetVertical(transforms, single1);
			}
			if (w.topAnchor.target)
			{
				w.topAnchor.SetVertical(transforms, single1 + (float)num5);
			}
		}
	}

	public static Bounds CalculateAbsoluteWidgetBounds(Transform trans)
	{
		if (trans == null)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		UIWidget[] componentsInChildren = trans.GetComponentsInChildren<UIWidget>();
		if ((int)componentsInChildren.Length == 0)
		{
			return new Bounds(trans.position, Vector3.zero);
		}
		Vector3 vector3 = new Vector3(Single.MaxValue, Single.MaxValue, Single.MaxValue);
		Vector3 vector31 = new Vector3(Single.MinValue, Single.MinValue, Single.MinValue);
		int num = 0;
		int length = (int)componentsInChildren.Length;
		while (num < length)
		{
			UIWidget uIWidget = componentsInChildren[num];
			if (uIWidget.enabled)
			{
				Vector3[] vector3Array = uIWidget.worldCorners;
				for (int i = 0; i < 4; i++)
				{
					Vector3 vector32 = vector3Array[i];
					if (vector32.x > vector31.x)
					{
						vector31.x = vector32.x;
					}
					if (vector32.y > vector31.y)
					{
						vector31.y = vector32.y;
					}
					if (vector32.z > vector31.z)
					{
						vector31.z = vector32.z;
					}
					if (vector32.x < vector3.x)
					{
						vector3.x = vector32.x;
					}
					if (vector32.y < vector3.y)
					{
						vector3.y = vector32.y;
					}
					if (vector32.z < vector3.z)
					{
						vector3.z = vector32.z;
					}
				}
			}
			num++;
		}
		Bounds bound = new Bounds(vector3, Vector3.zero);
		bound.Encapsulate(vector31);
		return bound;
	}

	public static Bounds CalculateRelativeWidgetBounds(Transform trans)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans, false, true);
	}

	public static Bounds CalculateRelativeWidgetBounds(Transform trans, bool considerInactive)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(trans, trans, considerInactive, true);
	}

	public static Bounds CalculateRelativeWidgetBounds(Transform relativeTo, Transform content)
	{
		return NGUIMath.CalculateRelativeWidgetBounds(relativeTo, content, false, true);
	}

	public static Bounds CalculateRelativeWidgetBounds(Transform relativeTo, Transform content, bool considerInactive, bool considerChildren = true)
	{
		if (content != null && relativeTo != null)
		{
			bool flag = false;
			Matrix4x4 matrix4x4 = relativeTo.worldToLocalMatrix;
			Vector3 vector3 = new Vector3(Single.MaxValue, Single.MaxValue, Single.MaxValue);
			Vector3 vector31 = new Vector3(Single.MinValue, Single.MinValue, Single.MinValue);
			NGUIMath.CalculateRelativeWidgetBounds(content, considerInactive, true, ref matrix4x4, ref vector3, ref vector31, ref flag, considerChildren);
			if (flag)
			{
				Bounds bound = new Bounds(vector3, Vector3.zero);
				bound.Encapsulate(vector31);
				return bound;
			}
		}
		return new Bounds(Vector3.zero, Vector3.zero);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private static void CalculateRelativeWidgetBounds(Transform content, bool considerInactive, bool isRoot, ref Matrix4x4 toLocal, ref Vector3 vMin, ref Vector3 vMax, ref bool isSet, bool considerChildren)
	{
		UIPanel component;
		if (content == null)
		{
			return;
		}
		if (!considerInactive && !NGUITools.GetActive(content.gameObject))
		{
			return;
		}
		if (!isRoot)
		{
			component = content.GetComponent<UIPanel>();
		}
		else
		{
			component = null;
		}
		UIPanel uIPanel = component;
		if (uIPanel != null && !uIPanel.enabled)
		{
			return;
		}
		if (!(uIPanel != null) || uIPanel.clipping == UIDrawCall.Clipping.None)
		{
			UIWidget uIWidget = content.GetComponent<UIWidget>();
			if (uIWidget != null && uIWidget.enabled)
			{
				Vector3[] vector3Array = uIWidget.worldCorners;
				for (int i = 0; i < 4; i++)
				{
					Vector3 vector3 = toLocal.MultiplyPoint3x4(vector3Array[i]);
					if (vector3.x > vMax.x)
					{
						vMax.x = vector3.x;
					}
					if (vector3.y > vMax.y)
					{
						vMax.y = vector3.y;
					}
					if (vector3.z > vMax.z)
					{
						vMax.z = vector3.z;
					}
					if (vector3.x < vMin.x)
					{
						vMin.x = vector3.x;
					}
					if (vector3.y < vMin.y)
					{
						vMin.y = vector3.y;
					}
					if (vector3.z < vMin.z)
					{
						vMin.z = vector3.z;
					}
					isSet = true;
				}
				if (!considerChildren)
				{
					return;
				}
			}
			int num = 0;
			int num1 = content.childCount;
			while (num < num1)
			{
				NGUIMath.CalculateRelativeWidgetBounds(content.GetChild(num), considerInactive, false, ref toLocal, ref vMin, ref vMax, ref isSet, true);
				num++;
			}
		}
		else
		{
			Vector3[] vector3Array1 = uIPanel.worldCorners;
			for (int j = 0; j < 4; j++)
			{
				Vector3 vector31 = toLocal.MultiplyPoint3x4(vector3Array1[j]);
				if (vector31.x > vMax.x)
				{
					vMax.x = vector31.x;
				}
				if (vector31.y > vMax.y)
				{
					vMax.y = vector31.y;
				}
				if (vector31.z > vMax.z)
				{
					vMax.z = vector31.z;
				}
				if (vector31.x < vMin.x)
				{
					vMin.x = vector31.x;
				}
				if (vector31.y < vMin.y)
				{
					vMin.y = vector31.y;
				}
				if (vector31.z < vMin.z)
				{
					vMin.z = vector31.z;
				}
				isSet = true;
			}
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int ClampIndex(int val, int max)
	{
		int num;
		if (val >= 0)
		{
			num = (val >= max ? max - 1 : val);
		}
		else
		{
			num = 0;
		}
		return num;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int ColorToInt(Color c)
	{
		int num = 0;
		num = num | Mathf.RoundToInt(c.r * 255f) << 24;
		num = num | Mathf.RoundToInt(c.g * 255f) << 16;
		num = num | Mathf.RoundToInt(c.b * 255f) << 8;
		num |= Mathf.RoundToInt(c.a * 255f);
		return num;
	}

	public static Vector2 ConstrainRect(Vector2 minRect, Vector2 maxRect, Vector2 minArea, Vector2 maxArea)
	{
		Vector2 vector2 = Vector2.zero;
		float single = maxRect.x - minRect.x;
		float single1 = maxRect.y - minRect.y;
		float single2 = maxArea.x - minArea.x;
		float single3 = maxArea.y - minArea.y;
		if (single > single2)
		{
			float single4 = single - single2;
			minArea.x -= single4;
			maxArea.x += single4;
		}
		if (single1 > single3)
		{
			float single5 = single1 - single3;
			minArea.y -= single5;
			maxArea.y += single5;
		}
		if (minRect.x < minArea.x)
		{
			vector2.x = vector2.x + (minArea.x - minRect.x);
		}
		if (maxRect.x > maxArea.x)
		{
			vector2.x = vector2.x - (maxRect.x - maxArea.x);
		}
		if (minRect.y < minArea.y)
		{
			vector2.y = vector2.y + (minArea.y - minRect.y);
		}
		if (maxRect.y > maxArea.y)
		{
			vector2.y = vector2.y - (maxRect.y - maxArea.y);
		}
		return vector2;
	}

	public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
	{
		Rect num = rect;
		if (!round)
		{
			num.xMin = rect.xMin * (float)width;
			num.xMax = rect.xMax * (float)width;
			num.yMin = (1f - rect.yMax) * (float)height;
			num.yMax = (1f - rect.yMin) * (float)height;
		}
		else
		{
			num.xMin = (float)Mathf.RoundToInt(rect.xMin * (float)width);
			num.xMax = (float)Mathf.RoundToInt(rect.xMax * (float)width);
			num.yMin = (float)Mathf.RoundToInt((1f - rect.yMax) * (float)height);
			num.yMax = (float)Mathf.RoundToInt((1f - rect.yMin) * (float)height);
		}
		return num;
	}

	public static Rect ConvertToTexCoords(Rect rect, int width, int height)
	{
		Rect rect1 = rect;
		if ((float)width != 0f && (float)height != 0f)
		{
			rect1.xMin = rect.xMin / (float)width;
			rect1.xMax = rect.xMax / (float)width;
			rect1.yMin = 1f - rect.yMax / (float)height;
			rect1.yMax = 1f - rect.yMin / (float)height;
		}
		return rect1;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string DecimalToHex24(int num)
	{
		num &= 16777215;
		return num.ToString("X6");
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string DecimalToHex32(int num)
	{
		return num.ToString("X8");
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string DecimalToHex8(int num)
	{
		num &= 255;
		return num.ToString("X2");
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static char DecimalToHexChar(int num)
	{
		if (num > 15)
		{
			return 'F';
		}
		if (num < 10)
		{
			return (char)(48 + num);
		}
		return (char)(65 + num - 10);
	}

	private static float DistancePointToLineSegment(Vector2 point, Vector2 a, Vector2 b)
	{
		float single = (b - a).sqrMagnitude;
		if (single == 0f)
		{
			return (point - a).magnitude;
		}
		float single1 = Vector2.Dot(point - a, b - a) / single;
		if (single1 < 0f)
		{
			return (point - a).magnitude;
		}
		if (single1 > 1f)
		{
			return (point - b).magnitude;
		}
		Vector2 vector2 = a + (single1 * (b - a));
		return (point - vector2).magnitude;
	}

	public static float DistanceToRectangle(Vector2[] screenPoints, Vector2 mousePos)
	{
		bool flag = false;
		int num = 4;
		for (int i = 0; i < 5; i++)
		{
			Vector3 vector3 = screenPoints[NGUIMath.RepeatIndex(i, 4)];
			Vector3 vector31 = screenPoints[NGUIMath.RepeatIndex(num, 4)];
			if (vector3.y > mousePos.y != vector31.y > mousePos.y && mousePos.x < (vector31.x - vector3.x) * (mousePos.y - vector3.y) / (vector31.y - vector3.y) + vector3.x)
			{
				flag = !flag;
			}
			num = i;
		}
		if (flag)
		{
			return 0f;
		}
		float single = -1f;
		for (int j = 0; j < 4; j++)
		{
			float lineSegment = NGUIMath.DistancePointToLineSegment(mousePos, screenPoints[j], screenPoints[NGUIMath.RepeatIndex(j + 1, 4)]);
			if (lineSegment < single || single < 0f)
			{
				single = lineSegment;
			}
		}
		return single;
	}

	public static float DistanceToRectangle(Vector3[] worldPoints, Vector2 mousePos, Camera cam)
	{
		Vector2[] screenPoint = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			screenPoint[i] = cam.WorldToScreenPoint(worldPoints[i]);
		}
		return NGUIMath.DistanceToRectangle(screenPoint, mousePos);
	}

	public static UIWidget.Pivot GetPivot(Vector2 offset)
	{
		if (offset.x == 0f)
		{
			if (offset.y == 0f)
			{
				return UIWidget.Pivot.BottomLeft;
			}
			if (offset.y == 1f)
			{
				return UIWidget.Pivot.TopLeft;
			}
			return UIWidget.Pivot.Left;
		}
		if (offset.x == 1f)
		{
			if (offset.y == 0f)
			{
				return UIWidget.Pivot.BottomRight;
			}
			if (offset.y == 1f)
			{
				return UIWidget.Pivot.TopRight;
			}
			return UIWidget.Pivot.Right;
		}
		if (offset.y == 0f)
		{
			return UIWidget.Pivot.Bottom;
		}
		if (offset.y == 1f)
		{
			return UIWidget.Pivot.Top;
		}
		return UIWidget.Pivot.Center;
	}

	public static Vector2 GetPivotOffset(UIWidget.Pivot pv)
	{
		Vector2 vector2 = Vector2.zero;
		if (pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Bottom)
		{
			vector2.x = 0.5f;
		}
		else if (pv == UIWidget.Pivot.TopRight || pv == UIWidget.Pivot.Right || pv == UIWidget.Pivot.BottomRight)
		{
			vector2.x = 1f;
		}
		else
		{
			vector2.x = 0f;
		}
		if (pv == UIWidget.Pivot.Left || pv == UIWidget.Pivot.Center || pv == UIWidget.Pivot.Right)
		{
			vector2.y = 0.5f;
		}
		else if (pv == UIWidget.Pivot.TopLeft || pv == UIWidget.Pivot.Top || pv == UIWidget.Pivot.TopRight)
		{
			vector2.y = 1f;
		}
		else
		{
			vector2.y = 0f;
		}
		return vector2;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color HexToColor(uint val)
	{
		return NGUIMath.IntToColor((int)val);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int HexToDecimal(char ch)
	{
		char chr = ch;
		switch (chr)
		{
			case '0':
			{
				return 0;
			}
			case '1':
			{
				return 1;
			}
			case '2':
			{
				return 2;
			}
			case '3':
			{
				return 3;
			}
			case '4':
			{
				return 4;
			}
			case '5':
			{
				return 5;
			}
			case '6':
			{
				return 6;
			}
			case '7':
			{
				return 7;
			}
			case '8':
			{
				return 8;
			}
			case '9':
			{
				return 9;
			}
			case 'A':
			{
				return 10;
			}
			case 'B':
			{
				return 11;
			}
			case 'C':
			{
				return 12;
			}
			case 'D':
			{
				return 13;
			}
			case 'E':
			{
				return 14;
			}
			case 'F':
			{
				return 15;
			}
			default:
			{
				switch (chr)
				{
					case 'a':
					{
						return 10;
					}
					case 'b':
					{
						return 11;
					}
					case 'c':
					{
						return 12;
					}
					case 'd':
					{
						return 13;
					}
					case 'e':
					{
						return 14;
					}
					case 'f':
					{
						return 15;
					}
				}
				return 15;
			}
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static string IntToBinary(int val, int bits)
	{
		string empty = string.Empty;
		int num = bits;
		while (num > 0)
		{
			if (num == 8 || num == 16 || num == 24)
			{
				empty = string.Concat(empty, " ");
			}
			string str = empty;
			int num1 = num - 1;
			num = num1;
			empty = string.Concat(str, ((val & 1 << (num1 & 31 & 31)) == 0 ? '0' : '1'));
		}
		return empty;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static Color IntToColor(int val)
	{
		float single = 0.003921569f;
		Color color = Color.black;
		color.r = single * (float)(val >> 24 & 255);
		color.g = single * (float)(val >> 16 & 255);
		color.b = single * (float)(val >> 8 & 255);
		color.a = single * (float)(val & 255);
		return color;
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float Lerp(float from, float to, float factor)
	{
		return from * (1f - factor) + to * factor;
	}

	public static Rect MakePixelPerfect(Rect rect)
	{
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return rect;
	}

	public static Rect MakePixelPerfect(Rect rect, int width, int height)
	{
		rect = NGUIMath.ConvertToPixels(rect, width, height, true);
		rect.xMin = (float)Mathf.RoundToInt(rect.xMin);
		rect.yMin = (float)Mathf.RoundToInt(rect.yMin);
		rect.xMax = (float)Mathf.RoundToInt(rect.xMax);
		rect.yMax = (float)Mathf.RoundToInt(rect.yMax);
		return NGUIMath.ConvertToTexCoords(rect, width, height);
	}

	public static void MoveRect(UIRect rect, float x, float y)
	{
		int num = Mathf.FloorToInt(x + 0.5f);
		int num1 = Mathf.FloorToInt(y + 0.5f);
		Transform vector3 = rect.cachedTransform;
		vector3.localPosition = vector3.localPosition + new Vector3((float)num, (float)num1);
		int num2 = 0;
		if (rect.leftAnchor.target)
		{
			num2++;
			rect.leftAnchor.absolute += num;
		}
		if (rect.rightAnchor.target)
		{
			num2++;
			rect.rightAnchor.absolute += num;
		}
		if (rect.bottomAnchor.target)
		{
			num2++;
			rect.bottomAnchor.absolute += num1;
		}
		if (rect.topAnchor.target)
		{
			num2++;
			rect.topAnchor.absolute += num1;
		}
		if (num2 != 0)
		{
			rect.UpdateAnchors();
		}
	}

	public static void MoveWidget(UIRect w, float x, float y)
	{
		NGUIMath.MoveRect(w, x, y);
	}

	public static void OverlayPosition(this Transform trans, Vector3 worldPos, Camera worldCam, Camera myCam)
	{
		worldPos = worldCam.WorldToViewportPoint(worldPos);
		worldPos = myCam.ViewportToWorldPoint(worldPos);
		Transform transforms = trans.parent;
		trans.localPosition = (transforms == null ? worldPos : transforms.InverseTransformPoint(worldPos));
	}

	public static void OverlayPosition(this Transform trans, Vector3 worldPos, Camera worldCam)
	{
		Camera camera = NGUITools.FindCameraForLayer(trans.gameObject.layer);
		if (camera != null)
		{
			trans.OverlayPosition(worldPos, worldCam, camera);
		}
	}

	public static void OverlayPosition(this Transform trans, Transform target)
	{
		Camera camera = NGUITools.FindCameraForLayer(trans.gameObject.layer);
		Camera camera1 = NGUITools.FindCameraForLayer(target.gameObject.layer);
		if (camera != null && camera1 != null)
		{
			trans.OverlayPosition(target.position, camera1, camera);
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static int RepeatIndex(int val, int max)
	{
		if (max < 1)
		{
			return 0;
		}
		while (val < 0)
		{
			val += max;
		}
		while (val >= max)
		{
			val -= max;
		}
		return val;
	}

	public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight)
	{
		NGUIMath.ResizeWidget(w, pivot, x, y, 2, 2, 100000, 100000);
	}

	public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight, int maxWidth, int maxHeight)
	{
		if (pivot == UIWidget.Pivot.Center)
		{
			int num = Mathf.RoundToInt(x - (float)w.width);
			int num1 = Mathf.RoundToInt(y - (float)w.height);
			num = num - (num & 1);
			num1 = num1 - (num1 & 1);
			if ((num | num1) != 0)
			{
				num >>= 1;
				num1 >>= 1;
				NGUIMath.AdjustWidget(w, (float)(-num), (float)(-num1), (float)num, (float)num1, minWidth, minHeight);
			}
			return;
		}
		Vector3 vector3 = new Vector3(x, y);
		vector3 = Quaternion.Inverse(w.cachedTransform.localRotation) * vector3;
		switch (pivot)
		{
			case UIWidget.Pivot.TopLeft:
			{
				NGUIMath.AdjustWidget(w, vector3.x, 0f, 0f, vector3.y, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.Top:
			{
				NGUIMath.AdjustWidget(w, 0f, 0f, 0f, vector3.y, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.TopRight:
			{
				NGUIMath.AdjustWidget(w, 0f, 0f, vector3.x, vector3.y, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.Left:
			{
				NGUIMath.AdjustWidget(w, vector3.x, 0f, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.Center:
			{
				return;
			}
			case UIWidget.Pivot.Right:
			{
				NGUIMath.AdjustWidget(w, 0f, 0f, vector3.x, 0f, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.BottomLeft:
			{
				NGUIMath.AdjustWidget(w, vector3.x, vector3.y, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.Bottom:
			{
				NGUIMath.AdjustWidget(w, 0f, vector3.y, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			case UIWidget.Pivot.BottomRight:
			{
				NGUIMath.AdjustWidget(w, 0f, vector3.y, vector3.x, 0f, minWidth, minHeight, maxWidth, maxHeight);
				return;
			}
			default:
			{
				return;
			}
		}
	}

	public static float RotateTowards(float from, float to, float maxAngle)
	{
		float single = NGUIMath.WrapAngle(to - from);
		if (Mathf.Abs(single) > maxAngle)
		{
			single = maxAngle * Mathf.Sign(single);
		}
		return from + single;
	}

	public static Vector2 ScreenToParentPixels(Vector2 pos, Transform relativeTo)
	{
		int num = relativeTo.gameObject.layer;
		if (relativeTo.parent != null)
		{
			relativeTo = relativeTo.parent;
		}
		Camera camera = NGUITools.FindCameraForLayer(num);
		if (camera == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("No camera found for layer ", num));
			return pos;
		}
		Vector3 worldPoint = camera.ScreenToWorldPoint(pos);
		return (relativeTo == null ? worldPoint : relativeTo.InverseTransformPoint(worldPoint));
	}

	public static Vector2 ScreenToPixels(Vector2 pos, Transform relativeTo)
	{
		int num = relativeTo.gameObject.layer;
		Camera camera = NGUITools.FindCameraForLayer(num);
		if (camera == null)
		{
			UnityEngine.Debug.LogWarning(string.Concat("No camera found for layer ", num));
			return pos;
		}
		return relativeTo.InverseTransformPoint(camera.ScreenToWorldPoint(pos));
	}

	public static Vector3 SpringDampen(ref Vector3 velocity, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		float single = 1f - strength * 0.001f;
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		float single1 = Mathf.Pow(single, (float)num);
		Vector3 vector3 = velocity * ((single1 - 1f) / Mathf.Log(single));
		velocity *= single1;
		return vector3 * 0.06f;
	}

	public static Vector2 SpringDampen(ref Vector2 velocity, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		float single = 1f - strength * 0.001f;
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		float single1 = Mathf.Pow(single, (float)num);
		Vector2 vector2 = velocity * ((single1 - 1f) / Mathf.Log(single));
		velocity *= single1;
		return vector2 * 0.06f;
	}

	public static float SpringLerp(float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		float single = 0f;
		for (int i = 0; i < num; i++)
		{
			single = Mathf.Lerp(single, 1f, deltaTime);
		}
		return single;
	}

	public static float SpringLerp(float from, float to, float strength, float deltaTime)
	{
		if (deltaTime > 1f)
		{
			deltaTime = 1f;
		}
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		deltaTime = 0.001f * strength;
		for (int i = 0; i < num; i++)
		{
			from = Mathf.Lerp(from, to, deltaTime);
		}
		return from;
	}

	public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime)
	{
		return Vector2.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
	{
		return Vector3.Lerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
	{
		return Quaternion.Slerp(from, to, NGUIMath.SpringLerp(strength, deltaTime));
	}

	public static Vector3 WorldToLocalPoint(Vector3 worldPos, Camera worldCam, Camera uiCam, Transform relativeTo)
	{
		worldPos = worldCam.WorldToViewportPoint(worldPos);
		worldPos = uiCam.ViewportToWorldPoint(worldPos);
		if (relativeTo == null)
		{
			return worldPos;
		}
		relativeTo = relativeTo.parent;
		if (relativeTo == null)
		{
			return worldPos;
		}
		return relativeTo.InverseTransformPoint(worldPos);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float Wrap01(float val)
	{
		return val - (float)Mathf.FloorToInt(val);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public static float WrapAngle(float angle)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}
}