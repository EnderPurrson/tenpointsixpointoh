using AnimationOrTween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation")]
public class ActiveAnimation : MonoBehaviour
{
	public static ActiveAnimation current;

	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[HideInInspector]
	public GameObject eventReceiver;

	[HideInInspector]
	public string callWhenFinished;

	private Animation mAnim;

	private Direction mLastDirection;

	private Direction mDisableDirection;

	private bool mNotify;

	private Animator mAnimator;

	private string mClip = string.Empty;

	public bool isPlaying
	{
		get
		{
			bool flag;
			if (this.mAnim == null)
			{
				if (this.mAnimator == null)
				{
					return false;
				}
				if (this.mLastDirection == Direction.Reverse)
				{
					if (this.playbackTime == 0f)
					{
						return false;
					}
				}
				else if (this.playbackTime == 1f)
				{
					return false;
				}
				return true;
			}
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (this.mAnim.IsPlaying(current.name))
					{
						if (this.mLastDirection == Direction.Forward)
						{
							if (current.time < current.length)
							{
								flag = true;
								return flag;
							}
						}
						else if (this.mLastDirection != Direction.Reverse)
						{
							flag = true;
							return flag;
						}
						else if (current.time > 0f)
						{
							flag = true;
							return flag;
						}
					}
				}
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			return flag;
		}
	}

	private float playbackTime
	{
		get
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.mAnimator.GetCurrentAnimatorStateInfo(0);
			return Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime);
		}
	}

	public ActiveAnimation()
	{
	}

	public void Finish()
	{
		if (this.mAnim != null)
		{
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (this.mLastDirection != Direction.Forward)
					{
						if (this.mLastDirection != Direction.Reverse)
						{
							continue;
						}
						current.time = 0f;
					}
					else
					{
						current.time = current.length;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			this.mAnim.Sample();
		}
		else if (this.mAnimator != null)
		{
			this.mAnimator.Play(this.mClip, 0, (this.mLastDirection != Direction.Forward ? 0f : 1f));
		}
	}

	private void Play(string clipName, Direction playDirection)
	{
		if (playDirection == Direction.Toggle)
		{
			playDirection = (this.mLastDirection == Direction.Forward ? Direction.Reverse : Direction.Forward);
		}
		if (this.mAnim != null)
		{
			base.enabled = true;
			this.mAnim.enabled = false;
			if (string.IsNullOrEmpty(clipName))
			{
				if (!this.mAnim.isPlaying)
				{
					this.mAnim.Play();
				}
			}
			else if (!this.mAnim.IsPlaying(clipName))
			{
				this.mAnim.Play(clipName);
			}
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (!string.IsNullOrEmpty(clipName) && !(current.name == clipName))
					{
						continue;
					}
					float single = Mathf.Abs(current.speed);
					current.speed = single * (float)playDirection;
					if (playDirection != Direction.Reverse || current.time != 0f)
					{
						if (playDirection != Direction.Forward || current.time != current.length)
						{
							continue;
						}
						current.time = 0f;
					}
					else
					{
						current.time = current.length;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			this.mLastDirection = playDirection;
			this.mNotify = true;
			this.mAnim.Sample();
		}
		else if (this.mAnimator != null)
		{
			if (base.enabled && this.isPlaying && this.mClip == clipName)
			{
				this.mLastDirection = playDirection;
				return;
			}
			base.enabled = true;
			this.mNotify = true;
			this.mLastDirection = playDirection;
			this.mClip = clipName;
			this.mAnimator.Play(this.mClip, 0, (playDirection != Direction.Forward ? 1f : 0f));
		}
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int num = 0;
			int length = (int)componentsInChildren.Length;
			while (num < length)
			{
				componentsInChildren[num].Refresh();
				num++;
			}
		}
		ActiveAnimation component = anim.GetComponent<ActiveAnimation>();
		if (component == null)
		{
			component = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		component.mAnim = anim;
		component.mDisableDirection = (Direction)disableCondition;
		component.onFinished.Clear();
		component.Play(clipName, playDirection);
		if (component.mAnim != null)
		{
			component.mAnim.Sample();
		}
		else if (component.mAnimator != null)
		{
			component.mAnimator.Update(0f);
		}
		return component;
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animator anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (enableBeforePlay != EnableCondition.IgnoreDisabledState && !NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int num = 0;
			int length = (int)componentsInChildren.Length;
			while (num < length)
			{
				componentsInChildren[num].Refresh();
				num++;
			}
		}
		ActiveAnimation component = anim.GetComponent<ActiveAnimation>();
		if (component == null)
		{
			component = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		component.mAnimator = anim;
		component.mDisableDirection = (Direction)disableCondition;
		component.onFinished.Clear();
		component.Play(clipName, playDirection);
		if (component.mAnim != null)
		{
			component.mAnim.Sample();
		}
		else if (component.mAnimator != null)
		{
			component.mAnimator.Update(0f);
		}
		return component;
	}

	public void Reset()
	{
		if (this.mAnim != null)
		{
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (this.mLastDirection != Direction.Reverse)
					{
						if (this.mLastDirection != Direction.Forward)
						{
							continue;
						}
						current.time = 0f;
					}
					else
					{
						current.time = current.length;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
		}
		else if (this.mAnimator != null)
		{
			this.mAnimator.Play(this.mClip, 0, (this.mLastDirection != Direction.Reverse ? 0f : 1f));
		}
	}

	private void Start()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	private void Update()
	{
		float single = RealTime.deltaTime;
		if (single == 0f)
		{
			return;
		}
		if (this.mAnimator == null)
		{
			if (this.mAnim == null)
			{
				base.enabled = false;
				return;
			}
			bool flag = false;
			IEnumerator enumerator = this.mAnim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AnimationState current = (AnimationState)enumerator.Current;
					if (this.mAnim.IsPlaying(current.name))
					{
						float single1 = current.speed * single;
						AnimationState animationState = current;
						animationState.time = animationState.time + single1;
						if (single1 < 0f)
						{
							if (current.time <= 0f)
							{
								current.time = 0f;
							}
							else
							{
								flag = true;
							}
						}
						else if (current.time >= current.length)
						{
							current.time = current.length;
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable == null)
				{
				}
				disposable.Dispose();
			}
			this.mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
		}
		else
		{
			this.mAnimator.Update((this.mLastDirection != Direction.Reverse ? single : -single));
			if (this.isPlaying)
			{
				return;
			}
			this.mAnimator.enabled = false;
			base.enabled = false;
		}
		if (this.mNotify)
		{
			this.mNotify = false;
			if (ActiveAnimation.current == null)
			{
				ActiveAnimation.current = this;
				EventDelegate.Execute(this.onFinished);
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, SendMessageOptions.DontRequireReceiver);
				}
				ActiveAnimation.current = null;
			}
			if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
			{
				NGUITools.SetActive(base.gameObject, false);
			}
		}
	}
}