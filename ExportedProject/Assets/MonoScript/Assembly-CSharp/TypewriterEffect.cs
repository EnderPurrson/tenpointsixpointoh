using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
[RequireComponent(typeof(UILabel))]
public class TypewriterEffect : MonoBehaviour
{
	public static TypewriterEffect current;

	public int charsPerSecond = 20;

	public float fadeInTime;

	public float delayOnPeriod;

	public float delayOnNewLine;

	public UIScrollView scrollView;

	public bool keepFullDimensions;

	public List<EventDelegate> onFinished = new List<EventDelegate>();

	private UILabel mLabel;

	private string mFullText = string.Empty;

	private int mCurrentOffset;

	private float mNextChar;

	private bool mReset = true;

	private bool mActive;

	private BetterList<TypewriterEffect.FadeEntry> mFade = new BetterList<TypewriterEffect.FadeEntry>();

	public bool isActive
	{
		get
		{
			return this.mActive;
		}
	}

	public TypewriterEffect()
	{
	}

	public void Finish()
	{
		if (this.mActive)
		{
			this.mActive = false;
			if (!this.mReset)
			{
				this.mCurrentOffset = this.mFullText.Length;
				this.mFade.Clear();
				this.mLabel.text = this.mFullText;
			}
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
		}
	}

	private void OnDisable()
	{
		this.Finish();
	}

	private void OnEnable()
	{
		this.mReset = true;
		this.mActive = true;
	}

	public void ResetToBeginning()
	{
		this.Finish();
		this.mReset = true;
		this.mActive = true;
		this.mNextChar = 0f;
		this.mCurrentOffset = 0;
		this.Update();
	}

	private void Update()
	{
		if (!this.mActive)
		{
			return;
		}
		if (this.mReset)
		{
			this.mCurrentOffset = 0;
			this.mReset = false;
			this.mLabel = base.GetComponent<UILabel>();
			this.mFullText = this.mLabel.processedText;
			this.mFade.Clear();
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
		}
		if (string.IsNullOrEmpty(this.mFullText))
		{
			return;
		}
		while (this.mCurrentOffset < this.mFullText.Length && this.mNextChar <= RealTime.time)
		{
			int num = this.mCurrentOffset;
			this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
			if (this.mLabel.supportEncoding)
			{
				while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
				{
				}
			}
			this.mCurrentOffset++;
			if (this.mCurrentOffset <= this.mFullText.Length)
			{
				float single = 1f / (float)this.charsPerSecond;
				char chr = (num >= this.mFullText.Length ? '\n' : this.mFullText[num]);
				if (chr == '\n')
				{
					single += this.delayOnNewLine;
				}
				else if (num + 1 == this.mFullText.Length || this.mFullText[num + 1] <= ' ')
				{
					if (chr == '.')
					{
						if (num + 2 >= this.mFullText.Length || this.mFullText[num + 1] != '.' || this.mFullText[num + 2] != '.')
						{
							single += this.delayOnPeriod;
						}
						else
						{
							single = single + this.delayOnPeriod * 3f;
							num += 2;
						}
					}
					else if (chr == '!' || chr == '?')
					{
						single += this.delayOnPeriod;
					}
				}
				if (this.mNextChar != 0f)
				{
					this.mNextChar += single;
				}
				else
				{
					this.mNextChar = RealTime.time + single;
				}
				if (this.fadeInTime == 0f)
				{
					this.mLabel.text = (!this.keepFullDimensions ? this.mFullText.Substring(0, this.mCurrentOffset) : string.Concat(this.mFullText.Substring(0, this.mCurrentOffset), "[00]", this.mFullText.Substring(this.mCurrentOffset)));
					if (this.keepFullDimensions || !(this.scrollView != null))
					{
						continue;
					}
					this.scrollView.UpdatePosition();
				}
				else
				{
					TypewriterEffect.FadeEntry fadeEntry = new TypewriterEffect.FadeEntry()
					{
						index = num,
						alpha = 0f,
						text = this.mFullText.Substring(num, this.mCurrentOffset - num)
					};
					this.mFade.Add(fadeEntry);
				}
			}
			else
			{
				break;
			}
		}
		if (this.mCurrentOffset >= this.mFullText.Length)
		{
			this.mLabel.text = this.mFullText;
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
			this.mActive = false;
		}
		else if (this.mFade.size != 0)
		{
			int num1 = 0;
			while (num1 < this.mFade.size)
			{
				TypewriterEffect.FadeEntry item = this.mFade[num1];
				item.alpha = item.alpha + RealTime.deltaTime / this.fadeInTime;
				if (item.alpha >= 1f)
				{
					this.mFade.RemoveAt(num1);
				}
				else
				{
					this.mFade[num1] = item;
					num1++;
				}
			}
			if (this.mFade.size != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.mFade.size; i++)
				{
					TypewriterEffect.FadeEntry item1 = this.mFade[i];
					if (i == 0)
					{
						stringBuilder.Append(this.mFullText.Substring(0, item1.index));
					}
					stringBuilder.Append('[');
					stringBuilder.Append(NGUIText.EncodeAlpha(item1.alpha));
					stringBuilder.Append(']');
					stringBuilder.Append(item1.text);
				}
				if (this.keepFullDimensions)
				{
					stringBuilder.Append("[00]");
					stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
				}
				this.mLabel.text = stringBuilder.ToString();
			}
			else if (!this.keepFullDimensions)
			{
				this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
			}
			else
			{
				this.mLabel.text = string.Concat(this.mFullText.Substring(0, this.mCurrentOffset), "[00]", this.mFullText.Substring(this.mCurrentOffset));
			}
		}
	}

	private struct FadeEntry
	{
		public int index;

		public string text;

		public float alpha;
	}
}