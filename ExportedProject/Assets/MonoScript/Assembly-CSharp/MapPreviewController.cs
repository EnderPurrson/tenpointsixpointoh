using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class MapPreviewController : MonoBehaviour
{
	private readonly string[] _ratingLabelsKeys = new string[] { "Key_0545", "Key_0546", "Key_0547", "Key_0548", "Key_0549", "Key_2183" };

	public UILabel NameMapLbl;

	public UILabel SizeMapNameLbl;

	public UILabel popularityLabel;

	public UISprite popularitySprite;

	public GameObject premium;

	public GameObject milee;

	public GameObject dater;

	public int mapID;

	public UITexture mapPreviewTexture;

	private MyCenterOnChild centerChild;

	[ReadOnly]
	[SerializeField]
	private int _ratingVal;

	private int _rating
	{
		get
		{
			return this._ratingVal;
		}
		set
		{
			this._ratingVal = value;
			if (this._ratingVal >= 0)
			{
				this.popularityLabel.text = LocalizationStore.Get(this._ratingLabelsKeys[this._ratingVal]);
				this.popularitySprite.spriteName = string.Format("Nb_Players_{0}", this._ratingVal);
				this.popularityLabel.gameObject.SetActive(true);
			}
			else
			{
				this.popularityLabel.gameObject.SetActive(false);
			}
		}
	}

	public MapPreviewController()
	{
	}

	[DebuggerHidden]
	public IEnumerator MyWaitForSeconds(float tm)
	{
		MapPreviewController.u003cMyWaitForSecondsu003ec__IteratorBA variable = null;
		return variable;
	}

	private void OnClick()
	{
		ConnectSceneNGUIController.sharedController.StopFingerAnim();
		if (this.centerChild.centeredObject != base.transform.gameObject)
		{
			this.centerChild.CenterOn(base.transform);
		}
		else if (!ConnectSceneNGUIController.sharedController.createPanel.activeSelf)
		{
			ConnectSceneNGUIController.sharedController.HandleGoBtnClicked(null, EventArgs.Empty);
		}
	}

	[DebuggerHidden]
	private IEnumerator SetPopularity()
	{
		MapPreviewController.u003cSetPopularityu003ec__IteratorB9 variable = null;
		return variable;
	}

	private void Start()
	{
		base.StartCoroutine(this.SetPopularity());
		this.centerChild = ConnectSceneNGUIController.sharedController.grid.GetComponent<MyCenterOnChild>();
	}
}