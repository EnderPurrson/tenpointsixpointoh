using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public class SceneLoader : Singleton<SceneLoader>
	{
		[CompilerGenerated]
		private sealed class _003CGetSceneInfo_003Ec__AnonStorey2F7
		{
			internal string sceneName;

			internal bool _003C_003Em__433(ExistsSceneInfo i)
			{
				return i.Name == sceneName;
			}

			internal bool _003C_003Em__434(ExistsSceneInfo i)
			{
				return i.Path == sceneName;
			}
		}

		public const string SCENE_INFOS_ASSET_PATH = "Assets/Resources/ScenesList.asset";

		[SerializeField]
		private ScenesList _scenesList;

		[SerializeField]
		private List<SceneLoadInfo> _loadingHistory = new List<SceneLoadInfo>();

		public Action<SceneLoadInfo> OnSceneLoading;

		public Action<SceneLoadInfo> OnSceneLoaded;

		[CompilerGenerated]
		private static Action<SceneLoadInfo> _003C_003Ef__am_0024cache4;

		[CompilerGenerated]
		private static Action<SceneLoadInfo> _003C_003Ef__am_0024cache5;

		[CompilerGenerated]
		private static Func<ExistsSceneInfo, string> _003C_003Ef__am_0024cache6;

		[CompilerGenerated]
		private static Func<IGrouping<string, ExistsSceneInfo>, bool> _003C_003Ef__am_0024cache7;

		[CompilerGenerated]
		private static Func<IGrouping<string, ExistsSceneInfo>, string> _003C_003Ef__am_0024cache8;

		[CompilerGenerated]
		private static Func<string, string, string> _003C_003Ef__am_0024cache9;

		public static string ActiveSceneName
		{
			get
			{
				return SceneManager.GetActiveScene().name ?? string.Empty;
			}
		}

		public SceneLoader()
		{
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003COnSceneLoading_003Em__42D;
			}
			OnSceneLoading = _003C_003Ef__am_0024cache4;
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003COnSceneLoaded_003Em__42E;
			}
			OnSceneLoaded = _003C_003Ef__am_0024cache5;
			base._002Ector();
		}

		private void OnInstanceCreated()
		{
			if (_scenesList == null)
			{
				throw new Exception("scenes list is null");
			}
			List<ExistsSceneInfo> infos = _scenesList.Infos;
			if (_003C_003Ef__am_0024cache6 == null)
			{
				_003C_003Ef__am_0024cache6 = _003COnInstanceCreated_003Em__42F;
			}
			IEnumerable<IGrouping<string, ExistsSceneInfo>> source = infos.GroupBy(_003C_003Ef__am_0024cache6);
			if (_003C_003Ef__am_0024cache7 == null)
			{
				_003C_003Ef__am_0024cache7 = _003COnInstanceCreated_003Em__430;
			}
			IGrouping<string, ExistsSceneInfo>[] source2 = source.Where(_003C_003Ef__am_0024cache7).ToArray();
			if (source2.Any())
			{
				if (_003C_003Ef__am_0024cache8 == null)
				{
					_003C_003Ef__am_0024cache8 = _003COnInstanceCreated_003Em__431;
				}
				IEnumerable<string> source3 = source2.Select(_003C_003Ef__am_0024cache8);
				if (_003C_003Ef__am_0024cache9 == null)
				{
					_003C_003Ef__am_0024cache9 = _003COnInstanceCreated_003Em__432;
				}
				string text = source3.Aggregate(_003C_003Ef__am_0024cache9);
				Debug.LogError("[SCENELOADER] duplicate scenes: " + text);
			}
			else
			{
				OnSceneLoaded = (Action<SceneLoadInfo>)Delegate.Combine(OnSceneLoaded, new Action<SceneLoadInfo>(_loadingHistory.Add));
			}
		}

		public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			ExistsSceneInfo sceneInfo = GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = default(SceneLoadInfo);
			sceneLoadInfo.SceneName = sceneInfo.Name;
			sceneLoadInfo.LoadMode = mode;
			SceneLoadInfo obj = sceneLoadInfo;
			OnSceneLoading(obj);
			SceneManager.LoadScene(sceneName, mode);
			OnSceneLoaded(obj);
		}

		public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			ExistsSceneInfo sceneInfo = GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = default(SceneLoadInfo);
			sceneLoadInfo.SceneName = sceneInfo.Name;
			sceneLoadInfo.LoadMode = mode;
			SceneLoadInfo sceneLoadInfo2 = sceneLoadInfo;
			OnSceneLoading(sceneLoadInfo2);
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneInfo.Name, mode);
			Singleton<SceneLoader>.Instance.StartCoroutine(WaitSceneIsLoaded(sceneLoadInfo2, asyncOperation));
			return asyncOperation;
		}

		public ExistsSceneInfo GetSceneInfo(string sceneName)
		{
			_003CGetSceneInfo_003Ec__AnonStorey2F7 _003CGetSceneInfo_003Ec__AnonStorey2F = new _003CGetSceneInfo_003Ec__AnonStorey2F7();
			_003CGetSceneInfo_003Ec__AnonStorey2F.sceneName = sceneName;
			ExistsSceneInfo existsSceneInfo = ((!string.IsNullOrEmpty(Path.GetDirectoryName(_003CGetSceneInfo_003Ec__AnonStorey2F.sceneName))) ? _scenesList.Infos.FirstOrDefault(_003CGetSceneInfo_003Ec__AnonStorey2F._003C_003Em__434) : _scenesList.Infos.FirstOrDefault(_003CGetSceneInfo_003Ec__AnonStorey2F._003C_003Em__433));
			if (existsSceneInfo == null)
			{
				throw new ArgumentException(string.Format("Unknown scene : '{0}'", _003CGetSceneInfo_003Ec__AnonStorey2F.sceneName));
			}
			return existsSceneInfo;
		}

		private IEnumerator WaitSceneIsLoaded(SceneLoadInfo loadInfo, AsyncOperation op)
		{
			while (!op.isDone)
			{
				yield return null;
			}
			OnSceneLoaded(loadInfo);
		}

		[CompilerGenerated]
		private static void _003COnSceneLoading_003Em__42D(SceneLoadInfo i)
		{
		}

		[CompilerGenerated]
		private static void _003COnSceneLoaded_003Em__42E(SceneLoadInfo i)
		{
		}

		[CompilerGenerated]
		private static string _003COnInstanceCreated_003Em__42F(ExistsSceneInfo i)
		{
			return i.Name;
		}

		[CompilerGenerated]
		private static bool _003COnInstanceCreated_003Em__430(IGrouping<string, ExistsSceneInfo> g)
		{
			return g.Count() > 1;
		}

		[CompilerGenerated]
		private static string _003COnInstanceCreated_003Em__431(IGrouping<string, ExistsSceneInfo> g)
		{
			return g.Key;
		}

		[CompilerGenerated]
		private static string _003COnInstanceCreated_003Em__432(string cur, string next)
		{
			return string.Format("{0},{1}{2}", cur, next, Environment.NewLine);
		}
	}
}
