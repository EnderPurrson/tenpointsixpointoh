using System;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

internal sealed class NearbyFriendScript : MonoBehaviour
{
	public UIRect nearbyFriendHeader;

	public UIRect nearbyFriendGrid;

	public UIRect otherFriendHeader;

	public UIRect otherFriendGrid;

	[CompilerGenerated]
	private static Action<UIRect> _003C_003Ef__am_0024cache4;

	[CompilerGenerated]
	private static Action<UIRect> _003C_003Ef__am_0024cache5;

	public bool NearbyFriendSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	private void Start()
	{
		if (!NearbyFriendSupported)
		{
			if (nearbyFriendGrid != null && otherFriendGrid != null)
			{
				otherFriendGrid.topAnchor.Set(nearbyFriendGrid.topAnchor.relative, nearbyFriendGrid.topAnchor.absolute);
			}
			if (nearbyFriendHeader != null && otherFriendHeader != null)
			{
				otherFriendHeader.topAnchor.Set(nearbyFriendHeader.topAnchor.relative, nearbyFriendHeader.topAnchor.absolute);
				otherFriendHeader.bottomAnchor.Set(nearbyFriendHeader.bottomAnchor.relative, nearbyFriendHeader.bottomAnchor.absolute);
			}
			UIRect o = nearbyFriendHeader;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				_003C_003Ef__am_0024cache4 = _003CStart_003Em__2B2;
			}
			o.Do(_003C_003Ef__am_0024cache4);
			UIRect o2 = nearbyFriendGrid;
			if (_003C_003Ef__am_0024cache5 == null)
			{
				_003C_003Ef__am_0024cache5 = _003CStart_003Em__2B3;
			}
			o2.Do(_003C_003Ef__am_0024cache5);
		}
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__2B2(UIRect h)
	{
		h.gameObject.SetActive(false);
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__2B3(UIRect g)
	{
		g.gameObject.SetActive(false);
	}
}
