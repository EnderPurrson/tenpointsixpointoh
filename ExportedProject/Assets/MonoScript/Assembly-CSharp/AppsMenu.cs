using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obfuscation(Exclude=true)]
internal sealed class AppsMenu : MonoBehaviour
{
	private const string _suffix = "Scene";

	public Texture androidFon;

	public Texture riliFon;

	public Texture commicsFon;

	public Material fadeMaterial;

	public GameObject activityIndikatorPrefab;

	public string intendedSignatureHash;

	private Texture currentFon;

	private static Material m_Material;

	private static int _startFrameIndex;

	internal volatile object _preventAggressiveOptimisation;

	private static volatile uint _preventInlining;

	private IDisposable _backSubscription;

	private Lazy<string> _expansionFilePath = new Lazy<string>(new Func<string>(GooglePlayDownloader.GetExpansionFilePath));

	private readonly TaskCompletionSource<bool> _storagePermissionGrantedPromise = new TaskCompletionSource<bool>();

	private bool _storagePermissionRequested;

	private TaskCompletionSource<string> _fetchObbPromise;

	internal static bool ApplicationBinarySplitted
	{
		get
		{
			return true;
		}
	}

	private Task<string> FetchObbFuture
	{
		get
		{
			if (this._fetchObbPromise == null)
			{
				return null;
			}
			return this._fetchObbPromise.Task;
		}
	}

	private Task<bool> StoragePermissionFuture
	{
		get
		{
			return this._storagePermissionGrantedPromise.Task;
		}
	}

	static AppsMenu()
	{
		AppsMenu._preventInlining = -729383235;
	}

	public AppsMenu()
	{
	}

	[DebuggerHidden]
	internal IEnumerable<float> AppsMenuAwakeCoroutine()
	{
		AppsMenu.u003cAppsMenuAwakeCoroutineu003ec__Iterator3 variable = null;
		return variable;
	}

	private void Awake()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted && Storager.getInt("ShopNGUIController.TrainingShopStageStepKey", false) == 6)
		{
			TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShopCompleted;
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Back_Shop, true);
		}
		this.currentFon = this.riliFon;
		if (ActivityIndicator.instance == null && this.activityIndikatorPrefab != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate<GameObject>(this.activityIndikatorPrefab));
		}
		ActivityIndicator.SetLoadingFon(this.currentFon);
		ActivityIndicator.IsShowWindowLoading = true;
	}

	private static void CheckRenameOldLanguageName()
	{
		int num;
		if (Storager.IsInitialized(Defs.ChangeOldLanguageName))
		{
			return;
		}
		Storager.SetInitialized(Defs.ChangeOldLanguageName);
		string str = PlayerPrefs.GetString(Defs.CurrentLanguage, string.Empty);
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		string str1 = str;
		if (str1 != null)
		{
			if (AppsMenu.u003cu003ef__switchu0024map0 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(4)
				{
					{ "Français", 0 },
					{ "Deutsch", 1 },
					{ "日本人", 2 },
					{ "Español", 3 }
				};
				AppsMenu.u003cu003ef__switchu0024map0 = strs;
			}
			if (AppsMenu.u003cu003ef__switchu0024map0.TryGetValue(str1, out num))
			{
				switch (num)
				{
					case 0:
					{
						PlayerPrefs.SetString(Defs.CurrentLanguage, "French");
						PlayerPrefs.Save();
						break;
					}
					case 1:
					{
						PlayerPrefs.SetString(Defs.CurrentLanguage, "German");
						PlayerPrefs.Save();
						break;
					}
					case 2:
					{
						PlayerPrefs.SetString(Defs.CurrentLanguage, "Japanese");
						PlayerPrefs.Save();
						break;
					}
					case 3:
					{
						PlayerPrefs.SetString(Defs.CurrentLanguage, "Spanish");
						PlayerPrefs.Save();
						break;
					}
				}
			}
		}
	}

	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		if (!(AppsMenu.m_Material != null) || !AppsMenu.m_Material.SetPass(0))
		{
			UnityEngine.Debug.LogWarning("Couldnot set pass for material.");
		}
		else
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(7);
			GL.Color(aColor);
			GL.Vertex3(0f, 0f, -1f);
			GL.Vertex3(0f, 1f, -1f);
			GL.Vertex3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, -1f);
			GL.End();
			GL.PopMatrix();
		}
	}

	[DebuggerHidden]
	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime)
	{
		AppsMenu.u003cFadeu003ec__Iterator7 variable = null;
		return variable;
	}

	private static string GetAbuseKey_21493d18(uint pad)
	{
		uint num = -442282975 ^ pad;
		AppsMenu._preventInlining++;
		return num.ToString("x");
	}

	private static string GetAbuseKey_53232de5(uint pad)
	{
		uint num = -1748411172 ^ pad;
		AppsMenu._preventInlining++;
		return num.ToString("x");
	}

	private static string GetTerminalSceneName_4de1(uint gamma)
	{
		return "Closing4de1Scene".Replace(gamma.ToString("x"), string.Empty);
	}

	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		UnityEngine.Debug.LogFormat("GameThrive HandleNotification('{0}', ..., {1})", new object[] { message, isActive });
	}

	private void HandleStoragePermissionDialog(bool permissionGranted)
	{
		this._storagePermissionGrantedPromise.TrySetResult(permissionGranted);
		NoodlePermissionGranter.PermissionRequestCallback = null;
	}

	private void LoadLoading()
	{
		GlobalGameController.currentLevel = -1;
		SceneManager.LoadSceneAsync("Loading");
	}

	[DebuggerHidden]
	private IEnumerator LoadLoadingScene()
	{
		return new AppsMenu.u003cLoadLoadingSceneu003ec__Iterator8();
	}

	[DebuggerHidden]
	private static IEnumerator MeetTheCoroutine(string sceneName, long abuseTicks, long nowTicks)
	{
		AppsMenu.u003cMeetTheCoroutineu003ec__Iterator4 variable = null;
		return variable;
	}

	[DebuggerHidden]
	private IEnumerator OnApplicationPause(bool pause)
	{
		AppsMenu.u003cOnApplicationPauseu003ec__Iterator6 variable = null;
		return variable;
	}

	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(Application.Quit), "AppsMenu");
	}

	private void OnGUI()
	{
		if (Launcher.UsingNewLauncher)
		{
			return;
		}
		if (Application.isEditor || GooglePlayDownloader.RunningOnAndroid())
		{
			return;
		}
		GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use GooglePlayDownloader only on Android device!");
	}

	public static Rect RiliFonRect()
	{
		float single = (float)Screen.height * 1.7766234f;
		return new Rect((float)Screen.width / 2f - single / 2f, 0f, single, (float)Screen.height);
	}

	internal static void SetCurrentLanguage()
	{
		AppsMenu.CheckRenameOldLanguageName();
		string str = PlayerPrefs.GetString(Defs.CurrentLanguage);
		if (string.IsNullOrEmpty(str))
		{
			str = LocalizationStore.CurrentLanguage;
			return;
		}
		LocalizationStore.CurrentLanguage = str;
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		AppsMenu.u003cStartu003ec__Iterator5 variable = null;
		return variable;
	}
}