using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated]
internal sealed class _003C_003E__AnonType1<_003CAmmoInClip_003E__T, _003CAmmoInBackpack_003E__T>
{
	private readonly _003CAmmoInClip_003E__T _003CAmmoInClip_003E;

	private readonly _003CAmmoInBackpack_003E__T _003CAmmoInBackpack_003E;

	public _003CAmmoInClip_003E__T AmmoInClip
	{
		get
		{
			return _003CAmmoInClip_003E;
		}
	}

	public _003CAmmoInBackpack_003E__T AmmoInBackpack
	{
		get
		{
			return _003CAmmoInBackpack_003E;
		}
	}

	[DebuggerHidden]
	public _003C_003E__AnonType1(_003CAmmoInClip_003E__T AmmoInClip, _003CAmmoInBackpack_003E__T AmmoInBackpack)
	{
		_003CAmmoInClip_003E = AmmoInClip;
		_003CAmmoInBackpack_003E = AmmoInBackpack;
	}

	[DebuggerHidden]
	public override bool Equals(object obj)
	{
		_003C_003E__AnonType1<_003CAmmoInClip_003E__T, _003CAmmoInBackpack_003E__T> anon = obj as _003C_003E__AnonType1<_003CAmmoInClip_003E__T, _003CAmmoInBackpack_003E__T>;
		return anon != null && EqualityComparer<_003CAmmoInClip_003E__T>.Default.Equals(_003CAmmoInClip_003E, anon._003CAmmoInClip_003E) && EqualityComparer<_003CAmmoInBackpack_003E__T>.Default.Equals(_003CAmmoInBackpack_003E, anon._003CAmmoInBackpack_003E);
	}

	[DebuggerHidden]
	public override int GetHashCode()
	{
		int num = (((-2128831035 ^ EqualityComparer<_003CAmmoInClip_003E__T>.Default.GetHashCode(_003CAmmoInClip_003E)) * 16777619) ^ EqualityComparer<_003CAmmoInBackpack_003E__T>.Default.GetHashCode(_003CAmmoInBackpack_003E)) * 16777619;
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return num + (num << 5);
	}

	[DebuggerHidden]
	public override string ToString()
	{
		string[] obj = new string[6] { "{", " AmmoInClip = ", null, null, null, null };
		string text;
		if (_003CAmmoInClip_003E != null)
		{
			_003CAmmoInClip_003E__T val = _003CAmmoInClip_003E;
			text = val.ToString();
		}
		else
		{
			text = string.Empty;
		}
		obj[2] = text;
		obj[3] = ", AmmoInBackpack = ";
		string text2;
		if (_003CAmmoInBackpack_003E != null)
		{
			_003CAmmoInBackpack_003E__T val2 = _003CAmmoInBackpack_003E;
			text2 = val2.ToString();
		}
		else
		{
			text2 = string.Empty;
		}
		obj[4] = text2;
		obj[5] = " }";
		return string.Concat(obj);
	}
}
