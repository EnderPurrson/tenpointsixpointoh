using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponCustomCrosshair : MonoBehaviour
{
	public const string ASSET_PATH = "Common/crosshairs";

	[ReadOnly]
	[SerializeField]
	private int _dataId;

	private CrosshairData _data;

	private static CrosshairsSo _so;

	public CrosshairData Data
	{
		get
		{
			CrosshairData crosshairDatum;
			if (this._data != null)
			{
				return this._data;
			}
			if (WeaponCustomCrosshair.So == null)
			{
				CrosshairData crosshairDatum1 = new CrosshairData();
				crosshairDatum = crosshairDatum1;
				this._data = crosshairDatum1;
				return crosshairDatum;
			}
			this._data = WeaponCustomCrosshair.So.Crosshairs.FirstOrDefault<CrosshairData>((CrosshairData c) => c.ID == this.DataId);
			CrosshairData crosshairDatum2 = this._data;
			if (crosshairDatum2 == null)
			{
				CrosshairData crosshairDatum3 = new CrosshairData();
				crosshairDatum = crosshairDatum3;
				this._data = crosshairDatum3;
				crosshairDatum2 = crosshairDatum;
			}
			return crosshairDatum2;
		}
	}

	public int DataId
	{
		get
		{
			return this._dataId;
		}
		set
		{
			this._dataId = value;
			this._data = null;
		}
	}

	public static CrosshairsSo So
	{
		get
		{
			CrosshairsSo crosshairsSo = WeaponCustomCrosshair._so;
			if (crosshairsSo == null)
			{
				crosshairsSo = Resources.Load<CrosshairsSo>("Common/crosshairs");
				WeaponCustomCrosshair._so = crosshairsSo;
			}
			return crosshairsSo;
		}
	}

	public WeaponCustomCrosshair()
	{
	}
}