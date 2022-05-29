using System;
using UnityEngine;

public class InvitationsPanelSwitch : MonoBehaviour
{
	public bool left = true;

	public bool Middle;

	public GameObject leftPanel;

	public GameObject MiddlePanel;

	public GameObject rightPanel;

	public GameObject[] anotherButtons;

	public GameObject[] anotherChekmarks;

	public GameObject chekmark;

	public GameObject butt;

	public GameObject[] anotherToggles;

	public InvitationsPanelSwitch()
	{
	}

	private void OnClick()
	{
		Debug.Log("OnClick");
		if (this.left)
		{
			this.leftPanel.SetActive(true);
			this.MiddlePanel.SetActive(false);
			this.rightPanel.SetActive(false);
		}
		else if (!this.Middle)
		{
			this.leftPanel.SetActive(false);
			this.MiddlePanel.SetActive(false);
			this.rightPanel.SetActive(true);
		}
		else
		{
			this.leftPanel.SetActive(false);
			this.MiddlePanel.SetActive(true);
			this.rightPanel.SetActive(false);
		}
		base.GetComponent<UIButton>().enabled = false;
		this.butt.GetComponent<UILabel>().gameObject.SetActive(false);
		this.chekmark.SetActive(true);
		base.GetComponent<UISprite>().spriteName = "trans_btn_n";
		GameObject[] gameObjectArray = this.anotherButtons;
		for (int i = 0; i < (int)gameObjectArray.Length; i++)
		{
			gameObjectArray[i].SetActive(true);
		}
		GameObject[] gameObjectArray1 = this.anotherToggles;
		for (int j = 0; j < (int)gameObjectArray1.Length; j++)
		{
			GameObject gameObject = gameObjectArray1[j];
			gameObject.GetComponent<UIButton>().enabled = true;
			gameObject.GetComponent<UISprite>().spriteName = "trans_btn";
		}
		GameObject[] gameObjectArray2 = this.anotherChekmarks;
		for (int k = 0; k < (int)gameObjectArray2.Length; k++)
		{
			gameObjectArray2[k].SetActive(false);
		}
		ButtonClickSound.Instance.PlayClick();
	}

	private void OnPress(bool isPress)
	{
		Debug.Log(string.Concat("press ", isPress));
		if (!isPress)
		{
			base.GetComponent<UISprite>().spriteName = "trans_btn";
		}
		else
		{
			base.GetComponent<UISprite>().spriteName = "trans_btn_n";
		}
	}
}