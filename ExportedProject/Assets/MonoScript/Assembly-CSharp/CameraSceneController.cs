using System;
using UnityEngine;

public sealed class CameraSceneController : MonoBehaviour
{
	public static CameraSceneController sharedController;

	private Vector3 posCam = new Vector3(17f, 11f, 17f);

	private Quaternion rotateCam = Quaternion.Euler(new Vector3(39f, 226f, 0f));

	private Transform myTransform;

	public RPG_Camera killCamController;

	public Transform objListener;

	public bool EnableSounds
	{
		get
		{
			return this.objListener.localPosition.Equals(Vector3.zero);
		}
		set
		{
			if (!value)
			{
				this.objListener.localPosition = new Vector3(0f, 10000f, 0f);
			}
			else
			{
				this.objListener.localPosition = Vector3.zero;
			}
		}
	}

	public CameraSceneController()
	{
	}

	private void Awake()
	{
		CameraSceneController.sharedController = this;
		this.myTransform = base.transform;
		this.EnableSounds = false;
	}

	private void OnDestroy()
	{
		CameraSceneController.sharedController = null;
	}

	public void SetTargetKillCam(Transform target = null)
	{
		if (target != null)
		{
			this.killCamController.enabled = true;
			this.killCamController.cameraPivot = target;
			this.myTransform.position = target.position;
			this.myTransform.rotation = target.rotation;
			this.EnableSounds = true;
		}
		else
		{
			this.killCamController.enabled = false;
			this.killCamController.cameraPivot = null;
			this.myTransform.position = this.posCam;
			this.myTransform.rotation = this.rotateCam;
			this.EnableSounds = false;
		}
	}

	private void Start()
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(Application.loadedLevelName);
		if (infoScene != null)
		{
			this.posCam = infoScene.positionCam;
			this.rotateCam = Quaternion.Euler(infoScene.rotationCam);
		}
		this.myTransform.position = this.posCam;
		this.myTransform.rotation = this.rotateCam;
		this.killCamController.enabled = false;
	}
}