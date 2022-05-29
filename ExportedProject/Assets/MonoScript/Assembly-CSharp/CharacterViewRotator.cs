using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterViewRotator : MonoBehaviour
{
	public CharacterView characterView;

	private Transform _character;

	private Quaternion _defaultLocalRotation;

	private float _toDefaultOrientationTime;

	private float _lastRotateTime;

	public CharacterViewRotator()
	{
	}

	private void Awake()
	{
		this._character = this.characterView.transform;
		this._defaultLocalRotation = this._character.localRotation;
	}

	private void OnDrag(Vector2 delta)
	{
		if (HOTween.IsTweening(this._character))
		{
			return;
		}
		this.RefreshToDefaultOrientationTime();
		float single = -30f;
		this._character.Rotate(Vector3.up, delta.x * single * (Time.realtimeSinceStartup - this._lastRotateTime));
		this._lastRotateTime = Time.realtimeSinceStartup;
	}

	private void OnDragStart()
	{
		this._lastRotateTime = Time.realtimeSinceStartup;
	}

	private void OnEnable()
	{
		this.ReturnCharacterToDefaultOrientation();
	}

	private void OnScroll(float delta)
	{
		this.OnDrag(new Vector2(-delta * 20f, 0f));
	}

	private void RefreshToDefaultOrientationTime()
	{
		this._toDefaultOrientationTime = Time.realtimeSinceStartup + ShopNGUIController.IdleTimeoutPers;
	}

	private void ReturnCharacterToDefaultOrientation()
	{
		HOTween.Kill(this._character);
		this.RefreshToDefaultOrientationTime();
		TweenParms tweenParm = (new TweenParms()).Prop("localRotation", new PlugQuaternion(this._defaultLocalRotation)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(() => this.RefreshToDefaultOrientationTime());
		HOTween.To(this._character, 0.5f, tweenParm);
	}

	private void Start()
	{
		this.ReturnCharacterToDefaultOrientation();
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup > this._toDefaultOrientationTime)
		{
			this.ReturnCharacterToDefaultOrientation();
		}
	}
}