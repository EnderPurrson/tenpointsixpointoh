using System;
using System.Collections.Generic;
using UnityEngine;

public class EveryplayRecButtons : MonoBehaviour
{
	private const int atlasWidth = 256;

	private const int atlasHeight = 256;

	private const int atlasPadding = 2;

	public Texture2D atlasTexture;

	public EveryplayRecButtons.ButtonsOrigin origin;

	public Vector2 containerMargin = new Vector2(16f, 16f);

	private Vector2 containerOffset = Vector2.zero;

	private float containerScaling = 1f;

	private int buttonTitleHorizontalMargin = 16;

	private int buttonTitleVerticalMargin = 20;

	private int buttonMargin = 8;

	private bool faceCamPermissionGranted;

	private bool startFaceCamWhenPermissionGranted;

	private EveryplayRecButtons.TextureAtlasSrc editVideoAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc faceCamAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc openEveryplayAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc shareVideoAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc startRecordingAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc stopRecordingAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc facecamToggleOnAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc facecamToggleOffAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc bgHeaderAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc bgFooterAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc bgAtlasSrc;

	private EveryplayRecButtons.TextureAtlasSrc buttonAtlasSrc;

	private EveryplayRecButtons.Button shareVideoButton;

	private EveryplayRecButtons.Button editVideoButton;

	private EveryplayRecButtons.Button openEveryplayButton;

	private EveryplayRecButtons.Button startRecordingButton;

	private EveryplayRecButtons.Button stopRecordingButton;

	private EveryplayRecButtons.ToggleButton faceCamToggleButton;

	private EveryplayRecButtons.Button tappedButton;

	private List<EveryplayRecButtons.Button> visibleButtons;

	public EveryplayRecButtons()
	{
	}

	private void Awake()
	{
		this.containerScaling = this.GetScalingByResolution();
		this.editVideoAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(112, 19, 0, 0, this.containerScaling);
		this.faceCamAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(103, 19, 116, 0, this.containerScaling);
		this.openEveryplayAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(178, 23, 0, 23, this.containerScaling);
		this.shareVideoAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(134, 19, 0, 50, this.containerScaling);
		this.startRecordingAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(171, 23, 0, 73, this.containerScaling);
		this.stopRecordingAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(169, 23, 0, 100, this.containerScaling);
		this.facecamToggleOnAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(101, 42, 0, 127, this.containerScaling);
		this.facecamToggleOffAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(101, 42, 101, 127, this.containerScaling);
		this.bgHeaderAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(256, 9, 0, 169, this.containerScaling);
		this.bgFooterAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(256, 9, 0, 169, this.containerScaling);
		this.bgAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(256, 6, 0, 178, this.containerScaling);
		this.buttonAtlasSrc = new EveryplayRecButtons.TextureAtlasSrc(220, 64, 0, 190, this.containerScaling);
		this.buttonTitleHorizontalMargin = Mathf.RoundToInt((float)this.buttonTitleHorizontalMargin * this.containerScaling);
		this.buttonTitleVerticalMargin = Mathf.RoundToInt((float)this.buttonTitleVerticalMargin * this.containerScaling);
		this.buttonMargin = Mathf.RoundToInt((float)this.buttonMargin * this.containerScaling);
		this.shareVideoButton = new EveryplayRecButtons.Button(this.buttonAtlasSrc, this.shareVideoAtlasSrc, new EveryplayRecButtons.ButtonTapped(this.ShareVideo));
		this.editVideoButton = new EveryplayRecButtons.Button(this.buttonAtlasSrc, this.editVideoAtlasSrc, new EveryplayRecButtons.ButtonTapped(this.EditVideo));
		this.openEveryplayButton = new EveryplayRecButtons.Button(this.buttonAtlasSrc, this.openEveryplayAtlasSrc, new EveryplayRecButtons.ButtonTapped(this.OpenEveryplay));
		this.startRecordingButton = new EveryplayRecButtons.Button(this.buttonAtlasSrc, this.startRecordingAtlasSrc, new EveryplayRecButtons.ButtonTapped(this.StartRecording));
		this.stopRecordingButton = new EveryplayRecButtons.Button(this.buttonAtlasSrc, this.stopRecordingAtlasSrc, new EveryplayRecButtons.ButtonTapped(this.StopRecording));
		this.faceCamToggleButton = new EveryplayRecButtons.ToggleButton(this.buttonAtlasSrc, this.faceCamAtlasSrc, new EveryplayRecButtons.ButtonTapped(this.FaceCamToggle), this.facecamToggleOnAtlasSrc, this.facecamToggleOffAtlasSrc);
		this.visibleButtons = new List<EveryplayRecButtons.Button>();
		this.bgFooterAtlasSrc.normalizedAtlasRect.y = this.bgFooterAtlasSrc.normalizedAtlasRect.y + this.bgFooterAtlasSrc.normalizedAtlasRect.height;
		this.bgFooterAtlasSrc.normalizedAtlasRect.height = -this.bgFooterAtlasSrc.normalizedAtlasRect.height;
		this.SetButtonVisible(this.startRecordingButton, true);
		this.SetButtonVisible(this.openEveryplayButton, true);
		this.SetButtonVisible(this.faceCamToggleButton, true);
		if (!Everyplay.IsRecordingSupported())
		{
			this.startRecordingButton.enabled = false;
			this.stopRecordingButton.enabled = false;
		}
		Everyplay.RecordingStarted += new Everyplay.RecordingStartedDelegate(this.RecordingStarted);
		Everyplay.RecordingStopped += new Everyplay.RecordingStoppedDelegate(this.RecordingStopped);
		Everyplay.ReadyForRecording += new Everyplay.ReadyForRecordingDelegate(this.ReadyForRecording);
		Everyplay.FaceCamRecordingPermission += new Everyplay.FaceCamRecordingPermissionDelegate(this.FaceCamRecordingPermission);
	}

	private int CalculateContainerHeight()
	{
		float single = 0f;
		float single1 = this.bgHeaderAtlasSrc.atlasRect.height + ((float)(this.buttonMargin * 2) - this.bgHeaderAtlasSrc.atlasRect.height);
		foreach (EveryplayRecButtons.Button visibleButton in this.visibleButtons)
		{
			visibleButton.screenRect.x = (this.bgAtlasSrc.atlasRect.width - visibleButton.screenRect.width) / 2f;
			visibleButton.screenRect.y = single1;
			single1 = single1 + ((float)this.buttonMargin + visibleButton.screenRect.height);
			single = single + ((float)this.buttonMargin + visibleButton.screenRect.height);
		}
		int num = Mathf.RoundToInt(single + this.bgHeaderAtlasSrc.atlasRect.height + this.bgFooterAtlasSrc.atlasRect.height);
		return num;
	}

	private void Destroy()
	{
		Everyplay.RecordingStarted -= new Everyplay.RecordingStartedDelegate(this.RecordingStarted);
		Everyplay.RecordingStopped -= new Everyplay.RecordingStoppedDelegate(this.RecordingStopped);
		Everyplay.ReadyForRecording -= new Everyplay.ReadyForRecordingDelegate(this.ReadyForRecording);
		Everyplay.FaceCamRecordingPermission -= new Everyplay.FaceCamRecordingPermissionDelegate(this.FaceCamRecordingPermission);
	}

	private void DrawBackround(int containerHeight)
	{
		this.DrawTexture(0f, 0f, this.bgHeaderAtlasSrc.atlasRect.width, this.bgHeaderAtlasSrc.atlasRect.height, this.atlasTexture, this.bgHeaderAtlasSrc.normalizedAtlasRect);
		this.DrawTexture(0f, this.bgHeaderAtlasSrc.atlasRect.height, this.bgAtlasSrc.atlasRect.width, (float)containerHeight - this.bgHeaderAtlasSrc.atlasRect.height - this.bgFooterAtlasSrc.atlasRect.height, this.atlasTexture, this.bgAtlasSrc.normalizedAtlasRect);
		this.DrawTexture(0f, (float)containerHeight - this.bgFooterAtlasSrc.atlasRect.height, this.bgFooterAtlasSrc.atlasRect.width, this.bgFooterAtlasSrc.atlasRect.height, this.atlasTexture, this.bgFooterAtlasSrc.normalizedAtlasRect);
	}

	private void DrawButton(EveryplayRecButtons.Button button, Color tintColor)
	{
		object obj;
		Color color = GUI.color;
		bool flag = false;
		flag = typeof(EveryplayRecButtons.ToggleButton).IsAssignableFrom(button.GetType());
		if (!flag)
		{
			GUI.color = tintColor;
			this.DrawTexture(button.screenRect.x, button.screenRect.y, button.bg.atlasRect.width, button.bg.atlasRect.height, this.atlasTexture, button.bg.normalizedAtlasRect);
			GUI.color = color;
		}
		else
		{
			EveryplayRecButtons.ToggleButton toggleButton = (EveryplayRecButtons.ToggleButton)button;
			if (button != null)
			{
				float single = button.screenRect.x + button.screenRect.width - toggleButton.toggleOn.atlasRect.width;
				float single1 = button.screenRect.y + button.screenRect.height / 2f - toggleButton.toggleOn.atlasRect.height / 2f;
				EveryplayRecButtons.TextureAtlasSrc textureAtlasSrc = (!toggleButton.toggled ? toggleButton.toggleOff : toggleButton.toggleOn);
				GUI.color = tintColor;
				this.DrawTexture(single, single1, textureAtlasSrc.atlasRect.width, textureAtlasSrc.atlasRect.height, this.atlasTexture, textureAtlasSrc.normalizedAtlasRect);
				GUI.color = color;
			}
		}
		if (!flag)
		{
			obj = this.buttonTitleHorizontalMargin;
		}
		else
		{
			obj = null;
		}
		float single2 = (float)obj;
		if (!button.enabled)
		{
			GUI.color = tintColor;
		}
		this.DrawTexture(button.screenRect.x + single2, button.screenRect.y + (float)this.buttonTitleVerticalMargin, button.title.atlasRect.width, button.title.atlasRect.height, this.atlasTexture, button.title.normalizedAtlasRect);
		if (!button.enabled)
		{
			GUI.color = color;
		}
	}

	private void DrawButtons()
	{
		foreach (EveryplayRecButtons.Button visibleButton in this.visibleButtons)
		{
			if (!visibleButton.enabled)
			{
				this.DrawButton(visibleButton, new Color(0.5f, 0.5f, 0.5f, 0.3f));
			}
			else
			{
				this.DrawButton(visibleButton, (this.tappedButton != visibleButton ? Color.white : Color.gray));
			}
		}
	}

	private void DrawTexture(float x, float y, float width, float height, Texture2D texture, Rect uvRect)
	{
		x += this.containerOffset.x;
		y += this.containerOffset.y;
		GUI.DrawTextureWithTexCoords(new Rect(x, y, width, height), texture, uvRect, true);
	}

	private void EditVideo()
	{
		Everyplay.PlayLastRecording();
	}

	private void FaceCamRecordingPermission(bool granted)
	{
		this.faceCamPermissionGranted = granted;
		if (this.startFaceCamWhenPermissionGranted)
		{
			this.faceCamToggleButton.toggled = granted;
			Everyplay.FaceCamStartSession();
			if (Everyplay.FaceCamIsSessionRunning())
			{
				this.startFaceCamWhenPermissionGranted = false;
			}
		}
	}

	private void FaceCamToggle()
	{
		if (!this.faceCamPermissionGranted)
		{
			Everyplay.FaceCamRequestRecordingPermission();
			this.startFaceCamWhenPermissionGranted = true;
		}
		else
		{
			this.faceCamToggleButton.toggled = !this.faceCamToggleButton.toggled;
			if (this.faceCamToggleButton.toggled)
			{
				if (!Everyplay.FaceCamIsSessionRunning())
				{
					Everyplay.FaceCamStartSession();
				}
			}
			else if (Everyplay.FaceCamIsSessionRunning())
			{
				Everyplay.FaceCamStopSession();
			}
		}
	}

	private float GetScalingByResolution()
	{
		int num = Mathf.Max(Screen.height, Screen.width);
		int num1 = Mathf.Min(Screen.height, Screen.width);
		if (num >= 640 && (num != 1024 || num1 != 768))
		{
			return 1f;
		}
		return 0.5f;
	}

	private void OnGUI()
	{
		if (Event.current.type.Equals(EventType.Repaint))
		{
			int num = this.CalculateContainerHeight();
			this.UpdateContainerOffset(num);
			this.DrawBackround(num);
			this.DrawButtons();
		}
	}

	private void OpenEveryplay()
	{
		Everyplay.Show();
	}

	private void ReadyForRecording(bool enabled)
	{
		this.startRecordingButton.enabled = enabled;
		this.stopRecordingButton.enabled = enabled;
	}

	private void RecordingStarted()
	{
		this.ReplaceVisibleButton(this.startRecordingButton, this.stopRecordingButton);
		this.SetButtonVisible(this.shareVideoButton, false);
		this.SetButtonVisible(this.editVideoButton, false);
		this.SetButtonVisible(this.faceCamToggleButton, false);
	}

	private void RecordingStopped()
	{
		this.ReplaceVisibleButton(this.stopRecordingButton, this.startRecordingButton);
		this.SetButtonVisible(this.shareVideoButton, true);
		this.SetButtonVisible(this.editVideoButton, true);
		this.SetButtonVisible(this.faceCamToggleButton, true);
	}

	private void ReplaceVisibleButton(EveryplayRecButtons.Button button, EveryplayRecButtons.Button replacementButton)
	{
		int num = this.visibleButtons.IndexOf(button);
		if (num >= 0)
		{
			this.visibleButtons[num] = replacementButton;
		}
	}

	private void SetButtonVisible(EveryplayRecButtons.Button button, bool visible)
	{
		if (this.visibleButtons.Contains(button))
		{
			if (!visible)
			{
				this.visibleButtons.Remove(button);
			}
		}
		else if (visible)
		{
			this.visibleButtons.Add(button);
		}
	}

	private void ShareVideo()
	{
		Everyplay.ShowSharingModal();
	}

	private void StartRecording()
	{
		Everyplay.StartRecording();
	}

	private void StopRecording()
	{
		Everyplay.StopRecording();
	}

	private void Update()
	{
		Touch[] touchArray = Input.touches;
		for (int i = 0; i < (int)touchArray.Length; i++)
		{
			Touch touch = touchArray[i];
			if (touch.phase == TouchPhase.Began)
			{
				foreach (EveryplayRecButtons.Button visibleButton in this.visibleButtons)
				{
					float single = touch.position.x - this.containerOffset.x;
					float single1 = (float)Screen.height;
					Vector2 vector2 = touch.position;
					if (!visibleButton.screenRect.Contains(new Vector2(single, single1 - vector2.y - this.containerOffset.y)))
					{
						continue;
					}
					this.tappedButton = visibleButton;
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				foreach (EveryplayRecButtons.Button button in this.visibleButtons)
				{
					float single2 = touch.position.x - this.containerOffset.x;
					float single3 = (float)Screen.height;
					Vector2 vector21 = touch.position;
					if (!button.screenRect.Contains(new Vector2(single2, single3 - vector21.y - this.containerOffset.y)) || button.onTap == null)
					{
						continue;
					}
					button.onTap();
				}
				this.tappedButton = null;
			}
			else if (touch.phase == TouchPhase.Canceled)
			{
				this.tappedButton = null;
			}
		}
	}

	private void UpdateContainerOffset(int containerHeight)
	{
		if (this.origin == EveryplayRecButtons.ButtonsOrigin.TopRight)
		{
			this.containerOffset.x = (float)Screen.width - this.containerMargin.x * this.containerScaling - this.bgAtlasSrc.atlasRect.width;
			this.containerOffset.y = this.containerMargin.y * this.containerScaling;
		}
		else if (this.origin == EveryplayRecButtons.ButtonsOrigin.BottomLeft)
		{
			this.containerOffset.x = this.containerMargin.x * this.containerScaling;
			this.containerOffset.y = (float)Screen.height - this.containerMargin.y * this.containerScaling - (float)containerHeight;
		}
		else if (this.origin != EveryplayRecButtons.ButtonsOrigin.BottomRight)
		{
			this.containerOffset.x = this.containerMargin.x * this.containerScaling;
			this.containerOffset.y = this.containerMargin.y * this.containerScaling;
		}
		else
		{
			this.containerOffset.x = (float)Screen.width - this.containerMargin.x * this.containerScaling - this.bgAtlasSrc.atlasRect.width;
			this.containerOffset.y = (float)Screen.height - this.containerMargin.y * this.containerScaling - (float)containerHeight;
		}
	}

	private class Button
	{
		public bool enabled;

		public Rect screenRect;

		public EveryplayRecButtons.TextureAtlasSrc bg;

		public EveryplayRecButtons.TextureAtlasSrc title;

		public EveryplayRecButtons.ButtonTapped onTap;

		public Button(EveryplayRecButtons.TextureAtlasSrc bg, EveryplayRecButtons.TextureAtlasSrc title, EveryplayRecButtons.ButtonTapped buttonTapped)
		{
			this.enabled = true;
			this.bg = bg;
			this.title = title;
			this.screenRect.width = bg.atlasRect.width;
			this.screenRect.height = bg.atlasRect.height;
			this.onTap = buttonTapped;
		}
	}

	public enum ButtonsOrigin
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	private delegate void ButtonTapped();

	private class TextureAtlasSrc
	{
		public Rect atlasRect;

		public Rect normalizedAtlasRect;

		public TextureAtlasSrc(int width, int height, int x, int y, float scale)
		{
			this.atlasRect.x = (float)(x + 2);
			this.atlasRect.y = (float)(y + 2);
			this.atlasRect.width = (float)width * scale;
			this.atlasRect.height = (float)height * scale;
			this.normalizedAtlasRect.width = (float)width / 256f;
			this.normalizedAtlasRect.height = (float)height / 256f;
			this.normalizedAtlasRect.x = this.atlasRect.x / 256f;
			this.normalizedAtlasRect.y = 1f - (this.atlasRect.y + (float)height) / 256f;
		}
	}

	private class ToggleButton : EveryplayRecButtons.Button
	{
		public EveryplayRecButtons.TextureAtlasSrc toggleOn;

		public EveryplayRecButtons.TextureAtlasSrc toggleOff;

		public bool toggled;

		public ToggleButton(EveryplayRecButtons.TextureAtlasSrc bg, EveryplayRecButtons.TextureAtlasSrc title, EveryplayRecButtons.ButtonTapped buttonTapped, EveryplayRecButtons.TextureAtlasSrc toggleOn, EveryplayRecButtons.TextureAtlasSrc toggleOff) : base(bg, title, buttonTapped)
		{
			this.toggleOn = toggleOn;
			this.toggleOff = toggleOff;
		}
	}
}