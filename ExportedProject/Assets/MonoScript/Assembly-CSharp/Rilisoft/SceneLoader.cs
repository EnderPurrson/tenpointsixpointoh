using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public class SceneLoader : Singleton<SceneLoader>
	{
		public const string SCENE_INFOS_ASSET_PATH = "Assets/Resources/ScenesList.asset";

		[SerializeField]
		private ScenesList _scenesList;

		[SerializeField]
		private List<SceneLoadInfo> _loadingHistory = new List<SceneLoadInfo>();

		public Action<SceneLoadInfo> OnSceneLoading = new Action<SceneLoadInfo>((SceneLoadInfo i) => {
		});

		public Action<SceneLoadInfo> OnSceneLoaded = new Action<SceneLoadInfo>((SceneLoadInfo i) => {
		});

		public static string ActiveSceneName
		{
			get
			{
				return SceneManager.GetActiveScene().name ?? string.Empty;
			}
		}

		public SceneLoader()
		{
		}

		public ExistsSceneInfo GetSceneInfo(string sceneName)
		{
			ExistsSceneInfo existsSceneInfo = (!string.IsNullOrEmpty(Path.GetDirectoryName(sceneName)) ? this._scenesList.Infos.FirstOrDefault<ExistsSceneInfo>((ExistsSceneInfo i) => i.Path == sceneName) : this._scenesList.Infos.FirstOrDefault<ExistsSceneInfo>((ExistsSceneInfo i) => i.Name == sceneName));
			if (existsSceneInfo == null)
			{
				throw new ArgumentException(string.Format("Unknown scene : '{0}'", sceneName));
			}
			return existsSceneInfo;
		}

		public void LoadScene(string sceneName, LoadSceneMode mode = 0)
		{
			ExistsSceneInfo sceneInfo = this.GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = new SceneLoadInfo();
			SceneLoadInfo name = sceneLoadInfo;
			name.SceneName = sceneInfo.Name;
			name.LoadMode = mode;
			sceneLoadInfo = name;
			this.OnSceneLoading(sceneLoadInfo);
			SceneManager.LoadScene(sceneName, mode);
			this.OnSceneLoaded(sceneLoadInfo);
		}

		public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = 0)
		{
			ExistsSceneInfo sceneInfo = this.GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = new SceneLoadInfo();
			SceneLoadInfo name = sceneLoadInfo;
			name.SceneName = sceneInfo.Name;
			name.LoadMode = mode;
			sceneLoadInfo = name;
			this.OnSceneLoading(sceneLoadInfo);
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneInfo.Name, mode);
			Singleton<SceneLoader>.Instance.StartCoroutine(this.WaitSceneIsLoaded(sceneLoadInfo, asyncOperation));
			return asyncOperation;
		}

		private void OnInstanceCreated()
		{
			if (this._scenesList == null)
			{
				throw new Exception("scenes list is null");
			}
			IGrouping<string, ExistsSceneInfo>[] array = (
				from i in this._scenesList.Infos
				group i by i.Name into g
				where g.Count<ExistsSceneInfo>() > 1
				select g).ToArray<IGrouping<string, ExistsSceneInfo>>();
			if (!array.Any<IGrouping<string, ExistsSceneInfo>>())
			{
				SceneLoader sceneLoader = this;
				Action<SceneLoadInfo> onSceneLoaded = sceneLoader.OnSceneLoaded;
				List<SceneLoadInfo> sceneLoadInfos = this._loadingHistory;
				sceneLoader.OnSceneLoaded = (Action<SceneLoadInfo>)Delegate.Combine(onSceneLoaded, new Action<SceneLoadInfo>(sceneLoadInfos.Add));
				return;
			}
			string str = (
				from g in array
				select g.Key).Aggregate<string>((string cur, string next) => string.Format("{0},{1}{2}", cur, next, Environment.NewLine));
			UnityEngine.Debug.LogError(string.Concat("[SCENELOADER] duplicate scenes: ", str));
		}

		[DebuggerHidden]
		private IEnumerator WaitSceneIsLoaded(SceneLoadInfo loadInfo, AsyncOperation op)
		{
			SceneLoader.u003cWaitSceneIsLoadedu003ec__Iterator18D variable = null;
			return variable;
		}
	}
}