using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

public class CharacterViewRotator : MonoBehaviour
{
	public CharacterView characterView;

	private Transform _character;

	private Quaternion _defaultLocalRotation;

	private float _toDefaultOrientationTime;

	private float _lastRotateTime;

	private void Awake()
	{
		_character = characterView.transform;
		_defaultLocalRotation = _character.localRotation;
	}

	private void Start()
	{
		ReturnCharacterToDefaultOrientation();
	}

	private void OnEnable()
	{
		ReturnCharacterToDefaultOrientation();
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup > _toDefaultOrientationTime)
		{
			ReturnCharacterToDefaultOrientation();
		}
	}

	private void OnDragStart()
	{
		_lastRotateTime = Time.realtimeSinceStartup;
	}

	private void OnDrag(Vector2 delta)
	{
		if (!HOTween.IsTweening((object)_character))
		{
			RefreshToDefaultOrientationTime();
			float num = -30f;
			_character.Rotate(Vector3.up, delta.x * num * (Time.realtimeSinceStartup - _lastRotateTime));
			_lastRotateTime = Time.realtimeSinceStartup;
		}
	}

	private void OnScroll(float delta)
	{
		OnDrag(new Vector2((0f - delta) * 20f, 0f));
	}

	private void RefreshToDefaultOrientationTime()
	{
		_toDefaultOrientationTime = Time.realtimeSinceStartup + ShopNGUIController.IdleTimeoutPers;
	}

	private void ReturnCharacterToDefaultOrientation()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		int num = HOTween.Kill((object)_character);
		RefreshToDefaultOrientationTime();
		TweenParms val = new TweenParms().Prop("localRotation", (ABSTweenPlugin)new PlugQuaternion(_defaultLocalRotation)).UpdateType((UpdateType)3).Ease((EaseType)0)
			.OnComplete((TweenCallback)delegate
			{
				RefreshToDefaultOrientationTime();
			});
		HOTween.To((object)_character, 0.5f, val);
	}
}
