using System;

namespace Facebook.Unity.Canvas
{
	internal interface ICanvasJSWrapper
	{
		string IntegrationMethodJs
		{
			get;
		}

		void DisableFullScreen();

		void ExternalCall(string functionName, params object[] args);

		void ExternalEval(string script);

		string GetSDKVersion();
	}
}