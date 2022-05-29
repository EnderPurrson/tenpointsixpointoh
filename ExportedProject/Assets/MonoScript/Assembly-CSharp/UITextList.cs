using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	public UILabel textLabel;

	public UIProgressBar scrollBar;

	public UITextList.Style style;

	public int paragraphHistory = 100;

	protected char[] mSeparator = new char[] { '\n' };

	protected float mScroll;

	protected int mTotalLines;

	protected int mLastWidth;

	protected int mLastHeight;

	private BetterList<UITextList.Paragraph> mParagraphs;

	private static Dictionary<string, BetterList<UITextList.Paragraph>> mHistory;

	public bool isValid
	{
		get
		{
			return (this.textLabel == null ? false : this.textLabel.ambigiousFont != null);
		}
	}

	protected float lineHeight
	{
		get
		{
			return (this.textLabel == null ? 20f : (float)this.textLabel.fontSize + this.textLabel.effectiveSpacingY);
		}
	}

	protected BetterList<UITextList.Paragraph> paragraphs
	{
		get
		{
			if (this.mParagraphs == null && !UITextList.mHistory.TryGetValue(base.name, out this.mParagraphs))
			{
				this.mParagraphs = new BetterList<UITextList.Paragraph>();
				UITextList.mHistory.Add(base.name, this.mParagraphs);
			}
			return this.mParagraphs;
		}
	}

	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar == null)
				{
					this.mScroll = value;
					this.UpdateVisibleText();
				}
				else
				{
					this.scrollBar.@value = value;
				}
			}
		}
	}

	static UITextList()
	{
		UITextList.mHistory = new Dictionary<string, BetterList<UITextList.Paragraph>>();
	}

	public UITextList()
	{
	}

	public void Add(string text)
	{
		this.Add(text, true);
	}

	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph item = null;
		if (this.paragraphs.size >= this.paragraphHistory)
		{
			item = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		else
		{
			item = new UITextList.Paragraph();
		}
		item.text = text;
		this.mParagraphs.Add(item);
		this.Rebuild();
	}

	public void Clear()
	{
		this.paragraphs.Clear();
		this.UpdateVisibleText();
	}

	public void OnDrag(Vector2 delta)
	{
		int num = this.scrollHeight;
		if (num != 0)
		{
			float single = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + single / (float)num;
		}
	}

	public void OnScroll(float val)
	{
		int num = this.scrollHeight;
		if (num != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)num;
		}
	}

	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.@value;
		this.UpdateVisibleText();
	}

	protected void Rebuild()
	{
		string str;
		if (this.isValid)
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.textLabel.UpdateNGUIText();
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
			this.mTotalLines = 0;
			for (int i = 0; i < this.paragraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				NGUIText.WrapText(paragraph.text, out str, false, true, false);
				paragraph.lines = str.Split(new char[] { '\n' });
				this.mTotalLines += (int)paragraph.lines.Length;
			}
			this.mTotalLines = 0;
			int num = 0;
			int num1 = this.mParagraphs.size;
			while (num < num1)
			{
				this.mTotalLines += (int)this.mParagraphs.buffer[num].lines.Length;
				num++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uIScrollBar = this.scrollBar as UIScrollBar;
				if (uIScrollBar != null)
				{
					uIScrollBar.barSize = (this.mTotalLines != 0 ? 1f - (float)this.scrollHeight / (float)this.mTotalLines : 1f);
				}
			}
			this.UpdateVisibleText();
		}
	}

	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style != UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.TopLeft;
			this.scrollValue = 0f;
		}
		else
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
		}
	}

	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.Rebuild();
		}
	}

	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			if (this.mTotalLines == 0)
			{
				this.textLabel.text = string.Empty;
				return;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			int num1 = Mathf.Max(0, this.mTotalLines - num);
			int num2 = Mathf.RoundToInt(this.mScroll * (float)num1);
			if (num2 < 0)
			{
				num2 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			int num4 = this.paragraphs.size;
			while (num > 0 && num3 < num4)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num3];
				int num5 = 0;
				int length = (int)paragraph.lines.Length;
				while (num > 0 && num5 < length)
				{
					string str = paragraph.lines[num5];
					if (num2 <= 0)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(str);
						num--;
					}
					else
					{
						num2--;
					}
					num5++;
				}
				num3++;
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