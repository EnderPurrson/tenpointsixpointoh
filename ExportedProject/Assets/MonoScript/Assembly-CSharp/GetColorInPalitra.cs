using System;
using UnityEngine;

public class GetColorInPalitra : MonoBehaviour
{
	public UITexture canvasTexture;

	private bool isMouseDown;

	public UISprite newColor;

	public UIButton okColorInPalitraButton;

	public GetColorInPalitra()
	{
	}

	private Vector2 GetEditPixelPos(Vector2 pos)
	{
		float single = (float)Screen.height / 768f;
		Vector3 vector3 = this.canvasTexture.transform.localPosition;
		Vector3 vector31 = this.canvasTexture.transform.localPosition;
		Vector2 vector2 = pos - new Vector2(((float)Screen.width - (float)this.canvasTexture.width * single) * 0.5f + vector3.x * single, ((float)Screen.height - (float)this.canvasTexture.height * single) * 0.5f + vector31.y * single);
		return new Vector2((float)Mathf.FloorToInt(vector2.x / ((float)this.canvasTexture.width * single) * (float)this.canvasTexture.mainTexture.width), (float)Mathf.FloorToInt(vector2.y / ((float)this.canvasTexture.height * single) * (float)this.canvasTexture.mainTexture.height));
	}

	private bool IsCanvasConteinPosition(Vector2 pos)
	{
		float single = (float)Screen.height / 768f;
		Vector3 vector3 = this.canvasTexture.transform.localPosition;
		Vector3 vector31 = this.canvasTexture.transform.localPosition;
		Vector2 vector2 = new Vector2(((float)Screen.width - (float)this.canvasTexture.width * single) * 0.5f + vector3.x * single, ((float)Screen.height - (float)this.canvasTexture.height * single) * 0.5f + vector31.y * single);
		Rect rect = new Rect(vector2.x, vector2.y, (float)this.canvasTexture.width * single, (float)this.canvasTexture.height * single);
		return rect.Contains(pos);
	}

	private void Start()
	{
	}

	private void Update()
	{
		Vector2 vector2;
		if (Input.GetMouseButtonUp(0))
		{
			this.isMouseDown = false;
		}
		if (Input.touchCount > 0 && Input.touches[0].phase != TouchPhase.Ended && Input.touches[0].phase != TouchPhase.Canceled || this.isMouseDown || Input.GetMouseButtonDown(0))
		{
			if (Input.touchCount <= 0)
			{
				vector2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			}
			else
			{
				float single = Input.touches[0].position.x;
				Vector2 vector21 = Input.touches[0].position;
				vector2 = new Vector2(single, vector21.y);
			}
			Vector2 vector22 = vector2;
			if (this.IsCanvasConteinPosition(vector22))
			{
				if (Input.GetMouseButtonDown(0))
				{
					this.isMouseDown = true;
				}
				Vector2 editPixelPos = this.GetEditPixelPos(vector22);
				Color pixel = ((Texture2D)this.canvasTexture.mainTexture).GetPixel(Mathf.RoundToInt(editPixelPos.x), Mathf.RoundToInt(editPixelPos.y));
				this.newColor.color = pixel;
				this.okColorInPalitraButton.defaultColor = pixel;
				this.okColorInPalitraButton.pressed = pixel;
				this.okColorInPalitraButton.hover = pixel;
			}
		}
	}
}