using Rilisoft;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LoadConnectScene : MonoBehaviour
{
	public static string sceneToLoad;

	public static Texture textureToShow;

	public static Texture noteToShow;

	public static float interval;

	public Texture loadingNote;

	private readonly static float _defaultInterval;

	private Texture loading;

	private LoadingNGUIController _loadingNGUIController;

	public static LoadConnectScene Instance;

	static LoadConnectScene()
	{
		LoadConnectScene.sceneToLoad = string.Empty;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.noteToShow = null;
		LoadConnectScene.interval = LoadConnectScene._defaultInterval;
		LoadConnectScene._defaultInterval = 1f;
	}

	public LoadConnectScene()
	{
	}

	[Obfuscation(Exclude=true)]
	private void _loadConnectScene()
	{
		if (!LoadConnectScene.sceneToLoad.Equals("ConnectScene"))
		{
			Singleton<SceneLoader>.Instance.LoadSceneAsync(LoadConnectScene.sceneToLoad, LoadSceneMode.Single);
		}
		else
		{
			Singleton<SceneLoader>.Instance.LoadScene(LoadConnectScene.sceneToLoad, LoadSceneMode.Single);
		}
	}

	private void Awake()
	{
		this.loading = LoadConnectScene.textureToShow;
		if (this.loading == null)
		{
			this.loading = Resources.Load<Texture>(ConnectSceneNGUIController.MainLoadingTexture());
		}
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = LoadConnectScene.sceneToLoad;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = this.loading;
		this._loadingNGUIController.Init();
		NotificationController.instance.SaveTimeValues();
	}

	public static void LoadScene()
	{
		if (LoadConnectScene.Instance == null)
		{
			return;
		}
		LoadConnectScene.Instance._loadConnectScene();
	}

	private void OnDestroy()
	{
		LoadConnectScene.Instance = null;
		if (!LoadConnectScene.sceneToLoad.Equals("ConnectScene"))
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
		LoadConnectScene.textureToShow = null;
	}

	private void OnGUI()
	{
		ActivityIndicator.IsActiveIndicator = true;
	}

	private void Start()
	{
		LoadConnectScene.Instance = this;
		if (LoadConnectScene.interval != -1f)
		{
			base.Invoke("_loadConnectScene", LoadConnectScene.interval);
		}
		LoadConnectScene.interval = LoadConnectScene._defaultInterval;
		ActivityIndicator.IsActiveIndicator = true;
	}
}