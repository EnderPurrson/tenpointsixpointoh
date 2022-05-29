using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingInAfterGame : MonoBehaviour
{
	public static Texture loadingTexture;

	public static bool isShowLoading;

	private float timerShow = 2f;

	private LoadingNGUIController _loadingNGUIController;

	private bool ShouldShowLoading
	{
		get
		{
			return (this.timerShow <= 0f || LoadingInAfterGame.loadingTexture == null || !Defs.isMulti ? 1 : (int)Defs.isHunger) == 0;
		}
	}

	public LoadingInAfterGame()
	{
	}

	private void Awake()
	{
		if (this.ShouldShowLoading)
		{
			this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
			this._loadingNGUIController.SceneToLoad = SceneManager.GetActiveScene().name;
			this._loadingNGUIController.loadingNGUITexture.mainTexture = LoadingInAfterGame.loadingTexture;
			this._loadingNGUIController.transform.localPosition = Vector3.zero;
			this._loadingNGUIController.Init();
			LoadingInAfterGame.isShowLoading = true;
		}
	}

	private void OnDestroy()
	{
		LoadingInAfterGame.isShowLoading = false;
	}

	private void Update()
	{
		if (this.timerShow > 0f)
		{
			this.timerShow -= Time.deltaTime;
		}
		if (!ActivityIndicator.IsActiveIndicator)
		{
			ActivityIndicator.IsActiveIndicator = true;
		}
		if (!this.ShouldShowLoading)
		{
			LoadingInAfterGame.isShowLoading = false;
			base.enabled = false;
			LoadingInAfterGame.loadingTexture = null;
			ActivityIndicator.IsActiveIndicator = false;
			if (this._loadingNGUIController != null)
			{
				UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
				this._loadingNGUIController = null;
				Resources.UnloadUnusedAssets();
			}
		}
	}
}