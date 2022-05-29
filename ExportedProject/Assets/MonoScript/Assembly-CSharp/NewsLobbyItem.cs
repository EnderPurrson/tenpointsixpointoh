using Rilisoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewsLobbyItem : MonoBehaviour
{
	public GameObject indicatorNew;

	public UILabel headerLabel;

	public UILabel shortDescLabel;

	public UILabel dateLabel;

	public UITexture previewPic;

	public string previewPicUrl;

	public NewsLobbyItem()
	{
	}

	public void LoadPreview(string url)
	{
		base.StartCoroutine(this.LoadPreviewPicture(url));
	}

	[DebuggerHidden]
	private IEnumerator LoadPreviewPicture(string picLink)
	{
		NewsLobbyItem.u003cLoadPreviewPictureu003ec__IteratorCF variable = null;
		return variable;
	}
}