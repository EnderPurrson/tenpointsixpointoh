using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponCustomCrosshair : MonoBehaviour
{
	public const string ASSET_PATH = "Common/crosshairs";

	[SerializeField]
	[ReadOnly]
	private int _dataId;

	private CrosshairData _data;

	private static CrosshairsSo _so;

	public int DataId
	{
		get
		{
			return _dataId;
		}
		set
		{
			_dataId = value;
			_data = null;
		}
	}

	public CrosshairData Data
	{
		get
		{
			if (_data != null)
			{
				return _data;
			}
			if (So == null)
			{
				return _data = new CrosshairData();
			}
			_data = So.Crosshairs.FirstOrDefault(_003Cget_Data_003Em__563);
			return _data ?? (_data = new CrosshairData());
		}
	}

	public static CrosshairsSo So
	{
		get
		{
			return _so ?? (_so = Resources.Load<CrosshairsSo>("Common/crosshairs"));
		}
	}

	[CompilerGenerated]
	private bool _003Cget_Data_003Em__563(CrosshairData c)
	{
		return c.ID == DataId;
	}
}
