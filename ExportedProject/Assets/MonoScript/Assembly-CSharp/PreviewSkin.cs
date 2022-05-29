using System;
using UnityEngine;

public class PreviewSkin : MonoBehaviour
{
	public Camera previewCamera;

	private Vector2 touchPosition;

	private bool isTapDown;

	private GameObject selectedGameObject;

	private float sideMargin = 100f;

	private float topBottMargins = 120f;

	private Rect swipeZone;

	private Vector3 rememberedScale;

	private Vector3 rememberedBodyOffs;

	public PreviewSkin()
	{
	}

	public GameObject GameObjectOnTouch(Vector2 touchPosition)
	{
		RaycastHit raycastHit;
		if (!Physics.Raycast(this.previewCamera.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0f)), out raycastHit))
		{
			return null;
		}
		return raycastHit.collider.gameObject;
	}

	public void Highlight(GameObject go)
	{
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		Color color = component.materials[0].color;
		component.materials[0].color = new Color(color.r, color.g, color.b, 0.6f);
	}

	private void OnDisable()
	{
		this.isTapDown = false;
		this.selectedGameObject = null;
	}

	private void OnEnable()
	{
		this.isTapDown = false;
		this.selectedGameObject = null;
	}

	private void Start()
	{
		this.swipeZone = new Rect(this.sideMargin, this.topBottMargins, (float)Screen.width - this.sideMargin * 2f, (float)Screen.height - this.topBottMargins * 2f);
	}

	public void Unhighlight(GameObject go)
	{
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		Color color = component.materials[0].color;
		component.materials[0].color = new Color(color.r, color.g, color.b, 1f);
	}

	private void Update()
	{
		float single;
		if (!this.isTapDown && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			this.touchPosition = (Input.touchCount <= 0 ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : Input.GetTouch(0).position);
			if (!this.swipeZone.Contains(this.touchPosition))
			{
				return;
			}
			this.isTapDown = true;
			this.selectedGameObject = this.GameObjectOnTouch(this.touchPosition);
			if (this.selectedGameObject != null)
			{
				this.Highlight(this.selectedGameObject);
			}
			return;
		}
		if (this.isTapDown && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			if (Input.touchCount <= 0)
			{
				single = this.touchPosition.x - Input.mousePosition.x;
			}
			else
			{
				float single1 = this.touchPosition.x;
				Vector2 touch = Input.GetTouch(0).position;
				single = single1 - touch.x;
			}
			float single2 = single;
			if (!(this.selectedGameObject != null) || Mathf.Abs(single2) <= 2f)
			{
				float single3 = 0.5f;
				base.transform.Rotate(0f, single3 * single2, 0f, Space.Self);
				this.touchPosition = (Input.touchCount <= 0 ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : Input.GetTouch(0).position);
			}
			else
			{
				this.Unhighlight(this.selectedGameObject);
				this.selectedGameObject = null;
			}
		}
		if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
		{
			if (this.selectedGameObject != null)
			{
				ButtonClickSound.Instance.PlayClick();
				this.Unhighlight(this.selectedGameObject);
				if (SkinEditorController.sharedController != null)
				{
					SkinEditorController.sharedController.SelectPart(this.selectedGameObject.name);
				}
				this.selectedGameObject = null;
			}
			this.isTapDown = false;
		}
	}
}