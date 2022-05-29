using System;
using System.Reflection;
using UnityEngine;

internal sealed class AddCamFX : MonoBehaviour
{
	public AddCamFX()
	{
	}

	private Component CopyComponent(Component original, GameObject destination)
	{
		Type type = original.GetType();
		Component component = destination.AddComponent(type);
		FieldInfo[] fields = type.GetFields();
		for (int i = 0; i < (int)fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
			fieldInfo.SetValue(component, fieldInfo.GetValue(original));
		}
		return component;
	}

	private void Start()
	{
	}
}