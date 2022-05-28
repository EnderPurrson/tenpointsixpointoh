using System;
using Holoville.HOTween;
using UnityEngine;

internal sealed class TrainingFinger : MonoBehaviour
{
	public float AngleX;

	public float AngleY;

	private Vector3 initialPosition;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		initialPosition = rectTransform.localPosition;
	}

	private void OnEnable()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		rectTransform.localPosition = initialPosition;
		AngleX = 0f;
		HOTween.To((object)this, 4f, new TweenParms().Prop("AngleX", (object)((float)Math.PI * 2f), true).Ease((EaseType)0).Loops(-1, (LoopType)0));
		AngleY = 0f;
		HOTween.To((object)this, 2f, new TweenParms().Prop("AngleY", (object)((float)Math.PI * 2f), true).Ease((EaseType)0).Loops(-1, (LoopType)0));
	}

	private void Update()
	{
		Vector3 vector = new Vector3(60f * Mathf.Sin(AngleX), 30f * Mathf.Sin(AngleY), 0f);
		base.transform.localPosition = initialPosition + vector;
	}
}
