using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

internal sealed class PlayerArrowToPortalController : MonoBehaviour
{
	public GameObject ArrowToPortalPoint;

	[Range(0f, 45f)]
	public float ArrowAngle = 30f;

	[Range(0f, 4f)]
	public float ArrowHeight = 1.5f;

	[SerializeField]
	private Texture greenTexture;

	[SerializeField]
	private Texture redTexture;

	private static readonly Queue<GameObject> _arrowPool = new Queue<GameObject>();

	private static readonly Lazy<UnityEngine.Object> _arrowPrefab;

	private Transform _arrowToPortal;

	private readonly Lazy<Transform> _arrowToPortalPoint;

	private readonly Lazy<Camera> _camera;

	private Transform _poi;

	private Renderer[] _renderers;

	[CompilerGenerated]
	private static Func<UnityEngine.Object> _003C_003Ef__am_0024cacheC;

	public PlayerArrowToPortalController()
	{
		_arrowToPortalPoint = new Lazy<Transform>(_003CPlayerArrowToPortalController_003Em__1FA);
		_camera = new Lazy<Camera>(_003CPlayerArrowToPortalController_003Em__1FB);
	}

	static PlayerArrowToPortalController()
	{
		if (_003C_003Ef__am_0024cacheC == null)
		{
			_003C_003Ef__am_0024cacheC = _003C_arrowPrefab_003Em__1FC;
		}
		_arrowPrefab = new Lazy<UnityEngine.Object>(_003C_003Ef__am_0024cacheC);
	}

	private void Update()
	{
		if (_poi != null && _arrowToPortal != null)
		{
			float num = ArrowHeight + 0.4f;
			float z = num / Mathf.Tan(_camera.Value.fieldOfView * 0.5f * ((float)Math.PI / 180f));
			Vector3 localPosition = _arrowToPortalPoint.Value.localPosition;
			localPosition.y = ArrowHeight;
			localPosition.z = z;
			_arrowToPortalPoint.Value.localPosition = localPosition;
			Vector3 position = _poi.position;
			position.y = 0f;
			Vector3 position2 = _arrowToPortal.position;
			position2.y = 0f;
			Vector3 forward = position - position2;
			_arrowToPortal.rotation = Quaternion.LookRotation(forward);
			float num2 = Mathf.Clamp(ArrowAngle, 0f, 45f);
			_arrowToPortal.RotateAround(_arrowToPortal.position, _arrowToPortalPoint.Value.parent.transform.right, _arrowToPortalPoint.Value.parent.rotation.eulerAngles.x + num2 - 0.5f * _camera.Value.fieldOfView);
			Vector3 position3 = _camera.Value.gameObject.transform.position;
			position3.y = 0f;
			float num3 = Vector3.SqrMagnitude(position2 - position3);
			float num4 = Vector3.SqrMagnitude(position - position2);
			float num5 = Vector3.SqrMagnitude(position - position3);
			float num6 = Mathf.Max(4f, 0.25f * num3);
			bool flag = num4 < num6 || num5 < num6;
			Renderer[] renderers = _renderers;
			foreach (Renderer renderer in renderers)
			{
				renderer.enabled = !flag;
			}
		}
	}

	public void SetPointOfInterest(Transform poi)
	{
		SetPointOfInterest(poi, Color.green);
	}

	public void SetPointOfInterest(Transform poi, Color color)
	{
		_poi = poi;
		GameObject arrowFromPool = GetArrowFromPool();
		if (arrowFromPool == null)
		{
			return;
		}
		_arrowToPortal = arrowFromPool.transform;
		_renderers = arrowFromPool.GetComponentsInChildren<Renderer>();
		_arrowToPortal.parent = _arrowToPortalPoint.Value;
		_arrowToPortal.localPosition = Vector3.zero;
		if (_renderers.Length <= 0)
		{
			return;
		}
		Renderer renderer = _renderers[0];
		if (!(renderer == null))
		{
			Texture texture = null;
			texture = ((!(color == Color.red)) ? greenTexture : redTexture);
			if (texture != null && !object.ReferenceEquals(texture, renderer.material.mainTexture))
			{
				renderer.material.mainTexture = texture;
			}
		}
	}

	public void RemovePointOfInterest()
	{
		if (!(_arrowToPortal == null))
		{
			_poi = null;
			DisposeArrowToPool(_arrowToPortal.gameObject);
			_arrowToPortal = null;
			_renderers = new Renderer[0];
		}
	}

	public static bool PopulateArrowPoolIfEmpty()
	{
		if (_arrowPool.Count > 0)
		{
			return true;
		}
		if (_arrowPrefab.Value == null)
		{
			return false;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(_arrowPrefab.Value);
		gameObject.SetActive(false);
		Transform transform = gameObject.transform;
		transform.parent = null;
		transform.localPosition = Vector3.zero;
		_arrowPool.Enqueue(gameObject);
		return true;
	}

	public static GameObject GetArrowFromPool()
	{
		GameObject gameObject = null;
		while (_arrowPool.Count > 0 && gameObject == null)
		{
			gameObject = _arrowPool.Dequeue();
			if (gameObject == null)
			{
				Debug.LogWarning("Arrow pointer from pool is null.");
			}
		}
		if (gameObject == null)
		{
			if (!PopulateArrowPoolIfEmpty())
			{
				return null;
			}
			gameObject = _arrowPool.Dequeue();
		}
		Transform transform = gameObject.transform;
		transform.parent = null;
		transform.localPosition = Vector3.zero;
		gameObject.SetActive(true);
		return gameObject;
	}

	public static void DisposeArrowToPool(GameObject arrow)
	{
		if (!(arrow == null))
		{
			arrow.SetActive(false);
			_arrowPool.Enqueue(arrow);
		}
	}

	[CompilerGenerated]
	private Transform _003CPlayerArrowToPortalController_003Em__1FA()
	{
		return ArrowToPortalPoint.transform;
	}

	[CompilerGenerated]
	private Camera _003CPlayerArrowToPortalController_003Em__1FB()
	{
		return ArrowToPortalPoint.transform.parent.GetComponent<Camera>();
	}

	[CompilerGenerated]
	private static UnityEngine.Object _003C_arrowPrefab_003Em__1FC()
	{
		return Resources.Load("ArrowToPortal");
	}
}
