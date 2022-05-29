using System;
using UnityEngine;

[ExecuteInEditMode]
public class LightTuner : MonoBehaviour
{
	public Light[] lighters;

	public float @value;

	public bool apply;

	public LightTuner()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.apply)
		{
			Light[] lightArray = this.lighters;
			for (int i = 0; i < (int)lightArray.Length; i++)
			{
				Light light = lightArray[i];
				light.intensity = light.intensity * this.@value;
			}
			this.apply = false;
		}
	}
}