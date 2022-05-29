using System;
using UnityEngine;

public class ButtonJoystickAdjust : MonoBehaviour
{
	[SerializeField]
	private GameObject ZoneTopLeft;

	[SerializeField]
	private GameObject ZoneBottonRight;

	private bool _isZone = true;

	public float TimeBlink = 0.5f;

	private float _curTimeBlink;

	public bool IsPress;

	public bool _isDrag;

	private bool _isHidden;

	private bool _isDragLate = true;

	public static EventHandler<EventArgs> PressedDown;

	private Vector3? _nonClampedPosition;

	public ButtonJoystickAdjust()
	{
	}

	public Vector2 GetJoystickPosition()
	{
		Vector3 vector3 = base.transform.localPosition;
		return new Vector2(vector3.x, vector3.y);
	}

	public bool IsDrag()
	{
		bool flag = this._isDrag;
		this._isDrag = false;
		return flag;
	}

	private void LateUpdate()
	{
		if (this.IsPress)
		{
			if (this._curTimeBlink >= 0f)
			{
				this._curTimeBlink -= Time.deltaTime;
			}
			else
			{
				if (!this._isHidden)
				{
					TweenAlpha.Begin(base.gameObject, this.TimeBlink, 0.1f);
					this._isHidden = true;
				}
				else
				{
					TweenAlpha.Begin(base.gameObject, this.TimeBlink, 0.9f);
					this._isHidden = false;
				}
				this._curTimeBlink = this.TimeBlink;
			}
			this._isDragLate = false;
		}
		else if (!this._isDragLate)
		{
			TweenAlpha.Begin(base.gameObject, 0.5f, 1f);
			this._isHidden = false;
			this._curTimeBlink = this.TimeBlink;
			this._isDragLate = true;
		}
	}

	private void OnDrag(Vector2 delta)
	{
		delta /= Defs.Coef;
		this._isDrag = true;
		if (!this._isZone)
		{
			float single = base.transform.localPosition.x + delta.x;
			Vector3 vector3 = base.transform.localPosition;
			Vector3 vector31 = new Vector3(single, vector3.y + delta.y, 0f);
			base.transform.localPosition = vector31;
		}
		else
		{
			Vector3 zoneTopLeft = this.ZoneTopLeft.transform.localPosition;
			Vector3 zoneBottonRight = this.ZoneBottonRight.transform.localPosition;
			if (this._nonClampedPosition.HasValue)
			{
				float value = this._nonClampedPosition.Value.x + delta.x;
				Vector3 value1 = this._nonClampedPosition.Value;
				this._nonClampedPosition = new Vector3?(new Vector3(value, value1.y + delta.y, 0f));
				Vector3 value2 = this._nonClampedPosition.Value;
				float single1 = Mathf.Clamp(value2.x, zoneTopLeft.x, zoneBottonRight.x);
				Vector3 vector32 = this._nonClampedPosition.Value;
				float single2 = Mathf.Clamp(vector32.y, zoneBottonRight.y, zoneTopLeft.y);
				Vector3 value3 = this._nonClampedPosition.Value;
				Vector3 vector33 = new Vector3(single1, single2, value3.z);
				base.transform.localPosition = vector33;
			}
		}
	}

	private void OnPress(bool isDown)
	{
		this.IsPress = isDown;
		if (!isDown)
		{
			this._nonClampedPosition = null;
		}
		else
		{
			float single = base.transform.localPosition.x;
			Vector3 vector3 = base.transform.localPosition;
			this._nonClampedPosition = new Vector3?(new Vector3(single, vector3.y, 0f));
			EventHandler<EventArgs> pressedDown = ButtonJoystickAdjust.PressedDown;
			if (pressedDown != null)
			{
				pressedDown(base.gameObject, EventArgs.Empty);
			}
		}
	}

	public void SetJoystickPosition(Vector2 position)
	{
		base.transform.localPosition = position;
	}

	private void Start()
	{
		if (this.ZoneBottonRight == null || this.ZoneTopLeft == null)
		{
			this._isZone = false;
		}
	}
}