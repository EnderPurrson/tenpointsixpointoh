using Rilisoft;
using System;
using System.Runtime.CompilerServices;

internal sealed class TrafficForwardingInfo : EventArgs
{
	private readonly static Lazy<TrafficForwardingInfo> _disabledInstance;

	private readonly int _minLevel;

	private readonly int _maxLevel;

	private readonly string _url;

	public static TrafficForwardingInfo DisabledInstance
	{
		get
		{
			return TrafficForwardingInfo._disabledInstance.Value;
		}
	}

	public bool Enabled
	{
		get
		{
			return !string.IsNullOrEmpty(this._url);
		}
	}

	public int MaxLevel
	{
		get
		{
			return this._maxLevel;
		}
	}

	public int MinLevel
	{
		get
		{
			return this._minLevel;
		}
	}

	public string Url
	{
		get
		{
			return this._url;
		}
	}

	static TrafficForwardingInfo()
	{
		TrafficForwardingInfo._disabledInstance = new Lazy<TrafficForwardingInfo>(() => new TrafficForwardingInfo(null, 0, ExperienceController.maxLevel));
	}

	public TrafficForwardingInfo(string url, int minLevel, int maxLevel)
	{
		this._url = url;
		this._minLevel = minLevel;
		this._maxLevel = maxLevel;
	}
}