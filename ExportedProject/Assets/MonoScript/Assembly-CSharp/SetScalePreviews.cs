using System;
using UnityEngine;

public class SetScalePreviews : MonoBehaviour
{
	public UIPanel myScrollPanel;

	private float widthCell;

	public SetScalePreviews()
	{
	}

	public void LateUpdate()
	{
		MapPreviewController[] componentsInChildren = base.transform.GetComponentsInChildren<MapPreviewController>();
		if (componentsInChildren != null)
		{
			float single = this.myScrollPanel.clipOffset.x;
			for (int i = 0; i < (int)componentsInChildren.Length; i++)
			{
				Vector3 vector3 = componentsInChildren[i].transform.localPosition;
				float single1 = 1f - Mathf.Abs(vector3.x - single) / this.widthCell * 0.1f;
				if (single1 <= 0f)
				{
					single1 = 0.1f;
				}
				componentsInChildren[i].transform.localScale = new Vector3(single1, single1, single1);
			}
		}
	}

	private void Start()
	{
		this.widthCell = ConnectSceneNGUIController.sharedController.widthCell;
	}
}