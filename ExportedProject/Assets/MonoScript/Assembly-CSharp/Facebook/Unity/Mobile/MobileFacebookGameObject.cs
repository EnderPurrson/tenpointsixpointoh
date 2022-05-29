using Facebook.Unity;
using System;

namespace Facebook.Unity.Mobile
{
	internal abstract class MobileFacebookGameObject : FacebookGameObject, IFacebookCallbackHandler, IMobileFacebookCallbackHandler
	{
		private IMobileFacebookImplementation MobileFacebook
		{
			get
			{
				return (IMobileFacebookImplementation)base.Facebook;
			}
		}

		protected MobileFacebookGameObject()
		{
		}

		public void OnAppInviteComplete(string message)
		{
			this.MobileFacebook.OnAppInviteComplete(message);
		}

		public void OnFetchDeferredAppLinkComplete(string message)
		{
			this.MobileFacebook.OnFetchDeferredAppLinkComplete(message);
		}

		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			this.MobileFacebook.OnRefreshCurrentAccessTokenComplete(message);
		}
	}
}