using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SampleWebView : MonoBehaviour
{
	public string Url;

	private WebViewObject webViewObject;

	[CompilerGenerated]
	private static Action<string> _003C_003Ef__am_0024cache2;

	private void Start()
	{
		webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
		WebViewObject obj = webViewObject;
		if (_003C_003Ef__am_0024cache2 == null)
		{
			_003C_003Ef__am_0024cache2 = _003CStart_003Em__533;
		}
		obj.Init(_003C_003Ef__am_0024cache2);
		webViewObject.LoadURL(Url);
		webViewObject.SetVisibility(true);
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.IPhonePlayer)
		{
			webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.Unity = {\t\tcall:function(msg) {\t\t\tvar iframe = document.createElement('IFRAME');\t\t\tiframe.setAttribute('src', 'unity:' + msg);\t\t\tdocument.documentElement.appendChild(iframe);\t\t\tiframe.parentNode.removeChild(iframe);\t\t\tiframe = null;\t\t}\t}}, false);");
		}
		webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.addEventListener('click', function() {\t\tUnity.call('clicked');\t}, false);}, false);");
	}

	[CompilerGenerated]
	private static void _003CStart_003Em__533(string msg)
	{
		Debug.Log(string.Format("CallFromJS[{0}]", msg));
	}
}
