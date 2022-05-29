using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class VisibleAtScene : MonoBehaviour
	{
		[SerializeField]
		private VisibleState _visible;

		[SerializeField]
		private bool _includeMenuScene;

		[SerializeField]
		private List<string> _scenes = new List<string>();

		private bool _baseVisible;

		public VisibleAtScene()
		{
		}

		private void Awake()
		{
			if (this._includeMenuScene)
			{
				this._scenes.Add(Defs.MainMenuScene);
			}
			this._baseVisible = base.gameObject.activeSelf;
			this._scenes = (
				from s in this._scenes
				select s.ToLower()).ToList<string>();
			this.SetVisible(SceneLoader.ActiveSceneName);
			Singleton<SceneLoader>.Instance.OnSceneLoaded += new Action<SceneLoadInfo>(this.OnSceneLoaded);
		}

		private void OnDestroy()
		{
			Singleton<SceneLoader>.Instance.OnSceneLoaded -= new Action<SceneLoadInfo>(this.OnSceneLoaded);
		}

		private void OnSceneLoaded(SceneLoadInfo inf)
		{
			this.SetVisible(inf.SceneName);
		}

		private void SetVisible(string currentSceneName)
		{
			currentSceneName = currentSceneName.ToLower();
			if (!this._scenes.Contains(currentSceneName))
			{
				base.gameObject.SetActiveSafe(this._baseVisible);
			}
			else
			{
				base.gameObject.SetActiveSafe(this._visible == VisibleState.On);
			}
		}
	}
}