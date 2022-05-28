using System;
using System.Runtime.CompilerServices;
using Rilisoft;

internal sealed class TrafficForwardingInfo : EventArgs
{
	private static readonly Lazy<TrafficForwardingInfo> _disabledInstance;

	private readonly int _minLevel;

	private readonly int _maxLevel;

	private readonly string _url;

	[CompilerGenerated]
	private static Func<TrafficForwardingInfo> _003C_003Ef__am_0024cache4;

	public static TrafficForwardingInfo DisabledInstance
	{
		get
		{
			return _disabledInstance.Value;
		}
	}

	public bool Enabled
	{
		get
		{
			return !string.IsNullOrEmpty(_url);
		}
	}

	public int MinLevel
	{
		get
		{
			return _minLevel;
		}
	}

	public int MaxLevel
	{
		get
		{
			return _maxLevel;
		}
	}

	public string Url
	{
		get
		{
			return _url;
		}
	}

	public TrafficForwardingInfo(string url, int minLevel, int maxLevel)
	{
		_url = url;
		_minLevel = minLevel;
		_maxLevel = maxLevel;
	}

	static TrafficForwardingInfo()
	{
		if (_003C_003Ef__am_0024cache4 == null)
		{
			_003C_003Ef__am_0024cache4 = _003C_disabledInstance_003Em__46A;
		}
		_disabledInstance = new Lazy<TrafficForwardingInfo>(_003C_003Ef__am_0024cache4);
	}

	[CompilerGenerated]
	private static TrafficForwardingInfo _003C_disabledInstance_003Em__46A()
	{
		return new TrafficForwardingInfo(null, 0, ExperienceController.maxLevel);
	}
}
