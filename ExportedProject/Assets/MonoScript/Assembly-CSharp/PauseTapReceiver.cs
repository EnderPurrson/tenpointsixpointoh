using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PauseTapReceiver : MonoBehaviour
{
	public PauseTapReceiver()
	{
	}

	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (SceneLoader.ActiveSceneName.Equals("Training") && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			return;
		}
		if (PauseTapReceiver.PauseClicked != null)
		{
			PauseTapReceiver.PauseClicked();
		}
	}

	public static event Action PauseClicked;
}