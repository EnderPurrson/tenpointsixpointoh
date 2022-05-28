using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WebViewStarter
{
	[CompilerGenerated]
	private static Action<string> _003C_003Ef__am_0024cache0;

	public static WebViewObject StartBrowser(string Url)
	{
		WebViewObject webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = _003CStartBrowser_003Em__534;
		}
		webViewObject.Init(_003C_003Ef__am_0024cache0);
		webViewObject.LoadURL(Url);
		webViewObject.SetVisibility(true);
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.IPhonePlayer)
		{
			webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.Unity = {\t\tcall:function(msg) {\t\t\tvar iframe = document.createElement('IFRAME');\t\t\tiframe.setAttribute('src', 'unity:' + msg);\t\t\tdocument.documentElement.appendChild(iframe);\t\t\tiframe.parentNode.removeChild(iframe);\t\t\tiframe = null;\t\t}\t}}, false);");
		}
		webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.addEventListener('click', function() {\t\tUnity.call('clicked');\t}, false);}, false);");
		return webViewObject;
	}

	[CompilerGenerated]
	private static void _003CStartBrowser_003Em__534(string msg)
	{
		Debug.Log(string.Format("CallFromJS[{0}]", msg));
	}
}
