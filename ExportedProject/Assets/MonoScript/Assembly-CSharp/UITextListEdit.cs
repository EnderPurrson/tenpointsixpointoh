using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextListEdit : MonoBehaviour
{
	public UITextListEdit.Style style;

	public UILabel textLabel;

	public float maxWidth;

	public float maxHeight;

	public int maxEntries = 50;

	public bool supportScrollWheel = true;

	protected char[] mSeparator = new char[] { '\n' };

	protected List<UITextListEdit.Paragraph> mParagraphs = new List<UITextListEdit.Paragraph>();

	protected float mScroll;

	protected bool mSelected;

	protected int mTotalLines;

	public UITextListEdit()
	{
	}

	public void Add(string text)
	{
		this.Add(text, true);
	}

	protected void Add(string text, bool updateVisible)
	{
		UITextListEdit.Paragraph item = null;
		if (this.mParagraphs.Count >= this.maxEntries)
		{
			item = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		else
		{
			item = new UITextListEdit.Paragraph();
		}
		item.text = text;
		this.mParagraphs.Add(item);
		if (this.textLabel != null && this.textLabel.font != null)
		{
			this.mTotalLines = 0;
			int num = 0;
			int count = this.mParagraphs.Count;
			while (num < count)
			{
				this.mTotalLines += (int)this.mParagraphs[num].lines.Length;
				num++;
			}
		}
		if (updateVisible)
		{
			this.UpdateVisibleText();
		}
	}

	private void Awake()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.textLabel != null)
		{
			this.textLabel.lineWidth = 0;
		}
		Collider component = base.GetComponent<Collider>();
		if (component != null)
		{
			if (this.maxHeight <= 0f)
			{
				Bounds bound = component.bounds;
				this.maxHeight = bound.size.y / base.transform.lossyScale.y;
			}
			if (this.maxWidth <= 0f)
			{
				Bounds bound1 = component.bounds;
				this.maxWidth = bound1.size.x / base.transform.lossyScale.x;
			}
		}
	}

	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	public void OnScroll(float val)
	{
		if (this.mSelected && this.supportScrollWheel)
		{
			val = val * (this.style != UITextListEdit.Style.Chat ? -10f : 10f);
			this.mScroll = Mathf.Max(0f, this.mScroll + val);
			this.UpdateVisibleText();
		}
	}

	public void OnSelect(bool selected)
	{
		this.mSelected = selected;
	}

	protected void UpdateVisibleText()
	{
		int num;
		if (this.textLabel != null && this.textLabel.font != null)
		{
			int num1 = 0;
			if (this.maxHeight <= 0f)
			{
				num = 100000;
			}
			else
			{
				float single = this.maxHeight;
				Vector3 vector3 = this.textLabel.cachedTransform.localScale;
				num = Mathf.FloorToInt(single / vector3.y);
			}
			int num2 = num;
			int num3 = Mathf.RoundToInt(this.mScroll);
			if (num2 + num3 > this.mTotalLines)
			{
				num3 = Mathf.Max(0, this.mTotalLines - num2);
				this.mScroll = (float)num3;
			}
			if (this.style == UITextListEdit.Style.Chat)
			{
				num3 = Mathf.Max(0, this.mTotalLines - num2 - num3);
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int count = this.mParagraphs.Count;
			while (num4 < count)
			{
				UITextListEdit.Paragraph item = this.mParagraphs[this.mParagraphs.Count - 1 - num4];
				int num5 = 0;
				int length = (int)item.lines.Length;
				while (num5 < length)
				{
					string str = item.lines[num5];
					if (num3 <= 0)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(str);
						num1++;
						if (num1 >= num2)
						{
							break;
						}
					}
					else
					{
						num3--;
					}
					num5++;
				}
				if (num1 < num2)
				{
					num4++;
				}
				else
				{
					break;
				}
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	protected class Paragraph
	{
		public string text;

		public string[] lines;

		public Paragraph()
		{
		}
	}

	public enum Style
	{
		Text,
		Chat
	}
}