using Facebook.Unity;
using System;

namespace Facebook.Unity.Canvas
{
	internal interface ICanvasFacebookCallbackHandler : IFacebookCallbackHandler
	{
		void OnFacebookAuthResponseChange(string message);

		void OnHideUnity(bool hide);

		void OnPayComplete(string message);

		void OnUrlResponse(string message);
	}
}