using System;
using UnityEngine;

[ExecuteInEditMode]
public class Label3d : MonoBehaviour
{
	public bool apply;

	public Color shadedColor = Color.gray;

	public float offset = -3f;

	public Label3d()
	{
	}

	private void Create3dText()
	{
		GameObject component = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		UnityEngine.Object.DestroyImmediate(component.GetComponent<Label3d>());
		component.GetComponent<UILabel>().depth = component.GetComponent<UILabel>().depth - 2;
		GameObject vector3 = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		UnityEngine.Object.DestroyImmediate(vector3.GetComponent<Label3d>());
		vector3.transform.parent = base.transform;
		vector3.GetComponent<UILabel>().depth = vector3.GetComponent<UILabel>().depth - 1;
		vector3.transform.localScale = new Vector3(1f, 1f, 1f);
		vector3.transform.localPosition = new Vector3(0f, this.offset, 0f);
		vector3.GetComponent<UILabel>().color = this.shadedColor;
		component.transform.parent = base.transform;
		component.transform.localScale = new Vector3(1f, 1f, 1f);
		component.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.gameObject.GetComponent<UILabel>().effectStyle = UILabel.Effect.None;
		base.gameObject.SetActive(false);
		this.DeleteScript();
	}

	private void DeleteScript()
	{
		base.gameObject.SetActive(true);
		UnityEngine.Object.DestroyImmediate(base.gameObject.GetComponent<Label3d>());
	}

	private void Update()
	{
		if (!this.apply)
		{
			return;
		}
		this.apply = false;
		this.Create3dText();
	}
}