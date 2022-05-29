using System;
using UnityEngine;

public sealed class TextureSetter : MonoBehaviour
{
	public string TextureName;

	public TextureSetter()
	{
	}

	private void Awake()
	{
		SkipPresser.SkipPressed += new Action(this.SetTexture);
		SkipTrainingButton.SkipTrClosed += new Action(this.UnsetTexture);
	}

	private void OnDestroy()
	{
		SkipPresser.SkipPressed -= new Action(this.SetTexture);
		SkipTrainingButton.SkipTrClosed -= new Action(this.UnsetTexture);
	}

	private void SetTexture()
	{
		if (string.IsNullOrEmpty(this.TextureName))
		{
			return;
		}
		string str = ResPath.Combine("SkipTraining", this.TextureName);
		base.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(str);
	}

	private void UnsetTexture()
	{
		base.GetComponent<UITexture>().mainTexture = null;
	}
}