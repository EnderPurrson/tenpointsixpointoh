using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

internal sealed class TrainingArrow : MonoBehaviour
{
	public Vector2 arrowDelta = Vector2.zero;

	private Vector2 initialPosition;

	private RectTransform rectTransform;

	private Tweener tweener;

	public TrainingArrow()
	{
	}

	private void Awake()
	{
		this.Init();
	}

	private void Init()
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.initialPosition = this.rectTransform.anchoredPosition;
		}
	}

	private void OnDisable()
	{
		if (this.tweener != null)
		{
			this.tweener.Kill();
			this.tweener = null;
		}
	}

	private void OnEnable()
	{
		this.rectTransform.anchoredPosition = this.initialPosition;
		if (this.tweener != null)
		{
			this.tweener.Kill();
		}
		this.tweener = HOTween.To(this.rectTransform, 0.5f, (new TweenParms()).Prop("anchoredPosition", this.arrowDelta, true).Loops(-1, LoopType.YoyoInverse));
	}

	public void SetAnchoredPosition(Vector3 position)
	{
		this.Init();
		if (this.rectTransform != null)
		{
			this.rectTransform.anchoredPosition = position;
			this.initialPosition = this.rectTransform.anchoredPosition;
			if (this.tweener != null)
			{
				this.tweener.Kill();
			}
			this.tweener = HOTween.To(this.rectTransform, 0.5f, (new TweenParms()).Prop("anchoredPosition", this.arrowDelta, true).Loops(-1, LoopType.YoyoInverse));
		}
	}
}