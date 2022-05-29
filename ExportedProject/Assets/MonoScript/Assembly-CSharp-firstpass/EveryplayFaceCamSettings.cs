using System;
using UnityEngine;

public class EveryplayFaceCamSettings : MonoBehaviour
{
	public bool previewVisible = true;

	public int iPhonePreviewSideWidth = 64;

	public int iPhonePreviewPositionX = 16;

	public int iPhonePreviewPositionY = 16;

	public int iPhonePreviewBorderWidth = 2;

	public int iPadPreviewSideWidth = 96;

	public int iPadPreviewPositionX = 24;

	public int iPadPreviewPositionY = 24;

	public int iPadPreviewBorderWidth = 2;

	public Color previewBorderColor = Color.white;

	public Everyplay.FaceCamPreviewOrigin previewOrigin = Everyplay.FaceCamPreviewOrigin.BottomRight;

	public bool previewScaleRetina = true;

	public bool audioOnly;

	public EveryplayFaceCamSettings()
	{
	}

	private void Start()
	{
		if (Everyplay.GetUserInterfaceIdiom() != 1)
		{
			Everyplay.FaceCamSetPreviewSideWidth(this.iPhonePreviewSideWidth);
			Everyplay.FaceCamSetPreviewBorderWidth(this.iPhonePreviewBorderWidth);
			Everyplay.FaceCamSetPreviewPositionX(this.iPhonePreviewPositionX);
			Everyplay.FaceCamSetPreviewPositionY(this.iPhonePreviewPositionY);
		}
		else
		{
			Everyplay.FaceCamSetPreviewSideWidth(this.iPadPreviewSideWidth);
			Everyplay.FaceCamSetPreviewBorderWidth(this.iPadPreviewBorderWidth);
			Everyplay.FaceCamSetPreviewPositionX(this.iPadPreviewPositionX);
			Everyplay.FaceCamSetPreviewPositionY(this.iPadPreviewPositionY);
		}
		Everyplay.FaceCamSetPreviewBorderColor(this.previewBorderColor.r, this.previewBorderColor.g, this.previewBorderColor.b, this.previewBorderColor.a);
		Everyplay.FaceCamSetPreviewOrigin(this.previewOrigin);
		Everyplay.FaceCamSetPreviewScaleRetina(this.previewScaleRetina);
		Everyplay.FaceCamSetPreviewVisible(this.previewVisible);
		Everyplay.FaceCamSetAudioOnly(this.audioOnly);
	}
}