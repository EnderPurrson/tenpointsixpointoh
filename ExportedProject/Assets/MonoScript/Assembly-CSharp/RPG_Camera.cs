using System;
using UnityEngine;

public class RPG_Camera : MonoBehaviour
{
	public static RPG_Camera instance;

	public Transform cameraPivot;

	public float TimeRotationCam = 15f;

	public float distance = 5f;

	public float distanceMin = 1f;

	public float distanceMax = 30f;

	public float mouseSpeed = 8f;

	public float mouseScroll = 15f;

	public float mouseSmoothingFactor = 0.08f;

	public float camDistanceSpeed = 0.7f;

	public float camBottomDistance = 1f;

	public float firstPersonThreshold = 0.8f;

	public float characterFadeThreshold = 1.8f;

	private float speedRotateXfree = 180f;

	public bool isDragging;

	private Vector3 desiredPosition;

	public float desiredDistance;

	public float offsetMaxDistance;

	public float offsetY;

	public float lastDistance;

	public float mouseX;

	public float deltaMouseX;

	private float mouseXSmooth;

	private float mouseXVel;

	public float mouseY;

	public float mouseYSmooth;

	private float mouseYVel;

	private float mouseYMin = -89.5f;

	private float mouseYMax = 89.5f;

	private float distanceVel;

	private bool camBottom;

	private bool constraint;

	private static float halfFieldOfView;

	private static float planeAspect;

	private static float halfPlaneHeight;

	private static float halfPlaneWidth;

	public Vector2 controlVector;

	public Vector3 curTargetEulerAngles;

	private bool enabledSledCamera = true;

	private bool isCameraIntersect;

	private RaycastHit hitInfo;

	[HideInInspector]
	public static Camera cam;

	private float nearClipPlaneNormal;

	public LayerMask collisionLayer;

	public RPG_Camera()
	{
	}

	private void Awake()
	{
		RPG_Camera.instance = this;
		RPG_Camera.cam = base.GetComponent<Camera>();
		this.nearClipPlaneNormal = RPG_Camera.cam.nearClipPlane;
	}

	public static void CameraSetup()
	{
		GameObject gameObject;
		if (RPG_Camera.cam == null)
		{
			gameObject = new GameObject("Main Camera");
			gameObject.AddComponent<Camera>();
			gameObject.tag = "MainCamera";
		}
		else
		{
			gameObject = RPG_Camera.cam.gameObject;
		}
		if (!gameObject.GetComponent("RPG_Camera"))
		{
			gameObject.AddComponent<RPG_Camera>();
		}
		RPG_Camera component = gameObject.GetComponent("RPG_Camera") as RPG_Camera;
		component.cameraPivot = GameObject.Find("cameraPivot").transform;
	}

	private void CharacterFade()
	{
		if (RPG_Animation.instance == null)
		{
			return;
		}
		if (this.distance < this.firstPersonThreshold)
		{
			RPG_Animation.instance.GetComponent<Renderer>().enabled = false;
		}
		else if (this.distance >= this.characterFadeThreshold)
		{
			RPG_Animation.instance.GetComponent<Renderer>().enabled = true;
			if (RPG_Animation.instance.GetComponent<Renderer>().material.color.a != 1f)
			{
				Material component = RPG_Animation.instance.GetComponent<Renderer>().material;
				float single = RPG_Animation.instance.GetComponent<Renderer>().material.color.r;
				float component1 = RPG_Animation.instance.GetComponent<Renderer>().material.color.g;
				Color color = RPG_Animation.instance.GetComponent<Renderer>().material.color;
				component.color = new Color(single, component1, color.b, 1f);
			}
		}
		else
		{
			RPG_Animation.instance.GetComponent<Renderer>().enabled = true;
			float single1 = 1f - (this.characterFadeThreshold - this.distance) / (this.characterFadeThreshold - this.firstPersonThreshold);
			if (RPG_Animation.instance.GetComponent<Renderer>().material.color.a != single1)
			{
				Material material = RPG_Animation.instance.GetComponent<Renderer>().material;
				float component2 = RPG_Animation.instance.GetComponent<Renderer>().material.color.r;
				float single2 = RPG_Animation.instance.GetComponent<Renderer>().material.color.g;
				Color color1 = RPG_Animation.instance.GetComponent<Renderer>().material.color;
				material.color = new Color(component2, single2, color1.b, single1);
			}
		}
	}

	private float CheckCameraClipPlane(Vector3 from, Vector3 to)
	{
		float single = -1f;
		RPG_Camera.ClipPlaneVertexes clipPlaneAt = this.GetClipPlaneAt(to);
		if (Physics.Linecast(from, to, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			single = this.hitInfo.distance - RPG_Camera.cam.nearClipPlane;
		}
		else if (Physics.Linecast((from - (base.transform.right * RPG_Camera.halfPlaneWidth)) + (base.transform.up * RPG_Camera.halfPlaneHeight), clipPlaneAt.UpperLeft, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			if (this.hitInfo.distance < single || single == -1f)
			{
				single = Vector3.Distance((this.hitInfo.point + (base.transform.right * RPG_Camera.halfPlaneWidth)) - (base.transform.up * RPG_Camera.halfPlaneHeight), from);
			}
		}
		else if (Physics.Linecast((from + (base.transform.right * RPG_Camera.halfPlaneWidth)) + (base.transform.up * RPG_Camera.halfPlaneHeight), clipPlaneAt.UpperRight, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			if (this.hitInfo.distance < single || single == -1f)
			{
				single = Vector3.Distance((this.hitInfo.point - (base.transform.right * RPG_Camera.halfPlaneWidth)) - (base.transform.up * RPG_Camera.halfPlaneHeight), from);
			}
		}
		else if (Physics.Linecast((from - (base.transform.right * RPG_Camera.halfPlaneWidth)) - (base.transform.up * RPG_Camera.halfPlaneHeight), clipPlaneAt.LowerLeft, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			if (this.hitInfo.distance < single || single == -1f)
			{
				single = Vector3.Distance((this.hitInfo.point + (base.transform.right * RPG_Camera.halfPlaneWidth)) + (base.transform.up * RPG_Camera.halfPlaneHeight), from);
			}
		}
		else if (Physics.Linecast((from + (base.transform.right * RPG_Camera.halfPlaneWidth)) - (base.transform.up * RPG_Camera.halfPlaneHeight), clipPlaneAt.LowerRight, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo) && (this.hitInfo.distance < single || single == -1f))
		{
			single = Vector3.Distance((this.hitInfo.point - (base.transform.right * RPG_Camera.halfPlaneWidth)) + (base.transform.up * RPG_Camera.halfPlaneHeight), from);
		}
		return single;
	}

	private float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f || angle > 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle <= 360f)
			{
				continue;
			}
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	private Vector3 GetCameraPosition(float xAxis, float yAxis, float distance)
	{
		Vector3 vector3 = new Vector3(0f, 0f, -distance);
		Quaternion quaternion = Quaternion.Euler(xAxis, yAxis, 0f);
		return this.cameraPivot.position + (quaternion * vector3);
	}

	public RPG_Camera.ClipPlaneVertexes GetClipPlaneAt(Vector3 pos)
	{
		RPG_Camera.ClipPlaneVertexes upperLeft = new RPG_Camera.ClipPlaneVertexes();
		if (RPG_Camera.cam == null)
		{
			return upperLeft;
		}
		Transform transforms = RPG_Camera.cam.transform;
		float single = RPG_Camera.cam.nearClipPlane;
		upperLeft.UpperLeft = pos - (transforms.right * RPG_Camera.halfPlaneWidth);
		upperLeft.UpperLeft = upperLeft.UpperLeft + (transforms.up * RPG_Camera.halfPlaneHeight);
		upperLeft.UpperLeft = upperLeft.UpperLeft + (transforms.forward * single);
		upperLeft.UpperRight = pos + (transforms.right * RPG_Camera.halfPlaneWidth);
		upperLeft.UpperRight = upperLeft.UpperRight + (transforms.up * RPG_Camera.halfPlaneHeight);
		upperLeft.UpperRight = upperLeft.UpperRight + (transforms.forward * single);
		upperLeft.LowerLeft = pos - (transforms.right * RPG_Camera.halfPlaneWidth);
		upperLeft.LowerLeft = upperLeft.LowerLeft - (transforms.up * RPG_Camera.halfPlaneHeight);
		upperLeft.LowerLeft = upperLeft.LowerLeft + (transforms.forward * single);
		upperLeft.LowerRight = pos + (transforms.right * RPG_Camera.halfPlaneWidth);
		upperLeft.LowerRight = upperLeft.LowerRight - (transforms.up * RPG_Camera.halfPlaneHeight);
		upperLeft.LowerRight = upperLeft.LowerRight + (transforms.forward * single);
		return upperLeft;
	}

	private void GetDesiredPosition()
	{
		this.distance = this.desiredDistance;
		this.desiredPosition = this.GetCameraPosition(this.mouseYSmooth, this.mouseX, this.distance);
		this.constraint = false;
		float single = this.CheckCameraClipPlane(this.cameraPivot.position, this.desiredPosition);
		if (single != -1f)
		{
			this.distance = single;
			this.desiredPosition = this.GetCameraPosition(this.mouseYSmooth, this.mouseX, this.distance);
			this.constraint = true;
		}
		this.distance -= RPG_Camera.cam.nearClipPlane;
		if (this.lastDistance < this.distance || !this.constraint)
		{
			this.distance = Mathf.SmoothDamp(this.lastDistance, this.distance, ref this.distanceVel, this.camDistanceSpeed);
		}
		if (this.distance < this.distanceMin)
		{
			this.distance = this.distanceMin;
		}
		this.lastDistance = this.distance;
		this.desiredPosition = this.GetCameraPosition(this.mouseYSmooth, this.mouseX, this.distance);
		if (this.distance >= 4f || !(this.hitInfo.normal != Vector3.zero))
		{
			this.isCameraIntersect = false;
			if (RPG_Camera.cam.nearClipPlane != this.nearClipPlaneNormal)
			{
				RPG_Camera.cam.nearClipPlane = this.nearClipPlaneNormal;
			}
		}
		else
		{
			RPG_Camera rPGCamera = this;
			rPGCamera.desiredPosition = rPGCamera.desiredPosition - (((this.hitInfo.normal * this.offsetY) * (4f - this.distance)) * 0.25f);
			this.isCameraIntersect = true;
		}
	}

	private void GetInput()
	{
		bool flag;
		if ((double)this.distance > 0.1)
		{
			this.camBottom = Physics.Linecast(base.transform.position, base.transform.position - (Vector3.up * this.camBottomDistance), this.collisionLayer);
		}
		if (!this.camBottom)
		{
			flag = false;
		}
		else
		{
			float single = base.transform.position.y;
			Vector3 vector3 = this.cameraPivot.transform.position;
			flag = single - vector3.y <= 0f;
		}
		bool flag1 = flag;
		this.mouseY = this.ClampAngle(this.mouseY, -89.5f, 89.5f);
		this.mouseXSmooth = Mathf.SmoothDamp(this.mouseXSmooth, this.mouseX, ref this.mouseXVel, this.mouseSmoothingFactor);
		this.mouseYSmooth = Mathf.SmoothDamp(this.mouseYSmooth, this.mouseY, ref this.mouseYVel, this.mouseSmoothingFactor);
		if (!flag1)
		{
			this.mouseYMin = -89.5f;
		}
		else
		{
			this.mouseYMin = this.mouseY;
		}
		this.mouseYSmooth = this.ClampAngle(this.mouseYSmooth, this.mouseYMin, this.mouseYMax);
		if (this.desiredDistance > this.distanceMax)
		{
			this.desiredDistance = this.distanceMax;
		}
		if (this.desiredDistance < this.distanceMin)
		{
			this.desiredDistance = this.distanceMin;
		}
		this.controlVector = Vector2.zero;
	}

	private bool IsIgnorCollider(RaycastHit curHitInfo)
	{
		if (curHitInfo.collider.tag != "Player" && curHitInfo.collider.tag != "Vision" && curHitInfo.collider.tag != "colliderPoint" && curHitInfo.collider.tag != "Helicopter")
		{
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		RPG_Camera.cam = null;
	}

	private void PositionUpdate()
	{
		base.transform.position = this.desiredPosition;
		if (this.distance > this.distanceMin)
		{
			base.transform.LookAt(this.cameraPivot);
			Transform vector3 = base.transform;
			vector3.eulerAngles = vector3.eulerAngles - new Vector3(2f, 0f, 0f);
		}
	}

	public void RotateWithCharacter()
	{
		float axis = Input.GetAxis("Horizontal") * RPG_Controller.instance.turnSpeed;
		this.mouseX += axis;
	}

	private void Start()
	{
		this.distance = Mathf.Clamp(this.distance, this.distanceMin, this.distanceMax);
		this.desiredDistance = this.distance;
		RPG_Camera.halfFieldOfView = RPG_Camera.cam.fieldOfView / 2f * 0.017453292f;
		RPG_Camera.planeAspect = RPG_Camera.cam.aspect;
		RPG_Camera.halfPlaneHeight = RPG_Camera.cam.nearClipPlane * Mathf.Tan(RPG_Camera.halfFieldOfView);
		RPG_Camera.halfPlaneWidth = RPG_Camera.halfPlaneHeight * RPG_Camera.planeAspect;
		this.mouseY = 15f;
	}

	private void Update()
	{
		if (this.cameraPivot == null)
		{
			return;
		}
		if (!RotatorKillCam.isDraggin && this.distance < 2f)
		{
			RPG_Camera rPGCamera = this;
			rPGCamera.deltaMouseX = rPGCamera.deltaMouseX + Time.deltaTime * this.speedRotateXfree;
		}
		this.curTargetEulerAngles = this.cameraPivot.eulerAngles;
		this.GetInput();
		this.GetDesiredPosition();
		this.PositionUpdate();
	}

	public void UpdateMouseX()
	{
		if (this.cameraPivot == null)
		{
			return;
		}
		float single = 150f + this.deltaMouseX;
		Vector3 vector3 = this.cameraPivot.rotation.eulerAngles;
		this.mouseX = single + vector3.y;
		while (this.mouseX > 360f)
		{
			this.mouseX -= 360f;
		}
	}

	public struct ClipPlaneVertexes
	{
		public Vector3 UpperLeft;

		public Vector3 UpperRight;

		public Vector3 LowerLeft;

		public Vector3 LowerRight;
	}
}