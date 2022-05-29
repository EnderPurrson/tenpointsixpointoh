using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
	public GameObject[] objects;

	public Text text;

	private int m_CurrentActiveObject;

	public CameraSwitch()
	{
	}

	public void NextCamera()
	{
		int num = (this.m_CurrentActiveObject + 1 < (int)this.objects.Length ? this.m_CurrentActiveObject + 1 : 0);
		for (int i = 0; i < (int)this.objects.Length; i++)
		{
			this.objects[i].SetActive(i == num);
		}
		this.m_CurrentActiveObject = num;
		this.text.text = this.objects[this.m_CurrentActiveObject].name;
	}

	private void OnEnable()
	{
		this.text.text = this.objects[this.m_CurrentActiveObject].name;
	}
}