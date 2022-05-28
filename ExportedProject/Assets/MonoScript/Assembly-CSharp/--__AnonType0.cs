using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[CompilerGenerated]
internal sealed class _003C_003E__AnonType0<_003CItemID_003E__T, _003CDiscount_003E__T>
{
	private readonly _003CItemID_003E__T _003CItemID_003E;

	private readonly _003CDiscount_003E__T _003CDiscount_003E;

	public _003CItemID_003E__T ItemID
	{
		get
		{
			return _003CItemID_003E;
		}
	}

	public _003CDiscount_003E__T Discount
	{
		get
		{
			return _003CDiscount_003E;
		}
	}

	[DebuggerHidden]
	public _003C_003E__AnonType0(_003CItemID_003E__T ItemID, _003CDiscount_003E__T Discount)
	{
		_003CItemID_003E = ItemID;
		_003CDiscount_003E = Discount;
	}

	[DebuggerHidden]
	public override bool Equals(object obj)
	{
		_003C_003E__AnonType0<_003CItemID_003E__T, _003CDiscount_003E__T> anon = obj as _003C_003E__AnonType0<_003CItemID_003E__T, _003CDiscount_003E__T>;
		return anon != null && EqualityComparer<_003CItemID_003E__T>.Default.Equals(_003CItemID_003E, anon._003CItemID_003E) && EqualityComparer<_003CDiscount_003E__T>.Default.Equals(_003CDiscount_003E, anon._003CDiscount_003E);
	}

	[DebuggerHidden]
	public override int GetHashCode()
	{
		int num = (((-2128831035 ^ EqualityComparer<_003CItemID_003E__T>.Default.GetHashCode(_003CItemID_003E)) * 16777619) ^ EqualityComparer<_003CDiscount_003E__T>.Default.GetHashCode(_003CDiscount_003E)) * 16777619;
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return num + (num << 5);
	}

	[DebuggerHidden]
	public override string ToString()
	{
		string[] obj = new string[6] { "{", " ItemID = ", null, null, null, null };
		string text;
		if (_003CItemID_003E != null)
		{
			_003CItemID_003E__T val = _003CItemID_003E;
			text = val.ToString();
		}
		else
		{
			text = string.Empty;
		}
		obj[2] = text;
		obj[3] = ", Discount = ";
		string text2;
		if (_003CDiscount_003E != null)
		{
			_003CDiscount_003E__T val2 = _003CDiscount_003E;
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
