using System;
using UnityEngine;

public class WebViewObject : MonoBehaviour
{
	private Action<string> callback;

	private AndroidJavaObject webView;

	private Vector2 offset;

	public WebViewObject()
	{
	}

	public void CallFromJS(string message)
	{
		if (this.callback != null)
		{
			this.callback(message);
		}
	}

	public void EvaluateJS(string js)
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("LoadURL", new object[] { string.Concat("javascript:", js) });
	}

	public void goHome()
	{
	}

	public void Init(Action<string> cb = null)
	{
		this.callback = cb;
		this.offset = new Vector2(0f, 0f);
		this.webView = new AndroidJavaObject("net.gree.unitywebview.WebViewPlugin", new object[0]);
		this.webView.Call("Init", new object[] { base.name });
	}

	public void LoadURL(string url)
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("LoadURL", new object[] { url });
	}

	private void OnDestroy()
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("Destroy", new object[0]);
	}

	public void SetMargins(int left, int top, int right, int bottom)
	{
		if (this.webView == null)
		{
			return;
		}
		this.offset = new Vector2((float)left, (float)top);
		this.webView.Call("SetMargins", new object[] { left, top, right, bottom });
	}

	public void SetVisibility(bool v)
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("SetVisibility", new object[] { v });
	}
}