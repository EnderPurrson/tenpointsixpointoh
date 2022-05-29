using System;
using UnityEngine;

public class MainMenuPersClickHandler : MonoBehaviour
{
	public float DragDistance = 20f;

	private Vector3 _startPos;

	public MainMenuPersClickHandler()
	{
	}

	private void OnMouseDown()
	{
		this._startPos = Input.mousePosition;
	}

	private void OnMouseUp()
	{
		Vector3 vector3 = Input.mousePosition;
		if (Mathf.Abs(this._startPos.magnitude - vector3.magnitude) > this.DragDistance)
		{
			return;
		}
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.mainPanel != null && !MainMenuController.sharedController.mainPanel.activeInHierarchy)
		{
			return;
		}
		if (UICamera.lastHit.collider != null)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted)
		{
			return;
		}
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SetStaticticTab(StatisticHUD.TypeOpenTab.multiplayer);
		}
		MainMenuController.sharedController.GoToProfile();
	}
}