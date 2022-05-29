using Rilisoft;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Device
{
	private readonly static IDictionary<string, int> _gpuRatings;

	public static bool isPixelGunLow;

	public static bool IsLoweMemoryDevice
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return SystemInfo.systemMemorySize < 900;
			}
			if (Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				if (Application.isEditor)
				{
					return false;
				}
				return true;
			}
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return false;
			}
			return false;
		}
	}

	public static bool isNonRetinaDevice
	{
		get
		{
			return true;
		}
	}

	public static bool isPixelGunLowDevice
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return Device.IsWeakAndroid();
			}
			if (Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				return true;
			}
			return false;
		}
	}

	public static bool isRetinaAndStrong
	{
		get
		{
			if (Application.isEditor)
			{
				return true;
			}
			return false;
		}
	}

	public static bool isWeakDevice
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return !Device.IsQuiteGoodAndroidDevice();
			}
			if (Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				return true;
			}
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			return false;
		}
	}

	static Device()
	{
		Dictionary<string, int> strs = new Dictionary<string, int>()
		{
			{ "Adreno (TM) 330", 17 },
			{ "PowerVR SGX 554MP", 15 },
			{ "Mali-T628", 15 },
			{ "Mali-T624", 15 },
			{ "PowerVR G6430", 15 },
			{ "PowerVR Rogue", 14 },
			{ "Mali-T604", 11 },
			{ "Adreno (TM) 320", 11 },
			{ "PowerVR SGX G6200", 10 },
			{ "PowerVR SGX 543MP", 8 },
			{ "PowerVR SGX 544", 8 },
			{ "PowerVR SGX 544MP", 8 },
			{ "Intel HD Graphics", 8 },
			{ "Mali-450 MP", 8 },
			{ "Vivante GC4000", 6 },
			{ "Adreno (TM) 305", 5 },
			{ "NVIDIA Tegra 3", 5 },
			{ "NVIDIA Tegra 3 / Chainfire3D", 5 },
			{ "Vivante GC2000", 5 },
			{ "GC2000 core / Chainfire3D", 5 },
			{ "Mali-400 MP", 4 },
			{ "MALI-400MP4", 4 },
			{ "Mali-400 MP / Chainfire3D", 4 },
			{ "Adreno (TM) 225", 4 },
			{ "VideoCore IV HW", 4 },
			{ "NVIDIA Tegra", 3 },
			{ "GC1000 core", 3 },
			{ "Adreno (TM) 220", 3 },
			{ "Adreno (TM) 220 / Chainfire3D", 3 },
			{ "Vivante GC1000", 3 },
			{ "Adreno (TM) 203", 2 },
			{ "Adreno (TM) 205", 2 },
			{ "PowerVR SGX 531 / Chainfire3D", 2 },
			{ "PowerVR SGX 540", 2 },
			{ "PowerVR SGX 540 / Chainfire3D", 2 },
			{ "Adreno (TM) 200", 1 },
			{ "Adreno (TM) 200 / Chainfire3D", 1 },
			{ "Immersion.16", 1 },
			{ "Immersion.16 / Chainfire3D", 1 },
			{ "Bluestacks", 1 },
			{ "GC800 core", 1 },
			{ "GC800 core / Chainfire3D", 1 },
			{ "Mali-200", 1 },
			{ "Mali-300", 1 },
			{ "GC400 core", 1 },
			{ "S5 Multicore c", 1 },
			{ "PowerVR SGX530", 1 },
			{ "PowerVR SGX 530", 1 },
			{ "PowerVR SGX 531", 1 },
			{ "PowerVR SGX 535", 1 },
			{ "PowerVR SGX 543", 1 }
		};
		Device._gpuRatings = strs;
		Device.isPixelGunLow = true;
	}

	public Device()
	{
	}

	internal static string FormatDeviceModelMemoryRating()
	{
		int num;
		string str = string.Concat(SystemInfo.deviceModel, ": {{ Memory: {0}Mb, Rating: {1} }}");
		return (!Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out num) ? string.Format(str, SystemInfo.systemMemorySize, "?") : string.Format(str, SystemInfo.systemMemorySize, num));
	}

	internal static string FormatGpuModelMemoryRating()
	{
		int num;
		string str = string.Concat(SystemInfo.graphicsDeviceName, ": {{ Memory: {0}Mb, Rating: {1} }}");
		return (!Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out num) ? string.Format(str, SystemInfo.graphicsMemorySize, "?") : string.Format(str, SystemInfo.graphicsMemorySize, num));
	}

	public static bool GpuRatingIsAtLeast(int desiredGpuRating)
	{
		int num;
		return (!Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out num) ? true : num >= desiredGpuRating);
	}

	public static bool IsQuiteGoodAndroidDevice()
	{
		return false;
	}

	public static bool IsWeakAndroid()
	{
		int num = SystemInfo.processorCount;
		int num1 = SystemInfo.processorFrequency;
		int num2 = SystemInfo.systemMemorySize;
		Debug.LogFormat("Device info: {{ 'processorCount':{0}, 'processorFrequency (MHz)':{1}, 'systemMemorySize':{2} }}", new object[] { num, num1, num2 });
		if (num == 4)
		{
			if (num2 <= 1300 || num1 <= 1400)
			{
				return true;
			}
		}
		else if (num <= 2 && (num2 <= 1300 || num1 <= 1400))
		{
			return true;
		}
		if (num == 1)
		{
			return true;
		}
		return false;
	}

	internal static bool TryGetGpuRating(out int rating)
	{
		return Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out rating);
	}
}