using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

public class BlinkingColor : MonoBehaviour
{
	private Material mainMaterial;

	public bool IsActive = true;

	public string nameColor = "_MainColor";

	public float speed = 1f;

	public Color normal;

	public Color blink;

	[HideInInspector]
	public Color curColor;

	private Color cashColor;

	private bool startBlink;

	private void Start()
	{
		Renderer component = GetComponent<Renderer>();
		if ((bool)component)
		{
			mainMaterial = component.sharedMaterial;
			if ((bool)mainMaterial)
			{
				cashColor = mainMaterial.GetColor(nameColor);
			}
		}
	}

	private void OnDestroy()
	{
		ResetColor();
	}

	private void Update()
	{
		if (IsActive)
		{
			if ((bool)mainMaterial)
			{
				mainMaterial.SetColor(nameColor, curColor);
			}
			if (!startBlink)
			{
				SetColorTwo();
			}
		}
		else if (startBlink)
		{
			ResetColor();
		}
	}

	private void ResetColor()
	{
		if ((bool)mainMaterial)
		{
			mainMaterial.SetColor(nameColor, cashColor);
		}
		startBlink = false;
		HOTween.Kill((object)this);
	}

	private void SetColorOne()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		startBlink = true;
		HOTween.To((object)this, speed, new TweenParms().Prop("curColor", (object)normal).Ease((EaseType)0).OnComplete(new TweenCallback(SetColorTwo)));
	}

	private void SetColorTwo()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		startBlink = true;
		HOTween.To((object)this, speed, new TweenParms().Prop("curColor", (object)blink).Ease((EaseType)0).OnComplete(new TweenCallback(SetColorOne)));
	}
}
