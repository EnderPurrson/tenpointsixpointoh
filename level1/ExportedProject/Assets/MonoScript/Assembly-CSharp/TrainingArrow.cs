using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

internal sealed class TrainingArrow : MonoBehaviour
{
	public Vector2 arrowDelta = Vector2.zero;

	private Vector2 initialPosition;

	private RectTransform rectTransform;

	private Tweener tweener;

	private void Init()
	{
		if (rectTransform == null)
		{
			rectTransform = GetComponent<RectTransform>();
			initialPosition = rectTransform.anchoredPosition;
		}
	}

	public void SetAnchoredPosition(Vector3 position)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		Init();
		if (rectTransform != null)
		{
			rectTransform.anchoredPosition = position;
			initialPosition = rectTransform.anchoredPosition;
			if (tweener != null)
			{
				((ABSTweenComponent)tweener).Kill();
			}
			tweener = HOTween.To((object)rectTransform, 0.5f, new TweenParms().Prop("anchoredPosition", (object)arrowDelta, true).Loops(-1, (LoopType)2));
		}
	}

	private void Awake()
	{
		Init();
	}

	private void OnEnable()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		rectTransform.anchoredPosition = initialPosition;
		if (tweener != null)
		{
			((ABSTweenComponent)tweener).Kill();
		}
		tweener = HOTween.To((object)rectTransform, 0.5f, new TweenParms().Prop("anchoredPosition", (object)arrowDelta, true).Loops(-1, (LoopType)2));
	}

	private void OnDisable()
	{
		if (tweener != null)
		{
			((ABSTweenComponent)tweener).Kill();
			tweener = null;
		}
	}
}
