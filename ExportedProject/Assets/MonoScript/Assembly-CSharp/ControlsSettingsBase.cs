using Rilisoft;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ControlsSettingsBase : MonoBehaviour
{
	public GameObject settingsPanel;

	public GameObject savePosJoystikButton;

	public GameObject defaultPosJoystikButton;

	public GameObject cancelPosJoystikButton;

	public GameObject SettingsJoysticksPanel;

	public GameObject zoomButton;

	public GameObject reloadButton;

	public GameObject jumpButton;

	public GameObject fireButton;

	public GameObject joystick;

	public GameObject grenadeButton;

	public GameObject fireButtonInJoystick;

	public UIAnchor BottomLeftControlsAnchor;

	public UIAnchor BottomRightControlsAnchor;

	public Transform BottomLeft;

	public Transform TopLeft;

	public Transform BottomRight;

	public Transform TopRight;

	public static string JoystickSett;

	public bool _isCancellationRequested;

	static ControlsSettingsBase()
	{
		ControlsSettingsBase.JoystickSett = "JoystickSettSettSett";
	}

	public ControlsSettingsBase()
	{
	}

	protected virtual void HandleCancelPosJoystikClicked(object sender, EventArgs e)
	{
		this._isCancellationRequested = true;
	}

	protected void HandleControlsClicked()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			FlurryEvents.LogAfterTraining("Controls", TrainingController.TrainingCompletedFlagForLogging.Value);
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (!GlobalGameController.LeftHanded)
		{
			this.BottomRight.localPosition = new Vector3(512f, 0f, 0f);
			this.TopRight.localPosition = new Vector3(0f, 450f, 0f);
			this.TopLeft.localPosition = new Vector3(-300f, 380f, 0f);
			this.BottomLeft.localPosition = new Vector3(0f, 0f, 0f);
			this.BottomLeftControlsAnchor.side = UIAnchor.Side.BottomRight;
			this.BottomRightControlsAnchor.side = UIAnchor.Side.BottomLeft;
		}
		else
		{
			this.BottomRight.localPosition = new Vector3(0f, 0f, 0f);
			this.TopRight.localPosition = new Vector3(-512f, 450f, 0f);
			this.TopLeft.localPosition = new Vector3(0f, 380f, 0f);
			this.BottomLeft.localPosition = new Vector3(300f, 0f, 0f);
			this.BottomLeftControlsAnchor.side = UIAnchor.Side.BottomLeft;
			this.BottomRightControlsAnchor.side = UIAnchor.Side.BottomRight;
		}
		this.SetControlsCoords();
	}

	private void HandleDefaultPosJoystikClicked(object sender, EventArgs e)
	{
		float single = (float)((!GlobalGameController.LeftHanded ? -1 : 1));
		Defs.InitCoordsIphone();
		Transform vector3 = this.zoomButton.transform;
		float zoomButtonY = (float)Defs.ZoomButtonY;
		Vector3 vector31 = this.zoomButton.transform.localPosition;
		vector3.localPosition = new Vector3((float)Defs.ZoomButtonX * single, zoomButtonY, vector31.z);
		Transform transforms = this.reloadButton.transform;
		float reloadButtonY = (float)Defs.ReloadButtonY;
		Vector3 vector32 = this.reloadButton.transform.localPosition;
		transforms.localPosition = new Vector3((float)Defs.ReloadButtonX * single, reloadButtonY, vector32.z);
		Transform transforms1 = this.jumpButton.transform;
		float jumpButtonY = (float)Defs.JumpButtonY;
		Vector3 vector33 = this.jumpButton.transform.localPosition;
		transforms1.localPosition = new Vector3((float)Defs.JumpButtonX * single, jumpButtonY, vector33.z);
		Transform transforms2 = this.fireButton.transform;
		float fireButtonY = (float)Defs.FireButtonY;
		Vector3 vector34 = this.fireButton.transform.localPosition;
		transforms2.localPosition = new Vector3((float)Defs.FireButtonX * single, fireButtonY, vector34.z);
		Transform transforms3 = this.joystick.transform;
		float joyStickY = (float)Defs.JoyStickY;
		Vector3 vector35 = this.joystick.transform.localPosition;
		transforms3.localPosition = new Vector3((float)Defs.JoyStickX * single, joyStickY, vector35.z);
		Transform transforms4 = this.grenadeButton.transform;
		float grenadeY = (float)Defs.GrenadeY;
		Vector3 vector36 = this.grenadeButton.transform.localPosition;
		transforms4.localPosition = new Vector3((float)Defs.GrenadeX * single, grenadeY, vector36.z);
		Transform transforms5 = this.fireButtonInJoystick.transform;
		float fireButton2Y = (float)Defs.FireButton2Y;
		Vector3 vector37 = this.fireButtonInJoystick.transform.localPosition;
		transforms5.localPosition = new Vector3((float)Defs.FireButton2X * single, fireButton2Y, vector37.z);
	}

	protected virtual void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		float single = (float)((!GlobalGameController.LeftHanded ? -1 : 1));
		string joystickSett = ControlsSettingsBase.JoystickSett;
		Vector3[] vector3 = new Vector3[7];
		Vector3 vector31 = this.zoomButton.transform.localPosition;
		float single1 = this.zoomButton.transform.localPosition.y;
		Vector3 vector32 = this.zoomButton.transform.localPosition;
		vector3[0] = new Vector3(vector31.x * single, single1, vector32.z);
		Vector3 vector33 = this.reloadButton.transform.localPosition;
		float single2 = this.reloadButton.transform.localPosition.y;
		Vector3 vector34 = this.reloadButton.transform.localPosition;
		vector3[1] = new Vector3(vector33.x * single, single2, vector34.z);
		Vector3 vector35 = this.jumpButton.transform.localPosition;
		float single3 = this.jumpButton.transform.localPosition.y;
		Vector3 vector36 = this.jumpButton.transform.localPosition;
		vector3[2] = new Vector3(vector35.x * single, single3, vector36.z);
		Vector3 vector37 = this.fireButton.transform.localPosition;
		float single4 = this.fireButton.transform.localPosition.y;
		Vector3 vector38 = this.fireButton.transform.localPosition;
		vector3[3] = new Vector3(vector37.x * single, single4, vector38.z);
		Vector3 vector39 = this.joystick.transform.localPosition;
		float single5 = this.joystick.transform.localPosition.y;
		Vector3 vector310 = this.joystick.transform.localPosition;
		vector3[4] = new Vector3(vector39.x * single, single5, vector310.z);
		Vector3 vector311 = this.grenadeButton.transform.localPosition;
		float single6 = this.grenadeButton.transform.localPosition.y;
		Vector3 vector312 = this.grenadeButton.transform.localPosition;
		vector3[5] = new Vector3(vector311.x * single, single6, vector312.z);
		Vector3 vector313 = this.fireButtonInJoystick.transform.localPosition;
		float single7 = this.fireButtonInJoystick.transform.localPosition.y;
		Vector3 vector314 = this.fireButtonInJoystick.transform.localPosition;
		vector3[6] = new Vector3(vector313.x * single, single7, vector314.z);
		Save.SaveVector3Array(joystickSett, vector3);
		this.SettingsJoysticksPanel.SetActive(false);
		this.settingsPanel.SetActive(true);
		ExperienceController.sharedController.isShowRanks = false;
		if (ControlsSettingsBase.ControlsChanged != null)
		{
			ControlsSettingsBase.ControlsChanged();
		}
	}

	protected void OnEnable()
	{
		if (ExperienceController.sharedController != null && !ShopNGUIController.GuiActive)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.InterfaceEnabled = false;
		}
		this.SetControlsCoords();
	}

	private void SetControlsCoords()
	{
		float single = (!GlobalGameController.LeftHanded ? -1f : 1f);
		Vector3[] vector3Array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (vector3Array == null || (int)vector3Array.Length < 7)
		{
			Defs.InitCoordsIphone();
			Transform vector3 = this.zoomButton.transform;
			float zoomButtonY = (float)Defs.ZoomButtonY;
			Vector3 vector31 = this.zoomButton.transform.localPosition;
			vector3.localPosition = new Vector3((float)Defs.ZoomButtonX * single, zoomButtonY, vector31.z);
			Transform transforms = this.reloadButton.transform;
			float reloadButtonY = (float)Defs.ReloadButtonY;
			Vector3 vector32 = this.reloadButton.transform.localPosition;
			transforms.localPosition = new Vector3((float)Defs.ReloadButtonX * single, reloadButtonY, vector32.z);
			Transform transforms1 = this.jumpButton.transform;
			float jumpButtonY = (float)Defs.JumpButtonY;
			Vector3 vector33 = this.jumpButton.transform.localPosition;
			transforms1.localPosition = new Vector3((float)Defs.JumpButtonX * single, jumpButtonY, vector33.z);
			Transform transforms2 = this.fireButton.transform;
			float fireButtonY = (float)Defs.FireButtonY;
			Vector3 vector34 = this.fireButton.transform.localPosition;
			transforms2.localPosition = new Vector3((float)Defs.FireButtonX * single, fireButtonY, vector34.z);
			Transform transforms3 = this.grenadeButton.transform;
			float grenadeY = (float)Defs.GrenadeY;
			Vector3 vector35 = this.grenadeButton.transform.localPosition;
			transforms3.localPosition = new Vector3((float)Defs.GrenadeX * single, grenadeY, vector35.z);
			Transform transforms4 = this.joystick.transform;
			float joyStickY = (float)Defs.JoyStickY;
			Vector3 vector36 = this.joystick.transform.localPosition;
			transforms4.localPosition = new Vector3((float)Defs.JoyStickX * single, joyStickY, vector36.z);
			Transform transforms5 = this.fireButtonInJoystick.transform;
			float fireButton2Y = (float)Defs.FireButton2Y;
			Vector3 vector37 = this.fireButtonInJoystick.transform.localPosition;
			transforms5.localPosition = new Vector3((float)Defs.FireButton2X * single, fireButton2Y, vector37.z);
		}
		else
		{
			for (int i = 0; i < (int)vector3Array.Length; i++)
			{
				vector3Array[i].x *= single;
			}
			this.zoomButton.transform.localPosition = vector3Array[0];
			this.reloadButton.transform.localPosition = vector3Array[1];
			this.jumpButton.transform.localPosition = vector3Array[2];
			this.fireButton.transform.localPosition = vector3Array[3];
			this.joystick.transform.localPosition = vector3Array[4];
			this.grenadeButton.transform.localPosition = vector3Array[5];
			this.fireButtonInJoystick.transform.localPosition = vector3Array[6];
		}
		this.grenadeButton.transform.GetChild(0).GetComponent<UISprite>().spriteName = (!Defs.isDaterRegim ? "grenade_btn" : "grenade_like_btn");
	}

	protected void Start()
	{
		if (this.savePosJoystikButton != null)
		{
			ButtonHandler component = this.savePosJoystikButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				ControlsSettingsBase controlsSettingsBase = this;
				component.Clicked += new EventHandler(controlsSettingsBase.HandleSavePosJoystikClicked);
			}
		}
		if (this.defaultPosJoystikButton != null)
		{
			ButtonHandler buttonHandler = this.defaultPosJoystikButton.GetComponent<ButtonHandler>();
			if (buttonHandler != null)
			{
				buttonHandler.Clicked += new EventHandler(this.HandleDefaultPosJoystikClicked);
			}
		}
		if (this.cancelPosJoystikButton != null)
		{
			ButtonHandler component1 = this.cancelPosJoystikButton.GetComponent<ButtonHandler>();
			if (component1 != null)
			{
				ControlsSettingsBase controlsSettingsBase1 = this;
				component1.Clicked += new EventHandler(controlsSettingsBase1.HandleCancelPosJoystikClicked);
			}
		}
	}

	public static event Action ControlsChanged;
}