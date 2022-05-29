using Rilisoft;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

	private readonly static Queue<GameObject> _arrowPool;

	private readonly static Lazy<UnityEngine.Object> _arrowPrefab;

	private Transform _arrowToPortal;

	private readonly Lazy<Transform> _arrowToPortalPoint;

	private readonly Lazy<Camera> _camera;

	private Transform _poi;

	private Renderer[] _renderers;

	static PlayerArrowToPortalController()
	{
		PlayerArrowToPortalController._arrowPool = new Queue<GameObject>();
		PlayerArrowToPortalController._arrowPrefab = new Lazy<UnityEngine.Object>(() => Resources.Load("ArrowToPortal"));
	}

	public PlayerArrowToPortalController()
	{
		this._arrowToPortalPoint = new Lazy<Transform>(() => this.ArrowToPortalPoint.transform);
		this._camera = new Lazy<Camera>(() => this.ArrowToPortalPoint.transform.parent.GetComponent<Camera>());
	}

	public static void DisposeArrowToPool(GameObject arrow)
	{
		if (arrow == null)
		{
			return;
		}
		arrow.SetActive(false);
		PlayerArrowToPortalController._arrowPool.Enqueue(arrow);
	}

	public static GameObject GetArrowFromPool()
	{
		GameObject gameObject = null;
		while (PlayerArrowToPortalController._arrowPool.Count > 0 && gameObject == null)
		{
			gameObject = PlayerArrowToPortalController._arrowPool.Dequeue();
			if (gameObject != null)
			{
				continue;
			}
			Debug.LogWarning("Arrow pointer from pool is null.");
		}
		if (gameObject == null)
		{
			if (!PlayerArrowToPortalController.PopulateArrowPoolIfEmpty())
			{
				return null;
			}
			gameObject = PlayerArrowToPortalController._arrowPool.Dequeue();
		}
		Transform transforms = gameObject.transform;
		transforms.parent = null;
		transforms.localPosition = Vector3.zero;
		gameObject.SetActive(true);
		return gameObject;
	}

	public static bool PopulateArrowPoolIfEmpty()
	{
		if (PlayerArrowToPortalController._arrowPool.Count > 0)
		{
			return true;
		}
		if (PlayerArrowToPortalController._arrowPrefab.Value == null)
		{
			return false;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(PlayerArrowToPortalController._arrowPrefab.Value);
		gameObject.SetActive(false);
		Transform transforms = gameObject.transform;
		transforms.parent = null;
		transforms.localPosition = Vector3.zero;
		PlayerArrowToPortalController._arrowPool.Enqueue(gameObject);
		return true;
	}

	public void RemovePointOfInterest()
	{
		if (this._arrowToPortal == null)
		{
			return;
		}
		this._poi = null;
		PlayerArrowToPortalController.DisposeArrowToPool(this._arrowToPortal.gameObject);
		this._arrowToPortal = null;
		this._renderers = new Renderer[0];
	}

	public void SetPointOfInterest(Transform poi)
	{
		this.SetPointOfInterest(poi, Color.green);
	}

	public void SetPointOfInterest(Transform poi, Color color)
	{
		this._poi = poi;
		GameObject arrowFromPool = PlayerArrowToPortalController.GetArrowFromPool();
		if (arrowFromPool == null)
		{
			return;
		}
		this._arrowToPortal = arrowFromPool.transform;
		this._renderers = arrowFromPool.GetComponentsInChildren<Renderer>();
		this._arrowToPortal.parent = this._arrowToPortalPoint.Value;
		this._arrowToPortal.localPosition = Vector3.zero;
		if ((int)this._renderers.Length > 0)
		{
			Renderer renderer = this._renderers[0];
			if (renderer == null)
			{
				return;
			}
			Texture texture = null;
			texture = (color != Color.red ? this.greenTexture : this.redTexture);
			if (texture != null && !object.ReferenceEquals(texture, renderer.material.mainTexture))
			{
				renderer.material.mainTexture = texture;
			}
		}
	}

	private void Update()
	{
		if (this._poi != null && this._arrowToPortal != null)
		{
			float arrowHeight = this.ArrowHeight + 0.4f;
			float single = arrowHeight / Mathf.Tan(this._camera.Value.fieldOfView * 0.5f * 0.017453292f);
			Vector3 value = this._arrowToPortalPoint.Value.localPosition;
			value.y = this.ArrowHeight;
			value.z = single;
			this._arrowToPortalPoint.Value.localPosition = value;
			Vector3 vector3 = this._poi.position;
			vector3.y = 0f;
			Vector3 vector31 = this._arrowToPortal.position;
			vector31.y = 0f;
			Vector3 vector32 = vector3 - vector31;
			this._arrowToPortal.rotation = Quaternion.LookRotation(vector32);
			float single1 = Mathf.Clamp(this.ArrowAngle, 0f, 45f);
			Transform transforms = this._arrowToPortal;
			Vector3 vector33 = this._arrowToPortal.position;
			Vector3 value1 = this._arrowToPortalPoint.Value.parent.transform.right;
			Vector3 value2 = this._arrowToPortalPoint.Value.parent.rotation.eulerAngles;
			transforms.RotateAround(vector33, value1, value2.x + single1 - 0.5f * this._camera.Value.fieldOfView);
			Vector3 value3 = this._camera.Value.gameObject.transform.position;
			value3.y = 0f;
			float single2 = Vector3.SqrMagnitude(vector31 - value3);
			float single3 = Vector3.SqrMagnitude(vector3 - vector31);
			float single4 = Vector3.SqrMagnitude(vector3 - value3);
			float single5 = Mathf.Max(4f, 0.25f * single2);
			bool flag = (single3 < single5 ? true : single4 < single5);
			Renderer[] rendererArray = this._renderers;
			for (int i = 0; i < (int)rendererArray.Length; i++)
			{
				rendererArray[i].enabled = !flag;
			}
		}
	}
}