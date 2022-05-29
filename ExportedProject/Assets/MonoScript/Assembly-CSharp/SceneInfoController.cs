using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneInfoController : MonoBehaviour
{
	private const float timerUpdateDataFromServer = 870f;

	public static SceneInfoController instance;

	public List<SceneInfo> allScenes = new List<SceneInfo>();

	public List<AllScenesForMode> modeInfo = new List<AllScenesForMode>();

	private bool _isLoadingDataActive;

	private List<SceneInfo> copyAllScenes;

	private List<AllScenesForMode> copyModeInfo;

	private readonly static Dictionary<TypeModeGame, int> _modeUnlockLevels;

	private readonly static Dictionary<TypeModeGame, ConnectSceneNGUIController.RegimGame> _modesMap;

	private Version CurrentVersion
	{
		get
		{
			return base.GetType().Assembly.GetName().Version;
		}
	}

	public static string UrlForLoadData
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.WP8Player)
				{
					return "https://secure.pixelgunserver.com/infomap_pixelgun_wp8.json";
				}
				return string.Empty;
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_android.json";
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_amazon.json";
			}
			return string.Empty;
		}
	}

	static SceneInfoController()
	{
		SceneInfoController.instance = null;
		Dictionary<TypeModeGame, int> typeModeGames = new Dictionary<TypeModeGame, int>()
		{
			{ TypeModeGame.Deathmatch, 1 },
			{ TypeModeGame.TeamFight, 2 },
			{ TypeModeGame.TimeBattle, 3 },
			{ TypeModeGame.FlagCapture, 4 },
			{ TypeModeGame.DeadlyGames, 5 },
			{ TypeModeGame.CapturePoints, 6 }
		};
		SceneInfoController._modeUnlockLevels = typeModeGames;
		Dictionary<TypeModeGame, ConnectSceneNGUIController.RegimGame> typeModeGames1 = new Dictionary<TypeModeGame, ConnectSceneNGUIController.RegimGame>()
		{
			{ TypeModeGame.Deathmatch, ConnectSceneNGUIController.RegimGame.Deathmatch },
			{ TypeModeGame.TeamFight, ConnectSceneNGUIController.RegimGame.TeamFight },
			{ TypeModeGame.TimeBattle, ConnectSceneNGUIController.RegimGame.TimeBattle },
			{ TypeModeGame.FlagCapture, ConnectSceneNGUIController.RegimGame.FlagCapture },
			{ TypeModeGame.DeadlyGames, ConnectSceneNGUIController.RegimGame.DeadlyGames },
			{ TypeModeGame.CapturePoints, ConnectSceneNGUIController.RegimGame.CapturePoints }
		};
		SceneInfoController._modesMap = typeModeGames1;
	}

	public SceneInfoController()
	{
	}

	private void AddSceneIfAvaliableVersion(string nameScene, string minVersion, string maxVersion)
	{
		if (this.GetInfoScene(nameScene, this.copyAllScenes) == null)
		{
			Version currentVersion = this.CurrentVersion;
			Version version = new Version(maxVersion);
			if (currentVersion >= new Version(minVersion) && currentVersion <= version)
			{
				GameObject gameObject = Resources.Load(string.Concat("SceneInfo/", nameScene)) as GameObject;
				SceneInfo component = gameObject.GetComponent<SceneInfo>();
				GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(component.gameObject);
				gameObject1.transform.SetParent(base.transform);
				gameObject1.gameObject.name = nameScene;
				component = gameObject1.GetComponent<SceneInfo>();
				component.minAvaliableVersion = minVersion;
				component.maxAvaliableVersion = maxVersion;
				component.UpdateKeyLoaded();
				this.copyAllScenes.Add(component);
			}
		}
	}

	private void AddSceneInModeGame(string nameScene, TypeModeGame needMode)
	{
		SceneInfo infoScene = this.GetInfoScene(nameScene, this.copyAllScenes);
		if (infoScene != null)
		{
			infoScene.AddMode(needMode);
			if (infoScene.IsLoaded)
			{
				AllScenesForMode listScenesForMode = this.GetListScenesForMode(needMode, this.copyModeInfo);
				if (listScenesForMode == null)
				{
					listScenesForMode = new AllScenesForMode()
					{
						mode = needMode
					};
					this.copyModeInfo.Add(listScenesForMode);
				}
				listScenesForMode.AddInfoScene(infoScene);
			}
		}
	}

	private void Awake()
	{
		SceneInfoController.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ExperienceController.onLevelChange += new Action(this.UpdateListAvaliableMap);
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.OnChangeLocalize));
		this.UpdateListAvaliableMap();
	}

	public static TypeModeGame ConvertModeToEnum(string modeStr)
	{
		int num;
		string str = modeStr;
		if (str != null)
		{
			if (SceneInfoController.u003cu003ef__switchu0024map10 == null)
			{
				Dictionary<string, int> strs = new Dictionary<string, int>(7)
				{
					{ "Deathmatch", 0 },
					{ "TimeBattle", 1 },
					{ "TeamFight", 2 },
					{ "DeadlyGames", 3 },
					{ "FlagCapture", 4 },
					{ "CapturePoints", 5 },
					{ "Dater", 6 }
				};
				SceneInfoController.u003cu003ef__switchu0024map10 = strs;
			}
			if (SceneInfoController.u003cu003ef__switchu0024map10.TryGetValue(str, out num))
			{
				switch (num)
				{
					case 0:
					{
						return TypeModeGame.Deathmatch;
					}
					case 1:
					{
						return TypeModeGame.TimeBattle;
					}
					case 2:
					{
						return TypeModeGame.TeamFight;
					}
					case 3:
					{
						return TypeModeGame.DeadlyGames;
					}
					case 4:
					{
						return TypeModeGame.FlagCapture;
					}
					case 5:
					{
						return TypeModeGame.CapturePoints;
					}
					case 6:
					{
						return TypeModeGame.Dater;
					}
				}
			}
		}
		return TypeModeGame.Deathmatch;
	}

	[DebuggerHidden]
	private IEnumerator DownloadDataFormServer()
	{
		SceneInfoController.u003cDownloadDataFormServeru003ec__Iterator18B variable = null;
		return variable;
	}

	public int GetCountScenesForMode(TypeModeGame needMode)
	{
		AllScenesForMode allScenesForMode = this.modeInfo.Find((AllScenesForMode nM) => nM.mode == needMode);
		if (allScenesForMode == null)
		{
			return 0;
		}
		return allScenesForMode.avaliableScenes.Count;
	}

	[DebuggerHidden]
	private IEnumerator GetDataFromServerLoop()
	{
		SceneInfoController.u003cGetDataFromServerLoopu003ec__Iterator18A variable = null;
		return variable;
	}

	public SceneInfo GetInfoScene(string nameScene)
	{
		return this.allScenes.Find((SceneInfo curInf) => curInf.gameObject.name.ToLower() == nameScene.ToLower());
	}

	public SceneInfo GetInfoScene(string nameScene, List<SceneInfo> needList)
	{
		if (needList == null)
		{
			return null;
		}
		return needList.Find((SceneInfo curInf) => curInf.gameObject.name.ToLower() == nameScene.ToLower());
	}

	public SceneInfo GetInfoScene(TypeModeGame needMode, int indexMap)
	{
		SceneInfo infoScene = this.GetInfoScene(indexMap);
		if (infoScene != null && infoScene.IsAvaliableForMode(needMode))
		{
			return infoScene;
		}
		return null;
	}

	public SceneInfo GetInfoScene(int indexMap)
	{
		return this.allScenes.Find((SceneInfo curInf) => curInf.indexMap == indexMap);
	}

	public AllScenesForMode GetListScenesForMode(TypeModeGame needMode)
	{
		return this.modeInfo.Find((AllScenesForMode mG) => mG.mode == needMode);
	}

	public AllScenesForMode GetListScenesForMode(TypeModeGame needMode, List<AllScenesForMode> needList)
	{
		if (needList == null)
		{
			return null;
		}
		return needList.Find((AllScenesForMode mG) => mG.mode == needMode);
	}

	internal static HashSet<TypeModeGame> GetUnlockedModesByLevel(int level)
	{
		HashSet<TypeModeGame> typeModeGames = new HashSet<TypeModeGame>();
		foreach (KeyValuePair<TypeModeGame, int> _modeUnlockLevel in SceneInfoController._modeUnlockLevels)
		{
			if (_modeUnlockLevel.Value > level)
			{
				continue;
			}
			typeModeGames.Add(_modeUnlockLevel.Key);
		}
		return typeModeGames;
	}

	public bool MapExistInProject(string nameScene)
	{
		return true;
	}

	private void OnChangeLocalize()
	{
		for (int i = 0; i < this.allScenes.Count; i++)
		{
			this.allScenes[i].UpdateLocalize();
		}
	}

	private void OnDataLoaded()
	{
		this.allScenes = this.copyAllScenes;
		this.modeInfo = this.copyModeInfo;
		this.OnChangeLocalize();
		if (SceneInfoController.onChangeInfoMap != null)
		{
			SceneInfoController.onChangeInfoMap();
		}
		this._isLoadingDataActive = false;
	}

	private void OnDestroy()
	{
		ExperienceController.onLevelChange -= new Action(this.UpdateListAvaliableMap);
		SceneInfoController.instance = null;
	}

	[DebuggerHidden]
	private IEnumerator ParseLoadData(string lData)
	{
		SceneInfoController.u003cParseLoadDatau003ec__Iterator18C variable = null;
		return variable;
	}

	internal static HashSet<ConnectSceneNGUIController.RegimGame> SelectModes(IEnumerable<TypeModeGame> modes)
	{
		ConnectSceneNGUIController.RegimGame regimGame;
		HashSet<ConnectSceneNGUIController.RegimGame> regimGames = new HashSet<ConnectSceneNGUIController.RegimGame>();
		IEnumerator<TypeModeGame> enumerator = modes.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				TypeModeGame current = enumerator.Current;
				if (!SceneInfoController._modesMap.TryGetValue(current, out regimGame))
				{
					continue;
				}
				regimGames.Add(regimGame);
			}
		}
		finally
		{
			if (enumerator == null)
			{
			}
			enumerator.Dispose();
		}
		return regimGames;
	}

	public void UpdateListAvaliableMap()
	{
		if (!this._isLoadingDataActive)
		{
			this._isLoadingDataActive = true;
			TextAsset textAsset = Resources.Load<TextAsset>("infomap_pixelgun_test");
			if (textAsset == null)
			{
				UnityEngine.Debug.LogWarning("Bindata == null");
			}
			else
			{
				base.StartCoroutine(this.ParseLoadData(textAsset.text));
			}
		}
	}

	public static event Action onChangeInfoMap;
}