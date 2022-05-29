using System;
using UnityEngine;

public class PlayerPrefsX
{
	public static string SndSetting
	{
		get
		{
			return "SoundMusicSetting";
		}
	}

	public static string SoundFXSetting
	{
		get
		{
			return "SoundFXSetting";
		}
	}

	public static string SoundMusicSetting
	{
		get
		{
			return "SoundMusicSetting";
		}
	}

	public PlayerPrefsX()
	{
	}

	public static bool GetBool(string name)
	{
		return (PlayerPrefs.GetInt(name) != 1 ? false : true);
	}

	public static bool GetBool(string name, bool defaultValue)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return defaultValue;
		}
		return PlayerPrefsX.GetBool(name);
	}

	public static void SetBool(string name, bool booleanValue)
	{
		PlayerPrefs.SetInt(name, (!booleanValue ? 0 : 1));
	}
}