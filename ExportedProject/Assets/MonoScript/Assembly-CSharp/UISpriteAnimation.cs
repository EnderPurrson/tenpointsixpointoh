using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Sprite Animation")]
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
public class UISpriteAnimation : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	protected int mFPS = 30;

	[HideInInspector]
	[SerializeField]
	protected string mPrefix = string.Empty;

	[HideInInspector]
	[SerializeField]
	protected bool mLoop = true;

	[HideInInspector]
	[SerializeField]
	protected bool mSnap = true;

	protected UISprite mSprite;

	protected float mDelta;

	protected int mIndex;

	protected bool mActive = true;

	protected List<string> mSpriteNames = new List<string>();

	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	public UISpriteAnimation()
	{
	}

	public void Pause()
	{
		this.mActive = false;
	}

	public void Play()
	{
		this.mActive = true;
	}

	public void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> uISpriteDatas = this.mSprite.atlas.spriteList;
			int num = 0;
			int count = uISpriteDatas.Count;
			while (num < count)
			{
				UISpriteData item = uISpriteDatas[num];
				if (string.IsNullOrEmpty(this.mPrefix) || item.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(item.name);
				}
				num++;
			}
			this.mSpriteNames.Sort();
		}
	}

	public void ResetToBeginning()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	protected virtual void Start()
	{
		this.RebuildSpriteList();
	}

	protected virtual void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && this.mFPS > 0)
		{
			this.mDelta += RealTime.deltaTime;
			float single = 1f / (float)this.mFPS;
			if (single < this.mDelta)
			{
				this.mDelta = (single <= 0f ? 0f : this.mDelta - single);
				UISpriteAnimation uISpriteAnimation = this;
				int num = uISpriteAnimation.mIndex + 1;
				int num1 = num;
				uISpriteAnimation.mIndex = num;
				if (num1 >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.mLoop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					if (this.mSnap)
					{
						this.mSprite.MakePixelPerfect();
					}
				}
			}
		}
	}
}