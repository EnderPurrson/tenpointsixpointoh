using Facebook.Unity;
using System;

namespace Facebook.Unity.Mobile
{
	internal interface IMobileFacebookCallbackHandler : IFacebookCallbackHandler
	{
		void OnAppInviteComplete(string message);

		void OnFetchDeferredAppLinkComplete(string message);

		void OnRefreshCurrentAccessTokenComplete(string message);
	}
}