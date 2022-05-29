using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

internal sealed class TrainingFingerSwipe : MonoBehaviour
{
	public Vector2 arrowDdelta = new Vector3(300f, 0f);

	private Vector3 initialAnchoredPosition;

	private RectTransform rectTransform;

	private Tweener tweener;

	public TrainingFingerSwipe()
	{
	}

	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.initialAnchoredPosition = this.rectTransform.anchoredPosition;
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
		this.rectTransform.anchoredPosition = this.initialAnchoredPosition;
		if (this.tweener != null)
		{
			this.tweener.Kill();
		}
		this.tweener = HOTween.To(this.rectTransform, 1f, (new TweenParms()).Prop("anchoredPosition", this.arrowDdelta, true).Ease(EaseType.EaseInQuad).Loops(-1, LoopType.Restart));
	}

	private void Update()
	{
		int num = this.tweener.completedLoops;
	}
}