using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChooseBoxNGUIController : MonoBehaviour
{
	public MultipleToggleButton difficultyToggle;

	public UIButton backButton;

	public UIButton startButton;

	public GameObject grid;

	public Transform ScrollTransform;

	public GameObject SelectMapPanel;

	public int selectIndexMap;

	public int countMap;

	public float widthCell = 824f;

	public ChooseBoxNGUIController()
	{
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		ChooseBoxNGUIController.u003cStartu003ec__Iterator10 variable = null;
		return variable;
	}

	private void Update()
	{
		if (this.SelectMapPanel.activeInHierarchy)
		{
			if (this.ScrollTransform.localPosition.x <= 0f)
			{
				float scrollTransform = this.ScrollTransform.localPosition.x;
				Vector3 vector3 = this.ScrollTransform.localPosition;
				this.selectIndexMap = -1 * Mathf.RoundToInt((scrollTransform - (float)Mathf.CeilToInt(vector3.x / this.widthCell / (float)this.countMap) * this.widthCell * (float)this.countMap) / this.widthCell);
			}
			else
			{
				float single = this.ScrollTransform.localPosition.x;
				Vector3 scrollTransform1 = this.ScrollTransform.localPosition;
				this.selectIndexMap = Mathf.RoundToInt((single - (float)Mathf.FloorToInt(scrollTransform1.x / this.widthCell / (float)this.countMap) * this.widthCell * (float)this.countMap) / this.widthCell);
				this.selectIndexMap = this.countMap - this.selectIndexMap;
			}
			if (this.selectIndexMap == this.countMap)
			{
				this.selectIndexMap = 0;
			}
		}
	}
}