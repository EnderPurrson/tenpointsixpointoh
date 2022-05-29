using Edelweiss.DecalSystem;
using System;
using UnityEngine;

public class DS_SkinnedDecals : SkinnedDecals
{
	public DS_SkinnedDecals()
	{
	}

	protected override SkinnedDecalsMeshRenderer AddSkinnedDecalsMeshRendererComponentToGameObject(GameObject a_GameObject)
	{
		return a_GameObject.AddComponent<DS_SkinnedDecalsMeshRenderer>();
	}
}