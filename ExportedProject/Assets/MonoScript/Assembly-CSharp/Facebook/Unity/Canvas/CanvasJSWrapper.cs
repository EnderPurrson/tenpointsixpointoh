using System;
using UnityEngine;

namespace Facebook.Unity.Canvas
{
	internal class CanvasJSWrapper : ICanvasJSWrapper
	{
		private const string JSSDKBindingFileName = "JSSDKBindings";

		public string IntegrationMethodJs
		{
			get
			{
				TextAsset textAsset = Resources.Load("JSSDKBindings") as TextAsset;
				if (!textAsset)
				{
					return null;
				}
				return textAsset.text;
			}
		}

		public CanvasJSWrapper()
		{
		}

		public void DisableFullScreen()
		{
			if (Screen.fullScreen)
			{
				Screen.fullScreen = false;
			}
		}

		public void ExternalCall(string functionName, params object[] args)
		{
			Application.ExternalCall(functionName, args);
		}

		public void ExternalEval(string script)
		{
			Application.ExternalEval(script);
		}

		public string GetSDKVersion()
		{
			return "v2.5";
		}
	}
}