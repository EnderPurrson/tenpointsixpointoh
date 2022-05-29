using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SampleWebView : MonoBehaviour
{
	public string Url;

	private WebViewObject webViewObject;

	public SampleWebView()
	{
	}

	private void Start()
	{
		this.webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
		this.webViewObject.Init((string msg) => Debug.Log(string.Format("CallFromJS[{0}]", msg)));
		this.webViewObject.LoadURL(this.Url);
		this.webViewObject.SetVisibility(true);
		RuntimePlatform runtimePlatform = Application.platform;
		if (runtimePlatform == RuntimePlatform.OSXEditor || runtimePlatform == RuntimePlatform.OSXPlayer || runtimePlatform == RuntimePlatform.IPhonePlayer)
		{
			this.webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.Unity = {\t\tcall:function(msg) {\t\t\tvar iframe = document.createElement('IFRAME');\t\t\tiframe.setAttribute('src', 'unity:' + msg);\t\t\tdocument.documentElement.appendChild(iframe);\t\t\tiframe.parentNode.removeChild(iframe);\t\t\tiframe = null;\t\t}\t}}, false);");
		}
		this.webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.addEventListener('click', function() {\t\tUnity.call('clicked');\t}, false);}, false);");
	}
}