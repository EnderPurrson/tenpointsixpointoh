using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UICenterOnClick : MonoBehaviour
{
	public UICenterOnClick()
	{
	}

	private void OnClick()
	{
		UICenterOnChild uICenterOnChild = NGUITools.FindInParents<UICenterOnChild>(base.gameObject);
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		if (uICenterOnChild != null)
		{
			if (uICenterOnChild.enabled)
			{
				uICenterOnChild.CenterOn(base.transform);
			}
		}
		else if (uIPanel != null && uIPanel.clipping != UIDrawCall.Clipping.None)
		{
			UIScrollView component = uIPanel.GetComponent<UIScrollView>();
			Vector3 vector3 = -uIPanel.cachedTransform.InverseTransformPoint(base.transform.position);
			if (!component.canMoveHorizontally)
			{
				vector3.x = uIPanel.cachedTransform.localPosition.x;
			}
			if (!component.canMoveVertically)
			{
				vector3.y = uIPanel.cachedTransform.localPosition.y;
			}
			SpringPanel.Begin(uIPanel.cachedGameObject, vector3, 6f);
		}
	}
}