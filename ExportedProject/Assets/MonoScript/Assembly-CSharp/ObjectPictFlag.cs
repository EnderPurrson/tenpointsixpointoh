using System;
using UnityEngine;

internal sealed class ObjectPictFlag : MonoBehaviour
{
	public Camera cameraToUse;

	public Camera cam;

	public Transform target;

	public Vector3 posLabel;

	private Transform camTransform;

	public bool isBaza;

	public FlagController myFlagController;

	public ObjectPictFlag()
	{
	}

	public void SetTexture(Texture _texture)
	{
		base.GetComponent<GUITexture>().texture = _texture;
	}

	private void Update()
	{
		try
		{
			this.cam = NickLabelController.currentCamera;
			if (this.cam != null)
			{
				this.camTransform = this.cam.transform;
			}
			if (this.target == null || this.cam == null)
			{
				if (Time.frameCount % 60 == 0)
				{
					Debug.Log("target == null");
				}
				base.transform.position = new Vector3(-1000f, -1000f, -1000f);
			}
			else
			{
				this.posLabel = this.cam.WorldToViewportPoint(this.target.position);
				if (this.posLabel.z < 0f || !(ShopNGUIController.sharedShop != null) || ShopNGUIController.GuiActive)
				{
					base.transform.position = new Vector3(-1000f, -1000f, -1000f);
				}
				else
				{
					base.transform.position = this.posLabel;
				}
				if (this.isBaza && this.myFlagController.isBaza && this.myFlagController.flagModel.activeInHierarchy)
				{
					base.transform.position = new Vector3(-1000f, -1000f, -1000f);
				}
				if (!this.isBaza && !this.target.parent.GetComponent<FlagController>().flagModel.activeInHierarchy)
				{
					base.transform.position = new Vector3(-1000f, -1000f, -1000f);
				}
			}
		}
		catch (Exception exception)
		{
			Debug.Log(string.Concat("Exception in ObjectLabel: ", exception));
		}
	}
}