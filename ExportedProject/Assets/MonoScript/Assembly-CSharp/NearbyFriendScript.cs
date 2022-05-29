using Rilisoft;
using Rilisoft.NullExtensions;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

internal sealed class NearbyFriendScript : MonoBehaviour
{
	public UIRect nearbyFriendHeader;

	public UIRect nearbyFriendGrid;

	public UIRect otherFriendHeader;

	public UIRect otherFriendGrid;

	public bool NearbyFriendSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	public NearbyFriendScript()
	{
	}

	private void Start()
	{
		if (this.NearbyFriendSupported)
		{
			return;
		}
		if (this.nearbyFriendGrid != null && this.otherFriendGrid != null)
		{
			this.otherFriendGrid.topAnchor.Set(this.nearbyFriendGrid.topAnchor.relative, (float)this.nearbyFriendGrid.topAnchor.absolute);
		}
		if (this.nearbyFriendHeader != null && this.otherFriendHeader != null)
		{
			this.otherFriendHeader.topAnchor.Set(this.nearbyFriendHeader.topAnchor.relative, (float)this.nearbyFriendHeader.topAnchor.absolute);
			this.otherFriendHeader.bottomAnchor.Set(this.nearbyFriendHeader.bottomAnchor.relative, (float)this.nearbyFriendHeader.bottomAnchor.absolute);
		}
		this.nearbyFriendHeader.Do<UIRect>((UIRect h) => h.gameObject.SetActive(false));
		this.nearbyFriendGrid.Do<UIRect>((UIRect g) => g.gameObject.SetActive(false));
	}
}