using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTextures : MonoBehaviour
{
	private Color colorForEraser = new Color(1f, 1f, 1f, 1f);

	public UITexture canvasTexture;

	private bool isMouseDown;

	private Vector2 oldEditPixelPos = new Vector2(-1f, -1f);

	private bool isSetNewTexture;

	public UITexture fonCanvas;

	public ButtonHandler prevHistoryButton;

	public ButtonHandler nextHistoryButton;

	private UIButton prevHistoryUIButton;

	private UIButton nextHistoryUIButton;

	public ArrayList arrHistory = new ArrayList();

	public int currentHistoryIndex;

	private bool saveToHistory;

	public GameObject saveFrame;

	private bool symmetry;

	public EditorTextures()
	{
	}

	public void AddCanvasTextureInHistory()
	{
		while (this.currentHistoryIndex < this.arrHistory.Count - 1)
		{
			this.arrHistory.RemoveAt(this.arrHistory.Count - 1);
		}
		this.arrHistory.Add(EditorTextures.CreateCopyTexture((Texture2D)this.canvasTexture.mainTexture));
		if (this.arrHistory.Count > 30)
		{
			this.arrHistory.RemoveAt(0);
		}
		this.currentHistoryIndex = this.arrHistory.Count - 1;
	}

	public static Texture2D CreateCopyTexture(Texture tekTexture)
	{
		return EditorTextures.CreateCopyTexture((Texture2D)tekTexture);
	}

	public static Texture2D CreateCopyTexture(Texture2D tekTexture)
	{
		Texture2D texture2D = new Texture2D(tekTexture.width, tekTexture.height, TextureFormat.RGBA32, false);
		texture2D.SetPixels(tekTexture.GetPixels());
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	private void EditCanvas(Vector2 pos)
	{
		if (this.saveFrame != null && this.saveFrame.activeSelf)
		{
			return;
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			if (SkinEditorController.sharedController != null)
			{
				SkinEditorController.sharedController.newColor.color = ((Texture2D)this.canvasTexture.mainTexture).GetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
				SkinEditorController.sharedController.HandleSetColorClicked(null, null);
			}
			return;
		}
		SkinEditorController.isEditingPartSkin = true;
		Texture2D texture2D = EditorTextures.CreateCopyTexture(this.canvasTexture.mainTexture as Texture2D);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pencil)
		{
			texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			if (this.symmetry)
			{
				texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			}
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Brash)
		{
			texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			if (Mathf.RoundToInt(pos.x) > 0)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x) - 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			}
			if (Mathf.RoundToInt(pos.x) < texture2D.width - 1)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x) + 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			}
			if (Mathf.RoundToInt(pos.y) > 0)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) - 1, SkinEditorController.colorForPaint);
			}
			if (Mathf.RoundToInt(pos.y) < texture2D.height - 1)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) + 1, SkinEditorController.colorForPaint);
			}
			if (this.symmetry)
			{
				texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
				if (Mathf.RoundToInt(pos.x) > 0)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x) + 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
				}
				if (Mathf.RoundToInt(pos.x) < texture2D.width - 1)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x) - 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
				}
				if (Mathf.RoundToInt(pos.y) > 0)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) - 1, SkinEditorController.colorForPaint);
				}
				if (Mathf.RoundToInt(pos.y) < texture2D.height - 1)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) + 1, SkinEditorController.colorForPaint);
				}
			}
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Eraser)
		{
			texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), this.colorForEraser);
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Fill)
		{
			int num = Mathf.RoundToInt(pos.x) + Mathf.RoundToInt(pos.y) * texture2D.width;
			Color[] pixels = texture2D.GetPixels();
			Color color = pixels[num];
			if (color != SkinEditorController.colorForPaint)
			{
				List<int> nums = new List<int>()
				{
					num
				};
				while (nums.Count > 0)
				{
					int num1 = Mathf.FloorToInt((float)nums[0] / (float)texture2D.width);
					int item = nums[0] - num1 * texture2D.width;
					pixels[nums[0]] = SkinEditorController.colorForPaint;
					nums.RemoveAt(0);
					if (item + 1 < texture2D.width && pixels[item + 1 + num1 * texture2D.width] == color && !nums.Contains(item + 1 + num1 * texture2D.width))
					{
						nums.Add(item + 1 + num1 * texture2D.width);
					}
					if (item - 1 >= 0 && pixels[item - 1 + num1 * texture2D.width] == color && !nums.Contains(item - 1 + num1 * texture2D.width))
					{
						nums.Add(item - 1 + num1 * texture2D.width);
					}
					if (num1 + 1 < texture2D.height && pixels[item + (num1 + 1) * texture2D.width] == color && !nums.Contains(item + (num1 + 1) * texture2D.width))
					{
						nums.Add(item + (num1 + 1) * texture2D.width);
					}
					if (num1 - 1 < 0 || !(pixels[item + (num1 - 1) * texture2D.width] == color) || nums.Contains(item + (num1 - 1) * texture2D.width))
					{
						continue;
					}
					nums.Add(item + (num1 - 1) * texture2D.width);
				}
				texture2D.SetPixels(pixels);
			}
		}
		texture2D.Apply();
		this.isSetNewTexture = true;
		this.canvasTexture.mainTexture = texture2D;
	}

	private Vector2 GetEditPixelPos(Vector2 pos)
	{
		float single = (float)Screen.height / 768f;
		return new Vector2((float)Mathf.FloorToInt((pos.x - ((float)Screen.width - (float)this.canvasTexture.width * single) * 0.5f) / ((float)this.canvasTexture.width * single) * (float)this.canvasTexture.mainTexture.width), (float)Mathf.FloorToInt((pos.y - ((float)Screen.height - (float)this.canvasTexture.height * single) * 0.5f) / ((float)this.canvasTexture.height * single) * (float)this.canvasTexture.mainTexture.height));
	}

	private void HandleNextHistoryButtonClicked(object sender, EventArgs e)
	{
		if (this.currentHistoryIndex < this.arrHistory.Count - 1)
		{
			this.currentHistoryIndex++;
		}
		this.UpdateTextureFromHistory();
	}

	private void HandlePrevHistoryButtonClicked(object sender, EventArgs e)
	{
		if (this.currentHistoryIndex > 0)
		{
			this.currentHistoryIndex--;
		}
		this.UpdateTextureFromHistory();
	}

	private bool IsCanvasConteinPosition(Vector2 pos)
	{
		float single = (float)Screen.height / 768f;
		Vector2 vector2 = new Vector2(((float)Screen.width - single * (float)this.canvasTexture.width) * 0.5f, ((float)Screen.height + single * (float)this.canvasTexture.height) * 0.5f);
		Vector2 vector21 = new Vector2(((float)Screen.width + single * (float)this.canvasTexture.width) * 0.5f, ((float)Screen.height - single * (float)this.canvasTexture.height) * 0.5f);
		if (pos.x > vector2.x && pos.x < vector21.x && pos.y < vector2.y && pos.y > vector21.y)
		{
			return true;
		}
		return false;
	}

	private void OnCanvasClickUp()
	{
		if (this.saveFrame != null && this.saveFrame.activeSelf)
		{
			return;
		}
		if (SkinEditorController.brashMode != SkinEditorController.BrashMode.Pipette)
		{
			return;
		}
		if (SkinEditorController.sharedController != null)
		{
			SkinEditorController.sharedController.SetColorClickedUp();
		}
	}

	public void SetStartCanvas(Texture2D _texure)
	{
		this.canvasTexture.mainTexture = EditorTextures.CreateCopyTexture(EditorTextures.CreateCopyTexture(_texure));
		float single = 400f / (float)this.canvasTexture.mainTexture.width;
		float single1 = 400f / (float)this.canvasTexture.mainTexture.height;
		int num = (single >= single1 ? Mathf.RoundToInt(single1) : Mathf.RoundToInt(single));
		this.canvasTexture.width = this.canvasTexture.mainTexture.width * num;
		this.canvasTexture.height = this.canvasTexture.mainTexture.height * num;
		this.UpdateFonCanvas();
		this.arrHistory.Clear();
		this.AddCanvasTextureInHistory();
	}

	private void Start()
	{
		if (this.prevHistoryButton != null)
		{
			this.prevHistoryButton.Clicked += new EventHandler(this.HandlePrevHistoryButtonClicked);
			this.prevHistoryUIButton = this.prevHistoryButton.gameObject.GetComponent<UIButton>();
		}
		if (this.nextHistoryButton != null)
		{
			this.nextHistoryButton.Clicked += new EventHandler(this.HandleNextHistoryButtonClicked);
			this.nextHistoryUIButton = this.nextHistoryButton.gameObject.GetComponent<UIButton>();
		}
	}

	public void ToggleSymmetry(bool isSymmetry)
	{
		this.symmetry = isSymmetry;
	}

	private void Update()
	{
		Vector2 vector2;
		Vector2 vector21;
		if (this.prevHistoryUIButton != null && this.nextHistoryUIButton != null)
		{
			if (this.prevHistoryUIButton.isEnabled != this.currentHistoryIndex != 0)
			{
				this.prevHistoryUIButton.isEnabled = this.currentHistoryIndex != 0;
			}
			if (this.nextHistoryUIButton.isEnabled != this.currentHistoryIndex < this.arrHistory.Count - 1)
			{
				this.nextHistoryUIButton.isEnabled = this.currentHistoryIndex < this.arrHistory.Count - 1;
			}
		}
		if (this.isMouseDown && (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) || Input.GetMouseButtonUp(0)))
		{
			if (Input.touchCount <= 0)
			{
				vector21 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			}
			else
			{
				float single = Input.touches[0].position.x;
				Vector2 vector22 = Input.touches[0].position;
				vector21 = new Vector2(single, vector22.y);
			}
			if (this.IsCanvasConteinPosition(vector21))
			{
				this.OnCanvasClickUp();
			}
			this.isMouseDown = false;
			this.oldEditPixelPos = new Vector2(-1f, -1f);
			this.AddCanvasTextureInHistory();
		}
		if (this.isSetNewTexture)
		{
			this.isSetNewTexture = false;
			this.UpdateFonCanvas();
		}
		if (Input.touchCount > 0 && Input.touches[0].phase != TouchPhase.Ended && Input.touches[0].phase != TouchPhase.Canceled || this.isMouseDown || Input.GetMouseButtonDown(0))
		{
			if (Input.touchCount <= 0)
			{
				vector2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			}
			else
			{
				float single1 = Input.touches[0].position.x;
				Vector2 vector23 = Input.touches[0].position;
				vector2 = new Vector2(single1, vector23.y);
			}
			Vector2 vector24 = vector2;
			if (this.IsCanvasConteinPosition(vector24))
			{
				this.isMouseDown = true;
				Vector2 editPixelPos = this.GetEditPixelPos(vector24);
				if (!editPixelPos.Equals(this.oldEditPixelPos))
				{
					this.oldEditPixelPos = editPixelPos;
					this.EditCanvas(editPixelPos);
				}
			}
		}
	}

	public void UpdateFonCanvas()
	{
		this.fonCanvas.width = this.canvasTexture.width;
		this.fonCanvas.height = this.canvasTexture.height;
		this.fonCanvas.mainTexture = this.canvasTexture.mainTexture;
	}

	private void UpdateTextureFromHistory()
	{
		this.canvasTexture.mainTexture = EditorTextures.CreateCopyTexture((Texture2D)this.arrHistory[this.currentHistoryIndex]);
	}
}