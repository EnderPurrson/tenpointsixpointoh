using System;
using System.Collections.Generic;

[Serializable]
public class AllScenesForMode
{
	public TypeModeGame mode;

	public List<SceneInfo> avaliableScenes = new List<SceneInfo>();

	public AllScenesForMode()
	{
	}

	public void AddInfoScene(SceneInfo needInfo)
	{
		this.avaliableScenes.Add(needInfo);
	}
}