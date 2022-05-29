using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SceneInfo : MonoBehaviour
{
	[Header("Parametr map")]
	public int indexMap;

	public ModeWeapon AvaliableWeapon;

	public List<TypeModeGame> avaliableInModes = new List<TypeModeGame>();

	public string minAvaliableVersion = "0.0.0.0";

	public string maxAvaliableVersion = "0.0.0.0";

	public bool isPremium;

	public bool isPreloading;

	private bool _isLoaded;

	public InfoSizeMap sizeMap;

	[Header("Number key for translate")]
	public string keyTranslateName = string.Empty;

	private string transName = string.Empty;

	public string keyTranslateShortName = string.Empty;

	private string transShortName = string.Empty;

	private string transEngShortName = string.Empty;

	private string transSizeMap = string.Empty;

	[Header("Camera on start")]
	public Vector3 positionCam;

	public Vector3 rotationCam;

	public Sounds GetBackgroundSound
	{
		get
		{
			return null;
		}
	}

	public bool IsAvaliableVersion
	{
		get
		{
			return true;
		}
	}

	public bool IsLoaded
	{
		get
		{
			if (this._isLoaded)
			{
				return true;
			}
			this.UpdateKeyLoaded();
			return this._isLoaded;
		}
	}

	public string KeyTranslateSizeMap
	{
		get
		{
			switch (this.sizeMap)
			{
				case InfoSizeMap.small:
				{
					return "Key_0541";
				}
				case InfoSizeMap.normal:
				{
					return "Key_0539";
				}
				case InfoSizeMap.big:
				{
					return "Key_0538";
				}
				case InfoSizeMap.veryBig:
				{
					return "Key_0540";
				}
			}
			return string.Empty;
		}
	}

	public string NameScene
	{
		get
		{
			return base.gameObject.name;
		}
	}

	public string TranslateEngShortName
	{
		get
		{
			return this.transShortName;
		}
	}

	public string TranslateName
	{
		get
		{
			return this.transName;
		}
	}

	public string TranslatePreviewName
	{
		get
		{
			return this.transShortName;
		}
	}

	public string TranslateSizeMap
	{
		get
		{
			return this.transSizeMap;
		}
	}

	public SceneInfo()
	{
	}

	public void AddMode(TypeModeGame curMode)
	{
		for (int i = 0; i < this.avaliableInModes.Count; i++)
		{
			if (curMode == this.avaliableInModes[i])
			{
				return;
			}
		}
		this.avaliableInModes.Add(curMode);
	}

	[ContextMenu("Set Next Index")]
	private void DoSomething()
	{
		int num = 0;
		UnityEngine.Object[] objArray = Resources.LoadAll("SceneInfo");
		for (int i = 0; i < (int)objArray.Length; i++)
		{
			SceneInfo component = (objArray[i] as GameObject).GetComponent<SceneInfo>();
			if (component.indexMap > num)
			{
				num = component.indexMap;
			}
		}
		this.indexMap = num + 1;
	}

	public bool IsAvaliableForMode(TypeModeGame curMode)
	{
		if (this.IsAvaliableVersion && this.avaliableInModes != null && this.avaliableInModes.Count > 0)
		{
			for (int i = 0; i < this.avaliableInModes.Count; i++)
			{
				if (curMode == this.avaliableInModes[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SetStartPositionCamera(GameObject curCamObj)
	{
		if (curCamObj != null)
		{
			curCamObj.transform.position = this.positionCam;
			curCamObj.transform.eulerAngles = this.rotationCam;
		}
	}

	public void UpdateKeyLoaded()
	{
		if (!this.isPreloading)
		{
			this._isLoaded = true;
		}
		else
		{
			this._isLoaded = false;
		}
	}

	public void UpdateLocalize()
	{
		this.transName = LocalizationStore.Get(this.keyTranslateName);
		this.transShortName = LocalizationStore.Get(this.keyTranslateShortName);
		this.transSizeMap = LocalizationStore.Get(this.KeyTranslateSizeMap);
		this.transEngShortName = LocalizationStore.GetByDefault(this.keyTranslateShortName);
	}
}