using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkipTrainingButton : MonoBehaviour
{
	public SkipTrainingButton()
	{
	}

	protected virtual void OnClick()
	{
		if (SkipTrainingButton.SkipTrClosed != null)
		{
			SkipTrainingButton.SkipTrClosed();
		}
		Resources.UnloadUnusedAssets();
	}

	public static event Action SkipTrClosed;
}