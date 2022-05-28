using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

internal sealed class TrainingFingerSwipe : MonoBehaviour
{
	public Vector2 arrowDdelta = new Vector3(300f, 0f);

	private Vector3 initialAnchoredPosition;

	private RectTransform rectTransform;

	private Tweener tweener;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		initialAnchoredPosition = rectTransform.anchoredPosition;
	}

	private void OnEnable()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		rectTransform.anchoredPosition = initialAnchoredPosition;
		if (tweener != null)
		{
			((ABSTweenComponent)tweener).Kill();
		}
		tweener = HOTween.To((object)rectTransform, 1f, new TweenParms().Prop("anchoredPosition", (object)arrowDdelta, true).Ease((EaseType)4).Loops(-1, (LoopType)0));
	}

	private void Update()
	{
		int completedLoops = ((ABSTweenComponent)tweener).get_completedLoops();
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
