using System;
using UnityEngine;

public class EveryplaySettings : ScriptableObject
{
	public string clientId;

	public string clientSecret;

	public string redirectURI = "https://m.everyplay.com/auth";

	public bool iosSupportEnabled;

	public bool tvosSupportEnabled;

	public bool androidSupportEnabled;

	public bool standaloneSupportEnabled;

	public bool testButtonsEnabled;

	public bool earlyInitializerEnabled = true;

	public bool IsEnabled
	{
		get
		{
			return this.androidSupportEnabled;
		}
	}

	public bool IsValid
	{
		get
		{
			if (this.clientId != null && this.clientSecret != null && this.redirectURI != null && this.clientId.Trim().Length > 0 && this.clientSecret.Trim().Length > 0 && this.redirectURI.Trim().Length > 0)
			{
				return true;
			}
			return false;
		}
	}

	public EveryplaySettings()
	{
	}
}