using System;
using UnityEngine;

public class EveryplayEarlyInitializer : MonoBehaviour
{
	public EveryplayEarlyInitializer()
	{
	}

	[RuntimeInitializeOnLoadMethod]
	private static void InitializeEveryplayOnStartup()
	{
		EveryplaySettings everyplaySetting = (EveryplaySettings)Resources.Load("EveryplaySettings");
		if (everyplaySetting != null && everyplaySetting.earlyInitializerEnabled && everyplaySetting.IsEnabled && everyplaySetting.IsValid)
		{
			Everyplay.Initialize();
		}
	}
}