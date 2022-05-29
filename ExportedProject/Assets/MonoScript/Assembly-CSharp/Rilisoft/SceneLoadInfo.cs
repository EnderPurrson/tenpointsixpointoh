using System;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public struct SceneLoadInfo
	{
		public LoadSceneMode LoadMode
		{
			get;
			set;
		}

		public string SceneName
		{
			get;
			set;
		}
	}
}