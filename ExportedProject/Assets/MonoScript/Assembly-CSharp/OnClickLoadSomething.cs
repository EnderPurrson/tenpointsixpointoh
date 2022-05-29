using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickLoadSomething : MonoBehaviour
{
	public OnClickLoadSomething.ResourceTypeOption ResourceTypeToLoad;

	public string ResourceToLoad;

	public OnClickLoadSomething()
	{
	}

	public void OnClick()
	{
		OnClickLoadSomething.ResourceTypeOption resourceTypeToLoad = this.ResourceTypeToLoad;
		if (resourceTypeToLoad == OnClickLoadSomething.ResourceTypeOption.Scene)
		{
			SceneManager.LoadScene(this.ResourceToLoad);
		}
		else if (resourceTypeToLoad == OnClickLoadSomething.ResourceTypeOption.Web)
		{
			Application.OpenURL(this.ResourceToLoad);
		}
	}

	public enum ResourceTypeOption : byte
	{
		Scene,
		Web
	}
}