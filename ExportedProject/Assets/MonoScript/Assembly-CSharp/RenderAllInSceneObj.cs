using Rilisoft;
using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;

public class RenderAllInSceneObj : MonoBehaviour
{
	public RenderAllInSceneObj()
	{
	}

	private void Awake()
	{
		string str = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[] { base.GetType().Name });
		ScopeLogger scopeLogger = new ScopeLogger(str, Defs.IsDeveloperBuild);
		try
		{
			if (!Device.IsLoweMemoryDevice)
			{
				Transform transforms = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("RenderAllInSceneObjInner")).transform;
				transforms.parent = base.transform;
				transforms.localPosition = Vector3.zero;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		finally
		{
			scopeLogger.Dispose();
		}
	}
}