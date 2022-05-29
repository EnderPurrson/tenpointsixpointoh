using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WebViewStarter
{
	public WebViewStarter()
	{
	}

	public static WebViewObject StartBrowser(string Url)
	{
		WebViewObject webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
		webViewObject.Init((string msg) => Debug.Log(string.Format("CallFromJS[{0}]", msg)));
		webViewObject.LoadURL(Url);
		webViewObject.SetVisibility(true);
		RuntimePlatform runtimePlatform = Application.platform;
		if (runtimePlatform == RuntimePlatform.OSXEditor || runtimePlatform == RuntimePlatform.OSXPlayer || runtimePlatform == RuntimePlatform.IPhonePlayer)
		{
			webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.Unity = {\t\tcall:function(msg) {\t\t\tvar iframe = document.createElement('IFRAME');\t\t\tiframe.setAttribute('src', 'unity:' + msg);\t\t\tdocument.documentElement.appendChild(iframe);\t\t\tiframe.parentNode.removeChild(iframe);\t\t\tiframe = null;\t\t}\t}}, false);");
		}
		webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.addEventListener('click', function() {\t\tUnity.call('clicked');\t}, false);}, false);");
		return webViewObject;
	}
}