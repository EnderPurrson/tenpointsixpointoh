using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhotonPingManager
{
	public bool UseNative;

	public static int Attempts;

	public static bool IgnoreInitialAttempt;

	public static int MaxMilliseconsPerPing;

	private int PingsRunning;

	public Region BestRegion
	{
		get
		{
			Region region = null;
			int ping = 2147483647;
			foreach (Region availableRegion in PhotonNetwork.networkingPeer.AvailableRegions)
			{
				UnityEngine.Debug.Log(string.Concat("BestRegion checks region: ", availableRegion));
				if (availableRegion.Ping == 0 || availableRegion.Ping >= ping)
				{
					continue;
				}
				ping = availableRegion.Ping;
				region = availableRegion;
			}
			return region;
		}
	}

	public bool Done
	{
		get
		{
			return this.PingsRunning == 0;
		}
	}

	static PhotonPingManager()
	{
		PhotonPingManager.Attempts = 5;
		PhotonPingManager.IgnoreInitialAttempt = true;
		PhotonPingManager.MaxMilliseconsPerPing = 800;
	}

	public PhotonPingManager()
	{
	}

	[DebuggerHidden]
	public IEnumerator PingSocket(Region region)
	{
		PhotonPingManager.u003cPingSocketu003ec__IteratorD3 variable = null;
		return variable;
	}

	public static string ResolveHost(string hostName)
	{
		string str;
		string empty = string.Empty;
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
			if ((int)hostAddresses.Length != 1)
			{
				for (int i = 0; i < (int)hostAddresses.Length; i++)
				{
					IPAddress pAddress = hostAddresses[i];
					if (pAddress != null)
					{
						if (pAddress.AddressFamily.ToString().Contains("6"))
						{
							str = pAddress.ToString();
							return str;
						}
						else if (string.IsNullOrEmpty(empty))
						{
							empty = hostAddresses.ToString();
						}
					}
				}
				return empty;
			}
			else
			{
				str = hostAddresses[0].ToString();
			}
		}
		catch (Exception exception1)
		{
			Exception exception = exception1;
			UnityEngine.Debug.Log(string.Concat("Exception caught! ", exception.Source, " Message: ", exception.Message));
			return empty;
		}
		return str;
	}
}