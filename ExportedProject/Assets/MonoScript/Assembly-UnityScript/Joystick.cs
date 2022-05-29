using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Joystick : MonoBehaviour
{
	[NonSerialized]
	private static Joystick[] joysticks;

	[NonSerialized]
	private static bool enumeratedJoysticks;

	[NonSerialized]
	private static float tapTimeDelta;

	public bool touchPad;

	public Rect touchZone;

	public Vector2 deadZone;

	public bool normalize;

	public Vector2 position;

	public int tapCount;

	public bool halfScreenZone;

	private int lastFingerId;

	private float tapTimeWindow;

	public Vector2 fingerDownPos;

	private float fingerDownTime;

	private float firstDeltaTime;

	private Rect defaultRect;

	private Boundary guiBoundary;

	private Vector2 guiTouchOffset;

	private Vector2 guiCenter;

	public bool jumpPressed;

	public Texture fireTexture;

	public Texture reloadTexture;

	public Texture reloadTextureNoAmmo;

	private Rect fireZone;

	public GameObject _playerGun;

	private Rect reloadZone;

	private Rect joystickZone;

	private Vector2 _lastFingerPosition;

	private bool blink;

	private bool NormalReloadMode;

	private bool isSerialShooting;

	public Vector3 pos;

	private Rect guiPixelInset;

	private Rect jumpTexturePixelInset;

	private Texture2D gui;

	private Texture2D jumpTexture;

	private float guiCoeff;

	private bool touchBeginsOnFireZone;

	static Joystick()
	{
		Joystick.tapTimeDelta = 0.3f;
	}

	public Joystick()
	{
		this.deadZone = Vector2.zero;
		this.lastFingerId = -1;
		this.firstDeltaTime = 0.5f;
		this.guiBoundary = new Boundary();
		this.NormalReloadMode = true;
		this.pos = new Vector3((float)0, (float)0, (float)0);
		this.guiCoeff = (float)Screen.height / 640f;
	}

	public override void Awake()
	{
	}

	public override IEnumerator BlinkReload()
	{
		return (new Joystick.u0024BlinkReloadu002425(this)).GetEnumerator();
	}

	public override void Disable()
	{
		this.gameObject.active = false;
		Joystick.enumeratedJoysticks = false;
	}

	public override void Enable()
	{
		this.gameObject.active = true;
	}

	public override void HasAmmo()
	{
		if (!this.NormalReloadMode)
		{
			this.NormalReloadMode = true;
			this.StopCoroutine("BlinkReload");
			this.blink = false;
		}
	}

	public override bool IsFingerDown()
	{
		return this.lastFingerId != -1;
	}

	public override void LatchedFinger(int fingerId)
	{
		if (this.lastFingerId == fingerId)
		{
			this.ResetJoystick();
		}
	}

	public override void Main()
	{
	}

	public override void NoAmmo()
	{
		if (this.NormalReloadMode)
		{
			this.NormalReloadMode = false;
			this.StartCoroutine("BlinkReload");
		}
	}

	public override void OnGUI()
	{
		Texture texture;
		Color color = GUI.color;
		GUI.color = new Color(color.r, color.g, color.b, (float)38);
		if (this.fireTexture)
		{
			GUI.DrawTexture(new Rect(this.fireZone.x, (float)Screen.height - this.fireZone.height - this.fireZone.y, this.fireZone.width, this.fireZone.height), this.fireTexture);
		}
		if (this.reloadTexture)
		{
			Rect rect = new Rect(this.reloadZone.x, (float)Screen.height - this.reloadZone.height - this.reloadZone.y, this.reloadZone.height, this.reloadZone.height);
			if (!this.NormalReloadMode)
			{
				texture = (!this.blink ? this.reloadTexture : this.reloadTextureNoAmmo);
			}
			else
			{
				texture = this.reloadTexture;
			}
			GUI.DrawTexture(rect, texture);
		}
		if (this.gui)
		{
			GUI.DrawTexture(new Rect(this.guiPixelInset.x, (float)Screen.height - this.guiPixelInset.height - this.guiPixelInset.y, this.guiPixelInset.width, this.guiPixelInset.height), this.gui);
		}
		GUI.color = color;
	}

	public override void ResetJoystick()
	{
		if ((!this.halfScreenZone || !this.touchPad || !this.touchPad) && this.gui)
		{
			this.guiPixelInset = this.defaultRect;
		}
		this.lastFingerId = -1;
		this.position = Vector2.zero;
		this.fingerDownPos = Vector2.zero;
	}

	public override void setSeriya(bool isSeriya)
	{
		this.isSerialShooting = isSeriya;
	}

	public override void Start()
	{
		if (this.touchPad)
		{
			this.guiPixelInset = new Rect((float)-200, (float)0, (float)200, (float)125);
			this.gui = (Texture2D)(Resources.Load("Jump") as Texture);
		}
		else
		{
			this.guiPixelInset = new Rect((float)0, (float)0, (float)128, (float)128);
			this.gui = (Texture2D)(Resources.Load("Move") as Texture);
		}
		if (this.touchPad)
		{
			int num = 1;
			int num1 = num;
			Vector3 vector3 = this.transform.position;
			Vector3 vector31 = vector3;
			float single = (float)num1;
			float single1 = single;
			vector31.x = single;
			Vector3 vector32 = vector31;
			Vector3 vector33 = vector32;
			this.transform.position = vector32;
		}
		this.guiPixelInset = new Rect(this.guiPixelInset.x * (float)Screen.height / (float)640, this.guiPixelInset.y * (float)Screen.height / (float)640, this.guiPixelInset.width * (float)Screen.height / (float)640, this.guiPixelInset.height * (float)Screen.height / (float)640);
		this.defaultRect = this.guiPixelInset;
		this.defaultRect.x = this.defaultRect.x + this.pos.x * (float)Screen.width;
		this.defaultRect.y = this.defaultRect.y + this.pos.y * (float)Screen.height;
		float single2 = 1.2f;
		if (this.halfScreenZone)
		{
			this.defaultRect.y = (float)0;
			this.defaultRect.x = (float)Screen.width / 2f;
			this.defaultRect.width = (float)Screen.width / 2f;
			this.defaultRect.height = (float)Screen.height * 0.6f;
			this.jumpTexture = this.gui;
			float single3 = (single2 - 1f) * 0.5f;
			this.jumpTexture = this.gui;
			this.jumpTexturePixelInset = new Rect((float)Screen.width - (float)this.jumpTexture.width * (single3 + 1f) * (float)Screen.height / (float)640, (float)(this.jumpTexture.height * Screen.height / 640) * single3 / (float)2, (float)(this.jumpTexture.width * Screen.height / 640), (float)(this.jumpTexture.height * Screen.height / 640));
			this.guiPixelInset = this.jumpTexturePixelInset;
			int num2 = this.fireTexture.width * Screen.height / 640;
			this.fireZone = new Rect((float)Screen.width - (float)Screen.height * 0.4f, (float)Screen.height * 0.15f - (float)(num2 / 2), (float)num2, (float)num2);
			if (this.reloadTexture)
			{
				this.reloadZone = new Rect((float)Screen.width - (float)this.reloadTexture.width * 1.1f * (float)Screen.height / (float)640, (float)Screen.height * 0.4f, this.fireZone.width * 0.65f, this.fireZone.height * 0.65f);
			}
		}
		else if (this.reloadTexture)
		{
			this.reloadZone = new Rect((float)Screen.width - (float)this.reloadTexture.width * 1.1f * (float)Screen.height / (float)640, (float)Screen.height * 0.4f, this.fireZone.width * 0.65f, this.fireZone.height * 0.65f);
		}
		this.pos.x = (float)0;
		this.pos.y = (float)0;
		if (!this.touchPad)
		{
			this.joystickZone = new Rect((float)0, (float)0, (float)Screen.width / 2f, (float)Screen.height / 2f);
			this.defaultRect = this.guiPixelInset;
			this.defaultRect.x = (float)Screen.height * 0.1f;
			this.defaultRect.y = (float)Screen.height * 0.1f;
			this.guiTouchOffset.x = this.defaultRect.width * 0.5f;
			this.guiTouchOffset.y = this.defaultRect.height * 0.5f;
			this.guiCenter.x = this.defaultRect.x + this.guiTouchOffset.x;
			this.guiCenter.y = this.defaultRect.y + this.guiTouchOffset.y;
			this.guiBoundary.min.x = this.defaultRect.x - this.guiTouchOffset.x;
			this.guiBoundary.max.x = this.defaultRect.x + this.guiTouchOffset.x;
			this.guiBoundary.min.y = this.defaultRect.y - this.guiTouchOffset.y;
			this.guiBoundary.max.y = this.defaultRect.y + this.guiTouchOffset.y;
		}
		else
		{
			this.touchZone = this.defaultRect;
		}
	}

	public override void Update()
	{
		if (!Joystick.enumeratedJoysticks)
		{
			Joystick.joysticks = (Joystick[])UnityEngine.Object.FindObjectsOfType(typeof(Joystick)) as Joystick[];
			Joystick.enumeratedJoysticks = true;
		}
		int num = Input.touchCount;
		if (this.tapTimeWindow <= (float)0)
		{
			this.tapCount = 0;
		}
		else
		{
			this.tapTimeWindow -= Time.deltaTime;
		}
		if (num != 0)
		{
			for (int i = 0; i < num; i++)
			{
				Touch touch = Input.GetTouch(i);
				Vector2 vector2 = touch.position - this.guiTouchOffset;
				bool flag = false;
				if (this.touchPad)
				{
					if (this.touchZone.Contains(touch.position))
					{
						flag = true;
					}
				}
				else if (this.guiPixelInset.Contains(touch.position))
				{
					flag = true;
				}
				this.isSerialShooting = PlayerPrefs.GetInt("setSeriya") == 1;
				bool flag1 = flag;
				if (flag1)
				{
					flag1 = this.lastFingerId == -1;
					if (!flag1)
					{
						flag1 = this.lastFingerId != touch.fingerId;
					}
				}
				bool flag2 = flag1;
				if (flag2)
				{
					this.touchBeginsOnFireZone = this.fireZone.Contains(touch.position);
				}
				if (this.isSerialShooting && this.touchPad && flag)
				{
					if (!this.fireTexture || !this.touchZone.Contains(touch.position) || !this.touchBeginsOnFireZone || this.blink)
					{
						this.touchBeginsOnFireZone = false;
					}
					else
					{
						this._playerGun.SendMessage("ShotPressed");
					}
				}
				if (flag2)
				{
					if (this.touchPad)
					{
						this.lastFingerId = touch.fingerId;
						this.fingerDownPos = touch.position;
						this.fingerDownTime = Time.time;
					}
					this.lastFingerId = touch.fingerId;
					if (this.tapTimeWindow <= (float)0)
					{
						this.tapCount = 1;
						this.tapTimeWindow = Joystick.tapTimeDelta;
					}
					else
					{
						this.tapCount++;
					}
					int num1 = 0;
					Joystick[] joystickArray = Joystick.joysticks;
					int length = joystickArray.Length;
					while (num1 < length)
					{
						if (joystickArray[num1] != this)
						{
							joystickArray[num1].LatchedFinger(touch.fingerId);
						}
						num1++;
					}
					if (this.fireTexture && this.fireZone.Contains(touch.position) && !this.isSerialShooting)
					{
						goto Label1;
					}
					if (this.jumpTexture && this.jumpTexturePixelInset.Contains(touch.position))
					{
						this.jumpPressed = true;
					}
					if (this.touchPad && this.reloadZone.Contains(touch.position))
					{
						this._playerGun.SendMessage("ReloadPressed");
					}
					if (this.touchPad)
					{
						this._lastFingerPosition = touch.position;
					}
				}
				if (this.lastFingerId == touch.fingerId)
				{
					if (touch.tapCount > this.tapCount)
					{
						this.tapCount = touch.tapCount;
					}
					if (!this.touchPad)
					{
						this.guiPixelInset.x = Mathf.Clamp(vector2.x, this.guiBoundary.min.x, this.guiBoundary.max.x);
						this.guiPixelInset.y = Mathf.Clamp(vector2.y, this.guiBoundary.min.y, this.guiBoundary.max.y);
					}
					else
					{
						float single = (float)25;
						Vector2 vector21 = touch.position;
						this.position.x = Mathf.Clamp((vector21.x - this.fingerDownPos.x) * 1f / (float)1, -single, single);
						Vector2 vector22 = touch.position;
						this.position.y = Mathf.Clamp((vector22.y - this.fingerDownPos.y) * 1f / (float)1, -single, single);
						this.fingerDownPos = touch.position;
					}
					if (!flag2 && this.touchPad && this.touchZone.Contains(touch.position))
					{
						this._lastFingerPosition = touch.position;
					}
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						this.ResetJoystick();
					}
				}
			Label0:
			}
		}
		else
		{
			this.ResetJoystick();
		}
		if (!this.touchPad)
		{
			this.position.x = (this.guiPixelInset.x + this.guiTouchOffset.x - this.guiCenter.x) / this.guiTouchOffset.x;
			this.position.y = (this.guiPixelInset.y + this.guiTouchOffset.y - this.guiCenter.y) / this.guiTouchOffset.y;
		}
		float single1 = Mathf.Abs(this.position.x);
		float single2 = Mathf.Abs(this.position.y);
		if (single1 < this.deadZone.x)
		{
			this.position.x = (float)0;
		}
		else if (this.normalize)
		{
			this.position.x = Mathf.Sign(this.position.x) * (single1 - this.deadZone.x) / ((float)1 - this.deadZone.x);
		}
		if (single2 < this.deadZone.y)
		{
			this.position.y = (float)0;
		}
		else if (this.normalize)
		{
			this.position.y = Mathf.Sign(this.position.y) * (single2 - this.deadZone.y) / ((float)1 - this.deadZone.y);
		}
		return;
	Label1:
		this._playerGun.SendMessage("ShotPressed");
		goto Label0;
	}
}