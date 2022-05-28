using UnityEngine;

public class GetColorInPalitra : MonoBehaviour
{
	public UITexture canvasTexture;

	private bool isMouseDown;

	public UISprite newColor;

	public UIButton okColorInPalitraButton;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			isMouseDown = false;
		}
		if ((Input.touchCount <= 0 || Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) && !isMouseDown && !Input.GetMouseButtonDown(0))
		{
			return;
		}
		Vector2 pos = ((Input.touchCount <= 0) ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : new Vector2(Input.touches[0].position.x, Input.touches[0].position.y));
		if (IsCanvasConteinPosition(pos))
		{
			if (Input.GetMouseButtonDown(0))
			{
				isMouseDown = true;
			}
			Vector2 editPixelPos = GetEditPixelPos(pos);
			Color pixel = ((Texture2D)canvasTexture.mainTexture).GetPixel(Mathf.RoundToInt(editPixelPos.x), Mathf.RoundToInt(editPixelPos.y));
			newColor.color = pixel;
			okColorInPalitraButton.defaultColor = pixel;
			okColorInPalitraButton.pressed = pixel;
			okColorInPalitraButton.hover = pixel;
		}
	}

	private bool IsCanvasConteinPosition(Vector2 pos)
	{
		float num = (float)Screen.height / 768f;
		Vector2 vector = new Vector2(((float)Screen.width - (float)canvasTexture.width * num) * 0.5f + canvasTexture.transform.localPosition.x * num, ((float)Screen.height - (float)canvasTexture.height * num) * 0.5f + canvasTexture.transform.localPosition.y * num);
		return new Rect(vector.x, vector.y, (float)canvasTexture.width * num, (float)canvasTexture.height * num).Contains(pos);
	}

	private Vector2 GetEditPixelPos(Vector2 pos)
	{
		float num = (float)Screen.height / 768f;
		Vector2 vector = pos - new Vector2(((float)Screen.width - (float)canvasTexture.width * num) * 0.5f + canvasTexture.transform.localPosition.x * num, ((float)Screen.height - (float)canvasTexture.height * num) * 0.5f + canvasTexture.transform.localPosition.y * num);
		return new Vector2(Mathf.FloorToInt(vector.x / ((float)canvasTexture.width * num) * (float)canvasTexture.mainTexture.width), Mathf.FloorToInt(vector.y / ((float)canvasTexture.height * num) * (float)canvasTexture.mainTexture.height));
	}
}
