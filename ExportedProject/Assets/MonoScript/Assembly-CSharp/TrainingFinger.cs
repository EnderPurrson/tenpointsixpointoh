using Holoville.HOTween;
using System;
using UnityEngine;

internal sealed class TrainingFinger : MonoBehaviour
{
	public float AngleX;

	public float AngleY;

	private Vector3 initialPosition;

	private RectTransform rectTransform;

	public TrainingFinger()
	{
	}

	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.initialPosition = this.rectTransform.localPosition;
	}

	private void OnEnable()
	{
		this.rectTransform.localPosition = this.initialPosition;
		this.AngleX = 0f;
		HOTween.To(this, 4f, (new TweenParms()).Prop("AngleX", 6.2831855f, true).Ease(EaseType.Linear).Loops(-1, LoopType.Restart));
		this.AngleY = 0f;
		HOTween.To(this, 2f, (new TweenParms()).Prop("AngleY", 6.2831855f, true).Ease(EaseType.Linear).Loops(-1, LoopType.Restart));
	}

	private void Update()
	{
		Vector3 vector3 = new Vector3(60f * Mathf.Sin(this.AngleX), 30f * Mathf.Sin(this.AngleY), 0f);
		base.transform.localPosition = this.initialPosition + vector3;
	}
}