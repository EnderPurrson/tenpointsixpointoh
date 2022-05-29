using Edelweiss.DecalSystem;
using System;
using UnityEngine;

public class DS_Decals : Decals
{
	public DS_Decals()
	{
	}

	protected override DecalsMeshRenderer AddDecalsMeshRendererComponentToGameObject(GameObject a_GameObject)
	{
		return a_GameObject.AddComponent<DS_DecalsMeshRenderer>();
	}
}