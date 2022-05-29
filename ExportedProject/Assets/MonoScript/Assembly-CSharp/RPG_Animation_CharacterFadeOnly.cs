using System;
using UnityEngine;

public class RPG_Animation_CharacterFadeOnly : MonoBehaviour
{
	public static RPG_Animation_CharacterFadeOnly instance;

	public RPG_Animation_CharacterFadeOnly()
	{
	}

	private void Awake()
	{
		RPG_Animation_CharacterFadeOnly.instance = this;
	}
}