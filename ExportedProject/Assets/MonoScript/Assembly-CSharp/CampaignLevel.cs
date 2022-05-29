using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CampaignLevel
{
	public float timeToComplete = 300f;

	private Vector3 _localPosition = Vector3.forward;

	private string _sceneName;

	private string _localizeKeyForLevelMap;

	public string localizeKeyForLevelMap
	{
		get
		{
			return this._localizeKeyForLevelMap;
		}
		set
		{
			this._localizeKeyForLevelMap = value;
		}
	}

	public Vector3 LocalPosition
	{
		get
		{
			return this._localPosition;
		}
		set
		{
			this._localPosition = value;
		}
	}

	public string predlog
	{
		get;
		set;
	}

	public string sceneName
	{
		get
		{
			return this._sceneName;
		}
		set
		{
			this._sceneName = value;
		}
	}

	public CampaignLevel(string sceneName, string keyForLevelMap, string pr = "in")
	{
		this._sceneName = sceneName;
		this._localizeKeyForLevelMap = keyForLevelMap;
		this.predlog = pr;
	}

	public CampaignLevel()
	{
		this._sceneName = string.Empty;
	}
}